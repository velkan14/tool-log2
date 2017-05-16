﻿using Log2CyclePrototype.LoG2API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype.Layers
{
    
    public class Area
    {
        public string Name { get; set; }
        public List<Cell> Cells { get; set; }
        public Cell StartCell { get; set; }
        public bool Visible { get; set; }
        public Difficulty Difficulty { get; set; }
        public bool Selected { get; internal set; }
        public int Size { get { return Cells.Count; } }

        public Area(string name, List<Cell> cells)
        {
            Name = name;
            Cells = cells;
            Difficulty = Difficulty.Easy;
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
       
    }
}