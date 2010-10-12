using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FannyHeirdooBot
{
	public class Strategy_BackingItUp : IBotStrategy
	{
		List<AttackPlan> attackQueue = new List<AttackPlan>();

		protected void ProcessAttackQueue()
		{
			DefendNeutralPlanets(30);
			DefendOnwedPlanets();
			foreach (var planet in currentUniverse.Planets.Values)
			{
				planet.IsInAttackQueue = false;
			}
			foreach (var planInMotion in attackQueue.ToList())
			{
				ExecutePlans(planInMotion);
			}
		}

		protected bool ExecutePlans(AttackPlan sweetPlan)
		{
			bool movemade = false;
			foreach (AttackPlanParticipant attacker in sweetPlan.Participants)
			{
				Planet agressor = attacker.Source;
				agressor.IsInAttackQueue = attacker.ScheduledTurn > 0;
				if (attacker.ScheduledTurn <= 0)
				{
					if (attacker.DefendersTurn.Owner != 1 || sweetPlan.DominationMove)
					{
						if (agressor.IsMine)
						{
							int ships = attacker.AttackersTurn.NumShips;
							if (!sweetPlan.Target.IsMine)
							{
								ships = Math.Min(attacker.DefendersTurn.NumShips + 1, ships);
							}
							ships = Math.Min(ships, agressor.AttackForce);

							currentUniverse.MakeMove(agressor, sweetPlan.Target, ships);
							movemade = true;
							agressor.CreateTurnPredictions();
						}
					}
				}
				attacker.ScheduledTurn--;
			}
			if (sweetPlan.Target.IsMine || sweetPlan.Participants.Count == 0)
			{
				foreach (AttackPlanParticipant attacker in sweetPlan.Participants)
				{
					attacker.Source.IsInAttackQueue = false;
				}
				attackQueue.Remove(sweetPlan);
			}
			sweetPlan.Target.AttackMovesAllowed = false;
			return movemade;
		}

		private Universe currentUniverse;
		protected IEnumerable<Planet> CanUseInAtttack()
		{
			foreach (Planet subject in currentUniverse.Me.Planets)
			{
				if (!subject.IsInAttackQueue)
				{
					if (!subject.IsUnderAttack || subject.WinningArmadaIsMine)
					{
						if (subject.AttackForce > 1)
						{
							yield return subject;
						}
					}
				}
			}
		}

		protected bool IsNotEventuallyMine(Planet subject)
		{
			if (subject.IsMine || subject.WinningArmadaIsMine)
			{
				return false;
			}
			return true;
		}

		public void DoTurn(Universe uni)
		{
			currentUniverse = uni;
			ProcessAttackQueue();
			bool attackMade = false;
			int sumAttackForce = 0;


			Dictionary<Planet, PlanetTurn> possibleTargets = new Dictionary<Planet, PlanetTurn>();
			foreach (var planet in CanUseInAtttack())
			{
				sumAttackForce += planet.AttackForce;
				possibleTargets.Clear();
				PlanetTurn attackTurn = planet.TurnPrediction[0];
				//find planets that can be attacked with this planets force.
				foreach (var route in planet.Routes
					.Where(target => IsNotEventuallyMine(target.Destination)
									 && target.Destination.GrowthRate > 0))
				{
					PlanetTurn state = route.Destination.TurnPrediction[route.DistanceInTurns + 1];
					if (state.Owner != 1 && state.NumShips < planet.AttackForce)
					{
						possibleTargets.Add(route.Destination, state);
					}
				}
				// from the list find the planets from which I have most to gain or which are nearest.

				if (Universe.TurnCount < 3)
				{
					foreach (var item in possibleTargets.OrderBy(
						target => -target.Key.GrowthRate)
						.ThenBy(target => target.Value.NumShips))
					{
						if (planet.IsNeutral)
						{
							if (planet.AttackForce > item.Value.NumShips)
							{
								AttackPlan plan = new AttackPlan();
								plan.Enabled = true;
								plan.AddParticipant(planet, attackTurn, item.Value);
								plan.Target = item.Key;
								attackMade = true;
								ExecutePlans(plan);
							}
							else
							{
								break;
							}
						}
					}
				}
				else
				{
					foreach (var item in possibleTargets.OrderBy(
						target => -((target.Key.IsNeutral ? 2 : 0) + target.Key.GrowthRate) / currentUniverse.Me.ShipOnPlanetFocus.Delta(target.Key)))
					{
						if (planet.AttackForce > item.Value.NumShips)
						{
							AttackPlan plan = new AttackPlan();
							plan.Enabled = true;
							plan.AddParticipant(planet, attackTurn, item.Value);
							plan.Target = item.Key;
							attackMade = true;
							ExecutePlans(plan);
						}
						else
						{
							break;
						}
					}
				}
			}

			var centerPlanet = currentUniverse.Me.Planets
				.OrderBy(item => Universe.Center.Delta(item)
					+ currentUniverse.Enemy.ShipOnPlanetFocus.Delta(item)).FirstOrDefault();
			if (centerPlanet != null)
			{
				int supportCount = 0;
				foreach (var planet in CanUseInAtttack())
				{
					if (centerPlanet != planet)
					{
						supportCount++;
						currentUniverse.MakeMove(planet, centerPlanet, Math.Min(planet.AttackForce, 2));
					}
				}

			}


			if (!attackMade)
			{
				int targetOwnerId = 0;
				if (!currentUniverse.IsDominating)
				{
					if (rnd.NextDouble() > currentUniverse.Neutral.Planets.Count / (currentUniverse.Enemy.Planets.Count + 0.1))
					{
						targetOwnerId = 2;
					}
				}
				else
				{
					targetOwnerId = 2;
				}
				var target = currentUniverse.Planets.Values
					   .Where(planet => planet.LastAttackTurn.NumShips < sumAttackForce - planet.GrowthRate * 2
										&& IsNotEventuallyMine(planet)
										&& planet.GrowthRate > 1
										&& planet.Owner == targetOwnerId
										)
					.OrderBy(planet => -((planet.GrowthRate + Math.Sqrt(planet.NumShips)) / 3) * 3)
					.ThenBy(planet => currentUniverse.Me.ShipOnPlanetFocus.Delta(planet))
					.FirstOrDefault();

				if (target != null)
				{
					AttackPlan plan = new AttackPlan();
					plan.Enabled = true;
					plan.Target = target;
					foreach (var planet in CanUseInAtttack())
					{
						plan.AddParticipant(planet, planet.TurnPrediction[0], target.TurnPrediction[0]);
					}
					attackMade = true;
					ExecutePlans(plan);
				}
			}



			if (!attackMade && sumAttackForce > 100)
			{

				AttackPlan plan = new AttackPlan();
				plan.Enabled = true;
				plan.DominationMove = true;
				plan.Target = centerPlanet;
				foreach (var planet in CanUseInAtttack().Where(item => item != centerPlanet))
				{
					plan.AddParticipant(planet, planet.TurnPrediction[0], centerPlanet.TurnPrediction[0]);
				}
				attackMade = true;
				ExecutePlans(plan);
			}
		}
		Random rnd = new Random();
		protected IEnumerable<Planet> OrderedListOfAttackedPlanets(int playerId)
		{
			return currentUniverse.All.Planets
				.Where(planet => planet.IsUnderAttack && planet.Owner == playerId)
				.OrderBy(planet => planet.WinningArmadaIsMine)
				.ThenBy(planet => -planet.LastAttackTurn.TurnsFromNow);
		}

		protected void DefendNeutralPlanets(int maxTurnLookahead)
		{

			foreach (Planet neutralPlanet in OrderedListOfAttackedPlanets(0))
			{
				#region Try defend any owned planets.
				//if we have ships invested, protect investment,
				bool shouldDefend = neutralPlanet.IsUnderAttack &&
					!neutralPlanet.WinningArmadaIsMine
						&& !neutralPlanet.DoesNotChangeOwner;
				if (shouldDefend)
				{
					AttackPlan defensePlan = BuildDefensePlan(neutralPlanet, maxTurnLookahead);
					if (!neutralPlanet.IsLost)
					{
						foreach (AttackPlanParticipant armada in defensePlan.Participants)
						{
							currentUniverse.MakeMove(armada.Source, defensePlan.Target, armada.Source.AttackForce);
						}
						neutralPlanet.AttackMovesAllowed = false;
					}
				}
				#endregion
			}
		}
		protected void DefendOnwedPlanets()
		{
			foreach (Planet onwedPlanet in OrderedListOfAttackedPlanets(1))
			{
				#region Try defend any owned planets.
				bool shouldDefend = onwedPlanet.IsUnderAttack &&
					!onwedPlanet.WinningArmadaIsMine;

				if (shouldDefend)
				{
					AttackPlan defensePlan = BuildDefensePlan(onwedPlanet, 1000);
					if (onwedPlanet.IsLost)
					{
						foreach (var route in onwedPlanet.Routes)
						{
							if (route.DistanceInTurns <= onwedPlanet.LastAttackTurn.TurnsFromNow)
							{
								Planet target = route.Destination;
								if (target.IsNeutral && target.MaxDesertersAllowed < onwedPlanet.NumShips)
								{
									currentUniverse.MakeMove(onwedPlanet, target, onwedPlanet.NumShips);
									break;
								}
							}
							else
							{
								break;
							}
						}

						//Recapture it the planet provides good resource.
						if (onwedPlanet.GrowthRate >2)
						{
							currentUniverse.AddToWishList(onwedPlanet);
						}
					}
					else
					{
						foreach (AttackPlanParticipant armada in defensePlan.Participants)
						{
							currentUniverse.MakeMove(armada.Source, defensePlan.Target, armada.Source.AttackForce);
							armada.Source.AttackMovesAllowed = false;
							armada.Source.CreateTurnPredictions();
						}
						onwedPlanet.AttackMovesAllowed = false;
						onwedPlanet.CreateTurnPredictions();
					}
				}
				else
				{

				}
				#endregion
			}
		}
		private AttackPlan BuildDefensePlan(Planet planetUnderAttack, int maxTurnLookAhead)
		{
			AttackPlan defensePlan = new AttackPlan();
			defensePlan.Target = planetUnderAttack;
			defensePlan.Reason = "Defense";
			planetUnderAttack.IsLost = !planetUnderAttack.WinningArmadaIsMine;

			PlanetTurn turn = planetUnderAttack.TurnPrediction.FirstTurn;
			int defenseRequired = 0;
			if (turn != null)
			{
				//this is weird, we are in no danger of losing the planet still this method was called. Abort.
				if (planetUnderAttack.WinningArmadaIsMine)
				{
					return defensePlan;
				}
				//the next turns tells me how many ships I needed, only works if my fleet was not the winning fleet.
				//that's why the previous exit.
				defenseRequired = planetUnderAttack.TurnPrediction[turn.TurnsFromNow + 1].NumShips + 1;
			}
			else
			{
				// So apparently I did not lose ownership of the planet, which means I never did own it.
				//see if we can block the attack.
				turn = planetUnderAttack.TurnPrediction.FirstTurn;
				defenseRequired = turn.NumShips;
			}

			int armadaVictoryTurn = turn.TurnsFromNow;

			//if we want to recapture the planet the next turn how many ships do we need.
			//Since the enemy gets reinforces immediately after a capture, we are at a -GrowthRate disadvantage.
			int maxWorthItDistance;
			switch (planetUnderAttack.GrowthRate)
			{
				case 0:
				case 1: maxWorthItDistance = 5; break;
				case 2: maxWorthItDistance = 10; break;
				case 3:
				case 4: maxWorthItDistance = 15; break;
				default: maxWorthItDistance = 25; break;
			}

			//defend, remember, it's sorted on distance so the closest planets are considered first.
			foreach (Route route in planetUnderAttack.Routes)
			{
				if (route.DistanceInTurns <= Math.Min(maxTurnLookAhead, maxWorthItDistance))
				{
					//look for planet with defense capability
					Planet defender = route.Destination;
					if (defender.IsMine && planetUnderAttack.PlanetID != defender.PlanetID)
					{
						//is it close enough to contribute to the defense?
						int defenseTravelTime = (route.DistanceInTurns - 1);
						if (defenseTravelTime <= armadaVictoryTurn + 1)
						{
							//How many ships can we use
							int maxDelivarableShips = defender.AttackForce;
							if (defender.DoesNotChangeOwner)
							{
								int turnIndex = Math.Max(armadaVictoryTurn - defenseTravelTime, 0);
								maxDelivarableShips = defender.TurnPrediction[turnIndex].NumShips;
							}

							if (maxDelivarableShips > 0)
							{
								int prevRequired = defenseRequired;
								defenseRequired -= maxDelivarableShips;
								if (defender.NumShips > 0)
								{
									defensePlan.Enabled = true;
									int shipCount;
									if (defenseRequired > -1)
									{
										shipCount = Math.Min(defender.AttackForce, defender.NumShips);
										defensePlan.AddParticipant(defender, null, shipCount);
									}
									else
									{
										shipCount = Math.Min(Math.Min(defender.AttackForce, defender.NumShips), prevRequired);
										defensePlan.AddParticipant(defender, null, shipCount);
										break;
									}
								}
							}
						}
					}
				}
			}
			if (defenseRequired < 0)
			{

				planetUnderAttack.DoesNotChangeOwner = true;
				planetUnderAttack.AttackMovesAllowed = false;
				planetUnderAttack.IsLost = false;
			}

			//Planet is lost, no defense plan.
			if (planetUnderAttack.IsLost)
			{
				defensePlan.Participants.Clear();
			}
			return defensePlan;
		}
	}
}
