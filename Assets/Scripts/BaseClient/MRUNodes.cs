using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sign;

namespace RS2Sharp
{
	public class MRUNodes
	{
		public MRUNodes(int i)
		{
			emptyNodeSub = new NodeSub();
			nodeSubList = new NodeSubList();
			initialCount = i;
			spaceLeft = i;
			nodeCache = new NodeCache();
		}

		public NodeSub insertFromCache(long l)
		{
			NodeSub nodeSub = (NodeSub)nodeCache.findNodeByID(l);
			if (nodeSub != null)
			{
				nodeSubList.insertHead(nodeSub);
			}
			return nodeSub;
		}

		public void removeFromCache(NodeSub nodeSub, long l)
		{
			try
			{
				if (spaceLeft == 0)
				{
					NodeSub nodeSub_1 = nodeSubList.popTail();
					nodeSub_1.unlink();
					nodeSub_1.unlinkSub();
					if (nodeSub_1 == emptyNodeSub)
					{
						NodeSub nodeSub_2 = nodeSubList.popTail();
						nodeSub_2.unlink();
						nodeSub_2.unlinkSub();
					}
				}
				else
				{
					spaceLeft--;
				}
				nodeCache.removeFromCache(nodeSub, l);
				nodeSubList.insertHead(nodeSub);
				return;
			}
			catch (Exception runtimeexception)
			{
				signlink.reporterror("47547, " + nodeSub + ", " + l + ", " + (byte)2 + ", " + runtimeexception.Message);
			}
			throw new Exception();
		}

		public void unlinkAll()
		{
			do
			{
				NodeSub nodeSub = nodeSubList.popTail();
				if (nodeSub != null)
				{
					nodeSub.unlink();
					nodeSub.unlinkSub();
				}
				else
				{
					spaceLeft = initialCount;
					return;
				}
			} while (true);
		}

		private NodeSub emptyNodeSub;
		private int initialCount;
		private int spaceLeft;
		private NodeCache nodeCache;
		private NodeSubList nodeSubList;
	}
}
