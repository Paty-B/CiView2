using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writer;

namespace Reader
{
    public class LogDeserializerFromStream
    {
        private Stream _stream;

        public LogDeserializerFromStream()
        {
            byte firstByte = 1;
            byte version = 3;
            if (version == firstByte >> 4)
            {
                firstByte = (byte)(firstByte ^ (version<<4));
            }

            switch((LogType)firstByte)
            {
                case LogType.OnUnfilteredLog :

                    break;
                case LogType.OnOpenGroup :

                    break;
                case LogType.OnOpenGroupWithException :

                    break;
                case LogType.OnGroupClosed :

                    break;
                default:

                    break;
            }
        }
    }
}
