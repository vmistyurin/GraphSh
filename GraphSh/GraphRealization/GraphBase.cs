using System;
using System.Collections.Generic;

namespace GraphSh
{
    public abstract class GraphBase : ICloneable
    {
        public abstract int Dimension { get; }
        public abstract int[,] GetMatrix();
        public abstract List<Edge> GetEdges();
        public abstract List<int> DeleteVertexes(params int[] numbers);
        public abstract int[] GetVertexesDegree();
        public abstract void ReNumerate(int[] newNumbers);
        public abstract int[] GetConnectedVertex(int vertex);
        public abstract int[] GetConnectedComponent(int vertex);
        public abstract List<int> MergeVertexes(List<List<int>> mergedList);
        public abstract int GetNumberOfEdges();

        public abstract object Clone();

        protected GraphBase(int[,] matrix) { }
        protected GraphBase(int dimension, List<Edge> edges) { }
    }
}
