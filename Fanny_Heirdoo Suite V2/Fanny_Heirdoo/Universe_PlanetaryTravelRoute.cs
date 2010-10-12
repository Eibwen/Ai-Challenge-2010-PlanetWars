using System;
using System.Collections.Generic;
using System.Linq;

namespace FannyHeirdooBot
{
	public partial class Universe
	{
		/// <summary>
		/// Gets or sets the distance average.
		/// You can compare the average of a route with this value.
		/// That way you can determine if this planet is more or less centralized compared to the other planets.
		/// </summary>
		/// <value>The distance average.</value>
		public double AverageDistance { get; set; }

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
		public static Quadrant Center = new Quadrant();
		public void CreatePlanetaryTravelRoutes()
		{
			Center = new Quadrant();
			int totalGrowthRate = 0;
			double totalAvarageDistance = 0;

			int routeCount = Planets.Count;
			foreach (Planet source in Planets.Values)
			{
				Center.X += source.X;
				Center.Y += source.Y;
				PlanetaryTravelRoute buildingRoute = source.Routes;
				buildingRoute.SmallestDistance = 9999;
				totalGrowthRate += source.GrowthRate;


				List<Route> routesBuilder = new List<Route>();
				foreach (Planet dest in Planets.Values)
				{
					if (source.PlanetID != dest.PlanetID)
					{
						var newRoute = new Route(source, dest);

						#region calculate statistics

						double actualDistance = ActualDistance(source, dest);
						newRoute.ActualDistance = actualDistance;
						newRoute.DistanceInTurns = GridDistance(source, dest);

						LargestDistanceInTurns = Math.Max(newRoute.DistanceInTurns, LargestDistanceInTurns);

						#region Smallest and greates distance
						if (buildingRoute.SmallestDistance > actualDistance)
						{
							buildingRoute.SmallestDistance = actualDistance;
						}

						if (buildingRoute.GreatestDistance < actualDistance)
						{
							buildingRoute.GreatestDistance = actualDistance;
						}
						#endregion

						#endregion

						routesBuilder.Add(newRoute);

						if (source.IsMine && dest.IsEnemy)
						{
							InitialEnemyFleetDistance = newRoute.DistanceInTurns;
						}
						newRoute.DestinationStateOnArrival = newRoute.Destination.TurnPrediction[newRoute.DistanceInTurns];
					}

				}
				double distance = 0;
				double distanceDevider = 0;
				double growthdistance = 0;
				foreach (Route route in routesBuilder.OrderBy(route => route.ActualDistance))
				{
					distanceDevider++;
					distance += route.ActualDistance;
					source.Routes.Add(route);
					source.Routes.AverageDistancesByNumberOfPlanets.Add(distance / distanceDevider);

					growthdistance += route.GrowthRate;
					source.Routes.AverageConnectedGrowthByNumberOfPlanets.Add(growthdistance / distanceDevider);
				}
				//value will be lower for the more central planets
				buildingRoute.AverageDistance = distance / distanceDevider;
				source.Connectiveness = buildingRoute.AverageDistance / source.Routes.AverageDistancesByNumberOfPlanets[6];
				source.Growthyness = source.Routes.AverageConnectedGrowthByNumberOfPlanets[6];

				totalAvarageDistance += buildingRoute.AverageDistance;
				buildingRoute.DistanceRange = buildingRoute.GreatestDistance - buildingRoute.SmallestDistance;

				source.Routes.Capacity = source.Routes.Count;
				source.Routes.CreatePlanetsByNumberOfTurnsDistance();
			}

			AverageDistance = totalAvarageDistance / Planets.Count;

			Center.X = Center.X / Planets.Count;
			Center.Y = Center.Y / Planets.Count;
		}

		public int LargestDistanceInTurns { get; set; }
	}
}