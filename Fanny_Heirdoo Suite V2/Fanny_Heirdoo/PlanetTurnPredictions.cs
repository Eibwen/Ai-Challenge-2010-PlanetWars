using System.Collections.Generic;
using System.Linq;
using System;

namespace FannyHeirdooBot
{
	public class PlanetTurnPredictions : List<PlanetTurn>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PlanetTurnPredictions"/> class.
		/// </summary>
		/// <param name="maxTurns">The max turns.</param>
		public PlanetTurnPredictions(int maxTurns)
		{
			PlanetTurn prior = null;
			do
			{
				PlanetTurn turn = new PlanetTurn(Count);
				if (prior != null)
				{
					prior.Next = turn;
				}
				turn.Prior = prior;
				prior = turn;
				Add(turn);
			} while (--maxTurns > -1);
			FirstTurn = this[0];
			LastTurn = this[Count - 1];
		}

		public int GrowthRate { get; private set; }
		public PlanetTurn FirstTurn { get; private set; }
		public PlanetTurn LastTurn { get; private set; }
		public PlanetTurn LastAttackTurn { get; private set; }

		public void ResetStats()
		{

		}
		public void CreateTurnPredictionsForPlanet(Planet planet)
		{
			int planetID = planet.PlanetID;
			GrowthRate = planet.GrowthRate;
			ResetStats();
			PlanetTurn lastCalculatedTurn = this[0];
			lastCalculatedTurn.SetValues(planet.Owner, planet.NumShips);
			planet.LastAttackTurn = lastCalculatedTurn;

			PlanetTurn currentCalculatingTurn = lastCalculatedTurn.Next;

			planet.MaxDesertersAllowed = planet.NumShips;
			planet.ShipsRequiredToSurviveAttack = planet.NumShips;
			planet.DoesNotChangeOwner = true;

			int currentTurn = 1;
			if (planetID == 15)
			{
			}
			if (planet.Armada.Count > 0)
			{
				planet.IdleForThisNumberOfTurns = 0;
				foreach (var turn in planet.FleetArrivalByTurnsRemaining)
				{
					do
					{
						lastCalculatedTurn = currentCalculatingTurn;
						currentCalculatingTurn = GrowTurn(GrowthRate, currentCalculatingTurn);
					} while (currentTurn++ < turn.Key);

					LastAttackTurn = lastCalculatedTurn;
					bool survived = Universe.FightBattleAndOwnerSurvived(lastCalculatedTurn, turn);
					if (lastCalculatedTurn.IsMine)
					{
						planet.MaxDesertersAllowed = Math.Min(planet.MaxDesertersAllowed, lastCalculatedTurn.NumShips);
					}
					if (!survived)
					{
						planet.DoesNotChangeOwner = false;
					}
				}
			}

			while (currentCalculatingTurn != null)
			{
				lastCalculatedTurn = currentCalculatingTurn;
				currentCalculatingTurn = GrowTurn(GrowthRate, currentCalculatingTurn);
			}
			planet.WinningArmadaIsMine = lastCalculatedTurn.Owner == 1;

			//int lastTurn = planet.FleetArrivalByTurnsRemaining.Last().Key;
			//lastAttackTurn = this[lastTurn];
			//for (int turnNumber = 1; turnNumber <= lastTurn; turnNumber++)
			//{
			//    lastCalculatedTurn = this[turnNumber];
			//    calc.Grow();
			//    lastCalculatedTurn.SetValues(calc.Owner, calc.NumShips);

			//    if (Universe.FightBattle(calc, turnNumber))
			//    {
			//        lastCalculatedTurn.SetValues(calc.Owner, calc.NumShips);
			//        planet.LastAttackTurn = lastCalculatedTurn;
			//        if (calc.IsMine)
			//        {
			//            planet.MaxDesertersAllowed = Math.Min(planet.MaxDesertersAllowed, calc.NumShips);
			//        }
			//        if (calc.IsEnemy)
			//        {
			//            planet.ShipsRequiredToSurviveAttack = Math.Max(planet.MaxDesertersAllowed, calc.NumShips + 1);
			//        }
			//        if (calc.IsNeutral)
			//        {
			//            planet.ShipsRequiredToSurviveAttack = Math.Min(planet.ShipsRequiredToSurviveAttack, calc.NumShips + 1);
			//        }
			//    }
			//    else
			//    {
			//        lastCalculatedTurn.SetValues(calc.Owner, calc.NumShips);
			//    }

			//    if (!calc.DoesNotChangeOwner)
			//    {
			//        planet.DoesNotChangeOwner = false;
			//    }
			//}
			 

		}

		private static PlanetTurn GrowTurn(int growthRate, PlanetTurn current)
		{
			PlanetTurn prior = current.Prior;
			if (prior != null)
			{
				current.SetValues(prior.Owner, prior.NumShips);
				current.Grow(growthRate);
			}
			return current.Next;
		}

		internal PlanetTurn CalcMaxGainUsingThisManyShips(PlanetTurn lowBoundFleetArrival, int attackShipCount, PlanetTurnPredictions prediction)
		{
			int targetTurn = lowBoundFleetArrival.TurnsFromNow;
			PlanetTurn current = FirstTurn;
			PlanetTurn copyTo = prediction.FirstTurn;
			while (targetTurn-- > 0)
			{
				copyTo.SetValues(current.Owner, current.NumShips);
				current = current.Next;
				copyTo = copyTo.Next;
			}
			current = copyTo;
			copyTo.Prior.SetValues(1, attackShipCount - current.NumShips);

			PlanetTurn prior = current;
			while (current != null)
			{
				prior = current;
				current = GrowTurn(GrowthRate, current);
			}
			return prior;
		}

		internal PlanetTurn FindCheapestTakeOverPoint(int shipsCanNotArriveBeforeThisTurn, PlanetTurnPredictions prediction)
		{
			return this[shipsCanNotArriveBeforeThisTurn].Clone();
		}
	}
}