using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace EditorBuddyMonster.WinAPI
{
    public enum CursorType
    {
        Default,
        Plus,
        Minus,
        Select
    }

    public class CursorManager
    {
        private static Cursor PlusCursor, MinusCursor;

        private static CursorManager instance;

        private CursorManager()
        {
            //String[] resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            //foreach (string s in resourceNames)
            //    Debug.WriteLine(s);
            try
            {
                PlusCursor = new Cursor(new System.IO.MemoryStream(Properties.Resources.cursor_plus));
                MinusCursor = new Cursor(new System.IO.MemoryStream(Properties.Resources.cursor_minus));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        public static CursorManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CursorManager();
                }
                return instance;
            }
        }


        public void ResetCursor()
        {
            SetCursor(CursorType.Default);
        }

        public void SetCursor(CursorType t)
        {
            switch (t)
            {
                case CursorType.Default:
                    Cursor.Current = Cursors.Default;
                    break;
                case CursorType.Plus:
                    Cursor.Current = PlusCursor;
                    break;
                case CursorType.Minus:
                    Cursor.Current = MinusCursor;
                    break;
                case CursorType.Select:
                    Cursor.Current = Cursors.Cross;
                    break;
                default:
                    break;
            }
        }

    }
}
