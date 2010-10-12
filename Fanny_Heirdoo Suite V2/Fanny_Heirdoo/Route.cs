using System;

namespace FannyHeirdooBot
{
	public class Route
	{
		public Planet Destination { get; set; }
		public PlanetTurn DestinationStateOnArrival;
		public Planet Source { get; set; }

		//How many ships get added to the target each turn>
		public int GrowthRate; 

		public Route(Planet source, Planet destination)
		{
			Source = source;
			Destination = destination; 
		}

		// distance to the planet (equals the numTurns required to reach it)
		public double ActualDistance;
		public int DistanceInTurns;
	}
}