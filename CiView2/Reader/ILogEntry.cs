using CK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiView.Recorder.Reader
{
    public interface ILogEntry
    {
        public LogType LogType { get; }

        public CKTrait Tags { get; }
        public LogLevel LogLevel { get; }
        public string Text { get; }
        public DateTime LogTimeUtc { get; }

        public Exception Exception { get; }

        public ICKReadOnlyList<ActivityLogGroupConclusion> Conclusions { get; }
    }
}
