using System;
using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class DijkstraPathFinder
{
	public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start,
		IEnumerable<Point> targets)
	{
		var notVisited = new List<Point>() { start };
		var track = new Dictionary<Point, DijkstraData>();
		track[start] = new DijkstraData { Previous = new Point(-1, -1) , Price = 0};

		while (true)
		{
			Point toOpen = new Point(-1, -1);
			var neighbors = new List<Point>();

			toOpen = SelectOpenPoint(toOpen, track, notVisited);

            AddPointsAround(state, toOpen, neighbors, notVisited, track);

            if (toOpen == new Point(-1, -1)) yield break;
			if (targets.Contains(toOpen)) yield return CollectResult(track, toOpen);

			OpenPoint(state, toOpen, track, neighbors);
			
			notVisited.Remove(toOpen);
		}
	}

	private static Point SelectOpenPoint(Point toOpen, 
		Dictionary<Point, DijkstraData> track, List<Point> notVisited)
	{
        var bestPrice = double.PositiveInfinity;
        foreach (var item in notVisited)
        {
            if (track.ContainsKey(item) && track[item].Price < bestPrice)
            {
                bestPrice = track[item].Price;
                toOpen = item;
            }
        }
		return toOpen;
    }

	private static void OpenPoint (State state, Point toOpen, 
		Dictionary<Point, DijkstraData> track, List<Point> neighbors)
	{
        foreach (var item in neighbors)
        {
            var currentPrice = track[toOpen].Price + state.CellCost[item.X, item.Y];
            var nextNode = item;
            if (!track.ContainsKey(nextNode) || track[nextNode].Price > currentPrice)
                track[nextNode] = new DijkstraData { Previous = toOpen, Price = currentPrice };
        }
    }

	private static PathWithCost CollectResult(Dictionary<Point, DijkstraData> track, Point end)
	{
		var result = new List<Point>();
		int cost = track[end].Price;
		while(end != new Point(-1, -1))
		{
			result.Add(end);
			end = track[end].Previous;
		}
		result.Reverse();
		return new PathWithCost(cost, result.ToArray());
	}

	private static void AddPointsAround(State state, Point toOpen, List<Point> neighbors,
        List<Point> notVisited, Dictionary<Point, DijkstraData> track)
	{
        for (int dy = -1; dy <= 1; dy++)
            for (int dx = -1; dx <= 1; dx++)
                if (dy != 0 && dx != 0) continue;
                else
                {
                    var point = new Point(dx + toOpen.X, dy + toOpen.Y);
					if (state.InsideMap(point) &&
                        !state.IsWallAt(point) && !track.ContainsKey(point))
					{
						notVisited.Add(point);
						neighbors.Add(point);
					}
                }
    }
}

public class DijkstraData
{
    public Point Previous { get; set; }
    public int Price { get; set; }
}