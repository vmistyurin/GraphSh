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

        public static ProbGraph<MatrixGraph> GetGraph1()
        {
            List<Edge> edges = new List<Edge>
            {
                new Edge(0,5),
                new Edge(1,5),
                new Edge(1,4),
                new Edge(0,4),
                new Edge(0,3),
                new Edge(3,4),
                new Edge(2,3),
                new Edge(2,4)
            };
            var g = new MatrixGraph(6, edges);
            return new ProbGraph<MatrixGraph>(g, 3, new List<double>
            {
                1, 1, 1, 0.3, 0.4, 0.2
            });
        }

        public static ProbGraph<MatrixGraph> GetGraphhh()
        {
            List<Edge> edges = new List<Edge>
            {
                new Edge(0, 2),
                new Edge(1, 2),
                new Edge(1, 3),
                new Edge(0, 3), 
            };
            var g = new MatrixGraph(4, edges);
            return new ProbGraph<MatrixGraph>(g, 2, new List<double> { 1, 1, 0.5, 0.8});
        }
        public static ProbGraph<MatrixGraph> GetGraphh()
        {
            List<Edge> edges = new List<Edge>
            {
                new Edge(0, 2),
                new Edge(2, 3),
                new Edge(0, 4),
                new Edge(1, 4),
                new Edge(1, 3)
            };
            var g = new MatrixGraph(5, edges);
            return new ProbGraph<MatrixGraph>(g, 2, new List<double> { 1, 1, 0.3, 0.4, 0.8 });
        }
        public static MatrixGraph GetGraph()
        {
            List<Edge> edges = new List<Edge>
            {
                new Edge(0, 10),
                new Edge(0, 8),
                new Edge(1, 2),
                new Edge(1, 6),
                new Edge(1, 8),
                new Edge(7, 8),
                new Edge(2, 9),
                new Edge(4, 9),
                new Edge(5, 9),
                new Edge(3, 10),
                new Edge(3, 4)
            };
            var g = new MatrixGraph(11, edges);
            return g;
        }
    }
}
