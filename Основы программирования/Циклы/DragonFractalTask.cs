using System.Drawing;
using System;

namespace Fractals
{
	internal static class DragonFractalTask
	{
		public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed)
		{
			double x = 1.0d;
			double y = 0.0d;
			var random = new Random(seed);
			
			for(int i = 0; i < iterationsCount; i++)
			{
				var numTransform = random.Next(2);
				if(numTransform == 0) FirstTransform(ref x, ref y);
				else SecondTransform(ref x, ref y);
				pixels.SetPixel(x, y);
			}
		}

        static void FirstTransform(ref double x, ref double y)
		{
			double temp = 0;
			temp = (x * (Math.Cos(Math.PI / 4)) - y * Math.Sin(Math.PI / 4)) / Math.Sqrt(2);
            y = (x * (Math.Sin(Math.PI / 4)) + y * Math.Cos(Math.PI / 4)) / Math.Sqrt(2);
			x = temp;
        }

        static void SecondTransform(ref double x, ref double y)
        {
            double temp = 0;
            temp = (x * (Math.Cos(3 * Math.PI / 4)) - y * Math.Sin(3 * Math.PI / 4)) / Math.Sqrt(2) + 1;
            y = (x * (Math.Sin(3 * Math.PI / 4)) + y * Math.Cos(3 * Math.PI / 4)) / Math.Sqrt(2);
            x = temp;
        }
    }
}