using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS2Sharp
{
	// Decompiled by Jad v1.5.8f. Copyright 2001 Pavel Kouznetsov.
	// Jad home page: http://www.kpdus.com/jad.html
	// Decompiler options: packimports(3) 

	public class ISAACRandomGen
	{

		public ISAACRandomGen(int[] ai)
		{
			anIntArray65 = new int[256];
			outKeys = new int[256];
			for (int j = 0; j < ai.Length; j++)
				outKeys[j] = ai[j];

			generateKeys();
		}

		public int getNextKey()
		{
			if (anInt63-- == 0)
			{
				method204();
				anInt63 = 255;
			}
			if (anInt63 == 255)
			{

			}
			return outKeys[anInt63];
		}

		private void method204()
		{
			anInt67 += ++anInt68;
			for (int i = 0; i < 256; i++)
			{
				int j = anIntArray65[i];
				if ((i & 3) == 0)
					anInt66 ^= anInt66 << 13;
				else
					if ((i & 3) == 1)
						anInt66 ^= (int)((uint)anInt66 >> 6);
					else
						if ((i & 3) == 2)
							anInt66 ^= anInt66 << 2;
						else
							if ((i & 3) == 3)
								anInt66 ^= (int)((uint)anInt66 >> 16);
				anInt66 += anIntArray65[i + 128 & 0xff];
				int k;
				anIntArray65[i] = k = anIntArray65[(j & 0x3fc) >> 2] + anInt66 + anInt67;
				outKeys[i] = anInt67 = anIntArray65[(k >> 8 & 0x3fc) >> 2] + j;
			}

		}

		private void generateKeys()
		{
			int i1;
			int j1;
			int k1;
			int l1;
			int i2;
			int j2;
			int k2;
			int l = i1 = j1 = k1 = l1 = i2 = j2 = k2 = unchecked((int)0x9e3779b9);
			for (int i = 0; i < 4; i++)
			{
				l ^= i1 << 11;
				k1 += l;
				i1 += j1;
				i1 ^= (int)((uint)j1 >> 2);
				l1 += i1;
				j1 += k1;
				j1 ^= k1 << 8;
				i2 += j1;
				k1 += l1;
				k1 ^= (int)((uint)l1 >> 16);
				j2 += k1;
				l1 += i2;
				l1 ^= i2 << 10;
				k2 += l1;
				i2 += j2;
				i2 ^= (int)((uint)j2 >> 4);
				l += i2;
				j2 += k2;
				j2 ^= k2 << 8;
				i1 += j2;
				k2 += l;
				k2 ^= (int)((uint)l >> 9);
				j1 += k2;
				l += i1;
			}

			for (int j = 0; j < 256; j += 8)
			{
				l += outKeys[j];
				i1 += outKeys[j + 1];
				j1 += outKeys[j + 2];
				k1 += outKeys[j + 3];
				l1 += outKeys[j + 4];
				i2 += outKeys[j + 5];
				j2 += outKeys[j + 6];
				k2 += outKeys[j + 7];
				l ^= i1 << 11;
				k1 += l;
				i1 += j1;
				i1 ^= (int)((uint)j1 >> 2);
				l1 += i1;
				j1 += k1;
				j1 ^= k1 << 8;
				i2 += j1;
				k1 += l1;
				k1 ^= (int)((uint)l1 >> 16);
				j2 += k1;
				l1 += i2;
				l1 ^= i2 << 10;
				k2 += l1;
				i2 += j2;
				i2 ^= (int)((uint)j2 >> 4);
				l += i2;
				j2 += k2;
				j2 ^= k2 << 8;
				i1 += j2;
				k2 += l;
				k2 ^= (int)((uint)l >> 9);
				j1 += k2;
				l += i1;
				anIntArray65[j] = l;
				anIntArray65[j + 1] = i1;
				anIntArray65[j + 2] = j1;
				anIntArray65[j + 3] = k1;
				anIntArray65[j + 4] = l1;
				anIntArray65[j + 5] = i2;
				anIntArray65[j + 6] = j2;
				anIntArray65[j + 7] = k2;
			}

			for (int k = 0; k < 256; k += 8)
			{
				l += anIntArray65[k];
				i1 += anIntArray65[k + 1];
				j1 += anIntArray65[k + 2];
				k1 += anIntArray65[k + 3];
				l1 += anIntArray65[k + 4];
				i2 += anIntArray65[k + 5];
				j2 += anIntArray65[k + 6];
				k2 += anIntArray65[k + 7];
				l ^= i1 << 11;
				k1 += l;
				i1 += j1;
				i1 ^= (int)((uint)j1 >> 2);
				l1 += i1;
				j1 += k1;
				j1 ^= k1 << 8;
				i2 += j1;
				k1 += l1;
				k1 ^= (int)((uint)l1 >> 16);
				j2 += k1;
				l1 += i2;
				l1 ^= i2 << 10;
				k2 += l1;
				i2 += j2;
				i2 ^= (int)((uint)j2 >> 4);
				l += i2;
				j2 += k2;
				j2 ^= k2 << 8;
				i1 += j2;
				k2 += l;
				k2 ^= (int)((uint)l >> 9);
				j1 += k2;
				l += i1;
				anIntArray65[k] = l;
				anIntArray65[k + 1] = i1;
				anIntArray65[k + 2] = j1;
				anIntArray65[k + 3] = k1;
				anIntArray65[k + 4] = l1;
				anIntArray65[k + 5] = i2;
				anIntArray65[k + 6] = j2;
				anIntArray65[k + 7] = k2;
			}

			method204();
			anInt63 = 256;
		}

		public int anInt63;
		public int[] outKeys;
		public int[] anIntArray65;
		public int anInt66;
		public int anInt67;
		public int anInt68;
	}
}
