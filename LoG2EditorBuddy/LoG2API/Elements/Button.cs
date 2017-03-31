using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype.LoG2API.Elements
{
    public class Button : MapElement
    {
        public enum ButtonType
        {
            beach_rock_wall_button_01,
            beach_wall_button,
            castle_wall_button,
            dungeon_secret_button_large,
            dungeon_secret_button_small,
            forest_ruins_secret_button_large,
            forest_ruins_secret_button_small,
            mine_support_secret_button,
            mine_support_wall_button,
            tomb_secret_button_small,
            wall_button
        }

        public ButtonType type;

        public bool DisableSelf { get; set; }

        public Button(string type, int x, int y, int orientation, int h, string uniqueId) : base(x, y, orientation, h, uniqueId)
        {
            ButtonType.TryParse(type, true, out this.type);
            DisableSelf = false;
        }

        public override string ElementType
        {
            get
            {
                return this.type.ToString();
            }
        }

        public override void Draw(Graphics panel, int cellWidth, int cellHeight)
        {
            throw new NotImplementedException();
        }

        public override string PrintElement()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(String.Format(@"spawn(""{0}"",{1},{2},{3},{4},""{5}"")", type, x, y, (int)orientation, h, uniqueID));
            sb.AppendLine(String.Format(@"{0}.button:setDisableSelf({1})", uniqueID, DisableSelf ? "true" : "false"));

            return sb.ToString();
        }

    }
}
