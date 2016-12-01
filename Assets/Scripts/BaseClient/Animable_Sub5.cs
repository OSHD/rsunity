using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RS2Sharp
{
	// Decompiled by Jad v1.5.8f. Copyright 2001 Pavel Kouznetsov.
	// Jad home page: http://www.kpdus.com/jad.html
	// Decompiler options: packimports(3) 

	public class Animable_Sub5 : Animable
	{

		public override Model getRotatedModel()
		{
			int j = -1;
			if (aAnimation_1607 != null) {
				int k = UnityClient.loopCycle - anInt1608;
				if (k > 100 && aAnimation_1607.anInt356 > 0)
					k = 100;
				while (k > aAnimation_1607.duration(anInt1599)) {
					k -= aAnimation_1607.duration(anInt1599);
					anInt1599++;
					if (anInt1599 < aAnimation_1607.frameCount)
						continue;
					anInt1599 -= aAnimation_1607.anInt356;
					if (anInt1599 >= 0 && anInt1599 < aAnimation_1607.frameCount)
						continue;
					aAnimation_1607 = null;
					break;
				}
				anInt1608 = UnityClient.loopCycle - k;
				if (aAnimation_1607 != null)
					j = aAnimation_1607.anIntArray353[anInt1599];
			}
			ObjectDef class46;
			if (anIntArray1600 != null)
				class46 = method457();
			else
				class46 = ObjectDef.forID(myId);
			if (class46 == null) {
				return null;
			} else {
				Model theModel = class46.method578(anInt1611, anInt1612, anInt1603,
				                         anInt1604, anInt1605, anInt1606, j);
				return theModel;
			}
		}

		private ObjectDef method457()
		{
			int i = -1;
			if (anInt1601 != -1)
			{
				try{
				VarBit varBit = VarBit.cache[anInt1601];
				int k = varBit.anInt648;
				int l = varBit.anInt649;
				int i1 = varBit.anInt650;
					int j1 = UnityClient.anIntArray1232[i1 - l];
				i = clientInstance.variousSettings[k] >> l & j1;
				} catch(Exception ex) {}
			}
//			else
//				if (anInt1602 != -1)
//					i = clientInstance.variousSettings[anInt1602];
			if (i < 0 || i >= anIntArray1600.Length || anIntArray1600[i] == -1)
				return null;
			else
				return ObjectDef.forID(anIntArray1600[i]);
		}

		public Animable_Sub5(int i, int j, int k, int l, int i1, int j1,
							 int k1, int l1, bool flag) : base()
		{
			myId = i;
			anInt1611 = k;
			anInt1612 = j;
			anInt1603 = j1;
			anInt1604 = l;
			anInt1605 = i1;
			anInt1606 = k1;
			try{
			if (l1 != -1)
			{
				aAnimation_1607 = Animation.anims[l1];
				anInt1599 = 0;
					anInt1608 = UnityClient.loopCycle;
				if (flag && aAnimation_1607.anInt356 != -1)
				{
					anInt1599 = (int)(Random.Next(aAnimation_1607.frameCount));
					anInt1608 -= (int)(Random.Next(aAnimation_1607.duration(anInt1599)));
				}
			}
			}
			catch(Exception ex)
			{
				Debug.Log("Animable Sub5 Error : " + ex.Message + " : " + ex.StackTrace);
			}
			ObjectDef class46 = ObjectDef.forID(myId);
			anInt1601 = class46.anInt774;
			anInt1602 = class46.anInt749;
			anIntArray1600 = class46.childrenIDs;
		}

		private int anInt1599;
		private int[] anIntArray1600;
		private int anInt1601;
		private int anInt1602;
		private int anInt1603;
		private int anInt1604;
		private int anInt1605;
		private int anInt1606;
		private Animation aAnimation_1607;
		private int anInt1608;
		public static UnityClient clientInstance;
		public int myId;
		private int anInt1611;
		private int anInt1612;
	}
}
