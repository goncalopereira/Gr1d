using System;
using G.Skills;
using Gr1d.Api.Agent;
using Gr1d.Api.Deck;
using Gr1d.Api.Node;

namespace G
{
    public class Pawn1 : Pawn, IEngineer1 { }
    public class Pawn2 : Pawn, IEngineer2 { }
    public class Pawn3 : Pawn, IEngineer3 { }
    public class Pawn4 : Pawn, IEngineer4 { }
    public class Pawn5 : Pawn, IEngineer5 { }
    public class Pawn6 : Pawn, IEngineer6 { }

    public class Pawn : Bishop
    {   
        protected override void AgentMove(IAgentUpdateInfo agentUpdate)
        {

            if (agentUpdate.Node.Sector.Id == ArenaId)
            {
                MoveSkills.GreedyMoveToFirstAgent(agentUpdate, this, Deck);
            }

            switch(agentUpdate.Action)
            {
                case AgentAction.Raiding:
                    ClaimSkills.TryClaim(agentUpdate.Node, this);
                    MoveSkills.GreedyMoveToFirstAgent(agentUpdate, this,Deck);
                    break;
                case AgentAction.Defending:
                    MoveSkills.GreedyMoveToFirstAgent(agentUpdate, this,Deck);
                    break;
                default:
                    GreedyClaim(agentUpdate, this);
            
                    break;
            }
        }

        protected virtual void GreedyClaim(IAgentUpdateInfo agentUpdate, Pawn agent)
        {
            string displayHandle = agentUpdate.Node.Owner.DisplayHandle;
            if (ClaimSkills.TryClaim(agentUpdate.Node, agent))
            {
                Deck.Trace("Claimed From Node Owner: " + displayHandle, TraceType.Information);
                MoveSkills.GreedyMoveToFirstClaimable(agentUpdate, agent);
            }
        }
    }


   
}