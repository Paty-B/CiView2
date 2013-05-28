using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CK.Core;

namespace Reader
{
    public class LogPlayer
    {
        IEnumerable<LogData> _enum;
        ActivityLogger activityLogger;
        IEnumerator<LogData> _enumerator;

        public LogPlayer(string filePath)
        {
            _enum = LogReader.Open(filePath);
            _enumerator = _enum.GetEnumerator();
            activityLogger = new ActivityLogger();
        }

        public int Play(int count = 1)
        {
            LogData log;
            while(count>0 &&_enumerator.MoveNext())
            {
                log=_enumerator.Current;
                    switch (log.GetLogType())
                    {                 
                case LogType.OnGroupClosed:
                     activityLogger.CloseGroup(log.GetDate(),log.);
                    break;
                case LogType.OnOpenGroup:
                    activityLogger.OpenGroup(log.GetTag,log.GetLogLevel,/*Func<string> GetConclusionsText*/,log.GetText,log.GetDate);
                    break;
                case LogType.OnUnfilteredLog:
                    activityLogger.UnfilteredLog(
                        // log.GetTag,log.GetLogLevel,log.GetText,log.GetDate
                        );
                    break;
                case LogType.OnOpenGroupWithException:
                   activityLogger.OpenGroup((log.GetTag,log.GetLogLevel,/*Func<string> GetConclusionsText*/,log.GetText,log.GetDate,/*log.GetException*/);
                    break;
                    }
               count--;
            }

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
