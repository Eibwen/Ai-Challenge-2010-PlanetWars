using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlanetDebug;
using System.Windows.Forms;

namespace BatchRunner
{
	internal class SilentGameEngine
	{
		public bool IsRunning { get; private set; }
		public bool Aborted { get; private set; }
		public void Abort()
		{
			Aborted = true;
		}

		public botDebugBase Player1 { get; set; }
		public botDebugBase Player2 { get; set; }
		public int PlayerWinnerId { get; private set; }
		public String MapName { get; set; }
		public int MaxTurns { get; set; }
		public int TurnsPlayed { get; private set; }

		Game engine = null;
		public bool Start()
		{
			if (IsRunning)
			{
				return false;
			}
			else
			{
				System.Diagnostics.Debug.Assert(Player1 != null, "Player 1 should be assigned");
				System.Diagnostics.Debug.Assert(Player2 != null, "Player 2 should be assigned");
				System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(MapName), "MapName should be assigned");

				if (MaxTurns <= 0)
				{
					MaxTurns = 200;
				}
				Aborted = false;
				IsRunning = true;

				TurnsPlayed = 0;
				PlayerWinnerId = 0;
				try
				{
					engine = new Game(MapName, MaxTurns, 0, null);
					engine.Init();
					List<botDebugBase> players = new List<botDebugBase>();
					players.Add(Player1);
					players.Add(Player2);
					ParallelQuery<botDebugBase> parallel = players.AsParallel();
					int turnsLeft = MaxTurns;

					// I looked at the original engine, which had a lot of overhead because it was working with application instances
					// and was capturing the output. Since I have a direct instance to the bot (wall, the debug wrapper) I can skip a lot of that
					// and just use the bare essentials.
					while (turnsLeft > 0 && (engine.Winner()) < 0)
					{
						if (!SilentGameEngine.CheckForAlliveOrDropPlayer(engine, players[0]))
							PlayerWinnerId = 1;
						if (!SilentGameEngine.CheckForAlliveOrDropPlayer(engine, players[1]))
							PlayerWinnerId = 2;
						if (PlayerWinnerId > 0) break;


						players.ForEach(player => player.GameBoardData = engine.PovRepresentation(player.Id));

						parallel.ForAll(PushGameDataToPlayer);

						turnsLeft--;
						if (Aborted)
						{
							return false;
						}
						else
						{
							TurnsPlayed++;
							engine.FlushGamePlaybackString();
							engine.DoTimeStep();

							if (!SilentGameEngine.CheckForAlliveOrDropPlayer(engine, players[0]))
								PlayerWinnerId = 1;
							if (!SilentGameEngine.CheckForAlliveOrDropPlayer(engine, players[1]))
								PlayerWinnerId = 2;
							if (PlayerWinnerId > 0) 
								break;
						}
					}
				}
				finally
				{
					IsRunning = false;
				}

				return true;
			}
		}

		public static bool CheckForAlliveOrDropPlayer(Game engine, botDebugBase player)
		{
			if (engine.IsAlive(player.Id))
				return true;
			else
			{
				engine.DropPlayer(player.Id);
				return false;
			}
		}

		static char[] newline = new char[] { '\n' };
		private void PushGameDataToPlayer(botDebugBase player)
		{
			try
			{
				player.CreateGameBoardInstance();
				player.DoMove();
				//Windows & Unix compatability.
				string allMoves = player.OutputBuffer.Replace("\r\n", "\n");

				//submit moves.
				foreach (string move in allMoves.Split(newline, StringSplitOptions.RemoveEmptyEntries))
				{
					engine.IssueOrder(player.Id, move);
				}
			}
			catch
			{
			}
			finally
			{
				player.Clear();
			}
		}
	}
}
