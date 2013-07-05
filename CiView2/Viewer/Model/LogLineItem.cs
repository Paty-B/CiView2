﻿using CK.Core;
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
        internal void HideChildOrNot(ILineItemImpl parent, bool hide)
        {
            var child = parent.FirstChild;
            while (child != null)
            {
                if (hide)
                {
                    child.Hidden();
                }
                else { child.unHidden(); }
                HideChildOrNot(child, hide);
                child = child.Next;
            }

        }
        internal void Collapse()
        {
            Status = Model.Status.Collapsed;
            Grow(-(TotalLineHeight - LineHeight));
            Host.OnCollapsed(this);
            HideChildOrNot(this, true);
 
        }

        internal void UnCollapse()
        {
            Status = Model.Status.Expanded;
            Grow(restoreTotalLineHeight()-LineHeight);
            Host.OnExpended(this);
            HideChildOrNot(this, false);
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

        public void Filtered()
        {
            Status = Model.Status.Filtered;
            Host.OnFiltered(this);

        }

        public void UnFiltered()
        {
            Status = Model.Status.Expanded;
            Host.OnUnfiltered(this);
        }
    }
}
