using System;
using System.Collections.Generic;


public class AttackPlan
{
	public Planet Source;
	public Planet Target;
	public Double Distance;
	public bool DominationMove;
	/// <summary>
	/// Gets or sets the value.
	/// </summary>
	/// <value>The value.</value>
	public Double Sweetness;
	public int ShipCount;
	public bool Enabled;
}