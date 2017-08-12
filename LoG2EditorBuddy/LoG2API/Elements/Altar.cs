using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Povoater.Utilities;

namespace Povoater.LoG2API.Elements
{
    class Altar : MapElement
    {
        public enum AltarType
        {
            altar,
            catacomb_altar_01,
            catacomb_altar_02,
            catacomb_altar_candle_01,
            forest_altar
        }

        public AltarType type;

        private static Rectangle srcRectTop = new Rectangle(80, 120, 20, 20);
        private static Rectangle srcRectRight = new Rectangle(100, 120, 20, 20);

        public Altar(string type, int x, int y, int orientation, int h, string uniqueID) : base(x,y,orientation,h,uniqueID)
        {
            Enum.TryParse(type, true, out this.type);
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
                return srcRectTop;
            }
        }

        protected override Rectangle RectLeft
        {
            get
            {
                return srcRectRight;
            }
        }

        protected override Rectangle RectRight
        {
            get
            {
                return srcRectRight;
            }
        }

        protected override Rectangle RectTop
        {
            get
            {
                return srcRectTop;
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
                return 1.0f;
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
