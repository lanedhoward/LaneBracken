using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneBracken
{
    public class Player
    {
        public List<Item> Inventory;
        public Item money;
        public Player()
        {
            Inventory = new List<Item>();

            money = new Item() { Name = "Money", Amount = 1000 };
            Inventory.Add(money);
        }

        public bool CanBuy(int cost)
        {
            

            if (money.Amount >= cost)
            {
                money.Amount -= cost;
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
