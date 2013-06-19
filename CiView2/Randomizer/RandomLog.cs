using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using CK.Core;

namespace Randomizer
{
    public class RandomLog
    {
        Random r;
        ActivityLogger _activityLogger;
        int depth;
  

        static readonly double _openGroupProba = 0.3;
        static readonly double _openGroupWithExProba = 0.2;
        static readonly double _unfilteredProba = 0.4;
        static readonly double _closeGroupProba = 0.1;

        public RandomLog()
        {
            _activityLogger = new ActivityLogger();
            depth = 0;
        }

        public ActivityLogger Logger { get { return _activityLogger; } }

        public int GetDepth()
        {
            return depth;
        }

        private void RandomOnUnfilteredLog()
        {
            CKTrait tag = ActivityLogger.RegisteredTags.FindOrCreate("Tag" + RandomString(3));
            LogLevel logLevel = (LogLevel)r.Next(5);
            string text = "Text" + RandomString(3);
            DateTime dateTime = DateTime.UtcNow;
          
            _activityLogger.UnfilteredLog(tag, logLevel, text, dateTime);
        }

        private void RandomOnOpenGroup()
        {
            CKTrait tag = ActivityLogger.RegisteredTags.FindOrCreate("GroupTag" + RandomString(3));
            LogLevel logLevel = (LogLevel)r.Next(5);
            string text = "GroupText" + RandomString(3);
            DateTime dateTime = DateTime.UtcNow;
            
            _activityLogger.OpenGroup(tag, logLevel, null, text, dateTime);

        }

        private void RandomOnOpenGroupWithException()
        {
            CKTrait tag = ActivityLogger.RegisteredTags.FindOrCreate("GroupTagEx" + RandomString(3));
            LogLevel logLevel = (LogLevel)r.Next(5);
            string text = "GroupText" + RandomString(3);
            DateTime dateTime = DateTime.UtcNow;
            Exception e = new Exception("Erreur");
        
            _activityLogger.OpenGroup(tag, logLevel, null, text, dateTime, e);

        }

        private void RandomOnGroupClosed()
        {
            DateTime dateTime = DateTime.UtcNow;
          
            _activityLogger.CloseGroup(dateTime);
        }

        private string RandomString(int length)
        {
            string chars = "abcdefghijklmnopqrstuvwxyz";
            StringBuilder sb = new StringBuilder(length);
            r = new Random();

            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[r.Next(chars.Length - 1)]);
            }
            return sb.ToString();
        }

        #region Generate function
        public void GenerateOneLog( int maxDepth)
        {
            r = new Random();
            double randomNum;
                     
            randomNum = r.NextDouble();
            if (randomNum <= _openGroupProba)
            {
                if (depth < maxDepth)
                {
                    RandomOnOpenGroup();
                    depth++;
                }
            }
            else if (randomNum <= _openGroupProba + _openGroupWithExProba)
            {
                if (depth < maxDepth)
                {
                    RandomOnOpenGroupWithException();
                    depth++;
                }
            }
            else if (randomNum <= _openGroupProba + _openGroupWithExProba + _unfilteredProba)
            {
                RandomOnUnfilteredLog();
            }
            else
            {
                if (depth > 0)
                {
                    RandomOnGroupClosed();
                    depth--;
                }
            }
        }
        #endregion

        public void CloseStayingGroup(int nbOpenGroup)
        {
            for (int i = 0; i < nbOpenGroup; i++)
            {
                RandomOnGroupClosed();
            }
            depth -= nbOpenGroup;
        }
    }
}
