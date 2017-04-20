using Log2CyclePrototype.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Log2CyclePrototype.LoG2API.Elements
{
    /// <summary>
    /// Class representing Door element when parsed from a dungeon file
    /// </summary>
    public class Door : MapElement
    {

        /// <summary>
        /// Type of door 
        /// </summary>
        public enum DoorType
        {
            beach_door_portcullis,
            beach_door_wood,
            beach_secret_door,
            castle_door_portcullis,
            castle_door_wood,
            castle_entrance_door,
            castle_secret_door,
            castle_wall_grating,
            castle_wall_grating_ornament,
            dungeon_door_iron,
            dungeon_door_iron_barred,
            dungeon_door_portcullis,
            dungeon_door_stone,
            dungeon_door_wooden,
            dungeon_door_wooden_double,
            dungeon_iron_gate,
            dungeon_secret_door,
            dungeon_wall_grating,
            forest_ruins_secret_door,
            mine_door_camo,
            mine_door_heavy,
            mine_door_spear,
            mine_door_support,
            mine_moss_door_support,
            portal,
            portal_locked,
            prison_cage_door,
            prison_cage_door_breakable,
            tomb_door_portcullis,
            tomb_door_serpent,
            tomb_door_stone,
            tomb_iron_gate,
            tomb_secret_door,
            tomb_wall_grating
        }


        public DoorType type;

        /// <summary>
        /// Internal state. Represents if the door has a chain to open or close. 
        /// </summary>
        public bool PullChain{get; set;}

        /// <summary>
        /// Internal state. Represents if the door is open or closed.
        /// </summary>
        public bool Open{get; set;}

        /// <summary>
        /// Internal use of MapElement type
        /// </summary>
        public override string ElementType
        {
            get { return type.ToString(); }
        }

        protected override string ConnectorName
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// Door object representation
        /// </summary>
        /// <param name="type"> Type of door - ingame value </param>
        /// <param name="x"> Object x coord </param>
        /// <param name="y"> Object y coord </param>
        /// <param name="orientation"> Direction where the object is facing </param>
        /// <param name="h"> Level where the object is placed - default 0 </param>
        /// <param name="uniqueID"> Unique ID for the object </param>
        /// <param name="state"> Initial state of the object </param>
        public Door(string type, int x, int y, int orientation, int h, string uniqueID) : base(x, y, orientation, h, uniqueID)
        {
            Enum.TryParse(type, true, out this.type); //Check performance
            //Enum.TryParse(state, true, out _state);
            PullChain = false;
            Open = false;
        }


        /// <summary>
        /// Returns a LUA representation of this object
        /// </summary>
        /// <returns></returns>
        protected override string PrintElement(ListQueue<MapElement> elements)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"spawn(""{0}"",{1},{2},{3},{4},""{5}""){6}", type,x,y,(int)orientation,h,uniqueID, '\n');
            if(Open) sb.AppendFormat(@"{0}.door:setDoorState(""{1}""){2}", uniqueID, "open", '\n');
            sb.AppendFormat(@"{0}.door:setPullChain({1}){2}", uniqueID, PullChain ? "true" : "false", '\n');
            return sb.ToString();
        }

        private static Rectangle srcRectTop = new Rectangle(80, 300, 20, 20);
        private static Rectangle srcRectRight = new Rectangle(100, 300, 20, 20);
        private static Rectangle srcRectDown = new Rectangle(120, 300, 20, 20);
        private static Rectangle srcRectLeft = new Rectangle(140, 300, 20, 20);

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
                return true;
            }
        }

        protected override float Transparency
        {
            get
            {
                return 1.0f;
            }
        }

        public override void setAttribute(string name, string value)
        {
            if (name.Contains("setDoorState"))
            {
                Open = value.Contains("open") ? true : false;
            }
        }

        public override void setAttribute(string name, bool value)
        {
            if (name.Contains("setPullChain"))
            {
                PullChain = value;
            }
            else if (name.Contains("setState"))
            {
                Open = value;
            }
        }
        
    }
}
