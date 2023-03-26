using System;

namespace AngryBirds
{
	public static class AngryBirdsTask
	{
		public static double FindSightAngle(double v, double distance)
		{
			return 0.5 * Math.Asin(9.8 * distance / (v * v));
		}
	}
}