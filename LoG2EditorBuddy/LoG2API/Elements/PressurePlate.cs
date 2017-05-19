using EditorBuddyMonster.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace EditorBuddyMonster.LoG2API.Elements
{
    public class PressurePlate : MapElement
    {
        public enum PressurePlateType
        {
            beach_grass_pressure_plate,
            beach_pressure_plate,
            castle_pressure_plate,
            dungeon_pressure_plate,
            dungeon_pressure_plate_dirt_floor,
            forest_pressure_plate,
            forest_underwater_pressure_plate,
            mine_pressure_plate,
            tomb_pressure_plate

        }

        public PressurePlateType type;
        public bool TriggeredByParty { get; set; }
        public bool TriggeredByMonster { get; set; }
        public bool TriggeredByItem { get; set; }
        public bool TriggeredByDigging { get; set; }
        public bool DisableSelf { get; set; }

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
                return "floortrigger";
            }
        }

        public PressurePlate(string type, int x, int y, int orientation, int h, string uniqueID) : base(x,y,orientation,h,uniqueID)
        {
            Enum.TryParse(type, true, out this.type);
            TriggeredByDigging = false;
            TriggeredByItem = true;
            TriggeredByMonster = true;
            TriggeredByParty = true;
        }

        private static Rectangle srcRectTop = new Rectangle(80, 340, 20, 20);
        private static Rectangle srcRectRight = new Rectangle(100, 340, 20, 20);
        private static Rectangle srcRectDown = new Rectangle(120, 340, 20, 20);
        private static Rectangle srcRectLeft = new Rectangle(140, 340, 20, 20);

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
                return 0.3f;
            }
        }

        protected override string PrintElement(ListQueue<MapElement> elements)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"spawn(""{0}"",{1},{2},{3},{4},""{5}""){6}", type, x, y, (int)orientation, h, uniqueID, '\n');
            sb.AppendFormat(@"{0}.floortrigger:setTriggeredByParty({1}){2}", uniqueID, TriggeredByParty ? "true" : "false", '\n');
            sb.AppendFormat(@"{0}.floortrigger:setTriggeredByMonster({1}){2}", uniqueID, TriggeredByMonster ? "true" : "false", '\n');
            sb.AppendFormat(@"{0}.floortrigger:setTriggeredByItem({1}){2}", uniqueID, TriggeredByItem ? "true" : "false", '\n');
            sb.AppendFormat(@"{0}.floortrigger:setTriggeredByDigging({1}){2}", uniqueID, TriggeredByDigging ? "true" : "false", '\n');
            sb.AppendFormat(@"{0}.floortrigger:setDisableSelf({1}){2}", uniqueID, DisableSelf ? "true" : "false", '\n');

            return sb.ToString();
        }

        public override void setAttribute(string name, string value)
        {
            //Do Nothing
        }

        public override void setAttribute(string name, bool value)
        {
            if (name.Contains("setTriggeredByParty"))
            {
                TriggeredByParty = value;
            }
            else if (name.Contains("setTriggeredByMonster"))
            {
                TriggeredByMonster = value;
            }
            else if(name.Contains("setTriggeredByItem"))
            {
                TriggeredByItem = value;
            }
            else if (name.Contains("setTriggeredByDigging"))
            {
                TriggeredByDigging = value;
            }
            else if (name.Contains("setDisableSelf"))
            {
                DisableSelf = value;
            }
        }
    }
}
