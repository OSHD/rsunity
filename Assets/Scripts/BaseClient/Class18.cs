using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS2Sharp
{
	// Decompiled by Jad v1.5.8f. Copyright 2001 Pavel Kouznetsov.
	// Jad home page: http://www.kpdus.com/jad.html
	// Decompiler options: packimports(3) 

	public class Class18
	{

		public Class18(Stream stream, int junk)
		{
//			int anInt341 = stream.readUnsignedByte();
//			opcode = new int[anInt341];
//			skinlist = new int[anInt341][];
//			for (int j = 0; j < anInt341; j++)
//				opcode[j] = stream.readUnsignedByte();
//
//			for (int k = 0; k < anInt341; k++)
//			{
//				int l = stream.readUnsignedByte();
//				skinlist[k] = new int[l];
//				for (int i1 = 0; i1 < l; i1++)
//					skinlist[k][i1] = stream.readUnsignedByte();
//
//			}
			int anInt341 = stream.readUnsignedWord();
			opcode = new int[anInt341];
			skinlist = new int[anInt341][];
			for(int j = 0; j < anInt341; j++)
				opcode[j] = stream.readUnsignedWord();
			
			for(int j = 0; j < anInt341; j++)
				skinlist[j] = new int[stream.readUnsignedWord()];
			
			for(int j = 0; j < anInt341; j++)
				for(int l = 0; l < skinlist[j].Length; l++)
					skinlist[j][l] = stream.readUnsignedWord();

		}

		public int[] opcode;
		public int[][] skinlist;
	}
}
