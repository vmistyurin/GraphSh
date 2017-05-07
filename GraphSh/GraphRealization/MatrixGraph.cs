using System.Collections.Generic;
using System.Linq;

namespace GraphSh
{
    public sealed class MatrixGraph : GraphBase
    {
        private int[,] _matrix;

        public MatrixGraph(int[,] matrix) : base(matrix)
        {
            _matrix = matrix;
        }

        public MatrixGraph(int dimension, List<Edge> edges) : base(dimension, edges)
        {
            _matrix = new int[dimension, dimension];
            foreach (var e in edges)
            {
                _matrix[e.FirstV, e.SecondV] = 1;
                _matrix[e.SecondV, e.FirstV] = 1;
            }
            for (int i = 0; i < dimension; i++)
                _matrix[i, i] = 1;
        }

        public override object Clone()
        {
            return new MatrixGraph(_matrix);
        }

        public override int Dimension => _matrix.GetLength(0);

        public override int GetNumberOfEdges()
        {
            int numberOfEdges = 0;
            for (int i = 0; i < Dimension; i++)
            {
                for (int j = 0; j < i; j++)
                    numberOfEdges += _matrix[i,j];
            }
            return numberOfEdges;
        }

        public override List<int> DeleteVertexes(params int[] numbers)
        {
            int newDimension = Dimension - numbers.Length;
            int[,] newMatrix = new int[newDimension, newDimension];
            int[] newNumbers = new int[Dimension];
            int count = 0;
            for (int i = 0; i < Dimension; i++)
            {
                if (numbers.Contains(i))
                    newNumbers[i] = -1;
                else
                    newNumbers[i] = count++;
            }
            for (int i = 0; i < Dimension; i++)
            {
                if (newNumbers[i] == -1)
                    continue;
                for (int j = 0; j < Dimension; j++)
                {
                    if (newNumbers[j] == -1) continue;
                    if (_matrix[i, j] == 0) continue;
                    newMatrix[newNumbers[i], newNumbers[j]] = 1;
                    newMatrix[newNumbers[j], newNumbers[i]] = 1;
                }
            }
            _matrix = newMatrix;
            return newNumbers.ToList();
        }

        public override List<Edge> GetEdges()
        {
            List<Edge> edges = new List<Edge>();
            for (int i = 0; i < Dimension; i++)
            for (int j = 0; j < i; j++)
            {
                if (_matrix[i, j] == 1)
                {
                    edges.Add(new Edge(i, j));
                }
            }
            return edges;
        }

        public override int[,] GetMatrix()
        {
            return _matrix;
        }

        public override int[] GetVertexesDegree()
        {
            int[] degrees = new int[Dimension];
            for (int i = 0; i < Dimension; i++)
            {
                degrees[i] = -1;
                for (int j = 0; j < Dimension; j++)
                    degrees[i] += _matrix[i, j];
            }
            return degrees;
        }

        public override void ReNumerate(int[] newNumbers)
        {
            int[,] newMatrix = new int[Dimension, Dimension];
            for (int i = 0; i < Dimension; i++)
            for (int j = 0; j < Dimension; j++)
            {
                if (_matrix[i, j] == 1)
                {
                    newMatrix[newNumbers[i], newNumbers[j]] = 1;
                }
            }
            _matrix = newMatrix;
        }

        public override int[] GetConnectedVertex(int vertex)
        {
            List<int> vertexes = new List<int>();
            for (int i = 0; i < Dimension; i++)
            {
                if (_matrix[vertex, i] == 1 && i != vertex)
                    vertexes.Add(i);
            }
            return vertexes.ToArray();
        }

        /*public override List<int> MergeVertexes(List<List<int>> mergedList, List<bool> isImportant)
        {
            List<int> newNumbers = new List<int>(Dimension);
            for (int i = 0; i < Dimension; i++)
                newNumbers.Add(-1);
            Queue<int> freeNumbers = new Queue<int>();
            int newSize = Dimension - GetCapacity(mergedList);

            List<int>[] mergedPoints = new List<int>[newSize];
            int mergedPointsCount = -1;

            List<int> freeNumberss = new List<int>();
            for(int i = 0; i < Dimension; i++)
                freeNumberss.Add(i);

            for (int i = 0; i < Dimension; i++)
            {
                if(newNumbers[i] != -1) continue;
                int newPoint = GetNewNumber(mergedList, i);
                mergedPointsCount++;
                if (newPoint == -1)
                {
                    int newNumber = 0;
                    if (freeNumbers.Count == 0)
                    {
                        newNumber = i;
                    }
                    else
                    {
                        if (i < freeNumbers.Peek())
                            newNumber = i;
                        else
                            newNumber = freeNumbers.Dequeue();
                    }
                    newNumbers[i] = newNumber;
                    mergedPoints[mergedPointsCount] = new List<int> {i};
                }
                else
                {
                    for (int j = 0; j < mergedList[newPoint].Count; j++)
                        newNumbers[mergedList[newPoint][j]] = mergedList[newPoint][0];                    
                    for (int j = 1; j < mergedList[newPoint].Count; j++)
                        freeNumbers.Enqueue(mergedList[newPoint][j]);
                    mergedPoints[mergedPointsCount] = mergedList[newPoint];
                }
            }

            int[,] newMatrix = new int[newSize,newSize];
            for (int i = 0; i < newSize; i++)
            {
                for (int j = 0; j < newSize; j++)
                {
                    if (WereConnected(mergedPoints, i, j))
                        newMatrix[i, j] = 1;
                }
            }
            _matrix = newMatrix;
            return newNumbers;
        }*/
        public override List<int> MergeVertexes(List<List<int>> mergedList, List<bool> isImportant)
        {
            List<int> newNumbers = new List<int>(Dimension);
            for (int i = 0; i < Dimension; i++)
                newNumbers.Add(-1);
            Queue<int> freeNumbers = new Queue<int>();
            for(int i = 0; i < Dimension; i++)
                freeNumbers.Enqueue(i);
            int newSize = Dimension - GetCapacity(mergedList);

            List<int>[] mergedPoints = new List<int>[newSize];
            int mergedPointsCount = -1;

            for (int i = 0; i < Dimension; i++)
            {
                if(newNumbers[i] != -1) continue;
                int newPoint = GetNewNumber(mergedList, i);
                mergedPointsCount++;
                if (newPoint == -1)
                {
                    int newNumber = freeNumbers.Dequeue();
                    newNumbers[i] = newNumber;
                    mergedPoints[mergedPointsCount] = new List<int> {i};
                }
                else
                {
                    int mergedNumber = freeNumbers.Dequeue();
                    for (int j = 0; j < mergedList[newPoint].Count; j++)
                        newNumbers[mergedList[newPoint][j]] = mergedNumber;                    
                    mergedPoints[mergedPointsCount] = mergedList[newPoint];
                }
            }

            int[,] newMatrix = new int[newSize,newSize];
            for (int i = 0; i < newSize; i++)
            {
                for (int j = 0; j < newSize; j++)
                {
                    if (WereConnected(mergedPoints, i, j))
                        newMatrix[i, j] = 1;
                }
            }
            _matrix = newMatrix;
            return newNumbers;
        }
        private static int GetFreeNumber(List<int> freeNumbers)
        {
            return 0;
        }
        private static int GetNewNumber(List<List<int>> mergedList, int number)
        {
            for (int i = 0; i < mergedList.Count; i++)
            {
                if (mergedList[i].Contains(number))
                    return i;
            }
            return -1;
        }

        private static int GetCapacity(List<List<int>> mergedList)
        {
            int capacity = 0;
            foreach (var newPoint in mergedList)
                capacity += newPoint.Count;
            return capacity - mergedList.Count;
        }

        private bool WereConnected(List<int>[] mergedPoints, int firstVertex, int secondVertex)
        {
            for (int i = 0; i < mergedPoints[firstVertex].Count; i++)
            {
                for (int j = 0; j < mergedPoints[secondVertex].Count; j++)
                {
                    if (_matrix[mergedPoints[firstVertex][i], mergedPoints[secondVertex][j]] == 1)
                        return true;
                }
            }
            return false;
        }

        private bool[] _used;
        public override int[] GetConnectedComponent(int vertex)
        {
            _used = new bool[Dimension];
            for (int i = 0; i < Dimension; i++)
            {
                _used[i] = false;
            }
            Dfs(vertex);
            List<int> result = new List<int>();
            for (int i = 0; i < Dimension; i++)
            {
                if (_used[i])
                    result.Add(i);
            }
            return result.ToArray();
        }

        private void Dfs(int v)
        {
            _used[v] = true;
            for (int i = 0; i < Dimension; i++)
            {
                if (_used[i] == false && _matrix[v, i] == 1)
                    Dfs(i);
            }
        }
    }
}