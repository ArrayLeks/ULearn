
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Policy;

namespace Inheritance.Geometry.Visitor
{
    public interface IVisitor
    {
        Body Visit(Ball ball);
        Body Visit(RectangularCuboid rectangularCuboid);
        Body Visit(Cylinder cylinder);
        Body Visit(CompoundBody compoundBody);
    }

    public abstract class Body
    {
        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }

        public abstract Body Accept(IVisitor visitor);
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }

        public override Body Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
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

        public override Body Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
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

        public override Body Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class CompoundBody : Body
    {
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }

        public override Body Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class BoundingBoxVisitor : IVisitor
    {
        public Body Visit(Ball ball)
        {
            return new RectangularCuboid
                (ball.Position, ball.Radius * 2, ball.Radius * 2, ball.Radius * 2);
        }

        public Body Visit(RectangularCuboid rectangularCuboid)
        {
            return rectangularCuboid;
        }

        public Body Visit(Cylinder cylinder)
        {
            return new RectangularCuboid
                (cylinder.Position, cylinder.Radius * 2, cylinder.Radius * 2, cylinder.SizeZ);
        }

        public Body Visit(CompoundBody compoundBody)
        {
            var listX = new List<double>();
            var listY = new List<double>();
            var listZ = new List<double>();

            foreach (var body in compoundBody.Parts.Select(s => s.Accept(this)))
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

    public class BoxifyVisitor : IVisitor
    {
        public Body Visit(Ball ball)
        {
            return new RectangularCuboid
                (ball.Position, ball.Radius * 2, ball.Radius * 2, ball.Radius * 2);
        }

        public Body Visit(RectangularCuboid rectangularCuboid)
        {
            return rectangularCuboid;
        }

        public Body Visit(Cylinder cylinder)
        {
            return new RectangularCuboid
                (cylinder.Position, cylinder.Radius * 2, cylinder.Radius * 2, cylinder.SizeZ);
        }

        public Body Visit(CompoundBody compoundBody)
        {
            return new CompoundBody(
                compoundBody.Parts
                .Select(s => s.Accept(this))
                .ToList());
        }
    }
}