namespace FannyHeirdooBot
{

	public class PlanetTurn
	{
		public int TurnsFromNow { get; private set; }
		public int Owner;
		public int NumShips;
		public PlanetTurn Next;
		public PlanetTurn Prior;
		public PlanetTurn(int turnsFromNow)
		{
			TurnsFromNow = turnsFromNow;
		}
		internal void SetValues(int owner, int numShips)
		{
			Owner = owner;
			IsMine = Owner == 1;
			NumShips = numShips;
		}

		public bool IsMine { get; private set; }

		internal PlanetTurn Clone()
		{
			PlanetTurn result = new PlanetTurn(TurnsFromNow);
			result.SetValues(Owner, NumShips);
			return result;
		}

		public void Grow(int numberOfShipsToGrow)
		{
			if (Owner > 0)
			{
				NumShips += numberOfShipsToGrow;
			}
		}

	}
}