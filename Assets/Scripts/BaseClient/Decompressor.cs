using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace RS2Sharp
{
	public class Decompressor
	{

		public Decompressor(FileStream randomaccessfile, FileStream randomaccessfile1, int j)
		{
			anInt311 = j;
			dataFile = randomaccessfile;
			indexFile = randomaccessfile1;		
		}
		
		public byte[] decompressgzip(int i)
		{
			byte[] buffer = decompress(i);
			byte[] gzipInputBuffer = new byte[999999];
			try {
				Ionic.Zlib.GZipStream gzipinputstream = new Ionic.Zlib.GZipStream(
					new MemoryStream(buffer), Ionic.Zlib.CompressionMode.Decompress);
				do {
					if (i == gzipInputBuffer.Length)
						throw new Exception("buffer overflow!");
					int k = gzipinputstream.Read(gzipInputBuffer, i,
					                             gzipInputBuffer.Length - i);
					if (k == 0)
						break;
					i += k;
				} while (true);
			} catch (IOException _ex) {
				throw new Exception("error unzipping");
			}
			buffer = new byte[i];
			//System.Array.Copy(gzipInputBuffer, 0, onDemandData.buffer, 0, i);
			Buffer.BlockCopy(gzipInputBuffer,0, buffer,0, i);
			return buffer;
		}

		public byte[] decompress(int i)
		{
			try
			{
				seekTo(indexFile, i * 6);
				int l;
				for (int j = 0; j < 6; j += l)
				{
					l = indexFile.Read(buffer, j, 6 - j);
					if (l == -1)
						return null;
				}

				int i1 = ((buffer[0] & 0xff) << 16) + ((buffer[1] & 0xff) << 8) + (buffer[2] & 0xff);
				int j1 = ((buffer[3] & 0xff) << 16) + ((buffer[4] & 0xff) << 8) + (buffer[5] & 0xff);
				//if (i1 < 0 || i1 > 0x7a120)
				//	return null;
				if (j1 <= 0 || j1 > dataFile.Length / 520L)
					return null;
				byte[] abyte0 = new byte[i1];
				int k1 = 0;
				for (int l1 = 0; k1 < i1; l1++)
				{
					if (j1 == 0)
						return null;
					seekTo(dataFile, j1 * 520);
					int k = 0;
					int i2 = i1 - k1;
					if (i2 > 512)
						i2 = 512;
					int j2;
					for (; k < i2 + 8; k += j2)
					{
						j2 = dataFile.Read(buffer, k, (i2 + 8) - k);
						if (j2 == -1)
							return null;
					}

					int k2 = ((buffer[0] & 0xff) << 8) + (buffer[1] & 0xff);
					int l2 = ((buffer[2] & 0xff) << 8) + (buffer[3] & 0xff);
					int i3 = ((buffer[4] & 0xff) << 16) + ((buffer[5] & 0xff) << 8) + (buffer[6] & 0xff);
					int j3 = buffer[7] & 0xff;
					if (k2 != i || l2 != l1 || j3 != anInt311)
						return null;
					if (i3 < 0 || i3 > dataFile.Length / 520L)
						return null;
					for (int k3 = 0; k3 < i2; k3++)
						abyte0[k1++] = buffer[k3 + 8];

					j1 = i3;
				}

				return abyte0;
			}
			catch (IOException _ex)
			{
				return null;
			}
		}

		public bool method234(int i, byte[] abyte0, int j)
		{
			bool flag = method235(true, j, i, abyte0);
			if (!flag)
				flag = method235(false, j, i, abyte0);
			return flag;
		}

		private bool method235(bool flag, int j, int k, byte[] abyte0)
		{
			try
			{
				int l = 0;
				if (flag)
				{
					seekTo(indexFile, j * 6);
					int k1 = 0;
					for (int i1 = 0; i1 < 6; i1 += k1)
					{
						k1 = indexFile.Read(buffer, i1, 6 - i1);
						if (k1 <= 0)
							return false;
					}

					l = ((buffer[3] & 0xff) << 16) + ((buffer[4] & 0xff) << 8) + (buffer[5] & 0xff);
					if (l <= 0 || (long)l > dataFile.Length / 520L)
						return false;
				}
				else
				{
					l = (int)((dataFile.Length + 519L) / 520L);
					if (l == 0)
						l = 1;
				}
				buffer[0] = (byte)(k >> 16);
				buffer[1] = (byte)(k >> 8);
				buffer[2] = (byte)k;
				buffer[3] = (byte)(l >> 16);
				buffer[4] = (byte)(l >> 8);
				buffer[5] = (byte)l;
				seekTo(indexFile, j * 6);
				indexFile.Write(buffer, 0, 6);
				int j1 = 0;
				for (int l1 = 0; j1 < k; l1++)
				{
					int i2 = 0;
					if (flag)
					{
						seekTo(dataFile, l * 520);
						int j2 = 0;
						int l2 = 0;
						for (j2 = 0; j2 < 8; j2 += l2)
						{
							l2 = dataFile.Read(buffer, j2, 8 - j2);
							if (l2 == -1)
								break;
						}

						if (j2 == 8)
						{
							int i3 = ((buffer[0] & 0xff) << 8) + (buffer[1] & 0xff);
							int j3 = ((buffer[2] & 0xff) << 8) + (buffer[3] & 0xff);
							i2 = ((buffer[4] & 0xff) << 16) + ((buffer[5] & 0xff) << 8) + (buffer[6] & 0xff);
							int k3 = buffer[7] & 0xff;
							if (i3 != j || j3 != l1 || k3 != anInt311)
								return false;
							if (i2 < 0 || (long)i2 > dataFile.Length / 520L)
								return false;
						}
					}
					if (i2 == 0)
					{
						flag = false;
						i2 = (int)((dataFile.Length + 519L) / 520L);
						if (i2 == 0)
							i2++;
						if (i2 == l)
							i2++;
					}
					if (k - j1 <= 512)
						i2 = 0;
					buffer[0] = (byte)(j >> 8);
					buffer[1] = (byte)j;
					buffer[2] = (byte)(l1 >> 8);
					buffer[3] = (byte)l1;
					buffer[4] = (byte)(i2 >> 16);
					buffer[5] = (byte)(i2 >> 8);
					buffer[6] = (byte)i2;
					buffer[7] = (byte)anInt311;
					seekTo(dataFile, l * 520);
					dataFile.Write(buffer, 0, 8);
					int k2 = k - j1;
					if (k2 > 512)
						k2 = 512;
					dataFile.Write(abyte0, j1, k2);
					j1 += k2;
					l = i2;
				}

				return true;
			}
			catch (IOException _ex)
			{
				return false;
			}
		}

		private void seekTo(FileStream randomaccessfile, int j)
		{
//			if (j < 0 || j > 0x3c00000)
//			{
//				UnityEngine.Debug.Log("Badseek - pos:" + j + " len:" + randomaccessfile.Length);
//				j = 0x3c00000;
//				try
//				{
//				//	Thread.Sleep(1000);
//				}
//				catch (Exception _ex) { }
//			}
			try{
			randomaccessfile.Seek(j, SeekOrigin.Begin);
			}
			catch(Exception ex)
			{
				UnityEngine.Debug.Log(ex.StackTrace);
			}
		}

		private static byte[] buffer = new byte[520];
		private FileStream dataFile;
		private FileStream indexFile;
		private int anInt311;

	}
}
