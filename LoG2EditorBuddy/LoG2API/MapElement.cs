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
        protected static Image imageIcons = Image.FromFile("../../img/icons.png");

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

        protected abstract Rectangle RectTop { get; }
        protected abstract Rectangle RectRight { get; }
        protected abstract Rectangle RectDown { get; }
        protected abstract Rectangle RectLeft { get; }

        protected abstract bool UseOffset { get; }

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
                sb.Append(String.Format(@"{0}.{1}:addConnector(""{2}"", ""{3}"", ""{4}""){5}", uniqueID, ConnectorName, c.Trigger, c.Target, c.Action, '\n'));
            }
         
            return sb.ToString();
        }

        protected abstract string ConnectorName { get; }

        protected abstract string PrintElement(ListQueue<MapElement> elements);

        public void Draw(Graphics panel, int cellWidth, int cellHeight)
        {
            Rectangle destRect = new Rectangle(x * cellWidth, y * cellHeight, cellWidth, cellHeight);
            
            switch (orientation)
            {
                case MapElement.Orientation.Top:
                    if (UseOffset)
                    {
                        Point offset = new Point(0, -cellHeight / 2);
                        destRect.Offset(offset);
                    }
                    
                    panel.DrawImage(imageIcons, destRect, RectTop, GraphicsUnit.Pixel);
                    break;
                case MapElement.Orientation.Right:
                    if (UseOffset)
                    {
                        Point offset = new Point(cellWidth / 2, 0);
                        destRect.Offset(offset);
                    }
                    panel.DrawImage(imageIcons, destRect, RectRight, GraphicsUnit.Pixel);
                    break;
                case MapElement.Orientation.Down:
                    if (UseOffset)
                    {
                        Point offset = new Point(0, cellHeight / 2);
                        destRect.Offset(offset);
                    }
                    panel.DrawImage(imageIcons, destRect, RectDown, GraphicsUnit.Pixel);
                    break;
                case MapElement.Orientation.Left:
                    if (UseOffset)
                    {
                        Point offset = new Point(-cellWidth / 2, 0);
                        destRect.Offset(offset);
                    }
                    panel.DrawImage(imageIcons, destRect, RectLeft, GraphicsUnit.Pixel);
                    break;
            }        
        }

        public abstract void setAttribute(string name, string value);

        public abstract void setAttribute(string name, bool value);
    }
}
