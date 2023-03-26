using System;
using System.Windows.Forms;

namespace Digger
{
    class Player : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            if (Game.KeyPressed == Keys.Up && y - 1 >= 0 && Game.Map[x, y -1]?.ToString() != "Digger.Sack")
                return new CreatureCommand() { DeltaX = 0, DeltaY = - 1 };
            else if (Game.KeyPressed == Keys.Down && y + 1 < Game.MapHeight
                        && Game.Map[x, y + 1]?.ToString() != "Digger.Sack")
                return new CreatureCommand() { DeltaX = 0, DeltaY = 1 };
            else if (Game.KeyPressed == Keys.Left && x - 1 >= 0 && Game.Map[x - 1, y]?.ToString() != "Digger.Sack")
                return new CreatureCommand() { DeltaX = - 1, DeltaY = 0 };
            else if (Game.KeyPressed == Keys.Right 
                        && x + 1 < Game.MapWidth && Game.Map[x + 1, y]?.ToString() != "Digger.Sack")
                return new CreatureCommand() { DeltaX = 1, DeltaY = 0 };
            else 
                return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Sack;
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public string GetImageFileName()
        {
            return "Digger.png";
        }
    }

    class Terrain : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand() { DeltaX= 0, DeltaY = 0 };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 10;
        }

        public string GetImageFileName()
        {
            return "Terrain.png";
        }
    }

    class Sack : ICreature
    {
        bool isMoving = false;
        int countMove = 0;
        
        public CreatureCommand Act(int x, int y)
        {
            if (isMoving && y + 1 < Game.MapHeight && Game.Map[x, y + 1] is Player)
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
            else if(isMoving && countMove > 1)
            {
                return new CreatureCommand() { DeltaX = 0, DeltaY = 0, TransformTo = new Gold()};
            }
            else
            {
                countMove = 0;
                isMoving = false;
                return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };
            }
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public string GetImageFileName()
        {
            return "Sack.png";
        }
    }

    class Gold : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if(conflictedObject is Player)
            {
                Game.Scores += 10;
            }
            return true;
        }

        public int GetDrawingPriority()
        {
            return 3;
        }

        public string GetImageFileName()
        {
            return "Gold.png";
        }
    }
}