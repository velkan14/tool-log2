using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorBuddyMonster
{
    /// <summary>
    /// Logger class handles raised events
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Provides a custom EventHandler
        /// </summary>
        public static event EventHandler<LogEntryEventArgs> EntryWritten;

        /// <summary>
        /// Append text method sends new Event message
        /// </summary>
        /// <param name="text"></param>
        public static void AppendText(string text)
        {
            DateTime now = DateTime.Now;

            var tmp = EntryWritten;
            if (tmp != null)
                tmp(null, new LogEntryEventArgs("[" + now.Hour.ToString() + ":"+ now.Minute.ToString()+":"+ now.Second.ToString() + "] "+text)); //sender, EventArgs (Args hold message)
        }
    }


    /// <summary>
    /// Holds the event message
    /// </summary>
    public class LogEntryEventArgs : EventArgs
    {
        private readonly String message;

        /// <summary>
        /// Stores message
        /// </summary>
        /// <param name="pMessage"></param>
        public LogEntryEventArgs(String pMessage)
        {
            message = pMessage;
        }

        /// <summary>
        /// Local message string
        /// </summary>
        public String Message
        {
            get { return message; }
        }
    }

}
