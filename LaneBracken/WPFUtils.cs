using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace LaneBracken
{
    static class WPFUtils
    {
        public static TextBox OutputBox;
        

        public delegate void PrintDelegate(string message);

        public static PrintDelegate Print = PrintWPF;

        public static void PrintWPF(string message)
        {
            // some help from https://stackoverflow.com/questions/18260702/textbox-appendtext-not-autoscrolling
            OutputBox.AppendText(message + "\n");
            //appendtext should scroll automatically but sometimes it wasn't so i did this
            OutputBox.ScrollToEnd();
            
        }
        public static void PrintEmpty()
        {
            Print("");
        }

        public static void Clear()
        {
            OutputBox.Text = "";
        }

        //write a function like choose() from gamemaker, maybe it takes an array or a list of objects, generates a random index, and returns one of them back, to then be casted to the real type
        public static T Choose<T>(T[] choices)
        {
            int max = choices.Length;
            int rand = GameUtils.random.Next(max - 1); //get a random index
            return choices[rand];

        }

        public static T ChooseWeighted<T>(Dictionary<T,int> choices)
        {
            /// Pass in a dictionary with Keys: choices and Values: weights


            //easiest way to do this in place in memory is to loop through all items in the dictionary and add up for total weight,
            int totalWeight = 0;
            foreach (int v in choices.Values)
            {
                totalWeight += v;
            }

            // then generate a random number up to that weight,
            int rand = GameUtils.random.Next(1,totalWeight);


            //then loop again and add up weights until you pass the random value
            T result = choices.First().Key;
            
            int progress = 0;

            
            foreach (KeyValuePair<T, int> keyValuePair in choices)
            {
                
                int newProgress = progress + keyValuePair.Value;

                //Print($"{newProgress}... {rand}");
                if (rand <  newProgress)
                {

                    result = keyValuePair.Key;
                    break;
                }
                else
                {
                    progress = newProgress;
                }
            }

            return result;
            
        }

    }
}
