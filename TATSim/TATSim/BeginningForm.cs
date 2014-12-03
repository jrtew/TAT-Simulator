using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace TATSim
{
    public partial class TATSimForm : Form
    {
        private SoundPlayer newSoundPlayer;

        public Motorcycle playersMoto = new Motorcycle();
        public int startCash = 1800;
        int tireCost = 0;
        int tankCost = 0;
        int exhCost = 0;
        int tirePerf = 0;
        int tireWeight = 0;
        int tankWeight = 0;
        int tankRange = 0;
        int exhPerf = 0;
        int exhWeight = 0;
        int exhRange = 0;

        public TATSimForm()
        {
            InitializeComponent();
            startScreenPanel.Visible = true;
            motoSelectPanel.Visible = false;
            gearSelectPanel.Visible = false;
            drRadBut_CheckedChanged(new object(), new EventArgs());
        }

        //to make sure program closes completely if X is used to close program
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnInstructions_Click(object sender, EventArgs e)
        {
            this.Hide();

            InstructionsForm instForm = new InstructionsForm();
            instForm.Show();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            startScreenPanel.Visible = false;
            motoSelectPanel.Visible = true;
        }

        private void motoSelectButton_Click(object sender, EventArgs e)
        {
            string motoName = null;

            if (!drRadBut.Checked && !klrRadBut.Checked && !xrRadBut.Checked)
                MessageBox.Show("Please select a motorcycle!", "Message", MessageBoxButtons.OK);
            else
            {
                if (drRadBut.Checked)
                    motoName = "dr";
                else if (klrRadBut.Checked)
                    motoName = "klr";
                else if (xrRadBut.Checked)
                    motoName = "xr";

                playersMoto = CreateMoto.CreateMotoObj(motoName);

                newSoundPlayer = new SoundPlayer("motoStartSound.wav");
                newSoundPlayer.Play();

                motoSelectPanel.Visible = false;
                gearSelectPanel.Visible = true;
                gearSelectMotoPB.Image = playersMoto.Image;

                performProgBar.Value = playersMoto.Performance;
                weightProgBar.Value = playersMoto.Weight;
                rangeProgBar.Value = playersMoto.Range;
                scoutTireRB_CheckedChanged(new object(), new EventArgs());
                resinTankRB_CheckedChanged(new object(), new EventArgs());
                standExhaRB_CheckedChanged(new object(), new EventArgs());
                //walletTB.Text = "$1800";
            }
        }

        private void gearDoneBtn_Click(object sender, EventArgs e)
        {
            string tireType, tankType, exhaustType = null;

            if (!scoutTireRB.Checked && !s244TireRB.Checked && !explrTireRB.Checked)
                MessageBox.Show("Please select some tires!", "Message", MessageBoxButtons.OK);
            else if (!expdTankRB.Checked && !italTankRB.Checked && !resinTankRB.Checked)
                MessageBox.Show("Please select a gas tank!", "Message", MessageBoxButtons.OK);
            else if (!standExhaRB.Checked && !nightStalkExhaRB.Checked && !yoshiExhaRB.Checked)
                MessageBox.Show("Please select an exhaust!", "Message", MessageBoxButtons.OK);
            else
            {
                if (scoutTireRB.Checked)
                    tireType = "scout";
                else if (s244TireRB.Checked)
                    tireType = "s244";
                else
                    tireType = "explor";

                if (expdTankRB.Checked)
                    tankType = "expedition";
                else if (italTankRB.Checked)
                    tankType = "italy";
                else
                    tankType = "resin";

                if (standExhaRB.Checked)
                    exhaustType = "standard";
                else if (nightStalkExhaRB.Checked)
                    exhaustType = "night";
                else
                    exhaustType = "yoshi";

                CreateMoto.CreateTireObj(playersMoto, tireType);
                CreateMoto.CreateTankObj(playersMoto, tankType);
                CreateMoto.CreateExhaustObj(playersMoto, exhaustType);

                //startCash -= playersMoto.Tires.Cost;
                //startCash -= playersMoto.GasTank.Cost;
                //startCash -= playersMoto.TheExhaust.Cost;

                //playersMoto.Tires = playersTire;
                //playersMoto.GasTank = playersTank;
                //playersMoto.TheExhaust = playersExhaust;

                startCash = Convert.ToInt32(walletTB.Text.Replace("$", ""));

                GameBoardForm gbForm = new GameBoardForm(this);
                gbForm.Show();
                this.Hide();
            }
        }

        private void drRadBut_CheckedChanged(object sender, EventArgs e)
        {
            perfromProgBar2.Value = 5;
            weightProgBar2.Value = 4;
            rangeProgBar2.Value = 5;
        }

        private void klrRadBut_CheckedChanged(object sender, EventArgs e)
        {
            perfromProgBar2.Value = 5;
            weightProgBar2.Value = 3;
            rangeProgBar2.Value = 6;
        }

        private void xrRadBut_CheckedChanged(object sender, EventArgs e)
        {
            perfromProgBar2.Value = 6;
            weightProgBar2.Value = 5;
            rangeProgBar2.Value = 3;
        }

        private void scoutTireRB_CheckedChanged(object sender, EventArgs e)
        {
            tirePerf = 1;
            tireWeight = 0;
            UpdateStatusBars(tirePerf, tireWeight, tankWeight, tankRange, exhPerf, exhWeight, exhRange);
            tireCost = 75;
            UpdateWallet(tireCost, tankCost, exhCost, startCash);
        }

        private void s244TireRB_CheckedChanged(object sender, EventArgs e)
        {
            tirePerf = 0;
            tireWeight = 1;
            UpdateStatusBars(tirePerf, tireWeight, tankWeight, tankRange, exhPerf, exhWeight, exhRange);
            tireCost = 50;
            UpdateWallet(tireCost, tankCost, exhCost, startCash);
        }

        private void explrTireRB_CheckedChanged(object sender, EventArgs e)
        {
            tirePerf = 2;
            tireWeight = -1;
            UpdateStatusBars(tirePerf, tireWeight, tankWeight, tankRange, exhPerf, exhWeight, exhRange);
            tireCost = 100;
            UpdateWallet(tireCost, tankCost, exhCost, startCash);
        }

        private void expdTankRB_CheckedChanged(object sender, EventArgs e)
        {
            tankWeight = -2;
            tankRange = 3;
            UpdateStatusBars(tirePerf, tireWeight, tankWeight, tankRange, exhPerf, exhWeight, exhRange);
            tankCost = 350;
            UpdateWallet(tireCost, tankCost, exhCost, startCash);
        }

        private void italTankRB_CheckedChanged(object sender, EventArgs e)
        {
            tankWeight = -1;
            tankRange = 2;
            UpdateStatusBars(tirePerf, tireWeight, tankWeight, tankRange, exhPerf, exhWeight, exhRange);
            tankCost = 300;
            UpdateWallet(tireCost, tankCost, exhCost, startCash);
        }

        private void resinTankRB_CheckedChanged(object sender, EventArgs e)
        {
            tankWeight = 0;
            tankRange = 1;
            UpdateStatusBars(tirePerf, tireWeight, tankWeight, tankRange, exhPerf, exhWeight, exhRange);
            tankCost = 250;
            UpdateWallet(tireCost, tankCost, exhCost, startCash);
        }

        private void standExhaRB_CheckedChanged(object sender, EventArgs e)
        {
            exhPerf = -1;
            exhWeight = 0;
            exhRange = 1;
            UpdateStatusBars(tirePerf, tireWeight, tankWeight, tankRange, exhPerf, exhWeight, exhRange);
            exhCost = 150;
            UpdateWallet(tireCost, tankCost, exhCost, startCash);
        }

        private void nightStalkExhaRB_CheckedChanged(object sender, EventArgs e)
        {
            exhPerf = 1;
            exhWeight = 0;
            exhRange = 0;
            UpdateStatusBars(tirePerf, tireWeight, tankWeight, tankRange, exhPerf, exhWeight, exhRange);
            exhCost = 200;
            UpdateWallet(tireCost, tankCost, exhCost, startCash);
        }

        private void yoshiExhaRB_CheckedChanged(object sender, EventArgs e)
        {
            exhPerf = 2;
            exhWeight = 1;
            exhRange = -1;
            UpdateStatusBars(tirePerf, tireWeight, tankWeight, tankRange, exhPerf, exhWeight, exhRange);
            exhCost = 250;
            UpdateWallet(tireCost, tankCost, exhCost, startCash);
        }

        private void UpdateWallet(int tireAmt, int tankAmt, int exhAmt, int walletAmt)
        {
            walletAmt = (walletAmt - tireAmt - tankAmt - exhAmt);
            walletTB.Text = "$" + walletAmt.ToString();
        }

        private void UpdateStatusBars(int tirePerf, int tireWeight, int tankWeight, int tankRange, int exhPerf, int exhWeight, int exhRange)
        {
            if (playersMoto.Performance + tirePerf + exhPerf > 10)
            {
                performProgBar.Value = 10;
            }
            else
            {
                performProgBar.Value = playersMoto.Performance + tirePerf + exhPerf;
            }

            if (playersMoto.Weight + tireWeight + tankWeight + exhWeight > 10)
            {
                weightProgBar.Value = 10;
            }
            else
            {
                weightProgBar.Value = playersMoto.Weight + tireWeight + tankWeight + exhWeight;
            }

            if (playersMoto.Range + tankRange + exhRange > 10)
            {
                rangeProgBar.Value = 10;
            }
            else
            {
                rangeProgBar.Value = playersMoto.Range + tankRange + exhRange;
            }
        }

        private void label27_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\n- Each item has different pros and cons\n" +
                "\n- Check out how the item impacts your motorcycle's stats by selecting the item\n" +
                "\n- Your available money will update as you select different items, watch out how much you spend!\n" +
                "\n- If you choose the most expensive item in each category, you will most likely not be able to finish the trip!\n" +
                "\n- Choose your items wisely!", "Quick Tips", MessageBoxButtons.OK);
        }
    }
}
