using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract class BotStrategy
{
	protected IEnumerable<Planet> OrderOwnAttackedPlanetsFirst()
	{
		return currentUniverse.Me.Planets
			.Where(planet => planet.IsUnderAttack)
			.OrderBy(planet => planet.CanSurviveIncommingAttack)
			.ThenBy(planet => planet.LostAfterNumberOfTurns);
	}

	protected List<AttackPlan> SortDefensePlan(List<AttackPlan> resultBuilder)
	{
		List<AttackPlan> result = resultBuilder.OrderBy(item => item.ShipCount).ToList();
		return result;
	}


	protected IEnumerable<Planet> OrderNeutralAttackedPlanetsFirst()
	{
		return currentUniverse.Neutral.Planets
			.Where(planet => planet.IsUnderAttack)
			.OrderBy(planet => planet.CanSurviveIncommingAttack)
			.ThenBy(planet => planet.LostAfterNumberOfTurns);
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

	public Planet FindNicestNeutral(int maxDefense, bool allowAllreadyAttacked)
	{
		Quadrant focus = currentUniverse.OwnShipFocus;
		Quadrant eFocus = currentUniverse.EnemyShipFocus;
		return currentUniverse.All.Planets
			.Where(planet => planet.IsNeutral
				  && planet.GrowthRate > 1
				  && planet.NumShips < maxDefense
				  && (allowAllreadyAttacked || !planet.IsUnderAttack))
			.OrderBy(planet => (int)focus.Delta(planet) - (eFocus.Delta(planet) / 2.0))
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
	protected abstract void DefendOnwedPlanets();
	protected abstract void DefendNeutralPlanets(int maxTurnLookahead);

	protected int MaxTurnLookaheadForNeutralDefense;

	public List<AttackPlan> BuildActionPlan(Universe uni)
	{
		currentUniverse = uni;
		StartOfTurn();
		DefendOnwedPlanets();
		DefendNeutralPlanets(MaxTurnLookaheadForNeutralDefense);
		List<AttackPlan> battleplan = new List<AttackPlan>();
		foreach (Planet onwedPlanet in currentUniverse.Me.Planets.Where(OwnedPlanetCanSendAttackForce))
		{
			if (SourcePlanetSelected(onwedPlanet))
			{
				var planetTravelMap = Universe.TravelMap[onwedPlanet.PlanetID];
				if (PlanetRouteSelected(onwedPlanet, planetTravelMap))
				{
					bool continueRoute = true;
					AttackPlan attack = new AttackPlan();
					foreach (Route route in planetTravelMap)
					{
						Planet hostile = currentUniverse.AllPlanetsOnPlanetId[route.TagetPlanetId];
						if (!hostile.IsMine)
						{
							attack.Distance = route.DistanceInTurns;
							attack.Source = onwedPlanet;
							attack.Target = hostile;
							attack.Enabled = false;

							CreateAttackPlanForOwnedPlanet(attack, route, planetTravelMap, ref continueRoute);

							if (attack.Enabled)
							{
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


	protected abstract void CreateAttackPlanForOwnedPlanet(AttackPlan attack, Route attackRoute, PlanetaryTravelRoute planetTravelMap, ref bool continueRoute);

	public virtual void AttackPlanFailed()
	{
	}
}

