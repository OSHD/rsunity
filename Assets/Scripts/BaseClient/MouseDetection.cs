using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RS2Sharp
{
	// Decompiled by Jad v1.5.8f. Copyright 2001 Pavel Kouznetsov.
	// Jad home page: http://www.kpdus.com/jad.html
	// Decompiler options: packimports(3) 

	public class MouseDetection
	{

		public void run()
		{
//			while (running)// && !client.finished)
//			{
//				lock (syncObject)
//				{
//					if (coordsIndex < 500)
//					{
//						coordsX[coordsIndex] = clientInstance.mouseX;
//						coordsY[coordsIndex] = clientInstance.mouseY;
//						coordsIndex++;
//					}
//				}
//				try
//				{
//					//Thread.Sleep(50);
//				}
//				catch (Exception _ex) { }
//			}
		}

		public MouseDetection(UnityClient client1)
		{
			syncObject = new Object();
			coordsY = new int[500];
			running = true;
			coordsX = new int[500];
			clientInstance = client1;
		}

		private UnityClient clientInstance;
		public Object syncObject;
		public int[] coordsY;
		public bool running;
		public int[] coordsX;
		public int coordsIndex;
	}
}
