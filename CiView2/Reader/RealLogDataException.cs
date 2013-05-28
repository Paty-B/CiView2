using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiView.Recorder.Reader
{
    public class RealLogDataException : RealLogData
    {
        internal RealLogDataException() { }
        public Exception LogException { get; internal set; }
    }
}
