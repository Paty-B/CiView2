using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Core;

namespace CiView.Recorder.Reader
{
    public class LogEntryException: LogEntry
    {
        internal LogEntryException() { }

        public new Exception Exception { get; internal set; }
    }
}
