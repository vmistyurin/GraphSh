using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            AnswerCreator t = new AnswerCreator(Console.Out);
            t.LoadTestsFromDirectory($"{Directory.GetCurrentDirectory()}\\Tests");
            Console.WriteLine("ВСЕ");
            /*var timer = new Stopwatch();
            timer.Start();
            var graph = Tester.LoadFromFile(new StreamReader(File.Open(@"C:\Users\beave\Documents\Visual Studio 2015\Projects\GraphSh\GridTests\101.txt", FileMode.Open)));

            var index = graph.RelIndex();
            Console.WriteLine($"Висячих вершин - {GraphWithMemory<MatrixGraph>.HangedVertexes}");
            Console.WriteLine($"Удалено  - {GraphWithMemory<MatrixGraph>.DisconnectedDeleted}");
            Console.WriteLine($"Факторизаций - {GraphWithMemory<MatrixGraph>.Factorized}");
            Console.WriteLine($"Расчитанных графов - {GraphWithMemory<MatrixGraph>.CalculatedGraphs}");
            Console.WriteLine($"Деревьев - {GraphWithMemory<MatrixGraph>.Trees}");
            Console.WriteLine($"Цепей - {GraphWithMemory<MatrixGraph>.Chains}");
            Console.WriteLine($"Показатель надёжности - {index}");
            Console.WriteLine($"Затраченное время - {timer.ElapsedMilliseconds/1000.0} секунд");*/
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
