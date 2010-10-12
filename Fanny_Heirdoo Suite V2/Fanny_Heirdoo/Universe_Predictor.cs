using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FannyHeirdooBot
{
	public partial class Universe
	{
		//Resolves the battle at planet p, if there is one.
		//* Removes all fleets involved in the battle
		//* Sets the number of ships and owner of the planet according the outcome
		public static bool FightBattle(Planet planet, int turnNumber)
		{
			Dictionary<int, int> participants = new Dictionary<int, int>();
			participants.Add(planet.Owner, planet.NumShips);
			bool doBattle = false;

			foreach (Fleet fl in planet.Armada)
			{
				if (fl.TurnsRemaining == turnNumber)
				{
					int attackForce;
					doBattle = true;
					if (participants.TryGetValue(fl.Owner, out attackForce))
					{
						participants[fl.Owner] = attackForce + fl.NumShips;
					}
					else
					{
						participants.Add(fl.Owner, fl.NumShips);
					}
					//Fleets.Remove(fl);
				}
			}

			if (doBattle)
			{
				Fleet winner = new Fleet(0, 0, null, null, 0, 0);
				Fleet second = new Fleet(0, 0, null, null, 0, 0);
				foreach (var f in participants)
				{
					if (f.Value > second.NumShips)
					{
						if (f.Value > winner.NumShips)
						{
							second = winner;
							winner = new Fleet(f.Key, f.Value, null, null, 0, 0);
						}
						else
						{
							second = new Fleet(f.Key, f.Value, null, null, 0, 0);
						}
					}
				}

				if (winner.NumShips > second.NumShips)
				{
					planet.DoesNotChangeOwner = planet.Owner == winner.Owner;
					planet.ChangeOwner(winner.Owner, winner.NumShips - second.NumShips);

				}
				else
				{
					planet.ChangeOwner(planet.Owner, 0);
				}
			}
			return doBattle;
		}

		internal static bool FightBattleAndOwnerSurvived(PlanetTurn lastCalculatedTurn, IGrouping<int, Fleet> turn)
		{
			Dictionary<int, int> participants = new Dictionary<int, int>();
			participants.Add(lastCalculatedTurn.Owner, lastCalculatedTurn.NumShips);

			foreach (Fleet fl in turn)
			{
				int attackForce;
				if (participants.TryGetValue(fl.Owner, out attackForce))
				{
					participants[fl.Owner] = attackForce + fl.NumShips;
				}
				else
				{
					participants.Add(fl.Owner, fl.NumShips);
				}
			}


			Fleet winner = new Fleet(0, 0, null, null, 0, 0);
			Fleet second = new Fleet(0, 0, null, null, 0, 0);
			foreach (var fleet in participants)
			{
				if (fleet.Value > second.NumShips)
				{
					if (fleet.Value > winner.NumShips)
					{
						second = winner;
						winner = new Fleet(fleet.Key, fleet.Value, null, null, 0, 0);
					}
					else
					{
						second = new Fleet(fleet.Key, fleet.Value, null, null, 0, 0);
					}
				}
			}

			bool survived;
			if (winner.NumShips > second.NumShips)
			{
				survived = lastCalculatedTurn.Owner == winner.Owner;
				lastCalculatedTurn.SetValues(winner.Owner, winner.NumShips - second.NumShips);

			}
			else
			{
				survived = true;
				lastCalculatedTurn.SetValues(lastCalculatedTurn.Owner, 0);
			}
			return survived;
		}

		public void DeterminePlanetStrengthOnFleetArrival()
		{
			foreach (Planet defendingPlanet in All.PlanetsUnderattack())
			{
				PlanetTurn lastTurn = defendingPlanet.TurnPrediction.LastTurn;
				if (defendingPlanet.DoesNotChangeOwner)
				{
					//defendingPlanet.MaxDesertersAllowed = Math.Min(defendingPlanet.NumShips, lastTurn.NumShips);
				}
				else
				{
					//So it does change owner, but is the owner me?
					if (lastTurn.Owner == 1)
					{
						defendingPlanet.ShipsRequiredToSurviveAttack = -1;
						//If I did not own it from the start, I can send no ships.
						if (defendingPlanet.IsMine)
						{
							//So the part that's left standing does not have to be here.
							// (this is a too simplified prediction, improve later.
							defendingPlanet.MaxDesertersAllowed =
								Math.Max(0                                     // Negative value, not good for the engine.
								, Math.Min(defendingPlanet.MaxDesertersAllowed
								, lastTurn.NumShips                     // Do not send ships we do not have, will end game quickly.
							));
						}
						else
						{
							defendingPlanet.MaxDesertersAllowed = -1;
						}
					}
					else
					{
						//no, it is not mine, how many ships (at least) do we need to capture this?
						if (defendingPlanet.IsMine)
						{
							defendingPlanet.ShipsRequiredToSurviveAttack = lastTurn.NumShips + 1;
							defendingPlanet.MaxDesertersAllowed = -1;
						}
						else
						{
							if (defendingPlanet.WinningArmadaIsMine)
							{
							}
							else
							{
								defendingPlanet.MaxDesertersAllowed = 0;
								defendingPlanet.ShipsRequiredToSurviveAttack = defendingPlanet.LastAttackTurn.NumShips + 1;
							}
						}
					}
				}
			}
		}
	}
}