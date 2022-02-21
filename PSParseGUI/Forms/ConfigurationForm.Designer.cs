namespace PSParseGUI.Forms
{
    partial class ConfigurationForm
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
            this.HandMatchTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TableMatchTextBox = new System.Windows.Forms.TextBox();
            this.CancelConfigButton = new System.Windows.Forms.Button();
            this.SaveConfigButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hand Match Text - Text in the Green Square";
            // 
            // HandMatchTextBox
            // 
            this.HandMatchTextBox.Location = new System.Drawing.Point(12, 146);
            this.HandMatchTextBox.Name = "HandMatchTextBox";
            this.HandMatchTextBox.Size = new System.Drawing.Size(458, 20);
            this.HandMatchTextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 174);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(210, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Table Match Text - Text in the Red Square";
            // 
            // TableMatchTextBox
            // 
            this.TableMatchTextBox.Location = new System.Drawing.Point(15, 190);
            this.TableMatchTextBox.Name = "TableMatchTextBox";
            this.TableMatchTextBox.Size = new System.Drawing.Size(455, 20);
            this.TableMatchTextBox.TabIndex = 2;
            // 
            // CancelConfigButton
            // 
            this.CancelConfigButton.Location = new System.Drawing.Point(395, 216);
            this.CancelConfigButton.Name = "CancelConfigButton";
            this.CancelConfigButton.Size = new System.Drawing.Size(75, 23);
            this.CancelConfigButton.TabIndex = 4;
            this.CancelConfigButton.Text = "Cancel";
            this.CancelConfigButton.UseVisualStyleBackColor = true;
            this.CancelConfigButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SaveConfigButton
            // 
            this.SaveConfigButton.Location = new System.Drawing.Point(314, 216);
            this.SaveConfigButton.Name = "SaveConfigButton";
            this.SaveConfigButton.Size = new System.Drawing.Size(75, 23);
            this.SaveConfigButton.TabIndex = 3;
            this.SaveConfigButton.Text = "Save";
            this.SaveConfigButton.UseVisualStyleBackColor = true;
            this.SaveConfigButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PSParseGUI.Properties.Resources.HandConfigurationLogExample;
            this.pictureBox1.Location = new System.Drawing.Point(12, 38);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(458, 76);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.MaximumSize = new System.Drawing.Size(450, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(438, 26);
            this.label3.TabIndex = 6;
            this.label3.Text = "Open the Hand History Panel in Stars and find any hand, then copy the values higl" +
    "ighted in the example picture below into the text boxes.. If blank, will match a" +
    "ll table names.";
            // 
            // ConfigurationForm
            // 
            this.AcceptButton = this.SaveConfigButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 244);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.SaveConfigButton);
            this.Controls.Add(this.CancelConfigButton);
            this.Controls.Add(this.TableMatchTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.HandMatchTextBox);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "ConfigurationForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Hand Parse Configuration";
            this.Shown += new System.EventHandler(this.ConfigurationForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox HandMatchTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TableMatchTextBox;
        private System.Windows.Forms.Button CancelConfigButton;
        private System.Windows.Forms.Button SaveConfigButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
    }
}