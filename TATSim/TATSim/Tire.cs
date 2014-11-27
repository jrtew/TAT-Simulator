using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TATSim
{
    public class Tire
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int performance;

        public int Performance
        {
            get { return performance; }
            set { performance = value; }
        }

        private double wear;

        public double Wear
        {
            get { return wear; }
            set { wear = value; }
        }

        private int cost;

        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        private int weight;

        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public Tire(string newName, int newPerformance, double newWear, int newCost, int newWeight)
        {
            name = newName;
            performance = newPerformance;
            wear = newWear;
            cost = newCost;
            weight = newWeight;
        }
        public Tire()
        {

        }

        internal void travel(int speed, int distance)
        {
           double percent =(double) ((speed + distance + (10 - weight)) / 30);
           percent = percent / 100;
           wear += percent;
        }
    }
}
