using System;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using PlanetDebug;
using botDebug;
using System.Data;
using System.Threading;

namespace BatchRunner
{
	/// <summary>
	/// FUCKING SHORT DISCLAIMER: Use this software any way you like.
	/// IF YOU USE IT FOR GOOD  : Give me credits, kuddos or cash.
	/// IF YOU USE IT FOR BAD   : I don't know you.
	/// .
	/// WARNING: No guarantees given.
	/// WARNING: May contain hard file or path locations.
	/// WARNING: May not run with your version of C#.
	/// WARNING: May not always make sense.
	/// .
	/// MEGAWARNING: Game results do not match original 100%, still tying to find out why.
	/// </summary>
	public partial class BotDebuggerForm : Form
	{
		/// <summary>
		/// Expected solution path, contains maps.
		/// This paths is also used as the base to launch the original engine on tab Two and Three.
		/// </summary>
        private static string solutionBase = @"D:\Projects\CSharp\!Personal\GoogleAIChallenge2010\Fanny_Heirdoo Suite V2\Run";

		public string MapFolder
		{
			get
			{
				return Path.Combine(solutionBase, "maps");
			}
		}

		/// <summary>
		/// When rendering the game ourself, hit a brake point on this turn number.
		/// </summary>
		Game _CurrentGameEngine;
		int _NumberOfTurnsPlayed;
		bool _ReleaseBreakpoint;
		bool _ForceRender;
		bool _Aborted;
		bool _Restart;

		protected int _HitBreakPointOnTurnNumber;
		static char[] _NewLineSplitter = new char[] { '\n' };
		ColorDictionary _RenderingColors = new ColorDictionary();
		DataTable _MatchesDataBase;
		DataView _CurrentViewOfMatchesDataBase;

		public BotDebuggerForm()
		{
			InitializeComponent();
			//Change the player rendering colors here.
			InitializeColors();
			comboBoxOpponent1.SelectedIndex = 0;
			comboBoxOpponent2.SelectedIndex = 0;

			comboBoxRunID.SelectedIndex = 0;
			_HitBreakPointOnTurnNumber = 1;
			this.DoubleBuffered = true;
			#region Make gui behave
			cbOpponentTwoOwnBot.Checked = true;
			ToggleComboBox(groupBoxPlayer2, cbOpponentTwoOwnBot, comboBoxOpponent2);
			ToggleComboBox(groupBoxPlayer1, cbOpponentOneOwnBot, comboBoxOpponent1);
			System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			textBoxBreakPoint.TextChanged += (sender, e) =>
			{
				_HitBreakPointOnTurnNumber = GetBreakPointTurnNumber();
			};
			#endregion

			LoadOrCreateTable();
		}

		private void InitializeColors()
		{
			_RenderingColors.Clear();
			_RenderingColors.Add(RenderColor.NeutralPlayerLight, labelNeutralPlanetColor.ForeColor);
			_RenderingColors.Add(RenderColor.NeutralPlayerDark, labelNeutralPlanetColor.BackColor);
			_RenderingColors.Add(RenderColor.Player1Light, groupBoxPlayer1.ForeColor);
			_RenderingColors.Add(RenderColor.Player1Dark, groupBoxPlayer1.BackColor);
			_RenderingColors.Add(RenderColor.Player2Light, groupBoxPlayer2.ForeColor);
			_RenderingColors.Add(RenderColor.Player2Dark, groupBoxPlayer2.BackColor);
			_RenderingColors.Add(RenderColor.GridLight, Color.White);
			_RenderingColors.Add(RenderColor.GridDark, Color.Black);
			_RenderingColors.Add(RenderColor.GridLine, Color.DarkGray);

			_RenderingColors.Add(RenderColor.PlanetIdDark, Color.Black);
			_RenderingColors.Add(RenderColor.PlanetIdLight, Color.White);

			_RenderingColors.Add(RenderColor.FleetDistanceLight, Color.White);
			_RenderingColors.Add(RenderColor.FleetDistanceDark, Color.Black);

			_RenderingColors.Add(RenderColor.PlanetNumShipsLight, Color.White);
			_RenderingColors.Add(RenderColor.PlanetNumShipsDark, Color.Black);

			_RenderingColors.Add(RenderColor.PlanetGrowthLight, Color.PaleGreen);
			_RenderingColors.Add(RenderColor.PlanetGrowthDark, Color.DarkGreen);

			_RenderingColors.Add(RenderColor.PlanetAttackLight, Color.Magenta);
			_RenderingColors.Add(RenderColor.PlanetAttackDark, Color.Black);

			_RenderingColors.Add(RenderColor.PlanetDefenceLight, Color.LightBlue);
			_RenderingColors.Add(RenderColor.PlanetDefenceDark, Color.DarkBlue);

			_RenderingColors.Add(RenderColor.FleetIncomingAttackLine, Color.Red);
			_RenderingColors.Add(RenderColor.FleetOutgoingAttackLine, Color.Blue);
			_RenderingColors.Add(RenderColor.FleetDefensiveLine, Color.LawnGreen);



			_RenderingColors.Add(RenderColor.PlanetAttackNettoNegativeLight, Color.Red);
			_RenderingColors.Add(RenderColor.PlanetAttackNettoNegativeDark, Color.Black);
			_RenderingColors.Add(RenderColor.PlanetAttackNettoPositiveLight, Color.PaleGreen);
			_RenderingColors.Add(RenderColor.PlanetAttackNettoPositiveDark, Color.DarkGreen);
		}

		/// <summary>
		/// Gets the break point turn number.
		/// </summary>
		/// <returns></returns>
		private int GetBreakPointTurnNumber()
		{
			int result;
			if (!int.TryParse(textBoxBreakPoint.Text, out result))
			{
				result = -1;
			}
			return result;
		}

		#region Tab One - Render the entire game ourselve so we can debug every line of it
		private void Next()
		{
			if (_NumberOfTurnsPlayed == _HitBreakPointOnTurnNumber)
			{
				textBoxBreakPoint.Text = (_HitBreakPointOnTurnNumber + 1).ToString();
			}
			_ReleaseBreakpoint = true;
		}

		private void Previous()
		{
			textBoxBreakPoint.Text = (Math.Max(_NumberOfTurnsPlayed - 1, Math.Max(_HitBreakPointOnTurnNumber - 1, 1))).ToString();
			FastForwardToBreakPoint();
		}

		private void FastForwardToBreakPoint()
		{
			Stop();
			_Restart = true;
			FullSpeed();
		}

		private void PerformNextTurn()
		{
			_PlayAtFullSpeed = false;
			textBoxBreakPoint.Text = (_NumberOfTurnsPlayed + 1).ToString();
			checkBoxBreakpoint.Checked = true;
		}

		private void Stop()
		{
			_ReleaseBreakpoint = true;
			_Aborted = true;
		}
		private string GetSelectedMapFilename()
		{
			return Path.Combine(MapFolder, GetSelectedMapName());
		}
		private string GetSelectedMapName()
		{
			return dataGridView1.SelectedRows[0].Cells["Map"].Value.ToString();
		}
		bool _PlayAtFullSpeed = false;
		private void FullSpeed()
		{
			_PlayAtFullSpeed = true;
		}

		private void buttonDebugMove_Click(object sender, EventArgs e)
		{
            string debugMove = @"D:\Projects\CSharp\!Personal\GoogleAIChallenge2010\Fanny_Heirdoo Suite V2\Run\FANNY_MOVES\Turn145.txt";
			Start(debugMove);
		}

		private void Start(string filename)
		{
			_NumberOfTurnsPlayed = 0;
			_CurrentGameEngine = null;
			_Aborted = false;
			if (dataGridView1.SelectedRows.Count == 0)
			{
				//you forgot to chose a map, try again.
				MessageBox.Show("Please select a map on the \"Batch Run\" tavb first.");
				tabControlBotDebugger.SelectedTab = tabPageBatchRun;
				return;
			}
			InitializeColors();
			buttonPlay.Enabled = false;
			List<botDebugBase> players = new List<botDebugBase>();
			try
			{
				players.Add(GetBotInstance(cbOpponentOneOwnBot.Checked, comboBoxOpponent1.SelectedIndex, 1));
				players.Add(GetBotInstance(cbOpponentTwoOwnBot.Checked, comboBoxOpponent2.SelectedIndex, 2));

				int turnsLeft;

				if (!int.TryParse(textBoxTurns.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out turnsLeft)) { turnsLeft = 200; }


				_CurrentGameEngine = new Game(filename, turnsLeft, 0, null);
				_CurrentGameEngine.Init();

				//CalcTSM(cbOpponentOneOwnBot.Checked ? 1 : 2);
				ForceRender();
                IQueryable<botDebugBase> parallel = players.AsQueryable<botDebugBase>();
				// I looked at the original engine, which had a lot of overhead because it was working with application instances
				// and was capturing the output. Since I have a direct instance to the bot (wall, the debug wrapper) I can skip a lot of that
				// and just use the bare essentials.
				while (turnsLeft > 0 && _CurrentGameEngine.Winner() < 0)
				{
					if (!SilentGameEngine.CheckForAlliveOrDropPlayer(_CurrentGameEngine, players[0]) &&
						SilentGameEngine.CheckForAlliveOrDropPlayer(_CurrentGameEngine, players[1]))
					{
						return;
					}

					players.ForEach(player => player.GameBoardData = _CurrentGameEngine.PovRepresentation(player.Id));

					if (checkBoxBreakpoint.Checked && _HitBreakPointOnTurnNumber == _NumberOfTurnsPlayed)
					{
						_PlayAtFullSpeed = false;
						while (checkBoxBreakpoint.Checked && !_Aborted && !_ReleaseBreakpoint)
						{
							// Blink
							labelBreakpoint.Visible = DateTime.Now.Millisecond % 300 < 100;
							Application.DoEvents();
						}
						labelBreakpoint.Visible = false;
					}
					_ReleaseBreakpoint = false;

					if (checkBoxParallel.Checked)
					{
                        foreach (botDebugBase player in parallel)
                        {
                            PushGameDataToPlayer(player);
                        }
					}
					else
					{
						players.ForEach(PushGameDataToPlayer);
					}

					if (!SilentGameEngine.CheckForAlliveOrDropPlayer(_CurrentGameEngine, players[0]) &&
						SilentGameEngine.CheckForAlliveOrDropPlayer(_CurrentGameEngine, players[1]))
					{
						return;
					}

					turnsLeft--;
					if (_Aborted)
					{
						break;
					}
					else
					{
						_NumberOfTurnsPlayed++;
						_CurrentGameEngine.FlushGamePlaybackString();
						labelTurn.Text = _NumberOfTurnsPlayed.ToString();

						_CurrentGameEngine.DoTimeStep();
						if (_Aborted)
						{
							break;
						}
						#region Rendering and pausing
						if (cbRender.Checked)
						{
							panelRender.Refresh();
							Application.DoEvents();
							if (!_PlayAtFullSpeed)
							{
								int value = -1 + trackBarRenderDelay.Value;
								System.Threading.Thread.Sleep(value);
							}
						}
						else
						{
							Application.DoEvents();
						}
						#endregion
					}
				}
			}
			finally
			{
				ForceRender();
				buttonPlay.Enabled = true;
				if (_Restart)
				{
					_Restart = false;
					Start(filename);
				}
				else
				{
					_PlayAtFullSpeed = false;
				}
			}
		}

		private void CalcTSM(int myId)
		{
			botDebug.IPlanet me = null;
			List<botDebug.IPlanet> castBuilder = new List<IPlanet>();
			foreach (var planet in _CurrentGameEngine.Planets)
			{
				if (planet.Owner == myId)
				{
					me = planet;
				}
				else
				{
					if (planet.Owner == 0 && planet.GrowthRate > 0)
					{
						castBuilder.Add(planet);
					}
				}
				if (castBuilder.Count > 80)
				{
					break;
				}
			}
			castBuilder.Insert(0, me);

			//panelRender.ShortestRoute = FannyHeirdooBot.TSM.STMFinder.Start(castBuilder);
		}

		private void ForceRender()
		{
			_ForceRender = true;
			panelRender.Refresh();
			Application.DoEvents();
		}


		private void PushGameDataToPlayer(botDebugBase player)
		{
			try
			{
				player.CreateGameBoardInstance();
				player.IssueOrderCallback = _CurrentGameEngine.IssueOrder;
				player.DoMove();
				player.IssueOrderCallback = null;

				////Windows & Unix compatability.
				//string allMoves = player.OutputBuffer.Replace("\r\n", "\n");

				////submit moves.
				//foreach (string move in allMoves.Split(_NewLineSplitter, StringSplitOptions.RemoveEmptyEntries))
				//{
				//    _CurrentGameEngine.IssueOrder(player.Id, move);
				//}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Assert(ex == null, ex.ToString());
			}
			finally
			{
				player.Clear();
			}
		}
		botDebugBase CanSurviveCallbackBot;
		/// <summary>
		/// Gets the bot instance.
		/// Change this method any way you like, I choose the simples form.
		/// Just including all the required name spaces and just create an instance of the desired bot.
		/// It would be NICE to make it more configurable, where you just select an assembly and 
		/// type name and this method would create the instance. (Lot of more work too.)
		/// </summary>
		/// <param name="ownBot">if set to <c>true</c> [own bot].</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		private PlanetDebug.botDebugBase GetBotInstance(bool ownBot, int index, int playerIndex)
		{
			PlanetDebug.botDebugBase result = null;
			try
			{
				if (ownBot)
				{
					result = new Fanny_HeirdooDebugger.botDebug();
					CanSurviveCallbackBot = result;
				}
				else
				{
					switch (index)
					{
						default:
						case 0: return result = new BullyBotDebugger.botDebug();
						case 1: return result = new DualBotDebugger.botDebug();
						case 2: return result = new ProspectorBotDebugger.botDebug();
						case 3: return result = new RageBotDebugger.botDebug();
						case 4: return result = new RandomBotDebugger.botDebug();
						case 5: return result = new ReferenceBotDebugger.botDebug();
					}
				}
			}
			finally
			{
				if (result != null)
				{
					result.Id = playerIndex;
					result.Reset();
				}
			}
			return result;
		}

		#endregion

		private void button3_Click(object sender, EventArgs e)
		{
			_Aborted = true;
		}


		#region Tab one, Play all maps

		/// <summary>
		/// Makes the sure db is exists.
		/// </summary>
		/// <returns></returns>
		private bool MakeSureDbExists()
		{
			if (_MatchesDataBase == null)
			{
				MessageBox.Show("Create or load table first");
				return false;
			}
			else
			{
				return true;
			}
		}

		private SilentGameEngine CreateSilentEngine()
		{
			SilentGameEngine silentEngine = new SilentGameEngine();
			int maxTurns;
			if (int.TryParse(textBoxMaxBatchTurns.Text, out maxTurns))
			{
				silentEngine.MaxTurns = maxTurns;
			}
			return silentEngine;
		}
		private void PerformBatchRun()
		{
			if (MakeSureDbExists())
			{
				_Aborted = false;
				buttonStart.Enabled = false;
				textBoxMaxBatchTurns.Enabled = false;
				bool errors = false;
				try
				{

					string turnsColumnName;
					string winnerColumnName;
					DetermineColumNames(out turnsColumnName, out winnerColumnName);
					SilentGameEngine silentEngine = CreateSilentEngine();
					int counter = 0;
					DataRow[] selection = _MatchesDataBase.Select(turnsColumnName + " = -1");
					int total = selection.Length;
					foreach (DataRow dr in selection)
					{
						labelProgress.Text = counter++ + " / " + total;
						ExecuteSelectedRow(turnsColumnName, winnerColumnName, silentEngine, dr);

						Application.DoEvents();

						if (_Aborted)
						{
							break;
						}
					}
				}
				catch
				{
					errors = true;
					if (MessageBox.Show("Error occurred, save anyway?", "Whoops", MessageBoxButtons.YesNo) == DialogResult.Yes)
					{
						SaveTestDb();
					}
				}
				finally
				{
					if (!errors)
					{
						if (MessageBox.Show("Save results?", "Whoops", MessageBoxButtons.YesNo) == DialogResult.Yes)
						{
							SaveTestDb();
						}
					}
					buttonStart.Enabled = true;
					textBoxMaxBatchTurns.Enabled = true;
				}
			}
		}

		private void CreateTests()
		{
			_MatchesDataBase = new DataTable("TestDb");
			_MatchesDataBase.Columns.Add("id");
			_MatchesDataBase.Columns.Add("Map");
			_MatchesDataBase.Columns.Add("CounterMatch", typeof(bool));
			_MatchesDataBase.Columns.Add("Player1");
			_MatchesDataBase.Columns.Add("Player2");

			_MatchesDataBase.Columns.Add("Turns_1", typeof(int));
			_MatchesDataBase.Columns.Add("WinnerId_1", typeof(int));

			_MatchesDataBase.Columns.Add("Turns_2", typeof(int));
			_MatchesDataBase.Columns.Add("WinnerId_2", typeof(int));

			_MatchesDataBase.Columns.Add("Turns_3", typeof(int));
			_MatchesDataBase.Columns.Add("WinnerId_3", typeof(int));


			int[] opponents = new int[] { 0, 1, 2, 3, 4 };
			int recordId = 0;
			foreach (string file in Directory.GetFiles(MapFolder))
			{
				List<object> row = new List<object>();
				List<object> counterRow = new List<object>();
				string mapname = Path.GetFileName(file);
				foreach (int opponent in opponents)
				{
					row.Clear(); counterRow.Clear();
					row.Add(recordId++); counterRow.Add(recordId++);
					row.Add(mapname); counterRow.Add(mapname);
					row.Add(true); counterRow.Add(false);
					row.Add(opponent); counterRow.Add("Fanny");
					row.Add("Fanny"); counterRow.Add(opponent);

					row.Add(-1); counterRow.Add(-1);
					row.Add(0); counterRow.Add(0);

					row.Add(-1); counterRow.Add(-1);
					row.Add(0); counterRow.Add(0);

					row.Add(-1); counterRow.Add(-1);
					row.Add(0); counterRow.Add(0);
					_MatchesDataBase.Rows.Add(row.ToArray());
					_MatchesDataBase.Rows.Add(counterRow.ToArray());
					if (_Aborted) break;

				}
			}
			SetDataSource(_MatchesDataBase);

		}
		private void StartDebugging()
		{
			if (MakeSureDbExists())
			{
				if (dataGridView1.SelectedRows.Count > 0)
				{
					DataRowView row = (DataRowView)dataGridView1.SelectedRows[0].DataBoundItem;

					bool isCounterMatch = (bool)row["CounterMatch"];
					cbOpponentOneOwnBot.Checked = !isCounterMatch;
					cbOpponentTwoOwnBot.Checked = isCounterMatch;
					if (isCounterMatch)
					{
						comboBoxOpponent1.SelectedIndex = int.Parse(row["Player1"].ToString());
					}
					else
					{
						comboBoxOpponent2.SelectedIndex = int.Parse(row["Player2"].ToString());
					}

					tabControlBotDebugger.SelectedTab = tabPageDebugBot;
					Start(GetSelectedMapFilename());
				}
			}
		}

		private void PerformRunOnSelectedRecord()
		{
			if (MakeSureDbExists())
			{
				if (dataGridView1.SelectedRows.Count > 0)
				{
					string turnsColumnName;
					string winnerColumnName;
					DetermineColumNames(out turnsColumnName, out winnerColumnName);
					SilentGameEngine silentEngine = CreateSilentEngine();
					DataRowView dr = (DataRowView)dataGridView1.SelectedRows[0].DataBoundItem;
					ExecuteSelectedRow(turnsColumnName, winnerColumnName, silentEngine, dr.Row);
				}
			}
		}
		private void buttonClearRun_Click(object sender, EventArgs e)
		{
			if (MakeSureDbExists())
			{
				string turnsColumnName;
				string winnerColumnName;
				DetermineColumNames(out turnsColumnName, out winnerColumnName);
				foreach (DataRow row in _MatchesDataBase.Rows)
				{
					row[winnerColumnName] = -1;
					row[turnsColumnName] = -1;
				}
			}
		}
		private void DetermineColumNames(out string turnsColumnName, out string winnerColumnName)
		{
			int runId = comboBoxRunID.SelectedIndex + 1;
			turnsColumnName = "Turns_" + runId;
			winnerColumnName = "WinnerId_" + runId;
		}

		private void ExecuteSelectedRow(string turnsColumnName, string winnerColumnName, SilentGameEngine silentEngine, DataRow dr)
		{
			bool isCounterMatch = (bool)dr["CounterMatch"];
			int botIndex;
			if (isCounterMatch)
			{
				botIndex = int.Parse(dr["Player1"].ToString());
				silentEngine.Player1 = GetBotInstance(false, botIndex, 1);
				silentEngine.Player2 = GetBotInstance(true, botIndex, 2);
			}
			else
			{
				botIndex = int.Parse(dr["Player2"].ToString());
				silentEngine.Player1 = GetBotInstance(true, botIndex, 1);
				silentEngine.Player2 = GetBotInstance(false, botIndex, 2);
			}

			silentEngine.MapName = Path.Combine(MapFolder, (string)dr["Map"]);

			if (silentEngine.Start())
			{
				switch (silentEngine.PlayerWinnerId)
				{
					case 0: dr[winnerColumnName] = 0;
						break;
					default:
						if (isCounterMatch)
						{
							dr[winnerColumnName] = silentEngine.PlayerWinnerId;
						}
						else
						{
							dr[winnerColumnName] = 3 - silentEngine.PlayerWinnerId;
						}
						break;
				}

				dr[turnsColumnName] = silentEngine.TurnsPlayed;
			}
		}

		private void LoadTestDb()
		{
			if (File.Exists(TestDbFileName))
			{
				try
				{
					SetDataSource(SerializationHelper.Deserialize<DataTable>(File.ReadAllText(TestDbFileName), typeof(DataTable)));
				}
				catch
				{
					_MatchesDataBase = null;
				}
			}
		}

		private void SetDataSource(DataTable newSource)
		{
			_MatchesDataBase = newSource;
			_CurrentViewOfMatchesDataBase = newSource.DefaultView;
			dataGridView1.DataSource = _CurrentViewOfMatchesDataBase;
		}
		private string TestDbFileName
		{
			get
			{
				return Path.Combine(MapFolder, "TestDb\\TestDb.dta"); ;
			}
		}
		private void SaveTestDb()
		{
			if (_MatchesDataBase != null)
			{
				File.WriteAllText(TestDbFileName, SerializationHelper.Serialize(_MatchesDataBase));
			}
		}

		private void LoadOrCreateTable()
		{
			if (File.Exists(TestDbFileName))
			{
				LoadTestDb();
			}
			else
			{
				CreateTests();
			}
		}


		private void StartMatch()
		{
			string argumentsTemplate = "-jar tools\\PlayGame.jar maps\\[MAPNAME] 1000000 1000000 log.txt \"[OPPONENT1].exe\"  \"[OPPONENT2].exe\"";
			ProcessStartInfo info = new ProcessStartInfo();

			argumentsTemplate += " | java.exe -jar tools\\ShowGame.jar";
			info.FileName = Path.Combine(solutionBase, "ShellExecuter.cmd");
			info.CreateNoWindow = false;
			info.RedirectStandardOutput = false;
			info.RedirectStandardError = false;
			info.UseShellExecute = false;
			info.ErrorDialog = true;

			info.WorkingDirectory = solutionBase;

			string actualArguments = argumentsTemplate.Replace("[MAPNAME]", GetSelectedMapName());
			if (cbOpponentOneOwnBot.Checked)
			{
				actualArguments = actualArguments.Replace("[OPPONENT1]", "FANNY_HEIRDOO");
			}
			else
			{
				actualArguments = actualArguments.Replace("[OPPONENT1]", comboBoxOpponent1.Text + "bot");
			}

			if (cbOpponentTwoOwnBot.Checked)
			{
				actualArguments = actualArguments.Replace("[OPPONENT2]", "FANNY_HEIRDOO");
			}
			else
			{
				actualArguments = actualArguments.Replace("[OPPONENT2]", comboBoxOpponent2.Text + "bot");
			}


			try
			{
				File.WriteAllText(info.FileName, "java.exe " + actualArguments);

				using (Process proc = Process.Start(info))
				{
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
		#endregion

		#region Event handlers GAME-ENGINE

		/// <summary>
		/// RENDERING !!!
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void panelRender_Paint(object sender, PaintEventArgs e)
		{
			if (_CurrentGameEngine != null)
			{
				if (cbRender.Checked || _ForceRender)
				{
					_ForceRender = false;
					if (CanSurviveCallbackBot == null)
					{
						panelRender.Render(_CurrentGameEngine, null, _RenderingColors, e.Graphics, null);
					}
					else
					{
						panelRender.Render(_CurrentGameEngine, null, _RenderingColors, e.Graphics, CanSurviveCallbackBot);
					}
				}
			}
		}

		private void cbOpponentTwoOwnBot_CheckedChanged(object sender, EventArgs e)
		{
			ToggleComboBox(groupBoxPlayer2, cbOpponentTwoOwnBot, comboBoxOpponent2);
		}

		private void ToggleComboBox(Control parent, CheckBox ownBot, ComboBox otherBot)
		{
			if (ownBot.Checked)
			{
				otherBot.Visible = false;
				parent.Font = labelBreakpoint.Font;
			}
			else
			{
				otherBot.Visible = true;
				parent.Font = checkBoxParallel.Font;
			}
		}

		private void cbOpponentOneOwnBot_CheckedChanged(object sender, EventArgs e)
		{
			ToggleComboBox(groupBoxPlayer1, cbOpponentOneOwnBot, comboBoxOpponent1);
		}


		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			Stop();
		}
		private void buttonStop_Click(object sender, EventArgs e)
		{
			Stop();
		}

		private void buttonPlay_Click(object sender, EventArgs e)
		{
			Start(GetSelectedMapFilename());
		}
		private void buttonNextMove_Click(object sender, EventArgs e)
		{
			Next();
		}

		private void btnPrevious_Click(object sender, EventArgs e)
		{
			Previous();
		}

		private void buttonSave_Click(object sender, EventArgs e)
		{
			SaveTestDb();
		}
		private void buttonLoad_Click(object sender, EventArgs e)
		{
			LoadTestDb();
		}


		private void btnCreate_Click(object sender, EventArgs e)
		{
			CreateTests();
		}
		private void buttonRunSelected_Click(object sender, EventArgs e)
		{
			PerformRunOnSelectedRecord();
		}
		private void button2_Click(object sender, EventArgs e)
		{
			PerformBatchRun();
		}
		private void buttonResetFilter_Click(object sender, EventArgs e)
		{
			if (MakeSureDbExists())
			{
				_CurrentViewOfMatchesDataBase.RowFilter = "";
			}
		}


		private void buttonTiedOrLost_Click(object sender, EventArgs e)
		{
			if (MakeSureDbExists())
			{
				_CurrentViewOfMatchesDataBase.RowFilter =
@"(WinnerId_1 <> 1 and Turns_1 <> -1) or 
(WinnerId_2 <> 1 and Turns_2 <> -1) or
(WinnerId_3 <> 1 and Turns_3 <> -1)";
			}
		}
		private void buttonShowLostGames_Click(object sender, EventArgs e)
		{
			if (MakeSureDbExists())
			{
				_CurrentViewOfMatchesDataBase.RowFilter =
@"(WinnerId_1 = 2 and Turns_1 <> -1) or 
(WinnerId_2 = 2 and Turns_2 <> -1) or
(WinnerId_3 = 2 and Turns_3 <> -1)";
			}
		}
		private void buttonWon_Click(object sender, EventArgs e)
		{
			if (MakeSureDbExists())
			{
				_CurrentViewOfMatchesDataBase.RowFilter =
@"(WinnerId_1 = 1 and Turns_1 <> -1) or 
(WinnerId_2 = 1 and Turns_2 <> -1) or
(WinnerId_3 = 1 and Turns_3 <> -1)";
			}
		}

		private void buttonTied_Click(object sender, EventArgs e)
		{
			if (MakeSureDbExists())
			{
				_CurrentViewOfMatchesDataBase.RowFilter =
@"(WinnerId_1 = 0  and Turns_1 <> -1) or 
(WinnerId_2 =0 and Turns_2 <> -1) or
(WinnerId_3 = 0 and Turns_3 <> -1)";
			}
		}
		private void button12_Click(object sender, EventArgs e)
		{
			PerformNextTurn();
		}

		private void buttonDebugMatch_Click(object sender, EventArgs e)
		{
			StartDebugging();
		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			StartDebugging();
		}

		private void buttonLaunchInReference_Click(object sender, EventArgs e)
		{
			StartMatch();
		}
		private void button1_Click(object sender, EventArgs e)
		{
			FullSpeed();
		}


		private void checkBoxGridLines_CheckedChanged(object sender, EventArgs e)
		{
			panelRender.DrawGrid = checkBoxGridLines.Checked;
			panelRender.Refresh();
		}

		private void checkboxDrawAttacklines_CheckedChanged(object sender, EventArgs e)
		{
			panelRender.DrawAttacklines = checkboxDrawAttacklines.Checked;
			panelRender.Refresh();
		}

		private void checkBoxPlanetStatitics_CheckedChanged(object sender, EventArgs e)
		{
			panelRender.DrawPlanetStatistics = checkBoxPlanetStatitics.Checked;
			panelRender.Refresh();
		}

		private void checkBoxDrawFleetArrival_CheckedChanged(object sender, EventArgs e)
		{
			panelRender.DrawFleetArrival = checkBoxDrawFleetArrival.Checked;
			panelRender.Refresh();
		}

		private void checkBoxDrawUniverseStatistics_CheckedChanged(object sender, EventArgs e)
		{
			panelRender.DrawUniverseStatistics = checkBoxDrawUniverseStatistics.Checked;
			panelRender.Refresh();
		}


		private void checkBoxDrawPlayerTwo_CheckedChanged(object sender, EventArgs e)
		{
			panelRender.DrawPlayerTwo = checkBoxDrawPlayerTwo.Checked;
			panelRender.Refresh();
		}

		private void checkBoxDrawPlayerOne_CheckedChanged(object sender, EventArgs e)
		{
			panelRender.DrawPlayerOne = checkBoxDrawPlayerOne.Checked;
			panelRender.Refresh();
		}
		#endregion

		private void textBoxBreakPoint_KeyUp(object sender, KeyEventArgs e)
		{
			if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Return))
			{
				checkBoxBreakpoint.Checked = true;
				if (buttonPlay.Enabled)
				{
					_PlayAtFullSpeed = true;
					Start(GetSelectedMapFilename());
				}
				else
				{
					FastForwardToBreakPoint();
				}
			}
		}
	}
}
