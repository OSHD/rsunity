using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

namespace RS2Sharp
{

	public class RSImageProducer
	{
		public RSImageProducer(int i, int j)
		{
			anInt316 = i;
			anInt317 = j;
			anIntArray315 = new int[anInt316 * anInt317];
			initDrawingArea();
		}

		public void initDrawingArea()
		{
			DrawingArea.initDrawingArea(anInt317, anInt316, anIntArray315);
		}

		public void drawGraphics(int y, Graphics g, int x)
		{
			//if (client.drawCanvas)
			//	g.DrawImage (anIntArray315, x, y,anInt316,anInt317);
		}

		public bool imageUpdate(Texture2D image, int i, int j, int k, int l, int i1)
		{
			return true;
		}

		public int[] anIntArray315;
		private int anInt316;
		private int anInt317;
	}

}
