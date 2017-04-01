using Log2CyclePrototype.LoG2API;
using Log2CyclePrototype.LoG2API.Elements;
using Log2CyclePrototype.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log2CyclePrototype
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

        private Image imageMonster;

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
                      _emptyCellColor = Color.LightGray;

        private List<Cell> _cellsToDraw;
        private Panel gridPanel;
        private Bitmap prevDraw;

        public Draw(Panel gridPanel)
        {
            this.gridPanel = gridPanel;

            cellBitmap = new Bitmap(gridPanel.Width, gridPanel.Height);
            cellPanelGraphics = Graphics.FromImage(cellBitmap);
            imageMonster = new Bitmap("../../monster.png");
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
            cellPanelGraphics.Clear(_emptyCellColor);

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

                /*c.SelectedToDraw = false;

                switch (_cellHighlightSettings)
                {
                    case CellHighlight.None:
                        if (c.IsWalkable)
                            FillCell(c.X, c.Y, _groundTileColorSelected);
                        break;

                    case CellHighlight.All:
                        if ((APIClass.CurrentMap.Cells[c.Y * 32 + c.X].IsWalkable) && !(c.IsWalkable))
                        {
                            if (userSelectedPoints.FindIndex(e => (e.X == c.X && e.Y == c.Y)) != -1)
                                FillCell(c.X, c.Y, _removedCellColorSelected);
                            else
                                FillCell(c.X, c.Y, _removedCellColorUnselected);
                        }
                        else if (!(APIClass.CurrentMap.Cells[c.Y * 32 + c.X].IsWalkable) && c.IsWalkable)
                        {
                            if (userSelectedPoints.FindIndex(e => (e.X == c.X && e.Y == c.Y)) != -1)
                                FillCell(c.X, c.Y, _addedCellColorSelected);
                            else
                                FillCell(c.X, c.Y, _addedCellColorUnselected);
                        }
                        else if (c.IsWalkable)
                            FillCell(c.X, c.Y, _groundTileColorSelected);
                        break;

                    case CellHighlight.Added:
                        if (!(APIClass.CurrentMap.Cells[c.Y * 32 + c.X].IsWalkable) && c.IsWalkable)
                        {
                            if (userSelectedPoints.FindIndex(e => (e.X == c.X && e.Y == c.Y)) != -1)
                                FillCell(c.X, c.Y, _addedCellColorSelected);
                            else
                                FillCell(c.X, c.Y, _addedCellColorUnselected);
                        }
                        else if (c.IsWalkable)
                            FillCell(c.X, c.Y, _groundTileColorSelected);
                        break;

                    case CellHighlight.Removed:
                        if ((APIClass.CurrentMap.Cells[c.Y * 32 + c.X].IsWalkable) && !(c.IsWalkable))
                        {
                            if (userSelectedPoints.FindIndex(e => (e.X == c.X && e.Y == c.Y)) != -1)
                                FillCell(c.X, c.Y, _removedCellColorSelected);
                            else
                                FillCell(c.X, c.Y, _removedCellColorUnselected);
                        }
                        else if (c.IsWalkable)
                            FillCell(c.X, c.Y, _groundTileColorSelected);
                        break;

                    default:
                        if (c.IsWalkable)
                            FillCell(c.X, c.Y, _groundTileColorSelected);
                        break;
                }*/

                /*if (c.Monster != null)
                {
                    var el = c.Monster;
                    switch (el.orientation)
                    {
                        case MapElement.Orientation.Top:
                            cellPanelGraphics.DrawImage(imageMonster, el.x * cellWidth, el.y * cellHeight, cellWidth, cellHeight);
                            break;
                        case MapElement.Orientation.Right:
                            imageMonster.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            cellPanelGraphics.DrawImage(imageMonster, el.x * cellWidth, el.y * cellHeight, cellWidth, cellHeight);
                            imageMonster.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                        case MapElement.Orientation.Down:
                            imageMonster.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            cellPanelGraphics.DrawImage(imageMonster, el.x * cellWidth, el.y * cellHeight, cellWidth, cellHeight);
                            imageMonster.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case MapElement.Orientation.Left:
                            imageMonster.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            cellPanelGraphics.DrawImage(imageMonster, el.x * cellWidth, el.y * cellHeight, cellWidth, cellHeight);
                            imageMonster.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                    }
                }*/
            }

            //draw user selected cells
            if (userSelectedPoints.Count > 0)
                DrawUserSelection(currentMap, userSelectedPoints);
            /*if (_lockedCellList.Count > 0)
                DrawUserLockedCells();*/

            DrawStartEndPoints(APIClass.CurrentMap.StartPoint, APIClass.CurrentMap.EndPointList);

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
