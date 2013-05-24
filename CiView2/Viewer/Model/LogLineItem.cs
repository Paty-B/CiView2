using CK.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model
{
    class LogLineItem : LineItemBase
    {
        String Content;
        LogLevel LogLevel;
        CKTrait Tag;
        Status Status;
        DateTime LogtimeUtc;
        int LineHeight;

        public LogLineItem(String content, LogLevel loglevel, CKTrait tag, DateTime logTimeUtc)
        {
            Content = content;
            LogLevel = loglevel;
            Status = Status.Expanded;
            Tag = tag;
            LogtimeUtc = logTimeUtc;
        }
    }
}
