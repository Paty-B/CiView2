using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Core;
using System.Runtime.Serialization.Formatters.Binary;

namespace CiView.Recorder.Writer
{
    public class LogWriter : IActivityLoggerClient, IDisposable
    {
        private byte[] _cacheHeaders;

        public const byte VERSION = 3;

        private bool _mustClose;
        private Stream _stream;
        private BinaryWriter _binaryWriter;
        private BinaryFormatter _binaryFormatter;

        public LogWriter(Stream stream, bool mustClose = false)
        {
            _stream = stream;
            _binaryWriter = new BinaryWriter(stream, Encoding.UTF8);
            _binaryFormatter = new BinaryFormatter();
            _mustClose = mustClose;

            int logTypeCount = 4;
            _cacheHeaders = new byte[logTypeCount];
            for (int i = 0; i < logTypeCount; i++)
                _cacheHeaders[i] = (byte)((VERSION << 4) + i);
        }

        static public LogWriter LogWriterIntoFile(string logFileEmplacement, string autoNameFile = "CiView2_{0:u}.log")
        {
            string finalLinkEmplacement;
            if (autoNameFile == null)
            {
                finalLinkEmplacement = String.Format(logFileEmplacement + "/" + autoNameFile, DateTime.UtcNow);
            }
            else
            {
                finalLinkEmplacement = logFileEmplacement;
            }
            StreamWriter streamWriter = new StreamWriter(finalLinkEmplacement, true, Encoding.UTF8);
            return new LogWriter(streamWriter.BaseStream, false);
        }

        public void OnUnfilteredLog(CKTrait tags, LogLevel level, string text, DateTime logTimeUtc)
        {
            EasyWriteLog(LogType.OnUnfilteredLog, tags, level, text, logTimeUtc);
        }

        public void OnOpenGroup(IActivityLogGroup group)
        {
            if (group.Exception == null)
            {
                EasyWriteLog(LogType.OnOpenGroup, group.GroupTags, group.GroupLevel, group.GroupText, group.LogTimeUtc);
            }
            else
            {
                EasyWriteLog(LogType.OnOpenGroupWithException, group.GroupTags, group.GroupLevel, group.GroupText, group.LogTimeUtc);
                //Add exception
                _binaryFormatter.Serialize(_stream, group.Exception);
            }
        }
        
        public void OnGroupClosed(IActivityLogGroup group, ICKReadOnlyList<ActivityLogGroupConclusion> conclusions)
        {
            EasyWriteLog(LogType.OnGroupClosed, group.GroupTags, group.GroupLevel, group.GroupText, group.CloseLogTimeUtc);
            _binaryWriter.Write(conclusions.Count);
            foreach (ActivityLogGroupConclusion conclusion in conclusions)
            {
                _binaryWriter.Write(conclusion.Tag.ToString());
                _binaryWriter.Write(conclusion.Text);
            }
        }

        private void EasyWriteLog(LogType logType, CKTrait tags, LogLevel logLevel, string text, DateTime LogTimeUtc)
        {
            if (_stream == null) throw new ObjectDisposedException("LogWriter");

            _binaryWriter.Write(_cacheHeaders[(int)logType]);
            _binaryWriter.Write(tags.ToString());
            _binaryWriter.Write((byte)logLevel);
            _binaryWriter.Write(text);
            _binaryWriter.Write(LogTimeUtc.ToBinary());
        }

        #region function useless for the save of logs

        public void OnGroupClosing(IActivityLogGroup group, ref List<ActivityLogGroupConclusion> conclusions)
        {
        }

        public void OnFilterChanged(LogLevelFilter current, LogLevelFilter newValue)
        {
        }

        #endregion

        public void Dispose()
        {
            if (_stream == null) return;
            _binaryWriter.Write((byte)0);
            _binaryWriter.Close();
            if (_mustClose)
                _stream.Close();
            _stream = null;
        }
    }
}
