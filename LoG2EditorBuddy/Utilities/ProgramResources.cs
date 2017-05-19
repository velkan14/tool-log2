using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorBuddyMonster.Utilities
{


    public static class ProgramInfo{
        public const string DevelopmentYear = "2016";
        public const string ProgrammerName = "Daniel Amado";
        public const string ProfQueDeuBecaDeCarry = "Carlos Martinho";
        public const string ProgramName = "LoG2 Editor Buddy";        
    }

    public static class StringResources
    {


        public const string ControlHelpString = 
            "Description:\n\n"+

            "Legend of Grimrock 2 Editor Buddy is an Evolutionary Algorithm\n"+
            "driven program whose main objective it to assist the user in the\n"+
            "creative process of levels building playable levels for the \n"+
            "Legend of Grimrock 2 game using it's native Level Editor.\n\n\n"+


            "Program Usage:\n\n"+

            "After starting the Legend of Grimrock 2 Level Editor and setting\n"+
            "your project, you can execute the LoG2 Editor Buddy program and\n"+
            "select your project directory in File > Select project"+
            "After selecting your project dir, you can start building your\n"+
            "level with the dafault settings and the program should periodically\n"+
            "present potentially useful suggestions while you design your level.\n\n"+


            "Keys:\n\n"+

            "Shift+LeftClick the solution preview will let you select a portion\n"+
            "of the solution to a export to your current level."+
            "Ctrl+LeftClick the solution preview will remove the previously\n"+
            "created selection."+
            "RightClick the solution preview will allow you to Export or Clear\n"+
            "any selections you have made.";




        public const string CreditsHelpString =
            ProgramInfo.ProgramName + " created by " + ProgramInfo.ProgrammerName + ", " + ProgramInfo.DevelopmentYear + "\n" +
            "Special thanks to proffessor " + ProgramInfo.ProfQueDeuBecaDeCarry + " for guidance and suggestions.";

        public const string PickDirString = "Please pick a LoG2 project directory(containing a .dungeon_editor file).";

        public const string StartGrimrockString = "No instances of Legend of Grimrock 2 Editor found. Please start Legend of Grimrock 2.";


    }



}
