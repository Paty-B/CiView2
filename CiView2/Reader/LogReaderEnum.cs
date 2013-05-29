using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiView.Recorder.Reader
{
    public class LogReaderEnum: IEnumerator<ILogEntry>
    {
        private List<ILogEntry> _logEntryList;
        private LogReader _logReader;
        int _position = -1;

        public LogReaderEnum(LogReader logReader)
        {
            _logReader = logReader;
            _logEntryList = new List<ILogEntry>();
        }

        public ILogEntry Current
        {
            get
            {
                try
                {
                    return _logEntryList[_position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Dispose()
        {
            _logReader.Dispose();
        }

        public bool MoveNext()
        {
            _position++;
            if (_position < _logEntryList.Count)
                return true;
            ILogEntry logEntry = _logReader.ReadOneLog();
            if (logEntry == null)
                return false;
            _logEntryList.Add(logEntry);
            return true;
        }

        public void Reset()
        {
            _position = -1;
        }

        object IEnumerator.Current
        {
            get { return this.Current; }
        }
    }
}
