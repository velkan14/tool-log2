using Povoater.LoG2API.Elements;
using Povoater.Utilities;
using GAF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Povoater.LoG2API
{
    static class ChromosomeUtils
    {
        public static int NUMBER_GENES = 8;

        public static Chromosome ChromosomeFromMap(Map map)
        {
            StringBuilder sb = new StringBuilder();

            List<Cell> cells = map.SpawnCells;
            
            foreach (Cell c in cells)
            {
                int type = 0;

                if (c.Monster == null)
                {
                    //Nothing 000
                }
                else if (c.Exists("turtle"))
                {
                    type = 1;
                }
                else if (c.Exists("mummy"))
                {
                    type = 14;
                }
                else if (c.Exists("skeleton_trooper"))
                {
                    type = 27;
                }

                //------ Check weapons ------
                if (c.Exists("rapier"))
                {
                    type = 39;
                }
                else if (c.Exists("battle_axe"))
                {
                    type = 42;
                }

                //------ Check Resources ------
                if (c.Exists("potion_healing"))
                {
                    type = 45;
                }
                else if (c.Exists("borra"))
                {
                    type = 48;
                }
                else if (c.Exists("bread"))
                {
                    type = 51;
                }

                //------ Check Armor ------
                if (c.Exists("leather_cap"))
                {
                    type = 54;
                }
                else if (c.Exists("leather_brigandine"))
                {
                    type = 57;
                }
                else if (c.Exists("leather_pants"))
                {
                    type = 60;
                }
                else if (c.Exists("leather_boots"))
                {
                    type = 63;
                }

                string s = IntToBinaryString(type).PadLeft(NUMBER_GENES, '0');
                
                sb.Append(s);
            }
            return new Chromosome(sb.ToString());
        }

        internal static Map MapFromChromosome(Map originalMap, Chromosome solution)
        {
            Map mapObject = originalMap.CloneJson() as Map;
            string binaryString = solution.ToBinaryString();

            List<Cell> cells = mapObject.SpawnCells;
            
            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].Monster = null;
                cells[i].RemoveElement("rapier");
                cells[i].RemoveElement("battle_axe");
                cells[i].RemoveElement("potion_healing");
                cells[i].RemoveElement("borra");
                cells[i].RemoveElement("bread");
                cells[i].RemoveElement("leather_cap");
                cells[i].RemoveElement("leather_brigandine");
                cells[i].RemoveElement("leather_pants");
                cells[i].RemoveElement("leather_boots");

                string s = binaryString.Substring(i * NUMBER_GENES, NUMBER_GENES);

                int j = Convert.ToInt32(s, 2);

                if (j == 1 || j == 2 || j == 3 || j == 4 || j == 5 || j == 6 || j == 7 || j == 8 || j == 9 || j == 10 || j == 11 || j == 12 || j == 13)
                {
                    //Turtle
                    cells[i].Monster = new Monster("turtle", cells[i].X, cells[i].Y, 0, 0, "turtle_" + i);
                }
                else if (j == 14 || j == 15 || j == 16 || j == 17 || j == 18 || j == 19 || j == 20 || j == 21 || j == 22 || j == 23 || j == 24 || j == 25 || j == 26)
                {
                    //Mummy
                    cells[i].Monster = new Monster("mummy", cells[i].X, cells[i].Y, 0, 0, "mummy_" + i);
                }
                else if (j == 27 || j == 28 || j == 29 || j == 30 || j == 31 || j == 32 || j == 33 || j == 34 || j == 35 || j == 36 || j == 37 || j == 38)
                {
                    //Skeleton
                    cells[i].Monster = new Monster("skeleton_trooper", cells[i].X, cells[i].Y, 0, 0, "skeleton_trooper_" + i);
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
                else if (j == 45 || j == 46 || j == 47)
                {
                    //Potion
                    cells[i].AddElement(new Potion("potion_healing", cells[i].X, cells[i].Y, 0, 0, "potion_healing_" + i));
                }
                else if (j == 48 || j == 49 || j == 50)
                {
                    //Borra
                    cells[i].AddElement(new Food("borra", cells[i].X, cells[i].Y, 0, 0, "borra_" + i));
                }
                else if (j == 51 || j == 52 || j == 53)
                {
                    //Bread
                    cells[i].AddElement(new Food("bread", cells[i].X, cells[i].Y, 0, 0, "bread_" + i));
                }
                else if (j == 54 || j == 55 || j == 56)
                {
                    //Leather cap
                    cells[i].AddElement(new Armor("leather_cap", cells[i].X, cells[i].Y, 0, 0, "leather_cap_" + i));
                }
                else if (j == 57 || j == 58 || j == 59)
                {
                    //Leather brigandine
                    cells[i].AddElement(new Armor("leather_brigandine", cells[i].X, cells[i].Y, 0, 0, "leather_brigandine_" + i));
                }
                else if (j == 60 || j == 61 || j == 62)
                {
                    //Leather pants
                    cells[i].AddElement(new Armor("leather_pants", cells[i].X, cells[i].Y, 0, 0, "leather_pants_" + i));
                }
                else if (j == 63 || j == 64 || j == 65)
                {
                    //Leather boots
                    cells[i].AddElement(new Armor("leather_boots", cells[i].X, cells[i].Y, 0, 0, "leather_boots_" + i));
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

        internal static void ReloadElementsMap(Map map)
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
    }
}
