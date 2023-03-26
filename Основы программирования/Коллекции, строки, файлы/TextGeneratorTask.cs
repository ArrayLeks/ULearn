using System.Collections.Generic;
using System;
using System.Linq;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        private static string GetNextWord(string total,
            Dictionary<string, string> nextWords)
        {
            var start = total.Split(' ').Skip(total.Split(' ').Count() - 2);
            var startBigram = start.LastOrDefault();
            var startTrigram = start.Count() > 1 ? start.FirstOrDefault() + " " + startBigram : "";
            return nextWords.ContainsKey(startTrigram) ? total + " " + nextWords[startTrigram] :
                nextWords.ContainsKey(startBigram) ? total + " " + nextWords[startBigram] : total;
        }

        public static string ContinuePhrase(Dictionary<string, string> nextWords,
            string phraseBeginning, int wordsCount) =>
            Enumerable.Repeat("", wordsCount)
                .Aggregate(string.Join(" ", phraseBeginning
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)),
                (total, next) => GetNextWord(total, nextWords));
    }
}