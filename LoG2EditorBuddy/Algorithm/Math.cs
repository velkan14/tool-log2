using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Povoater.Algorithm
{
    static class Math
    {
        public static double Function(double x, double desire, double min, double max)
        {
            double result = 0.0;

            /*double distanceMin = desire - min;
            double distanceMax = max - desire;  

            if(distanceMax < distanceMin)
            {
                if (desire == x)
                {
                    result = 1.0;
                }
                else if (x < desire)
                {
                    result = (x + max - 2 * desire) / (max - desire);
                }
                else
                {
                    result = (x - max) / (desire - max);
                }
            }
            else
            {
                if (desire == x)
                {
                    result = 1.0;
                }
                else if (x < desire)
                {
                    result = (x - min) / (desire - min);
                }
                else
                {
                    result = (x - min - 2 * desire) / (-min - desire);
                }
            }*/

            if (desire == x)
            {
                result = 1.0;
            }
            else if (x < desire)
            {
                result = (1 / (desire - min)) * x + (-min / (desire - min));
            }
            else
            {
                result = (1 / (desire - max)) * x + (-max / (desire - max));
            }

            if (Double.IsNaN(result))
            { Logger.AppendText("Error: NaN Guidline"); }

            return System.Math.Max(0.0, result);
        }
    }
}
