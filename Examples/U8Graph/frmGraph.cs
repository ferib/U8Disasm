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
            greenBrush = new SharpDX.Direct2D1.SolidColorBrush(DrawTarget, new RawColor4(0x77, 0xDD, 0x77, 0xFF));
            whiteBrush = new SharpDX.Direct2D1.SolidColorBrush(DrawTarget, new RawColor4(0, 0, 0, 0xFF));
            purpleBrush = new SharpDX.Direct2D1.SolidColorBrush(DrawTarget, new RawColor4(0xB2, 0x9D, 0xD9, 0xFF));
            blueBrush = new SharpDX.Direct2D1.SolidColorBrush(DrawTarget, new RawColor4(0x77, 0x9E, 0xCB, 0xFF));
            blackBrush = new SharpDX.Direct2D1.SolidColorBrush(DrawTarget, new RawColor4(0x00, 0x00, 0x00, 0xFF));

            //create textformat
            fontFat = new SharpDX.DirectWrite.TextFormat(factoryWrite, "Consolas", 36);
            fontBig = new SharpDX.DirectWrite.TextFormat(factoryWrite, "Consolas", 24);
            font = new SharpDX.DirectWrite.TextFormat(factoryWrite, "Consolas", 16);
            fontSmall = new SharpDX.DirectWrite.TextFormat(factoryWrite, "Consolas", 12);
            fontMini = new SharpDX.DirectWrite.TextFormat(factoryWrite, "Consolas", 8);

            //avoid artifacts
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            Thread t = new Thread(new ThreadStart(drawloop));
            t.Start();

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
            DrawTarget.DrawText($"U8Disasm {graphX},{graphY}", fontSmall, new RawRectangleF(1, 1, 100, 20), blackBrush);
            DrawTarget.DrawLine(new RawVector2(1, 18), new RawVector2(65, 18), blackBrush);

            // draw stuff
            //foreach(var sub in flow.Stubs)
            //{

            //}
            if(flow != null && flow.Blocks != null && flow.Blocks.Count > 5)
                DrawBlock(flow.Blocks[59]);

            DrawTarget.EndDraw();
        }

        // TODO: make more re-usable
        private void DrawBlock(U8CodeBlock block)
        {
            // idk?
            RawRectangleF box = new RawRectangleF();
            box.Left = this.Width / 2; // start center?
            box.Top = (this.Height / 2) - (block.Ops.Length * 7); // start center?
            box.Bottom = (this.Height / 2) + (block.Ops.Length * 7); // start center?
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

            DrawTarget.DrawLine(new RawVector2(box.Left-2, box.Top), new RawVector2(box.Right+1, box.Top), blackBrush);         // ----
            DrawTarget.DrawLine(new RawVector2(box.Left-2, box.Top), new RawVector2(box.Left-2, box.Bottom), blackBrush);       // |
            DrawTarget.DrawLine(new RawVector2(box.Right+1, box.Top), new RawVector2(box.Right+1, box.Bottom), blackBrush);     //      |
            DrawTarget.DrawLine(new RawVector2(box.Left-2, box.Bottom), new RawVector2(box.Right+1, box.Bottom), blackBrush);   // _____
            DrawTarget.DrawText(blockInnerStr, fontSmall, box, blackBrush);

        }

        private void frmGraph_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown)
                return;

            graphX += (e.X - LastDownX);
            graphY += (e.Y - LastDownY);
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
            mouseDown = true;
        }
    }
}
