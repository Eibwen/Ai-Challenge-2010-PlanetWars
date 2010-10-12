using System;
namespace FannyHeirdooBot
{
	public interface IBotStrategy
	{
		void DoTurn(Universe uni);
	}
}
