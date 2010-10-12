using System;
public class Planet
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
		ShipCountAtStartOfTurn = numShips;
		this.growthRate = growthRate;
		this.x = x;
		this.y = y;
		MaxDesertersAllowed = -1;
		IsMine = owner == 1;
		IsNeutral = owner == 0;
		IsEnemy = !IsMine && !IsNeutral;

		Armada = new PlanetArmada(this);

		CanSurviveIncommingAttack = true;
		AttackMovesAllowed = true;
	}
	public int ShipCountAtStartOfTurn;
	public bool IsAttackable
	{
		get
		{
			if (IsMine)
			{
				return false;
			}
			else
			{
				return NumShips > -1;
			}
		}
	}

	public PlanetArmada Armada { get; private set; }

	internal void AddArmada(Fleet armada)
	{
		if (armada.Owner == 1)
		{
			Armada.FriendlyFleetSize += armada.NumShips;
			if (Armada.FriendlyFirst == null ||
				Armada.FriendlyFirst.TurnsRemaining > armada.TurnsRemaining)
			{
				Armada.FriendlyFirst = armada;
			}
			if (Armada.FriendlyLast == null ||
				Armada.FriendlyLast.TurnsRemaining < armada.TurnsRemaining)
			{
				Armada.FriendlyLast = armada;
			}
		}
		if (armada.Owner == 2)
		{
			Armada.EnemyFleetSize += armada.NumShips;
			if (Armada.EnemyFirst == null ||
				Armada.EnemyFirst.TurnsRemaining > armada.TurnsRemaining)
			{
				Armada.EnemyFirst = armada;
			}
			if (Armada.EnemyLast== null ||
				Armada.EnemyLast.TurnsRemaining < armada.TurnsRemaining)
			{
				Armada.EnemyLast = armada;
			}
		}
		Armada.Add(armada);
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

	/// <summary>
	/// Gets the attack force.
	/// </summary>
	/// <value>The attack force.</value>
	public int AttackForce
	{
		get
		{
			if (MaxDesertersAllowed > -1)
			{
				return MaxDesertersAllowed;
			}
			else
			{
				return NumShips;
			}
		}
	}

	/// <summary>
	/// Removes the ships.
	/// </summary>
	/// <param name="amount">The amount.</param>
	public void RemoveShips(int amount)
	{
		MaxDesertersAllowed -= amount;
		if (MaxDesertersAllowed < 0)
		{
			MaxDesertersAllowed = -1;
		}
		numShips -= amount;
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

	/// <summary>
	/// Changes the owner.
	/// </summary>
	/// <param name="newOwner">The new owner.</param>
	/// <param name="newShipCount">The new ship count.</param>
	public void ChangeOwner(int newOwner, int newShipCount)
	{
		owner = newOwner;
		numShips = newShipCount;
	}

	public void AddShips(int amount)
	{
		numShips += amount;
	}

	public bool IsIdled;
	public bool IsLost;
	public bool IsUnderAttack;
	public bool CanSurviveIncommingAttack;
	public int LostAfterNumberOfTurns;
	public int ShipsRequiredToSurviveAttack;
	public int MaxDesertersAllowed;
	private int planetID;
	private int owner;
	private int numShips;
	private int growthRate;
	private double x, y;

	public bool IsMine { get; private set; }
	public bool IsNeutral { get; set; }
	public bool IsEnemy { get; set; }
	public bool IsOnWishList { get; set; }
	public double RelativeFleetStrength { get; set; }

	public bool AttackMovesAllowed;

	internal Planet Clone()
	{
		Planet result = new Planet(planetID, owner, numShips, growthRate, x, y);
		result.IsIdled = IsIdled;
		result.IsLost = IsLost;
		result.IsUnderAttack = IsIdled;
		result.CanSurviveIncommingAttack = CanSurviveIncommingAttack;
		result.LostAfterNumberOfTurns = LostAfterNumberOfTurns;
		result.ShipsRequiredToSurviveAttack = ShipsRequiredToSurviveAttack;
		result.MaxDesertersAllowed = MaxDesertersAllowed;
		result.IsNeutral = IsNeutral;
		result.IsEnemy = IsEnemy;
		result.IsOnWishList = IsOnWishList;
		result.RelativeFleetStrength = RelativeFleetStrength;
		return result;
	}
}

