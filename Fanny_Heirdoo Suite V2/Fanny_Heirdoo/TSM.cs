using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using botDebug;

namespace FannyHeirdooBot.TSM
{
	public static class STMFinder
	{
		public static double DistanceInPlanets(IPlanet first, IPlanet other)
		{
			return (Math.Sqrt(Math.Pow(first.X - other.X, 2) +
			 Math.Pow(first.Y - other.Y, 2)));
		}

		public static int WorldGainPerTurn;

		public static IEnumerable<IPlanet> Start(List<IPlanet> planets)
		{
			//IEnumerable<Stop> stops = planets.Select(planet => new Stop(new City(planet))).NearestNeighbors().ToList();
			//IEnumerable<Stop> stops = planets.Select(planet => new Stop(new City(planet))).ToList();
			List<IPlanet> copy = new List<IPlanet>(planets);
			IPlanet startPlanet = copy[0];
			copy.RemoveAt(0);
			Stop startStop = new Stop(new City(startPlanet));

			//var sortedPlanets = copy.OrderBy(planet => -planet.GrowthRate).ThenBy(planet => DistanceInPlanets(startPlanet, planet)).Take(planets.Count / 2);
			var sortedPlanets = copy.OrderBy(planet => DistanceInPlanets(startPlanet, planet)).Take(planets.Count / 2);
			var cutPlanets = sortedPlanets.ToList().OrderBy(planet => -planet.GrowthRate);
			WorldGainPerTurn = 0;
			foreach (var planet in cutPlanets)
			{
				WorldGainPerTurn += planet.GrowthRate;
			}

			List<Stop> stops = cutPlanets.Select(planet => new Stop(new City(planet))).ToList();
			stops.Insert(0, startStop);
			//create next pointers between them 
			stops.Connect(true);

			//wrap in a tour object 
			Tour startingTour = new Tour(stops);
			System.Diagnostics.Debug.WriteLine("Start: " + startingTour);

			//the actual algorithm 
			while (true)
			{
				var newGeneratedTour = startingTour.GenerateMutations();
				var newTour = newGeneratedTour.MinBy(tour => tour.Cost());
				System.Diagnostics.Debug.WriteLine("new" + startingTour);
				if (newTour.Cost() < startingTour.Cost())
					startingTour = newTour;
				else
				{
					break;
				}
			}
			return GetPlanets(startingTour, planets);
		}

		private static IEnumerable<IPlanet> GetPlanets(Tour startingTour, IList<IPlanet> planets)
		{
			foreach (var stop in startingTour.Cycle())
			{
				yield return stop.City.Planet;
			}
		}

		//take an ordered list of nodes and set their next properties 
		public static void Connect(this IEnumerable<Stop> stops, bool loop)
		{
			Stop prev = null, first = null;
			foreach (var stop in stops)
			{
				if (first == null)
				{
					first = stop;
				}
				if (prev != null)
				{
					prev.Next = stop;
				}
				prev = stop;
			}

			if (loop)
			{
				prev.Next = first;
			}
		}

		//T with the smallest func(T) 
		public static T MinBy<T, TComparable>(this IEnumerable<T> xs, Func<T, TComparable> func)
		 where TComparable : IComparable<TComparable>
		{
			return xs.DefaultIfEmpty().Aggregate((maxSoFar, elem) =>
			 func(elem).CompareTo(func(maxSoFar)) > 0 ? maxSoFar : elem);
		}

		//return an ordered nearest neighbor set 
		public static IEnumerable<Stop> NearestNeighbors(this IEnumerable<Stop> stops)
		{
			var stopsLeft = stops.ToList();
			for (var stop = stopsLeft.First();
			 stop != null;
			 stop = stopsLeft.MinBy(s => STMFinder.DistanceWithDefense(stop, s)))
			{
				stopsLeft.Remove(stop);
				yield return stop;
			}
		}

		public static double DistanceWithDefense(Stop first, Stop other)
		{
			return (Math.Sqrt(Math.Pow(first.City.X - other.City.X, 2) +
			 Math.Pow(first.City.Y - other.City.Y, 2)));

			//+ (first.City.Defense - other.City.Defense) / 10);

			// (first.City.Gain + other.City.Gain);
		}

	}


	public class City
	{
		private static Random rand = new Random();

		public City(IPlanet planet)
		{
			X = planet.X;
			Y = planet.Y;
			CityName = planet.PlanetID;
			Gain = planet.GrowthRate;
			Defense = planet.NumShips;
			Planet = planet;
		}

		public IPlanet Planet { get; private set; }
		public int Defense { get; private set; }
		public int Gain { get; private set; }
		public double X { get; private set; }
		public double Y { get; private set; }
		public int CityName { get; private set; }
	}

	public class Stop
	{
		public Stop(City city)
		{
			City = city;
		}

		private Stop _Next;
		public Stop Next
		{
			get
			{
				return _Next;
			}
			set
			{
				if (_Next == value)
				{
				}
				else
				{
					if (_Next != null && _Next.Prior == this)
					{
						_Next.Prior = null;
					}
					if (value != null)
					{
						value.Prior = this;
					}
					_Next = value;

					if (_Next != null)
					{
						NextDistance = Tour.Distance(Next, this);
					}
					if (Prior != null)
					{
						PriorDistance = Tour.Distance(Prior, this);
					}
				}
			}
		}
		public Stop Prior { get; private set; }

		public Double NextDistance { get; set; }
		public Double PriorDistance { get; set; }

		public City City { get; set; }

		public Stop Clone()
		{
			return new Stop(City);
		}

		//list of nodes, including this one, that we can get to 
		public IEnumerable<Stop> CanGetTo()
		{
			var current = this;
			while (true)
			{
				yield return current;
				current = current.Next;
				if (current == this)
					break;
			}
		}

		public override bool Equals(object obj)
		{
			return City == ((Stop)obj).City;
		}

		public override int GetHashCode()
		{
			return City.GetHashCode();
		}

		public override string ToString()
		{
			return City.CityName.ToString();
		}
	}

	public class Tour
	{
		public Tour(IEnumerable<Stop> stops)
		{
			Anchor = stops.First();

		}

		//the set of tours we can make with 2-opt out of this one 
		public IEnumerable<Tour> GenerateMutations()
		{
			for (Stop stop = Anchor; stop.Next != Anchor; stop = stop.Next)
			{
				//skip the next one, since you can't swap with that 
				Stop current = stop.Next.Next;
				while (current != Anchor)
				{
					Tour cloneWithSwap = CloneWithSwap(stop.City, current.City);
					yield return cloneWithSwap;
					current = current.Next;
				}
			}
		}

		public Stop Anchor { get; set; }

		public Tour CloneWithSwap(City firstCity, City secondCity)
		{
			Stop firstFrom = null, secondFrom = null;
			var stops = UnconnectedClones();
			stops.Connect(true);

			foreach (Stop stop in stops)
			{
				if (stop.City == firstCity)
					firstFrom = stop;

				if (stop.City == secondCity)
					secondFrom = stop;
			}

			//the swap part 
			var firstTo = firstFrom.Next;
			var secondTo = secondFrom.Next;

			//reverse all of the links between the swaps 
			firstTo.CanGetTo()
			 .TakeWhile(stop => stop != secondTo)
			 .Reverse()
			 .Connect(false);

			firstTo.Next = secondTo;
			firstFrom.Next = secondFrom;
			var tour = new Tour(stops);
			return tour;
		}

		public static double Distance(Stop first, Stop other)
		{
			return Math.Sqrt(Math.Pow(first.City.X - other.City.X, 2) +
			 Math.Pow(first.City.Y - other.City.Y, 2));
		}


		public IList<Stop> UnconnectedClones()
		{
			return Cycle().Select(stop => stop.Clone()).ToList();
		}

		public double Cost()
		{
			double backtrackDistance = 0;
			return Cycle().Aggregate(0.0, (sum, stop) =>
				   {
					   if (stop != Anchor)
					   {
						   backtrackDistance += stop.PriorDistance;
					   }

					   return sum + CostForGrowth(backtrackDistance, stop);
				   });
		}

		private double CostForGrowth(double turnNumber, Stop stop)
		{
			//System.Diagnostics.Debug.WriteLine(stop.City.Planet.PlanetID + " -> " + stop.Next.City.Planet.PlanetID + " " + stop.NextDistance + " " );
			//return (stop.NextDistance + (stop.Next.City.Defense / 10)) / stop.Next.City.Gain;
			//return stop.NextDistance;
			double cost = TSM.STMFinder.WorldGainPerTurn * turnNumber;
			double gain = 0;

			if (stop != Anchor)
			{
				double backtrackDistance = 0;
				Stop backtrack = stop;
				do
				{
					backtrack = backtrack.Prior;
					backtrackDistance += (int)(backtrack.NextDistance + 0.5);
					gain += (int)(backtrack.City.Gain * backtrackDistance);
				}
				while (backtrack != Anchor);
			}

			return cost - gain + ((6 - stop.Next.City.Gain) * stop.Next.NextDistance);
		}

		public IEnumerable<Stop> Cycle()
		{
			return Anchor.CanGetTo();
		}

		public override string ToString()
		{
			string path = String.Join("->",
			 Cycle().Select(stop => stop.ToString()).ToArray());
			return String.Format("Cost: {0}, Path:{1}", Cost(), path);
		}

	}


}


