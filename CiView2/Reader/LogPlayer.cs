using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Reader
{
    public class LogPlayer
    {
        IEnumerable<LogData> _enum;

        public LogPlayer(string filePath)
        {
            _enum = LogReader.Open(filePath);
        }
        

        public void Next()
        {
            _enum.GetEnumerator().MoveNext();          
        }

        public LogData GetCurrent()
        {
            return _enum.GetEnumerator().Current;
        }

        public IEnumerable<LogData> ReadAll()
        {
            foreach (LogData ld in _enum)
            {
                yield return ld;
            }
        }
    }
}
