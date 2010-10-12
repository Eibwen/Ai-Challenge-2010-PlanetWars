using System;
using System.Collections.Generic;
using System.Linq;

public class PlanetaryTravelRouteDictionary : Dictionary<int, PlanetaryTravelRoute>
{
	/// <summary>
	/// Gets or sets the distance average.
	/// You can compare the average of a route with this value.
	/// That way you can determine if this planet is more or less centralized compared to the other planets.
	/// </summary>
	/// <value>The distance average.</value>
	public double AverageDistance { get; set; }

	#region Immutable statistic Growth rate
	public int GreatesGrowRate { get; set; }
	public int SmallestGrowRate { get; set; }
	public double GrowRateRange { get; set; }
	public double GrowRateAverage { get; set; }
	#endregion


	/// <summary>
	/// Grids the distance.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="destination">The destination.</param>
	/// <returns></returns>
	public static int GridDistance(Planet source, Planet destination)
	{
		return (int)Math.Ceiling(ActualDistance(source, destination));
	}

	public static double ActualDistance(Planet source, Planet destination)
	{
		double dx = source.X - destination.X;
		double dy = source.Y - destination.Y;
		double squared = dx * dx + dy * dy;
		return Math.Sqrt(squared);
	}


	/// <summary>
	/// Gets  all planets.
	/// </summary>
	/// <value>All planets.</value>
	private Dictionary<int, Planet> AllPlanetsOnPlanetId;

	public static PlanetaryTravelRouteDictionary Create(List<Planet> planets)
	{
		PlanetaryTravelRouteDictionary universe = new PlanetaryTravelRouteDictionary();

		universe.SmallestGrowRate = 9999;
		int totalGrowthRate = 0;
		double totalDistance = 0;
		double totalAvarageDistance = 0;

		int routeCount = planets.Count - 1;
		Dictionary<int, double> planetConnectiveness = new Dictionary<int, double>();
		foreach (Planet source in planets)
		{
			double totalDistanceForThisRoute = 0;
			PlanetaryTravelRoute routesFromThisPlanet = new PlanetaryTravelRoute();
			routesFromThisPlanet.SmallestDistance = 9999;
			List<Route> routesBuilder = new List<Route>();
			foreach (Planet dest in planets)
			{
				if (source.PlanetID != dest.PlanetID)
				{
					var newRoute = new Route();
					#region calculate statistics
					double distance = ActualDistance(source, dest);
					newRoute.ActualDistance = distance;
					newRoute.DistanceInTurns = GridDistance(source, dest);
					totalDistanceForThisRoute += distance;
					if (routesFromThisPlanet.SmallestDistance > distance)
					{
						routesFromThisPlanet.SmallestDistance = distance;
					}

					if (routesFromThisPlanet.GreatestDistance < distance)
					{
						routesFromThisPlanet.GreatestDistance = distance;
					}


					int growthrate = dest.GrowthRate;

					if (universe.SmallestGrowRate > growthrate)
					{
						universe.SmallestGrowRate = growthrate;
					}

					if (universe.GreatesGrowRate < growthrate)
					{
						universe.GreatesGrowRate = growthrate;
					}

					if (source.IsMine && dest.IsEnemy)
					{
						Universe.InitialEnemyFleetDistance = newRoute.DistanceInTurns;
					}

					newRoute.TagetPlanetId = dest.PlanetID;
					newRoute.GrowthRate = growthrate;
					newRoute.GrowthForTravelDistance = (int)((double)dest.GrowthRate * distance);
					
					routesBuilder.Add(newRoute);
					#endregion
				}
			}

			totalGrowthRate += source.GrowthRate;
			totalDistance += totalDistanceForThisRoute;
			routesFromThisPlanet.AddRange(routesBuilder.OrderBy(route => route.ActualDistance));
			//value will be lower for the more central planets
			routesFromThisPlanet.AverageDistance = totalDistanceForThisRoute / (double)routeCount;
			totalAvarageDistance += routesFromThisPlanet.AverageDistance;
			routesFromThisPlanet.DistanceRange = routesFromThisPlanet.GreatestDistance - routesFromThisPlanet.SmallestDistance;
			universe.Add(source.PlanetID, routesFromThisPlanet);
			
			double shortedRoutes = 0;
			int measureCount = routesFromThisPlanet.Count / 3;
			foreach (var route in routesFromThisPlanet.Take(measureCount))
			{
				shortedRoutes += route.ActualDistance;
			}
			routesFromThisPlanet.SourceConnectiveNess = shortedRoutes / measureCount;
			planetConnectiveness.Add(source.PlanetID, routesFromThisPlanet.SourceConnectiveNess);
		}

		universe.AverageDistance = totalAvarageDistance / universe.Count;
		universe.GrowRateAverage = totalGrowthRate / (double)routeCount;
		universe.GrowRateRange = universe.GreatesGrowRate - universe.SmallestGrowRate;

		foreach (PlanetaryTravelRoute allRoutesFromPlanet in universe.Values)
		{
			foreach (var route in allRoutesFromPlanet)
			{
				route.TargetConnectiveNess = planetConnectiveness[route.TagetPlanetId];
				route.SourceConnectiveNess = allRoutesFromPlanet.SourceConnectiveNess;
				//compare the distance of this route to the average distance from this planet
				route.RelativePlanetDistance = allRoutesFromPlanet.AverageDistance / route.ActualDistance;
				//compare the distance of this route to the average distance from all routes from all planets
				route.RelativeUniverseDistance = universe.AverageDistance / route.ActualDistance;
				//compare growth rate to all other planets.
				route.RelativeGrowthRate = route.GrowthRate / universe.GrowRateAverage;
			}
		}


		return universe;
	}

	private static void CreateDistanceIndex(List<Planet> planets, PlanetaryTravelRouteDictionary result)
	{
		result.AllPlanetsOnPlanetId = planets.ToDictionary(item => item.PlanetID);
	}


}
