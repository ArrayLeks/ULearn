using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class BfsTask
{
	public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
	{
		var queue = new Queue<SinglyLinkedList<Point>>();
		var visited = new HashSet<Point>();
		var myChest = new HashSet<Point>(chests);
		queue.Enqueue(new SinglyLinkedList<Point>(start, null));
		while (queue.Count > 0)
		{
			var point = queue.Dequeue();
			if (CheckPath(map, point.Value, visited)) continue;
			if (myChest.Contains(point.Value))
			{
				myChest.Remove(point.Value);
				yield return point;
			}
            visited.Add(point.Value);

			if(myChest.Count < 1) yield break;
			FindNextPoints(queue, point);
		}
	}

	public static bool CheckPath(Map map, Point point, HashSet<Point> visited)
	{
		return !map.InBounds(point) || visited.Contains(point)
                || map.Dungeon[point.X, point.Y] == MapCell.Wall;
    }

	public static void FindNextPoints(Queue<SinglyLinkedList<Point>> queue, SinglyLinkedList<Point> point)
	{
        for (var dy = -1; dy <= 1; dy++)
            for (var dx = -1; dx <= 1; dx++)
            {
                if (dy != 0 && dx != 0) continue;
                else queue.Enqueue(
					new SinglyLinkedList<Point>(new Point(point.Value.X + dx, point.Value.Y + dy), point));
            }
    }
}