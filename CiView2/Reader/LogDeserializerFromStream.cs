using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using CK.Core;
using Writer;

namespace Reader
{
    class LogDeserializerFromStream 
    {
        private Stream _stream;
        private BinaryReader _binaryReader;
        private BinaryFormatter _binaryFormatter;
       

        public LogDeserializerFromStream(Stream s)
        {
            _stream = s;
            _binaryReader = new BinaryReader(s, Encoding.UTF8);
            _binaryFormatter = new BinaryFormatter();
        }

        public LogData Read()
        {
            LogData ld = null;
            int nbConclusion = 0;
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
                    ld = new LogData(_binaryReader.ReadString(),
                                      _binaryReader.ReadByte(),
                                      _binaryReader.ReadString(),
                                      DateTime.FromBinary(_binaryReader.ReadInt64()));
                    return ld;
                    
                case LogType.OnOpenGroup:
                    ld = new LogData(_binaryReader.ReadString(),
                                      _binaryReader.ReadByte(),
                                      _binaryReader.ReadString(),
                                      DateTime.FromBinary(_binaryReader.ReadInt64()));
                    return ld;

                case LogType.OnOpenGroupWithException:
                    ld = new LogData(_binaryReader.ReadString(),
                                      _binaryReader.ReadByte(),
                                      _binaryReader.ReadString(),
                                      DateTime.FromBinary(_binaryReader.ReadInt64()),
                                      (Exception)_binaryFormatter.Deserialize(_stream));
                    return ld;

                case LogType.OnGroupClosed:
                    ld = new LogData(_binaryReader.ReadString(),
                                      _binaryReader.ReadByte(),
                                      _binaryReader.ReadString(),
                                      DateTime.FromBinary(_binaryReader.ReadInt64()),
                                      nbConclusion = _binaryReader.ReadInt32(),
                                      BuildDictionary(nbConclusion));
                    return ld;

                default:
                    return null;
            }

        }

        public Dictionary<String, String> BuildDictionary(int nb)
        {
            Dictionary<String, String> dict = new Dictionary<string, string>();
            for (int i = 0; i < nb; i++)
                dict.Add(_binaryReader.ReadString(), _binaryReader.ReadString());

            return dict;
        }

        public void Free()
        {
            _stream.Close();
            _binaryReader.Close();
        }

    }
}
