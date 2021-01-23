using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using u8_lib.Disasm;

namespace u8_Forum
{
    public partial class Form1 : Form
    {
        private u8_Disasm disasm;
        public Form1()
        {
            InitializeComponent();
            disasm = new u8_Disasm(@"L:\Projects\Calculator\Casio\ROM_Dump.mem");
            foreach(var x in disasm.FlowAnalyses.Stubs)
                lstSubs.Items.Add("sub_" + x[0].Address.ToString("X8") );
            lblInfo.Text = $"Subs Count: {disasm.FlowAnalyses.Stubs.Count}\nByte Count: {disasm.Buffer.Length}";
        }

        private void lstSubs_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            var sub = disasm.FlowAnalyses.Stubs[lstSubs.SelectedIndex];
            foreach (var block in sub)
            {
                if (block == null)
                    continue;

                foreach(var o in block.Ops)
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
}
