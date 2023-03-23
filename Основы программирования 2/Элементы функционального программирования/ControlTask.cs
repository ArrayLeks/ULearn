using System;

namespace func_rocket;

public class ControlTask
{
	private static double angle;
	
	public static Turn ControlRocket(Rocket rocket, Vector target)
	{
		var direction = new Vector(target.X - rocket.Location.X, target.Y - rocket.Location.Y);

		if(Math.Abs(direction.Angle - rocket.Direction) < 0.5 ||
			Math.Abs(direction.Angle - rocket.Velocity.Angle) < 0.5)
			angle = (direction.Angle - rocket.Direction + direction.Angle - rocket.Velocity.Angle) / 2;
		else
			angle = direction.Angle - rocket.Direction;
			
		return angle < 0 ? Turn.Left :
			angle > 0 ? Turn.Right : Turn.None;
	}
}