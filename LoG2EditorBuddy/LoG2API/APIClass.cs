using GAF;
using EditorBuddyMonster.Exceptions;
using EditorBuddyMonster.LoG2API.Elements;
using EditorBuddyMonster.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace EditorBuddyMonster.LoG2API
{

    /// <summary>
    /// Auxiliary class to manipulate LoG2 map elements (add, remove) like start/ending points, puzzle elements, etc
    /// </summary>
    public static class APIClass
    {
        public static int NUMBER_GENES = 8;

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
            for (int i = 0; i < prevMap.Cells.Count; i++)
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
            Debug.WriteLine("User disturbance: " + difference);
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
        public static Map ParseMapFile()
        {
            Char[] delimiters = { '=', ' ', ',', '"', '{', '}', '\r', '\n', '\t', ')', '(' };
            Char[] delimitersAttributes = { '.', ':', ',', '"', '(', ')', '\r', '\n', '\t' };
            Char[] delimitersAttributes2 = { '.', ':', ',', '"', '(', ')', '\r', '\n', '\t', ' ' };

            Dictionary<string, MapElement> elements = new Dictionary<string, MapElement>();
            Monster.MonsterType tmpMonstertype;
            Text.TextType tmpTextType;
            Door.DoorType tmpDoorType;
            Lever.LeverType tmpLeverType;
            Lock.LockType tmpLockType;
            ButtonE.ButtonType tmpButtonType;
            Alcove.AlcoveType tmpAlcoveType;
            PressurePlate.PressurePlateType tmpPressurePlateType;
            TrapDoor.TrapDoorType tmpTrapDoorType;
            TorchHolder.TorchHolderType tmpTorchHolderType;
            Lantern.LanternType tmpLanternType;
            Altar.AltarType tmpAltarType;
            WallEffect.WallEffectType tmpWallEffectType;
            Weapon.WeaponType tmpWeaponType;

            string fileText = System.IO.File.ReadAllText(DirectoryManager.DungeonFilePath);

            string patternSpawn = @"spawn\(.*\)";
            string patternAttributes = @"\w+\.\w+\:\w+\(.*\)";
            string patternLoadLayer = @"loadLayer\(([^)]+)\)";
            string patternParameters = @"\w+ = .*,";
            string patternTiles = @"tiles = {(\n|\t|[^{])*(?=})";


            MatchCollection matchsSpawn = Regex.Matches(fileText, patternSpawn, RegexOptions.IgnoreCase);
            MatchCollection matchsAttribute = Regex.Matches(fileText, patternAttributes, RegexOptions.IgnoreCase);
            MatchCollection matchsParameters = Regex.Matches(fileText, patternParameters, RegexOptions.IgnoreCase);
            Match matchLayer = Regex.Match(fileText, patternLoadLayer, RegexOptions.IgnoreCase);
            Match matchTiles = Regex.Match(fileText, patternTiles, RegexOptions.IgnoreCase);

            string name = "", ambientTrack = "";
            int width = 0, height = 0;
            int[] levelCoord = null;
            List<string> tiles = new List<string>();
            List<Cell> cells = new List<Cell>();
            List<EndingPoint> endingPoints = new List<EndingPoint>();
            StartingPoint startingPoint = null;

            foreach (Match m in matchsParameters)
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

            for (int i = 1; i < splitTiles.Length; i++)
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

            foreach (Match m in matchsSpawn)
            {
                string[] splitString = m.Value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                string id = splitString[1];
                int x = Convert.ToInt32(splitString[2]);
                int y = Convert.ToInt32(splitString[3]);
                int o = Convert.ToInt32(splitString[4]);
                int h = Convert.ToInt32(splitString[5]);
                string uniqueId = splitString[6];

                Cell tmpCell = cells.Where(c => c.X == x && c.Y == y).First();
                MapElement tmpElement = null;

                if (id.Contains("starting_location"))
                {
                    startingPoint = new StartingPoint(id, x, y, o, h, uniqueId);
                    tmpCell.IsStartingPoint = true;
                    tmpCell.StartPoint = startingPoint;

                    elements.Add(uniqueId, startingPoint);
                }
                else if (id.Contains("exit") || id.Contains("stairs") || id.Contains("healing_crystal"))
                {
                    var newEndingPoint = new EndingPoint(id, x, y, o, h, uniqueId);
                    tmpCell.IsEndingPoint = true;
                    tmpCell.EndPoint = newEndingPoint;

                    endingPoints.Add(newEndingPoint);
                    elements.Add(uniqueId, newEndingPoint);
                }
                else if (Enum.TryParse(id, true, out tmpTorchHolderType))
                {
                    tmpElement = new TorchHolder(id, x, y, o, h, uniqueId);
                }
                else if (id.Equals("scroll"))
                {
                    tmpElement = new Scroll(id, x, y, o, h, uniqueId);
                }
                else if (Enum.TryParse(id, true, out tmpAlcoveType))
                {
                    tmpElement = new Alcove(id, x, y, o, h, uniqueId);
                }
                else if (Enum.TryParse(id, true, out tmpButtonType))
                {
                    tmpElement = new ButtonE(id, x, y, o, h, uniqueId);
                }
                else if (Enum.TryParse(id, true, out tmpDoorType))
                {
                    tmpElement = new Door(id, x, y, o, h, uniqueId);
                }
                else if (Enum.TryParse(id, true, out tmpLeverType))
                {
                    tmpElement = new Lever(id, x, y, o, h, uniqueId);
                }
                else if (Enum.TryParse(id, true, out tmpLockType))
                {
                    tmpElement = new Lock(id, x, y, o, h, uniqueId);
                }
                else if (Enum.TryParse(id, true, out tmpMonstertype))
                {
                    var newMonster = new Monster(id, x, y, o, h, uniqueId);
                    tmpCell.Monster = newMonster;
                    elements.Add(uniqueId, newMonster);
                }
                else if (Enum.TryParse(id, true, out tmpPressurePlateType))
                {
                    tmpElement = new PressurePlate(id, x, y, o, h, uniqueId);
                }
                else if (Enum.TryParse(id, true, out tmpTextType))
                {
                    tmpElement = new Text(id, x, y, o, h, uniqueId);
                }
                else if (Enum.TryParse(id, true, out tmpTrapDoorType))
                {
                    tmpElement = new TrapDoor(id, x, y, o, h, uniqueId);
                }
                else if (Enum.TryParse(id, true, out tmpLanternType))
                {
                    tmpElement = new Lantern(id, x, y, o, h, uniqueId);
                }
                else if (Enum.TryParse(id, true, out tmpAltarType))
                {
                    tmpElement = new Altar(id, x, y, o, h, uniqueId);
                }
                else if (Enum.TryParse(id, true, out tmpWallEffectType))
                {
                    tmpElement = new WallEffect(id, x, y, o, h, uniqueId);
                }
                else if(Enum.TryParse(id, true, out tmpWeaponType))
                {
                    tmpElement = new Weapon(id, x, y, o, h, uniqueId);
                }
                else
                {
                    tmpElement = new Item(id, x, y, o, h, uniqueId);
                }

                if (tmpElement != null)
                {
                    tmpCell.AddElement(tmpElement);
                    elements.Add(uniqueId, tmpElement);
                }

            }

            foreach (Match m in matchsAttribute)
            {
                Char[] delimit = delimitersAttributes;
                if (m.Value.Contains("addConnector"))
                {
                    delimit = delimitersAttributes2;
                }

                string[] split = m.Value.Split(delimit, StringSplitOptions.RemoveEmptyEntries);

                MapElement tmpElement = null;
                if (elements.TryGetValue(split[0], out tmpElement))
                {
                    if (split[2].Contains("addConnector"))
                    {
                        tmpElement.addConnector(split[3], split[4], split[5]);
                    }
                    else if (split[2].Contains("addItem") || split[2].Contains("setWallText") || split[2].Contains("setOpenedBy") || split[2].Contains("setState") || split[2].Contains("setDoorState") || split[2].Contains("setScrollText"))
                    {
                        tmpElement.setAttribute(split[2], split[3]);
                    }
                    else if (split[2].Contains("disable"))
                    {
                        tmpElement.setAttribute(split[2], "");
                    }
                    else // True or false
                    {
                        tmpElement.setAttribute(split[2], split[3].Contains("true") ? true : false);
                    }
                }
            }

            Map map = new Map(name, width, height)
            {
                StartPoint = startingPoint,
                EndPointList = endingPoints,
                AmbientTrack = ambientTrack,
                LevelCoord = levelCoord,
                Cells = cells,
                Tiles = new ArrayList(tiles), //FIXME
                Elements = elements,
            };

            CurrentMap = map;

            return map;
        }

        internal static Map MapFromChromosome(Map originalMap, Chromosome solution)
        {
            Map mapObject = originalMap.CloneJson() as Map;
            string binaryString = solution.ToBinaryString();

            List<Cell> cells = mapObject.SpawnCells;

            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].Monster = null;
                cells[i].RemoveElement("cudgel");
                cells[i].RemoveElement("machete");
                cells[i].RemoveElement("rapier");
                cells[i].RemoveElement("battle_axe");
                cells[i].RemoveElement("potion_healing");
                cells[i].RemoveElement("borra");
                cells[i].RemoveElement("bread");
                cells[i].RemoveElement("peasant_cap");
                cells[i].RemoveElement("peasant_breeches");
                cells[i].RemoveElement("peasant_tunic");
                cells[i].RemoveElement("leather_cap");
                cells[i].RemoveElement("leather_brigandine");
                cells[i].RemoveElement("leather_pants");
                cells[i].RemoveElement("leather_boots");

                string s = binaryString.Substring(i * NUMBER_GENES, NUMBER_GENES);
                int j = Convert.ToInt32(s, 2);

                if (j == 1 || j == 2  || j == 3 || j == 4 || j == 5 || j == 6 || j == 7 || j == 8 || j == 9 || j == 10 || j == 11)
                {
                    //Turtle
                    cells[i].Monster = new Monster("turtle", cells[i].X, cells[i].Y, 0, 0, "turtle_" + i);
                }
                else if (j == 12 || j == 13 || j == 14 || j == 15 || j == 16 || j == 17 || j == 18 || j == 19 || j == 20 || j == 21 || j == 22)
                {
                    //Mummy
                    cells[i].Monster = new Monster("mummy", cells[i].X, cells[i].Y, 0, 0, "mummy_" + i);
                }
                else if (j == 23 || j == 24 || j == 25 || j == 26 || j == 27 || j == 28 || j == 29 || j == 30 || j == 31 || j == 32)
                {
                    //Skeleton
                    cells[i].Monster = new Monster("skeleton_trooper", cells[i].X, cells[i].Y, 0, 0, "skeleton_trooper_" + i);
                }
                else if (j == 33 || j == 34 || j == 35)
                {
                    //Cudgel
                    cells[i].AddElement(new Weapon("cudgel", cells[i].X, cells[i].Y, 0, 0, "cudgel_" + i));
                }
                else if (j == 36 || j == 37 || j == 38)
                {
                    //Machete
                    cells[i].AddElement(new Weapon("machete", cells[i].X, cells[i].Y, 0, 0, "machete_" + i));
                }
                else if (j == 39 || j == 40 || j == 41)
                {
                    //Rapier
                    cells[i].AddElement(new Weapon("rapier", cells[i].X, cells[i].Y, 0, 0, "rapier_" + i));
                }
                else if (j == 42 || j == 43 || j == 44)
                {
                    //Battle Axe
                    cells[i].AddElement(new Weapon("battle_axe", cells[i].X, cells[i].Y, 0, 0, "battle_axe_" + i));
                }
                else if (j == 45 || j == 46 || j == 47 || j == 48)
                {
                    //Potion
                    cells[i].AddElement(new Item("potion_healing", cells[i].X, cells[i].Y, 0, 0, "potion_healing_" + i));
                }
                else if (j == 49 || j == 50 || j == 51)
                {
                    //Borra
                    cells[i].AddElement(new Item("borra", cells[i].X, cells[i].Y, 0, 0, "borra_" + i));
                }
                else if (j == 52 || j == 53 || j == 54)
                {
                    //Bread
                    cells[i].AddElement(new Item("bread", cells[i].X, cells[i].Y, 0, 0, "bread_" + i));
                }
                else if (j == 55 || j == 56)
                {
                    //Peasant cap
                    cells[i].AddElement(new Item("peasant_cap", cells[i].X, cells[i].Y, 0, 0, "peasant_cap_" + i));
                }
                else if (j == 57 || j == 58)
                {
                    //Peasant breeches
                    cells[i].AddElement(new Item("peasant_breeches", cells[i].X, cells[i].Y, 0, 0, "peasant_breeches_" + i));
                }
                else if (j == 59)
                {
                    //Peasant tunic
                    cells[i].AddElement(new Item("peasant_tunic", cells[i].X, cells[i].Y, 0, 0, "peasant_tunic_" + i));
                }
                else if (j == 60)
                {
                    //Sandals
                    cells[i].AddElement(new Item("sandals", cells[i].X, cells[i].Y, 0, 0, "sandals_" + i));
                }
                else if (j == 61)
                {
                    //Leather cap
                    cells[i].AddElement(new Item("leather_cap", cells[i].X, cells[i].Y, 0, 0, "leather_cap_" + i));
                }
                else if (j == 62)
                {
                    //Leather brigandine
                    cells[i].AddElement(new Item("leather_brigandine", cells[i].X, cells[i].Y, 0, 0, "leather_brigandine_" + i));
                }
                else if (j == 63)
                {
                    //Leather pants
                    cells[i].AddElement(new Item("leather_pants", cells[i].X, cells[i].Y, 0, 0, "leather_pants_" + i));
                }
                else if (j == 64)
                {
                    //Leather boots
                    cells[i].AddElement(new Item("leather_boots", cells[i].X, cells[i].Y, 0, 0, "leather_boots_" + i));
                }
                else
                {
                    //Nothing
                }
            }

            ReloadElementsMap(mapObject);
            
            return mapObject;
        }

        internal static Map MapFromChromosome(Map originalMap, Chromosome solution, List<Point> selectedPoints)
        {
            Map newMap = MapFromChromosome(originalMap, solution);

            Map map = originalMap.CloneJson() as Map;

            foreach (Point p in selectedPoints)
            {
                map.SetCell((Cell)newMap.GetCellAt(p.X, p.Y).CloneJson());
            }

            ReloadElementsMap(map);

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

        private static string IntToBinaryString(int number)
        {
            const int mask = 1;
            var binary = string.Empty;
            while (number > 0)
            {
                // Logical AND the number and prepend it to the result string
                binary = (number & mask) + binary;
                number = number >> 1;
            }

            return binary;
        }

        public static Chromosome ChromosomeFromMap(Map map)
        {
            StringBuilder sb = new StringBuilder();

            List<Cell> cells = map.SpawnCells;

            foreach (Cell c in cells)
            {
                int i = 0;

                if (c.Monster == null)
                {
                    //Nothing 000
                }
                else if (c.Exists("turtle"))
                {
                    i = 1; 
                }
                else if (c.Exists("mummy"))
                {
                    i = 12;
                }
                else if (c.Exists("skeleton_trooper"))
                {
                    i = 23; 
                }

                //------ Check weapons ------
                if (c.Exists("cudgel"))
                {
                    i = 33;
                }
                else if (c.Exists("machete"))
                {
                    i = 36;
                }
                else if(c.Exists("rapier"))
                {
                    i = 39;
                }
                else if (c.Exists("battle_axe"))
                {
                    i = 42;
                }

                //------ Check Resources ------
                if (c.Exists("potion_healing"))
                {
                    i = 45;
                }
                else if (c.Exists("borra"))
                {
                    i = 49;
                }
                else if (c.Exists("bread"))
                {
                    i = 52;
                }

                //------ Check Armor ------
                if (c.Exists("peasant_cap"))
                {
                    i = 55;
                }
                else if (c.Exists("peasant_breeches"))
                {
                    i = 57;
                }
                else if (c.Exists("peasant_tunic"))
                {
                    i = 59;
                }
                else if(c.Exists("sandals"))
                {
                    i = 60;
                }
                
                else if (c.Exists("leather_cap"))
                {
                    i = 61;
                }
                else if (c.Exists("leather_brigandine"))
                {
                    i = 62;
                }
                else if (c.Exists("leather_pants"))
                {
                    i = 63;
                }
                else if (c.Exists("leather_boots"))
                {
                    i = 64;
                }

                string s = IntToBinaryString(i).PadLeft(NUMBER_GENES, '0');
                sb.Append(s);
            }
            
            return new Chromosome(sb.ToString());
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
        /// <param name="currenMap"></param>
        /// <param name="selectedPoints"></param>
        public static bool ExportSelection(Map currenMap, Map originalMap, List<Point> selectedPoints)
        {
            if (currenMap == null) throw new CurrentMapNullException();

            Map map = originalMap.CloneJson() as Map;

            foreach(Point p in selectedPoints)
            {
                map.SetCell((Cell)currenMap.GetCellAt(p.X, p.Y).CloneJson());
            }

            ReloadElementsMap(map);

            return SaveMapFile(map);
        }

        private static void ReloadElementsMap(Map map)
        {
            map.Elements.Clear();

            map.Elements.Add(map.StartPoint.uniqueID, map.StartPoint);

            foreach (EndingPoint e in map.EndPointList)
            {
                map.Elements.Add(e.uniqueID, e);
            }
            //int itemNumber = 1;
            foreach (Cell c in map.Cells)
            {
                foreach (MapElement e in c.ElementsInCell)
                {
                    //e.uniqueID = e.ElementType + "_" + itemNumber++;
                    map.Elements.Add(e.uniqueID, e);
                }
                if (c.Monster != null) map.Elements.Add(c.Monster.uniqueID, c.Monster);
            }
        }

        /// <summary>
        /// Saves the Map object passed as an argument to a .lua file, interpretable by the LoG2 game
        /// </summary>
        /// <param name="mapToSave"></param>
        /// <returns></returns>
        public static bool SaveMapFile(Map mapToSave) //TODO: Change the temp dir to the final dir
        {
            _emergencyRestoreMap = CurrentMap.CloneJson();

            if (MainForm._lockedCellList != null && MainForm._lockedCellList.Count > 0)
            {
                foreach (var c in MainForm._lockedCellList)
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
                for (int y = 0; y < mapToSave.Height; y++)
                {
                    sb.Append("\t");
                    for (int x = 0; x < mapToSave.Width; x++)
                    {
                        sb.Append(mapToSave.Cells[y * mapToSave.Width + x].CellType + ",");
                        if (x == mapToSave.Width - 1)
                            sb.Append("\n");
                    }
                }

                sb.AppendLine("})");
                
                ListQueue<MapElement> fifo = new ListQueue<MapElement>(mapToSave.Elements.Values);
                while(fifo.Count != 0)
                {
                    MapElement el = fifo.Dequeue();
                    sb.Append(el.Print(fifo));
                }

                //start and ending points
                /*if (mapToSave.StartPoint != null)
                {
                    sb.Append(mapToSave.StartPoint.PrintElement());
                }
                if (mapToSave.EndPointList != null && mapToSave.EndPointList.Count > 0)
                    foreach (var e in mapToSave.EndPointList)
                    {
                        sb.Append(e.PrintElement());
                    }

                foreach (Cell c in mapToSave.Cells)
                {
                    foreach (MapElement el in c.ElementsInCell)
                    {
                        sb.Append(el.PrintElement());
                    }
                }*/

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

                Debug.WriteLine("Msg: " + e.Message);
                Debug.WriteLine("Inner: " + e.InnerException);
                Debug.WriteLine("Base: " + e.GetBaseException());
                return false;
            }

            return result;
        }


    }
}
