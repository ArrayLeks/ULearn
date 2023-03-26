using System;

namespace DistanceTask
{
    public static class DistanceTask
    {
        // Расстояние от точки (x, y) до отрезка AB с координатами A(ax, ay), B(bx, by)
        public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y)
        {
            double da = Math.Sqrt((ax - bx) * (ax - bx) + (ay - by) * (ay - by));
            double db = Math.Sqrt((bx - x) * (bx - x) + (by - y) * (by - y));
            double dc = Math.Sqrt((ax - x) * (ax - x) + (ay - y) * (ay - y));

            if ((dc * dc) > (da * da) + (db * db) || (db * db) > (da * da) + (dc * dc) || da == 0)
            {
                return Math.Min(db, dc);
            }
            else
            {
                double a = by - ay;
                double b = ax - bx;
                double c = (-ax * (by - ay)) + (ay * (bx - ax));
                return Math.Abs(a * x + b * y + c) / Math.Sqrt(a * a + b * b);
            }
        }
    }
}