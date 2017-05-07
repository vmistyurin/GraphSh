using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Assert.AreEqual(0.824, index);
        }
}
}
