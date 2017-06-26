using EditorBuddyMonster.LoG2API;
using EditorBuddyMonster.LoG2API.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorBuddyMonster.Layers
{
    public class AreaManager
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
        //private static Brush selectionBrushAdd = new SolidBrush(Color.FromArgb(128, 72, 145, 220));
        //private static Pen pen = new Pen(new SolidBrush(Color.FromArgb(51, 153, 255)), 1);
        /*private static Brush selectionBrushAddEasy = new SolidBrush(Color.FromArgb(128, 255, 170, 170));
        private static Pen penEasy = new Pen(new SolidBrush(Color.FromArgb(255, 170, 170)), 2);
        private static Brush selectionBrushAddMedium = new SolidBrush(Color.FromArgb(128, 170, 57, 75));
        private static Pen penMedium = new Pen(new SolidBrush(Color.FromArgb(170, 57, 57)), 2);
        private static Brush selectionBrushAddHard = new SolidBrush(Color.FromArgb(128, 85, 0, 0));
        private static Pen penHard = new Pen(new SolidBrush(Color.FromArgb(85, 0, 0)), 2);*/

        private static Brush selectionBrushAddEasy = new SolidBrush(Color.FromArgb(128, 72, 145, 220));
        private static Pen penEasy = new Pen(new SolidBrush(Color.FromArgb(51, 153, 255)), 2);
        private static Brush selectionBrushAddMedium = new SolidBrush(Color.FromArgb(128, 220, 214, 72));
        private static Pen penMedium = new Pen(new SolidBrush(Color.FromArgb(255, 247, 51)), 2);
        private static Brush selectionBrushAddHard = new SolidBrush(Color.FromArgb(128, 220, 72, 72));
        private static Pen penHard = new Pen(new SolidBrush(Color.FromArgb(255, 51, 51)), 2);


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

            //area1.Add(map.GetCellAt(20, 11));
            //area1.Add(map.GetCellAt(20, 12));
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
            area2.Add(map.GetCellAt(15, 12));

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
            //area6.Add(map.GetCellAt(5, 20));
            area6.Add(map.GetCellAt(6, 20));
            //area6.Add(map.GetCellAt(7, 20));
            area6.Add(map.GetCellAt(8, 20));
            area6.Add(map.GetCellAt(9, 20));
            area6.Add(map.GetCellAt(10, 20));
            area6.Add(map.GetCellAt(6, 21));
            area6.Add(map.GetCellAt(7, 21));
            area6.Add(map.GetCellAt(8, 21));
            area6.Add(map.GetCellAt(9, 21));
            area6.Add(map.GetCellAt(10, 21));

            AreaList.Add(new Area("Area 1", area1));
            AreaList.Add(new Area("Area 2", area2));
            AreaList.Add(new Area("Area 3", area3));
            AreaList.Add(new Area("Area 4", area4));
            AreaList.Add(new Area("Area 5", area5));
            AreaList.Add(new Area("Area 6", area6));

            foreach (Area a in AreaList)
            {
                a.StartCell = GetStartCell(a);
            }
        }

        public AreaManager(Map map, Panel gridPanel, AreaManager manager)
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

            //area1.Add(map.GetCellAt(20, 11));
            //area1.Add(map.GetCellAt(20, 12));
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
            area2.Add(map.GetCellAt(15, 12));

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
            //area6.Add(map.GetCellAt(5, 20));
            area6.Add(map.GetCellAt(6, 20));
            //area6.Add(map.GetCellAt(7, 20));
            area6.Add(map.GetCellAt(8, 20));
            area6.Add(map.GetCellAt(9, 20));
            area6.Add(map.GetCellAt(10, 20));
            area6.Add(map.GetCellAt(6, 21));
            area6.Add(map.GetCellAt(7, 21));
            area6.Add(map.GetCellAt(8, 21));
            area6.Add(map.GetCellAt(9, 21));
            area6.Add(map.GetCellAt(10, 21));

            AreaList.Add(new Area("Area 1", area1));
            AreaList.Add(new Area("Area 2", area2));
            AreaList.Add(new Area("Area 3", area3));
            AreaList.Add(new Area("Area 4", area4));
            AreaList.Add(new Area("Area 5", area5));
            AreaList.Add(new Area("Area 6", area6));

            for (int i = 0; i < AreaList.Count; i++)
            {
                AreaList[i].StartCell = GetStartCell(AreaList[i]);
                AreaList[i].Difficulty = manager.AreaList[i].Difficulty;
                AreaList[i].Visible = manager.AreaList[i].Visible;
                AreaList[i].Selected = manager.AreaList[i].Selected;
            }

        }

        public Cell GetStartCell(Area area)
        {
            StartingPoint st = map.StartPoint;
            List<Cell> unvisited = map.WalkableCells;

            Cell firstCell = unvisited.FirstOrDefault(x => x.X == st.x && x.Y == st.y);
            unvisited.RemoveAll(x => x.X == st.x && x.Y == st.y);

            Cell c = IterativeFlood(firstCell, area, unvisited);
            return c;
        }

        private Cell IterativeFlood(Cell cell, Area target, List<Cell> unvisited)
        {
            if (target.Contains(cell)) return cell;

            List<Cell> neighboors = new List<Cell>();

            Cell c1 = unvisited.FirstOrDefault(x => x.X == cell.X - 1 && x.Y == cell.Y);
            if (c1 != null)
            {
                unvisited.RemoveAll(x => x.X == c1.X && x.Y == c1.Y);
                Cell c = IterativeFlood(c1, target, unvisited);
                if (c != null) return c;
            }

            Cell c2 = unvisited.FirstOrDefault(x => x.X == cell.X && x.Y == cell.Y - 1);
            if (c2 != null)
            {
                unvisited.RemoveAll(x => x.X == c2.X && x.Y == c2.Y);
                Cell c = IterativeFlood(c2, target, unvisited);
                if (c != null) return c;
            }
            Cell c3 = unvisited.FirstOrDefault(x => x.X == cell.X + 1 && x.Y == cell.Y);
            if (c3 != null)
            {
                unvisited.RemoveAll(x => x.X == c3.X && x.Y == c3.Y);
                Cell c = IterativeFlood(c3, target, unvisited);
                if (c != null) return c;
            }
            Cell c4 = unvisited.FirstOrDefault(x => x.X == cell.X && x.Y == cell.Y + 1);
            if (c4 != null)
            {
                unvisited.RemoveAll(x => x.X == c4.X && x.Y == c4.Y);
                Cell c = IterativeFlood(c4, target, unvisited);
                if (c != null) return c;
            }

            return null;
        }

        public Image Draw(int width, int height, Image backgroundImage)
        {
            using (Graphics g = Graphics.FromImage(backgroundImage))
            {
                foreach (Area area in AreaList)
                {
                    if (area.Visible)
                    {
                        foreach (var p in area.Cells)
                        {
                            if (p.X > 0)//test left
                            {
                                var indexLeft = area.Cells.FindIndex(point => (point.X == (p.X - 1) && point.Y == p.Y));
                                if (indexLeft == -1)
                                {
                                    DrawBorder(p.X, p.Y, area, BorderSide.Left, g);
                                }
                                    
                            }
                            else
                            {
                                DrawBorder(p.X, p.Y, area, BorderSide.Left, g);
                            }

                            if (p.X < map.Width - 1) //test right
                            {
                                var indexRight = area.Cells.FindIndex(point => (point.X == (p.X + 1) && point.Y == p.Y));
                                if (indexRight == -1)
                                    DrawBorder(p.X, p.Y, area, BorderSide.Right, g);
                            }
                            else
                            {
                                DrawBorder(p.X, p.Y, area, BorderSide.Right, g);
                            }
                            if (p.Y > 0) //test up
                            {
                                var indexUp = area.Cells.FindIndex(point => (point.X == p.X && point.Y == (p.Y - 1)));
                                if (indexUp == -1)
                                    DrawBorder(p.X, p.Y, area, BorderSide.Top, g);
                            }
                            else
                            {
                                DrawBorder(p.X, p.Y, area, BorderSide.Top, g);
                            }
                            if (p.Y < map.Height - 1) //test bottom
                            {
                                var indexBot = area.Cells.FindIndex(point => (point.X == p.X && point.Y == (p.Y + 1)));
                                if (indexBot == -1)
                                    DrawBorder(p.X, p.Y, area, BorderSide.Bottom, g);
                            }
                            else
                            {
                                DrawBorder(p.X, p.Y, area, BorderSide.Bottom, g);
                            }

                            if (area.Selected)
                            {
                                if (area.Difficulty == RoomDifficulty.Safe) g.FillRectangle(selectionBrushAddEasy, p.X * cellWidth, p.Y * cellHeight, cellWidth, cellHeight);
                                else if (area.Difficulty == RoomDifficulty.Medium) g.FillRectangle(selectionBrushAddMedium, p.X * cellWidth, p.Y * cellHeight, cellWidth, cellHeight);
                                else if (area.Difficulty == RoomDifficulty.Hard) g.FillRectangle(selectionBrushAddHard, p.X * cellWidth, p.Y * cellHeight, cellWidth, cellHeight);

                            }
                        }
                    }

                }
            }

            return backgroundImage;
        }

        private void DrawBorder(int x, int y, Area area, BorderSide s, Graphics g)
        {
            if (area.Difficulty == RoomDifficulty.Safe) DrawBorder(x, y, penEasy, s, g);
            else if (area.Difficulty == RoomDifficulty.Medium) DrawBorder(x, y, penMedium, s, g);
            else if (area.Difficulty == RoomDifficulty.Hard) DrawBorder(x, y, penHard, s, g);
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
            foreach (Area a in AreaList)
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
