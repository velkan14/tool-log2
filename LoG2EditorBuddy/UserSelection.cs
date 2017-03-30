using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype
{
    public class UserSelection
    {

        private List<Point> userSelectedPoints;

        public UserSelection()
        {
            userSelectedPoints = new List<Point>();
        }

        public List<Point> getSelectedPoints()
        {
            return userSelectedPoints;
        }

        public void addSelectedPoint(Point p)
        {
            if (!userSelectedPoints.Contains(p))
                userSelectedPoints.Add(p);
            else Debug.WriteLine("Point already in list, skipping");
        }

        public void removeSelectedPoint(Point p)
        {
            if (userSelectedPoints.Contains(p))
                userSelectedPoints.Remove(p);
        }

        public void clearSelection()
        {
            userSelectedPoints = new List<Point>();
        }

        public bool invertSelection()
        {
            if (userSelectedPoints.Count > 0)
            {
                var invertion = new List<Point>();
                for (int y = 0; y < APIClass.CurrentMap.Height; y++) //FIXME: Remove the APIClass from here!!
                {
                    for (int x = 0; x < APIClass.CurrentMap.Width; x++)
                    {
                        if (userSelectedPoints.FindIndex(c => (c.X == x && c.Y == y)) == -1)
                            invertion.Add(new Point(x, y));
                    }
                }
                userSelectedPoints = invertion;
                return true;
            }
            return false;
           
        }
    }
}
