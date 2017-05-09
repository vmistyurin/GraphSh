using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphSh
{
    public class SimpleSummator: ISummator
    {
        public bool IsReady { get; private set; }
        public double Answer { get; private set; }
        private double probability;

        public SimpleSummator()
        {
            Answer = 0;
            probability = 0;
            IsReady = false;
        }
        public void Add(double probability, int answer)
        {
            this.probability += probability;
            Answer += answer * probability;
            //Console.WriteLine($"обновлено! Пока {this.probability} \t {Answer}");
            if (this.probability.Equal(1))
            {
                IsReady = true;
            }
        }

        public void StartSummator()
        {
        }


    }
}
