using System;
using System.Windows.Forms;

namespace Digger
{
    class Player : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            if (Game.KeyPressed == Keys.Up && CheckPathY(x, y - 1))
                return new CreatureCommand() { DeltaX = 0, DeltaY = -1 };
            else if (Game.KeyPressed == Keys.Down && CheckPathY(x, y + 1))
                return new CreatureCommand() { DeltaX = 0, DeltaY = 1 };
            else if (Game.KeyPressed == Keys.Left && CheckPathX(x - 1, y))
                return new CreatureCommand() { DeltaX = -1, DeltaY = 0 };
            else if (Game.KeyPressed == Keys.Right && CheckPathX(x + 1, y))
                return new CreatureCommand() { DeltaX = 1, DeltaY = 0 };
            else
                return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };
        }

        public bool CheckPathY(int x, int y)
        {
            return y >= 0 && y < Game.MapHeight && Game.Map[x, y]?.ToString() != "Digger.Sack";
        }

        public bool CheckPathX(int x, int y)
        {
            return x >= 0 && x < Game.MapWidth && Game.Map[x, y]?.ToString() != "Digger.Sack";
        }

        public bool DeadInConflict(ICreature conflictedObject)
            => conflictedObject is Monster || conflictedObject is Sack;

        public int GetDrawingPriority() => 1;

        public string GetImageFileName() => "Digger.png";
    }

    class Terrain : ICreature
    {
        public CreatureCommand Act(int x, int y)
            => new CreatureCommand() { DeltaX = 0, DeltaY = 0 };

        public bool DeadInConflict(ICreature conflictedObject) => true;

        public int GetDrawingPriority() => 10;

        public string GetImageFileName() => "Terrain.png";
    }

    class Sack : ICreature
    {
        bool isMoving = false;
        int countMove = 0;

        public CreatureCommand Act(int x, int y)
        {
            if (isMoving && y + 1 < Game.MapHeight && (Game.Map[x, y + 1] is Player || Game.Map[x, y + 1] is Monster))
            {
                Game.Map[x, y + 1] = null;
                countMove++;
                return new CreatureCommand() { DeltaX = 0, DeltaY = 1 };
            }
            else if (y + 1 < Game.MapHeight && Game.Map[x, y + 1] is null)
            {
                countMove++;
                isMoving = true;
                return new CreatureCommand() { DeltaX = 0, DeltaY = 1 };
            }
            else if (isMoving && countMove > 1)
            {
                return new CreatureCommand() { DeltaX = 0, DeltaY = 0, TransformTo = new Gold() };
            }
            else
            {
                countMove = 0;
                isMoving = false;
                return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };
            }
        }

        public bool DeadInConflict(ICreature conflictedObject) => false;

        public int GetDrawingPriority() => 2;

        public string GetImageFileName() => "Sack.png";
    }

    class Gold : ICreature
    {
        public CreatureCommand Act(int x, int y)
            => new CreatureCommand() { DeltaX = 0, DeltaY = 0 };

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Player) Game.Scores += 10;
            return true;
        }

        public int GetDrawingPriority() => 3;

        public string GetImageFileName() => "Gold.png";
    }

    class Monster : ICreature
    {
        public (int, int) PlayerPosition()
        {
            for (int x = 0; x < Game.MapWidth; x++)
                for (int y = 0; y < Game.MapHeight; y++)
                    if (Game.Map[x, y] is Player) return (x, y);
            return (-1, -1);
        }

        public CreatureCommand Act(int x, int y)
        {
            int playerX = 0, playerY = 0;
            (playerX, playerY) = PlayerPosition();
            if (playerX != -1 && playerY != -1)
            {
                if (x - playerX < 0 && CheckPath(x + 1, y))
                    return new CreatureCommand() { DeltaX = 1, DeltaY = 0 };
                if (x - playerX > 0 && CheckPath(x - 1, y))
                    return new CreatureCommand() { DeltaX = -1, DeltaY = 0 };
                if (y - playerY < 0 && CheckPath(x, y + 1))
                    return new CreatureCommand() { DeltaX = 0, DeltaY = 1 };
                if (y - playerY > 0 && y - 1 >= 0 && CheckPath(x, y - 1))
                    return new CreatureCommand() { DeltaX = 0, DeltaY = -1 };
            }
            return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        => conflictedObject is Monster || conflictedObject is Sack;

        public int GetDrawingPriority() => 0;

        public string GetImageFileName() => "Monster.png";

        public bool CheckPath(int x, int y)
        {
            return Game.Map[x, y]?.ToString() != "Digger.Sack" 
                && Game.Map[x, y]?.ToString() != "Digger.Monster" 
                && Game.Map[x, y]?.ToString() != "Digger.Terrain";
        }
    }
}