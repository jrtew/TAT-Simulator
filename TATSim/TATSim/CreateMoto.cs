using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TATSim
{
    class CreateMoto
    {
        public static Motorcycle CreateMotoObj(string motoType)
        {
            Motorcycle moto = new Motorcycle();

            if (motoType.Equals("dr"))
            {
                moto.Name = "Dirty Ron";
                moto.Performance = 5;
                moto.Weight = 4;
                moto.Range = 5;
                moto.Image = System.Drawing.Image.FromFile(System.Environment.CurrentDirectory + "\\DRCutOut.png");
            }
            else if (motoType.Equals("klr"))
            {
                moto.Name = "Kan't Love Roads";
                moto.Performance = 5;
                moto.Weight = 3;
                moto.Range = 6;
                moto.Image = System.Drawing.Image.FromFile(System.Environment.CurrentDirectory + "\\KLRCutOut.png");
            }
            else
            {
                moto.Name = "X Ray";
                moto.Performance = 6;
                moto.Weight = 5;
                moto.Range = 3;
                moto.Image = System.Drawing.Image.FromFile(System.Environment.CurrentDirectory + "\\XRLCutOut.png");
            }

            return moto;
        }

        public static void CreateTireObj(Motorcycle moto, string tireType)
        {
            Tire tire = new Tire();

            if (tireType.Equals("scout"))
            {
                tire.Name = "Trail Scouts";
                tire.Cost = 75;
                tire.Performance = 1;
                tire.Weight = 0;
                moto.Performance += tire.Performance;
                moto.Weight += tire.Weight;
            }
            else if (tireType.Equals("s244"))
            {
                tire.Name = "S244";
                tire.Cost = 50;
                tire.Performance = 0;
                tire.Weight = 1;
                moto.Performance += tire.Performance;
                moto.Weight += tire.Weight;
            }
            else
            {
                tire.Name = "Expl-Rs";
                tire.Cost = 100;
                tire.Performance = 2;
                tire.Weight = -1;
                moto.Performance += tire.Performance;
                moto.Weight += tire.Weight;
            }

            moto.Tires = tire;
        }

        public static void CreateTankObj(Motorcycle moto, string tankType)
        {
            Tank tank = new Tank();

            if (tankType.Equals("expedition"))
            {
                tank.Name = "Expedition";
                tank.Cost = 350;
                tank.Range = 3;
                tank.Weight = -2;
                moto.Range += tank.Range;
                moto.Weight += tank.Weight;
            }
            else if (tankType.Equals("italy"))
            {
                tank.Name = "Italian Special";
                tank.Cost = 300;
                tank.Range = 2;
                tank.Weight = -1;
                moto.Range += tank.Range;
                moto.Weight += tank.Weight;
            }
            else
            {
                tank.Name = "Resin Ranger";
                tank.Cost = 250;
                tank.Range = 1;
                tank.Weight = 0;
                moto.Range += tank.Range;
                moto.Weight += tank.Weight;
            }

            moto.GasTank = tank;
        }

        public static void CreateExhaustObj(Motorcycle moto, string exhaustType)
        {
            Exhaust exhaust = new Exhaust();

            if (exhaustType.Equals("standard"))
            {
                exhaust.Name = "Standard Edition";
                exhaust.Cost = 150;
                exhaust.Performance = -1;
                exhaust.Range = 1;
                exhaust.Weight = 0;
                moto.Performance += exhaust.Performance;
                moto.Range += exhaust.Range;
                moto.Weight += exhaust.Weight;
            }
            else if (exhaustType.Equals("night"))
            {
                exhaust.Name = "Night Stalker";
                exhaust.Cost = 200;
                exhaust.Performance = 1;
                exhaust.Range = 0;
                exhaust.Weight = 0;
                moto.Performance += exhaust.Performance;
                moto.Range += exhaust.Range;
                moto.Weight += exhaust.Weight;
            }
            else
            {
                exhaust.Name = "Yoshi-San";
                exhaust.Cost = 250;
                exhaust.Performance = 2;
                exhaust.Range = -1;
                exhaust.Weight = 1;
                moto.Performance += exhaust.Performance;
                moto.Range += exhaust.Range;
                moto.Weight += exhaust.Weight;
            }

            moto.TheExhaust = exhaust;
        }
    }
}
