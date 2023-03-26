using System;
using System.Linq;

namespace Names
{
    internal static class HistogramTask
    {
        public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
        {
            string[] xLabel = new string[31];
            double[] yValues = new double[31];
            FillXLabel(xLabel);
            FillYValue(yValues, names, name);

            return new HistogramData(
                string.Format("Рождаемость людей с именем '{0}'", name), 
                xLabel, 
                yValues);
        }

        private static void FillXLabel(string[] array)
        {
            array[0] = "1";
            for (int i = 0; i < 31; i++)
                array[i] = $"{i + 1}";
        }

        private static void FillYValue(double[] yValues, NameData[] names, string name)
        {
            for(int i = 0; i < names.Length; i++)
            {
                if (names[i].Name == name && names[i].BirthDate.Day != 1)
                {
                    yValues[names[i].BirthDate.Day - 1]++;
                }
            }
        }
    }
}