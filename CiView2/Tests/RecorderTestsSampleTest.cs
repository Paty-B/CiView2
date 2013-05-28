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
            CKTrait tags = ActivityLogger.RegisteredTags.FindOrCreate("Tag");
            LogLevel logLevel = LogLevel.Info;
            string text = "I'm a log who do nothing but i exist";
            DateTime logTimeUtc = DateTime.UtcNow;

            byte[] saveMemory;
            using (MemoryStream memory = new MemoryStream())
            using (LogWriter logWriter = new LogWriter(memory, true))
            {
                logWriter.OnUnfilteredLog(tags, logLevel, text, logTimeUtc);
                saveMemory = memory.ToArray();
            }
            using (MemoryStream memory = new MemoryStream(saveMemory))
            using (RealLogReader logReader = new RealLogReader(memory, LogWriter.VERSION, true))
            {
                RealLogData logData = logReader.ReadOneLog();
                Assert.That(logData.Level == logLevel);
                Assert.That(logData.Text == text);
                Assert.That(logData.Tags == tags);
                Assert.That(logData.LogTimeUtc == logTimeUtc);
            }
        }

        [Test]
        public void OnOpenGroupSampleTestWriteRead()
        {
            FakeLogGroup fakeLogGroup;
            byte[] saveMemory;
            using (MemoryStream memory = new MemoryStream())
            using (LogWriter logWriter = new LogWriter(memory, true))
            {
                fakeLogGroup = new FakeLogGroup()
                {
                    GroupLevel = LogLevel.Info,
                    GroupText = "this group is nothing because he is a troll",
                    GroupTags = ActivityLogger.RegisteredTags.FindOrCreate("Tag"),
                    LogTimeUtc = DateTime.UtcNow
                };
                logWriter.OnOpenGroup(fakeLogGroup);
                saveMemory = memory.ToArray();
            }
            using (MemoryStream memory = new MemoryStream(saveMemory))
            using (RealLogReader logReader = new RealLogReader(memory, LogWriter.VERSION, true))
            {
                RealLogData logData = logReader.ReadOneLog();
                Assert.That(logData.Level == fakeLogGroup.GroupLevel);
                Assert.That(logData.Text == fakeLogGroup.GroupText);
                Assert.That(logData.Tags == fakeLogGroup.GroupTags);
                Assert.That(logData.LogTimeUtc == fakeLogGroup.LogTimeUtc);
            }
        }

        [Test]
        public void OnOpenGroupWithExceptionSampleTestWriteRead()
        {
            string text = "this group is nothing with exception because he is a troll";
            FakeLogGroup fakeLogGroup = new FakeLogGroup()
            {
                GroupLevel = LogLevel.Info,
                GroupText = text,
                GroupTags = ActivityLogger.RegisteredTags.FindOrCreate("Tag"),
                LogTimeUtc = DateTime.UtcNow,
                Exception = new Exception(text)
            };
            byte[] saveMemory;
            using (MemoryStream memory = new MemoryStream())
            using (LogWriter logWriter = new LogWriter(memory, true))
            {
                logWriter.OnOpenGroup(fakeLogGroup);
                saveMemory = memory.ToArray();
            }
            using (MemoryStream memory = new MemoryStream(saveMemory))
            using (RealLogReader logReader = new RealLogReader(memory, LogWriter.VERSION, true))
            {
                RealLogData logData = logReader.ReadOneLog();
                Assert.That(logData.Level == fakeLogGroup.GroupLevel);
                Assert.That(logData.Text == fakeLogGroup.GroupText);
                Assert.That(logData.Tags == fakeLogGroup.GroupTags);
                Assert.That(logData.LogTimeUtc == fakeLogGroup.LogTimeUtc);
                Assert.That(logData.LogException.ToString() == fakeLogGroup.Exception.ToString());
            }
        }

        [Test]
        public void OnGroupClosedSampleTestWriteRead()
        {
            byte[] saveMemory;
            FakeLogGroup fakeLogGroup = new FakeLogGroup()
            {
                GroupLevel = LogLevel.Info,
                GroupText = "it's so close, try!",
                GroupTags = ActivityLogger.RegisteredTags.FindOrCreate("Tag"),
                CloseLogTimeUtc = DateTime.UtcNow
            };
            List<ActivityLogGroupConclusion> conclusions = new List<ActivityLogGroupConclusion>();
            conclusions.Add(new ActivityLogGroupConclusion("first conlusion",ActivityLogger.RegisteredTags.FindOrCreate("A")));
            conclusions.Add(new ActivityLogGroupConclusion("second conclusion",ActivityLogger.RegisteredTags.FindOrCreate("C")));
            conclusions.Add(new ActivityLogGroupConclusion("final conclusion",ActivityLogger.RegisteredTags.FindOrCreate("Z")));
            ICKReadOnlyList<ActivityLogGroupConclusion> ConclusionsReadOnly = new CKReadOnlyListOnIList<ActivityLogGroupConclusion>(conclusions);
            using (MemoryStream memory = new MemoryStream())
            using (LogWriter logWriter = new LogWriter(memory, true))
            {
                logWriter.OnGroupClosed(fakeLogGroup,ConclusionsReadOnly);
                saveMemory = memory.ToArray();
            }
            using (MemoryStream memory = new MemoryStream(saveMemory))
            using (RealLogReader logReader = new RealLogReader(memory, LogWriter.VERSION, true))
            {
                RealLogData logData = logReader.ReadOneLog();
                Assert.That(logData.Level == fakeLogGroup.GroupLevel);
                Assert.That(logData.Text == fakeLogGroup.GroupText);
                Assert.That(logData.Tags == fakeLogGroup.GroupTags);
                Assert.That(logData.LogTimeUtc == fakeLogGroup.CloseLogTimeUtc);
                CollectionAssert.AreEqual(logData.Conclusions, ConclusionsReadOnly);
            }
        }
    }
}
