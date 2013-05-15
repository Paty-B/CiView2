using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Core;

namespace Reader
{
    public class LogData
    {
        CKTrait _tag;
        LogLevel _level;
        string _text;
        DateTime _date;
        Exception _exception;
        CKTrait[] _conclusionTraits;
        string[] _conclusionTexts;

        #region constructeurs
        
        internal LogData( string tag, byte level, string text, DateTime date )
        {
            _tag = ActivityLogger.RegisteredTags.FindOrCreate( tag );
            _level = (LogLevel)level;
            _text = text;
            _date = date;
        }

        internal LogData(string tag, byte level, string text, DateTime date, Exception e) 
            : this(tag, level, text, date)
        {
            _exception = e;
        }

        internal LogData(string tag, byte level, string text, DateTime date, int nb, CKTrait[] cTraits, string[] cTexts )
            : this(tag, level, text, date)
        {
            Debug.Assert(cTraits != null && cTexts != null && cTraits.Length > 0 && cTraits.Length == cTexts.Length);
            _conclusionTraits = cTraits;
            _conclusionTexts = cTexts;
        }
        #endregion
    }
}
