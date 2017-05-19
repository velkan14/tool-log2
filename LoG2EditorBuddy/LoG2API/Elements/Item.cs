using EditorBuddyMonster.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace EditorBuddyMonster.LoG2API.Elements
{
    public class Item : MapElement
    {
        public string type;
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

        private static Rectangle srcRectTop = new Rectangle(0, 60, 20, 20);
        private static Rectangle srcRectRight = new Rectangle(20, 60, 20, 20);
        private static Rectangle srcRectDown = new Rectangle(40, 60, 20, 20);
        private static Rectangle srcRectLeft = new Rectangle(60, 60, 20, 20);

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
                if (ElementType.Equals("cudgel") ||
                   ElementType.Equals("machete") ||
                   ElementType.Equals("rapier") ||
                   ElementType.Equals("battle_axe") ||
                   ElementType.Equals("potion_healing") ||
                   ElementType.Equals("borra") ||
                   ElementType.Equals("bread") ||
                   ElementType.Equals("peasant_cap") ||
                   ElementType.Equals("peasant_breeches") ||
                   ElementType.Equals("peasant_tunic") ||
                   ElementType.Equals("sandals") ||
                   ElementType.Equals("leather_cap") ||
                   ElementType.Equals("leather_brigandine") ||
                   ElementType.Equals("leather_pants") ||
                   ElementType.Equals("leather_boots"))
                {
                    return 1.0f;
                }
                return 0.3f;
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
