using Log2CyclePrototype.LoG2API.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Log2CyclePrototype.LoG2API
{

    [Serializable]
    public class Map : DrawAbstract
    {

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public StartingPoint StartPoint
        {
            get { return startingPoint; }
            set { startingPoint = value; }
        }

        public List<EndingPoint> EndPointList
        {
            get { return endingPoints; }
            set { endingPoints = value; }
        }

        public List<Cell> Cells
        {
            get { return cells; }
            set { cells = value; }
        }

        public List<Cell> WalkableCells
        {
            get
            {
                List<Cell> list = new List<Cell>();
                foreach (Cell c in cells)
                {
                    if (c.IsWalkable)
                    {
                        list.Add(c);
                    }
                }
                return list;
            }
        }

        public List<Cell> SpawnCells
        {
            get
            {
                List<Cell> list = new List<Cell>();
                Altar.AltarType type;
                foreach (Cell c in cells)
                {
                    if (c.IsWalkable && !c.IsEndingPoint && !c.IsStartingPoint && !c.ElementsInCell.Exists(el => Altar.AltarType.TryParse(el.ElementType, out type)))
                    {
                        list.Add(c);
                    }
                }
                return list;
            }
        }

        public ArrayList Tiles
        {
            get { return tiles; }
            set { tiles = value; }
        }


        public int[] LevelCoord
        {
            get { return levelCoord; }
            set { levelCoord = value; }
        }

        public string AmbientTrack
        {
            get { return ambientTrack; }
            set { ambientTrack = value; }
        }

        public int Id
        {
            get;
            set;
        }

        public Dictionary<string, MapElement> Elements { get; set; }

        private string name;
        private int width, height;
        private int[] levelCoord;
        private string ambientTrack;
        private ArrayList tiles;
        private List<Cell> cells;
        private StartingPoint startingPoint;
        private List<EndingPoint> endingPoints;

        /// <summary>
        /// Empty constructor (needed for JSONClone)
        /// </summary>
        public Map() { }

        /// <summary>
        /// Initial Map constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public Map(string name, int w, int h)
        {
            this.name = name;
            width = w;
            height = h;
            cells = new List<Cell>();
            startingPoint = null;
            endingPoints = new List<EndingPoint>();
        }

        internal bool BelongToHorde(Cell cell)
        {
            //Check if neighbours have a monster.
            if (GetCellAt(cell.X - 1, cell.Y - 1).Monster != null) return true;
            if (GetCellAt(cell.X,     cell.Y - 1).Monster != null) return true;
            if (GetCellAt(cell.X + 1, cell.Y - 1).Monster != null) return true;

            if (GetCellAt(cell.X - 1, cell.Y).Monster != null) return true;
            if (GetCellAt(cell.X + 1, cell.Y).Monster != null) return true;

            if (GetCellAt(cell.X - 1, cell.Y + 1).Monster != null) return true;
            if (GetCellAt(cell.X,     cell.Y + 1).Monster != null) return true;
            if (GetCellAt(cell.X + 1, cell.Y + 1).Monster != null) return true;
            return false;
        }

        internal bool CloseToElement(Cell cell, string element)
        {
            if (GetCellAt(cell.X, cell.Y).GetElement(element) != null) return true;
            /*if (GetCellAt(monster.x - 1, monster.y - 1).GetElement(element) != null) return true;
            if (GetCellAt(monster.x, monster.y - 1).GetElement(element) != null) return true;
            if (GetCellAt(monster.x + 1, monster.y - 1).GetElement(element) != null) return true;

            if (GetCellAt(monster.x - 1, monster.y).GetElement(element) != null) return true;
            if (GetCellAt(monster.x + 1, monster.y).GetElement(element) != null) return true;

            if (GetCellAt(monster.x - 1, monster.y + 1).GetElement(element) != null) return true;
            if (GetCellAt(monster.x, monster.y + 1).GetElement(element) != null) return true;
            if (GetCellAt(monster.x + 1, monster.y + 1).GetElement(element) != null) return true;*/
            return false;
        }

        public Cell GetCellAt(int x, int y)
        {
            return Cells[Height * y + x];
        }

        public bool SetCellAt(int x, int y, Cell value)
        {
            bool result = false;
            try
            {
                Cells[Height * y + x] = value;
                result = true;
            }
            catch (Exception e) { Utilities.DebugUtilities.DebugException(e); }
            return result;
        }

        public bool SetCell(Cell value)
        {
            bool result = false;
            try
            {
                Cells[Height * value.Y + value.X] = value;
                result = true;
            }
            catch (Exception e) { Utilities.DebugUtilities.DebugException(e); }
            return result;
        }

        public Cell GetCellAt(int index)
        {
            return Cells[index];
        }

        public bool SetCellAt(int index, Cell value)
        {
            bool result = false;
            try
            {
                Cells[index] = value;
                result = true;
            }
            catch (Exception e) { Utilities.DebugUtilities.DebugException(e); }
            return result;
        }

        public string PrintMap()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("TILES");
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    sb.Append(cells[j + height * i].CellType);  // REVER
                }
                sb.AppendLine();
            }
            sb.AppendLine("ELEMENTS");
            /*foreach (string key in mapElements.Keys)
            {

                if (puzzleConnections.ContainsKey(key))
                    sb.AppendLine(key + " -> " + puzzleConnections[key]);
                else sb.AppendLine(key);
            }*/

            return sb.ToString();
        }

        public override Image Draw(int width, int height)
        {
            Bitmap image = new Bitmap(width, height);

            using (Graphics panel = Graphics.FromImage(image))
            {
                panel.Clear(Color.LightGray);

                int cellWidth = width / this.Width;
                int cellHeight = height / this.Height;

                foreach (Cell c in Cells)
                {
                    c.Draw(panel, cellWidth, cellHeight);
                }

                foreach (MapElement el in Elements.Values)
                {
                    el.Draw(panel, cellWidth, cellHeight);
                }
            }

            return image;
        }

        public override Image Draw(int width, int height, Image backgroundImage)
        {
            using (Graphics panel = Graphics.FromImage(backgroundImage))
            {
                int cellWidth = width / this.Width;
                int cellHeight = height / this.Height;

                foreach (Cell c in Cells)
                {
                    c.Draw(panel, cellWidth, cellHeight);
                }

                foreach (MapElement el in Elements.Values)
                {
                    el.Draw(panel, cellWidth, cellHeight);
                }
            }

            return backgroundImage;
        }

        public string getToolTipInfo(int xx, int yy, int width, int height)
        {
            int cellWidth = width / this.Width;
            int cellHeight = height / this.Height;
            int x = xx / cellWidth;
            int y = yy / cellHeight;

            Cell c = GetCellAt(x, y);
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"X:{0} Y:{1}", c.X, c.Y);
            if(c.Monster != null)
                sb.AppendFormat(@"{0}{1}", '\n', c.Monster.ElementType);
            foreach(MapElement e in c.ElementsInCell)
            {
                if(e.ElementType.Equals("cudgel") ||
                   e.ElementType.Equals("machete") ||
                   e.ElementType.Equals("rapier") ||
                   e.ElementType.Equals("battle_axe") ||
                   e.ElementType.Equals("potion_healing") ||
                   e.ElementType.Equals("borra") ||
                   e.ElementType.Equals("bread") ||
                   e.ElementType.Equals("peasant_cap") ||
                   e.ElementType.Equals("peasant_breeches") ||
                   e.ElementType.Equals("peasant_tunic") ||
                   e.ElementType.Equals("sandals") ||
                   e.ElementType.Equals("leather_cap") ||
                   e.ElementType.Equals("leather_brigandine") ||
                   e.ElementType.Equals("leather_pants") ||
                   e.ElementType.Equals("leather_boots"))
                {
                    sb.AppendFormat(@"{0}{1}", '\n', e.ElementType);
                }
            }

            return sb.ToString();
        }
    }
}
