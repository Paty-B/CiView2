using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Core;

namespace Reader
{
    class LogDataCloseGroup : LogData
    {
        CKTrait[] _conclusionTraits;
        string[] _conclusionTexts;

         internal LogDataCloseGroup(string tag, byte level, string text, DateTime date, int nb, CKTrait[] cTraits, string[] cTexts )
            : base(tag, level, text, date)
        {
            Debug.Assert(cTraits != null && cTexts != null && cTraits.Length > 0 && cTraits.Length == cTexts.Length);
            _conclusionTraits = cTraits;
            _conclusionTexts = cTexts;
        }
    }
}
