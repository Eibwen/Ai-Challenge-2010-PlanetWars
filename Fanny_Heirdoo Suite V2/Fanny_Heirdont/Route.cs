using System;


public class Route
{
	//TagetPlanet
	public int TagetPlanetId;

	// distance to the planet (equals the numTurns required to reach it)
	public double ActualDistance;
	public int DistanceInTurns;

	//How many ships get added to the target each turn>
	public int GrowthRate;

	// In the time it takes a ship to travel the distance, 
	// how many ship will have been added to the planet?
	// basically this is Distance * GrowtRate.
	// Since it's immutable we can store it here on the route.
	public int GrowthForTravelDistance;

	// Compare the growth rate of the planet to that of all other planets.
	// when = 1 it's exactly average, when greater, it grows quicker.
	public double RelativeGrowthRate;

	//compare the distance of this route to the average distance for all routes of the source planet.
	// if value == 1 it means the target is at exactly the average distance.
	// When greater, the target is closer than average (which could be more favorable)
	public double RelativePlanetDistance;

	// compare the distance of this route to the average distance from all routes from all planets
	// if value == 1 it means the target is at exactly the average distance for this universe.
	// When greater, the target is closer than average (which could be more favorable)
	public double RelativeUniverseDistance;

	public double TargetConnectiveNess { get; set; }
	public double SourceConnectiveNess { get; set; }
}
