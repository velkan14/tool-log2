using Log2CyclePrototype.Utilities;
using System;
using System.Collections.Generic;
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

        protected override string ConnectorName
        {
            get
            {
                return "socket";
            }
        }

        public override void Draw(Graphics panel, int cellWidth, int cellHeight)
        {
            throw new NotImplementedException();
        }

        protected override string PrintElement(ListQueue<MapElement> elements)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"spawn(""{0}"",{1},{2},{3},{4},""{5}""){6}", "torch_holder", x, y, (int)orientation, h, uniqueID, '\n');
            sb.AppendFormat(@"{0}.controller:setHasTorch({1}){2}", uniqueID, HasTorch ? "true" : "false", '\n');
  
            return sb.ToString();
        }

        public override void setAttribute(string name, string value)
        {
            //Do nothing
        }

        public override void setAttribute(string name, bool value)
        {
            if (name.Contains("setHasTorch"))
            {
                HasTorch = value;
            }
        }
    }
}
