using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS2Sharp
{
	// Decompiled by Jad v1.5.8f. Copyright 2001 Pavel Kouznetsov.
	// Jad home page: http://www.kpdus.com/jad.html
	// Decompiler options: packimports(3) 


	public class WallDecoration
	{

		public WallDecoration()
		{
		}

		public int anInt499;
		public int anInt500;
		public int anInt501;
		public int anInt502;
		public int anInt503;
		public Animable aClass30_Sub2_Sub4_504;
		public int uid;
		public sbyte aByte506;
		public RuneObject runeObj;
		public Ground tile;
		
		public int spawnType;
		public int centre;
		public int east;
		public int northEast;
		public int north;
		public int id;
		public int anim;
		public int type;
		private int attributes;
		
		private byte config;
		private int height;
		private int key;
		private int orientation;
		public Animable renderable;
		private int x;
		private int y;
		
		public int getAttributes() {
			return attributes;
		}
		
		public byte getConfig() {
			return config;
		}
		
		public int getHeight() {
			return height;
		}
		
		public int getKey() {
			return key;
		}
		
		public int getOrientation() {
			return orientation;
		}
		
		public Animable getRenderable() {
			return renderable;
		}
		
		public int getX() {
			return x;
		}
		
		public int getY() {
			return y;
		}
		
		public void setAttributes(int attributes) {
			this.attributes = attributes;
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
		
		public void setOrientation(int orientation) {
			this.orientation = orientation;
		}
		
		public void setRenderable(Animable renderable) {
			this.renderable = renderable;
		}
		
		public void setX(int x) {
			this.x = x;
		}
		
		public void setY(int y) {
			this.y = y;
		}
	}

}
