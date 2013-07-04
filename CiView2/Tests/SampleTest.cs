using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CK.Core;
using System.IO;
using CiView.Recorder.Writer;
using CiView.Recorder;

namespace Tests
{
    [TestFixture]
    public class SampleTest
    {
        [Test]
        public void OnUnfilteredLogSampleTestWriteRead()
        {
            ActivityLogger logger = new ActivityLogger();
            CKTrait tags = ActivityLogger.RegisteredTags.FindOrCreate("Tag");
            string text = "I'm a log who do nothing but i exist";
            ILogEntry logEntry;
            using (MemoryStream memory = new MemoryStream())
            {
                using (LogWriter logWriter = new LogWriter(memory,false,false))
                {
                    logger.Output.RegisterClient(logWriter);
                    logger.Trace(tags, text);
                }
                memory.Seek(0, SeekOrigin.Begin);
                using (LogReader logReader = new LogReader(memory, 4))
                {
                    logReader.MoveNext();
                    logEntry = logReader.Current;
                }
            }
            Assert.That(logEntry.LogType == LogType.Log);
            Assert.That(logEntry.LogLevel == LogLevel.Trace);
            Assert.That(logEntry.Text == text);
            Assert.That(logEntry.Tags.ToString() == tags.ToString());
        }
    }
}
