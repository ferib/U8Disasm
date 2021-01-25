using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using U8Disasm.Disassembler;
using U8Disasm.Analyser;

namespace U8Form
{
    public partial class Form1 : Form
    {
        public static U8Flow FlowAnalyser;
        public static Thread worker;
        private int CachedSubCount = 0;

        public Form1()
        {
            InitializeComponent();

            timer1.Enabled = true;

            worker = new Thread(new ParameterizedThreadStart(doTask));
            worker.Name = "u8_Disasm";
            worker.Start(null);
            
            void doTask(object x)
            {
                Form1.FlowAnalyser = new U8Flow(File.ReadAllBytes(@"L:\Projects\Calculator\Casio\ROM_Dump.mem"));
                Form1.FlowAnalyser.Analyse();
                //updateSubsView();
            }

        }

        private void lstSubs_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            lock (FlowAnalyser.Stubs)
            {
                if (FlowAnalyser.Stubs.Count-1 < lstSubs.SelectedIndex)
                    return;

                var sub = FlowAnalyser.Stubs[lstSubs.SelectedIndex];
                foreach (var block in sub)
                {
                    if (FlowAnalyser.Blocks[block] == null)
                        continue;

                    foreach (var o in FlowAnalyser.Blocks[block].Ops)
                    {
                        DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                        row.Cells[0].Value = o.Address.ToString("X4");
                        row.Cells[1].Value = o.Opcode.ToString("X2");
                        row.Cells[2].Value = o.Instruction;
                        row.Cells[3].Value = o.Operands;
                        dataGridView1.Rows.Add(row);
                    }
                    DataGridViewRow row2 = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                    row2.Cells[0].Value = "======";
                    row2.Cells[1].Value = "======";
                    row2.Cells[2].Value = "======";
                    row2.Cells[3].Value = "======";
                    dataGridView1.Rows.Add(row2);
                }
            }
        }

        private void updateSubsView()
        {
            if (FlowAnalyser == null || FlowAnalyser == null || FlowAnalyser.Stubs == null)
                return;

            lock(FlowAnalyser.Stubs)
            {
                if (CachedSubCount == FlowAnalyser.Stubs.Count)
                    return;

                lstSubs.Items.Clear();
                foreach (var x in FlowAnalyser.Stubs)
                    lstSubs.Items.Add("sub_" + FlowAnalyser.Blocks[x[0]].Address.ToString("X8"));
                lblInfo.Text = $"Subs Count: {FlowAnalyser.Stubs.Count}\nByte Count: {FlowAnalyser.Memory.Length}";
                CachedSubCount = FlowAnalyser.Stubs.Count;
            }


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            updateSubsView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // quick & dirty
            if(worker != null & worker.IsAlive)
            {
                if (worker.ThreadState == ThreadState.Suspended)
                    worker.Resume();
                else
                    worker.Suspend();
            }
        }
    }
}
