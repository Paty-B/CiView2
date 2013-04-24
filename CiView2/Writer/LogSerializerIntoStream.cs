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
    public class LogSerializerIntoStream : IActivityLoggerClient
    {
        private const byte VERSION = 2;
        private const int LOG_TYPE_COUNT = 4;

        private Stream _stream;
        private byte[] _cacheHeaders = new byte[LOG_TYPE_COUNT];

        public LogSerializerIntoStream(Stream stream)
        {
            _stream = stream;
            for (int i = 0; i < LOG_TYPE_COUNT; i++)
                _cacheHeaders[i] = (byte)((VERSION << 4)+i);
        }

        public void OnUnfilteredLog(CKTrait tags, LogLevel level, string text, DateTime logTimeUtc)
        {
            using (BinaryWriter writer = new BinaryWriter(_stream, Encoding.UTF8))
            {
                writer.Write(_cacheHeaders[(int)LogType.OnUnfilteredLog]);
                writer.Write(tags.ToString());
                writer.Write((byte)level);
                writer.Write(text);
                writer.Write(logTimeUtc.ToBinary());
            }
        }

        public void OnOpenGroup(IActivityLogGroup group)
        {
            using (BinaryWriter writer = new BinaryWriter(_stream, Encoding.UTF8))
            {
                if (group.Exception == null)
                {
                    writer.Write(_cacheHeaders[(int)LogType.OnOpenGroup]);
                    writer.Write(group.GroupTags.ToString());
                    writer.Write((byte)group.GroupLevel);
                    writer.Write(group.GroupText);
                    writer.Write(group.LogTimeUtc.ToBinary());
                }
                else
                {
                    writer.Write(_cacheHeaders[(int)LogType.OnOpenGroupWithException]);
                    writer.Write(group.GroupTags.ToString());
                    writer.Write((byte)group.GroupLevel);
                    writer.Write(group.GroupText);
                    writer.Write(group.LogTimeUtc.ToBinary());
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
                writer.Write(_cacheHeaders[(int)LogType.OnGroupClosed]);
                writer.Write(group.GroupTags.ToString());
                writer.Write((byte)group.GroupLevel);
                writer.Write(group.GroupText);
                writer.Write(group.LogTimeUtc.ToBinary());
                writer.Write(conclusions.Count);
                foreach (ActivityLogGroupConclusion conclusion in conclusions)
                {
                    writer.Write(conclusion.Tag.ToString());
                    writer.Write(conclusion.Text);
                }
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
    }
}
