using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneBracken
{
    public abstract class Entity : INameSearchable, ISayable
    {

        public string Name
        {
            get;
            set;
        }
        public int Amount;
        public bool HasEndStep = false;

        public virtual void OnWorldInitialized()
        {
            World.GetWorld().EntityStep += Step;
            World.GetWorld().EntityEndStep += EndStep;
        }

        public abstract void Step();

        public virtual void EndStep()
        {
            //nothing, override if u need it
        }

        public void Say(string s)
        {
            WPFUtils.Print("[" + Name + "]" + " " + s);
        }

    }
}
