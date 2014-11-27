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

        public Stop ThisStop
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

            // Create Stops for the trail
            // New York:
            Stop s1 = new Stop(new Point(620, 149));
            Stop s2 = new Stop(new Point(591, 153));
            Stop s3 = new Stop(new Point(577, 173));
            Stop s4 = new Stop(new Point(556, 188));
            Stop s5 = new Stop(new Point(535, 205));
            Stop s6 = new Stop(new Point(507, 210));

            // Cape Hatteras:
            Stop s7 = new Stop(new Point(602, 229));
            Stop s8 = new Stop(new Point(581, 235));
            Stop s9 = new Stop(new Point(566, 229));
            Stop s10 = new Stop(new Point(544, 225));
            Stop s11 = new Stop(new Point(528, 218));
            Stop s12 = new Stop(new Point(507, 210));

            // Great Plains:
            Stop s13 = new Stop(new Point(501, 192));
            Stop s14 = new Stop(new Point(463, 189));
            Stop s15 = new Stop(new Point(432, 169));
            Stop s16 = new Stop(new Point(405, 143));
            Stop s17 = new Stop(new Point(382, 141));
            Stop s18 = new Stop(new Point(343, 137));
            Stop s19 = new Stop(new Point(298, 147));
            Stop s20 = new Stop(new Point(268, 164));
            Stop s21 = new Stop(new Point(237, 170));
            Stop s22 = new Stop(new Point(214, 184));

            // The South:
            Stop s23 = new Stop(new Point(482, 222));
            Stop s24 = new Stop(new Point(450, 232));
            Stop s25 = new Stop(new Point(412, 227));
            Stop s26 = new Stop(new Point(383, 220));
            Stop s27 = new Stop(new Point(366, 205));
            Stop s28 = new Stop(new Point(343, 204));
            Stop s29 = new Stop(new Point(304, 203));
            Stop s30 = new Stop(new Point(265, 194));
            Stop s31 = new Stop(new Point(244, 186));
            Stop s32 = new Stop(new Point(214, 184));

            // Oregon Coast
            Stop s33 = new Stop(new Point(188, 176));
            Stop s34 = new Stop(new Point(163, 176));
            Stop s35 = new Stop(new Point(142, 169));
            Stop s36 = new Stop(new Point(131, 152));
            Stop s37 = new Stop(new Point(123, 135));
            Stop s38 = new Stop(new Point(94, 133));
            Stop s39 = new Stop(new Point(76, 108));
            Stop s40 = new Stop(new Point(44, 119));

            // Los Angeles
            Stop s41 = new Stop(new Point(203, 195));
            Stop s42 = new Stop(new Point(189, 189));
            Stop s43 = new Stop(new Point(180, 203));
            Stop s44 = new Stop(new Point(164, 208));
            Stop s45 = new Stop(new Point(147, 213));
            Stop s46 = new Stop(new Point(137, 228));
            Stop s47 = new Stop(new Point(124, 235));
            Stop s48 = new Stop(new Point(111, 246));

            // Create the LinkedList and add those Points to the list
            LinkedList<Stop> NewYork = new LinkedList<Stop>();
            NewYork.AddLast(s1);
            NewYork.AddLast(s2);
            NewYork.AddLast(s3);
            NewYork.AddLast(s4);
            NewYork.AddLast(s5);
            NewYork.AddLast(s6);

            // Create the LinkedList and add those Points to the list
            LinkedList<Stop> CapeHattteras = new LinkedList<Stop>();
            CapeHattteras.AddLast(s7);
            CapeHattteras.AddLast(s8);
            CapeHattteras.AddLast(s9);
            CapeHattteras.AddLast(s10);
            CapeHattteras.AddLast(s11);
            CapeHattteras.AddLast(s12);

            // Create the LinkedList and add those Points to the list
            LinkedList<Stop> GreatPlains = new LinkedList<Stop>();
            GreatPlains.AddLast(s13);
            GreatPlains.AddLast(s14);
            GreatPlains.AddLast(s15);
            GreatPlains.AddLast(s16);
            GreatPlains.AddLast(s17);
            GreatPlains.AddLast(s18);
            GreatPlains.AddLast(s19);
            GreatPlains.AddLast(s20);
            GreatPlains.AddLast(s21);
            GreatPlains.AddLast(s22);

            // Create the LinkedList and add those Points to the list
            LinkedList<Stop> TheSouth = new LinkedList<Stop>();
            TheSouth.AddLast(s23);
            TheSouth.AddLast(s24);
            TheSouth.AddLast(s25);
            TheSouth.AddLast(s26);
            TheSouth.AddLast(s27);
            TheSouth.AddLast(s28);
            TheSouth.AddLast(s29);
            TheSouth.AddLast(s30);
            TheSouth.AddLast(s31);
            TheSouth.AddLast(s32);

            // Create the LinkedList and add those Points to the list
            LinkedList<Stop> OregonCoast = new LinkedList<Stop>();
            OregonCoast.AddLast(s25);
            OregonCoast.AddLast(s26);
            OregonCoast.AddLast(s27);
            OregonCoast.AddLast(s28);
            OregonCoast.AddLast(s29);
            OregonCoast.AddLast(s30);
            OregonCoast.AddLast(s31);
            OregonCoast.AddLast(s32);

            // Create the LinkedList and add those Points to the list
            LinkedList<Stop> LosAngeles = new LinkedList<Stop>();
            LosAngeles.AddLast(s25);
            LosAngeles.AddLast(s26);
            LosAngeles.AddLast(s27);
            LosAngeles.AddLast(s28);
            LosAngeles.AddLast(s29);
            LosAngeles.AddLast(s30);
            LosAngeles.AddLast(s31);
            LosAngeles.AddLast(s32);

            //Create a trail with that LinkedList of Points
            Trail NYTrail = new Trail("New York", NewYork);
            Trail CapeHat = new Trail("Cape Hatteras", CapeHattteras);
            Trail GreatP = new Trail("Great Plains", GreatPlains);
            Trail South = new Trail("Southern", TheSouth);
            Trail OregCoast = new Trail("Oregon Coast", OregonCoast);
            Trail LA = new Trail("Los Angeles", LosAngeles);

            //Add the trail to the dictionary
            trails.Add(NYTrail.name, NYTrail);
            trails.Add(CapeHat.name, CapeHat);
            trails.Add(GreatP.name, GreatP);
            trails.Add(South.name, South);
            trails.Add(OregCoast.name, OregCoast);
            trails.Add(LA.name, LA);

            return trails;
        }
    }
}
