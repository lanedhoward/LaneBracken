using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneBracken
{
    public class Item : INameSearchable, ISayable
    {
        public string Name
        {
            get;
            set;
        }
        public int Amount;
        public bool HasStep = false;

        public void Say(string s)
        {
            WPFUtils.Print("[" + Name + "]" + " " + s);
        }

        public virtual void Step()
        {
            // nothing, but can be overridden
        }
    }
}
