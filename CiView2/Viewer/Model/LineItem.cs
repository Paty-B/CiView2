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

        public static ILineItem CreateLogLineItem(String text, LogLevel loglevel, CKTrait tag, DateTime logTimeUtc, bool group)
        {
            return new LogLineItem(text,loglevel,tag,logTimeUtc, group);
        }

        public static ILineItem CreateFilteredLineItem()
        {
            return new FilteredLineItem();
        }

        internal static ILineItem CreateExceptionLineItem(String text, LogLevel logLevel, CKTrait tag, DateTime logTimeUtc, Exception exception)
        {
            return new ExceptionLineItem(text, logLevel, tag, logTimeUtc, exception);
        }
    }
}
