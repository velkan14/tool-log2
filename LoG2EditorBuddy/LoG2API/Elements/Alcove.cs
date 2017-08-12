using Povoater.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Povoater.LoG2API.Elements
{

    class Alcove : MapElement
    {
        public enum AlcoveType
        {
            castle_alcove,
            dungeon_alcove,
            mine_alcove,
            tomb_alcove
        }

        public AlcoveType type;

        public List<string> itemsId;

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

        private static Rectangle srcRectTop = new Rectangle(0, 20, 20, 20);
        private static Rectangle srcRectRight = new Rectangle(20, 20, 20, 20);
        private static Rectangle srcRectDown = new Rectangle(40, 20, 20, 20);
        private static Rectangle srcRectLeft = new Rectangle(60, 20, 20, 20);

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
                return 0.3f;
            }
        }

        public Alcove(string type, int x, int y, int orientation, int h, string uniqueID) : base(x,y,orientation,h,uniqueID)
        {
            Enum.TryParse(type, true, out this.type);
            itemsId = new List<string>();
        }

        protected override string PrintElement(ListQueue<MapElement> elements)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"spawn(""{0}"",{1},{2},{3},{4},""{5}""){6}", type, x, y, (int)orientation, h, uniqueID, '\n');
            foreach (string item in itemsId)
            {
                int index = elements.FindIndex(x => x.uniqueID.Equals(item));
                MapElement element = elements[index];
                elements.RemoveAt(index);

                sb.Append(element.Print(elements));
                sb.AppendFormat(@"{0}.surface:addItem({1}.item){2}", uniqueID, item, '\n');//FIXME: tem de estar feito o spawn do item.
            }
            return sb.ToString();
        }

        public override void setAttribute(string name, string value)
        {
            if (name.Contains("addItem"))
            {
                itemsId.Add(value);
            }
        }

        public override void setAttribute(string name, bool value)
        {
            //Do Nothing
        }
        
    }
}
