using System;
using System.Collections.Generic;
using CK.Core;

namespace CiView.Recorder
{
    class LELog : ILogEntry
    {
        readonly DateTime _time;
        readonly string _text;
        readonly LogLevel _level;

        public LELog( string text, DateTime t, LogLevel l ) 
        {
            _text = text;
            _time = t;
            _level = l;
        }

        public LogType LogType { get { return LogType.Log; } }

        public LogLevel LogLevel { get { return _level; } }

        public string Text { get { return _text; } }

        public CKTrait Tags { get { return ActivityLogger.EmptyTag; } }

        public DateTime LogTimeUtc { get { return _time; } }

        public Exception Exception { get { return null; } }

        public IReadOnlyList<ActivityLogGroupConclusion> Conclusions { get { return null; } }

    }
}
