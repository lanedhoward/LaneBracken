using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LaneBracken
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public World world;
        public MainWindow()
        {
            InitializeComponent();

            world = World.GetWorld();

            WPFUtils.OutputBox = tBoxOutput;

            NextDay();
        }

        private void UpdateDisplays()
        {
            UpdateEntityDisplay();
            UpdateInventoryDisplay();
        }

        private void UpdateEntityDisplay()
        {
            string s = "";

            foreach (Entity e in world.Entities)
            {
                s += e.Name + ":  " + e.Amount + "\n";
            }

            tBoxStatus.Text = s;
        }

        private void UpdateInventoryDisplay()
        {
            string s = "";

            foreach (Item i in world.player.Inventory)
            {
                s += i.Name + ":  " + i.Amount + "\n";
            }

            tBoxInventory.Text = s;
        }

        private void buttonNextDay_Click(object sender, RoutedEventArgs e)
        {
            NextDay();
        }

        private void NextDay()
        {
            world.Step();
            UpdateDisplays();
        }

        private void buttonBuyCorn_Click(object sender, RoutedEventArgs e)
        {
            int cost = 500;
            int quantity = 2000;


            if (world.player.CanBuy(cost))
            {
                world.Say("Purchased " + quantity + " corn seeds for " + cost.ToString("C"));

                if (GameUtils.SearchListByName("Corn", world.Entities, out Entity entity))
                {
                    Producer p = (Producer)entity;
                    p.Seeds += 2000;
                }

                UpdateInventoryDisplay();
            }
            else
            {
                world.Say("Insufficient funds to buy corn seeds. Cost: " + cost.ToString("C"));
            }
        }
        private void buttonSellCorn_Click(object sender, RoutedEventArgs e)
        {
            int price = 1;
            if (GameUtils.SearchListByName("Corn", world.player.Inventory, out Item item))
            {
                if (item.Amount > 0)
                {
                    int total = price * item.Amount;
                    world.player.money.Amount += total;

                    world.Say("Sold " + item.Amount + " corn for " + total.ToString("C"));

                    item.Amount = 0; //probably redundant but just in case
                    world.player.Inventory.Remove(item);

                    UpdateInventoryDisplay();

                    return;
                }
            }
            
            world.Say(" !!! Error: No corn in inventory. !!! ");
            return;
        }
    }
}
