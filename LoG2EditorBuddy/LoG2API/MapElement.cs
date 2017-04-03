using Log2CyclePrototype.Utilities;
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

        public string Print(ListQueue<MapElement> elements)
        {
            
            StringBuilder sb = new StringBuilder();

            sb.Append(PrintElement(elements));

            foreach(Connector c in connectors)
            {
                sb.Append(String.Format(@"{0}.{1}:addConnector(""{2}"", ""{3}"", ""{4}""){5}", uniqueID, ConnectorName, c.Target, c.Trigger, c.Action, '\n'));
            }
         
            return sb.ToString();
        }

        protected abstract string ConnectorName { get; }

        protected abstract string PrintElement(ListQueue<MapElement> elements);

        public abstract void Draw(Graphics panel, int cellWidth, int cellHeight);

        public abstract void setAttribute(string name, string value);

        public abstract void setAttribute(string name, bool value);
    }
}
