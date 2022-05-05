using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneBracken
{
    public class ExtinctEventArgs : System.EventArgs
    {
        /*
        public event EventHandler<ExtinctEventArgs> Extinct;

        protected virtual void OnExtinct(ExtinctEventArgs e)
        {
            Extinct?.Invoke(this, e);
        }
        */

        public readonly string Name;

        public ExtinctEventArgs(string name)
        {
            Name = name;
        }
    }
}
