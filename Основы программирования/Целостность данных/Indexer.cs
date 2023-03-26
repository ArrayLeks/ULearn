using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketGoogle
{
    public class Indexer : IIndexer
    {
		Dictionary<string, Dictionary<int, List<int>>> indexesWord 
			= new Dictionary<string, Dictionary<int, List<int>>>();

        public void Add(int id, string documentText)
        {
            string[] words = 
                documentText.Split
					(new char[] { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0, j = 0; i < words.Length; i++)
            {
                if(i > 0) j += words[i - 1].Length + 1;
                if (indexesWord.ContainsKey(words[i])) 
                {
                    if (indexesWord[words[i]].ContainsKey(id)) 
                        indexesWord[words[i]][id].Add(documentText.IndexOf(words[i], j));
                    else
                    {
                        indexesWord[words[i]]
                            .Add(id, new List<int> { documentText.IndexOf(words[i], j) });
                    }
                }
                else
                {
                    indexesWord
                        .Add(words[i], 
							new Dictionary<int, List<int>> { { id, new List<int> { documentText.IndexOf(words[i], j) } } });
                }
            }
        }

        public List<int> GetIds(string word)
        {
            List<int> list = new List<int>();
            if (!indexesWord.ContainsKey(word)) return list;
            foreach (var dic in indexesWord[word])
                list.Add(dic.Key);
            return list;
        }

        public List<int> GetPositions(int id, string word)
        {
            if (!indexesWord.ContainsKey(word) || !indexesWord[word].ContainsKey(id)) 
                return new List<int>();
            return indexesWord[word][id];
        }

        public void Remove(int id)
        {
            foreach (var word in indexesWord)
            {
                if(word.Value.ContainsKey(id)) word.Value.Remove(id);
            }
        }
    }
}