using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype.LoG2API.Elements
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

        public Alcove(string type, int x, int y, int orientation, int h, string uniqueID) : base(x,y,orientation,h,uniqueID)
        {
            Enum.TryParse(type, true, out this.type);
            itemsId = new List<string>();
        }

        public override void Draw(Graphics panel, int cellWidth, int cellHeight)
        {
            throw new NotImplementedException();
        }

        public override string PrintElement()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(String.Format(@"spawn(""{0}"",{1},{2},{3},{4},""{5}"")", type, x, y, (int)orientation, h, uniqueID));
            foreach(string item in itemsId)
            {
                sb.AppendLine(String.Format(@"{0}.surface:addItem({1}.item)", type, item));
            }
            return sb.ToString();
        }
    }
}
