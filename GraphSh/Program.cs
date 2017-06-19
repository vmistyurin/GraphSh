using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphSh
{
    class Program
    {
        public static void Main(string[] args)
        {
            using (StreamWriter writer = new StreamWriter(File.Open("results1.txt", FileMode.Create)))
            {
                Tester t = new Tester(writer, Console.Out, @"C:\Users\beave\Documents\Visual Studio 2015\Projects\GraphSh\GraphSh\bin\Debug\results.txt");
                t.LoadTestsFromDirectory(@"C:\Users\beave\Documents\Visual Studio 2015\Projects\GraphSharp\GraphSharp\bin\Debug\Tests");
            }
            
            //var m = Tester.LoadFromFile(new StreamReader(File.Open("1.txt", FileMode.Open)));            
            //Console.WriteLine(m.RelIndex());
            //Console.WriteLine(GetGraph1().RelIndex());
            Console.ReadLine();
        }
    }
}
