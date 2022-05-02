using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneBracken
{
    public class Producer : Entity
    {
        public int DaysToHarvest = -100; // -100 will serve as a code for uninitialized
        public int DaysToHarvestMax;
        public double SurvivalRate;

        private bool PlantTomorrow = false;
        private bool PlantedSinceLastHarvest = true;

        public int Seeds = 0; // seeds will be planted after the harvest

        public new bool HasEndStep = true;

        private bool HarvestTonight = false;

        protected virtual void Harvest()
        {
            Item temp;
            

            if (GameUtils.SearchListByName(Name, World.GetWorld().player.Inventory, out temp))
            {
                // player already has some of this product
                temp.Amount += Amount;
            }
            else
            {
                // player does not yet have this product
                temp = new Item();
                temp.Name = Name;
                temp.Amount = Amount;
                World.GetWorld().player.Inventory.Add(temp);
            }
            
            // get rid of corn crop and add to corn product ??
            // maybe only get rid of like 90%

            Amount = 0;
        }

        protected void Plant()
        {
            Say("Planting " + Seeds + " seeds.");
            Amount += Seeds;
            Seeds = 0;
        }

        public override void Step()
        {
            if (DaysToHarvest == -100) DaysToHarvest = DaysToHarvestMax;

            if (PlantTomorrow)
            {
                if (Seeds > 0)
                {
                    Plant();
                    PlantTomorrow = false;
                    PlantedSinceLastHarvest = true;
                }
                else
                {
                    Say("No " + Name + " planted. Buy some seeds please. ");
                }
            }

            if (PlantedSinceLastHarvest)
            {
                bool raining = (World.GetWorld().TodaysWeather == Weather.Rain);

                if (raining)
                {
                    Say("The rain is good for growth.");
                    DaysToHarvest -= 2;
                }
                else
                {
                    DaysToHarvest -= 1;
                }


                Say("Days till harvest: " + Math.Max(DaysToHarvest, 0).ToString());

                if (DaysToHarvest <= 0)
                {
                    if (!raining)
                    {
                        HarvestTonight = true;


                    }
                    else
                    {
                        Say("It is too rainy to harvest today.");
                        DaysToHarvest = 0;
                    }
                }
            }

        }

        public override void EndStep()
        {
            if (HarvestTonight)
            {
                HarvestTonight = false;

                int tempAmt = Amount;
                Harvest();

                Say(tempAmt + " " + Name + " " + " harvested. ");

                PlantTomorrow = true;
                PlantedSinceLastHarvest = false;

                DaysToHarvest = DaysToHarvestMax;

            }
        }

    }
}
