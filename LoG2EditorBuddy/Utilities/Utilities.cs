using GAF;
using EditorBuddyMonster.LoG2API;
//using Log2CyclePrototype.Algorithm;
//using Log2CyclePrototype.LoG2API;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorBuddyMonster.Utilities
{
    /// <summary>
    /// Utility class to help with logging
    /// </summary>
    public static class LogUtilities
    {





    }



    public static class CloneUtilities
    {

        /// <summary>
        /// Perform a deep Copy of the object, using Json as a serialisation method.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T CloneJson<T>(this T source)
        {
            try
            {
                //JsonSerializerSettings settings = new JsonSerializerSettings();
                //settings.Error = (serializer, err) =>
                //{
                //    err.ErrorContext.Handled = true;
                //};
                // Don't serialize a null object, simply return the default for that object
                if (Object.ReferenceEquals(source, null))
                {
                    return default(T);
                }
                var settings = new JsonSerializerSettings();
                settings.TypeNameHandling = TypeNameHandling.Auto;
                //settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects; //was having trouble when serielizing neighbours
                //settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                var json = JsonConvert.SerializeObject(source, settings);
                return JsonConvert.DeserializeObject<T>(json, settings);

            }
            catch (Exception jse)
            {
                Debug.WriteLine("Base: " + jse.GetBaseException());
                Debug.WriteLine("Std: " + jse.Message);
                Debug.WriteLine("Inner: " + jse.InnerException);
            }

            return default(T);
        }

    }



    /// <summary>
    /// Utility class to help with debug writting
    /// </summary>
    public static class DebugUtilities
    {

        /// <summary>
        /// Print to the console an arraylist containing the cells of a chromosome solution
        /// </summary>
        /// <param name="toPrint"></param>
        public static void PrintSolutionArray(ArrayList toPrint)
        {
            var solString = new System.Text.StringBuilder();
            var array = toPrint.ToArray();
            for (int i = 0; i < 32; i++){
                for (int j = 0; j < 32; j++){
                    solString.Append(array[j + 32 * i].ToString());
                }
                solString.Append("\n");
            }
            Debug.WriteLine(solString);
        }

        /// <summary>
        /// Prints to the console a Cell vector representing a chromosome solution
        /// </summary>
        /// <param name="toPrint"></param>
        public static void PrintCellVector(Cell[] toPrint)
        {
            var solString = new System.Text.StringBuilder();
            var array = toPrint.ToArray();
            for (int i = 0; i < 32; i++){
                for (int j = 0; j < 32; j++){
                    solString.Append(array[j + 32 * i].CellType + " ");
                }
                solString.Append("\n");
            }
            Debug.WriteLine(solString);
        }

        /// <summary>
        /// Prints the string value of the elements of an object vector to the console
        /// </summary>
        /// <param name="toPrint"></param>
        public static void PrintObjectVector(object[] toPrint)
        {
            var solString = new System.Text.StringBuilder();
            var array = toPrint.ToArray();
            for (int i = 0; i < array.Length; i++){
                solString.Append(array[i].ToString());
                solString.Append("\n");
            }
            Debug.WriteLine(solString);
        }
    
        /// <summary>
        /// Prints the cells of a map to the console
        /// </summary>
        /// <param name="toPrint"></param>
        public static void PrintMap(Map toPrint)
        {
            var solString = new System.Text.StringBuilder();
            var array = toPrint.Cells.ToArray();

            for (int i = 0; i < 32; i++){
                for (int j = 0; j < 32; j++){
                    solString.Append(array[j + 32 * i].CellType + " ");
                }
                solString.Append("\n");
            }
            Debug.WriteLine(solString);
        }


        /// <summary>
        /// Prints the chromosome solution to the Debug console
        /// </summary>
        /// <param name="c"></param>
        public static void PrintChromosomeSolution(Chromosome c)
        {
            var cellString = new StringBuilder();
            var objectString = "";
            var genes = c.Genes.ToArray();
            Debug.WriteLine("Num Genes: " + genes.Length);
            for (int i = 0; i < 32; i++){
                for (int j = 0; j < 32; j++){
                    var tempC = (Cell)genes[j + 32 * i].ObjectValue;
                    cellString.Append(tempC.CellType);
                    /*if (tempC.ElementsInCell.Count > 0){
                        foreach (var k in tempC.ElementsInCell.Keys)
                            objectString += k + " -> " + ((MapElement)tempC.ElementsInCell[k]).uniqueID + " @ " + tempC.X + " - " + tempC.Y + "\n";
                    }*/
                }
                cellString.Append("\n");
            }
            Debug.WriteLine(cellString.ToString() + objectString);
        }




        /// <summary>
        /// Prints to the console a more detailed version of the exception at hand
        /// </summary>
        /// <param name="ex"></param>
        public static void DebugException(Exception ex)
        {
            Debug.WriteLine("Base: " + ex.GetBaseException());
            Debug.WriteLine("Std: " + ex.Message);
            Debug.WriteLine("Inner: " + ex.InnerException);
        }


        /// <summary>
        /// Used to Debug a path returned by maze solver to the console
        /// </summary>
        /// <param name="toDebug"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void Debug2DMazeArray(int[,] toDebug, int width, int height)
        {
            StringBuilder sb = new StringBuilder();
            try{
                for (int i = 0; i < width; i++){
                    for (int j = 0; j < height; j++){
                        if (toDebug[j, i] == 100)
                            sb.Append("@,");
                        else
                            sb.Append(toDebug[j, i].ToString() + ",");
                        if (j == height - 1)
                            sb.Append("\n");
                    }
                }
                Debug.WriteLine(sb.ToString());
            }catch (Exception e) { DebugUtilities.DebugException(e); }
        }

    }


    /// <summary>
    /// Used to create .txt files for testing and debugging
    /// </summary>
    public static class WriteUtilities
    {

        /// <summary>
        /// Writes a chromosome content to a file, to the specified location
        /// </summary>
        /// <param name="mutation"></param>
        /// <param name="num"></param>
        /// <param name="fileDir"></param>
        public static void WriteMutations( int num, string fileDir, Chromosome mutation, int floorNum)
        {
            var cellString = new StringBuilder();
            var objectString = "";
            var mutGenes = mutation.Genes.ToArray();

            //Debug.WriteLine("Num Genes: " + mutGenes.Length);

            cellString.Append("floor " + floorNum);
            cellString.Append("\n");

            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    var tempC = (Cell)mutGenes[j + 32 * i].ObjectValue;
                    cellString.Append(tempC.CellType);
                    cellString.Append(",");
                    //if (tempC.ElementsInCell.Count > 0)
                    //{
                    //    foreach (var k in tempC.ElementsInCell.Keys)
                    //        objectString += k + " -> " + ((MapElement)tempC.ElementsInCell[k]).uniqueID + " @ " + tempC.X + " - " + tempC.Y + "\n";
                    //}
                }
                cellString.Append("\n");
            }

            File.WriteAllText(fileDir + @"\mut" + num + ".txt", cellString.ToString());

            //Debug.WriteLine(cellString.ToString() + objectString);
        }




        public static void WriteSolution(int generation, string fileDir, Chromosome solution, int floorTile)
        {
            var cellString = new StringBuilder();
            var objectString = "";
            var mutGenes = solution.Genes.ToArray();

            //Debug.WriteLine("Num Genes: " + mutGenes.Length);

            cellString.Append("floor "+floorTile);
            cellString.Append("\n");

            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    var tempC = (Cell)mutGenes[j + 32 * i].ObjectValue;
                    cellString.Append(tempC.CellType);
                    cellString.Append(",");
                    //if (tempC.ElementsInCell.Count > 0)
                    //{
                    //    foreach (var k in tempC.ElementsInCell.Keys)
                    //        objectString += k + " -> " + ((MapElement)tempC.ElementsInCell[k]).uniqueID + " @ " + tempC.X + " - " + tempC.Y + "\n";
                    //}
                }
                cellString.Append("\n");
            }

            File.WriteAllText(fileDir + @"\sol" + generation + ".txt", cellString.ToString());

            //Debug.WriteLine(cellString.ToString() + objectString);
        }
        
    }



    class Vector
    {
        public float i { get; internal set; }
        public float j { get; internal set; }
        public float k { get; internal set; }
        public float Magnitude { get { return (float)System.Math.Sqrt(System.Math.Pow(i, 2) + System.Math.Pow(j, 2) + System.Math.Pow(k, 2)); } }

        public Vector(float v1, float v2, float v3) { i = v1; j = v2; k = v3; }

        public static Vector Add(Vector u, Vector v)
        {
            return new Vector(u.i + v.i, u.j + v.j, u.k + v.k);
        }

        public static Vector Sub(Vector u, Vector v)
        {
            return new Vector(u.i - v.i, u.j - v.j, u.k - v.k);
        }

        public static Vector Mult(Vector u, Vector v)
        {
            return new Vector(u.i * v.i, u.j * v.j, u.k * v.k);
        }

        public static Vector Div(Vector u, Vector v)
        {
            return new Vector(u.i / v.i, u.j / v.j, u.k / v.k);
        }

        public static Vector CrossProduct(Vector u, Vector v)
        {
            return new Vector(u.j * v.k - u.k * v.j, u.k * v.i - u.i * v.k, u.i * v.j - u.j * v.i);
        }

        public static float DotProduct(Vector u, Vector v)
        {
            return (u.i * v.i + u.j * v.j + u.k * v.k);
        }
    }


    public static class MathUtilities
    {
        static bool SameSide(Point p1, Point p2, Vector a, Vector b)
        {
            Vector p1v = new Vector(p1.X, p1.Y, 0);
            Vector p2v = new Vector(p2.X, p2.Y, 0);
            Vector cp1 = Vector.CrossProduct(Vector.Sub(b, a), Vector.Sub(p1v, a));
            Vector cp2 = Vector.CrossProduct(Vector.Sub(b, a), Vector.Sub(p2v, a));
            if (Vector.DotProduct(cp1, cp2) >= 0)
                return true;
            else return false;
        }

        public static bool PointInTriangle(Point p, Point a, Point b, Point c)
        {
            Vector va = new Vector(a.X, a.Y, 0);
            Vector vb = new Vector(b.X, b.Y, 0);
            Vector vc = new Vector(c.X, c.Y, 0);

            if (SameSide(p, a, vb, vc) && SameSide(p, b, va, vc) && SameSide(p, c, va, vb))
                return true;
            else return false;
        }

        public static float DistanceBetweenPoints(PointF p1, PointF p2)
        {
            return (float)System.Math.Sqrt(System.Math.Pow((p1.X - p2.X), 2) + System.Math.Pow((p1.Y - p2.Y), 2));
        }

        // Calculate the distance between
        // point pt and the segment p1 --> p2.
        public static double FindDistanceToSegment(
            PointF pt, PointF p1, PointF p2, out PointF closest)
        {
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;
            if ((dx == 0) && (dy == 0))
            {
                // It's a point not a line segment.
                closest = p1;
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
                return System.Math.Sqrt(dx * dx + dy * dy);
            }

            // Calculate the t that minimizes the distance.
            float t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) /
                (dx * dx + dy * dy);

            // See if this represents one of the segment's
            // end points or a point in the middle.
            if (t < 0)
            {
                closest = new PointF(p1.X, p1.Y);
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
            }
            else if (t > 1)
            {
                closest = new PointF(p2.X, p2.Y);
                dx = pt.X - p2.X;
                dy = pt.Y - p2.Y;
            }
            else
            {
                closest = new PointF(p1.X + t * dx, p1.Y + t * dy);
                dx = pt.X - closest.X;
                dy = pt.Y - closest.Y;
            }

            return System.Math.Sqrt(dx * dx + dy * dy);
        }


        /// <summary>
        /// Specifying a line Clockwise, negative is outside/above
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static float LineSide(Point p1, Point p2, Point p) //A, B, point
        {
            return (p2.X - p1.X) * (p.Y - p1.Y) - (p2.Y - p1.Y) * (p.X - p1.X); //(Bx - Ax) * (Cy - Ay) - (By - Ay) * (Cx - Ax)
        }


    }



}
