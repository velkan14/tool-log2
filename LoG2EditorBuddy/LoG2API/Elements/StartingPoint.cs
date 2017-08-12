using Povoater.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Povoater.LoG2API.Elements
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

        private static Rectangle srcRectTop = new Rectangle(0, 80, 20, 20);
        private static Rectangle srcRectRight = new Rectangle(20, 80, 20, 20);
        private static Rectangle srcRectDown = new Rectangle(40, 80, 20, 20);
        private static Rectangle srcRectLeft = new Rectangle(60, 80, 20, 20);

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
