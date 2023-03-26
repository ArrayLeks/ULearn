using System;
using System.Windows.Forms;

namespace Digger
{
    class Player : ICreature
    {
        public CreatureCommand Act(int x, int y)
        { 
            if (Game.KeyPressed == Keys.Up && y - 1 >= 0)
                return new CreatureCommand() { DeltaX = 0, DeltaY = - 1 };
            else if (Game.KeyPressed == Keys.Down && y + 1 < Game.MapHeight)
                return new CreatureCommand() { DeltaX = 0, DeltaY = 1 };
            else if (Game.KeyPressed == Keys.Left && x - 1 >= 0)
                return new CreatureCommand() { DeltaX = - 1, DeltaY = 0 };
            else if (Game.KeyPressed == Keys.Right && x + 1 < Game.MapWidth)
                return new CreatureCommand() { DeltaX = 1, DeltaY = 0 };
            else 
                return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
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
            return 1;
        }

        public string GetImageFileName()
        {
            return "Terrain.png";
        }
    }
}