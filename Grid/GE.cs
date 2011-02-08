using System;
using Gr1d.Api.Agent;
using Gr1d.Api.Deck;
using Gr1d.Api.Node;
using Gr1d.Api.Skill;
[assembly:CLSCompliant(true)]

namespace G
{
    public class GE : IEngineer1
    {
        private IDeck _deck;

        public void Initialise(IDeck deck)
        {
            _deck = deck;
        }

        public void Tick(IAgentUpdateInfo agentUpdate)
        {
            Engineer.UnitTest(agentUpdate,this);

            if (!agentUpdate.Node.IsClaimable || TryClaim(agentUpdate.Node))
            {
                Move.GreedyMoveToFirstClaimable(agentUpdate, this);
            }
        }

        private bool TryClaim(INodeInformation node)
        {
            return this.Claim(node).Result == NodeResultType.Success;
        }

        public void OnAttacked(IAgentInfo attacker, IAgentUpdateInfo agentUpdate)
        {
            _deck.Trace(string.Format("Attacked {0}:{1}:{2}", attacker.Owner, attacker.Level, attacker.Type),TraceType.Warning);
            this.Attack(attacker);
            Move.MoveToFirst(agentUpdate, this);
        }

        public void OnArrived(IAgentInfo arriver, IAgentUpdateInfo agentUpdate)
        {
            _deck.Trace(string.Format("Arrived {0}:{1}:{2}", arriver.Owner, arriver.Level, arriver.Type), TraceType.Warning);
            Move.MoveToFirst(agentUpdate, this);
        }
    }
}
