
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace BatchRunner
{
	public class Game
	{

		DoubleMapper Double = new DoubleMapper(); 
		IntegerMapper Integer = new IntegerMapper();

		// There are two modes:
		//   * If mode == 0, then s is interpreted as a filename, and the game is
		//     initialized by reading map data out of the named file.
		//   * If mode == 1, then s is interpreted as a string that contains map
		//     data directly. The string is parsed in the same way that the
		//     contents of a map file would be.
		// This constructor does not actually initialize the game object. You must
		// always call Init() before the game object will be in any kind of
		// coherent state.
		public Game(String mapName, int maxGameLength, int mode, String logFilename)
		{
			this.logFilename = logFilename;
			Planets = new List<Planet>();
			Fleets = new List<Fleet>();
			gamePlayback = new StringBuilder();
			initMode = mode;
			switch (initMode)
			{
				case 0:
					mapFilename = mapName;
					break;
				case 1:
					mapData = mapName;
					break;
				default:
					break;
			}
			this.maxGameLength = maxGameLength;
			numTurns = 0;
		}

		// Initializes a game of Planet Wars. Loads the map data from the file
		// specified in the constructor. Returns 1 on success, 0 on failure.
		public int Init()
		{
			// Delete the contents of the log file.
			if (logFilename != null)
			{
				try
				{
					//PORT: Simplified
					File.WriteAllText(logFilename, "initializing");
				}
				catch 
				{
					// do nothing.
				}
			}

			switch (initMode)
			{
				case 0:
					return LoadMapFromFile(mapFilename);
				case 1:
					return ParseGameState(mapData);
				default:
					return 0;
			}
		}

		public void WriteLogMessage(String message)
		{
			if (logFilename == null)
			{
				return;
			}
			try
			{
				//PORT: Simplified
				File.AppendAllText(logFilename, message + Environment.NewLine);
			}
			catch (Exception e)
			{
				// whatev
			}
		}

		// Returns the number of planets. Planets are numbered starting with 0.
		public int NumPlanets()
		{
			return Planets.Count;
		}

		// Returns the planet with the given planet_id. There are NumPlanets()
		// planets. They are numbered starting at 0.
		public Planet GetPlanet(int planetID)
		{
			return Planets[planetID];
		}

		// Returns the number of fleets.
		public int NumFleets()
		{
			return Fleets.Count;
		}

		// Returns the fleet with the given fleet_id. Fleets are numbered starting
		// with 0. There are NumFleets() fleets. fleet_id's are not consistent from
		// one turn to the next.
		public Fleet GetFleet(int fleetID)
		{
			return Fleets[fleetID];
		}

		// Writes a string which represents the current game state. No point-of-
		// view switching is performed.
		public String toString()
		{
			return PovRepresentation(-1);
		}

		// Writes a string which represents the current game state. This string
		// conforms to the Point-in-Time format from the project Wiki.
		//
		// Optionally, you may specify the pov (Point of View) parameter. The pov
		// parameter is a player number. If specified, the player numbers 1 and pov
		// will be swapped in the game state output. This is used when sending the
		// game state to individual players, so that they can always assume that
		// they are player number 1.
		public String PovRepresentation(int pov)
		{
			StringBuilder s = new StringBuilder();
			foreach (Planet p in Planets)
			{
				// We can't use String.format here because in certain locales, the ,
				// and . get switched for X and Y (yet just appending them using the
				// default toString methods apparently doesn't switch them?)
				s.AppendFormat("P {0} {1} {2} {3} {4}\n", p.X, p.Y, PovSwitch(pov, p.Owner), p.NumShips, p.GrowthRate);

			}
			foreach (Fleet f in Fleets)
			{
				s.AppendFormat("F {0} {1} {2} {3} {4} {5}\n", PovSwitch(pov, f.Owner),
					f.NumShips, f.SourcePlanet, f.DestinationPlanet, f.TotalTripLength, f.TurnsRemaining);

			}
			return s.ToString();
		}

		// Carries out the point-of-view switch operation, so that each player can
		// always assume that he is player number 1. There are three cases.
		// 1. If pov < 0 then no pov switching is being used. Return player_id.
		// 2. If player_id == pov then return 1 so that each player thinks he is
		//    player number 1.
		// 3. If player_id == 1 then return pov so that the real player 1 looks
		//    like he is player number "pov".
		// 4. Otherwise return player_id, since players other than 1 and pov are
		//    unaffected by the pov switch.
		public static int PovSwitch(int pov, int playerID)
		{
			if (pov < 0) return playerID;
			if (playerID == pov) return 1;
			if (playerID == 1) return pov;
			return playerID;
		}

		// Returns the distance between two planets, rounded up to the next highest
		// integer. This is the number of discrete time steps it takes to get
		// between the two planets.
		public int Distance(int sourcePlanet, int destinationPlanet)
		{
			Planet source = Planets[sourcePlanet];
			Planet destination = Planets[destinationPlanet];
			double dx = source.X - destination.X;
			double dy = source.Y - destination.Y;
			double squared = dx * dx + dy * dy;
			double rooted = Math.Sqrt(squared);
			return (int)Math.Ceiling(rooted);
		}

		//Resolves the battle at planet p, if there is one.
		//* Removes all fleets involved in the battle
		//* Sets the number of ships and owner of the planet according the outcome
		private void FightBattle(Planet p)
		{
			Map<int, int> participants = new TreeMap<int, int>();
			participants.put(p.Owner, p.NumShips);
			bool doBattle = false;
			//PORT: Deleting items from iterators is not allowed.
			//      converted to deletable for loop
			for (int fleetIndex = 0; fleetIndex < Fleets.Count; )
			{
				Fleet fl = Fleets[fleetIndex];
				if (fl.TurnsRemaining <= 0 && fl.DestinationPlanet == p.PlanetID)
				{
					int attackForce;
					doBattle = true;
					if (participants.TryGetValue(fl.Owner, out attackForce))
					{
						participants[fl.Owner] = attackForce + fl.NumShips;
					}
					else
					{
						participants.Add(fl.Owner, fl.NumShips);
					}
					Fleets.Remove(fl);
				}
				else { fleetIndex++; }
			}

			if (doBattle)
			{
				Fleet winner = new Fleet(0, 0);
				Fleet second = new Fleet(0, 0);
				foreach (var f in participants)
				{
					if (f.Value > second.NumShips)
					{
						if (f.Value > winner.NumShips)
						{
							second = winner;
							winner = new Fleet(f.Key, f.Value);
						}
						else
						{
							second = new Fleet(f.Key, f.Value);
						}
					}
				}

				if (winner.NumShips > second.NumShips)
				{
					p.SetNumShips(winner.NumShips - second.NumShips);
					p.SetOwner(winner.Owner);
				}
				else
				{
					p.SetNumShips(0);
				}
			}
		}

		// Executes one time step.
		//   * Planet bonuses are added to non-neutral planets.
		//   * Fleets are advanced towards their destinations.
		//   * Fleets that arrive at their destination are dealt with.
		public void DoTimeStep()
		{
			ClearDupeCache();
			// Add ships to each non-neutral planet according to its growth rate.
			foreach (Planet p in Planets)
			{
				if (p.Owner > 0)
				{
					p.AddShips(p.GrowthRate);
				}
			}
			// Advance all fleets by one time step.
			foreach (Fleet f in Fleets)
			{
				f.TimeStep();
			}
			// Determine the result of any battles
			foreach (Planet p in Planets)
			{
				FightBattle(p);
			}

			Boolean needcomma = false;
			foreach (Planet p in Planets)
			{
				if (needcomma)
					gamePlayback.Append(",");
				gamePlayback.Append(p.Owner);
				gamePlayback.Append(".");
				gamePlayback.Append(p.NumShips);
				needcomma = true;
			}
			foreach (Fleet f in Fleets)
			{
				if (needcomma)
					gamePlayback.Append(",");
				gamePlayback.Append(f.Owner);
				gamePlayback.Append(".");
				gamePlayback.Append(f.NumShips);
				gamePlayback.Append(".");
				gamePlayback.Append(f.SourcePlanet);
				gamePlayback.Append(".");
				gamePlayback.Append(f.DestinationPlanet);
				gamePlayback.Append(".");
				gamePlayback.Append(f.TotalTripLength);
				gamePlayback.Append(".");
				gamePlayback.Append(f.TurnsRemaining);
			}
			gamePlayback.Append(":");
			// Check to see if the maximum number of turns has been reached.
			++numTurns;
		}

		Map<string, Fleet> deDupe = new Map<string, Fleet>();
		public void ClearDupeCache()
		{
			deDupe.Clear();
		}
		// Issue an order. This function takes num_ships off the source_planet,
		// puts them into a newly-created fleet, calculates the distance to the
		// destination_planet, and sets the fleet's total trip time to that
		// distance. Checks that the given player_id is allowed to give the given
		// order. If not, the offending player is kicked from the game. If the
		// order was carried out without any issue, and everything is peachy, then
		// 0 is returned. Otherwise, -1 is returned.
		public int IssueOrder(int playerID,
							  int sourcePlanet,
							  int destinationPlanet,
							  int numShips)
		{
			Planet source = Planets[sourcePlanet];
			bool isIllegalMove = source.Owner != playerID ||
							numShips > source.NumShips ||
							numShips < 0;

			if (isIllegalMove)
			{
				string message = String.Format(
					"Illegal move player: {0}. From planet {1} owned by {2} with {3} ships, wants to send {4} ships", playerID, sourcePlanet, source.Owner, numShips, source.NumShips);
				System.Diagnostics.Debug.Assert(false, message);
				WriteLogMessage(message);
				DropPlayer(playerID);
				return -1;
			}
			source.RemoveShips(numShips);

			string fleetKey = playerID + ";" + sourcePlanet + ";" + destinationPlanet;

			Fleet duplicate;
			if (deDupe.TryGetValue(fleetKey, out duplicate))
			{
				//ha there is no add...
				duplicate.RemoveShips(-numShips);
			}
			else
			{
				int distance = Distance(sourcePlanet, destinationPlanet);
				Fleet f = new Fleet(source.Owner,
									numShips,
									sourcePlanet,
									destinationPlanet,
									distance,
									distance);

				deDupe.Add(fleetKey, f);
				Fleets.Add(f);
			}

			return 0;
		}

		public void AddFleet(Fleet f)
		{
			Fleets.Add(f);
		}

		// Behaves just like the longer form of IssueOrder, but takes a string
		// of the form "source_planet destination_planet num_ships". That is, three
		// integers separated by space characters.
		public int IssueOrder(int playerID, String order)
		{
			String[] tokens = order.Split(' ');
			if (tokens.Length != 3)
			{
				return -1;
			}
			int sourcePlanet = Integer.parseInt(tokens[0]);
			int destinationPlanet = Integer.parseInt(tokens[1]);
			int numShips = Integer.parseInt(tokens[2]);
			return IssueOrder(playerID, sourcePlanet, destinationPlanet, numShips);
		}

		// Kicks a player out of the game. This is used in cases where a player
		// tries to give an illegal order or runs over the time limit.
		public void DropPlayer(int playerID)
		{
			foreach (Planet p in Planets)
			{
				if (p.Owner == playerID)
				{
					p.SetOwner(0);
				}
			}
			foreach (Fleet f in Fleets)
			{
				if (f.Owner == playerID)
				{
					f.Kill();
				}
			}
		}

		// Returns true if the named player owns at least one planet or fleet.
		// Otherwise, the player is deemed to be dead and false is returned.
		public Boolean IsAlive(int playerID)
		{
			foreach (Planet p in Planets)
			{
				if (p.Owner == playerID)
				{
					return true;
				}
			}
			foreach (Fleet f in Fleets)
			{
				if (f.Owner == playerID)
				{
					return true;
				}
			}
			return false;
		}

		// If the game is not yet over (ie: at least two players have planets or
		// fleets remaining), returns -1. If the game is over (ie: only one player
		// is left) then that player's number is returned. If there are no
		// remaining players, then the game is a draw and 0 is returned.
		public int Winner()
		{
			List<int> remainingPlayers = new List<int>();
			foreach (Planet p in Planets)
			{
				if (p.Owner > 0)
				{
					if (!remainingPlayers.Contains(p.Owner))
					{
						remainingPlayers.Add(p.Owner);
					}
				}
			}
			foreach (Fleet f in Fleets)
			{
				if (f.Owner > 0)
				{
					if (!remainingPlayers.Contains(f.Owner))
					{
						remainingPlayers.Add(f.Owner);
					}
				}
			}
			switch (remainingPlayers.Count)
			{
				case 0:
					return 0;
				case 1:
					return remainingPlayers[0];
				default:
					return -1;
			}
		}

		// Returns the game playback string. This is a complete record of the game,
		// and can be passed to a visualization program to playback the game.
		public String GamePlaybackString()
		{
			return gamePlayback.ToString();
		}


		// Returns the playback string so far, then clears it.
		// Used for live streaming output
		public String FlushGamePlaybackString()
		{
			StringBuilder oldGamePlayback = gamePlayback;
			gamePlayback = new StringBuilder();
			return oldGamePlayback.ToString();
		}

		// Returns the number of ships that the current player has, either located
		// on planets or in flight.
		public int NumShips(int playerID)
		{
			int numShips = 0;
			foreach (Planet p in Planets)
			{
				if (p.Owner == playerID)
				{
					numShips += p.NumShips;
				}
			}
			foreach (Fleet f in Fleets)
			{
				if (f.Owner == playerID)
				{
					numShips += f.NumShips;
				}
			}
			return numShips;
		}

		// Parses a game state from a string. On success, returns 1. On failure,
		// returns 0.
		private int ParseGameState(String s)
		{
			Planets.Clear();
			Fleets.Clear();
			ClearDupeCache();
			//PORT: Make WindowsCompatible
			String[] lines = s.Replace("\r\n", "\n").Split('\n');
			int planetid = 0;
			for (int i = 0; i < lines.Length; ++i)
			{
				String line = lines[i];
				int commentBegin = line.IndexOf('#');
				if (commentBegin >= 0)
				{
					line = line.Substring(0, commentBegin);
				}
				if (line.Trim().Length == 0)
				{
					continue;
				}
				String[] tokens = line.Split(' ');
				if (tokens.Length == 0)
				{
					continue;
				}
				if (tokens[0].Equals("P"))
				{
					if (tokens.Length != 6)
					{
						return 0;
					}
					double x = Double.parseDouble(tokens[1]);
					double y = Double.parseDouble(tokens[2]);
					int owner = Integer.parseInt(tokens[3]);
					int numShips = Integer.parseInt(tokens[4]);
					int growthRate = Integer.parseInt(tokens[5]);
					Planet p = new Planet(planetid++, owner, numShips, growthRate, x, y);
					Planets.Add(p);
					if (gamePlayback.Length > 0)
					{
						gamePlayback.Append(":");
					}
					gamePlayback.Append("" + x + "," + y + "," + owner + "," + numShips + "," + growthRate);
				}
				else if (tokens[0].Equals("F"))
				{
					if (tokens.Length != 7)
					{
						return 0;
					}
					int owner = Integer.parseInt(tokens[1]);
					int numShips = Integer.parseInt(tokens[2]);
					int source = Integer.parseInt(tokens[3]);
					int destination = Integer.parseInt(tokens[4]);
					int totalTripLength = Integer.parseInt(tokens[5]);
					int turnsRemaining = Integer.parseInt(tokens[6]);
					Fleet f = new Fleet(owner,
							numShips,
							source,
							destination,
							totalTripLength,
							turnsRemaining);
					Fleets.Add(f);
				}
				else
				{
					return 0;
				}
			}
			gamePlayback.Append("|");
			return 1;
		}

		// Loads a map from a test file. The text file contains a description of
		// the starting state of a game. See the project wiki for a description of
		// the file format. It should be called the Planet Wars Point-in-Time
		// format. On success, return 1. On failure, returns 0.
		private int LoadMapFromFile(String mapFilename)
		{
			try
			{
				//PORT: Simplified
				return ParseGameState(File.ReadAllText(mapFilename));
			}
			catch (Exception ex)
			{
				WriteLogMessage(ex.ToString());
				return 0;
			}
		}

		// Store all the planets and fleets. OMG we wouldn't wanna lose all the
		// planets and fleets, would we!?
		public List<Planet> Planets { get; private set; }
		public List<Fleet> Fleets { get; private set; }

		// The filename of the map that this game is being played on.
		private String mapFilename;

		// The string of map data to parse.
		private String mapData;

		// Stores a mode identifier which determines how to initialize this object.
		// See the constructor for details.
		private int initMode;

		// This is the game playback string. It's a complete description of the
		// game. It can be read by a visualization program to visualize the game.
		private StringBuilder gamePlayback;

		// The maximum length of the game in turns. After this many turns, the game
		// will end, with whoever has the most ships as the winner. If there is no
		// player with the most ships, then the game is a draw.
		private int maxGameLength;
		private int numTurns;

		// This is the name of the file in which to write log messages.
		private String logFilename;

		private Game(Game _g)
		{
			Planets = new List<Planet>();
			foreach (Planet p in _g.Planets)
			{
				Planets.Add((Planet)(p.clone()));
			}
			Fleets = new List<Fleet>();
			foreach (Fleet f in _g.Fleets)
			{
				Fleets.Add((Fleet)(f.clone()));
			}

			//PORT: Strings are immutable
			if (_g.mapFilename != null)
				mapFilename = _g.mapFilename;

			if (_g.mapData != null)
				mapData = _g.mapData;

			initMode = _g.initMode;
			if (_g.gamePlayback != null)
				gamePlayback = new StringBuilder(_g.gamePlayback.ToString());
			maxGameLength = _g.maxGameLength;
			numTurns = _g.numTurns;
			// Dont need to init the drawing stuff (it does it itself)
		}
		public Object clone()
		{
			return new Game(this);
		}
	}
}