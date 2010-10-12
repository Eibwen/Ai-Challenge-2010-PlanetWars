using System;
using System.Collections.Generic;
using System.Linq;

namespace FannyHeirdooBot
{
	public class AttackPlan
	{
		public AttackPlan()
		{
			Participants = new List<AttackPlanParticipant>();
		}
		public String Strategy;
		public String Reason;
		public Planet Target;
		public Double Sweetness;
		public bool Enabled;
		public bool DominationMove;
		public List<AttackPlanParticipant> Participants { get; private set; }

		public void AddParticipant(Planet participant, PlanetTurn turn, int scheduledTurn)
		{
			AttackPlanParticipant order = new AttackPlanParticipant();
			order.Source = participant;
			order.AttackersTurn = turn;
			order.ScheduledTurn = scheduledTurn;
			Participants.Add(order);
		}

		public void AddParticipant(Planet participant, PlanetTurn turn, PlanetTurn defendersTurn)
		{
			AttackPlanParticipant order = new AttackPlanParticipant();
			order.Source = participant;
			order.AttackersTurn = turn;
			order.ScheduledTurn = turn.TurnsFromNow;
			order.DefendersTurn = defendersTurn;
			Participants.Add(order);
		}
	}
}