using CK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viewer.View;

namespace Viewer.Model
{
    class ExceptionLineItem : LogLineItem
    {
        internal Exception exception;

        public ExceptionLineItem(String content, LogLevel loglevel, CKTrait tag, DateTime logTimeUtc, Exception ex)
            :base(content, loglevel, tag, logTimeUtc, true)
        {
            exception = ex;
        }

        public override View.VisualLineItem CreateVisualLine()
        {
            VisualLineItem Vl;
            Vl = new VisualExceptionLineItem(this);
            return Vl;
        }
    }
}
