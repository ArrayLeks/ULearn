using System;

namespace GeometryTasks
{
    class Vector
    {
        public double X;
        public double Y;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public Vector Add(Vector vector)
        {
            return Geometry.Add(this, vector);
        }

        public bool Belongs(Segment segment)
        {
            return Geometry.IsVectorInSegment(this, segment);
        }
    }

    class Segment
    {
        public Vector Begin;
        public Vector End;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public bool Contains(Vector vector)
        {
            return Geometry.IsVectorInSegment(vector, this);
        }
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