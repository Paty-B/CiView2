using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Core;

namespace CiView.Recorder.Reader
{
    public class LogData
    {
        LogType _type;

        CKTrait _tag;
        LogLevel _level;
        string _text;
        DateTime _date;

        #region constructeurs

        internal LogData(LogType type, string tag, byte level, string text, DateTime date)
        {
            _type = type;

            _tag = ActivityLogger.RegisteredTags.FindOrCreate(tag);
            _level = (LogLevel)level;
            _text = text;
            _date = date;
        }

        #endregion

        public LogType GetLogType()
        {
            return _type;
        }
        public CKTrait GetTag()
        {
            return _tag;
        }
        public LogLevel GetLogLevel()
        {
            return _level;
        }
        public String GetText()
        {
            return _text;
        }
        public DateTime GetDate()
        {
            return _date;
        }

    }
}
