using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TATSim
{
    public class Exhaust
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

        private int range;

        public int Range
        {
            get { return range; }
            set { range = value; }
        }

        public Exhaust(string newName, int newPerformance, int newCost, int newWeight, int newRange)
        {
            name = newName;
            performance = newPerformance;
            cost = newCost;
            weight = newWeight;
            range = newRange;
        }

        public Exhaust()
        {
            
        }
    }
}
