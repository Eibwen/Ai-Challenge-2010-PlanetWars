using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class PlanetArmada : List<Fleet>
{
	public Planet PlanetUnderAttack;
	public PlanetArmada(Planet owner)
	{
		PlanetUnderAttack = owner;
	}

	public int EnemyFleetSize;
	public Fleet EnemyFirst;
	public Fleet EnemyLast;

	public int FriendlyFleetSize;
	public Fleet FriendlyFirst;
	public Fleet FriendlyLast;


 	//note does not take into account distance.
	internal int EffectiveFleetSizeForMe()
	{
		int size = FriendlyFleetSize - EnemyFleetSize;
		if (PlanetUnderAttack.IsMine)
		{
			//I can add my own ships to the count
			return size + PlanetUnderAttack.ShipCountAtStartOfTurn;
		}
		else
		{
			if (PlanetUnderAttack.IsEnemy)
			{
				//enemy is reinforcing himself
				return size - PlanetUnderAttack.ShipCountAtStartOfTurn;
			}
			else
			{
				//enemy is attacking, worst case
				return size + PlanetUnderAttack.ShipCountAtStartOfTurn;
			}
		}
	}
}

