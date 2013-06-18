using CK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiView.Recorder
{

    /// <summary>
    /// Unified interface for log entry
    /// All log entries will be exposed through this rich interface.
    /// </summary>
    public interface ILogEntry
    {
        LogType LogType { get; }

        LogLevel LogLevel { get; }

        string Text { get; }

        CKTrait Tags { get; }

        DateTime LogTimeUtc { get; }
        
        Exception Exception { get; }

        IReadOnlyList<ActivityLogGroupConclusion> Conclusions { get; }
    }
}
