namespace tic_tac_toe
{
    partial class Game
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tpChats = new TabPage();
            groupBox1 = new GroupBox();
            btnPlaySolo = new Button();
            tpGame = new TabPage();
            img9 = new Button();
            img8 = new Button();
            img7 = new Button();
            img6 = new Button();
            img5 = new Button();
            img4 = new Button();
            img3 = new Button();
            img2 = new Button();
            img1 = new Button();
            tabControl1.SuspendLayout();
            tpChats.SuspendLayout();
            groupBox1.SuspendLayout();
            tpGame.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tpChats);
            tabControl1.Controls.Add(tpGame);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1064, 681);
            tabControl1.TabIndex = 0;
            // 
            // tpChats
            // 
            tpChats.Controls.Add(groupBox1);
            tpChats.Location = new Point(4, 24);
            tpChats.Name = "tpChats";
            tpChats.Padding = new Padding(3);
            tpChats.Size = new Size(1056, 653);
            tpChats.TabIndex = 0;
            tpChats.Text = "Chat";
            tpChats.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.AutoSize = true;
            groupBox1.Controls.Add(btnPlaySolo);
            groupBox1.Dock = DockStyle.Left;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(124, 647);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Chats";
            // 
            // btnPlaySolo
            // 
            btnPlaySolo.Location = new Point(3, 19);
            btnPlaySolo.Name = "btnPlaySolo";
            btnPlaySolo.Size = new Size(115, 47);
            btnPlaySolo.TabIndex = 0;
            btnPlaySolo.Text = "Play against self / hotseat";
            btnPlaySolo.UseVisualStyleBackColor = true;
            btnPlaySolo.Click += btnPlaySolo_Click;
            // 
            // tpGame
            // 
            tpGame.Controls.Add(img9);
            tpGame.Controls.Add(img8);
            tpGame.Controls.Add(img7);
            tpGame.Controls.Add(img6);
            tpGame.Controls.Add(img5);
            tpGame.Controls.Add(img4);
            tpGame.Controls.Add(img3);
            tpGame.Controls.Add(img2);
            tpGame.Controls.Add(img1);
            tpGame.Location = new Point(4, 24);
            tpGame.Name = "tpGame";
            tpGame.Padding = new Padding(3);
            tpGame.Size = new Size(1056, 653);
            tpGame.TabIndex = 1;
            tpGame.Text = "Game";
            tpGame.UseVisualStyleBackColor = true;
            // 
            // img9
            // 
            img9.Location = new Point(345, 364);
            img9.Name = "img9";
            img9.Size = new Size(100, 100);
            img9.TabIndex = 13;
            img9.UseVisualStyleBackColor = true;
            img9.Click += onBtnClick;
            // 
            // img8
            // 
            img8.Location = new Point(239, 364);
            img8.Name = "img8";
            img8.Size = new Size(100, 100);
            img8.TabIndex = 12;
            img8.UseVisualStyleBackColor = true;
            img8.Click += onBtnClick;
            // 
            // img7
            // 
            img7.Location = new Point(133, 364);
            img7.Name = "img7";
            img7.Size = new Size(100, 100);
            img7.TabIndex = 11;
            img7.UseVisualStyleBackColor = true;
            img7.Click += onBtnClick;
            // 
            // img6
            // 
            img6.Location = new Point(345, 258);
            img6.Name = "img6";
            img6.Size = new Size(100, 100);
            img6.TabIndex = 10;
            img6.UseVisualStyleBackColor = true;
            img6.Click += onBtnClick;
            // 
            // img5
            // 
            img5.Location = new Point(239, 258);
            img5.Name = "img5";
            img5.Size = new Size(100, 100);
            img5.TabIndex = 9;
            img5.UseVisualStyleBackColor = true;
            img5.Click += onBtnClick;
            // 
            // img4
            // 
            img4.Location = new Point(133, 258);
            img4.Name = "img4";
            img4.Size = new Size(100, 100);
            img4.TabIndex = 8;
            img4.UseVisualStyleBackColor = true;
            img4.Click += onBtnClick;
            // 
            // img3
            // 
            img3.Location = new Point(345, 152);
            img3.Name = "img3";
            img3.Size = new Size(100, 100);
            img3.TabIndex = 7;
            img3.UseVisualStyleBackColor = true;
            img3.Click += onBtnClick;
            // 
            // img2
            // 
            img2.Location = new Point(239, 152);
            img2.Name = "img2";
            img2.Size = new Size(100, 100);
            img2.TabIndex = 4;
            img2.UseVisualStyleBackColor = true;
            img2.Click += onBtnClick;
            // 
            // img1
            // 
            img1.Location = new Point(133, 152);
            img1.Name = "img1";
            img1.Size = new Size(100, 100);
            img1.TabIndex = 1;
            img1.UseVisualStyleBackColor = true;
            img1.Click += onBtnClick;
            // 
            // Game
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1064, 681);
            Controls.Add(tabControl1);
            Name = "Game";
            Text = "Game";
            tabControl1.ResumeLayout(false);
            tpChats.ResumeLayout(false);
            tpChats.PerformLayout();
            groupBox1.ResumeLayout(false);
            tpGame.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tpChats;
        private TabPage tpGame;
        private Button img9;
        private Button img8;
        private Button img7;
        private Button img6;
        private Button img5;
        private Button img4;
        private Button img3;
        private Button img2;
        private Button img1;
        private GroupBox groupBox1;
        private Button btnPlaySolo;
    }
}