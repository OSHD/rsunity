using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS2Sharp
{
	// Decompiled by Jad v1.5.8f. Copyright 2001 Pavel Kouznetsov.
	// Jad home page: http://www.kpdus.com/jad.html
	// Decompiler options: packimports(3) 

	public class VarBit
	{

		public static void unpackConfig(StreamLoader streamLoader)
		{
			Stream stream = new Stream(streamLoader.getDataForName("varbit.dat"));
			int cacheSize = stream.readUnsignedWord();
			if (cache == null)
				cache = new VarBit[cacheSize];
			for (int j = 0; j < cacheSize; j++)
			{
				if (cache[j] == null)
					cache[j] = new VarBit();
				cache[j].readValues(stream);
				if (cache[j].aBoolean651)
					Varp.cache[cache[j].anInt648].aBoolean713 = true;
			}

			if (stream.currentOffset != stream.buffer.Length)
				UnityEngine.Debug.Log("varbit load mismatch");
		}

		private void readValues(Stream stream)
		{
			do
			{
				int j = stream.readUnsignedByte();
				if (j == 0)
					return;
				if (j == 1)
				{
					anInt648 = stream.readUnsignedWord();
					anInt649 = stream.readUnsignedByte();
					anInt650 = stream.readUnsignedByte();
				}
				else
					if (j == 10)
						stream.readString();
					else
						if (j == 2)
							aBoolean651 = true;
						else
							if (j == 3)
								stream.readDWord();
							else
								if (j == 4)
									stream.readDWord();
								else
									UnityEngine.Debug.Log("Error unrecognised config code: " + j);
			} while (true);
		}

		private VarBit()
		{
			aBoolean651 = false;
		}

		public static VarBit[] cache;
		public int anInt648;
		public int anInt649;
		public int anInt650;
		private bool aBoolean651;
	}
}
