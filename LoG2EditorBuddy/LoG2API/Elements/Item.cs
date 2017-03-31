using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype.LoG2API.Elements
{
    public class Item : MapElement
    {
        string type;
        public Item(string type, int x, int y, int orientation, int h, string uniqueID) : base(x,y,orientation,h,uniqueID)
        {
            this.type = type;
        }

        public override string ElementType
        {
            get
            {
                return type;
            }
        }

        public override void Draw(Graphics panel, int cellWidth, int cellHeight)
        {
            throw new NotImplementedException();
        }

        public override string PrintElement()
        {
            throw new NotImplementedException();
        }
    }
}
