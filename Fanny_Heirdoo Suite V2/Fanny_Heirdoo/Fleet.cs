using System;
namespace FannyHeirdooBot
{
	public class Fleet
	{
		// Initializes a fleet.
		public Fleet(int owner, int numShips, Planet sourcePlanet, Planet destinationPlanet, int totalTripLength, int turnsRemaining)
		{
			Owner = owner;
			NumShips = numShips;
			SourcePlanet = sourcePlanet;
			if (sourcePlanet != null)
			{
				SourcePlanetId = sourcePlanet.PlanetID;
			}
			DestinationPlanet = destinationPlanet;
			TotalTripLength = totalTripLength;
			TurnsRemaining = turnsRemaining;
			IsMine = owner == 1;

			if (destinationPlanet != null)
			{
				DestinationPlanetId = destinationPlanet.PlanetID;
				destinationPlanet.AddArmada(this);
				if (owner != 1 && destinationPlanet.Owner != this.Owner)
				{
					destinationPlanet.IsUnderAttack = true;
				}
			}
		}
		public void AddShips(int amount)
		{
			NumShips += amount;
		}
		public void RemoveShips(int amount)
		{
			NumShips -= amount;
		}

		public int Owner { get; private set; }
		public int NumShips { get; private set; }
		public int TotalTripLength { get; private set; }
		public int TurnsRemaining { get; private set; }
		public bool IsMine { get; private set; }

		public int SourcePlanetId { get; private set; }
		public int DestinationPlanetId { get; private set; }
		public Planet SourcePlanet { get; private set; }
		public Planet DestinationPlanet { get; private set; }


		// Subtracts one turn remaining. Call this function to make the fleet get
		// one turn closer to its destination.
		public void TimeStep()
		{
			if (TurnsRemaining > 0)
			{
				--TurnsRemaining;
			}
			else
			{
				TurnsRemaining = 0;
			}
		}
	}
}