using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphSh
{
    public class ProbGraph<T> where T:GraphBase
    {
        private readonly T _graph;
        private readonly int _impNumber;
        public readonly List<double> _probabilities;
        public ProbGraph(T graph, int impNumber, List<double> probabilities )
        {
            _graph = graph;
            this._impNumber = impNumber;
            this._probabilities = probabilities;
        }
        public double RelIndex()
        {
            var c = new Computation<T>(_graph, _impNumber, _probabilities);
            return c.Start();
        }
    }
}
