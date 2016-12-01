using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;

namespace RS2Sharp
{
	public class NetDrawingTools
	{
		private static readonly DateTime Jan1st1970 = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static long CurrentTimeMillis()
		{
			return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
		}

		public static int[] ReadPixels(Texture2D img, int width, int height)
		{
			//FastPixel fp = new FastPixel(width2, height2);
			//fp.rgbValues = new byte[fp.Width * fp.Height * 4];

			int[] output = new int[width * height];
			//Color32[] imgPixels = img.GetPixels32 ();
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					output[x + y * width] = ToArgb(img.GetPixel(x,y));
				}
			}

			return output;
		}
		
		public static int ToArgb(Color32 color)
        {
			if ((color.r == 255 && color.g == 255 && color.b == 255) || (color.r == 255 && color.g == 0 && color.b == 255)) return 0;
			int argb = color.a << 24;
            argb += color.r << 16;
            argb += color.g << 8;
            argb += color.b;

            return argb;
        }


		public static NodeList[][][] CreateTrippleNodeListArray(int count1, int count2, int count3)
		{
			NodeList[][][] output = new NodeList[count1][][];
			for (int i = 0; i < count1; i++)
			{
				output[i] = new NodeList[count2][];
				for (int k = 0; k < count2; k++)
					output[i][k] = new NodeList[count3];
			}
			return output;
		}

		public static int[][] CreateDoubleIntArray(int count1, int count2)
		{
			int[][] output = new int[count1][];
			for (int i = 0; i < count1; i++)
				output[i] = new int[count2];
			return output;
		}

		public static Color32[][] CreateDoubleColor32Array(int count1, int count2)
		{
			Color32[][] output = new Color32[count1][];
			for (int i = 0; i < count1; i++)
				output[i] = new Color32[count2];
			return output;
		}

		public static Class47[][] CreateDoubleClass47Array(int count1, int count2)
		{
			Class47[][] output = new Class47[count1][];
			for (int i = 0; i < count1; i++)
				output[i] = new Class47[count2];
			return output;
		}

		public static uint[][] CreateDoubleUIntArray(int count1, int count2)
		{
			uint[][] output = new uint[count1][];
			for (int i = 0; i < count1; i++)
				output[i] = new uint[count2];
			return output;
		}

		public static Class36[][] CreateDoubleClass36Array(int count1, int count2)
		{
			Class36[][] output = new Class36[count1][];
			for (int i = 0; i < count1; i++)
				output[i] = new Class36[count2];
			return output;
		}

		public static int[][][] CreateTrippleIntArray(int count1, int count2, int count3)
		{
			int[][][] output = new int[count1][][];
			for (int i = 0; i < count1; i++)
			{
				output[i] = new int[count2][];
				for (int k = 0; k < count2; k++)
					output[i][k] = new int[count3];
			}
			return output;
		}

		public static sbyte[][][] CreateTrippleSByteArray(int count1, int count2, int count3)
		{
			sbyte[][][] output = new sbyte[count1][][];
			for (int i = 0; i < count1; i++)
			{
				output[i] = new sbyte[count2][];
				for (int k = 0; k < count2; k++)
					output[i][k] = new sbyte[count3];
			}
			return output;
		}

		public static uint[][][] CreateTrippleUIntArray(int count1, int count2, int count3)
		{
			uint[][][] output = new uint[count1][][];
			for (int i = 0; i < count1; i++)
			{
				output[i] = new uint[count2][];
				for (int k = 0; k < count2; k++)
					output[i][k] = new uint[count3];
			}
			return output;
		}

		public static Ground[][][] CreateTrippleGroundArray(int count1, int count2, int count3)
		{
			Ground[][][] output = new Ground[count1][][];
			for (int i = 0; i < count1; i++)
			{
				output[i] = new Ground[count2][];
				for (int k = 0; k < count2; k++)
					output[i][k] = new Ground[count3];
			}
			return output;
		}

		public static bool[][][][] CreateQuadBoolArray(int count1, int count2, int count3, int count4)
		{
			bool[][][][] output = new bool[count1][][][];
			for (int i = 0; i < count1; i++)
			{
				output[i] = new bool[count2][][];
				for (int k = 0; k < count2; k++)
				{
					output[i][k] = new bool[count3][];
					for (int j = 0; j < count4; j++)
						output[i][k][j] = new bool[count4];
				}
			}
			return output;
		}

		public static byte[][] CreateDoubleByteArray(int count1, int count2)
		{
			byte[][] output = new byte[count1][];
			for (int i = 0; i < count1; i++)
				output[i] = new byte[count2];
			return output;
		}

		public static byte[][][] CreateTrippleByteArray(int count1, int count2, int count3)
		{
			byte[][][] output = new byte[count1][][];
			for (int i = 0; i < count1; i++)
			{
				output[i] = new byte[count2][];
				for (int k = 0; k < count2; k++)
					output[i][k] = new byte[count3];
			}
			return output;
		}
	}
}
