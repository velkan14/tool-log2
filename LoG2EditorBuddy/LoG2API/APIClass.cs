using GAF;
using Log2CyclePrototype.Exceptions;
using Log2CyclePrototype.LoG2API.Elements;
using Log2CyclePrototype.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Log2CyclePrototype.LoG2API
{

    /// <summary>
    /// Auxiliary class to manipulate LoG2 map elements (add, remove) like start/ending points, puzzle elements, etc
    /// </summary>
    public static class APIClass
    {

        public static Map _emergencyRestoreMap { get; set; }
        public static Map CurrentMap { get; set; }
        //Needed to distinguish map changes by user or algorithm
        public static bool _mapSaved = false;
        public static ArrayList DifferentMapTiles { get { return CurrentMap.Tiles; } }

        private static Tuple<string, int> _walkableTile = new Tuple<string, int>("dungeon_floor", 1);
        private static Tuple<string, int> _unwalkableTile = new Tuple<string, int>("dungeon_wall", 2);

        public static int UnwalkableCellValue { get { return _unwalkableTile.Item2; } }
        public static int WalkableCellValue { get { return _walkableTile.Item2; } }


        /// <summary>
        /// Quickly calculate the amount of difference between 2 Map objects
        /// </summary>
        /// <param name="prevMap"> Previous Map parsed object </param>
        /// <param name="currMap"> Currently loaded Map object </param>
        /// <returns></returns>
        public static double CalculateDifference(Map prevMap, Map currMap)
        {
            double difference = 0.0;

            //comparar genes
            for (int i = 0; i < prevMap.Cells.Count; i++ )
            {
                if (prevMap.Cells[i].CellType != currMap.Cells[i].CellType)
                    difference++;
            }
            //if the different tiles array is different, means the difference will be = (CurrentMap.Width * CurrentMap.Height) because all cell indexes will increase/decrease
            if (prevMap.Tiles.Count != prevMap.Tiles.Count) 
            {
                difference -= (CurrentMap.Width * CurrentMap.Height) * System.Math.Abs(prevMap.Tiles.Count - prevMap.Tiles.Count);
            }
            //comparar elementos de puzzle
            //difference += System.Math.Abs(prevMap.MapElements.Count - currMap.MapElements.Count);
            Debug.WriteLine("User disturbance: "+difference);
            return difference;
        }


        /// <summary>
        /// Counts the number of walkable cells in a chromosome
        /// </summary>
        /// <param name="chromosome"></param>
        /// <returns></returns>
        public static int CountWalkableCellsInChromosome(Chromosome chromosome)
        {
            int res = 0;
            foreach (var c in chromosome.Genes)
            {
                var tmpC = (Cell)c.ObjectValue;
                if (tmpC.IsWalkable)
                    res++;
            }
            return res;
        }


        /// <summary>
        /// Parses the dungeon.lua file in the current project directory
        /// </summary>
        /// <returns> A Map object representing the current dungeon.lua file contents </returns>
        public static Map ParseMapFile2()
        {

            Map localMap = null;
            string name = "", ambientTrack = "";
            int[] levelCoord = null;
            int width = 0, height = 0;

            StartingPoint startingPoint = null;
            List<EndingPoint> endingPoints = null;
            List<Cell> localCells = new List<Cell>();
            ArrayList localT = new ArrayList(32 * 32);
            ArrayList localDifferentTiles = new ArrayList();
            int stage = -1;
            int tileStartingLine = 0;

            Monster.MonsterType tmpMonstertype;
            Text.TextType tmpTexttype;
            Door.DoorType tmpDoortype;
            Lever.LeverType tmpLeverType;
            Lock.LockType tmpLockType;
            ButtonE.ButtonType tmpButtonType;

            Dictionary<string, MapElement> uniqueIDElement = new Dictionary<string, MapElement>();
            string id = "";
            int x = 0;
            int y = 0;
            int o = 0;
            int h = 0;
            string uniqueId = "";

            try
            {
                string[] lines = System.IO.File.ReadAllLines(DirectoryManager.DungeonFilePath);

                for (int i = 0; i < lines.Length; i++)
                {
                    string currentLine = lines[i];


                    if (currentLine.Contains("name"))
                    {
                        name = lines[i].Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries)[1];
                    }
                    else if (currentLine.Contains("width"))
                    {
                        width = Convert.ToInt32(lines[i].Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)[2]);
                        height = Convert.ToInt32(lines[i + 1].Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)[2]);

                        localMap = new Map(name, width, height);
                        //Logger.AppendText("map created");
                    }
                    else if (currentLine.Contains("levelCoord"))
                    {
                        var tmp = lines[i].Split(new char[] { '{', ',', '}' }, StringSplitOptions.RemoveEmptyEntries);
                        levelCoord = new int[] { Convert.ToInt32(tmp[1]), Convert.ToInt32(tmp[2]), Convert.ToInt32(tmp[3]) };
                    }
                    else if (currentLine.Contains("ambientTrack"))
                    {
                        ambientTrack = lines[i].Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries)[1];
                    }
                    else if (currentLine.Contains("tiles = {"))
                    {
                        // entered tile type parsing stage
                        stage = 0;
                        //i++; // skip to the next line
                        continue;
                    }
                    else if (currentLine.Contains("loadLayer"))
                    {
                        //entered tile grid parsing stage
                        stage = 1;
                        i++; //skip to the next line
                        tileStartingLine = i;
                    }
                    else if (currentLine.Contains("spawn(") && stage == -1)
                    {
                        //entered object parsing stage
                        stage = 2;
                    }

                    switch (stage)
                    {
                        case 0:
                            //parse tile type
                            if (currentLine.Contains("}"))
                            {                                
                                stage = -1;
                                //Logger.AppendText("Finished parsing floors");
                            }
                            else
                            {
                                string l = currentLine.Trim();
                                //Debug.WriteLine(l.Length);
                                string tmpS = l.Substring(1, l.Length - 3);
                                
                                localDifferentTiles.Add(tmpS);
                            }
                            break;

                        case 1:
                            //parse tiles                            
                            if (currentLine.Contains("}")) //finished parsing tiles
                            {

                                stage = -1;
                            }
                            else
                            {
                                var cellLine = lines[i].Split(new char[] { ',', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                int tempI = i - tileStartingLine;

                                for (int j = 0; j < width; j++)
                                {

                                    int cVal = Convert.ToInt32(cellLine[j]); 
                                    
                                    //get tile name
                                    string tileName = (string)localDifferentTiles[cVal - 1];
                                    Cell newCell;
                                    if (tileName.Contains("wall"))
                                    {
                                        newCell = new Cell(j, tempI, _unwalkableTile.Item2);
                                        newCell.IsWalkable = false;
                                    }
                                    else
                                    {
                                        newCell = new Cell(j, tempI, _walkableTile.Item2);
                                        newCell.IsWalkable = true;
                                    }

                                    localCells.Add(newCell);
                                }
                            }
                            break;

                        case 2:
                            string[] splitString = currentLine.Split(new Char[] { ',', '(', ')', '"' }, StringSplitOptions.RemoveEmptyEntries);

                            id = splitString[1];
                            x = Convert.ToInt32(splitString[2]);
                            y = Convert.ToInt32(splitString[3]);
                            o = Convert.ToInt32(splitString[4]);
                            h = Convert.ToInt32(splitString[5]);
                            uniqueId = splitString[6];

                            var tmpCell = localCells[(y * localMap.Height) + x];
                            
                            if (id.Contains("starting"))
                            {                            
                                startingPoint = new StartingPoint(id, x, y, o, h, uniqueId);
      
                                tmpCell.IsStartingPoint = true;
                                tmpCell.StartPoint = startingPoint;

                                uniqueIDElement.Add(uniqueId, startingPoint);
                            }
                            else if (id.Contains("exit") || id.Contains("stairs") || id.Contains("healing_crystal"))
                            {
                                Debug.WriteLine("Ending point suported!");
                                if (endingPoints == null)
                                    endingPoints = new List<EndingPoint>();

                                var newEndingPoint = new EndingPoint(id, x, y, o, h, uniqueId);
               
                                tmpCell.IsEndingPoint = true;
                                tmpCell.EndPoint = newEndingPoint;
                                endingPoints.Add(newEndingPoint);
                                
                                uniqueIDElement.Add(uniqueId, newEndingPoint);
                            }
                            else if (id.Contains("torch_holder"))
                            {
                                var newTorchHolder = new TorchHolder(id, x, y, o, h, uniqueId);


                                while (lines[i + 1].Contains(uniqueId))
                                {
                                    if (lines[i + 1].Contains("socket:addConnector"))
                                    {
                                        string[] line = lines[i + 1].Split(new Char[] { ',', '(', ')', '"' }, StringSplitOptions.RemoveEmptyEntries);
                                        newTorchHolder.addConnector(line[1], line[3], line[5]);
                                    }
                                    else if (lines[i + 1].Contains("controller:setHasTorch"))
                                    {
                                        string[] line = lines[i + 1].Split(new Char[] { ',', '(', ')', '"' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (line[1].Contains("true")) newTorchHolder.HasTorch = true;
                                        else newTorchHolder.HasTorch = false;
                                    }
                                    i++;
                                }

                                tmpCell.AddElement(newTorchHolder);
                            }
                            else if (Monster.MonsterType.TryParse(id, true, out tmpMonstertype))
                            {
                                var newMonster = new Monster(id, x, y, o, h, uniqueId);

                                tmpCell.Monster = newMonster;
                            }
                            else if (Text.TextType.TryParse(id, true, out tmpTexttype))
                            {
                                var newText = new Text(id, x, y, o, h, uniqueId);
                                while (lines[i + 1].Contains(uniqueId))
                                {
                                    if (lines[i + 1].Contains("walltext:setWallText"))
                                    {
                                        string[] line = lines[i + 1].Split(new Char[] { ',', '(', ')', '"' }, StringSplitOptions.RemoveEmptyEntries);
                                        newText.TextWritten = line[1];
                                    }
                                    i++;
                                }
                                tmpCell.AddElement(newText);
                            }
                            else if(Door.DoorType.TryParse(id, true, out tmpDoortype))
                            {
                                var newDoor = new Door(id, x, y, o, h, uniqueId);
                                while (lines[i + 1].Contains(uniqueId))
                                {
                                    if (lines[i + 1].Contains("door:setPullChain"))
                                    {
                                        string[] line = lines[i + 1].Split(new Char[] { ',', '(', ')', '"' }, StringSplitOptions.RemoveEmptyEntries);
                                        if(line[1].Contains("true")) newDoor.PullChain = true;
                                        else newDoor.PullChain = false;
                                    }
                                    i++;
                                }
                                tmpCell.AddElement(newDoor);
                            }
                            else if(Lever.LeverType.TryParse(id, true, out tmpLeverType))
                            {
                                var newLever = new Lever(id, x, y, o, h, uniqueId);
                                while (lines[i + 1].Contains(uniqueId))
                                {
                                    if (lines[i + 1].Contains("setDisableSelf"))
                                    {
                                        string[] line = lines[i + 1].Split(new Char[] { ',', '(', ')', '"' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (line[1].Contains("true")) newLever.DisableSelf = true;
                                        else newLever.DisableSelf = false;
                                    }else if(lines[i + 1].Contains("addConnector"))
                                    {
                                        string[] line = lines[i + 1].Split(new Char[] { ',', '(', ')', '"' }, StringSplitOptions.RemoveEmptyEntries);
                                        newLever.addConnector(line[1], line[3], line[5]);
                                    }
                                    i++;
                                }
                                tmpCell.AddElement(newLever);
                            }
                            else if(Lock.LockType.TryParse(id, true, out tmpLockType))
                            {
                                var newLock = new Lock(id, x, y, o, h, uniqueId);
                                while (lines[i + 1].Contains(uniqueId))
                                {
                                    if (lines[i + 1].Contains("lock:setOpenedBy"))
                                    {
                                        string[] line = lines[i + 1].Split(new Char[] { ',', '(', ')', '"' }, StringSplitOptions.RemoveEmptyEntries);
                                        newLock.OpenedBy = line[1];
                                    }
                                    else if (lines[i + 1].Contains("addConnector"))
                                    {
                                        string[] line = lines[i + 1].Split(new Char[] { ',', '(', ')', '"' }, StringSplitOptions.RemoveEmptyEntries);
                                        newLock.addConnector(line[1], line[3], line[5]);
                                    }
                                    i++;
                                }
                                tmpCell.AddElement(newLock);
                            }
                            else if (ButtonE.ButtonType.TryParse(id, true, out tmpButtonType))
                            {
                                var newElement = new ButtonE(id, x, y, o, h, uniqueId);
                                while (lines[i + 1].Contains(uniqueId))
                                {
                                    if (lines[i + 1].Contains("setDisableSelf"))
                                    {
                                        string[] line = lines[i + 1].Split(new Char[] { ',', '(', ')', '"' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (line[1].Contains("true")) newElement.DisableSelf = true;
                                        else newElement.DisableSelf = false;
                                    }
                                    else if (lines[i + 1].Contains("addConnector"))
                                    {
                                        string[] line = lines[i + 1].Split(new Char[] { ',', '(', ')', '"' }, StringSplitOptions.RemoveEmptyEntries);
                                        newElement.addConnector(line[1], line[3], line[5]);
                                    }
                                    i++;
                                }
                                tmpCell.AddElement(newElement);
                            }
                            else //any other object
                            {

                            }

                            break;

                        default: break;
                    }
                }

                var wc = new List<Cell>();
                foreach (Cell c in localCells)
                {
                    if (c.IsWalkable)
                        wc.Add(c);
                }

                localMap.StartPoint = startingPoint;
                localMap.EndPointList = endingPoints;
                localMap.AmbientTrack = ambientTrack;
                localMap.LevelCoord = levelCoord;
                localMap.Cells = localCells;
                localMap.Tiles = localDifferentTiles;
                localMap.Id = -1; //testing

                CurrentMap = localMap;

            }
            catch (Exception e) { DebugUtilities.DebugException(e); }

            return localMap;
        }

        public static Map ParseMapFile()
        {
            Char[] delimiters = { '=', ' ', ',', '"', '{', '}' , '\r', '\n', '\t' , ')', '(' };

            Monster.MonsterType tmpMonstertype;
            Text.TextType tmpTextType;
            Door.DoorType tmpDoorType;
            Lever.LeverType tmpLeverType;
            Lock.LockType tmpLockType;
            ButtonE.ButtonType tmpButtonType;
            Alcove.AlcoveType tmpAlcoveType;
            PressurePlate.PressurePlateType tmpPressurePlateType;
           
            string fileText = System.IO.File.ReadAllText(DirectoryManager.DungeonFilePath);

            string patternSpawn = @"spawn\(.*\)";
            string patternParameter = @"\w+\.\w+\:\w+\(.*\)";
            string patternLoadLayer = @"loadLayer\(([^)]+)\)";
            string patternMap = @"\w+ = .*,";
            string patternTiles = @"tiles = {(\n|\t|[^{])*(?=})";


            MatchCollection matchsSpawn = Regex.Matches(fileText, patternSpawn, RegexOptions.IgnoreCase);
            MatchCollection matchsParameters = Regex.Matches(fileText, patternParameter, RegexOptions.IgnoreCase);
            MatchCollection matchsMap = Regex.Matches(fileText, patternMap, RegexOptions.IgnoreCase);
            Match matchLayer = Regex.Match(fileText, patternLoadLayer, RegexOptions.IgnoreCase);
            Match matchTiles = Regex.Match(fileText, patternTiles, RegexOptions.IgnoreCase);

            string name = "", ambientTrack = "";
            int width = 0, height = 0;
            int[] levelCoord = null;
            List<string> tiles = new List<string>();
            List<Cell> cells = new List<Cell>();
            List<EndingPoint> endingPoints = new List<EndingPoint>();
            StartingPoint startingPoint = null;

            foreach (Match m in matchsMap)
            {           
                string[] split = m.Value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                if (split[0].Contains("name"))
                {
                    name = split[1]; 
                }
                else if (split[0].Contains("width"))
                {
                    width = Convert.ToInt32(split[1]);
                }
                else if (split[0].Contains("height"))
                {
                    height = Convert.ToInt32(split[1]);
                }
                else if (split[0].Contains("levelCoord"))
                {
                    levelCoord = new int[] { Convert.ToInt32(split[1]), Convert.ToInt32(split[2]), Convert.ToInt32(split[3]) };
                }
                else if (split[0].Contains("ambientTrack"))
                {
                    ambientTrack = split[1];
                }
            }

            string[] splitTiles = matchTiles.Value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            for(int i = 1; i < splitTiles.Length; i++)
            {
                tiles.Add(splitTiles[i]);
            }

            string[] splitLayer = matchLayer.Value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 2; i < splitLayer.Length; i++)
            {
                int x = (i - 2) % width;
                int y = (i - 2) / width;
                int type = Convert.ToInt32(splitLayer[i]);
                Cell c = new Cell(x, y, type);
                if (type == 1) c.IsWalkable = true;
                else if (type == 2) c.IsWalkable = false; //FIXME: Se houverem mais tipos esta comparação pode não chegar.
                cells.Add(c);
            }

            foreach(Match m in matchsSpawn)
            {
                string[] splitString = m.Value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                string id = splitString[1];
                int  x = Convert.ToInt32(splitString[2]);
                int y = Convert.ToInt32(splitString[3]);
                int o = Convert.ToInt32(splitString[4]);
                int h = Convert.ToInt32(splitString[5]);
                string uniqueId = splitString[6];

                Cell tmpCell = cells.Where(c => c.X == x && c.Y == y).First();

                if (id.Contains("starting_location"))
                {
                    startingPoint = new StartingPoint(id, x, y, o, h, uniqueId);
                    tmpCell.IsStartingPoint = true;
                    tmpCell.StartPoint = startingPoint;
                }
                else if (id.Contains("exit") || id.Contains("stairs") || id.Contains("healing_crystal"))
                {
                    var newEndingPoint = new EndingPoint(id, x, y, o, h, uniqueId);
                    tmpCell.IsEndingPoint = true;
                    tmpCell.EndPoint = newEndingPoint;

                    endingPoints.Add(newEndingPoint);
                }
                else if (id.Contains("torch_holder"))
                {
                    var newTorchHolder = new TorchHolder(id, x, y, o, h, uniqueId);
                    tmpCell.AddElement(newTorchHolder);
                }
                else if (Alcove.AlcoveType.TryParse(id, true, out tmpAlcoveType))
                {
                    var newElement = new Alcove(id, x, y, o, h, uniqueId);
                    tmpCell.AddElement(newElement);
                }
                else if (ButtonE.ButtonType.TryParse(id, true, out tmpButtonType))
                {
                    var newElement = new ButtonE(id, x, y, o, h, uniqueId);
                    tmpCell.AddElement(newElement);
                }
                else if (Door.DoorType.TryParse(id, true, out tmpDoorType))
                {
                    var newDoor = new Door(id, x, y, o, h, uniqueId);
                    tmpCell.AddElement(newDoor);
                }
                /*else if(false)
                {
                //ITEM
                }*/
                else if (Lever.LeverType.TryParse(id, true, out tmpLeverType))
                {
                    var newLever = new Lever(id, x, y, o, h, uniqueId);
                    tmpCell.AddElement(newLever);
                }
                else if (Lock.LockType.TryParse(id, true, out tmpLockType))
                {
                    var newLock = new Lock(id, x, y, o, h, uniqueId);
                    tmpCell.AddElement(newLock);
                }
                else if (Monster.MonsterType.TryParse(id, true, out tmpMonstertype))
                {
                    var newMonster = new Monster(id, x, y, o, h, uniqueId);
                    tmpCell.Monster = newMonster;
                }
                else if (PressurePlate.PressurePlateType.TryParse(id, true, out tmpPressurePlateType))
                {
                    var newElement = new PressurePlate(id, x, y, o, h, uniqueId);
                    tmpCell.AddElement(newElement);
                }
                else if (Text.TextType.TryParse(id, true, out tmpTextType))
                {
                    var newText = new Text(id, x, y, o, h, uniqueId);
                    tmpCell.AddElement(newText);
                }
                else
                {
                    var newElement = new Item(id, x, y, o, h, uniqueId);
                    tmpCell.AddElement(newElement);
                }
            }

            Map map = new Map(name, width, height);

            map.StartPoint = startingPoint;
            map.EndPointList = endingPoints;
            map.AmbientTrack = ambientTrack;
            map.LevelCoord = levelCoord;
            map.Cells = cells;
            map.Tiles = new ArrayList(tiles); //FIXME
            CurrentMap = map;
            return map;
        }

        /// <summary>
        /// Creates a Map object from a Chromosome
        /// </summary>
        /// <param name="chromosome"></param>
        /// <returns></returns>
        public static Map MapObjectFromChromosome(Chromosome chromosome)
        {
            //some attributes remain equal to the originally loaded map
            Map mapObject = new Map(APIClass.CurrentMap.Name, APIClass.CurrentMap.Width, APIClass.CurrentMap.Height);
            mapObject.AmbientTrack = APIClass.CurrentMap.AmbientTrack;
            mapObject.LevelCoord = APIClass.CurrentMap.LevelCoord;
            mapObject.Tiles = APIClass.CurrentMap.Tiles;

            try
            {
                foreach (var g in chromosome.Genes)
                {
                    var tmpC = ((Cell)g.ObjectValue);

                    //if (tmpC.CellType < mapObject.GroundFirstIndex || tmpC.CellType > mapObject.GroundLastIndex)
                    //    tmpC.IsWalkable = true;
                    //else tmpC.IsWalkable = false;

                    if (tmpC.IsStartingPoint)
                        mapObject.StartPoint = tmpC.StartPoint.CloneJson();//new StartingPoint("starting_location",tmpC.X, tmpC.Y, (int)MapElement.Orientation.Top, 0, "generated_starting_point");
                    else if (tmpC.IsEndingPoint)
                        mapObject.EndPointList.Add(tmpC.EndPoint.CloneJson());//new EndingPoint("castle_arena_stairs_down", tmpC.X, tmpC.Y, (int)MapElement.Orientation.Top, 0, "generated_ending_point_" + mapObject.EndPointList.Count + 1));


                    //if(tmpC.ElementsInCell.Count > 0)
                    //{
                    //    foreach (MapElement e in tmpC.ElementsInCell.Values)
                    //        mapObject.MapElements.Add();
                    //}
                    mapObject.Cells.Add(tmpC);
                }

            }
            catch (Exception e) { DebugUtilities.DebugException(e); }

            return mapObject;
        }

        /// <summary>
        /// Takes the user selected cells from the displayed solution and saves them to a new map file
        /// </summary>
        /// <param name="currentSolution"></param>
        /// <param name="selectedPoints"></param>
        public static bool ExportSelection(List<Cell> currentSolution, List<Point> selectedPoints)
        {
            if (APIClass.CurrentMap == null) throw new CurrentMapNullException();
            bool result = false;
            try
            {
                List<Cell> chosenOnes = new List<Cell>();
                //take current solution and selected points and identify the common elements between them
                foreach (var p in selectedPoints)
                {
                    var tmpCell = currentSolution.Find(c => (c.X == p.X && c.Y == p.Y));
                    if (tmpCell != null)
                        chosenOnes.Add(tmpCell.CloneJson());
                    else Debug.WriteLine("[ExportSelection] Couldn't find matching Cell element in current solution.");
                }
                //replace those points and apply them to a clone of the CurrentMap
                var mapClone = CurrentMap.CloneJson();
                foreach (var c in chosenOnes)
                {
                    mapClone.SetCell(c);
                }
                //save the newly changed cloned map
                SaveMapFile(mapClone);
                result = true;
            }
            catch (Exception e) { DebugUtilities.DebugException(e); }
            return result;
        }

        /// <summary>
        /// Takes the user selected cells from the displayed solution and saves them to a new map file
        /// </summary>
        /// <param name="currentSolution"></param>
        /// <param name="selectedPoints"></param>
        public static bool ExportSelection(Map currentSolution, List<Point> selectedPoints)
        {
            if (APIClass.CurrentMap == null) throw new CurrentMapNullException();
            bool result = false;
            try
            {
                List<Cell> chosenOnes = new List<Cell>();
                //take current solution and selected points and identify the common elements between them
                foreach (var p in selectedPoints)
                {
                    var tmpCell = currentSolution.Cells.Find(c => (c.X == p.X && c.Y == p.Y));
                    if (tmpCell != null)
                        chosenOnes.Add(tmpCell.CloneJson());
                    else Debug.WriteLine("[ExportSelection] Couldn't find matching Cell element in current solution.");
                }
                //replace those points and apply them to a clone of the CurrentMap
                var mapClone = CurrentMap.CloneJson();
                foreach (var c in chosenOnes)
                {
                    mapClone.SetCell(c);
                    if (c.IsEndingPoint && !(CurrentMap.GetCellAt(c.X, c.Y).IsEndingPoint))
                        mapClone.EndPointList.Add(c.EndPoint);
                }
                //save the newly changed cloned map
                SaveMapFile(mapClone);
                result = true;
            }
            catch (Exception e) { DebugUtilities.DebugException(e); }
            return result;
        }

        /// <summary>
        /// Saves the Map object passed as an argument to a .lua file, interpretable by the LoG2 game
        /// </summary>
        /// <param name="mapToSave"></param>
        /// <returns></returns>
        public static bool SaveMapFile(Map mapToSave) //TODO: Change the temp dir to the final dir
        {
            _emergencyRestoreMap = CurrentMap.CloneJson();

            if(MainForm._lockedCellList != null && MainForm._lockedCellList.Count > 0)
            {
                foreach(var c in MainForm._lockedCellList)
                {
                    //var toReplace = toSave.Cells.Find(cell => (cell.X == c.X && cell.Y == c.Y));
                    //toReplace = MainForm._lockedCellList.Find(cell2 => (cell2.X == c.X && cell2.Y == c.Y)) ;
                    if (mapToSave.SetCell(c)) Debug.WriteLine("REPLACED CELL!");
                    
                }
            }


            var result = false;
            StringBuilder sb = new StringBuilder();

            Logger.AppendText("Saving new map to:");
            Logger.AppendText(DirectoryManager.DungeonFilePath);
            //Logger.AppendText("@\testSave.lua...");

            try
            {
                
                sb.AppendLine("-- This file has been generated by Dungeon Editor 2.1.13 and modified by GA assisted program");
                sb.AppendLine("--- level 1 ---");

                sb.Append("newMap{\n");
                sb.Append("\tname = " + "\"" + mapToSave.Name + "\",\n");
                sb.Append("\twidth = " + mapToSave.Width + ",\n");
                sb.Append("\theight = " + mapToSave.Height + ",\n");
                sb.Append("\tlevelCoord = {" + mapToSave.LevelCoord[0] + "," + mapToSave.LevelCoord[1] + "," + mapToSave.LevelCoord[2] + "},\n");
                sb.Append("\tambientTrack = " + "\"" + mapToSave.AmbientTrack + "\",\n");
                sb.Append("\ttiles = {\n");
                //for (int i = 0; i < toSave.DifferentTiles.Count; i++)
                //sb.Append("\t\t" + "\"" + toSave.DifferentTiles[i] + "\",\n");

                sb.Append("\t\t" + "\"" + _walkableTile.Item1 + "\",\n");
                sb.Append("\t\t" + "\"" + _unwalkableTile.Item1 + "\",\n");

                sb.Append("\t}\n");
                sb.AppendLine("}");
                sb.Append("loadLayer(\"tiles\", {\n");

                //tiles
                for (int y = 0; y < mapToSave.Height; y++){
                    sb.Append("\t");
                    for (int x = 0; x < mapToSave.Width; x++){
                        sb.Append(mapToSave.Cells[y * mapToSave.Width + x].CellType + ",");
                        if (x == mapToSave.Width - 1)
                            sb.Append("\n");
                    }
                }
                
                sb.AppendLine("})");

                //start and ending points
                if (mapToSave.StartPoint != null){
                    sb.Append(mapToSave.StartPoint.PrintElement());
                    sb.Append("\n");
                }
                if (mapToSave.EndPointList != null && mapToSave.EndPointList.Count > 0)
                    foreach (var e in mapToSave.EndPointList){
                        sb.Append(e.PrintElement());
                        sb.Append("\n");
                    }

                foreach(Cell c in mapToSave.Cells)
                {
                    foreach(MapElement el in c.ElementsInCell)
                    {
                        sb.Append(el.PrintElement());
                        sb.Append("\n");
                    }
                }

                //foreach (string o in CurrentMap.MapObjects)
                //{
                //    sb.Append(o);
                //    sb.Append("\n");
                //}


                ////TODO: HACKERINO, CHECK http://stackoverflow.com/questions/24644464/json-net-type-is-an-interface-or-abstract-class-and-cannot-be-instantiated 

                //foreach (JObject mapElement in toSave.MapElements.Values)
                //{
                //    //Logger.AppendText(mapElement.ToString());
                //    var rep = mapElement.ToString();
                //    if (rep.Contains("DOOR"))
                //    {
                //        //Logger.AppendText("found door");
                //        sb.Append((mapElement.ToObject<Door>()).PrintElement().Replace("\n", ""));
                //    }
                //    else if (rep.Contains("LEVER"))
                //    {
                //        //Logger.AppendText("found lever");
                //        sb.Append((mapElement.ToObject<Lever>()).PrintElement().Replace("\n", ""));
                //    }
                //    //sb.Append(mapElement.PrintElement().Replace("\n", ""));
                //}


                APIClass._mapSaved = true;

                //System.IO.File.WriteAllLines(DirectoryManager.ProjectDir + @"\testSave.lua", sb.ToString().Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries)); //write back to file
                System.IO.File.WriteAllLines(DirectoryManager.DungeonFilePath, sb.ToString().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)); //write back to file

                Logger.AppendText("Save successful.");

                result = true;

            }
            catch (Exception e)
            {
                Logger.AppendText("Save failed! Check console for report.");

                Debug.WriteLine("Msg: "+e.Message);
                Debug.WriteLine("Inner: " + e.InnerException);
                Debug.WriteLine("Base: " + e.GetBaseException());
                return false;
            }

            return result;
        }


    }
}
