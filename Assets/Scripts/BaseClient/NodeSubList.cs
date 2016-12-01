using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS2Sharp
{
	public class NodeSubList
	{
		public NodeSubList()
		{
			head = new NodeSub();
			head.prevNodeSub = head;
			head.nextNodeSub = head;
		}

		public void insertHead(NodeSub nodeSub)
		{
			if (nodeSub.nextNodeSub != null)
				nodeSub.unlinkSub();
			nodeSub.nextNodeSub = head.nextNodeSub;
			nodeSub.prevNodeSub = head;
			nodeSub.nextNodeSub.prevNodeSub = nodeSub;
			nodeSub.prevNodeSub.nextNodeSub = nodeSub;
		}

		public NodeSub popTail()
		{
			NodeSub nodeSub = head.prevNodeSub;
			if (nodeSub == head)
			{
				return null;
			}
			else
			{
				nodeSub.unlinkSub();
				return nodeSub;
			}
		}

		public NodeSub reverseGetFirst()
		{
			NodeSub nodeSub = head.prevNodeSub;
			if (nodeSub == head)
			{
				current = null;
				return null;
			}
			else
			{
				current = nodeSub.prevNodeSub;
				return nodeSub;
			}
		}

		public NodeSub reverseGetNext()
		{
			NodeSub nodeSub = current;
			if (nodeSub == head)
			{
				current = null;
				return null;
			}
			else
			{
				current = nodeSub.prevNodeSub;
				return nodeSub;
			}
		}

		public int getNodeCount()
		{
			int i = 0;
			for (NodeSub nodeSub = head.prevNodeSub; nodeSub != head; nodeSub = nodeSub.prevNodeSub)
				i++;

			return i;
		}

		private NodeSub head;
		private NodeSub current;
	}
}
