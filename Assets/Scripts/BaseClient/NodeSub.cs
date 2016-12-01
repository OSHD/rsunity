using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS2Sharp
{
	public class NodeSub : Node
	{
		public void unlinkSub()
		{
			if (nextNodeSub == null)
			{
			}
			else
			{
				nextNodeSub.prevNodeSub = prevNodeSub;
				prevNodeSub.nextNodeSub = nextNodeSub;
				prevNodeSub = null;
				nextNodeSub = null;
			}
		}

		public NodeSub()
		{
		}

		public NodeSub prevNodeSub;
		public NodeSub nextNodeSub;
		public static int anInt1305;
	}
}
