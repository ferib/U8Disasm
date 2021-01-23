using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using u8_lib.Disasm;

namespace u8_Forum
{
    public partial class Form1 : Form
    {
        public static u8_Disasm disasm;
        public static Thread worker;
        private int CachedSubCount = 0;

        public Form1()
        {
            InitializeComponent();

            u8_Disasm disasm;

            timer1.Enabled = true;

            worker = new Thread(new ParameterizedThreadStart(doTask));
            worker.Name = "u8_Disasm";
            worker.Start(null);
            
            void doTask(object x)
            {
                Form1.disasm = new u8_Disasm(@"L:\Projects\Calculator\Casio\ROM_Dump.mem");
                Form1.disasm.FlowAnalyses.Analyse();
                updateSubsView();
            }

        }

        private void lstSubs_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            lock (disasm.FlowAnalyses.Stubs)
            {
                if (disasm.FlowAnalyses.Stubs.Count-1 < lstSubs.SelectedIndex)
                    return;

                var sub = disasm.FlowAnalyses.Stubs[lstSubs.SelectedIndex];
                foreach (var block in sub)
                {
                    if (block == null)
                        continue;

                    foreach (var o in block.Ops)
                    {
                        DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                        row.Cells[0].Value = o.address.ToString("X4");
                        row.Cells[1].Value = o.opcode.ToString("X2");
                        row.Cells[2].Value = o.instr;
                        row.Cells[3].Value = o.operands;
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
            if (disasm == null || disasm.FlowAnalyses == null || disasm.FlowAnalyses.Stubs == null)
                return;

            lock(disasm.FlowAnalyses.Stubs)
            {
                if (CachedSubCount == disasm.FlowAnalyses.Stubs.Count)
                    return;

                lstSubs.Items.Clear();
                foreach (var x in disasm.FlowAnalyses.Stubs)
                    lstSubs.Items.Add("sub_" + x[0].Address.ToString("X8"));
                lblInfo.Text = $"Subs Count: {disasm.FlowAnalyses.Stubs.Count}\nByte Count: {disasm.Buffer.Length}";
                CachedSubCount = disasm.FlowAnalyses.Stubs.Count;
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
