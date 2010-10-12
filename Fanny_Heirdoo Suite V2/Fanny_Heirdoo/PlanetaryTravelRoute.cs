using System;
using System.Collections.Generic;
using System.Linq;

namespace FannyHeirdooBot
{
	public class PlanetaryTravelRoute : List<Route>
	{
		public PlanetaryTravelRoute()
		{
			AverageDistancesByNumberOfPlanets = new List<double>();
			AverageConnectedGrowthByNumberOfPlanets = new List<double>();
		}
		public double SmallestDistance { get; set; }
		public double GreatestDistance { get; set; }
		//value will be lower for the more central planets
		public double DistanceRange { get; set; }
		public double AverageDistance { get; set; }
		public List<double> AverageDistancesByNumberOfPlanets { get; set; }
		public List<double> AverageConnectedGrowthByNumberOfPlanets { get; set; }

		public IOrderedEnumerable<IGrouping<int, Route>> PlanetsByNumberOfTurnsDistance { get; private set; }
		public void CreatePlanetsByNumberOfTurnsDistance()
		{
			PlanetsByNumberOfTurnsDistance = this.ToLookup(item => item.DistanceInTurns).OrderBy(item => item.Key);
		}

		public IEnumerable<Route> DistanceReversed()
		{
			return this.OrderBy(route => -route.ActualDistance);
		}

		internal Route LastRouteForPlayer(int playerId)
		{
			return this.Where(route => route.Destination.Owner == playerId)
				.OrderBy(route => route.DistanceInTurns).Last();
		}
	}
}