using System;
using System.Collections.Generic;
using System.Linq;
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism;

public class LevenshteinCalculator
{
	public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
	{
        var results = new List<ComparisonResult>();
		var hash = new List<(DocumentTokens, DocumentTokens)>();

        foreach (DocumentTokens first in documents)
			foreach (DocumentTokens second in documents)
			{
				if (first == second) continue;
				if (hash.Contains((first, second)) || hash.Contains((second, first))) continue;
				
				results.Add(new ComparisonResult(first, second, CalculateDistant(first, second)));
				hash.Add((first, second));
			}

		return results.OrderBy(s => s.Distance).ToList();
	}

	private static double CalculateDistant(DocumentTokens first, DocumentTokens second)
	{
		var opt = new double[first.Count + 1, second.Count + 1];

		for (int i = 0; i <= first.Count; i++)
			opt[i, 0] = i;

		for (int i = 0; i <= second.Count; i++)
			opt[0, i] = i;

		for (int i = 1; i <= first.Count; i++)
			for (int j = 1; j <= second.Count; j++)
			{
				if (first[i - 1] == second[j - 1])
					opt[i, j] = opt[i - 1, j - 1];
				else
				{
					var replace = TokenDistanceCalculator.GetTokenDistance(first[i - 1], second[j - 1]);
					if (1 + opt[i - 1, j] >= replace + opt[i - 1, j - 1])
						opt[i, j] = replace + opt[i - 1, j - 1] <= 1 + opt[i, j - 1] ? replace + opt[i - 1, j - 1] : 1 + opt[i, j - 1];
					else
						opt[i, j] = 1 + opt[i - 1, j] <= 1 + opt[i, j - 1] ? 1 + opt[i - 1, j] : 1 + opt[i, j - 1];
				}
			}

		return opt[first.Count, second.Count];
	}
}