using System;
using System.Collections.Generic;
using System.Text;

namespace botDebug
{
	public interface IPlanet
	{
		int PlanetID { get; }
		int Owner { get; }
		int NumShips { get; }
		int GrowthRate { get; }
		double X { get; }
		double Y { get; }
	}
}
