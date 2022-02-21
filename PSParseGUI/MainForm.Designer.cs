namespace PSParseGUI
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processHookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attachToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detachToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monitoringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startMonitoringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopMonitoringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testLogoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.Live_Seat1 = new System.Windows.Forms.TextBox();
            this.Live_Seat2 = new System.Windows.Forms.TextBox();
            this.Live_Seat3 = new System.Windows.Forms.TextBox();
            this.Live_Seat4 = new System.Windows.Forms.TextBox();
            this.Live_Seat5 = new System.Windows.Forms.TextBox();
            this.Live_Seat6 = new System.Windows.Forms.TextBox();
            this.Live_Seat7 = new System.Windows.Forms.TextBox();
            this.Live_Seat8 = new System.Windows.Forms.TextBox();
            this.Live_Seat9 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.HandText = new System.Windows.Forms.TextBox();
            this.HandList = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.LiveViewLog = new System.Windows.Forms.DataGridView();
            this.playerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.handNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.starsHandNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LiveViewLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.transactionBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.processHookToolStripMenuItem,
            this.manualToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.modeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1064, 33);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(141, 34);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // processHookToolStripMenuItem
            // 
            this.processHookToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurationToolStripMenuItem,
            this.hookToolStripMenuItem,
            this.monitoringToolStripMenuItem});
            this.processHookToolStripMenuItem.Name = "processHookToolStripMenuItem";
            this.processHookToolStripMenuItem.Size = new System.Drawing.Size(103, 29);
            this.processHookToolStripMenuItem.Text = "Real Time";
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.configurationToolStripMenuItem.Text = "Configuration...";
            this.configurationToolStripMenuItem.Click += new System.EventHandler(this.configurationToolStripMenuItem_Click);
            // 
            // hookToolStripMenuItem
            // 
            this.hookToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.attachToolStripMenuItem,
            this.detachToolStripMenuItem});
            this.hookToolStripMenuItem.Name = "hookToolStripMenuItem";
            this.hookToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.hookToolStripMenuItem.Text = "Hook...";
            // 
            // attachToolStripMenuItem
            // 
            this.attachToolStripMenuItem.Name = "attachToolStripMenuItem";
            this.attachToolStripMenuItem.Size = new System.Drawing.Size(169, 34);
            this.attachToolStripMenuItem.Text = "Attach";
            this.attachToolStripMenuItem.Click += new System.EventHandler(this.attachToolStripMenuItem_Click);
            // 
            // detachToolStripMenuItem
            // 
            this.detachToolStripMenuItem.Enabled = false;
            this.detachToolStripMenuItem.Name = "detachToolStripMenuItem";
            this.detachToolStripMenuItem.Size = new System.Drawing.Size(169, 34);
            this.detachToolStripMenuItem.Text = "Detach";
            this.detachToolStripMenuItem.Click += new System.EventHandler(this.detachToolStripMenuItem_Click);
            // 
            // monitoringToolStripMenuItem
            // 
            this.monitoringToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testLogoutToolStripMenuItem,
            this.startMonitoringToolStripMenuItem,
            this.stopMonitoringToolStripMenuItem});
            this.monitoringToolStripMenuItem.Name = "monitoringToolStripMenuItem";
            this.monitoringToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.monitoringToolStripMenuItem.Text = "Monitoring...";
            // 
            // startMonitoringToolStripMenuItem
            // 
            this.startMonitoringToolStripMenuItem.Enabled = false;
            this.startMonitoringToolStripMenuItem.Name = "startMonitoringToolStripMenuItem";
            this.startMonitoringToolStripMenuItem.Size = new System.Drawing.Size(303, 34);
            this.startMonitoringToolStripMenuItem.Text = "Start Monitoring";
            this.startMonitoringToolStripMenuItem.Click += new System.EventHandler(this.startMonitoringToolStripMenuItem_Click);
            // 
            // stopMonitoringToolStripMenuItem
            // 
            this.stopMonitoringToolStripMenuItem.Enabled = false;
            this.stopMonitoringToolStripMenuItem.Name = "stopMonitoringToolStripMenuItem";
            this.stopMonitoringToolStripMenuItem.Size = new System.Drawing.Size(303, 34);
            this.stopMonitoringToolStripMenuItem.Text = "Stop Monitoring";
            this.stopMonitoringToolStripMenuItem.Click += new System.EventHandler(this.stopMonitoringToolStripMenuItem_Click);
            // 
            // testLogoutToolStripMenuItem
            // 
            this.testLogoutToolStripMenuItem.Enabled = false;
            this.testLogoutToolStripMenuItem.Name = "testLogoutToolStripMenuItem";
            this.testLogoutToolStripMenuItem.Size = new System.Drawing.Size(303, 34);
            this.testLogoutToolStripMenuItem.Text = "Configure Logout Hook";
            this.testLogoutToolStripMenuItem.Click += new System.EventHandler(this.testLogoutToolStripMenuItem_Click);
            // 
            // manualToolStripMenuItem
            // 
            this.manualToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.processToolStripMenuItem});
            this.manualToolStripMenuItem.Name = "manualToolStripMenuItem";
            this.manualToolStripMenuItem.Size = new System.Drawing.Size(97, 29);
            this.manualToolStripMenuItem.Text = "Log Files";
            // 
            // processToolStripMenuItem
            // 
            this.processToolStripMenuItem.Name = "processToolStripMenuItem";
            this.processToolStripMenuItem.Size = new System.Drawing.Size(169, 34);
            this.processToolStripMenuItem.Text = "Import";
            this.processToolStripMenuItem.Click += new System.EventHandler(this.processToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cSVToolStripMenuItem,
            this.dumpHistoryToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(79, 29);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // cSVToolStripMenuItem
            // 
            this.cSVToolStripMenuItem.Name = "cSVToolStripMenuItem";
            this.cSVToolStripMenuItem.Size = new System.Drawing.Size(226, 34);
            this.cSVToolStripMenuItem.Text = "CSV";
            this.cSVToolStripMenuItem.Click += new System.EventHandler(this.cSVToolStripMenuItem_Click);
            // 
            // dumpHistoryToolStripMenuItem
            // 
            this.dumpHistoryToolStripMenuItem.Name = "dumpHistoryToolStripMenuItem";
            this.dumpHistoryToolStripMenuItem.Size = new System.Drawing.Size(226, 34);
            this.dumpHistoryToolStripMenuItem.Text = "Dump History";
            this.dumpHistoryToolStripMenuItem.Click += new System.EventHandler(this.dumpHistoryToolStripMenuItem_Click);
            // 
            // modeToolStripMenuItem
            // 
            this.modeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.debugModeToolStripMenuItem});
            this.modeToolStripMenuItem.Enabled = false;
            this.modeToolStripMenuItem.Name = "modeToolStripMenuItem";
            this.modeToolStripMenuItem.Size = new System.Drawing.Size(75, 29);
            this.modeToolStripMenuItem.Text = "Mode";
            // 
            // debugModeToolStripMenuItem
            // 
            this.debugModeToolStripMenuItem.Name = "debugModeToolStripMenuItem";
            this.debugModeToolStripMenuItem.Size = new System.Drawing.Size(220, 34);
            this.debugModeToolStripMenuItem.Text = "Debug Mode";
            this.debugModeToolStripMenuItem.Click += new System.EventHandler(this.debugModeToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 679);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1064, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(0, 15);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Controls.Add(this.Live_Seat1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.Live_Seat2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.Live_Seat3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.Live_Seat4, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.Live_Seat5, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.Live_Seat6, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.Live_Seat7, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.Live_Seat8, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.Live_Seat9, 4, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(535, 20);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(526, 300);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // Live_Seat1
            // 
            this.Live_Seat1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Live_Seat1.Location = new System.Drawing.Point(3, 211);
            this.Live_Seat1.Multiline = true;
            this.Live_Seat1.Name = "Live_Seat1";
            this.Live_Seat1.ReadOnly = true;
            this.Live_Seat1.Size = new System.Drawing.Size(99, 101);
            this.Live_Seat1.TabIndex = 1;
            this.Live_Seat1.Text = "Seat Empty";
            this.Live_Seat1.Click += new System.EventHandler(this.Live_Seat_Click);
            // 
            // Live_Seat2
            // 
            this.Live_Seat2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Live_Seat2.Location = new System.Drawing.Point(3, 107);
            this.Live_Seat2.Multiline = true;
            this.Live_Seat2.Name = "Live_Seat2";
            this.Live_Seat2.ReadOnly = true;
            this.Live_Seat2.Size = new System.Drawing.Size(99, 98);
            this.Live_Seat2.TabIndex = 2;
            this.Live_Seat2.Text = "Seat Empty";
            this.Live_Seat2.Click += new System.EventHandler(this.Live_Seat_Click);
            // 
            // Live_Seat3
            // 
            this.Live_Seat3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Live_Seat3.Location = new System.Drawing.Point(3, 3);
            this.Live_Seat3.Multiline = true;
            this.Live_Seat3.Name = "Live_Seat3";
            this.Live_Seat3.ReadOnly = true;
            this.Live_Seat3.Size = new System.Drawing.Size(99, 98);
            this.Live_Seat3.TabIndex = 3;
            this.Live_Seat3.Text = "Seat Empty";
            this.Live_Seat3.Click += new System.EventHandler(this.Live_Seat_Click);
            // 
            // Live_Seat4
            // 
            this.Live_Seat4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Live_Seat4.Location = new System.Drawing.Point(108, 3);
            this.Live_Seat4.Multiline = true;
            this.Live_Seat4.Name = "Live_Seat4";
            this.Live_Seat4.ReadOnly = true;
            this.Live_Seat4.Size = new System.Drawing.Size(99, 98);
            this.Live_Seat4.TabIndex = 4;
            this.Live_Seat4.Text = "Seat Empty";
            this.Live_Seat4.Click += new System.EventHandler(this.Live_Seat_Click);
            // 
            // Live_Seat5
            // 
            this.Live_Seat5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Live_Seat5.Location = new System.Drawing.Point(213, 3);
            this.Live_Seat5.Multiline = true;
            this.Live_Seat5.Name = "Live_Seat5";
            this.Live_Seat5.ReadOnly = true;
            this.Live_Seat5.Size = new System.Drawing.Size(99, 98);
            this.Live_Seat5.TabIndex = 5;
            this.Live_Seat5.Text = "Seat Empty";
            this.Live_Seat5.Click += new System.EventHandler(this.Live_Seat_Click);
            // 
            // Live_Seat6
            // 
            this.Live_Seat6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Live_Seat6.Location = new System.Drawing.Point(318, 3);
            this.Live_Seat6.Multiline = true;
            this.Live_Seat6.Name = "Live_Seat6";
            this.Live_Seat6.ReadOnly = true;
            this.Live_Seat6.Size = new System.Drawing.Size(99, 98);
            this.Live_Seat6.TabIndex = 6;
            this.Live_Seat6.Text = "Seat Empty";
            this.Live_Seat6.Click += new System.EventHandler(this.Live_Seat_Click);
            // 
            // Live_Seat7
            // 
            this.Live_Seat7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Live_Seat7.Location = new System.Drawing.Point(423, 3);
            this.Live_Seat7.Multiline = true;
            this.Live_Seat7.Name = "Live_Seat7";
            this.Live_Seat7.ReadOnly = true;
            this.Live_Seat7.Size = new System.Drawing.Size(100, 98);
            this.Live_Seat7.TabIndex = 7;
            this.Live_Seat7.Text = "Seat Empty";
            this.Live_Seat7.Click += new System.EventHandler(this.Live_Seat_Click);
            // 
            // Live_Seat8
            // 
            this.Live_Seat8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Live_Seat8.Location = new System.Drawing.Point(423, 107);
            this.Live_Seat8.Multiline = true;
            this.Live_Seat8.Name = "Live_Seat8";
            this.Live_Seat8.ReadOnly = true;
            this.Live_Seat8.Size = new System.Drawing.Size(100, 98);
            this.Live_Seat8.TabIndex = 8;
            this.Live_Seat8.Text = "Seat Empty";
            this.Live_Seat8.Click += new System.EventHandler(this.Live_Seat_Click);
            // 
            // Live_Seat9
            // 
            this.Live_Seat9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Live_Seat9.Location = new System.Drawing.Point(423, 211);
            this.Live_Seat9.Multiline = true;
            this.Live_Seat9.Name = "Live_Seat9";
            this.Live_Seat9.ReadOnly = true;
            this.Live_Seat9.Size = new System.Drawing.Size(100, 101);
            this.Live_Seat9.TabIndex = 9;
            this.Live_Seat9.Text = "Seat Empty";
            this.Live_Seat9.Click += new System.EventHandler(this.Live_Seat_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(535, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Live View";
            // 
            // HandText
            // 
            this.HandText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HandText.Location = new System.Drawing.Point(2, 345);
            this.HandText.Margin = new System.Windows.Forms.Padding(2);
            this.HandText.Multiline = true;
            this.HandText.Name = "HandText";
            this.HandText.ReadOnly = true;
            this.HandText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.HandText.Size = new System.Drawing.Size(528, 299);
            this.HandText.TabIndex = 1;
            // 
            // HandList
            // 
            this.HandList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HandList.FormattingEnabled = true;
            this.HandList.Location = new System.Drawing.Point(2, 22);
            this.HandList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 0);
            this.HandList.Name = "HandList";
            this.HandList.Size = new System.Drawing.Size(528, 301);
            this.HandList.TabIndex = 0;
            this.HandList.SelectedIndexChanged += new System.EventHandler(this.HandList_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.LiveViewLog, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.HandList, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.HandText, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 33);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1064, 646);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Hand List";
            // 
            // LiveViewLog
            // 
            this.LiveViewLog.AllowUserToAddRows = false;
            this.LiveViewLog.AllowUserToDeleteRows = false;
            this.LiveViewLog.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.LiveViewLog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.LiveViewLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LiveViewLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.playerDataGridViewTextBoxColumn,
            this.transactionTypeDataGridViewTextBoxColumn,
            this.handNumberDataGridViewTextBoxColumn,
            this.starsHandNumberDataGridViewTextBoxColumn,
            this.amountDataGridViewTextBoxColumn});
            this.LiveViewLog.DataSource = this.transactionBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.LiveViewLog.DefaultCellStyle = dataGridViewCellStyle2;
            this.LiveViewLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LiveViewLog.Location = new System.Drawing.Point(535, 346);
            this.LiveViewLog.Name = "LiveViewLog";
            this.LiveViewLog.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.LiveViewLog.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.LiveViewLog.RowHeadersWidth = 62;
            this.LiveViewLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LiveViewLog.Size = new System.Drawing.Size(526, 297);
            this.LiveViewLog.TabIndex = 3;
            // 
            // playerDataGridViewTextBoxColumn
            // 
            this.playerDataGridViewTextBoxColumn.DataPropertyName = "Player";
            this.playerDataGridViewTextBoxColumn.HeaderText = "Player";
            this.playerDataGridViewTextBoxColumn.MinimumWidth = 8;
            this.playerDataGridViewTextBoxColumn.Name = "playerDataGridViewTextBoxColumn";
            this.playerDataGridViewTextBoxColumn.ReadOnly = true;
            this.playerDataGridViewTextBoxColumn.Width = 150;
            // 
            // transactionTypeDataGridViewTextBoxColumn
            // 
            this.transactionTypeDataGridViewTextBoxColumn.DataPropertyName = "TransactionType";
            this.transactionTypeDataGridViewTextBoxColumn.HeaderText = "TransactionType";
            this.transactionTypeDataGridViewTextBoxColumn.MinimumWidth = 8;
            this.transactionTypeDataGridViewTextBoxColumn.Name = "transactionTypeDataGridViewTextBoxColumn";
            this.transactionTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.transactionTypeDataGridViewTextBoxColumn.Width = 150;
            // 
            // handNumberDataGridViewTextBoxColumn
            // 
            this.handNumberDataGridViewTextBoxColumn.DataPropertyName = "HandNumber";
            this.handNumberDataGridViewTextBoxColumn.HeaderText = "HandNumber";
            this.handNumberDataGridViewTextBoxColumn.MinimumWidth = 8;
            this.handNumberDataGridViewTextBoxColumn.Name = "handNumberDataGridViewTextBoxColumn";
            this.handNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.handNumberDataGridViewTextBoxColumn.Width = 150;
            // 
            // starsHandNumberDataGridViewTextBoxColumn
            // 
            this.starsHandNumberDataGridViewTextBoxColumn.DataPropertyName = "StarsHandNumber";
            this.starsHandNumberDataGridViewTextBoxColumn.HeaderText = "StarsHandNumber";
            this.starsHandNumberDataGridViewTextBoxColumn.MinimumWidth = 8;
            this.starsHandNumberDataGridViewTextBoxColumn.Name = "starsHandNumberDataGridViewTextBoxColumn";
            this.starsHandNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.starsHandNumberDataGridViewTextBoxColumn.Width = 150;
            // 
            // amountDataGridViewTextBoxColumn
            // 
            this.amountDataGridViewTextBoxColumn.DataPropertyName = "Amount";
            this.amountDataGridViewTextBoxColumn.HeaderText = "Amount";
            this.amountDataGridViewTextBoxColumn.MinimumWidth = 8;
            this.amountDataGridViewTextBoxColumn.Name = "amountDataGridViewTextBoxColumn";
            this.amountDataGridViewTextBoxColumn.ReadOnly = true;
            this.amountDataGridViewTextBoxColumn.Width = 150;
            // 
            // transactionBindingSource
            // 
            this.transactionBindingSource.DataSource = typeof(PSParseGUI.Model.Parsing.Transaction);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(535, 326);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Transaction History";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 326);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Raw Hand Log Text";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 701);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "PSParse";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LiveViewLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.transactionBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem processHookToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem processToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Live_Seat1;
        private System.Windows.Forms.TextBox Live_Seat2;
        private System.Windows.Forms.TextBox Live_Seat3;
        private System.Windows.Forms.TextBox Live_Seat4;
        private System.Windows.Forms.TextBox Live_Seat5;
        private System.Windows.Forms.TextBox Live_Seat6;
        private System.Windows.Forms.TextBox Live_Seat7;
        private System.Windows.Forms.TextBox Live_Seat8;
        private System.Windows.Forms.TextBox Live_Seat9;
        private System.Windows.Forms.TextBox HandText;
        private System.Windows.Forms.ListBox HandList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView LiveViewLog;
        private System.Windows.Forms.ToolStripMenuItem dumpHistoryToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn playerRefDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource transactionBindingSource;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem modeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hookToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem monitoringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startMonitoringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopMonitoringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testLogoutToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn playerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn handNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn starsHandNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStripMenuItem attachToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detachToolStripMenuItem;
    }
}

