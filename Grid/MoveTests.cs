
using Gr1d.Api.Agent;
using Gr1d.Api.Deck;

namespace G
{
   
    class MoveTests
    {
        public void GreedyMoveToFirstClaimable_first_level_non_agent()
        {
            IAgentUpdateInfo agentUpdate;
            GAgent agent = new Pawn();
            const bool goToEnemy = false;
            IDeck deck = null;
            MoveSkills.GreedyMoveToFirstClaimable(agentUpdate, agent, goToEnemy, deck);
        }
    }
}
