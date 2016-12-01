using UnityEngine;
using System.Collections;
using RS2Sharp;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using sign;
using System.IO;

public class UnityClient : MonoBehaviour
{
	public static List<Mesh> meshesAboutToBeCollected = new List<Mesh> ();
	public static List<RuneLiveObject> allRuneObjects = new List<RuneLiveObject> ();
	//public client runeClient;
	public static int loopCycle;
	public static int[] anIntArray1232;
	public int playerX;
	public int playerY;
	public static int gfxTimeout = 10;
	public int gfxTimeoutDebug = 10;
	public static int currentRegionX;
	public static int currentRegionY;
	public int viewDistance = 25;
	private bool drawOverlays = true;
	private List<Player> localPlayers = new List<Player> ();
	public static Material waterMat;
	public static Material terrainMat;
	public static Material objectMat;
	public static Material objectMatTexture;
	public static Material alphaObject;
	public Material waterMatToApply;
	public Material terrainMatToApply;
	public Material objectMatToApply;
	public Material objectMatTextureToApply;
	public Material alphaObjectMatToApply;
	public static Vector3 playerPos;
	public static int renderBufferDistance = 0;
	public static float rotationLerpSpeed = 4F;
	public static GameObject grassObjStatic;
	public GameObject grassObj;
	public static float disposeDistance = 52f;
	public static GameObject deadTreeStatic;
	public GameObject deadTree;
	public static GameObject oakTreeStatic;
	public GameObject oakTree;
	public static GameObject standardTreeStatic;
	public GameObject standardTree;
	public static GameObject willowTreeStatic;
	public GameObject willowTree;
	public static GameObject mapleTreeStatic;
	public GameObject mapleTree;
	public static GameObject yewTreeStatic;
	public GameObject yewTree;
	public static GameObject magicTreeStatic;
	public GameObject magicTree;
	private static List<GameObject> recycledRuneObjects = new List<GameObject> ();
	public bool justTeleported = false;
	private byte[] gameInfo;
	public RS2Sharp.Graphics graphics;
	public static int baseX;
	public static int baseY;
	public int xCameraPos;
	public int yCameraPos;
	public int zCameraPos;
	public int yCameraCurve;
	public int xCameraCurve;
	public static OnDemandFetcher onDemandFetcher;
	public static Decompressor[] decompressors = new Decompressor[5];
	public int[] variousSettings = new int[2000];
	public static bool isEditor = true;
	public static List<RuneObject> objectsToRender = new List<RuneObject> ();
	public static List<GameObject> objectsToDispose = new List<GameObject> ();
	public static int[][] anIntArrayArray1003 = {
		new int[]{ 6798, 107, 10283, 16, 4797, 7744, 5799, 4634, 33697, 22433, 2983,
			54193 },
		new int[]{ 8741, 12, 64030, 43162, 7735, 8404, 1701, 38430, 24094, 10153,
			56621, 4783, 1341, 16578, 35003, 25239 },
		new int[]{ 25238, 8742, 12, 64030, 43162, 7735, 8404, 1701, 38430, 24094,
			10153, 56621, 4783, 1341, 16578, 35003 },
		new int[]{ 4626, 11146, 6439, 12, 4758, 10270 },
		new int[]{ 4550, 4537, 5681, 5673, 5790, 6806, 8076, 4574 } };
	public static int[] anIntArray1204 = { 9104, 10275, 7595, 3610, 7975, 8526,
		918, 38802, 24466, 10145, 58654, 5027, 1457, 16565, 34991, 25486 };
	public static Player myPlayer;
	private byte[] receiveBytes;
	
	public static void DisposeRuneObject (GameObject rune)
	{
		if (rune.GetComponent<RuneLiveObject> ())
			GameObject.DestroyImmediate (rune.GetComponent<RuneLiveObject> ());
		if (rune.GetComponent<RuneLiveObjectUpdate> ())
			GameObject.DestroyImmediate (rune.GetComponent<RuneLiveObjectUpdate> ());
		GameObject.DestroyImmediate (rune.GetComponent<BoxCollider> ());
		rune.GetComponent<MeshFilter> ().sharedMesh = null;
		rune.GetComponent<MeshRenderer> ().enabled = false;
		rune.SetActive (false);
		recycledRuneObjects.Add (rune);
	}
	
	public static byte[] ReadAllBytes (string fileName)
	{
		try {
			byte[] buffer = null;
			using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read)) {
				buffer = new byte[fs.Length];
				fs.Read (buffer, 0, (int)fs.Length);
			}
			return buffer;
		} catch (System.Exception ex) {
			Debug.Log ("Error: " + ex.Message);
			return null;
		}
	}
	
	public void renderWorld ()
	{
		while (objectsToRender.Count > 0) {
			RuneObject rune = null;
			if (objectsToRender.Count > 0)
				rune = objectsToRender [0];
			if (rune != null)
				rune.RenderMesh ();
			if (objectsToRender.Count > 0)
				objectsToRender.RemoveAt (0);
		}
		while (objectsToDispose.Count > 0) {
			GameObject rune = null;
			if (objectsToDispose.Count > 0)
				rune = objectsToDispose [0];
			if (rune != null)
				UnityClient.DisposeRuneObject (rune);
			if (objectsToDispose.Count > 0)
				objectsToDispose.RemoveAt (0);
		}
	}
	
	public static Mesh GetMesh ()
	{
		if (meshesAboutToBeCollected.Count > 0) {
			Mesh mesh = meshesAboutToBeCollected [0];
			meshesAboutToBeCollected.RemoveAt (0);
			mesh.Clear ();
			return mesh;
		}
		return new Mesh ();
	}
	
	public static GameObject GetRuneObject ()
	{
		GameObject anObj = null;
		if (recycledRuneObjects.Count > 0) {
			anObj = recycledRuneObjects [0];
			recycledRuneObjects.RemoveAt (0);
			anObj.GetComponent<MeshRenderer> ().enabled = true;
			anObj.SetActive (true);
		}
		if (anObj == null) {
			anObj = new GameObject ();
			anObj.layer = LayerMask.NameToLayer ("Objects");
			anObj.AddComponent<MeshFilter> ();
			anObj.AddComponent<MeshRenderer> ();
			
		}
		return anObj;
	}
	
	public static int objectsSinceLastBatch = 0;
	public static int[] willowIds = new int[] {
		139,
		142,
		1308,
		2210,
		2372,
		8481,
		8482,
		8483,
		8484,
		8485,
		8486,
		8487,
		8488,
		13414,
		13421,
		37480,
		38616,
		38627,
		38717,
		38718,
		38725,
		51682,
		58006
	};
	public static int[] oakIds = new int[] {
		1281,
		3037,
		8462,
		8463,
		8464,
		8465,
		8466,
		8467,
		10083,
		11999,
		13413,
		13420,
		37479,
		38381,
		38731,
		38732,
		38736,
		38739,
		38741,
		38754,
		51675,
		65667,
		65668
	};
	public static int[] treeIds = new int[] {
		678,
		1276,
		1277,
		1278,
		1280,
		1301,
		1303,
		1304,
		1305,
		1330,
		1331,
		1332,
		2410,
		2411,
		2447,
		2448,
		3033,
		3034,
		3036,
		3879,
		3881,
		3882,
		3883,
		3885,
		3886,
		3887,
		3888,
		3889,
		3890,
		3891,
		3893,
		3928,
		3967,
		3968,
		4048,
		4049,
		4050,
		4051,
		4052,
		4053,
		4054,
		5004,
		5005,
		5045,
		8742,
		8743,
		8973,
		8974,
		10081,
		10082,
		12559,
		12560,
		12895,
		13412,
		13419,
		14308,
		14309,
		14521,
		14571,
		14600,
		14642,
		14738,
		15489,
		16265,
		17857,
		17928,
		18862,
		19093,
		19165,
		19166,
		19167,
		19172,
		19370,
		19446,
		19731,
		19755,
		19765,
		19773,
		19795,
		19801,
		19815,
		25169,
		25174,
		25183,
		25184,
		26201,
		26204,
		26724,
		26725,
		27089,
		30795,
		36765,
		37368,
		37477,
		37478,
		37652,
		38379,
		38380,
		38760,
		38782,
		38783,
		38784,
		38785,
		38786,
		38787,
		38788,
		38789,
		38791,
		38792,
		38795,
		38805,
		38842,
		38870,
		38872,
		38874,
		38875,
		38878,
		38880,
		38881,
		38883,
		38884,
		38886,
		38887,
		38888,
		38889,
		38892,
		38895,
		38896,
		38897,
		38898,
		38900,
		38910,
		38911,
		38912,
		38914,
		39090,
		39097,
		39100,
		39119,
		39121,
		39136,
		39137,
		39138,
		39145,
		39154,
		39166,
		39168,
		39328,
		39430,
		40196,
		40294,
		40295,
		40296,
		40297,
		40298,
		40299,
		40300,
		40301,
		40302,
		40303,
		40312,
		40313,
		40318,
		40320,
		40345,
		40346,
		40347,
		40348,
		40349,
		41860,
		43528,
		43548,
		43550,
		43551,
		43552,
		46276,
		7755,
		49900,
		51668,
		59911,
		59912,
		59913,
		59914,
		59915,
		59916,
		59917,
		59918,
		59920,
		61379,
		61527,
		61528,
		61532,
		61533,
		61537,
		61538,
		61542,
		61543,
		61588,
		61900,
		61901,
		61902,
		61903,
		61904,
		62644,
		65253,
		65254,
		65255,
		65976,
		65977,
		66001,
		66002,
		66003,
		66004,
		68953,
		68954,
		69506,
		70036,
		70037,
		70060,
		70061,
		70063,
		70064,
		72589,
		72609,
		72610
	};
	public static int[] yewIds = new int[] {
		1309,
		8503,
		8504,
		8505,
		8506,
		8507,
		8508,
		8509,
		8510,
		8511,
		8512,
		8513,
		12000,
		38755,
		38758,
		38759,
		46278,
		51645
	};
	public static int[] deadIds = new int[] {
		1282,
		1283,
		1284,
		1285,
		1286,
		1287,
		1288,
		1289,
		1291,
		1365,
		1383,
		1384,
		2020,
		6756,
		11112,
		11866,
		12732,
		13411,
		13418,
		20750,
		20751,
		23381,
		32294,
		37334,
		37481,
		37482,
		37483,
		38382,
		38383,
		41713,
		42893,
		46757,
		46758,
		46759,
		46760,
		46761,
		46762,
		46763,
		46764,
		47594,
		47596,
		47598,
		47600,
		52786,
		68901,
		68902,
		68903,
		69139,
		69141,
		69142,
		69143,
		69144,
		69145,
		69146,
		70099,
		70306,
		77095
	};
	public static int[] mapleIds = new int[] {
		1307,
		4674,
		8435,
		8436,
		8437,
		8438,
		8439,
		8440,
		8441,
		8442,
		8443,
		8444,
		13415,
		13423,
		46277,
		51843
	};
	public static int[] grassIds = new int[] {
		17476,1253,1254,1255,1256,1257,1258,1259,1260,1272,1273,1274,1275,1241,1242,1243,1244,1245,3898,3698,3699,3700,3701,3899,3900,3907,3908,3909,3910,3911,4735,6792,6793,6794,6795,6796,6797
	};
	
	public void Start ()
	{
		terrainMat = terrainMatToApply;
		waterMat = waterMatToApply;
		objectMat = objectMatToApply;
		objectMatTexture = objectMatTextureToApply;
		alphaObject = alphaObjectMatToApply;
		grassObjStatic = grassObj;
		deadTreeStatic = deadTree;
		oakTreeStatic = oakTree;
		standardTreeStatic = standardTree;
		willowTreeStatic = willowTree;
		mapleTreeStatic = mapleTree;
		yewTreeStatic = yewTree;
		magicTreeStatic = magicTree;
		Loom.StartSingleThread (() => {
			StartOSSocket ();}, System.Threading.ThreadPriority.Normal, false);
		//Loom.StartSingleThread(()=>{GameLoop();},System.Threading.ThreadPriority.Normal,false);
	}
	
//	private void GameLoop()
//	{
//		processOnDemandQueue();
//	}
	
	public static byte[] GetModel (int modelId)
	{
		return ReadAllBytes (sign.signlink.findcachedir() + "7/" + modelId + ".mdl");
	
		byte[] modelData = UnityClient.decompressors [1].decompress (modelId);
		if (modelData != null) {
			byte[] gzipInputBuffer = new byte[modelData.Length * 100];
			int i2 = 0;
			try {
				Ionic.Zlib.GZipStream gzipinputstream = new Ionic.Zlib.GZipStream (
					new MemoryStream (modelData), Ionic.Zlib.CompressionMode.Decompress);
				do {
					if (i2 == gzipInputBuffer.Length)
						throw new Exception ("buffer overflow!");
					int k = gzipinputstream.Read (gzipInputBuffer, i2,
					                             gzipInputBuffer.Length - i2);
					if (k == 0)
						break;
					i2 += k;
				} while (true);
			} catch (IOException _ex) {
				throw new Exception ("error unzipping");
			}
			modelData = new byte[i2];
			System.Array.Copy (gzipInputBuffer, 0, modelData, 0, i2);
		}
		return modelData;
	}
	private bool initialized = false;
	private void InitializeRune ()
	{
		anIntArray1232 = new int[32];
		int a = 2;
		for (int k = 0; k < 32; k++) {
			anIntArray1232 [k] = a - 1;
			a += a;
		}
		signlink.storeid = 0;
		signlink.run ();
		for (int i = 0; i < 5; i++)
			decompressors [i] = new Decompressor (signlink.cache_dat, signlink.cache_idx [i], i + 1);
		StreamLoader streamLoader_6 = streamLoaderForName (5, "update list",
		                                                  "versionlist");
		onDemandFetcher = new OnDemandFetcher ();
		onDemandFetcher.start (streamLoader_6);
		
		StreamLoader streamLoader = streamLoaderForName (2, "config", "config");
		RS2Sharp.Animation.unpackConfig (streamLoader);
		//RS2Sharp.Texture.method368(streamLoader_3);
		RS2Sharp.Texture.method372 (0.80000000000000004D);
		RS2Sharp.Texture.method367 ();
		ItemDef.unpackConfig (streamLoader);
		EntityDef.unpackConfig (streamLoader);
		IDK.unpackConfig (streamLoader);
		SpotAnim.unpackConfig (streamLoader);
		Varp.unpackConfig (streamLoader);
		VarBit.unpackConfig (streamLoader);
		ObjectDef.unpackConfig (streamLoader);
		Flo.unpackConfig (streamLoader);
		OverLayFlo317.unpackConfig (streamLoader);
		Model.initialize (65535, onDemandFetcher);
		initialized = true;
	}
	
	private StreamLoader streamLoaderForName (int i, String s, String s1)
	{
		byte[] abyte0 = null;
		int l = 5;
		if (decompressors [0] != null)
			abyte0 = decompressors [0].decompress (i);
		StreamLoader streamLoader = new StreamLoader (abyte0);
		return streamLoader;
	}

	private void StartOSSocket ()
	{	
		string returnData;
		
		using (UdpClient udpClient = new UdpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 43592))) {
			IPEndPoint remoteIpEndPoint = new IPEndPoint (IPAddress.Parse ("127.0.0.1"), 43592);
			while (true) {
				receiveBytes = udpClient.Receive (ref remoteIpEndPoint);
				if (receiveBytes != null && receiveBytes.Length > 0) {
					if (receiveBytes [0] == 1 && receiveBytes.Length < 10000) { //Game info
						gameInfo = TrimFirst (receiveBytes);
					} else if (receiveBytes [0] == 2 && receiveBytes.Length < 1000) { //Base Info
						if (signlink.osHDDir == null) {
							signlink.osHDDir = Encoding.UTF8.GetString (TrimFirst (receiveBytes));
							Debug.Log ("Cache Dir: " + signlink.osHDDir);
							if(!initialized) Loom.DispatchToMainThread(()=>{InitializeRune ();},false,true);
						}
					} else {
						if (RS2Sharp.Graphics.gameScreen == null && ((sbyte)receiveBytes [0] == -119 && (sbyte)receiveBytes [1] == 80 && (sbyte)receiveBytes [2] == 78))
							RS2Sharp.Graphics.gameScreen = receiveBytes;
						else if (RS2Sharp.Graphics.gameScreen2 == null) {
							RS2Sharp.Graphics.gameScreen2 = receiveBytes;
							graphics.dirty = true;
						}
					}
				}
			}
		}
	}
	
	private byte[] TrimFirst (byte[] input)
	{
		byte[] newBytes = new byte[input.Length - 1];
		for (int i = 0; i < newBytes.Length; ++i)
			newBytes [i] = input [i + 1];
		return newBytes;
	}

	public static Texture2D GetTextureResource (int texID)
	{
		Texture2D tex = Resources.Load ("Textures/Rune Textures/" + texID) as Texture2D;
		if (tex != null)
			return tex;
		return null;
		byte[] texData = client.ReadAllBytes (sign.signlink.findcachedir () + "images/" + texID + ".png");
		tex.LoadImage (texData);
		tex.Apply ();
		tex.name = "" + texID;
		return tex;
	}

	public static Texture2D LoadTexture (int texID)
	{
		return GetTextureResource (texID);
	}

	public static Material getMaterial (int matID)
	{
		if(matID == -1) return objectMat;
		else
		{
			Material newMat = new Material(objectMatTexture);
			Texture2D textureToLoad = Resources.Load ("OS Textures/"+matID) as Texture2D;
			newMat.SetTexture("_MainTex",textureToLoad);
			if(textureToLoad == null) Debug.Log ("Missing texture ID: " + matID);
			return newMat;
		}
	}

	public static Material getGroundMaterial (int texId)
	{
		return terrainMat;
	}
	
	public static float GetPixelGray (int x, int y, Color[] colorList, int tWidth)
	{
		float grayOut = 0f;
		grayOut = colorList [x + (tWidth * y)].grayscale * 255;
		return grayOut;
	}
	
	private void UpdateGameInfo ()
	{
		if (gameInfo != null && gameInfo.Length > 0) {
			string gameInfoString = Encoding.UTF8.GetString (gameInfo);
			string[] split = gameInfoString.Split ('}');
			
			string playerInfo = split [0].Replace ("Players:", "");
			string[] playersSplit = playerInfo.Split ('#');
			List<string> currentPlayerNames = new List<string> ();
			foreach (string s in playersSplit) {
				string[] info = s.Split (',');
				if (info.Length == 5) {
					currentPlayerNames.Add (info [0]);
					bool containsPlayer = false;
					foreach (Player p in localPlayers) {
						if (p.name == info [0]) {
							containsPlayer = true;
							p.x = int.Parse (info [1]);
							p.y = int.Parse (info [2]);
							p.playerObj.UpdatePos (((float.Parse (info [1]))/128f), float.Parse (info [3])/128f, ((float.Parse (info [2])/128f)));
							break;
						}
					}
					if (!containsPlayer) {
						Player p = new Player ();//info [0], new Vector3 (int.Parse (info [1]), int.Parse (info [3]), int.Parse (info [2])), int.Parse (info [4]));
						p.x = int.Parse (info [1]);
						p.y = int.Parse (info [2]);
						p.playerObj.UpdatePos ((float.Parse ((info [1]))/128f), float.Parse (info [3])/128f, ((float.Parse (info [2])/128f)));
						p.name = info [0];
						localPlayers.Add (p);
						if (info [0].StartsWith ("Oldschool")) {
							UnityClient.myPlayer = p;
							p.playerObj.isMainPlayer = true;
							//myPlayer = playerArray[myPlayerIndex] = new Player();
							//myPlayer.playerObj.isMainPlayer = true;
						}
						p.playerObj.Render (new Model (0));
					}
				}
			}
			List<Player> playersToRemove = new List<Player> ();
			foreach (Player player in localPlayers) {
				string playerName = player.name;
				if (!currentPlayerNames.Contains (playerName)) {
					//player.Destroy ();
					GameObject.DestroyImmediate (player.playerObj.myObject);
					playersToRemove.Add (player);
				}
			}
			foreach (Player player in playersToRemove)
				localPlayers.Remove (player);
			
			string npcInfo = split [1].Replace ("Npcs:", "");
			string[] npcSplit = npcInfo.Split ('#');
			List<int> currentNpcNames = new List<int> ();
			foreach (string s in npcSplit) {
				string[] info = s.Split (',');
				if (info.Length == 6) {
//					currentNpcNames.Add (int.Parse(info [5]));
//					bool containsNpc = false;
//					foreach (Npc n in localNpcs) {
//						if (n.uniqueId == int.Parse(info [5])) {
//							containsNpc = true;
//							n.UpdatePos (new Vector3 (int.Parse (info [1]), int.Parse (info [3]), int.Parse (info [2])));
//							break;
//						}
//					}
//					if (!containsNpc)
//						localNpcs.Add (new Npc (info [0], new Vector3 (int.Parse (info [1]), int.Parse (info [3]), int.Parse (info [2])), int.Parse (info [4]), int.Parse (info [5])));
				}
			}
//			List<Npc> npcsToRemove = new List<Npc> ();
//			foreach (Npc npc in localNpcs) {
//				int npcName = npc.uniqueId;
//				if (!currentNpcNames.Contains (npcName)) {
//					npc.Destroy ();
//					npcsToRemove.Add (npc);
//				}
//			}
//			foreach (Npc npc in npcsToRemove)
//				localNpcs.Remove (npc);
			
			string clientInfo = split [2].Replace ("GameInfo:", "");
			string[] clientInfoSplit = clientInfo.Split (',');
			baseX = int.Parse (clientInfoSplit [0]);
			baseY = int.Parse (clientInfoSplit [1]);
			xCameraPos = int.Parse (clientInfoSplit [2]);
			yCameraPos = int.Parse (clientInfoSplit [3]);
			zCameraPos = int.Parse (clientInfoSplit [4]);
			yCameraCurve = int.Parse (clientInfoSplit [5]);
			xCameraCurve = int.Parse (clientInfoSplit [6]);
			currentRegionX = int.Parse (clientInfoSplit [7]);
			currentRegionY = int.Parse (clientInfoSplit [8]);
			gameInfo = null;
		}
	}
	
	public static Texture2D CreateDOT3 (Texture2D pixmap, float contrast, float treshold, int id)
	{
		Texture2D resourceBump = Resources.Load ("Textures/Rune Textures/" + id + " Bump") as Texture2D;
		if (resourceBump)
			return resourceBump;
//		Debug.Log("Missing Bump Texture? " + id);
//		return null;
		return null;
		Texture2D retTexture = new Texture2D (pixmap.width, pixmap.height, TextureFormat.ARGB32, false);
		Color[] pixColor = pixmap.GetPixels ();
		Color[] retColor = new Color[pixColor.Length];
		for (int x = 0; x<pixmap.width; x++) {
			for (int y = 0; y<pixmap.height; y++) {
				
				float tl = -1.0f;
				float tm = -1.0f;
				float tr = -1.0f;
				float ml = -1.0f;
				float mm = -1.0f;
				float mr = -1.0f;
				float bl = -1.0f;
				float bm = -1.0f;
				float br = -1.0f;
				
				if (x > 0 && y > 0) {
					tl = GetPixelGray (x - 1, y - 1, pixColor, pixmap.width);
					if (treshold > 0) {
						if (tl > treshold)
							tl = 255f;
						else
							tl = 0;
					}
				}	
				if (y > 0) {
					tm = GetPixelGray (x, y - 1, pixColor, pixmap.width);
					if (treshold > 0) {
						if (tm > treshold)
							tm = 255f;
						else
							tm = 0;
					}
				}
				if (x < pixmap.width - 1 && y > 0) {				
					tr = GetPixelGray (x + 1, y - 1, pixColor, pixmap.width);
					if (treshold > 0) {
						if (tr > treshold)
							tr = 255f;
						else
							tr = 0;
					}
				}
				if (x > 0) {										
					ml = GetPixelGray (x - 1, y, pixColor, pixmap.width);
					if (treshold > 0) {
						if (ml > treshold)
							ml = 255f;
						else
							ml = 0;
					}
					mm = GetPixelGray (x, y, pixColor, pixmap.width);
					if (treshold > 0) {
						if (mm > treshold)
							mm = 255f;
						else
							mm = 0;
					}
				}
				if (x < pixmap.width - 1) {					
					mr = GetPixelGray (x + 1, y, pixColor, pixmap.width);
					if (treshold > 0) {
						if (mr > treshold)
							mr = 255f;
						else
							mr = 0;
					}
				}
				if (x > 0 && y < pixmap.height - 1) {		
					bl = GetPixelGray (x - 1, y + 1, pixColor, pixmap.width);
					if (treshold > 0) {
						if (bl > treshold)
							bl = 255f;
						else
							bl = 0;
					}
				}
				if (y < pixmap.height - 1) {		
					bm = GetPixelGray (x, y + 1, pixColor, pixmap.width);
					if (treshold > 0) {
						if (bm > treshold)
							bm = 255f;
						else
							bm = 0;
					}
				}
				if (x < pixmap.width - 1 && y < pixmap.height - 1) {	
					br = GetPixelGray (x + 1, y + 1, pixColor, pixmap.width);
					if (treshold > 0) {
						if (br > treshold)
							br = 255f;
						else
							br = 0;
					}
				}
				
				if (tl == -1.0f)
					tl = mm;
				if (tm == -1.0f)
					tm = mm;
				if (tr == -1.0f)
					tr = mm;
				if (ml == -1.0f)
					ml = mm;
				if (mr == -1.0f)
					mr = mm;
				if (bl == -1.0f)
					bl = mm;
				if (bm == -1.0f)
					bm = mm;
				if (br == -1.0f)
					br = mm;
				
				float vx = 0.0f;
				float vy = 0.0f;
				float vz = 1.0f;
				
				float isq2 = 1.0f / Mathf.Sqrt (2.0f);
				float sum = 1.0f + isq2 + isq2;
				
				float al = (tl * isq2 + ml + bl * isq2) / sum;
				float ar = (tr * isq2 + mr + br * isq2) / sum;
				float at = (tl * isq2 + tm + tr * isq2) / sum;
				float ab = (bl * isq2 + bm + br * isq2) / sum;			
				
				vx = (al - ar) / 255.0f * contrast;
				vy = (at - ab) / 255.0f * contrast;
				
				
				float r = vx * 128.5f + 128.5f;
				float g = vy * 128.5f + 128.5f;
				float b = vz * 255.0f;
				
				if (r < 0)
					r = 0f;
				if (r > 255)
					r = 255f;
				if (g < 0)
					g = 0f;
				if (g > 255)
					g = 255f;
				if (b < 0)
					b = 0f;
				if (b > 255)
					b = 255f;
				
				Color rgb = new Color (r / 255f, g / 255f, b / 255f, 0.5f);
				
				retColor [x + (pixmap.width * y)] = rgb;
			}
		}
		retTexture.SetPixels (retColor);
		retTexture.filterMode = FilterMode.Trilinear;
		retTexture.Apply ();
		retTexture.name = id + " Bump";
		return retTexture;
	}

	public void Update ()
	{
		if(initialized)
		{
		UpdateGameInfo ();
		gfxTimeout = gfxTimeoutDebug;
		if (UnityClient.myPlayer != null) {
			if (!MapManager.initialized)
				MapManager.Initialize ();
			playerX = (UnityClient.myPlayer.x >> 7) + baseX;
			playerY = (UnityClient.myPlayer.y >> 7) + baseY;
			if (currentRegionX != 0 || currentRegionY != 0)
				for (int pX = playerX - viewDistance; pX <= playerX + viewDistance; pX+=viewDistance) {
					for (int pY = playerY - viewDistance; pY <= playerY + viewDistance; pY+=viewDistance) {
						int cX = pX / 64;
						int cY = pY / 64;
						if (!MapManager.containsRegion (cX, cY)) {
							MapManager.DrawRegion (cX, cY);
						}
					}
				}
		}
//		
		if (objectsSinceLastBatch > 300) {
			objectsSinceLastBatch = 0;
			foreach (Region region in MapManager.activeRegions) {
				GameObject objectRoot = region.objectRoot;
				for (int i = 0; i < objectRoot.transform.childCount; ++i) {
					GameObject go = objectRoot.transform.GetChild (i).gameObject;
					MeshFilter filter = go.GetComponent<MeshFilter> ();
					if (filter != null) {
						Mesh mesh = filter.sharedMesh;
						if (mesh != null && !mesh.name.StartsWith ("Combined"))
							meshesAboutToBeCollected.Add (mesh);
					}
				}
				StaticBatchingUtility.Combine (objectRoot);
			}
			
			System.GC.Collect ();
		}
		
		if (justTeleported && !teleWaiting)
			StartCoroutine (teleWait ());
		
//		if(!justTeleported)
//		for(int i = 0; i < allRuneObjects.Count; ++i)
//			{
//				start:
//				if(allRuneObjects.Count > i)
//				{
//				RuneLiveObject obj = allRuneObjects[i];
//				if(obj == null)
//				{
//					allRuneObjects.RemoveAt(i);
//					goto start;
//				}
//				else
//				{
//					if(obj.runeScript.myObject != null)
//					{
//						Vector3 objPos = obj.runeScript.objPos;
//						if(playerPos != Vector3.zero && !obj.runeScript.isMainPlayer)
//						if(((objPos.x < (playerPos.x-disposeDistance)) || (objPos.x > (playerPos.x+disposeDistance)) || (objPos.z < (playerPos.z-disposeDistance)) || (objPos.z > (playerPos.z+disposeDistance))))
//						{
//							if(!UnityClient.objectsToDispose.Contains(obj.runeScript.myObject) && !obj.runeScript.isGrassObj && !obj.runeScript.isTreeObj) UnityClient.objectsToDispose.Add(obj.runeScript.myObject);
//							allRuneObjects.RemoveAt(i);
//							goto start;
//						}
//					}		
//				}
//				}
//			}
		renderWorld ();
		}
	}
	
	private bool teleWaiting = false;

	private IEnumerator teleWait ()
	{
		teleWaiting = true;
		yield return new WaitForSeconds (3f);
		teleWaiting = false;
		justTeleported = false;
	}

}