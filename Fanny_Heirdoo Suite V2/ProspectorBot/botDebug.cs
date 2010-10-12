using DefaultPackage;
using PlanetDebug;

namespace ProspectorBotDebugger
{
	/// <summary>
	/// DebugginPlugin
	/// </summary>
	public class botDebug : botDebugBase
	{
		PlanetWars gameboard;
		/// <summary>
		/// The pattern of the default bots would be to create a game board
		/// (GameWars) and then have the bot do a move on the board.
		/// I split this into two parts, here you should create the board and store it on a member.
		/// </summary>
		/// <param name="gameData">The game data.</param>
		public override void CreateGameBoardInstance()
		{
			gameboard = new PlanetWars(GameBoardData, this);

		}

		/// <summary>
		/// In CreateGameBoardInstance you have created the gameboard, now use it with your bot.
		/// </summary>
		public override void DoMove()
		{
			ProspectorBot.DoTurn(gameboard);
			gameboard.FinishTurn();
		}
	}
}
