using CK.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viewer.View;

namespace Viewer.Model
{
    class LogLineItem : LineItemBase
    {
        internal String Content;
        internal LogLevel LogLevel;
        internal CKTrait Tag;
        internal Status Status;
        internal DateTime LogtimeUtc;
        internal int LineHeight;

        public LogLineItem(String content, LogLevel loglevel, CKTrait tag, DateTime logTimeUtc)
        {
            Content = content;
            LogLevel = loglevel;
            Status = Status.Expanded;
            Tag = tag;
            LogtimeUtc = logTimeUtc;
            LineHeight = 1;
        }

        public override VisualLineItem CreateVisualLine()
        {
            VisualLineItem Vl;

            if (this.FirstChild == null)
            {
                Vl = new VisualLogLineItem(this);
            }
            else
            {
                Vl = new VisualGroupLineItem(this);
            }
            return Vl;
        }
    }
}
