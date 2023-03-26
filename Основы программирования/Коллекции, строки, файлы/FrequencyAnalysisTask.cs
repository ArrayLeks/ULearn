using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
            => GetNGrams(text, 2)
                .Concat(GetNGrams(text, 3))
                .ToDictionary(k => k.Key, v => v.Value);

        public static Dictionary<string, string> GetNGrams(List<List<string>> text, int n)
            => text.SelectMany(sentence => GetNGramsInSentence(sentence, n))
                .GroupBy(key => key.Item1, value => value.Item2)
                .ToDictionary(group => group.Key, group => group.GroupBy(e => e)
                .ToDictionary(k => k.Key, v => v.Count()))
                .Select(pair => (pair.Key, GetPairPriorityNGram(pair.Value)))
                .ToDictionary(pair => pair.Key, pair => pair.Item2);

        public static List<(string, string)> GetNGramsInSentence(List<string> sentence, int n)
        {
            var res = new List<(string, string)>();
            for (int i = 0; i < sentence.Count - n + 1; i++)
            {
                var startNGram = string.Join(" ", sentence.GetRange(i, n - 1));
                var endNGram = sentence[i + n - 1];

                res.Add((startNGram, endNGram));
            }
            return res;
        }

        public static string GetPairPriorityNGram(Dictionary<string, int> ends)
            => ends.OrderByDescending(pair => pair.Value)
                .ThenBy(e => e.Key, StringComparer.Ordinal)
                .First().Key;
    }
}