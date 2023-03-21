using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;

namespace yield;

public static class MovingMaxTask
{
	public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
	{
		var list = new LinkedList<double>();
		var queue = new Queue<double>();
		
		foreach (var item in data)
		{
			queue.Enqueue(item.OriginalY);
			if(queue.Count > windowWidth)
			{
				if (list.First.Value == queue.Peek())
				{
					list.RemoveFirst();
				}
				queue.Dequeue();
			}

			while (list.Count != 0 && list.Last.Value < item.OriginalY)
				list.RemoveLast();

			list.AddLast(item.OriginalY);

            yield return item.WithMaxY(list.First.Value);
		}
	}
}