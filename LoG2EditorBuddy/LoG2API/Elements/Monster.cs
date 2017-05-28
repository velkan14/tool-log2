using EditorBuddyMonster.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace EditorBuddyMonster.LoG2API.Elements
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

        private static Rectangle srcRectTop = new Rectangle(80, 0, 20, 20);
        private static Rectangle srcRectRight = new Rectangle(100, 0, 20, 20);
        private static Rectangle srcRectDown = new Rectangle(120, 0, 20, 20);
        private static Rectangle srcRectLeft = new Rectangle(140, 0, 20, 20);

        private static Rectangle srcRectTopLight = new Rectangle(0, 36*20, 20, 20);
        private static Rectangle srcRectRightLight = new Rectangle(20, 36 * 20, 20, 20);
        private static Rectangle srcRectDownLight = new Rectangle(40, 36 * 20, 20, 20);
        private static Rectangle srcRectLeftLight = new Rectangle(60, 36 * 20, 20, 20);

        private static Rectangle srcRectTopMedium = new Rectangle(80, 36 * 20, 20, 20);
        private static Rectangle srcRectRightMedium = new Rectangle(100, 36 * 20, 20, 20);
        private static Rectangle srcRectDownMedium = new Rectangle(120, 36 * 20, 20, 20);
        private static Rectangle srcRectLeftMedium = new Rectangle(140, 36 * 20, 20, 20);

        public override string ElementType
        {
            get
            {
                return type.ToString();
            }
        }

        protected override string ConnectorName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected override Rectangle RectTop
        {
            get
            {
                if (ElementType.Equals("turtle")) return srcRectTopMedium;
                return srcRectTop;
            }
        }

        protected override Rectangle RectRight
        {
            get
            {
                if (ElementType.Equals("turtle")) return srcRectRightMedium;
                return srcRectRight;
            }
        }

        protected override Rectangle RectDown
        {
            get
            {
                if (ElementType.Equals("turtle")) return srcRectDownMedium;
                return srcRectDown;
            }
        }

        protected override Rectangle RectLeft
        {
            get
            {
                if (ElementType.Equals("turtle")) return srcRectLeftMedium;
                return srcRectLeft;
            }
        }

        protected override bool UseOffset
        {
            get
            {
                return false;
            }
        }

        protected override float Transparency
        {
            get
            {
                return 1.0f;
            }
        }

        public Monster(string type, int x, int y, int orientation, int h, string uniqueID) : base(x,y,orientation,h,uniqueID)
        {
            Enum.TryParse(type, true, out this.type);
        }

        protected override string PrintElement(ListQueue<MapElement> elements)
        {
            /*StringBuilder sb = new StringBuilder();

            sb.AppendLine(String.Format(@"spawn(""{0}"",{1},{2},{3},{4},""{5}"")", monsterType, x, y, (int)orientation, h, uniqueID));

            return sb.ToString();*/
            return String.Format(@"spawn(""{0}"",{1},{2},{3},{4},""{5}""){6}", type, x, y, (int)orientation, h, uniqueID, '\n');
        }


        public override void setAttribute(string name, string value)
        {
            //Do Nothing
        }

        public override void setAttribute(string name, bool value)
        {
            //Do  Nothing
        }
    }
}
