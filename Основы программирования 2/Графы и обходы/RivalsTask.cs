using System.Collections.Generic;

namespace Rivals;

public class RivalsTask
{
	public static IEnumerable<OwnedLocation> AssignOwners(Map map)
	{
		var queue = new Queue<OwnedLocation>();
		var visited = new HashSet<Point>();
		int owner = 0;
		foreach (var player in map.Players)
			queue.Enqueue(new OwnedLocation(owner++, player, 0));

		while (queue.Count > 0)
		{
			var owned = queue.Dequeue();
			if (CheckIncorrectPath(map, owned.Location, visited)) continue;
			yield return owned;

			visited.Add(owned.Location);

			FindNextPoint(queue, owned);
		}
	}

	private static void FindNextPoint(Queue<OwnedLocation> queue, OwnedLocation owned)
	{
		for (int dy = -1; dy <= 1; dy++)
			for (int dx = -1; dx <= 1; dx++)
			{
				if (dy != 0 && dx != 0) continue;
				else queue.Enqueue(new OwnedLocation(owned.Owner,
					new Point(owned.Location.X + dx, owned.Location.Y + dy),
					owned.Distance + 1
					));
			}
	}

	private static bool CheckIncorrectPath(Map map, Point point, HashSet<Point> visited)
	{
        return !map.InBounds(point) || visited.Contains(point)
                || map.Maze[point.X, point.Y] == MapCell.Wall;
    }
}