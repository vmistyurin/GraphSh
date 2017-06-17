using System.Collections.Generic;
using GraphSh;
using NUnit.Framework;

namespace TestGraph
{
    public abstract class GraphTestsBase
    {
        [Test]
        public void CreateGraph()
        {
            Assert.DoesNotThrow(() => GetGraph());
        }

        [Test]
        public void GetDimension()
        {
            Assert.AreEqual(8, GetGraph().Dimension);
        }

        [TestCase(0)]
        public void GetConnectedVertex(int vertex)
        {
            Assert.AreEqual(new[] {3, 4}, GetGraph().GetConnectedVertex(vertex));
        }

        [TestCase(0)]
        public void GetConnectedComponent(int vertex)
        {
            Assert.AreEqual(new[] {0, 3, 4}, GetGraph().GetConnectedComponent(vertex));
        }

        [Test]
        public void GetEdges()
        {
            var expectedList = new List<Edge>()
            {
                new Edge(0, 3),
                new Edge(0, 4),
                new Edge(3, 4),
                new Edge(1, 2),
                new Edge(1, 5),
                new Edge(2, 5),
                new Edge(6, 7)
            };
            var listFromGraph = GetGraph().GetEdges();

            Assert.IsTrue(isListofEdgeEqual(expectedList, listFromGraph));
        }

        [TestCase(0, 2, 7)]
        public void DeleteVertexes(params int[] deletedVertexes)
        {
            var expectedList = new List<Edge>()
            {
                new Edge(1, 2),
                new Edge(0, 3)
            };
            var graph = GetGraph();

            graph.DeleteVertexes(deletedVertexes);
            var listFromGraph = graph.GetEdges();

            Assert.IsTrue(isListofEdgeEqual(expectedList, listFromGraph));
        }

        [Test]
        public void GetDegrees()
        {
            var expectedDegree = new List<int>(new[] {2, 2, 2, 2, 2, 2, 1, 1});
            Assert.AreEqual(expectedDegree, GetGraph().GetVertexesDegree());
        }

        [Test]
        public void ReNumerateTest()
        {
            var expectedList = new List<Edge>
            {
                new Edge(0, 7),
                new Edge(4, 7),
                new Edge(0, 4),
                new Edge(2, 3),
                new Edge(3, 6),
                new Edge(2, 6),
                new Edge(1, 5),
            };

            var g = GetGraph();
            g.ReNumerate(new[] {7, 3, 2, 0, 4, 6, 1, 5});
            var listFromGraph = g.GetEdges();

            Assert.IsTrue(isListofEdgeEqual(expectedList, listFromGraph));
        }

        [Test]
        public void MergeVertexes()
        {
            var expectedList = new List<Edge>
            {
                new Edge(1, 2),
                new Edge(2, 3),
                new Edge(1, 3)
            };

            var graph = GetGraph();
            List<List<int>> mergedVertex = new List<List<int>>
            {
                new List<int> {0, 3, 4},
                new List<int> {6,7}
            };
            List<bool> important = new List<bool> {true,false};
            graph.MergeVertexes(mergedVertex);
            var actualList = graph.GetEdges();

            Assert.IsTrue(isListofEdgeEqual(expectedList, actualList));
        }
        protected abstract GraphBase GetGraph();

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