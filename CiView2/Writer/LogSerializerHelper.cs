using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Writer
{
    public class LogSerializerHelper
    {
        private byte[] _cacheHeaders;

        public LogSerializerHelper(byte version, int logTypeCount)
        {
            Version = version;
            LogTypeCount = logTypeCount;
            _cacheHeaders = new byte[logTypeCount];
            for (int i = 0; i < logTypeCount; i++)
                _cacheHeaders[i] = (byte)((version << 4) + i);
        }

        public byte Version { get; private set; }
        public int LogTypeCount { get; private set; }

        public byte Hearder(LogType logType)
        {
            return _cacheHeaders[(int)logType];
        }
    }
}
