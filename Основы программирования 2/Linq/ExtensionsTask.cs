using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace linq_slideviews;

public static class ExtensionsTask
{
	public static double Median(this IEnumerable<double> items)
	{
		var list = new List<double>();
		foreach (var item in items)
			list.Add(item);
		
		if (list.Count == 0) throw new InvalidOperationException();

        list.Sort();

        if (list.Count % 2 != 0) return list[list.Count / 2];
		else return (list[list.Count / 2 - 1] + list[list.Count / 2]) / 2;
    }

	public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items)
	{
		T prev = default(T);
		bool start = true;
		foreach (var item in items)
			if (start) { prev = item; start = false; }
			else
			{
				yield return (prev, item);
				prev = item;
			}
	}
}