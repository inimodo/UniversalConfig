using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UniversalConfig;

namespace TestBench
{
    class Program
    {
        static void Main(string[] args)
        {
            using (UniversalConfigCreator o_Creator = new UniversalConfigCreator("test.ufg"))
            {
                o_Creator.AppendUnit("REG1");
                o_Creator.AppendUnit("REG2");
                o_Creator.AppendRegister("REG1", "HEl", 0);
                o_Creator.AppendRegister("REG1", "HI", 1);
                o_Creator.AppendRegister("REG1", "LO", 2);
                Console.WriteLine(o_Creator.Build());
                Console.WriteLine(o_Creator.S_FileContent);
            }
            Console.ReadLine();
        }
    }
}
