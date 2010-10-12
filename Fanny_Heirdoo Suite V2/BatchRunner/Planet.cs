
using botDebug;

namespace BatchRunner
{
	/// <summary>
	/// Almost straight from the original packages
	/// but converted some methods to properties as I got sick of typing all those ()
	/// </summary>
	public class Planet : IPlanet
	{
		// Initializes a planet.
		public Planet(int planetID,
					  int owner,
			  int numShips,
			  int growthRate,
			  double x,
			  double y)
		{
			this.planetID = planetID;
			this.owner = owner;
			this.numShips = numShips;
			this.growthRate = growthRate;
			this.x = x;
			this.y = y;
		}

		// Accessors and simple modification functions. These should be mostly
		// self-explanatory.
		public int PlanetID
		{
			get
			{
				return planetID;
			}
		}

		public int Owner
		{
			get
			{
				return owner;
			}
		}

		public int NumShips
		{
			get
			{
				return numShips;
			}
		}

		public int GrowthRate
		{
			get
			{
				return growthRate;
			}
		}

		public double X
		{
			get
			{
				return x;
			}
		}

		public double Y
		{
			get
			{
				return y;
			}
		}

		public void SetOwner(int newOwner)
		{
			this.owner = newOwner;
		}

		public void SetNumShips(int newNumShips)
		{
			this.numShips = newNumShips;
		}

		public void AddShips(int amount)
		{
			numShips += amount;
		}

		public void RemoveShips(int amount)
		{
			numShips -= amount;
		}

		private int planetID;
		private int owner;
		private int numShips;
		private int growthRate;
		private double x, y;

		private Planet(Planet _p)
		{
			planetID = _p.planetID;
			owner = _p.owner;
			numShips = _p.numShips;
			growthRate = _p.growthRate;
			x = _p.x;
			y = _p.y;
		}

		internal Planet clone()
		{
			return new Planet(this);
		}

	}
}