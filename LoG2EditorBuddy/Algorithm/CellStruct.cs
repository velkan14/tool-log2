using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Povoater.Algorithm
{
    class CellStruct
    {
        public int type;
        public int x;
        public int y;
        public bool visited;

        public CellStruct(int type, int x, int y)
        {
            this.type = type;
            this.x = x;
            this.y = y;
            this.visited = false;
        }
    }
}
