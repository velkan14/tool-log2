using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log2CyclePrototype
{
    class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {

            //Application.Run(new MainForm());
            Application.Run(new Monsters());

        }
    }
}
