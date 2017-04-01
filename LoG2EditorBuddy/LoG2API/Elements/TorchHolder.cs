using System;
using System.Drawing;
using System.Text;

namespace Log2CyclePrototype.LoG2API.Elements
{
    public class TorchHolder : MapElement
    {
        public bool HasTorch { get; set; }

        public TorchHolder(string type, int x, int y, int orientation, int h, string uniqueId) : base(x, y, orientation, h, uniqueId)
        {
            HasTorch = true;
        }

        public override string ElementType
        {
            get
            {
                return "torch_holder";
            }
        }

        public override void Draw(Graphics panel, int cellWidth, int cellHeight)
        {
            throw new NotImplementedException();
        }

        public override string PrintElement()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(String.Format(@"spawn(""{0}"",{1},{2},{3},{4},""{5}"")", "torch_holder", x, y, (int)orientation, h, uniqueID));

            sb.AppendLine(String.Format(@"{0}.controller:setHasTorch({1})", uniqueID, HasTorch ? "true" : "false"));
            return sb.ToString();
        }

    }
}
