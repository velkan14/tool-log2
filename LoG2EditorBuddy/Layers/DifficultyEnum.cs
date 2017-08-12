using Povoater.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Povoater.Layers
{
    public enum RoomDifficulty
    {
        [StringValue("Cordless Power Drill")] Hard = 0,
        [StringValue("Cordless Power ")] Medium = 1,
        [StringValue("Cordless")] Safe = 2
    }

    public enum ItemAccessibility
    {
        HardToGet, SafeToGet
    }
   
}
