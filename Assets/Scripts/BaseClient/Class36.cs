using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using sign;

namespace RS2Sharp
{
	public class Class36
	{

//		public static void method528(int i)
//		{
//			aClass36Array635 = new Class36[i + 1];
//			aBooleanArray643 = new bool[i + 1];
//			for (int j = 0; j < i + 1; j++)
//				aBooleanArray643[j] = true;
//			
//		}
//		
//		public static void method529(byte[] abyte0)
//		{
//			Stream stream = new Stream(abyte0);
//			stream.currentOffset = abyte0.Length - 8;
//			int i = stream.readUnsignedWord();
//			int j = stream.readUnsignedWord();
//			int k = stream.readUnsignedWord();
//			int l = stream.readUnsignedWord();
//			int i1 = 0;
//			Stream stream_1 = new Stream(abyte0);
//			stream_1.currentOffset = i1;
//			i1 += i + 2;
//			Stream stream_2 = new Stream(abyte0);
//			stream_2.currentOffset = i1;
//			i1 += j;
//			Stream stream_3 = new Stream(abyte0);
//			stream_3.currentOffset = i1;
//			i1 += k;
//			Stream stream_4 = new Stream(abyte0);
//			stream_4.currentOffset = i1;
//			i1 += l;
//			Stream stream_5 = new Stream(abyte0);
//			stream_5.currentOffset = i1;
//			Class18 class18 = new Class18(stream_5);
//			int k1 = stream_1.readUnsignedWord();
//			int[] ai = new int[500];
//			int[] ai1 = new int[500];
//			int[] ai2 = new int[500];
//			int[] ai3 = new int[500];
//			for (int l1 = 0; l1 < k1; l1++)
//			{
//				int i2 = stream_1.readUnsignedWord();
//				Class36 class36 = aClass36Array635[i2] = new Class36();
//				class36.anInt636 = stream_4.readUnsignedByte();
//				class36.aClass18_637 = class18;
//				int j2 = stream_1.readUnsignedByte();
//				int k2 = -1;
//				int l2 = 0;
//				for (int i3 = 0; i3 < j2; i3++)
//				{
//					int j3 = stream_2.readUnsignedByte();
//					if (j3 > 0)
//					{
//						if (class18.anIntArray342[i3] != 0)
//						{
//							for (int l3 = i3 - 1; l3 > k2; l3--)
//							{
//								if (class18.anIntArray342[l3] != 0)
//									continue;
//								ai[l2] = l3;
//								ai1[l2] = 0;
//								ai2[l2] = 0;
//								ai3[l2] = 0;
//								l2++;
//								break;
//							}
//							
//						}
//						ai[l2] = i3;
//						char c = '\0';
//						if (class18.anIntArray342[i3] == 3)
//							c = (char)128;
//						if ((j3 & 1) != 0)
//							ai1[l2] = stream_3.method421();
//						else
//							ai1[l2] = c;
//						if ((j3 & 2) != 0)
//							ai2[l2] = stream_3.method421();
//						else
//							ai2[l2] = c;
//						if ((j3 & 4) != 0)
//							ai3[l2] = stream_3.method421();
//						else
//							ai3[l2] = c;
//						k2 = i3;
//						l2++;
//						if (class18.anIntArray342[i3] == 5)
//							aBooleanArray643[i2] = false;
//					}
//				}
//				
//				class36.anInt638 = l2;
//				class36.anIntArray639 = new int[l2];
//				class36.anIntArray640 = new int[l2];
//				class36.anIntArray641 = new int[l2];
//				class36.anIntArray642 = new int[l2];
//				for (int k3 = 0; k3 < l2; k3++)
//				{
//					class36.anIntArray639[k3] = ai[k3];
//					class36.anIntArray640[k3] = ai1[k3];
//					class36.anIntArray641[k3] = ai2[k3];
//					class36.anIntArray642[k3] = ai3[k3];
//				}
//				
//			}
//			
//		}
//		
//		public static void nullLoader()
//		{
//			aClass36Array635 = null;
//		}
//		
//		public static Class36 method531(int j)
//		{
//			if (aClass36Array635 == null)
//				return null;
//			else
//				return aClass36Array635[j];
//		}
//		
//		public static bool method532(int i)
//		{
//			return i == -1;
//		}
//		
//		private Class36()
//		{
//		}
//		
//		private static Class36[] aClass36Array635;
//		public int anInt636;
//		public Class18 aClass18_637;
//		public int anInt638;
//		public int[] anIntArray639;
//		public int[] anIntArray640;
//		public int[] anIntArray641;
//		public int[] anIntArray642;
//		private static bool[] aBooleanArray643;

		public static byte[] getData(int i1, int i2) {
			if (i1 == 0)
			{
				return frameData[i2];
			}
			else
			{
				return skinData[i2];
			}
		}
		
		public static void method528(int i) {
			animationlist = NetDrawingTools.CreateDoubleClass36Array (4000, 0);//new Class36[4000][0];
		}
		
		public static void LoadHighRevision(int file) {
//			try {
//				Stream stream = new Stream(System.IO.File.ReadAllBytes(signlink.findcachedir() + "/Data/Higher Revision/"+file+".dat"));
//				Class18 class18 = new Class18(stream, 0);
//				int k1 = stream.readUnsignedWord();
//				animationlist[file] = new Class36[(int) (k1 * 3.0)];
//				int[] ai = new int[500];
//				int[] ai1 = new int[500];
//				int[] ai2 = new int[500];
//				int[] ai3 = new int[500];
//				for(int l1 = 0; l1 < k1; l1++) {
//					int i2 = stream.readUnsignedWord();
//					Class36 class36 = animationlist[file][i2] = new Class36();
//					class36.aClass18_637 = class18;
//					int j2 = stream.readUnsignedByte();
//					int l2 = 0;
//					int k2 = -1;
//					for(int i3 = 0; i3 < j2; i3++) {
//						int j3 = stream.readUnsignedByte();
//						if(j3 > 0) {
//							if(class18.opcode[i3] != 0) {
//								for(int l3 = i3 - 1; l3 > k2; l3--) {
//									if(class18.opcode[l3] != 0)
//										continue;
//									ai[l2] = l3;
//									ai1[l2] = 0;
//									ai2[l2] = 0;
//									ai3[l2] = 0;
//									l2++;
//									break;
//								}
//								
//							}
//							ai[l2] = i3;
//							short c = 0;
//							if(class18.opcode[i3] == 3)
//								c = (short)128;
//							if((j3 & 1) != 0)
//								ai1[l2] = (short)stream.readShort2();
//							else
//								ai1[l2] = c;
//							if((j3 & 2) != 0)
//								ai2[l2] = stream.readShort2();
//							else
//								ai2[l2] = c;
//							if((j3 & 4) != 0)
//								ai3[l2] = stream.readShort2();
//							else
//								ai3[l2] = c;
//							k2 = i3;
//							l2++;
//						}
//					}
//					class36.stepCounter = l2;
//					class36.opcodeLinkTable = new int[l2];
//					class36.modifier1 = new int[l2];
//					class36.modifier2 = new int[l2];
//					class36.modifier3 = new int[l2];
//					for(int k3 = 0; k3 < l2; k3++) {
//						class36.opcodeLinkTable[k3] = ai[k3];
//						class36.modifier1[k3] = ai1[k3];
//						class36.modifier2[k3] = ai2[k3];
//						class36.modifier3[k3] = ai3[k3];
//					}
//				}
//			}catch(Exception exception) { }
		}
		
		public static void load(int file, byte[] fileData){
			try {
				Stream stream = new Stream(fileData);
				Class18 class18 = new Class18(stream, 0);
				int k1 = stream.readUnsignedWord();
				animationlist[file] = new Class36[(int) (k1 * 3.0)];
				int[] ai = new int[500];
				int[] ai1 = new int[500];
				int[] ai2 = new int[500];
				int[] ai3 = new int[500];
				for(int l1 = 0; l1 < k1; l1++) {
					int i2 = stream.readUnsignedWord();
					Class36 class36 = animationlist[file][i2] = new Class36();
					class36.aClass18_637 = class18;
					int j2 = stream.readUnsignedByte();
					int l2 = 0;
					int k2 = -1;
					for(int i3 = 0; i3 < j2; i3++) {
						int j3 = stream.readUnsignedByte();
						if(j3 > 0) {
							if(class18.opcode[i3] != 0) {
								for(int l3 = i3 - 1; l3 > k2; l3--) {
									if(class18.opcode[l3] != 0)
										continue;
									ai[l2] = l3;
									ai1[l2] = 0;
									ai2[l2] = 0;
									ai3[l2] = 0;
									l2++;
									break;
								}
							}
							ai[l2] = i3;
							short c = 0;
							if(class18.opcode[i3] == 3)
								c = (short)128;
							if((j3 & 1) != 0)
								ai1[l2] = (short)stream.readShort2();
							else
								ai1[l2] = c;
							if((j3 & 2) != 0)
								ai2[l2] = stream.readShort2();
							else
								ai2[l2] = c;
							if((j3 & 4) != 0)
								ai3[l2] = stream.readShort2();
							else
								ai3[l2] = c;
							k2 = i3;
							l2++;
						}
					}
					class36.stepCounter = l2;
					class36.opcodeLinkTable = new int[l2];
					class36.modifier1 = new int[l2];
					class36.modifier2 = new int[l2];
					class36.modifier3 = new int[l2];
					for(int k3 = 0; k3 < l2; k3++) {
						class36.opcodeLinkTable[k3] = ai[k3];
						class36.modifier1[k3] = ai1[k3];
						class36.modifier2[k3] = ai2[k3];
						class36.modifier3[k3] = ai3[k3];
					}
				}
			}catch(Exception exception) { }
		}
		
		public static void nullLoader() {
			animationlist = null;
		}
		
		public static Class36 method531(int i) {
			//try {
				byte fileData;
				String s = "";
				int file = 0;
				int k = 0;
				s = i.ToString("X4");
				file = Convert.ToInt32(s.Substring(0, s.Length - 4), 16);
				k = Convert.ToInt32(s.Substring(s.Length - 4), 16);
				if(animationlist == null || animationlist[file].Length == 0) {
					
					//	System.out.println(client.instance);
					//	System.out.println(client.instance.onDemandFetcher);
					UnityClient.onDemandFetcher.method558(1, file);
					return null;
				}
				return animationlist[file][k];
//			} catch(Exception e) {
//				Debug.Log(e.Message);
//				return null;
//			}
		}
		
		
		
		
		public static bool method532(int i) {
			return i == -1;
		}
		
		private Class36() {
		}
		private static Class36[][] animationlist;
		public int anInt636;
		public Class18 aClass18_637;
		public int stepCounter;
		public static byte[][] frameData = null;
		public static byte[][] skinData = null;
		public int[] opcodeLinkTable;
		public int[] modifier1;
		public int[] modifier2;
		public int[] modifier3;

	}
}
