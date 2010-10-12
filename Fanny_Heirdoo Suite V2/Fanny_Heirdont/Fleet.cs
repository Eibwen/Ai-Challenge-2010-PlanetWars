using System;
public class Fleet
{
	// Initializes a fleet.
	public Fleet(int owner,
		 int numShips,
		 int sourcePlanet,
		 int destinationPlanet,
		 int totalTripLength,
		 int turnsRemaining)
	{
		this.owner = owner;
		this.numShips = numShips;
		this.sourcePlanet = sourcePlanet;
		this.destinationPlanet = destinationPlanet;
		this.totalTripLength = totalTripLength;
		this.turnsRemaining = turnsRemaining;
		IsMine = owner == 1;
	}

	/// <summary>
	/// Gets or sets a value indicating whether this instance is mine.
	/// </summary>
	/// <value><c>true</c> if this instance is mine; otherwise, <c>false</c>.</value>
	public bool IsMine { get; private set; }

	// Accessors and simple modification functions. These should be mostly
	// self-explanatory.
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

	public int SourcePlanet
	{
		get
		{
			return sourcePlanet;
		}
	}

	public int DestinationPlanet
	{
		get
		{
			return destinationPlanet;
		}
	}

	public int TotalTripLength
	{
		get
		{
			return totalTripLength;
		}
	}

	public int TurnsRemaining
	{
		get
		{
			return turnsRemaining;
		}
	}
	public void AddShips(int amount)
	{
		numShips += amount;
	}
	public void RemoveShips(int amount)
	{
		numShips -= amount;
	}

	// Subtracts one turn remaining. Call this function to make the fleet get
	// one turn closer to its destination.
	public void TimeStep()
	{
		if (turnsRemaining > 0)
		{
			--turnsRemaining;
		}
		else
		{
			turnsRemaining = 0;
		}
	}

	private int owner;
	private int numShips;
	private int sourcePlanet;
	private int destinationPlanet;
	private int totalTripLength;
	private int turnsRemaining;

	private Fleet(Fleet _f)
	{
		owner = _f.owner;
		numShips = _f.numShips;
		sourcePlanet = _f.sourcePlanet;
		destinationPlanet = _f.destinationPlanet;
		totalTripLength = _f.totalTripLength;
		turnsRemaining = _f.turnsRemaining;
	}
}
