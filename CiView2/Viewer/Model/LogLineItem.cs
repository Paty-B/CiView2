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
        public String Content { get; private set; }
        public LogLevel LogLevel { get; private set; }
        public CKTrait Tag { get; private set; }
        public Exception Ex { get; private set; }
        public LogLineItem()
        {
        }

        public LogLineItem(String content, LogLevel loglevel,/* BagItems host,*/ Status status, CKTrait tag, Exception ex)
            : base()
        {
            Content = content;
            LogLevel = loglevel;
            Tag = tag;
            Ex = ex;
        }
    }
}
