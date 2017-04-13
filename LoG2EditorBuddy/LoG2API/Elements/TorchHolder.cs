using Log2CyclePrototype.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Log2CyclePrototype.LoG2API.Elements
{
    public class TorchHolder : MapElement
    {
        public enum TorchHolderType
        {
            castle_torch_holder,
            tomb_torch_holder,
            torch_holder
        }

        public TorchHolderType type;

        public bool HasTorch { get; set; }

        public TorchHolder(string type, int x, int y, int orientation, int h, string uniqueId) : base(x, y, orientation, h, uniqueId)
        {
            TorchHolderType.TryParse(type, true, out this.type);
            HasTorch = true;
        }

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
                return "socket";
            }
        }

        private static Rectangle srcRectTop = new Rectangle(80, 200, 20, 20);
        private static Rectangle srcRectRight = new Rectangle(100, 200, 20, 20);
        private static Rectangle srcRectDown = new Rectangle(120, 200, 20, 20);
        private static Rectangle srcRectLeft = new Rectangle(140, 200, 20, 20);

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
