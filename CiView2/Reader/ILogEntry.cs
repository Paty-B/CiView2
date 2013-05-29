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
        LogType LogType { get; }

        CKTrait Tags { get; }
        LogLevel LogLevel { get; }
        string Text { get; }
        DateTime LogTimeUtc { get; }

        Exception Exception { get; }

        ICKReadOnlyList<ActivityLogGroupConclusion> Conclusions { get; }
    }
}
