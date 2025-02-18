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
            if (_signingUp)
            {
                // add account to DB, with input validation and stuff
            } else
            {
                // find account in DB check pwd hash, with input validation and stuff
            }

            this.Hide();
            Game game = new Game();
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
            } else
            {
                lblMainText.Text = "Log In";
                btnSwitch.Text = "Don't have an account? Sign up!";
            }
        }
    }
}
