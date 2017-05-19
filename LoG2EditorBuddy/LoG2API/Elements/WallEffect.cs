using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EditorBuddyMonster.Utilities;

namespace EditorBuddyMonster.LoG2API.Elements
{
    class WallEffect : MapElement
    {
        public enum WallEffectType
        {
            dungeon_wall_broken_01,
            dungeon_wall_broken_02
        }

        public WallEffectType type;

        private static Rectangle srcRectTop = new Rectangle(80, 220, 20, 20);
        private static Rectangle srcRectRight = new Rectangle(100, 220, 20, 20);
        private static Rectangle srcRectDown = new Rectangle(120, 220, 20, 20);
        private static Rectangle srcRectLeft = new Rectangle(140, 220, 20, 20);

        public WallEffect(string type, int x, int y, int orientation, int h, string uniqueID) : base(x,y,orientation,h,uniqueID)
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
                return true;
            }
        }

        protected override float Transparency
        {
            get
            {
                return 0.30f;
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
