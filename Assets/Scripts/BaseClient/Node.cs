using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS2Sharp
{
	public class Node
	{
		public void unlink()
		{
			if (next == null)
			{
			}
			else
			{
				//try{
				next.prev = prev;
				prev.next = next;
				prev = null;
				next = null;
				//}
				//catch(Exception ex){}
			}
		}

		public Node()
		{
		}

		public long id;
		public Node prev;
		public Node next;
	}
}
