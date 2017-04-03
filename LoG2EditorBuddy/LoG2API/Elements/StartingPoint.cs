using Log2CyclePrototype.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Log2CyclePrototype.LoG2API.Elements
{
    public class StartingPoint : MapElement
    {
        public string Type {
            set; get;
        }
        public StartingPoint(string id, int x, int y, int o, int h, string uniqueID) : base(x, y, o, h, uniqueID)
        {
            Type = id;
        }

        public override string ElementType
        {
            get
            {
                return Type;
            }
        }

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
            return String.Format(@"spawn(""{0}"",{1},{2},{3},{4},""{5}""){6}", Type, x, y, (int)orientation, h, uniqueID, '\n');
        }

        public override void setAttribute(string name, string value)
        {
            //Do Nothing
        }

        public override void setAttribute(string name, bool value)
        {
            //Do Nothing
        }
    }
}
