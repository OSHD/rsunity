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

	public class Animation
	{

/*//		public static void unpackConfig(StreamLoader streamLoader)
//		{
//			Stream stream = new Stream(streamLoader.getDataForName("seq.dat"));
//			int length = stream.readUnsignedWord() + 3299;
//			if (anims == null)
//				anims = new Animation[length];
//			for (int j = 0; j < length; j++)
//			{
//				if (anims[j] == null)
//					anims[j] = new Animation();
//				if (j < 3997)
//					anims[j].readValues(stream);
//				else
//					setAnimBase(j);
//				//anims[j].readValues(stream);
//			}
//		}
//
//		public static void setAnimBase(int j) {
//			anims[j].anInt352 = anims[808].anInt352;
//			anims[j].anIntArray353 = anims[808].anIntArray353;
//			anims[j].anIntArray354 = anims[808].anIntArray354;
//			anims[j].anIntArray355 = anims[808].anIntArray355;
//			anims[j].anInt356 = anims[808].anInt356;
//			anims[j].anIntArray357 = anims[808].anIntArray357;
//			anims[j].anInt359 = anims[808].anInt359;
//			anims[j].anInt360 = anims[808].anInt360;
//			anims[j].anInt361 = anims[808].anInt361;
//			anims[j].anInt362 = anims[808].anInt362;
//			anims[j].anInt363 = anims[808].anInt363;
//			anims[j].anInt364 = anims[808].anInt364;
//			anims[j].anInt365 = anims[808].anInt365;
//			anims[j].anInt352 = anims[808].anInt352;
//		}
//
//		public int method258(int i)
//		{
//			int j = anIntArray355[i];
//			if (j == 0)
//			{
//				Class36 class36 = Class36.method531(anIntArray353[i]);
//				if (class36 != null)
//					j = anIntArray355[i] = class36.anInt636;
//			}
//			if (j == 0)
//				j = 1;
//			return j;
//		}
//
//		private void readValues(Stream stream)
//		{
//			do
//			{
//				int i = stream.readUnsignedByte();
//				if (i == 0)
//					break;
//				if (i == 1)
//				{
//					anInt352 = stream.readUnsignedWord();
//					anIntArray353 = new int[anInt352];
//					anIntArray354 = new int[anInt352];
//					anIntArray355 = new int[anInt352];
////					for (int j = 0; j < anInt352; j++)
////					{
////						anIntArray353[j] = stream.readUnsignedWord();
////						anIntArray354[j] = stream.readUnsignedWord();
////						if (anIntArray354[j] == 65535)
////							anIntArray354[j] = -1;
////						anIntArray355[j] = stream.readUnsignedWord();
////					}
//					for(int i_ = 0; i_ < anInt352; i_++){
//						anIntArray353[i_] = stream.readDWord();
//						anIntArray354[i_] = -1;
//					}
//					for(int i_ = 0; i_ < anInt352; i_++)
//						anIntArray355[i_] = stream.readUnsignedByte();
//
//				}
//				else
//					if (i == 2)
//						anInt356 = stream.readUnsignedWord();
//					else
//						if (i == 3)
//						{
//							int k = stream.readUnsignedByte();
//							anIntArray357 = new int[k + 1];
//							for (int l = 0; l < k; l++)
//								anIntArray357[l] = stream.readUnsignedByte();
//
//							anIntArray357[k] = 0x98967f;
//						}
//						else
//							if (i == 4)
//								aBoolean358 = true;
//							else
//								if (i == 5)
//									anInt359 = stream.readUnsignedByte();
//								else
//									if (i == 6)
//										anInt360 = stream.readUnsignedWord();
//									else
//										if (i == 7)
//											anInt361 = stream.readUnsignedWord();
//										else
//											if (i == 8)
//												anInt362 = stream.readUnsignedByte();
//											else
//												if (i == 9)
//													anInt363 = stream.readUnsignedByte();
//												else
//													if (i == 10)
//														anInt364 = stream.readUnsignedByte();
//													else
//														if (i == 11)
//															anInt365 = stream.readUnsignedByte();
////														else
////															if (i == 12)
////																stream.readDWord();
//															else
//																Console.WriteLine("Error unrecognised seq config code: " + i);
//			} while (true);
//			if (anInt352 == 0)
//			{
//				anInt352 = 1;
//				anIntArray353 = new int[1];
//				anIntArray353[0] = -1;
//				anIntArray354 = new int[1];
//				anIntArray354[0] = -1;
//				anIntArray355 = new int[1];
//				anIntArray355[0] = -1;
//			}
//			if (anInt363 == -1)
//				if (anIntArray357 != null)
//					anInt363 = 2;
//				else
//					anInt363 = 0;
//			if (anInt364 == -1)
//			{
//				if (anIntArray357 != null)
//				{
//					anInt364 = 2;
//					return;
//				}
//				anInt364 = 0;
//			}
//		}
//
//		private Animation()
//		{
//			anInt356 = -1;
//			aBoolean358 = false;
//			anInt359 = 5;
//			anInt360 = -1;
//			anInt361 = -1;
//			anInt362 = 99;
//			anInt363 = -1;
//			anInt364 = -1;
//			anInt365 = 2;
//		}
//
//		public static Animation[] anims;
//		public int anInt352;
//		public int[] anIntArray353;
//		public int[] anIntArray354;
//		private int[] anIntArray355;
//		public int anInt356;
//		public int[] anIntArray357;
//		public bool aBoolean358;
//		public int anInt359;
//		public int anInt360;
//		public int anInt361;
//		public int anInt362;
//		public int anInt363;
//		public int anInt364;
//		public int anInt365;
//		public static int anInt367;*/
		
		public static int[] FrameStart = new int[1800];
		
		public static void unpackConfig(StreamLoader streamLoader) {
			for (int j = 0; j < FrameStart.Length; j++)
				FrameStart[j] = 0;
			Stream stream = new Stream(streamLoader.getDataForName("seq.dat"));
			int length = stream.readUnsignedWord() + 3299;
			if (anims == null)
				anims = new Animation[length];
			for (int j = 0; j < length; j++) {
				if (anims[j] == null)
					anims[j] = new Animation();
				if (j < 3997) {
					anims[j].readValues(stream);
				} else {
					setAnimBase(j);
				}
			}
		}
		
		public static void setAnimBase(int j) {
			anims[j].frameCount = anims[808].frameCount;
			anims[j].anIntArray353 = anims[808].anIntArray353;
			anims[j].anIntArray354 = anims[808].anIntArray354;
			anims[j].anIntArray355 = anims[808].anIntArray355;
			anims[j].anInt356 = anims[808].anInt356;
			anims[j].anIntArray357 = anims[808].anIntArray357;
			anims[j].anInt359 = anims[808].anInt359;
			anims[j].anInt360 = anims[808].anInt360;
			anims[j].anInt361 = anims[808].anInt361;
			anims[j].anInt362 = anims[808].anInt362;
			anims[j].anInt363 = anims[808].anInt363;
			anims[j].anInt364 = anims[808].anInt364;
			anims[j].anInt365 = anims[808].anInt365;
			anims[j].frameCount = anims[808].frameCount;
		}
		
		public int duration(int i) {
			int j = anIntArray355[i];
			if (j == 0) {
				Class36 class36 = Class36.method531(anIntArray353[i]);
				if (class36 != null)
					j = anIntArray355[i] = class36.anInt636;
			}
			if (j == 0)
				j = 1;
			return j;
		}
		
		public void readValues(Stream stream)
		{
			do {
				int i = stream.readUnsignedByte();
				if(i == 0)
					break;
				if(i == 1) {
					frameCount = stream.readUnsignedWord();
					anIntArray353 = new int[frameCount];
					anIntArray354 = new int[frameCount];
					anIntArray355 = new int[frameCount];
					for(int i_ = 0; i_ < frameCount; i_++){
						anIntArray353[i_] = stream.readDWord();
						anIntArray354[i_] = -1;
					}
					for(int i_ = 0; i_ < frameCount; i_++)
						anIntArray355[i_] = stream.readUnsignedByte();
				}
				else if(i == 2)
					anInt356 = stream.readUnsignedWord();
				else if(i == 3) {
					int k = stream.readUnsignedByte();
					anIntArray357 = new int[k + 1];
					for(int l = 0; l < k; l++)
						anIntArray357[l] = stream.readUnsignedByte();
					anIntArray357[k] = 0x98967f;
				}
				else if(i == 4)
					aBoolean358 = true;
				else if(i == 5)
					anInt359 = stream.readUnsignedByte();
				else if(i == 6)
					anInt360 = stream.readUnsignedWord();
				else if(i == 7)
					anInt361 = stream.readUnsignedWord();
				else if(i == 8)
					anInt362 = stream.readUnsignedByte();
				else if(i == 9)
					anInt363 = stream.readUnsignedByte();
				else if(i == 10)
					anInt364 = stream.readUnsignedByte();
				else if(i == 11)
					anInt365 = stream.readUnsignedByte();
				else 
					Debug.Log("Unrecognized seq.dat config code: "+i);
			} while(true);
			if(frameCount == 0)
			{
				frameCount = 1;
				anIntArray353 = new int[1];
				anIntArray353[0] = -1;
				anIntArray354 = new int[1];
				anIntArray354[0] = -1;
				anIntArray355 = new int[1];
				anIntArray355[0] = -1;
			}
			if(anInt363 == -1)
				if(anIntArray357 != null)
					anInt363 = 2;
			else
				anInt363 = 0;
			if(anInt364 == -1)
			{
				if(anIntArray357 != null)
				{
					anInt364 = 2;
					return;
				}
				anInt364 = 0;
			}
		}
		
		private Animation() {
			anInt356 = -1;
			aBoolean358 = false;
			anInt359 = 5;
			anInt360 = -1;
			anInt361 = -1;
			anInt362 = 99;
			anInt363 = -1;
			anInt364 = -1;
			anInt365 = 2;
		}
		
		public static Animation[] anims;
		public int frameCount;
		public int[] anIntArray353;
		public int[] anIntArray354;
		public int[] anIntArray355;
		public int anInt356;
		public int[] anIntArray357;
		public bool aBoolean358;
		public int anInt359;
		public int anInt360;
		public int anInt361;
		public int anInt362;
		public int anInt363;
		public int anInt364;
		public int anInt365;
		public static int anInt367;
	}
}
