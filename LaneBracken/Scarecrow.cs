using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneBracken
{
    class Scarecrow : Item
    {
        public Scarecrow()
        {
            Name = "Scarecrow";
            HasStep = true;
            
        }
        
        public override void Step()
        {
            if (Amount > 0)
            {
                Amount -= 1;
                if (GameUtils.SearchListByName("Hawk", World.GetWorld().Entities, out Entity entity))
                {
                    Consumer cons = (Consumer)entity;
                    cons.AffectedByScarecrow = true;
                    Say("I'm scaring off some hawks today.");
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
