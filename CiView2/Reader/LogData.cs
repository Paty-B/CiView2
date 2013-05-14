using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader
{
    public class LogData
    {
        public string _tag;
        byte _level;
        string _text;
        public DateTime _date;
        Exception _exception;
        int _numConclusion;
        Dictionary<String, String> _conclusions;

        #region constructeurs
        public LogData()
        {
            _tag = null;
            _level = 0;
            _text = null;
            _date = DateTime.MinValue;
            _exception = null;
            _numConclusion = 0;
            _conclusions = null;
        }

        public LogData(string tag, byte level, string text, DateTime date)
        {
            _tag = tag;
            _level = level;
            _text = text;
            _date = date;
        }

        public LogData(string tag, byte level, string text, DateTime date, Exception e) 
            : this(tag, level, text, date)
        {
            _exception = e;
        }

        public LogData(string tag, byte level, string text, DateTime date, int nb, Dictionary<String, String> conclusions)
            : this(tag, level, text, date)
        {
            _numConclusion = nb;
            _conclusions = conclusions;
        }
        #endregion
    }
}
