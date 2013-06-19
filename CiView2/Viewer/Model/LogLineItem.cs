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

        internal void toogleCollapse()
        {
            if (Status == Model.Status.Expanded)
            {
                Status = Model.Status.Collapsed;
                Host.OnCollapsed(this);

                    var child = FirstChild;
                    while (child != null)
                    {
                        child.toogleHidden();
                        var next = child.Next;
                        while (next != null)
                        {
                            next.toogleHidden();
                            if (next.Next == null)
                            {
                                next = next.FirstChild;
                            }
                            else
                            {
                                next = next.Next;
                            }
                            
                        }
                        child = child.FirstChild;
                    }
            }
            else if (Status == Model.Status.Collapsed)
            {
                Status = Model.Status.Expanded;
                Host.OnExpended(this);

                var child = FirstChild;
                while (child != null)
                {
                    child.toogleHidden();
                    var next = child.Next;
                    while (next != null)
                    {
                        next.toogleHidden();
                        if (next.Next == null)
                        {
                            next = next.FirstChild;
                        }
                        else
                        {
                            next = next.Next;
                        }

                    }
                    child = child.FirstChild;
                }
            }
            else
            {
                throw new NotImplementedException("toogleCollapse status = hidden");
            }
        }

        public override void toogleHidden()
        {
            if (Status == Model.Status.Hidden)
            {
                Status = Model.Status.Expanded;
                Host.OnExpended(this);
            }
            else
            {
                Status = Model.Status.Hidden;
                Host.OnHiddened(this);
            }
        }
    }
}
