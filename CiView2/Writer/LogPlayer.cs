using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Core;

namespace CiView.Recorder
{
    public class LogPlayer : IDisposable
    {
        
        IEnumerator<ILogEntry> _readerEnum;
        ActivityLogger _activityLogger;

        TimeSpan _waitingTime;

        DateTime _lastSendTime;
        DateTime _lastLogTimeUtc;

        bool _isPlayingOnTime;
        bool _playing;

        



        public LogPlayer(string filePath)
        {
            _readerEnum = LogReader.Open(filePath, 4);
            _activityLogger = new ActivityLogger();
            _lastSendTime = DateTime.Now;
            _lastLogTimeUtc = DateTime.MinValue;
            _isPlayingOnTime = false;
            _playing = false;
            
        }

        public ActivityLogger Logger { get { return _activityLogger; } }

        public int Play(int Count = 1)
        {
            _playing = true;
            while (Count > 0 && _playing && _readerEnum.MoveNext())
            {
                Send(_readerEnum.Current);
                Count--;
            }
            return Count;
        }

        public int PlayOnTime(int Count = 1)
        {

            _playing = true;

            if (_isPlayingOnTime == false)
            {
                _isPlayingOnTime = true;
                _lastSendTime = DateTime.Now;                
            }
            while (Count>0 && _playing && _isPlayingOnTime && _readerEnum.MoveNext())
            {

                ILogEntry log = _readerEnum.Current;
                if (_lastLogTimeUtc.Equals(DateTime.MinValue))
                {
                    _lastLogTimeUtc = log.LogTimeUtc;
                }
                _waitingTime = _lastLogTimeUtc - log.LogTimeUtc;
                if (_waitingTime < (_lastSendTime - DateTime.Now).Duration())
                {
                    Send(_readerEnum.Current);
                    Count--;
                }
            }
            return Count;  
        }


        private void Send(ILogEntry log)
        {
            switch (log.LogType)
            {
                case LogType.CloseGroup:
                    _activityLogger.CloseGroup(log.LogTimeUtc, log.Conclusions);
                    break;
                case LogType.Log:
                    _activityLogger.UnfilteredLog(log.Tags, log.LogLevel, log.Text, log.LogTimeUtc);
                    break;
                case LogType.OpenGroup:
                    if (log.Exception == null)
                    {
                        _activityLogger.OpenGroup(log.Tags, log.LogLevel, null, log.Text, log.LogTimeUtc);
                    }
                    else
                    {
                        _activityLogger.OpenGroup(log.Tags, log.LogLevel, null, log.Text, log.LogTimeUtc, log.Exception);
                    }
                    break;
            }
        }
        

        public void Pause()
        {
            _playing = false;
        }

        public void Stop()
        {
            _playing = false;
            _isPlayingOnTime = false;
            
        }

        public void Dispose()
        {
            _readerEnum.Dispose();
        }
    }
}
