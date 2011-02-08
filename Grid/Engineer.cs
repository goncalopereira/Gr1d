using System.Linq;
using Gr1d.Api.Agent;
using Gr1d.Api.Skill;

namespace G
{
    class Engineer
    {
        public static void UnitTest(IAgentUpdateInfo agentUpdate, IEngineer1 engineer)
        {
            if (!agentUpdate.Effects.Contains(AgentEffect.UnitTest))
            {
                engineer.UnitTest();
            }
        }

		public static void Pin(IEngineer2 engineer, IAgentInfo attacker)
		{
			if (!attacker.Effects.Contains(AgentEffect.Pin))
			{
				engineer.Pin(attacker);
			}
		}
	}
}
