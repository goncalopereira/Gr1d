using System;
using System.Collections.Generic;
using Gr1d.Api.Agent;
using Gr1d.Api.Deck;
using Gr1d.Api.Node;
using Gr1d.Api.Player;
using NUnit.Framework;
using Rhino.Mocks;

namespace G
{
	[TestFixture]
	class MoveTests
	{
		[SetUp]
		public void setup()
		{
			_deck = MockRepository.GenerateMock<IDeck>();
			_agent = MockRepository.GenerateMock<IGAgent>();
			_agent.Stub(x => x.Deck).Return(_deck);
           
			_owner = MockRepository.GenerateMock<IPlayer>();
		    _owner.Stub(x => x.Id).Return(Guid.NewGuid());
		    _owner.Stub(x => x.DisplayHandle).Return("owner1");

			_owner2 = MockRepository.GenerateMock<IPlayer>();
		    _owner2.Stub(x => x.Id).Return(Guid.NewGuid());
		    _owner2.Stub(x => x.DisplayHandle).Return("owner2");

            myPlayer = MockRepository.GenerateMock<IPlayer>();
		    myPlayer.Stub(x => x.Id).Return(Guid.NewGuid());
		    myPlayer.Stub(x => x.DisplayHandle).Return("player");

		    uid = MockRepository.GenerateMock < IPlayer>();
		    uid.Stub(x => x.Id).Return(Guid.NewGuid());
            uid.Stub(x => x.DisplayHandle).Return("uid0");
		}

		[Test]
		public void no_target()
		{		
		
			INodeInformation nodeInformation = GetNodeInformation(new List<NodeEffect>(), new Dictionary<NodeExit, INodeInformation>(), 0, 1, 2, false, _owner);
			
			IAgentUpdateInfo agentUpdate = GetAgentUpdate(nodeInformation);

			var result = MoveSkills.GreedyMoveToFirstClaimable(agentUpdate, _agent);

			_agent.AssertWasNotCalled(x => x.Move(Arg<INodeInformation>.Is.Anything));
		}

		[Test]
		public void one_target_claimable()
		{
			_lvl1Node = GetNodeInformation(new List<NodeEffect>(), new Dictionary<NodeExit, INodeInformation>(), 1, 2, 3, true, _owner2);
			var lvl1Exists = new Dictionary<NodeExit, INodeInformation> { { NodeExit.Alpha, _lvl1Node } };
			INodeInformation nodeInformation = GetNodeInformation(new List<NodeEffect>(), lvl1Exists, 0, 1, 2,false,myPlayer);

			IAgentUpdateInfo agentUpdate = GetAgentUpdate(nodeInformation);
			var result = MoveSkills.GreedyMoveToFirstClaimable(agentUpdate, _agent);
			nodeInformation.AssertWasCalled(x => x.RouteTo(nodeInformation, _lvl1Node.Layer, _lvl1Node.Row, _lvl1Node.Column));

		}

		[Test]
		public void one_target_claimable_same_owner()
		{
			_lvl1Node = GetNodeInformation(new List<NodeEffect>(), new Dictionary<NodeExit, INodeInformation>(), 1, 2, 3, true, _owner);
			var lvl1Exists = new Dictionary<NodeExit, INodeInformation> { { NodeExit.Alpha, _lvl1Node } };
			INodeInformation nodeInformation = GetNodeInformation(new List<NodeEffect>(), lvl1Exists, 0, 1, 2, false, myPlayer);

			IAgentUpdateInfo agentUpdate = GetAgentUpdate(nodeInformation);
			var result = MoveSkills.GreedyMoveToFirstClaimable(agentUpdate, _agent);
			nodeInformation.AssertWasCalled(x => x.RouteTo(nodeInformation, _lvl1Node.Layer, _lvl1Node.Row, _lvl1Node.Column));
		}


		[Test]
		public void one_target_non_claimable()
		{	
			_lvl1Node = GetNodeInformation(new List<NodeEffect>(), new Dictionary<NodeExit, INodeInformation>(), 1, 2, 3, false, _owner2);
		
			var exits = new Dictionary<NodeExit, INodeInformation> { { NodeExit.Alpha, _lvl1Node } };
			INodeInformation nodeInformation = GetNodeInformation(new List<NodeEffect>(), exits, 0, 1, 2, false, myPlayer);
		
			IAgentUpdateInfo agentUpdate = GetAgentUpdate(nodeInformation);
			var result = MoveSkills.GreedyMoveToFirstClaimable(agentUpdate, _agent);
			nodeInformation.AssertWasCalled(x => x.RouteTo(nodeInformation, _lvl1Node.Layer, _lvl1Node.Row, _lvl1Node.Column));
		}



        [Test]
        public void two_target_claimable_2nd_with_owner()
        {
            IPlayer nodeOwner = MockRepository.GenerateMock<IPlayer>();
            nodeOwner.Stub(x => x.Id).Return(Guid.NewGuid());
            nodeOwner.Stub(x => x.DisplayHandle).Return("nodeowner");

            var lvl2Node = GetNodeInformation(new List<NodeEffect>(), new Dictionary<NodeExit, INodeInformation>(), 2, 3, 4, true, nodeOwner);

            _lvl1Node = GetNodeInformation(new List<NodeEffect>(), new Dictionary<NodeExit, INodeInformation>(), 1, 2, 3, true, uid);

            var exits = new Dictionary<NodeExit, INodeInformation> { { NodeExit.Alpha, _lvl1Node }, { NodeExit.Theta, lvl2Node } };
            
            INodeInformation nodeInformation = GetNodeInformation(new List<NodeEffect>(), exits, 0, 1, 2, false, myPlayer);

            IAgentUpdateInfo agentUpdate = GetAgentUpdate(nodeInformation);
            var result = MoveSkills.GreedyMoveToFirstClaimable(agentUpdate, _agent);
            nodeInformation.AssertWasCalled(x => x.RouteTo(nodeInformation, lvl2Node.Layer, lvl2Node.Row, lvl2Node.Column));
        }

		[Test]
		public void lvl2_claimable()
		{
			INodeInformation lvl2Node = GetNodeInformation(new List<NodeEffect>(), new Dictionary<NodeExit, INodeInformation>(), 2, 3, 4, true, _owner2);
			
			Dictionary<NodeExit, INodeInformation> lvl1Exits = new Dictionary<NodeExit, INodeInformation>
			                                                   	{{new NodeExit(), lvl2Node}};
			_lvl1Node = GetNodeInformation(new List<NodeEffect>(), lvl1Exits, 1, 2, 3, false, _owner2);
			
			var exits = new Dictionary<NodeExit, INodeInformation> { { NodeExit.Alpha, _lvl1Node } };
			INodeInformation nodeInformation = GetNodeInformation(new List<NodeEffect>(), exits, 0, 1, 2, false, myPlayer);

			IAgentUpdateInfo agentUpdate = GetAgentUpdate(nodeInformation);
			var result = MoveSkills.GreedyMoveToFirstClaimable(agentUpdate, _agent);
			nodeInformation.AssertWasCalled(x => x.RouteTo(nodeInformation, lvl2Node.Layer, lvl2Node.Row, lvl2Node.Column));
		}

		[Test]
		public void lvl2_claimable_parent_claimable()
		{
			INodeInformation lvl2node = GetNodeInformation(new List<NodeEffect>(), new Dictionary<NodeExit, INodeInformation>(), 2, 3, 4, true, _owner2);

			Dictionary<NodeExit, INodeInformation> lvl1Exits = new Dictionary<NodeExit, INodeInformation> { { new NodeExit(), lvl2node } };
			_lvl1Node = GetNodeInformation(new List<NodeEffect>(), lvl1Exits, 1, 2, 3, true, _owner2);

			var exits = new Dictionary<NodeExit, INodeInformation> { { NodeExit.Alpha, _lvl1Node } };
			INodeInformation nodeInformation = GetNodeInformation(new List<NodeEffect>(), exits, 0, 1, 2, false, myPlayer);

			IAgentUpdateInfo agentUpdate = GetAgentUpdate(nodeInformation);
			var result = MoveSkills.GreedyMoveToFirstClaimable(agentUpdate, _agent);
			nodeInformation.AssertWasCalled(x => x.RouteTo(nodeInformation, _lvl1Node.Layer, _lvl1Node.Row, _lvl1Node.Column));
		}

		[Test]
		public void lvl2_claimable_not_from_first_node()
		{
			INodeInformation lvl2node = GetNodeInformation(new List<NodeEffect>(), new Dictionary<NodeExit, INodeInformation>(), 2, 3, 4, true, _owner2);

			Dictionary<NodeExit, INodeInformation> lvl1Exits = new Dictionary<NodeExit, INodeInformation> { { new NodeExit(), lvl2node } };
			_lvl1Node = GetNodeInformation(new List<NodeEffect>(), new Dictionary<NodeExit, INodeInformation>(), 1, 2, 3, false, _owner2);
			var _lvl1Node2 = GetNodeInformation(new List<NodeEffect>(), lvl1Exits, 1, 2, 3, false, _owner2);

			var exits = new Dictionary<NodeExit, INodeInformation> { { NodeExit.Alpha, _lvl1Node }, {NodeExit.Zeta, _lvl1Node2} };
			INodeInformation nodeInformation = GetNodeInformation(new List<NodeEffect>(), exits, 0, 1, 2, false, myPlayer);

			IAgentUpdateInfo agentUpdate = GetAgentUpdate(nodeInformation);
			var result = MoveSkills.GreedyMoveToFirstClaimable(agentUpdate, _agent);
			nodeInformation.AssertWasCalled(x => x.RouteTo(nodeInformation, lvl2node.Layer, lvl2node.Row, lvl2node.Column));
		}


        //[Test]
        //public void lvl2_claimable_not_from_first_node_same_owner()
        //{
        //    INodeInformation lvl2node = GetNodeInformation(new List<NodeEffect>(), new Dictionary<NodeExit, INodeInformation>(), 2, 3, 4, true, _owner2);

        //    Dictionary<NodeExit, INodeInformation> lvl1Exits = new Dictionary<NodeExit, INodeInformation> { { new NodeExit(), lvl2node } };
        //    _lvl1Node = GetNodeInformation(new List<NodeEffect>(), new Dictionary<NodeExit, INodeInformation>(), 1, 2, 3, false, myPlayer);
        //    var lvl1Node2 = GetNodeInformation(new List<NodeEffect>(), lvl1Exits, 1, 2, 3, false, _owner2);

        //    var exits = new Dictionary<NodeExit, INodeInformation> { { NodeExit.Alpha, _lvl1Node }, { NodeExit.Zeta, lvl1Node2 } };
        //    INodeInformation nodeInformation = GetNodeInformation(new List<NodeEffect>(), exits, 0, 1, 2, false, myPlayer);

        //    IAgentUpdateInfo agentUpdate = GetAgentUpdate(nodeInformation);
        //    var result = MoveSkills.GreedyMoveToFirstClaimable(agentUpdate, _agent);

        //    nodeInformation.AssertWasCalled(x => x.RouteTo(nodeInformation, _lvl1Node.Layer, _lvl1Node.Row, _lvl1Node.Column));
        //}

		[Test]
		public void lvl3_claimable()
		{
			var lvl2Exists = new Dictionary<NodeExit, INodeInformation>();
			INodeInformation lvl3Node = GetNodeInformation(new List<NodeEffect>(), lvl2Exists, 3, 4, 5, true, _owner2);
			lvl2Exists.Add(NodeExit.Alpha,lvl3Node);
			INodeInformation lvl2Node = GetNodeInformation(new List<NodeEffect>(), lvl2Exists, 2, 3, 4, false, _owner2);

			Dictionary<NodeExit, INodeInformation> lvl1Exits = new Dictionary<NodeExit, INodeInformation> { { new NodeExit(), lvl2Node } };
			_lvl1Node = GetNodeInformation(new List<NodeEffect>(), lvl1Exits, 1, 2, 3, false, _owner2);

			var exits = new Dictionary<NodeExit, INodeInformation> { { NodeExit.Alpha, _lvl1Node } };
			INodeInformation nodeInformation = GetNodeInformation(new List<NodeEffect>(), exits, 0, 1, 2, false, myPlayer);
			
			IAgentUpdateInfo agentUpdate = GetAgentUpdate(nodeInformation);
			var result = MoveSkills.GreedyMoveToFirstClaimable(agentUpdate, _agent);
			nodeInformation.AssertWasCalled(x => x.RouteTo(nodeInformation, lvl3Node.Layer, lvl3Node.Row, lvl3Node.Column));
		}
		
		IGAgent _agent;
		IDeck _deck;
		IPlayer _owner;
		INodeInformation _lvl1Node;
		IPlayer _owner2;
	    private IPlayer myPlayer;
        IPlayer uid;

		private IAgentUpdateInfo GetAgentUpdate(INodeInformation nodeInformation)
		{
			IAgentUpdateInfo agentUpdate = MockRepository.GenerateMock<IAgentUpdateInfo>();
			agentUpdate.Stub(x => x.Owner).Return(_owner);
			agentUpdate.Stub(x => x.Node).Return(nodeInformation);
			agentUpdate.Stub(x => x.Effects).Return(new List<AgentEffect>());
			return agentUpdate;
		}

		private static INodeInformation GetNodeInformation(IEnumerable<NodeEffect> nodeEfx, Dictionary<NodeExit, INodeInformation> exists, int row, int layer, int column, bool claimable, IPlayer player)
		{
			var nodeInformation = MockRepository.GenerateMock<INodeInformation>();
			nodeInformation.Stub(x => x.Exits).Return(exists);
			nodeInformation.Stub(x => x.Effects).Return(nodeEfx);
			nodeInformation.Stub(x => x.Row).Return(row);
			nodeInformation.Stub(x => x.Layer).Return(layer);
			nodeInformation.Stub(x => x.Column).Return(column);
			nodeInformation.Stub(x => x.IsClaimable).Return(claimable);
			nodeInformation.Stub(x => x.Owner).Return(player);
		
			return nodeInformation;
		}
	}
}