using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gr1d.Api.Agent;
using Gr1d.Api.Deck;
using Gr1d.Api.Node;

namespace G
{
    public class MoveSkills
    {
        public static INodeInformation GreedyMoveToFirstClaimable(IAgentUpdateInfo agentUpdate, IGAgent agent)
        {
            List<INodeInformation> possibleNodes =
                NextLevelFirstClaimable(agent.Deck,
                                        new List<INodeInformation>() {agentUpdate.Node},
                                        agentUpdate.Owner.Id,1);

            if (!possibleNodes.Any())
            {
               ICollection<INodeInformation> nodeInformations = agentUpdate.Node.Exits.Values;
               possibleNodes =  NextLevelFirstClaimable(agent.Deck, nodeInformations, agentUpdate.Owner.Id,2);

                if (!possibleNodes.Any())
                {
                    var nextLevel = new List<INodeInformation>();

                    //3rd level
                    foreach (INodeInformation nodeInformation in nodeInformations)
                    {
                     nextLevel.AddRange(nodeInformation.Exits.Values);   
                    }

                  possibleNodes =  NextLevelFirstClaimable(agent.Deck, nextLevel, agentUpdate.Owner.Id,3);
                }
              
            }
        
            
           return PickMove(agentUpdate, possibleNodes, agent);
        }

        private static List<INodeInformation> NextLevelFirstClaimable(IDeck deck, IEnumerable nodeInformations, Guid agentId, int i)
        {
            List<INodeInformation> possibleNodes = new List<INodeInformation>();
            foreach (INodeInformation nodeInformation in nodeInformations)
            {
                possibleNodes.AddRange(nodeInformation.Exits.Values.Where(x=> ClaimThis(x, agentId)));
            }

            deck.Trace(string.Format("I'm in level{0} with possible claimable {1} ", i, possibleNodes.Count),TraceType.Information);

            IEnumerable<INodeInformation> hasOwner = possibleNodes.Where(x => !x.Owner.DisplayHandle.Equals("uid0"));

            return hasOwner.Any() ? hasOwner.ToList() : possibleNodes;
        }

        private static bool ClaimThis(INodeInformation node, Guid agentId)
        {
            return node.IsClaimable
                   && !node.Effects.Contains(NodeEffect.Struts)
                   && (HasNodeOwner(node, agentId));
        }

        internal static bool HasNodeOwner(INodeInformation x, Guid agentId)
        {
            return (x.Owner != null
                && x.Owner.Id != agentId);
        }

        public static INodeInformation GreedyMoveToFirstAgent(IAgentUpdateInfo agentUpdate, GAgent agent, IDeck deck)
        {
            List<IAgentInfo> possibleAgents = GetPossibleNodes(agentUpdate.Node);

            if (!possibleAgents.Any())
            {
                foreach (INodeInformation nodeInformation in agentUpdate.Node.Exits.Values)
                {
                    possibleAgents.AddRange(GetPossibleNodes(nodeInformation));
                }

                deck.Trace("No local agents, possible second level=" + possibleAgents.Count, TraceType.Information);    
            }

            IOrderedEnumerable<IAgentInfo> enemiesOrderByStack = possibleAgents.Where(x=> x.Stack > 0).OrderBy(x => x.Stack);

            IEnumerable<IAgentInfo> enemiesWithMinimumStack = enemiesOrderByStack.Where(x => x.Stack == enemiesOrderByStack.First().Stack);
            
            IEnumerable<INodeInformation> nodesWeakestEnemiesAndMostFriends = enemiesWithMinimumStack.Select(x=> x.Node)
                //no node fx
                .Where(x => !x.Effects.Contains(NodeEffect.Struts))
                
                .OrderByDescending(y => y.MyAgents.Count()).ToList();

            if (nodesWeakestEnemiesAndMostFriends.Any())
            {
                agent.Deck.Trace(
                    string.Format("Enemies:{0} nodeWeakestEnemies:{1}:{2}:{3}", 
                        enemiesOrderByStack.Count(), 
                        nodesWeakestEnemiesAndMostFriends.First().Row, 
                        nodesWeakestEnemiesAndMostFriends.First().Column, 
                        nodesWeakestEnemiesAndMostFriends.First().Layer),
                    TraceType.Information);
            }
            else
            {
                agent.Deck.Trace("Can't find any agents",TraceType.Information);
            }

           return PickMove(agentUpdate, nodesWeakestEnemiesAndMostFriends, agent);         
        }

        private static List<IAgentInfo> GetPossibleNodes(INodeInformation information)
        {
            List<IAgentInfo> possibleNodes = new List<IAgentInfo>();
 
            foreach (IEnumerable<IAgentInfo> targets in information.Exits.Values
                .Where(nodeInformation => nodeInformation.OpposingAgents.Any())
                .Select(x => x.OpposingAgents))
            {
                possibleNodes.AddRange(targets);
            }
            return possibleNodes;
        }

        private static INodeInformation PickMove(IAgentUpdateInfo agentUpdate, IEnumerable<INodeInformation> possibleNodes, IGAgent agent)
        {
            INodeInformation targetNode;
            if (CanAgentMove(agentUpdate))
            {
              targetNode = possibleNodes.Any() ? possibleNodes.First() : GetRandomNode(agentUpdate,agent.Deck);
              NodeExit exit = agentUpdate.Node.RouteTo(agentUpdate.Node, targetNode.Layer, targetNode.Row, targetNode.Column);
			
				if (exit != NodeExit.None)
				{
					agent.Move(agentUpdate.Node.Exits[exit]);				
				}
            }
            else
            {
                targetNode = agentUpdate.Node;         
            }

            agentUpdate.Node.RouteTo(agentUpdate.Node, targetNode.Layer, targetNode.Row, targetNode.Column);

            return targetNode;
        }

        private static INodeInformation GetRandomNode(IAgentUpdateInfo agentUpdate,IDeck deck)
        {
            int seed = DateTime.Now.Second;
            Random random = new Random(seed);
            IEnumerable<INodeInformation> nodeInformations = agentUpdate.Node.Exits.Values.Where(x => !x.Effects.Contains(NodeEffect.Struts));
        
			if (nodeInformations.Count() == 0)
				return agentUpdate.Node;

        	int exitNumber = random.Next(0, nodeInformations.Count() - 1);
            deck.Trace(string.Format("Exits: {0} Random: {1}", nodeInformations.Count(), exitNumber),
                       TraceType.Information);
            return nodeInformations.ElementAt(exitNumber);
        }

        private static bool CanAgentMove(IAgentUpdateInfo agentUpdate)
        {
            return !agentUpdate.Node.Effects.Contains(NodeEffect.Struts)
                   && !agentUpdate.Effects.Contains(AgentEffect.Pin);
        }
    }
}
