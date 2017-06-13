﻿using EditorBuddyMonster.LoG2API.Elements;
using EditorBuddyMonster.Utilities;
using GAF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorBuddyMonster.LoG2API
{
    static class ChromosomeUtils
    {
        public static int NUMBER_GENES = 8;
        public static int NUMBER_GENES_ID = 10; //We have 1024 tiles max
        public static int NUMBER_GENES_TOTAL { get { return NUMBER_GENES + NUMBER_GENES_ID; } }

        public static Chromosome ChromosomeFromMap(Map map)
        {
            return ChromosomeFromMap(map, false);
        }

        public static Chromosome ChromosomeFromMap(Map map, bool withID)
        {
            StringBuilder sb = new StringBuilder();

            List<Cell> cells = map.SpawnCells;

            int id = 0;
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
                    type = 12;
                }
                else if (c.Exists("skeleton_trooper"))
                {
                    type = 23;
                }

                //------ Check weapons ------
                if (c.Exists("cudgel"))
                {
                    type = 33;
                }
                else if (c.Exists("machete"))
                {
                    type = 36;
                }
                else if (c.Exists("rapier"))
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
                    type = 49;
                }
                else if (c.Exists("bread"))
                {
                    type = 52;
                }

                //------ Check Armor ------
                if (c.Exists("peasant_cap"))
                {
                    type = 55;
                }
                else if (c.Exists("peasant_breeches"))
                {
                    type = 57;
                }
                else if (c.Exists("peasant_tunic"))
                {
                    type = 59;
                }
                else if (c.Exists("sandals"))
                {
                    type = 60;
                }

                else if (c.Exists("leather_cap"))
                {
                    type = 61;
                }
                else if (c.Exists("leather_brigandine"))
                {
                    type = 62;
                }
                else if (c.Exists("leather_pants"))
                {
                    type = 63;
                }
                else if (c.Exists("leather_boots"))
                {
                    type = 64;
                }

                string s = IntToBinaryString(type).PadLeft(NUMBER_GENES, '0');

                if (withID)
                {
                    string idString = IntToBinaryString(id).PadLeft(NUMBER_GENES_ID, '0');
                    sb.Append(idString);
                    id++;
                }
                
                sb.Append(s);
            }
            if (withID)  Console.WriteLine(sb.ToString());
            return new Chromosome(sb.ToString());
        }

        internal static Map MapFromChromosome(Map originalMap, Chromosome solution)
        {
            Map mapObject = originalMap.CloneJson() as Map;
            string binaryString = solution.ToBinaryString();

            List<Cell> cells = mapObject.SpawnCells;

            bool withId = false;
            if(solution.Genes.Count == NUMBER_GENES_TOTAL * cells.Count)
            {
                withId = true;
            }
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

                string s = "";
                if (withId)
                {
                    s = binaryString.Substring(i * NUMBER_GENES_TOTAL + NUMBER_GENES_ID, NUMBER_GENES);
                }
                else
                {
                    s = binaryString.Substring(i * NUMBER_GENES, NUMBER_GENES);
                }
                int j = Convert.ToInt32(s, 2);

                if (j == 1 || j == 2 || j == 3 || j == 4 || j == 5 || j == 6 || j == 7 || j == 8 || j == 9 || j == 10 || j == 11)
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
