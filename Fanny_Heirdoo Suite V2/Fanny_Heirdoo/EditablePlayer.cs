using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;

namespace FannyHeirdooBot
{
	internal class EditablePlayer
	{
		public List<Planet> Planets = new List<Planet>();
		public List<Fleet> Fleets = new List<Fleet>();
		public List<int> Targets = new List<int>();
		public int ShipsInTransit;
		public int ShipGrowth;
		public int ShipsOnBase;
		public Quadrant ShipsHeavyPoint = new Quadrant();
		public Quadrant AbsolutePlanetFocus = new Quadrant();
		public Quadrant ShipInTransitFocus = new Quadrant();

		public void InitializePlayer(Player player)
		{
			player.ShipOnPlanetFocus = ShipsHeavyPoint.Calculate(ShipsOnBase);
			player.AbsolutePlanetFocus = AbsolutePlanetFocus.Calculate(player.Planets.Count);
			player.ShipInTransitFocus = ShipInTransitFocus.Calculate(ShipsInTransit);
			player.ShipCountInBase = ShipsOnBase;
			player.ShipGrowth = ShipGrowth;
			player.ShipCountInTransit = ShipsInTransit;
		}

	}
}