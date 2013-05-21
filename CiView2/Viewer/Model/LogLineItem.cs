using CK.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model
{
    class LogLineItem : LineItem
    {
        public String Content { get; private set; }
        public LogLevel LogLevel { get; private set; }
        public CKTrait Tag { get; private set; }
        public Exception Ex { get; private set; }

        public LogLineItem(String content, LogLevel loglevel,/* BagItems host,*/ Status status, CKTrait tag, Exception ex)
        {
            Content = content;
            //Host = host;
            LogLevel = loglevel;
            Tag = tag;
            Ex = ex;
        }
        public void InsertChildren(List<LogLineItem> children)
        {
            Contract.Requires(children != null, "List<LineItem> children must be not null");
            foreach (LogLineItem child in children)
            {
                InsertChild(child);
            }
        }
        public void InsertChild(LogLineItem child)
        {
            Contract.Requires(child != null, "LineItem child must be not null");
            child.Parent = this;
            child.Depth = Depth + 1;

            if (FirstChild == null)
            {
                FirstChild = child;
                LastChild = child;
            }
            else
            {
                child.Previous = LastChild;
                LastChild.Next = child;
                LastChild = child;
            }
        }
        public void Delete()
        {
        }
    }
}
