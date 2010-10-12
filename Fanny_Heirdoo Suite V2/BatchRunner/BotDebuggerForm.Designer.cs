namespace BatchRunner
{
	partial class BotDebuggerForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BotDebuggerForm));
			this.tabControlBotDebugger = new System.Windows.Forms.TabControl();
			this.tabPageBatchRun = new System.Windows.Forms.TabPage();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.buttonRunSelected = new System.Windows.Forms.Button();
			this.buttonDebugMatch = new System.Windows.Forms.Button();
			this.labelProgress = new System.Windows.Forms.Label();
			this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
			this.label5 = new System.Windows.Forms.Label();
			this.comboBoxRunID = new System.Windows.Forms.ComboBox();
			this.button3 = new System.Windows.Forms.Button();
			this.buttonStart = new System.Windows.Forms.Button();
			this.buttonClearRun = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnCreate = new System.Windows.Forms.Button();
			this.buttonLoad = new System.Windows.Forms.Button();
			this.buttonSave = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.textBoxMaxBatchTurns = new System.Windows.Forms.TextBox();
			this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
			this.buttonResetFilter = new System.Windows.Forms.Button();
			this.buttonShowLostGames = new System.Windows.Forms.Button();
			this.buttonTiedOrLost = new System.Windows.Forms.Button();
			this.buttonTied = new System.Windows.Forms.Button();
			this.buttonWon = new System.Windows.Forms.Button();
			this.tabPageDebugBot = new System.Windows.Forms.TabPage();
			this.panelRender = new BatchRunner.Renderer();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.buttonDebugMove = new System.Windows.Forms.Button();
			this.checkBoxParallel = new System.Windows.Forms.CheckBox();
			this.cbRender = new System.Windows.Forms.CheckBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPagePlayers = new System.Windows.Forms.TabPage();
			this.groupBoxPlayer2 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.comboBoxOpponent2 = new System.Windows.Forms.ComboBox();
			this.cbOpponentTwoOwnBot = new System.Windows.Forms.CheckBox();
			this.groupBoxPlayer1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.comboBoxOpponent1 = new System.Windows.Forms.ComboBox();
			this.cbOpponentOneOwnBot = new System.Windows.Forms.CheckBox();
			this.tabPageRenderingOptions = new System.Windows.Forms.TabPage();
			this.checkBoxDrawPlayerOne = new System.Windows.Forms.CheckBox();
			this.trackBarRenderDelay = new System.Windows.Forms.TrackBar();
			this.checkBoxDrawPlayerTwo = new System.Windows.Forms.CheckBox();
			this.checkboxDrawAttacklines = new System.Windows.Forms.CheckBox();
			this.checkBoxPlanetStatitics = new System.Windows.Forms.CheckBox();
			this.checkBoxDrawUniverseStatistics = new System.Windows.Forms.CheckBox();
			this.checkBoxDrawFleetArrival = new System.Windows.Forms.CheckBox();
			this.checkBoxGridLines = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxTurns = new System.Windows.Forms.TextBox();
			this.btnPrevious = new System.Windows.Forms.Button();
			this.button12 = new System.Windows.Forms.Button();
			this.buttonPlay = new System.Windows.Forms.Button();
			this.buttonStop = new System.Windows.Forms.Button();
			this.buttonLaunchInReference = new System.Windows.Forms.Button();
			this.labelTurn = new System.Windows.Forms.Label();
			this.textBoxBreakPoint = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.checkBoxBreakpoint = new System.Windows.Forms.CheckBox();
			this.buttonNextMove = new System.Windows.Forms.Button();
			this.labelBreakpoint = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.labelNeutralPlanetColor = new System.Windows.Forms.Label();
			this.tabControlBotDebugger.SuspendLayout();
			this.tabPageBatchRun.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.tableLayoutPanel4.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			this.flowLayoutPanel4.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel6.SuspendLayout();
			this.tabPageDebugBot.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPagePlayers.SuspendLayout();
			this.groupBoxPlayer2.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.groupBoxPlayer1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tabPageRenderingOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarRenderDelay)).BeginInit();
			this.tabPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControlBotDebugger
			// 
			this.tabControlBotDebugger.Controls.Add(this.tabPageBatchRun);
			this.tabControlBotDebugger.Controls.Add(this.tabPageDebugBot);
			this.tabControlBotDebugger.Controls.Add(this.tabPage1);
			this.tabControlBotDebugger.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlBotDebugger.Location = new System.Drawing.Point(0, 0);
			this.tabControlBotDebugger.Name = "tabControlBotDebugger";
			this.tabControlBotDebugger.SelectedIndex = 0;
			this.tabControlBotDebugger.Size = new System.Drawing.Size(794, 622);
			this.tabControlBotDebugger.TabIndex = 5;
			// 
			// tabPageBatchRun
			// 
			this.tabPageBatchRun.Controls.Add(this.dataGridView1);
			this.tabPageBatchRun.Controls.Add(this.tableLayoutPanel4);
			this.tabPageBatchRun.Location = new System.Drawing.Point(4, 22);
			this.tabPageBatchRun.Name = "tabPageBatchRun";
			this.tabPageBatchRun.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageBatchRun.Size = new System.Drawing.Size(786, 596);
			this.tabPageBatchRun.TabIndex = 1;
			this.tabPageBatchRun.Text = "Batch run";
			this.tabPageBatchRun.UseVisualStyleBackColor = true;
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.dataGridView1.Location = new System.Drawing.Point(3, 79);
			this.dataGridView1.MultiSelect = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView1.Size = new System.Drawing.Size(780, 514);
			this.dataGridView1.TabIndex = 5;
			this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.94872F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.05128F));
			this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel3, 1, 1);
			this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel4, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel1, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel6, 0, 1);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 2;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.Size = new System.Drawing.Size(780, 76);
			this.tableLayoutPanel4.TabIndex = 7;
			// 
			// flowLayoutPanel3
			// 
			this.flowLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.flowLayoutPanel3.Controls.Add(this.buttonRunSelected);
			this.flowLayoutPanel3.Controls.Add(this.buttonDebugMatch);
			this.flowLayoutPanel3.Controls.Add(this.labelProgress);
			this.flowLayoutPanel3.Location = new System.Drawing.Point(416, 38);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size(360, 35);
			this.flowLayoutPanel3.TabIndex = 6;
			// 
			// buttonRunSelected
			// 
			this.buttonRunSelected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.buttonRunSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
			this.buttonRunSelected.Location = new System.Drawing.Point(3, 3);
			this.buttonRunSelected.Name = "buttonRunSelected";
			this.buttonRunSelected.Size = new System.Drawing.Size(124, 29);
			this.buttonRunSelected.TabIndex = 7;
			this.buttonRunSelected.Text = "Run Selected match";
			this.buttonRunSelected.UseVisualStyleBackColor = false;
			this.buttonRunSelected.Click += new System.EventHandler(this.buttonRunSelected_Click);
			// 
			// buttonDebugMatch
			// 
			this.buttonDebugMatch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.buttonDebugMatch.Location = new System.Drawing.Point(133, 3);
			this.buttonDebugMatch.Name = "buttonDebugMatch";
			this.buttonDebugMatch.Size = new System.Drawing.Size(101, 29);
			this.buttonDebugMatch.TabIndex = 0;
			this.buttonDebugMatch.Text = "Debug match";
			this.buttonDebugMatch.UseVisualStyleBackColor = false;
			this.buttonDebugMatch.Click += new System.EventHandler(this.buttonDebugMatch_Click);
			// 
			// labelProgress
			// 
			this.labelProgress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.labelProgress.AutoSize = true;
			this.labelProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelProgress.Location = new System.Drawing.Point(240, 8);
			this.labelProgress.Name = "labelProgress";
			this.labelProgress.Size = new System.Drawing.Size(77, 18);
			this.labelProgress.TabIndex = 8;
			this.labelProgress.Text = "Progress";
			this.labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// flowLayoutPanel4
			// 
			this.flowLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel4.AutoSize = true;
			this.flowLayoutPanel4.Controls.Add(this.label5);
			this.flowLayoutPanel4.Controls.Add(this.comboBoxRunID);
			this.flowLayoutPanel4.Controls.Add(this.button3);
			this.flowLayoutPanel4.Controls.Add(this.buttonStart);
			this.flowLayoutPanel4.Controls.Add(this.buttonClearRun);
			this.flowLayoutPanel4.Location = new System.Drawing.Point(416, 3);
			this.flowLayoutPanel4.Name = "flowLayoutPanel4";
			this.flowLayoutPanel4.Size = new System.Drawing.Size(361, 29);
			this.flowLayoutPanel4.TabIndex = 0;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.ForeColor = System.Drawing.Color.DarkMagenta;
			this.label5.Location = new System.Drawing.Point(3, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(41, 29);
			this.label5.TabIndex = 5;
			this.label5.Text = "RunId";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboBoxRunID
			// 
			this.comboBoxRunID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxRunID.FormattingEnabled = true;
			this.comboBoxRunID.Items.AddRange(new object[] {
            "Run 1",
            "Run 2",
            "Run 3"});
			this.comboBoxRunID.Location = new System.Drawing.Point(50, 3);
			this.comboBoxRunID.Name = "comboBoxRunID";
			this.comboBoxRunID.Size = new System.Drawing.Size(62, 21);
			this.comboBoxRunID.TabIndex = 4;
			// 
			// button3
			// 
			this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.button3.Location = new System.Drawing.Point(118, 3);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(74, 23);
			this.button3.TabIndex = 3;
			this.button3.Text = "Stop";
			this.button3.UseVisualStyleBackColor = false;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// buttonStart
			// 
			this.buttonStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.buttonStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonStart.Location = new System.Drawing.Point(198, 3);
			this.buttonStart.Name = "buttonStart";
			this.buttonStart.Size = new System.Drawing.Size(74, 23);
			this.buttonStart.TabIndex = 2;
			this.buttonStart.Text = "Start";
			this.buttonStart.UseVisualStyleBackColor = false;
			this.buttonStart.Click += new System.EventHandler(this.button2_Click);
			// 
			// buttonClearRun
			// 
			this.buttonClearRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
			this.buttonClearRun.Location = new System.Drawing.Point(278, 3);
			this.buttonClearRun.Name = "buttonClearRun";
			this.buttonClearRun.Size = new System.Drawing.Size(77, 23);
			this.buttonClearRun.TabIndex = 8;
			this.buttonClearRun.Text = "Clear run";
			this.buttonClearRun.UseVisualStyleBackColor = false;
			this.buttonClearRun.Click += new System.EventHandler(this.buttonClearRun_Click);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.btnCreate);
			this.flowLayoutPanel1.Controls.Add(this.buttonLoad);
			this.flowLayoutPanel1.Controls.Add(this.buttonSave);
			this.flowLayoutPanel1.Controls.Add(this.label6);
			this.flowLayoutPanel1.Controls.Add(this.textBoxMaxBatchTurns);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(373, 29);
			this.flowLayoutPanel1.TabIndex = 4;
			// 
			// btnCreate
			// 
			this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this.btnCreate.Location = new System.Drawing.Point(3, 3);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.Size = new System.Drawing.Size(77, 23);
			this.btnCreate.TabIndex = 3;
			this.btnCreate.Text = "Create";
			this.btnCreate.UseVisualStyleBackColor = false;
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// buttonLoad
			// 
			this.buttonLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.buttonLoad.Location = new System.Drawing.Point(86, 3);
			this.buttonLoad.Name = "buttonLoad";
			this.buttonLoad.Size = new System.Drawing.Size(74, 23);
			this.buttonLoad.TabIndex = 3;
			this.buttonLoad.Text = "Load";
			this.buttonLoad.UseVisualStyleBackColor = false;
			this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
			// 
			// buttonSave
			// 
			this.buttonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.buttonSave.Location = new System.Drawing.Point(166, 3);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.Size = new System.Drawing.Size(74, 23);
			this.buttonSave.TabIndex = 3;
			this.buttonSave.Text = "Save";
			this.buttonSave.UseVisualStyleBackColor = false;
			this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.ForeColor = System.Drawing.Color.DarkMagenta;
			this.label6.Location = new System.Drawing.Point(246, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(62, 29);
			this.label6.TabIndex = 8;
			this.label6.Text = "MaxTurns";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBoxMaxBatchTurns
			// 
			this.textBoxMaxBatchTurns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxMaxBatchTurns.Location = new System.Drawing.Point(314, 4);
			this.textBoxMaxBatchTurns.Name = "textBoxMaxBatchTurns";
			this.textBoxMaxBatchTurns.Size = new System.Drawing.Size(52, 20);
			this.textBoxMaxBatchTurns.TabIndex = 7;
			this.textBoxMaxBatchTurns.Text = "200";
			// 
			// flowLayoutPanel6
			// 
			this.flowLayoutPanel6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel6.AutoSize = true;
			this.flowLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel6.Controls.Add(this.buttonResetFilter);
			this.flowLayoutPanel6.Controls.Add(this.buttonShowLostGames);
			this.flowLayoutPanel6.Controls.Add(this.buttonTiedOrLost);
			this.flowLayoutPanel6.Controls.Add(this.buttonTied);
			this.flowLayoutPanel6.Controls.Add(this.buttonWon);
			this.flowLayoutPanel6.Location = new System.Drawing.Point(3, 44);
			this.flowLayoutPanel6.Name = "flowLayoutPanel6";
			this.flowLayoutPanel6.Size = new System.Drawing.Size(407, 29);
			this.flowLayoutPanel6.TabIndex = 8;
			// 
			// buttonResetFilter
			// 
			this.buttonResetFilter.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.buttonResetFilter.Location = new System.Drawing.Point(3, 3);
			this.buttonResetFilter.Name = "buttonResetFilter";
			this.buttonResetFilter.Size = new System.Drawing.Size(77, 23);
			this.buttonResetFilter.TabIndex = 0;
			this.buttonResetFilter.Text = "Reset filter";
			this.buttonResetFilter.UseVisualStyleBackColor = true;
			this.buttonResetFilter.Click += new System.EventHandler(this.buttonResetFilter_Click);
			// 
			// buttonShowLostGames
			// 
			this.buttonShowLostGames.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.buttonShowLostGames.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.buttonShowLostGames.Location = new System.Drawing.Point(86, 3);
			this.buttonShowLostGames.Name = "buttonShowLostGames";
			this.buttonShowLostGames.Size = new System.Drawing.Size(74, 23);
			this.buttonShowLostGames.TabIndex = 0;
			this.buttonShowLostGames.Text = "Lost matches";
			this.buttonShowLostGames.UseVisualStyleBackColor = false;
			this.buttonShowLostGames.Click += new System.EventHandler(this.buttonShowLostGames_Click);
			// 
			// buttonTiedOrLost
			// 
			this.buttonTiedOrLost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.buttonTiedOrLost.Location = new System.Drawing.Point(166, 3);
			this.buttonTiedOrLost.Name = "buttonTiedOrLost";
			this.buttonTiedOrLost.Size = new System.Drawing.Size(74, 23);
			this.buttonTiedOrLost.TabIndex = 8;
			this.buttonTiedOrLost.Text = "Tied or  Lost";
			this.buttonTiedOrLost.UseVisualStyleBackColor = false;
			this.buttonTiedOrLost.Click += new System.EventHandler(this.buttonTiedOrLost_Click);
			// 
			// buttonTied
			// 
			this.buttonTied.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
			this.buttonTied.Location = new System.Drawing.Point(246, 3);
			this.buttonTied.Name = "buttonTied";
			this.buttonTied.Size = new System.Drawing.Size(74, 23);
			this.buttonTied.TabIndex = 8;
			this.buttonTied.Text = "Tied";
			this.buttonTied.UseVisualStyleBackColor = false;
			this.buttonTied.Click += new System.EventHandler(this.buttonTied_Click);
			// 
			// buttonWon
			// 
			this.buttonWon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.buttonWon.Location = new System.Drawing.Point(326, 3);
			this.buttonWon.Name = "buttonWon";
			this.buttonWon.Size = new System.Drawing.Size(77, 23);
			this.buttonWon.TabIndex = 2;
			this.buttonWon.Text = "Won";
			this.buttonWon.UseVisualStyleBackColor = false;
			this.buttonWon.Click += new System.EventHandler(this.buttonWon_Click);
			// 
			// tabPageDebugBot
			// 
			this.tabPageDebugBot.Controls.Add(this.panelRender);
			this.tabPageDebugBot.Controls.Add(this.tableLayoutPanel3);
			this.tabPageDebugBot.Location = new System.Drawing.Point(4, 22);
			this.tabPageDebugBot.Name = "tabPageDebugBot";
			this.tabPageDebugBot.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageDebugBot.Size = new System.Drawing.Size(786, 596);
			this.tabPageDebugBot.TabIndex = 2;
			this.tabPageDebugBot.Text = "Debug Match";
			this.tabPageDebugBot.UseVisualStyleBackColor = true;
			// 
			// panelRender
			// 
			this.panelRender.BarSpacing = 2;
			this.panelRender.BarWidth = 6;
			this.panelRender.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelRender.DrawAttacklines = true;
			this.panelRender.DrawFleetArrival = true;
			this.panelRender.DrawPlanetStatistics = true;
			this.panelRender.DrawPlayerOne = true;
			this.panelRender.DrawPlayerTwo = true;
			this.panelRender.DrawUniverseStatistics = true;
			this.panelRender.Location = new System.Drawing.Point(3, 144);
			this.panelRender.Name = "panelRender";
			this.panelRender.ShortestRoute = null;
			this.panelRender.Size = new System.Drawing.Size(780, 449);
			this.panelRender.TabIndex = 8;
			this.panelRender.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRender_Paint);
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.ColumnCount = 5;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 203F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99F));
			this.tableLayoutPanel3.Controls.Add(this.buttonDebugMove, 1, 4);
			this.tableLayoutPanel3.Controls.Add(this.checkBoxParallel, 1, 2);
			this.tableLayoutPanel3.Controls.Add(this.cbRender, 1, 3);
			this.tableLayoutPanel3.Controls.Add(this.tabControl1, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.label4, 3, 0);
			this.tableLayoutPanel3.Controls.Add(this.textBoxTurns, 4, 0);
			this.tableLayoutPanel3.Controls.Add(this.btnPrevious, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.button12, 2, 3);
			this.tableLayoutPanel3.Controls.Add(this.buttonPlay, 3, 3);
			this.tableLayoutPanel3.Controls.Add(this.buttonStop, 4, 3);
			this.tableLayoutPanel3.Controls.Add(this.buttonLaunchInReference, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.labelTurn, 4, 2);
			this.tableLayoutPanel3.Controls.Add(this.textBoxBreakPoint, 4, 1);
			this.tableLayoutPanel3.Controls.Add(this.label3, 3, 2);
			this.tableLayoutPanel3.Controls.Add(this.checkBoxBreakpoint, 3, 1);
			this.tableLayoutPanel3.Controls.Add(this.buttonNextMove, 2, 2);
			this.tableLayoutPanel3.Controls.Add(this.labelBreakpoint, 1, 1);
			this.tableLayoutPanel3.Controls.Add(this.button1, 2, 1);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 5;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(780, 141);
			this.tableLayoutPanel3.TabIndex = 12;
			// 
			// buttonDebugMove
			// 
			this.buttonDebugMove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.buttonDebugMove.Location = new System.Drawing.Point(254, 115);
			this.buttonDebugMove.Name = "buttonDebugMove";
			this.buttonDebugMove.Size = new System.Drawing.Size(100, 23);
			this.buttonDebugMove.TabIndex = 12;
			this.buttonDebugMove.Text = "Debug Move";
			this.buttonDebugMove.UseVisualStyleBackColor = true;
			this.buttonDebugMove.Click += new System.EventHandler(this.buttonDebugMove_Click);
			// 
			// checkBoxParallel
			// 
			this.checkBoxParallel.BackColor = System.Drawing.SystemColors.Control;
			this.checkBoxParallel.Checked = true;
			this.checkBoxParallel.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxParallel.Location = new System.Drawing.Point(254, 62);
			this.checkBoxParallel.Name = "checkBoxParallel";
			this.checkBoxParallel.Size = new System.Drawing.Size(96, 24);
			this.checkBoxParallel.TabIndex = 7;
			this.checkBoxParallel.Text = "Parallel";
			this.checkBoxParallel.UseVisualStyleBackColor = false;
			this.checkBoxParallel.Visible = false;
			// 
			// cbRender
			// 
			this.cbRender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbRender.AutoSize = true;
			this.cbRender.Checked = true;
			this.cbRender.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbRender.Location = new System.Drawing.Point(254, 92);
			this.cbRender.Name = "cbRender";
			this.cbRender.Size = new System.Drawing.Size(100, 17);
			this.cbRender.TabIndex = 7;
			this.cbRender.Text = "Render";
			this.cbRender.UseVisualStyleBackColor = true;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPagePlayers);
			this.tabControl1.Controls.Add(this.tabPageRenderingOptions);
			this.tabControl1.Location = new System.Drawing.Point(3, 3);
			this.tabControl1.Name = "tabControl1";
			this.tableLayoutPanel3.SetRowSpan(this.tabControl1, 5);
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(245, 135);
			this.tabControl1.TabIndex = 9;
			// 
			// tabPagePlayers
			// 
			this.tabPagePlayers.Controls.Add(this.groupBoxPlayer2);
			this.tabPagePlayers.Controls.Add(this.groupBoxPlayer1);
			this.tabPagePlayers.Location = new System.Drawing.Point(4, 22);
			this.tabPagePlayers.Name = "tabPagePlayers";
			this.tabPagePlayers.Padding = new System.Windows.Forms.Padding(3);
			this.tabPagePlayers.Size = new System.Drawing.Size(237, 109);
			this.tabPagePlayers.TabIndex = 0;
			this.tabPagePlayers.Text = "Players";
			this.tabPagePlayers.UseVisualStyleBackColor = true;
			// 
			// groupBoxPlayer2
			// 
			this.groupBoxPlayer2.BackColor = System.Drawing.Color.Indigo;
			this.groupBoxPlayer2.Controls.Add(this.tableLayoutPanel2);
			this.groupBoxPlayer2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBoxPlayer2.ForeColor = System.Drawing.Color.MediumTurquoise;
			this.groupBoxPlayer2.Location = new System.Drawing.Point(3, 61);
			this.groupBoxPlayer2.Name = "groupBoxPlayer2";
			this.groupBoxPlayer2.Size = new System.Drawing.Size(231, 45);
			this.groupBoxPlayer2.TabIndex = 2;
			this.groupBoxPlayer2.TabStop = false;
			this.groupBoxPlayer2.Text = "Opponent Two";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Controls.Add(this.comboBoxOpponent2, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.cbOpponentTwoOwnBot, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(225, 27);
			this.tableLayoutPanel2.TabIndex = 3;
			// 
			// comboBoxOpponent2
			// 
			this.comboBoxOpponent2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxOpponent2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxOpponent2.FormattingEnabled = true;
			this.comboBoxOpponent2.Items.AddRange(new object[] {
            "Bully",
            "Dual",
            "Prospector",
            "Rage",
            "Random"});
			this.comboBoxOpponent2.Location = new System.Drawing.Point(115, 3);
			this.comboBoxOpponent2.Name = "comboBoxOpponent2";
			this.comboBoxOpponent2.Size = new System.Drawing.Size(107, 21);
			this.comboBoxOpponent2.TabIndex = 1;
			// 
			// cbOpponentTwoOwnBot
			// 
			this.cbOpponentTwoOwnBot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbOpponentTwoOwnBot.AutoSize = true;
			this.cbOpponentTwoOwnBot.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.cbOpponentTwoOwnBot.Location = new System.Drawing.Point(3, 5);
			this.cbOpponentTwoOwnBot.Name = "cbOpponentTwoOwnBot";
			this.cbOpponentTwoOwnBot.Size = new System.Drawing.Size(106, 17);
			this.cbOpponentTwoOwnBot.TabIndex = 3;
			this.cbOpponentTwoOwnBot.Text = "My own bot please";
			this.cbOpponentTwoOwnBot.UseVisualStyleBackColor = true;
			this.cbOpponentTwoOwnBot.CheckedChanged += new System.EventHandler(this.cbOpponentTwoOwnBot_CheckedChanged);
			// 
			// groupBoxPlayer1
			// 
			this.groupBoxPlayer1.BackColor = System.Drawing.Color.Maroon;
			this.groupBoxPlayer1.Controls.Add(this.tableLayoutPanel1);
			this.groupBoxPlayer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBoxPlayer1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this.groupBoxPlayer1.Location = new System.Drawing.Point(3, 3);
			this.groupBoxPlayer1.Name = "groupBoxPlayer1";
			this.groupBoxPlayer1.Size = new System.Drawing.Size(231, 53);
			this.groupBoxPlayer1.TabIndex = 2;
			this.groupBoxPlayer1.TabStop = false;
			this.groupBoxPlayer1.Text = "Opponent One";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.comboBoxOpponent1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.cbOpponentOneOwnBot, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(225, 27);
			this.tableLayoutPanel1.TabIndex = 3;
			// 
			// comboBoxOpponent1
			// 
			this.comboBoxOpponent1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxOpponent1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxOpponent1.FormattingEnabled = true;
			this.comboBoxOpponent1.Items.AddRange(new object[] {
            "Bully",
            "Dual",
            "Prospector",
            "Rage",
            "Random"});
			this.comboBoxOpponent1.Location = new System.Drawing.Point(115, 3);
			this.comboBoxOpponent1.Name = "comboBoxOpponent1";
			this.comboBoxOpponent1.Size = new System.Drawing.Size(107, 21);
			this.comboBoxOpponent1.TabIndex = 1;
			// 
			// cbOpponentOneOwnBot
			// 
			this.cbOpponentOneOwnBot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbOpponentOneOwnBot.AutoSize = true;
			this.cbOpponentOneOwnBot.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.cbOpponentOneOwnBot.Location = new System.Drawing.Point(3, 5);
			this.cbOpponentOneOwnBot.Name = "cbOpponentOneOwnBot";
			this.cbOpponentOneOwnBot.Size = new System.Drawing.Size(106, 17);
			this.cbOpponentOneOwnBot.TabIndex = 3;
			this.cbOpponentOneOwnBot.Text = "My own bot please";
			this.cbOpponentOneOwnBot.UseVisualStyleBackColor = true;
			this.cbOpponentOneOwnBot.CheckedChanged += new System.EventHandler(this.cbOpponentOneOwnBot_CheckedChanged);
			// 
			// tabPageRenderingOptions
			// 
			this.tabPageRenderingOptions.Controls.Add(this.checkBoxDrawPlayerOne);
			this.tabPageRenderingOptions.Controls.Add(this.trackBarRenderDelay);
			this.tabPageRenderingOptions.Controls.Add(this.checkBoxDrawPlayerTwo);
			this.tabPageRenderingOptions.Controls.Add(this.checkboxDrawAttacklines);
			this.tabPageRenderingOptions.Controls.Add(this.checkBoxPlanetStatitics);
			this.tabPageRenderingOptions.Controls.Add(this.checkBoxDrawUniverseStatistics);
			this.tabPageRenderingOptions.Controls.Add(this.checkBoxDrawFleetArrival);
			this.tabPageRenderingOptions.Controls.Add(this.checkBoxGridLines);
			this.tabPageRenderingOptions.Location = new System.Drawing.Point(4, 22);
			this.tabPageRenderingOptions.Name = "tabPageRenderingOptions";
			this.tabPageRenderingOptions.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageRenderingOptions.Size = new System.Drawing.Size(237, 109);
			this.tabPageRenderingOptions.TabIndex = 1;
			this.tabPageRenderingOptions.Text = "Rendering";
			this.tabPageRenderingOptions.UseVisualStyleBackColor = true;
			// 
			// checkBoxDrawPlayerOne
			// 
			this.checkBoxDrawPlayerOne.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBoxDrawPlayerOne.AutoSize = true;
			this.checkBoxDrawPlayerOne.Checked = true;
			this.checkBoxDrawPlayerOne.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxDrawPlayerOne.Location = new System.Drawing.Point(144, 8);
			this.checkBoxDrawPlayerOne.Name = "checkBoxDrawPlayerOne";
			this.checkBoxDrawPlayerOne.Size = new System.Drawing.Size(64, 17);
			this.checkBoxDrawPlayerOne.TabIndex = 9;
			this.checkBoxDrawPlayerOne.Text = "Player 2";
			this.checkBoxDrawPlayerOne.UseVisualStyleBackColor = true;
			this.checkBoxDrawPlayerOne.CheckedChanged += new System.EventHandler(this.checkBoxDrawPlayerOne_CheckedChanged);
			// 
			// trackBarRenderDelay
			// 
			this.trackBarRenderDelay.AutoSize = false;
			this.trackBarRenderDelay.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.trackBarRenderDelay.LargeChange = 200;
			this.trackBarRenderDelay.Location = new System.Drawing.Point(3, 72);
			this.trackBarRenderDelay.Maximum = 600;
			this.trackBarRenderDelay.Minimum = 1;
			this.trackBarRenderDelay.Name = "trackBarRenderDelay";
			this.trackBarRenderDelay.Size = new System.Drawing.Size(231, 34);
			this.trackBarRenderDelay.TabIndex = 8;
			this.trackBarRenderDelay.TickFrequency = 20;
			this.trackBarRenderDelay.Value = 75;
			// 
			// checkBoxDrawPlayerTwo
			// 
			this.checkBoxDrawPlayerTwo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBoxDrawPlayerTwo.AutoSize = true;
			this.checkBoxDrawPlayerTwo.Checked = true;
			this.checkBoxDrawPlayerTwo.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxDrawPlayerTwo.Location = new System.Drawing.Point(80, 8);
			this.checkBoxDrawPlayerTwo.Name = "checkBoxDrawPlayerTwo";
			this.checkBoxDrawPlayerTwo.Size = new System.Drawing.Size(64, 17);
			this.checkBoxDrawPlayerTwo.TabIndex = 7;
			this.checkBoxDrawPlayerTwo.Text = "Player 1";
			this.checkBoxDrawPlayerTwo.UseVisualStyleBackColor = true;
			this.checkBoxDrawPlayerTwo.CheckedChanged += new System.EventHandler(this.checkBoxDrawPlayerTwo_CheckedChanged);
			// 
			// checkboxDrawAttacklines
			// 
			this.checkboxDrawAttacklines.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.checkboxDrawAttacklines.AutoSize = true;
			this.checkboxDrawAttacklines.Checked = true;
			this.checkboxDrawAttacklines.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkboxDrawAttacklines.Location = new System.Drawing.Point(120, 28);
			this.checkboxDrawAttacklines.Name = "checkboxDrawAttacklines";
			this.checkboxDrawAttacklines.Size = new System.Drawing.Size(81, 17);
			this.checkboxDrawAttacklines.TabIndex = 7;
			this.checkboxDrawAttacklines.Text = "Attack lines";
			this.checkboxDrawAttacklines.UseVisualStyleBackColor = true;
			this.checkboxDrawAttacklines.CheckedChanged += new System.EventHandler(this.checkboxDrawAttacklines_CheckedChanged);
			// 
			// checkBoxPlanetStatitics
			// 
			this.checkBoxPlanetStatitics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBoxPlanetStatitics.AutoSize = true;
			this.checkBoxPlanetStatitics.Checked = true;
			this.checkBoxPlanetStatitics.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxPlanetStatitics.Location = new System.Drawing.Point(8, 28);
			this.checkBoxPlanetStatitics.Name = "checkBoxPlanetStatitics";
			this.checkBoxPlanetStatitics.Size = new System.Drawing.Size(96, 17);
			this.checkBoxPlanetStatitics.TabIndex = 7;
			this.checkBoxPlanetStatitics.Text = "Planet statisics";
			this.checkBoxPlanetStatitics.UseVisualStyleBackColor = true;
			this.checkBoxPlanetStatitics.CheckedChanged += new System.EventHandler(this.checkBoxPlanetStatitics_CheckedChanged);
			// 
			// checkBoxDrawUniverseStatistics
			// 
			this.checkBoxDrawUniverseStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBoxDrawUniverseStatistics.AutoSize = true;
			this.checkBoxDrawUniverseStatistics.Checked = true;
			this.checkBoxDrawUniverseStatistics.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxDrawUniverseStatistics.Location = new System.Drawing.Point(8, 48);
			this.checkBoxDrawUniverseStatistics.Name = "checkBoxDrawUniverseStatistics";
			this.checkBoxDrawUniverseStatistics.Size = new System.Drawing.Size(111, 17);
			this.checkBoxDrawUniverseStatistics.TabIndex = 7;
			this.checkBoxDrawUniverseStatistics.Text = "Universe statistics";
			this.checkBoxDrawUniverseStatistics.UseVisualStyleBackColor = true;
			this.checkBoxDrawUniverseStatistics.CheckedChanged += new System.EventHandler(this.checkBoxDrawUniverseStatistics_CheckedChanged);
			// 
			// checkBoxDrawFleetArrival
			// 
			this.checkBoxDrawFleetArrival.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBoxDrawFleetArrival.AutoSize = true;
			this.checkBoxDrawFleetArrival.Checked = true;
			this.checkBoxDrawFleetArrival.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxDrawFleetArrival.Location = new System.Drawing.Point(120, 48);
			this.checkBoxDrawFleetArrival.Name = "checkBoxDrawFleetArrival";
			this.checkBoxDrawFleetArrival.Size = new System.Drawing.Size(80, 17);
			this.checkBoxDrawFleetArrival.TabIndex = 7;
			this.checkBoxDrawFleetArrival.Text = "Fleet arrival";
			this.checkBoxDrawFleetArrival.UseVisualStyleBackColor = true;
			this.checkBoxDrawFleetArrival.CheckedChanged += new System.EventHandler(this.checkBoxDrawFleetArrival_CheckedChanged);
			// 
			// checkBoxGridLines
			// 
			this.checkBoxGridLines.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBoxGridLines.AutoSize = true;
			this.checkBoxGridLines.Checked = true;
			this.checkBoxGridLines.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxGridLines.Location = new System.Drawing.Point(9, 7);
			this.checkBoxGridLines.Name = "checkBoxGridLines";
			this.checkBoxGridLines.Size = new System.Drawing.Size(69, 17);
			this.checkBoxGridLines.TabIndex = 7;
			this.checkBoxGridLines.Text = "Grid lines";
			this.checkBoxGridLines.UseVisualStyleBackColor = true;
			this.checkBoxGridLines.CheckedChanged += new System.EventHandler(this.checkBoxGridLines_CheckedChanged);
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.Color.MediumSlateBlue;
			this.label4.Location = new System.Drawing.Point(481, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(197, 13);
			this.label4.TabIndex = 13;
			this.label4.Text = "Max Turns";
			// 
			// textBoxTurns
			// 
			this.textBoxTurns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxTurns.Location = new System.Drawing.Point(684, 4);
			this.textBoxTurns.Name = "textBoxTurns";
			this.textBoxTurns.Size = new System.Drawing.Size(93, 20);
			this.textBoxTurns.TabIndex = 5;
			this.textBoxTurns.Text = "200";
			// 
			// btnPrevious
			// 
			this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnPrevious.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.btnPrevious.Location = new System.Drawing.Point(360, 3);
			this.btnPrevious.Name = "btnPrevious";
			this.btnPrevious.Size = new System.Drawing.Size(115, 23);
			this.btnPrevious.TabIndex = 10;
			this.btnPrevious.Text = "Previous";
			this.btnPrevious.UseVisualStyleBackColor = false;
			this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
			// 
			// button12
			// 
			this.button12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.button12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
			this.button12.Location = new System.Drawing.Point(360, 92);
			this.button12.Name = "button12";
			this.tableLayoutPanel3.SetRowSpan(this.button12, 2);
			this.button12.Size = new System.Drawing.Size(115, 46);
			this.button12.TabIndex = 10;
			this.button12.Text = "Pause";
			this.button12.UseVisualStyleBackColor = false;
			this.button12.Click += new System.EventHandler(this.button12_Click);
			// 
			// buttonPlay
			// 
			this.buttonPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.buttonPlay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.buttonPlay.Location = new System.Drawing.Point(481, 92);
			this.buttonPlay.Name = "buttonPlay";
			this.tableLayoutPanel3.SetRowSpan(this.buttonPlay, 2);
			this.buttonPlay.Size = new System.Drawing.Size(197, 46);
			this.buttonPlay.TabIndex = 0;
			this.buttonPlay.Text = "Play";
			this.buttonPlay.UseVisualStyleBackColor = false;
			this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
			// 
			// buttonStop
			// 
			this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.buttonStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.buttonStop.Location = new System.Drawing.Point(684, 92);
			this.buttonStop.Name = "buttonStop";
			this.tableLayoutPanel3.SetRowSpan(this.buttonStop, 2);
			this.buttonStop.Size = new System.Drawing.Size(93, 46);
			this.buttonStop.TabIndex = 5;
			this.buttonStop.Text = "Stop";
			this.buttonStop.UseVisualStyleBackColor = false;
			this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
			// 
			// buttonLaunchInReference
			// 
			this.buttonLaunchInReference.Location = new System.Drawing.Point(254, 3);
			this.buttonLaunchInReference.Name = "buttonLaunchInReference";
			this.buttonLaunchInReference.Size = new System.Drawing.Size(100, 23);
			this.buttonLaunchInReference.TabIndex = 14;
			this.buttonLaunchInReference.Text = "Reference";
			this.buttonLaunchInReference.UseVisualStyleBackColor = true;
			this.buttonLaunchInReference.Click += new System.EventHandler(this.buttonLaunchInReference_Click);
			// 
			// labelTurn
			// 
			this.labelTurn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTurn.AutoSize = true;
			this.labelTurn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTurn.ForeColor = System.Drawing.Color.Purple;
			this.labelTurn.Location = new System.Drawing.Point(684, 67);
			this.labelTurn.Name = "labelTurn";
			this.labelTurn.Size = new System.Drawing.Size(93, 13);
			this.labelTurn.TabIndex = 11;
			this.labelTurn.Text = "Turn";
			// 
			// textBoxBreakPoint
			// 
			this.textBoxBreakPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxBreakPoint.Location = new System.Drawing.Point(684, 34);
			this.textBoxBreakPoint.Name = "textBoxBreakPoint";
			this.textBoxBreakPoint.Size = new System.Drawing.Size(93, 20);
			this.textBoxBreakPoint.TabIndex = 5;
			this.textBoxBreakPoint.Text = "1";
			this.textBoxBreakPoint.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxBreakPoint_KeyUp);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.Color.MediumSlateBlue;
			this.label3.Location = new System.Drawing.Point(481, 67);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(197, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Current turn";
			// 
			// checkBoxBreakpoint
			// 
			this.checkBoxBreakpoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBoxBreakpoint.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBoxBreakpoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.checkBoxBreakpoint.ForeColor = System.Drawing.Color.MediumSlateBlue;
			this.checkBoxBreakpoint.Location = new System.Drawing.Point(478, 30);
			this.checkBoxBreakpoint.Margin = new System.Windows.Forms.Padding(0);
			this.checkBoxBreakpoint.Name = "checkBoxBreakpoint";
			this.checkBoxBreakpoint.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
			this.checkBoxBreakpoint.Size = new System.Drawing.Size(203, 28);
			this.checkBoxBreakpoint.TabIndex = 13;
			this.checkBoxBreakpoint.Text = "Breakpoint";
			this.checkBoxBreakpoint.UseVisualStyleBackColor = true;
			// 
			// buttonNextMove
			// 
			this.buttonNextMove.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.buttonNextMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.buttonNextMove.Location = new System.Drawing.Point(360, 62);
			this.buttonNextMove.Name = "buttonNextMove";
			this.buttonNextMove.Size = new System.Drawing.Size(115, 24);
			this.buttonNextMove.TabIndex = 10;
			this.buttonNextMove.Text = "Next";
			this.buttonNextMove.UseVisualStyleBackColor = false;
			this.buttonNextMove.Click += new System.EventHandler(this.buttonNextMove_Click);
			// 
			// labelBreakpoint
			// 
			this.labelBreakpoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.labelBreakpoint.AutoSize = true;
			this.labelBreakpoint.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBreakpoint.ForeColor = System.Drawing.Color.Maroon;
			this.labelBreakpoint.Location = new System.Drawing.Point(254, 37);
			this.labelBreakpoint.Name = "labelBreakpoint";
			this.labelBreakpoint.Size = new System.Drawing.Size(100, 14);
			this.labelBreakpoint.TabIndex = 10;
			this.labelBreakpoint.Text = "Breakpoint hit";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.button1.ForeColor = System.Drawing.Color.DarkSlateGray;
			this.button1.Location = new System.Drawing.Point(360, 32);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(115, 24);
			this.button1.TabIndex = 10;
			this.button1.Text = "Fast forward";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.LightGray;
			this.tabPage1.Controls.Add(this.labelNeutralPlanetColor);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(786, 596);
			this.tabPage1.TabIndex = 3;
			this.tabPage1.Text = "Colors";
			// 
			// labelNeutralPlanetColor
			// 
			this.labelNeutralPlanetColor.AutoSize = true;
			this.labelNeutralPlanetColor.BackColor = System.Drawing.Color.DarkSeaGreen;
			this.labelNeutralPlanetColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.labelNeutralPlanetColor.Location = new System.Drawing.Point(32, 8);
			this.labelNeutralPlanetColor.Name = "labelNeutralPlanetColor";
			this.labelNeutralPlanetColor.Size = new System.Drawing.Size(35, 13);
			this.labelNeutralPlanetColor.TabIndex = 10;
			this.labelNeutralPlanetColor.Text = "label2";
			this.labelNeutralPlanetColor.Visible = false;
			// 
			// BotDebuggerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(794, 622);
			this.Controls.Add(this.tabControlBotDebugger);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "BotDebuggerForm";
			this.Text = "Bot debugger by: Bas Mommehof";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.tabControlBotDebugger.ResumeLayout(false);
			this.tabPageBatchRun.ResumeLayout(false);
			this.tabPageBatchRun.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			this.flowLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel4.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.flowLayoutPanel6.ResumeLayout(false);
			this.tabPageDebugBot.ResumeLayout(false);
			this.tabPageDebugBot.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPagePlayers.ResumeLayout(false);
			this.groupBoxPlayer2.ResumeLayout(false);
			this.groupBoxPlayer2.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.groupBoxPlayer1.ResumeLayout(false);
			this.groupBoxPlayer1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tabPageRenderingOptions.ResumeLayout(false);
			this.tabPageRenderingOptions.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarRenderDelay)).EndInit();
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControlBotDebugger;
		private System.Windows.Forms.TabPage tabPageDebugBot;
		private System.Windows.Forms.Button buttonPlay;
		private System.Windows.Forms.GroupBox groupBoxPlayer1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.ComboBox comboBoxOpponent1;
		private System.Windows.Forms.CheckBox cbOpponentOneOwnBot;
		private System.Windows.Forms.GroupBox groupBoxPlayer2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.ComboBox comboBoxOpponent2;
		private System.Windows.Forms.CheckBox cbOpponentTwoOwnBot;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxTurns;
		private Renderer panelRender;
		private System.Windows.Forms.CheckBox cbRender;
		private System.Windows.Forms.TrackBar trackBarRenderDelay;
		private System.Windows.Forms.Label labelTurn;
		private System.Windows.Forms.Button buttonStop;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.CheckBox checkBoxBreakpoint;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxBreakPoint;
		private System.Windows.Forms.Label labelBreakpoint;
		private System.Windows.Forms.Button button12;
		private System.Windows.Forms.Button buttonNextMove;
		private System.Windows.Forms.CheckBox checkBoxParallel;
		private System.Windows.Forms.Button btnPrevious;
		private System.Windows.Forms.TabPage tabPageBatchRun;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button buttonStart;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.Button buttonLoad;
		private System.Windows.Forms.Button buttonSave;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox comboBoxRunID;
		private System.Windows.Forms.Button buttonRunSelected;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
		private System.Windows.Forms.Button buttonShowLostGames;
		private System.Windows.Forms.Button buttonResetFilter;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.Button buttonDebugMatch;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
		private System.Windows.Forms.Button buttonLaunchInReference;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
		private System.Windows.Forms.Button buttonTied;
		private System.Windows.Forms.Button buttonWon;
		private System.Windows.Forms.Button buttonTiedOrLost;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textBoxMaxBatchTurns;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox checkBoxGridLines;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Label labelNeutralPlanetColor;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPagePlayers;
		private System.Windows.Forms.TabPage tabPageRenderingOptions;
		private System.Windows.Forms.CheckBox checkboxDrawAttacklines;
		private System.Windows.Forms.CheckBox checkBoxPlanetStatitics;
		private System.Windows.Forms.CheckBox checkBoxDrawUniverseStatistics;
		private System.Windows.Forms.CheckBox checkBoxDrawFleetArrival;
		private System.Windows.Forms.Button buttonDebugMove;
		private System.Windows.Forms.Button buttonClearRun;
		private System.Windows.Forms.Label labelProgress;
		private System.Windows.Forms.CheckBox checkBoxDrawPlayerOne;
		private System.Windows.Forms.CheckBox checkBoxDrawPlayerTwo;
	}
}

