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
        private Brush backgroundBrush;
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

        private List<GraphArrow> GArrows;
        private List<GraphBlock> GBlocks;

        // class to keep track of all visual information about a block
        public class GraphBlock
        {
            public RawRectangleF BoundryBox;
            public U8CodeBlock Block;
            public string BlockInnerString;

            private int OffsetLeft;
            private int BlockHeight;
            private int LastGraphX;
            private int LastGraphY;

            public static int BlocksHeightOffset = 0;

            public GraphBlock(U8CodeBlock block, int frmWidth, int frmHeight, int graphX = 0, int graphY = 0)
            {
                this.Block = block;
                this.BlockInnerString = "";
                this.BlockHeight = 0;
                this.LastGraphX = 0;
                this.LastGraphY = 0;
                this.OffsetLeft = 0;
                Initialize(frmWidth, frmHeight, graphX, graphY);
            }

            public void Initialize(int frmWidth, int frmHeight, int graphX, int graphY)
            {
                BoundryBox.Left = frmWidth / 2; // start center?
                BoundryBox.Top = 30 - (Block.Ops.Length * 7); // start center?
                BoundryBox.Bottom = 30 + (Block.Ops.Length * 7); // start center?
                int longestData = 0;
                BlockInnerString = "";
                foreach (var b in Block.Ops)
                {
                    string data = $"0x{b.Address.ToString("X4")} {b.Opcode.ToString("X4")} {b.Instruction} {b.Operands}";
                    if (data.Length > longestData)
                    {
                        BoundryBox.Left = (frmWidth / 2) - (data.Length * 3) - 2; // move to left
                        BoundryBox.Right = (frmWidth / 2) + (data.Length * 4) + 1; // mov to right
                        longestData = data.Length;
                    }
                    BlockInnerString += data + "\n";
                }

                Update(graphX, graphY);
            }

            public void Update(int graphX, int graphY)
            {
                ApplyGraphOffsets(graphX, graphY); // mouse movement
                ApplyHeightOffset(GetBlockHeightOffset()); // block height indexing
            }

            public void ApplyGraphOffsets(int x, int y)
            {
                BoundryBox.Left += LastGraphX; // rebase
                BoundryBox.Left -= x; // add offset
                BoundryBox.Right += LastGraphX; // rebase
                BoundryBox.Right -= x; // add offset
                BoundryBox.Top += LastGraphY; // rebase
                BoundryBox.Top -= y; // offset
                BoundryBox.Bottom += LastGraphY; // rebase
                BoundryBox.Bottom -= y; // offset

                // save last
                LastGraphX = x;
                LastGraphY = y;
            }

            public void ApplyHeightOffset(int newHeight)
            {
                // rebase blocks
                GraphBlock.BlocksHeightOffset -= this.BlockHeight;
                GraphBlock.BlocksHeightOffset += newHeight;
                this.BlockHeight = newHeight;
                BoundryBox.Top += GraphBlock.BlocksHeightOffset;
                BoundryBox.Bottom += GraphBlock.BlocksHeightOffset;
            }

            public int GetBlockHeightOffset()
            {
                return (int)(BoundryBox.Bottom - BoundryBox.Top) + 40;
            }

            public RawVector2 GetTopCenter()
            {
                return new RawVector2(((BoundryBox.Left + BoundryBox.Right) / 2), BoundryBox.Top);
            }

            public RawVector2 GetBottomCenter()
            {
                return new RawVector2(((BoundryBox.Left + BoundryBox.Right) / 2), BoundryBox.Bottom);
            }

            public int GetBlockHeight()
            {
                int sum = (int)(BoundryBox.Top - BoundryBox.Bottom);
                if (sum >= 0)
                    return sum;
                else
                    return (int)(BoundryBox.Bottom - BoundryBox.Top);
            }

            public int GetBlockWidth()
            {
                int sum = (int)(BoundryBox.Left - BoundryBox.Right);
                if (sum >= 0)
                    return sum;
                else
                    return (int)(BoundryBox.Right - BoundryBox.Left);
            }
        }

        public class GraphArrow
        {
            public int Start;
            public int End;
            public RawVector2 Start2D; // unused?
            public RawVector2 End2D;
            public Brush Brush;

            public int StartPixelOffsetH; // shift blocks left/right for arrow lines
            public int EndPixelOffsetH;

            private int LastGraphX;
            private int LastGraphY;

            private List<GraphBlock> GBlocks;

            public GraphArrow(ref List<GraphBlock> gblocks)
            {
                GBlocks = gblocks;
                SetGraphOffsets(0, 0);
            }

            public void SetGraphOffsets(int x, int y)
            {
                // save last
                LastGraphX = x;
                LastGraphY = y;
            }
            private RawVector2 ApplyGraphOffsets(RawVector2 vec)
            {
                // save last
                vec.X -= LastGraphX;
                vec.X -= LastGraphY;
                return vec;
            }

            public List<RawVector2> CalculatePaths()
            {
                List<RawVector2> result = new List<RawVector2>();

                if (this.GBlocks == null)
                    return result;

                int startIndex = this.GBlocks.IndexOf(this.GBlocks.Find(x => x.Block.Address == this.Start));
                int endIndex = this.GBlocks.IndexOf(this.GBlocks.Find(x => x.Block.Address == this.End));


                if (startIndex < 0 || endIndex < 0 ||  this.GBlocks.Count <= startIndex || this.GBlocks.Count <= endIndex )
                    return result;

                // TODO: add caching
                // TODO: implement

                // just basic test ray
                result.Add(GBlocks[startIndex].GetBottomCenter());
                result.Add(GBlocks[endIndex].GetTopCenter());

                // graph
                for(int i = 0; i < result.Count; i++)
                    result[i] = ApplyGraphOffsets(result[i]);

                return result;
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

            backgroundBrush = new SharpDX.Direct2D1.LinearGradientBrush(DrawTarget, new LinearGradientBrushProperties()
            {
                StartPoint = new RawVector2(0, (int)(this.Height * 0.2)),
                EndPoint = new RawVector2(0, this.Height),
            },
            new SharpDX.Direct2D1.GradientStopCollection(DrawTarget, new SharpDX.Direct2D1.GradientStop[]
            {
                new GradientStop()
                {
                    Color = new RawColor4(0xFF, 0xFF, 0xFF, 0xFF),
                    Position = 0,
                },
                new GradientStop()
                {
                    Color = new RawColor4(140, 200, 00, 10), //E0F8FF
                    Position = 1,
                }
            }));

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
            DrawTarget.FillRectangle(new RawRectangleF(0, 0, this.Width, this.Height), backgroundBrush);
            DrawTarget.DrawText($"U8Disasm ({graphX},{graphY})", fontSmall, new RawRectangleF(1, 1, 150, 20), blackBrush);
            DrawTarget.DrawLine(new RawVector2(1, 18), new RawVector2(150, 18), blackBrush);

            // draw mouse snapline
            if(mouseDown)
                DrawTarget.DrawLine(new RawVector2(DownClickX, DownClickY), new RawVector2(LastDownX, LastDownY), redBrush);

            // draw blocks when we have target ready
            int target = 0;
            if (flow != null && flow.Stubs != null && flow.Stubs.Count > target)
            {
                // start of each rendering loop
                GraphBlock.BlocksHeightOffset = 0;
                DrawTarget.DrawText($"[{flow.Stubs[target].Count}]", fontSmall, new RawRectangleF(1, 21, 150, 20), blackBrush);

                // Create & Init GBlock List if needed
                if (GBlocks == null)
                    CreateBlocks(target);

                // create GraphArrows
                if (GArrows == null)
                    CreateArrows();
                
                // draw GBlocks
                foreach (var gb in GBlocks)
                    DrawBlock(gb);

                // draw GArrows
                DrawArrows();
            }
            DrawTarget.EndDraw();
        }

        private void CreateBlocks(int target)
        {
            GBlocks = new List<GraphBlock>();
            foreach (var b in flow.Stubs[target])
            {
                GBlocks.Add(new GraphBlock(flow.Blocks[b], this.Width, this.Height));
            }
        }

        private void CreateArrows()
        {
            GArrows = new List<GraphArrow>();
            // TODO: implement objects instead of this mess
            // TODO: dont render all blocks

            // TODO: More the arrow renderings to a spcific class with collision detection
            // add arrow dict with block offsets if needed
            foreach(var GB in GBlocks)
            {
                var thicc = GB.GetBlockWidth();
                if (GB.Block.JumpsToBlock == -1 && GB.Block.NextBlock != -1)
                {
                    //BArrows.Add(block.NextBlock, new RawVector2(((box.Left + box.Right) / 2), box.Bottom)); //blue, only one jump
                    GArrows.Add(new GraphArrow(ref GBlocks)
                    {
                        Brush = blue2Brush,
                        Start2D = new RawVector2(GB.GetBlockWidth(), GB.BoundryBox.Bottom),
                        Start = GB.Block.Address,
                        End = GB.Block.NextBlock
                    });
                }
                else if (GB.Block.NextBlock != -1)
                {
                    //GB.BoundryBox.Left += thicc;
                    //GB.BoundryBox.Right += thicc;
                    //RArrows.Add(block.NextBlock, new RawVector2(((box.Left + box.Right) / 2), box.Bottom)); // red, jump failed but not only one
                    GArrows.Add(new GraphArrow(ref GBlocks)
                    {
                        Brush = redBrush,
                        Start2D = new RawVector2(GB.GetBlockWidth(), GB.BoundryBox.Bottom),
                        Start = GB.Block.Address,
                        End = GB.Block.NextBlock
                    });
                }
                if (GB.Block.JumpsToBlock != -1)
                {
                    //GArrows.Add(block.JumpsToBlock, new RawVector2(((box.Left + box.Right) / 2), box.Bottom)); // green, jump OK
                    GArrows.Add(new GraphArrow(ref GBlocks)
                    {
                        Brush = greenBrush,
                        Start2D = new RawVector2(GB.GetBlockWidth(), GB.BoundryBox.Bottom),
                        Start = GB.Block.Address,
                        End = GB.Block.JumpsToBlock
                    });
                }
            }
            
        }

        private void DrawBlock(GraphBlock block)
        {
            // update box with mouse positions
            block.Update(graphX, graphY);

            // draw
            DrawTarget.FillRectangle(block.BoundryBox, whiteBrush);
            DrawTarget.DrawLine(new RawVector2(block.BoundryBox.Left - 2, block.BoundryBox.Top), new RawVector2(block.BoundryBox.Right + 1, block.BoundryBox.Top), blackBrush);         // ----
            DrawTarget.DrawLine(new RawVector2(block.BoundryBox.Left - 2, block.BoundryBox.Top), new RawVector2(block.BoundryBox.Left - 2, block.BoundryBox.Bottom), blackBrush);       // |
            DrawTarget.DrawLine(new RawVector2(block.BoundryBox.Right + 1, block.BoundryBox.Top), new RawVector2(block.BoundryBox.Right + 1, block.BoundryBox.Bottom), blackBrush);     //      |
            DrawTarget.DrawLine(new RawVector2(block.BoundryBox.Left - 2, block.BoundryBox.Bottom), new RawVector2(block.BoundryBox.Right + 1, block.BoundryBox.Bottom), blackBrush);   // _____
            DrawTarget.DrawText(block.BlockInnerString, fontSmall, block.BoundryBox, blackBrush);
        }

        private void DrawArrows()
        {
            foreach (var ar in GArrows)
            {
                var path = ar.CalculatePaths();
                RawVector2? lastPoint = null;
                foreach(var p in path)
                {
                    if (lastPoint.HasValue)
                        DrawTarget.DrawLine(lastPoint.Value, p, ar.Brush);
                    lastPoint = p;
                }
                    
            }
        }

        private void frmGraph_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown)
                return;

            graphX += (int)((LastDownX - e.X) * 1.25);
            graphY += (int)((LastDownY - e.Y) * 1.25);

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
