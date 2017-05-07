using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GraphSh;

namespace TestGraph
{
    [TestFixture]
    public class MatrixGraphTests:GraphTestsBase
    {
       /* protected override GraphBase GetGraph(int _dimension, List<Edge> Edges)
        {
            return new MatrixGraphBase(_dimension, Edges);
        }

        protected override GraphBase GetGraph(int[,] M)
        {
            return new MatrixGraphBase(M);
        }
        */
        protected override GraphBase GetGraph()
        {
            int[,] m = { { 1, 0, 0, 1, 1, 0, 0, 0 },
                         { 0, 1, 1, 0, 0, 1, 0, 0 },
                         { 0, 1, 1, 0, 0, 1, 0, 0 },
                         { 1, 0, 0, 1, 1, 0, 0, 0 },
                         { 1, 0, 0, 1, 1, 0, 0, 0 },
                         { 0, 1, 1, 0, 0, 1, 0, 0 },
                         { 0, 0, 0, 0, 0, 0, 1, 1 },
                         { 0, 0, 0, 0, 0, 0, 1, 1 } };
            return new MatrixGraph(m);
        }
    }
}
