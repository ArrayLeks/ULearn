using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StructBenchmarking
{
    public class Experiments
    {
        public static ChartData BuildChartDataForArrayCreation(
            IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();

            for(int i = 0, j = 16; i < 6; i++, j *= 2)
            {
                double result = benchmark.MeasureDurationInMs(new StructArrayCreationTask(j), repetitionsCount);
                structuresTimes.Add(new ExperimentResult(j, result));
                result = benchmark.MeasureDurationInMs(new ClassArrayCreationTask(j), repetitionsCount);
                classesTimes.Add(new ExperimentResult(j, result));
            }

            return new ChartData
            {
                Title = "Create array",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }

        public static ChartData BuildChartDataForMethodCall(
            IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();

            for (int i = 0, j = 16; i < 6; i++, j *= 2)
            {
                double result = 
                    benchmark.MeasureDurationInMs(new MethodCallWithStructArgumentTask(j), repetitionsCount);
                structuresTimes.Add(new ExperimentResult(j, result));
                result = benchmark.MeasureDurationInMs(new MethodCallWithClassArgumentTask(j), repetitionsCount);
                classesTimes.Add(new ExperimentResult(j, result));
            }

            return new ChartData
            {
                Title = "Call method with argument",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }
    }
}