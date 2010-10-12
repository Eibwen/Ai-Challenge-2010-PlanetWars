using System;
using System.Collections.Generic;

namespace FannyHeirdooBot
{
	public class AttackPlanParticipant
	{
		public Planet Source;
		public int ScheduledTurn;
		public PlanetTurn DefendersTurn { get; set; }
		public PlanetTurn AttackersTurn { get; set; }

	}
}