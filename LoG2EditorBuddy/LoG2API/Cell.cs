using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Log2CyclePrototype.LoG2API.Elements;
//using Log2CyclePrototype.Algorithm;

namespace Log2CyclePrototype
{
    [Serializable]
    public class Cell
    {

        public enum Type
        {
            //floor types
            beach_ground,
            beach_ground_grass,
            beach_ground_stones,
            beach_ground_water,
            beach_wall,
            castle_arena_floor,
            castle_floor,
            castle_floor_tall,
            castle_floor_water,
            castle_wall,
            catacomb_floor,
            dungeon_floor,
            dungeon_floor_tiles,
            dungeon_floor_water,
            dungeon_wall,
            forest_ground,
            forest_ground2,
            forest_hedge,
            forest_trail,
            forest_trees,
            forest_underwater,
            forest_wall,
            mine_floor,
            mine_floor_crystal,
            mine_floor_grass,
            mine_floor_grass_leaves,
            mine_wall,
            mine_wall_crystal,
            mine_wall_grass,
            mine_wall_grass_leaves,
            swamp_ground,
            swamp_ground2,
            swamp_trees,
            swamp_trees_saplings,
            tomb_floor,
            tomb_wall
        }

        /// <summary>
        /// Make cell walkable by changing CellType
        /// </summary>
        public bool IsWalkable
        {
            get { return isWalkable; }
            set { isWalkable = value; }
        }

        /// <summary>
        /// Indicates if Cell is StartingPoint
        /// </summary>
        public bool IsStartingPoint
        {
            get { return isStartingPoint; }
            set { isStartingPoint = value; }
        }

        /// <summary>
        /// Indicates if Cell is EndingPoint
        /// </summary>
        public bool IsEndingPoint
        {
            get { return isEndingPoint; }
            set { isEndingPoint = value; }
        }

        /// <summary>
        /// Indicates Cell type
        /// </summary>
        public int CellType
        {
            get { return type; }
            set { type = value; }
        }

        public List<MapElement> ElementsInCell
        {
            get { return _mapElements; }
            set { _mapElements = value; }
        }

        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public bool SelectedToDraw { get; set; }

        public int NumNeighbours
        {
            get { return _neighbours; }
            set { _neighbours = value; }
        }

        public int Id
        {
            get;
            set;
        }

        public StartingPoint StartPoint { get; set; }

        public EndingPoint EndPoint { get; set; }

        private int _x, _y;
        private int type;
        private List<MapElement> _mapElements;
        private bool isStartingPoint, isEndingPoint;
        private bool isWalkable;
        private int _neighbours;
        public Monster Monster { get; set; }

        //Draw ----------------------------------------------
        private static Pen penBorder = new Pen(new SolidBrush(Color.FromArgb(70, 70, 70)), 1);
        private static SolidBrush brushWalkable = new SolidBrush(Color.FromArgb(125, 125, 125));
        private static SolidBrush brushUnwalkable = new SolidBrush(Color.LightGray);


        public Cell(int x, int y, int type)
        {
            _mapElements = new List<MapElement>();
            _x = x;
            _y = y;
            this.type = type;
            isStartingPoint = false;
            isEndingPoint = false;
            isWalkable = false;
            _neighbours = 0;
            SelectedToDraw = false;
            //Enum.TryParse(type, true, out _type);
        }

        //public void AddNeighbour(Cell newNeighbour)
        //{
        //    _neighbours++;
        //}

        public void AddElement(MapElement element)
        {
            _mapElements.Add(element);
        }


        /// <summary>
        /// Simple Manhattan distance between cells
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public double DistanceTo(Cell other)
        {
            //return (double)System.Math.Abs(_type - other._type);
            return Math.Abs(other.X - this.X) + Math.Abs(other.Y - this.Y);
        }

        public bool SameMonsters(Cell c)
        {
            if(c.Monster != null && Monster != null)
            {
                return Monster.type == c.Monster.type;
            }
            if (c.Monster == null && Monster == null) return true;

            return false;
        }

        public void Draw(Graphics cellPanelGraphics, int cellWidth, int cellHeight)
        {
            if (IsWalkable)
                cellPanelGraphics.FillRectangle(brushWalkable, X * cellWidth + 1, Y * cellHeight + 1, cellWidth - 1, cellHeight - 1);
            else
                cellPanelGraphics.FillRectangle(brushUnwalkable, X * cellWidth + 1, Y * cellHeight + 1, cellWidth - 1, cellHeight - 1);
           
            cellPanelGraphics.DrawRectangle(penBorder, X * cellWidth, Y * cellHeight, cellWidth, cellHeight);

            if(Monster != null)
            {
                Monster.Draw(cellPanelGraphics, cellWidth, cellHeight);
            }
        }


    }
}
