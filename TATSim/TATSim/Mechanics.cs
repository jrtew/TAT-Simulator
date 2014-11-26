using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TATSim
{
    class Mechanics
    {
        public static void NextDay(int cash, int day, Player player, Motorcycle moto)
        {
            if (player.Route.Equals("cape hatteras"))
            {
                //Test fo Ross
                // Test for Jordon
            }
            else if (player.Route.Equals("new york"))
            {

            }
        }

        public static int CaclDaysMileage()
        {
            Random randy = new Random();
            int randoInt = randy.Next(150, 199);
            Console.WriteLine(randoInt);
            return randoInt;
        }

        public static void CalcRangeofMoto(Motorcycle moto)
        {
            switch(moto.Range)
            {
                case 3:
                    moto.Range = 200;
                    break;
                case 4:
                    moto.Range = 207;
                    break;
                case 5:
                    moto.Range = 213;
                    break;
                case 6:
                    moto.Range = 219;
                    break;
                case 7:
                    moto.Range = 224;
                    break;
                case 8:
                    moto.Range = 228;
                    break;
                case 9:
                    moto.Range = 231;
                    break;
                case 10:
                    moto.Range = 234;
                    break;
                default:
                    moto.Range = 200;
                    break;
            }
        }
    }
}
