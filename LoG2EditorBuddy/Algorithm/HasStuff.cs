using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorBuddyMonster.Algorithm
{
    class HasStuff
    {
        public static bool IsEmpty(CellStruct cell)
        {
            if(cell.type == 0 || cell.type > 65)
            {
                return true;
            }
            return false;
        }

        public static bool HasItem(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            if (cell == null) return false;

            return HasItem(cell);
        }

        public static bool HasItem(CellStruct cell)
        {
            int j = cell.type;
            if (j == 39 || j == 40 || j == 41)
            {
                //Rapier
                return true;
            }
            else if (j == 42 || j == 43 || j == 44)
            {
                //Battle Axe
                return true;
            }
            else if (j == 45 || j == 46 || j == 47)
            {
                //Potion
                return true;
            }
            else if (j == 48 || j == 49 || j == 50)
            {
                //Borra
                return true;
            }
            else if (j == 51 || j == 52 || j == 53)
            {
                //Bread
                return true;
            }
            else if (j == 54 || j == 55 || j == 56)
            {
                //Leather cap
                return true;
            }
            else if (j == 57 || j == 58 || j == 59)
            {
                //Leather brigandine
                return true;
            }
            else if (j == 60 || j == 61 || j == 62)
            {
                //Leather pants
                return true;
            }
            else if (j == 63 || j == 64 || j == 65)
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

            return HasWeapon(cell);
        }

        public static bool HasWeapon(CellStruct cell)
        {
            int j = cell.type;

            if (j == 39 || j == 40 || j == 41)
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

            return HasResource(cell);
        }

        public static bool HasResource(CellStruct cell)
        {
            int j = cell.type;

            if (j == 45 || j == 46 || j == 47)
            {
                //Potion
                return true;
            }
            else if (j == 48 || j == 49 || j == 50)
            {
                //Borra
                return true;
            }
            else if (j == 51 || j == 52 || j == 53)
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

            return HasArmor(cell);
        }

        public static bool HasArmor(CellStruct cell)
        {
            int j = cell.type;

            if (j == 54 || j == 55 || j == 56)
            {
                //Leather cap
                return true;
            }
            else if (j == 57 || j == 58 || j == 59)
            {
                //Leather brigandine
                return true;
            }
            else if (j == 60 || j == 61 || j == 62)
            {
                //Leather pants
                return true;
            }
            else if (j == 63 || j == 64 || j == 65)
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

            return HasMonster(cell);
        }

        public static bool HasMonster(CellStruct cell)
        {
            int j = cell.type;

            if (j == 1 || j == 2 || j == 3 || j == 4 || j == 5 || j == 6 || j == 7 || j == 8 || j == 9 || j == 10 || j == 11 || j == 12 || j == 13)
            {
                //Turtle
                return true;
            }
            else if (j == 14 || j == 15 || j == 16 || j == 17 || j == 18 || j == 19 || j == 20 || j == 21 || j == 22 || j == 23 || j == 24 || j == 25 || j == 26)
            {
                //Mummy
                return true;
            }
            else if (j == 27 || j == 28 || j == 29 || j == 30 || j == 31 || j == 32 || j == 33 || j == 34 || j == 35 || j == 36 || j == 37 || j == 38)
            {
                //Skeleton
                return true;
            }
            return false;
        }

        public static bool HasTurtle(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            return HasTurtle(cell);
        }
        public static bool HasTurtle(CellStruct cell)
        {
            int j = cell.type;

            if (j == 1 || j == 2 || j == 3 || j == 4 || j == 5 || j == 6 || j == 7 || j == 8 || j == 9 || j == 10 || j == 11 || j == 12 || j == 13)
            {
                //Turtle
                return true;
            }
            return false;
        }

        public static bool HasMummy(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            return HasMummy(cell);
        }

        public static bool HasMummy(CellStruct cell)
        {
            int j = cell.type;

            if (j == 14 || j == 15 || j == 16 || j == 17 || j == 18 || j == 19 || j == 20 || j == 21 || j == 22 || j == 23 || j == 24 || j == 25 || j == 26)
            {
                //Mummy
                return true;
            }
            return false;
        }

        public static bool HasSkeleton(int x, int y, List<CellStruct> listCells)
        {
            CellStruct cell = listCells.FirstOrDefault(c => c.x == x && c.y == y);

            return HasSkeleton(cell);
        }

        public static bool HasSkeleton(CellStruct cell)
        {
            int j = cell.type;

            if (j == 27 || j == 28 || j == 29 || j == 30 || j == 31 || j == 32 || j == 33 || j == 34 || j == 35 || j == 36 || j == 37 || j == 38)
            {
                //Skeleton
                return true;
            }
            return false;
        }
    }
}
