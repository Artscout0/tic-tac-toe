namespace tic_tac_toe
{
    partial class LoginSignup
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblUsername = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            lblPassword = new Label();
            lblMainText = new Label();
            btnLogin = new Button();
            btnSwitch = new Button();
            SuspendLayout();
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(430, 296);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(60, 15);
            lblUsername.TabIndex = 0;
            lblUsername.Text = "Username";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(550, 293);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(100, 23);
            txtUsername.TabIndex = 1;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(550, 344);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(100, 23);
            txtPassword.TabIndex = 2;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(430, 347);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(57, 15);
            lblPassword.TabIndex = 3;
            lblPassword.Text = "Password";
            // 
            // lblMainText
            // 
            lblMainText.AutoSize = true;
            lblMainText.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblMainText.Location = new Point(430, 159);
            lblMainText.Name = "lblMainText";
            lblMainText.Size = new Size(160, 65);
            lblMainText.TabIndex = 4;
            lblMainText.Text = "Log In";
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(430, 396);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(220, 48);
            btnLogin.TabIndex = 5;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnSwitch
            // 
            btnSwitch.Location = new Point(430, 450);
            btnSwitch.Name = "btnSwitch";
            btnSwitch.Size = new Size(220, 23);
            btnSwitch.TabIndex = 6;
            btnSwitch.Text = "Don't have an account? Sign up!";
            btnSwitch.UseVisualStyleBackColor = true;
            btnSwitch.Click += btnSwitch_Click;
            // 
            // LoginSignup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1064, 681);
            Controls.Add(btnSwitch);
            Controls.Add(btnLogin);
            Controls.Add(lblMainText);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(lblUsername);
            Name = "LoginSignup";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblUsername;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Label lblPassword;
        private Label lblMainText;
        private Button btnLogin;
        private Button btnSwitch;
    }
}
