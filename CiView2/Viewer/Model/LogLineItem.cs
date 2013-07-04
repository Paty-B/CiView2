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
        internal VisualLineItem vl;

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
            
            if (group)
            {
                vl = new VisualGroupLineItem(this);
            }
            else
            {
                vl = new VisualLogLineItem(this);
            }
            return vl;
        }

        public VisualLineItem CreateFilteredVisualLine()
        {
            vl = new VisualFilteredLineItem(this);
            return vl;
        }

        internal void Collapse()
        {
                Status = Model.Status.Collapsed;
                Grow(-(TotalLineHeight - LineHeight));
                Host.OnCollapsed(this);

                var child = FirstChild;
                while (child != null)
                {
                    child.Hidden();
                    var next = child.FirstChild;
                    while (next != null)
                    {
                        next.Hidden();
                        if (next.FirstChild == null)
                        {
                            next = next.Next;
                        }
                        else
                        {
                            next = next.FirstChild;
                        }
                    }
                    child = child.Next;
                }
        }

        internal void UnCollapse()
        {
                Status = Model.Status.Expanded;
                Grow(restoreTotalLineHeight()-LineHeight);
                Host.OnExpended(this);

                var child = FirstChild;
                while (child != null)
                {
                    child.unHidden();
                    var next = child.FirstChild;
                    while (next != null)
                    {
                        next.unHidden();
                        if (next.FirstChild == null)
                        {
                            next = next.Next;
                        }
                        else
                        {
                            next = next.FirstChild;
                        }
                    }
                    child = child.Next;
                }
            }

        private int restoreTotalLineHeight()
        {
            int tlh = LineHeight;
            var child = FirstChild;
            while (child != null)
            {
                tlh += child.TotalLineHeight;
                child = child.Next;
            }
            return tlh;
        }

        public override void Hidden()
        {
                Status = Model.Status.Hidden;
                Host.OnHiddened(this);
        }

        public override void unHidden()
        {
                Status = Model.Status.Expanded;
                Host.OnExpended(this);
        }
    }
}
