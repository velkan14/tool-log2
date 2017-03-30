using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype.LoG2API.Elements
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

        public PressurePlate(string type, int x, int y, int orientation, int h, string uniqueID) : base(x,y,orientation,h,uniqueID)
        {
            Enum.TryParse(type, true, out this.type);
            TriggeredByDigging = false;
            TriggeredByItem = true;
            TriggeredByMonster = true;
            TriggeredByParty = true;
        }

        public override void Draw(Graphics panel, int cellWidth, int cellHeight)
        {
            throw new NotImplementedException();
        }

        public override string PrintElement()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(String.Format(@"spawn(""{0}"",{1},{2},{3},{4},""{5}"")", type, x, y, (int)orientation, h, uniqueID));
            sb.AppendLine(String.Format(@"{0}.floortrigger:setTriggeredByParty({1})", uniqueID, TriggeredByParty ? "true" : "false"));
            sb.AppendLine(String.Format(@"{0}.floortrigger:setTriggeredByMonster({1})", uniqueID, TriggeredByMonster ? "true" : "false"));
            sb.AppendLine(String.Format(@"{0}.floortrigger:setTriggeredByItem({1})", uniqueID, TriggeredByItem ? "true" : "false"));
            sb.AppendLine(String.Format(@"{0}.floortrigger:setTriggeredByDigging({1})", uniqueID, TriggeredByDigging ? "true" : "false"));
            sb.AppendLine(String.Format(@"{0}.floortrigger:setDisableSelf({1})", uniqueID, DisableSelf ? "true" : "false"));

            if (connectors.Count > 0)
            {
                sb.AppendLine(PrintConnectors());
            }

            return sb.ToString();
        }
    }
}
