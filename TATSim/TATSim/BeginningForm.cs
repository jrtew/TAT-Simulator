using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TATSim
{
    public partial class TATSimForm : Form
    {
        public Motorcycle playersMoto = new Motorcycle();
        public int startCash = 5000;
        //public Tire playersTire = new Tire();
        //public Tank playersTank = new Tank();
        //public Exhaust playersExhaust = new Exhaust();

        public TATSimForm()
        {
            InitializeComponent();
            startScreenPanel.Visible = true;
            motoSelectPanel.Visible = false;
            gearSelectPanel.Visible = false;
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

                motoSelectPanel.Visible = false;
                gearSelectPanel.Visible = true;
                gearSelectMotoPB.Image = playersMoto.Image;
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

                startCash -= playersMoto.Tires.Cost;
                startCash -= playersMoto.GasTank.Cost;
                startCash -= playersMoto.TheExhaust.Cost;

                //playersMoto.Tires = playersTire;
                //playersMoto.GasTank = playersTank;
                //playersMoto.TheExhaust = playersExhaust;

                GameBoardForm gbForm = new GameBoardForm(this);
                gbForm.Show();
                this.Hide();
            }
        }
    }
}
