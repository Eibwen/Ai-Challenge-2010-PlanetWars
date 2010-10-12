// Contestants do not need to worry about anything in this file. This is just
// helper code that does the boring stuff for you, so you can focus on the
// interesting stuff. That being said, you're welcome to change anything in
// this file if you know what you're doing.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PlanetDebug; 

public class Universe
{
	/// <summary>
	/// Resets this instance.
	/// </summary>
	public static void Reset()
	{
		TurnCount = 0;
		TravelMap = null;
		WishList.Clear();
		BotEnvironment.ClearTrace();
	}
	botDebugBase Debugger;
	// Constructs a PlanetWars object instance, given a string containing a
	// description of a game state.
	public Universe(string boardLayout, botDebugBase debugger)
	{
		Debugger = debugger;
		TurnCount++;
		EarlyOrLateStageBias = Math.Max(0, 4 - (Universe.TurnCount + 1) / 10.0);
		BotEnvironment.DumpLayout(boardLayout);
		ParseGameState(boardLayout);
		if (TurnCount > 10 && Me.Planets.Count > 3)
		{
			int enemyPlanetCount = All.Planets.Count - Neutral.Planets.Count - Me.Planets.Count;
			IsDominating = (TotalFleetCount / ((TotalFleetCount - Me.TotalShipCount)+1) > 1.5) ||
				(enemyPlanetCount == 0) ||
				Me.Planets.Count / enemyPlanetCount > 1.5;
		}
		if (IsDominating)
		{
			if (TurnCount < 10)
			{
				IsDominating = false;
			}
			else
			{

			}
		}
	}


	/// <summary>
	/// Gets the turn count.
	/// </summary>
	/// <value>The turn count.</value>
	public static int TurnCount { get; private set; }
	public static double EarlyOrLateStageBias { get; private set; }
	public static bool IsDominating { get; private set; }

	/// <summary>
	/// 
	/// </summary>
	public int TotalFleetCount;
	public static int InitialEnemyFleetDistance { get; set; }
	/// <summary>
	/// If this number is very high, then capturing a planet will be difficult and requires a different approach.
	/// </summary>
	/// <value>The initial size of the universe fleet.</value>
	public static int InitialTotalFleetSize { get; private set; }
	/// <summary>
	/// Gets or sets the initial size of the universe fleet.
	/// </summary>
	/// <value>The initial size of the universe fleet.</value>
	public static double InitialFleetToPlanetRatio { get; private set; }
	public Quadrant EnemyShipFocus { get; private set; }
	public Quadrant OwnShipFocus { get; private set; }
	/// <summary>
	/// Gets or sets the difficulty.
	/// Zero to +/- 4
	/// </summary>
	/// <value>The difficulty.</value>
	public static double Difficulty { get; set; }

	public Player Me { get; private set; }
	public Player Neutral { get; private set; }
	public Player All { get; private set; }

	//only needs to be determined once.
	public static PlanetaryTravelRouteDictionary TravelMap;

	public Dictionary<int, Planet> AllPlanetsOnPlanetId { get; private set; }

	private static List<int> WishList = new List<int>();
	public static void AddToWishList(Planet onwedPlanet)
	{
		if (!WishList.Contains(onwedPlanet.PlanetID))
		{
			WishList.Add(onwedPlanet.PlanetID);
		}
	}

	// Parses a game state from a string. On success, returns 1. On failure,
	// returns 0.
	private int ParseGameState(string boardLayout)
	{
		List<Fleet> fleetbuilder = new List<Fleet>();
		List<Planet> planetBuilder = new List<Planet>();
		Dictionary<int, EditablePlayer> playerBuilder = new Dictionary<int, EditablePlayer>();
		int planetID = 0;
		string[] lines = boardLayout.Replace("\r", "").Split('\n');
		for (int i = 0; i < lines.Length; ++i)
		{
			string line = lines[i];
			int commentBegin = line.IndexOf('#');
			if (commentBegin >= 0)
			{
				line = line.Substring(0, commentBegin);
			}
			if (line.Trim().Length == 0)
			{
				continue;
			}
			string[] tokens = line.Split(' ');
			if (tokens.Length == 0)
			{
				continue;
			}
			switch (tokens[0])
			{
				default:
					break;
				case "P":
				case "p":
					#region Add planet
					{
						if (tokens.Length != 6)
						{
							return 0;
						}
						Planet planet = BuildPlanet(planetID++, tokens);
						planetBuilder.Add(planet);
						EditablePlayer player = EnsurePlayer(playerBuilder, planet.Owner);
						player.Planets.Add(planet);
						player.ShipsOnBase += planet.NumShips;
						player.ShipsHeavyPoint.X += planet.X * (double)planet.NumShips;
						player.ShipsHeavyPoint.Y += planet.Y * (double)planet.NumShips;

						int wishId = WishList.IndexOf(planet.PlanetID);
						if (wishId > -1)
						{
							if (planet.IsMine)
							{
								WishList.RemoveAt(wishId);
							}
							else
							{
								planet.IsOnWishList = true;
							}
						}
					#endregion
					}
					break;
				case "F":
				case "f":
					{
						#region AddFleet
						if (tokens.Length != 7)
						{
							return 0;
						}
						Fleet fleet = BuildFleet(tokens);
						EditablePlayer player = EnsurePlayer(playerBuilder, fleet.Owner);
						player.Fleets.Add(fleet);
						player.Targets.Add(fleet.DestinationPlanet);
						fleetbuilder.Add(fleet);
						player.ShipsInTransit += fleet.NumShips;

						#endregion
					}
					break;
			}
		}
		AllPlanetsOnPlanetId = planetBuilder.ToDictionary(item => item.PlanetID);

		//That's me
		EditablePlayer personalityBuilder = EnsurePlayer(playerBuilder, 1);
		Me = new Player(personalityBuilder.Fleets, personalityBuilder.Planets, personalityBuilder.Targets);
		Me.ShipCountInBase = personalityBuilder.ShipsOnBase;
		Me.ShipCountInTransit = personalityBuilder.ShipsInTransit;

		EditablePlayer neutralityBuilder = EnsurePlayer(playerBuilder, 0);
		Neutral = new Player(neutralityBuilder.Fleets, neutralityBuilder.Planets, personalityBuilder.Targets);
		Neutral.ShipCountInBase = neutralityBuilder.ShipsOnBase;
		Neutral.ShipCountInTransit = neutralityBuilder.ShipsInTransit;

		All = new Player(fleetbuilder, planetBuilder, new List<int>());

		foreach (EditablePlayer player in playerBuilder.Values)
		{
			TotalFleetCount += player.ShipsOnBase + player.ShipsInTransit;
		}

		foreach (Fleet attackForce in All.Fleets)
		{
			Planet target = AllPlanetsOnPlanetId[attackForce.DestinationPlanet];
			if (target.Owner != attackForce.Owner)
			{
				target.IsUnderAttack = true;
			}
			target.AddArmada(attackForce);

			//EditablePlayer player = EnsurePlayer(playerBuilder, target.PlanetID);
			//player.ShipsHeavyPoint.X += target.X * attackForce.NumShips;
			//player.ShipsHeavyPoint.Y += target.Y * attackForce.NumShips;
		}

		EditablePlayer enemy = EnsurePlayer(playerBuilder, 2);
		EnemyShipFocus = enemy.ShipsHeavyPoint.Calculate(enemy.ShipsInTransit + enemy.ShipsOnBase);
		EditablePlayer me = EnsurePlayer(playerBuilder, 1);
		OwnShipFocus = me.ShipsHeavyPoint.Calculate(me.ShipsOnBase);

		DeterminePlanetStrengthOnFleetArrival();

		if (TravelMap == null)
		{
			TravelMap = PlanetaryTravelRouteDictionary.Create(planetBuilder);
			Universe.InitialTotalFleetSize = TotalFleetCount;
			Universe.InitialFleetToPlanetRatio = (Universe.InitialTotalFleetSize - 200) / (double)(All.Planets.Count - 2);
			Universe.Difficulty = (Universe.InitialFleetToPlanetRatio - 30) / 5;
			FannyHeirdooBot.strategy = new DefensiveBotStrategy();
		}

		if (Universe.TurnCount % 15 == 0)
		{
			WishList.Clear();
		}
		return 1;
	}

	private void DeterminePlanetStrengthOnFleetArrival()
	{
		foreach (Planet defendingPlanet in All.Planets)
		{
			defendingPlanet.RelativeFleetStrength = (double)defendingPlanet.NumShips / (double)TotalFleetCount;
			if (defendingPlanet.IsUnderAttack)
			{
				defendingPlanet.MaxDesertersAllowed = defendingPlanet.NumShips;
				if (defendingPlanet.PlanetID == 1)
				{
				}
				int lastTurn = 0;
				int defendingForce = defendingPlanet.NumShips;
				foreach (var prediction in FleetArrivalByTurnsRemaining(defendingPlanet))
				{
					// Neutrals do not grow
					if (defendingPlanet.Owner > 0)
					{
						int turnDiff = prediction.Key - lastTurn;
						//when the first ships arrives, what is our strength?
						defendingForce += turnDiff * defendingPlanet.GrowthRate;
					}

					//Remove the fill force of that first wave.
					foreach (Fleet attackers in prediction)
					{
						if (attackers.Owner == defendingPlanet.Owner)
						{
							defendingForce += attackers.NumShips;
						}
						else
						{
							defendingForce -= attackers.NumShips;
						}
					}
					//did we survive?
					if (defendingForce >= 0)
					{

						//So the part that's left standing does not have to be here.
						if (defendingPlanet.IsMine)
						{
							defendingPlanet.MaxDesertersAllowed =
								 Math.Min(defendingPlanet.MaxDesertersAllowed // Value can only go down, otherwise we'll lose the planet in an earlier turn.
								, Math.Max(0                                   // Negative value, not good for the engine.
								, Math.Min(defendingForce                      // Do not send everything if we can spare it.
								, defendingPlanet.NumShips                     // Do not send ships we do not have, will end game quickly.
							)));

						}
						else
						{
							defendingPlanet.MaxDesertersAllowed = Math.Max(Math.Min(defendingForce, defendingPlanet.NumShips), 0);
						}

						lastTurn = prediction.Key;
					}
					else
					{
						//did not survive, the planet is lost
						defendingPlanet.LostAfterNumberOfTurns = prediction.Key;
						defendingPlanet.ShipsRequiredToSurviveAttack = (-defendingForce) + 3;
						defendingPlanet.CanSurviveIncommingAttack = false;
						break;
					}
				}
				if (!defendingPlanet.CanSurviveIncommingAttack)
				{
					defendingPlanet.MaxDesertersAllowed = 0;
				}
			}
		}
	}
	public static IOrderedEnumerable<IGrouping<int, Fleet>> FleetArrivalByTurnsRemaining(Planet target)
	{
		return target.Armada.ToLookup(item => item.TurnsRemaining).OrderBy(item => item.Key);
	}
	// Sends an order to the game engine. An order is composed of a source
	// planet number, a destination planet number, and a number of ships. A
	// few things to keep in mind:
	//   * you can issue many orders per turn if you like.
	//   * the planets are numbered starting at zero, not one.
	//   * you must own the source planet. If you break this rule, the game
	//     engine kicks your bot out of the game instantly.
	//   * you can't move more ships than are currently on the source planet.
	//   * the ships will take a few turns to reach their destination. Travel
	//     is not instant. See the Distance() function for more info.
	private void IssueOrder(Planet source, Planet dest, int numShips)
	{
		string message = String.Format("{0} {1} {2}", source.PlanetID, dest.PlanetID, numShips);
		Console.WriteLine(message);
		Console.Out.Flush();
		BotEnvironment.DumpMove(message);
		if (Debugger != null)
		{
			Debugger.IssueOrder(message);
		}
	}

	internal void MakeUnsafeMove(Planet ownedPlanet, Planet target, int shipCount)
	{
		MakeMove(ownedPlanet, target, shipCount, false);
	}
	private void MakeMove(Planet ownedPlanet, Planet target, int shipCount, bool safe)
	{
		if (ownedPlanet != null && target != null)
		{
			int ships = shipCount;
			if (safe)
			{
				ships = Math.Min(shipCount, ownedPlanet.AttackForce);
			}

			if (ships > 0)
			{
				ownedPlanet.RemoveShips(ships);
				if (target.IsMine)
				{
					//target.AddShips(shipCount);
				}
				else
				{
					target.RemoveShips(ships);
				}
				Me.ShipCountInBase -= ships;
				IssueOrder(ownedPlanet, target, ships);
			}
			else
			{
			}
		}
	}
	public void MakeMove(Planet ownedPlanet, Planet target, int shipCount)
	{
		MakeMove(ownedPlanet, target, shipCount, true);
	}



	// Sends the game engine a message to let it know that we're done sending
	// orders. This signifies the end of our turn.
	public void FinishTurn()
	{
		Console.WriteLine("# FANNY  ==> go");
		Console.WriteLine("go");
		Console.Out.Flush();
	}

	public static int ParseInt(string value)
	{
		return int.Parse(value, CultureInfo.InvariantCulture);
	}
	public static Double ParseDouble(string value)
	{
		return Double.Parse(value, CultureInfo.InvariantCulture);
	}


	private static Fleet BuildFleet(string[] tokens)
	{
		int owner = ParseInt(tokens[1]);
		int numShips = ParseInt(tokens[2]);
		int source = ParseInt(tokens[3]);
		int destination = ParseInt(tokens[4]);
		int totalTripLength = ParseInt(tokens[5]);
		int turnsRemaining = ParseInt(tokens[6]);

		return new Fleet(owner,
			numShips,
			source,
			destination,
			totalTripLength,
			turnsRemaining);
	}

	private static Planet BuildPlanet(int planetID, string[] tokens)
	{
		double x = ParseDouble(tokens[1]);
		double y = ParseDouble(tokens[2]);
		int owner = ParseInt(tokens[3]);
		int numShips = ParseInt(tokens[4]);
		int growthRate = ParseInt(tokens[5]);

		return new Planet(planetID++,
		 owner,
		 numShips,
		 growthRate,
		 x, y);
	}

	/// <summary>
	/// Ensures the player.
	/// </summary>
	/// <param name="playerBuilder">The player builder.</param>
	/// <param name="id">The id.</param>
	private static EditablePlayer EnsurePlayer(Dictionary<int, EditablePlayer> playerBuilder, int id)
	{
		EditablePlayer result;
		if (!playerBuilder.TryGetValue(id, out result))
		{
			result = new EditablePlayer();
			playerBuilder.Add(id, result);
		}
		return result;
	}

}
