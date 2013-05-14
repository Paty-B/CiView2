using CK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model
{
    class LogLineItem : LineItem
    {
        public String content;
        public LogLevel loglevel;
        public CKTrait trait;

        public LogLineItem(String content, LogLevel loglevel, BagItems host, Status status, CKTrait trait)
        {
        }
        public void InsertChildren(List<LogLineItem> children)
        {
        }
        public void insertChild(LogLineItem child)
        {
        }
        public void Delete()
        {
        }
    }
}
