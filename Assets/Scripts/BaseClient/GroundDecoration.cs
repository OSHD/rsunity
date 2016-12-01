using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS2Sharp
{
	// Decompiled by Jad v1.5.8f. Copyright 2001 Pavel Kouznetsov.
	// Jad home page: http://www.kpdus.com/jad.html
	// Decompiler options: packimports(3) 


	public class GroundDecoration
	{

		public GroundDecoration()
		{
		}

		public int anInt811;
		public int anInt812;
		public int anInt813;
		public Animable aClass30_Sub2_Sub4_814;
		public int uid;
		public sbyte aByte816;
		public RuneObject runeObj;
		public Ground tile;
		
		
		public int spawnType;
		public int orientation;
		public int centre;
		public int east;
		public int northEast;
		public int north;
		public int id;
		public int anim;
		public int type;
		
		/**
	 * A packed config value containing the type and orientation of this decoration, in the form
	 * {@code (orientation << 6) | type}.
	 */
		private byte config; // (orientation << 6) | type
		
		/**
	 * The draw height of this decoration.
	 */
		private int height;
		
		/**
	 * The key of this decoration.
	 */
		private int key; // 0x4_000_0000 | (id << 14) | y << 7 | x (and if it's not interactive, | 0x8_000_000)
		
		/**
	 * The renderable of this decoration.
	 */
		public Animable renderable;
		
		/**
	 * The x coordinate of this decoration.
	 */
		private int x;
		
		/**
	 * The y coordinate of this decoration.
	 */
		private int y;
		
		/**
	 * A packed config value containing the type and orientation of this decoration, in the form
	 * {@code (orientation << 6) | type}.
	 */
		public byte getConfig() {
			return config;
		}
		
		/**
	 * The draw height of this decoration.
	 */
		public int getHeight() {
			return height;
		}
		
		/**
	 * The key of this decoration.
	 */
		public int getKey() {
			return key;
		}
		
		/**
	 * The x coordinate of this decoration.
	 */
		public int getX() {
			return x;
		}
		
		/**
	 * The y coordinate of this decoration.
	 */
		public int getY() {
			return y;
		}
		
		public void setConfig(byte config) {
			this.config = config;
		}
		
		public void setHeight(int height) {
			this.height = height;
		}
		
		public void setKey(int key) {
			this.key = key;
		}
		
		public void setRenderable(Animable renderable) {
			this.renderable = renderable;
		}
		
		public Animable getRenderable()
		{
			return this.renderable;
		}
		
		public void setX(int x) {
			this.x = x;
		}
		
		public void setY(int y) {
			this.y = y;
		}
		
	}
}
