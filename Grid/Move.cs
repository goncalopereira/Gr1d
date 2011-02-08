using System.Collections.Generic;
using System.Linq;
using Gr1d.Api.Agent;
using Gr1d.Api.Node;
using Gr1d.Api.Skill;

namespace G
{
    class Move
    {
        public static void GreedyMoveToFirstClaimable(IAgentUpdateInfo agentUpdate, IAgent agent)
        {
            IEnumerable<INodeInformation> possibleNodes = agentUpdate.Node.Exits.Values.Where(x => x.IsClaimable);
            PickMove(agentUpdate, possibleNodes, agent);
        }

        public static void GreedyMoveToFirstAgent(IAgentUpdateInfo agentUpdate, IAgent agent, IAgentInfo agentBeingAttacked)
        {
            IEnumerable<INodeInformation> agentBeingAttackedMoved =
                agentUpdate.Node.Exits.Values.Where(x => x.OpposingAgents.Contains(agentBeingAttacked));

            if (agentBeingAttackedMoved.Count() > 0)
            {
                PickMove(agentUpdate, agentBeingAttackedMoved,agent);
            }
            else
            {
                IEnumerable<INodeInformation> possibleNodes =
                    agentUpdate.Node.Exits.Values.Where(x => x.OpposingAgents.Count() > 0).OrderBy(
                        x => x.OpposingAgents.Count());

                PickMove(agentUpdate, possibleNodes, agent);      
            }
        }

        private static void PickMove(IAgentUpdateInfo agentUpdate, IEnumerable<INodeInformation> possibleNodes, IAgent agent)
        {
            agent.Move(possibleNodes.Count() > 0 ? possibleNodes.First() : agentUpdate.Node.Exits.First().Value);
        }
    }

}
