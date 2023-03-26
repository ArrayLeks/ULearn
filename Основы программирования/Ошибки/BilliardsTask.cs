using System;

namespace Billiards
{
    public static class BilliardsTask
    {
        public static double BounceWall(double directionRadians, double wallInclinationRadians)
        {
            double directionInclination = wallInclinationRadians - directionRadians;
            return directionInclination + wallInclinationRadians;
        }
    }
}