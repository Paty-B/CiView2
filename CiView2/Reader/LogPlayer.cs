using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using CK.Core;

namespace CiView.Recorder.Reader
{
    public class LogPlayer
    {
        IEnumerator<ILogEntry> _readerEnum;
        ActivityLogger _activityLogger;

        TimeSpan _waitingTime;

        DateTime _lastSendTime;
        DateTime _lastLogTimeUtc;

        bool _IsPlayingOnTime;
        bool _Playing;
 
        public LogPlayer(string filePath)
        {
            _readerEnum = LogReader.Open(filePath).GetEnumerator();
            _activityLogger = new ActivityLogger();
            _lastSendTime = DateTime.Now;
            _IsPlayingOnTime = false;
            _Playing = false;
        }

        public void PlayWithRealTime()
        {
            if(_Playing==false)
            {
                _IsPlayingOnTime=true;
                _Playing = true;

                if (_IsPlayingOnTime == false)
                    _lastSendTime = DateTime.Now;
            }

        }

        public int Play(int count = 1,bool sendOnTime=false)
        {

            _Playing = true;

            if (sendOnTime)
            {
                if (_IsPlayingOnTime == false)
                {
                    _lastSendTime = DateTime.Now;
                }
                _IsPlayingOnTime = true;

                while (_Playing &&_IsPlayingOnTime && _readerEnum.MoveNext())
                {
                    ILogEntry log = _readerEnum.Current;
                    _waitingTime = _lastLogTimeUtc - log.LogTimeUtc;
                    if (_waitingTime < (_lastSendTime - DateTime.Now))
                    {
                        Send(_readerEnum.Current);
                        count--;
                    }
                }
                return count;
            }

            else
            {
                while (count > 0 && _Playing && _readerEnum.MoveNext())
                {
                    Send(_readerEnum.Current);
                    count--;
                }
                return count;
            }
        }

        private void Send(ILogEntry log)
        {
            switch (log.LogType)
            {
                case LogType.OnGroupClosed:
                    _activityLogger.CloseGroup(log.LogTimeUtc, log.Conclusions);
                    break;
                case LogType.OnOpenGroup:
                    _activityLogger.OpenGroup(log.Tags, log.LogLevel, null, log.Text, log.LogTimeUtc);
                    break;
                case LogType.OnUnfilteredLog:
                    _activityLogger.UnfilteredLog(log.Tags, log.LogLevel, log.Text, log.LogTimeUtc);
                    break;
                case LogType.OnOpenGroupWithException:
                    _activityLogger.OpenGroup(log.Tags, log.LogLevel, null, log.Text, log.LogTimeUtc, log.Exception);
                    break;
            }
        }
        

        public void Pause()
        {
            _Playing = false;
        }

        public void Stop()
        {
            _Playing = false;
            _IsPlayingOnTime = false;
        }

    }
}
