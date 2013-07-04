﻿using CK.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CiView.Recorder
{
    /// <summary>
    /// A log reader acts as an enumerator of <see cref="ILogEntry"/> that are stored in a <see cref="Stream"/>.
    /// </summary>
    public class LogReader : IEnumerator<ILogEntry>
    {
        Stream _stream;
        BinaryFormatter _binaryFormatter;
        BinaryReader _binaryReader;
        ILogEntry _current;
        int _streamVersion;
        bool _endOfStream;
        
        public const int CurrentStreamVersion = 4;

        /// <summary>
        /// Initializes a new <see cref="LogReader"/> on a stream that must start with the version number.
        /// </summary>
        /// <param name="stream">Stream to read logs from.</param>
        /// <param name="mustClose">
        /// Defaults to true (the stream wil be automaticaaly closed).
        /// False to let the stream opened once this reader is disposed, the end of the log data is reached or an error is encoutered.
        /// </param>
        public LogReader( Stream stream, bool mustClose = true )
            : this( stream, -1, mustClose )
        {
        }

        /// <summary>
        /// Initializes a new <see cref="LogReader"/> on a stream with an explicit version number.
        /// </summary>
        /// <param name="stream">Stream to read logs from.</param>
        /// <param name="streamVersion">Version of the log stream. Use -1 to read the version if the stream starts with it.</param>
        /// <param name="mustClose">
        /// Defaults to true (the stream wil be automaticaaly closed).
        /// False to let the stream opened once this reader is disposed, the end of the log data is reached or an error is encoutered.
        /// </param>
        public LogReader( Stream stream, int streamVersion, bool mustClose = true )
        {
            if( streamVersion < 4 && streamVersion != -1 ) throw new ArgumentException( "Must be -1 or greater or equal to 4 (the first version).", "streamVersion" );
            _stream = stream;
            _binaryReader = new BinaryReader( stream, Encoding.UTF8, !mustClose );
            _binaryFormatter = new BinaryFormatter();
            _streamVersion = streamVersion;
        }

        /// <summary>
        /// Opens a <see cref="LogReader"/> to read the content of a file.
        /// </summary>
        /// <param name="path">Path of the log file.</param>
        /// <returns>A <see cref="LogReader"/> that will close the file when disposed.</returns>
        public static LogReader Open( string path )
        {
            return new LogReader( File.OpenRead( path ) );
        }

        /// <summary>
        /// Opens a <see cref="LogReader"/> to read the content of a file for which the version is known.
        /// </summary>
        /// <param name="path">Path of the log file.</param>
        /// <param name="version">Version of the log data: the file must not start with the version.</param>
        /// <returns>A <see cref="LogReader"/> that will close the file when disposed.</returns>
        public static LogReader Open( string path, int version )
        {
            return new LogReader( File.OpenRead( path ), version );
        }

        /// <summary>
        /// Current <see cref="ILogEntry"/>. 
        /// <see cref="MoveNext"/> must be called before getting the first entry.
        /// </summary>
        public ILogEntry Current
        {
            get 
            { 
                if( _current == null ) throw new InvalidOperationException();
                return _current; 
            }
        }

        /// <summary>
        /// Attempts to read the next <see cref="ILogEntry"/>.
        /// </summary>
        /// <returns>True on success, false otherwise.</returns>
        public bool MoveNext()
        {
            if( _stream == null ) return false;
            if( _streamVersion == -1 )
            {
                _streamVersion = _binaryReader.ReadInt32();
                if( _streamVersion != CurrentStreamVersion )
                {
                    throw new InvalidOperationException( String.Format( "Stream is not a log stream or its version is not handled (Current Version = {0}).", CurrentStreamVersion ) );
                }
            }
            FileLogType h = (FileLogType)_binaryReader.ReadByte();
            if( h == FileLogType.EndOfStream )
            {
                Close( false );
                return false;
            }
            switch( h )
            {
                case FileLogType.TypeLog:
                    {
                        var tags = ActivityLogger.RegisteredTags.FindOrCreate( _binaryReader.ReadString() );
                        var logLevel = (LogLevel)_binaryReader.ReadByte();
                        var text = _binaryReader.ReadString();
                        var logTimeUtc = DateTime.FromBinary( _binaryReader.ReadInt64() );
                        if( tags != ActivityLogger.EmptyTag ) _current = new LELogWithTrait( text, logTimeUtc, logLevel, tags );
                        else _current = new LELog( text, logTimeUtc, logLevel );
                        break;
                    }
                case FileLogType.TypeOpenGroup:
                    {
                        var tags = ActivityLogger.RegisteredTags.FindOrCreate( _binaryReader.ReadString() );
                        var logLevel = (LogLevel)_binaryReader.ReadByte();
                        var text = _binaryReader.ReadString();
                        var logTimeUtc = DateTime.FromBinary( _binaryReader.ReadInt64() );
                        if( tags != ActivityLogger.EmptyTag ) _current = new LEOpenGroupWithTrait( text, logTimeUtc, logLevel, tags );
                        _current = new LEOpenGroup( text, logTimeUtc, logLevel );
                        break;
                    }
                case FileLogType.TypeOpenGroupWithException:
                    {
                        var tags = ActivityLogger.RegisteredTags.FindOrCreate( _binaryReader.ReadString() );
                        var logLevel = (LogLevel)_binaryReader.ReadByte();
                        var text = _binaryReader.ReadString();
                        var logTimeUtc = DateTime.FromBinary( _binaryReader.ReadInt64() );
                        var exception = (Exception)_binaryFormatter.Deserialize( _stream );

                        _current = new LEOpenGroupWithException( text, logTimeUtc, logLevel, tags, exception );
                        break;
                    }
                case FileLogType.TypeGroupClosed:
                    {
                        DateTime time = DateTime.FromBinary( _binaryReader.ReadInt64() );
                        int conclusionsCount = _binaryReader.ReadInt32();
                        List<ActivityLogGroupConclusion> conclusions = new List<ActivityLogGroupConclusion>();
                        for( int i = 0; i < conclusionsCount; i++ )
                        {
                            CKTrait tags = ActivityLogger.RegisteredTags.FindOrCreate( _binaryReader.ReadString() );
                            string text = _binaryReader.ReadString();
                            conclusions.Add( new ActivityLogGroupConclusion( text, tags ) );
                        }
                        _current = new LECloseGroup( time, conclusions.ToReadOnlyList() );
                        break;
                    }
                default: 
                    {
                        Close( true );
                        break;
                    }
            }
            return true;
        }

        /// <summary>
        /// Close the inner stream if this reader has been asked to do so (thanks to constructors' parameter mustClose sets to true).
        /// </summary>
        public void Dispose()
        {
            Close( false );
        }

        void Close( bool throwError )
        {
            if( _stream != null )
            {
                _current = null;
                _binaryReader.Dispose();
                _stream = null;
                _binaryReader = null;
                _binaryFormatter = null;
            }
            if( throwError ) throw new InvalidOperationException( "Invalid log data." );
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        void IEnumerator.Reset()
        {
            throw new NotSupportedException();
        }
    }
}
