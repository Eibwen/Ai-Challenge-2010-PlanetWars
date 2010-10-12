using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FannyHeirdooBot
{
	public abstract class BotStrategy : IBotStrategy
	{
		/// <summary>
		/// Does the turn.
		/// </summary>
		/// <param name="uni">The uni.</param>
		public void DoTurn(Universe uni)
		{
			List<AttackPlan> actionPlan = BuildActionPlan(uni);
			if (!SortAndMakeMoves(uni, actionPlan))
			{
				AttackPlanFailed();
			}
		}

		protected IEnumerable<Planet> OrderedListOfAttackedPlanets(int playerId)
		{
			return currentUniverse.All.Planets
				.Where(planet => planet.IsUnderAttack && planet.Owner == playerId)
				.OrderBy(planet => planet.WinningArmadaIsMine)
				.ThenBy(planet => -planet.LastAttackTurn.TurnsFromNow);
		}

		protected Planet WeakestEnemyPlanetWithGrowth;
		public void FindWeakestEnemyPlanetWithGrowth()
		{
			WeakestEnemyPlanetWithGrowth = currentUniverse.All.Planets
				.Where(planet => planet.Owner == 2 && planet.GrowthRate > 0)
				.OrderBy(planet => planet.GrowthRate)
				.FirstOrDefault();
		}
		protected Planet StrongestEnemyPlanet;
		public void FindStrongestEnemyPlanet()
		{
			StrongestEnemyPlanet = currentUniverse.All.Planets
				.Where(planet => planet.Owner == 2)
				.OrderBy(planet => -planet.GrowthRate)
				.FirstOrDefault();
		}


		protected static IOrderedEnumerable<IGrouping<int, Route>> PlanetRouteByDistanceDictionary(PlanetaryTravelRoute routesFromPlanet)
		{
			return routesFromPlanet.ToLookup(item => item.DistanceInTurns).OrderBy(item => item.Key); ;
		}

		public bool TakeGamble_DoNotCountOutgoingShipsForOneTurn { get; set; }
		protected Universe currentUniverse;
		//occurs when currentUniverse has been set and before all other processing.
		protected virtual void StartOfTurn()
		{

		}
		//return true if we should should search this planet. false to skip it.
		protected virtual bool SourcePlanetSelected(Planet onwedPlanet)
		{
			return true;
		}

		//return true if we should should search this route. false to skip it
		protected virtual bool PlanetRouteSelected(Planet onwedPlanet, PlanetaryTravelRoute routesFromPlanet)
		{
			return true;
		}

		protected virtual void PlanetRouteDeSelected(Planet onwedPlanet, PlanetaryTravelRoute planetTravelMap)
		{
		}

		protected abstract bool OwnedPlanetCanSendAttackForce(Planet ownedPlanet);

		protected bool CanDefendNeutralPlanets;
		protected abstract void DefendOnwedPlanets();
		protected abstract void DefendNeutralPlanets(int maxTurnLookahead);

		protected int MaxTurnLookaheadForNeutralDefense;

		public List<AttackPlan> BuildActionPlan(Universe uni)
		{
			currentUniverse = uni;
			StartOfTurn();
			DefendOnwedPlanets();
			DefendNeutralPlanets(MaxTurnLookaheadForNeutralDefense);
			ProcessAttackQueue();
			List<AttackPlan> battleplan = new List<AttackPlan>();
			foreach (Planet onwedPlanet in currentUniverse.Me.Planets.Where(OwnedPlanetCanSendAttackForce))
			{
				if (SourcePlanetSelected(onwedPlanet))
				{
					var planetTravelMap = onwedPlanet.Routes;
					if (PlanetRouteSelected(onwedPlanet, planetTravelMap))
					{
						bool continueRoute = true;
						AttackPlan attack = new AttackPlan();
						foreach (Route route in SortRoutesForBattlePlanCreation(planetTravelMap))
						{
							Planet hostile = route.Destination;
							if (!hostile.IsMine)
							{
								attack.Target = hostile;
								attack.Enabled = false;
								attack.Strategy = GetType().Name;
								attack.Reason = "CreateAttackPlanForOwnedPlanet";
								CreateAttackPlanForOwnedPlanet(attack, route, planetTravelMap, ref continueRoute);

								if (attack.Enabled)
								{
									if (onwedPlanet.IdleForThisNumberOfTurns >= 15)
									{
										attack.Sweetness += onwedPlanet.IdleForThisNumberOfTurns;
									}
									battleplan.Add(attack);
									attack = new AttackPlan();
								}
							}
							if (!continueRoute) { break; }
						}
					}
					PlanetRouteDeSelected(onwedPlanet, planetTravelMap);
				}
			}
			if (!TakeGamble_DoNotCountOutgoingShipsForOneTurn)
			{
				//attack planets for all fleets in transit
				foreach (Planet planet in uni.All.Planets)
				{
					if (!(planet.IsLost || planet.Armada.Count == 0))
					{
						foreach (Fleet attackforce in planet.Armada)
						{
							if (attackforce.IsMine)
							{
								planet.RemoveShips(attackforce.NumShips);
							}
						}
					}
				}
			}
			TakeGamble_DoNotCountOutgoingShipsForOneTurn = false;
			return battleplan;
		}

		protected virtual void ProcessAttackQueue()
		{
		}

		protected virtual IEnumerable<Route> SortRoutesForBattlePlanCreation(PlanetaryTravelRoute planetTravelMap)
		{
			return planetTravelMap;
		}
		protected abstract void CreateAttackPlanForOwnedPlanet(AttackPlan attack, Route attackRoute, PlanetaryTravelRoute planetTravelMap, ref bool continueRoute);

		public virtual void AttackPlanFailed()
		{
		}

		private bool SortAndMakeMoves(Universe uni, List<AttackPlan> sweetnesses)
		{
			return MakeSweetestMoves(uni, sweetnesses.OrderBy(item => -item.Sweetness));
		}

		private bool MakeSweetestMoves(Universe uni, IEnumerable<AttackPlan> attackPlan)
		{
			bool attackPerformed = false;
			Planet defender = null;

			double bestSweetness = Double.MaxValue;
			foreach (var attack in attackPlan)
			{
				defender = attack.Target;
				if (bestSweetness == Double.MaxValue)
				{
					bestSweetness = attack.Sweetness;
				}
				else
				{
					double relativeSweetness = attack.Sweetness / bestSweetness;
					if (relativeSweetness < 0.20)
					{
						// Compared to the moved that we like, this is really bad.
						// So instead of making a very bad move, we will just end our turn.
						break;
					}
				}
				foreach (AttackPlanParticipant order in attack.Participants)
				{
					Planet agressor = order.Source;
					if (agressor.AttackMovesAllowed)
					{
						bool domination = currentUniverse.IsDominating || attack.DominationMove;
						bool canConquer = agressor.DoesNotChangeOwner
							&& Math.Max(defender.ShipsRequiredToSurviveAttack, defender.AttackForce) < agressor.AttackForce;

						if (domination || agressor.IsLost || canConquer)
						{
							if (defender.IsAttackable || domination)
							{
								uni.AddToWishList(defender);
								//uni.MakeUnsafeMove(agressor, Defender, attack.ShipCount);
								uni.MakeMove(agressor, defender, agressor.AttackForce);
								attackPerformed = true;
							}
						}
					}
				}
			}
			return attackPerformed;
		}
	}

}