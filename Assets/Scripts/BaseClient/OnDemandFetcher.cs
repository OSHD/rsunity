// Decompiled by Jad v1.5.8f. Copyright 2001 Pavel Kouznetsov.
// Jad home page: http://www.kpdus.com/jad.html
// Decompiler options: packimports(3) 

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.IO.Compression;
using System.Threading;
using sign;
using UnityEngine;

namespace RS2Sharp
{

	public class OnDemandFetcher : OnDemandFetcherParent
	{
//		public bool initializedLoop = false;
//		public void Update()
//		{
// 			if (getNodeCount () > 0 && !initializedLoop)// && client.runOnThread)
// 			{
// 				initializedLoop = true;
// 				Loom.StartSingleThread(()=>{OnDemandLoop();},ThreadPriority.Normal,true);
// 			}
////			if (!client.runOnThread && getNodeCount () > 0) {
////								run ();
////								clientInstance.processOnDemandQueue ();
////						}
//		}
//
//		public void OnDemandLoop()
//		{
//			while (true) {
//				//if(getNodeCount() > 0)
//				//{
//					//UnityEngine.Debug.Log ("In run");
//					run ();
//					//UnityEngine.Debug.Log ("Out run");
//					//UnityEngine.Debug.Log ("In processOnDemandQueue");
//					clientInstance.processOnDemandQueue ();
//					//UnityEngine.Debug.Log ("Out processOnDemandQueue");
//				//}
//				Thread.Sleep(100);
//			}
//		}
//// 		private bool crcMatches(int i, int j, byte[] abyte0)
//// 		{
//// 			if (abyte0 == null || abyte0.Length < 2)
//// 				return false;
//// 			int k = abyte0.Length - 2;
//// 			int l = ((abyte0[k] & 0xff) << 8) + (abyte0[k + 1] & 0xff);
//// 			//crc32.reset();
//// 			//crc32.update(abyte0, 0, k);
//// 			//int i1 = (int)crc32.getValue();
//// 			//return l == i && i1 == j;
//// 			return true;
//// 		}
//
//		private void readData()
//		{
//			try
//			{
//				int j = socket.Available;
//				if (expectedSize == 0 && j >= 6)
//				{
//					waiting = true;
//					for (int k = 0; k < 6; k += inputStream.Read(ioBuffer, k, 6 - k)) ;
//					int l = ioBuffer[0] & 0xff;
//					int j1 = ((ioBuffer[1] & 0xff) << 8) + (ioBuffer[2] & 0xff);
//					int l1 = ((ioBuffer[3] & 0xff) << 8) + (ioBuffer[4] & 0xff);
//					int i2 = ioBuffer[5] & 0xff;
//					current = null;
//					for (OnDemandData onDemandData = (OnDemandData)requested.reverseGetFirst(); onDemandData != null; onDemandData = (OnDemandData)requested.reverseGetNext())
//					{
//						if (onDemandData.dataType == l && onDemandData.ID == j1)
//							current = onDemandData;
//						if (current != null)
//							onDemandData.loopCycle = 0;
//					}
//
//					if (current != null)
//					{
//						loopCycle = 0;
//						if (l1 == 0)
//						{
//							signlink.reporterror("Rej: " + l + "," + j1);
//							current.buffer = null;
//							if (current.incomplete)
//								lock (aClass19_1358)
//								{
//									aClass19_1358.insertHead(current);
//								}
//							else
//								current.unlink();
//							current = null;
//						}
//						else
//						{
//							if (current.buffer == null && i2 == 0)
//								current.buffer = new byte[l1];
//							if (current.buffer == null && i2 != 0)
//								throw new IOException("missing start of file");
//						}
//					}
//					completedSize = i2 * 500;
//					expectedSize = 500;
//					if (expectedSize > l1 - i2 * 500)
//						expectedSize = l1 - i2 * 500;
//				}
//				if (expectedSize > 0 && j >= expectedSize)
//				{
//					waiting = true;
//					byte[] abyte0 = ioBuffer;
//					int i1 = 0;
//					if (current != null)
//					{
//						abyte0 = current.buffer;
//						i1 = completedSize;
//					}
//					for (int k1 = 0; k1 < expectedSize; k1 += inputStream.Read(abyte0, k1 + i1, expectedSize - k1)) ;
//					if (expectedSize + completedSize >= abyte0.Length && current != null)
//					{
//						if (client.decompressors[0] != null)
//							client.decompressors[current.dataType + 1].method234(abyte0.Length, abyte0, current.ID);
//						if (!current.incomplete && current.dataType == 3)
//						{
//							current.incomplete = true;
//							current.dataType = 93;
//						}
//						if (current.incomplete)
//							lock (aClass19_1358)
//							{
//								aClass19_1358.insertHead(current);
//							}
//						else
//							current.unlink();
//					}
//					expectedSize = 0;
//				}
//			}
//			catch (IOException ioexception)
//			{
//				try
//				{
//					socket.Close();
//				}
//				catch (Exception _ex) { }
//				socket = null;
//				inputStream = null;
//				outputStream = null;
//				expectedSize = 0;
//			}
//		}
//
//		public void start(StreamLoader streamLoader, client client1)
//		{
////			string[] mvs = {
////            "model_version", "anim_version", "midi_version", "map_version"
////        };
////			for (int i = 0; i < 4; i++)
////			{
////				byte[] abyte0 = streamLoader.getDataForName(mvs[i]);
////				int j = abyte0.Length / 2;
////				Stream stream = new Stream(abyte0);
////				versions[i] = new int[j];
////				fileStatus[i] = new byte[j];
////				for (int l = 0; l < j; l++)
////					versions[i][l] = stream.g2();
////
////			}
////
////			string[] as1 = {
////            "model_crc", "anim_crc", "midi_crc", "map_crc"
////        };
////			for (int k = 0; k < 4; k++)
////			{
////				byte[] abyte1 = streamLoader.getDataForName(as1[k]);
////				int i1 = abyte1.Length / 4;
////				Stream stream_1 = new Stream(abyte1);
////				crcs[k] = new int[i1];
////				for (int l1 = 0; l1 < i1; l1++)
////					crcs[k][l1] = stream_1.g4();
////
////			}
////
////			byte[] abyte2 = streamLoader.getDataForName("model_index");
////			int j1 = versions[0].Length;
////			modelIndices = new byte[j1];
////			for (int k1 = 0; k1 < j1; k1++)
////				if (k1 < abyte2.Length)
////					modelIndices[k1] = abyte2[k1];
////				else
////					modelIndices[k1] = 0;
//
//			byte[] abyte2 = streamLoader.getDataForName("map_index");
//			Stream stream2 = new Stream(abyte2);
//			//int totalRegions = stream2.g2();
//			int j1 = abyte2.Length / 6;
//			mapIndices1 = new int[j1];
//			mapIndices2 = new int[j1];
//			mapIndices3 = new int[j1];
//			//mapIndices4 = new int[j1];
//			for (int i2 = 0; i2 < j1; i2++)
//			{
//				mapIndices1[i2] = stream2.readUnsignedWord();
//				mapIndices2[i2] = stream2.readUnsignedWord();
//				mapIndices3[i2] = stream2.readUnsignedWord();
//				//mapIndices4[i2] = stream2.g1();
//			}
//
//			UnityEngine.Debug.Log ("Loaded : " + j1 + " Maps");
//
//// 			abyte2 = streamLoader.getDataForName("anim_index");
//// 			stream2 = new Stream(abyte2);
//// 			j1 = abyte2.Length / 2;
//// 			anIntArray1360 = new int[j1];
//// 			for (int j2 = 0; j2 < j1; j2++)
//// 				anIntArray1360[j2] = stream2.g2();
//
//			abyte2 = streamLoader.getDataForName("midi_index");
//			stream2 = new Stream(abyte2);
//			j1 = abyte2.Length;
//			anIntArray1348 = new int[j1];
//			for (int k2 = 0; k2 < j1; k2++)
//				anIntArray1348[k2] = stream2.readUnsignedByte();
//
//			clientInstance = client1;
//			running = true;
////			clientInstance.startRunnable(new ThreadStart(run), 2);
//		//	Loom.StartSingleThread(()=>run(),ThreadPriority.Normal,true);
//		}
//
//		public int getNodeCount()
//		{
//			lock (nodeSubList)
//			{
//				return nodeSubList.getNodeCount();
//			}
//		}
//
//		public void disable()
//		{
//			running = false;
//		}
//
//		public void method554(bool flag)
//		{
//			int j = mapIndices1.Length;
//			for (int k = 0; k < j; k++)
//				if (flag || mapIndices4[k] != 0)
//				{
//					method563((byte)2, 3, mapIndices3[k]);
//					method563((byte)2, 3, mapIndices2[k]);
//				}
//
//		}
//
//// 		public int getVersionCount(int j)
//// 		{
//// 			return versions[j].Length;
//// 		}
//
//		private void closeRequest(OnDemandData onDemandData)
//		{
//			//try
//			//{
//				if (socket == null)
//				{
//					long l = NetDrawingTools.CurrentTimeMillis();
//					if (l - openSocketTime < 4000L)
//						return;
//					openSocketTime = l;
//					socket = clientInstance.openSocket(43594 + client.portOff);
//					inputStream = new NetworkStream(socket);
//					outputStream = new NetworkStream(socket);
//					outputStream.WriteByte(15);
//					for (int j = 0; j < 8; j++)
//						inputStream.ReadByte();
//
//					loopCycle = 0;
//				}
//				ioBuffer[0] = (byte)onDemandData.dataType;
//				ioBuffer[1] = (byte)(onDemandData.ID >> 8);
//				ioBuffer[2] = (byte)onDemandData.ID;
//				if (onDemandData.incomplete)
//					ioBuffer[3] = 2;
//				else
//					if (!clientInstance.loggedIn)
//						ioBuffer[3] = 1;
//					else
//						ioBuffer[3] = 0;
//				outputStream.Write(ioBuffer, 0, 4);
//				writeLoopCycle = 0;
//				anInt1349 = -10000;
//				return;
//			//}
//			//catch (SocketException ioexception) { }
//			//try
//			//{
//			//	socket.Close();
//			//}
//			//catch (Exception _ex) { }
//			socket = null;
//			inputStream = null;
//			outputStream = null;
//			expectedSize = 0;
//			anInt1349++;
//		}
//
//		public int getAnimCount()
//		{
//			return short.MaxValue;//29192;//anIntArray1360.Length;
//		}
//
//		public void method558(int i, int j)
//		{
//			//if (i < 0 || j < 0)
//			//	return;
////			if (versions[i][j] == 0)
////				return;
//			lock (nodeSubList)
//			{
//				for (OnDemandData onDemandData = (OnDemandData)nodeSubList.reverseGetFirst(); onDemandData != null; onDemandData = (OnDemandData)nodeSubList.reverseGetNext())
//					if (onDemandData.dataType == i && onDemandData.ID == j)
//						return;
//
//				OnDemandData onDemandData_1 = new OnDemandData();
//				onDemandData_1.dataType = i;
//				onDemandData_1.ID = j;
//				onDemandData_1.incomplete = true;
//				//lock (aClass19_1370)
//				//{
//					aClass19_1370.insertHead(onDemandData_1);
//				//}
//				nodeSubList.insertHead(onDemandData_1);
//			}
//		}
//
//		public int getModelIndex(int i)
//		{
//			return modelIndices[i] & 0xff;
//		}
//
//		public int getModelCount() {
//			return 41761;//29191;
//		}
//
//		public void run()
//		{
//			//try
//			//{
//				if (running && !client.finished)
//				{
//					onDemandCycle++;
//					int i = 20;
//					if (anInt1332 == 0 && client.decompressors[0] != null)
//						i = 50;
//					//try
//					//{
//					//	Thread.Sleep(i);
//					//}
//					//catch (Exception _ex) { }
//					waiting = true;
//					for (int j = 0; j < 100; j++)
//					{
//						if (!waiting)
//							break;
//						waiting = false;
//						checkReceived();
//						//handleFailed();
//						if (uncompletedCount == 0 && j >= 5)
//							break;
//						method568();
//						if (inputStream != null)
//							readData();
//					}
//
//					bool flag = false;
//					for (OnDemandData onDemandData = (OnDemandData)requested.reverseGetFirst(); onDemandData != null; onDemandData = (OnDemandData)requested.reverseGetNext())
//						if (onDemandData.incomplete)
//						{
//							flag = true;
//							onDemandData.loopCycle++;
//							if (onDemandData.loopCycle > 50)
//							{
//								onDemandData.loopCycle = 0;
//								closeRequest(onDemandData);
//							}
//						}
//
//					if (!flag)
//					{
//						for (OnDemandData onDemandData_1 = (OnDemandData)requested.reverseGetFirst(); onDemandData_1 != null; onDemandData_1 = (OnDemandData)requested.reverseGetNext())
//						{
//							flag = true;
//							onDemandData_1.loopCycle++;
//							if (onDemandData_1.loopCycle > 50)
//							{
//								onDemandData_1.loopCycle = 0;
//								closeRequest(onDemandData_1);
//							}
//						}
//
//					}
//					if (flag)
//					{
//						loopCycle++;
//						if (loopCycle > 750)
//						{
//							try
//							{
//								socket.Close();
//							}
//							catch (Exception _ex) { }
//							socket = null;
//							inputStream = null;
//							outputStream = null;
//							expectedSize = 0;
//						}
//					}
//					else
//					{
//						loopCycle = 0;
//						statusString = "";
//					}
//					if (clientInstance.loggedIn && socket != null && outputStream != null && (anInt1332 > 0 || client.decompressors[0] == null))
//					{
//						writeLoopCycle++;
//						if (writeLoopCycle > 500)
//						{
//							writeLoopCycle = 0;
//							ioBuffer[0] = 0;
//							ioBuffer[1] = 0;
//							ioBuffer[2] = 0;
//							ioBuffer[3] = 10;
//							try
//							{
//								outputStream.Write(ioBuffer, 0, 4);
//							}
//							catch (IOException _ex)
//							{
//								loopCycle = 5000;
//							}
//						}
//					}
//				}
//			//}
//			//catch (Exception exception)
//			//{
//				//signlink.reporterror("od_ex " + exception.ToString());
//			//}
//		}
//
//		public void method560(int i, int j)
//		{
//			if (client.decompressors[0] == null)
//				return;
//			if (anInt1332 == 0)
//				return;
//			OnDemandData onDemandData = new OnDemandData();
//			if(j == 3) UnityEngine.Debug.Log ("Adding new 2 " + j + " " + i);
//			onDemandData.dataType = j;
//			onDemandData.ID = i;
//			onDemandData.incomplete = false;
//			lock (aClass19_1344)
//			{
//				aClass19_1344.insertHead(onDemandData);
//			}
//		}
//
//		public OnDemandData getNextNode()
//		{
//			OnDemandData onDemandData;
//			lock (aClass19_1358)
//			{
//				onDemandData = (OnDemandData)aClass19_1358.popHead();
//			}
//			if (onDemandData == null) {
//								return null;
//						}
//			lock (nodeSubList)
//			{
//				onDemandData.unlinkSub();
//			}
//			if (onDemandData.buffer == null)
//				return onDemandData;
//			int i = 0;
//			//try
//			//{
//				Ionic.Zlib.GZipStream gzipinputstream = new Ionic.Zlib.GZipStream(new MemoryStream(onDemandData.buffer), Ionic.Zlib.CompressionMode.Decompress);
//				do
//				{
//					if (i == gzipInputBuffer.Length)
//						throw new Exception("buffer overflow!");
//					int k = gzipinputstream.Read(gzipInputBuffer, i, gzipInputBuffer.Length - i);
//					if (k == 0)
//						break;
//					i += k;
//				} while (true);
//			//}
//			//catch (IOException _ex)
//			//{
//			//	throw new Exception("error unzipping");
//			//}
//			onDemandData.buffer = new byte[i];
//			Array.Copy(gzipInputBuffer, 0, onDemandData.buffer, 0, i);
//
//			return onDemandData;
//		}
//
//		public int method562(int i, int k, int l)
//		{
//			int i1 = (l << 8) + k;
//			for (int j1 = 0; j1 < mapIndices1.Length; j1++)
//				if (mapIndices1[j1] == i1)
//					if (i == 0)
//						return mapIndices2[j1];
//					else
//						return mapIndices3[j1];
//			return -1;
//		}
//
//		public override void method548(int i)
//		{
//			method558(0, i);
//		}
//
//		public void method563(byte byte0, int i, int j)
//		{
//			if (client.decompressors[0] == null)
//				return;
//			byte[] abyte0 = client.decompressors[i + 1].decompress(j);
////			if (crcMatches(versions[i][j], crcs[i][j], abyte0))
////				return;
//			//fileStatus[i][j] = byte0;
//			if (byte0 > anInt1332)
//				anInt1332 = byte0;
//			totalFiles++;
//		}
//
//		public bool method564(int i)
//		{
//			for (int k = 0; k < mapIndices1.Length; k++)
//				if (mapIndices3[k] == i)
//					return true;
//			return false;
//		}
//
//		private void handleFailed()
//		{
//			uncompletedCount = 0;
//			completedCount = 0;
//			for (OnDemandData onDemandData = (OnDemandData)requested.reverseGetFirst(); onDemandData != null; onDemandData = (OnDemandData)requested.reverseGetNext())
//				if (onDemandData.incomplete)
//					uncompletedCount++;
//				else
//					completedCount++;
//
//			while (uncompletedCount < 10)
//			{
//				OnDemandData onDemandData_1 = (OnDemandData)aClass19_1368.popHead();
//				if (onDemandData_1 == null)
//					break;
//				if (fileStatus[onDemandData_1.dataType][onDemandData_1.ID] != 0)
//					filesLoaded++;
//				fileStatus[onDemandData_1.dataType][onDemandData_1.ID] = 0;
//				requested.insertHead(onDemandData_1);
//				uncompletedCount++;
//				closeRequest(onDemandData_1);
//				waiting = true;
//			}
//		}
//
//		public void method566()
//		{
//			lock (aClass19_1344)
//			{
//				aClass19_1344.removeAll();
//			}
//		}
//
//		private void checkReceived()
//		{
//			OnDemandData onDemandData;
//			lock (aClass19_1370)
//			{
//				onDemandData = (OnDemandData)aClass19_1370.popHead();
//			}
//			while (onDemandData != null)
//			{
//				waiting = true;
//				byte[] abyte0 = null;
//				if (client.decompressors[0] != null)
//					abyte0 = client.decompressors[onDemandData.dataType + 1].decompress(onDemandData.ID);
////				if (!crcMatches(versions[onDemandData.dataType][onDemandData.ID], crcs[onDemandData.dataType][onDemandData.ID], abyte0))
////					abyte0 = null;
//				lock (aClass19_1370)
//				{
//					if (abyte0 == null)
//					{
//						aClass19_1368.insertHead(onDemandData);
//					}
//					else
//					{
//						onDemandData.buffer = abyte0;
//						lock (aClass19_1358)
//						{
//							aClass19_1358.insertHead(onDemandData);
//						}
//					}
//					onDemandData = (OnDemandData)aClass19_1370.popHead();
//				}
//			}
//		}
//
//		private void method568()
//		{
//			while (uncompletedCount == 0 && completedCount < 10)
//			{
//				if (anInt1332 == 0)
//					break;
//				OnDemandData onDemandData;
//				lock (aClass19_1344)
//				{
//					onDemandData = (OnDemandData)aClass19_1344.popHead();
//				}
//				while (onDemandData != null)
//				{
//					if (fileStatus[onDemandData.dataType][onDemandData.ID] != 0)
//					{
//						fileStatus[onDemandData.dataType][onDemandData.ID] = 0;
//						requested.insertHead(onDemandData);
//						closeRequest(onDemandData);
//						waiting = true;
//						if (filesLoaded < totalFiles)
//							filesLoaded++;
//						statusString = "Loading extra files - " + (filesLoaded * 100) / totalFiles + "%";
//						completedCount++;
//						if (completedCount == 10)
//							return;
//					}
//					lock (aClass19_1344)
//					{
//						onDemandData = (OnDemandData)aClass19_1344.popHead();
//					}
//				}
//				for (int j = 0; j < 4; j++)
//				{
//					byte[] abyte0 = fileStatus[j];
//					int k = abyte0.Length;
//					for (int l = 0; l < k; l++)
//						if (abyte0[l] == anInt1332)
//						{
//							abyte0[l] = 0;
//						if(j == 3) UnityEngine.Debug.Log ("Adding new 3 " + j + " " + l);
//							OnDemandData onDemandData_1 = new OnDemandData();
//							onDemandData_1.dataType = j;
//							onDemandData_1.ID = l;
//							onDemandData_1.incomplete = false;
//							requested.insertHead(onDemandData_1);
//							closeRequest(onDemandData_1);
//							waiting = true;
//							if (filesLoaded < totalFiles)
//								filesLoaded++;
//							statusString = "Loading extra files - " + (filesLoaded * 100) / totalFiles + "%";
//							completedCount++;
//							if (completedCount == 10)
//								return;
//						}
//
//				}
//
//				anInt1332--;
//			}
//		}
//
//		public bool method569(int i)
//		{
//			return anIntArray1348[i] == 1;
//		}
//
//		public OnDemandFetcher()
//		{
//			requested = new NodeList();
//			statusString = "";
//			//crc32 = new CRC32();
//			ioBuffer = new byte[500];
//			fileStatus = new byte[4][];
//			aClass19_1344 = new NodeList();
//			running = true;
//			waiting = false;
//			aClass19_1358 = new NodeList();
//			gzipInputBuffer = new byte[0x71868];
//			nodeSubList = new NodeSubList();
//			versions = new int[4][];
//			crcs = new int[4][];
//			aClass19_1368 = new NodeList();
//			aClass19_1370 = new NodeList();
//		}
//
//		private int totalFiles;
//		private NodeList requested;
//		private int anInt1332;
//		public String statusString;
//		private int writeLoopCycle;
//		private long openSocketTime;
//		private int[] mapIndices3;
//		//private CRC32 crc32;
//		private byte[] ioBuffer;
//		public int onDemandCycle;
//		private byte[][] fileStatus;
//		public client clientInstance;
//		private NodeList aClass19_1344;
//		private int completedSize;
//		private int expectedSize;
//		private int[] anIntArray1348;
//		public int anInt1349;
//		private int[] mapIndices2;
//		private int filesLoaded;
//		private bool running;
//		private NetworkStream outputStream;
//		private int[] mapIndices4;
//		private bool waiting;
//		private NodeList aClass19_1358;
//		private byte[] gzipInputBuffer;
//		private int[] anIntArray1360;
//		private NodeSubList nodeSubList;
//		private NetworkStream inputStream;
//		private Socket socket;
//		private int[][] versions;
//		private int[][] crcs;
//		private int uncompletedCount;
//		private int completedCount;
//		private NodeList aClass19_1368;
//		private OnDemandData current;
//		private NodeList aClass19_1370;
//		private int[] mapIndices1;
//		private byte[] modelIndices;
//		public int loopCycle;


//		private bool crcMatches(int i, int j, byte[] abyte0) {
//			if (abyte0 == null || abyte0.Length < 2)
//				return false;// false
//			int k = abyte0.Length - 2;
//			int l = ((abyte0[k] & 0xff) << 8) + (abyte0[k + 1] & 0xff);
//			crc32.reset();
//			crc32.update(abyte0, 0, k);
//			int i1 = (int) crc32.getValue();
//			return l == i && i1 == j;
//		}
		
		private void readData() {
			try {
				int j = socket.Available;
				if (expectedSize == 0 && j >= 6) {
					waiting = true;
					for (int k = 0; k < 6; k += inputStream
					     .Read(ioBuffer, k, 6 - k))
						;
					int l = ioBuffer[0] & 0xff;
					int j1 = ((ioBuffer[1] & 0xff) << 8) + (ioBuffer[2] & 0xff);
					int l1 = ((ioBuffer[3] & 0xff) << 8) + (ioBuffer[4] & 0xff);
					int i2 = ioBuffer[5] & 0xff;
					current = null;
					for (OnDemandData onDemandData = (OnDemandData) requested
					     .reverseGetFirst(); onDemandData != null; onDemandData = (OnDemandData) requested
					     .reverseGetNext()) {
						if (onDemandData.dataType == l && onDemandData.ID == j1)
							current = onDemandData;
						if (current != null)
							onDemandData.loopCycle = 0;
					}
					
					if (current != null) {
						loopCycle = 0;
						if (l1 == 0) {
							signlink.reporterror("Rej: " + l + "," + j1);
							current.buffer = null;
							if (current.incomplete)
							lock (aClass19_1358) {
								aClass19_1358.insertHead(current);
							}
							else
								current.unlink();
							current = null;
						} else {
							if (current.buffer == null && i2 == 0)
								current.buffer = new byte[l1];
							if (current.buffer == null && i2 != 0)
								throw new IOException("missing start of file");
						}
					}
					completedSize = i2 * 500;
					expectedSize = 500;
					if (expectedSize > l1 - i2 * 500)
						expectedSize = l1 - i2 * 500;
				}
				if (expectedSize > 0 && j >= expectedSize) {
					waiting = true;
					byte[] abyte0 = ioBuffer;
					int i1 = 0;
					if (current != null) {
						abyte0 = current.buffer;
						i1 = completedSize;
					}
					for (int k1 = 0; k1 < expectedSize; k1 += inputStream.Read(
						abyte0, k1 + i1, expectedSize - k1))
						;
					if (expectedSize + completedSize >= abyte0.Length
					    && current != null) {
						if (UnityClient.decompressors[0] != null)
							UnityClient.decompressors[current.dataType + 1]
							.method234(abyte0.Length, abyte0, current.ID);
						if (!current.incomplete && current.dataType == 3) {
							current.incomplete = true;
							current.dataType = 93;
						}
						if (current.incomplete)
						lock (aClass19_1358) {
							aClass19_1358.insertHead(current);
						}
						else
							current.unlink();
					}
					expectedSize = 0;
				}
			} catch (IOException ioexception) {
				try {
					socket.Close();
				} catch (Exception _ex) {
				}
				socket = null;
				inputStream = null;
				outputStream = null;
				expectedSize = 0;
			}
		}
		
		public int mapAmount = 0;
		public static byte[] mapIndexData;
		public void start(StreamLoader streamLoader)
		{
			mapIndexData = UnityClient.ReadAllBytes(signlink.findcachedir() + "map_index.dat");//streamLoader.getDataForName("map_index");
			byte[] mapIndexDataNew = UnityClient.ReadAllBytes(signlink.findcachedir() + "map_index.dat");//streamLoader.getDataForName("map_index");
			Stream stream2 = new Stream(mapIndexDataNew);
			int j1 = stream2.readUnsignedWord();//mapIndexDataNew.Length / 6;
			mapIndices1 = new int[j1];
			mapIndices2 = new int[j1];
			mapIndices3 = new int[j1];
			for(int i2 = 0; i2 < j1; i2++)
			{
				mapIndices1[i2] = stream2.readUnsignedWord();
				mapIndices2[i2] = stream2.readUnsignedWord();
				mapIndices3[i2] = stream2.readUnsignedWord();
				mapAmount++;
			}
			Debug.Log("Map Amount: "+mapAmount+"");
			mapIndexDataNew = streamLoader.getDataForName("midi_index");
			stream2 = new Stream(mapIndexDataNew);
			j1 = mapIndexDataNew.Length;
			anIntArray1348 = new int[j1];
			for(int k2 = 0; k2 < j1; k2++)
				anIntArray1348[k2] = stream2.readUnsignedByte();
			
			//clientInstance = client1;
			running = true;
			//clientInstance.startRunnable(this, 2);
			Loom.StartSingleThread (() => {
						run ();}, System.Threading.ThreadPriority.Normal, true);
		}
		
		public int getNodeCount() {
			lock (nodeSubList) {
				return nodeSubList.getNodeCount();
			}
		}
		
		public void disable() {
			running = false;
		}
		
		public void method554(bool flag) {
			int j = mapIndices1.Length;
			for (int k = 0; k < j; k++)
			if (flag || mapIndices4[k] != 0) {
				method563((byte) 2, 3, mapIndices3[k]);
				method563((byte) 2, 3, mapIndices2[k]);
			}
			
		}
		
		public int getVersionCount(int j) {
			return short.MaxValue;
		}
		
		private void closeRequest(OnDemandData onDemandData) {
//			try {
//				if (socket == null) {
//					long l = NetDrawingTools.CurrentTimeMillis();
//					if (l - openSocketTime < 4000L)
//						return;
//					openSocketTime = l;
//					socket = client.openSocket(43594 + client.portOff);
//					inputStream = new NetworkStream(socket);
//					outputStream = new NetworkStream(socket);
//					outputStream.WriteByte(15);
//					for (int j = 0; j < 8; j++)
//						inputStream.ReadByte();
//					
//					loopCycle = 0;
//				}
//				ioBuffer[0] = (byte) onDemandData.dataType;
//				ioBuffer[1] = (byte) (onDemandData.ID >> 8);
//				ioBuffer[2] = (byte) onDemandData.ID;
//				if (onDemandData.incomplete)
//					ioBuffer[3] = 2;
//				else if (!clientInstance.loggedIn)
//					ioBuffer[3] = 1;
//				else
//					ioBuffer[3] = 0;
//				outputStream.Write(ioBuffer, 0, 4);
//				writeLoopCycle = 0;
//				anInt1349 = -10000;
//				return;
//			} catch (IOException ioexception) {
//			}
//			try {
//				socket.Close();
//			} catch (Exception _ex) {
//			}
//			socket = null;
//			inputStream = null;
//			outputStream = null;
//			expectedSize = 0;
//			anInt1349++;
		}
		
		public int getAnimCount() {
			return short.MaxValue;
		}
		
		public void method558(int i, int j) {
			lock (nodeSubList) {
				for (OnDemandData onDemandData = (OnDemandData) nodeSubList
				     .reverseGetFirst(); onDemandData != null; onDemandData = (OnDemandData) nodeSubList
				     .reverseGetNext())
					if (onDemandData.dataType == i && onDemandData.ID == j)
						return;
				OnDemandData onDemandData_1 = new OnDemandData();
				onDemandData_1.dataType = i;
				onDemandData_1.ID = j;
				onDemandData_1.incomplete = true;
				lock (aClass19_1370) {
					aClass19_1370.insertHead(onDemandData_1);
				}
				nodeSubList.insertHead(onDemandData_1);
			}
		}
		
		public int getModelIndex(int i) {
			return short.MaxValue;
		}

		public void run() {
			try {
				while (running) {
					onDemandCycle++;
					int i = 20;
					if (anInt1332 == 0 && UnityClient.decompressors[0] != null)
						i = 50;
					try {
						Thread.Sleep(i);
					} catch (Exception _ex) {
					}
					waiting = true;
					for (int j = 0; j < 100; j++) {
						if (!waiting)
							break;
						waiting = false;
						checkReceived();
						//	handleFailed();
						if (uncompletedCount == 0 && j >= 5)
							break;
						method568();
						if (inputStream != null)
							readData();
					}
					
					bool flag = false;
					for (OnDemandData onDemandData = (OnDemandData) requested
					     .reverseGetFirst(); onDemandData != null; onDemandData = (OnDemandData) requested
					     .reverseGetNext())
					if (onDemandData.incomplete) {
						flag = true;
						onDemandData.loopCycle++;
						if (onDemandData.loopCycle > 50) {
							onDemandData.loopCycle = 0;
							closeRequest(onDemandData);
						}
					}
					
					if (!flag) {
						for (OnDemandData onDemandData_1 = (OnDemandData) requested
						     .reverseGetFirst(); onDemandData_1 != null; onDemandData_1 = (OnDemandData) requested
						     .reverseGetNext()) {
							flag = true;
							onDemandData_1.loopCycle++;
							if (onDemandData_1.loopCycle > 50) {
								onDemandData_1.loopCycle = 0;
								closeRequest(onDemandData_1);
							}
						}
						
					}
					if (flag) {
						loopCycle++;
						if (loopCycle > 750) {
							try {
								socket.Close();
							} catch (Exception _ex) {
							}
							socket = null;
							inputStream = null;
							outputStream = null;
							expectedSize = 0;
						}
					} else {
						loopCycle = 0;
						statusString = "";
					}
					if (socket != null
					    && outputStream != null
					    && (anInt1332 > 0 || UnityClient.decompressors[0] == null)) {
						writeLoopCycle++;
						if (writeLoopCycle > 500) {
							writeLoopCycle = 0;
							ioBuffer[0] = 0;
							ioBuffer[1] = 0;
							ioBuffer[2] = 0;
							ioBuffer[3] = 10;
							try {
								outputStream.Write(ioBuffer, 0, 4);
							} catch (IOException _ex) {
								loopCycle = 5000;
							}
						}
					}
				}
			} catch (Exception exception) {
				signlink.reporterror("od_ex " + exception.Message);
			}
		}
		
		public void method560(int i, int j) {
			if (UnityClient.decompressors[0] == null)
				return;
			if (anInt1332 == 0)
				return;
			OnDemandData onDemandData = new OnDemandData();
			onDemandData.dataType = j;
			onDemandData.ID = i;
			onDemandData.incomplete = false;
			lock (aClass19_1344) {
				aClass19_1344.insertHead(onDemandData);
			}
		}
		
		public OnDemandData getNextNode() {
			OnDemandData onDemandData;
			lock (aClass19_1358) {
				onDemandData = (OnDemandData) aClass19_1358.popHead();
			}
			if (onDemandData == null)
			{
				return null;
			}
			lock (nodeSubList) {
				onDemandData.unlinkSub();
			}
			if (onDemandData.buffer == null)
				return onDemandData;
			int i = 0;
			try {
				Ionic.Zlib.GZipStream gzipinputstream = new Ionic.Zlib.GZipStream(
					new MemoryStream(onDemandData.buffer), Ionic.Zlib.CompressionMode.Decompress);
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
			onDemandData.buffer = new byte[i];
			//System.Array.Copy(gzipInputBuffer, 0, onDemandData.buffer, 0, i);
			Buffer.BlockCopy(gzipInputBuffer,0, onDemandData.buffer,0, i);
			return onDemandData;
		}
		
		public int method562(int i, int k, int l)
		{
			int i1 = (l << 8) + k;
			for(int j1 = 0; j1 < mapIndices1.Length; j1++)
			{
				if(mapIndices1[j1] == i1)
				{
					if(i == 0)
						return mapIndices2[j1];
					else
						return mapIndices3[j1];
				}
			}
			return -1;
		}
		

		public override void method548(int i) {
			method558(0, i);
		}
		
		public void method563(byte byte0, int i, int j) {
			if (UnityClient.decompressors[0] == null)
				return;
			//if (versions[i][j] == 0)
			//	return;
			byte[] abyte0 = UnityClient.decompressors[i + 1].decompress(j);
			// if(crcMatches(versions[i][j], crcs[i][j], abyte0))
			// return;
			// fileStatus[i][j] = byte0;
			if (byte0 > anInt1332)
				anInt1332 = byte0;
			totalFiles++;
		}
		
		public bool method564(int i) {
			for (int k = 0; k < mapIndices1.Length; k++)
				if (mapIndices3[k] == i)
					return true;
			return false;
		}
		
		private void handleFailed() {
			uncompletedCount = 0;
			completedCount = 0;
			for (OnDemandData onDemandData = (OnDemandData) requested
			     .reverseGetFirst(); onDemandData != null; onDemandData = (OnDemandData) requested
			     .reverseGetNext())
				if (onDemandData.incomplete)
					uncompletedCount++;
			else
				completedCount++;
			
			while (uncompletedCount < 10) {
				OnDemandData onDemandData_1 = (OnDemandData) aClass19_1368
					.popHead();
				if (onDemandData_1 == null)
					break;
				if (fileStatus[onDemandData_1.dataType][onDemandData_1.ID] != 0)
					filesLoaded++;
				fileStatus[onDemandData_1.dataType][onDemandData_1.ID] = 0;
				requested.insertHead(onDemandData_1);
				uncompletedCount++;
				closeRequest(onDemandData_1);
				waiting = true;
			}
		}
		
		public void method566() {
			lock (aClass19_1344) {
				aClass19_1344.removeAll();
			}
		}
		
		private void checkReceived() {
			OnDemandData onDemandData;
			lock (aClass19_1370) {
				onDemandData = (OnDemandData) aClass19_1370.popHead();
			}
			while (onDemandData != null) {
				waiting = true;
				byte[] abyte0 = null;
				if (UnityClient.decompressors[0] != null)
					abyte0 = UnityClient.decompressors[onDemandData.dataType + 1]
					.decompress(onDemandData.ID);
				// if(!crcMatches(versions[onDemandData.dataType][onDemandData.ID],
				// crcs[onDemandData.dataType][onDemandData.ID], abyte0))
				// abyte0 = null;
				lock (aClass19_1370) {
					if (abyte0 == null) {
						aClass19_1368.insertHead(onDemandData);
					} else {
						onDemandData.buffer = abyte0;
						lock (aClass19_1358) {
							aClass19_1358.insertHead(onDemandData);
						}
					}
					onDemandData = (OnDemandData) aClass19_1370.popHead();
				}
			}
		}
		
		private void method568() {
			while (uncompletedCount == 0 && completedCount < 10) {
				if (anInt1332 == 0)
					break;
				OnDemandData onDemandData;
				lock (aClass19_1344) {
					onDemandData = (OnDemandData) aClass19_1344.popHead();
				}
				while (onDemandData != null) {
					if (fileStatus[onDemandData.dataType][onDemandData.ID] != 0) {
						fileStatus[onDemandData.dataType][onDemandData.ID] = 0;
						requested.insertHead(onDemandData);
						closeRequest(onDemandData);
						waiting = true;
						if (filesLoaded < totalFiles)
							filesLoaded++;
						statusString = "Loading extra files - "
							+ (filesLoaded * 100) / totalFiles + "%";
						completedCount++;
						if (completedCount == 10)
							return;
					}
					lock (aClass19_1344) {
						onDemandData = (OnDemandData) aClass19_1344.popHead();
					}
				}
				for (int j = 0; j < 4; j++) {
					byte[] abyte0 = fileStatus[j];
					int k = abyte0.Length;
					for (int l = 0; l < k; l++)
					if (abyte0[l] == anInt1332) {
						abyte0[l] = 0;
						if(j == 3) Debug.Log ("Adding new 3 " + j + " " + l);
						OnDemandData onDemandData_1 = new OnDemandData();
						onDemandData_1.dataType = j;
						onDemandData_1.ID = l;
						onDemandData_1.incomplete = false;
						requested.insertHead(onDemandData_1);
						closeRequest(onDemandData_1);
						waiting = true;
						if (filesLoaded < totalFiles)
							filesLoaded++;
						statusString = "Loading extra files - "
							+ (filesLoaded * 100) / totalFiles + "%";
						completedCount++;
						if (completedCount == 10)
							return;
					}
					
				}
				
				anInt1332--;
			}
		}
		
		public bool method569(int i) {
			return anIntArray1348[i] == 1;
		}
		
		public OnDemandFetcher() {
			requested = new NodeList();
			statusString = "";
			//crc32 = new CRC32();
			ioBuffer = new byte[500];
			fileStatus = new byte[4][];
			aClass19_1344 = new NodeList();
			running = true;
			waiting = false;
			aClass19_1358 = new NodeList();
			gzipInputBuffer = new byte[999999];
			nodeSubList = new NodeSubList();
			versions = new int[4][];
			crcs = new int[4][];
			aClass19_1368 = new NodeList();
			aClass19_1370 = new NodeList();
		}
		
		private int totalFiles;
		private NodeList requested;
		private int anInt1332;
		public String statusString;
		private int writeLoopCycle;
		private long openSocketTime;
		private int[] mapIndices3;
		//private CRC32 crc32;
		private byte[] ioBuffer;
		public int onDemandCycle;
		private byte[][] fileStatus;
		private UnityClient clientInstance;
		private NodeList aClass19_1344;
		private int completedSize;
		private int expectedSize;
		private int[] anIntArray1348;
		public int anInt1349;
		private int[] mapIndices2;
		private int filesLoaded;
		private bool running;
		private NetworkStream outputStream;
		private int[] mapIndices4;
		private bool waiting;
		private NodeList aClass19_1358;
		private byte[] gzipInputBuffer;
		private int[] anIntArray1360;
		private NodeSubList nodeSubList;
		private NetworkStream inputStream;
		private Socket socket;
		private int[][] versions;
		private int[][] crcs;
		private int uncompletedCount;
		private int completedCount;
		private NodeList aClass19_1368;
		private OnDemandData current;
		private NodeList aClass19_1370;
		private int[] mapIndices1;
		private byte[] modelIndices;
		private int loopCycle;
	}
}