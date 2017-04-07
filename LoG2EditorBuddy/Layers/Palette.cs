using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype.Layers
{
    class Palette
    {
        Dictionary<int, Color> colors = new Dictionary<int, Color>();

        int currentValue = 0;

        public Palette(Color c1, Color c2, Color c3, Color c4, Color c5)
        {
            int i = 0;
            colors.Add(i++, c1);
            colors.Add(i++, c2);
            colors.Add(i++, c3);
            colors.Add(i++, c4);
            colors.Add(i++, c5);
        }
    }
}
