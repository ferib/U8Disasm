using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX;
using SharpDX.Mathematics.Interop;
using System.Threading;
using U8Disasm.Analyser;
using U8Disasm.Disassembler;
using U8Disasm.Structs;
using System.IO;

namespace U8Graph
{
    public partial class frmMain : Form
    {
        public static U8Flow Flow;
        private Thread Analyser;
        private int CachedSubsCount = 0;

        public frmMain()
        {
            InitializeComponent();

            Thread Analyser = new Thread(new ThreadStart(analyse));
            Analyser.Start();

            void analyse()
            {
                frmMain.Flow = new U8Flow(File.ReadAllBytes(@"L:\Projects\Calculator\Casio\ROM_Dump.mem"));
                Flow.Analyse(); // start analysis so others can check blocks
            }

        }

        private void tsfOpen_Click(object sender, EventArgs e)
        {

        }

        private void btnGraph_Click(object sender, EventArgs e)
        {
            if (dgvSubroutines.CurrentCell.RowIndex >= Flow.Stubs.Count)
                return;

            // create list to pass
            var list = new List<U8CodeBlock>();
            for (int i = 0; i < Flow.Stubs[dgvSubroutines.CurrentCell.RowIndex].Count; i++)
                list.Add(Flow.Blocks[Flow.Stubs[dgvSubroutines.CurrentCell.RowIndex][i]]);
               
            frmGraph graph = new frmGraph(list);
            graph.Show();
        }

        private void timerTick_Tick(object sender, EventArgs e)
        {
            UpdateSubsView();
        }

        private void UpdateSubsView()
        {
            if (Flow == null || Flow.Stubs == null)
                return;

            int selectedCell = -1;
            
            if(dgvSubroutines.CurrentCell != null)
                selectedCell = dgvSubroutines.CurrentCell.RowIndex;

            lock (Flow.Stubs)
            {
                if (CachedSubsCount == Flow.Stubs.Count)
                    return;

                dgvSubroutines.Rows.Clear();
                foreach (var sub in Flow.Stubs)
                {
                    if (sub == null || sub.Count == 0)
                        continue;

                    DataGridViewRow row = (DataGridViewRow)dgvSubroutines.Rows[0].Clone();
                    row.Cells[0].Value = "sub_" + sub[0].ToString("X8");
                    row.Cells[1].Value = "0x" + sub[0].ToString("X8");
                    dgvSubroutines.Rows.Add(row);
                }
            }
            if(selectedCell != -1)
            {
                dgvSubroutines.Rows[0].Selected = false;
                dgvSubroutines.Rows[selectedCell].Selected = true;
            }
                
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Analyser.Abort(); // TODO: let them suicide instead
            Application.Exit(); // kill all threads
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // TODO: ask to save?
        }
    }
}
