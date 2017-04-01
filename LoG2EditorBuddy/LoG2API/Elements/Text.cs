using System;
using System.Drawing;
using System.Text;

namespace Log2CyclePrototype.LoG2API.Elements
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
        public string TextWritten
        {
            get; set; }

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

        public override void Draw(Graphics panel, int cellWidth, int cellHeight)
        {
            throw new NotImplementedException();
        }

        public override string PrintElement()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(String.Format(@"spawn(""{0}"",{1},{2},{3},{4},""{5}"")", textType, x, y, (int)orientation, h, uniqueID));
            sb.AppendLine(String.Format(@"{0}.walltext:setWallText(""{1}"")", uniqueID, TextWritten));
            
            return sb.ToString();
        }
    }
}
