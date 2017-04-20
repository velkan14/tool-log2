using Log2CyclePrototype.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Log2CyclePrototype.LoG2API.Elements
{

    /// <summary>
    /// Represents a Lever object when parsed from a dungeon file
    /// </summary>
    public class Lever : MapElement
    {
        /// <summary>
        /// State of the lever
        /// </summary>
        public enum State
        {
            activated,
            deactivated
        }

        /// <summary>
        /// Type of action needed do trigger the lever 
        /// </summary>
        public enum Action
        {
            onActivate,
            onDeactivate,
            onToggle
        }

        /// <summary>
        /// Type of reaction when lever is triggered
        /// </summary>
        public enum Reaction
        {
            open,
            close,
            toggle
        }

        /// <summary>
        /// Type of the lever
        /// </summary>
        public enum LeverType
        {
            //doors, torch, lever, etc...
            beach_lever,
            castle_wall_lever,
            door_lever,
            lever,
            lever_ruins,
            mine_lever,
            mine_lever_corner,
            mine_secret_lever,
            tomb_wall_lever          
        }

        public LeverType _type;
        private State _state;
        private string _connectedTo;
        public bool _disableSelf;
        private Action _action;
        private Reaction _reaction;

        /// <summary>
        /// Other MapElement the lever is connected to
        /// </summary>
        public string ConnectedTo
        {
            get { return _connectedTo; }
            set { _connectedTo = value; }
        }

        /// <summary>
        /// Set if the lever should disable self after being triggered (meaning it will only work once)
        /// </summary>
        public bool DisableSelf
        {
            get { return _disableSelf; }
            set { _disableSelf = value; }
        }

        /// <summary>
        /// Internal use of MapElement type
        /// </summary>
        public override string ElementType
        {
            get { return "LEVER"; }
        }

        protected override string ConnectorName
        {
            get
            {
                return "lever";
            }
        }

        /// <summary>
        /// Generic lever object constructor
        /// </summary>
        /// <param name="type"> Lever type - Ingame value </param>
        /// <param name="x"> Object x coord </param>
        /// <param name="y"> Object y coord </param>
        /// <param name="orientation"> Where the object is facing </param>
        /// <param name="h"> Level where the object is placed </param>
        /// <param name="uniqueID"> Onject unique ID, customizable </param>
        /// <param name="state"> Initial state of the object </param>
        /// <param name="disableSelf"> True if the lever should only work once, false if otherwise </param>
        public Lever(string type, int x, int y, int orientation, int h, string uniqueID) : base(x,y,orientation,h,uniqueID)
        {
            Enum.TryParse(type, true, out _type);
            //Enum.TryParse(state, true, out _state);
            _disableSelf = false; //true = works only once
            _state = State.deactivated;
        }


        /// <summary>
        /// Method for setting an action to be executed by the lever
        /// </summary>
        /// <param name="action"> Whant is performed on the lever object</param>
        /// <param name="connectedTo"> Which door obect it will affect </param>
        /// <param name="reaction"> In what way the door object will be affected </param>
        public void SetConnector(string action, string connectedTo, string reaction)
        {
            Enum.TryParse(action, true, out _action);
            Enum.TryParse(reaction, true, out _reaction);
            //_action = action;
            _connectedTo = connectedTo;
            //_reaction = reaction;
        }

        /// <summary>
        /// Sets the state of the lever after parsing it from the dungeon file
        /// </summary>
        /// <param name="state"></param>
        public void SetState(string state)
        {
            Enum.TryParse(state, true, out _state);
        }

        /// <summary>
        /// Set the disable self flag
        /// </summary>
        /// <param name="value"></param>
        public void SetDisableSelf(string value)
        {
            _disableSelf = Convert.ToBoolean(value);
        }


        /// <summary>
        /// Returns the LUA representation of this Lever object
        /// </summary>
        /// <returns></returns>
        protected override string PrintElement(ListQueue<MapElement> elements)
        {
            //throw new NotImplementedException();

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"spawn(""{0}"",{1},{2},{3},{4},""{5}""){6}", _type, x, y, (int)orientation, h, uniqueID, '\n');
            switch (_state)
            {
                case 0:
                    sb.AppendFormat(@"{0}.lever:setState({1}){2}", uniqueID, _state.ToString().ToLower(), '\n');
                    break;
            }
            sb.AppendFormat(@"{0}.lever:setDisableSelf({1}){2}", uniqueID,DisableSelf.ToString().ToLower(), '\n');
            if (_connectedTo != null)
                sb.AppendFormat(@"{0}.lever:addConnector(""{1}"", ""{2}"", ""{3}""){4}", uniqueID, _action, _connectedTo, _reaction, '\n');

            return sb.ToString();
            //spawn("beach_lever",16,14,0,0,"beach_lever_1")
            //beach_lever_1.lever:setDisableSelf(false) //so funciona 1x
            //beach_lever_1.lever:addConnector("onToggle", "beach_door_portcullis_1", "toggle")
        }

        private static Rectangle srcRectTop = new Rectangle(80, 20, 20, 20);
        private static Rectangle srcRectRight = new Rectangle(100, 20, 20, 20);
        private static Rectangle srcRectDown = new Rectangle(120, 20, 20, 20);
        private static Rectangle srcRectLeft = new Rectangle(140, 20, 20, 20);


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
                return 0.3f;
            }
        }

        public override void setAttribute(string name, string value)
        {
            throw new NotImplementedException();
        }

        public override void setAttribute(string name, bool value)
        {
            if (name.Contains("setDisableSelf"))
            {
                DisableSelf = value;
            }
        }

     
    }
}
