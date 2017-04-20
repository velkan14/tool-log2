using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Log2CyclePrototype.Utilities;

namespace Log2CyclePrototype.LoG2API.Elements
{
    class Lantern : MapElement
    {
        public enum LanternType
        {
            castle_ceiling_lantern,
            ceiling_witch_lantern,
            mine_ceiling_lantern
        }

        public LanternType type;
        private static Rectangle srcRect = new Rectangle(0, 220, 20, 20);

        public Lantern(string type, int x, int y, int orientation, int h, string uniqueID) : base(x,y,orientation,h,uniqueID)
        {
            LanternType.TryParse(type, true, out this.type);
        }

        public override string ElementType
        {
            get
            {
                return type.ToString();
            }
        }

        protected override string ConnectorName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected override Rectangle RectDown
        {
            get
            {
                return srcRect;
            }
        }

        protected override Rectangle RectLeft
        {
            get
            {
                return srcRect;
            }
        }

        protected override Rectangle RectRight
        {
            get
            {
                return srcRect;
            }
        }

        protected override Rectangle RectTop
        {
            get
            {
                return srcRect;
            }
        }

        protected override bool UseOffset
        {
            get
            {
                return false;
            }
        }

        protected override float Transparency
        {
            get
            {
                return 0.0f;
            }
        }

        public override void setAttribute(string name, bool value)
        {
            throw new NotImplementedException();
        }

        public override void setAttribute(string name, string value)
        {
            throw new NotImplementedException();
        }

        protected override string PrintElement(ListQueue<MapElement> elements)
        {
            return String.Format(@"spawn(""{0}"",{1},{2},{3},{4},""{5}""){6}", ElementType, x, y, (int)orientation, h, uniqueID, '\n');
        }
    }
}
