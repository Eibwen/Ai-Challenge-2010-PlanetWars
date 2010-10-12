using System;
using System.Collections.Generic;
using System.Linq;

public class PlanetaryTravelRoute : List<Route>
{
	public double SmallestDistance { get; set; }
	public double GreatestDistance { get; set; }
	//value will be lower for the more central planets
	public double DistanceRange { get; set; }
	//value will be lower for the more central planets
	public double AverageDistance { get; set; }

	public double SourceConnectiveNess { get; set; }

	public IEnumerable<Route> DistanceReversed()
	{
		return this.OrderBy(route => -route.ActualDistance);
	}
}
