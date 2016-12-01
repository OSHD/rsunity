using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS2Sharp
{
	public class StreamLoader
	{

		public StreamLoader(byte[] abyte0)
		{
			Stream stream = new Stream(abyte0);
			int i = stream.read3Bytes();
			int j = stream.read3Bytes();
			if (j != i)
			{
				byte[] abyte1 = new byte[i];
				Class13.method225(abyte1, i, abyte0, j, 6);
				aByteArray726 = abyte1;
				stream = new Stream(aByteArray726);
				aBoolean732 = true;
			}
			else
			{
				aByteArray726 = abyte0;
				aBoolean732 = false;
			}
			dataSize = stream.readUnsignedWord();
			anIntArray728 = new int[dataSize];
			anIntArray729 = new int[dataSize];
			anIntArray730 = new int[dataSize];
			anIntArray731 = new int[dataSize];
			int k = stream.currentOffset + dataSize * 10;
			for (int l = 0; l < dataSize; l++)
			{
				anIntArray728[l] = stream.readDWord();
				anIntArray729[l] = stream.read3Bytes();
				anIntArray730[l] = stream.read3Bytes();
				anIntArray731[l] = k;
				k += anIntArray730[l];
			}
		}

		public byte[] getDataForName(String s)
		{
			byte[] abyte0 = null; //was a parameter
			int i = 0;
			s = s.ToUpper();
			for (int j = 0; j < s.Length; j++)
				i = (i * 61 + s[j]) - 32;

			for (int k = 0; k < dataSize; k++)
				if (anIntArray728[k] == i)
				{
					if (abyte0 == null)
						abyte0 = new byte[anIntArray729[k]];
					if (!aBoolean732)
					{
						Class13.method225(abyte0, anIntArray729[k], aByteArray726, anIntArray730[k], anIntArray731[k]);
					}
					else
					{
						Array.Copy(aByteArray726, anIntArray731[k], abyte0, 0, anIntArray729[k]);

					}
					return abyte0;
				}

			return null;
		}

		private byte[] aByteArray726;
		private int dataSize;
		private int[] anIntArray728;
		private int[] anIntArray729;
		private int[] anIntArray730;
		private int[] anIntArray731;
		private bool aBoolean732;
	}
}
