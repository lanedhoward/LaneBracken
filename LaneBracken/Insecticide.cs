using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneBracken
{
    class Insecticide : Item
    {
        double killFactor = 0.2;
        public Insecticide()
        {
            Name = "Insecticide";
            HasStep = true;
            World.GetWorld().ItemStep += Step;
        }
        
        public override void Step()
        {
            if (Amount > 0)
            {
                Amount -= 1;
                if (GameUtils.SearchListByName("Corn Earworm", World.GetWorld().Entities, out Entity entity))
                {
                    Consumer cons = (Consumer)entity;

                    int amountKilled = (int)Math.Floor(cons.Amount * killFactor);

                    cons.Amount -= amountKilled;

                    Say("Killed " + amountKilled + " corn earworms today.");
                }
            }
            else
            {
                Amount = 0;
                //World.GetWorld().player.Inventory.Remove(this);
            }
        }
    }
}
