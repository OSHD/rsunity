using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS2Sharp
{
	// Decompiled by Jad v1.5.8f. Copyright 2001 Pavel Kouznetsov.
	// Jad home page: http://www.kpdus.com/jad.html
	// Decompiler options: packimports(3) 

	public class Entity : Animable
	{

		public void setPos(int i, int j, bool flag)
		{
			if (anim != -1 && Animation.anims[anim].anInt364 == 1)
				anim = -1;
			if (!flag)
			{
				int k = i - smallX[0];
				int l = j - smallY[0];
				if (k >= -8 && k <= 8 && l >= -8 && l <= 8)
				{
					if (smallXYIndex < 9)
						smallXYIndex++;
					for (int i1 = smallXYIndex; i1 > 0; i1--)
					{
						smallX[i1] = smallX[i1 - 1];
						smallY[i1] = smallY[i1 - 1];
						aBooleanArray1553[i1] = aBooleanArray1553[i1 - 1];
					}

					smallX[0] = i;
					smallY[0] = j;
					aBooleanArray1553[0] = false;
					return;
				}
			}
			smallXYIndex = 0;
			anInt1542 = 0;
			anInt1503 = 0;
			smallX[0] = i;
			smallY[0] = j;
			x = smallX[0] * 128 + anInt1540 * 64;
			y = smallY[0] * 128 + anInt1540 * 64;
		}

		public void method446()
		{
			smallXYIndex = 0;
			anInt1542 = 0;
		}

//		public void updateHitData(int j, int k, int l)
//		{
//			for (int i1 = 0; i1 < 4; i1++)
//				if (hitsLoopCycle[i1] <= l)
//				{
//					hitArray[i1] = k;
//					hitMarkTypes[i1] = j;
//					hitsLoopCycle[i1] = l + 70;
//					return;
//				}
//		}

		public void updateHitData(int j, int k, int z, int l)
		{
//			for(int i1 = 0; i1 < 4; i1++)
//				if(hitsLoopCycle[i1] <= l)
//			{
//				hitType[i1] = z;
//				hitArray[i1] = k * ((client.newDamage == true && k > 0) ? 10 : 1);
//				if (client.newDamage && k > 0) {
//					hitArray[i1] += RS2Sharp.Random.Next (9);
//				}
//				hitMarkTypes[i1] = j;
//				hitsLoopCycle[i1] = l + 70;
//				return;
//			}
		}

		public void moveInDir(bool flag, int i)
		{
			int j = smallX[0];
			int k = smallY[0];
			if (i == 0)
			{
				j--;
				k++;
			}
			if (i == 1)
				k++;
			if (i == 2)
			{
				j++;
				k++;
			}
			if (i == 3)
				j--;
			if (i == 4)
				j++;
			if (i == 5)
			{
				j--;
				k--;
			}
			if (i == 6)
				k--;
			if (i == 7)
			{
				j++;
				k--;
			}
			if (anim != -1 && Animation.anims[anim].anInt364 == 1)
				anim = -1;
			if (smallXYIndex < 9)
				smallXYIndex++;
			for (int l = smallXYIndex; l > 0; l--)
			{
				smallX[l] = smallX[l - 1];
				smallY[l] = smallY[l - 1];
				aBooleanArray1553[l] = aBooleanArray1553[l - 1];
			}
			smallX[0] = j;
			smallY[0] = k;
			aBooleanArray1553[0] = flag;
		}

		public int entScreenX;
		public int entScreenY;
		public int index = -1;
		public virtual bool isVisible()
		{
			return false;
		}

		public Entity() : base()
		{
			smallX = new int[10];
			smallY = new int[10];
			interactingEntity = -1;
			anInt1504 = 32;
			anInt1505 = -1;
			height = 200;
			anInt1511 = -1;
			anInt1512 = -1;
			hitArray = new int[4];
			hitMarkTypes = new int[4];
			hitsLoopCycle = new int[4];
			hitType = new int[4];
			anInt1517 = -1;
			anInt1520 = -1;
			anim = -1;
			loopCycleStatus = -1000;
			textCycle = 100;
			anInt1540 = 1;
			aBoolean1541 = false;
			aBooleanArray1553 = new bool[10];
			anInt1554 = -1;
			anInt1555 = -1;
			anInt1556 = -1;
			anInt1557 = -1;
		}
	public int nextAnimationFrame;
	public int nextGraphicsAnimationFrame;
	public int nextIdleAnimationFrame;
		public int[] smallX;
		public int[] smallY;
		public int interactingEntity;
		public int anInt1503;
		public int anInt1504;
		public int anInt1505;
		public String textSpoken;
		public int height;
		public int turnDirection;
		public int anInt1511;
		public int anInt1512;
		public int anInt1513;
		public int[] hitType;
		public int[] hitArray;
		public int[] hitMarkTypes;
		public int[] hitsLoopCycle;
		public int anInt1517;
		public int anInt1518;
		public int anInt1519;
		public int anInt1520;
		public int anInt1521;
		public int anInt1522;
		public int anInt1523;
		public int anInt1524;
		public int smallXYIndex;
		public int anim;
		public int anInt1527;
		public int anInt1528;
		public int anInt1529;
		public int anInt1530;
		public int anInt1531;
		public int loopCycleStatus;
		public int currentHealth;
		public int maxHealth;
		public int textCycle;
		public int anInt1537;
		public int anInt1538;
		public int anInt1539;
		public int anInt1540;
		public bool aBoolean1541;
		public int anInt1542;
		public int anInt1543;
		public int anInt1544;
		public int anInt1545;
		public int anInt1546;
		public int anInt1547;
		public int anInt1548;
		public int anInt1549;
		public int x;
		public int y;
		public int anInt1552;
		public bool[] aBooleanArray1553;
		public int anInt1554;
		public int anInt1555;
		public int anInt1556;
		public int anInt1557;
	}
}
