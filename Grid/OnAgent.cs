using System.Linq;
using G.Skills;
using Gr1d.Api.Agent;
using Gr1d.Api.Deck;
using Gr1d.Api.Skill;

namespace G
{
	class OnAgent
	{
	    public static void AttackAndTrySkills(IAgentInfo target, IAgent engineer, IDeck deck, IAgentUpdateInfo update)
	    {
            if (target == null)
            {
                deck.Trace("Target is null",TraceType.Information);
                return;
            }

            deck.Trace(string.Format("Attack: Owner {0} Level {1} Stack {2} at {3}:{4}:{5}", target.Owner, target.Level,  target.Stack,
               update.Node.Row, update.Node.Column, update.Node.Layer), TraceType.Information);
          
            engineer.Attack(target);

            Skills(target, update, engineer);
	    }

	    private static void Skills(IAgentInfo target, IAgentUpdateInfo update, IAgent engineer)
	    {
	        if (!(engineer is Bishop)
                
              //  && HaveAtLeastTwoMoreAgentsThanEnemiesInSameNode(update)
                
                )
	        {

                if (engineer is IEngineer6)
                {
                    EngineerSkills.Scaffold(target.Node, (IEngineer6)engineer);
                    return;
                }

	            if (engineer is IEngineer5)
	            {
	                EngineerSkills.Decompile(target, (IEngineer5)engineer);
	                return;
	            }

	            if (engineer is IEngineer4)
	            {
	                EngineerSkills.Mentor(update, (IEngineer4)engineer);
	                return;
	            }

	            if (engineer is IEngineer3)
	            {
	                EngineerSkills.Struts(target.Node, (IEngineer3)engineer);
	                return;
	            }

	            if (engineer is IEngineer2)
	            {
	                EngineerSkills.Pin(target, (IEngineer2)engineer);
	                return;
	            }
	        }
	    }

	    private static bool HaveAtLeastTwoMoreAgentsThanEnemiesInSameNode(IAgentUpdateInfo update)
	    {
	        return update.Node.MyAgents.Count() > update.Node.OpposingAgents.Count()+1;
	    }

    }
}
