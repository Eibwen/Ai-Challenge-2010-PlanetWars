using System.Collections.Generic;
using System.Collections.ObjectModel;



public class Player
{
	public Player(IList<Fleet> fleets, IList<Planet> planets, IList<int> targets)
	{
		Fleets = new ReadOnlyCollection<Fleet>(fleets);
		Planets = new ReadOnlyCollection<Planet>(planets);
	}
	public readonly ReadOnlyCollection<Planet> Planets;
	public readonly ReadOnlyCollection<Fleet> Fleets;
	public int ShipCountInBase;
	public int ShipCountInTransit;
	public int TotalShipCount
	{
		get { return ShipCountInBase + ShipCountInTransit; }
	}
}
