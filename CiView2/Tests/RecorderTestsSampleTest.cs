using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CK.Core;
using CiView.Recorder.Writer;
using CiView.Recorder.Reader;

namespace CiView.Recorder.Tests
{
    [TestFixture]
    public class RecorderTestsSampleTest
    {
        internal class FakeLogGroup : IActivityLogGroup
        {
            public DateTime CloseLogTimeUtc { get; set; }

            public Exception Exception { get; set; }

            public LogLevel GroupLevel { get; set; }

            public CKTrait GroupTags { get; set; }

            public string GroupText { get; set; }

            public DateTime LogTimeUtc { get; set; }

            #region function useless for the save of logs

            public int Depth
            {
                get { throw new NotImplementedException(); }
            }

            public IActivityLogger OriginLogger
            {
                get { throw new NotImplementedException(); }
            }

            public IActivityLogGroup Parent
            {
                get { throw new NotImplementedException(); }
            }

            public CKTrait SavedLoggerTags
            {
                get { throw new NotImplementedException(); }
            }

            public LogLevelFilter SavedLoggerFilter
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsGroupTextTheExceptionMessage
            {
                get { throw new NotImplementedException(); }
            }

            #endregion
        }

        [Test]
        public void OnUnfilteredLogSampleTestWriteRead()
        {
            using (MemoryStream memory = new MemoryStream())
            using (LogWriter logWriter = new LogWriter(memory, true))
            {
                CKTrait tag = ActivityLogger.RegisteredTags.FindOrCreate("Tag");
                LogLevel logLevel = LogLevel.Info;
                string text = "I'm a log who do nothing but i exist";
                DateTime dateTime = DateTime.UtcNow;

                //OnUnfilteredLog(CKTrait tags, LogLevel level, string text, DateTime logTimeUtc)
                logWriter.OnUnfilteredLog(tag, logLevel, text, dateTime);
            }
        }

        [Test]
        public void OnOpenGroupSampleTestWriteRead()
        {
            FakeLogGroup fakeLogGroup;
            byte[] saveMemory;
            using (MemoryStream memory = new MemoryStream())
            {
                using (LogWriter logWriter = new LogWriter(memory, true))
                {
                    fakeLogGroup = new FakeLogGroup()
                    {
                        GroupLevel = LogLevel.Info,
                        GroupText = "this group is nothing because he is a troll",
                        GroupTags = ActivityLogger.RegisteredTags.FindOrCreate("Tag"),
                        LogTimeUtc = DateTime.UtcNow
                    };
                    //OnOpenGroup(IActivityLogGroup group)
                    logWriter.OnOpenGroup(fakeLogGroup);
                }
                saveMemory = memory.ToArray();
            }
            using (MemoryStream memory = new MemoryStream(saveMemory))
            {
                using (RealLogReader logReader = new RealLogReader(memory, LogWriter.VERSION, true))
                {
                    RealLogData logData = logReader.ReadOneLog();
                    System.Console.WriteLine(logData.Text + " == " + fakeLogGroup.GroupText);
                    Assert.That(logData.Text == fakeLogGroup.GroupText);
                }
            }
        }

        [Test]
        public void OnOpenGroupWithExceptionSampleTestWriteRead()
        {
            //OnGroupClosed(IActivityLogGroup group, ICKReadOnlyList<ActivityLogGroupConclusion> conclusions)
        }

        [Test]
        public void OnGroupClosedSampleTestWriteRead()
        {
            //OnGroupClosed(IActivityLogGroup group, ICKReadOnlyList<ActivityLogGroupConclusion> conclusions)
        }
    }
}
