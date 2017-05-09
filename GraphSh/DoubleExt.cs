using System;

namespace GraphSh
{
    public static class DoubleExt
    {
        public static bool Equal(this double d, double compare)
        {
            return Math.Abs(d - compare) < 0.000000000001;
        }
    }
}
