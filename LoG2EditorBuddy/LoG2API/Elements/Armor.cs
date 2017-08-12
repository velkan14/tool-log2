using Povoater.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Povoater.LoG2API.Elements
{
    class Armor : MapElement
    {
        public enum ArmorType
        {
            leather_cap,
            leather_brigandine,
            leather_pants,
            leather_boots
        };

        public ArmorType type;

        public Armor(string type, int x, int y, int orientation, int h, string uniqueID) : base(x, y, orientation, h, uniqueID)
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

        private static Rectangle helmet = new Rectangle(0, 38 * 20, 20, 20);
        private static Rectangle brigandine = new Rectangle(20, 38 * 20, 20, 20);
        private static Rectangle pants = new Rectangle(40, 38 * 20, 20, 20);
        private static Rectangle boots = new Rectangle(60, 38 * 20, 20, 20);

        private Rectangle GetRectangle()
        {
            if (ElementType.Equals("leather_cap"))
            {
                return helmet;
            }
            else if (ElementType.Equals("leather_brigandine"))
            {
                return brigandine;
            }
            else if (ElementType.Equals("leather_pants"))
            {
                return pants;
            }
            else if (ElementType.Equals("leather_boots"))
            {
                return boots;
            }
            return helmet;
        }

        protected override Rectangle RectTop
        {
            get
            {
                return GetRectangle();
            }
        }

        protected override Rectangle RectRight
        {
            get
            {
                return GetRectangle();
            }
        }

        protected override Rectangle RectDown
        {
            get
            {
                return GetRectangle();
            }
        }

        protected override Rectangle RectLeft
        {
            get
            {
                return GetRectangle();
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
