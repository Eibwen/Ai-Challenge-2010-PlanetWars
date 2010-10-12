using PlanetDebug;

 
	/// <summary>
	/// DebugginPlugin
	/// </summary>
	public class botDebug : botDebugBase
	{
		Universe gameboard;
		public override void Reset()
		{
			Universe.Reset();
		}
		/// <summary>
		/// The pattern of the default bots would be to create a game board
		/// (GameWars) and then have the bot do a move on the board.
		/// I split this into two parts, here you should create the board and store it on a member.
		/// </summary>
		/// <param name="gameData">The game data.</param>
		public override void CreateGameBoardInstance()
		{
			gameboard = new Universe(GameBoardData, this);
		}
		/// <summary>
		/// In CreateGameBoardInstance you have created the gameboard, now use it with your bot.
		/// </summary>
		public override void DoMove()
		{
			FannyHeirdooBot.DoTurn(gameboard);
			gameboard.FinishTurn();
		}

		/// <summary>
		/// Planets the can survive attack query.
		/// </summary>
		/// <param name="planetId">The planet id.</param>
		/// <returns>true if, GO, Walk out that door..</returns>
		public override bool QueryPlanetCanSurviveAttack(int planetId)
		{
			if (gameboard != null)
			{
				if (planetId == 2)
				{
					return gameboard.AllPlanetsOnPlanetId[planetId].CanSurviveIncommingAttack;
				}
			}
			return true;
		}
	}
