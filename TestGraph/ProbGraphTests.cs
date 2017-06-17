using System;
using System.Collections.Generic;
using System.IO;
using GraphSh;
using NUnit.Framework;

namespace TestGraph
{
    [TestFixture]
    class ProbGraphTests
    {
        [Test]
        public void RelIndex1()
        {
            List<Edge> edges = new List<Edge>
            {
                new Edge(0, 2),
                new Edge(1, 2),
                new Edge(1, 3),
                new Edge(0, 3),
            };
            var g = new MatrixGraph(4, edges);
            var gr = new ProbGraph<MatrixGraph>(g, 2, new List<double> { 1, 1, 0.5, 0.8 });

            double index = gr.RelIndex();
            
            Assert.AreEqual(0.9, index);
        }

        [Test]
        public void RelIndex2()
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
            var gr = new ProbGraph<MatrixGraph>(g, 2, new List<double> { 1, 1, 0.3, 0.4, 0.8 });

            double index = gr.RelIndex();

            Assert.True(0.824.Equal(index));
        }

       /* [Test]
        public void GridTest1()
        {
            var graph = Tester.LoadFromFile(new StreamReader(File.Open(@"C:\Users\beave\Documents\Visual Studio 2015\Projects\GraphSh\GridTests\101.txt", FileMode.Open)));

            var index = graph.RelIndex();

            Assert.True(index.Equal(0.264892578125));
        }
        [Test]
        public void GridTest2()
        {
            var graph = Tester.LoadFromFile(new StreamReader(File.Open(@"C:\Users\beave\Documents\Visual Studio 2015\Projects\GraphSh\GridTests\102.txt", FileMode.Open)));

            var index = graph.RelIndex();

            Assert.True(index.Equal(0.1863043308258));
        }

        [Test]
        public void GridTest3()
        {
            var graph = Tester.LoadFromFile(new StreamReader(File.Open(@"C:\Users\beave\Documents\Visual Studio 2015\Projects\GraphSh\GridTests\103.txt", FileMode.Open)));

            var index = graph.RelIndex();

            Assert.True(index.Equal(0.1866688728332519));
        }*/
    }
}
