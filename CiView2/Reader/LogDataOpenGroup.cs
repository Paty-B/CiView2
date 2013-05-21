using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader
{
    class LogDataOpenGroup : LogData
    {
        Exception _exception;
         internal LogDataOpenGroup(string tag, byte level, string text, DateTime date, Exception e) 
            : base(tag, level, text, date)
        {
            _exception = e;
        }
    }
}
