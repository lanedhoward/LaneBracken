using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneBracken
{
    class WeatherMachine : Item
    {
        
        public WeatherMachine()
        {
            Name = "Weather Machine";
            HasStep = true;
            
        }
        
        public override void Step()
        {
            if (Amount > 0)
            {
                Amount -= 1;
                World.GetWorld().WeatherMachineOn = true;
                Say("Weather machine on! The weather will be the same as yesterday. ");
            }
            else
            {
                World.GetWorld().WeatherMachineOn = false;
                Amount = 0;
                //World.GetWorld().player.Inventory.Remove(this);
            }
        }
    }
}
