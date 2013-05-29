using CK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiView.Recorder.Reader
{
    public class LogEntryCloseGroup: ILogEntry
    {
        public LogEntryCloseGroup() { }

        public LogType LogType { get; internal set; }

        public DateTime LogTimeUtc { get; internal set; }

        public ICKReadOnlyList<ActivityLogGroupConclusion> Conclusions { get; internal set; }

        #region paramater useless for the log type

        public Exception Exception
        {
            get { throw new NotImplementedException(); }
        }

        public CKTrait Tags
        {
            get { throw new NotImplementedException(); }
        }

        public LogLevel LogLevel
        {
            get { throw new NotImplementedException(); }
        }

        public string Text
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
