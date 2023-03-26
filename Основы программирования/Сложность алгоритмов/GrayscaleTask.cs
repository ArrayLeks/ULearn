namespace Recognizer
{
	public static class GrayscaleTask
	{
        public static double[,] ToGrayscale(Pixel[,] original)
        {
			var lengthFirst = original.GetLength(0);
			var lengthSecond = original.GetLength(1);
            var gray = new double[original.GetLength(0), original.GetLength(1)];

            for (int i = 0; i < lengthFirst; i++)
            {
                for (int j = 0; j < lengthSecond; j++)
                {
					gray[i, j] = ColorToGray(original[i, j]);
                }
            }
            return gray;
		}

        public static double ColorToGray(Pixel original) =>
			(0.299 * original.R + 0.587d * original.G + 0.114d * original.B) / 255d;
    }
}