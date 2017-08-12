using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Povoater.WinAPI
{
    //TODO probability to screw up mouse speed if we close the program when holding the knob. but thats not gonna happen, is it? Kappa

    class MouseSpeedController
    {
        public const UInt32 SPI_SETMOUSESPEED = 0x0071;
        public const UInt32 SPIF_UPDATEINIFILE = 0x01;
        private const UInt32 SPIF_SENDWININICHANGE = 0x02;
        private static uint _currentMouseSpeed = 0;

        [DllImport("User32.dll")]
        static extern Boolean SystemParametersInfo(
            UInt32 uiAction,
            UInt32 uiParam,
            UInt32 pvParam,
            UInt32 fWinIni);

        public static void SetMouseSpeed(uint val){

            _currentMouseSpeed = (uint)SystemInformation.MouseSpeed;

            SystemParametersInfo(
                SPI_SETMOUSESPEED,
                0,
                val,
                0);
        }

        public static void ResetMouseSpeed()
        {
            SystemParametersInfo(
                SPI_SETMOUSESPEED,
                0,
                _currentMouseSpeed,
                0);
            }
    }
}
