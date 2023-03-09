using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inheritance.MapObjects
{
    public interface IOwner
    {
        int Owner { get; set; }
    }

    public interface IArmy
    {
        Army Army { get; set; }
    }

    public interface ITreasure
    {
        Treasure Treasure { get; set; }
    }
    
    public class Dwelling : IOwner
    {
        public int Owner { get; set; }
    }

    public class Mine : IOwner, ITreasure, IArmy
    {
        public int Owner { get; set; }
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Creeps : IArmy, ITreasure 
    {
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Wolves : IArmy
    {
        public Army Army { get; set; }
    }

    public class ResourcePile : ITreasure
    {
        public Treasure Treasure { get; set; }
    }

    public static class Interaction
    {
        public static void Make(Player player, object mapObject)
        {
            if (mapObject is IArmy army)
                ConquerArmy(player, army);
            else if (mapObject is ITreasure treasure)
                player.Consume(treasure.Treasure);
            else if (mapObject is IOwner owner)
                owner.Owner = player.Id;
        }

        private static void ConquerArmy(Player player, IArmy army)
        {
            if (army is ITreasure treasure)
            {
                    if (player.CanBeat(army.Army) && army is IOwner owner)
                    {
                        owner.Owner = player.Id;
                        player.Consume(treasure.Treasure);
                    }
                    else if (player.CanBeat(army.Army))
                        player.Consume(treasure.Treasure);
                    else
                        player.Die();
            }
            else if (!player.CanBeat(army.Army))
                player.Die();
        }
    }
}