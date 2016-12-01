// Decompiled by Jad v1.5.8f. Copyright 2001 Pavel Kouznetsov.
// Jad home page: http://www.kpdus.com/jad.html
// Decompiler options: packimports(3)
// Source File Name:   signlink.java

namespace sign
{

	using System;
	using System.IO;
	using System.Net;
	using System.Net.Sockets;
	using System.Threading;
	using RS2Sharp;

	public class signlink
	{
	
		public static string osHDDir = null;

		public static void startpriv(IPAddress inetaddress)
		{
			threadliveid = (int)(RS2Sharp.Random.Next() * 99999999D);
			active = false;
			socketreq = 0;
			dnsreq = null;
			savereq = null;
			urlreq = null;
			socketip = inetaddress;
		}

		public static void run()
		{
	
			active = true;
			String s = findcachedir();
			uid = getuid(s);
			try
			{
				if(cache_dat == null) cache_dat = new FileStream(s + "main_file_cache.dat", FileMode.Open);
				//if (cache_dat.Length > 0x3200000L)
				//	File.Delete(s + "main_file_cache.dat");
				for (int j = 0; j < 5; j++)
				if(cache_idx[j] == null) 	cache_idx[j] = new FileStream(s + "main_file_cache.idx" + j, FileMode.Open);

			}
			catch (Exception exception)
			{
				UnityEngine.Debug.Log(exception.StackTrace);
			}
			//for (int i = threadliveid; threadliveid == i; )
			//{
			//if(client.finished) return;
				if (socketreq != 0 && socket == null)
				{
					try
					{
						socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
						socket.Connect(socketip, socketreq);
					}
					catch (Exception _ex)
					{
						socket = null;
					}
					socketreq = 0;
				}
				else
//					if (threadreq != null)
//					{
//						Thread thread = threadreq;
//						thread.IsBackground = true;
//						thread.Start();
//						//thread.Priority = (ThreadPriority)threadreqpri;
//						threadreq = null;
//					}
//					else
						if (dnsreq != null)
						{
							try
							{
								dns = Dns.GetHostByAddress(dnsreq).HostName;
							}
							catch (Exception _ex)
							{
								dns = "unknown";
							}
							dnsreq = null;
						}
						else
							if (savereq != null)
							{
								if (savebuf != null)
									try
									{
										FileStream fileoutputstream = new FileStream(s + savereq, FileMode.Open);
										fileoutputstream.Write(savebuf, 0, savelen);
										fileoutputstream.Close();
									}
									catch (Exception _ex) { }
								if (waveplay)
								{
									String wave = s + savereq;
									waveplay = false;
								}
								if (midiplay)
								{
									midi = s + savereq;
									midiplay = false;
								}
								savereq = null;
							}
							else
								if (urlreq != null)
								{
									try
									{
										UnityEngine.Debug.Log("urlstream");
										//TODO: implement
										//urlstream = new DataInputStream((new URL(mainapp.getCodeBase(), urlreq)).openStream());
										//urlstream = new MemoryStream(
									}
									catch (Exception _ex)
									{
										urlstream = null;
									}
									urlreq = null;
								}
				//try
				//{
				//	Thread.Sleep(50);
				//}
				//catch (Exception _ex) { }
			//}

		}

		public static String findcachedir()
		{
			if(UnityClient.isEditor) return System.Environment.GetFolderPath( System.Environment.SpecialFolder.Personal )+"/Documents/runehdcache/";
				return System.Environment.GetFolderPath( System.Environment.SpecialFolder.Personal )+"/runehdcache/";
		}
		
		public static String findcachedirORIG()
		{
			string[] locations = new string[] {
            "./", "c:/windows/", "c:/winnt/", "d:/windows/", "d:/winnt/", "e:/windows/", "e:/winnt/", "f:/windows/", "f:/winnt/", "c:/", "~/",
            "/tmp/", "", "c:/rscache", "/rscache" };
			if (storeid < 32 || storeid > 34)
				storeid = 32;
			String s = ".file_store_" + storeid;
			for (int i = 0; i < locations.Length; i++)
				try
				{
					String s1 = locations[i];
					if (s1.Length > 0)
					{
						if (!Directory.Exists(s1))
							continue;
					}
					if (File.Exists(s1 + s) || Directory.Exists(s1 + s))
						return s1 + s + "/";
				}
				catch (Exception _ex) { }

			return null;

		}

		private static int getuid(String s)
		{
			try
			{
				bool exists = File.Exists(s + "uid.dat");
				FileStream file = null;
				if (exists)
					file = File.Open(s + "uid.dat", FileMode.Open);
				if (!exists || file.Length < 4L)
				{
					BinaryWriter dataoutputstream = new BinaryWriter(File.OpenRead(s + "uid.dat"));
					dataoutputstream.Write((int)(RS2Sharp.Random.Next() * 99999999D));
					dataoutputstream.Close();
				}
			}
			catch (Exception _ex) { }
			try
			{
				BinaryReader datainputstream = new BinaryReader(File.OpenRead(s + "uid.dat"));
				int i = datainputstream.ReadInt32();
				datainputstream.Close();
				return i + 1;
			}
			catch (Exception _ex)
			{
				return 0;
			}
		}

		public static Socket opensocket(int i)
		{
			return null;
			//for (socketreq = i; socketreq != 0; )
			//	try
			//	{
			//		Thread.Sleep(50);
			//	}
			//	catch (Exception _ex) { }

			if (socket == null)
			{
				socket = new System.Net.Sockets.TcpClient("127.0.0.1", i).Client;
			}
			//else
				return socket;
		}

		public static MemoryStream openurl(string s)
		{
			for (urlreq = s; urlreq != null; )
				try
				{
					//Thread.Sleep(50);
				}
				catch (Exception _ex) { }

			if (urlstream == null)
				throw new IOException("could not open: " + s);
			else
				return urlstream;
		}

		public static void dnslookup(String s)
		{
			dns = s;
			dnsreq = s;
		}

//		public static void startthread(Thread runnable, int i)
//		{
//			threadreqpri = i;
//			threadreq = runnable;
//		}

		public static bool wavesave(byte[] abyte0, int i)
		{
			if (i > 0x1e8480)
				return false;
			if (savereq != null)
			{
				return false;
			}
			else
			{
				wavepos = (wavepos + 1) % 5;
				savelen = i;
				savebuf = abyte0;
				waveplay = true;
				savereq = "sound" + wavepos + ".wav";
				return true;
			}
		}

		public static bool wavereplay()
		{
			if (savereq != null)
			{
				return false;
			}
			else
			{
				savebuf = null;
				waveplay = true;
				savereq = "sound" + wavepos + ".wav";
				return true;
			}
		}

		public static void midisave(byte[] abyte0, int i)
		{
			if (i > 0x1e8480)
				return;
			if (savereq != null)
			{
			}
			else
			{
				midipos = (midipos + 1) % 5;
				savelen = i;
				savebuf = abyte0;
				midiplay = true;
				savereq = "jingle" + midipos + ".mid";
			}
		}

		public static void reporterror(String s)
		{
			UnityEngine.Debug.Log("Error: " + s);
		}

		private signlink()
		{
		}

		public static int clientversion = 317;
		public static int uid;
		public static int storeid = 32;
		public static FileStream cache_dat = null;
		public static FileStream[] cache_idx = new FileStream[5];
		public static bool sunjava;
		private static bool active;
		private static int threadliveid;
		private static IPAddress socketip;
		private static int socketreq;
		private static Socket socket = null;
		private static int threadreqpri = 1;
		//private static Thread threadreq = null;
		private static String dnsreq = null;
		public static String dns = null;
		private static String urlreq = null;
		private static MemoryStream urlstream = null;
		private static int savelen;
		private static String savereq = null;
		private static byte[] savebuf = null;
		private static bool midiplay;
		private static int midipos;
		public static String midi = null;
		public static int midivol;
		public static int midifade;
		private static bool waveplay;
		private static int wavepos;
		public static int wavevol;
		public static bool doReportError = true;
		public static String errorname = "";

	}
}