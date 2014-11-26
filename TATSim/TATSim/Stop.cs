using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TATSim
{
    class Stop
    {
        private Point point;

        public Point Point
        {
            get { return point; }
            set { point = value; }
        }

        private int distance;

        public int Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        public Stop(Point p)
        {
            point = p;
            distance = Mechanics.CaclDaysMileage();
        }
    }
}
