using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype
{
    public abstract class DrawAbstract
    {
        
        public abstract Image Draw(int width, int height);
        public abstract Image Draw(int width, int height, Image backgroundImage);
    }
}
