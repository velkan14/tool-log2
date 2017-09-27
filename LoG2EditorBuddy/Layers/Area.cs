using Povoater.LoG2API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Povoater.Layers
{
    
    public class Area
    {
        public string Name { get; set; }
        public List<Cell> Cells { get; set; }
        public Cell StartCell { get; set; }
        public bool Visible { get; set; }
        public RoomDifficulty Difficulty { get; set; }
        public ItemAccessibility ItemAccessibility { get; set; }

        public bool Selected { get; internal set; }
        public int Size { get { return Cells.Count; } }

        public Area(string name, List<Cell> cells)
        {
            Name = name;
            Cells = cells;
            Difficulty = RoomDifficulty.Safe;
            ItemAccessibility = ItemAccessibility.SafeToGet;
            Visible = true;
        }

        public bool Contains(Cell c)
        {
            foreach(Cell k in Cells)
            {
                if (k.X == c.X && k.Y == c.Y) return true;
            }
            return false;
        }

        public double GetMaxDifficulty()
        {
            return Size / 9.0;
        }

        public double GetDesireDifficulty()
        {
            switch (Difficulty)
            {
                case RoomDifficulty.Safe:
                    {
                        return 0.0;
                    }
                case RoomDifficulty.Medium:
                    {
                        return GetMaxDifficulty() / 2.0;
                    }
                case RoomDifficulty.Hard:
                    {
                        return GetMaxDifficulty();
                    }
            }
            return 0;
        }
       
    }
}
