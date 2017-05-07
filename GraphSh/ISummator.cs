using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphSh
{
    public interface ISummator1
    {
    }

    public interface ISummator : ISummator1
    {
        void Add(double probability, int answer);
        void StartSummator();
        bool IsReady { get; }
        double Answer { get; }
    }
}
