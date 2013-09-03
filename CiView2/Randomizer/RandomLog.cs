﻿using System;
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
        List<CKTrait> tags = new List<CKTrait>();
        bool _groupOpened = false;

        static readonly double _openGroupProba = 0.3;
        static readonly double _openGroupWithExProba = 0;
        static readonly double _unfilteredProba = 0.5;

        public RandomLog()
        {
            _activityLogger = new ActivityLogger();
            depth = 0;
            r = new Random();
            CKTrait firstTag = ActivityLogger.RegisteredTags.FindOrCreate("Cacahuete");
            CKTrait secondTag = ActivityLogger.RegisteredTags.FindOrCreate("Chips");
            CKTrait thirdTag = ActivityLogger.RegisteredTags.FindOrCreate("Pistache");
            CKTrait fourthTag = ActivityLogger.RegisteredTags.FindOrCreate("Jambon");

            tags.Add(firstTag);
            tags.Add(secondTag);
            tags.Add(thirdTag);
            tags.Add(fourthTag);
        }

        public ActivityLogger Logger { get { return _activityLogger; } }

        public int GetDepth()
        {
            return depth;
        }

        private void RandomOnUnfilteredLog()
        {
            _groupOpened = false;
            LogLevel logLevel = (LogLevel)r.Next(6);
            string text = ChooseRealLog(r.Next(6));
            DateTime dateTime = DateTime.UtcNow;
          
            _activityLogger.UnfilteredLog(tags[r.Next(4)], logLevel, text, dateTime);
        }

        private void RandomOnOpenGroup()
        {
            _groupOpened = true;
            LogLevel logLevel = (LogLevel)r.Next(6);
            string text = ChooseRealLog(r.Next(6));
            DateTime dateTime = DateTime.UtcNow;

            _activityLogger.OpenGroup(tags[r.Next(4)], logLevel, null, text, dateTime);

        }

        private void RandomOnOpenGroupWithException()
        {
            _groupOpened = true;
            LogLevel logLevel = (LogLevel)r.Next(6);
            string text = ChooseRealLog(r.Next(6));
            DateTime dateTime = DateTime.UtcNow;
            Exception e = new Exception("Erreur");

            _activityLogger.OpenGroup(tags[r.Next(4)], logLevel, null, text, dateTime, e);

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

            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[r.Next(chars.Length - 1)]);
            }
            return sb.ToString();
        }

        #region Generate function
        public void GenerateOneLog( int maxDepth)
        {
            
            double randomNum;
                     
            randomNum = r.NextDouble();
            if (randomNum <= _openGroupProba)
            {
                if (depth < maxDepth)
                {
                    depth++;
                    RandomOnOpenGroup();                    
                }
            }
            else if (randomNum <= _openGroupProba + _openGroupWithExProba)
            {
                if (depth < maxDepth)
                {
                    depth++;
                    RandomOnOpenGroupWithException();                   
                }
            }
            else if (randomNum <= _openGroupProba + _openGroupWithExProba + _unfilteredProba)
            {
                RandomOnUnfilteredLog();
            }
            else
            {
                if (depth > 0 && _groupOpened == false)
                {
                    depth--;
                    RandomOnGroupClosed();                   
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

        private string ChooseRealLog(int r)
        {
            if (r == 0)
                return "Registering assembly";
            if(r == 1)
                return "Creating Structure Objects";
            if (r == 2)
                return "Handling dependencies";
            if (r == 3)
                return "Preparing";
            if (r == 4)
                return "First setup";
            if (r == 5)
                return "Collecting Ambient Contracts";
            return "Preparing";
        }
    }
}
