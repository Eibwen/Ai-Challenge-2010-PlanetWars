using System;
using System.Collections.Generic;


/// <summary>
/// Average Aggression Strategy
/// </summary>
public class AverageDefenderStrategy : BotStrategy
{
	protected override bool OwnedPlanetCanSendAttackForce(Planet ownedPlanet)
	{
		return (ownedPlanet.AttackMovesAllowed || ownedPlanet.IsLost) && (ownedPlanet.AttackForce > 2);
	}

	protected override void CreateAttackPlanForOwnedPlanet(AttackPlan attack, Route attackRoute, PlanetaryTravelRoute planetTravelMap, ref bool continueRoute)
	{
		Planet hostile = attack.Target;
		Planet onwedPlanet = attack.Source;
		if (!hostile.IsMine)
		{
			//No use for planets that do not produce.
			if (hostile.GrowthRate == 0)
			{
				return;
			}

			// If my enemy is attacking this planet, but not doing this strong enough,
			// Do not attack this planet and let my enemies ships crash and burn.
			if (hostile.IsNeutral && hostile.IsUnderAttack && hostile.CanSurviveIncommingAttack && !hostile.IsOnWishList)
			{
				return;
			}
			double isNeutralBias = 0;
			double defenseBias = 0;
			if (hostile.IsNeutral)
			{
				isNeutralBias = 1;
				defenseBias = Math.Max(Math.Min(onwedPlanet.AttackForce / (hostile.NumShips + 1.0), 5), 0) / 2;
			}
			else
			{
				defenseBias = (Math.Max((int)Math.Min(onwedPlanet.AttackForce / (attackRoute.GrowthForTravelDistance + 1), 5), 0)) / 2.0;
			}

			//we are dominating, favor enemy.
			if (Universe.IsDominating)
			{
				if (hostile.IsNeutral)
				{
					isNeutralBias = -2;
				}
				else
				{
					isNeutralBias = 2;
				}
			}


			//Dominate planet?
			double dominatePlanetBias = 0.0;
			if (!hostile.IsNeutral)
			{
				if ((currentUniverse.Me.ShipCountInBase / 3 > attackRoute.GrowthForTravelDistance + hostile.NumShips)
					&& (attackRoute.RelativePlanetDistance < 0.3))
				{
					dominatePlanetBias = 2.72;
					attack.DominationMove = true;
				}
			}

			double growthBias = attackRoute.RelativeGrowthRate * 2;

			attack.ShipCount = (int)(onwedPlanet.NumShips * 0.6);

			attack.Sweetness =
				//favor closer planets
				attackRoute.RelativePlanetDistance +
				attackRoute.RelativeUniverseDistance +
				//favor stronger growing planets
				growthBias +
				//favor neutrals, they do not grow.
				isNeutralBias +

				dominatePlanetBias +
				defenseBias +
				hostile.GrowthRate
				;

			attack.Enabled = true;
		}
	}

	protected bool CanDefendNeutralPlanets;
	protected override void DefendNeutralPlanets(int maxTurnLookahead)
	{
		if (CanDefendNeutralPlanets)
		{
			foreach (Planet neutralPlanet in OrderNeutralAttackedPlanetsFirst())
			{
				#region Try defend any owned planets.
				bool shouldDefend = neutralPlanet.IsUnderAttack && !neutralPlanet.CanSurviveIncommingAttack;
				if (shouldDefend && !neutralPlanet.IsOnWishList)
				{
					IEnumerable<AttackPlan> defensePlan = BuildDefensePlan(neutralPlanet, maxTurnLookahead);
					if (!neutralPlanet.IsLost)
					{
						foreach (AttackPlan armada in defensePlan)
						{
							currentUniverse.MakeMove(armada.Source, armada.Target, armada.ShipCount);
						}
						neutralPlanet.AttackMovesAllowed = false;
					}
				}
				#endregion
			}
		}
	}

	protected override void DefendOnwedPlanets()
	{
		foreach (Planet onwedPlanet in OrderOwnAttackedPlanetsFirst())
		{
			#region Try defend any owned planets.
			bool shouldDefend = onwedPlanet.IsUnderAttack && !onwedPlanet.CanSurviveIncommingAttack;
			if (shouldDefend)
			{
				if (onwedPlanet.AttackForce > 0 && onwedPlanet.ShipsRequiredToSurviveAttack == 0)
				{
					shouldDefend = false;
				}
				else
				{

					IEnumerable<AttackPlan> defensePlan = BuildDefensePlan(onwedPlanet, 1000);
					if (onwedPlanet.IsLost)
					{
						foreach (var route in Universe.TravelMap[onwedPlanet.PlanetID])
						{
							if (route.DistanceInTurns <= onwedPlanet.LostAfterNumberOfTurns)
							{
								Planet target = currentUniverse.AllPlanetsOnPlanetId[route.TagetPlanetId];
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

						//Recapture !
						Universe.AddToWishList(onwedPlanet);
					}
					else
					{
						foreach (AttackPlan armada in defensePlan)
						{
							currentUniverse.MakeMove(armada.Source, armada.Target, armada.ShipCount);
							armada.Source.AttackMovesAllowed = false;
						}
						onwedPlanet.AttackMovesAllowed = false;
					}
				}
			}
			#endregion
		}
	}

	private IEnumerable<AttackPlan> BuildDefensePlan(Planet planetUnderAttack, int maxTurnLookAhead)
	{
		List<AttackPlan> resultBuilder = new List<AttackPlan>();
		planetUnderAttack.IsLost = true;
		int defenseRequired = planetUnderAttack.ShipsRequiredToSurviveAttack;
		//if we want to recapture the planet the next turn how many ships do we need.
		//Since the enemy gets reinforces immediately after a capture, we are at a -GrowthRate disadvantage.
		int recaptureArmadaStrength = -planetUnderAttack.GrowthRate;
		int armadaVictoryTurn = planetUnderAttack.LostAfterNumberOfTurns;
		//defend, remember, it's sorted on distance so the closest planets are considered first.
		foreach (Route route in Universe.TravelMap[planetUnderAttack.PlanetID])
		{
			if (route.DistanceInTurns <= maxTurnLookAhead)
			{
				//look for planet with defense capability
				Planet defender = currentUniverse.AllPlanetsOnPlanetId[route.TagetPlanetId];
				if (defender.IsMine && planetUnderAttack.PlanetID != defender.PlanetID)
				{
					//is it close enough to contribute to the defense?
					int defenseTravelTime = (route.DistanceInTurns - 1);
					if (defenseTravelTime <= armadaVictoryTurn)
					{
						//How many ships can we use
						int maxDelivarableShips;
						maxDelivarableShips = defender.AttackForce;
						if (defender.CanSurviveIncommingAttack)
						{
							maxDelivarableShips = defender.AttackForce;
							// How many turns left before reinforcements will not make it in time?
							// armadaVictoryTurn - turnsLeftForDefense
							maxDelivarableShips += (armadaVictoryTurn - defenseTravelTime) * defender.GrowthRate;
							//can we do a recapture, count the fleets we can use for a counter attack in the turn after capture.
							recaptureArmadaStrength += defender.GrowthRate;
						}


						if (maxDelivarableShips > 0)
						{
							int prevRequired = defenseRequired;
							defenseRequired -= maxDelivarableShips;
							if (defender.NumShips > 0)
							{
								AttackPlan armada = new AttackPlan();
								armada.Target = planetUnderAttack;
								armada.Source = defender;
								armada.Enabled = true;
								if (defenseRequired > -1)
								{
									armada.ShipCount = Math.Min(defender.AttackForce, defender.NumShips);
									resultBuilder.Add(armada);
								}
								else
								{
									armada.ShipCount = Math.Min(Math.Min(defender.AttackForce, defender.NumShips), maxDelivarableShips - prevRequired);
									resultBuilder.Add(armada);
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

			planetUnderAttack.CanSurviveIncommingAttack = true;
			planetUnderAttack.AttackMovesAllowed = false;
			planetUnderAttack.IsLost = false;
		}

		//Planet is lost, no defense plan.
		if (planetUnderAttack.IsLost)
		{
			resultBuilder.Clear();
		}

		return resultBuilder;
	}
}
