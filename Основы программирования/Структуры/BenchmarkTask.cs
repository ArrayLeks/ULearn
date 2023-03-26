using System;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;
using NUnit;


namespace StructBenchmarking
{
    public class Benchmark : IBenchmark
	{
        public double MeasureDurationInMs(ITask task, int repetitionCount)
        {
            GC.Collect();                   
            GC.WaitForPendingFinalizers();
            task.Run();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < repetitionCount ; i++)
            {
                task.Run();
            }

            stopwatch.Stop();
            return stopwatch.Elapsed.TotalMilliseconds / repetitionCount;
		}
	}

    [TestFixture]
    public class RealBenchmarkUsageSample
    {
        [Test]
        public void StringConstructorFasterThanStringBuilder()
        {
            var builderTest = new BuilderTest();
            var stringTest = new StringTest();
            var benchmark = new Benchmark();

            var firstTest = benchmark.MeasureDurationInMs(stringTest, 10000);
            var secondTest = benchmark.MeasureDurationInMs(builderTest, 10000);
            Assert.Less(firstTest, secondTest);
        }
    }

    public class BuilderTest : ITask
    {
        public void Run()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 10000; i++)
            {
                builder.Append("abc");
            }
            builder.ToString();
        }
    }

    public class StringTest : ITask
    {
        public void Run()
        {
            string str = new string('a', 10000);
        }
    }
}