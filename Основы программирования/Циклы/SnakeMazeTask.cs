namespace Mazes
{
	public static class SnakeMazeTask
	{
		public static void MoveOut(Robot robot, int width, int height)
		{
			while(!robot.Finished)
            {
                MoveRightToEnd(width, robot);
                if (robot.Finished) break;
                MoveDown(2, robot);
                MoveLeftToEnd(width, robot);
                if (robot.Finished) break;
                MoveDown(2, robot);
            }
		}

        private static void MoveRightToEnd(int count, Robot robot)
        {
            for (int i = 0; i < count - 3; i++)
                robot.MoveTo(Direction.Right);
        }

        private static void MoveLeftToEnd(int count, Robot robot)
        {
            for (int i = 0; i < count - 3; i++)
                robot.MoveTo(Direction.Left);
        }

        private static void MoveDown(int count, Robot robot)
        {
            for (int i = 0; i < count; i++)
                robot.MoveTo(Direction.Down);
        }
    }
}