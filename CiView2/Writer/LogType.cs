using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiView.Recorder
{

    public enum LogType
    {
        /// <summary>
        /// Non applicable.
        /// </summary>
        None,

        /// <summary>
        /// A standard log entry.
        /// Except <see cref="ILogEntry.Conclusions"/> (reserved to <see cref="CloseGroup"/>) and <see cref="ILogEntry.Exception"/> (only <see cref="OpenGroup"/> can carry
        /// an exception), all other properties of the <see cref="ILogEntry"/> may be set.
        /// </summary>
        Log,

        /// <summary>
        /// Group is opened.
        /// Except <see cref="ILogEntry.Conclusions"/>, all other properties of the <see cref="ILogEntry"/> may be set.
        /// </summary>
        OpenGroup,

        /// <summary>
        /// Group is closed. 
        /// Note that the only available information are its <see cref="ILogEntry.Conclusions"/> and its <see cref="ILogEntry.LogTimeUtc"/>.
        /// All other properties are set to their default: <see cref="ILogEntry.LogLevel"/> for instance 
        /// is <see cref="LogLevel.None"/>.
        /// </summary>
        CloseGroup
    }
}
