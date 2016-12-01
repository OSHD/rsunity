using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS2Sharp
{
	// Decompiled by Jad v1.5.8f. Copyright 2001 Pavel Kouznetsov.
	// Jad home page: http://www.kpdus.com/jad.html
	// Decompiler options: packimports(3) 

	public class Item : Animable
	{

		public override Model getRotatedModel()
		{
			ItemDef itemDef = ItemDef.forID(ID);
			return itemDef.method201(anInt1559);
		}

		public Item()
		{
		}

		public int ID;
		public int x;
		public int y;
		public int anInt1559;
	}
}
