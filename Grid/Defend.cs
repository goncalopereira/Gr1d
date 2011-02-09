using System.Linq;
using G.Skills;
using Gr1d.Api.Agent;

namespace G
{
    public class Defend3 : Defend, IEngineer3 { }

    public class Defend4 : Defend, IEngineer4 { }

    public class Defend5 : Defend, IEngineer5 { }

    public class Defend : Bishop
    {
        protected override void AgentMove(IAgentUpdateInfo agentUpdate)
        {
            if (agentUpdate.Node.Column == 0 
                && agentUpdate.Node.Row == 0 
                && agentUpdate.Node.Layer == 0)
            {
                if (agentUpdate.Node.AlliedAgents.Count() > 0)
                {
                  
                    if (this is IEngineer3 || this is IEngineer4  || this is IEngineer5)
                    {
                        EngineerSkills.Struts(agentUpdate.Node, (IEngineer3) this);
                    }

                }
            }

            else
            {
                Move(agentUpdate.Node.Exits[ agentUpdate.Node.RouteTo(agentUpdate.Node, 0, 0, 0)]);
            }


            if (agentUpdate.Node.Sector.Id == ArenaId)
            {
                MoveSkills.GreedyMoveToFirstAgent(agentUpdate, this, Deck);
            }

            switch (agentUpdate.Action)
            {
                case AgentAction.Raiding:
                    ClaimSkills.TryClaim(agentUpdate.Node, this);
                    MoveSkills.GreedyMoveToFirstAgent(agentUpdate, this, Deck);
                    break;
                case AgentAction.Defending:
                    MoveSkills.GreedyMoveToFirstAgent(agentUpdate, this, Deck);
                    break;
                default:
                  

                    break;
            }
        }
    }
}