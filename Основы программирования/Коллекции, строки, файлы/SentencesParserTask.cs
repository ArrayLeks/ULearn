using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static List<List<string>> ParseSentences(string text) =>
            text.ToLower().Split(
                new string[] { ".", "?", "!", ":", ")", "(", ";" }, StringSplitOptions.RemoveEmptyEntries)
            .Select( sent => SplitWords(sent)).Where(sent => sent.Count != 0).ToList();
        
        private static List<string> SplitWords(string sent) =>
            new string(GetWordWithoutSymbols(sent))
            .Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();

        private static char[] GetWordWithoutSymbols(string word) =>
            word.Select(s => char.IsLetter(s) || s == '\'' ? s : ' ').ToArray();
    }
}