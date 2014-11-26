using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TATSim
{
    public class Player
    {
        private int enjoyment;

        public int Enjoyment
        {
            get { return enjoyment; }
            set { enjoyment = value; }
        }

        private int exhaustion;

        public int Exhaustion
        {
            get { return exhaustion; }
            set { exhaustion = value; }
        }

        private int hunger;

        public int Hunger
        {
            get { return hunger; }
            set { hunger = value; }
        }

        private string route;

        public string Route
        {
            get { return route; }
            set { route = value; }
        }

        public Player(int newEnjoyment, int newExhaustion, int newHunger)
        {
            enjoyment = newEnjoyment;
            exhaustion = newExhaustion;
            hunger = newHunger;
        }

        public Player()
        {

        }
    }
}
