using System;
using System.IO;

namespace FannyHeirdooBot
{
	public static class BotEnvironment
	{
		public const bool TRACE_ENABLED = false;
		public const bool DUMP_ENABLED = false;
		public static bool DUMP_MAP = true;

		public static int MIN_SHIPS_AFTER_MOVE = 2;
		public static int MIN_SHIPS_BEFORE_MOVE = 7;

        const string SOLUTION_PATH = @"D:\Projects\CSharp\!Personal\GoogleAIChallenge2010\Fanny_Heirdoo Suite V2\";
        public const string TRACE_PATH = SOLUTION_PATH + "FANNY.txt";
        public const string DUMP_MAPS = SOLUTION_PATH + @"Run\Maps\dumped";
        public const string DUMP_PATH = SOLUTION_PATH + @"Run\FANNY_MOVES";

		internal static void ClearTrace()
		{
			if (TRACE_ENABLED)
			{
				File.WriteAllText(BotEnvironment.TRACE_PATH, "");
			}
			if (DUMP_ENABLED)
			{
				try
				{
					Directory.Delete(BotEnvironment.DUMP_PATH);
				}
				catch
				{
				}
				finally
				{
					System.Threading.Thread.Sleep(100);
					Directory.CreateDirectory(BotEnvironment.DUMP_PATH);
				}

			}
		}

		public static void DumpMove(string move)
		{
			if (DUMP_ENABLED)
			{
				File.WriteAllText(MoveDumpFilename, move + "\r\n");
			}
		}
		private static string MoveDumpFilename
		{
			get
			{
				return Path.Combine(BotEnvironment.DUMP_PATH, "Move" + Universe.TurnCount + ".txt");
			}
		}

		internal static void DumpLayout(string boardLayout)
		{
			if (DUMP_MAP && Universe.TurnCount == 1)
			{
				int Hash = boardLayout.GetHashCode();
				string targetFile = Path.Combine(BotEnvironment.DUMP_MAPS, Hash + ".txt");
				if (!File.Exists(targetFile))
				{
					File.WriteAllText(targetFile, boardLayout);
				}
			}

			if (DUMP_ENABLED)
			{
				string targetFile = Path.Combine(BotEnvironment.DUMP_PATH, "Turn" + Universe.TurnCount + ".txt");
				File.WriteAllText(targetFile, boardLayout);
				File.WriteAllText(MoveDumpFilename, "");
			}
		}

		internal static void Trace(string message)
		{
			if (TRACE_ENABLED)
			{
				File.AppendAllText(BotEnvironment.TRACE_PATH, message);
				File.AppendAllText(BotEnvironment.TRACE_PATH, Environment.NewLine);
			}
		}
		internal static void Trace(string message, int someValue)
		{
			if (TRACE_ENABLED)
			{
				File.AppendAllText(BotEnvironment.TRACE_PATH, message);
				File.AppendAllText(BotEnvironment.TRACE_PATH, "Value: " + someValue);
				File.AppendAllText(BotEnvironment.TRACE_PATH, Environment.NewLine);
			}
		}
	}
}