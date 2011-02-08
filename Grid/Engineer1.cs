using System;
using System.Linq;
using Gr1d.Api.Agent;
using Gr1d.Api.Deck;
using Gr1d.Api.Skill;
[assembly:CLSCompliant(true)]

namespace G
{
    public class Engineer1 : IEngineer1
    {
        private IDeck _deck;

        public void Initialise(IDeck deck)
        {
            _deck = deck;
        }

        public void Tick(IAgentUpdateInfo agentUpdate)
        {
            Engineer.UnitTest(agentUpdate,this);

            if (!agentUpdate.Node.IsClaimable || Claim.TryClaim(agentUpdate.Node, this))
            {
                Move.GreedyMoveToFirstClaimable(agentUpdate, this);
            }
        }

    	public void OnAttacked(IAgentInfo attacker, IAgentUpdateInfo agentUpdate)
    	{
    	    _deck.Trace(String.Format("Attacked {0}:{1}:{2}", attacker.Owner, attacker.Level, attacker.Type),TraceType.Warning);		
    	    this.Attack(attacker);
    	    this.Move(agentUpdate.Node.Exits.First().Value);
    	}

    	public void OnArrived(IAgentInfo arriver, IAgentUpdateInfo agentUpdate)
    	{
    	    _deck.Trace(String.Format("Arrived {0}:{1}:{2}", arriver.Owner, arriver.Level, arriver.Type), TraceType.Warning);
    	    this.Move(agentUpdate.Node.Exits.First().Value);
    	}
    }
}
