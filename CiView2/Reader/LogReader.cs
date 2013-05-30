using CK.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CiView.Recorder.Reader
{
    public class LogReader : IDisposable, IEnumerable<ILogEntry>
    {
        private Stream _stream;
        private BinaryReader _binaryReader;
        private BinaryFormatter _binaryFormatter;
        private bool _mustClose;
        public const byte VERSION = 3;

        public LogReader(Stream stream, bool mustClose = false)
        {
            _stream = stream;
            _binaryReader = new BinaryReader(stream, Encoding.UTF8);
            _binaryFormatter = new BinaryFormatter();
            _mustClose = mustClose;
        }

        #region read function

        public ILogEntry ReadOneLog()
        {
            byte header = _binaryReader.ReadByte();

            if (header == 0)
                return null;

            byte logVersion = (byte)(header >> 4);

            if (VERSION != logVersion)
                throw new NotSupportedException("the version of the record log ( "
                    + logVersion + " ) is not the same than current reader ( "
                    + VERSION + " ) authorized to read");

            header &= 0xF;

            if (header < 0 || header > 3)
                throw new InvalidOperationException("the header of the log ("
                    + header + "don't exist");

            switch ((LogType)header)
            {
                case LogType.OnUnfilteredLog:
                    {
                        LogEntry logEntry = new LogEntry();
                        logEntry.LogType = LogType.OnUnfilteredLog;
                        EasyReadFirstStandardParameter(logEntry);
                        return logEntry;
                    }
                case LogType.OnOpenGroup:
                    {
                        LogEntry logEntry = new LogEntry();
                        logEntry.LogType = LogType.OnOpenGroup;
                        EasyReadFirstStandardParameter(logEntry);
                        return logEntry;
                    }
                case LogType.OnOpenGroupWithException:
                    {
                        LogEntryException logEntryException = new LogEntryException();
                        logEntryException.LogType = LogType.OnOpenGroupWithException;
                        EasyReadFirstStandardParameter(logEntryException);
                        logEntryException.Exception = (Exception)_binaryFormatter.Deserialize(_stream);
                        return logEntryException;
                    }
                case LogType.OnGroupClosed:
                    {
                        LogEntryCloseGroup logEntryCloseGroup = new LogEntryCloseGroup()
                        {
                            LogType = LogType.OnGroupClosed,
                            LogTimeUtc = DateTime.FromBinary(_binaryReader.ReadInt64())
                        };
                        int conclusionsCount = _binaryReader.ReadInt32();
                        List<ActivityLogGroupConclusion> conclusions = new List<ActivityLogGroupConclusion>();
                        for (int i = 0; i < conclusionsCount; i++)
                        {
                            CKTrait tags = ActivityLogger.RegisteredTags.FindOrCreate(_binaryReader.ReadString());
                            string text = _binaryReader.ReadString();
                            conclusions.Add(new ActivityLogGroupConclusion(text, tags));
                        }
                        logEntryCloseGroup.Conclusions = new CKReadOnlyListOnIList<ActivityLogGroupConclusion>(conclusions);
                        return logEntryCloseGroup;
                    }
                default:
                    throw new InvalidOperationException(
                        "the header of the log (" + header + "don't exist");
            }
        }

        private LogEntry EasyReadFirstStandardParameter(LogEntry logEntry)
        {
            logEntry.Tags = ActivityLogger.RegisteredTags.FindOrCreate(_binaryReader.ReadString());
            logEntry.LogLevel = (LogLevel)_binaryReader.ReadByte();
            logEntry.Text = _binaryReader.ReadString();
            logEntry.LogTimeUtc = DateTime.FromBinary(_binaryReader.ReadInt64());
            return logEntry;
        }

        #endregion

        public static IEnumerable<ILogEntry> Open(string filePath)
        {
            return new LogReader(File.Open(filePath, FileMode.Open, FileAccess.Read));
        }

        public void Dispose()
        {
            Close();
        }

        public void Close()
        {
            if (_stream == null) return;
            if (_mustClose)
                _binaryReader.Dispose();
            _stream = null;
        }

        public IEnumerator<ILogEntry> GetEnumerator()
        {
            return new LogReaderEnum(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
