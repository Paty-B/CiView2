﻿using System;
using System.Collections.Generic;
using CK.Core;

namespace CiView.Recorder
{
    public class LELogWithTrait : ILogEntry
    {
        readonly DateTime _time;
        readonly string _text;
        readonly CKTrait _tags;
        readonly LogLevel _level;

        public LELogWithTrait( string text, DateTime t, LogLevel l, CKTrait tags )
        {
            _text = text;
            _time = t;
            _level = l;
            _tags = tags;
        }

        public LogType LogType { get { return LogType.Log; } }

        public LogLevel LogLevel { get { return _level; } }

        public string Text { get { return _text; } }

        public CKTrait Tags { get { return _tags; } }

        public DateTime LogTimeUtc { get { return _time; } }

        public Exception Exception { get { return null; } }

        public IReadOnlyList<ActivityLogGroupConclusion> Conclusions { get { return null; } }

    }
}
