using CK.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CiView.Recorder.Reader
{
    public class RealLogReader: IDisposable
    {
        private Stream _stream;
        private BinaryReader _binaryReader;
        private BinaryFormatter _binaryFormatter;
        private byte _version;
        private bool _mustClose;

        public RealLogReader(Stream stream, byte version, bool mustClose = false)
        {
            _stream = stream;
            _binaryReader = new BinaryReader(stream, Encoding.UTF8);
            _binaryFormatter = new BinaryFormatter();
            _version = version;
            _mustClose = mustClose;
        }

        public RealLogData ReadOneLog()
        {
            byte header = _binaryReader.ReadByte();
            if (header == 0)
                throw new EndOfStreamException("the header is egual to 0");
            byte logVersion = (byte)(header>>4);
            if (_version != logVersion)
                throw new NotSupportedException("the version of the record log ( " 
                    + logVersion + " ) is not the same than current reader ( "
                    + _version + " ) authorized to read");
            header &= 0xF;

            if(header<0 || header >3)
                throw new InvalidOperationException("the header of the log (" 
                    + header + "don't exist");

            CKTrait tags = ActivityLogger.RegisteredTags.FindOrCreate(_binaryReader.ReadString());
            LogLevel level = (LogLevel)_binaryReader.ReadByte();
            string text = _binaryReader.ReadString();
            DateTime logTimeUtc = DateTime.FromBinary(_binaryReader.ReadInt64());

            RealLogData logData = new RealLogData()
            {
                Tags = tags,
                Level = level,
                Text = text,
                LogTimeUtc = logTimeUtc,
                Type = (LogType)header
            };

            switch (logData.Type)
            {
                case LogType.OnOpenGroupWithException:
                    logData.LogException = (Exception)_binaryFormatter.Deserialize(_stream);
                    break;
                case LogType.OnGroupClosed:
                    int conclusionsCount = _binaryReader.ReadInt32();
                    List<ActivityLogGroupConclusion> conclusions = new List<ActivityLogGroupConclusion>();
                    for(int i = 0; i < conclusionsCount; i++){
                        tags = ActivityLogger.RegisteredTags.FindOrCreate(_binaryReader.ReadString());
                        text = _binaryReader.ReadString();
                        conclusions.Add(new ActivityLogGroupConclusion(text,tags));
                    }
                    logData.Conclusions = new CKReadOnlyListOnIList<ActivityLogGroupConclusion>(conclusions);
                    break;
            }
            return logData;
        }

        public void Dispose()
        {
            if (_stream == null) return;
            _binaryReader.Close();
            if (_mustClose)
                _stream.Close();
            _stream = null;
        }
    }
}
