using System.Collections.Generic;
using System.Linq;

namespace yield;

public static class ExpSmoothingTask
{
	public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
	{
        double prevPoint = 0;
		bool first = true;

		foreach (var point in data)
		{
			if(first) 
			{ 
				first = false; 
				prevPoint = point.OriginalY;  
			}
			
			double expSmoothedY = alpha * point.OriginalY + (1 - alpha) * prevPoint;
			yield return point.WithExpSmoothedY(expSmoothedY);
            prevPoint = expSmoothedY;
        }
	}
}