using Log2CyclePrototype.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype.LoG2API.Elements
{
    public class TrapDoor : MapElement
    {
        /// <summary>
        /// Type of door 
        /// </summary>
        public enum TrapDoorType
        {
            castle_pit_trapdoor,
            dungeon_pit_trapdoor,
            forest_pit_trapdoor,
            mine_pit_trapdoor,
            tomb_pit_trapdoor
        }

        public TrapDoorType type;

        public bool Disable { get; set; }
        public bool Open { get; set; }

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

        public TrapDoor(string type, int x, int y, int orientation, int h, string uniqueID) : base(x,y,orientation,h,uniqueID)
        {
            TrapDoorType.TryParse(type, true, out this.type);
            Disable = false;
            Open = false;
        }

        private static Rectangle srcRectTop = new Rectangle(0, 100, 20, 20);
        private static Rectangle srcRectRight = new Rectangle(20, 100, 20, 20);
        private static Rectangle srcRectDown = new Rectangle(40, 100, 20, 20);
        private static Rectangle srcRectLeft = new Rectangle(60, 100, 20, 20);

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

        protected override string PrintElement(ListQueue<MapElement> elements)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"spawn(""{0}"",{1},{2},{3},{4},""{5}""){6}", type, x, y, (int)orientation, h, uniqueID, '\n');
            sb.AppendFormat(@"{0}.pit:setState(""{1}""){2}", uniqueID, Open ? "open" : "closed", '\n');
            if (Disable) sb.AppendFormat(@"{0}.platform:disable(){1}", uniqueID, '\n');
            return sb.ToString();
        }

        public override void setAttribute(string name, bool value)
        {
            //Do Nothing
        }

        public override void setAttribute(string name, string value)
        {
            if (name.Contains("setState"))
            {
                Open = value.Contains("Open") ? true : false;
            }
            else if (name.Contains("disable"))
            {
                Disable = true;
            }
        }
    }
}
