using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Core;
using System.Runtime.Serialization.Formatters.Binary;

namespace Writer
{
    public class LogSerializerIntoStream : IActivityLoggerClient, IDisposable
    {
        public const LogSerializerHelper _helper = 
            new LogSerializerHelper(
                /* VERSION          */ 2,
                /* LOG TYPE COUNT   */ 4    );

        private Stream _stream;

        public LogSerializerIntoStream(Stream stream)
        {
            _stream = stream;
        }

        public void OnUnfilteredLog(CKTrait tags, LogLevel level, string text, DateTime logTimeUtc)
        {
            EasyWriteLog(LogType.OnUnfilteredLog, tags, level, text, logTimeUtc);
        }

        public void OnOpenGroup(IActivityLogGroup group)
        {
            if (_stream == null)
                throw new NullReferenceException("Stream is not define");
            using (BinaryWriter writer = new BinaryWriter(_stream, Encoding.UTF8))
            {
                if (group.Exception == null)
                {
                    EasyWriteLog(LogType.OnOpenGroup, group.GroupTags, group.GroupLevel, group.GroupText, group.LogTimeUtc);
                }
                else
                {
                    EasyWriteLog(LogType.OnOpenGroupWithException, group.GroupTags, group.GroupLevel, group.GroupText, group.LogTimeUtc);
                    //Add exception
                    BinaryFormatter bFormatter = new BinaryFormatter();
                    bFormatter.Serialize(writer.BaseStream, group.Exception);
                }
            }
        }
        
        public void OnGroupClosed(IActivityLogGroup group, ICKReadOnlyList<ActivityLogGroupConclusion> conclusions)
        {
            using (BinaryWriter writer = new BinaryWriter(_stream, Encoding.UTF8))
            {
                EasyWriteLog(LogType.OnGroupClosed, group.GroupTags, group.GroupLevel, group.GroupText, group.CloseLogTimeUtc);
                writer.Write(conclusions.Count);
                foreach (ActivityLogGroupConclusion conclusion in conclusions)
                {
                    writer.Write(conclusion.Tag.ToString());
                    writer.Write(conclusion.Text);
                }
            }
        }

        private void EasyWriteLog(LogType logType, CKTrait tags, LogLevel logLevel, string text, DateTime LogTimeUtc)
        {
            if (_stream == null)
                throw new NullReferenceException("Stream is not define");
            using (BinaryWriter writer = new BinaryWriter(_stream, Encoding.UTF8))
            {
                writer.Write(_helper.Hearder(logType));
                writer.Write(tags.ToString());
                writer.Write((byte)logLevel);
                writer.Write(text);
                writer.Write(LogTimeUtc.ToBinary());
            }
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
            using (BinaryWriter writer = new BinaryWriter(_stream, Encoding.UTF8))
            {
                writer.Write((byte)0);
            }
            _stream.Close();
            _stream = null;
        }
    }
}
