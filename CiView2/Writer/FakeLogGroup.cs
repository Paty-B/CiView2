using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Core;

namespace CiView.Recorder.Writer
{
    public class FakeLogGroup : IActivityLogGroup
    {
        public DateTime CloseLogTimeUtc { get; set; }

        public Exception Exception { get; set; }

        public LogLevel GroupLevel { get; set; }

        public CKTrait GroupTags { get; set; }

        public string GroupText { get; set; }

        public DateTime LogTimeUtc { get; set; }

        #region function useless for the save of logs

        public int Depth
        {
            get { throw new NotImplementedException(); }
        }

        public IActivityLogger OriginLogger
        {
            get { throw new NotImplementedException(); }
        }

        public IActivityLogGroup Parent
        {
            get { throw new NotImplementedException(); }
        }

        public CKTrait SavedLoggerTags
        {
            get { throw new NotImplementedException(); }
        }

        public LogLevelFilter SavedLoggerFilter
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsGroupTextTheExceptionMessage
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
