using System;
using System.Collections.Generic;
using System.Linq;

namespace Delegates.PairsAnalysis
{
    public static class Analysis
    {
        public static int FindMaxPeriodIndex(params DateTime[] data)
        {
            if (data.Length < 2) throw new InvalidOperationException();
            return data.Pairs().Select(s => s.Item2 - s.Item1).MaxIndex();
        }

        public static double FindAverageRelativeDifference(params double[] data)
        {
            if (data.Length < 2) throw new InvalidOperationException();
            return data.Pairs().Average(s => (s.Item2 - s.Item1) / s.Item1);
        }

        public static IEnumerable<Tuple<T,T>> Pairs<T>(this IEnumerable<T> items)
        {
            T prev = default(T);
            bool start = true;
            foreach (var item in items)
                if (start) { prev = item; start = false; }
                else
                {
                    yield return new Tuple<T,T>(prev,item);
                    prev = item;
                }
        }

        public static int MaxIndex<T>(this IEnumerable<T> items)
            where T : IComparable<T>
        {
            T max = default(T);
            bool start = true;
            int index = 0, maxIndex = 0;
            foreach (var item in items)
            {
                if(start) { max = item; start = false; }
                else if (max.CompareTo(item) <= 0)
                {
                    max = item;
                    maxIndex = index;
                }
                index++;
            }
            if (start) throw new InvalidOperationException();
            return maxIndex;
        }
    }
}