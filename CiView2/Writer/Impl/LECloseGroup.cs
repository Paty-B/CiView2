using CK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiView.Recorder
{
    public class LECloseGroup: ILogEntry
    {
        readonly DateTime _time;
        readonly IReadOnlyList<ActivityLogGroupConclusion> _conclusions;

        public LECloseGroup( DateTime t, IReadOnlyList<ActivityLogGroupConclusion> c ) 
        {
            _time = t;
            _conclusions = c;
        }

        public LogType LogType { get { return LogType.CloseGroup; } }

        public string Text { get { return null; } }

        public LogLevel LogLevel { get { return LogLevel.None; } }

        public DateTime LogTimeUtc { get { return _time; } }

        public Exception Exception { get { return null; } }

        public CKTrait Tags { get { return ActivityLogger.EmptyTag; } }

        public IReadOnlyList<ActivityLogGroupConclusion> Conclusions { get { return _conclusions; } }
    }
}
