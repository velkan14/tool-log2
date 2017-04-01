namespace Log2CyclePrototype.LoG2API
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
