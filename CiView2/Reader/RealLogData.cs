using CK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiView.Recorder.Reader
{
    public class RealLogData
    {
        internal RealLogData() { }

        public LogType Type { get; internal set; }

        public CKTrait Tags { get; internal set; }
        public LogLevel Level { get; internal set; }
        public string Text { get; internal set; }
        public DateTime LogTimeUtc { get; internal set; }

        public Exception LogException { get; internal set; }
        public ICKReadOnlyList<ActivityLogGroupConclusion> Conclusions { get; internal set; }
    }
}
