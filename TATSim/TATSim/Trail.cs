using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TATSim
{
    class Trail
    {
        private string name;

        public string Name
        {
            get { return name; }
        }

        private LinkedList<Stop> stops;

        public Stop NextStop
        {
            get 
            {
                if (IsEmpty())
                {
                    return null;
                }
                else
                {
                    Stop temp = stops.First.Value;
                    stops.RemoveFirst();
                    return temp;
                }
            }
        }

        public bool IsEmpty()
        {
            if (stops.First == null)
            {
                return true;
            }
            return false;
        }


        public Trail(string newName, LinkedList<Stop> newStops)
        {
            name = newName;
            stops = newStops;
        }

        public static Dictionary<string, Trail> createTrials()
        {
            Dictionary<string, Trail> trails = new Dictionary<string, Trail>();

            //Create Stops for the trail
            Stop s1 = new Stop(new Point(500, 200));
            Stop s2 = new Stop(new Point(450, 190));

            //Create the LinkedList and add those Points to the list
            LinkedList<Stop> NewYork = new LinkedList<Stop>();
            NewYork.AddLast(s1);
            NewYork.AddLast(s2);

            //Create a trail with that LinkedList of Points
            Trail NYTrail = new Trail("New York", NewYork);

            //Add the trail to the dictionary
            trails.Add(NYTrail.name, NYTrail);

            return trails;
        }
    }
}
