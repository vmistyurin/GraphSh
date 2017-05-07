using System;

namespace GraphSh
{
    public sealed class Edge
    {
        public int FirstV { get; }
        public int SecondV { get; }

        public Edge(int firstV, int secondV)
        {
            if (firstV < secondV)
            {
                FirstV = firstV;
                SecondV = secondV;
            }
            else
            {
                FirstV = secondV;
                SecondV =  firstV;
            }
        }
        public override bool Equals(object obj)
        {
            var e = (Edge)obj;
            return (e?.FirstV == this.FirstV && e?.SecondV == this.SecondV) ? true : false;
        }

        public override int GetHashCode()
        {
            return (int)(Math.Pow(2.0,FirstV) * Math.Pow(3.0,SecondV));
        }
    }
}
