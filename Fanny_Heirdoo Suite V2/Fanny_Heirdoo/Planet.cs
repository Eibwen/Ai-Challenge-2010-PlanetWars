using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using botDebug;

namespace FannyHeirdooBot
{
	[DebuggerDisplay("Id: {planetID} Owner: {Owner} Ships: {NumShips} Grow: {growthRate}")]
	public class Planet : IPlanet
	{
		// Initializes a planet.
		public Planet(int planetID, int owner, int numShips, int growthRate, double x, double y)
		{
			this.planetID = planetID;
			this.growthRate = growthRate;
			this.x = x;
			this.y = y;
			Routes = new PlanetaryTravelRoute();
			SynchronizeWithGameStatus(owner, numShips);
		}

		public int ShipCountAtStartOfTurn;
		public bool IsAttackable
		{
			get
			{
				if (IsMine)
				{
					return false;
				}
				else
				{
					return NumShips > -1;
				}
			}
		}
		public PlanetArmada Armada { get; private set; }
		internal void AddArmada(Fleet armada)
		{
			Armada.Add(armada);
		}

		public PlanetaryTravelRoute Routes { get; private set; }
		// Accessors and simple modification functions. These should be mostly
		// self-explanatory.
		public int PlanetID
		{
			get
			{
				return planetID;
			}
		}

		// Synchronizes the with game status.
		internal void SynchronizeWithGameStatus(int ownerId, int numShips)
		{
			owner = ownerId;
			_NumShips = numShips;
			ShipCountAtStartOfTurn = numShips;
			ShipsRequiredToSurviveAttack = -1;
			MaxDesertersAllowed = -1;
			DoesNotChangeOwner = true;
			AttackMovesAllowed = true;
			Armada = new PlanetArmada(this);

			IsUnderAttack = false;
			IsMine = owner == 1;
			IsNeutral = owner == 0;
			IsEnemy = !IsMine && !IsNeutral;
			WinningArmadaIsMine = IsMine;
		}

		public int Owner
		{
			get
			{
				return owner;
			}
		}

		public int NumShips
		{
			get
			{
				return _NumShips;
			}
		}

		public void Grow()
		{
			if (Owner > 0)
			{
				AddShips(GrowthRate);
			}
		}

		/// <summary>
		/// Gets the attack force.
		/// </summary>
		/// <value>The attack force.</value>
		public int AttackForce
		{
			get
			{
				if (MaxDesertersAllowed > -1)
				{
					return MaxDesertersAllowed;
				}
				else
				{
					return NumShips;
				}
			}
		}

		/// <summary>
		/// Removes the ships.
		/// </summary>
		/// <param name="amount">The amount.</param>
		public void RemoveShips(int amount)
		{
			MaxDesertersAllowed -= amount;
			if (MaxDesertersAllowed < 0)
			{
				MaxDesertersAllowed = -1;
			}
			_NumShips -= amount;
		}


		public int GrowthRate
		{
			get
			{
				return growthRate;
			}
		}

		public double X
		{
			get
			{
				return x;
			}
		}

		public double Y
		{
			get
			{
				return y;
			}
		}

		/// <summary>
		/// Changes the owner.
		/// </summary>
		/// <param name="newOwner">The new owner.</param>
		/// <param name="newShipCount">The new ship count.</param>
		public void ChangeOwner(int newOwner, int newShipCount)
		{
			owner = newOwner;
			_NumShips = newShipCount;

			IsUnderAttack = false;
			IsMine = owner == 1;
			IsNeutral = owner == 0;
			IsEnemy = !IsMine && !IsNeutral;
			WinningArmadaIsMine = IsMine;
		}

		public void AddShips(int amount)
		{
			_NumShips += amount;
		}
		#region properties

		public bool IsLost;
		public bool IsUnderAttack;

		private int planetID;
		private int owner;
		private int _NumShips;
		private int growthRate;
		private double x, y;

		public bool IsMine { get; private set; }
		public bool IsNeutral { get; set; }
		public bool IsEnemy { get; set; }
		public bool IsOnWishList { get; set; }
		public double Connectiveness { get; set; }
		public double Growthyness { get; set; }
		public bool AttackMovesAllowed;
		#endregion properties

		internal Planet Clone(bool cloneArmada)
		{
			Planet result = new Planet(planetID, owner, _NumShips, growthRate, x, y);
			result.IsLost = IsLost;
			result.IsUnderAttack = IsUnderAttack;
			result.WinningArmadaIsMine = WinningArmadaIsMine;
			result.ShipCountAtStartOfTurn = ShipCountAtStartOfTurn;
			result.DoesNotChangeOwner = DoesNotChangeOwner;
			result.MaxDesertersAllowed = MaxDesertersAllowed;
			result.IsNeutral = IsNeutral;
			result.IsEnemy = IsEnemy;
			result.IsAttackedByEnemy = IsAttackedByEnemy;
			result.IsOnWishList = IsOnWishList;
			if (cloneArmada)
			{
				result.Armada = Armada.Clone();
			}
			return result;
		}


		// NOTE: this is not a prediction, based on current board status, this is fact.
		public PlanetTurnPredictions TurnPrediction;
		public bool DoesNotChangeOwner;
		public bool WinningArmadaIsMine;
		public IOrderedEnumerable<IGrouping<int, Fleet>> FleetArrivalByTurnsRemaining { get; private set; }
		public void CreateFleetArrivalByTurnsRemaining()
		{
			FleetArrivalByTurnsRemaining = Armada.ToLookup(item => item.TurnsRemaining).OrderBy(item => item.Key);
		}

		public int ShipsRequiredToSurviveAttack;
		public int MaxDesertersAllowed;
		public PlanetTurn LastAttackTurn;
		public bool IsAttackedByEnemy;

		public Route NearestEnemy { get; private set; }
		public double NearestEnemyDistance { get; private set; }
		public Route NearestNeutral { get; private set; }
		public double NearestNeutralDistance { get; private set; }
		public Route NearestFriendly { get; private set; }
		public double NearestFriendlyDistance { get; private set; }
		public Route FarthestFriendly { get; private set; }

		// NOTE: this is not a prediction, based on current board status, this is fact.
		public void CreateTurnPredictions()
		{
			CreateFleetArrivalByTurnsRemaining();
			TurnPrediction.CreateTurnPredictionsForPlanet(this);
			NearestEnemy = null;
			NearestNeutral = null;
			NearestFriendly = null;
			FarthestFriendly = null;
			NearestEnemyDistance = 0;
			NearestNeutralDistance = 0;
			NearestFriendlyDistance = 0;
			foreach (var route in Routes)
			{
				if (route.Destination.IsNeutral && NearestNeutral == null)
				{
					NearestNeutral = route;
					NearestNeutralDistance = route.ActualDistance;
				}
				if (route.Destination.IsEnemy && NearestEnemy == null)
				{
					NearestEnemy = route;
					NearestEnemyDistance = route.ActualDistance;
				}
				if (route.Destination.IsMine)
				{
					if (NearestFriendly == null)
					{
						NearestFriendly = route;
						NearestFriendlyDistance = route.ActualDistance;
					}
					FarthestFriendly = route;
				}
			}
		}

		public int IdleForThisNumberOfTurns;

		internal void CreateTurns(int maxTurns)
		{
			// Pre create all the turns for the board, 
			// At the beginning of each turn we need to update these states.
			// Updating these states is actually prediction the future with the known facts.
			// Based on these facts we can start looking for good moves.
			// That way we know the objects will all be there which make the rest of the code simpler.
			// it also reduces the number of memory allocations, which should help speed.
			TurnPrediction = new PlanetTurnPredictions(maxTurns);
			TurnPrediction.Capacity = TurnPrediction.Count;
		}

		public bool IsInAttackQueue { get; set; }

		/// <summary>
		/// Calcs the max gain using this many ships.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="arrival">The arrival.</param>
		/// <param name="numShips">The num ships.</param>
		/// <returns></returns>
		internal PlanetTurn CalcMaxGainUsingThisManyShips(Planet source, int arrival, int numShips)
		{
			Planet clone = Clone(true);
			clone.CreateTurns(TurnPrediction.Count);
			Fleet attack = new Fleet(1, numShips, source, clone, arrival, arrival);
			clone.CreateTurnPredictions();
			return clone.TurnPrediction.LastTurn;
		}
	}
}