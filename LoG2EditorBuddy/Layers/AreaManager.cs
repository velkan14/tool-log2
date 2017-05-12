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
    class AreaManager
    {
        enum BorderSide
        {
            Top,
            Left,
            Bottom,
            Right
        }

        public List<Area> AreaList { get; set; }
        Map map;

        int cellWidth = 0, cellHeight = 0;
        private Brush selectionBrushAdd = new SolidBrush(Color.FromArgb(128, 72, 145, 220));

        public AreaManager(Map map, Panel gridPanel)
        {
            this.map = map;
            AreaList = new List<Area>();

            cellWidth = gridPanel.Width / map.Width;
            cellHeight = gridPanel.Height / map.Height;

            List<Cell> area1 = new List<Cell>();
            List<Cell> area2 = new List<Cell>();
            List<Cell> area3 = new List<Cell>();
            List<Cell> area4 = new List<Cell>();
            List<Cell> area5 = new List<Cell>();
            List<Cell> area6 = new List<Cell>();

            area1.Add(map.GetCellAt(20, 11));
            area1.Add(map.GetCellAt(20, 12));
            area1.Add(map.GetCellAt(20, 13));
            area1.Add(map.GetCellAt(21, 13));
            area1.Add(map.GetCellAt(21, 14));
            area1.Add(map.GetCellAt(19, 13));
            area1.Add(map.GetCellAt(18, 13));
            area1.Add(map.GetCellAt(17, 13));
            area1.Add(map.GetCellAt(16, 13));
            area1.Add(map.GetCellAt(16, 14));
            
            area2.Add(map.GetCellAt(16, 12));
            area2.Add(map.GetCellAt(16, 11));
            area2.Add(map.GetCellAt(16, 10));
            area2.Add(map.GetCellAt(15, 11));
            area2.Add(map.GetCellAt(15, 12 ));

            area3.Add(map.GetCellAt(18, 15));
            area3.Add(map.GetCellAt(19, 15));
            area3.Add(map.GetCellAt(20, 15));
            area3.Add(map.GetCellAt(21, 15));
            area3.Add(map.GetCellAt(22, 15));
            area3.Add(map.GetCellAt(17, 16));
            area3.Add(map.GetCellAt(18, 16));
            area3.Add(map.GetCellAt(19, 16));
            area3.Add(map.GetCellAt(21, 16));
            area3.Add(map.GetCellAt(22, 16));
            area3.Add(map.GetCellAt(15, 17));
            area3.Add(map.GetCellAt(16, 17));
            area3.Add(map.GetCellAt(17, 17));
            area3.Add(map.GetCellAt(19, 17));
            area3.Add(map.GetCellAt(20, 17));
            area3.Add(map.GetCellAt(21, 17));
            area3.Add(map.GetCellAt(17, 18));
            area3.Add(map.GetCellAt(18, 18));
            area3.Add(map.GetCellAt(19, 18));
            area3.Add(map.GetCellAt(20, 18));
            area3.Add(map.GetCellAt(18, 19));
            area3.Add(map.GetCellAt(19, 19));
            area3.Add(map.GetCellAt(20, 19));
            area3.Add(map.GetCellAt(21, 19));
            area3.Add(map.GetCellAt(18, 20));
            area3.Add(map.GetCellAt(20, 20));

            area4.Add(map.GetCellAt(20, 22));
            area4.Add(map.GetCellAt(20, 21));

            area5.Add(map.GetCellAt(13, 17));
            area5.Add(map.GetCellAt(14, 17));
            area5.Add(map.GetCellAt(13, 18));
            area5.Add(map.GetCellAt(12, 19));
            area5.Add(map.GetCellAt(13, 19));
            area5.Add(map.GetCellAt(14, 19));
            area5.Add(map.GetCellAt(11, 20));
            area5.Add(map.GetCellAt(12, 20));
            area5.Add(map.GetCellAt(14, 20));
            area5.Add(map.GetCellAt(15, 20));
            area5.Add(map.GetCellAt(12, 21));
            area5.Add(map.GetCellAt(13, 21));
            area5.Add(map.GetCellAt(14, 21));
            area5.Add(map.GetCellAt(15, 21));
            area5.Add(map.GetCellAt(13, 22));
            area5.Add(map.GetCellAt(15, 22));
            area5.Add(map.GetCellAt(13, 23));
            area5.Add(map.GetCellAt(14, 23));
            area5.Add(map.GetCellAt(15, 23));

            area6.Add(map.GetCellAt(6, 19));
            area6.Add(map.GetCellAt(7, 19));
            area6.Add(map.GetCellAt(8, 19));
            area6.Add(map.GetCellAt(9, 19));
            area6.Add(map.GetCellAt(10, 19));
            area6.Add(map.GetCellAt(5, 20));
            area6.Add(map.GetCellAt(6, 20));
            area6.Add(map.GetCellAt(7, 20));
            area6.Add(map.GetCellAt(8, 20));
            area6.Add(map.GetCellAt(9, 20));
            area6.Add(map.GetCellAt(10, 20));
            area6.Add(map.GetCellAt(6, 21));
            area6.Add(map.GetCellAt(7, 21));
            area6.Add(map.GetCellAt(8, 21));
            area6.Add(map.GetCellAt(9, 21));
            area6.Add(map.GetCellAt(10, 21));

            AreaList.Add(new Area("Area 1", area1));
            AreaList.Add(new Area("Area 2",area2));
            AreaList.Add(new Area("Area 3",area3));
            AreaList.Add(new Area("Area 4",area4));
            AreaList.Add(new Area("Area 5", area5));
            AreaList.Add(new Area("Area 6", area6));
        }

        public Image Draw(int width, int height, Image backgroundImage)
        {
            using (Graphics g = Graphics.FromImage(backgroundImage))
            {
                using (Pen pen = new Pen(new SolidBrush(Color.FromArgb(51, 153, 255)), 1))
                {
                    foreach(Area area in AreaList)
                    {
                        if (area.Visible)
                        {
                            foreach (var p in area.Cells)
                            {
                                if (p.X > 0)//test left
                                {
                                    var indexLeft = area.Cells.FindIndex(point => (point.X == (p.X - 1) && point.Y == p.Y));
                                    if (indexLeft == -1)
                                        DrawBorder(p.X, p.Y, pen, BorderSide.Left, g);
                                }
                                else
                                {
                                    DrawBorder(p.X, p.Y, pen, BorderSide.Left, g);
                                }

                                if (p.X < map.Width - 1) //test right
                                {
                                    var indexRight = area.Cells.FindIndex(point => (point.X == (p.X + 1) && point.Y == p.Y));
                                    if (indexRight == -1)
                                        DrawBorder(p.X, p.Y, pen, BorderSide.Right, g);
                                }
                                else
                                {
                                    DrawBorder(p.X, p.Y, pen, BorderSide.Right, g);
                                }
                                if (p.Y > 0) //test up
                                {
                                    var indexUp = area.Cells.FindIndex(point => (point.X == p.X && point.Y == (p.Y - 1)));
                                    if (indexUp == -1)
                                        DrawBorder(p.X, p.Y, pen, BorderSide.Top, g);
                                }
                                else
                                {
                                    DrawBorder(p.X, p.Y, pen, BorderSide.Top, g);
                                }
                                if (p.Y < map.Height - 1) //test bottom
                                {
                                    var indexBot = area.Cells.FindIndex(point => (point.X == p.X && point.Y == (p.Y + 1)));
                                    if (indexBot == -1)
                                        DrawBorder(p.X, p.Y, pen, BorderSide.Bottom, g);
                                }
                                else
                                {
                                    DrawBorder(p.X, p.Y, pen, BorderSide.Bottom, g);
                                }

                                if(area.Selected) g.FillRectangle(selectionBrushAdd, p.X * cellWidth, p.Y * cellHeight, cellWidth, cellHeight);
                            }
                        }
                        
                    }
                    
                }
            }

            return backgroundImage;
        }

        private void DrawBorder(int x, int y, Pen p, BorderSide s, Graphics graphics)
        {
            switch (s)
            {
                case BorderSide.Top:
                    graphics.DrawLine(p, new Point(x * cellWidth, y * cellHeight), new Point(x * cellWidth + cellWidth, y * cellHeight));
                    break;
                case BorderSide.Left:
                    graphics.DrawLine(p, new Point(x * cellWidth, y * cellHeight), new Point(x * cellWidth, y * cellHeight + cellHeight));
                    break;
                case BorderSide.Bottom:
                    graphics.DrawLine(p, new Point(x * cellWidth, y * cellHeight + cellHeight), new Point(x * cellWidth + cellWidth, y * cellHeight + cellHeight));
                    break;
                case BorderSide.Right:
                    graphics.DrawLine(p, new Point(x * cellWidth + cellWidth, y * cellHeight), new Point(x * cellWidth + cellWidth, y * cellHeight + cellHeight));
                    break;
                default:
                    break;
            }
        }

        internal void SetSelected(string value)
        {
            foreach(Area a in AreaList)
            {
                if (a.Name.Equals(value))
                {
                    a.Selected = true;
                }
            }
        }

        internal void SetUnselected(string value)
        {
            foreach (Area a in AreaList)
            {
                if (a.Name.Equals(value))
                {
                    a.Selected = false;
                }
            }
        }
    }
}
