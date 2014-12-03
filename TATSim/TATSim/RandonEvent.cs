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

        private double highCost;

        public double HighCost
        {
            get { return highCost; }
            set { highCost = value; }
        }

        private double lowCost;

        public double LowCost
        {
            get { return lowCost; }
        }
  

        public RandomEvent(string newName, string newDescript, double newHigh, double newLow)
        {
            name = newName;
            description = newDescript;            
            highCost = newHigh;
            lowCost = newLow;            
            chanceToHappen = 1.0;
        }

        public void increaseChance(double moreChance)
        {
            chanceToHappen += moreChance;
        }

        public void resetChance()
        {
            chanceToHappen = 1.0;
        }

        public static Dictionary<string, RandomEvent> createEvents(Motorcycle moto)
        {
            RandomEvent flatTire = new RandomEvent("Flat Tire", "Bummer! You were driving along and ran over a nail. Your tire is flat!\n\nHigh Dollar Fix - $" + moto.Tires.Cost + "\nLow Dollar Fix - $15.00 but it increases the chance of a small wreck.\nIgnore - ?", moto.Tires.Cost, 15.0);
            RandomEvent severeWeather = new RandomEvent("Severe Weather", "Your area has been subject to severe weather. The path ahead is still bearable, but proceed at your own risk.\n\nHigh Dollar Fix - $60.00 because you have to stay in a hotel.\nLow Dollar Fix - $20.00 by using better camping area but decreases enjoyment by 3 points.\nIgnore - Decrease enjoyment by 5 points.", 60.0, 20.0);
            RandomEvent smallWreck = new RandomEvent("Small Wreck", "Uh-oh! You had a spill in the sand and broke a mirror. You should replace that.\n\nHigh Dollar Fix - $30.00 because you have to replace a side mirror.\nLow Dollar Fix - $1.00 to buy some duct tape but decreases exhaustion by 3 points.\nIgnore - Increases the chance of a bad wreck by 5%.", 30.0, 1.0);
            RandomEvent bigWreck = new RandomEvent("Big Wreck", "YIKES! You had a pretty bad wreck, but luckily you are ok and the damage is reparable.\n\nHigh Dollar Fix - $250.00 to repair your cycle.\nLow Dollar Fix - $50.00 for a temporary fix but decreases enjoyment by 3 points and exhaustion by 2 points.\nIgnore - Increases the chance of a bad wreck by 25% and you lose a day.", 250.0, 50.0);
            RandomEvent brokenChain = new RandomEvent("Broken Chain", "Your chain has failed... It broke. Hopefully you are not.\n\nHigh Dollar Fix - $75.00 to replace your chain.\nLow Dollar Fix - $10.00 for a temporary fix but it increases the chance of a small wreck by 10%.", 75.0, 10.0);
            RandomEvent sick = new RandomEvent("Sick", "Cough, cough. Sniffle, sniffle. Looks like you are sick and in a pickle.\n\nHigh Dollar Fix - $150.00 used for a medical visit.\nLow Dollar Fix - $40.00 for over-the-counter drugs but decreases your exhaustion meter by 3 points.\nIgnore - Lose a day plus your enjoyment and exhaution decrease by 3 points.", 150.0, 40.0);
            RandomEvent gasLeak = new RandomEvent("Gas Leak", "The gas meter slows falls, and you realize your tank is cracked. Will you replace it or fix it\n\nHigh Dollar Fix - A full tank of gas and $" + moto.GasTank.Cost + " to replace your gas tank.\nLow Dollar Fix - $1.00 but further increases your chance of a gas leak by 15%.", moto.GasTank.Cost, 1.0);
            RandomEvent bustedTaillight = new RandomEvent("Busted Taillight", "That faint, familiar red glow behind you ceases to bring warning to drivers behind you. Your taillight is busted.\n\nHigh Dollar Fix - $250.00 to replace the taillight.\nLow Dollar Fix - $50.00 to make a temporary fix but increases the chances of further taillight issues by 10%.\nIgnore - Increases the chances of you getting a ticket by 25%.", 30.0, 5.0);
            RandomEvent ticket = new RandomEvent("Ticket", "Red and blue lights are not your friend. The coppas have pulled you over. Keep it under the limit or get a ticket!\n\nHigh Dollar Fix - $60.00 because you can't outrun the law!", 60.0, 0.0);
            RandomEvent holeyGloves = new RandomEvent("Holey Gloves", "Not to be confused with Holy gloves, the Holey Gloves were used by a bike enthusiast with an aptitude for bad luck.\n\nHigh Dollar Fix - $75.00 for super soft, comfortable gloves.\nLow Dollar Fix - $15.00 for a small, temporary fix but decreases your enjoyment by 2 points and exhaution by 1 point.\nIgnore - Decreases your enjoyment by 3 points, exhaution by 2 point, and increases the chance of a small wreck by 15%.", 75.0, 15.0);
            RandomEvent lost = new RandomEvent("Lost", "Your GPS has stopped working. Your maps are of no help. You haven't seen civilization for miles. You are lost.\n\nIgnore - Your only option is to lose a day.", 0.0, 0.0);

            Dictionary<string, RandomEvent> tempDict = new Dictionary<string, RandomEvent>();

            tempDict.Add(flatTire.name, flatTire);
            tempDict.Add(severeWeather.name, severeWeather);
            tempDict.Add(smallWreck.name, smallWreck);
            tempDict.Add(bigWreck.name, bigWreck);

            tempDict.Add(lost.name, lost);

            return tempDict;
        }
    }
}
