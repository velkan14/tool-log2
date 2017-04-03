using Log2CyclePrototype.LoG2API.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Log2CyclePrototype.LoG2API
{

    public class PuzzleConnection
    {
        private MapElement _element1, _element2;

        public MapElement[] Elements
        {
            get { return new MapElement[2] { _element1, _element2 }; }
        }

        public PuzzleConnection(MapElement ele1, MapElement ele2)
        {
            _element1 = ele1;
            _element2 = ele2;
        }
    }

    [Serializable]
    public class Map
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
                foreach(Cell c in cells)
                {
                    if (c.IsWalkable)
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
        public Dictionary<string, MapElement> Elements { get; internal set; }

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


    }
}
