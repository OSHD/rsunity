using UnityEngine;
using System;
using System.Collections.Generic;
using RS2Sharp;
using System.IO;

public class Region
{
	public bool isPreMade = false;
	public int anInt131;
	public bool lowMemory = false;
	public int plane458 = 99;
	private int[] anIntArray140 = { 16, 32, 64, 128 };
	private int[] anIntArray152 = { 1, 2, 4, 8 }; // orientation -> ??
	private int BLOCKED_TILE = 1;
	private int BRIDGE_TILE = 2;
	private int[] COSINE_VERTICES = { 1, 0, -1, 0 };
	private int FORCE_GROUND = 8;
	private int hueOffset = (int)(UnityEngine.Random.value * 17) - 8;
	private int luminanceOffset = (int)(UnityEngine.Random.value * 33) - 16;
	private int[] SINE_VERTICIES = { 0, -1, 0, 1 };
	private int[] anIntArray128;
	private int[][] anIntArrayArray139;
	private int[][][] anIntArrayArrayArray135;
	private int[] chromas;
	private byte[][][] collisionPlaneModifiers; // awful name - 0x2 = bridge, 0x4 = roof, etc
	private int[] hues;
	private int length;
	private int[] luminances;
	public byte[][][] tile_layer1_orientation;
	public sbyte[][][] tile_layer1_type;
	public byte[][][] tile_layer1_shape;
	private int[] saturations;
	private byte[][][] shading;
	public byte[][][] tile_layer0_type;
	public int[][][] localTileH;
	private int width;
	public int myRegionX;
	public int myRegionY;
	public GameObject objectRoot;
	public GameObject animatedObjectRoot;
	public RSTerrain rsTerrain;
	private byte[][][] aByteArrayArrayArray1258;
	private int[] localRegionIds;
	private sbyte[][] localRegionLandscapeData;
	private int[] localRegionLandscapeIds;
	private byte[][] localRegionMapData;
	private int[] localRegionMapIds;
	private int[][][] localRegions = NetDrawingTools.CreateTrippleIntArray (4, 13, 13);//new int[4][13][13];
	private int regionbasesX;
	private int regionbasesY;
	public int anInt470 = -1;
	public int anInt471 = -1;
	public int anInt475;
	public static int PLANE_COUNT = 4;
	public int sceneWidth;
	bool aBoolean467;
	bool[][] aBooleanArrayArray492;
	bool[][][][] aBooleanArrayArrayArrayArray491 = NetDrawingTools.CreateQuadBoolArray (8, 32, 51, 51);//new bool[8][32][51][51];
	Class47[] aClass47Array476 = new Class47[500];
	int anInt446;
	int anInt447;
	int anInt448;
	int anInt449;
	int anInt450;
	int anInt451;
	int anInt452;
	int anInt453;
	int anInt454;
	int anInt458;
	int anInt459;
	int anInt460;
	int anInt461;
	int anInt468;
	int anInt469;
	int anInt493;
	int anInt494;
	int anInt495;
	int anInt496;
	int anInt497;
	int anInt498;
	int[] anIntArray463 = { 53, -53, -53, 53 };
	int[] anIntArray464 = { -53, -53, 53, 53 };
	int[] anIntArray465 = { -45, 45, 45, -45 };
	int[] anIntArray466 = { 45, 45, -45, -45 };
	int[] anIntArray478 = { 19, 55, 38, 155, 255, 110, 137, 205, 76 };
	int[] anIntArray479 = { 160, 192, 80, 96, 0, 144, 80, 48, 160 };
	int[] anIntArray480 = { 76, 8, 137, 4, 0, 1, 38, 2, 19 };
	int[] anIntArray481 = { 0, 0, 2, 0, 0, 2, 1, 1, 0 };
	int[] anIntArray482 = { 2, 0, 0, 2, 0, 0, 0, 4, 4 };
	int[] anIntArray483 = { 0, 4, 4, 8, 0, 0, 8, 0, 0 };
	int[] anIntArray484 = { 1, 1, 0, 0, 0, 8, 0, 0, 8 };
	int[] anIntArray485 = { 41, 39248, 41, 4643, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 43086, 41, 41, 41, 41,
		41, 41, 41, 8602, 41, 28992, 41, 41, 41, 41, 41, 5056, 41, 41, 41, 7079, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41,
		3131, 41, 41, 41 };
	int[] clusterCounts = new int[PLANE_COUNT];
	InteractiveObject[] interactables = new InteractiveObject[100];
	public int[][][] tile_layer0_colour;
	public int[] terrainIndices;
	public int[] objectLandscapeIDS;
	private CollisionMap[] collisionMaps = new CollisionMap[4];
	//
	public void DrawObjects (GameObject regionContainer)
	{
		anInt446 = 0;
		
		anInt446 = 0;
		for (int z = 0; z < 4; ++z) {
			Ground[][] floorTiles = this.tiles [z];
			for (int x = 0; x < 104; ++x) {
				for (int y = 0; y < 104; ++y) {
					Ground singleTile = floorTiles [x] [y];
					if (singleTile != null) {
						singleTile.aBoolean1322 = true;
						singleTile.aBoolean1323 = true;
						singleTile.aBoolean1324 = singleTile.objectCount > 0;
						anInt446++;
					}
				}
			}
		}
		for (int zLoop = 0; zLoop < 4; zLoop++) {
			Ground[][] floorTiles = this.tiles [zLoop];
			for (int xLoop = 0; xLoop < 104; ++xLoop) {
				int x = xLoop;
				for (int yLoop = 0; yLoop < 104; ++yLoop) {
					int y = yLoop;
					
					Ground class30_sub3_1 = floorTiles [x] [y];
					if (class30_sub3_1 != null)// && class30_sub3_1.aBoolean1322)
						renderTile (class30_sub3_1, regionContainer);
					
					Ground class30_sub3_5 = floorTiles [x] [y];
					if (class30_sub3_5 != null)// && class30_sub3_5.aBoolean1322)
						renderTile (class30_sub3_5, regionContainer);
				}
			}
		}
		
		aBoolean467 = false;
	}
	//
	public GameObject regionContainer;

	public Region (GameObject container)
	{
		this.regionContainer = container;
		anIntArray486 = new int[10000];
		anIntArray487 = new int[10000];
		planeCount = 4;
		sceneWidth = 104;
		height = 104;
		tiles = NetDrawingTools.CreateTrippleGroundArray (4, 104, 104);//new Ground[z][x][y];
		anIntArrayArrayArray445 = NetDrawingTools.CreateTrippleIntArray (4, 104 + 1, 104 + 1);//new int[z][x + 1][y + 1];
		anIntArrayArrayArray440 = NetDrawingTools.CreateTrippleIntArray (4, 105, 105);
		byte[][][] abyte0 = NetDrawingTools.CreateTrippleByteArray (4, 104, 104);
		for (int z = 0; z < 4; z++) {
			for (int x = 0; x < 104; x++) {
				for (int y = 0; y < 104; y++) {
					abyte0 [z] [x] [y] = 0;
				}
			}
		}
		int length = 104;
		int width = 104;
		plane458 = 99;
		this.width = width;
		this.length = length;
		collisionPlaneModifiers = abyte0;
		tile_layer0_type = NetDrawingTools.CreateTrippleByteArray (4, width, length);//new byte[4][width][length];
		tile_layer1_type = NetDrawingTools.CreateTrippleSByteArray (4, width, length);//new byte[4][width][length];
		tile_layer1_shape = NetDrawingTools.CreateTrippleByteArray (4, width, length);//new byte[4][width][length];
		tile_layer1_orientation = NetDrawingTools.CreateTrippleByteArray (4, width, length);//new byte[4][width][length];
		tile_layer0_colour = NetDrawingTools.CreateTrippleIntArray (4, width, length);
		anIntArrayArrayArray135 = NetDrawingTools.CreateTrippleIntArray (4, width + 1, length + 1);//new int[4][width + 1][length + 1];
		shading = NetDrawingTools.CreateTrippleByteArray (4, width + 1, length + 1);//new byte[4][width + 1][length + 1];
		anIntArrayArray139 = NetDrawingTools.CreateDoubleIntArray (width + 1, length + 1);//new int[width + 1][length + 1];
		hues = new int[length];
		saturations = new int[length];
		luminances = new int[length];
		chromas = new int[length];
		anIntArray128 = new int[length];
		objectRoot = new GameObject ("Objects");
		animatedObjectRoot = new GameObject ("Animated Objects");
		objectRoot.transform.parent = container.transform;
		animatedObjectRoot.transform.parent = container.transform;
	}
	//
	public void LoadRegion (int xCoord, int yCoord)
	{
		anInt448++;
		myRegionX = xCoord; //50
		myRegionY = yCoord; //50
		regionbasesX = xCoord * 64; //3200
		regionbasesY = yCoord * 64; //3200
		xCoord = xCoord * 8; //800
		yCoord = yCoord * 8; //800
		int regionCount = 1;//0;
		//		for (int x = (regionbasesX - 6) / 8; x <= (regionbasesX + 6) / 8; x++) {
		//			for (int y = (regionbasesY - 6) / 8; y <= (regionbasesY + 6) / 8; y++) {
		//				regionCount++;
		//			}
		//		}
		localTileH = MapManager.getTileHeight (myRegionX, myRegionY);//ai;
		if (localTileH == null)
			return;
		localRegionMapData = new byte[regionCount][];
		localRegionLandscapeData = new sbyte[regionCount][];
		localRegionIds = new int[regionCount];
		terrainIndices = new int[regionCount];
		objectLandscapeIDS = new int[regionCount];
		
		regionCount = 0;
		
		localRegionIds [0] = ((xCoord) / 8 << 8) + (yCoord) / 8;
		terrainIndices [0] = MapManager.getMapData (0, (yCoord) / 8, (xCoord) / 8);
		objectLandscapeIDS [0] = MapManager.getMapData (1, (yCoord) / 8, (xCoord) / 8);
		localRegionMapData [0] = MapManager.getFile (5, terrainIndices [0]);//client.decompressors[4].decompressgzip(terrainIndices[0]);
		
		sbyte[] objectMap = MapManager.getMapFile (myRegionX << 8 | myRegionY);//objectLandscapeIDS [0]);
//		string ss = "";
//		for(int i = 0; i < objectMap.Length; ++i) ss += objectMap[i] +", ";
//		Debug.Log ("Loaded: " + ss);
//		int[] xteas = MapManager.GetXTEAS (myRegionX * 256 + myRegionY);
//		if (xteas != null && (0 != xteas [0] || xteas [1] != 0 || xteas [2] != 0 || 0 != xteas [3])) {
//			objectMap = XTEADecrypter.decryptXTEA (xteas, objectMap, 5, objectMap.Length);
//		}
//		byte[] objectMapBytes = new byte[objectMap.Length];
//		Buffer.BlockCopy(objectMap,0,objectMapBytes,0,objectMap.Length);
//		ByteBuffer deciphered = Container.decode(new ByteBuffer(new MemoryStream(objectMapBytes))).getData();
//		
//		objectMapBytes = deciphered.GetBuffer();
//		Buffer.BlockCopy(objectMapBytes,0,objectMap,0,objectMapBytes.Length);
//		ss = "";
//		for(int i = 0; i < objectMap.Length; ++i) ss += objectMap[i] +", ";
//		Debug.Log ("Decrypted: " + ss);
		localRegionLandscapeData [0] = objectMap;
		
		if (MapManager.getFile (5, terrainIndices [0]) == null)//client.decompressors[4].decompressgzip(terrainIndices[0]) == null)
			terrainIndices [0] = -1;
		
		for (int j = 0; j < 4; j++) {
			collisionMaps [j] = new CollisionMap (104, 104);
		}
		for (int plane = 0; plane < 4; plane++) {
			collisionMaps [plane].method210 ();
		}
		aByteArrayArrayArray1258 = NetDrawingTools.CreateTrippleByteArray (4, 104, 104);// new byte[4][104][104];
		for (int z = 0; z < 4; z++) {
			for (int x = 0; x < 104; x++) {
				for (int y = 0; y < 104; y++) {
					aByteArrayArrayArray1258 [z] [x] [y] = 0;
				}
			}
		}
		
		int count = localRegionMapData.Length;
		
		for (int id = 0; id < count; id++) {
			int dX = (localRegionIds [id] >> 8) * 64 - regionbasesX;
			int dY = (localRegionIds [id] & 0xff) * 64 - regionbasesY;
			byte[] terrainData = localRegionMapData [id];
			if (terrainData != null) {
				decodeRegionMapData (terrainData, dY, dX, (myRegionX - 6) * 8, (myRegionY - 6) * 8, collisionMaps);
			}
		}
		for (int i = 0; i < count; i++) {
			if (localRegionLandscapeData [i] != null && localRegionLandscapeData.Length > 0) {
				decodeRegionLandscapes (collisionMaps, localRegionLandscapeData [i]);
			}
		}
		method171 (collisionMaps);
	}
	
	private int method170 (int i, int j)
	{
		int k = i + j * 57;
		k = k << 13 ^ k;
		int l = k * (k * k * 15731 + 0xc0ae5) + 0x5208dd0d & 0x7fffffff;
		return l >> 19 & 0xff;
	}
	
	private int method184 (int i, int j, int k, int l)
	{
		int i1 = 0x10000 - RS2Sharp.Texture.COSINE [k * 1024 / l] >> 1;
		return (i * (0x10000 - i1) >> 16) + (j * i1 >> 16);
	}
	
	private int method186 (int i, int j)
	{
		int k = method170 (i - 1, j - 1) + method170 (i + 1, j - 1) + method170 (i - 1, j + 1) + method170 (i + 1, j + 1);
		int l = method170 (i - 1, j) + method170 (i + 1, j) + method170 (i, j - 1) + method170 (i, j + 1);
		int i1 = method170 (i, j);
		return k / 16 + l / 8 + i1 / 4;
	}
	
	private int method187 (int i, int j)
	{
		if (i == -1) {
			return 0xbc614e;
		}
		j = j * (i & 0x7f) / 128;
		if (j < 2) {
			j = 2;
		} else if (j > 126) {
			j = 126;
		}
		return (i & 0xff80) + j;
	}
	
	public void decodeRegionLandscapes (CollisionMap[] maps, sbyte[] sdata)
	{
//		ByteInputStream str1 = new ByteInputStream(sdata);
//			
//		int objectId = -1;
//		int incr;
//		while ((incr = str1.readSmart2()) != 0) {
//			objectId += incr;
//			int location = 0;
//			int incr2;
//			while ((incr2 = str1.readSmart()) != 0) {
//				location += incr2 - 1;
//				int localX = location >> 6 & 0x3f;
//				int localY = location & 0x3f;
//				int height = location >> 12;
//				int objectData = str1.readUByte();
//				int type = objectData >> 2;
//				int rotation = objectData & 0x3;
//				Debug.Log (localX + " " + localY + " " + height);
////				if (localX < 0 || localX >= 64 || localY < 0
////				    || localY >= 64) {
////					continue;
////				}
////				if ((collisionPlaneModifiers[1][localX][localY] & 2) == 2) {
////					height--;
////				}
//				if (height >= 0 && height <= 3) {
//					CollisionMap map = null;
//					method175(map, objectId, localX, localY, height, type, rotation);
//				}
//			}
//		}

//		Packet buffer = new Packet (sdata);
//		int id = -1;
//					
//		do {
//			int idOffset = buffer.gsmarts ();
//			if (idOffset == 0) {
//				goto breakdecoding;
//			}
//			id += idOffset;
//			int position = 0;
//				
//			do {
//				int offset = buffer.gsmarts ();
//				if (offset == 0) {
//					break;
//				}
//				position += offset - 1;
//				int yOffset = position & 63;
//				int xOffset = position >> 6 & 63;
//				int z = position >> 12;
//				int config = buffer.readUnsignedByte ();
//				int type = config >> 2;
//				int orientation = config & 3;
//				int x = xOffset;
//				int y = yOffset;
//				Debug.Log (x + " " + y);
//				if (x > 0 && y > 0 && x < 103 && y < 103) {
//					int plane = z;
//					if ((collisionPlaneModifiers [1] [x] [y] & 2) == 2) {
//						plane = z - 1;
//						//plane--;
//					}
//					if (z > 3)
//						z = 3;
//					if (z < 0)
//						z = 0;
//					//CollisionMap map = (plane >= 0) ? maps[plane] : null;
//					CollisionMap map = null;
////						if(plane >= 0) map = maps[plane];
//						
//					method175 (map, id, x, y, z, type, orientation);
//				}
//			} while (true);
//		} while (true);
//		breakdecoding:
//		int dummy = 0;

		Packet objectDataBuffer = new Packet(sdata);
		int id = -1;
		
		while(true) {
			int idOffset = objectDataBuffer.gsmarts();
			if(idOffset == 0) {
				return;
			}
			
			id += idOffset;
			int position = 0;
			
			while(true) {
				int var12 = objectDataBuffer.gsmarts();
				if(var12 == 0) {
					break;
				}

				position += var12 - 1;
				int yOffset = position & 63;
				int xOffset = position >> 6 & 63;
				int height = position >> 12;
				int config = objectDataBuffer.readUnsignedByte();
				int type = config >> 2;
				int orientation = config & 3;
				int x = xOffset;
				int y = yOffset;
				
				if (x >= 0 && x < 64 && y >= 0 && y < 64) {
					if ((collisionPlaneModifiers[1][x][y] & 2) == 2) {
						height--;
					}
					if (height >= 0 && height <= 3) {
						AddObject (id, x, y, height, type, orientation);
					}
				}
			}
		}
	}
	
	public void decodeRegionMapData (byte[] data, int dY, int dX, int regionX, int regionY, CollisionMap[] maps)
	{
		for (int z = 0; z < 4; z++) {
			for (int localX = 0; localX < 64; localX++) {
				for (int localY = 0; localY < 64; localY++) {
					if (dX + localX > 0 && dX + localX < 103 && dY + localY > 0 && dY + localY < 103) {
						maps [z].anIntArrayArray294 [dX + localX] [dY + localY] &= unchecked((uint)0xfeffffff);
					}
				}
			}
		}
		
		Packet buffer = new Packet (data);
		for (int z = 0; z < 4; z++) {
			for (int localX = 0; localX < 64; localX++) {
				for (int localY = 0; localY < 64; localY++) {
					decodeMapData (buffer, localX + dX, localY + dY, z, regionX, regionY, 0);
				}
			}
		}
	}
	
	/**
	 * Returns the plane that actually contains the collision flag, to adjust for objects such as bridges. TODO better
	 * name
	 *
	 * @param x The x coordinate.
	 * @param y The y coordinate.
	 * @param z The z coordinate.
	 * @return The correct z coordinate.
	 */
	public int getCollisionPlane (int x, int y, int z)
	{
		if ((collisionPlaneModifiers [z] [x] [y] & FORCE_GROUND) != 0) {
			return 0;
		} else if (z > 0 && (collisionPlaneModifiers [1] [x] [y] & BRIDGE_TILE) != 0) {
			return z - 1;
		}
		
		return z;
	}
	
	public void method171 (CollisionMap[] maps)
	{
		for (int z = 0; z < 4; z++) {
			for (int x = 0; x < 104; x++) {
				for (int y = 0; y < 104; y++) {
					if ((collisionPlaneModifiers [z] [x] [y] & BLOCKED_TILE) == 1) {
						int plane = z;
						if ((collisionPlaneModifiers [1] [x] [y] & BRIDGE_TILE) == 2) {
							plane--;
						}
						
						if (plane >= 0) {
							maps [plane].method213 (x, y);
						}
					}
				}
			}
		}
		
		for (int z = 0; z < 4; z++) {
			byte[][] shading = this.shading [z];
			byte byte0 = 96;
			char c = '\u0300';
			sbyte byte1 = -50;
			sbyte byte2 = -10;
			sbyte byte3 = -50;
			
			int l3 = c * (int)Mathf.Sqrt (byte1 * byte1 + byte2 * byte2 + byte3 * byte3) >> 8;
			for (int y = 1; y < length - 1; y++) {
				for (int x = 1; x < width - 1; x++) {
					int width_dh = localTileH [z] [x + 1] [y] - localTileH [z] [x - 1] [y];
					int length_dh = localTileH [z] [x] [y + 1] - localTileH [z] [x] [y - 1];
					
					int r = (int)Mathf.Sqrt (width_dh * width_dh + 0x10000 + length_dh * length_dh);
					int k12 = (width_dh << 8) / r;
					int l13 = 0x10000 / r;
					int j15 = (length_dh << 8) / r;
					int j16 = byte0 + (byte1 * k12 + byte2 * l13 + byte3 * j15) / l3;
					int j17 = (shading [x - 1] [y] >> 2) + (shading [x + 1] [y] >> 3) + (shading [x] [y - 1] >> 2)
						+ (shading [x] [y + 1] >> 3) + (shading [x] [y] >> 1);
					anIntArrayArray139 [x] [y] = j16 - j17;
				}
			}
			
			for (int index = 0; index < length; index++) {
				hues [index] = 0;
				saturations [index] = 0;
				luminances [index] = 0;
				chromas [index] = 0;
				anIntArray128 [index] = 0;
			}
			
			for (int centreX = -5; centreX < width + 5; centreX++) {
				if (centreX >= 0 && centreX < width - 1) {
					for (int centreY = -5; centreY < length + 5; centreY++) {
						if (centreY >= 0
							&& centreY < length - 1
							&& (!lowMemory || (collisionPlaneModifiers [0] [centreX] [centreY] & 2) != 0 || (collisionPlaneModifiers [z] [centreX] [centreY] & 0x10) == 0
							&& getCollisionPlane (centreX, centreY, z) == anInt131)) {
							if (z < plane458) {
								plane458 = z;
							}
							
							int underlay = tile_layer0_type [z] [centreX] [centreY] & 0xff;
							int overlay = tile_layer1_type [z] [centreX] [centreY] & 0xff;
							
							if (underlay > 0 || overlay > 0) {
								int centreZ = localTileH [z] [centreX] [centreY];
								int eastZ = localTileH [z] [centreX + 1] [centreY];
								int northEastZ = localTileH [z] [centreX + 1] [centreY + 1];
								int northZ = localTileH [z] [centreX] [centreY + 1];
								int j20 = anIntArrayArray139 [centreX] [centreY];
								int k20 = anIntArrayArray139 [centreX + 1] [centreY];
								int l20 = anIntArrayArray139 [centreX + 1] [centreY + 1];
								int i21 = anIntArrayArray139 [centreX] [centreY + 1];
								int colour = 0;
								
								if (underlay > 0 && underlay < Flo.cache.Length) {
									Flo floor = Flo.cache [underlay - 1];
									colour = floor.rgb;
								}
								
								if (z > 0) {
									bool flag = true;
									if (underlay == 0 && tile_layer1_shape [z] [centreX] [centreY] != 0) {
										flag = false;
									}
									
									if (overlay > 0 && !OverLayFlo317.overLayFlo317s [overlay - 1].aBoolean393) {
										flag = false;
									}
									
									if (flag && centreZ == eastZ && centreZ == northEastZ && centreZ == northZ) {
										anIntArrayArrayArray135 [z] [centreX] [centreY] |= 0x924;
									}
								}
								
								tile_layer0_colour [z] [centreX] [centreY] = colour;
								if (overlay == 0) {
									method279 (z, centreX, centreY, 0, 0, -1, centreZ, eastZ, northEastZ, northZ,
									          method187 (colour, j20), method187 (colour, k20), method187 (colour, l20),
									          method187 (colour, i21), 0, 0, 0, 0, colour, 0);
								} else {
									int tileType = tile_layer1_shape [z] [centreX] [centreY] + 1;
									byte orientation = tile_layer1_orientation [z] [centreX] [centreY];
									//Floor floor = Floor.floors[overlay - 1];
									if (overlay >= OverLayFlo317.overLayFlo317s.Length || overlay <= 0)
										overlay = 1;
									OverLayFlo317 overlay_flo = OverLayFlo317.overLayFlo317s [overlay - 1];
									int texture = overlay_flo.textureId;
									int floorColour;
									int k23;
									
									if (texture >= 0) {
										k23 = RS2Sharp.Texture.method369 (texture);
										floorColour = -1;
									} else if (overlay_flo.rgb == 0xff00ff) {
										k23 = 0;
										floorColour = -2;
										texture = -1;
									} else {
										floorColour = encode (overlay_flo.anInt394, overlay_flo.anInt395, overlay_flo.anInt396);
										k23 = RS2Sharp.Texture.hsl2rgb [method185 (overlay_flo.anInt399, 96)];
									}
									
									method279 (z, centreX, centreY, tileType, orientation, texture, centreZ, eastZ,
									          northEastZ, northZ, method187 (colour, j20), method187 (colour, k20),
									          method187 (colour, l20), method187 (colour, i21), method185 (floorColour, j20),
									          method185 (floorColour, k20), method185 (floorColour, l20),
									          method185 (floorColour, i21), colour, k23);
								}
							}
						}
					}
				}
			}
			
			for (int y = 1; y < length - 1; y++) {
				for (int x = 1; x < width - 1; x++) {
					setCollisionPlane (x, y, z, getCollisionPlane (x, y, z));
				}
			}
		}
		
		method305 (-10, 64, -50, 768, -50);
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < length; y++) {
				if ((collisionPlaneModifiers [1] [x] [y] & 2) == 2) {
					method276 (x, y);
				}
			}
		}
		
		int flagg = 1;
		int j2 = 2;
		int k2 = 4;
		for (int plane = 0; plane < 4; plane++) {
			if (plane > 0) {
				flagg <<= 3;
				j2 <<= 3;
				k2 <<= 3;
			}
			
			for (int z = 0; z <= plane; z++) {
				for (int y = 0; y <= length; y++) {
					for (int x = 0; x <= width; x++) {
						if ((anIntArrayArrayArray135 [z] [x] [y] & flagg) != 0) {
							int currentY = y;
							int l5 = y;
							int i7 = z;
							int k8 = z;
							
							for (; currentY > 0 && (anIntArrayArrayArray135[z][x][currentY - 1] & flagg) != 0; currentY--) {
								;
							}
							
							for (; l5 < length && (anIntArrayArrayArray135[z][x][l5 + 1] & flagg) != 0; l5++) {
								;
							}
							
							for (; i7 > 0; i7--) {
								for (int j10 = currentY; j10 <= l5; j10++) {
									if ((anIntArrayArrayArray135 [i7 - 1] [x] [j10] & flagg) == 0) {
										goto breaklabel0;
									}
								}
							}
							breaklabel0:
							label1:
							for (; k8 < plane; k8++) {
								for (int k10 = currentY; k10 <= l5; k10++) {
									if ((anIntArrayArrayArray135 [k8 + 1] [x] [k10] & flagg) == 0) {
										goto breaklabel1;
									}
								}
							}
							breaklabel1:
							int l10 = (k8 + 1 - i7) * (l5 - currentY + 1);
							if (l10 >= 8) {
								char c1 = (char)360;
								int k14 = localTileH [k8] [x] [currentY] - c1;
								int l15 = localTileH [i7] [x] [currentY];
								method277 (plane, x * 128, l15, x * 128, l5 * 128 + 128, k14, currentY * 128, 1);
								for (int l16 = i7; l16 <= k8; l16++) {
									for (int l17 = currentY; l17 <= l5; l17++) {
										anIntArrayArrayArray135 [l16] [x] [l17] &= ~flagg;
									}
								}
							}
						}
						
						if ((anIntArrayArrayArray135 [z] [x] [y] & j2) != 0) {
							int l4 = x;
							int i6 = x;
							int j7 = z;
							int l8 = z;
							for (; l4 > 0 && (anIntArrayArrayArray135[z][l4 - 1][y] & j2) != 0; l4--) {
								
							}
							for (; i6 < width && (anIntArrayArrayArray135[z][i6 + 1][y] & j2) != 0; i6++) {
								
							}
							label2:
							for (; j7 > 0; j7--) {
								for (int i11 = l4; i11 <= i6; i11++) {
									if ((anIntArrayArrayArray135 [j7 - 1] [i11] [y] & j2) == 0) {
										goto breaklabel2;
									}
								}
							}
							breaklabel2:
							label3:
							for (; l8 < plane; l8++) {
								for (int j11 = l4; j11 <= i6; j11++) {
									if ((anIntArrayArrayArray135 [l8 + 1] [j11] [y] & j2) == 0) {
										goto breaklabel3;
									}
								}
							}
							breaklabel3:
							int k11 = (l8 + 1 - j7) * (i6 - l4 + 1);
							if (k11 >= 8) {
								char c2 = (char)360;
								int l14 = localTileH [l8] [l4] [y] - c2;
								int i16 = localTileH [j7] [l4] [y];
								method277 (plane, l4 * 128, i16, i6 * 128 + 128, y * 128, l14, y * 128, 2);
								for (int i17 = j7; i17 <= l8; i17++) {
									for (int i18 = l4; i18 <= i6; i18++) {
										anIntArrayArrayArray135 [i17] [i18] [y] &= ~j2;
									}
								}
							}
						}
						
						if ((anIntArrayArrayArray135 [z] [x] [y] & k2) != 0) {
							int i5 = x;
							int j6 = x;
							int k7 = y;
							int i9 = y;
							for (; k7 > 0 && (anIntArrayArrayArray135[z][x][k7 - 1] & k2) != 0; k7--) {
								
							}
							for (; i9 < length && (anIntArrayArrayArray135[z][x][i9 + 1] & k2) != 0; i9++) {
								
							}
							label4:
							for (; i5 > 0; i5--) {
								for (int l11 = k7; l11 <= i9; l11++) {
									if ((anIntArrayArrayArray135 [z] [i5 - 1] [l11] & k2) == 0) {
										goto breaklabel4;
									}
								}
							}
							breaklabel4:
							label5:
							for (; j6 < width; j6++) {
								for (int i12 = k7; i12 <= i9; i12++) {
									if ((anIntArrayArrayArray135 [z] [j6 + 1] [i12] & k2) == 0) {
										goto breaklabel5;
									}
								}
							}
							breaklabel5:
							if ((j6 - i5 + 1) * (i9 - k7 + 1) >= 4) {
								int j12 = localTileH [z] [i5] [k7];
								method277 (plane, i5 * 128, j12, j6 * 128 + 128, i9 * 128 + 128, j12, k7 * 128, 4);
								for (int k13 = i5; k13 <= j6; k13++) {
									for (int i15 = k7; i15 <= i9; i15++) {
										anIntArrayArrayArray135 [z] [k13] [i15] &= ~k2;
									}
								}
							}
						}
					}
				}
			}
		}
	}
	
	private void decodeMapData (Packet buffer, int x, int y, int z, int regionX, int regionY, int rotation)
	{
		if (x >= 0 && x < 104 && y >= 0 && y < 104) {
			collisionPlaneModifiers [z] [x] [y] = 0;
			do {
				int type = buffer.readUnsignedByte ();
				
				if (type == 0) {
					if (z == 0) {
						//localTileH[0][x][y] = -calculateHeight(0xe3b7b + x + regionX, 0x87cce + y + regionY) * 8;
					} else {
						//localTileH[z][x][y] = localTileH[z - 1][x][y] - 240;
					}
					
					return;
				} else if (type == 1) {
					int height = buffer.readUnsignedByte ();
					if (height == 1) {
						height = 0;
					}
					
					if (z == 0) {
						//localTileH[0][x][y] = -height * 8;
					} else {
						//localTileH[z][x][y] = localTileH[z - 1][x][y] - height * 8;
					}
					
					return;
				} else if (type <= 49) {
					tile_layer1_type [z] [x] [y] = buffer.readSignedByte ();
					tile_layer1_shape [z] [x] [y] = (byte)((type - 2) / 4);
					tile_layer1_orientation [z] [x] [y] = (byte)(type - 2 + rotation & 3);
				} else if (type <= 81) {
					collisionPlaneModifiers [z] [x] [y] = (byte)(type - 49);
				} else {
					tile_layer0_type [z] [x] [y] = (byte)(type - 81);
				}
			} while (true);
		}
		
		do {
			int inn = buffer.readUnsignedByte ();
			if (inn == 0) {
				break;
			} else if (inn == 1) {
				buffer.readUnsignedByte ();
				return;
			} else if (inn <= 49) {
				buffer.readUnsignedByte ();
			}
		} while (true);
	}
	
	/**
	 * Encodes the hue, saturation, and luminance into a colour value.
	 * 
	 * @param hue The hue.
	 * @param saturation The saturation.
	 * @param luminance The luminance.
	 * @return The colour.
	 */
	private int encode (int hue, int saturation, int luminance)
	{
		if (luminance > 179) {
			saturation /= 2;
		}
		if (luminance > 192) {
			saturation /= 2;
		}
		if (luminance > 217) {
			saturation /= 2;
		}
		if (luminance > 243) {
			saturation /= 2;
		}
		
		return (hue / 4 << 10) + (saturation / 32 << 7) + luminance / 2;
	}
	
	private void AddObject (int id, int x, int y, int z, int type, int orientation)
	{
		plane458 = Mathf.Min (z, plane458);
		
		int mean = 0;
		int centre = localTileH [z] [x] [y];
		int east = localTileH [z] [x + 1] [y];
		int northEast = localTileH [z] [x + 1] [y + 1];
		int north = localTileH [z] [x] [y + 1];
		
		if ((collisionPlaneModifiers [z] [x] [y] & FORCE_GROUND) != 0) {
			mean = northEast;
		} else {
			mean = centre + east + northEast + north >> 2;
		}

		ObjectDef definition = ObjectDef.forID (id);
		int key = x | (y << 7) | (id << 14) | 0x40000000;
		
		if (!definition.hasActions) {
			key |= unchecked((int)0x80000000); // msb
		}
		
		byte config = (byte)((orientation << 6) | type);
		if (type == 22) {
			if (lowMemory && !definition.hasActions && !definition.aBoolean736) {
				return;
			}
			
			Animable objectt;
			int spawnType = 0;
			if (definition.animId == -1 && definition.childrenIDs == null) {
				spawnType = 0;
				objectt = definition.method578 (22, orientation, centre, east, northEast, north, -1);
			} else {
				spawnType = 1;
				objectt = new Animable_Sub5 (id, orientation, 22, centre, east, northEast, north, definition.animId,
				                            true);
			}
			
			addFloorDecoration (x, y, z, objectt, key, config, mean, spawnType, 0, orientation, centre, east, northEast, north, id, definition.animId, type);
//			if (definition.aBoolean767 && definition.hasActions && map != null) {
//				map.method213 (x, y);
//			}
			return;
		}
		
		if (type == 10 || type == 11) {
			Animable objectt;
			int spawnType = 0;
			if (definition.animId == -1 && definition.childrenIDs == null) {
				objectt = definition.method578 (10, orientation, centre, east, northEast, north, -1);
			} else {
				spawnType = 1;
				objectt = new Animable_Sub5 (id, orientation, 10, centre, east, northEast, north, definition.animId, true);
			}
			if (objectt != null) {
				int yaw = 0;
				if (type == 11) {
					yaw += 256;
				}
				int width;
				int length;
				
				if (orientation == 1 || orientation == 3) {
					width = definition.anInt761;
					length = definition.anInt744;
				} else {
					width = definition.anInt744;
					length = definition.anInt761;
				}
				if (addObject (x, y, z, width, length, objectt, key, config, yaw, mean, spawnType, 10, centre, east, northEast, north, id, definition.animId, type, orientation) && definition.aBoolean779) {
					Model model;
					if (objectt is Model) {
						model = (Model)objectt;
					} else {
						model = definition.method578 (10, orientation, centre, east, northEast, north, -1);
					}
					if (model != null) {
						for (int dx = 0; dx <= width; dx++) {
							for (int dy = 0; dy <= length; dy++) {
								int l5 = Mathf.Max (30, model.diagonal2DAboveorigin / 4);
								
								if (l5 > shading [z] [x + dx] [y + dy]) {
									shading [z] [x + dx] [y + dy] = (byte)l5;
								}
							}
						}
					}
				}
			}
//			if (definition.aBoolean767 && map != null) {
//				map.flagObject (x, y, definition.anInt744, definition.anInt761, definition.aBoolean757);
//			}
			return;
		}
		if (type >= 12) {
			Animable objectt;
			int spawnType = 0;
			if (definition.animId == -1 && definition.childrenIDs == null) {
				objectt = definition.method578 (type, orientation, centre, east, northEast, north, -1);
			} else {
				spawnType = 1;
				objectt = new Animable_Sub5 (id, orientation, type, centre, east, northEast, north, definition.animId,
				                            true);
			}
			addObject (x, y, z, 1, 1, objectt, key, config, 0, mean, spawnType, type, centre, east, northEast, north, id, definition.animId, type, orientation);
			if (type >= 12 && type <= 17 && type != 13 && z > 0) {
				anIntArrayArrayArray135 [z] [x] [y] |= 0x924;
			}
			
//			if (definition.aBoolean767 && map != null) {
//				map.flagObject (x, y, definition.anInt744, definition.anInt761, definition.aBoolean757);
//			}
			return;
		}
		if (type == 0) {
			Animable objectt;
			int spawnType = 0;
			if (definition.animId == -1 && definition.childrenIDs == null) {
				spawnType = 0;
				objectt = definition.method578 (0, orientation, centre, east, northEast, north, -1);
			} else {
				spawnType = 1;
				objectt = new Animable_Sub5 (id, orientation, 0, centre, east, northEast, north, definition.animId, true);
			}
			addWall (key, x, y, z, anIntArray152 [orientation], objectt, config, null, mean, 0, spawnType, 0, orientation, centre, east, northEast, north, id, definition.animId, type);
			if (orientation == 0) {
				if (definition.aBoolean779) {
					shading [z] [x] [y] = 50;
					shading [z] [x] [y + 1] = 50;
				}
				if (definition.aBoolean764) {
					anIntArrayArrayArray135 [z] [x] [y] |= 0x249;
				}
			} else if (orientation == 1) {
				if (definition.aBoolean779) {
					shading [z] [x] [y + 1] = 50;
					shading [z] [x + 1] [y + 1] = 50;
				}
				if (definition.aBoolean764) {
					anIntArrayArrayArray135 [z] [x] [y + 1] |= 0x492;
				}
			} else if (orientation == 2) {
				if (definition.aBoolean779) {
					shading [z] [x + 1] [y] = 50;
					shading [z] [x + 1] [y + 1] = 50;
				}
				if (definition.aBoolean764) {
					anIntArrayArrayArray135 [z] [x + 1] [y] |= 0x249;
				}
			} else if (orientation == 3) {
				if (definition.aBoolean779) {
					shading [z] [x] [y] = 50;
					shading [z] [x + 1] [y] = 50;
				}
				if (definition.aBoolean764) {
					anIntArrayArrayArray135 [z] [x] [y] |= 0x492;
				}
			}
//			if (definition.aBoolean767 && map != null) {
//				map.flagObject (x, y, orientation, type, definition.aBoolean757);
//			}
			if (definition.anInt775 != 16) {
				displaceWallDecor (x, y, z, definition.anInt775);
			}
			return;
		}
		if (type == 1) {
			Animable objectt;
			int spawnType = 0;
			if (definition.animId == -1 && definition.childrenIDs == null) {
				spawnType = 0;
				objectt = definition.method578 (1, orientation, centre, east, northEast, north, -1);
			} else {
				spawnType = 1;
				objectt = new Animable_Sub5 (id, orientation, 1, centre, east, northEast, north, definition.animId, true);
			}
			
			addWall (key, x, y, z, anIntArray140 [orientation], objectt, config, null, mean, 0, spawnType, 1, orientation, centre, east, northEast, north, id, definition.animId, type);
			if (definition.aBoolean779) {
				if (orientation == 0) {
					shading [z] [x] [y + 1] = 50;
				} else if (orientation == 1) {
					shading [z] [x + 1] [y + 1] = 50;
				} else if (orientation == 2) {
					shading [z] [x + 1] [y] = 50;
				} else if (orientation == 3) {
					shading [z] [x] [y] = 50;
				}
			}
//			if (definition.aBoolean767 && map != null) {
//				map.flagObject (x, y, orientation, type, definition.aBoolean757);
//			}
			return;
		}
		if (type == 2) {
			int oppositeOrientation = orientation + 1 & 3;
			Animable obj11;
			Animable obj12;
			int spawnType = 0;
			if (definition.animId == -1 && definition.childrenIDs == null) {
				spawnType = 0;
				obj11 = definition.method578 (2, 4 + orientation, centre, east, northEast, north, -1);
				obj12 = definition.method578 (2, oppositeOrientation, centre, east, northEast, north, -1);
			} else {
				spawnType = 1;
				obj11 = new Animable_Sub5 (id, 4 + orientation, 2, centre, east, northEast, north, definition.animId,
				                          true);
				obj12 = new Animable_Sub5 (id, oppositeOrientation, 2, centre, east, northEast, north,
				                          definition.animId, true);
			}
			addWall (key, x, y, z, anIntArray152 [orientation], obj11, config, obj12, mean, anIntArray152 [oppositeOrientation], spawnType, 2, 4 + orientation, centre, east, northEast, north, id, definition.animId, type);
			if (definition.aBoolean764) {
				if (orientation == 0) {
					anIntArrayArrayArray135 [z] [x] [y] |= 0x249;
					anIntArrayArrayArray135 [z] [x] [y + 1] |= 0x492;
				} else if (orientation == 1) {
					anIntArrayArrayArray135 [z] [x] [y + 1] |= 0x492;
					anIntArrayArrayArray135 [z] [x + 1] [y] |= 0x249;
				} else if (orientation == 2) {
					anIntArrayArrayArray135 [z] [x + 1] [y] |= 0x249;
					anIntArrayArrayArray135 [z] [x] [y] |= 0x492;
				} else if (orientation == 3) {
					anIntArrayArrayArray135 [z] [x] [y] |= 0x492;
					anIntArrayArrayArray135 [z] [x] [y] |= 0x249;
				}
			}
//			if (definition.aBoolean767 && map != null) {
//				map.flagObject (x, y, orientation, type, definition.aBoolean757);
//			}
			if (definition.anInt775 != 16) {
				displaceWallDecor (x, y, z, definition.anInt775);
			}
			return;
		}
		if (type == 3) {
			Animable objectt;
			int spawnType = 0;
			if (definition.animId == -1 && definition.childrenIDs == null) {
				spawnType = 0;
				objectt = definition.method578 (3, orientation, centre, east, northEast, north, -1);
			} else {
				spawnType = 1;
				objectt = new Animable_Sub5 (id, orientation, 3, centre, east, northEast, north, definition.animId, true);
			}
			
			addWall (key, x, y, z, anIntArray140 [orientation], objectt, config, null, mean, 0, spawnType, 3, orientation, centre, east, northEast, north, id, definition.animId, type);
			if (definition.aBoolean779) {
				if (orientation == 0) {
					shading [z] [x] [y + 1] = 50;
				} else if (orientation == 1) {
					shading [z] [x + 1] [y + 1] = 50;
				} else if (orientation == 2) {
					shading [z] [x + 1] [y] = 50;
				} else if (orientation == 3) {
					shading [z] [x] [y] = 50;
				}
			}
//			if (definition.aBoolean767 && map != null) {
//				map.flagObject (x, y, orientation, type, definition.aBoolean757);
//			}
			return;
		}
		if (type == 9) {
			Animable objectt;
			int spawnType = 0;
			if (definition.animId == -1 && definition.childrenIDs == null) {
				objectt = definition.method578 (type, orientation, centre, east, northEast, north, -1);
			} else {
				spawnType = 1;
				objectt = new Animable_Sub5 (id, orientation, type, centre, east, northEast, north, definition.animId,
				                            true);
			}
			addObject (x, y, z, 1, 1, objectt, key, config, 0, mean, spawnType, type, centre, east, northEast, north, id, definition.animId, type, orientation);
//			if (definition.aBoolean767 && map != null) {
//				map.flagObject (x, y, definition.anInt744, definition.anInt761, definition.aBoolean757);
//			}
			return;
		}
		
		if (definition.aBoolean762) {
			if (orientation == 1) {
				int tmp = north;
				north = northEast;
				northEast = east;
				east = centre;
				centre = tmp;
			} else if (orientation == 2) {
				int tmp = north;
				north = east;
				east = tmp;
				tmp = northEast;
				northEast = centre;
				centre = tmp;
			} else if (orientation == 3) {
				int tmp = north;
				north = centre;
				centre = east;
				east = northEast;
				northEast = tmp;
			}
		}
		
		if (type == 4) {
			Animable objectt;
			int spawnType = 0;
			if (definition.animId == -1 && definition.childrenIDs == null) {
				objectt = definition.method578 (4, orientation, centre, east, northEast, north, -1);
			} else {
				spawnType = 1;
				objectt = new Animable_Sub5 (id, orientation, 4, centre, east, northEast, north, definition.animId, true);
			}
			addWallDecoration (key, y, orientation, z, 0, mean, objectt, x, config, 0, anIntArray152 [orientation], spawnType, 4, centre, east, northEast, north, id, definition.animId, type);
			return;
		}
		if (type == 5) {
			int displacement = 16;
			int existing = getWallKey (x, y, z);
			if (existing > 0) {
				displacement = ObjectDef.forID (existing >> 14 & 0x7fff).anInt775;
			}
			Animable objectt;
			int spawnType = 0;
			if (definition.animId == -1 && definition.childrenIDs == null) {
				objectt = definition.method578 (4, orientation, centre, east, northEast, north, -1);
			} else {
				spawnType = 1;
				objectt = new Animable_Sub5 (id, orientation, 4, centre, east, northEast, north, definition.animId, true);
			}
			addWallDecoration (key, y, orientation, z, COSINE_VERTICES [orientation] * displacement, mean, objectt, x,
			                  config, SINE_VERTICIES [orientation] * displacement, anIntArray152 [orientation], spawnType, 4, centre, east, northEast, north, id, definition.animId, type);
			return;
		}
		if (type == 6) {
			Animable objectt;
			int spawnType = 0;
			if (definition.animId == -1 && definition.childrenIDs == null) {
				objectt = definition.method578 (4, orientation, centre, east, northEast, north, -1);
			} else {
				spawnType = 1;
				objectt = new Animable_Sub5 (id, orientation, 4, centre, east, northEast, north, definition.animId, true);
			}
			addWallDecoration (key, y, orientation, z, 0, mean, objectt, x, config, 0, 256, spawnType, 4, centre, east, northEast, north, id, definition.animId, type);
			return;
		}
		if (type == 7) {
			Animable objectt;
			int spawnType = 0;
			if (definition.animId == -1 && definition.childrenIDs == null) {
				objectt = definition.method578 (4, orientation, centre, east, northEast, north, -1);
			} else {
				spawnType = 1;
				objectt = new Animable_Sub5 (id, orientation, 4, centre, east, northEast, north, definition.animId, true);
			}
			addWallDecoration (key, y, orientation, z, 0, mean, objectt, x, config, 0, 512, spawnType, 4, centre, east, northEast, north, id, definition.animId, type);
			return;
		}
		if (type == 8) {
			Animable objectt;
			int spawnType = 0;
			if (definition.animId == -1 && definition.childrenIDs == null) {
				objectt = definition.method578 (4, orientation, centre, east, northEast, north, -1);
			} else {
				spawnType = 1;
				objectt = new Animable_Sub5 (id, orientation, 4, centre, east, northEast, north, definition.animId, true);
			}
			addWallDecoration (key, y, orientation, z, 0, mean, objectt, x, config, 0, 768, spawnType, 4, centre, east, northEast, north, id, definition.animId, type);
		}
	}
	
	private int method185 (int colour, int j)
	{
		if (colour == -2) {
			return 0xbc614e;
		}
		
		if (colour == -1) {
			if (j < 0) {
				j = 0;
			} else if (j > 127) {
				j = 127;
			}
			return 127 - j;
		}
		
		j = j * (colour & 0x7f) / 128;
		if (j < 2) {
			j = 2;
		} else if (j > 126) {
			j = 126;
		}
		return (colour & 0xff80) + j;
	}

	public void dispose ()
	{
		interactables = null;
		clusterCounts = null;
		//clusters = null;
		//		aClass19_477 = null;
		aBooleanArrayArrayArrayArray491 = null;
		aBooleanArrayArray492 = null;
	}
	
	public void method277 (int plane, int j, int k, int l, int i1, int j1, int l1, int i2)
	{
		Class47 cluster = new Class47 ();
		cluster.anInt787 = j / 128;
		cluster.anInt788 = l / 128;
		cluster.anInt789 = l1 / 128;
		cluster.anInt790 = i1 / 128;
		cluster.anInt791 = i2;
		cluster.anInt792 = j;
		cluster.anInt793 = l;
		cluster.anInt794 = l1;
		cluster.anInt795 = i1;
		cluster.anInt796 = j1;
		cluster.anInt797 = k;
	}
	
	public void method310 (int i, int j, int k, int l, int[] ai)
	{
		anInt495 = 0;
		anInt496 = 0;
		anInt497 = k;
		anInt498 = l;
		anInt493 = k / 2;
		anInt494 = l / 2;
		bool[][][][] aflag = NetDrawingTools.CreateQuadBoolArray (9, 32, 53, 53);
		
		for (int i1 = 128; i1 <= 384; i1 += 32) {
			for (int j1 = 0; j1 < 2048; j1 += 64) {
				anInt458 = Model.SINE [i1];
				anInt459 = Model.COSINE [i1];
				anInt460 = Model.SINE [j1];
				anInt461 = Model.COSINE [j1];
				int l1 = (i1 - 128) / 32;
				int j2 = j1 / 64;
				for (int l2 = -26; l2 <= 26; l2++) {
					for (int j3 = -26; j3 <= 26; j3++) {
						int k3 = l2 * 128;
						int i4 = j3 * 128;
						bool flag2 = false;
						for (int k4 = -i; k4 <= j; k4 += 128) {
							if (!method311 (ai [l1] + k4, i4, k3)) {
								continue;
							}
							flag2 = true;
							break;
						}
						aflag [l1] [j2] [l2 + 25 + 1] [j3 + 25 + 1] = flag2;
					}
				}
			}
		}
		
		for (int k1 = 0; k1 < 8; k1++) {
			for (int i2 = 0; i2 < 32; i2++) {
				for (int k2 = -25; k2 < 25; k2++) {
					for (int i3 = -25; i3 < 25; i3++) {
						bool flag1 = false;
						label0:
						for (int l3 = -1; l3 <= 1; l3++) {
							for (int j4 = -1; j4 <= 1; j4++) {
								if (aflag [k1] [i2] [k2 + l3 + 25 + 1] [i3 + j4 + 25 + 1]) {
									flag1 = true;
								} else if (aflag [k1] [(i2 + 1) % 31] [k2 + l3 + 25 + 1] [i3 + j4 + 25 + 1]) {
									flag1 = true;
								} else if (aflag [k1 + 1] [i2] [k2 + l3 + 25 + 1] [i3 + j4 + 25 + 1]) {
									flag1 = true;
								} else {
									if (!aflag [k1 + 1] [(i2 + 1) % 31] [k2 + l3 + 25 + 1] [i3 + j4 + 25 + 1]) {
										continue;
									}
									flag1 = true;
								}
								goto breaklabel0;
							}
						}
						breaklabel0:
						aBooleanArrayArrayArrayArray491 [k1] [i2] [k2 + 25] [i3 + 25] = flag1;
					}
				}
			}
		}
	}
	
	public bool method311 (int i, int j, int k)
	{
		int l = j * anInt460 + k * anInt461 >> 16;
		int i1 = j * anInt461 - k * anInt460 >> 16;
		int j1 = i * anInt458 + i1 * anInt459 >> 16;
		int k1 = i * anInt459 - i1 * anInt458 >> 16;
		if (j1 < 50 || j1 > 3500) {
			return false;
		}
		int l1 = anInt493 + (l << 9) / j1;
		int i2 = anInt494 + (k1 << 9) / j1;
		return l1 >= anInt495 && l1 <= anInt497 && i2 >= anInt496 && i2 <= anInt498;
	}
	
	int anInt442;
	int anInt443;
	int anInt488;
	int[] anIntArray486;
	int[] anIntArray487;
	int[][][] anIntArrayArrayArray440;
	int[][][] anIntArrayArrayArray445;
	int height;
	int planeCount;
	Ground[][][] tiles;
	
	public void addFloorDecoration (int x, int y, int z, Animable renderable, int key, byte config, int meanY, int spawnType, int otherThing, int orientation, int centre, int east, int northEast, int north, int id, int anim, int type)
	{
		if (renderable == null) {
			return;
		}
		GroundDecoration decoration = new GroundDecoration ();
		decoration.setX (x * 128);
		decoration.setY (y * 128);
		decoration.setHeight (meanY);
		decoration.setKey (key);
		decoration.setConfig (config);
		decoration.setRenderable ((Animable)renderable);
		decoration.spawnType = spawnType;
		decoration.orientation = 1;//orientation;
		decoration.centre = centre;
		decoration.east = east;
		decoration.northEast = northEast;
		decoration.north = north;
		decoration.id = id;
		decoration.type = type;
		decoration.anim = anim;
		if (tiles [z] [x] [y] == null) {
			tiles [z] [x] [y] = new Ground (x, y, z);
		}
		
		tiles [z] [x] [y].groundDecoration = decoration;
		
		decoration.runeObj = new RuneObject ((x * 128), meanY, (y * 128), 3, decoration, UnityClient.baseX, UnityClient.baseY, id, objectRoot, animatedObjectRoot);
	}
	
	public bool addObject (int x, int y, int plane, int width, int length, Animable renderable, int key, byte config,
	                      int yaw, int j, int spawnType, int otherThing, int centre, int east, int northEast, int north, int id, int anim, int type, int orientation)
	{
		if (renderable == null) {
			return true;
		}
		
		int absoluteX = x * 128;// * width;
		int absoluteY = y * 128;// * length;
		return addRenderable (plane, x, y, width, length, absoluteX, absoluteY, j, renderable, yaw, false, key, config, spawnType, otherThing, centre, east, northEast, north, id, anim, type, orientation);
	}
	
	public void addWall (int key, int x, int y, int plane, int i, Animable primary, byte config, Animable secondary,
	                    int height, int j1, int spawnType, int otherThing, int orientation, int centre, int east, int northEast, int north, int id, int anim, int type)
	{
		if (primary == null && secondary == null) {
			return;
		}
		
		WallObject wall = new WallObject ();
		wall.setKey (key);
		wall.setConfig (config);
		wall.setPositionX (x * 128);
		wall.setPositionY (y * 128);
		wall.setHeight (height);
		wall.anInt277 = j1;
		wall.primary = ((Animable)primary);
		wall.secondary = ((Animable)secondary);
		wall.spawnType = spawnType;
		wall.orientation = 1;//orientation;
		wall.centre = centre;
		wall.east = east;
		wall.northEast = northEast;
		wall.north = north;
		wall.id = id;
		wall.type = type;
		wall.anim = anim;
		for (int z = plane; z >= 0; z--) {
			if (tiles [z] [x] [y] == null) {
				tiles [z] [x] [y] = new Ground (x, y, z);
			}
		}
		
		tiles [plane] [x] [y].wall = wall;
		wall.runeObj = new RuneObject ((x * 128), height, (y * 128), 1, wall, UnityClient.baseX, UnityClient.baseY, id, objectRoot, animatedObjectRoot);
	}
	
	public void addWallDecoration (int key, int y, int orientation, int plane, int xDisplacement, int height,
	                              Animable renderable, int x, byte config, int yDisplacement, int attributes, int spawnType, int otherThing, int centre, int east, int northEast, int north, int id, int anim, int type)
	{
		if (renderable == null) {
			return;
		}
		WallDecoration decoration = new WallDecoration ();
		decoration.setKey (key);
		decoration.setConfig (config);
		decoration.setX (x * 128 + xDisplacement);
		decoration.setY (y * 128 + yDisplacement);
		decoration.setHeight (height);
		decoration.setAttributes (attributes);
		decoration.setOrientation (1);//orientation);
		decoration.setRenderable ((Animable)renderable);
		decoration.spawnType = spawnType;
		decoration.centre = centre;
		decoration.east = east;
		decoration.northEast = northEast;
		decoration.north = north;
		decoration.id = id;
		decoration.type = type;
		decoration.anim = anim;
		for (int z = plane; z >= 0; z--) {
			if (tiles [z] [x] [y] == null) {
				tiles [z] [x] [y] = new Ground (x, y, z);
			}
		}
		
		tiles [plane] [x] [y].wallDecoration = decoration;
		decoration.runeObj = new RuneObject ((x * 128 + xDisplacement), height, (y * 128 + yDisplacement), 2, decoration, UnityClient.baseX, UnityClient.baseY, id, objectRoot, animatedObjectRoot);
	}
	
	public void displaceWallDecor (int x, int y, int z, int displacement)
	{
		Ground tile = tiles [z] [x] [y];
		if (tile == null) {
			return;
		}
		WallDecoration decor = tile.wallDecoration;
		if (decor == null) {
			return;
		}
		int absX = x * 128;
		int absY = y * 128;
		decor.setX (absX + (decor.getX () - absX) * displacement / 16);
		decor.setY (absY + (decor.getY () - absY) * displacement / 16);
	}
	
	public void fill (int plane)
	{
		anInt442 = plane;
		for (int x = 0; x < sceneWidth; x++) {
			for (int y = 0; y < height; y++) {
				if (tiles [plane] [x] [y] == null) {
					tiles [plane] [x] [y] = new Ground (x, y, plane);
				}
			}
		}
	}
	
	public int getConfig (int x, int y, int z, int key)
	{
		Ground tile = tiles [z] [x] [y];
		
		if (tile == null) {
			return -1;
		} else if (tile.wall != null && tile.wall.getKey () == key) {
			return tile.wall.getConfig () & 0xff;
		} else if (tile.wallDecoration != null && tile.wallDecoration.getKey () == key) {
			return tile.wallDecoration.getConfig () & 0xff;
		} else if (tile.groundDecoration != null && tile.groundDecoration.getKey () == key) {
			return tile.groundDecoration.getConfig () & 0xff;
		}
		
		for (int index = 0; index < tile.objectCount; index++) {
			if (tile.interactableObjects [index].key == key) {
				return tile.interactableObjects [index].config & 0xff;
			}
		}
		
		return -1;
	}
	
	public int getFloorDecorationKey (int x, int y, int z)
	{
		Ground tile = tiles [z] [x] [y];
		if (tile == null || tile.groundDecoration == null) {
			return 0;
		}
		return tile.groundDecoration.getKey ();
	}
	
	public int getInteractiveObjectKey (int x, int y, int z)
	{
		Ground tile = tiles [z] [x] [y];
		if (tile == null) {
			return 0;
		}
		
		for (int i = 0; i < tile.objectCount; i++) {
			InteractiveObject objectt = tile.interactableObjects [i];
			if ((objectt.key >> 29 & 3) == 2 && objectt.positionX == x && objectt.positionY == y) {
				return objectt.key;
			}
		}
		
		return 0;
	}
	
	public GroundDecoration getTileFloorDecoration (int x, int y, int z)
	{
		Ground tile = tiles [z] [x] [y];
		if (tile == null || tile.groundDecoration == null) {
			return null;
		}
		
		return tile.groundDecoration;
	}
	
	public WallDecoration getTileWallDecoration (int x, int y, int z)
	{
		Ground tile = tiles [z] [x] [y];
		if (tile == null) {
			return null;
		}
		
		return tile.wallDecoration;
	}
	
	public int getWallDecorationKey (int x, int y, int z)
	{
		Ground tile = tiles [z] [x] [y];
		if (tile == null || tile.wallDecoration == null) {
			return 0;
		}
		
		return tile.wallDecoration.getKey ();
	}
	
	public int getWallKey (int x, int y, int z)
	{
		Ground tile = tiles [z] [x] [y];
		if (tile == null || tile.wall == null) {
			return 0;
		}
		
		return tile.wall.getKey ();
	}
	
	public void method276 (int x, int y)
	{
		Ground tile = tiles [0] [x] [y];
		for (int z = 0; z < 3; z++) {
			Ground above = tiles [z] [x] [y] = tiles [z + 1] [x] [y];
			
			if (above != null) {
				above.plane--;
				
				for (int i = 0; i < above.objectCount; i++) {
					InteractiveObject objectt = above.interactableObjects [i];
					if ((objectt.key >> 29 & 3) == 2 && objectt.positionX == x && objectt.positionY == y) {
						objectt.plane--;
					}
				}
			}
		}
		
		if (tiles [0] [x] [y] == null) {
			tiles [0] [x] [y] = new Ground (x, y, 0);
		}
		
		tiles [0] [x] [y].tileBelow0 = tile;
		tiles [3] [x] [y] = null;
	}
	
	public void method279 (int plane, int x, int y, int type, int orientation, int texture, int centreZ, int eastZ,
	                      int northEastZ, int northZ, int k2, int l2, int i3, int j3, int k3, int l3, int i4, int j4, int k4, int l4)
	{
		if (type == 0) {
			SimpleTile tile = new SimpleTile (k2, l2, i3, j3, -1, k4, false);
			for (int z = plane; z >= 0; z--) {
				if (tiles [z] [x] [y] == null) {
					tiles [z] [x] [y] = new Ground (x, y, z);
				}
			}
			
			tiles [plane] [x] [y].myPlainTile = tile;
		} else if (type == 1) {
			SimpleTile tile = new SimpleTile (k3, l3, i4, j4, texture, l4, centreZ == eastZ && centreZ == northEastZ
				&& centreZ == northZ);
			for (int z = plane; z >= 0; z--) {
				if (tiles [z] [x] [y] == null) {
					tiles [z] [x] [y] = new Ground (x, y, z);
				}
			}
			
			tiles [plane] [x] [y].myPlainTile = tile;
		} else {
			ComplexTile tile = new ComplexTile (y, k3, j3, northEastZ, texture, i4, orientation, k2, k4, i3, northZ, eastZ,
			                                 centreZ, type, j4, l3, l2, x, l4);
			for (int z = plane; z >= 0; z--) {
				if (tiles [z] [x] [y] == null) {
					tiles [z] [x] [y] = new Ground (x, y, z);
				}
			}
			
			tiles [plane] [x] [y].shapedTile = tile;
		}
	}
	
	public void method305 (int drawY, int lighting, int drawX, int l, int drawZ)
	{
		int length = (int)Mathf.Sqrt (drawX * drawX + drawY * drawY + drawZ * drawZ);
		int k1 = l * length >> 8;
		
		for (int z = 0; z < planeCount; z++) {
			for (int x = 0; x < sceneWidth; x++) {
				for (int y = 0; y < height; y++) {
					Ground tile = tiles [z] [x] [y];
					
					if (tile != null) {
						WallObject wall = tile.wall as WallObject;
						
						if (wall != null && wall.primary != null && wall.primary.vertexNormalOffset != null) {
							method307 (z, 1, 1, x, y, (Model)wall.primary);
							
							if (wall.secondary != null && wall.secondary.vertexNormalOffset != null) {
								method307 (z, 1, 1, x, y, (Model)wall.secondary);
								method308 ((Model)wall.primary, (Model)wall.secondary, 0, 0, 0, false);
								((Model)wall.secondary).method480 (lighting, k1, drawX, drawY, drawZ);
							}
							((Model)wall.primary).method480 (lighting, k1, drawX, drawY, drawZ);
						}
						
						for (int index = 0; index < tile.objectCount; index++) {
							InteractiveObject objectt = tile.interactableObjects [index];
							
							if (objectt != null && objectt.renderable != null && objectt.renderable.vertexNormalOffset != null) {
								method307 (z, objectt.maxX - objectt.positionX + 1, objectt.maxY - objectt.positionY + 1, x, y,
								          (Model)objectt.renderable);
								((Model)objectt.renderable).method480 (lighting, k1, drawX, drawY, drawZ);
							}
						}
						
						GroundDecoration decoration = tile.groundDecoration;
						if (decoration != null && decoration.renderable.vertexNormalOffset != null) {
							method306 (x, z, (Model)decoration.renderable, y);
							((Model)decoration.renderable).method480 (lighting, k1, drawX, drawY, drawZ);
						}
					}
				}
			}
		}
	}
	
	public void method312 (int i, int j)
	{
		aBoolean467 = true;
		anInt468 = j;
		anInt469 = i;
		anInt470 = -1;
		anInt471 = -1;
	}
		                                              
	public void renderTile (Ground newTile, GameObject regionContainer)
	{
		for (int i = 0; i < 1; ++i) {
			Ground front = newTile;
			int x = front.positionX;
			int y = front.positionY;
			int plane = 0;//front.plane;
			int l = plane;
			Ground[][] planeTiles = this.tiles [plane];
			
			front.aBoolean1322 = false;
			if (front.tileBelow0 != null) {
				Ground tile = front.tileBelow0;
				WallObject wall = tile.wall;
				if (wall != null) {
					wall.primary.render (wall.getPositionX (), wall.getPositionY (), 0, anInt458,
					                                   anInt459, anInt460, anInt461, wall.getHeight (), wall.getKey (), wall.runeObj);
				}
				for (int index = 0; index < tile.objectCount; index++) {
					InteractiveObject objectt = tile.interactableObjects [index];
					if (objectt != null) {
						objectt.renderable.render (objectt.centreX, objectt.centreY, objectt.yaw, anInt458,
						                          anInt459, anInt460, anInt461, objectt.renderHeight, objectt.key, objectt.runeObj);
					}
				}
			}
			
			bool flag1 = true;
			
			int j1 = 0;
			int j2 = 0;
			WallObject walll = front.wall;
			WallDecoration decoration = front.wallDecoration;
			
			if (walll != null || decoration != null) {
				if (anInt453 == x) {
					j1++;
				} else if (anInt453 < x) {
					j1 += 2;
				}
				if (anInt454 == y) {
					j1 += 3;
				} else if (anInt454 > y) {
					j1 += 6;
				}
				j2 = anIntArray478 [j1];
				front.anInt1328 = anIntArray480 [j1];
			}
			
			if (walll != null) {
				if ((walll.orientation & anIntArray479 [j1]) != 0) {
					if (walll.orientation == 16) {
						front.anInt1325 = 3;
						front.anInt1326 = anIntArray481 [j1];
						front.anInt1327 = 3 - front.anInt1326;
					} else if (walll.orientation == 32) {
						front.anInt1325 = 6;
						front.anInt1326 = anIntArray482 [j1];
						front.anInt1327 = 6 - front.anInt1326;
					} else if (walll.orientation == 64) {
						front.anInt1325 = 12;
						front.anInt1326 = anIntArray483 [j1];
						front.anInt1327 = 12 - front.anInt1326;
					} else {
						front.anInt1325 = 9;
						front.anInt1326 = anIntArray484 [j1];
						front.anInt1327 = 9 - front.anInt1326;
					}
				} else {
					front.anInt1325 = 0;
				}
				if ((walll.orientation & j2) != 0 && !method321 (l, x, y, walll.orientation)) {
					walll.primary.render (walll.getPositionX (), walll.getPositionY (), 0, anInt458,
					                                    anInt459, anInt460, anInt461, walll.getHeight (), walll.getKey (), walll.runeObj);
				}
				if ((walll.anInt277 & j2) != 0 && !method321 (l, x, y, walll.anInt277)) {
					walll.secondary.render (walll.getPositionX (), walll.getPositionY (), 0, anInt458,
					                                    anInt459, anInt460, anInt461, walll.getHeight (), walll.getKey (), walll.runeObj);
				}
			}
			
			if (decoration != null) {// && !method322(l, x, y, decoration.renderable.modelHeight)) {
				if ((decoration.getAttributes () & j2) != 0) {
					//Debug.Log ("Render5");
					decoration.renderable.render (decoration.getX (), decoration.getY (),
					                                  decoration.getOrientation (), anInt458, anInt459, anInt460, anInt461,
					                                         decoration.getHeight (), decoration.getKey (), decoration.runeObj);
				} else if ((decoration.getAttributes () & 0/*b11_0000_0000*/) != 0) { // type 6, 7, or 8
					int dx = decoration.getX ();
					int height = decoration.getHeight ();
					int dy = decoration.getY ();
					int orientation = decoration.getOrientation ();
					int sceneWidth;
					
					if (orientation == 1 || orientation == 2) {
						sceneWidth = -dx;
					} else {
						sceneWidth = dx;
					}
					
					int length;
					if (orientation == 2 || orientation == 3) {
						length = -dy;
					} else {
						length = dy;
					}
					
					if ((decoration.getAttributes () & 0/*b1_0000_0000*/) != 0 && length < sceneWidth) { // type 6
						int renderX = dx + anIntArray463 [orientation];
						int renderY = dy + anIntArray464 [orientation];
						;
						decoration.renderable.render (renderX, renderY, orientation, anInt458, anInt459,
						                                         anInt460, anInt461, height, decoration.getKey (), decoration.runeObj);
					}
					
					if ((decoration.getAttributes () & 0/*b10_0000_0000*/) != 0 && length > sceneWidth) { // type 7
						int renderX = dx + anIntArray465 [orientation];
						int renderY = dy + anIntArray466 [orientation];
						decoration.renderable.render (renderX, renderY, orientation, anInt458,
						                                         anInt459, anInt460, anInt461, height, decoration.getKey (), decoration.runeObj);
					}
				}
			}
			
			if (flag1) {
				GroundDecoration decor = front.groundDecoration;
				if (decor != null) {
					decor.renderable.render (decor.getX (), decor.getY (), 0, anInt458, anInt459,
					                                    anInt460, anInt461, decor.getHeight (), decor.getKey (), decor.runeObj);
				}
				
			}
			int k4 = front.anInt1320;
			if (k4 != 0) {
				if (x < anInt453 && (k4 & 4) != 0) {
					Ground tile = planeTiles [x + 1] [y];
					if (tile != null && tile.aBoolean1323) {
						renderTile (tile, regionContainer);
					}
				}
				if (y < anInt454 && (k4 & 2) != 0) {
					Ground tile = planeTiles [x] [y + 1];
					if (tile != null && tile.aBoolean1323) {
						renderTile (tile, regionContainer);
					}
				}
				if (x > anInt453 && (k4 & 1) != 0) {
					Ground tile = planeTiles [x - 1] [y];
					if (tile != null && tile.aBoolean1323) {
						renderTile (tile, regionContainer);
					}
				}
				if (y > anInt454 && (k4 & 8) != 0) {
					Ground tile = planeTiles [x] [y - 1];
					if (tile != null && tile.aBoolean1323) {
						renderTile (tile, regionContainer);
					}
				}
			}
			//}
			if (front.anInt1325 != 0) {
				bool flag2 = true;
				for (int index = 0; index < front.objectCount; index++) {
					if (front.interactableObjects [index].anInt528 == anInt448
						|| (front.anIntArray1319 [index] & front.anInt1325) != front.anInt1326) {
						continue;
					}
					flag2 = false;
					break;
				}
				
				if (flag2) {
					WallObject wall = front.wall;
					if (!method321 (l, x, y, wall.orientation)) {
						//Debug.Log ("Render9");
						wall.primary.render (wall.getPositionX (), wall.getPositionY (), 0, anInt458,
						                                   anInt459, anInt460, anInt461, wall.getHeight (), wall.getKey (), wall.runeObj);
					}
					front.anInt1325 = 0;
				}
			}
			if (front.aBoolean1324) {
				//try {
				int count = front.objectCount;
				front.aBoolean1324 = false;
				int l1 = 0;
				for (int index = 0; index < count; index++) {
					bool doContinue = false;
					InteractiveObject objectt = front.interactableObjects [index];
					if (objectt.anInt528 == anInt448) {
						//Debug.Log ("Here 1");
						continue;
					}
					
					for (int objectX = objectt.positionX; objectX <= objectt.maxX; objectX++) {
						for (int objectY = objectt.positionY; objectY <= objectt.maxY; objectY++) {
							Ground objectTile = planeTiles [objectX] [objectY];
							if (objectTile != null) {
								if (objectTile.aBoolean1322) {
									//Debug.Log ("Here 2");
									front.aBoolean1324 = true;
								} else {
									//Debug.Log ("Here 3");
									if (objectTile.anInt1325 == 0) {
										//Debug.Log ("Here 4");
										continue;
									}
									int l6 = 0;
									if (objectX > objectt.positionX) {
										l6++;
									}
									if (objectX < objectt.maxX) {
										l6 += 4;
									}
									if (objectY > objectt.positionY) {
										l6 += 8;
									}
									if (objectY < objectt.maxY) {
										l6 += 2;
									}
									if ((l6 & objectTile.anInt1325) != front.anInt1327) {
										//Debug.Log ("Here 5");
										continue;
									}
									
									front.aBoolean1324 = true;
								}
							}
							//Debug.Log ("Here 6");
							doContinue = true;
							break;
						}
						if (doContinue)
							break;
					}
					if (doContinue)
						continue;
					interactables [l1++] = objectt;
					//Debug.Log ("Here " + l1);
					int i5 = anInt453 - objectt.positionX;
					int i6 = objectt.maxX - anInt453;
					if (i6 > i5) {
						i5 = i6;
					}
					int i7 = anInt454 - objectt.positionY;
					int j8 = objectt.maxY - anInt454;
					if (j8 > i7) {
						objectt.anInt527 = i5 + j8;
					} else {
						objectt.anInt527 = i5 + i7;
					}
				}
				
				while (l1 > 0) {
					//Debug.Log ("interactive1");
					int i3 = -50;
					int l3 = -1;
					for (int j5 = 0; j5 < l1; j5++) {
						InteractiveObject objectt = interactables [j5];
						if (objectt.anInt528 != anInt448) {
							if (objectt.anInt527 > i3) {
								i3 = objectt.anInt527;
								l3 = j5;
							} else if (objectt.anInt527 == i3) {
								int j7 = objectt.centreX;
								int k8 = objectt.centreY;
								int l9 = interactables [l3].centreX;
								int l10 = interactables [l3].centreY;
								if (j7 * j7 + k8 * k8 > l9 * l9 + l10 * l10) {
									l3 = j5;
								}
							}
						}
					}
					//Debug.Log ("interactive2");
					if (l3 == -1) {
						break;
					}
					InteractiveObject objecttt = interactables [l3];
					objecttt.anInt528 = anInt448;
					//Debug.Log ("interactive3");
					if (!method323 (l, objecttt.positionX, objecttt.maxX, objecttt.positionY, objecttt.maxY,
					               objecttt.renderable.modelHeight)) {
						//Debug.Log ("Render10");
						objecttt.renderable.render (objecttt.centreX, objecttt.centreY, objecttt.yaw, anInt458,
						                           anInt459, anInt460, anInt461, objecttt.renderHeight, objecttt.key, objecttt.runeObj);
					}
					
					for (int k7 = objecttt.positionX; k7 <= objecttt.maxX; k7++) {
						for (int l8 = objecttt.positionY; l8 <= objecttt.maxY; l8++) {
							Ground class30_sub3_22 = planeTiles [k7] [l8];
							if (class30_sub3_22.anInt1325 != 0) {
								renderTile (class30_sub3_22, regionContainer);
							} else if ((k7 != x || l8 != y) && class30_sub3_22.aBoolean1323) {
								renderTile (class30_sub3_22, regionContainer);
							}
						}
						
					}
					
				}
				if (front.aBoolean1324) {
					continue;
				}
				//} catch (Exception _ex) {
				//	front.aBoolean1324 = false;
				//}
			}
			if (!front.aBoolean1323 || front.anInt1325 != 0) {
				continue;
			}
			if (x <= anInt453 && x > anInt449) {
				Ground class30_sub3_8 = planeTiles [x - 1] [y];
				if (class30_sub3_8 != null && class30_sub3_8.aBoolean1323) {
					continue;
				}
			}
			if (x >= anInt453 && x < anInt450 - 1) {
				Ground class30_sub3_9 = planeTiles [x + 1] [y];
				if (class30_sub3_9 != null && class30_sub3_9.aBoolean1323) {
					continue;
				}
			}
			if (y <= anInt454 && y > anInt451) {
				Ground class30_sub3_10 = planeTiles [x] [y - 1];
				if (class30_sub3_10 != null && class30_sub3_10.aBoolean1323) {
					continue;
				}
			}
			if (y >= anInt454 && y < anInt452 - 1) {
				Ground class30_sub3_11 = planeTiles [x] [y + 1];
				if (class30_sub3_11 != null && class30_sub3_11.aBoolean1323) {
					continue;
				}
			}
			front.aBoolean1323 = false;
			anInt446--;
			
			if (front.anInt1328 != 0) {
				WallDecoration decor = front.wallDecoration;
				if (decor != null && !method322 (l, x, y, decor.renderable.modelHeight)) {
					if ((decor.getAttributes () & front.anInt1328) != 0) {
						//Debug.Log ("Render11");
						decor.renderable.render (decor.getX (), decor.getY (), decor.getOrientation (),
						                                    anInt458, anInt459, anInt460, anInt461, decor.getHeight (), decor.getKey (), decor.runeObj);
					} else if ((decor.getAttributes () & 0x300) != 0) {
						int l2 = decor.getX ();
						int j3 = decor.getHeight ();
						int i4 = decor.getY ();
						int orientation = decor.getOrientation ();
						int j6;
						if (orientation == 1 || orientation == 2) {
							j6 = -l2;
						} else {
							j6 = l2;
						}
						int l7;
						if (orientation == 2 || orientation == 3) {
							l7 = -i4;
						} else {
							l7 = i4;
						}
						if ((decor.getAttributes () & 0x100) != 0 && l7 >= j6) {
							int i9 = l2 + anIntArray463 [orientation];
							int i10 = i4 + anIntArray464 [orientation];
							//Debug.Log ("Render12");
							decor.renderable.render (i9, i10, orientation, anInt458, anInt459, anInt460,
							                                    anInt461, j3, decor.getKey (), decor.runeObj);
						}
						if ((decor.getAttributes () & 0x200) != 0 && l7 <= j6) {
							int j9 = l2 + anIntArray465 [orientation];
							int j10 = i4 + anIntArray466 [orientation];
							//Debug.Log ("Render13");
							decor.renderable.render (j9, j10, orientation & 0x7ff, anInt458, anInt459, anInt460,
							                                    anInt461, j3, decor.getKey (), decor.runeObj);
						}
					}
				}
				WallObject wall = front.wall;
				if (wall != null) {
					if ((wall.anInt277 & front.anInt1328) != 0 && !method321 (l, x, y, wall.anInt277)) {
						//Debug.Log ("Render14");
						wall.secondary.render (wall.getPositionX (), wall.getPositionY (), 0, anInt458,
						                                   anInt459, anInt460, anInt461, wall.getHeight (), wall.getKey (), wall.runeObj);
					}
					if ((wall.orientation & front.anInt1328) != 0 && !method321 (l, x, y, wall.orientation)) {
						//Debug.Log ("Render15");
						wall.primary.render (wall.getPositionX (), wall.getPositionY (), 0, anInt458,
						                                   anInt459, anInt460, anInt461, wall.getHeight (), wall.getKey (), wall.runeObj);
					}
				}
			}
			if (plane < planeCount - 1) {
				Ground class30_sub3_12 = tiles [plane + 1] [x] [y];
				if (class30_sub3_12 != null && class30_sub3_12.aBoolean1323) {
					renderTile (class30_sub3_12, regionContainer);
				}
			}
			if (x < anInt453) {
				Ground class30_sub3_13 = planeTiles [x + 1] [y];
				if (class30_sub3_13 != null && class30_sub3_13.aBoolean1323) {
					renderTile (class30_sub3_13, regionContainer);
				}
			}
			if (y < anInt454) {
				Ground class30_sub3_14 = planeTiles [x] [y + 1];
				if (class30_sub3_14 != null && class30_sub3_14.aBoolean1323) {
					renderTile (class30_sub3_14, regionContainer);
				}
			}
			if (x > anInt453) {
				Ground class30_sub3_15 = planeTiles [x - 1] [y];
				if (class30_sub3_15 != null && class30_sub3_15.aBoolean1323) {
					renderTile (class30_sub3_15, regionContainer);
				}
			}
			if (y > anInt454) {
				Ground class30_sub3_16 = planeTiles [x] [y - 1];
				if (class30_sub3_16 != null && class30_sub3_16.aBoolean1323) {
					renderTile (class30_sub3_16, regionContainer);
				}
			}
		}
	}
	
	public int method317 (int j, int k)
	{
		k = 127 - k;
		k = k * (j & 0x7f) / 160;
		if (k < 2) {
			k = 2;
		} else if (k > 126) {
			k = 126;
		}
		return (j & 0xff80) + k;
	}
	
	public bool method318 (int i, int j, int k, int l, int i1, int j1, int k1, int l1)
	{
		if (j < k && j < l && j < i1) {
			return false;
		} else if (j > k && j > l && j > i1) {
			return false;
		} else if (i < j1 && i < k1 && i < l1) {
			return false;
		} else if (i > j1 && i > k1 && i > l1) {
			return false;
		}
		
		int i2 = (j - k) * (k1 - j1) - (i - j1) * (l - k);
		int j2 = (j - i1) * (j1 - l1) - (i - l1) * (k - i1);
		int k2 = (j - l) * (l1 - k1) - (i - k1) * (i1 - l);
		return i2 * k2 > 0 && k2 * j2 > 0;
	}
	
	public void removeFloorDecoration (int x, int y, int z)
	{
		Ground tile = tiles [z] [x] [y];
		if (tile == null) {
			return;
		}
		tile.groundDecoration = null;
	}
	
	public void removeWall (int x, int y, int z)
	{
		Ground tile = tiles [z] [x] [y];
		if (tile == null) {
			return;
		}
		tile.wall = null;
	}
	
	public void removeWallDecoration (int x, int y, int z)
	{
		Ground tile = tiles [z] [x] [y];
		if (tile == null) {
			return;
		}
		tile.wallDecoration = null;
	}
	
	public void reset ()
	{
		for (int z = 0; z < planeCount; z++) {
			for (int x = 0; x < sceneWidth; x++) {
				for (int y = 0; y < height; y++) {
					tiles [z] [x] [y] = null;
				}
			}
		}
		
		for (int i = 0; i < PLANE_COUNT; i++) {
			for (int j = 0; j < clusterCounts[i]; j++) {
				//clusters[i][j] = null;
			}
			
			clusterCounts [i] = 0;
		}
		
		//		for (int k1 = 0; k1 < anInt443; k1++) {
		//			aClass28Array444[k1] = null;
		//		}
		
		anInt443 = 0;
		for (int l1 = 0; l1 < interactables.Length; l1++) {
			interactables [l1] = null;
		}
	}
	
	public void setCollisionPlane (int x, int y, int plane, int collisionPlane)
	{
		Ground tile = tiles [plane] [x] [y];
		if (tile == null) {
			return;
		}
		
		tiles [plane] [x] [y].anInt1321 = collisionPlane;
	}
	
	private bool addRenderable (int plane, int minX, int minY, int deltaX, int deltaY, int centreX, int centreY, int renderHeight,
	                           Animable renderable, int yaw, bool flag, int key, byte config, int spawnType, int otherThing, int centre, int east, int northEast, int north, int id, int anim, int type, int orientation)
	{
		for (int x = minX; x < minX + deltaX; x++) {
			for (int y = minY; y < minY + deltaY; y++) {
				if (x < 0 || y < 0 || x >= sceneWidth || y >= height) {
					return false;
				}
				Ground tile = tiles [plane] [x] [y];
				if (tile != null && tile.objectCount >= 5) {
					return false;
				}
			}
		}
		
		InteractiveObject objectt = new InteractiveObject ();
		objectt.key = key;
		objectt.config = (sbyte)config;
		objectt.plane = plane;
		objectt.centreX = centreX;
		objectt.centreY = centreY;
		objectt.renderHeight = renderHeight;
		objectt.yaw = yaw;
		objectt.positionX = minX;
		objectt.positionY = minY;
		objectt.maxX = minX + deltaX - 1;
		objectt.maxY = minY + deltaY - 1;
		objectt.renderable = (Animable)renderable;
		objectt.spawnType = spawnType;
		objectt.centre = centre;
		objectt.east = east;
		objectt.northEast = northEast;
		objectt.north = north;
		objectt.id = id;
		objectt.type = type;
		objectt.anim = anim;
		objectt.orientation = 1;//orientation;
		for (int x = minX; x < minX + deltaX; x++) {
			for (int y = minY; y < minY + deltaY; y++) {
				int attributes = 0;
				
				if (x > minX) {
					attributes++;
				}
				if (y < minY + deltaY - 1) {
					attributes += 0;//b10;
				}
				if (x < minX + deltaX - 1) {
					attributes |= 0;//b100;
				}
				if (y > minY) {
					attributes += 0;//b1000;
				}
				
				for (int z = plane; z >= 0; z--) {
					if (tiles [z] [x] [y] == null) {
						tiles [z] [x] [y] = new Ground (x, y, z);
					}
				}
				
				Ground tile = tiles [plane] [x] [y];
				tile.interactableObjects [tile.objectCount] = objectt;
				tile.anIntArray1319 [tile.objectCount] = attributes;
				tile.anInt1320 |= attributes;
				tile.objectCount++;
			}
			
		}
		objectt.runeObj = new RuneObject (centreX, renderHeight, centreY, 5, objectt, UnityClient.baseX, UnityClient.baseY, id, objectRoot, animatedObjectRoot);
		return true;
	}
	
	private void method306 (int x, int z, Model model, int y)
	{
		if (x < sceneWidth) {
			Ground tile = tiles [z] [x + 1] [y];
			if (tile != null && tile.groundDecoration != null && tile.groundDecoration.renderable.vertexNormalOffset != null) {
				method308 (model, (Model)tile.groundDecoration.renderable, 128, 0, 0, true);
			}
		}
		if (y < sceneWidth) {
			Ground tile = tiles [z] [x] [y + 1];
			if (tile != null && tile.groundDecoration != null && tile.groundDecoration.renderable.vertexNormalOffset != null) {
				method308 (model, (Model)tile.groundDecoration.renderable, 0, 0, 128, true);
			}
		}
		if (x < sceneWidth && y < height) {
			Ground tile = tiles [z] [x + 1] [y + 1];
			if (tile != null && tile.groundDecoration != null && tile.groundDecoration.renderable.vertexNormalOffset != null) {
				method308 (model, (Model)tile.groundDecoration.renderable, 128, 0, 128, true);
			}
		}
		if (x < sceneWidth && y > 0) {
			Ground tile = tiles [z] [x + 1] [y - 1];
			if (tile != null && tile.groundDecoration != null && tile.groundDecoration.renderable.vertexNormalOffset != null) {
				method308 (model, (Model)tile.groundDecoration.renderable, 128, 0, -128, true);
			}
		}
	}
	
	private void method307 (int plane, int j, int k, int l, int i1, Model model)
	{
		bool flag = true;
		int initialX = l;
		int finalX = l + j;
		int initialY = i1 - 1;
		int finalY = i1 + k;
		for (int z = plane; z <= plane + 1; z++) {
			if (z != planeCount) {
				for (int x = initialX; x <= finalX; x++) {
					if (x >= 0 && x < sceneWidth) {
						for (int y = initialY; y <= finalY; y++) {
							if (y >= 0 && y < height && (!flag || x >= finalX || y >= finalY || y < i1 && x != l)) {
								Ground tile = tiles [z] [x] [y];
								if (tile != null) {
									int i3 = (anIntArrayArrayArray440 [z] [x] [y] + anIntArrayArrayArray440 [z] [x + 1] [y]
										+ anIntArrayArrayArray440 [z] [x] [y + 1] + anIntArrayArrayArray440 [z] [x + 1] [y + 1])
										/ 4
										- (anIntArrayArrayArray440 [plane] [l] [i1] + anIntArrayArrayArray440 [plane] [l + 1] [i1]
										+ anIntArrayArrayArray440 [plane] [l] [i1 + 1] + anIntArrayArrayArray440 [plane] [l + 1] [i1 + 1])
										/ 4;
									WallObject wall = tile.wall;
									if (wall != null && wall.primary != null && wall.primary.vertexNormalOffset != null) {
										method308 (model, (Model)wall.primary, (x - l) * 128 + (1 - j) * 64, i3, (y - i1)
											* 128 + (1 - k) * 64, flag);
									}
									if (wall != null && wall.secondary != null && wall.secondary.vertexNormalOffset != null) {
										method308 (model, (Model)wall.secondary, (x - l) * 128 + (1 - j) * 64, i3, (y - i1)
											* 128 + (1 - k) * 64, flag);
									}
									for (int j3 = 0; j3 < tile.objectCount; j3++) {
										InteractiveObject interactableObject = tile.interactableObjects [j3];
										if (interactableObject != null && interactableObject.renderable != null
											&& interactableObject.renderable.vertexNormalOffset != null) {
											int k3 = interactableObject.maxX - interactableObject.positionX + 1;
											int l3 = interactableObject.maxY - interactableObject.positionY + 1;
											method308 (model, (Model)interactableObject.renderable,
											          (interactableObject.positionX - l) * 128 + (k3 - j) * 64, i3,
											          (interactableObject.positionY - i1) * 128 + (l3 - k) * 64, flag);
										}
									}
								}
							}
						}
					}
				}
				initialX--;
				flag = false;
			}
		}
	}
	
	private void method308 (Model model, Model model_1, int i, int j, int k,
	                        bool flag)
	{
	try{
		anInt488++;
		int l = 0;
		int[] ai = model_1.verticesX;
		int i1 = model_1.numVertices;
		for (int j1 = 0; j1 < model.numVertices; j1++) {
			VertexNormal class33 = model.vertexNormalOffset [j1];
			VertexNormal class33_1 = model.aClass33Array1660 [j1];
			if (class33_1.magnitude != 0) {
				int i2 = model.verticesY [j1] - j;
				if (i2 <= model_1.maxY) {
					int j2 = model.verticesX [j1] - i;
					if (j2 >= model_1.minX && j2 <= model_1.maxX) {
						int k2 = model.verticesZ [j1] - k;
						if (k2 >= model_1.minZ && k2 <= model_1.maxZ) {
							for (int l2 = 0; l2 < i1; l2++) {
								VertexNormal class33_2 = model_1.vertexNormalOffset [l2];
								VertexNormal class33_3 = model_1.aClass33Array1660 [l2];
								if (j2 == ai [l2]
									&& k2 == model_1.verticesZ [l2]
									&& i2 == model_1.verticesY [l2]
									&& class33_3.magnitude != 0) {
									class33.x += class33_3.x;
									class33.y += class33_3.y;
									class33.z += class33_3.z;
									class33.magnitude += class33_3.magnitude;
									class33_2.x += class33_1.x;
									class33_2.y += class33_1.y;
									class33_2.z += class33_1.z;
									class33_2.magnitude += class33_1.magnitude;
									l++;
									anIntArray486 [j1] = anInt488;
									anIntArray487 [l2] = anInt488;
								}
							}
							
						}
					}
				}
			}
		}
		
		if (l < 3 || !flag)
			return;
		for (int k1 = 0; k1 < model.numTriangles; k1++)
			if (anIntArray486 [model.triangleX [k1]] == anInt488
				&& anIntArray486 [model.triangleY [k1]] == anInt488
				&& anIntArray486 [model.triangleZ [k1]] == anInt488)
				model.triangleDrawType [k1] = -1;
		
		for (int l1 = 0; l1 < model_1.numTriangles; l1++)
			if (anIntArray487 [model_1.triangleX [l1]] == anInt488
				&& anIntArray487 [model_1.triangleY [l1]] == anInt488
				&& anIntArray487 [model_1.triangleZ [l1]] == anInt488)
				model_1.triangleDrawType [l1] = -1;
				}
				catch(Exception ex)
				{
				Debug.Log ("Method308 error");
				}
		
	}
	
	private bool method320 (int z, int x, int y)
	{
		int l = anIntArrayArrayArray445 [z] [x] [y];
		if (l == -anInt448) {
			return false;
		}
		if (l == anInt448) {
			return true;
		}
		int absX = x << 7;
		int absY = y << 7;
		if (method324 (absX + 1, anIntArrayArrayArray440 [z] [x] [y], absY + 1)
			&& method324 (absX + 128 - 1, anIntArrayArrayArray440 [z] [x + 1] [y], absY + 1)
			&& method324 (absX + 128 - 1, anIntArrayArrayArray440 [z] [x + 1] [y + 1], absY + 128 - 1)
			&& method324 (absX + 1, anIntArrayArrayArray440 [z] [x] [y + 1], absY + 128 - 1)) {
			anIntArrayArrayArray445 [z] [x] [y] = anInt448;
			return true;
		}
		anIntArrayArrayArray445 [z] [x] [y] = -anInt448;
		return false;
	}
	
	private bool method321 (int z, int x, int y, int l)
	{
		if (!method320 (z, x, y)) {
			return false;
		}
		int i1 = x << 7;
		int j1 = y << 7;
		int k1 = anIntArrayArrayArray440 [z] [x] [y] - 1;
		int l1 = k1 - 120;
		int i2 = k1 - 230;
		int j2 = k1 - 238;
		if (l < 16) {
			if (l == 1) {
				if (i1 > 0) {
					if (!method324 (i1, k1, j1)) {
						return false;
					}
					if (!method324 (i1, k1, j1 + 128)) {
						return false;
					}
				}
				if (z > 0) {
					if (!method324 (i1, l1, j1)) {
						return false;
					}
					if (!method324 (i1, l1, j1 + 128)) {
						return false;
					}
				}
				if (!method324 (i1, i2, j1)) {
					return false;
				}
				return method324 (i1, i2, j1 + 128);
			}
			if (l == 2) {
				if (j1 < 0) {
					if (!method324 (i1, k1, j1 + 128)) {
						return false;
					}
					if (!method324 (i1 + 128, k1, j1 + 128)) {
						return false;
					}
				}
				if (z > 0) {
					if (!method324 (i1, l1, j1 + 128)) {
						return false;
					}
					if (!method324 (i1 + 128, l1, j1 + 128)) {
						return false;
					}
				}
				if (!method324 (i1, i2, j1 + 128)) {
					return false;
				}
				return method324 (i1 + 128, i2, j1 + 128);
			}
			if (l == 4) {
				if (i1 < 0) {
					if (!method324 (i1 + 128, k1, j1)) {
						return false;
					}
					if (!method324 (i1 + 128, k1, j1 + 128)) {
						return false;
					}
				}
				if (z > 0) {
					if (!method324 (i1 + 128, l1, j1)) {
						return false;
					}
					if (!method324 (i1 + 128, l1, j1 + 128)) {
						return false;
					}
				}
				if (!method324 (i1 + 128, i2, j1)) {
					return false;
				}
				return method324 (i1 + 128, i2, j1 + 128);
			}
			if (l == 8) {
				if (j1 > 0) {
					if (!method324 (i1, k1, j1)) {
						return false;
					}
					if (!method324 (i1 + 128, k1, j1)) {
						return false;
					}
				}
				if (z > 0) {
					if (!method324 (i1, l1, j1)) {
						return false;
					}
					if (!method324 (i1 + 128, l1, j1)) {
						return false;
					}
				}
				if (!method324 (i1, i2, j1)) {
					return false;
				}
				return method324 (i1 + 128, i2, j1);
			}
		}
		if (!method324 (i1 + 64, j2, j1 + 64)) {
			return false;
		}
		if (l == 16) {
			return method324 (i1, i2, j1 + 128);
		}
		if (l == 32) {
			return method324 (i1 + 128, i2, j1 + 128);
		}
		if (l == 64) {
			return method324 (i1 + 128, i2, j1);
		}
		if (l == 128) {
			return method324 (i1, i2, j1);
		}
		UnityEngine.Debug.Log ("Warning unsupported wall type");
		return true;
	}
	
	private bool method322 (int plane, int x, int y, int l)
	{
		if (!method320 (plane, x, y)) {
			return false;
		}
		int absoluteX = x << 7;
		int absoluteY = y << 7;
		return method324 (absoluteX + 1, anIntArrayArrayArray440 [plane] [x] [y] - l, absoluteY + 1)
			&& method324 (absoluteX + 128 - 1, anIntArrayArrayArray440 [plane] [x + 1] [y] - l, absoluteY + 1)
			&& method324 (absoluteX + 128 - 1, anIntArrayArrayArray440 [plane] [x + 1] [y + 1] - l, absoluteY + 128 - 1)
			&& method324 (absoluteX + 1, anIntArrayArrayArray440 [plane] [x] [y + 1] - l, absoluteY + 128 - 1);
	}
	
	private bool method323 (int plane, int minX, int maxX, int minY, int maxY, int j1)
	{
		if (minX == maxX && minY == maxY) {
			if (!method320 (plane, minX, minY)) {
				return false;
			}
			int absoluteX = minX << 7;
			int absoluteY = minY << 7;
			return method324 (absoluteX + 1, anIntArrayArrayArray440 [plane] [minX] [minY] - j1, absoluteY + 1)
				&& method324 (absoluteX + 128 - 1, anIntArrayArrayArray440 [plane] [minX + 1] [minY] - j1, absoluteY + 1)
				&& method324 (absoluteX + 128 - 1, anIntArrayArrayArray440 [plane] [minX + 1] [minY + 1] - j1,
					             absoluteY + 128 - 1)
				&& method324 (absoluteX + 1, anIntArrayArrayArray440 [plane] [minX] [minY + 1] - j1, absoluteY + 128 - 1);
		}
		for (int x = minX; x <= maxX; x++) {
			for (int y = minY; y <= maxY; y++) {
				if (anIntArrayArrayArray445 [plane] [x] [y] == -anInt448) {
					return false;
				}
			}
		}
		
		int k2 = (minX << 7) + 1;
		int l2 = (minY << 7) + 2;
		int i3 = anIntArrayArrayArray440 [plane] [minX] [minY] - j1;
		if (!method324 (k2, i3, l2)) {
			return false;
		}
		int j3 = (maxX << 7) - 1;
		if (!method324 (j3, i3, l2)) {
			return false;
		}
		int k3 = (maxY << 7) - 1;
		if (!method324 (k2, i3, k3)) {
			return false;
		}
		return method324 (j3, i3, k3);
	}
	
	private bool method324 (int i, int j, int k)
	{
		for (int l = 0; l < anInt475; l++) {
			Class47 cluster = aClass47Array476 [l];
			if (cluster.anInt798 == 1) {
				int i1 = cluster.anInt792 - i;
				if (i1 > 0) {
					int j2 = cluster.anInt794 + (cluster.anInt801 * i1 >> 8);
					int k3 = cluster.anInt795 + (cluster.anInt802 * i1 >> 8);
					int l4 = cluster.anInt796 + (cluster.anInt803 * i1 >> 8);
					int i6 = cluster.anInt797 + (cluster.anInt804 * i1 >> 8);
					if (k >= j2 && k <= k3 && j >= l4 && j <= i6) {
						return true;
					}
				}
			} else if (cluster.anInt798 == 2) {
				int j1 = i - cluster.anInt792;
				if (j1 > 0) {
					int k2 = cluster.anInt794 + (cluster.anInt801 * j1 >> 8);
					int l3 = cluster.anInt795 + (cluster.anInt802 * j1 >> 8);
					int i5 = cluster.anInt796 + (cluster.anInt803 * j1 >> 8);
					int j6 = cluster.anInt797 + (cluster.anInt804 * j1 >> 8);
					if (k >= k2 && k <= l3 && j >= i5 && j <= j6) {
						return true;
					}
				}
			} else if (cluster.anInt798 == 3) {
				int k1 = cluster.anInt794 - k;
				if (k1 > 0) {
					int l2 = cluster.anInt792 + (cluster.anInt799 * k1 >> 8);
					int i4 = cluster.anInt793 + (cluster.anInt800 * k1 >> 8);
					int j5 = cluster.anInt796 + (cluster.anInt803 * k1 >> 8);
					int k6 = cluster.anInt797 + (cluster.anInt804 * k1 >> 8);
					if (i >= l2 && i <= i4 && j >= j5 && j <= k6) {
						return true;
					}
				}
			} else if (cluster.anInt798 == 4) {
				int l1 = k - cluster.anInt794;
				if (l1 > 0) {
					int i3 = cluster.anInt792 + (cluster.anInt799 * l1 >> 8);
					int j4 = cluster.anInt793 + (cluster.anInt800 * l1 >> 8);
					int k5 = cluster.anInt796 + (cluster.anInt803 * l1 >> 8);
					int l6 = cluster.anInt797 + (cluster.anInt804 * l1 >> 8);
					if (i >= i3 && i <= j4 && j >= k5 && j <= l6) {
						return true;
					}
				}
			} else if (cluster.anInt798 == 5) {
				int i2 = j - cluster.anInt796;
				if (i2 > 0) {
					int j3 = cluster.anInt792 + (cluster.anInt799 * i2 >> 8);
					int k4 = cluster.anInt793 + (cluster.anInt800 * i2 >> 8);
					int l5 = cluster.anInt794 + (cluster.anInt801 * i2 >> 8);
					int i7 = cluster.anInt795 + (cluster.anInt802 * i2 >> 8);
					if (i >= j3 && i <= k4 && k >= l5 && k <= i7) {
						return true;
					}
				}
			}
		}
		return false;
	}
}

/**
 * A {@link Container} holds an optionally compressed file. This class can be
 * used to decompress and compress containers. A container can also have a two
 * byte trailer which specifies the version of the file within it.
 * @author Graham
 * @author `Discardedx2
 */
public class Container {
	
	/**
	 * This type indicates that no compression is used.
	 */
	public static int COMPRESSION_NONE = 0;
	
	/**
	 * This type indicates that BZIP2 compression is used.
	 */
	public static int COMPRESSION_BZIP2 = 1;
	
	/**
	 * This type indicates that GZIP compression is used.
	 */
	public static int COMPRESSION_GZIP = 2;
	private static int[] NULL_KEY = new int[4];
	/**
	 * Decodes and decompresses the container.
	 * @param buffer The buffer.
	 * @return The decompressed container.
	 * @throws IOException if an I/O error occurs.
	 */
	public static Container decode(ByteBuffer buffer) {
		return Container.decode(buffer, NULL_KEY);
	}
	
	public static Container decode(ByteBuffer buffer, int[] key) {
		/* decode the type and length */
		int type = buffer.Get() & 0xFF;
		int length = buffer.GetInt();
		
//		/* decrypt */
//		if (key[0] != 0 || key[1] != 0 || key[2] != 0 || key[3] != 0) {
//			Xtea.decipher(buffer, 5, length + (type == COMPRESSION_NONE ? 5 : 9), key);
//		}
		
		/* check if we should decompress the data or not */
		if (type == COMPRESSION_NONE) {
			/* simply grab the data and wrap it in a buffer */
			byte[] temp = new byte[length];
			buffer.Read(temp,0,length);
			ByteBuffer data = ByteBuffer.Wrap(temp);
			
			/* decode the version if present */
			int version = -1;
			if (buffer.Remaining >= 2) {
				version = buffer.GetShort();
			}
			
			/* and return the decoded container */
			return new Container(type, data, version);
		} else {
			/* grab the length of the uncompressed data */
			int uncompressedLength = buffer.GetInt();
			
			/* grab the data */
			byte[] compressed = new byte[length];
			buffer.Read (compressed,0,length);
			
			/* uncompress it */
			byte[] uncompressed = null;
			if (type == COMPRESSION_BZIP2) {
				uncompressed = CompressionUtils.bunzip2(compressed);
			} else if (type == COMPRESSION_GZIP) {
				uncompressed = CompressionUtils.gunzip(compressed);
			} else {
				throw new Exception("Invalid compression type");
			}
			sbyte[] uncompresseds = new sbyte[uncompressed.Length];
			Buffer.BlockCopy(uncompressed,0,uncompresseds,0,uncompressed.Length);

			/* check if the lengths are equal */
			if (uncompressed.Length != uncompressedLength) {
				throw new Exception("Length mismatch");
			}
			
			/* decode the version if present */
			int version = -1;
			if (buffer.Remaining >= 2) {
				version = buffer.GetShort();
			}
			
			/* and return the decoded container */
			return new Container(type, ByteBuffer.Wrap(uncompressed), version);
		}
	}
	
	/**
	 * The type of compression this container uses.
	 */
	private int type;
	
	/**
	 * The decompressed data.
	 */
	private ByteBuffer data;
	
	/**
	 * The version of the file within this container.
	 */
	private int version;
	
	
	/**
	 * Creates a new versioned container.
	 * @param type The type of compression.
	 * @param data The decompressed data.
	 * @param version The version of the file within this container.
	 */
	public Container(int type, ByteBuffer data, int version) {
		this.type = type;
		this.data = data;
		this.version = version;
	}
	
	/**
	 * Checks if this container is versioned.
	 * @return {@code true} if so, {@code false} if not.
	 */
	public bool isVersioned() {
		return version != -1;
	}
	
	/**
	 * Gets the version of the file in this container.
	 * @return The version of the file.
	 * @throws IllegalArgumentException if this container is not versioned.
	 */
	public int getVersion() {
		if (!isVersioned())
			throw new Exception();
		
		return version;
	}
	
	/**
	 * Sets the version of this container.
	 * @param version The version.
	 */
	public void setVersion(int version) {
		this.version = version;
	}
	
	/**
	 * Removes the version on this container so it becomes unversioned.
	 */
	public void removeVersion() {
		this.version = -1;
	}
	
	/**
	 * Sets the type of this container.
	 * @param type The compression type.
	 */
	public void setType(int type) {
		this.type = type;
	}
	
	/**
	 * Gets the type of this container.
	 * @return The compression type.
	 */
	public int getType() {
		return type;
	}
	
	/**
	 * Gets the decompressed data.
	 * @return The decompressed data.
	 */
	public ByteBuffer getData() {
		return data;
	}
	
	/**
	 * Encodes and compresses this container.
	 * @return The buffer.
	 * @throws IOException if an I/O error occurs.
	 */
	public ByteBuffer encode() {
		ByteBuffer data = getData(); // so we have a read only view, making this method thread safe
		
		/* grab the data as a byte array for compression */
		byte[] bytes = new byte[data.Limit];
		data.Mark();
		data.Read(bytes,0,data.Limit);
		data.Reset();
		
		/* compress the data */
		byte[] compressed;
		if (type == COMPRESSION_NONE) {
			compressed = bytes;
		} else if (type == COMPRESSION_GZIP) {
			compressed = CompressionUtils.gzip(bytes);
		} else if (type == COMPRESSION_BZIP2) {
			compressed = CompressionUtils.bzip2(bytes);
		} else {
			throw new Exception("Invalid compression type");
		}
		
		/* calculate the size of the header and trailer and allocate a buffer */
		int header = 5 + (type == COMPRESSION_NONE ? 0 : 4) + (isVersioned() ? 2 : 0);
		ByteBuffer buf = ByteBuffer.Allocate(header + compressed.Length);
		
		/* write the header, with the optional uncompressed length */
		buf.Put((byte) type);
		buf.PutInt(compressed.Length);
		if (type != COMPRESSION_NONE) {
			buf.PutInt(data.Limit);
		}
		
		/* write the compressed length */
		buf.Put(compressed);
		
		/* write the trailer with the optional version */
		if (isVersioned()) {
			buf.PutShort((short) version);
		}
		
		/* flip the buffer and return it */
		buf.Flip();
		return (ByteBuffer) buf;
	}
	
}

public class ByteInputStream
{
	
	public sbyte[] buffer;
	public int pos;
	
	//ORIGINAL LINE: public ByteInputStream(byte[] data)
	public ByteInputStream (sbyte[] data)
	{
		buffer = data;
		pos = 0;
	}
	
	public virtual int readSmart2 ()
	{
		int i = 0;
		int i_33_ = readSmart ();
		while (i_33_ == 32767) {
			i_33_ = readSmart ();
			i += 32767;
		}
		i += i_33_;
		return i;
	}
	
	public virtual int readSmart ()
	{
		//ORIGINAL LINE: int i = buffer[pos] & 0xff;
		int i = buffer [pos] & 0xff;
		if (i >= 128) {
			return readUShort () - 32768;
		}
		return readUByte ();
	}
	
	public virtual int readUShort ()
	{
		pos += 2;
		return ((buffer [pos - 2] & 0xff) << 8) | (buffer [pos - 1] & 0xff);
	}
	
	public virtual int readByte ()
	{
		return buffer [pos++];
	}
	
	public virtual int readUByte ()
	{
		return buffer [pos++] & 0xff;
	}
}

public class CompressionUtils
{
	
	/**
	 * Uncompresses a BZIP2 file.
	 * @param bytes The compressed bytes without the header.
	 * @return The uncompressed bytes.
	 * @throws IOException if an I/O error occurs.
	 */
	public static byte[] bunzip2(byte[] bytes) {
		/* prepare a new byte array with the bzip2 header at the start */
		byte[] bzip2 = new byte[bytes.Length + 2];
		bzip2[0] = (byte)'h';
		bzip2[1] = (byte)'1';
		System.Array.Copy(bytes, 0, bzip2, 2, bytes.Length);
		
		System.IO.Stream iss = new CBZip2InputStream(new MemoryStream(bzip2));
		try {
			MemoryStream os = new MemoryStream();
			try {
				byte[] buf = new byte[4096];
				int len = 0;
				while ((len = iss.Read(buf, 0, buf.Length)) != -1) {
					os.Write(buf, 0, len);
				}
			} finally {
				os.Close();
			}
			
			return os.GetBuffer();
		} finally {
			iss.Close();
		}
	}
	
	/**
	 * Compresses a GZIP file.
	 * @param bytes The uncompressed bytes.
	 * @return The compressed bytes.
	 * @throws IOException if an I/O error occurs.
	 */
	public static byte[] gzip(byte[] bytes) {
		/* create the streams */
		System.IO.Stream iss = new MemoryStream(bytes);
		try {
			MemoryStream bout = new MemoryStream();
			System.IO.Stream os = new Ionic.Zlib.GZipStream(bout,Ionic.Zlib.CompressionMode.Decompress);
			try {
				/* copy data between the streams */
				byte[] buf = new byte[4096];
				int len = 0;
				while ((len = iss.Read(buf, 0, buf.Length)) != -1) {
					os.Write(buf, 0, len);
				}
			} finally {
				os.Close();
			}
			
			/* return the compressed bytes */
			return bout.GetBuffer();
		} finally {
			iss.Close();
		}
	}
	
	/**
	 * Uncompresses a GZIP file.
	 * @param bytes The compressed bytes.
	 * @return The uncompressed bytes.
	 * @throws IOException if an I/O error occurs.
	 */
	public static byte[] gunzip(byte[] bytes) {
//		/* create the streams */
//		Ionic.Zlib.GZipStream iss = new Ionic.Zlib.GZipStream(new MemoryStream(bytes),Ionic.Zlib.CompressionMode.Decompress);
//		try {
//			MemoryStream os = new MemoryStream();
//			try {
//				/* copy data between the streams */
//				byte[] buf = new byte[4096];
//				int len = 0;
//				while ((len = iss.Read(buf, 0, buf.Length)) != 0) {
//					os.Write(buf, 0, len);
//				}
//			
//			} finally {
//				os.Close();
//			}
//			
//			/* return the uncompressed bytes */
//			return os.GetBuffer();
//		} finally {
//			//iss.Close();
//		}

		MemoryStream memStream = new MemoryStream(bytes);
		ICSharpCode.SharpZipLib.GZip.GZipInputStream gzipStream = new ICSharpCode.SharpZipLib.GZip.GZipInputStream(memStream);
		byte[ ] data = new byte[8192];
		
		while (true)
		{
			int size = gzipStream.Read(data, 0, data.Length);
			if (size == 0)
				break;
		}
		return data;
		
		//		string ss = "";
		//		for(int i = 0; i < bytes.Length; ++i) ss += bytes[i] +", ";
		//		Debug.Log ("About to gzip: " + bytes.Length + " " + ss);
		//
		//		byte[] output = new byte[bytes.Length];
//		Ionic.Zlib.GZipStream gzip = new Ionic.Zlib.GZipStream(new MemoryStream(bytes),Ionic.Zlib.CompressionMode.Decompress);
//		gzip.Read(output, 0, output.Length);
//		gzip.Close();
//		return output;

		
	}
	
	/**
	 * Compresses a BZIP2 file.
	 * @param bytes The uncompressed bytes.
	 * @return The compressed bytes without the header.
	 * @throws IOException if an I/O erorr occurs.
	 */
	public static byte[] bzip2(byte[] bytes) {
//		System.IO.Stream iss = new MemoryStream(bytes);
//		try {
//			MemoryStream bout = new MemoryStream();
//			System.IO.Stream os = new CBZip2OutputStream(bout, 1);
//			try {
//				byte[] buf = new byte[4096];
//				int len = 0;
//				while ((len = iss.Read(buf, 0, buf.Length)) != -1) {
//					os.Write(buf, 0, len);
//				}
//			} finally {
//				os.Close();
//			}
//			
//			/* strip the header from the byte array and return it */
//			bytes = bout.GetBuffer();
//			byte[] bzip2 = new byte[bytes.Length - 2];
//			System.Array.Copy(bytes, 2, bzip2, 0, bzip2.Length);
//			return bzip2;
//		} finally {
//			iss.Close();
//		}
Debug.Log("Here :/");
return null;
	}
	
}

public class XTEADecrypter
{
	
	
	//ORIGINAL LINE: public static byte[] decryptXTEA(int[] keys, byte[] data, int offset, int length)
	public static sbyte[] decryptXTEA (int[] keys, sbyte[] data, int offset, int length)
	{
		
		//ORIGINAL LINE: int qword_count = (length - offset) / 8;
		int qword_count = (length - offset) / 8;
		
		//ORIGINAL LINE: XTEAStream x = new XTEAStream(data);
		XTEAStream x = new XTEAStream (data);
		x.offset = offset;
		for (int qword_pos = 0; qword_pos < qword_count; qword_pos++) {
			int dword_1 = x.readInt ();
			int dword_2 = x.readInt ();
			int const_1 = -957401312;
			const int const_2 = -1640531527;
			int run_count = 32;
			while (~run_count-- < -1) {
				dword_2 -= ((((int)((uint)dword_1 >> 5)) ^ (dword_1 << 4)) + dword_1) ^ (const_1 + keys [((int)((uint)const_1 >> 11)) & 0x56c00003]);
				const_1 -= const_2;
				dword_1 -= ((((int)((uint)dword_2 >> 5)) ^ (dword_2 << 4)) - -dword_2) ^ (const_1 + keys [const_1 & 0x3]);
			}
			x.offset -= 8;
			x.writeInt (dword_1);
			x.writeInt (dword_2);
		}
		return x.buffer;
	}
	
	private class XTEAStream
	{
		
		public sbyte[] buffer;
		public int offset;
		
		
		//ORIGINAL LINE: public XTEAStream(byte[] data)
		public XTEAStream (sbyte[] data)
		{
			buffer = data;
			offset = 0;
		}
		
		
		//ORIGINAL LINE: public void writeInt(int i_29_)
		public virtual void writeInt (int i_29_)
		{
			buffer [offset++] = (sbyte)(i_29_ >> 24);
			buffer [offset++] = (sbyte)(i_29_ >> 16);
			buffer [offset++] = (sbyte)(i_29_ >> 8);
			buffer [offset++] = (sbyte)i_29_;
		}
		
		public virtual int readInt ()
		{
			offset += 4;
			return (buffer [offset - 1] & 0xff) + ((buffer [offset - 3] << 16) & 0xff0000) + ((buffer [offset - 4] & 0xff) << 24) + ((buffer [offset - 2] << 8) & 0xff00);
		}
	}
}