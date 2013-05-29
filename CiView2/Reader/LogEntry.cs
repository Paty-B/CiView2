using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Core;

namespace CiView.Recorder.Reader
{
    public class LogEntry: ILogEntry
    {
        internal LogEntry() { }

        public LogType LogType { get; internal set; }

        public CKTrait Tags { get; internal set; }

        public LogLevel LogLevel { get; internal set; }

        public string Text { get; internal set; }

        public DateTime LogTimeUtc { get; internal set; }

        #region paramater useless for the log type

        public Exception Exception
        {
            get { throw new NotImplementedException(); }
        }

        public ICKReadOnlyList<ActivityLogGroupConclusion> Conclusions
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}