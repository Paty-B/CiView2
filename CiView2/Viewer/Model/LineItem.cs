using CK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Model
{
    static public class LineItem
    {
        public static ILineItemHost CreateLineItemHost()
        {
            return new LineItemHost();
        }

        public static ILineItem CreateLogLineItem(String content, LogLevel loglevel, CKTrait tag, DateTime logTimeUtc)
        {
            return new LogLineItem(content,loglevel,tag,logTimeUtc);
        }

        public static ILineItem CreateFilteredLineItem()
        {
            return new FilteredLineItem();
        }
    }
}
