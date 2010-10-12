using System;
using DefaultPackage;

public class RageBot
{
	public static void DoTurn(PlanetWars pw)
	{
		foreach (Planet source in pw.MyPlanets())
		{
			if (source.NumShips() < 10 * source.GrowthRate())
			{
				continue;
			}
			Planet dest = null;
			int bestDistance = 999999;
			foreach (Planet p in pw.EnemyPlanets())
			{
				int dist = pw.Distance(source.PlanetID(), p.PlanetID());
				if (dist < bestDistance)
				{
					bestDistance = dist;
					dest = p;
				}
			}
			if (dest != null)
			{
				pw.IssueOrder(source, dest, source.NumShips());
			}
		}
	}

	public static void Main(String[] args)
	{
		String line = "";
		String message = "";
		int c;
		try
		{
			while ((c = Console.Read()) >= 0)
			{
				switch (c)
				{
					case '\n':
						if (line.Equals("go"))
						{
							PlanetWars pw = new PlanetWars(message, null);
							DoTurn(pw);
							pw.FinishTurn();
							message = "";
						}
						else
						{
							message += line + "\n";
						}
						line = "";
						break;
					default:
						line += (char)c;
						break;
				}
			}
		}
		catch (Exception e)
		{
			// Owned.
		}
	}
}

