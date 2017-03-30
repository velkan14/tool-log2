using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype
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

        public override void Draw(Graphics panel, int cellWidth, int cellHeight)
        {
            throw new NotImplementedException();
        }

        public override string PrintElement()
        {
            return String.Format(@"spawn(""{0}"",{1},{2},{3},{4},""{5}"")", Type, x, y, orientation, h, uniqueID);
        }

    }
}
