using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Core;

namespace Reader
{
    public class LogDataCloseGroup : LogData
    {
        public ICKReadOnlyList<ActivityLogGroupConclusion> Conclusions { get; set; }

        internal LogDataCloseGroup(LogType logType, string tag, byte level, string text, DateTime date, ICKReadOnlyList<ActivityLogGroupConclusion> conclusions)
            : base(logType, tag, level, text, date)
        {
            //  Debug.Assert(cTraits != null && cTexts != null && cTraits.Length > 0 && cTraits.Length == cTexts.Length);
            Conclusions = conclusions;
        }
        /*
                 public CKTrait [] GetConclusionTraits()
                 {
                     return _conclusionTraits;
                 }
                 public string[] GetConclusionTexts()
                 {
                     return _conclusionTexts;
                 }*/
    }
}
