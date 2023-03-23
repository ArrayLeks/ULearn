using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class ParsingTask
{
	public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
	{
		var dict = new Dictionary<int, SlideRecord>();
		foreach (var line in lines)
		{
			string[] array = line.Split(';');
			if (array.Length < 3) continue;
			if (Enum.TryParse(array[1], true, out SlideType slideType))
			{
				if (int.TryParse(array[0], out int id) && !dict.ContainsKey(id))
					dict.Add(id, new SlideRecord(id, slideType, array[2]));
			}
		}
		return dict;
	}

	
	public static IEnumerable<VisitRecord> ParseVisitRecords(
		IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
	{
		var list = new List<VisitRecord>();
		bool start = true;
		foreach (var line in lines)
		{
			if (start) { start = false; continue; }
			string[] array = line.Split(';');
            if (int.TryParse(array[0], out int userId) 
				&& int.TryParse(array[1], out int slideId)
				&& DateTime.TryParse($"{array[2]} {array[3]}", out DateTime dateTime)
				&& slides.ContainsKey(slideId))
			{
				list.Add(new VisitRecord(userId, slideId, dateTime, slides[slideId].SlideType));
			}
			else
				throw new FormatException($"Wrong line [{line}]");
        }

		return list;
	}
}