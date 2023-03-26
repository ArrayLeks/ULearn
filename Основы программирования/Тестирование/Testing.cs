[TestCase("hello world \\", new[] {"hello", "world", @"\"})]
[TestCase("\'", new[] {""})]
[TestCase("     a     ", new []{ "a"})]
[TestCase(@"""a ' b' 'c' d""", new[] {"a ' b' 'c' d"})]
[TestCase(@"'""1"" ""2"" ""3""'", new[] { @"""1"" ""2"" ""3""" })]
[TestCase("a 'b c d e'", new[] { "a", "b c d e" })]
[TestCase(@"""de ", new [] {"de "})]
[TestCase(@"""\""", new [] {@""""})]
[TestCase(@"""\\""", new [] {"\\"})]
[TestCase("'' 'x y'", new string[] {@"", "x y" } )]
[TestCase("a\"c\"", new [] {"a", "c"})]
[TestCase(@"", new string[0])]
[TestCase(@"""a b"" a", new [] {"a b", "a"})]
[TestCase(@"'a \''", new[] {"a \'"})]
public static void RunTests(string input, string[] expectedOutput)
{
    Test(input, expectedOutput);
}