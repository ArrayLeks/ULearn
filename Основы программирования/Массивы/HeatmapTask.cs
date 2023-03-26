using System;

namespace Names
{
    internal static class HeatmapTask
    {
        public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
        {
            string[] xLabel = new string[30];
            string[] yLabel = new string[12];
            double[,] heat = new double[30, 12];
            FillLabelX(xLabel);
            FillLabelY(yLabel);
            FillHeatMap(names, heat);
            
            return new HeatmapData( "Пример карты интенсивностей", heat, xLabel, yLabel);
        }

        private static void FillLabelX(string[] array)
        {
            for (int i = 0; i < 30; i++)
                array[i] = $"{i + 2}";
        }

        private static void FillLabelY(string[] array)
        {
            for (int i = 0; i < 12; i++)
                array[i] = $"{i + 1}";
        }

        private static void FillHeatMap(NameData[] names, double[,] heat)
        {
            foreach(var name in names) 
            {
                if (name.BirthDate.Day != 1) heat[name.BirthDate.Day - 2, name.BirthDate.Month - 1]++;
            }
        }
    }
}