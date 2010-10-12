// Contestants do not need to worry about anything in this file. This is just
// helper code that does the boring stuff for you, so you can focus on the
// interesting stuff. That being said, you're welcome to change anything in
// this file if you know what you're doing.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PlanetDebug;

namespace FannyHeirdooBot
{
	public partial class Universe
	{
		botDebugBase Debugger;
		// Constructs a PlanetWars object instance, given a string containing a
		// description of a game state.
		public Universe(botDebugBase debugger)
		{
			BotEnvironment.ClearTrace();
			Universe.TurnCount = 0;
			Debugger = debugger;
			Planets = new Dictionary<int, Planet>();
		}

		/// <summary>
		/// Gets the planets.
		/// </summary>
		/// <value>The planets.</value>
		public Dictionary<int, Planet> Planets { get; private set; }

		public void StartTurn(string boardLayout)
		{
			TurnCount++;
			BotEnvironment.DumpLayout(boardLayout);
			ParseGameState(boardLayout);
			ChooseStrategy().DoTurn(this);
		}


		/// <summary>
		/// Gets the turn count.
		/// </summary>
		/// <value>The turn count.</value>
		public static int TurnCount { get; private set; }
		public double EarlyOrLateStageBias { get; private set; }
		public bool IsDominating { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public int TotalFleetCount;
		public int InitialEnemyFleetDistance { get; set; }
		/// <summary>
		/// If this number is very high, then capturing a planet will be difficult and requires a different approach.
		/// </summary>
		/// <value>The initial size of the universe fleet.</value>
		public int InitialTotalFleetSize { get; private set; }
		/// <summary>
		/// Gets or sets the initial size of the universe fleet.
		/// </summary>
		/// <value>The initial size of the universe fleet.</value>
		public double InitialFleetToPlanetRatio { get; private set; }
		/// <summary>
		/// Gets or sets the difficulty.
		/// Zero to +/- 4
		/// </summary>
		/// <value>The difficulty.</value>
		public double Difficulty { get; set; }

		public Player Me { get; private set; }
		public Player Neutral { get; private set; }
		public Player All { get; private set; }
		public Player Enemy { get; private set; }


		// Parses a game state from a string. On success, returns 1. On failure,
		// returns 0.
		private int ParseGameState(string boardLayout)
		{
			List<Fleet> fleetbuilder = new List<Fleet>();
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
							EditablePlayer player = EnsurePlayer(playerBuilder, planet.Owner);
							player.Planets.Add(planet);
							planet.IdleForThisNumberOfTurns++;
							player.ShipsOnBase += planet.NumShips;
							player.ShipGrowth += planet.GrowthRate;
							player.ShipsHeavyPoint.X += planet.X * (double)planet.NumShips;
							player.ShipsHeavyPoint.Y += planet.Y * (double)planet.NumShips;
							player.AbsolutePlanetFocus.Y += planet.Y;
							player.AbsolutePlanetFocus.X += planet.X;
							if (WishList.ContainsKey(planet.PlanetID))
							{
								if (planet.IsMine)
								{
									WishList.Remove(planet.PlanetID);
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
							player.Targets.Add(fleet.DestinationPlanetId);
							fleetbuilder.Add(fleet);
							player.ShipsInTransit += fleet.NumShips;
							player.ShipInTransitFocus.X += fleet.DestinationPlanet.X * (double)fleet.NumShips;
							player.ShipInTransitFocus.Y += fleet.DestinationPlanet.Y * (double)fleet.NumShips;



							#endregion
						}
						break;
				}
			}

			//That's me
			EditablePlayer personalityBuilder = EnsurePlayer(playerBuilder, 1);
			Me = new Player(personalityBuilder.Fleets, personalityBuilder.Planets, personalityBuilder.Targets);
			personalityBuilder.InitializePlayer(Me);

			EditablePlayer neutralityBuilder = EnsurePlayer(playerBuilder, 0);
			Neutral = new Player(neutralityBuilder.Fleets, neutralityBuilder.Planets, personalityBuilder.Targets);
			neutralityBuilder.InitializePlayer(Neutral);

			All = new Player(fleetbuilder, Planets.Values.ToList(), new List<int>());

			foreach (EditablePlayer player in playerBuilder.Values)
			{
				TotalFleetCount += player.ShipsOnBase + player.ShipsInTransit;
			}

			EditablePlayer enemyBuilder = EnsurePlayer(playerBuilder, 2);
			Enemy = new Player(enemyBuilder.Fleets, enemyBuilder.Planets, enemyBuilder.Targets);
			enemyBuilder.InitializePlayer(Enemy);

			All.ShipGrowth = Me.ShipGrowth + Enemy.ShipGrowth + Neutral.ShipGrowth;


			if (Universe.TurnCount == 1)
			{
				CreatePlanetaryTravelRoutes();
			}

			foreach (Planet futurePlane in Planets.Values)
			{
				futurePlane.CreateTurnPredictions();
			}

			DeterminePlanetStrengthOnFleetArrival();
			return 1;
		}

		//public class Item
		//{
		//    public PlanetTurn turn;
		//    public Route route;
		//}
		//private void test()
		//{
		//    List<Item> totry = new List<Item>();
		//    foreach (Planet plan in Planets.Values)
		//    {
		//        PlanetTurn turn = plan.TurnPrediction[0];
		//        if (turn.Owner == 1)
		//        {
		//            foreach (Route route in plan.Routes)
		//            {
		//                if (route.DestinationStateOnArrival.NumShips < turn.NumShips &&
		//                    route.DestinationStateOnArrival.Owner != 1)
		//                {
		//                    Item item = new Item();
		//                    item.turn = turn;
		//                    item.route = route;
		//                    totry.Add(item);
		//                }
		//            }
		//        }
		//    }
		//}


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
			source.IdleForThisNumberOfTurns = 0;
			Console.WriteLine(message);
			Console.Out.Flush();
			BotEnvironment.DumpMove(message);
			if (Debugger != null)
			{
				Debugger.IssueOrder(message);
			}
		}

		// Sends the game engine a message to let it know that we're done sending
		// orders. This signifies the end of our turn.
		public void FinishTurn()
		{
			Console.WriteLine("go");
			Console.Out.Flush();
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
					ships = Math.Min(Math.Min(shipCount, ownedPlanet.AttackForce), ownedPlanet.NumShips);
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


		public static int ParseInt(string value)
		{
			return int.Parse(value, CultureInfo.InvariantCulture);
		}
		public static Double ParseDouble(string value)
		{
			return Double.Parse(value, CultureInfo.InvariantCulture);
		}

		private Fleet BuildFleet(string[] tokens)
		{
			int owner = ParseInt(tokens[1]);
			int numShips = ParseInt(tokens[2]);
			int source = ParseInt(tokens[3]);
			int destination = ParseInt(tokens[4]);
			int totalTripLength = ParseInt(tokens[5]);
			int turnsRemaining = ParseInt(tokens[6]);

			Planet destinationPlanet = Planets[destination];
			if (owner == 2)
			{
				destinationPlanet.IsAttackedByEnemy = true;
			}
			Planet sourcePlanet = Planets[source];
			return new Fleet(owner,
				numShips,
				sourcePlanet,
				destinationPlanet,
				totalTripLength,
				turnsRemaining);
		}

		private Planet BuildPlanet(int planetID, string[] tokens)
		{
			Planet result;
			int owner = ParseInt(tokens[3]);
			int numShips = ParseInt(tokens[4]);
			if (!Planets.TryGetValue(planetID, out result))
			{
				double x = ParseDouble(tokens[1]);
				double y = ParseDouble(tokens[2]);
				int growthRate = ParseInt(tokens[5]);
				result = new Planet(planetID, owner, numShips, growthRate, x, y);
				result.CreateTurns(40);
				Planets.Add(planetID, result);
			}
			else
			{
				result.SynchronizeWithGameStatus(owner, numShips);
			}
			return result;
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

		private Dictionary<int, int> WishList = new Dictionary<int, int>();
		public void AddToWishList(Planet onwedPlanet)
		{
			if (WishList.ContainsKey(onwedPlanet.PlanetID))
			{
				WishList[onwedPlanet.PlanetID] = 15;
			}
			else
			{
				WishList.Add(onwedPlanet.PlanetID, 15);
			}
		}

		IBotStrategy _Strategy = new Strategy_LastTry();
		private IBotStrategy ChooseStrategy()
		{
			bool wasDominating = IsDominating;

			foreach (var item in WishList.ToList())
			{
				if (item.Value < 1)
					WishList.Remove(item.Key);
				else
					WishList[item.Key] = item.Value - 1;
			}
			InitialTotalFleetSize = TotalFleetCount;
			InitialFleetToPlanetRatio = (InitialTotalFleetSize - 200) / (double)(All.Planets.Count - 2);
			Difficulty = (InitialFleetToPlanetRatio - 30) / 5;

			EarlyOrLateStageBias = Math.Max(0, 4 - (Universe.TurnCount + 1) / 10.0);
			if (TurnCount > 20 && Me.Planets.Count > 4)
			{
				int dominatorCounter = 0;
				double totalShipPercentile = Me.TotalShipCount / (double)(Enemy.TotalShipCount + 1);
				if (totalShipPercentile > 1.5) dominatorCounter++;

				double totalPlanetPercentile = Me.Planets.Count / (double)(Enemy.Planets.Count + 1);
				if (totalPlanetPercentile > 1.5) dominatorCounter++;

				double totalShipGrowthPercentile = Me.ShipGrowth / (double)(Enemy.ShipGrowth + 1);
				if (totalShipGrowthPercentile > 1.25) dominatorCounter++;
				if (totalShipGrowthPercentile > 2.0) dominatorCounter++;

				double totalShipInBasePercentile = Me.ShipCountInBase / (double)(Enemy.ShipCountInBase + 1);
				if (totalShipInBasePercentile > 1.5) dominatorCounter++;

				double totalAirSuperiority = Me.ShipCountInBase / (double)(Enemy.ShipCountInTransit + 1);
				if (totalAirSuperiority < 0.8) dominatorCounter--;

				IsDominating = dominatorCounter > 2;
			}

			switch (Universe.TurnCount)
			{
				case 0: break;
				case 1: _Strategy = new Strategy_LastTry();
					break;
				case 2: _Strategy = new Strategy_LastTry();
					break;
				case 15:
					_Strategy = new Strategy_LastTry();
					break;
				default:
					if (!IsDominating && (IsDominating != wasDominating))
					{
						_Strategy = new Strategy_LastTry();
					}
					if (IsDominating && (IsDominating != wasDominating))
					{
						_Strategy = new Strategy_LastTry();
					}
					break;
			}
			return _Strategy;
		}


	}
}