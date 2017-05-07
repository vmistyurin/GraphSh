using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphSh;

namespace TestGraph
{
    class FakeSummator : ISummator
    {
        public double Answer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsReady
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Add(double probability, int answer)
        {
        }

        public void StartSummator()
        {
        }
    }
}
