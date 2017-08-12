using Povoater.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Povoater.LoG2API.Elements
{
    class Food : MapElement
    {
        public enum FoodType
        {
            borra,
            bread
        };

        public FoodType type;

        public Food(string type, int x, int y, int orientation, int h, string uniqueID) : base(x, y, orientation, h, uniqueID)
        {
            Enum.TryParse(type, true, out this.type);
        }

        public override string ElementType { get { return type.ToString(); } }

        protected override string ConnectorName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private static Rectangle srcRectTop = new Rectangle(0, 37 * 20, 20, 20);
        private static Rectangle srcRectRight = new Rectangle(20, 37 * 20, 20, 20);
        private static Rectangle srcRectDown = new Rectangle(40, 37 * 20, 20, 20);
        private static Rectangle srcRectLeft = new Rectangle(60, 37 * 20, 20, 20);

        protected override Rectangle RectTop
        {
            get
            {
                return srcRectTop;
            }
        }

        protected override Rectangle RectRight
        {
            get
            {
                return srcRectRight;
            }
        }

        protected override Rectangle RectDown
        {
            get
            {
                return srcRectDown;
            }
        }

        protected override Rectangle RectLeft
        {
            get
            {
                return srcRectLeft;
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
