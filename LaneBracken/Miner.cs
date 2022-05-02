using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneBracken
{
    class Miner : Item
    {
        private int dailyMines = 300;
        private double guanoPrice = 0.75;
        public Miner()
        {
            Name = "Guano Miners";
            HasStep = true;
            
        }
        
        public override void Step()
        {
            if (Amount > 0)
            {
                Amount -= 1;
                if (GameUtils.SearchListByName("Guano", World.GetWorld().player.Inventory, out Item item))
                {
                    int mined;
                    if (item.Amount >= dailyMines)
                    {
                        item.Amount -= dailyMines;
                        mined = dailyMines;
                    }
                    else
                    {
                        mined = item.Amount;
                        item.Amount = 0;

                    }

                    int cash = (int)Math.Ceiling(guanoPrice * mined);
                    World.GetWorld().player.money.Amount += cash;

                    Say("Mined " + mined + " guano today for " + cash.ToString("C") + ".");


                }
            }
            else
            {
                if (GameUtils.SearchListByName("Hawk", World.GetWorld().Entities, out Entity entity))
                {
                    Consumer cons = (Consumer)entity;
                    cons.AffectedByScarecrow = false;
                }
                Amount = 0;
                //World.GetWorld().player.Inventory.Remove(this);
            }
        }
    }
}
