using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneBracken
{
    public class Decomposer : Consumer
    {
        // decomposers are just like consumers but they eat items from the players Inventory, not entities from the world
        public override void Eat()
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
                if (GameUtils.SearchListByName(Diet, World.GetWorld().player.Inventory, out Item food))
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

    }
}
