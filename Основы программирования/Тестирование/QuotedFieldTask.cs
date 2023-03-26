using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        [TestCase(@"""a 'b' 'c' d"" '""1"" ""2"" ""3""'", 0, "a 'b' 'c' d", 13)]
        [TestCase(@"""a 'b' 'c' d"" '""1"" ""2"" ""3""'", 14, @"""1"" ""2"" ""3""", 13)]
        [TestCase(@"""", 0, "", 1)]
        [TestCase(@"'", 0, "", 1)]
        [TestCase(@"'' ""bcd ef"" 'x y'", 0, "", 2)]
        [TestCase(@"'' ""bcd ef"" 'x y'", 3, "bcd ef", 8)]
        [TestCase(@"'' ""bcd ef"" 'x y'", 12, "x y", 5)]
        [TestCase(@"a""b c d e""f", 1, "b c d e", 9)]
        [TestCase(@"abc ""def g h", 4, "def g h", 8)]
        [TestCase(@"""a \""c\""", 0, "a \"c\"", 8)]
        [TestCase(@"""\\"" b", 3, " b", 3)]
        [TestCase(@"\""a b\""", 1, @"a b""", 6)]
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            int count = 0;
            var value = new StringBuilder();
            for (int i = startIndex; i < line.Length; i++)
            {
                count++;
                if (line[startIndex] == '"')
                {
                    if (i > startIndex && line[i] == '"' && line[i - 1] != '\\')
                        break;
                }
                else
                {
                    if (i > startIndex && line[i] == '\'' && line[i - 1] != '\\')
                        break;
                }
                if (line[i] != '\\' && i != startIndex) value.Append(line[i]);
            }
            
            return new Token(value.ToString(), startIndex, count);
        }
    }
}
