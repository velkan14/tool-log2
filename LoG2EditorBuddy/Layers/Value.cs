using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype.Layers
{
    class Value
    {
        public int x { get; set; }
        public int y { get; set; }
        public int value { get; set; }

        public Value(int x, int y)
        {
            this.x = x;
            this.y = y;
            value = 0;
        }

        public Value(int x, int y, int value)
        {
            this.x = x;
            this.y = y;
            this.value = value;
        }
    }
}
