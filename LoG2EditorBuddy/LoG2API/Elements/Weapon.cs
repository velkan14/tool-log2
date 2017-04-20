using Log2CyclePrototype.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype.LoG2API.Elements
{
    class Weapon : MapElement
    {
        public enum WeaponType {
            ancient_claymore,
            assassin_dagger,
            ax,
            backbiter,
            bane,
            baton,
            battle_axe,
            blowpipe,
            bone_blade,
            bone_club,
            boulder,
            branch,
            crossbow,
            cudgel,
            cutlass,
            dagger,
            dart,
            ethereal_blade,
            fire_blade,
            fire_bomb,
            fist_dagger,
            flail,
            frost_bomb,
            great_axe,
            hand_axe,
            legionary_spear,
            lightning_bow,
            lightning_rod,
            long_sword,
            longbow,
            machete,
            maul,
            meteor_hammer,
            moonblade,
            morning_star,
            pickaxe,
            poison_bomb,
            poleaxe,
            quarterstaff,
            rapier,
            rock,
            sabre,
            scythe,
            serpent_blade,
            shock_bomb,
            short_bow,
            shuriken,
            sickle_sword,
            skullcleave,
            sleep_dart,
            sling,
            spiked_club,
            throwing_axe,
            throwing_knife,
            torch,
            torch_everburning,
            tribal_spear,
            two_handed_word,
            venom_edge,
            venomfang_pick,
            vilson_orb,
            warhammer,
            zarchton_harpoon
        };

        public WeaponType type;

        public Weapon(string type, int x, int y, int orientation, int h, string uniqueID) : base(x,y,orientation,h,uniqueID)
        {
            Enum.TryParse(type, true, out this.type);
        }

        public override string ElementType { get { return type.ToString(); } }

        protected override string ConnectorName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private static Rectangle srcRectTop = new Rectangle(0, 60, 20, 20);
        private static Rectangle srcRectRight = new Rectangle(20, 60, 20, 20);
        private static Rectangle srcRectDown = new Rectangle(40, 60, 20, 20);
        private static Rectangle srcRectLeft = new Rectangle(60, 60, 20, 20);

        protected override Rectangle RectTop
        {
            get
            {
                return srcRectTop;
            }
        }

        protected override Rectangle RectRight
        {
            get
            {
                return srcRectRight;
            }
        }

        protected override Rectangle RectDown
        {
            get
            {
                return srcRectDown;
            }
        }

        protected override Rectangle RectLeft
        {
            get
            {
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

        protected override string PrintElement(ListQueue<MapElement> elements)
        {
            return String.Format(@"spawn(""{0}"",{1},{2},{3},{4},""{5}""){6}", ElementType, x, y, (int)orientation, h, uniqueID, '\n');
        }

        public override void setAttribute(string name, bool value)
        {
            throw new NotImplementedException();
        }

        public override void setAttribute(string name, string value)
        {
            throw new NotImplementedException();
        }
    }
}
