using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSlayer_02
{
    internal class DebugDump
    {
        public static void Dump(object msg)
        {
            StreamWriter write = new StreamWriter("dump.txt", true);
            write.WriteLine($"[{DateTime.Now}]: {msg}");
            write.Close();
        }

        public static void NewDump()
        {
            StreamWriter write = new StreamWriter("dump.txt");
            write.WriteLine("New session");
            write.Close();
        }
    }
}
