using CK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiView.Recorder.Reader
{
    public class RealLogDataConclusions : RealLogData
    {
        internal RealLogDataConclusions() { }
        public ICKReadOnlyList<ActivityLogGroupConclusion> Conclusions { get; internal set; }
    }
}
