using System;

namespace func_rocket;

public class ForcesTask
{
	public static RocketForce GetThrustForce(double forceValue)
		=> r => new Vector(
			Math.Cos(r.Direction) * forceValue,
			Math.Sin(r.Direction) * forceValue);

	public static RocketForce ConvertGravityToForce(Gravity gravity, Vector spaceSize) 
		=> r => gravity(spaceSize, r.Location);


	public static RocketForce Sum(params RocketForce[] forces)
		=> r =>
		{
			var sum = Vector.Zero;
			foreach (var force in forces)
				sum += force(r);

			return sum;
		};
}