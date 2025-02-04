using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tic_tac_toe
{
    public partial class Game : Form
    {
        string username;
        GameManager manager;

        short player = 0;

        public Game(string usr = "")
        {
            InitializeComponent();
            manager = new GameManager();
            username = usr;
        }

        public void onBtnClick(object sender, EventArgs e)
        {
            Button clicked = sender as Button;
            int i = Convert.ToInt32(clicked.Name[clicked.Name.Length - 1] - '0') - 1;

            if (manager[i] == 0)
            {
                manager[i] = player + 1;
                player++;
            }

            if (player == 2)
            {
                player = 0;
            }

            DisplayImageList(manager.Images);

            int victor = manager.CheckForVictory();
            if (victor > 0)
            {
                MessageBox.Show($"Player {victor} won!");
                manager = new GameManager();
                DisplayImageList(manager.Images);
                player = 0;
            }
            if (victor < 0)
            {
                MessageBox.Show("Stalemate!");
                manager = new GameManager();
                DisplayImageList(manager.Images);
                player = 0;
            }
        }

        /// <summary>
        /// This is a stupid way to do this, but it works so I'm not complaining.
        /// </summary>
        /// <param name="images"></param>
        private void DisplayImageList(Bitmap[] images)
        {
            img1.Image = images[0];
            img2.Image = images[1];
            img3.Image = images[2];
            img4.Image = images[3];
            img5.Image = images[4];
            img6.Image = images[5];
            img7.Image = images[6];
            img8.Image = images[7];
            img9.Image = images[8];
        }

        private void btnPlaySolo_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(1); // sets tab to 1.
        }
    }
}
