using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FannyHeirdooBot
{

	public class Player
	{
		public Player(IList<Fleet> fleets, IList<Planet> planets, IList<int> targets)
		{
			Fleets = new ReadOnlyCollection<Fleet>(fleets);
			Planets = new ReadOnlyCollection<Planet>(planets);
		}
		public readonly ReadOnlyCollection<Planet> Planets;
		public readonly ReadOnlyCollection<Fleet> Fleets;
		public int ShipGrowth;
		public int ShipCountInBase;
		public int ShipCountInTransit;
		public int TotalShipCount
		{
			get { return ShipCountInBase + ShipCountInTransit; }
		}
		public Quadrant ShipOnPlanetFocus { get; set; }
		public Quadrant AbsolutePlanetFocus { get; set; }
		public Quadrant ShipInTransitFocus { get; set; }

		public IEnumerable<Planet> PlanetsUnderattack()
		{
			return Planets.Where(planet => planet.IsUnderAttack);
		}
	}
}