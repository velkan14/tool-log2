using GAF;
using EditorBuddyMonster.LoG2API;
using EditorBuddyMonster.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorBuddyMonster
{
    public static class SearchUtils
    {
              
        
        private static int ExpandValue(Node n, Node[,] grid)
        {
            int n1 = 0, n2 = 0, n3 = 0, n4 = 0;
            if (n.visited || !n.walkable)
                return 0;
            else
            {
                n.visited = true;
                Node tmpNode = null;
                try
                {
                    if (n.x > 0){ //test left
                        //Debug.WriteLine("left");
                        tmpNode = grid[n.y, n.x - 1]; // chromosome.Genes[n.Y * _currentMap.Height + n.X - 1];
                        if (tmpNode != null && !tmpNode.visited && tmpNode.walkable){
                            n1 = ExpandValue(tmpNode, grid);
                        }
                    }
                    if (n.x < APIClass.CurrentMap.Width - 1){ //test right
                        //Debug.WriteLine("right");
                        tmpNode = grid[n.y, n.x + 1]; // chromosome.Genes[n.Y * _currentMap.Height + n.X - 1];
                        if (tmpNode != null && !tmpNode.visited && tmpNode.walkable){
                            n2 = ExpandValue(tmpNode, grid);
                        }
                    }
                    if (n.y > 0){ //test top
                        //Debug.WriteLine("top");
                        tmpNode = grid[n.y - 1, n.x]; // chromosome.Genes[n.Y * _currentMap.Height + n.X - 1];
                        if (tmpNode != null && !tmpNode.visited && tmpNode.walkable){
                            n3 = ExpandValue(tmpNode, grid);
                        }
                    }
                    if (n.y < APIClass.CurrentMap.Height - 1){ //test bottom
                        //Debug.WriteLine("bottom");
                        tmpNode = grid[n.y + 1, n.x]; // chromosome.Genes[n.Y * _currentMap.Height + n.X - 1];
                        if (tmpNode != null && !tmpNode.visited && tmpNode.walkable){
                            n4 = ExpandValue(tmpNode, grid);
                        }
                    }
                }
                catch (Exception e){DebugUtilities.DebugException(e);}

            }
            return 1 + n1 + n2 + n3 + n4;
        }


        
        /// <summary>
        /// Returns the longest walkable path in a chromosome
        /// </summary>
        /// <param name="chromosome"></param>
        /// <returns></returns>
        public static int EvaluateLinearityValue(Chromosome chromosome)
        {
            var result = 0;
            var biggestPath = 0;
            //create and populate 2d grid
            Node[,] grid = new Node[APIClass.CurrentMap.Height, APIClass.CurrentMap.Width]; //y,x

            for (int j = 0; j < APIClass.CurrentMap.Height; j++)
            {
                for (int i = 0; i < APIClass.CurrentMap.Width; i++)
                {
                    var newNode = new Node(i,j);
                    if (((Cell)chromosome.Genes[j * APIClass.CurrentMap.Height + i].ObjectValue).IsWalkable)
                        newNode.walkable = true;
                    grid[j, i] = newNode;
                }
            }
            //calculate biggest path TODO: improve performance (implement iterative approach)
            foreach (Node n in grid) {
                if (!n.walkable || n.visited)
                    continue;
                result = ExpandValue(n, grid);
                if (result > biggestPath)
                    biggestPath = result;
            }
            return biggestPath;
        }




        private static List<Node> ExpandList(Node n, Node[,] grid)
        {
            List<Node> n1 = new List<Node>(), n2 = new List<Node>(), n3 = new List<Node>(), n4 = new List<Node>();
            if (n.visited || !n.walkable)
                return null;
            else
            {
                n.visited = true;

                Node tmpNode = null;

                try
                {
                    if (n.x > 0)
                    { //test left
                        //Debug.WriteLine("left");
                        tmpNode = grid[n.y, n.x - 1]; // chromosome.Genes[n.Y * _currentMap.Height + n.X - 1];
                        if (tmpNode != null && !tmpNode.visited && tmpNode.walkable)
                        {
                            //totalArea += Expand(tmpNode, grid);
                            n1.AddRange(ExpandList(tmpNode, grid));
                        }
                    }
                    if (n.x < APIClass.CurrentMap.Width - 1)
                    { //test right
                        //Debug.WriteLine("right");
                        tmpNode = grid[n.y, n.x + 1]; // chromosome.Genes[n.Y * _currentMap.Height + n.X - 1];
                        if (tmpNode != null && !tmpNode.visited && tmpNode.walkable)
                        {
                            //totalArea += Expand(tmpNode, grid);
                            n2.AddRange(ExpandList(tmpNode, grid));
                        }
                    }
                    if (n.y > 0)
                    { //test top
                        //Debug.WriteLine("top");
                        tmpNode = grid[n.y - 1, n.x]; // chromosome.Genes[n.Y * _currentMap.Height + n.X - 1];
                        if (tmpNode != null && !tmpNode.visited && tmpNode.walkable)
                        {
                            //totalArea += Expand(tmpNode, grid);
                            n3.AddRange(ExpandList(tmpNode, grid));
                        }
                    }
                    if (n.y < APIClass.CurrentMap.Height - 1)
                    { //test bottom
                        //Debug.WriteLine("bottom");
                        tmpNode = grid[n.y + 1, n.x]; // chromosome.Genes[n.Y * _currentMap.Height + n.X - 1];
                        if (tmpNode != null && !tmpNode.visited && tmpNode.walkable)
                        {
                            //totalArea += Expand(tmpNode, grid);
                            n4.AddRange(ExpandList(tmpNode, grid));
                        }
                    }

                }
                catch (Exception e)
                {
                    DebugUtilities.DebugException(e);
                }

            }
            var result = new List<Node>();
            result.Add(n);
            if (n1.Count > 0)
                result.AddRange(n1);
            if (n2.Count > 0)
                result.AddRange(n2);
            if (n3.Count > 0)
                result.AddRange(n3);
            if (n4.Count > 0)
                result.AddRange(n4);
            return result;
        }





        /// <summary>
        /// Get a list of all paths in the grid
        /// </summary>
        /// <param name="chromosome"> Chromosome to evaluate </param>
        /// <returns></returns>
        public static List<List<Node>> GetAllPaths(Chromosome chromosome)
        {
            var result = new List<Node>();
            var pathList = new List<List<Node>>();
            //create and populate 2d grid
            Node[,] grid = new Node[APIClass.CurrentMap.Height, APIClass.CurrentMap.Width]; //y,x

            for (int j = 0; j < APIClass.CurrentMap.Height; j++)
            {
                for (int i = 0; i < APIClass.CurrentMap.Width; i++)
                {
                    var newNode = new Node(i, j);
                    if (((Cell)chromosome.Genes[j * APIClass.CurrentMap.Height + i].ObjectValue).IsWalkable)
                        newNode.walkable = true;
                    grid[j, i] = newNode;
                }
            }
            //calculate biggest path TODO: improve performance (implement iterative approach)
            foreach (Node n in grid)
            {
                if (!n.walkable || n.visited)
                    continue;
                result = ExpandList(n, grid);
                pathList.Add(result);
            }
            return pathList;
        }

        /// <summary>
        /// Creates and returns a Node grid for evaluation based on the chromosome passed as an argument
        /// </summary>
        /// <param name="chromosome"></param>
        /// <returns></returns>
        public static Node[,] GetSearchGridFromChromosome(Chromosome chromosome)
        {
            Node[,] grid = new Node[APIClass.CurrentMap.Height, APIClass.CurrentMap.Width]; //y,x
            for (int j = 0; j < APIClass.CurrentMap.Height; j++)
            {
                for (int i = 0; i < APIClass.CurrentMap.Width; i++)
                {
                    var newNode = new Node(i, j);
                    if (((Cell)chromosome.Genes[j * APIClass.CurrentMap.Height + i].ObjectValue).IsWalkable)
                        newNode.walkable = true;
                    grid[j, i] = newNode;
                }
            }
            return grid;
        }


        /// <summary>
        /// Identifies the path where a cell is located
        /// </summary>
        /// <param name="toLocate"></param>
        /// <param name="allPaths"></param>
        /// <returns> -1 if unsuccessful, valid path index otherwise </returns>
        public static int GetPathIndexForCell(Cell toLocate, List<List<Node>> allPaths)
        {
            for (int i = 0; i < allPaths.Count; i++)
            {
                var p = allPaths[i];
                var r = p.Find(n => (n.x == toLocate.X && n.y == toLocate.Y));
                if(r != null)
                    return i;
            }
            return -1;
        }



        /// <summary>
        /// Returns the shortest distance between 2 points
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static double DiagonalDistanceBetweenPoints(int x1, int y1, int x2, int y2)
        {
            int dx = System.Math.Abs(x2 - x1);
            int dy = System.Math.Abs(y2 - y1);

            int min = System.Math.Min(dx, dy);
            int max = System.Math.Max(dx, dy);

            int diagonalSteps = min;
            int straightSteps = max - min;

            return System.Math.Sqrt(2) * diagonalSteps + straightSteps;
        }


        /// <summary>
        /// Returns Manhattan distance between 2 points
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static double ManhattanDistanceBetweenPoints(int x1, int y1, int x2, int y2)
        {
            int dx = System.Math.Abs(x2 - x1);
            int dy = System.Math.Abs(y2 - y1);

            return dx+dy;
        }


        /// <summary>
        /// Returns the remaining manhattan distance between 2 walkable paths
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double GetRemainingDistanceBetweenPaths(List<Node> p1, List<Node> p2)
        {
            double dist = 1024; //maxvalue is overkill :p

            foreach(var n1 in p1){
                foreach (var n2 in p2)
                {
                    var tmpDist = ManhattanDistanceBetweenPoints(n1.x, n1.y, n2.x, n2.y);
                    if (tmpDist < dist)
                        dist = tmpDist;
                }
            }
            return dist-1;
        }


        ///// <summary>
        ///// Check if start and end points of a map are reachable
        ///// </summary>
        ///// <param name="startX"></param>
        ///// <param name="startY"></param>
        ///// <param name="endX"></param>
        ///// <param name="endY"></param>
        ///// <param name="allPaths"> List of all walkable paths in the grid </param>
        ///// <returns></returns>
        //public static bool StartEndPointsInSamePath(int startX, int startY, int endX, int endY, List<List<Node>> allPaths)
        //{
        //    bool result = false;

        //    foreach (var p in allPaths)
        //    {
        //        var s = p.Find(n => (n.x == startX && n.y == startY));
        //        var e = p.Find(n => (n.x == endX && n.y == endY));

        //        if (s != null && e != null)
        //            return true;
        //    }
        //    return result;
        //}


        /// <summary>
        /// Check if start and end points of a map are reachable
        /// </summary>
        /// <param name="start"></param>
        /// <param name="endPoint"></param>
        /// <param name="allPaths"></param>
        /// <returns> A tuple containing the 2 indexes of the paths the points belong to. If equal, they are in the same path </returns>
        public static Tuple<int, int> StartEndPointsInSamePath(Cell start, Cell endPoint, List<List<Node>> allPaths)
        {
            //bool result = false;
            int startPath = -1;
            int endPath = -1;

            for (int i = 0; i < allPaths.Count; i++)
            {
                var p = allPaths[i];
                var s = p.Find(n => (n.x == start.X && n.y == start.Y));
                var e = p.Find(n => (n.x == endPoint.X && n.y == endPoint.Y));

                if (s != null && e != null)
                    return new Tuple<int, int>(i, i);
                else if (s != null && e == null)
                {
                    if (startPath != -1)
                        Debug.WriteLine("[ALGORITHM SEARCH] Start point path already found!");
                    startPath = i;
                }
                else if (s == null && e != null)
                {
                    if (endPath != -1)
                        Debug.WriteLine("[ALGORITHM SEARCH] End point path already found!");
                    endPath = i;
                }
            }
            return new Tuple<int, int>(startPath, endPath);
        }

        /// <summary>
        /// Check if start and end points of a map are reachable
        /// </summary>
        /// <param name="start"></param>
        /// <param name="endPoint"></param>
        /// <param name="allPaths"></param>
        /// <returns> A tuple containing the 2 indexes of the paths the points belong to. If equal, they are in the same path </returns>
        public static Tuple<int, int> StartEndPointsInSamePath(int startX, int startY, int endX, int endY, List<List<Node>> allPaths)
        {
            //bool result = false;
            int startPath = -1;
            int endPath = -1;

            for (int i = 0; i < allPaths.Count; i++)
            {
                var p = allPaths[i];
                var s = p.Find(n => (n.x == startX && n.y == startY));
                var e = p.Find(n => (n.x == endX && n.y == endY));

                if (s != null && e != null)
                    return new Tuple<int, int>(i, i);
                else if (s != null && e == null)
                {
                    if (startPath != -1)
                        Debug.WriteLine("[ALGORITHM SEARCH] Start point path already found!");
                    startPath = i;
                }
                else if (s == null && e != null)
                {
                    if (endPath != -1)
                        Debug.WriteLine("[ALGORITHM SEARCH] End point path already found!");
                    endPath = i;
                }
            }
            return new Tuple<int, int>(startPath, endPath);
        }


        /// <summary>
        /// Returns a Vector containing the start and ending point coordinates
        /// </summary>
        /// <param name="chromosome"></param>
        /// <returns></returns>
        public static Cell[] FindStartEndPointsCoordinates(Chromosome chromosome)
        {
            Queue<Cell> q = new Queue<Cell>();
            var start = chromosome.Genes.Find( g => (((Cell)g.ObjectValue).IsStartingPoint == true));
            var endPoints = chromosome.Genes.FindAll(g => (((Cell)g.ObjectValue).IsEndingPoint == true));

            if (start != null && (endPoints != null && endPoints.Count > 0))
            {
                q.Enqueue((Cell)start.ObjectValue);
                foreach (var g in endPoints)
                {
                    var endP = ((Cell)g.ObjectValue);
                    q.Enqueue(endP);
                }
                return q.ToArray();
            }
            else
                return null;
        }




        /// <summary>
        /// Given a chromosome, count neighbours for individual cells
        /// </summary>
        /// <param name="chromosome"> Chromosome representing a map </param>
        /// <returns> An int vector containing cumulative number of neighbours for the cells in the map. (index 0 = 0 neighbours, index 1 = 1 neighbour, etc) </returns>
        public static int[] CountIndividualNeighbours(Chromosome chromosome)
        {
            int[] neighbourVec = new int[5]; //how many neighbours for each cell
            try
            {
                var genes = chromosome.Genes;
                //store cell neighbours
                foreach (Gene g in genes)
                {
                    var c = (Cell)g.ObjectValue;
                    if (!c.IsWalkable)
                        continue;
                    int surroundingNeighbours = 0;
                    Cell tmpCell = null;

                    if (c.X > 0)
                    { //test left
                        tmpCell = (Cell)genes[APIClass.CurrentMap.Height * c.Y + (c.X - 1)].ObjectValue;
                        if (tmpCell != null && tmpCell.IsWalkable)
                            //c.NumNeighbours++;
                            surroundingNeighbours++;
                    }
                    if (c.X < APIClass.CurrentMap.Width - 1)
                    { //test right
                        tmpCell = (Cell)genes[APIClass.CurrentMap.Height * c.Y + (c.X + 1)].ObjectValue;
                        if (tmpCell != null && tmpCell.IsWalkable)
                            //c.NumNeighbours++;
                            surroundingNeighbours++;
                    }
                    if (c.Y > 0)
                    { //test top
                        tmpCell = (Cell)genes[APIClass.CurrentMap.Height * (c.Y - 1) + c.X].ObjectValue;
                        if (tmpCell != null && tmpCell.IsWalkable)
                            //c.NumNeighbours++;
                            surroundingNeighbours++;
                    }
                    if (c.Y < APIClass.CurrentMap.Height - 1)
                    { //test bottom
                        tmpCell = (Cell)genes[APIClass.CurrentMap.Height * (c.Y + 1) + c.X].ObjectValue;
                        if (tmpCell != null && tmpCell.IsWalkable)
                            //c.NumNeighbours++;
                            surroundingNeighbours++;
                    }
                    switch (surroundingNeighbours)
                    {
                        case 0: neighbourVec[0]++; break;
                        case 1: neighbourVec[1]++; break;
                        case 2: neighbourVec[2]++; break;
                        case 3: neighbourVec[3]++; break;
                        case 4: neighbourVec[4]++; break;
                    }
                }
            }
            catch (Exception e)
            {
                Utilities.DebugUtilities.DebugException(e);
            }
            return neighbourVec;
        }


        /// <summary>
        /// Create a new maze as a [i,j] matrix from a given chromosome
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static MazeSolver ChromosomeToMaze(Chromosome c)
        {
            int[,] newMaze = new int[APIClass.CurrentMap.Width, APIClass.CurrentMap.Height];

            foreach(var g in c.Genes)
            {
                var tmpC = (Cell)g.ObjectValue;
                if (tmpC.IsWalkable)
                {
                    newMaze[tmpC.X, tmpC.Y] = 0;
                }
                else
                {
                    newMaze[tmpC.X, tmpC.Y] = 1;
                }
            }

            return new MazeSolver(newMaze);
        }



    }

    
    public class Node
    {
        public int x { get; internal set; }
        public int y { get; internal set; }
        public bool visited { get; set; }
        public bool walkable { get; internal set; }

        public Node(int x, int y) { this.x = x; this.y = y; walkable = false; visited = false; }
    }
        

    
}
