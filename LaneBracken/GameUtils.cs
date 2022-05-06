using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace LaneBracken
{
    class GameUtils
    {
        public static Random random = new Random();


        //LoadEntities code from by PROG 201 course demo
        public static List<Entity> LoadEntities(string fileName)
        {
            List<Entity> entities = new List<Entity>();
            if (File.Exists(fileName))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                XmlNode root = doc.DocumentElement;
                XmlNodeList entityList = root.SelectNodes("/environment/entity");
                foreach (XmlElement entity in entityList)
                {
                    Entity temp = null;
                    if (entity.GetAttribute("type") == "Producer")
                    {
                        temp = new Producer();
                        Producer prod = (Producer)temp;
                        if (int.TryParse(entity.GetAttribute("daysToHarvestMax"), out int a)) { prod.DaysToHarvestMax = a; }
                        if (double.TryParse(entity.GetAttribute("survivalRate"), out double b)) { prod.SurvivalRate = b; }

                    }
                    else if (entity.GetAttribute("type") == "Consumer" || entity.GetAttribute("type") == "Decomposer")
                    {
                        if (entity.GetAttribute("type") == "Decomposer")
                        {
                            temp = new Decomposer();
                        }
                        else
                        {
                            temp = new Consumer();
                        }
                        
                        Consumer cons = (Consumer)temp;
                        cons.Diet = entity.GetAttribute("diet");
                        if (double.TryParse(entity.GetAttribute("amountOfFoodForOne"), out double a)) { cons.AmountOfFoodForOne = a; }
                        if (double.TryParse(entity.GetAttribute("minEatChance"), out double b)) { cons.minEatChance = b; }
                        if (int.TryParse(entity.GetAttribute("daysToReproductionMax"), out int c)) { cons.DaysToReproductionMax = c; }
                        if (double.TryParse(entity.GetAttribute("reproductionRatio"), out double d)) { cons.ReproductionRatio = d; }
                        if (bool.TryParse(entity.GetAttribute("isFlying"), out bool e)) { cons.isFlying = e; }
                        if (bool.TryParse(entity.GetAttribute("makesGuano"), out bool f)) { cons.makesGuano = f; }

                        

                    }
                    

                    if (temp != null)
                    {
                        temp.Name = entity.GetAttribute("name");
                        if (int.TryParse(entity.GetAttribute("amount"), out int a)) { temp.Amount = a; }
                        entities.Add(temp);
                    }
                }
            }
            return entities;

        }


        public static bool SearchListByName(string name, List<INameSearchable> list, out INameSearchable result)
        {

            result = null;
            bool success = false;
            foreach (INameSearchable i in list)
            {
                if (i.Name == name)
                {
                    result = i;
                    success = true;
                    break;
                }
            }

            return success;
        }

        public static bool SearchListByName<T>(string name, List<T> list, out T result)
        {
            result = default(T);

            List<INameSearchable> nameSearchables = list.Cast<INameSearchable>().ToList();

            INameSearchable r;

            if (SearchListByName(name, nameSearchables, out r))
            {
                result = (T)r;
                return true;
            }
            else
            {
                return false;
            }
        }

        

    }
}
