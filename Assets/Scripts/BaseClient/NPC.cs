using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS2Sharp
{
	// Decompiled by Jad v1.5.8f. Copyright 2001 Pavel Kouznetsov.
	// Jad home page: http://www.kpdus.com/jad.html
	// Decompiler options: packimports(3) 

	public class NPC : Entity
	{
//
//		private Model method450()
//		{
//			if (base.anim >= 0 && base.anInt1529 == 0)
//			{
//				int k = Animation.anims[base.anim].anIntArray353[base.anInt1527];
//				int i1 = -1;
//				if (base.anInt1517 >= 0 && base.anInt1517 != base.anInt1511)
//					i1 = Animation.anims[base.anInt1517].anIntArray353[base.anInt1518];
//				return desc.method164(i1, k, Animation.anims[base.anim].anIntArray357);
//			}
//			int l = -1;
//			if (base.anInt1517 >= 0)
//				l = Animation.anims[base.anInt1517].anIntArray353[base.anInt1518];
//			return desc.method164(-1, l, null);
//		}
//
//		public override Model getRotatedModel()
//		{
//			if (desc == null)
//				return null;
//			Model model = method450();
//			if (model == null)
//				return null;
//			base.height = model.modelHeight;
//			if (base.anInt1520 != -1 && base.anInt1521 != -1)
//			{
//				SpotAnim spotAnim = SpotAnim.cache[base.anInt1520];
//				Model model_1 = spotAnim.getModel();
//				if (model_1 != null)
//				{
//					int j = spotAnim.aAnimation_407.anIntArray353[base.anInt1521];
//					Model model_2 = new Model(true, Class36.method532(j), false, model_1);
//					model_2.method475(0, -base.anInt1524, 0);
//					model_2.method469();
//					model_2.method470(j);
//					model_2.anIntArrayArray1658 = null;
//					model_2.anIntArrayArray1657 = null;
//					if (spotAnim.anInt410 != 128 || spotAnim.anInt411 != 128)
//						model_2.method478(spotAnim.anInt410, spotAnim.anInt410, spotAnim.anInt411);
//					model_2.method479(64 + spotAnim.anInt413, 850 + spotAnim.anInt414, -30, -50, -30, true);
//					Model[] aModel = {
//                        model, model_2
//                };
//					model = new Model(aModel);
//				}
//			}
//			if (desc.aByte68 == 1)
//				model.aBoolean1659 = true;
//			return model;
//		}
//
//		public override bool isVisible()
//		{
//			return desc != null;
//		}
//
//		public NPC()
//		{
//			npcObj = new RuneObject (this);
//		}
//
//		public RuneObject npcObj;
//		public EntityDef desc;
		
		private Model method450()
		{
			if(base.anim >= 0 && base.anInt1529 == 0)
			{
				Animation animation = Animation.anims[base.anim];
				int currentFrame = animation.anIntArray353[base.anInt1527];
				int nextFrame = animation.anIntArray353[base.nextAnimationFrame];
				int cycle1 = animation.anIntArray355[base.anInt1527];
				int cycle2 = base.anInt1528;
				//int frame = Animation.anims[base.anim].anIntArray353[base.anInt1527];
				int i1 = -1;
				if(base.anInt1517 >= 0 && base.anInt1517 != base.anInt1511)
					i1 = Animation.anims[base.anInt1517].anIntArray353[base.anInt1518];
				return desc.method164(i1, currentFrame, Animation.anims[base.anim].anIntArray357, nextFrame, cycle1, cycle2);
			}
			int currentFrame2 = -1;
			int nextFrame2 = -1;
			int cycle11 = 0;
			int cycle22 = 0;
			if(base.anInt1517 >= 0) {
				Animation animation = Animation.anims[base.anInt1517];
				currentFrame2 = animation.anIntArray353[base.anInt1518];
				nextFrame2 = animation.anIntArray353[base.nextIdleAnimationFrame];
				cycle11 = animation.anIntArray355[base.anInt1518];
				cycle22 = base.anInt1519;
			}
			return desc.method164(-1, currentFrame2, null, nextFrame2, cycle11, cycle22);
		}
		
		public override Model getRotatedModel()
		{
			if(desc == null)
				return null;
			Model model = method450();
			if(model == null)
				return null;
			base.height = model.modelHeight;
			if(base.anInt1520 != -1 && base.anInt1521 != -1)
			{
				SpotAnim spotAnim = SpotAnim.cache[base.anInt1520];
				Model model_1 = spotAnim.getModel();
				model_1.isGfx = true;
				if(model_1 != null)
				{
					int j = spotAnim.aAnimation_407.anIntArray353[base.anInt1521];
					int nextFrame = spotAnim.aAnimation_407.anIntArray353[base.nextGraphicsAnimationFrame];
					int cycle1 = spotAnim.aAnimation_407.anIntArray355[base.anInt1521];
					int cycle2 = base.anInt1522;
					Model model_2 = new Model(true, Class36.method532(j), false, model_1);
					model_2.isGfx = true;
					model_2.method475(0, -base.anInt1524, 0);
					model_2.method469();
					model_2.method470(j, nextFrame, cycle1, cycle2);
					model_2.anIntArrayArray1658 = null;
					model_2.anIntArrayArray1657 = null;
					if(spotAnim.anInt410 != 128 || spotAnim.anInt411 != 128)
						model_2.method478(spotAnim.anInt410, spotAnim.anInt410, spotAnim.anInt411);
					model_2.method479(64 + spotAnim.anInt413, 850 + spotAnim.anInt414, -30, -50, -30, true);
					Model[] aModel = {
						model, model_2
					};
					model = new Model(aModel);
					model.isGfx = true;
				}
			}
			if(desc.aByte68 == 1)
				model.aBoolean1659 = true;
			return model;
		}
		public override bool isVisible() {
			return desc != null;
		}
		
		public NPC() {
			npcObj = new RuneObject (this);
		}
		public RuneObject npcObj;
		public EntityDef desc;
	}
}
