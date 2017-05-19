using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorBuddyMonster.Algorithm
{
    class HasStuff
    {
        public static bool HasItem(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            if (cell == null) return false;

            int j = cell.type;

            if (j == 33 || j == 34 || j == 35)
            {
                //Cudgel
                return true;
            }
            else if (j == 36 || j == 37 || j == 38)
            {
                //Machete
                return true;
            }
            else if (j == 39 || j == 40 || j == 41)
            {
                //Rapier
                return true;
            }
            else if (j == 42 || j == 43 || j == 44)
            {
                //Battle Axe
                return true;
            }
            else if (j == 45 || j == 46 || j == 47 || j == 48)
            {
                //Potion
                return true;
            }
            else if (j == 49 || j == 50 || j == 51)
            {
                //Borra
                return true;
            }
            else if (j == 52 || j == 53 || j == 54)
            {
                //Bread
                return true;
            }
            else if (j == 55 || j == 56)
            {
                //Peasant cap
                return true;
            }
            else if (j == 57 || j == 58)
            {
                //Peasant breeches
                return true;
            }
            else if (j == 59)
            {
                //Peasant tunic
                return true;
            }
            else if (j == 60)
            {
                //Sandals
                return true;
            }
            else if (j == 61)
            {
                //Leather cap
                return true;
            }
            else if (j == 62)
            {
                //Leather brigandine
                return true;
            }
            else if (j == 63)
            {
                //Leather pants
                return true;
            }
            else if (j == 64)
            {
                //Leather boots
                return true;
            }
            return false;
        }

        public static bool HasWeapon(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            if (cell == null) return false;

            int j = cell.type;

            if (j == 33 || j == 34 || j == 35)
            {
                //Cudgel
                return true;
            }
            else if (j == 36 || j == 37 || j == 38)
            {
                //Machete
                return true;
            }
            else if (j == 39 || j == 40 || j == 41)
            {
                //Rapier
                return true;
            }
            else if (j == 42 || j == 43 || j == 44)
            {
                //Battle Axe
                return true;
            }
            return false;
        }

        public static bool HasResource(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            if (cell == null) return false;

            int j = cell.type;

            if (j == 45 || j == 46 || j == 47 || j == 48)
            {
                //Potion
                return true;
            }
            else if (j == 49 || j == 50 || j == 51)
            {
                //Borra
                return true;
            }
            else if (j == 52 || j == 53 || j == 54)
            {
                //Bread
                return true;
            }
            return false;
        }

        public static bool HasArmor(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            if (cell == null) return false;

            int j = cell.type;

            if (j == 55 || j == 56)
            {
                //Peasant cap
                return true;
            }
            else if (j == 57 || j == 58)
            {
                //Peasant breeches
                return true;
            }
            else if (j == 59)
            {
                //Peasant tunic
                return true;
            }
            else if (j == 60)
            {
                //Sandals
                return true;
            }
            else if (j == 61)
            {
                //Leather cap
                return true;
            }
            else if (j == 62)
            {
                //Leather brigandine
                return true;
            }
            else if (j == 63)
            {
                //Leather pants
                return true;
            }
            else if (j == 64)
            {
                //Leather boots
                return true;
            }
            return false;
        }

        public static bool HasMonster(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            if (cell == null) return false;

            int j = cell.type;

            if (j == 1 || j == 2 || j == 3 || j == 4 || j == 5 || j == 6 || j == 7 || j == 8 || j == 9 || j == 10 || j == 11)
            {
                //Turtle
                return true;
            }
            else if (j == 12 || j == 13 || j == 14 || j == 15 || j == 16 || j == 17 || j == 18 || j == 19 || j == 20 || j == 21 || j == 22)
            {
                //Mummy
                return true;
            }
            else if (j == 23 || j == 24 || j == 25 || j == 26 || j == 27 || j == 28 || j == 29 || j == 30 || j == 31 || j == 32)
            {
                //Skeleton
                return true;
            }
            return false;
        }

        public static bool HasTurtle(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            int j = cell.type;

            if (j == 1 || j == 2 || j == 3 || j == 4 || j == 5 || j == 6 || j == 7 || j == 8 || j == 9 || j == 10 || j == 11)
            {
                //Turtle
                return true;
            }
            return false;
        }

        public static bool HasMummy(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            int j = cell.type;

            if (j == 12 || j == 13 || j == 14 || j == 15 || j == 16 || j == 17 || j == 18 || j == 19 || j == 20 || j == 21 || j == 22)
            {
                //Mummy
                return true;
            }
            return false;
        }

        public static bool HasSkeleton(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            int j = cell.type;

            if (j == 23 || j == 24 || j == 25 || j == 26 || j == 27 || j == 28 || j == 29 || j == 30 || j == 31 || j == 32)
            {
                //Skeleton
                return true;
            }
            return false;
        }
    }
}
