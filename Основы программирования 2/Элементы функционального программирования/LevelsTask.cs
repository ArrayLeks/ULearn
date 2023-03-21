using System;
using System.Collections.Generic;

namespace func_rocket;

public class LevelsTask
{
	static readonly Physics standardPhysics = new();

	static readonly Vector standardTarget = new Vector(600, 200);

	static readonly Rocket standartRocket =
		new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);

	static readonly Gravity whiteGravity = (size, v) =>
	{
		var d = (standardTarget - v).Length;
		return (standardTarget - v).Normalize() * (-140 * d / (d * d + 1));
	};

	static readonly Gravity blackGravity = (size, v) =>
	{
		var blackHolePosition = new Vector(
			(standardTarget.X + standardTarget.Y) / 2,
			(standartRocket.Location.X + standartRocket.Location.Y) / 2);
		var d = (blackHolePosition - v).Length;
		return
		new Vector(blackHolePosition.X - v.X, blackHolePosition.Y - v.Y).Normalize() *
		(300 * d / (d * d + 1));
	};

    public static IEnumerable<Level> CreateLevels()
	{
		yield return new Level("Zero", 
			standartRocket,
			standardTarget, 
			(size, v) => Vector.Zero,
			standardPhysics);

		yield return new Level("Heavy",
			standartRocket,
			standardTarget,
			(size, v) => new Vector(0, 0.9),
			standardPhysics);

		yield return new Level("Up",
			standartRocket,
			new Vector(700, 500),
			(size, v) => new Vector(0, - 300 / (size.Y - v.Y + 300)),
			standardPhysics);

		yield return new Level("WhiteHole",
			standartRocket,
			standardTarget,
			whiteGravity,
			standardPhysics);

		yield return new Level("BlackHole",
			standartRocket,
			standardTarget,
			blackGravity, 
			standardPhysics);

		yield return new Level("BlackAndWhite",
			 standartRocket,
			 standardTarget,
			 (size, v) => (whiteGravity(size, v) + blackGravity(size, v)) / 2,
			 standardPhysics);
	}
}