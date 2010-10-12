using PlanetDebug;
using FannyHeirdooBot;

namespace Fanny_HeirdooDebugger
{
	/// <summary>
	/// DebugginPlugin
	/// </summary>
	public class botDebug : botDebugBase
	{
		Universe gameboard;
		/// <summary>
		/// Initializes a new instance of the <see cref="botDebug"/> class.
		/// </summary>
		public botDebug()
		{
			BotEnvironment.DUMP_MAP = false;
			gameboard = new Universe(this);
		}
		/// <summary>
		/// The pattern of the default bots would be to create a game board
		/// (GameWars) and then have the bot do a move on the board.
		/// I split this into two parts, here you should create the board and store it on a member.
		/// </summary>
		/// <param name="gameData">The game data.</param>
		public override void CreateGameBoardInstance()
		{
			
		}
		/// <summary>
		/// In CreateGameBoardInstance you have created the gameboard, now use it with your bot.
		/// </summary>
		public override void DoMove()
		{
			gameboard.StartTurn(GameBoardData);
			gameboard.FinishTurn();
		}

		/// <summary>
		/// Planets the can survive attack query.
		/// </summary>
		/// <param name="planetId">The planet id.</param>
		/// <returns>true if, GO, Walk out that door..</returns>
		public override bool QueryPlanetCanSurviveAttack(int planetId)
		{
			if (gameboard != null && gameboard.Planets.Count > 0)
			{
				return gameboard.Planets[planetId].DoesNotChangeOwner;
			}
			return true;
		}

		public override bool QueryIsDominating()
		{
			return gameboard.IsDominating;
		}

	}
}
