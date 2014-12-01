using System;
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
        bool nextStateSelected = false;
        bool needToRunMove = false;
        bool statAtZero = false;
       
        //Constants
        const double mpg = 50.0;
        const double pricePerGallon = 2.8;

        public GameBoardForm(TATSimForm incomingForm)
        {
            // Links the two forms together so we can 
            // carry over the players moto object
            originalForm = incomingForm;
            InitializeComponent();
            playerIcon.Parent = tatMapPB;
            playerIcon.BackColor = Color.Transparent;
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

            player.Enjoyment = 20;
            player.Exhaustion = 20;
            player.Hunger = 20;

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

            enjymntProgBar.Value = 20;
            exhaustProgBar.Value = 20;
            hungerProgBar.Value = 20;
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
                selection1 = "Southern";
                selection2 = "Great Plains";
            }
            else if (trailSelectionState == 2)
            {
                selection1 = "Oregon Coast";
                selection2 = "Los Angeles";
            }
            else
            {
                if (Convert.ToInt32(dayNumTextBox.Text) <= 28 && !statAtZero)
                {
                    //You win!!

                    MessageBox.Show("You win!!!");
                }
                else
                {
                    //You lose!!
                    MessageBox.Show("You lose!!!");
                }
            }

            radbtnSelection1.Text = "Take the " + selection1 + " trail";
            radbtnSelection2.Text = "Take the " + selection2 + " trail";
            nextStateSelected = false;
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
                //gameBoardPanel.BringToFront();
                if (!needToRunMove)
                {
                    playerIcon.Image = playersMotoObj.Image;
                    playerIcon.Location = new Point(currentTrail.ThisStop.Point.X - 25, currentTrail.ThisStop.Point.Y - 25);                    
                }
                else
                {
                    checkMileage();
                    needToRunMove = false;
                }
                nextStateSelected = true;
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
                if (nextStateSelected)
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
                    int nextDistance = movePlayer();
                    mileageTextBox.Text = nextDistance.ToString();
                    double[] array = playersMotoObj.travel(speed, todaysMileage, fuelRange);
                    updateCharacterStats();
                    fuelRangeTB.Text = array[0].ToString().Substring(0, array[0].ToString().IndexOf("."));
                    tireStatTB.Text = array[1].ToString();
                    int daysIntoTrip = Convert.ToInt32(dayNumTextBox.Text);
                    daysIntoTrip++;
                    dayNumTextBox.Text = daysIntoTrip.ToString();
                    updateMoneyFromDailyChoices();
                    checkMileage();

                    if (statAtZero)
                    {
                        trailSelectionState = 3;
                        checkTrailState();
                    }
                    //MessageBox.Show("A day has passed.");
                }
            }
        }

        private void updateCharacterStats()
        {
            //Determine sleeping arrangement
            if (campRadBut.Checked )
            {
                if (enjymntProgBar.Value < 20)
                    enjymntProgBar.Value += 1;

                if (exhaustProgBar.Value > 0)
                    exhaustProgBar.Value -= 1;
            }
            else
            {
                if (enjymntProgBar.Value > 0)
                    enjymntProgBar.Value -= 1;

                if (exhaustProgBar.Value < 19)
                    exhaustProgBar.Value += 2;
                else if (exhaustProgBar.Value < 20)
                    exhaustProgBar.Value += 1;
            }

            //Determine eating arragnement
            if (ramenRadBut.Checked)
            {
                if (hungerProgBar.Value > 0)
                    hungerProgBar.Value -= 1;
            }
            else
            {
                if (hungerProgBar.Value < 20)
                    hungerProgBar.Value += 1;
            }

            //Deterime speed
            if (radbtnSlow.Checked)
            {
                if (enjymntProgBar.Value < 19)
                    enjymntProgBar.Value += 2;
                else if (enjymntProgBar.Value < 20)
                    enjymntProgBar.Value += 1;

                if (exhaustProgBar.Value < 20)
                    exhaustProgBar.Value += 1;

                if (hungerProgBar.Value > 1)
                    hungerProgBar.Value -= 2;
                else if (hungerProgBar.Value > 0)
                    hungerProgBar.Value -= 1;
            }
            else if (radbtnMedium.Checked)
            {
                if (enjymntProgBar.Value < 20)
                    enjymntProgBar.Value += 1;

                if (exhaustProgBar.Value > 0)
                    exhaustProgBar.Value -= 1;

                if (hungerProgBar.Value > 0)
                    hungerProgBar.Value -= 1;
            }
            else
            {
                if (enjymntProgBar.Value > 0)
                    enjymntProgBar.Value -= 1;

                if (exhaustProgBar.Value > 1)
                    exhaustProgBar.Value -= 2;
                else if (exhaustProgBar.Value > 0)
                    exhaustProgBar.Value -= 1;

                if (hungerProgBar.Value < 20)
                    hungerProgBar.Value += 1;
            }

            if (enjymntProgBar.Value <= 0 || exhaustProgBar.Value <= 0 || hungerProgBar.Value <= 0)
            {
                statAtZero = true;
            }
        }

        private void updateMoneyFromDailyChoices()
        {
            double sleep = 0.0;
            double food = 0.0;
            if (campRadBut.Checked)
            {
                sleep = 5.0;
            }
            else
            {
                sleep = 30.0;
            }
            if (ramenRadBut.Checked)
            {
                food = 1.0;
            }
            else
            {
                food = 15.0;
            }

            double wallet = Convert.ToDouble(cashTextBox.Text.ToString().Substring(1));
            cashTextBox.Text = "$" + (wallet - (sleep + food));
        }

        private int movePlayer()
        {
            int lastDistance = Convert.ToInt32(mileageTextBox.Text);
            Stop currentStop = currentTrail.NextStop;
            if (currentStop == null)
            {
                trails.Remove(currentTrail.Name);
                trailSelectionState++;
                checkTrailState();
                needToRunMove = true;
                return lastDistance;                
            }

            playerIcon.Location = new Point(currentStop.Point.X - 25, currentStop.Point.Y - 25);            

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

        private void btnFillUp_Click(object sender, EventArgs e)
        {
            int rangeDiff = playersMotoObj.Range - Convert.ToInt32(fuelRangeTB.Text);
            double cost = 0.0;
            while ((rangeDiff - 50) > 0)
            {
                cost += Math.Round((50 / 2.8), 2);
                rangeDiff -= 50;
            }
            cost += Math.Round((rangeDiff / 2.8), 2);
            double wallet = Convert.ToDouble(cashTextBox.Text.ToString().Substring(1));
            cashTextBox.Text = "$" + (wallet - cost);
            fuelRangeTB.Text = playersMotoObj.Range.ToString();
        }

        //private void tatMapPB_MouseClick(object sender, MouseEventArgs e)
        //{
        //    int x = e.X;
        //    int y = e.Y;

        //    string position = "X = " + x + "\nY = " + y;

        //    MessageBox.Show(position, "Mouse Position", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}
    }
}
