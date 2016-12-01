using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS2Sharp
{
	// Decompiled by Jad v1.5.8f. Copyright 2001 Pavel Kouznetsov.
	// Jad home page: http://www.kpdus.com/jad.html
	// Decompiler options: packimports(3) 

	public class Projectile : Animable
	{

		public void target(int destX, int destY, int destZ, int endTick)
		{
			if (!mobile)
			{
				double dX = destX - sourceX;
				double dY = destY - sourceY;
				double distance = Math.Sqrt(dX * dX + dY * dY);
				x = (double)sourceX + (dX * (double)leapScale) / distance;
				y = (double)sourceY + (dY * (double)leapScale) / distance;
				z = sourceElevation;
			}
			double tick = (startTick + 1) - endTick;
			velocityX = ((double)destX - x) / tick;
			velocityY = ((double)destY - y) / tick;
			velocity = Math.Sqrt(velocityX * velocityX + velocityY * velocityY);
			if (!mobile)
				velocityZ = -velocity * Math.Tan((double)elevationPitch * 0.02454369D);
			acceleration = (2D * ((double)destZ - z - velocityZ * tick)) / (tick * tick);
		}

		public override Model getRotatedModel()
		{
			Model model = aSpotAnim_1592.getModel();
			if (model == null)
				return null;
			int j = -1;
			if (aSpotAnim_1592.aAnimation_407 != null)
				j = aSpotAnim_1592.aAnimation_407.anIntArray353[frameIndex];
			Model model_1 = new Model(true, Class36.method532(j), false, model);
			if (j != -1) {
				model_1.method469();
				model_1.method470(j);
				model_1.anIntArrayArray1658 = null;
				model_1.anIntArrayArray1657 = null;
			}
			if (aSpotAnim_1592.anInt410 != 128 || aSpotAnim_1592.anInt411 != 128)
				model_1.method478(aSpotAnim_1592.anInt410, aSpotAnim_1592.anInt410,
				                  aSpotAnim_1592.anInt411);
			model_1.method474(pitch);
			model_1.method479(64 + aSpotAnim_1592.anInt413,
			                  850 + aSpotAnim_1592.anInt414, -30, -50, -30, true);
			return model_1;
		}

		public Projectile(int i, int j, int l, int i1, int j1, int k1,
							 int l1, int i2, int j2, int k2, int l2)
			: base()
		{
			mobile = false;
			aSpotAnim_1592 = SpotAnim.cache[l2];
			plane = k1;
			sourceX = j2;
			sourceY = i2;
			sourceElevation = l1;
			endTick = l;
			startTick = i1;
			elevationPitch = i;
			leapScale = j1;
			target2 = k2;
			anInt1583 = j;
			mobile = false;
		}

		public void update(int time)
		{
			mobile = true;
			x += velocityX * (double)time;
			y += velocityY * (double)time;
			z += velocityZ * (double)time + 0.5D * acceleration * (double)time * (double)time;
			velocityZ += acceleration * (double)time;
			yaw = (int)(Math.Atan2(velocityX, velocityY) * 325.94900000000001D) + 1024 & 0x7ff;
			pitch = (int)(Math.Atan2(velocityZ, velocity) * 325.94900000000001D) & 0x7ff;
			if (aSpotAnim_1592.aAnimation_407 != null)
				for (elapsed += time; elapsed > aSpotAnim_1592.aAnimation_407.duration(frameIndex); )
				{
					elapsed -= aSpotAnim_1592.aAnimation_407.duration(frameIndex) + 1;
					frameIndex++;
					if (frameIndex >= aSpotAnim_1592.aAnimation_407.frameCount)
						frameIndex = 0;
				}

		}

		public int endTick;
		public int startTick;
		public InteractiveObject obj5;
		private double velocityX;
		private double velocityY;
		private double velocity;
		private double velocityZ;
		private double acceleration;
		private bool mobile;
		private int sourceX;
		private int sourceY;
		private int sourceElevation;
		public int anInt1583;
		public double x;
		public double y;
		public double z;
		private int elevationPitch;
		private int leapScale;
		public int target2;
		private SpotAnim aSpotAnim_1592;
		private int frameIndex;
		private int elapsed;
		public int yaw;
		private int pitch;
		public int plane;
	}
}
