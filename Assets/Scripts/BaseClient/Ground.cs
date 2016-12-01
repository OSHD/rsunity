using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS2Sharp
{
	// Decompiled by Jad v1.5.8f. Copyright 2001 Pavel Kouznetsov.
	// Jad home page: http://www.kpdus.com/jad.html
	// Decompiler options: packimports(3) 


	public class Ground : Node
	{

		public Ground(int i, int j, int k)
		{
			interactableObjects = new InteractiveObject[5];
			anIntArray1319 = new int[5];
			plane = anInt1307 = i;
			positionX = j;
			positionY = k;
		}

		public int anInt1307;
		public int positionX;
		public int positionY;
		public int plane;
		public SimpleTile myPlainTile;
		public ComplexTile shapedTile;
		public WallObject wall;
		public WallDecoration wallDecoration;
		public GroundDecoration groundDecoration;
		public Object4 groundItemTile;
		public int objectCount;
		public InteractiveObject[] interactableObjects;
		public int[] anIntArray1319;
		public int anInt1320;
		public int anInt1321;
		public bool aBoolean1322;
		public bool aBoolean1323;
		public bool aBoolean1324;
		public int anInt1325;
		public int anInt1326;
		public int anInt1327;
		public int anInt1328;
		public Ground tileBelow0;
	}
}
