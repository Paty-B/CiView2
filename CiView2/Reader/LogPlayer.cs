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
        bool _Playing;

        public LogPlayer(string filePath)
        {
            _enum = LogReader.Open(filePath);
            _Playing = false;
        }

        public void Play()
        {
            _Playing = true;
            while (_Playing == true)
            {
                Next();
                /// send ????
            }
        }

        public void Pause()
        {
            _Playing = false;
        }

        public void Stop()
        {
            _Playing = false;
            _enum.GetEnumerator().Dispose();
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
