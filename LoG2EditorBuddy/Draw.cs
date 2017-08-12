using Povoater.LoG2API;
using Povoater.LoG2API.Elements;
using Povoater.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Povoater
{
    class Draw
    {
        enum CellHighlight
        {
            None,
            All,
            Added,
            Removed
        }

        enum BorderSide
        {
            Top,
            Left,
            Bottom,
            Right
        }

        private CellHighlight _cellHighlightSettings = CellHighlight.All;

        private int cellWidth, cellHeight;
        private bool _hideSuggestionWhenSelecting = false;

        private Graphics cellPanelGraphics;
        public Bitmap cellBitmap;

        private SolidBrush _groundTile = new SolidBrush(Color.FromArgb(125, 125, 125));
        private Brush selectionBrushAdd = new SolidBrush(Color.FromArgb(128, 72, 145, 220));
        private Brush selectionBrushRemove = new SolidBrush(Color.FromArgb(128, 255, 59, 78));

        private Color _addedCellColorSelected = Color.FromArgb(102, 141, 255),
                      _removedCellColorSelected = Color.FromArgb(255, 102, 102),
                      _addedCellColorUnselected = Color.FromArgb(125, 102, 141, 255),
                      _removedCellColorUnselected = Color.FromArgb(125, 255, 102, 102),
                      _groundTileColorSelected = Color.FromArgb(125, 125, 125),
                      _groundTileColorUnselected = Color.FromArgb(125, 125, 125, 125),
                      _cellBorderColor = Color.FromArgb(70, 70, 70),
                      emptyCellColor = Color.LightGray;

        private List<Cell> _cellsToDraw;
        private Panel gridPanel;
        private Bitmap prevDraw;

        public Draw(Panel gridPanel)
        {
            this.gridPanel = gridPanel;

            cellBitmap = new Bitmap(gridPanel.Width, gridPanel.Height);
            cellPanelGraphics = Graphics.FromImage(cellBitmap);
        }

        private void DrawStartEndPoints(StartingPoint start, List<EndingPoint> endPoints)
        {
            if (start != null)
                cellPanelGraphics.DrawString("S", new Font("ArialBold", 12), new SolidBrush(Color.Yellow), new RectangleF(new PointF(start.x * cellWidth, start.y * cellHeight), new SizeF(cellWidth, cellHeight)), StringFormat.GenericDefault);
            if (endPoints != null)
                foreach (var e in endPoints)
                    cellPanelGraphics.DrawString("E", new Font("ArialBold", 12), new SolidBrush(Color.Yellow), new RectangleF(new PointF(e.x * cellWidth, e.y * cellHeight), new SizeF(cellWidth, cellHeight)), StringFormat.GenericDefault);
        }

        private void FillCell(int x, int y, Color c)
        {
            SolidBrush b = new SolidBrush(c);
            Pen p = new Pen(new SolidBrush(_cellBorderColor), 1);
            cellPanelGraphics.FillRectangle(b, x * cellWidth + 1, y * cellHeight + 1, cellWidth - 1, cellHeight - 1);
            cellPanelGraphics.DrawRectangle(p, x * cellWidth, y * cellHeight, cellWidth, cellHeight);
        }

        private void DrawBorder(int x, int y, Pen p, BorderSide s)
        {
            switch (s)
            {
                case BorderSide.Top:
                    cellPanelGraphics.DrawLine(p, new Point(x * cellWidth, y * cellHeight), new Point(x * cellWidth + cellWidth, y * cellHeight));
                    break;
                case BorderSide.Left:
                    cellPanelGraphics.DrawLine(p, new Point(x * cellWidth, y * cellHeight), new Point(x * cellWidth, y * cellHeight + cellHeight));
                    break;
                case BorderSide.Bottom:
                    cellPanelGraphics.DrawLine(p, new Point(x * cellWidth, y * cellHeight + cellHeight), new Point(x * cellWidth + cellWidth, y * cellHeight + cellHeight));
                    break;
                case BorderSide.Right:
                    cellPanelGraphics.DrawLine(p, new Point(x * cellWidth + cellWidth, y * cellHeight), new Point(x * cellWidth + cellWidth, y * cellHeight + cellHeight));
                    break;
                default:
                    break;
            }
        }

        private void DrawUserSelection(Map currentMap, List<Point> userSelectedPoints)
        {
            using (Pen pen = new Pen(new SolidBrush(Color.FromArgb(0, 170, 0)), 3))
            {
                foreach (var p in userSelectedPoints)
                {
                    if (p.X > 0)//test left
                    {
                        var indexLeft = userSelectedPoints.FindIndex(point => (point.X == (p.X - 1) && point.Y == p.Y));
                        if (indexLeft == -1)
                            DrawBorder(p.X, p.Y, pen, BorderSide.Left);
                    }
                    else
                    {
                        DrawBorder(p.X, p.Y, pen, BorderSide.Left);
                    }

                    if (p.X < currentMap.Width - 1) //test right
                    {
                        var indexRight = userSelectedPoints.FindIndex(point => (point.X == (p.X + 1) && point.Y == p.Y));
                        if (indexRight == -1)
                            DrawBorder(p.X, p.Y, pen, BorderSide.Right);
                    }
                    else
                    {
                        DrawBorder(p.X, p.Y, pen, BorderSide.Right);
                    }
                    if (p.Y > 0) //test up
                    {
                        var indexUp = userSelectedPoints.FindIndex(point => (point.X == p.X && point.Y == (p.Y - 1)));
                        if (indexUp == -1)
                            DrawBorder(p.X, p.Y, pen, BorderSide.Top);
                    }
                    else
                    {
                        DrawBorder(p.X, p.Y, pen, BorderSide.Top);
                    }
                    if (p.Y < currentMap.Height - 1) //test bottom
                    {
                        var indexBot = userSelectedPoints.FindIndex(point => (point.X == p.X && point.Y == (p.Y + 1)));
                        if (indexBot == -1)
                            DrawBorder(p.X, p.Y, pen, BorderSide.Bottom);
                    }
                    else
                    {
                        DrawBorder(p.X, p.Y, pen, BorderSide.Bottom);
                    }
                }
            }
        }

        /*private void DrawUserLockedCells()
        {
            Pen pen = new Pen(new SolidBrush(Color.FromArgb(170, 0, 0)), 3);
            foreach (var p in _lockedCellList)
            {
                _cellPanelGraphics.DrawRectangle(pen, p.X * cellWidth, p.Y * cellHeight, cellWidth, cellHeight);
            }

        }*/

        public Bitmap ReDraw(Map currentMap, List<Point> userSelectedPoints)
        {
            cellWidth = gridPanel.Width / currentMap.Width;
            cellHeight = gridPanel.Height / currentMap.Height;

            //Clear everything
            cellPanelGraphics.Clear(emptyCellColor);

            //Draw grid
            /*using (Pen p = new Pen(_groundTile, 1))
            {
                for (int i = 0; i < currentMap.Height + 1; i++)
                    cellPanelGraphics.DrawLine(p, new Point(0, i * cellHeight), new Point(currentMap.Width * cellWidth, i * cellHeight));
                for (int j = 0; j < currentMap.Height + 1; j++)
                    cellPanelGraphics.DrawLine(p, new Point(j * cellWidth, 0), new Point(j * cellWidth, currentMap.Height * cellHeight));
            }*/

            int numberOfCells = currentMap.Width * currentMap.Height;

            for (int k = 0; k < numberOfCells; k++)
            {
                var c = currentMap.Cells[k];
                c.Draw(cellPanelGraphics, cellWidth, cellHeight);
            }

            foreach(MapElement el in currentMap.Elements.Values)
            {
                el.Draw(cellPanelGraphics, cellWidth, cellHeight);
            }
            //draw user selected cells
            if (userSelectedPoints.Count > 0)
                DrawUserSelection(currentMap, userSelectedPoints);
            /*if (_lockedCellList.Count > 0)
                DrawUserLockedCells();*/

            //DrawStartEndPoints(APIClass.CurrentMap.StartPoint, APIClass.CurrentMap.EndPointList); //No need anymore

            prevDraw = (Bitmap)cellBitmap.Clone();

            return cellBitmap;
        }

        public Bitmap DrawSelectionPreviewRectangle(Rectangle SelectionRect, bool selectionAdd)
        {
            Bitmap b = (Bitmap)prevDraw.Clone();
            using (Graphics g = Graphics.FromImage(b))
            {
                //User selection preview rect
                if (SelectionRect != null && SelectionRect.Width > 0 && SelectionRect.Height > 0)
                {
                    if (selectionAdd)
                        g.FillRectangle(selectionBrushAdd, SelectionRect);
                    else
                        g.FillRectangle(selectionBrushRemove, SelectionRect);
                }
            }
            return b;
        }
    }
}
