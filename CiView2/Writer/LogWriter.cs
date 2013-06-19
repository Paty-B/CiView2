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
        Stream _stream;
        BinaryWriter _binaryWriter;
        BinaryFormatter _binaryFormatter;
        bool _writeVersion;

        public LogWriter( Stream stream, bool writeVersion, bool mustClose = true )
        {
            _stream = stream;
            _binaryWriter = new BinaryWriter(stream, Encoding.UTF8, !mustClose);
            _binaryFormatter = new BinaryFormatter();
            _writeVersion = writeVersion;
        }

        public static LogWriter Create( string fileDirectory, string autoNameFile = "CiView2_{0:u}.log", bool writeVersion = true )
        {
            if( autoNameFile == null ) autoNameFile = "CiView2_{0:u}.log";
            string path = Path.Combine(fileDirectory, String.Format(autoNameFile, DateTime.UtcNow) );
            return new LogWriter( new FileStream( path, FileMode.Create, FileAccess.Write ), writeVersion );
        }

        void IActivityLoggerClient.OnUnfilteredLog( CKTrait tags, LogLevel level, string text, DateTime logTimeUtc )
        {
            _binaryWriter.Write( (byte)FileLogType.TypeLog );
            _binaryWriter.Write( tags.ToString() );
            _binaryWriter.Write( (byte)level );
            _binaryWriter.Write( text );
            _binaryWriter.Write( logTimeUtc.ToBinary() );
        }

        void IActivityLoggerClient.OnOpenGroup( IActivityLogGroup group )
        {
            FileLogType o = group.Exception == null ? FileLogType.TypeOpenGroup : FileLogType.TypeOpenGroupWithException;
            _binaryWriter.Write( (byte)o );
            _binaryWriter.Write( group.GroupTags.ToString() );
            _binaryWriter.Write( (byte)group.GroupLevel );
            _binaryWriter.Write( group.GroupText );
            _binaryWriter.Write( group.LogTimeUtc.ToBinary() );
            if( group.Exception == null )
            {
                _binaryFormatter.Serialize( _stream, group.Exception );
            }
        }

        void IActivityLoggerClient.OnGroupClosed( IActivityLogGroup group, ICKReadOnlyList<ActivityLogGroupConclusion> conclusions )
        {
            _binaryWriter.Write( group.CloseLogTimeUtc.ToBinary() );
            _binaryWriter.Write( conclusions.Count );
            foreach( ActivityLogGroupConclusion c in conclusions)
            {
                _binaryWriter.Write(c.Tag.ToString());
                _binaryWriter.Write(c.Text);
            }
        }

        void IActivityLoggerClient.OnGroupClosing(IActivityLogGroup group, ref List<ActivityLogGroupConclusion> conclusions)
        {
        }

        void IActivityLoggerClient.OnFilterChanged(LogLevelFilter current, LogLevelFilter newValue)
        {
        }

        /// <summary>
        /// Closes the stream (without writing the end of log marker).
        /// Use an explicit call to <see cref="Close"/> to write the marker.
        /// </summary>
        public void Dispose()
        {
            Close( false );
        }

        /// <summary>
        /// Close the log by optionnaly writing a zero terminal byte into the inner stream.
        /// The stream itself will be closed only if this writer has been asked to do so (thanks to constructors' parameter mustClose sets to true).
        /// </summary>
        /// <param name="writeEndMarker">True to write an end byte marker in the inner stream.</param>
        public void Close( bool writeEndMarker = true )
        {
            if( _stream != null )
            {
                if( writeEndMarker ) _binaryWriter.Write( (byte)0 );
                _binaryWriter.Dispose();
                _stream = null;
                _binaryWriter = null;
                _binaryFormatter = null;
            }
        }
    }
}
