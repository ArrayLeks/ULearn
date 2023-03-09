using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Delegates.Reports
{
	public interface IReport
	{
		DesignReport designReport { get; }
		CalculateIndicators calculateIndicators { get; }
	}

    public class DesignReport
    {
        public string Caption { get; private set; }
        public string BeginList { get; private set; }
        public string EndList { get; private set; }

        public DesignReport(string caption, string beginList, string endList)
        {
            Caption = caption;
            BeginList = beginList;
            EndList = endList;
        }
    }

    public class CalculateIndicators
    {
        public Func<string, string> MakeCaption;
        public Func<string, string, string> MakeItem;
        public Func<IEnumerable<double>, object> MakeStatistics;

        public CalculateIndicators(
            Func<string, string> makeCaption,
            Func<string, string, string> makeItem,
            Func<IEnumerable<double>, object> makeStatistics)
        {
            MakeCaption = makeCaption;
            MakeItem = makeItem;
            MakeStatistics = makeStatistics;
        }
    }

    public class ReportMaker<TParameters>
		where TParameters : IReport, new()
	{
		public TParameters Report = new TParameters();
		public string MakeReport(IEnumerable<Measurement> measurements)
		{
			var data = measurements.ToList();
			var result = new StringBuilder();
			result.Append(Report.calculateIndicators.MakeCaption(Report.designReport.Caption));
			result.Append(Report.designReport.BeginList);
			result.Append(Report.calculateIndicators.MakeItem("Temperature", 
				Report.calculateIndicators.MakeStatistics(data.Select(z => z.Temperature)).ToString()));
			result.Append(Report.calculateIndicators.MakeItem("Humidity", 
				Report.calculateIndicators.MakeStatistics(data.Select(z => z.Humidity)).ToString()));
			result.Append(Report.designReport.EndList);
			return result.ToString();
		}
	}

	public class MeanAndStdHtmlReportMaker : IReport
	{
		public DesignReport designReport => new DesignReport("Mean and Std", "<ul>", "</ul>");

        public CalculateIndicators calculateIndicators => 
			new CalculateIndicators(
				caption => $"<h1>{caption}</h1>",
				(valueType, entry) => $"<li><b>{valueType}</b>: {entry}",
				_data =>
				{
                    var data = _data.ToList();
                    var mean = data.Average();
                    var std = Math.Sqrt(data.Select(z => Math.Pow(z - mean, 2)).Sum() / (data.Count - 1));

                    return new MeanAndStd
                    {
                        Mean = mean,
                        Std = std
                    };
                }
                );
	}

	public class MedianMarkdownReportMaker : IReport 
	{
        public DesignReport designReport => new DesignReport("Median", "", "");

        public CalculateIndicators calculateIndicators =>
            new CalculateIndicators(
                caption => $"## {caption}\n\n",
                (valueType, entry) => $" * **{valueType}**: {entry}\n\n",
                data =>
                {
                    var list = data.OrderBy(z => z).ToList();
                    if (list.Count % 2 == 0)
                        return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;

                    return list[list.Count / 2];
                }
                );
	}

    public class MeanAndStdMarkdownReport : IReport
    {
        public DesignReport designReport => new DesignReport("Mean and Std", "", "");

        public CalculateIndicators calculateIndicators =>
            new CalculateIndicators(
                caption => $"## {caption}\n\n",
                (valueType, entry) => $" * **{valueType}**: {entry}\n\n",
                _data =>
                {
                    var data = _data.ToList();
                    var mean = data.Average();
                    var std = Math.Sqrt(data.Select(z => Math.Pow(z - mean, 2)).Sum() / (data.Count - 1));

                    return new MeanAndStd
                    {
                        Mean = mean,
                        Std = std
                    };
                }
                );
    }

    public class MedianHtmlReport : IReport
    {
        public DesignReport designReport => new DesignReport("Median", "<ul>", "</ul>");

        public CalculateIndicators calculateIndicators =>
            new CalculateIndicators(
                caption => $"<h1>{caption}</h1>",
                (valueType, entry) => $"<li><b>{valueType}</b>: {entry}",
                data =>
                {
                    var list = data.OrderBy(z => z).ToList();
                    if (list.Count % 2 == 0)
                        return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;

                    return list[list.Count / 2];
                }
                );
    }

    public static class ReportMakerHelper
	{
		public static string MeanAndStdHtmlReport(IEnumerable<Measurement> data)
		{
			return new ReportMaker<MeanAndStdHtmlReportMaker>().MakeReport(data);
		}

		public static string MedianMarkdownReport(IEnumerable<Measurement> data)
		{
			return new ReportMaker<MedianMarkdownReportMaker>().MakeReport(data);
		}

		public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> measurements)
		{
			return new ReportMaker<MeanAndStdMarkdownReport>().MakeReport(measurements);
		}

		public static string MedianHtmlReport(IEnumerable<Measurement> measurements)
		{
            return new ReportMaker<MedianHtmlReport>().MakeReport(measurements);
		}
	}
}