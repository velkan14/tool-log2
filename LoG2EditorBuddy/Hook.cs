using gma.System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Povoater
{
    class Hook
    {
        private UserActivityHook activityHook;
        private Core core;

        public Hook(Core core)
        {
            this.core = core;
            activityHook = new UserActivityHook(true, false); // crate an instance with global hooks (mouse on / keyboard on)
            activityHook.OnMouseActivity += new MouseEventHandler(MouseMoved); // hang on events
        }

        public void Start()
        {
            activityHook.Start();
        }

        private void MouseMoved(object sender, MouseEventArgs e)
        {
            //labelMousePosition.Text = String.Format("x={0}  y={1} wheel={2}", e.X, e.Y, e.Delta);
            if (e.Clicks > 0)
            {
                //LogWrite("MouseButton 	- " + e.Button.ToString());
                //LogWrite(actHook.GetApplicationMouseIsOver()); //get app name on mouse click
                //LogWrite(actHook.sendKeystroke('a', actHook.GetHandleOfWindowMouseIsOver())); //send simple key
                if (activityHook.GetApplicationMouseIsOver() == "grimrock2.exe")
                {
                    if (core.HasMap && activityHook.LoG2Found)
                        activityHook.SendSaveCommand(activityHook.GetHandleOfWindowMouseIsOver());
                    
                }
            }
        }

        internal void ReloadLOG()
        {
            if(activityHook.LoG2Found) activityHook.SendReloadCommand();
            else
            {
                activityHook.Start();
                if (activityHook.LoG2Found) activityHook.SendReloadCommand();
            }
        }

        internal void ShowPovoater()
        {
            activityHook.ShowPovoater();
        }
    }
}
