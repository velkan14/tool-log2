using Log2CyclePrototype.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Log2CyclePrototype.LoG2API.Elements
{
    public class EndingPoint : MapElement
    {

        public string Type { get; set; }

        public EndingPoint(string id, int x, int y, int o, int h, string uniqueID) : base(x,y,o,h,uniqueID)
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

        private static Rectangle srcRectTop = new Rectangle(80, 100, 20, 20);
        private static Rectangle srcRectRight = new Rectangle(100, 100, 20, 20);
        private static Rectangle srcRectDown = new Rectangle(120, 100, 20, 20);
        private static Rectangle srcRectLeft = new Rectangle(140, 100, 20, 20);

        private static Rectangle srcRectTop2 = new Rectangle(0, 120, 20, 20);
        private static Rectangle srcRectRight2 = new Rectangle(20, 120, 20, 20);
        private static Rectangle srcRectDown2 = new Rectangle(40, 120, 20, 20);
        private static Rectangle srcRectLeft2 = new Rectangle(60, 120, 20, 20);

        private static Rectangle srcRectTop3 = new Rectangle(80, 140, 20, 20);
        private static Rectangle srcRectRight3 = new Rectangle(100, 140, 20, 20);
        private static Rectangle srcRectDown3 = new Rectangle(120, 140, 20, 20);
        private static Rectangle srcRectLeft3 = new Rectangle(140, 140, 20, 20);

        protected override Rectangle RectTop
        {
            get
            {
                if (Type.Equals("dungeon_stairs_down"))
                {
                    return srcRectTop2;
                } else if (Type.Equals("healing_crystal"))
                {
                    return srcRectTop3;
                }
                return srcRectTop;
            }
        }

        protected override Rectangle RectRight
        {
            get
            {
                if (Type.Equals("dungeon_stairs_down"))
                {
                    return srcRectRight2;
                }
                else if (Type.Equals("healing_crystal"))
                {
                    return srcRectRight3;
                }
                return srcRectRight;
            }
        }

        protected override Rectangle RectDown
        {
            get
            {
                if (Type.Equals("dungeon_stairs_down"))
                {
                    return srcRectDown2;
                }
                else if (Type.Equals("healing_crystal"))
                {
                    return srcRectDown3;
                }
                return srcRectDown;
            }
        }

        protected override Rectangle RectLeft
        {
            get
            {
                if (Type.Equals("dungeon_stairs_down"))
                {
                    return srcRectLeft2;
                }
                else if (Type.Equals("healing_crystal"))
                {
                    return srcRectLeft3;
                }
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
            return String.Format(@"spawn(""{0}"",{1},{2},{3},{4},""{5}""){6}", Type, x, y, (int)orientation, h, uniqueID, '\n');
        }

        public override void setAttribute(string name, string value)
        {
            //Do Nothing ??
        }

        public override void setAttribute(string name, bool value)
        {
            //Do Nothing ??
        }
        
    }
}
