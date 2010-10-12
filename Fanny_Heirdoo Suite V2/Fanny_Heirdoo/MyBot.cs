using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FannyHeirdooBot
{
	public class FannyHeirdooBot
	{
		public static void Main()
		{
			//System.Diagnostics.Debugger.Launch();
			int c;
			Universe universe = new Universe(null);
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
								universe.StartTurn(input.ToString());
								input.Length = 0;
								universe.FinishTurn();
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
}