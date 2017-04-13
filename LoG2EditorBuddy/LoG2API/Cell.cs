using System;
using System.Collections.Generic;
using System.Drawing;
using Log2CyclePrototype.LoG2API.Elements;

namespace Log2CyclePrototype.LoG2API
{
    [Serializable]
    public class Cell
    {

        protected static Image imageIcons = Image.FromFile("../../img/icons.png"); //FIXME

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
            get { return elements; }
            set { elements = value; }
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
        private List<MapElement> elements;
        private bool isStartingPoint, isEndingPoint;
        private bool isWalkable;
        private int _neighbours;
        public Monster Monster { get; set; }

        //Draw ----------------------------------------------
        private static Pen penBorder = new Pen(new SolidBrush(Color.FromArgb(70, 70, 70)), 1);
        private static SolidBrush brushWalkable = new SolidBrush(Color.FromArgb(100, Color.Black));
        private static SolidBrush brushUnwalkable = new SolidBrush(Color.FromArgb(170,Color.Black));


        public Cell(int x, int y, int type)
        {
            elements = new List<MapElement>();
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
            elements.Add(element);
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
                return Monster.ElementType.Equals(c.Monster.ElementType);
            }
            if (c.Monster == null && Monster == null) return true;

            return false;
        }

        public void Draw(Graphics panel, int cellWidth, int cellHeight)
        {
            Rectangle destRect = new Rectangle(X * cellWidth, Y * cellHeight, cellWidth, cellHeight);
            Rectangle srcRectWalk = new Rectangle(0, 480, 20, 20);
            Rectangle srcRectUnwalk = new Rectangle(80, 480, 20, 20);

            if (IsWalkable)
            {
                panel.DrawImage(imageIcons, destRect, srcRectWalk, GraphicsUnit.Pixel);
                panel.FillRectangle(brushWalkable, X * cellWidth + 1, Y * cellHeight + 1, cellWidth - 1, cellHeight - 1);
                //panel.FillRectangle(new TextureBrush(imageIcons), X * cellWidth + 1, Y * cellHeight + 1, cellWidth - 1, cellHeight - 1);
                
                
            }
            else
            {
                panel.DrawImage(imageIcons, destRect, srcRectUnwalk, GraphicsUnit.Pixel);
                panel.FillRectangle(brushUnwalkable, X * cellWidth + 1, Y * cellHeight + 1, cellWidth - 1, cellHeight - 1);
            }
                

            panel.DrawRectangle(penBorder, X * cellWidth, Y * cellHeight, cellWidth, cellHeight);

            if(Monster != null)
            {
                Monster.Draw(panel, cellWidth, cellHeight);
            }
        }


    }
}
