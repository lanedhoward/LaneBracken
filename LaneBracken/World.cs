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

        private bool worldInit = false;

        public delegate void WorldEvent();
        public event WorldEvent WorldInitialized;
        public event WorldEvent ItemStep;
        public event WorldEvent EntityStep;
        public event WorldEvent EntityEndStep;
        


        private World()
        {
            TodaysWeather = Weather.Sun;
            Day = 0;

            Entities = GameUtils.LoadEntities("../../data/gamedata.xml");

            foreach (Entity e in Entities)
            {
                // register them for world initialized event

                // this will basically do anything that normally i would do in an entity constructor
                // but an entity constructor cant reference GetWorld() because it causes a stack overflow
                // since World() calls LoadEntities() which calls Entity() which would call GetWorld()
                // which would stil have instance == null since World() hasn't finished yet 
                // so it tries to call World() again in and endless loop

                WorldInitialized += e.OnWorldInitialized;
            }    

            //RegisterExtinctionEvents();

            player = new Player();
        }

        public static World GetWorld()
        {
            if (instance == null)
            {
                instance = new World();
            }

            if (instance.worldInit == false)
            {
                // just initialized world for the first time
                instance.worldInit = true;
                instance.WorldInitialized?.Invoke();
            }
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
            ItemStep?.Invoke();
            /*
            foreach (Item item in player.Inventory)
            {
                if (item.HasStep) item.Step();
            }
            */

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
            EntityStep?.Invoke();
            EntityEndStep?.Invoke();
            /*
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
            */
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

        
        private void RegisterExtinctionEvents()
        {
            
            foreach (Entity e in Entities)
            {
                if (typeof(Consumer).IsAssignableFrom(e.GetType()))
                {
                    // if the entity is a consumer, register its extinction event
                    Consumer c = (Consumer)e;
                    c.Extinct += ExtinctionAlert;
                }
            }
        }

        public void ExtinctionAlert(object sender, ExtinctEventArgs e)
        {
            Say(e.Name + " has gone extinct. :( ");
        }
    }
}
