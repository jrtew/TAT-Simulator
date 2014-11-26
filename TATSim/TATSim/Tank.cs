using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TATSim
{
    public class Tank
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int range;

        public int Range
        {
            get { return range; }
            set { range = value; }
        }

        private int weight;

        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        private int cost;

        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        private double gallons;
        private double maxGallons;

        public double Gallons
        {
            get { return gallons; }
            set
            { 
                gallons = value;
                maxGallons = gallons;
            }
        }

        public double MaxGallons
        {
            get { return maxGallons; }
        }

        public Tank(int newRange, int newWeight, int newCost, double newGallons)
        {
            range = newRange;
            weight = newWeight;
            cost = newCost;
            gallons = newGallons;
        }

        public Tank()
        {

        }
    }
}
