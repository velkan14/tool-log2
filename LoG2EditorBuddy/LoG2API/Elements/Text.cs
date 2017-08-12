using Povoater.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Povoater.LoG2API.Elements
{
    public class Text : MapElement
    {
        public enum TextType
        {
            beach_wall_text,
            castle_wall_text,
            castle_wall_text_long,
            dungeon_wall_text,
            dungeon_wall_text_long,
            forest_wall_text_long,
            forest_wall_text_short,
            mine_wall_text,
            mine_wall_text_long,
            tomb_wall_text,
            tomb_wall_text_long,

        }

        public TextType textType;
        public string TextWritten{ get; set; }

        public Text(string type, int x, int y, int orientation, int h, string uniqueID) : base(x,y,orientation,h,uniqueID)
        {
            Enum.TryParse(type, true, out textType);
        }

        public override string ElementType
        {
            get
            {
                return textType.ToString();
            }
        }

        protected override string ConnectorName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private static Rectangle srcRectTop = new Rectangle(80, 60, 20, 20);
        private static Rectangle srcRectRight = new Rectangle(100, 60, 20, 20);
        private static Rectangle srcRectDown = new Rectangle(120, 60, 20, 20);
        private static Rectangle srcRectLeft = new Rectangle(140, 60, 20, 20);

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
                return true;
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

            sb.AppendFormat(@"spawn(""{0}"",{1},{2},{3},{4},""{5}""){6}", textType, x, y, (int)orientation, h, uniqueID, '\n');
            sb.AppendFormat(@"{0}.walltext:setWallText(""{1}""){2}", uniqueID, TextWritten, '\n');

            return sb.ToString();
        }

        public override void setAttribute(string name, string value)
        {
            if (name.Contains("setWallText"))
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
