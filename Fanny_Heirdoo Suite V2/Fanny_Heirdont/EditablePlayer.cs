using System.Collections.Generic;
using System;


internal class EditablePlayer
{
	public List<Planet> Planets = new List<Planet>();
	public List<Fleet> Fleets = new List<Fleet>();
	public List<int> Targets = new List<int>();
	public int ShipsInTransit;
	public int ShipsOnBase;
	public Quadrant ShipsHeavyPoint = new Quadrant();
}
