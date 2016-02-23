namespace GalacticConquest.Launcher
{
	partial class TestForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.txtProfile = new System.Windows.Forms.TextBox();
			this.btnStartGame = new System.Windows.Forms.Button();
			this.btnForceload = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Profile Name:";
			// 
			// txtProfile
			// 
			this.txtProfile.Location = new System.Drawing.Point(13, 30);
			this.txtProfile.Name = "txtProfile";
			this.txtProfile.Size = new System.Drawing.Size(100, 20);
			this.txtProfile.TabIndex = 1;
			// 
			// btnStartGame
			// 
			this.btnStartGame.Location = new System.Drawing.Point(13, 226);
			this.btnStartGame.Name = "btnStartGame";
			this.btnStartGame.Size = new System.Drawing.Size(75, 23);
			this.btnStartGame.TabIndex = 2;
			this.btnStartGame.Text = "Start Game";
			this.btnStartGame.UseVisualStyleBackColor = true;
			this.btnStartGame.Click += new System.EventHandler(this.btnStartGame_Click);
			// 
			// btnForceload
			// 
			this.btnForceload.Location = new System.Drawing.Point(12, 110);
			this.btnForceload.Name = "btnForceload";
			this.btnForceload.Size = new System.Drawing.Size(75, 23);
			this.btnForceload.TabIndex = 4;
			this.btnForceload.Text = "Force Load";
			this.btnForceload.UseVisualStyleBackColor = true;
			this.btnForceload.Click += new System.EventHandler(this.btnForceload_Click);
			// 
			// TestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.btnForceload);
			this.Controls.Add(this.btnStartGame);
			this.Controls.Add(this.txtProfile);
			this.Controls.Add(this.label1);
			this.Name = "TestForm";
			this.Text = "TestForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtProfile;
		private System.Windows.Forms.Button btnStartGame;
		private System.Windows.Forms.Button btnForceload;
	}
}