using Gr1d.Api.Agent;
using Gr1d.Api.Node;
using Gr1d.Api.Skill;

namespace G
{
	class Claim
	{
		public static bool TryClaim(INodeInformation node, IAgent agent)
		{
			return agent.Claim(node).Result == NodeResultType.Success;
		}
	}
}
