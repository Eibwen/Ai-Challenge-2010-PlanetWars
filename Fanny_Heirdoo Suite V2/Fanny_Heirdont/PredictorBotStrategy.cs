using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class VeryAgressiveBotStrategy : AverageDefenderStrategy
{
	protected override bool OwnedPlanetCanSendAttackForce(Planet ownedPlanet)
	{
		return ownedPlanet.AttackForce > 2;
	}

	protected override void StartOfTurn()
	{
		base.StartOfTurn();
		FindStrongestEnemyPlanet();
		FindWeakestEnemyPlanetWithGrowth();
	}

	protected override bool SourcePlanetSelected(Planet onwedPlanet)
	{
		currentUniverse.MakeMove(onwedPlanet, WeakestEnemyPlanetWithGrowth, 1);
		currentUniverse.MakeMove(onwedPlanet, StrongestEnemyPlanet, 1);
		return true;
	}
}

