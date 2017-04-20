using Log2CyclePrototype.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Log2CyclePrototype.LoG2API.Elements
{
    public class Lock : MapElement
    {
        public enum LockType
        {
            LOCK, //Hack
            beach_lock_gold,
            beach_lock_ornate,
            beach_lock_round,
            lock_gear,
            lock_gold,
            lock_gold_mask_statue,
            lock_ornate,
            lock_prison,
            lock_round,
            lock_tomb,
            mine_lock,
            nexus_lock,
            portal_lock,
            skull_lock
        }

        public LockType type;
        public string OpenedBy { get; set; }

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
                return "lock";
            }
        }

        public Lock(string type, int x, int y, int orientation, int h, string uniqueID) : base(x,y,orientation,h,uniqueID)
        {
            LockType.TryParse(type, true, out this.type);
        }

        private static Rectangle srcRectTop = new Rectangle(80, 40, 20, 20);
        private static Rectangle srcRectRight = new Rectangle(100, 40, 20, 20);
        private static Rectangle srcRectDown = new Rectangle(120, 40, 20, 20);
        private static Rectangle srcRectLeft = new Rectangle(140, 40, 20, 20);

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

        protected override string PrintElement(ListQueue<MapElement> elements)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"spawn(""{0}"",{1},{2},{3},{4},""{5}""){6}", type.ToString().ToLower(), x, y, (int)orientation, h, uniqueID, '\n');
            sb.AppendFormat(@"{0}.lock:setOpenedBy(""{1}""){2}", uniqueID, OpenedBy, '\n');

            return sb.ToString();
        }

        public override void setAttribute(string name, string value)
        {
            if (name.Contains("setOpenedBy"))
            {
                OpenedBy = value;
            }
        }

        public override void setAttribute(string name, bool value)
        {
            throw new NotImplementedException();
        }
    }
}
