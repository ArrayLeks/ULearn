using System;
using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class NotGreedyPathFinder : IPathFinder
{
    public List<Point> FindPathToCompleteGoal(State state)
    {
        var notVisitedTarget = state.Chests.ToList();
        var start = state.Position;
        var resultPath = new PathWithCost(0, start);
        var bestPath = new PathWithTarget { Path = new List<Point>(), CountNotVisitedTarget = state.Chests.Count };
        CreatePath(state, start, notVisitedTarget, resultPath, bestPath);
        return bestPath.Path.Skip(1).ToList();
    }

	private void CreatePath(State state, Point start, 
		List<Point> notVisitedTarget, PathWithCost resultPath, PathWithTarget bestPath)
    {
        if (bestPath.CountNotVisitedTarget > notVisitedTarget.Count)
        {
            bestPath.Path = resultPath.Path.ToList();
            bestPath.CountNotVisitedTarget = notVisitedTarget.Count;
        }

        var searcher = new DijkstraPathFinder();
        var paths = searcher.GetPathsByDijkstra(state, start, notVisitedTarget);
        foreach (var path in paths)
        {
            var cost = path.Cost + resultPath.Cost;
            if (cost <= state.Energy)
            {
                notVisitedTarget.Remove(path.End);
                var way = resultPath.Path.ToList();
                way.AddRange(path.Path.Skip(1));
                CreatePath(state, path.End, notVisitedTarget, new PathWithCost(cost, way.ToArray()), bestPath);
                if (bestPath.CountNotVisitedTarget == 0) return;
                notVisitedTarget.Add(path.End);
            }
        }
    }
}

class PathWithTarget
{
    public List<Point> Path { get; set; }
    public int CountNotVisitedTarget { get; set; }
}