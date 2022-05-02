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

        public void Eat()
        {
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

            }
        }

        public void HandleReproduction()
        {
            if (DaysToReproduction == -100) DaysToReproduction = DaysToReproductionMax;

            DaysToReproduction -= 1;

            Say("Days till reproduction: " + Math.Max(DaysToReproduction, 0).ToString());

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
            else
            {
                Say("We are extinct. No " + Name + " can reproduce.");
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
