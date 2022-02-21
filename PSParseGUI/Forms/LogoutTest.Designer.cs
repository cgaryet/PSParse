namespace PSParseGUI.Forms
{
    partial class LogoutTest
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
            this.StatusText = new System.Windows.Forms.TextBox();
            this.CloseTestButton = new System.Windows.Forms.Button();
            this.TestLogoutButton = new System.Windows.Forms.Button();
            this.ManualAddressInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // StatusText
            // 
            this.StatusText.Enabled = false;
            this.StatusText.Location = new System.Drawing.Point(12, 12);
            this.StatusText.Multiline = true;
            this.StatusText.Name = "StatusText";
            this.StatusText.Size = new System.Drawing.Size(281, 387);
            this.StatusText.TabIndex = 0;
            // 
            // CloseTestButton
            // 
            this.CloseTestButton.Location = new System.Drawing.Point(218, 444);
            this.CloseTestButton.Name = "CloseTestButton";
            this.CloseTestButton.Size = new System.Drawing.Size(75, 23);
            this.CloseTestButton.TabIndex = 1;
            this.CloseTestButton.Text = "Close Test";
            this.CloseTestButton.UseVisualStyleBackColor = true;
            this.CloseTestButton.Click += new System.EventHandler(this.CloseTestButton_Click);
            // 
            // TestLogoutButton
            // 
            this.TestLogoutButton.Location = new System.Drawing.Point(12, 444);
            this.TestLogoutButton.Name = "TestLogoutButton";
            this.TestLogoutButton.Size = new System.Drawing.Size(75, 23);
            this.TestLogoutButton.TabIndex = 2;
            this.TestLogoutButton.Text = "Test Logout";
            this.TestLogoutButton.UseVisualStyleBackColor = true;
            this.TestLogoutButton.Click += new System.EventHandler(this.TestLogoutButton_Click);
            // 
            // ManualAddressInput
            // 
            this.ManualAddressInput.Location = new System.Drawing.Point(12, 418);
            this.ManualAddressInput.Name = "ManualAddressInput";
            this.ManualAddressInput.Size = new System.Drawing.Size(280, 20);
            this.ManualAddressInput.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 402);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Manual Address Input";
            // 
            // LogoutTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 479);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ManualAddressInput);
            this.Controls.Add(this.TestLogoutButton);
            this.Controls.Add(this.CloseTestButton);
            this.Controls.Add(this.StatusText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "LogoutTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Test Logout Hook";
            this.Load += new System.EventHandler(this.LogoutTest_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox StatusText;
        private System.Windows.Forms.Button CloseTestButton;
        private System.Windows.Forms.Button TestLogoutButton;
        private System.Windows.Forms.TextBox ManualAddressInput;
        private System.Windows.Forms.Label label1;
    }
}