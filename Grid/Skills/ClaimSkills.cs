using Gr1d.Api.Agent;
using Gr1d.Api.Node;
using Gr1d.Api.Skill;

namespace G.Skills
{
	class ClaimSkills
	{
		private static bool ItsClaimableTryClaim(INodeInformation node, IAgent agent)
		{
			return agent.Claim(node).Result == NodeResultType.Success;
		}

	    public static bool TryClaim(INodeInformation node, IAgent agent)
	    {
	        return !node.IsClaimable ||  ItsClaimableTryClaim(node, agent);
	    }
	}
}
