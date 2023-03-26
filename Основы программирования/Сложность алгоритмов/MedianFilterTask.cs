using System;
using System.Collections.Generic;

namespace Recognizer
{
    internal static class MedianFilterTask
    {
        public static double[,] MedianFilter(double[,] original)
        {
            var lenthFirst = original.GetLength(0);
            var lenthSecond = original.GetLength(1);
            var result = new double[lenthFirst, lenthSecond];
            for (int i = 0; i < lenthFirst; i++)
                for (int j = 0; j < lenthSecond; j++)
                {
                    var list = new List<double>();
                    list.Add(original[i, j]);
                    if(i - 1 >= 0) list.Add(original[i - 1, j]);
                    if(i + 1 < lenthFirst) list.Add(original[i + 1, j]);
                    if(j - 1 >= 0) list.Add(original[i, j - 1]);
                    if(j + 1 < lenthSecond) list.Add(original[i, j + 1]);
                    if(i - 1 >= 0 && j - 1 >= 0) list.Add(original[i - 1, j - 1]);
                    if (i + 1 < lenthFirst && j + 1 < lenthSecond) list.Add(original[i + 1, j + 1]);
                    if (i - 1 >= 0 && j + 1 < lenthSecond) list.Add(original[i - 1, j + 1]);
                    if(i + 1 < lenthFirst && j - 1 >= 0)list.Add(original[i + 1, j - 1]);
                    list.Sort();
                    if(list.Count % 2 == 0)
                        result[i, j] = (list[(list.Count / 2) - 1] + list[(list.Count / 2)]) / 2;
                    else
                        result[i, j] = list[(list.Count / 2)];
                }
            return result;
        }
    }
}