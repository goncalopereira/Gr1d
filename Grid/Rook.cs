using System.Collections.Generic;
using System.Linq;
using G.Skills;
using Gr1d.Api.Agent;
using Gr1d.Api.Deck;
using Gr1d.Api.Node;

namespace G
{
    public class Rook1 : Rook, IEngineer1 {}
    public class Rook2 : Rook, IEngineer2  {}
    public class Rook3 : Rook, IEngineer3 {}
    public class Rook4 : Rook, IEngineer4 { }
    public class Rook5 : Rook, IEngineer5 { }
    public class Rook6 : Rook, IEngineer6 { }

    public abstract class Rook : GAgent
    {
        public override void Tick(IAgentUpdateInfo agentUpdate)
        {
            EngineerSkills.UnitTest(agentUpdate, (IEngineer1)this);
            RookAttack(agentUpdate);
        }

        protected void RookAttack(IAgentUpdateInfo agentUpdate)
        {
            Target = GetTarget(agentUpdate);
            if (Target != null)
            {
                OnAgent.AttackAndTrySkills(Target, this, Deck, agentUpdate);
            }
            else
            {
                AgentMove(agentUpdate);
            }
        }

        private IAgentInfo GetTarget(IAgentUpdateInfo agentUpdate)
        {
            string nodeEnemiesInfo = agentUpdate.Node.Exits.Values.Aggregate(string.Empty, (current, node) => GetNodeEnemiesInfo(node, current));

            Deck.Trace(string.Format("Alies: {0} Enemies: {1} Nodes:{2}",
                                     agentUpdate.Node.MyAgents.Count() - 1,
                                     agentUpdate.Node.OpposingAgents.Count(),
                                     nodeEnemiesInfo), TraceType.Information);

            IEnumerable<IAgentInfo> list = agentUpdate.Node.OpposingAgents.Where(x => x.Stack > 0);
            return list.Any() ? list.OrderBy(x => x.Stack).First() : null;
        }

        private static string GetNodeEnemiesInfo(INodeInformation node, string nodeEnemiesInfo)
        {
            return string.Format("{0}{1}:{2}:{3}={4}, ", nodeEnemiesInfo, node.Row, node.Column, node.Layer, node.OpposingAgents.Count());
        }

        public override void OnArrived(IAgentInfo arriver, IAgentUpdateInfo agentUpdate)
        {
            RookAttack(agentUpdate);
        }

        public override void OnAttacked(IAgentInfo attacker, IAgentUpdateInfo agentUpdate)
        {
            RookAttack(agentUpdate);
        }

        protected virtual void AgentMove(IAgentUpdateInfo agentUpdate)
        {   
            if (agentUpdate.Action == AgentAction.Raiding)
            {
                ClaimSkills.TryClaim(agentUpdate.Node, this);
            }
          
            MoveSkills.GreedyMoveToFirstAgent(agentUpdate, this,Deck);
        }
    }
}