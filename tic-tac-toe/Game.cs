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
        private string _username;
        private GameManager _manager;

        short player = 0;

        public Game(string usr = "")
        {
            InitializeComponent();

            // If no username is provided, we're playing offline.
            if (usr == "")
            {
                _manager = new GameManager();
            }
            else
            {
                try
                {
                    // Can connect => works
                    _manager = new GameManagerOnline();
                }
                catch
                {
                    MessageBox.Show("Failed to connect to online server!\nYou can still play localy.", "Can't connect to server!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Can't connect => still works, but only localy.
                    _manager = new GameManager();
                }
            }

            _username = usr;

            if (_manager is GameManagerOnline)
            {
                GameManagerOnline manager = (GameManagerOnline)_manager;
                manager.OnDataUpdated += UpdateDisplays;
                manager.Login(usr);
                manager.GetConnected();
            }
        }

        public void onBtnClick(object sender, EventArgs e)
        {
            if (!_manager.CanPlay) return;

            Button clicked = sender as Button;
            int i = Convert.ToInt32(clicked.Name[clicked.Name.Length - 1] - '0') - 1;

            if (_manager[i] == 0)
            {
                _manager[i] = player + 1;
                player++;
            }

            if (player == 2)
            {
                player = 0;
            }

            DisplayImageList(_manager.Images);

            int victor = _manager.CheckForVictory();
            if (victor > 0)
            {
                MessageBox.Show($"Player {victor} won!");
                _manager = new GameManager();
                DisplayImageList(_manager.Images);
                player = 0;
            }
            if (victor < 0)
            {
                MessageBox.Show("Stalemate!");
                _manager = new GameManager();
                DisplayImageList(_manager.Images);
                player = 0;
            }
        }

        /// <summary>
        /// This is a stupid way to do this, but it works so I'm not complaining.
        /// </summary>
        /// <param name="images">9 Images to display</param>
        private void DisplayImageList(Bitmap[] images)
        {
            if (images.Length != 9)
            {
                throw new ArgumentException("Must have 9 images");
            }

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

        /// <summary>
        /// Called when important data, such as who is online, is updated, and updates the displays accordigly.
        /// <br/>
        /// Only useful if online.
        /// </summary>
        private void UpdateDisplays()
        {
            Console.WriteLine("Called");

            if (!(_manager is GameManagerOnline))
            {
                return;
            }
            GameManagerOnline manager = (GameManagerOnline)_manager;
            Console.WriteLine("Is online");

            // Update the list of images / current game
            DisplayImageList(manager.Images);

            // Update the list of online people.
            gbChatboxes.Controls.OfType<Button>()
                .Where(x => x.Name != "btnPlaySolo" && x.Parent == gbChatboxes)
                .ToList()
                .ForEach(x =>
                {
                    Debug.WriteLine($"Button: {x.Name}, Parent: {x.Parent?.Name}");
                    x.Dispose();  // Or replace with x.Visible = false; depending on your goal
                });

            foreach (string name in manager.OnlinePeople)
            {
                Button btn = new Button();
                btn.Text = name;
                btn.Name = $"btn{name}";
                btn.Click += (sender, e) =>
                {
                    manager.Challenge(name);
                };

                MessageBox.Show("Unfortunatly, the chat and challenge features are not implemented yet.", "Not implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnReloadList_Click(object sender, EventArgs e)
        {
            if (!(_manager is GameManagerOnline))
            {
                return;
            }
            GameManagerOnline manager = (GameManagerOnline)_manager;

            manager.GetConnected();
        }
    }
}
