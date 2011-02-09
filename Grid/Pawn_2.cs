using G.Skills;
using Gr1d.Api.Agent;

namespace G
{
    public class Pawn21 : Pawn_2, IEngineer1 { }
    public class Pawn22 : Pawn_2, IEngineer2 { }
    public class Pawn23 : Pawn_2, IEngineer3 { }
    public class Pawn24 : Pawn_2, IEngineer4 { }
    public class Pawn25 : Pawn_2, IEngineer5 { }
    public class Pawn26 : Pawn_2, IEngineer6 { }

    public class Pawn_2 : Bishop
    {
        protected override void AgentMove(IAgentUpdateInfo agentUpdate)
        {

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
                    ClaimSkills.TryClaim(agentUpdate.Node, this);
                    MoveSkills.GreedyMoveToFirstAgent(agentUpdate, this, Deck);
                    break;
            }
        }
    }
}