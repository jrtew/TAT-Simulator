using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TATSim
{
    public class Motorcycle
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
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

        private int performance;

        public int Performance
        {
            get { return performance; }
            set { performance = value; }
        }

        private int oil;

        public int Oil
        {
            get { return oil; }
        }

        private int battery;

        public int Battery
        {
            get { return battery; }
        }

        private int lights;

        public int Lights
        {
            get { return lights; }
        }

        private Tire tires;

        public Tire Tires
        {
            get { return tires; }
            set { tires = value; }
        }

        private Tank gasTank;

        public Tank GasTank
        {
            get { return gasTank; }
            set { gasTank = value; }
        }

        private Exhaust exhaust;

        public Exhaust TheExhaust
        {
            get { return exhaust; }
            set { exhaust = value; }
        }

        private Image image;

        public Image Image
        {
            get { return image; }
            set { image = value; }
        }

        public Motorcycle(string newName, int newWeight, int newRange, int newPerformance, int newOil, int newBattery, int newLights, Tire newTires, Tank newTank, Image newImage)
        {
            name = newName;
            weight = newWeight;
            range = newRange;
            performance = newPerformance;
            oil = newOil;
            battery = newBattery;
            lights = newLights;
            tires = newTires;
            gasTank = newTank;
            image = newImage;
        }

        public Motorcycle()
        {

        }
    }
}
