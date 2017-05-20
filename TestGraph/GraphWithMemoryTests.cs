using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GraphSh;
using NUnit.Framework;
using NSubstitute;

namespace TestGraph
{
    [TestFixture]
    public class GraphWithMemoryTests
    {
        [Test]
        public void DeleteHangedVertex()
        {
            List<Edge> listofEdges = new List<Edge>()
            {
                new Edge(0, 1),
                new Edge(1, 4),
                new Edge(3, 4),
                new Edge(3, 9),
                new Edge(3, 8),
                new Edge(6, 8),
                new Edge(7, 8),
                new Edge(8, 10),
                new Edge(2, 10),
                new Edge(5, 10),
                new Edge(1, 5),
                new Edge(3, 10)
            };

            var summator = Substitute.For<ISummator>();
            //var summator = GetFakeSummator();
            GraphWithMemory<MatrixGraph> gr = new GraphWithMemory<MatrixGraph>(new MatrixGraph(11, listofEdges), 4, new List<double> { 1,1,1,1, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7}, summator ); 

                List<Edge> expectedList = new List<Edge>
            {
                new Edge(0, 4),
                new Edge(0, 3),
                new Edge(2, 4),
                new Edge(2, 5),
                new Edge(5, 1),
                new Edge(1, 3),
                new Edge(1, 2)
            };

            gr.DeleteHangedVertex();

            summator.Received().Add(Arg.Is<double>(x => x - 0.3 < 1e-16), 0);
            Assert.AreEqual(3, gr.ImpCount);
            Assert.True(isListofEdgeEqual(expectedList, gr.BaseGraph.GetEdges()));
        }


        [Test]
        public void DeleteExtraComponents_OK()
        {
            List<Edge> edges = new List<Edge>
            {
                new Edge(0, 1),
                new Edge(1, 2),
                new Edge(2, 9),
                new Edge(3, 4),
                new Edge(6, 8),
                new Edge(7, 8),
                new Edge(0, 5),
                new Edge(2, 5)

            };
            var g = new MatrixGraph(10, edges);
            var summator = Substitute.For<ISummator>();
            var mg = new GraphWithMemory<MatrixGraph>(g, 3, new List<double> {1,1,1,1,1,1,0.5,0.5,0.5,0.5}, summator);
            var expectedList = new List<Edge>
            {
                new Edge(0, 1),
                new Edge(1, 2),
                new Edge(2, 3),
                new Edge(2 ,4),
                new Edge(0, 3)
            };
            
            var answer = mg.DeleteExtraComponents();

            summator.DidNotReceive().Add(Arg.Any<double>(), Arg.Any<int>());
            Assert.IsTrue(isListofEdgeEqual(expectedList, mg.BaseGraph.GetEdges()));
            Assert.AreEqual(0, answer);
        }

        [Test]
        public void DeleteExtraComponents_NO()
        {
            List<Edge> edges = new List<Edge>
            {
                new Edge(0, 1),
                new Edge(1, 2),
                new Edge(2, 9),
                new Edge(3, 4),
                new Edge(6, 8),
                new Edge(7, 8),

            };
            var g = new MatrixGraph(10, edges);
            var summator = Substitute.For<Summator>();
            var mg = new GraphWithMemory<MatrixGraph>(g, 4, new List<double> {1,1,1,0,0,0,0,0,0,0}, summator);
            var expectedList = new List<Edge>
            {
                new Edge(0, 1),
                new Edge(1, 2),
                new Edge(2, 3)
            };

            var answer = mg.DeleteExtraComponents();

            summator.Received().Add(1, 0);
            Assert.AreEqual(1, answer);
        }

        [Test]
        public void DeleteVertexes()
        {
            var graph = GetGraph();
            graph.Probabilities[6] = 0.3;
            graph.Probabilities[9] = 0.8;
            graph.Probabilities[10] = 0.1;


            List<Edge> expectedList = new List<Edge>
            {
                new Edge(0, 1),
                new Edge(1, 2),
                new Edge(0, 7),
                new Edge(7, 3),
                new Edge(0, 5),
                new Edge(1, 5),
                new Edge(2, 6),
                new Edge(5, 4)
            };

            graph.DeleteVertexes(new List<int> { 6, 4, 5 });

            Assert.IsTrue(isListofEdgeEqual(graph.BaseGraph.GetEdges(),expectedList));
            Assert.AreEqual(0.1, graph.Probabilities[7]);
        }
        public GraphWithMemory<MatrixGraph> GetGraph()
        {
            List<Edge> edges = new List<Edge>
            {
                new Edge(0, 1),
                new Edge(0, 10),
                new Edge(0, 8),
                new Edge(1, 2),
                new Edge(1, 6),
                new Edge(1, 8),
                new Edge(7, 8),
                new Edge(2, 9),
                new Edge(4, 9),
                new Edge(5, 9),
                new Edge(3, 10)
            };
            var g = new MatrixGraph(11, edges);
            return new GraphWithMemory<MatrixGraph>(g,4,new List<double> {1,1,1,1,0.5,0.5,0.5,0.5,0.5,0.5,0.5}, null);
        }

        public GraphWithMemory<MatrixGraph> GetGraph1()
        {
            List<Edge> edges = new List<Edge>
            {
                new Edge(1, 5),
                new Edge(5, 11),
                new Edge(11, 10),
                new Edge(0, 10),
                new Edge(6, 11),
                new Edge(2, 6),
                new Edge(2, 8),
                new Edge(8, 7),
                new Edge(7, 9),
                new Edge(3, 9),
                new Edge(9, 4),
                new Edge(1, 7)
            };
            var g = new MatrixGraph(12, edges);
            var s = Substitute.For<ISummator>();
            var result = new GraphWithMemory<MatrixGraph>(g, 3, new List<double> { 1,1,1,0.5,0.5,1,0.5,0.5,0.1,0.5,0.5,0.5}, s);
            return result;
        }

        private static bool isListofEdgeEqual(List<Edge> l1, List<Edge> l2)
        {
            foreach (var edge in l1)
                if (!l2.Contains(edge))
                    return false;
            foreach (var edge in l2)
                if (!l1.Contains(edge))
                    return false;
            return true;
        }

    }
}
