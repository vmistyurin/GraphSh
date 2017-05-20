using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

//1 - расчиталось
//0 - не расчиталось


namespace GraphSh
{
    public class GraphWithMemory<T> :ICloneable where T : GraphBase
    {
        #region vars
        public int ImpCount { get; set; }
        public T BaseGraph { get; }
        public List<double> Probabilities { get; set; }
        public double Probability { get; set; }
        public double ProbabilityofGraph;
        public readonly ISummator sum;
        #endregion vars
        public int Dimension => BaseGraph.Dimension;

        public GraphWithMemory(T graph, int impCount, List<double> probability, ISummator summator )
        {
            this.BaseGraph = graph;
            this.ImpCount = impCount;
            Probability = 1;
            Probabilities = probability;
            sum = summator;
        }

        #region GraphAction
        public void ReNumerate(int[] newNumbers)
        {

            double[] newProbablilities = new double[Dimension];
            for (int i = 0; i < newNumbers.Length; i++)
            {                
                newProbablilities[newNumbers[i]] = Probabilities[i];
            }
            Probabilities = new List<double>(newProbablilities);
            BaseGraph.ReNumerate(newNumbers);           
        }

        public int MergeVertex(List<List<int>> mergedList)
        {
            for (int i = 0; i < mergedList.Count; i++)
                mergedList[i].Sort();
            List<bool> isImportant = new List<bool>();
            ImpCount = GetNewimpCount(mergedList);
            if (ImpCount == 1)
            {
                ToSummator(1, 1);
                return 1;
            }
            foreach (var newPoint in mergedList)
                isImportant.Add(newPoint.Any(number => number < ImpCount));

            List<int> newNumbers = BaseGraph.MergeVertexes(mergedList, isImportant);

            double[] newProb = new double[newNumbers.Max() + 1];
            for (int i = 0; i < newNumbers.Max() + 1; i++)
            {
                newProb[i] = Probabilities[GetCount(newNumbers, i)];
            }
            Probabilities = newProb.ToList();
            if (IsCalculated())
            {
                Calculate();
                return 1;
            }
            return 0;
        }
        private int GetCount(List<int> newNumbers, int number)
        {
            for (int i = 0; i < newNumbers.Count; i++)
            {
                if (number == newNumbers[i])
                    return i;
            }
            return -1;
        }
        private int GetNewimpCount(List<List<int>> mergedList)
        {
            int newImpCount = ImpCount;
            foreach (var newPoint in mergedList)
                newImpCount -= newPoint.Count(number => number < ImpCount) - (newPoint.Any(number => number < ImpCount) ? 1 : 0);
            return newImpCount;
        }

        public List<int> DeleteVertexes(List<int> vertexes)
        {
            List<double> newProbabilities = new List<double>();
            for (int i = 0; i < Probabilities.Count; i++)
            {
                if (!vertexes.Contains(i))
                {
                    newProbabilities.Add(Probabilities[i]);
                }
            }
            Probabilities = newProbabilities;
            ImpCount -= vertexes.Count(number => number < ImpCount);
            return BaseGraph.DeleteVertexes(vertexes.ToArray());
        }

        #endregion

        #region Factorization

        public bool SetReliable(int vertex)
        {
            Probability *= Probabilities[vertex];
            Probabilities[vertex] = 1;
            List<int> connected = new List<int>(BaseGraph.GetConnectedVertex(vertex));
            List<int> toMerge = new List<int> {vertex};
            for (int i = 0; i < connected.Count; i++)
            {
                if (Probabilities[connected[i]].Equal(1))
                    toMerge.Add(connected[i]);
            }
            if (toMerge.Count > 1)
            {
                if (MergeVertex(new List<List<int>> {toMerge}) == 1)
                    return true;
            }
            return TryMerge() == 1 ? true:false;
        }

        public bool DeleteVertex(int vertex)
        {
            Interlocked.Increment(ref Factorized);
            Probability *= (1 - Probabilities[vertex]);
            DeleteVertexes(new List<int> {vertex});
            if (DeleteExtraComponents() == 1)
                return true;
            if (DeleteHangedVertex() == 1)
                return true;
            return TryMerge() == 1 ? true : false;
        }

        #endregion

        #region Reduction
        public int DeleteExtraComponents()
        {
            int[] importantComponent = BaseGraph.GetConnectedComponent(0);
            for (int i = 1; i < ImpCount; i++)
            {
                if (!importantComponent.Contains(i))
                {
                    ToSummator(1, 0);
                    return 1;
                }
            }
            List<int> toDelele = new List<int>();
            for (int i = ImpCount; i < BaseGraph.Dimension; i++)
            {
                if (!importantComponent.Contains(i))
                    toDelele.Add(i);
            }
            if (toDelele.Count > 0)
            {
                DisconnectedDeleted += toDelele.Count;
                DeleteVertexes(toDelele);
                if (IsCalculated())
                {
                    Calculate();
                    return 1;
                }
            }
            return 0;

        }

        public int DeleteHangedVertex()
        {
            var degrees = BaseGraph.GetVertexesDegree();

            List<int> hangedVertex = new List<int>();
            for (int i = 0; i < degrees.Length; i++)
            {
                if (degrees[i] == 1)
                    hangedVertex.Add(i);
            }
            while (hangedVertex.Count > 0)
            {
                Interlocked.Add(ref HangedVertexes, hangedVertex.Count);
                if (IsCalculated())
                {
                    Calculate();
                    return 1;
                }
                List<int> newImportant = new List<int>();
                for (int i = 0; i < hangedVertex.Count; i++)
                {
                    // int connected = BaseGraph.GetConnectedVertex(hangedVertex[i])[0];
                    if (hangedVertex[i] < ImpCount)
                    {
                        int connected = BaseGraph.GetConnectedVertex(hangedVertex[i])[0];
                        if (connected >= ImpCount)
                        {
                            if(!newImportant.Contains(connected))
                                newImportant.Add(connected);
                            ToSummator(1 - Probabilities[connected], 0);
                            Probability *= Probabilities[connected];
                            Probabilities[connected] = 1;
                        }
                    }
                }
                

                List<int> newNumbers = DeleteVertexes(hangedVertex);
                ImpCount += newImportant.Count;
                List<int> newNewImpNumbers = new List<int>();
                for (int i = 0; i < newImportant.Count; i++)
                {
                    newNewImpNumbers.Add(newNumbers[newImportant[i]]);
                }
                SetImportant(newNewImpNumbers);

                degrees = BaseGraph.GetVertexesDegree();
                hangedVertex = new List<int>();
                for (int i = 0; i < degrees.Length; i++)
                {
                    if (degrees[i] == 1)
                        hangedVertex.Add(i);
                }
            }
            return 0;
        }

        private void SetImportant(List<int> vertexes)
        {
            List<int> newNumbers = new List<int>();
            for (int i = 0; i < ImpCount - vertexes.Count; i++)
                newNumbers.Add(i);
            foreach (var v in vertexes)
                newNumbers.Add(v);
            for (int i = ImpCount - vertexes.Count; i < Dimension; i++)
            {
                if (!vertexes.Contains(i))
                    newNumbers.Add(i);
            }
            var inverted = Invert(newNumbers);
            ReNumerate(inverted.ToArray());
        }
        private List<int> Invert(List<int> l) //убрать
        {
            int[] newList = new int[l.Count];
            for (int i = 0; i < l.Count; i++)
            {
                newList[l[i]] = i;
            }
            return new List<int>(newList);

        }
        #endregion

        private int[] merged;
        public int TryMerge()
        {
            List<List<int>> toMerge = new List<List<int>>();
            merged = new int[Dimension];
            for (int i = 0; i < Dimension; i++)
            {
                if (Probabilities[i].Equal(1))
                    merged[i] = -1;
                else
                    merged[i] = -2;
            }
            int chrom = 0;
            for (int i = 0; i < Dimension; i++)
            {
                if (merged[i] == -1)
                    chromatic(i, chrom++);
            }
            for (int i = 0; i < Dimension; i++)
            {
                if (merged.Count(number => number == i) > 1)
                {
                    List<int> tAdd = new List<int>();
                    for (int j = 0; j < merged.Length; j++)
                    {
                        if (merged[j] == i)
                            tAdd.Add(j);
                    }
                    toMerge.Add(tAdd.ToList());
                }
            }
            if (toMerge.Count > 0)
                return MergeVertex(toMerge);
            return 0;
        }

        private void chromatic(int vertex, int color)
        {
            merged[vertex] = color;
            int[] component = BaseGraph.GetConnectedVertex(vertex);
            for (int i = 0; i < component.Length; i++)
            {
                if (merged[component[i]] == -1)
                    chromatic(component[i], color);
            }
        }


        public bool IsCalculated()
        {
            if (Dimension < 3)
                return true;
            if (BaseGraph.Dimension == BaseGraph.GetNumberOfEdges() + 1)
                return true;
            return false;
        }
        public void Calculate()
        {
            Interlocked.Increment(ref CalculatedGraphs);
            if (BaseGraph.Dimension < 2)
            {
                switch (Dimension)
                {
                    case 3:
                    {
                        Console.WriteLine(ImpCount);
                        ToSummator(Probabilities[2], 1);
                        ToSummator(1 - Probabilities[2], 0);
                        break;
                    }
                    case 4:
                    {
                        Calculate4();
                        return;
                    }
                    case 2:
                    {
                        Console.WriteLine(ImpCount);
                        ToSummator(1, 1);
                        break;
                    }
                    case 1:
                    {
                        ToSummator(1, 1);
                        break;
                    }
                    default:
                    {
                        throw new Exception();
                    }
                }
            }
            else
            {
                if (BaseGraph.Dimension == BaseGraph.GetNumberOfEdges() + 1) //дерево
                {
                    Interlocked.Increment(ref Trees);
                    int[] toDelete = GetNumberOfNonReliableHangedVertex();
                    while (toDelete.Length != 0)
                    {
                        DeleteVertexes(toDelete.ToList());
                        toDelete = GetNumberOfNonReliableHangedVertex();
                    }
                    double answer = 1;
                    for (int i = 0; i < Dimension; i++)
                        answer *= Probabilities[i];
                    ToSummator(answer, 1);
                    ToSummator((1 - answer), 0);
                }
            }
            //Thread.CurrentThread.
        }
        private int[] GetNumberOfNonReliableHangedVertex()
        {
            int[] suspect = BaseGraph.GetVertexesDegree();
            List<int> result = new List<int>();
            for (int i = 0; i < suspect.Length; i++)
            {
                if (suspect[i] != 1) continue;
                if (i >= ImpCount) result.Add(i);
            }
            return result.ToArray();
        }

        private void Calculate4()
        {         
            if (ImpCount == 1 || ImpCount == 4)
            {
                ToSummator(1, 1);
                return;
            }
            if (ImpCount == 3)
            {
                ToSummator(Probabilities[3], 1);
                ToSummator(1 - Probabilities[3], 0);
            }
            if (ImpCount == 2)
            {
                
            }
        }
        public void ToSummator(double probability, int answer)
        {
            sum.Add(Probability * probability, answer);
        }

#region statistics

        public static int HangedVertexes;
        public static int DisconnectedDeleted;
        public static int CalculatedGraphs;
        public static int Factorized;
        public static int Chains;
        public static int Trees;

        public static void ClearStats()
        {
            HangedVertexes = 0;
            DisconnectedDeleted = 0;
            CalculatedGraphs = 0;
            Factorized = 0;
            Chains = 0;
            Trees = 0;
        }
        #endregion



        public GraphWithMemory(T graph, int impCount, List<double> probability, double prob, ISummator sumator)
        {
            BaseGraph = (T)graph.Clone();
            this.ImpCount = impCount;
            Probabilities = probability;
            Probability = prob;
            sum = sumator;
        }
        public object Clone()
        {
            T newGraph = (T)BaseGraph.Clone();
            return new GraphWithMemory<T>
            (
                newGraph,
                ImpCount,
                new List<double>(this.Probabilities),
                Probability,
                this.sum
            );
        }
        public GraphWithMemory<T> GetGraph()
        {
            return (GraphWithMemory<T>)Clone();
        }
    }
}
