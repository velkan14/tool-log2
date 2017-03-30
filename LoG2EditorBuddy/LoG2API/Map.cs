using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Log2CyclePrototype.Algorithm;

namespace Log2CyclePrototype
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
            get { return _name; }
            set { _name = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public StartingPoint StartPoint
        {
            get { return _startingPoint; }
            set { _startingPoint = value; }
        }

        public List<EndingPoint> EndPointList
        {
            get { return _endingPoints; }
            set { _endingPoints = value; }
        }

        public List<Cell> Cells
        {
            get { return _cells; }
            set { _cells = value; }
        }

        public List<Cell> WalkableCells
        {
            get { return _walkableCells; }
            set { _walkableCells = value; }
        }

        public Hashtable MapElements
        {
            get { return _mapElements; }
            set { _mapElements = value; }
        }

        public Hashtable PuzzleConnections
        {
            get { return _puzzleConnections; }
            set { _puzzleConnections = value; }
        }

        public ArrayList DifferentTiles
        {
            get { return _differentTiles; }
            set { _differentTiles = value; }
        }


        public int[] LevelCoord
        {
            get { return _levelCoord; }
            set { _levelCoord = value; }
        }

        public string AmbientTrack
        {
            get { return _ambientTrack; }
            set { _ambientTrack = value; }
        }

        public int Id
        {
            get;
            set;
        }

        public List<string> MapObjects { get { return _mapObjects; } set { _mapObjects = value; } }

        private string _name;
        private int _width, _height;
        private int[] _levelCoord;
        private string _ambientTrack;
        private ArrayList _differentTiles;
        //private List<Cell> _cells; //stores map cells
        private object[] _tiles;
        private List<Cell> _cells, _walkableCells;
        private Hashtable _mapElements; //id, object
        //private List<PuzzleConnection> _puzzleConnections; //stores puzzle element connections for easier evaluation
        private Hashtable _puzzleConnections;
        private StartingPoint _startingPoint;
        private List<EndingPoint> _endingPoints;
        private List<string> _mapObjects;

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
            _name = name;
            _width = w;
            _height = h;
            _tiles = new object[w * h];
            _cells = new List<Cell>();
            _walkableCells = new List<Cell>();
            _mapElements = new Hashtable();
            _puzzleConnections = new Hashtable();
            _startingPoint = null;
            _endingPoints = new List<EndingPoint>();
            _mapObjects = new List<string>();
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


        public void AddLever(Lever lever)
        {
            _mapElements.Add(lever.uniqueID, lever);
            _puzzleConnections.Add(lever.uniqueID, lever.ConnectedTo);
        }

        public void AddDoor(Door door)
        {
            _mapElements.Add(door.uniqueID, door);
        }


        public void AddTorch()
        {
            //_mapElements.Add();
        }



        public string PrintMap()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("TILES");
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    sb.Append(_cells[j + _height * i].CellType);  // REVER
                }
                sb.AppendLine();
            }
            sb.AppendLine("ELEMENTS");
            foreach (string key in _mapElements.Keys)
            {

                if (_puzzleConnections.ContainsKey(key))
                    sb.AppendLine(key + " -> " + _puzzleConnections[key]);
                else sb.AppendLine(key);
            }

            return sb.ToString();
        }


    }
}
