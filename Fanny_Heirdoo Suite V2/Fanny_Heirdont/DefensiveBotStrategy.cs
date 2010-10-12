using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
	public class DefensiveBotStrategy : AverageDefenderStrategy
	{
		protected override bool OwnedPlanetCanSendAttackForce(Planet ownedPlanet)
		{
			return ownedPlanet.AttackForce > 2;
		}

		protected bool BackupPlanEnabled;

		int SuperDefenseCutOffPoint = 10;
		protected override void StartOfTurn()
		{
			base.StartOfTurn();

			SuperDefenseCutOffPoint = (int)(100.0 / (Universe.InitialEnemyFleetDistance * 1.2));
			BackupPlanEnabled = Universe.TurnCount > SuperDefenseCutOffPoint;
			CanDefendNeutralPlanets = BackupPlanEnabled || Universe.InitialEnemyFleetDistance> 10.0; 
			MaxTurnLookaheadForNeutralDefense = 20;

			FindStrongestEnemyPlanet();
			FindWeakestEnemyPlanetWithGrowth();
		}


		Route NearestEnemy;
		//worst case prediction assuming everything is pushed to one planet.
		protected Planet WorstCasePrediction(Planet onwedPlanet, PlanetaryTravelRoute routesFromPlanet)
		{
			if (onwedPlanet.PlanetID == 1)
			{ }
			NearestEnemy = null;
			int lastTurn = 0;
			//copy, can be modified and discarded !
			Planet defender = onwedPlanet.Clone();
			Planet attacker = null;

			// the number of chips all planets can push towards this planet each round
			// Start with a negative value to compensate for the growth rate
			int attackValuePerRound = -defender.GrowthRate;

			// will be decreased by attackValuePerRound  each round.
			int defendingForce = defender.AttackForce;

			foreach (var routes in PlanetRouteByDistanceDictionary(routesFromPlanet))
			{
				//difference with previous turn?
				int turnDiff = routes.Key - lastTurn;
				defendingForce -= turnDiff * attackValuePerRound;

				foreach (var route in routes)
				{
					if (route.TagetPlanetId != defender.PlanetID)
					{
						attacker = currentUniverse.AllPlanetsOnPlanetId[route.TagetPlanetId];
						//owner 0 can not attack,
						if (attacker.Owner > 0)
						{
							if (attacker.Owner == defender.Owner)
							{
								defendingForce += (int)attacker.NumShips / 2;
								attackValuePerRound -= attacker.GrowthRate;
							}
							else
							{
								if (NearestEnemy == null)
								{
									NearestEnemy = route;
								}
								attackValuePerRound += attacker.GrowthRate;
								defendingForce -= attacker.NumShips;
							}
						}
					}
				}

				//did we survive?
				if (defendingForce > 0)
				{

					//So the part that's left standing does not have to be here.
					defender.MaxDesertersAllowed = Math.Max(Math.Min(defendingForce, defender.AttackForce), 0);
					lastTurn = routes.Key;
				}
				else
				{
					//did not survive, the planet is lost
					defender.LostAfterNumberOfTurns = routes.Key;
					defender.ShipsRequiredToSurviveAttack = (-defendingForce) + 3;
					defender.CanSurviveIncommingAttack = false;
					break;
				}
			}
			return defender;
		}

		Planet PlanetWorstCasePrediction;
		protected override bool PlanetRouteSelected(Planet onwedPlanet, PlanetaryTravelRoute routesFromPlanet)
		{
			PlanetWorstCasePrediction = WorstCasePrediction(onwedPlanet, routesFromPlanet);
			int diff = onwedPlanet.NumShips - PlanetWorstCasePrediction.MaxDesertersAllowed;
			onwedPlanet.RemoveShips(diff);
			return true;
		}


		protected override void CreateAttackPlanForOwnedPlanet(AttackPlan attack, Route attackRoute, PlanetaryTravelRoute planetTravelMap, ref bool continueRoute)
		{
			Planet myPlanet = attack.Source;
			Planet hostile = attack.Target;
			if (Universe.TurnCount > 1 && hostile.GrowthRate > 0)
			{
				//check to see if we need our strength (and leave some)
				if (myPlanet.IsUnderAttack && myPlanet.CanSurviveIncommingAttack && myPlanet.AttackForce < 10)
				{
					return;
				}
				if (myPlanet.PlanetID == 2)
				{
					if (hostile.PlanetID == 17)
					{
					}
				}

				if (Universe.TurnCount < SuperDefenseCutOffPoint)
				{
					//VERY aggressive move, putting a great army very near me.
					if ((attackRoute.DistanceInTurns < 5)
						&& (hostile.IsUnderAttack)
						&& ((double)hostile.Armada.EnemyFleetSize / ((double)myPlanet.AttackForce + 1) > 0.5))
					{
						//Counter with aggressive defense, I will capture the planet
						// how many ships needed to block enemy?
						int shipsNeeded = Math.Max(hostile.Armada.EnemyFleetSize - hostile.Armada.FriendlyFleetSize, hostile.NumShips + attackRoute.GrowthForTravelDistance);
						currentUniverse.MakeMove(myPlanet, hostile, shipsNeeded + 1);
						continueRoute = false;
						return;
					}

					//Close quarters fighting is special. Can I kill him early?
					if (hostile.IsEnemy && (attackRoute.GrowthForTravelDistance + hostile.NumShips) < myPlanet.ShipCountAtStartOfTurn)
					{
						if (myPlanet.IsUnderAttack)
						{
							// Do not lose the planet, be defensive.
							continueRoute = false;
							return;
						}
						else
						{
							if (hostile.CanSurviveIncommingAttack)
							{
								//He is still healthy, attack
								int max = attackRoute.GrowthForTravelDistance + hostile.NumShips + hostile.GrowthRate;
								currentUniverse.MakeUnsafeMove(myPlanet, hostile, Math.Min(max, myPlanet.NumShips));
								continueRoute = false;
								return;
							}
							//ok, so this planet is beat. next please.
						}

					}

					if (NearestEnemy == null || attackRoute.ActualDistance > NearestEnemy.ActualDistance)
					{
						return;
					}
				}

				if (Universe.IsDominating)
				{
					if (!myPlanet.IsUnderAttack)
					{
						if (hostile.CanSurviveIncommingAttack)
						{
							attack.ShipCount = (int)(myPlanet.AttackForce * 0.9);
							attack.Enabled = true;

							if (hostile.IsNeutral)
							{
								attack.Sweetness = (double)hostile.GrowthRate - 2;
							}
							if (hostile.IsEnemy)
							{
								attack.Sweetness = (double)hostile.GrowthRate;
							}
						}
					}
				}
				else
				{

					bool isNear = attackRoute.ActualDistance < Universe.TravelMap.AverageDistance;
					if (myPlanet.PlanetID == 7)
					{
						if (hostile.PlanetID == 14)
						{
						}
					}

					double ConnectedNess = Universe.TravelMap.AverageDistance / attackRoute.TargetConnectiveNess;


					if (isNear || Universe.IsDominating)
					{

						double powerbase = hostile.GrowthRate;
						if (hostile.IsNeutral)
							powerbase += 0.5;
						attack.Sweetness = (Math.Pow(powerbase, ConnectedNess) / (attackRoute.ActualDistance));

						attack.ShipCount = myPlanet.AttackForce;

						if (hostile.IsEnemy)
						{
							bool fleetStrongrThanPlanet = attackRoute.GrowthForTravelDistance + hostile.NumShips < myPlanet.AttackForce;
							if (fleetStrongrThanPlanet || hostile.IsOnWishList)
							{
								//attack.Sweetness = -(hostile.NumShips + (hostile.GrowthRate * attackRoute.Distance));
								//attack.Sweetness = Math.Pow(hostile.GrowthRate, 3) / Math.Pow(attackRoute.DistanceInTurns, 1.5);

								attack.Enabled = true;
							}
						}
						else
						{
							if (Universe.IsDominating || hostile.Armada.EffectiveFleetSizeForMe() < myPlanet.AttackForce || hostile.IsOnWishList)
							{
								//attack.ShipCount = (int)(hostile.NumShips * 1.10);
								attack.Enabled = true;
							}
						}

						System.Diagnostics.Debug.WriteLine(
"(" + myPlanet.PlanetID + ") " + attack.ShipCount + " ==> (" + hostile.PlanetID + ") C: " + ConnectedNess + "  D: (" + attackRoute.ActualDistance + ") Like:" + attack.Sweetness);

					}
				}
			}
		}

		protected override void PlanetRouteDeSelected(Planet onwedPlanet, PlanetaryTravelRoute planetTravelMap)
		{
			base.PlanetRouteDeSelected(onwedPlanet, planetTravelMap);
			if (Universe.IsDominating)
			{
			}
			else
			{
				if (onwedPlanet.CanSurviveIncommingAttack)
				{
					if (!PlanetWorstCasePrediction.CanSurviveIncommingAttack)
					{
						//determine defensive action.
					}
				}
			}
		}

		public override void AttackPlanFailed()
		{
			Planet nice = null;
			bool allow = false;
			if (Universe.TurnCount < SuperDefenseCutOffPoint)
			{
				nice = FindNicestNeutral(Universe.InitialEnemyFleetDistance * 5, false);
				allow = true;
			}
			else
			{
				if (BackupPlanEnabled)
				{
					nice = FindNicestNeutral(10000, true);
					if (nice == null)
					{
						nice = FindNicestNeutral(10000, false);
					}
					//this happens when the enemy occupies a planet without growth
					if (nice == null && NearestEnemy != null)
					{
						nice = currentUniverse.AllPlanetsOnPlanetId[NearestEnemy.TagetPlanetId];
					}
				}
			}
			if (nice != null)
			{
				foreach (Planet attacker in currentUniverse.Me.Planets)
				{
					if (attacker.CanSurviveIncommingAttack &&
						(allow || attacker.MaxDesertersAllowed > 0))
					{
						currentUniverse.MakeMove(attacker, nice, (int)((double)attacker.AttackForce * 0.1));
					}
				}
			}
		}
	}

