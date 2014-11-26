using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TATSim
{
    class RandomEvent
    {
        private double chanceToHappen;

        public double ChanceToHappen
        {
            get { return chanceToHappen; }
        }

        private string name;

        public string Name
        {
            get { return name; }
        }

        private string description;

        public string Description
        {
            get { return description; }
        }

        private int enjoymentImpact;

        public int EnjoymentImpact
        {
            get { return enjoymentImpact; }
        }

        private int exhaustionImpact;

        public int ExhaustionImpact
        {
            get { return exhaustionImpact; }
        }

        private int hungarImpact;

        public int HungarImpact
        {
            get { return hungarImpact; }
        }

        public RandomEvent(string newName, string newDescript, int newEnjoy, int newExhaust, int newHungar)
        {
            name = newName;
            description = newDescript;
            enjoymentImpact = newEnjoy;
            exhaustionImpact = newExhaust;
            hungarImpact = newHungar;
            chanceToHappen = 1.0;
        }

        public void increaseChance()
        {
            ;
            chanceToHappen += (new Random().Next(15) / 100);
        }

        public void resetChance()
        {
            chanceToHappen = 1.0;
        }

        public static Dictionary<string, RandomEvent> createEvents()
        {
            RandomEvent flatTire = new RandomEvent("Flat Tire", "Bummer! You were driving along and ran over a nail. Your tire is completely shredded and you must buy some more!", 2, 2, 0);
            RandomEvent severeWeather = new RandomEvent("Severe Weather", "Your area has been subject to severe weather. The path ahead is still bearable, but proceed at your own risk.", 2, 3, 1);

            Dictionary<string, RandomEvent> tempDict = new Dictionary<string, RandomEvent>();

            tempDict.Add(flatTire.name, flatTire);
            tempDict.Add(severeWeather.name, severeWeather);

            return tempDict;
        }
    }
}
