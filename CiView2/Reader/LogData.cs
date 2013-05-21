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
        
        #region constructeurs
        
        internal LogData( string tag, byte level, string text, DateTime date )
        {
            _tag = ActivityLogger.RegisteredTags.FindOrCreate( tag );
            _level = (LogLevel)level;
            _text = text;
            _date = date;
        }
     
        #endregion

        public CKTrait GetTag()
        {
            return _tag;
        }
    }
}
