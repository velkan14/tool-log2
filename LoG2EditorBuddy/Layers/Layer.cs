using Log2CyclePrototype.LoG2API;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log2CyclePrototype.Layers
{
    class Layer : DrawAbstract
    {
        Monsters window;
        Panel gridPanel;
        Panel palettePanel;

        Graphics palette;
        Bitmap image;

        List<Color> colors = new List<Color>();
        List<Value> values = new List<Value>();

        int currentvalue = 0;
        int cellWidthPalett = 0;
        int cellWidth = 0;
        int cellHeight = 0;
        private int tileDistance = 3;

        public Layer(Monsters window, Map map, Panel gridPanel, Panel palettePanel, Color color)
        {
            this.window = window;
            this.gridPanel = gridPanel;
            this.palettePanel = palettePanel;

            

            image = new Bitmap(palettePanel.Width, palettePanel.Height);
            palette = Graphics.FromImage(image);
            
            foreach(Cell c in map.WalkableCells)
            {
                values.Add(new Value(c.X, c.Y));
            }

            colors.Add(Color.FromArgb(10,color));
            //colors.Add(Color.FromArgb((int)60 * 255/100,255, 0, 0));
            colors.Add(Color.FromArgb((int)70 * 255/100,color));
            //colors.Add(Color.FromArgb((int)80 * 255/100,255, 0, 0));
            colors.Add(Color.FromArgb((int)90 * 255/100,color));

            cellWidthPalett = palettePanel.Height;//palettePanel.Width / colors.Count;
            cellWidth = gridPanel.Width / map.Width;
            cellHeight = gridPanel.Height / map.Height;

            DrawPalette();

            palettePanel.MouseDown += this.MouseDownPalett;

            Active = false;
            Attached = false;
        }
        
        public bool Attached { get; private set; }
        public bool Active { get { return active; } set { active = value; DrawPalette(); } }
        private bool active;

        public void Attach()
        {
            gridPanel.MouseDown += this.MouseDown;
            gridPanel.MouseMove += this.MouseMove;
            Attached = true;
            DrawPalette();
        }

        public void Dettach()
        {
            if (Attached)
            {
                gridPanel.MouseDown -= this.MouseDown;
                gridPanel.MouseMove -= this.MouseMove;
                Attached = false;
                DrawPalette();
            }
        }

        public int getValue(int x, int y)
        {
            Value v = values.SingleOrDefault(e => e.x == x && e.y == y);
            if( v == null)
            {
                return 0; //FIXME: verificar se 0 é o valor base;
            }
            return v.value;
        }

        public int getValue(Cell c)
        {
            return getValue(c.X, c.Y);
        } 

        private void addValue(int x, int y)
        {
            foreach(Value v in values)
            {
                if(v.x == x && v.y == y)
                {
                    v.value = currentvalue;
                }
            }
        }

        private static Pen whitePen = new Pen(Color.White, 1);
        private static Pen grayPen = new Pen(Color.FromArgb(160,160,160), 1);
        private static Pen bluePen = new Pen(Color.FromArgb(100, 165, 231), 1);
        private static Pen blueLightPen = new Pen(Color.FromArgb(203, 228, 253), 1);
        private static Color controlColor = Color.FromArgb(240, 240, 240);
        public void DrawPalette()
        {
            

            if (Active) {
                palette.Clear(controlColor);

                for (int i = 0; i< colors.Count; i++)
                {
                    SolidBrush brush = new SolidBrush(colors[i]);
                    
                    
                    palette.FillRectangle(new SolidBrush(Color.White), i * (cellWidthPalett + tileDistance), 0, cellWidthPalett, palettePanel.Height);
                    palette.FillRectangle(brush, i * (cellWidthPalett + tileDistance), 0, cellWidthPalett, palettePanel.Height);

                    palette.DrawRectangle(grayPen,  i * (cellWidthPalett + tileDistance), 0, cellWidthPalett, palettePanel.Height - 1);
                    palette.DrawRectangle(whitePen, i * (cellWidthPalett+ tileDistance) + 1, 1, cellWidthPalett -2 , palettePanel.Height-3);
                }

                if (Attached)
                {
                    palette.DrawRectangle(bluePen, currentvalue * (cellWidthPalett + tileDistance), 0, cellWidthPalett, palettePanel.Height - 1);
                    palette.DrawRectangle(blueLightPen, currentvalue * (cellWidthPalett + tileDistance) + 1, 1, cellWidthPalett - 2, palettePanel.Height - 3);
                }

                palettePanel.BackgroundImage = image;
            }
            else palette.Clear(controlColor);

            if (palettePanel.InvokeRequired)
            {
                window.Invoke((MethodInvoker)(() => { palettePanel.Refresh(); })); //needed when calling the callback from a different thread
            }
            else
            {
                palettePanel.Refresh();
            }
        }

        private void MouseDownPalett(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (Active)
            {
                if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
                {
                    for (int i = 0; i < colors.Count; i++)
                    {
                        if(i * (cellWidthPalett + tileDistance) < e.X && e.X < i * (cellWidthPalett + tileDistance) + cellWidthPalett)
                        {
                            currentvalue = i;
                            break;
                        }
                    }
                    DrawPalette();
                }

            }
        }

        private void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (Active)
            {
                if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
                {
                    int x = e.X / cellWidth;
                    int y = e.Y / cellHeight;
                    addValue(x, y);
                }
                window.ReDraw();
            }
            
        }

        private void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (Active)
            {
                if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
                {
                    int x = e.X / cellWidth;
                    int y = e.Y / cellHeight;

                    addValue(x, y);

                    window.ReDraw();
                }
            }
            
        }

        public override Image Draw(int width, int height)
        {
            Bitmap image = new Bitmap(width, height);

            if (Active)
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    foreach (Value v in values)
                    {
                        SolidBrush brush = new SolidBrush(colors[v.value]);
                        g.FillRectangle(brush, v.x * cellWidth, v.y * cellHeight, cellWidth, cellHeight);
                    }
                }
            }

            return image;
        }

        public override Image Draw(int width, int height, Image backgroundImage)
        {
            if (Active)
            {
                using (Graphics g = Graphics.FromImage(backgroundImage))
                {
                    foreach (Value v in values)
                    {
                        SolidBrush brush = new SolidBrush(colors[v.value]);
                        g.FillRectangle(brush, v.x * cellWidth, v.y * cellHeight, cellWidth, cellHeight);
                    }
                }
            }
            return backgroundImage;
       }
    }
}
