using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Log2CyclePrototype.LoG2API
{
    [Serializable]
    abstract public class MapElement
    {
        private MapElement _connectedTo;

        public List<Connector> connectors;

        public enum Orientation
        {
            Top,
            Right,
            Down,
            Left
        }

        [JsonProperty()] public int x { get; set; }
        [JsonProperty()] public int y { get; set; }
        [JsonProperty()] public int h { get; set; }
        [JsonProperty()] public string uniqueID { get; set; }
        [JsonProperty()] public Orientation orientation { get; set; }

        public abstract string ElementType { get; }

        public MapElement(int x, int y,int orientation, int h, string uniqueID)
        {
            this.x = x;
            this.y = y;
            this.h = h;
            this.orientation = (Orientation) orientation;
            this.uniqueID = uniqueID;

            connectors = new List<Connector>();
        }

        public void addConnector(string trigger, string target, string action)
        {
            connectors.Add(new Connector(trigger, target, action));
        }

        public string Print()
        {
            
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(PrintElement());

            if (connectors.Count > 0)
            {
                foreach(Connector c in connectors)
                    sb.AppendLine(String.Format(@"{0}.socket:addConnector(""{1}"", ""{2}"", ""{3}"")", uniqueID, c.Target, c.Trigger, c.Action));
            }
            return sb.ToString();
        }

        public abstract string PrintElement();

        public abstract void Draw(Graphics panel, int cellWidth, int cellHeight);

    }
}
