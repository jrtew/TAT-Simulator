﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace TATSim
{
    public partial class GameBoardForm : Form
    {
        TATSimForm originalForm;
        public Motorcycle playersMotoObj;
        public Player player = new Player();
        public int day = 1;
        public int cash;
        public int miles;
        Dictionary<string, RandomEvent> randomEvents;
        Dictionary<string, Trail> trails;
        Trail currentTrail;
        int trailSelectionState = 0;
        string selection1 = "";
        string selection2 = "";

        //Constants
        const double mpg = 50.0;
        const double pricePerGallon = 2.8;

        public GameBoardForm(TATSimForm incomingForm)
        {
            // Links the two forms together so we can 
            // carry over the players moto object
            originalForm = incomingForm;
            InitializeComponent();
        }

        //to make sure program closes completely if X is used to close program
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void GameBoardForm_Load(object sender, EventArgs e)
        {
            gameBoardPanel.Visible = false;
            routeSelectPanel1.Visible = true;

            playersMotoObj = originalForm.playersMoto;
            playersMotoPB.Image = playersMotoObj.Image;

            player.Enjoyment = 10;
            player.Exhaustion = 10;
            player.Hunger = 10;

            cash = originalForm.startCash;
            cashTextBox.Text = "$" + cash.ToString();
            dayNumTextBox.Text = day.ToString();

            Mechanics.CalcRangeofMoto(playersMotoObj);
            fuelRangeTB.Text = playersMotoObj.Range.ToString();
            miles = Mechanics.CaclDaysMileage();

            mileageTextBox.Text = miles.ToString();

            randomEvents = RandomEvent.createEvents();
            trails = Trail.createTrials();
            checkTrailState();
            //checkMileage();

            //Console.WriteLine(originalForm.playersMoto.Name.ToString());
            //Console.WriteLine(originalForm.playersMoto.Performance.ToString());
            //Console.WriteLine(originalForm.playersMoto.Range.ToString());
            //Console.WriteLine(originalForm.playersMoto.Weight.ToString());
            //Console.WriteLine(originalForm.playersMoto.Tires.Cost.ToString());
            //Console.WriteLine(originalForm.playersMoto.Tires.Name.ToString());
            //Console.WriteLine(originalForm.playersMoto.GasTank.Name.ToString());
            //Console.WriteLine(originalForm.playersMoto.GasTank.Cost.ToString());
            //Console.WriteLine(originalForm.playersMoto.TheExhaust.Name.ToString());
            //Console.WriteLine(originalForm.playersMoto.TheExhaust.Cost.ToString());
        }

        private void checkTrailState()
        {
            gameBoardPanel.Visible = false;
            routeSelectPanel1.Visible = true;

            if(trailSelectionState == 0)
            {
                selection1 = "Cape Hatteras";
                selection2 = "New York";

            }
            else if (trailSelectionState == 1)
            {
                selection1 = "Ross Needs to Put the Name Here";
                selection2 = "Ross Needs to Put the Name Here";
            }
            else
            {
                selection1 = "Ross Needs to Put the Name Here";
                selection2 = "Ross Needs to Put the Name Here";
            }

            radbtnSelection1.Text = "Travel on " + selection1;
            radbtnSelection2.Text = "Travel on " + selection2;
        }

        private void routeStartBtn1_Click(object sender, EventArgs e)
        {
            if (!radbtnSelection1.Checked && !radbtnSelection2.Checked)
                MessageBox.Show("Please select a route!", "Message", MessageBoxButtons.OK);
            else
            {
                if (radbtnSelection1.Checked)
                {
                    player.Route = selection1;
                    currentTrail = trails[selection1];
                    trails.Remove(selection2);
                }
                else
                {
                    player.Route = selection2;
                    currentTrail = trails[selection2];
                    trails.Remove(selection1);
                }

                routeSelectPanel1.Visible = false;
                gameBoardPanel.Visible = true;
                gameBoardPanel.BringToFront();
            }
        }

        private void nextDayBtn_Click(object sender, EventArgs e)
        {
            //Get a possible random event
            RandomEvent currentEvent = getRandomEvent();
            if (currentEvent != null)
            {
                //Display the Random Event screen so the player can determine what to do
                changeToRandomEventScreen();
            }

            //Checks to make sure the random event has been dealt with
            if (!grpbxRandomEvent.Visible && checkMileage() && checkDailyChoices())
            {
                int todaysMileage = Convert.ToInt32(mileageTextBox.Text);
                int fuelRange = Convert.ToInt32(fuelRangeTB.Text); 
                int speed = 0;
                foreach (RadioButton rb in grpbxSpeed.Controls)
                {
                    if (rb.Checked)
                    {
                        speed = Convert.ToInt32(rb.Tag);
                        break;
                    }
                }
                double[] array = playersMotoObj.travel(speed, todaysMileage, fuelRange);
                fuelRangeTB.Text = array[0].ToString().Substring(0, array[0].ToString().IndexOf("."));
                tireStatTB.Text = array[1].ToString();
                int nextDistance = movePlayer();
                mileageTextBox.Text = nextDistance.ToString();
                int daysIntoTrip = Convert.ToInt32(dayNumTextBox.Text);
                daysIntoTrip++;
                dayNumTextBox.Text = daysIntoTrip.ToString();

                MessageBox.Show("A day has passed.");
            }
        }

        private int movePlayer()
        {
            Stop currentStop = currentTrail.NextStop;
            if (currentStop == null)
            {
                trails.Remove(currentTrail.Name);
                trailSelectionState++;
                checkTrailState();
                currentStop = currentTrail.NextStop;
            }

            return currentStop.Distance;

        }

        private bool checkDailyChoices()
        {
            if ((campRadBut.Checked || hotelRadBut.Checked) && (ramenRadBut.Checked || steakRadBut.Checked))
            {
                return true;
            }
            else
            {
                MessageBox.Show("Error - Please select one sleeping choice and one eating choice.");
                return false;
            }
        }

        private void changeToRandomEventScreen()
        {
            //Change the map to the Random Event groupbox
            tatMapPB.Visible = false;
            grpbxRandomEvent.Visible = true;
        }

        /**
         * The getRandomEvent() method searches through the randomEvents dictionary to find an event with a chance 
         * of happening. If one is found, it is returned. Every event's chance increases randomly in this method.
         * 
         **/
        private RandomEvent getRandomEvent()
        {
            bool eventSelected = false;
            RandomEvent eventToReturn = null;
            double chance = 0.0;
            for (int i = 0; i < 100; i++)
            {
                chance += (double) new Random().Next(60, 101);
            }
            chance /= 100;

            foreach (string key in randomEvents.Keys)
            {
                if (randomEvents[key].ChanceToHappen >= chance)
                {
                    if (!eventSelected)
                    {
                        eventToReturn = randomEvents[key];
                        eventSelected = true;
                        eventToReturn.resetChance();
                    }
                }
                else
                {
                    randomEvents[key].increaseChance();
                }
            }

            return eventToReturn;
        }

        private bool checkMileage()
        {
            int todaysMileage = Convert.ToInt32(mileageTextBox.Text);
            int fuelRange = Convert.ToInt32(fuelRangeTB.Text);

            if (fuelRange < todaysMileage)
            {
                btnFillUp.Visible = true;
                return false;
            }
            else
            {
                btnFillUp.Visible = false;
                return true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void tatMapPB_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;

            string position = "X = " + x + "\nY = " + y;

            MessageBox.Show(position, "Mouse Position", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
