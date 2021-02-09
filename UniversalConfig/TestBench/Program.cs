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
            //using (UniversalConfigCreator o_Creator = new UniversalConfigCreator("test.ufg"))
            //{
            //    o_Creator.AppendUnit("REG1");
            //    o_Creator.AppendRegister("REG1", "Int", typeof(int));
    
            //    Console.WriteLine(o_Creator.Build());
            //    Console.WriteLine(o_Creator.S_FileContent);
            //}

            using (UniversalConfigReader o_Reader = new UniversalConfigReader("test.ufg"))
            {
                //int itest = o_Reader.GetValue<int>("REG1", "Int");

                //Console.WriteLine(itest);
                //o_Reader.SetValue<int>("REG1", "Int", itest + 1);


                string[] getliste = o_Reader.GetArray<string>("REG1", "Int");

                if (getliste != null)
                {
                    Console.WriteLine(getliste[0]);
                    Console.WriteLine(getliste[1]);
                    Console.WriteLine(getliste[2]);
                    Console.WriteLine(getliste[3]);
                }
                Console.WriteLine(o_Reader.s_Content);

                o_Reader.SaveConfig();


            }
            Console.ReadLine();
        }
    }
}
