using System;
using System.Drawing;
using System.Text;

namespace Log2CyclePrototype.LoG2API.Elements
{
    public class Lock : MapElement
    {
        public enum LockType
        {
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

        public Lock(string type, int x, int y, int orientation, int h, string uniqueID) : base(x,y,orientation,h,uniqueID)
        {
            LockType.TryParse(type, true, out this.type);
        }

        public override void Draw(Graphics panel, int cellWidth, int cellHeight)
        {
            throw new NotImplementedException();
        }

        public override string PrintElement()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(String.Format(@"spawn(""{0}"",{1},{2},{3},{4},""{5}"")", type, x, y, (int)orientation, h, uniqueID));
            sb.AppendLine(String.Format(@"{0}.lock:setOpenedBy({1})", uniqueID, OpenedBy));

            return sb.ToString();
        }
    }
}
