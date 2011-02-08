using G.Skills;
using Gr1d.Api.Agent;
namespace G
{
    public class Knight1 : Knight, IEngineer1 { }
    public class Knight2 : Knight, IEngineer2 { }
    public class Knight3 : Knight, IEngineer3 { }
    public class Knight4 : Knight, IEngineer4 { }
    public class Knight5 : Knight, IEngineer5 { }
    public class Knight6 : Knight, IEngineer6 { }

    public class Knight : Bishop
    {
        protected override void AgentMove(IAgentUpdateInfo agentUpdate)
        {
            ClaimSkills.TryClaim(agentUpdate.Node, this);
            MoveSkills.GreedyMoveToFirstAgent(agentUpdate, this,Deck);
        }
    }
}