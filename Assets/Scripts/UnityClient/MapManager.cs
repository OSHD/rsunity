using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RS2Sharp;
using sign;
using System.Net;
using System;
using System.IO;

public static class MapManager
{
	private static int[] mapIndices1;
	private static int[] mapIndices2;
	private static int[] mapIndices3;
	public static List<Region> regions = new List<Region> ();
	public static List<Region> activeRegions = new List<Region> ();
	private static List<RegionHeightMap> regionHeights = new List<RegionHeightMap> ();
	public static bool initialized = false;

	public static Region getRegion (int xCoord, int yCoord)
	{
		Debug.Log (xCoord + ":" + yCoord);
		for (int i = 0; i < regions.Count; ++i) {
			if (regions [i].myRegionX == xCoord && regions [i].myRegionY == yCoord) {
				return regions [i];
			}
		}
		return null;
	}
	
	public static int[] GetXTEAS (int regionId)
	{
		string[] lines = System.IO.File.ReadAllLines (sign.signlink.findcachedir () + "xteas" + "/" + regionId + ".txt");
		
		int[] xteas = new int[4];
		for (int i = 0; i < xteas.Length; ++i)
			xteas [i] = int.Parse (lines [i]);
		
		return xteas;
	}

	public static void Initialize ()
	{
		if (!initialized) {
			initialized = true;
			LoadMapData ();
		}
	}
	
	public static void InitializeSolo ()
	{
//		if(!initialized)
//		{
//			initialized = true;
//			signlink.storeid = 0;
//			signlink.startpriv(IPAddress.Parse("127.0.0.1"));
//			signlink.run();
//			for (int i = 0; i < 5; i++)
//				client.decompressors[i] = new Decompressor(signlink.cache_dat,
//			  	                                    signlink.cache_idx[i], i + 1);
//			  StreamLoader streamLoader_6 = client.streamLoaderForName2(5, "update list",
//			                                                    "versionlist", client.expectedCRCs[5], 60);
//			client.onDemandFetcher = new OnDemandFetcher();
//			client.onDemandFetcher.start(streamLoader_6, client.instance, false);
//			StreamLoader streamLoader = client.streamLoaderForName2(2, "config",
//			                                                "config", client.expectedCRCs[2], 30);
//			Flo.unpackConfig(streamLoader);
//			OverLayFlo317.unpackConfig(streamLoader);
//			Model.initialize(65535, client.onDemandFetcher);
//			LoadMapData ();
//		}
	}

	public static int[][][] getTileHeight (int rX, int rY)
	{
		bool isLoaded = false;
		foreach (RegionHeightMap rhm in regionHeights) {
			if (rhm.rX == rX && rhm.rY == rY) {
				isLoaded = true;
				break;
			}
		}
		if (!isLoaded)
			PreloadRegion (rX, rY);
		foreach (RegionHeightMap rhm in regionHeights) {
			if (rhm.rX == rX && rhm.rY == rY) {
				if (rhm.preMade != null) {
					return rhm.preMade;
				} else {
					int[][][] newHeightMap = NetDrawingTools.CreateTrippleIntArray (4, 105, 105);
					for (int z = 0; z < 4; ++z) {
						for (int x = 0; x < 64; ++x) {
							for (int y = 0; y < 64; ++y) {
								newHeightMap [z] [x] [y] = rhm.tile_height [z] [x] [y];
							}
						}
					}
					int[][][] heightMapX = null;
					int[][][] heightMapY = null;
					int[][][] heightMapXY = null;

					foreach (RegionHeightMap rhm2 in regionHeights) {
						if (rhm2.rX == rX + 1 && rhm2.rY == rY) {
							heightMapX = rhm2.tile_height;
						}
					}
					if (heightMapX == null) {
						PreloadRegion (rX + 1, rY);
						foreach (RegionHeightMap rhm2 in regionHeights) {
							if (rhm2.rX == rX + 1 && rhm2.rY == rY) {
								heightMapX = rhm2.tile_height;
							}
						}
					}

					foreach (RegionHeightMap rhm2 in regionHeights) {
						if (rhm2.rX == rX && rhm2.rY == rY + 1) {
							heightMapY = rhm2.tile_height;
						}
					}
					if (heightMapY == null) {
						PreloadRegion (rX, rY + 1);
						foreach (RegionHeightMap rhm2 in regionHeights) {
							if (rhm2.rX == rX && rhm2.rY == rY + 1) {
								heightMapY = rhm2.tile_height;
							}
						}
					}

					foreach (RegionHeightMap rhm2 in regionHeights) {
						if (rhm2.rX == rX + 1 && rhm2.rY == rY + 1) {
							heightMapXY = rhm2.tile_height;
						}
					}
					if (heightMapXY == null) {
						PreloadRegion (rX + 1, rY + 1);
						foreach (RegionHeightMap rhm2 in regionHeights) {
							if (rhm2.rX == rX + 1 && rhm2.rY == rY + 1) {
								heightMapXY = rhm2.tile_height;
							}
						}
					}
					if (heightMapXY != null && heightMapX != null && heightMapY != null) {
						for (int z = 0; z < 4; ++z)
							for (int y = 0; y < 64; ++y)
								newHeightMap [z] [64] [y] = heightMapX [z] [0] [y];
						for (int z = 0; z < 4; ++z)
							for (int x = 0; x < 64; ++x)
								newHeightMap [z] [x] [64] = heightMapY [z] [x] [0];
						for (int z = 0; z < 4; ++z)
							newHeightMap [z] [64] [64] = heightMapXY [z] [0] [0];
					}
					rhm.preMade = newHeightMap;
					return rhm.preMade;
				}
			}
		}
		return null;

	}

	public static byte[] getFile (int index, int fileID, bool loadNew = false)
	{
		if (fileID < 0)
			return null;
		if(loadNew) return UnityClient.ReadAllBytes (sign.signlink.findcachedir () + index + " encrypted/" + fileID + ".dat");//*/client.decompressors[index].decompress(fileID);
		return UnityClient.ReadAllBytes (sign.signlink.findcachedir () + index + "/" + fileID + ".dat");//*/client.decompressors[index].decompress(fileID);
	}
	
	public static sbyte[] getFileSigned (int index, int fileID, bool loadNew = false)
	{
		byte[] bytes = getFile(index, fileID, loadNew);
		sbyte[] sbytes = new sbyte[bytes.Length];
		Buffer.BlockCopy(bytes,0,sbytes,0,bytes.Length);
		return sbytes;
	}
	
	public static sbyte[] getMapFile (int fileID)
	{
		byte[] bytes = UnityClient.ReadAllBytes (signlink.osHDDir + "maps/" + fileID + ".dat");
		if(bytes == null) return null;
		sbyte[] sbytes = new sbyte[bytes.Length];
		Buffer.BlockCopy(bytes,0,sbytes,0,bytes.Length);
		return sbytes;
	}
	
	public static void LoadMapData ()
	{
		byte[] abyte2 = OnDemandFetcher.mapIndexData;
		Packet stream2 = new Packet (abyte2);
		int j1 = stream2.readUnsignedWord ();// = 667
		mapIndices1 = new int[j1];
		mapIndices2 = new int[j1];
		mapIndices3 = new int[j1];
		Debug.Log ("Loaded Maps: " + j1 + "");
		for (int i2 = 0; i2 < j1; i2++) {
			mapIndices1 [i2] = stream2.readUnsignedWord ();
			mapIndices2 [i2] = stream2.readUnsignedWord ();
			mapIndices3 [i2] = stream2.readUnsignedWord ();
		}
	}

	public static void PreloadRegion (int xCoord, int yCoord)
	{
		int myRegionX = xCoord; //50
		int myRegionY = yCoord; //50
		int regionbasesX = xCoord * 64; //3200
		int regionbasesY = yCoord * 64; //3200
		xCoord = xCoord * 8; //800
		yCoord = yCoord * 8; //800
		byte[][] localRegionMapData = new byte[1][];
		byte[][] localRegionLandscapeData = new byte[1][];
		int[] localRegionIds = new int[1];
		int[] terrainIndices = new int[1];
		int[] anIntArray1236 = new int[1];
		localRegionIds [0] = ((xCoord) / 8 << 8) + (yCoord) / 8;
		terrainIndices [0] = getMapData (0, (yCoord) / 8, (xCoord) / 8);
		anIntArray1236 [0] = getMapData (1, (yCoord) / 8, (xCoord) / 8);
		localRegionMapData [0] = getFile (5, terrainIndices [0]);
		localRegionLandscapeData [0] = getFile (5, anIntArray1236 [0]);
		if (getFile (5, terrainIndices [0]) == null)
			terrainIndices [0] = -1;
		int dX = (localRegionIds [0] >> 8) * 64 - regionbasesX;
		int dY = (localRegionIds [0] & 0xff) * 64 - regionbasesY;
		byte[] data = localRegionMapData [0];
		if (data == null || data.Length == 0)
			return;
		Packet buffer = new Packet (data);
		int[][][] heightMapToCreate = NetDrawingTools.CreateTrippleIntArray (4, 64, 64);
		for (int z = 0; z < 4; z++) {
			for (int localX = 0; localX < 64; localX++) {
				for (int localY = 0; localY < 64; localY++) {
					decodeMapData (myRegionX, myRegionY, buffer, localX + dX, localY + dY, z, (myRegionX - 6) * 8, (myRegionY - 6) * 8, 0, heightMapToCreate);
				}
			}
		}
		regionHeights.Add (new RegionHeightMap (heightMapToCreate, myRegionX, myRegionY));
	}

	private static void decodeMapData (int myRegionX, int myRegionY, Packet buffer, int x, int y, int z, int regionX, int regionY, int rotation, int[][][] heightMapToCreate)
	{
		int comeOnCount = 0;
		try {
			if (x >= 0 && x < 104 && y >= 0 && y < 104) {
				do {
					comeOnCount++;
					if (comeOnCount > 5000)
						return;
					int type = buffer.readUnsignedByte ();
					if (type == 0) {
						if (z == 0) {
							heightMapToCreate [0] [x] [y] = -calculateHeight (0xe3b7b + x + regionX, 0x87cce + y + regionY) * 8;
						} else {
							heightMapToCreate [z] [x] [y] = heightMapToCreate [z - 1] [x] [y] - 240;
						}
					
						return;
					} else if (type == 1) {
						int height = buffer.readUnsignedByte ();
						if (height == 1) {
							height = 0;
						}
					
						if (z == 0) {
							heightMapToCreate [0] [x] [y] = -height * 8;
						} else {
							heightMapToCreate [z] [x] [y] = heightMapToCreate [z - 1] [x] [y] - height * 8;
						}
					
						return;
					} else if (type <= 49) {
						byte dummy = (byte)buffer.readSignedByte ();
						dummy = (byte)((type - 2) / 4);
						dummy = (byte)(type - 2 + rotation & 3);
					} else if (type <= 81) {
						byte dummy = (byte)(type - 49);
					} else {
						byte dummy = (byte)(type - 81);
					}
				} while (true);
			}
			comeOnCount = 0;
			do {
				comeOnCount++;
				if (comeOnCount > 5000)
					return;
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
		} catch (System.Exception ex) {
			UnityEngine.Debug.Log ("Missing map data probably");
		}
	}

	private static int calculateHeight (int x, int y)
	{
		int height = method176 (x + 45365, y + 0x16713, 4) - 128 + (method176 (x + 10294, y + 37821, 2) - 128 >> 1)
			+ (method176 (x, y, 1) - 128 >> 2);
		height = (int)(height * 0.3D) + 35;
		if (height < 10) {
			height = 10;
		} else if (height > 60) {
			height = 60;
		}
		return height;
	}

	private static int method176 (int i, int j, int k)
	{
		int l = i / k;
		int i1 = i & k - 1;
		int j1 = j / k;
		int k1 = j & k - 1;
		int l1 = method186 (l, j1);
		int i2 = method186 (l + 1, j1);
		int j2 = method186 (l, j1 + 1);
		int k2 = method186 (l + 1, j1 + 1);
		int l2 = method184 (l1, i2, i1, k);
		int i3 = method184 (j2, k2, i1, k);
		return method184 (l2, i3, k1, k);
	}

	private static int method186 (int i, int j)
	{
		int k = method170 (i - 1, j - 1) + method170 (i + 1, j - 1) + method170 (i - 1, j + 1) + method170 (i + 1, j + 1);
		int l = method170 (i - 1, j) + method170 (i + 1, j) + method170 (i, j - 1) + method170 (i, j + 1);
		int i1 = method170 (i, j);
		return k / 16 + l / 8 + i1 / 4;
	}

	private static int method170 (int i, int j)
	{
		int k = i + j * 57;
		k = k << 13 ^ k;
		int l = k * (k * k * 15731 + 0xc0ae5) + 0x5208dd0d & 0x7fffffff;
		return l >> 19 & 0xff;
	}
	
	private static int method184 (int i, int j, int k, int l)
	{
		int i1 = 0x10000 - RS2Sharp.Texture.COSINE [k * 1024 / l] >> 1;
		return (i * (0x10000 - i1) >> 16) + (j * i1 >> 16);
	}

	public static int getMapData (int i, int k, int l)
	{
		int i1 = (l << 8) + k;
		for (int j1 = 0; j1 < mapIndices1.Length; j1++) {
			if (mapIndices1 [j1] == i1) {
				if (i == 0)
					return mapIndices2 [j1];
				else
					return mapIndices3 [j1];
			}
		}
		return -1;
	}

	public static void LoadRegion (int xCoord, int yCoord)
	{
		GameObject regionContainer = new GameObject ("Region " + xCoord + " " + yCoord);
		Region region = new Region (regionContainer);
		activeRegions.Add (region);
		
		//try{
		region.LoadRegion (xCoord, yCoord);
		//}catch(System.Exception ex)
		//{
		//	UnityEngine.Debug.Log(ex.Message);
		//}
		regions.Add (region);
	}

	public static Region GetRegion (int x, int z)
	{
		InitializeSolo ();
		Region region = null;
		LoadRegion (x, z);
		for (int i = 0; i < regions.Count; ++i) {
			if (regions [i].myRegionX == x && regions [i].myRegionY == z) {
				region = regions [i];
				break;
			}
		}
		return region;
	}

	public static void DrawRegion (int xCoord, int yCoord, bool isInitial = false)
	{
		LoadRegion (xCoord, yCoord);
		Region region = null;
		for (int i = 0; i < regions.Count; ++i) {
			if (regions [i].myRegionX == xCoord && regions [i].myRegionY == yCoord) {
				region = regions [i];
				break;
			}
		}
		
		GameObject regionContainer = region.regionContainer;

		//GameObject regionObj = null;//Resources.Load("Regions/Prefabs/Region " + xCoord + " " + yCoord) as GameObject;
		regionContainer.transform.position = new Vector3 (xCoord, 0, yCoord);
		//	regionContainer.name = regionContainer.name.Replace("(Clone)","");
		DrawTerrain (region, regionContainer);
		DrawObjects (region, regionContainer);
		//MakeRegionStatic (regionContainer);
	}
	
	public static void DrawObjects (Region region, GameObject regionContainer)
	{
		region.DrawObjects (regionContainer);
	}
	
	public static bool containsRegion (int x, int y)
	{
		foreach (Region go in activeRegions)
			if (go.regionContainer.name == "Region " + x + " " + y)
				return true;
		return false;
	}

	public static void MakeRegionStatic (GameObject regionContainer)
	{
//		List<GameObject> staticChildren = new List<GameObject> ();
//		List<Mesh> meshes = new List<Mesh> ();
//		for (int i = 0; i < regionContainer.transform.childCount; ++i) {
//			//if (!regionContainer.transform.GetChild (i).gameObject.isStatic)
//			//	regionContainer.transform.GetChild (i).gameObject.isStatic = true;
//			if (!regionContainer.transform.GetChild (i).GetComponent<Terrain> () && !regionContainer.transform.GetChild (i).name.Contains("Water") && !regionContainer.transform.GetChild (i).name.Contains("Grass")) {
//				staticChildren.Add (regionContainer.transform.GetChild (i).gameObject);
//				meshes.Add (regionContainer.transform.GetChild (i).GetComponent<MeshFilter> ().sharedMesh);
//			}
//		}
//		StaticBatchingUtility.Combine (staticChildren.ToArray (), regionContainer);
//		//foreach (Mesh mesh in meshes)
//		//	GameObject.DestroyImmediate (mesh, true);
//		System.GC.Collect ();
//		Resources.UnloadUnusedAssets ();



	}

	public static void DrawTerrain (Region region, GameObject regionContainer)
	{
		
		RSTerrain rsTerrain = region.rsTerrain = new RSTerrain (region);
		rsTerrain.generateNormalMap ();
		List<GameObject> overlays = null;
		overlays = rsTerrain.GenerateOverlays (regionContainer);
		GameObject terrain = rsTerrain.generateTerrainLayer (regionContainer, region.myRegionX, region.myRegionY);
		regionContainer.transform.position = new Vector3 (32, 0, 32);
		terrain.transform.parent = regionContainer.transform;
		terrain.transform.localPosition = new Vector3 (-0.5f, 0, -0.5f);
		regionContainer.transform.position = new Vector3 ((region.myRegionX * 64) + 32f, 0, (region.myRegionY * 64) + 32f);

		foreach (GameObject go in overlays) {
			go.transform.parent = regionContainer.transform;
			go.transform.localPosition = new Vector3 (-0.5f, go.transform.localPosition.y, -0.5f);
		}
		regionContainer.transform.position = new Vector3 ((region.myRegionX * 64), 0, (region.myRegionY * 64));
		//regionContainer.transform.parent = UnityClient.objectRoot.transform;
	}
	
}