using Log2CyclePrototype.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Log2CyclePrototype.LoG2API.Elements
{
    public class Item : MapElement
    {
        string type;
        public Item(string type, int x, int y, int orientation, int h, string uniqueID) : base(x,y,orientation,h,uniqueID)
        {
            this.type = type;
        }

        public override string ElementType { get { return type; }}

        protected override string ConnectorName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override void Draw(Graphics panel, int cellWidth, int cellHeight)
        {
            throw new NotImplementedException();
        }

        protected override string PrintElement(ListQueue<MapElement> elements)
        {
            return String.Format(@"spawn(""{0}"",{1},{2},{3},{4},""{5}""){6}", ElementType, x, y, (int)orientation, h, uniqueID, '\n');
        }

        public override void setAttribute(string name, bool value)
        {
            throw new NotImplementedException();
        }

        public override void setAttribute(string name, string value)
        {
            throw new NotImplementedException();
        }
        
    }
}
