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
        internal bool group;

        public LogLineItem(String content, LogLevel loglevel, CKTrait tag, DateTime logTimeUtc, bool group)
        {
            Content = content;
            LogLevel = loglevel;
            Status = Status.Expanded;
            Tag = tag;
            LogtimeUtc = logTimeUtc;
            this.group = group;
        }

        public override VisualLineItem CreateVisualLine()
        {
            VisualLineItem Vl;

            if (group)
            {
                Vl = new VisualGroupLineItem(this);
            }
            else
            {
                Vl = new VisualLogLineItem(this);
            }
            return Vl;
        }
    }
}
