using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype
{
    public class EndingPoint
    {
        public string UniqueID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int H { get; set; }
        public int Orientation { get; set; }
        public string ID { get; set; }

        public EndingPoint() { }

        public EndingPoint(string id, int x, int y, int o, int h, string uniqueID)
        {
            X = x;
            Y = y;
            H = h;
            Orientation = o;
            ID = id;
            UniqueID = uniqueID;
        }
        public string PrintElement()
        {
            return String.Format(@"spawn(""{0}"",{1},{2},{3},{4},""{5}"")", ID, X, Y, Orientation, H, UniqueID);
        }
    }
}
