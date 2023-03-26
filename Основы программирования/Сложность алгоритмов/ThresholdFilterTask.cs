using System.Collections.Generic;

namespace Recognizer
{
	public static class ThresholdFilterTask
	{
		public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
		{
            var lenthFirst = original.GetLength(0);
            var lenthSecond = original.GetLength(1);
            var countPixels = lenthFirst * lenthSecond;
            var toWhiteCount = (int)(countPixels * whitePixelsFraction);
            var list = new List<double>(countPixels);

            foreach (var item in original)
                list.Add(item);
            
            list.Sort();
            double thresholdValue = toWhiteCount != 0 
                ? list[countPixels - toWhiteCount] : double.MaxValue;

            for (int i = 0; i < lenthFirst; i++)
                for (int j = 0; j < lenthSecond; j++)
                    original[i, j] = original[i, j] >= thresholdValue ? 1.0d : 0.0d;

            return original;
        }
	}
}