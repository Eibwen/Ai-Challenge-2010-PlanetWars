using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FannyHeirdooBot
{

	public class PlanetArmada : List<Fleet>
	{
		public Planet PlanetUnderAttack;
		public PlanetArmada(Planet owner)
		{
			PlanetUnderAttack = owner;
		}
		internal PlanetArmada Clone()
		{
			PlanetArmada result = new PlanetArmada(PlanetUnderAttack);
 
			result.AddRange(this);
			return result;
		}
	}
}