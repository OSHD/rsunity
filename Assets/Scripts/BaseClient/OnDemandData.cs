using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS2Sharp
{
	// Decompiled by Jad v1.5.8f. Copyright 2001 Pavel Kouznetsov.
	// Jad home page: http://www.kpdus.com/jad.html
	// Decompiler options: packimports(3) 


	public class OnDemandData : NodeSub
	{

		public OnDemandData()
		{
			incomplete = true;
		}

		public int dataType;
		public byte[] buffer;
		public int ID;
		public bool incomplete;
		public int loopCycle;
	}
}
