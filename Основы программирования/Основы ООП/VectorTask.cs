using System;

namespace GeometryTasks
{
    class Vector
    {
        public double X;
        public double Y;
    }

    class Geometry
    {
        public static double GetLength(Vector vector)
        {
            return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static Vector Add(Vector first, Vector second)
        {
            Vector result = new Vector();
            result.X = first.X + second.X;
            result.Y = first.Y + second.Y;
            return result;
        }
    }
}