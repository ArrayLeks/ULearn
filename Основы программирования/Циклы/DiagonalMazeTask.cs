using System;

namespace Mazes
{
	public static class DiagonalMazeTask
	{
		public static void MoveOut(Robot robot, int width, int height)
		{
            int rightCount = 1;
            int downCount = 1;

            if (width > height)
            {
                rightCount = (int)Math.Round(width / (double)height);
                MoveRight(rightCount, robot);
            }
            else
                downCount = (int)Math.Round(height / (double)width);

            while(!robot.Finished)
            {
                MoveDown(downCount, robot);
                if (robot.Finished) break;
                MoveRight(rightCount, robot);
            }
		}

        private static void MoveRight(int count, Robot robot)
        {
            for (int i = 0; i < count; i++)
                robot.MoveTo(Direction.Right);
        }

        private static void MoveDown(int count, Robot robot)
        {
            for (int i = 0; i < count; i++)
                robot.MoveTo(Direction.Down);
        }
    }
}