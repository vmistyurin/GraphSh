﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphSh
{
    class SmartSummator:ISummator
    {
        public bool IsReady { get; private set; }

        public double Answer { get; private set; }

        private double _probability;
        private readonly object locker = new object();

        public SmartSummator()
        {
            _probability = 0;
            IsReady = false;
            Answer = 0;
        }
        public void Add(double probability, int answer)
        {
            lock (locker)
            {
                _probability += probability;
                Answer += probability * answer;
                //Console.WriteLine($"обновлено! Пока {this._probability} \t {Answer}");

                if (_probability.Equal(1))
                {
                    IsReady = true;                    
                }
            }
        }

        public void StartSummator()
        {
            throw new NotImplementedException();
        }


    }
}
