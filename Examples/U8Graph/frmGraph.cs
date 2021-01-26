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
    public partial class frmGraph : Form
    {
        private WindowRenderTarget DrawTarget;
        private SharpDX.Direct2D1.Factory factory = new SharpDX.Direct2D1.Factory();
        private SharpDX.DirectWrite.Factory factoryWrite = new SharpDX.DirectWrite.Factory();
        private Brush redBrush;
        private Brush greenBrush;
        private Brush yellowBrush;
        private Brush whiteBrush;
        private Brush purpleBrush;
        private Brush blueBrush;
        private Brush blue2Brush;
        private Brush blackBrush;
        private TextFormat fontFat;
        private TextFormat fontBig;
        private TextFormat font;
        private TextFormat fontSmall;
        private TextFormat fontMini;

        public static U8Flow flow;

        private int graphX;
        private int graphY;
        private bool mouseDown = false;
        private int LastDownX;
        private int LastDownY;
        private int DownClickX;
        private int DownClickY;
        private int BlockHeights = 0;

        private RawVector2? PreviousBlock;
        private U8CodeBlock PreviousBlockInfo;

        private Dictionary<int, RawVector2> RArrows = new Dictionary<int, RawVector2>();
        private Dictionary<int, RawVector2> GArrows = new Dictionary<int, RawVector2>();
        private Dictionary<int, RawVector2> BArrows = new Dictionary<int, RawVector2>();

        public class U8CodeBlockVisual : U8CodeBlock
        {
            public RawRectangleF BoundryBox;
            private int GraphX;
            private int GraphY;
            private WindowRenderTarget DrawTarget;

            public U8CodeBlockVisual(U8Cmd[] Ops, ref int graphX, ref int graphY) : base (Ops)
            {
                this.GraphX = graphX;
                this.GraphY = graphY;
            }

            public U8CodeBlockVisual(ref U8CodeBlock b) : base(b.Ops)
            {
                this.BoundryBox = new RawRectangleF();
            }

            public void ApplyGraphOffsets()
            {
                BoundryBox.Left -= GraphX;
                BoundryBox.Right -= GraphX;
                BoundryBox.Top -= GraphY;
                BoundryBox.Bottom -= GraphY;
            }

            public void ApplyGraphOffsets(int x, int y)
            {
                BoundryBox.Left -= x;
                BoundryBox.Right -= x;
                BoundryBox.Top -= y;
                BoundryBox.Bottom -= y;
            }

            public void ApplyHeightOffset(int HeightOffset)
            {
                // rebase other blocks
                BoundryBox.Top += HeightOffset;
                BoundryBox.Bottom += HeightOffset;
            }

            public int GetBlockHeightOffset()
            {
                return (int)(BoundryBox.Bottom - BoundryBox.Top) + 20;
            }

            public RawVector2 GetTopCenter()
            {
                return new RawVector2(((BoundryBox.Left + BoundryBox.Right) / 2), BoundryBox.Top);
            }

            public RawVector2 GetBottomCenter()
            {
                return new RawVector2(((BoundryBox.Left + BoundryBox.Right) / 2), BoundryBox.Bottom);
            }
        }


        public frmGraph()
        {
            InitializeComponent();

            //Init Direct Draw
            //Set Rendering properties
            RenderTargetProperties renderProp = new RenderTargetProperties()
            {
                PixelFormat = new PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied),
            };

            //set hwnd target properties (permit to attach Direct2D to window)
            HwndRenderTargetProperties winProp = new HwndRenderTargetProperties()
            {
                Hwnd = this.Handle,
                PixelSize = new Size2(this.ClientSize.Width, this.ClientSize.Height),
                PresentOptions = PresentOptions.None
            };

            //target creation
            DrawTarget = new WindowRenderTarget(factory, renderProp, winProp);

            //redBrush = new SharpDX.Direct2D1.SolidColorBrush(DrawTarget, new RawColor4(0xFE, 0x6B, 0x64, 0xFF));
            redBrush = new SharpDX.Direct2D1.SolidColorBrush(DrawTarget, new RawColor4(255, 0, 0, 255));
            yellowBrush = new SharpDX.Direct2D1.SolidColorBrush(DrawTarget, new RawColor4(0xFB, 0xFD, 0x98, 0xFF));
            greenBrush = new SharpDX.Direct2D1.SolidColorBrush(DrawTarget, new RawColor4(0, 255, 0, 0xFF));
            //greenBrush = new SharpDX.Direct2D1.SolidColorBrush(DrawTarget, new RawColor4(0x77, 0xDD, 0x77, 0xFF));
            whiteBrush = new SharpDX.Direct2D1.SolidColorBrush(DrawTarget, new RawColor4(0xFF, 0xFF, 0xFF, 0xFF));
            purpleBrush = new SharpDX.Direct2D1.SolidColorBrush(DrawTarget, new RawColor4(0xB2, 0x9D, 0xD9, 0xFF));
            blueBrush = new SharpDX.Direct2D1.SolidColorBrush(DrawTarget, new RawColor4(0x77, 0x9E, 0xCB, 0xFF));
            blue2Brush = new SharpDX.Direct2D1.SolidColorBrush(DrawTarget, new RawColor4(0, 0, 255, 255));
            blackBrush = new SharpDX.Direct2D1.SolidColorBrush(DrawTarget, new RawColor4(0x00, 0x00, 0x00, 0xFF));

            //create textformat
            fontFat = new SharpDX.DirectWrite.TextFormat(factoryWrite, "Consolas", 36);
            fontBig = new SharpDX.DirectWrite.TextFormat(factoryWrite, "Consolas", 24);
            font = new SharpDX.DirectWrite.TextFormat(factoryWrite, "Consolas", 16);
            fontSmall = new SharpDX.DirectWrite.TextFormat(factoryWrite, "Consolas", 12);
            fontMini = new SharpDX.DirectWrite.TextFormat(factoryWrite, "Consolas", 8);

            //avoid artifacts
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            Thread render = new Thread(new ThreadStart(drawloop));
            render.Priority = ThreadPriority.BelowNormal;
            render.Start();

            Thread analysis = new Thread(new ThreadStart(analyse));
            analysis.Start();

            void drawloop()
            {
                while(true)
                {
                    DrawData();
                    Thread.Sleep(50);
                }
                    
            }

            void analyse()
            {
                frmGraph.flow = new U8Flow(File.ReadAllBytes(@"L:\Projects\Calculator\Casio\ROM_Dump.mem"));

                flow.Analyse(); // start analysis so others can check blocks
            }


        }

        private void DrawData()
        {
            //Start to draw
            DrawTarget.BeginDraw();
            DrawTarget.Clear(new RawColor4(0x70, 0x70, 0x70, 255));
            DrawTarget.DrawText($"U8Disasm ({graphX},{graphY})", fontSmall, new RawRectangleF(1, 1, 150, 20), blackBrush);
            DrawTarget.DrawLine(new RawVector2(1, 18), new RawVector2(150, 18), blackBrush);

            // draw mouse snapline
            if(mouseDown)
                DrawTarget.DrawLine(new RawVector2(DownClickX, DownClickY), new RawVector2(LastDownX, LastDownY), redBrush);

            // draw blocks
            int target = 0;
            if (flow != null && flow.Stubs != null && flow.Stubs.Count > target)
            {
                DrawTarget.DrawText($"[{flow.Stubs[target].Count}]", fontSmall, new RawRectangleF(1, 21, 150, 20), blackBrush);
                BlockHeights = 0;
                PreviousBlock = null;
                foreach (var i in flow.Stubs[target])
                {
                    DrawBlock(flow.Blocks[i]);
                }
                RArrows.Clear();
                GArrows.Clear();
                BArrows.Clear();
            }
                

            DrawTarget.EndDraw();
        }


        private void DrawBlock(U8CodeBlock block)
        {
            // TODO: dont render all blocks
            RawRectangleF box = new RawRectangleF();
            box.Left = this.Width / 2; // start center?
            box.Top = 30 - (block.Ops.Length * 7); // start center?
            box.Bottom = 30 + (block.Ops.Length * 7); // start center?
            int longestData = 0;
            string blockInnerStr = "";
            foreach(var b in block.Ops)
            {
                string data = $"0x{b.Address.ToString("X4")} {b.Opcode.ToString("X4")} {b.Instruction} {b.Operands}";
                if(data.Length > longestData)
                {
                    box.Left = (this.Width / 2) - (data.Length * 3) -2; // move to left
                    box.Right = (this.Width / 2) + (data.Length * 4) +1; // mov to right
                    longestData = data.Length;
                }
                blockInnerStr += data + "\n";
            }

            // rebase - mouse
            box.Left -= graphX;
            box.Right -= graphX;
            box.Top -= graphY;
            box.Bottom -= graphY;

            // rebase other blocks
            box.Top += BlockHeights;
            box.Bottom += BlockHeights;

            // keep track of current block height
            BlockHeights += (int)(box.Bottom - box.Top) + 20;

            // check for incomming arrow
            //if (PreviousBlock.HasValue)
            //{
            //    if(PreviousBlockInfo.NextBlock != -1)
            //        DrawTarget.DrawLine(PreviousBlock.Value, new RawVector2(((box.Left + box.Right+3) / 2), box.Top), redBrush);

            //    if (PreviousBlockInfo.JumpsToBlock != -1)
            //        DrawTarget.DrawLine(PreviousBlock.Value, new RawVector2(((box.Left + box.Right - 3) / 2), box.Top), blue2Brush);

            //    PreviousBlock = null; // reset
            //}

            // add arrow dict with block offsets if needed
            var thicc = (box.Left - box.Right) / 2;
            if (block.JumpsToBlock == -1 && block.NextBlock != -1)
            {
                BArrows.Add(block.NextBlock, new RawVector2(((box.Left + box.Right) / 2), box.Bottom)); //blue, only one jump
            } 
            else if (block.NextBlock != -1)
            {
                box.Left += thicc;
                box.Right += thicc;
                RArrows.Add(block.NextBlock, new RawVector2(((box.Left + box.Right) / 2), box.Bottom)); // red, jump failed but not only one
            }
                

            if (block.JumpsToBlock != -1)
            {
                box.Left += thicc;
                box.Right += thicc;
                GArrows.Add(block.JumpsToBlock, new RawVector2(((box.Left + box.Right) / 2), box.Bottom)); // green, jump OK
            }
                


            DrawTarget.DrawLine(new RawVector2(box.Left-2, box.Top), new RawVector2(box.Right+1, box.Top), blackBrush);         // ----
            DrawTarget.DrawLine(new RawVector2(box.Left-2, box.Top), new RawVector2(box.Left-2, box.Bottom), blackBrush);       // |
            DrawTarget.DrawLine(new RawVector2(box.Right+1, box.Top), new RawVector2(box.Right+1, box.Bottom), blackBrush);     //      |
            DrawTarget.DrawLine(new RawVector2(box.Left-2, box.Bottom), new RawVector2(box.Right+1, box.Bottom), blackBrush);   // _____
            DrawTarget.DrawText(blockInnerStr, fontSmall, box, blackBrush);

            // check for incomming jumps
            if(BArrows.ContainsKey(block.Address))
            {
                DrawTarget.DrawLine(BArrows[block.Address], new RawVector2(((box.Left + box.Right - 3) / 2), box.Top), blue2Brush);
                BArrows.Remove(block.Address);
            }
            if (GArrows.ContainsKey(block.Address))
            {
                DrawTarget.DrawLine(GArrows[block.Address], new RawVector2(((box.Left + box.Right - 3) / 2), box.Top), greenBrush);
                GArrows.Remove(block.Address);
            }
            if (RArrows.ContainsKey(block.Address))
            {
                DrawTarget.DrawLine(RArrows[block.Address], new RawVector2(((box.Left + box.Right - 3) / 2), box.Top), redBrush);
                RArrows.Remove(block.Address);
            }


            // set arrow start point
            //if (block.JumpsToBlock != -1 || block.NextBlock != -1)
            //{
            //    PreviousBlockInfo = block;
            //    PreviousBlock = new RawVector2(((box.Left + box.Right) / 2), box.Bottom);
            //}
        }

        private void frmGraph_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown)
                return;

            graphX += (LastDownX - e.X);
            graphY += (LastDownY - e.Y);
            // do math and save
            LastDownX = e.X; 
            LastDownY = e.Y;
        }

        private void frmGraph_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void frmGraph_MouseDown(object sender, MouseEventArgs e)
        {
            LastDownX = e.X; // reset
            LastDownY = e.Y;
            DownClickX = e.X;
            DownClickY = e.Y;
            mouseDown = true;
        }

        private void frmGraph_Scroll(object sender, ScrollEventArgs e)
        {
            // this is not accepting?
            if(e.OldValue > e.NewValue)
            {
                // zoom out
                this.Width = (int)(this.Width*1.05);
                this.Height = (int)(this.Height * 1.05);
            }else
            {
                this.Width = (int)(this.Width * 0.95);
                this.Height = (int)(this.Height * 0.95);
            }
                
        }
    }
}
