using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Reader
{
    public class LogRecorder
    {
        List<LogData> _logs;
        LogReader _readerHelper;
        public LogRecorder(Stream s)
        {
            _readerHelper = new LogReader(s);
            _logs = new List<LogData>();
        }

        public List<LogData> getLogs()
        {
            return this._logs;
        }

        public void BuildActivity()
        {
            LogData temp;
            while ((temp = _readerHelper.Read()) != null)
            {
                _logs.Add(temp);
            }
            _readerHelper.Free();
        }

        public void ReadLogsFileOntime(double speed = 1)
        {
            LogData pastLog;
            LogData currentLog;

            Timer timer = new Timer();
            timer.AutoReset = false;
            timer.Enabled = false;

            pastLog = _readerHelper.Read();
            currentLog = _readerHelper.Read();
            SendToViewer(pastLog);
            do
            {
                Wait(pastLog, currentLog, timer, speed);
                SendToViewer(currentLog);
                pastLog = currentLog;

            } while ((currentLog = _readerHelper.Read()) != null);

            timer.Close();
            _readerHelper.Free();
        }

        private void Wait(LogData pastLog, LogData currentLog, Timer timer, Double speed = 1)
        {
            timer.Interval = speed * ((currentLog._date - pastLog._date).TotalMilliseconds);
            timer.Start();
            while (timer.Enabled != false) { /*Simulating time*/}
        }

        private void SendToViewer(LogData log)
        {
            Console.WriteLine("{0} Tag: {1}", log._date, log._tag);

            //  throw new NotImplementedException();
        }
    }
}
