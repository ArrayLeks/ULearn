using System;

namespace Rectangles
{
	public static class RectanglesTask
	{
		public static bool AreIntersected(Rectangle r1, Rectangle r2)
		{
			return !(r1.Left > r2.Right || r2.Left > r1.Right || r1.Top > r2.Bottom || r2.Top > r1.Bottom);
		}

		public static int IntersectionSquare(Rectangle r1, Rectangle r2)
		{
			if (AreIntersected(r1, r2)) 
				return (Math.Max(Math.Min(r1.Right, r2.Right) - Math.Max(r1.Left, r2.Left), 0)) 
				* (Math.Max(Math.Min(r1.Bottom, r2.Bottom) - Math.Max(r1.Top, r2.Top), 0));
			return 0;
		}

		public static int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
		{
			if ((r1.Top <= r2.Top) && (r1.Bottom >= r2.Bottom) && (r1.Left <= r2.Left) && (r1.Right >= r2.Right))
       			return 1;
			else if ((r2.Top <= r1.Top) && (r2.Bottom >= r1.Bottom) && (r2.Left <= r1.Left) && (r2.Right >= r1.Right))
				return 0;
			else return -1;
		}
	}
}