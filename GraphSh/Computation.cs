
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace GraphSh
{
    internal class Computation<T> where T : GraphBase
    {
        private readonly GraphWithMemory<T> _graph;
        private readonly int _impVertexes;
        private ISummator s;

        public Computation(T graph, int important, List<double> probability)
        {
            s = new SmartSummator();
            _graph = new GraphWithMemory<T>(graph, important, probability, s);
            _impVertexes = important;
        }

        public double Start()
        {
            //Task.Factory.StartNew(() => s.StartSummator());
            Task.Factory.StartNew(() => GetIndex());
            while (!s.IsReady)
            {
                
            }
            return s.Answer;
        }
        public void GetIndex()
        {
            if (_graph.DeleteExtraComponents() == 1)
                return;
            if (_graph.TryMerge() == 1)
               return;
            if (_graph.DeleteHangedVertex() == 1)
                return;
            if (_graph.IsCalculated())
            {
                _graph.Calculate();
                return;
            }
            
            int nextVertex = RuleToChoose(_graph);
            Task.Factory.StartNew(() => GetIndexFromSubgraph(_graph.GetGraph(), nextVertex, true));
            GetIndexFromSubgraph(_graph.GetGraph(), nextVertex, false);
        }
        private void GetIndexFromSubgraph(GraphWithMemory<T> graph, int resolvingVertex, bool byReliable)
        {
            if (byReliable)
            {
                if(graph.SetReliable(resolvingVertex))
                    return;
            }
            else
            {
                if(graph.DeleteVertex(resolvingVertex))
                    return;
            }
            int nextVertex = RuleToChoose(graph);
            GetIndexFromSubgraph(graph.GetGraph(), nextVertex, true);
            GetIndexFromSubgraph(graph.GetGraph(), nextVertex, false);
        }

        private static int RuleToChoose(GraphWithMemory<T> graph)
        {
            for (int i = 0; i < graph.Dimension; i++)
            {
                if (!graph.Probabilities[i].Equal(1))
                    return i;
            }
            throw new Exception();
            return -1;
        }


    }
}
