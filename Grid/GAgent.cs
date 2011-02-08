using System;
using G.Skills;
using Gr1d.Api.Agent;
using Gr1d.Api.Deck;
using Gr1d.Api.Node;
using Gr1d.Api.Skill;

[assembly: CLSCompliant(true)]
namespace G
{
	public interface IGAgent : IAgent
	{
		IDeck Deck { get; set; }
		IAgentInfo Target { get; set; }
		ITargetNodeResult Move(INodeInformation targetNode);
	}

	public abstract class GAgent : IGAgent
	{
        public static readonly Guid ArenaId = new Guid("36dcbd09-02e0-4795-9a47-5a0db787d81e");

		public IDeck Deck { get; set; }
        public IAgentInfo Target { get; set; }
		public ITargetNodeResult Move(INodeInformation targetNode)
		{
			return IAgentSkillExtensions.Move(this,targetNode);
		}

		public virtual void Initialise(IDeck deck)
        {
            Deck = deck;
            EngineerSkills.UnitTest(null, (IEngineer1)this);
        }
	
        public virtual void Tick(IAgentUpdateInfo agentUpdate)
        {
            throw new NotImplementedException();
        }

        public virtual void OnAttacked(IAgentInfo attacker, IAgentUpdateInfo agentUpdate)
        {
            throw new NotImplementedException();
        }

        public virtual void OnArrived(IAgentInfo arriver, IAgentUpdateInfo agentUpdate)
        {
            throw new NotImplementedException();
        }
    }
}