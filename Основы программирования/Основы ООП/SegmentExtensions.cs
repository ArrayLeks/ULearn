using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryTasks;

namespace GeometryPainting
{
	public static class SegmentExtensions
	{
		public static Dictionary<Segment, Color> Dicionary = new Dictionary<Segment, Color>();

		public static void SetColor(this Segment segment, Color color)
		{
			if (color == null) color = Color.Black;
			Dicionary[segment] = color;
		}

		public static Color GetColor(this Segment segment)
		{
			if (!Dicionary.ContainsKey(segment)) return Color.Black;
			else return Dicionary[segment];
		}
	}
}