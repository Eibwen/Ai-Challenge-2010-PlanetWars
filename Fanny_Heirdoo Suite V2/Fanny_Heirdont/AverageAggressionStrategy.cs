using System;
using System.Collections.Generic;


/// <summary>
/// Average Aggression Strategy
/// </summary>
public class AverageAggressionStrategy : AverageDefenderStrategy
{
	protected override bool OwnedPlanetCanSendAttackForce(Planet ownedPlanet)
	{
		return (ownedPlanet.AttackMovesAllowed || ownedPlanet.IsLost) && (ownedPlanet.AttackForce > 2);
	}


	protected override void StartOfTurn()
	{
		base.StartOfTurn();
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

			double wishListBias = 0;
			if (hostile.IsOnWishList)
			{
				//	wishListBias = 3;
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
				wishListBias +
				dominatePlanetBias +
				defenseBias +
				hostile.GrowthRate
				;

			attack.Enabled = true;
		}
	}

}
