using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using CK.Core;


namespace Reader
{
    class LogReader
    {
        readonly Stream _stream;
        readonly BinaryReader _binaryReader;
        readonly BinaryFormatter _binaryFormatter;

        public LogReader(Stream s)
        {
            _stream = s;
            _binaryReader = new BinaryReader(s, Encoding.UTF8);
            _binaryFormatter = new BinaryFormatter();

        }

        #region Enumerable Implementation

        class EnumImpl : IEnumerable<LogData>
        {
            Enumerator enumerator;

            public EnumImpl(Stream s)
            {
                enumerator = new Enumerator(s);
            }

            class Enumerator : IEnumerator<LogData>
            {
                LogReader lr;
                LogData current;

                public Enumerator(Stream s)
                {
                    lr = new LogReader(s);
                }
                public LogData Current { get { return current; } }

                object System.Collections.IEnumerator.Current
                {
                    get { return Current; }
                }

                public void Dispose()
                {
                    lr.Free();
                }

                public bool MoveNext()
                {
                    if ((current = lr.Read()) == null)
                        return false;
                    return true;
                }

                public void Reset()
                {
                    throw new NotSupportedException();
                }
            }

            public IEnumerator<LogData> GetEnumerator()
            {
                return enumerator;
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        #endregion

        public static IEnumerable<LogData> Open(string filePath)
        {
            return new EnumImpl(File.Open(filePath, FileMode.Open, FileAccess.Read));
        }


        #region read function

        public LogData Read()
        {
            LogData ld = null;
            int nbConclusion = 0;
            TabsBuilder ts;
            byte firstByte = _binaryReader.ReadByte();
            byte logType;
            if (firstByte == 0)
            {
                Console.WriteLine("Fin du fichier!");
                return null;
            }
            logType = (byte)(firstByte << 4);
            switch ((LogType)logType)
            {
                case LogType.OnUnfilteredLog:
                    ld = new LogData((LogType)logType,
                                      _binaryReader.ReadString(),
                                      _binaryReader.ReadByte(),
                                      _binaryReader.ReadString(),
                                      DateTime.FromBinary(_binaryReader.ReadInt64()));
                    return ld;

                case LogType.OnOpenGroup:
                    ld = new LogDataOpenGroup((LogType)logType,
                                        _binaryReader.ReadString(),
                                      _binaryReader.ReadByte(),
                                      _binaryReader.ReadString(),
                                      DateTime.FromBinary(_binaryReader.ReadInt64()),
                                      null);
                    return ld;

                case LogType.OnOpenGroupWithException:
                    ld = new LogDataOpenGroup((LogType)logType,
                                        _binaryReader.ReadString(),
                                      _binaryReader.ReadByte(),
                                      _binaryReader.ReadString(),
                                      DateTime.FromBinary(_binaryReader.ReadInt64()),
                                      (Exception)_binaryFormatter.Deserialize(_stream));
                    return ld;

                case LogType.OnGroupClosed:
                    ld = new LogDataCloseGroup((LogType)logType,
                                        _binaryReader.ReadString(),
                                      _binaryReader.ReadByte(),
                                      _binaryReader.ReadString(),
                                      DateTime.FromBinary(_binaryReader.ReadInt64()),
                                      (ts = new TabsBuilder(nbConclusion = _binaryReader.ReadInt32(), _binaryReader)).Conclusions);
                    return ld;

                default:
                    return null;
            }
        }

        #endregion

        internal class TabsBuilder
        {
            internal CKReadOnlyListOnIList<ActivityLogGroupConclusion> Conclusions { get; set; }

            internal TabsBuilder(int nb, BinaryReader reader)
            {
                List<ActivityLogGroupConclusion> conclusions = new List<ActivityLogGroupConclusion>();
                CKTrait[] tags = new CKTrait[nb];
                string[] texts = new string[nb];

                for (int i = 0; i < nb; i++)
                {
                    tags[i] = ActivityLogger.RegisteredTags.FindOrCreate(reader.ReadString());
                    texts[i] = reader.ReadString();
                    conclusions.Add(new ActivityLogGroupConclusion(texts[i], tags[i]));
                }
                Conclusions = new CKReadOnlyListOnIList<ActivityLogGroupConclusion>(conclusions);
            }
        }

        public void Free()
        {
            _stream.Close();
            _binaryReader.Close();
        }

    }
}