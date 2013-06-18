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
