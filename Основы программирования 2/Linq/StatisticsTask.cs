using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
	public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
	{
		var time = visits.DefaultIfEmpty()
			.GroupBy(visit => visit?.UserId)
			.Select(group => group.OrderBy(visit => visit?.DateTime))
			.Select(orderedVisit => ExtensionsTask.Bigrams(orderedVisit))
			.Select(bigramms => bigramms.Where(b => b.First.SlideType == slideType)
				.Select(bigramm => (bigramm.Second.DateTime - bigramm.First.DateTime).TotalMinutes))
			.SelectMany(time => time)
			.Where(time => time >= 1 && time <= 120);

		return time.Count() == 0 ? 0 : ExtensionsTask.Median(time);
	}
}