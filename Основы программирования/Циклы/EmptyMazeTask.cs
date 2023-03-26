namespace Mazes
{
	public static class EmptyMazeTask
	{
		public static void MoveOut(Robot robot, int width, int height)
		{
			while(!robot.Finished)
                if (robot.X != width - 2) MoveRight(1, robot);
                else MoveDown(1, robot);
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