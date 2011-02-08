using G.Skills;
using Gr1d.Api.Agent;
using Gr1d.Api.Deck;

namespace G
{
    public class BadPawn1 : BadPawn, IEngineer1 { }
    public class BadPawn2 : BadPawn, IEngineer2 { }
    public class BadPawn3 : BadPawn, IEngineer3 { }
    public class BadPawn4 : BadPawn, IEngineer4 { }
    public class BadPawn5 : BadPawn, IEngineer5 { }

    public class BadPawn : Pawn
    {
        //protected override void GreedyClaim(IAgentUpdateInfo agentUpdate, Pawn agent)
        //{
        //    if (MoveSkills.HasNodeOwner(agentUpdate.Node, agentUpdate.Owner.Id))
        //    {
        //        Deck.Trace("Node Owner: " + agentUpdate.Node.Owner.DisplayHandle,TraceType.Information);
        //        if (ClaimSkills.TryClaim(agentUpdate.Node, agent))
        //        {
        //            MoveSkills.GreedyMoveToFirstClaimable(agentUpdate, agent, true, Deck);
        //        }
        //    }
        //}
    }
}