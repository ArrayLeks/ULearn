using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class GreedyPathFinder : IPathFinder
{
	public List<Point> FindPathToCompleteGoal(State state)
	{
		var result = new List<Point>();
        if (state.Chests.Count < state.Goal) return result;
        var pathFinder = new DijkstraPathFinder();
		var energy = state.Energy;
		var currentPos = state.Position;
		var targets = state.Chests;
		var count = 0;

		while(count != state.Goal)
		{
            var nearPath = pathFinder.GetPathsByDijkstra(state, currentPos, targets).FirstOrDefault();
            if (nearPath == null) break;
            energy -= nearPath.Cost;
            if (energy < 0) return new List<Point>();
            AddPathToResult(nearPath, result);
            if (result.Count > 0) currentPos = result.Last();
            count++;
            targets.Remove(currentPos);
        }

		return result;
	}

	private static void AddPathToResult(PathWithCost nearPath, List<Point> result)
	{
        bool first = true;
        foreach (var item in nearPath.Path)
            if (first) { 
                first = false;
                continue; }
            else result.Add(item);
    }
}