using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneBracken
{
    public enum Weather
    {
        Sun,
        Rain
    }
    public class World : ISayable
    {
        public List<Entity> Entities;

        public Weather TodaysWeather;

        public Player player;

        public static World instance = null;

        public int Day;

        public bool WeatherMachineOn = false;


        private World()
        {
            TodaysWeather = Weather.Sun;
            Day = 0;

            Entities = GameUtils.LoadEntities("../../data/gamedata.xml");

            player = new Player();
        }

        public static World GetWorld()
        {
            if (instance == null) instance = new World();
            return instance;
        }

        public void Say(string s)
        {
            WPFUtils.Print("[World]" + " " + s);
        }

        public void Step()
        {
            Day += 1;
            Say("********** The sun rises on day " + Day + "! **********");

            // do item steps
            foreach (Item item in player.Inventory)
            {
                if (item.HasStep) item.Step();
            }

            // decide weather
            if (!WeatherMachineOn)
            {
                SetWeather();
            }
            switch (TodaysWeather)
            {
                case Weather.Rain:
                    Say("It is rainy today.");
                    break;
                case Weather.Sun:
                    Say("It is sunny today.");
                    break;
            }

            // do entity steps
            // ideally this goes in order producer -> consumer -> decomposer, but i guess it doesn't really matter
            foreach(Entity e in Entities)
            {
                e.Step();
            }

            //do entity end steps
            foreach (Entity e in Entities)
            {
                
                 e.EndStep();
                
            }
        }

        private void SetWeather()
        {
            Dictionary<Weather, int> potentialWeather = new Dictionary<Weather, int>()
                {
                    {Weather.Sun, 80 },
                    {Weather.Rain, 20 }
                };
            Weather newWeather = WPFUtils.ChooseWeighted<Weather>(potentialWeather);

            TodaysWeather = newWeather;
        }

    }
}
