using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype
{
    public class Monster : MapElement
    {
        /// <summary>
        /// Type of the lever
        /// </summary>
        public enum MonsterType
        {
            air_elemental,
            crab,
            crowern,
            dark_acolyte,
            dark_acolyte_pair,
            eyctopus,
            fire_elemental,
            fjeld_warg,
            fjeld_warg1,
            fjeld_warg2,
            fjeld_warg_pair,
            forest_ogre,
            giant_snake,
            green_slime,
            herder,
            herder_big,
            herder_small,
            herder_swarm,
            herder_twigroot_pair,
            ice_guardian,
            lindworm,
            lindworm_cinematic_flyby,
            lindworm_cinematic_tower,
            magma_golem,
            medusa,
            medusa_pair,
            mimic,
            mosquito_swarm,
            mummy, 
            mummy1,
            mummy2,
            mummy3,
            mummy_patrol,
            rat_swarm,
            ratling1,
            ratling2,
            ratling3,
            ratling_boss,
            ratling_pair,
            ratling_warg_pair,
            sand_warg,
            shrakk_torr,
            skeleton_archer,
            skeleton_archer_patrol,
            skeleton_commander,
            skeleton_trooper,
            skeleton_trooper1,
            skeleton_trooper2,
            skeleton_trooper_pair,
            spider,
            spider_walker,
            spider_ambush,
            spore_mushroom,
            summon_stone,
            summon_stone_dormant,
            swamp_toad,
            trickster,
            turtle,
            turtle_diving,
            twigroot,
            twigroot_dormant,
            twigroot_pair,
            uggardian,
            undead,
            viper_root,
            wizard,
            wizard_clone,
            wizard_scriptable,
            wyvern,
            xeloroid,
            zarchton,
            zarchton_ambush,
            zarchton_diving,
            zarchton_pair
        }

        public MonsterType type;

        public override string ElementType
        {
            get
            {
                return type.ToString();
            }
        }

        public Monster(string type, int x, int y, int orientation, int h, string uniqueID) : base(x,y,orientation,h,uniqueID)
        {
            Enum.TryParse(type, true, out this.type);
        }

        public override string PrintElement()
        {
            /*StringBuilder sb = new StringBuilder();

            sb.AppendLine(String.Format(@"spawn(""{0}"",{1},{2},{3},{4},""{5}"")", monsterType, x, y, (int)orientation, h, uniqueID));

            return sb.ToString();*/
            return String.Format(@"spawn(""{0}"",{1},{2},{3},{4},""{5}"")", type, x, y, (int)orientation, h, uniqueID);
        }

        private static Image imageMonster = new Bitmap("../../monster.png");

        public override void Draw(Graphics panel, int cellWidth, int cellHeight)
        {
            switch (orientation)
            {
                case MapElement.Orientation.Top:
                    panel.DrawImage(imageMonster, x * cellWidth, y * cellHeight, cellWidth, cellHeight);
                    break;
                case MapElement.Orientation.Right:
                    imageMonster.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    panel.DrawImage(imageMonster, x * cellWidth, y * cellHeight, cellWidth, cellHeight);
                    imageMonster.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case MapElement.Orientation.Down:
                    imageMonster.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    panel.DrawImage(imageMonster, x * cellWidth, y * cellHeight, cellWidth, cellHeight);
                    imageMonster.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case MapElement.Orientation.Left:
                    imageMonster.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    panel.DrawImage(imageMonster, x * cellWidth, y * cellHeight, cellWidth, cellHeight);
                    imageMonster.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
            }
        }
    }
}
