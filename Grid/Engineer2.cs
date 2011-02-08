using System;
using Gr1d.Api.Agent;
using Gr1d.Api.Deck;
using Gr1d.Api.Skill;

namespace G
{
	public class Engineer2 : IEngineer2
	{
	    protected IDeck Deck;
	    protected IAgentInfo Agent;

		public void Initialise(IDeck deck)
		{
			Deck = deck;
		}

		public void Tick(IAgentUpdateInfo agentUpdate)
		{
			if (Agent != null && Agent.Node == agentUpdate.Node)
			{
				AttackAndTryPin();
			}
			else
			{
                Engineer.UnitTest(agentUpdate, this);
				TryClaimAndMove(agentUpdate);
			}
		}

	    protected void TryClaimAndMove(IAgentUpdateInfo agentUpdate)
		{
			if (!agentUpdate.Node.IsClaimable || Claim.TryClaim(agentUpdate.Node, this))
			{
				Move.GreedyMoveToFirstClaimable(agentUpdate, this);
			}
		}

	    protected void AttackAndTryPin()
		{
	        Deck.Trace(String.Format("Attack And Pin {0}:{1}:{2}", Agent.Owner, Agent.Level, Agent.Type),TraceType.Warning);		
	        this.Attack(Agent);
	        Engineer.Pin(this, Agent);
		}

		public void OnAttacked(IAgentInfo attacker, IAgentUpdateInfo agentUpdate)
		{
			Agent = attacker;
			AttackAndTryPin();
		}

		public void OnArrived(IAgentInfo arriver, IAgentUpdateInfo agentUpdate)
		{
			Agent = arriver;
			AttackAndTryPin();				
		}
	}

}