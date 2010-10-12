using botDebug;
using System.Text;
using System;
using System.Linq;

namespace PlanetDebug
{
	/// <summary>
	/// Base class for the 'debugging system'
	/// It has not been thought out, I just tried to create a system that
	/// would leave as much intact from the original samples as possible.
	/// .
	/// If they ever update the default bots, integrating the new version into
	/// the debugger should be as simple as possible.
	/// </summary>
	public abstract class botDebugBase
	{
		StringBuilder _outputBuffer = new StringBuilder();
		/// <summary>
		/// Gets or sets the ID.
		/// </summary>
		/// <value>The ID.</value>
		public int Id { get; set; }
		/// <summary>
		/// Resets this instance.
		/// </summary>
		public virtual void Reset() { }

		public void IssueOrder(string command)
		{
			_outputBuffer.AppendLine(command);
			if (IssueOrderCallback != null)
			{
				IssueOrderCallback(this.Id, command);
			}
		}
		public Func<int, string, int> IssueOrderCallback;
		/// <summary>
		/// Gets or sets the output buffer.
		/// </summary>
		/// <value>The output buffer.</value>
		public string OutputBuffer
		{
			get
			{
				return _outputBuffer.ToString();
			}
		}
		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			_outputBuffer.Length = 0;
		}
		/// <summary>
		/// Gets or sets the game board data.
		/// </summary>
		/// <value>The game board data.</value>
		public string GameBoardData { get; set; }
		/// <summary>
		/// The pattern of the default bots would be to create a game board
		/// (GameWars) and then have the bot do a move on the board.
		/// I split this into two parts, here you should create the board and store it on a member.
		/// </summary>
		/// <param name="gameData">The game data.</param>
		public abstract void CreateGameBoardInstance();

		/// <summary>
		/// In CreateGameBoardInstance you have created the gameboard, now use it with your bot.
		/// </summary>
		public abstract void DoMove();

		/// <summary>
		/// Planets the can survive attack query.
		/// </summary>
		/// <param name="planetId">The planet id.</param>
		/// <returns></returns>
		public virtual bool QueryPlanetCanSurviveAttack(int planetId)
		{
			return true;
		}

		/// <summary>
		/// Determines whether [is dominating query].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [is dominating query]; otherwise, <c>false</c>.
		/// </returns>
		public virtual bool QueryIsDominating()
		{
			return false;
		}
	}
}
