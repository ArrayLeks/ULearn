using Inheritance.Geometry.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using static Inheritance.Geometry.Virtual.Body;

namespace Inheritance.Geometry.Virtual
{
    public abstract class Body
    {
        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }

        public abstract bool ContainsPoint(Vector3 point);

        public abstract RectangularCuboid GetBoundingBox();
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var vector = point - Position;
            var length2 = vector.GetLength2();
            return length2 <= Radius * Radius;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            return new RectangularCuboid(Position, Radius * 2, Radius * 2, Radius * 2);
        }
    }

    public class RectangularCuboid : Body
    {
        public double SizeX { get; }
        public double SizeY { get; }
        public double SizeZ { get; }

        public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var minPoint = new Vector3(
                    Position.X - SizeX / 2,
                    Position.Y - SizeY / 2,
                    Position.Z - SizeZ / 2);
            var maxPoint = new Vector3(
                Position.X + SizeX / 2,
                Position.Y + SizeY / 2,
                Position.Z + SizeZ / 2);

            return point >= minPoint && point <= maxPoint;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            return this;
        }
    }

    public class Cylinder : Body
    {
        public double SizeZ { get; }

        public double Radius { get; }

        public Cylinder(Vector3 position, double sizeZ, double radius) : base(position)
        {
            SizeZ = sizeZ;
            Radius = radius;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var vectorX = point.X - Position.X;
            var vectorY = point.Y - Position.Y;
            var length2 = vectorX * vectorX + vectorY * vectorY;
            var minZ = Position.Z - SizeZ / 2;
            var maxZ = minZ + SizeZ;
            return length2 <= Radius * Radius && point.Z >= minZ && point.Z <= maxZ;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            return new RectangularCuboid(Position, Radius * 2, Radius * 2, SizeZ);
        }
    }

    public class CompoundBody : Body
    {
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            return Parts.Any(body => body.ContainsPoint(point));
        }

        public override RectangularCuboid GetBoundingBox()
        {
			var listX = new List<double>();
            var listY = new List<double>();
            var listZ = new List<double>();

            foreach (var body in Parts.Select(s => s.GetBoundingBox()))
            {
                var item = body as RectangularCuboid;
                listX.Add(item.Position.X - (item.SizeX / 2));
                listX.Add(listX.Last() + item.SizeX);
                listY.Add(item.Position.Y - (item.SizeY / 2));
                listY.Add(listY.Last() + item.SizeY);
                listZ.Add(item.Position.Z - (item.SizeZ / 2));
                listZ.Add(listZ.Last() + item.SizeZ);
            }

            var sizeX = listX.Max() - listX.Min();
            var sizeY = listY.Max() - listY.Min();
            var sizeZ = listZ.Max() - listZ.Min();

            var center = new Vector3(listX.Max() - (sizeX / 2), listY.Max() - (sizeY / 2), listZ.Max() - (sizeZ / 2));

            return new RectangularCuboid(center, sizeX, sizeY, sizeZ);
        }
    }
}