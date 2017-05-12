using Log2CyclePrototype.LoG2API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype.Layers
{
    
    class Area
    {
        public string Name { get; set; }
        public List<Cell> Cells { get; set; }
        public bool Visible { get; set; }
        public Difficulty Difficulty { get; set; }
        public bool Selected { get; internal set; }

        public Area(string name, List<Cell> cells)
        {
            Name = name;
            Cells = cells;
            Difficulty = Difficulty.Easy;
            Visible = true;
        }
        
    }
}
