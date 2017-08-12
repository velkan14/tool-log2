using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Povoater.Exceptions
{
    /// <summary>
    /// Represents a Custom Exception used to treat attempts to access null CurrentMap prior to parsing.
    /// </summary>
    public class CurrentMapNullException : Exception
    {
        const string DefaultMessage = "APIClass CurrentMap is null. Make sure static method ParseMapFile() is being called.";
        /// <summary>
        /// Call the base Exception with a Default Message
        /// </summary>
        public CurrentMapNullException() : base(DefaultMessage) { }
        /// <summary>
        /// Call the base exception with a Custom Message
        /// </summary>
        /// <param name="message"></param>
        public CurrentMapNullException(string message) : base(message) { }
        /// <summary>
        /// Call the base exception with a Custom Message and an inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public CurrentMapNullException(string message, Exception inner) : base(message, inner) { }
    }
}
