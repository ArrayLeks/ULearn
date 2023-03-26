using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class DungeonTask
{
	public static MoveDirection[] FindShortestPath(Map map)
	{
        var directPath = BfsTask
            .FindPaths(map, map.InitialPosition, new Point[] { map.Exit })
            .Select(s => s.ToArray())
            .OrderBy(s => s.Length)
            .FirstOrDefault();
        if (directPath != null)
        {
            foreach (var path in directPath)
                if (map.Chests.Contains(path))
                {
                    Array.Reverse(directPath);
                    return TranslatePointToDirection(directPath);
                }
        }

        var pathFromStart = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);
		var pathToFinish = BfsTask.FindPaths(map, map.Exit, map.Chests);

        Point[] shorterPath = FindPathWithChests(pathFromStart, pathToFinish, map);

		if (shorterPath.Length > 0) 
            return TranslatePointToDirection(shorterPath);

        if (directPath != null)
        {
            Array.Reverse(directPath);
            return TranslatePointToDirection(directPath);
        }
		return new MoveDirection[0];
	}

    private static MoveDirection[] TranslatePointToDirection(Point[] path)
    {
        MoveDirection[] result = new MoveDirection[path.Length - 1];
        for (int i = 0; i < path.Length - 1; i++)
        {
            result[i] = offsetToDirection[path[i + 1] - path[i]];
        }
        return result;
    }

    private static Point[] FindPathWithChests(IEnumerable<SinglyLinkedList<Point>> pathFromStart,
        IEnumerable<SinglyLinkedList<Point>> pathToFinish, Map map)
	{
        int lengthPath = int.MaxValue;
        Point[] shorterPath = new Point[0];
        foreach (var start in pathFromStart)
        {
            if (start.Length > lengthPath) continue;
            foreach (var finish in pathToFinish)
            {
                if (start.Value == finish.Value && start.Length + finish.Length - 1 < lengthPath)
                {
                    var temp = new Point[start.Length + finish.Length - 1];
                    int index = 0;
                    foreach (var item in start.Reverse())
                        temp[index++] = item;
                    bool firstItem = true;
                    foreach (var item in finish)
                    {
                        if (firstItem) { firstItem = false; continue; }
                        temp[index++] = item;
                    }
                    lengthPath = temp.Length;
                    shorterPath = temp;
                }
            }
        }
        return shorterPath;
    }
	
	private static readonly Dictionary<Point, MoveDirection> offsetToDirection = new()
    {
        { new Point(0, -1), MoveDirection.Up },
        { new Point(0, 1), MoveDirection.Down },
        { new Point(-1, 0), MoveDirection.Left },
        { new Point(1, 0), MoveDirection.Right }
    };
}