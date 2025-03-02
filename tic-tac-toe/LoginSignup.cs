using System.Security.Cryptography;
using System.Text;

namespace tic_tac_toe
{
    public partial class LoginSignup : Form
    {
        private bool _signingUp = false;

        public LoginSignup()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // partly taken from https://stackoverflow.com/a/10402129
            DBConnector db = DBConnector.Instance;

            if (!DBConnector.TestConnection())
            {
                MessageBox.Show("Failed to connect to the servers. You can still play offline.", "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.Hide();
                Game gameOffline = new Game();
                gameOffline.Closed += (s, args) => this.Close();
                gameOffline.Show();

                return;
            }

            if (txtUsername.Text.Trim() == string.Empty || txtPassword.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter a username and password!", "User Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_signingUp)
            {
                if (db.UserExists(txtUsername.Text))
                {
                    MessageBox.Show("User with this username already exists!", "User Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte[] salt = new byte[16];
                RandomNumberGenerator.Create().GetBytes(salt);

                using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(txtPassword.Text, salt, 10000, HashAlgorithmName.SHA256))
                {
                    byte[] hash = pbkdf2.GetBytes(20);

                    // Combine salt and hash
                    byte[] hashBytes = new byte[36];
                    Array.Copy(salt, 0, hashBytes, 0, 16);
                    Array.Copy(hash, 0, hashBytes, 16, 20);

                    // Store as Base64
                    string base64Hash = Convert.ToBase64String(hashBytes);
                    txtPassword.Text = string.Empty;

                    db.InsertUser(txtUsername.Text, base64Hash);
                }
            }
            else
            {
                if (!db.UserExists(txtUsername.Text))
                {
                    MessageBox.Show("User with this username doesn't exist!", "User Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string storedHash = db.GetUserPasswordHash(txtUsername.Text);
                if (storedHash == null)
                {
                    MessageBox.Show("Error retrieving user data.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte[] hashBytes = Convert.FromBase64String(storedHash);

                byte[] salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);

                using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(txtPassword.Text, salt, 10000, HashAlgorithmName.SHA256))
                {
                    byte[] hash = pbkdf2.GetBytes(20);
                    txtPassword.Text = string.Empty;

                    for (int i = 0; i < 20; i++)
                    {
                        if (hashBytes[i + 16] != hash[i])
                        {
                            MessageBox.Show("Incorrect password!", "User Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }

            this.Hide();
            Game game = new Game(txtUsername.Text);
            game.Closed += (s, args) => this.Close();
            game.Show();
        }



        private void btnSwitch_Click(object sender, EventArgs e)
        {
            _signingUp = !_signingUp;

            if (_signingUp)
            {
                lblMainText.Text = "Sign Up!";
                btnSwitch.Text = "Have an account? Log in!";
            }
            else
            {
                lblMainText.Text = "Log In";
                btnSwitch.Text = "Don't have an account? Sign up!";
            }
        }
    }
}
