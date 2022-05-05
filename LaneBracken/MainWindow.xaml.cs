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
    /// 

    // CREDITS:
    // Events code taken from PROG201 events demo module
    // XML loading adapted from PROG201 sample code
    // WPF print with some light assistance from stack overflow (link in WPFUtils.cs)

    public partial class MainWindow : Window
    {
        public World world;
        private bool helpEngaged = false;
        public MainWindow()
        {
            InitializeComponent();


            world = World.GetWorld();

            WPFUtils.OutputBox = tBoxOutput;

            WelcomeMessage();
        }

        private void WelcomeMessage()
        {
            world.Say("Welcome to Lane's Bracken Cave simulation.");
            world.Say("Click the [?] if you have questions about what each button does.");
            world.Say("Click [Next Day] when you are ready to start!");
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
            if (!helpEngaged)
            {
                NextDay();
            }
            else
            {
                world.Say("Advance the game to the next day.");
            }
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

            if (!helpEngaged)
            {
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                {
                    //if you shift click, buy as much corn as possible
                    int money = world.player.money.Amount;

                    int maxBuy = (int)Math.Floor((double)(money / cost));

                    if (maxBuy > 0)
                    {
                        if (world.player.CanBuy(maxBuy * cost))
                        {
                            world.Say("Purchased " + (quantity * maxBuy) + " corn seeds for " + (maxBuy * cost).ToString("C"));

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
            else
            {
                // help mode engaged
                world.Say("This button will purchase " + quantity + " corn seeds for " + cost.ToString("C"));
                world.Say("You can hold shift and click to buy as many seeds as you can afford at once.");
            }
        }
        private void buttonSellCorn_Click(object sender, RoutedEventArgs e)
        {
            int price = 1;

            if (!helpEngaged)
            {
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
            else
            {
                //help engaged
                world.Say("Sell all of your corn corn for " + price.ToString("C") + " each.");
            }
        }

        private void buttonBuyScarecrow_Click(object sender, RoutedEventArgs e)
        {
            int cost = 500;
            int days = 2;

            if (!helpEngaged)
            {
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
            else
            {
                world.Say("Buy " + days + " days of Scarecrow for " + cost.ToString("C"));
                world.Say("Scarecrow will scare hawks away from hunting prey.");
            }
        }

        private void buttonBuyInsecticide_Click(object sender, RoutedEventArgs e)
        {
            int cost = 250;
            int days = 1;

            if (!helpEngaged)
            {
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
            else
            {
                world.Say("Buy " + days + " days of Insecticide for " + cost.ToString("C"));
                world.Say("Insecticide kills worms.");
            }
        }

        private void buttonBuyMiners_Click(object sender, RoutedEventArgs e)
        {
            int cost = 500;
            int days = 4;

            if (!helpEngaged)
            {
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
                    world.Say("Hired " + days + " days of Guano Miners for " + cost.ToString("C"));
                    UpdateInventoryDisplay();
                }
                else
                {
                    world.Say("Insufficient funds to buy Guano Miners. Cost: " + cost.ToString("C"));
                }
            }
            else
            {
                world.Say("Hire " + days + " days of Guano Miners for " + cost.ToString("C"));
                world.Say("Guano miners will mine and sell your Guano for you. ");
            }
        }

        private void buttonBuyWeatherMachine_Click(object sender, RoutedEventArgs e)
        {
            int cost = 100;
            int days = 1;

            if (!helpEngaged)
            {
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
            else
            {
                world.Say("Buy " + days + " days of Weather Machine for " + cost.ToString("C"));
                world.Say("The weather machine will make tomorrow's weather the same as today's.");
                world.Say("If it is rainy today, it will be rainy tomorrow.");
            }
        }

        private void buttonHelp_Click(object sender, RoutedEventArgs e)
        {
            if (helpEngaged)
            {
                helpEngaged = false;
                world.Say("Help mode disengaged.");
            }
            else
            {
                helpEngaged = true;
                world.Say("Help mode engaged.");
            }
        }
    }
}
