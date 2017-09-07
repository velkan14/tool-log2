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
        public static int NUMBER_GENES = 6;

        public static Chromosome ChromosomeFromMap(Map map)
        {
            StringBuilder sb = new StringBuilder();

            List<Cell> cells = map.SpawnCells;
            
            foreach (Cell c in cells)
            {
                int type = 0;

                if (c.Monster == null)
                {
                    type = 18;
                }
                else if (c.Exists("turtle"))
                {
                    type = 0;
                }
                else if (c.Exists("mummy"))
                {
                    type = 3;
                }
                else if (c.Exists("skeleton_trooper"))
                {
                    type = 6;
                }

                //------ Check weapons ------
                if (c.Exists("rapier"))
                {
                    type = 9;
                }
                else if (c.Exists("battle_axe"))
                {
                    type = 10;
                }

                //------ Check Resources ------
                if (c.Exists("potion_healing"))
                {
                    type = 11;
                }
                else if (c.Exists("borra"))
                {
                    type = 12;
                }
                else if (c.Exists("bread"))
                {
                    type = 13;
                }

                //------ Check Armor ------
                if (c.Exists("leather_cap"))
                {
                    type = 14;
                }
                else if (c.Exists("leather_brigandine"))
                {
                    type = 15;
                }
                else if (c.Exists("leather_pants"))
                {
                    type = 16;
                }
                else if (c.Exists("leather_boots"))
                {
                    type = 17;
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

                if (j == 0 || j == 1 || j == 2)
                {
                    //Turtle
                    cells[i].Monster = new Monster("turtle", cells[i].X, cells[i].Y, 0, 0, "turtle_" + i);
                }
                else if (j == 3 || j == 4 || j == 5)
                {
                    //Mummy
                    cells[i].Monster = new Monster("mummy", cells[i].X, cells[i].Y, 0, 0, "mummy_" + i);
                }
                else if (j == 6 || j == 7 || j == 8)
                {
                    //Skeleton
                    cells[i].Monster = new Monster("skeleton_trooper", cells[i].X, cells[i].Y, 0, 0, "skeleton_trooper_" + i);
                }
                else if (j == 9)
                {
                    //Rapier
                    cells[i].AddElement(new Weapon("rapier", cells[i].X, cells[i].Y, 0, 0, "rapier_" + i));
                }
                else if (j == 10)
                {
                    //Battle Axe
                    cells[i].AddElement(new Weapon("battle_axe", cells[i].X, cells[i].Y, 0, 0, "battle_axe_" + i));
                }
                else if (j == 11)
                {
                    //Potion
                    cells[i].AddElement(new Potion("potion_healing", cells[i].X, cells[i].Y, 0, 0, "potion_healing_" + i));
                }
                else if (j == 12)
                {
                    //Borra
                    cells[i].AddElement(new Food("borra", cells[i].X, cells[i].Y, 0, 0, "borra_" + i));
                }
                else if (j == 13)
                {
                    //Bread
                    cells[i].AddElement(new Food("bread", cells[i].X, cells[i].Y, 0, 0, "bread_" + i));
                }
                else if (j == 14)
                {
                    //Leather cap
                    cells[i].AddElement(new Armor("leather_cap", cells[i].X, cells[i].Y, 0, 0, "leather_cap_" + i));
                }
                else if (j == 15)
                {
                    //Leather brigandine
                    cells[i].AddElement(new Armor("leather_brigandine", cells[i].X, cells[i].Y, 0, 0, "leather_brigandine_" + i));
                }
                else if (j == 16)
                {
                    //Leather pants
                    cells[i].AddElement(new Armor("leather_pants", cells[i].X, cells[i].Y, 0, 0, "leather_pants_" + i));
                }
                else if (j == 17)
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
