using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Core;
using Reader;
using Writer;
using System.IO;

namespace ConsoleTest
{
    class Program : ActivityLogger
    {
        static void Main(string[] args)
        {
            CKTraitContext test = new CKTraitContext("test");
            LogSerializerIntoStream _writer = new LogSerializerIntoStream(File.Open("C:\\Users\\JB\\Desktop\\PI S4\\CiView - the last\\CiView2\\CiView2\\Test.bin", FileMode.Open, FileAccess.Write));
            _writer.OnUnfilteredLog(test.FindOrCreate("first"), (LogLevel)1, "first", DateTime.Now);
          
            _writer.OnUnfilteredLog(test.FindOrCreate("second"), (LogLevel)2, "second", DateTime.Now);
           
            _writer.OnUnfilteredLog(test.FindOrCreate("third"), (LogLevel)3, "third", DateTime.Now);
      
            _writer.OnUnfilteredLog(test.FindOrCreate("fourth"), (LogLevel)4, "fourth", DateTime.Now);
            _writer.Free();

            LogPlayer lp = new LogPlayer("C:\\Users\\JB\\Desktop\\PI S4\\CiView - the last\\CiView2\\CiView2\\Test.bin");
            foreach (LogData ld in lp.ReadAll())
            {
                Console.WriteLine(ld.GetTag().ToString());
            }

            Console.WriteLine("Lecture");
            Console.Read();
        
        }
    }
}
