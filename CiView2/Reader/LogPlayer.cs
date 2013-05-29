using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CK.Core;

namespace CiView.Recorder.Reader
{
    public class LogPlayer
    {
        IEnumerable<ILogEntry> _enum;
        ActivityLogger activityLogger;
        IEnumerator<ILogEntry> _enumerator;

        public LogPlayer(string filePath)
        {
            //_enum = LogReader.Open(filePath);
            _enumerator = _enum.GetEnumerator();
            activityLogger = new ActivityLogger();
        }

        public int Play(int count = 1)
        {
            ILogEntry log;
            while(count>0 &&_enumerator.MoveNext())
            {
                log=_enumerator.Current;
                    switch (log.LogType)
                    {                 
                case LogType.OnGroupClosed:
                     //activityLogger.CloseGroup(log.GetDate(),);
                    break;
                case LogType.OnOpenGroup:
                    //activityLogger.OpenGroup(log.GetTag,log.GetLogLevel,/*Func<string> GetConclusionsText*/,log.GetText,log.GetDate);
                    break;
                case LogType.OnUnfilteredLog:
                    /*activityLogger.UnfilteredLog(
                         log.GetTag,log.GetLogLevel,log.GetText,log.GetDate
                        );*/
                    break;
                case LogType.OnOpenGroupWithException:
                   //activityLogger.OpenGroup((log.GetTag,log.GetLogLevel,/*Func<string> GetConclusionsText*/,log.GetText,log.GetDate,/*log.GetException*/);
                    break;
                    }
               count--;
            }
            return 0;
        }

        public void Pause()
        {

        }

        public void Stop()
        {
            _enum.GetEnumerator().Dispose();
        }

    }
}
