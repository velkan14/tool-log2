using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Povoater.Algorithm
{
    class HasStuff
    {
        public static bool IsEmpty(CellStruct cell)
        {
            if(cell.type > 17)
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
            if (j == 9)
            {
                //Rapier
                return true;
            }
            else if (j == 10)
            {
                //Battle Axe
                return true;
            }
            else if (j == 11)
            {
                //Potion
                return true;
            }
            else if (j == 12)
            {
                //Borra
                return true;
            }
            else if (j == 13)
            {
                //Bread
                return true;
            }
            else if (j == 14)
            {
                //Leather cap
                return true;
            }
            else if (j == 15)
            {
                //Leather brigandine
                return true;
            }
            else if (j == 16)
            {
                //Leather pants
                return true;
            }
            else if (j == 17)
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

            if (j == 9)
            {
                //Rapier
                return true;
            }
            else if (j == 10)
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

            if (j == 11)
            {
                //Potion
                return true;
            }
            else if (j ==  12)
            {
                //Borra
                return true;
            }
            else if (j == 13)
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

            if (j == 14)
            {
                //Leather cap
                return true;
            }
            else if (j == 15)
            {
                //Leather brigandine
                return true;
            }
            else if (j == 16)
            {
                //Leather pants
                return true;
            }
            else if (j == 17)
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

            if (j == 0 || j == 1 || j == 2)
            {
                //Turtle
                return true;
            }
            else if (j == 3 || j == 4 || j == 5)
            {
                //Mummy
                return true;
            }
            else if (j == 6 || j == 7 || j == 8)
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

            if (j == 0 || j == 1 || j == 2)
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

            if (j == 3 || j == 4 || j == 5)
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

            if (j == 6 || j == 7 || j == 8)
            {
                //Skeleton
                return true;
            }
            return false;
        }
    }
}
