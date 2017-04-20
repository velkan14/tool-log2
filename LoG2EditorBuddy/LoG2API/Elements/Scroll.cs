using Log2CyclePrototype.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype.LoG2API.Elements
{
    public class Scroll : MapElement
    {
        public string TextWritten { get; set; }

        public Scroll(string type, int x, int y, int orientation, int h, string uniqueID) : base(x, y, orientation, h, uniqueID)
        {
        }

        public override string ElementType
        {
            get
            {
                return "scroll";
            }
        }

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
                return 1.0f;
            }
        }

        protected override string PrintElement(ListQueue<MapElement> elements)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"spawn(""{0}"",{1},{2},{3},{4},""{5}""){6}", ElementType, x, y, (int)orientation, h, uniqueID, '\n');
            sb.AppendFormat(@"{0}.scrollitem:setScrollText(""{1}""){2}", uniqueID, TextWritten, '\n');

            return sb.ToString();
        }

        public override void setAttribute(string name, string value)
        {
            if (name.Contains("setScrollText"))
            {
                TextWritten = value;
            }
        }

        public override void setAttribute(string name, bool value)
        {
            //Do Nothing
        }
    }
}
