using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace GraphSh
{
    public class AnswerCreator
    {
        private readonly TextWriter _logStream;
        public AnswerCreator(TextWriter logStream)
        {
            _logStream = logStream;
        }

        public void LoadTestsFromDirectory(string path)
        {
            using (
                StreamWriter writer =
                    new StreamWriter(
                        File.Open($"{Directory.GetCurrentDirectory()}\\Tests\\answers.txt",
                            FileMode.Create)))
            {
            }
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            DirectoryInfo directory = new DirectoryInfo(path);
            foreach (var d in directory.GetDirectories())
            {
                LoadFromSubDirectory(d);
            }
        }

        private void LoadFromSubDirectory(DirectoryInfo currentDirectory)
        {
            Tuple<int, int> dimension = GetDimension(currentDirectory.Name);
            double averageTime = 0;
            int numberOfTests = currentDirectory.GetFiles().Length;
            _logStream.WriteLine($"Тесты для {dimension.Item1},{dimension.Item2} начались!");
            GraphWithMemory<MatrixGraph>.ClearStats();
            foreach (var test in currentDirectory.GetFiles())
            {
                _logStream.WriteLine($"{test.Name} начался!");
                ProbGraph<MatrixGraph> graph = LoadFromFile(new StreamReader(File.Open(test.FullName, FileMode.Open)));
                Stopwatch timer = new Stopwatch();
                timer.Start();
                double answer = -1;
                try
                {
                    answer = graph.RelIndex();
                }
                catch (Exception e)
                {
                    //_outStream.WriteLine($"Произошла ошибка! {e.Message} в тесте {test.Name}");
                }
                finally
                {
                    using (
                        StreamWriter writer =
                            new StreamWriter(
                                 File.Open($"{Directory.GetCurrentDirectory()}\\Tests\\answers.txt",
                                    FileMode.Append)))
                    {
                        writer.WriteLine(answer);
                    }
                    averageTime += timer.ElapsedMilliseconds / (1000.0 * numberOfTests);
                }
            }
            string toOut = $"Тесты для {dimension.Item1},{dimension.Item2} завершены! Среднее время - {averageTime}";
            using (
                StreamWriter writer =
                    new StreamWriter(
                        File.Open( $"{Directory.GetCurrentDirectory()}\\Tests\\answers.txt",
                            //@"C:\Users\beave\Documents\Visual Studio 2015\Projects\GraphSh\Tests\answers.txt",
                            FileMode.Append)))
            {
                writer.WriteLine(toOut);
                writer.WriteLine($"Висячих вершин - {GraphWithMemory<MatrixGraph>.HangedVertexes/numberOfTests}");
                writer.WriteLine($"Удалено  - {GraphWithMemory<MatrixGraph>.DisconnectedDeleted / numberOfTests}");
                writer.WriteLine($"Факторизаций - {GraphWithMemory<MatrixGraph>.Factorized / numberOfTests}");
                writer.WriteLine($"Расчитанных графов - {GraphWithMemory<MatrixGraph>.CalculatedGraphs / numberOfTests}");
                writer.WriteLine($"Деревьев - {GraphWithMemory<MatrixGraph>.Trees / numberOfTests}");
                writer.WriteLine($"Цепей - {GraphWithMemory<MatrixGraph>.Chains / numberOfTests}");
            }
            _logStream.WriteLine(toOut);
        }

        private Tuple<int, int> GetDimension(string directoryName)
        {
            string[] s = directoryName.Remove(0, 5).Split(',');
            int V = Convert.ToInt32(s[0]);
            int E = Convert.ToInt32(s[1]);
            return new Tuple<int, int>(V, E);
        }

        public static ProbGraph<MatrixGraph> LoadFromFile(StreamReader test)
        {
            string[] dimension = test.ReadLine().Split(' ');
            int V = Convert.ToInt32(dimension[0]);
            int E = Convert.ToInt32(dimension[1]);
            int important = Convert.ToInt32(dimension[2]);

            string[] stringProbabilities = test.ReadLine().Trim().Split(' ');
            List<double> probabilities = new List<double>();
            foreach (var s in stringProbabilities)
                probabilities.Add(Convert.ToDouble(s));

            List<Edge> listOfEdges = new List<Edge>();
            for (int i = 0; i < E; i++)
            {
                string[] s = test.ReadLine().Split(' ');
                listOfEdges.Add(new Edge(Convert.ToInt32(s[0]), Convert.ToInt32(s[1])));
            }

            MatrixGraph graph = new MatrixGraph(V, listOfEdges);
            ProbGraph<MatrixGraph> p = new ProbGraph<MatrixGraph>(graph, important, probabilities);
            return p;
        }
    }
}
