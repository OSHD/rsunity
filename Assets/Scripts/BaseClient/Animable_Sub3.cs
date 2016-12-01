using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS2Sharp
{
	// Decompiled by Jad v1.5.8f. Copyright 2001 Pavel Kouznetsov.
	// Jad home page: http://www.kpdus.com/jad.html
	// Decompiler options: packimports(3) 

	public class Animable_Sub3 : Animable
	{

		public Animable_Sub3(int i, int j, int l, int i1, int j1, int k1,
							 int l1)
			: base()
		{
			aBoolean1567 = false;
			aSpotAnim_1568 = SpotAnim.cache[i1];
			anInt1560 = i;
			anInt1561 = l1;
			anInt1562 = k1;
			anInt1563 = j1;
			anInt1564 = j + l;
			aBoolean1567 = false;
		}

		public override Model getRotatedModel()
		{
//			Model model = aSpotAnim_1568.getModel();
//			if (model == null)
//				return null;
//			int j = aSpotAnim_1568.aAnimation_407.anIntArray353[anInt1569];
//			Model model_1 = new Model(true, Class36.method532(j), false, model);
//			if (!aBoolean1567)
//			{
//				model_1.method469();
//				model_1.method470(j);
//				model_1.anIntArrayArray1658 = null;
//				model_1.anIntArrayArray1657 = null;
//			}
//			if (aSpotAnim_1568.anInt410 != 128 || aSpotAnim_1568.anInt411 != 128)
//				model_1.method478(aSpotAnim_1568.anInt410, aSpotAnim_1568.anInt410, aSpotAnim_1568.anInt411);
//			if (aSpotAnim_1568.anInt412 != 0)
//			{
//				if (aSpotAnim_1568.anInt412 == 90)
//					model_1.method473();
//				if (aSpotAnim_1568.anInt412 == 180)
//				{
//					model_1.method473();
//					model_1.method473();
//				}
//				if (aSpotAnim_1568.anInt412 == 270)
//				{
//					model_1.method473();
//					model_1.method473();
//					model_1.method473();
//				}
//			}
//			model_1.method479(64 + aSpotAnim_1568.anInt413, 850 + aSpotAnim_1568.anInt414, -30, -50, -30, true);
//			return model_1;
			Model model = aSpotAnim_1568.getModel();
			if(model == null)
				return null;
			int j = aSpotAnim_1568.aAnimation_407.anIntArray353[anInt1569];
			//int nextFrame1 = aSpotAnim_1568.aAnimation_407.anIntArray353[nextFrame];
			int cycle1 = aSpotAnim_1568.aAnimation_407.anIntArray355[anInt1569];
			Model model_1 = new Model(true, Class36.method532(j), false, model);
			if(!aBoolean1567)
			{
				model_1.method469();
				model_1.method470(j);//, nextFrame1, cycle1, anInt1570);
				model_1.anIntArrayArray1658 = null;
				model_1.anIntArrayArray1657 = null;
			}
			if(aSpotAnim_1568.anInt410 != 128 || aSpotAnim_1568.anInt411 != 128)
				model_1.method478(aSpotAnim_1568.anInt410, aSpotAnim_1568.anInt410, aSpotAnim_1568.anInt411);
			if(aSpotAnim_1568.anInt412 != 0)
			{
				if(aSpotAnim_1568.anInt412 == 90)
					model_1.method473();
				if(aSpotAnim_1568.anInt412 == 180)
				{
					model_1.method473();
					model_1.method473();
				}
				if(aSpotAnim_1568.anInt412 == 270)
				{
					model_1.method473();
					model_1.method473();
					model_1.method473();
				}
			}
			model_1.method479(64 + aSpotAnim_1568.anInt413, 850 + aSpotAnim_1568.anInt414, -30, -50, -30, true);
			return model_1;
		}

		public void method454(int i)
		{
			for (anInt1570 += i; anInt1570 > aSpotAnim_1568.aAnimation_407.duration(anInt1569); )
			{
				anInt1570 -= aSpotAnim_1568.aAnimation_407.duration(anInt1569) + 1;
				anInt1569++;
				if (anInt1569 >= aSpotAnim_1568.aAnimation_407.frameCount && (anInt1569 < 0 || anInt1569 >= aSpotAnim_1568.aAnimation_407.frameCount))
				{
					anInt1569 = 0;
					aBoolean1567 = true;
				}
			}

		}

		public int anInt1560;
		public int anInt1561;
		public int anInt1562;
		public int anInt1563;
		public int anInt1564;
		public bool aBoolean1567;
		private SpotAnim aSpotAnim_1568;
		private int anInt1569;
		private int anInt1570;
	}
}
