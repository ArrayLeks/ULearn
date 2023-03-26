using System;

namespace GeometryTasks
{
    class Vector
    {
        public double X;
        public double Y;
    }

    class Segment
    {
        public Vector Begin;
        public Vector End;
    }

    class Geometry
    {
        public static double GetLength(Vector vector)
        {
            return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static double GetLength(Segment segment)
        {
            return Math.Sqrt(
                ((segment.Begin.X - segment.End.X) * (segment.Begin.X - segment.End.X))
                + ((segment.Begin.Y - segment.End.Y) * (segment.Begin.Y - segment.End.Y)));
        }

        public static bool IsVectorInSegment(Vector vector, Segment segment)
        {
            return GetLength(new Segment { Begin = segment.Begin, End = vector })
            	+ GetLength(new Segment { Begin = vector, End = segment.End })
            	== GetLength(segment);
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