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
using System.Media;

namespace TATSim
{
    public partial class GameBoardForm : Form
    {
        private SoundPlayer newSoundPlayer;
        TATSimForm originalForm;
        public Motorcycle playersMotoObj;
        public Player player = new Player();
        public int day = 1;
        public int cash;
        public int miles;
        Dictionary<string, RandomEvent> randomEvents;
        RandomEvent currentEvent;
        Dictionary<string, Trail> trails;
        Trail currentTrail;
        int trailSelectionState = 0;
        string selection1 = "";
        string selection2 = "";
        bool nextStateSelected = false;
        bool needToRunMove = false;
        static bool statAtZero = false;
       
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
            winLosePanel.Visible = false;
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

            randomEvents = RandomEvent.createEvents(playersMotoObj);
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
                routePicBox.Image = System.Drawing.Image.FromFile(System.Environment.CurrentDirectory + "\\Route1Map.png");
                selection1 = "Cape Hatteras";
                selection2 = "New York";

            }
            else if (trailSelectionState == 1)
            {
                routePicBox.Image = System.Drawing.Image.FromFile(System.Environment.CurrentDirectory + "\\Route2Map.png");
                selection1 = "Southern";
                selection2 = "Great Plains";
            }
            else if (trailSelectionState == 2)
            {
                routePicBox.Image = System.Drawing.Image.FromFile(System.Environment.CurrentDirectory + "\\Route3Map.png");
                selection1 = "Oregon Coast";
                selection2 = "Los Angeles";
            }
            else
            {
                radbtnSelection1.Visible = false;
                radbtnSelection2.Visible = false;
                routeStartBtn1.Visible = false;
                if (Convert.ToInt32(dayNumTextBox.Text) <= 28 && !statAtZero)
                {
                    //You win!!
                    routeSelectPanel1.Visible = false;
                    winLosePanel.Visible = true;
                    winLosePicBox.Image = System.Drawing.Image.FromFile(System.Environment.CurrentDirectory + "\\winPic.png");
                    //MessageBox.Show("You win!!!");
                }
                else
                {
                    //You lose!!
                    routeSelectPanel1.Visible = false;
                    winLosePanel.Visible = true;
                    winLosePicBox.Image = System.Drawing.Image.FromFile(System.Environment.CurrentDirectory + "\\losePic.png");
                    //MessageBox.Show("You lose!!!");
                }
                //Application.Exit();
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
            newSoundPlayer = new SoundPlayer("motoDriveOffSound.wav");
            newSoundPlayer.Play();
            //Get a possible random event
            currentEvent = getRandomEvent();
            if (currentEvent != null)
            {
                //Display the Random Event screen so the player can determine what to do
                changeToRandomEventScreen(currentEvent);                
            }

            //Checks to make sure the random event has been dealt with
            if (!grpbxRandomEvent.Visible && checkMileage() && checkDailyChoices())
            {                
                if (nextStateSelected)
                {
                    checkMileage();
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
                    movePlayer();                    
                    double[] array = playersMotoObj.travel(speed, todaysMileage, fuelRange);
                    updateCharacterStats();
                    if (array[0].ToString().Contains("."))
                    {
                        fuelRangeTB.Text = array[0].ToString().Substring(0, array[0].ToString().IndexOf("."));
                    }
                    else
                    {
                        fuelRangeTB.Text = array[0].ToString();
                    }
                    tireStatTB.Text = array[1].ToString();
                    int daysIntoTrip = Convert.ToInt32(dayNumTextBox.Text);
                    daysIntoTrip++;
                    dayNumTextBox.Text = daysIntoTrip.ToString();
                    updateMoneyFromDailyChoices();
                    
                    

                    if (statAtZero)
                    {
                        trailSelectionState = 3;
                        checkTrailState();
                    }
                    //MessageBox.Show("A day has passed.");
                }
            }
        }

        private void enableChoices()
        {
            campRadBut.Enabled = true;
            hotelRadBut.Enabled = true;
            ramenRadBut.Enabled = true;
            steakRadBut.Enabled = true;
            btnHighPriceFix.Enabled = true;
            btnLowPricedFix.Enabled = true;
            btnIgnore.Enabled = true;
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

        private void movePlayer()
        {
            int lastDistance = Convert.ToInt32(mileageTextBox.Text);
            Stop currentStop = currentTrail.NextStop();
            if (currentStop == null)
            {
                trails.Remove(currentTrail.Name);
                trailSelectionState++;
                checkTrailState();
                needToRunMove = true;
                return;                
            }

            playerIcon.Location = new Point(currentStop.Point.X - 25, currentStop.Point.Y - 25);
            mileageTextBox.Text = currentStop.Distance.ToString();
        }

        private bool checkDailyChoices()
        {
            if ((campRadBut.Checked || hotelRadBut.Checked) && (ramenRadBut.Checked || steakRadBut.Checked))
            {
                return true;
            }
            else
            {
                MessageBox.Show("Please select one sleeping choice and one eating choice.", "Whoa!");
                return false;
            }
        }

        private void changeToRandomEventScreen(RandomEvent currentEvent)
        {
            //Change the map to the Random Event groupbox
            tatMapPB.Visible = false;
            grpbxRandomEvent.Visible = true;

            if (currentEvent.Name.Equals("Broken Chain") || currentEvent.Name.Equals("Gas Leak") || currentEvent.Name.Equals("Ticket"))
            {
                btnIgnore.Enabled = false;
            }

            if (currentEvent.Name.Equals("Ticket") || currentEvent.Name.Equals("Lost"))
            {
                btnLowPricedFix.Enabled = false;
            }

            if (currentEvent.Name.Equals("Lost"))
            {
                btnHighPriceFix.Enabled = false;
            }

            lblRandomEventTitle.Text = currentEvent.Name;
            lblRandomEventTitle.Location = new Point((grpbxRandomEvent.Width / 2) - (lblRandomEventTitle.Width / 2), lblRandomEventTitle.Location.Y);
            rtbxRandomEventDescription.Text = currentEvent.Description;

        }

        /**
         * The getRandomEvent() method searches through the randomEvents dictionary to find an event with a chance 
         * of happening. If one is found, it is returned. Every event's chance increases randomly in this method.
         * 
         **/
        private RandomEvent getRandomEvent()
        {            
            RandomEvent eventToReturn = null;
            Random rand = new Random();
            double chance = 0.0;
            //int chanceToIncrease = 0;
            for (int i = 0; i < 100; i++)
            {
                chance += (double) rand.Next(45, 100);
            }
            chance /= 100;

            double highest = 0.0;

            foreach (string key in randomEvents.Keys)
            {
                if (randomEvents[key].ChanceToHappen >= chance && randomEvents[key].ChanceToHappen > highest)
                {                    
                    eventToReturn = randomEvents[key];
                    highest = randomEvents[key].ChanceToHappen;
                }
                else
                {
                    randomEvents[key].increaseChance((double) rand.Next(10));
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
                MessageBox.Show("You need to fill up!");
                return false;
            }
            else
            {
                btnFillUp.Visible = false;
                return true;
            }
        }

        
        private void btnFillUp_Click(object sender, EventArgs e)
        {
            int rangeDiff = playersMotoObj.Range - Convert.ToInt32(fuelRangeTB.Text);
            
            if (rangeDiff > 0)
            {
                double cost = Math.Round(((rangeDiff / 50) * 2.8), 2);
                takeoutMoney(cost);
                fuelRangeTB.Text = playersMotoObj.Range.ToString();
            }
            else
            {
                takeoutMoney(Math.Round(((playersMotoObj.Range / 50) * 2.8), 2));
                fuelRangeTB.Text = playersMotoObj.Range.ToString();
            }
        }

        private void campRadBut_CheckedChanged(object sender, EventArgs e)
        {
            newSoundPlayer = new SoundPlayer("owlSound.wav");
            newSoundPlayer.Play();
        }

        private void hotelRadBut_CheckedChanged(object sender, EventArgs e)
        {
            newSoundPlayer = new SoundPlayer("bellSound.wav");
            newSoundPlayer.Play();
        }

        private void ramenRadBut_CheckedChanged(object sender, EventArgs e)
        {
            newSoundPlayer = new SoundPlayer("waterPourSound.wav");
            newSoundPlayer.Play();
        }

        private void steakRadBut_CheckedChanged(object sender, EventArgs e)
        {
            newSoundPlayer = new SoundPlayer("cowSound.wav");
            newSoundPlayer.Play();
        }

        //private void tatMapPB_MouseClick(object sender, MouseEventArgs e)
        //{
        //    int x = e.X;
        //    int y = e.Y;

        //    string position = "X = " + x + "\nY = " + y;

        //    MessageBox.Show(position, "Mouse Position", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}

        
        private void btnHighPriceFix_Click(object sender, EventArgs e)
        {
            double wallet;
            currentEvent.resetChance();
            switch(currentEvent.Name)
            {                    
                case "Flat Tire":
                    takeoutMoney(currentEvent.HighCost);
                    playersMotoObj.Tires.Wear = 0.0;
                    break;
                case "Severe Weather":
                    campRadBut.Enabled = false;
                    hotelRadBut.Checked = true;
                    break;
                case "Small Wreck":
                    takeoutMoney(currentEvent.HighCost);
                    break;
                case "Big Wreck":
                    takeoutMoney(currentEvent.HighCost);
                    break;
                case "Broken Chain":
                    takeoutMoney(currentEvent.HighCost);
                    break;
                case "Sick":
                    takeoutMoney(currentEvent.HighCost);
                    break;
                case "Gas Leak":
                    takeoutMoney(20.0);
                    btnFillUp_Click(sender, e);
                    btnFillUp.Visible = false;
                    break;
                case "Busted Taillight":
                    takeoutMoney(30.0);
                    break;
                case "Ticket":
                    takeoutMoney(currentEvent.HighCost);
                    currentEvent.HighCost = currentEvent.HighCost * 1.5;
                    break;
                case "Holey Gloves":
                    if (enjymntProgBar.Value > 2)
                        enjymntProgBar.Value -= 3;
                    else if (enjymntProgBar.Value > 1)
                        enjymntProgBar.Value -= 2;
                    else
                        enjymntProgBar.Value -= 1;

                    if (exhaustProgBar.Value > 1)
                        exhaustProgBar.Value -= 2;
                    else
                        exhaustProgBar.Value -= 1;

                    foreach (string key in randomEvents.Keys)
                    {
                        if (key.Equals("Small Wreck"))
                        {
                            randomEvents[key].increaseChance(25.0);
                        }
                    }
                    break;
                //Nothing for Lost
                default:
                    break;                    
            }
            tatMapPB.Visible = true;
            grpbxRandomEvent.Visible = false;
            enableChoices();
            currentEvent.resetChance();           
        }

        private void btnLowPricedFix_Click(object sender, EventArgs e)
        {
            double wallet;

            switch (currentEvent.Name)
            {
                case "Flat Tire":
                    takeoutMoney(currentEvent.LowCost);
                    foreach (string key in randomEvents.Keys)
                    {
                        if (key.Equals("Small Wreck"))
                        {
                            randomEvents[key].increaseChance(10.0);
                        }
                    }
                    break;
                case "Severe Weather":
                    takeoutMoney(currentEvent.LowCost);
                    campRadBut.Checked = true;
                    hotelRadBut.Enabled = true;

                    if (enjymntProgBar.Value > 2)
                        enjymntProgBar.Value -= 3;
                    else if (enjymntProgBar.Value > 1)
                        enjymntProgBar.Value -= 2;
                    else
                        enjymntProgBar.Value -= 1;
                    break;
                case "Small Wreck":
                    takeoutMoney(currentEvent.LowCost);

                    if (exhaustProgBar.Value > 2)
                        exhaustProgBar.Value -= 3;
                    else if (exhaustProgBar.Value > 1)
                        exhaustProgBar.Value -= 2;
                    else
                        exhaustProgBar.Value -= 1;
                    break;
                case "Big Wreck":
                    takeoutMoney(currentEvent.LowCost);

                    if (enjymntProgBar.Value > 2)
                        enjymntProgBar.Value -= 3;
                    else if (enjymntProgBar.Value > 1)
                        enjymntProgBar.Value -= 2;
                    else
                        enjymntProgBar.Value -= 1;

                    if (exhaustProgBar.Value > 1)
                        exhaustProgBar.Value -= 2;
                    else
                        exhaustProgBar.Value -= 1;
                    break;
                case "Broken Chain":
                    takeoutMoney(currentEvent.LowCost);

                    foreach (string key in randomEvents.Keys)
                    {
                        if (key.Equals("Small Wreck"))
                        {
                            randomEvents[key].increaseChance(10.0);
                        }
                    }
                    break;
                case "Sick":
                    takeoutMoney(currentEvent.LowCost);

                    if (exhaustProgBar.Value > 2)
                        exhaustProgBar.Value -= 3;
                    else if (exhaustProgBar.Value > 1)
                        exhaustProgBar.Value -= 2;
                    else
                        exhaustProgBar.Value -= 1;
                    break;
                case "Gas Leak":
                    takeoutMoney(currentEvent.LowCost);
                    currentEvent.increaseChance(15.0);
                    break;
                case "Busted Taillight":
                    takeoutMoney(currentEvent.LowCost);
                    currentEvent.increaseChance(10.0);
                    break;
                    //No ticket here
                case "Holey Gloves":
                     if (enjymntProgBar.Value > 1)
                        enjymntProgBar.Value -= 2;
                    else
                        enjymntProgBar.Value -= 1;

                    if (exhaustProgBar.Value > 0)
                        exhaustProgBar.Value -= 1;
                    break;
                //Nothing for Lost
                default:
                    break;
            }
            tatMapPB.Visible = true;
            grpbxRandomEvent.Visible = false;
            enableChoices();
            currentEvent.resetChance();   
        }


        private void btnIgnore_Click(object sender, EventArgs e)
        {
            switch (currentEvent.Name)
            {
                case "Flat Tire":
                    statAtZero = true;
                    break;
                case "Severe Weather":
                    if (enjymntProgBar.Value > 4)
                        enjymntProgBar.Value -= 5;
                    else if (enjymntProgBar.Value > 3)
                        enjymntProgBar.Value -= 4;
                    else if (enjymntProgBar.Value > 2)
                        enjymntProgBar.Value -= 3;
                    else if (enjymntProgBar.Value > 1)
                        enjymntProgBar.Value -= 2;
                    else
                        enjymntProgBar.Value -= 1;
                    break;
                case "Small Wreck":
                    foreach (string key in randomEvents.Keys)
                    {
                        if (key.Equals("Big Wreck") || key.Equals("Flat Tire"))
                        {
                            randomEvents[key].increaseChance(5.0);
                        }
                    }
                    break;
                case "Big Wreck":
                    foreach (string key in randomEvents.Keys)
                    {
                        if (key.Equals("Big Wreck") || key.Equals("Flat Tire"))
                        {
                            randomEvents[key].increaseChance(25.0);
                        }
                    }

                    dayNumTextBox.Text = Convert.ToInt32(dayNumTextBox.Text) + 1 + "";
                    break;
                case "Sick":
                    if (enjymntProgBar.Value > 2)
                        enjymntProgBar.Value -= 3;
                    else if (enjymntProgBar.Value > 1)
                        enjymntProgBar.Value -= 2;
                    else
                        enjymntProgBar.Value -= 1;

                    if (exhaustProgBar.Value > 2)
                        exhaustProgBar.Value -= 3;
                    else if (exhaustProgBar.Value > 1)
                        exhaustProgBar.Value -= 2;
                    else
                        exhaustProgBar.Value -= 1;

                    dayNumTextBox.Text = Convert.ToInt32(dayNumTextBox.Text) + 1 + "";
                    break;
                case "Busted Taillight":
                    foreach (string key in randomEvents.Keys)
                    {
                        if (key.Equals("Ticket"))
                        {
                            randomEvents[key].increaseChance(25.0);
                        }
                    }
                    break;
                    //No ticket here
                case "Holey Gloves":
                    if (enjymntProgBar.Value > 2)
                        enjymntProgBar.Value -= 3;
                    else if (enjymntProgBar.Value > 1)
                        enjymntProgBar.Value -= 2;
                    else
                        enjymntProgBar.Value -= 1;

                    if (exhaustProgBar.Value > 1)
                        exhaustProgBar.Value -= 2;
                    else
                        exhaustProgBar.Value -= 1;

                    foreach (string key in randomEvents.Keys)
                    {
                        if (key.Equals("Small Wreck"))
                        {
                            randomEvents[key].increaseChance(15.0);
                        }
                    }
                    break;
                case "Lost":
                    dayNumTextBox.Text = Convert.ToInt32(dayNumTextBox.Text) + 1 + "";
                    break;
                default:
                    break;
            }
            tatMapPB.Visible = true;
            grpbxRandomEvent.Visible = false;
            enableChoices();
            currentEvent.resetChance();   
        }

        public void takeoutMoney(double amount)
        {
            double wallet = Convert.ToDouble(cashTextBox.Text.ToString().Substring(1));                   
            if ((wallet - amount) > 0.0)
            {
                cashTextBox.Text = "$" + (wallet - amount);
            }
            else
            {
                statAtZero = true;
            }
        }
        
    }
}
