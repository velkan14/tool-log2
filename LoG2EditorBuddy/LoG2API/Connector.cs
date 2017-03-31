using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2CyclePrototype.LoG2API.Elements
{
    public class Connector
    {
        public string Trigger {
            get; set; }
        public string Target
        {
            get; set;
        }
        public string Action
        {
            get; set;
        }

        public Connector(string trigger, string target, string action)
        {
            Trigger = trigger;
            Target = target;
            Action = action;
        }


    }
}
