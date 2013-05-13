using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader
{
    public class LogRecorder
    {
        List<LogData> _logs;
        LogDeserializerFromStream _readerHelper;
        public LogRecorder(Stream s)
        {
            _readerHelper = new LogDeserializerFromStream(s);
            _logs = new List<LogData>();
        }

        public List<LogData> getLogs()
        {
            return this._logs;
        }

        public void BuildActivity()
        {
            LogData temp;
            while ((temp = _readerHelper.Read()) != null)
            {
                _logs.Add(temp);
            }
            _readerHelper.Free();
        }
    }
}
