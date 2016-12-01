using UnityEngine;
using System.Collections;
using UnityEditor;
using RS2Sharp;
using System.IO;
using System.Collections.Generic;
using System;

public class RuneEditor{


static bool NotBlack(Color32 color)
{
	if(color.r == 0 && color.g == 0 && color.b == 0) return false;
	return true;
}

[MenuItem ("Runescape/Grab Textures Used")]
static void GetRuneTextures()
{
		int lowerRegion = 0;
		int upperRegion = 100;

        for (int regionX = lowerRegion; regionX < upperRegion; regionX++)
        {
            for (int regionZ = lowerRegion; regionZ < upperRegion; regionZ++)
            {
                Region region = MapManager.GetRegion(regionX,regionZ);
                
                Color32[][] colourMap = NetDrawingTools.CreateDoubleColor32Array (105, 105);
                int[][] underlaysRgb = region.tile_layer0_colour [0];
				int[,] overlayTextures = new int[105, 105];
				int[][] textureMap = new int[105][];//104];
				byte[][] underlays = region.tile_layer0_type [0];
				sbyte[][] overlays = region.tile_layer1_type [0];
				textureMap = NetDrawingTools.CreateDoubleIntArray (105, 105);
				for (int x = 0; x < 104; x++)
			for (int z = 0; z < 104; z++) {
				if (underlays [x] [z] == 0)
					underlays [x] [z] = 1;
				OverLayFlo317 underlay = OverLayFlo317.overLayFlo317s [underlays [x] [z] - 1];
				//Floor underlay = Floor.floors[underlays [x] [z] - 1];
				OverLayFlo317 overLay = null;
				int overLayId = overlays [x] [z] & 0xff;
				if (overLayId >= 1 && overLayId - 1 < OverLayFlo317.overLayFlo317s.Length)
					overLay = OverLayFlo317.overLayFlo317s [overLayId - 1];
				int underlayRgb = underlaysRgb [x] [z];
				colourMap [x] [z] = fromRgb (underlayRgb, 255);//new Color32(underlayRgb >> 16,underlayRgb >> 8,underlayRgb & 0xFF,255);
				textureMap [x] [z] = underlay.textureId;//texture;
				if (overLay != null) {
					overlayTextures [x, z] = overLay.textureId;
				}
			}
			List<int> texIds = new List<int>();
              for (int x = 0; x < 64; ++x) {
			for (int y = 0; y < 64; ++y) {
				int overlayTexture = overlayTextures [x, y];
				int underlayTexture = textureMap [x] [y];
				if(overlayTexture > 0)
				{
					if(!texIds.Contains(overlayTexture)) texIds.Add(overlayTexture);
				}
				if(underlayTexture > 0)
				{
					if(!texIds.Contains(underlayTexture)) texIds.Add(underlayTexture);
				}
            }
			  }
			 foreach (int tex in texIds)
			 {
				 Texture2D alreadyThere = Resources.Load("Textures/Rune Textures/" + tex) as Texture2D;
					if(alreadyThere == null)
					{
						Texture2D textureToGrab = UnityClient.LoadTexture (tex);
						Color32[] colorbytes = textureToGrab.GetPixels32();
						for(int i = 0; i < colorbytes.Length; ++i)
							colorbytes[i] = new Color32((byte)(colorbytes[i].r * 0.5f),(byte)(colorbytes[i].g * 0.5f),(byte)(colorbytes[i].b * 0.5f),(byte)(colorbytes[i].a));
						textureToGrab.SetPixels32(colorbytes);
						textureToGrab.Apply();
						byte[] bytes = textureToGrab.EncodeToPNG();
						File.WriteAllBytes(Application.dataPath + "/../Assets/Resources/Textures/Rune Textures/" + tex + ".png", bytes);
						File.WriteAllBytes(Application.dataPath + "/../Assets/Resources/Textures/Rune Textures/" + tex + " Bump.png", bytes);
					
					}
			 }
        }
		}
}

	[MenuItem ("Runescape/Generate TerrainData")]
	static void GenTerrainData() {
		int lowerRegion = 0;
		int upperRegion = 96;
        for (int regionX = 32; regionX < 65; regionX++)
        {
            for (int regionZ = lowerRegion; regionZ < upperRegion; regionZ++)
            {
                Region region = MapManager.GetRegion(regionX,regionZ);
	int[][] textureMap = new int[105][];//104];
	Color32[][] colourMap = new Color32[105][];//[104];
	int[][] heightMap = new int[105][];//[104];
	int heightLevel = 0;
	int[,] overlayTextures = new int[105, 105];
	Vector3[][] normalMap;
	
	textureMap = NetDrawingTools.CreateDoubleIntArray (105, 105);
		colourMap = NetDrawingTools.CreateDoubleColor32Array (105, 105);
		heightMap = NetDrawingTools.CreateDoubleIntArray (104, 104);
		byte[][] underlays = region.tile_layer0_type [heightLevel];
		sbyte[][] overlays = region.tile_layer1_type [heightLevel];
		int[][] underlaysRgb = region.tile_layer0_colour [heightLevel];
		int[][][] tileH = MapManager.getTileHeight (region.myRegionX, region.myRegionY);
		if (tileH == null)
			tileH = NetDrawingTools.CreateTrippleIntArray (4, 104, 104);
		int[][] heightmap = tileH [heightLevel];
		for (int x = 0; x < 104; x++)
			for (int z = 0; z < 104; z++) {
				if (underlays [x] [z] == 0)
					underlays [x] [z] = 1;
				OverLayFlo317 underlay = OverLayFlo317.overLayFlo317s [underlays [x] [z] - 1];
				//Floor underlay = Floor.floors[underlays [x] [z] - 1];
				OverLayFlo317 overLay = null;
				int overLayId = overlays [x] [z] & 0xff;
				if (overLayId >= 1 && overLayId - 1 < OverLayFlo317.overLayFlo317s.Length)
					overLay = OverLayFlo317.overLayFlo317s [overLayId - 1];
				int underlayRgb = underlaysRgb [x] [z];
				colourMap [x] [z] = fromRgb (underlayRgb, 255);//new Color32(underlayRgb >> 16,underlayRgb >> 8,underlayRgb & 0xFF,255);
				textureMap [x] [z] = underlay.textureId;//texture;
				heightMap [x] [z] = (((!(region.tile_layer1_type [heightLevel] [x] [z] == 6 && region.tile_layer1_shape [heightLevel] [x] [z] != 1)) || heightmap [x] [z] != 0) ? -heightmap [x] [z] : -200) - 9;
				if (overLay != null) {
					overlayTextures [x, z] = overLay.textureId;
				}
			}
	
                float[,] heights = new float[65, 65];
		for (int x = 0; x < 65; ++x) {
			for (int y = 0; y < 65; ++y) {
				int localX = Mathf.FloorToInt ((x / 1f));
				int localY = Mathf.FloorToInt ((y / 1f));
				heights [x, y] = heightMap [localY] [localX] / 2048.0f;
			}
		}

		List<SplatPrototype> splatPrototypes = new List<SplatPrototype> ();
		List<int> splatPrototypeIds = new List<int> ();
		TerrainData terrainData = new TerrainData ();
		terrainData.heightmapResolution = 64;
		terrainData.SetHeights (0, 0, heights);
		terrainData.alphamapResolution = 64;
		terrainData.baseMapResolution = 64;

		//		File.WriteAllBytes(Application.dataPath + "/../Assets/Resources/Maps/Colormaps/"+ "Regions X-" + xOffset + "-" + (xOffset+32) + " Y-" + yOffset + "-" + (yOffset + 32) +".png", bytes);
				int xOff = Mathf.FloorToInt(region.myRegionX/32)*32;
				int yOff = Mathf.FloorToInt(region.myRegionY/32)*32;
				string colormapId = "Regions X-" + xOff  + "-" + (Mathf.FloorToInt(region.myRegionX/32) == 0 ? 32 : Mathf.FloorToInt(region.myRegionX/32) == 1 ? 64 : Mathf.FloorToInt(region.myRegionX/32) == 2 ? 96 : 0) + " Y-" + yOff  + "-" + (Mathf.FloorToInt(region.myRegionY/32) == 0 ? 32 : Mathf.FloorToInt(region.myRegionY/32) == 1 ? 64 : Mathf.FloorToInt(region.myRegionY/32) == 2 ? 96 : 0);				Debug.Log (colormapId);
				Texture2D overlaySplat = Resources.Load("Maps/Colormaps/"+colormapId) as Texture2D;//new Texture2D (64, 64);
		for (int x = 0; x < 64; ++x) {
			for (int y = 0; y < 64; ++y) {
				Color32 color = colourMap [x] [y];
				Color newColor = new Color (color.r / 255f, color.g / 255f, color.b / 255f, color.a / 255f);
				//overlaySplat.SetPixel (x, y, newColor);
				int overlayTexture = overlayTextures [x, y];
				int underlayTexture = textureMap [x] [y];
				if(overlayTexture > 0 && overlayTexture != 669 && overlayTexture != 651)
				{
					if(!splatPrototypeIds.Contains(overlayTexture))
					{
						SplatPrototype splatToAdd = new SplatPrototype();

						Texture2D colorTexture = UnityClient.LoadTexture (overlayTexture);
						Texture2D normal = UnityClient.CreateDOT3 (colorTexture, 40f, 0f, overlayTexture);
						splatToAdd.texture = colorTexture;
						splatToAdd.normalMap = normal;
						splatToAdd.tileSize = new Vector2 (1, 1);
						splatToAdd.smoothness = 0.5f;
						splatToAdd.metallic = 0f;
						splatPrototypes.Add(splatToAdd);
						splatPrototypeIds.Add(overlayTexture);
					}
				}
				if(underlayTexture > 0 && underlayTexture != 669 && underlayTexture != 651)
				{
					if(!splatPrototypeIds.Contains(underlayTexture))
					{
						SplatPrototype splatToAdd = new SplatPrototype();

						Texture2D colorTexture = UnityClient.LoadTexture (underlayTexture);	
						Texture2D normal = normal = UnityClient.CreateDOT3 (colorTexture, 40f, 0f, underlayTexture);
						splatToAdd.texture = colorTexture;
						splatToAdd.normalMap = normal;
						splatToAdd.tileSize = new Vector2 (1, 1);
						splatToAdd.smoothness = 0.5f;
						splatToAdd.metallic = 0f;
						splatPrototypes.Add(splatToAdd);
						splatPrototypeIds.Add(underlayTexture);
					}
				}
			}
		}
		//overlaySplat.filterMode = FilterMode.Point;
		//overlaySplat.Apply ();
		//overlaySplat = FastBlur (overlaySplat, 4, 1);
		//overlaySplat.Apply ();
		SplatPrototype aSplat = new SplatPrototype ();
		string regionName = "Region " + region.myRegionX + " " + region.myRegionY;
		
		int myRegionX = region.myRegionX;
		int myRegionY = region.myRegionY;
		int offsetX = (myRegionX - xOff) * 64;
		int offsetY = (myRegionY - yOff) * 64;
		
		aSplat.texture = overlaySplat;

		aSplat.tileSize = new Vector2 (2048, 2048);
		aSplat.tileOffset = new Vector2(offsetX, offsetY);
		aSplat.smoothness = 0f;
		splatPrototypes.Insert (0, aSplat);
		splatPrototypeIds.Insert (0, 0);

		terrainData.splatPrototypes = splatPrototypes.ToArray ();
		float[, ,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];
		for (int x = 0; x < terrainData.alphamapWidth; ++x) {
			for (int y = 0; y < terrainData.alphamapHeight; ++y) {
				splatmapData[x,y,0] = 1;//1;
			}
		}

		for (int x = 0; x < terrainData.alphamapWidth; ++x) {
			for (int y = 0; y < terrainData.alphamapHeight; ++y) {
				int overlayTexture = overlayTextures [y, x];
				int underlayTexture = textureMap [y] [x];
				
				if(underlayTexture > 0)
				{
					splatmapData[x,y,0] = 0.5f;
					int idx = 0;
					for(int i = 0; i < terrainData.splatPrototypes.Length; ++i)
					{
						SplatPrototype item = terrainData.splatPrototypes[i];
						if(item.texture.name == ""+underlayTexture)
						{
							idx = i;
							break;
						}
					}
					splatmapData[x,y,idx] = 0.5f;
				}
				
				if(overlayTexture > 0)
				{
					for(int i = 0; i < terrainData.alphamapLayers; ++i)
						splatmapData[x,y,i ] = 0f;
						
					int idx = 0;
					for(int i = 0; i < terrainData.splatPrototypes.Length; ++i)
					{
						SplatPrototype item = terrainData.splatPrototypes[i];
						if(item.texture.name == ""+overlayTexture)
						{
							idx = i;
							break;
						}
					}
					splatmapData[x,y,idx] = 1f;
				}
				
			}
		}

		terrainData.SetAlphamaps (0, 0, splatmapData);

		terrainData.size = new Vector3 (64, 16.54f, 64);//4.14
		//GameObject terrain = Terrain.CreateTerrainGameObject (terrainData);
		//terrain.GetComponent<Terrain> ().basemapDistance = 35;
		//terrain.GetComponent<Terrain> ().materialType = Terrain.MaterialType.Custom;
		//		terrain.GetComponent<Terrain> ().materialTemplate = GameObject.Find ("ClientCamera").GetComponent<UnityClient>().terrainMatToApply;
		//terrain.GetComponent<Terrain> ().reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Simple;
		terrainData.wavingGrassSpeed = 0.219f;
		terrainData.wavingGrassStrength = 0.686f;
		terrainData.wavingGrassAmount = 0.106f;
		terrainData.SetDetailResolution (64, 8);
		//terrain.layer = LayerMask.NameToLayer ("Terrain");
               
				//Save Terrain
				string baseFolder = "Assets/Resources/Maps/TerrainData/";
				TerrainData dataToSave = terrainData;
				SplatPrototype[] splat = dataToSave.splatPrototypes;
				float[,,] alphaMaps = dataToSave.GetAlphamaps (0, 0, 64, 64);
				AssetDatabase.CreateAsset (dataToSave, baseFolder + "TerrainData " + region.myRegionX + " " + region.myRegionY + ".asset");
				AssetDatabase.SaveAssets ();
				TerrainData dataLoaded = (TerrainData)AssetDatabase.LoadAssetAtPath (baseFolder + "TerrainData " + region.myRegionX + " " + region.myRegionY + ".asset", typeof(TerrainData));
				dataLoaded.splatPrototypes = splat;
				dataLoaded.SetAlphamaps (0, 0, alphaMaps);
				dataLoaded.RefreshPrototypes ();
				AssetDatabase.SaveAssets ();
				AssetDatabase.Refresh ();
            }
        }
		
       
		Debug.Log ("Finished Generating " + ((upperRegion-lowerRegion)*(upperRegion-lowerRegion)) + " TerrainData's");
		
	}

	[MenuItem ("Runescape/Generate Colormaps")]
	static void GenColorMaps () {
		Debug.Log ("Generating Color Maps");
		for(int xOffset = 32; xOffset < 92; xOffset += 32)
		{
			for(int yOffset = 64; yOffset < 92; yOffset += 32)
			{
		Texture2D overlaySplat = new Texture2D (32*64, 32*64);
		for(int regionX = xOffset; regionX < xOffset+32; ++regionX)
		{
			for(int regionY = yOffset; regionY < yOffset+32; ++regionY)
			{
				Region region = MapManager.GetRegion(regionX,regionY);
				Color32[][] colourMap = NetDrawingTools.CreateDoubleColor32Array (105, 105);
				int[][] underlaysRgb = region.tile_layer0_colour [0];
				int averageR = 0;
				int averageG = 0;
				int averageB = 0;
				int averageCount = 0;
				Color32 averageColor = new Color32(0,0,0,255);
				for (int x = 0; x < 104; x++)
					for (int z = 0; z < 104; z++)
				{
					colourMap [x] [z] = fromRgb (underlaysRgb [x] [z], 255);//new Color32(underlayRgb >> 16,underlayRgb >> 8,underlayRgb & 0xFF,255);
					if( NotBlack(colourMap [x] [z]))
					{
						averageCount++;
						averageR += colourMap [x] [z].r;
						averageG += colourMap [x] [z].g;
						averageB += colourMap [x] [z].b;
					}
				} 
				if(averageCount > 0)
					averageColor = new Color32((byte)(averageR/averageCount),(byte)(averageG/averageCount),(byte)(averageB/averageCount),255);
				for (int x = 0; x < 64; ++x)
					for (int y = 0; y < 64; ++y)
						overlaySplat.SetPixel (x+((regionX)*64), y+((regionY)*64), NotBlack(colourMap [x] [y]) ? colourMap [x] [y] : averageColor);
				
			}
		}
		overlaySplat.Apply();
		overlaySplat = FastBlur (overlaySplat, 4, 1);
		overlaySplat.Apply ();
		byte[] bytes = overlaySplat.EncodeToPNG();
		File.WriteAllBytes(Application.dataPath + "/../Assets/Resources/Maps/Colormaps/"+ "Regions X-" + xOffset + "-" + (xOffset+32) + " Y-" + yOffset + "-" + (yOffset + 32) +".png", bytes);
		}
		return;
		}
//		int lowerRegion = 0;
//		int upperRegion = 32;
//        Texture2D overlaySplat = new Texture2D ((upperRegion - lowerRegion)*64, (upperRegion - lowerRegion)*64);
//        for (int regionX = lowerRegion; regionX < upperRegion; regionX++)
//        {
//            for (int regionZ = lowerRegion; regionZ < upperRegion; regionZ++)
//            {
//                Region region = MapManager.GetRegion(regionX,regionZ);
//                Color32[][] colourMap = NetDrawingTools.CreateDoubleColor32Array (105, 105);
//                int[][] underlaysRgb = region.tile_layer0_colour [0];
//				int averageR = 0;
//				int averageG = 0;
//				int averageB = 0;
//				int averageCount = 0;
//				Color32 averageColor = new Color32(0,0,0,255);
//                for (int x = 0; x < 104; x++)
//			         for (int z = 0; z < 104; z++)
//					 {
//				        colourMap [x] [z] = fromRgb (underlaysRgb [x] [z], 255);//new Color32(underlayRgb >> 16,underlayRgb >> 8,underlayRgb & 0xFF,255);
//					 	if( NotBlack(colourMap [x] [z]))
//						{
//							 averageCount++;
//						 	averageR +=  colourMap [x] [z].r;
//							 averageG +=  colourMap [x] [z].g;
//							 averageB +=  colourMap [x] [z].b;
//						 }
//					 } 
//				if(averageCount > 0)
//				averageColor = new Color32((byte)(averageR/averageCount),(byte)(averageG/averageCount),(byte)(averageB/averageCount),255);
//                for (int x = 0; x < 64; ++x)
//			         for (int y = 0; y < 64; ++y)
//				        overlaySplat.SetPixel (x+((regionX-40)*64), y+((regionZ-40)*64), NotBlack(colourMap [x] [y]) ? colourMap [x] [y] : averageColor);
//
//            }
//        }
//        overlaySplat.Apply();
//		overlaySplat = FastBlur (overlaySplat, 4, 1);
//		overlaySplat.Apply ();
//		for (int regionX = lowerRegion; regionX < upperRegion; regionX++)
//        {
//            for (int regionZ = lowerRegion; regionZ < upperRegion; regionZ++)
//            {
//            	Texture2D thisRegion = new Texture2D(64,64);
//				int xOffset = (regionX-lowerRegion)*64;
//				int yOffset = (regionZ-lowerRegion)*64;
//				for(int localX = 0; localX < 64; ++localX)
//				{
//					for(int localZ = 0; localZ < 64; ++localZ)
//					{
//						thisRegion.SetPixel(localX,localZ,overlaySplat.GetPixel(localX+xOffset,localZ+yOffset));
//					}
//				}
//				thisRegion.Apply();
//				byte[] bytes2 = thisRegion.EncodeToPNG();
//				File.WriteAllBytes(Application.dataPath + "/../Assets/Resources/Maps/Colormaps/" + regionX + " - " + regionZ + ".png", bytes2);
//            }
//        }
//        byte[] bytes = overlaySplat.EncodeToPNG();
//		File.WriteAllBytes(Application.dataPath + "/../Assets/Resources/Maps/Colormaps/OverallMap.png", bytes);
//		Debug.Log ("Finished Generating " + ((upperRegion-lowerRegion)*(upperRegion-lowerRegion)) + " Color Maps");
	}
	
	[MenuItem ("Runescape/Generate Unity Mesh's")]
	static void GenUnityMeshs () {
		Debug.Log ("Generating Unity Mesh's");
		MapManager.InitializeSolo();
		RS2Sharp.Texture.anIntArray1468 = new int[512];
		RS2Sharp.Texture.anIntArray1469 = new int[2048];
		RS2Sharp.Texture.SINE = new int[2048];
		RS2Sharp.Texture.COSINE = new int[2048];
		for(int i = 1; i < 512; i++)
			RS2Sharp.Texture.anIntArray1468[i] = 32768 / i;
		
		for(int j = 1; j < 2048; j++)
			RS2Sharp.Texture.anIntArray1469[j] = 0x10000 / j;
		
		for(int k = 0; k < 2048; k++)
		{
			RS2Sharp.Texture.SINE[k] = (int)(65536D * System.Math.Sin((double)k * 0.0030679614999999999D));
			RS2Sharp.Texture.COSINE[k] = (int)(65536D * System.Math.Cos((double)k * 0.0030679614999999999D));
		}
		RS2Sharp.Texture.method372(0.80000000000000004D);
		byte[] gzipInputBuffer = new byte[0];
for(int modelId = 0; modelId < 1000; ++modelId)
{

			byte[] modelData = UnityClient.decompressors[1].decompress(modelId);
		if(modelData != null) {
			gzipInputBuffer = new byte[modelData.Length*100];
		int i2 = 0;
		try {
			Ionic.Zlib.GZipStream gzipinputstream = new Ionic.Zlib.GZipStream(
				new MemoryStream(modelData), Ionic.Zlib.CompressionMode.Decompress);
			do {
				if (i2 == gzipInputBuffer.Length)
					throw new Exception("buffer overflow!");
				int k = gzipinputstream.Read(gzipInputBuffer, i2,
				                             gzipInputBuffer.Length - i2);
				if (k == 0)
					break;
				i2 += k;
			} while (true);
		} catch (IOException _ex) {
			throw new Exception("error unzipping");
		}
		modelData = new byte[i2];
		System.Array.Copy(gzipInputBuffer, 0, modelData, 0, i2);
		
		Model model = new Model(modelData,modelId);

		RuneMesh rMesh = new RuneMesh();
		rMesh.Fill (model,true);

		Mesh mesh = new Mesh();
		rMesh.Render(mesh,false);

		if(mesh.vertexCount > 0)
		{
			AssetDatabase.CreateAsset( mesh, "Assets/Resources/Meshes/" + modelId + ".asset");

		}
	}
			AssetDatabase.SaveAssets();
		}
	}
    
    static Color32 ToColor (int HexVal, int a)
	{
		byte R = (byte)((HexVal >> 16) & 0xFF);
		byte G = (byte)((HexVal >> 8) & 0xFF);
		byte B = (byte)((HexVal) & 0xFF);
		return new Color32 (R, G, B, (byte)a);
	}
	
	static Color32 fromRgb (int rgb, int a)
	{
		return ToColor (rgb, a);
	}
	
	static Texture2D FastBlur(Texture2D image, int radius, int iterations){
		
		Texture2D tex = image;
		
		for (var i = 0; i < iterations; i++) {
			
			tex = BlurImage(tex, radius, true);
			tex = BlurImage(tex, radius, false);
		}
		
		return tex;
	}
	private static float avgR = 0;
	private static float avgG = 0;
	private static float avgB = 0;
	private static float avgA = 0;
	private static float blurPixelCount = 0;
	static Texture2D BlurImage(Texture2D image, int blurSize, bool horizontal){
		
		Texture2D blurred = new Texture2D(image.width, image.height);
		int _W = image.width;
		int _H = image.height;
		int xx, yy, x, y;
		
		if (horizontal) {
			
			for (yy = 0; yy < _H; yy++) {
				
				for (xx = 0; xx < _W; xx++) {
					
					ResetPixel();
					
					//Right side of pixel
					for (x = xx; (x < xx + blurSize && x < _W); x++) {
						
						AddPixel(image.GetPixel(x, yy));
					}
					
					//Left side of pixel
					for (x = xx; (x > xx - blurSize && x > 0); x--) {
						
						AddPixel(image.GetPixel(x, yy));
					}
					
					CalcPixel();
					
					for (x = xx; x < xx + blurSize && x < _W; x++) {
						
						blurred.SetPixel(x, yy, new Color(avgR, avgG, avgB, 1.0f));
					}
				}
			}
		}
		
		else {
			
			for (xx = 0; xx < _W; xx++) {
				
				for (yy = 0; yy < _H; yy++) {
					
					ResetPixel();
					
					//Over pixel
					for (y = yy; (y < yy + blurSize && y < _H); y++) {
						
						AddPixel(image.GetPixel(xx, y));
					}
					
					//Under pixel
					for (y = yy; (y > yy - blurSize && y > 0); y--) {
						
						AddPixel(image.GetPixel(xx, y));
					}
					
					CalcPixel();
					
					for (y = yy; y < yy + blurSize && y < _H; y++) {
						
						blurred.SetPixel(xx, y, new Color(avgR, avgG, avgB, 1.0f));
					}
				}
			}
		}
		
		blurred.Apply();
		return blurred;
	}
	
	static void AddPixel(Color pixel) {
		
		avgR += pixel.r;
		avgG += pixel.g;
		avgB += pixel.b;
		blurPixelCount++;
	}
	
	static void ResetPixel() {
		
		avgR = 0.0f;
		avgG = 0.0f;
		avgB = 0.0f;
		blurPixelCount = 0;
	}
	
	static void CalcPixel() {
		
		avgR = avgR / blurPixelCount;
		avgG = avgG / blurPixelCount;
		avgB = avgB / blurPixelCount;
	}
    
}