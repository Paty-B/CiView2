using System;
using NUnit.Framework;
using CK.Core;
using Viewer.Model;

namespace Viewer.Tests
{
    [TestFixture]
    public class ModelCreationTests
    {
        IDefaultActivityLogger logger = new DefaultActivityLogger();
        ILineItemHost host = LineItem.CreateLineItemHost();
        
        [Test]
        public void EnvironmentCreatorRegistred()
        {
            EnvironmentCreator Ec = new EnvironmentCreator(host);
            logger.Output.Register(Ec);
        }

        [Test]
        public void EvironmentCtreation()
        {
            EnvironmentCreator Ec = new EnvironmentCreator(host);
            logger.Output.Register(Ec);

            var tag1 = ActivityLogger.RegisteredTags.FindOrCreate("Product");
            var tag2 = ActivityLogger.RegisteredTags.FindOrCreate("Sql");
            var tag3 = ActivityLogger.RegisteredTags.FindOrCreate("Combined Tag|Sql|Engine V2|Product");

            using (logger.OpenGroup(LogLevel.None, () => "EndMainGroup", "MainGroup"))
            {
                using (logger.OpenGroup(LogLevel.Trace, () => "EndMainGroup", "MainGroup"))
                {
                    logger.Trace(tag1, "First");
                    using (logger.AutoTags(tag1))
                    {
                        logger.Trace("Second");
                        logger.Trace(tag3, "Third");
                        using (logger.AutoTags(tag2))
                        {
                            logger.Info("First");
                        }
                    }
                    using (logger.OpenGroup(LogLevel.Info, () => "Conclusion of Info Group (no newline).", "InfoGroup"))
                    {
                        logger.Info("Second");
                        logger.Trace("Fourth");

                        string warnConclusion = "Conclusion of Warn Group" + Environment.NewLine + "with more than one line int it.";
                        using (logger.OpenGroup(LogLevel.Warn, () => warnConclusion, "WarnGroup {0} - Now = {1}", 4, DateTime.UtcNow))
                        {
                            logger.Info("Warn!");
                            logger.CloseGroup("User conclusion with multiple lines."
                                + Environment.NewLine + "It will be displayed on "
                                + Environment.NewLine + "multiple lines.");
                        }
                        logger.CloseGroup("Conclusions on one line are displayed separated by dash.");
                    }
                }
            }

            //parcourir l'arbre et l'afficher
            ILineItem curentLineItem;
            curentLineItem = host.Root;
            var next = curentLineItem.Next;
                      

            if (curentLineItem.FirstChild != null)
            {
                Console.WriteLine("ok");
            }
           // while(next != null)
            //{
              //  Console.WriteLine(next.Host.ToString);
                //next = next.Next;
            //}
        }
    }
}
