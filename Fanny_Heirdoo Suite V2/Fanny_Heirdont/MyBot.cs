using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


public class FannyHeirdooBot
{
	internal static BotStrategy strategy = new AverageAggressionStrategy();
	/// <summary>
	/// Does the turn.
	/// </summary>
	/// <param name="uni">The uni.</param>
	public static void DoTurn(Universe uni)
	{
		ChooseStrategy(uni);
		List<AttackPlan> actionPlan = strategy.BuildActionPlan(uni);
		if (!SortAndMakeMoves(uni, actionPlan))
		{
			strategy.AttackPlanFailed();
		}
	}

	private static void ChooseStrategy(Universe uni)
	{
		if (Universe.IsDominating || Universe.TurnCount > 100)
		{
			if (strategy is DefensiveBotStrategy)
			{
				//strategy = new AverageAggressionStrategy();
			}
		}
	}

	private static bool SortAndMakeMoves(Universe uni, List<AttackPlan> sweetnesses)
	{
		return MakeSweetestMoves(uni, sweetnesses.OrderBy(item => -item.Sweetness));
	}

	static Random t = new Random();

	private static bool MakeSweetestMoves(Universe uni, IEnumerable<AttackPlan> attackPlan)
	{
		bool attackPerformed = false;
		Planet Defender = null;
		Planet agressor = null;
		foreach (var attack in attackPlan)
		{
			agressor = attack.Source;
			if (agressor.PlanetID == 2)
			{
			}
			if (agressor.AttackMovesAllowed)
			{
				if ((agressor.AttackForce >= attack.ShipCount && agressor.CanSurviveIncommingAttack) || agressor.IsLost)
				{
					Defender = attack.Target;
					bool domination = Universe.IsDominating || attack.DominationMove;
					if (Defender.IsAttackable || domination)
					{
						//create bias towards planets for which we initiate battle
						Universe.AddToWishList(Defender);

						uni.MakeMove(agressor, Defender, attack.ShipCount);
						attackPerformed = true;
					}
				}
			}
		}
		return attackPerformed;
	}

	public static void Main()
	{
		int c;
		try
		{

			StringBuilder input = new StringBuilder();
			StringBuilder line = new StringBuilder();

			while ((c = Console.Read()) >= 0)
			{
				switch (c)
				{
					case '\r':
						break;
					case '\n':
						string thisLine = line.ToString();
						if (thisLine.Equals("go"))
						{
							Universe uni = new Universe(input.ToString(), null);
							input.Length = 0;

							FannyHeirdooBot.DoTurn(uni);
							uni.FinishTurn();
						}
						else
						{
							input.Append(thisLine + "\n");
						}
						line.Length = 0;
						break;
					default:
						line.Append((char)c);
						break;
				}
			}
		}
		catch (Exception)
		{
			// Owned.
		}
	}
}

