
using System;
using System.Collections.Generic;
using DefaultPackage;
public class RandomBot
{
	public static void DoTurn(PlanetWars pw)
	{
		// (1) If we current have a fleet in flight, then do nothing until it
		// arrives.
		if (pw.MyFleets().Count >= 1)
		{
			return;
		}
		// (2) Pick one of my planets at random.
		Random r = new Random();
		Planet source = null;
		List<Planet> p = pw.MyPlanets();
		if (p.Count > 0)
		{
			source = p[(int)(r.NextDouble() * p.Count)];
		}
		// (3) Pick a target planet at random.
		Planet dest = null;
		p = pw.Planets();
		if (p.Count > 0)
		{
			dest = p[(int)(r.NextDouble() * p.Count)];
		}
		// (4) Send half the ships from source to dest.
		if (source != null && dest != null)
		{
			int numShips = source.NumShips() / 2;
			pw.IssueOrder(source, dest, numShips);
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
