using System;
using System.Collections.Generic;
using System.Linq;
using Gr1d.Api.Agent;
using Gr1d.Api.Node;
using Gr1d.Api.Skill;

namespace G.Skills
{
    class EngineerSkills
    {
        public static void UnitTest(IAgentUpdateInfo agentUpdate, IEngineer1 engineer)
        {
            if (agentUpdate == null || !agentUpdate.Effects.Contains(AgentEffect.UnitTest))
            {
                engineer.UnitTest();
            }
        }

		public static void Pin(IAgentInfo attacker, IEngineer2 engineer)
		{  
            if (!attacker.Effects.Contains(AgentEffect.Pin))
			{
				engineer.Pin(attacker);
			}
		}

        public static void Struts(INodeInformation target, IEngineer3 engineer)
        {
          if (!target.Effects.Contains(NodeEffect.Struts))
          {
              engineer.Struts(target);
          }
        }

        public static void Mentor(IAgentUpdateInfo agentUpdate, IEngineer4 engineer)
        {
              IEnumerable<IAgentInfo> mentorTargets = agentUpdate.Node.MyAgents.Where(x => !x.Effects.Contains(AgentEffect.Mentor));
                if (mentorTargets.Any())
                {
                    engineer.Mentor(mentorTargets);
                }
        }

        public static void Decompile(IAgentInfo agentInfo, IEngineer5 engineer)
        {
            if (!agentInfo.Effects.Contains(AgentEffect.Decompile))
            {
                engineer.Decompile(agentInfo);
            }
        }

        public static void Scaffold(INodeInformation target, IEngineer6 engineer)
        {
           if (!target.Effects.Contains(NodeEffect.Scaffold))
           {
               engineer.Scaffold(target);
           }
        }
    }
}
