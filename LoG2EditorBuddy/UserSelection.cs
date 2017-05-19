using EditorBuddyMonster.LoG2API;
using EditorBuddyMonster.WinAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorBuddyMonster
{
    public class UserSelection : DrawAbstract
    {
        enum BorderSide
        {
            Top,
            Left,
            Bottom,
            Right
        }

        private List<Point> userSelectedPoints;

        private Point startPointSelection;
        private Rectangle SelectionRect = new Rectangle();
        private Panel gridPanel;
        private Core core;
        private Monsters windowInterface;
        private bool selectionAdd;

        int cellWidth = 0, cellHeight = 0;

        private Brush selectionBrushAdd = new SolidBrush(Color.FromArgb(128, 72, 145, 220));
        private Brush selectionBrushRemove = new SolidBrush(Color.FromArgb(128, 255, 59, 78));

        public bool Attached { get; private set; }

        public UserSelection(Monsters windowInterface, Core core, Panel gridPanel)
        {
            this.windowInterface = windowInterface;
            this.core = core;
            this.gridPanel = gridPanel;
            userSelectedPoints = new List<Point>();

            cellWidth = gridPanel.Width / core.CurrentMap.Width;
            cellHeight = gridPanel.Height / core.CurrentMap.Height;
        }

        public void Attach()
        {
            gridPanel.MouseDown += this.MouseDown;
            gridPanel.MouseMove += this.MouseMove;
            gridPanel.MouseUp += this.MouseUp;
            Attached = true;
            ClearSelection();
        }

        public void Dettach()
        {
            if (Attached)
            {
                gridPanel.MouseDown -= this.MouseDown;
                gridPanel.MouseMove -= this.MouseMove;
                gridPanel.MouseUp -= this.MouseUp;
                Attached = false;
            }
        }

        public List<Point> GetSelectedPoints()
        {
            return userSelectedPoints;
        }

        public void AddSelectedPoint(Point p)
        {
            if (!userSelectedPoints.Contains(p))
                userSelectedPoints.Add(p);
            else Debug.WriteLine("Point already in list, skipping");
        }

        public void RemoveSelectedPoint(Point p)
        {
            if (userSelectedPoints.Contains(p))
                userSelectedPoints.Remove(p);
        }

        public void ClearSelection()
        {
            userSelectedPoints = new List<Point>();
        }

        public bool InvertSelection()
        {
            if (userSelectedPoints.Count > 0)
            {
                var invertion = new List<Point>();
                for (int y = 0; y < APIClass.CurrentMap.Height; y++) //FIXME: Remove the APIClass from here!!
                {
                    for (int x = 0; x < APIClass.CurrentMap.Width; x++)
                    {
                        if (userSelectedPoints.FindIndex(c => (c.X == x && c.Y == y)) == -1)
                            invertion.Add(new Point(x, y));
                    }
                }
                userSelectedPoints = invertion;
                return true;
            }
            return false;
        }

        public override Image Draw(int width, int height)
        {
            throw new NotImplementedException();
        }

        public override Image Draw(int width, int height, Image backgroundImage)
        {
            using (Graphics g = Graphics.FromImage(backgroundImage))
            {
                //User selection preview rect
                if (SelectionRect != null && SelectionRect.Width > 0 && SelectionRect.Height > 0)
                {
                    if (selectionAdd)
                        g.FillRectangle(selectionBrushAdd, SelectionRect);
                    else
                        g.FillRectangle(selectionBrushRemove, SelectionRect);
                }

                using (Pen pen = new Pen(new SolidBrush(Color.FromArgb(51, 153, 255)), 1))
                {
                    foreach (var p in userSelectedPoints)
                    {
                        if (p.X > 0)//test left
                        {
                            var indexLeft = userSelectedPoints.FindIndex(point => (point.X == (p.X - 1) && point.Y == p.Y));
                            if (indexLeft == -1)
                                DrawBorder(p.X, p.Y, pen, BorderSide.Left, g);
                        }
                        else
                        {
                            DrawBorder(p.X, p.Y, pen, BorderSide.Left, g);
                        }

                        if (p.X < core.CurrentMap.Width - 1) //test right
                        {
                            var indexRight = userSelectedPoints.FindIndex(point => (point.X == (p.X + 1) && point.Y == p.Y));
                            if (indexRight == -1)
                                DrawBorder(p.X, p.Y, pen, BorderSide.Right, g);
                        }
                        else
                        {
                            DrawBorder(p.X, p.Y, pen, BorderSide.Right, g);
                        }
                        if (p.Y > 0) //test up
                        {
                            var indexUp = userSelectedPoints.FindIndex(point => (point.X == p.X && point.Y == (p.Y - 1)));
                            if (indexUp == -1)
                                DrawBorder(p.X, p.Y, pen, BorderSide.Top, g);
                        }
                        else
                        {
                            DrawBorder(p.X, p.Y, pen, BorderSide.Top, g);
                        }
                        if (p.Y < core.CurrentMap.Height - 1) //test bottom
                        {
                            var indexBot = userSelectedPoints.FindIndex(point => (point.X == p.X && point.Y == (p.Y + 1)));
                            if (indexBot == -1)
                                DrawBorder(p.X, p.Y, pen, BorderSide.Bottom, g);
                        }
                        else
                        {
                            DrawBorder(p.X, p.Y, pen, BorderSide.Bottom, g);
                        }

                        g.FillRectangle(selectionBrushAdd, p.X * cellWidth, p.Y * cellHeight, cellWidth, cellHeight);
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

        private void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                startPointSelection = e.Location;
                //Invalidate();

                if (e.Button == MouseButtons.Left)
                {
                    CursorManager.Instance.SetCursor(CursorType.Plus);
                    selectionAdd = true;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    CursorManager.Instance.SetCursor(CursorType.Minus);
                    selectionAdd = false;
                }
            }

        }


        private void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                Point tempEndPoint = e.Location;
                SelectionRect.Location = new Point(
                    System.Math.Min(startPointSelection.X, tempEndPoint.X),
                    System.Math.Min(startPointSelection.Y, tempEndPoint.Y));
                SelectionRect.Size = new Size(
                    System.Math.Abs(startPointSelection.X - tempEndPoint.X),
                    System.Math.Abs(startPointSelection.Y - tempEndPoint.Y));
                windowInterface.ReDraw();
            }

        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            int cellWidth = gridPanel.Width / core.OriginalMap.Width;
            int cellHeight = gridPanel.Height / core.OriginalMap.Height;

            Point topLeft = new Point(), bottomRight = new Point();

            Point startCellCoord = new Point(startPointSelection.X / cellWidth, startPointSelection.Y / cellHeight);
            Point endCellCoord = new Point(e.Location.X / cellWidth, e.Location.Y / cellHeight);

            //if 1 click on same cell
            if (startCellCoord.Equals(endCellCoord))
            {
                Debug.WriteLine("Click @ [" + startCellCoord.X.ToString() + "," + startCellCoord.Y.ToString() + "]");

                var tmpP = new Point(startCellCoord.X, startCellCoord.Y);

                //DELETE
                if (e.Button == MouseButtons.Right)
                {
                    RemoveSelectedPoint(tmpP);
                }
                //ADD
                else if (e.Button == MouseButtons.Left)
                {
                    AddSelectedPoint(tmpP);
                }

            }
            else //if ended on different cell
            {

                if (startCellCoord.X <= endCellCoord.X && startCellCoord.Y <= endCellCoord.Y) { topLeft = startCellCoord; bottomRight = endCellCoord; }
                else if (startCellCoord.X < endCellCoord.X && startCellCoord.Y > endCellCoord.Y) { topLeft = new Point(startCellCoord.X, endCellCoord.Y); bottomRight = new Point(endCellCoord.X, startCellCoord.Y); }
                else if (startCellCoord.X > endCellCoord.X && startCellCoord.Y < endCellCoord.Y) { topLeft = new Point(endCellCoord.X, startCellCoord.Y); bottomRight = new Point(startCellCoord.X, endCellCoord.Y); }
                else if (startCellCoord.X > endCellCoord.X && startCellCoord.Y > endCellCoord.Y) { topLeft = endCellCoord; bottomRight = startCellCoord; }

                int cellSelectionXRange = System.Math.Abs(startCellCoord.X - endCellCoord.X);
                int cellSelectionYRange = System.Math.Abs(startCellCoord.Y - endCellCoord.Y);

                if (topLeft != null && bottomRight != null)
                {
                    Debug.WriteLine(" TopLeft @ [" + topLeft.X.ToString() + "," + topLeft.Y.ToString() + "]");
                    Debug.WriteLine(" BottomRight @ [" + bottomRight.X.ToString() + "," + bottomRight.Y.ToString() + "]");
                }


                for (int j = topLeft.Y; j <= bottomRight.Y; j++)
                    for (int i = topLeft.X; i <= bottomRight.X; i++)
                    {
                        var tmpP = new Point(i, j);
                        //DELETE
                        if (e.Button == MouseButtons.Right)
                        {
                            RemoveSelectedPoint(tmpP);
                        }
                        //ADD
                        else if (e.Button == MouseButtons.Left)
                        {
                            AddSelectedPoint(tmpP);
                        }
                    }

            }


            //reset selection so it is no longer drawn on mouse up
            SelectionRect = new Rectangle(new Point(0, 0), new Size(0, 0));
            windowInterface.ReDraw();
        }
    }
}
