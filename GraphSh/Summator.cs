using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphSh
{
    public class Summator: ISummator
    {
        private readonly Queue<Tuple<double, int>> toAdd = new Queue<Tuple<double, int>>();
        private double _probability;

        public bool IsReady { get; private set; }
        public double Answer { get; private set; }

        public Summator()
        {
            Answer = 0;
            _probability = 0;
            IsReady = false;
        }

        public void Add(double probability, int connected)
        {
            Task.Factory.StartNew(() => toAdd.Enqueue(new Tuple<double, int>(probability, connected)));
        }

        public void StartSummator()
        {
            while (true)
            {
                if (toAdd.Count > 0)
                {
                    lock (toAdd)
                    {
                        while (toAdd.Count > 0)
                        {

                            var pair = toAdd.Dequeue();
                            Answer += pair.Item1 * pair.Item2;
                            _probability += pair.Item1;
                            Console.WriteLine($"обновлено! Пока {_probability} \t {Answer}");
                        }
                        if (Math.Abs(_probability - 1) < 0.00001)
                        {
                            Console.WriteLine($"ответ {Answer}");
                            IsReady = true;
                            return;
                        }
                    }
                }
            }
        }
    }
}
