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

            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                //if you shift click, buy as much corn as possible
                int money = world.player.money.Amount;

                int maxBuy = (int)Math.Floor((double)(money / cost));

                if (maxBuy > 0)
                {
                    if (world.player.CanBuy(maxBuy * cost))
                    {
                        world.Say("Purchased " + (quantity * maxBuy) + " corn seeds for " + (maxBuy*cost).ToString("C"));

                        if (GameUtils.SearchListByName("Corn", world.Entities, out Entity entity))
                        {
                            Producer p = (Producer)entity;
                            p.Seeds += quantity * maxBuy;
                        }

                        UpdateInventoryDisplay();
                    }
                    else
                    {
                        world.Say("Insufficient funds to buy corn seeds. Cost: " + cost.ToString("C"));
                    }
                }

            }
            else
            {
                
                if (world.player.CanBuy(cost))
                {
                    world.Say("Purchased " + quantity + " corn seeds for " + cost.ToString("C"));

                    if (GameUtils.SearchListByName("Corn", world.Entities, out Entity entity))
                    {
                        Producer p = (Producer)entity;
                        p.Seeds += quantity;
                    }

                    UpdateInventoryDisplay();
                }
                else
                {
                    world.Say("Insufficient funds to buy corn seeds. Cost: " + cost.ToString("C"));
                }
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

        private void buttonBuyScarecrow_Click(object sender, RoutedEventArgs e)
        {
            int cost = 500;
            int days = 2;

            if (world.player.CanBuy(cost))
            {
                if (GameUtils.SearchListByName("Scarecrow", world.player.Inventory, out Item item))
                {
                    item.Amount += days;
                }
                else
                {
                    Scarecrow scarecrow = new Scarecrow();
                    scarecrow.Amount = days;
                    Item scareItem = (Item)scarecrow;
                    world.player.Inventory.Add(scareItem);
                }
                world.Say("Purchased " + days + " days of Scarecrow for " + cost.ToString("C"));
                UpdateInventoryDisplay();
            }
            else
            {
                world.Say("Insufficient funds to buy scarecrow. Cost: " + cost.ToString("C"));
            }
        }

        private void buttonBuyInsecticide_Click(object sender, RoutedEventArgs e)
        {
            int cost = 250;
            int days = 1;

            if (world.player.CanBuy(cost))
            {
                if (GameUtils.SearchListByName("Insecticide", world.player.Inventory, out Item item))
                {
                    item.Amount += days;
                }
                else
                {
                    Insecticide insecticide = new Insecticide();
                    insecticide.Amount = days;
                    Item insItem = (Item)insecticide;
                    world.player.Inventory.Add(insItem);
                }
                world.Say("Purchased " + days + " days of Insecticide for " + cost.ToString("C"));
                UpdateInventoryDisplay();
            }
            else
            {
                world.Say("Insufficient funds to buy Insecticide. Cost: " + cost.ToString("C"));
            }
        }

        private void buttonBuyMiners_Click(object sender, RoutedEventArgs e)
        {
            int cost = 500;
            int days = 4;

            if (world.player.CanBuy(cost))
            {
                if (GameUtils.SearchListByName("Guano Miners", world.player.Inventory, out Item item))
                {
                    item.Amount += days;
                }
                else
                {
                    Miner miner = new Miner();
                    miner.Amount = days;
                    Item minerItem = (Item)miner;
                    world.player.Inventory.Add(minerItem);
                }
                world.Say("Purchased " + days + " days of Guano Miners for " + cost.ToString("C"));
                UpdateInventoryDisplay();
            }
            else
            {
                world.Say("Insufficient funds to buy Guano Miners. Cost: " + cost.ToString("C"));
            }
        }

        private void buttonBuyWeatherMachine_Click(object sender, RoutedEventArgs e)
        {
            int cost = 100;
            int days = 1;

            if (world.player.CanBuy(cost))
            {
                if (GameUtils.SearchListByName("Weather Machine", world.player.Inventory, out Item item))
                {
                    item.Amount += days;
                }
                else
                {
                    WeatherMachine wm = new WeatherMachine();
                    wm.Amount = days;
                    world.player.Inventory.Add(wm);
                }
                world.Say("Purchased " + days + " days of Weather Machine for " + cost.ToString("C"));
                UpdateInventoryDisplay();
            }
            else
            {
                world.Say("Insufficient funds to buy Weather Machine. Cost: " + cost.ToString("C"));
            }
        }
    }
}
