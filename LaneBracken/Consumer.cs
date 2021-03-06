using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneBracken
{
    public class Consumer : Entity
    {
        public string Diet;
        public double AmountOfFoodForOne;
        public double minEatChance = 0.75;
        public double DailyEfficiency = 1.0; // can be reduced by world conditions
        public int DaysToReproduction = -100;
        public int DaysToReproductionMax;
        public double ReproductionRatio; // ratio how many consumers before and after reproduction
        public bool isFlying = false;
        public bool AffectedByScarecrow = false;
        public bool makesGuano = false;

        
        //extinction events
        public event EventHandler<ExtinctEventArgs> Extinct;

        public override void OnWorldInitialized()
        {
            base.OnWorldInitialized();
            Extinct += World.GetWorld().ExtinctionAlert;
        }
        protected virtual void OnExtinct(ExtinctEventArgs e)
        {
            Extinct?.Invoke(this, e);
        }

        public virtual void Eat()
        {
            if (Amount > 0)
            {
                if (isFlying)
                {
                    bool raining = (World.GetWorld().TodaysWeather == Weather.Rain);
                    if (raining)
                    {
                        Say("It is more difficult to fly and hunt in the rain.");
                        DailyEfficiency = 0.75;
                    }
                    else
                    {
                        DailyEfficiency = 1.0;
                    }
                    if (AffectedByScarecrow)
                    {
                        DailyEfficiency *= 0.9;
                        Say("We are too scared to hunt effectively.");
                    }
                }
                if (GameUtils.SearchListByName(Diet, World.GetWorld().Entities, out Entity food))
                {
                    // eatPercent is a value between minEatChance and 1
                    double EatPercent = (minEatChance + GameUtils.random.NextDouble() * (1 - minEatChance)) * DailyEfficiency;

                    int amountEating = (int)Math.Ceiling(EatPercent * Amount);

                    int amountOfFoodNeeded = (int)Math.Ceiling(amountEating * AmountOfFoodForOne);

                    if (food.Amount >= amountOfFoodNeeded)
                    {
                        food.Amount -= amountOfFoodNeeded;
                        Amount = amountEating; //only those who ate survive

                        Say("Ate " + amountOfFoodNeeded + " " + Diet + ", feeding " + Amount + " " + Name + ".");

                    }
                    else
                    {
                        if (food.Amount > 0)
                        {

                            int amountAte = (int)Math.Ceiling(food.Amount / AmountOfFoodForOne);

                            Amount = amountAte;

                            Say("Not enough " + Diet + ". We are hungry.");
                            Say("Ate " + food.Amount + " " + Diet + ", feeding " + Amount + " " + Name + ".");

                            food.Amount = (int)Math.Floor(food.Amount * 0.1);
                        }
                        else
                        {
                            Amount = (int)Math.Floor(Amount * 0.1);
                            Say("No " + Diet + " available! We are starving!");
                        }
                    }

                    if (Amount > 0 && makesGuano)
                    {
                        int guanoAmount = (int)Math.Ceiling(Amount * 0.5);
                        Say("Made " + guanoAmount + " guano.");
                        MakeGuano(guanoAmount);

                    }
                }
            }
            else
            {
                // call extinction event
                OnExtinct(new ExtinctEventArgs(Name));
            }
        }

        protected void MakeGuano(int amt)
        {
            if (GameUtils.SearchListByName("Guano", World.GetWorld().player.Inventory, out Item item))
            {
                item.Amount += amt;
            }
            else
            {
                Item guano = new Item();
                guano.Name = "Guano";
                guano.Amount = amt;
                World.GetWorld().player.Inventory.Add(guano);
            }
        }

        public void HandleReproduction()
        {
            if (DaysToReproduction == -100) DaysToReproduction = DaysToReproductionMax;

            DaysToReproduction -= 1;

            //Say("Days till reproduction: " + Math.Max(DaysToReproduction, 0).ToString());

            if (DaysToReproduction <= 0)
            {
                // reproduce
                Reproduce();
            }
        }

        public virtual void Reproduce()
        {
            if (Amount > 0)
            {
                int temp = Amount;
                Amount = (int)Math.Ceiling(Amount * ReproductionRatio);

                Say((Amount - temp) + " " + Name + " were born.");
            }
            

            DaysToReproduction = DaysToReproductionMax;

        }
        public override void Step()
        {
            Eat();
            HandleReproduction();


        }
    }
}
