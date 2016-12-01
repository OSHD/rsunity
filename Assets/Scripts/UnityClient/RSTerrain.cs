using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using RS2Sharp;

public class RSTerrain
{
	private Region region;
	private int[][] textureMap = new int[105][];//104];
	private Color32[][] colourMap = new Color32[105][];//[104];
	private int[][] heightMap = new int[105][];//[104];
	private int heightLevel = 0;
	private Vector3[][] normalMap;
	public List<MeshFilter> waterMeshList = new List<MeshFilter> ();
	private List<Vector3> _vertices = new List<Vector3> ();//[this.vertex_count];
	private List<Vector3> _normals = new List<Vector3> ();//[this.vertex_count];
	private List<Vector2> _uv = new List<Vector2> ();//[this.vertex_count];
	private List<Color32> _colors = new List<Color32> ();//[this.vertex_count];
	private List<List<int>> _triangles = new List<List<int>> ();//[this.triangle_count * 3];
	private int vertexCount = 0;
	private int[,] overlayTextures = new int[105, 105];
	private RSTerrainOverlay[] overlays = new RSTerrainOverlay[4];
	
	public Color32 ToColor (int HexVal, int a)
	{
		byte R = (byte)((HexVal >> 16) & 0xFF);
		byte G = (byte)((HexVal >> 8) & 0xFF);
		byte B = (byte)((HexVal) & 0xFF);
		return new Color32 (R, G, B, (byte)a);
	}
	
	private Color32 fromRgb (int rgb, int a)
	{
		return ToColor (rgb, a);
	}
	
	public RSTerrain (Region region)
	{
		this.region = region;
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
	}
	
	public void generateNormalMap ()
	{
//		int[][] hm_ = heightMap;
//		int side = 104;
//		normalMap = NetDrawingTools.CreateDoubleVector3Array (side, side);//new Vector3[side][side];
//		int tileZ;
//		int tileX;
//		for (tileZ = 0; tileZ < side; tileZ++)
//			for (tileX = 0; tileX < side; tileX++)
//				normalMap [tileX] [tileZ] = new Vector3 (0, 1, 0);
//		for (tileZ = 1; tileZ < side - 1; tileZ++) {
//			for (tileX = 1; tileX < side - 1; tileX++) {
//				int diff1 = hm_ [tileX + 1] [tileZ] - hm_ [tileX - 1] [tileZ];
//				int diff2 = hm_ [tileX] [tileZ + 1] - hm_ [tileX] [tileZ - 1];
//				float div = (float)RuneMath.Sqrt ((float)((double)(diff2 * diff2 + 65536 + diff1 * diff1)));
//				normalMap [tileX] [tileZ] = new Vector3 ((float)diff1 / div, 256.0F / div, (float)diff2 / div);
//			}
//		}
	}
	
	public GameObject generateTerrainLayer (GameObject regionContainer, int myRegionX, int myRegionY)
	{
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
		
		TerrainData terrainData = new TerrainData ();//Resources.Load("Maps/TerrainData/TerrainData " + myRegionX + " " + myRegionY) as TerrainData;//new TerrainData ();
//		if(terrainData == null)
//		{
//		Debug.Log ("Need to export terrain : " + myRegionX + " " + myRegionY);
//		return new GameObject("Null Terrain");
//		}
		
		  terrainData.heightmapResolution = 64;
		  terrainData.SetHeights (0, 0, heights);
		  terrainData.alphamapResolution = 64;
		  terrainData.baseMapResolution = 64;

		Texture2D overlaySplat = new Texture2D (64, 64);//Resources.Load("Maps/Colormaps/"+region.myRegionX +" - " + region.myRegionY ) as Texture2D;//new Texture2D (64, 64);
		  for (int x = 0; x < 64; ++x) {
		  	for (int y = 0; y < 64; ++y) {
		  		Color32 color = Brighten (colourMap [x] [y]);
		  		Color newColor = new Color (color.r / 255f, color.g / 255f, color.b / 255f, color.a / 255f);
		  		overlaySplat.SetPixel (x, y, newColor);
		  		int overlayTexture = overlayTextures [x, y];
		  		int underlayTexture = textureMap [x] [y];
		  		if(overlayTexture > 0 && overlayTexture != 669 && overlayTexture != 651)
		  		{
		  			if(!splatPrototypeIds.Contains(overlayTexture))
		  			{
		  				SplatPrototype splatToAdd = new SplatPrototype();
						Texture2D colorTexture = UnityClient.LoadTexture (overlayTexture);
						if(colorTexture != null)
						{
		  				Texture2D normal = UnityClient.CreateDOT3 (colorTexture, 40f, 0f, overlayTexture);
		  				splatToAdd.texture = colorTexture;
						if(normal != null) splatToAdd.normalMap = normal;
		  				splatToAdd.tileSize = new Vector2 (1, 1);
		  				splatToAdd.smoothness = 0.5f;
		  				splatToAdd.metallic = 0f;
		  				splatPrototypes.Add(splatToAdd);
		  				splatPrototypeIds.Add(overlayTexture);
		  				}
		  			}
		  		}
		  		if(underlayTexture > 0 && underlayTexture != 669 && underlayTexture != 651)
		  		{
		  			if(!splatPrototypeIds.Contains(underlayTexture))
		  			{
		  				SplatPrototype splatToAdd = new SplatPrototype();

		  				Texture2D colorTexture = UnityClient.LoadTexture (underlayTexture);	
		  				if(colorTexture != null)
		  				{
		  				Texture2D normal = UnityClient.CreateDOT3 (colorTexture, 40f, 0f, underlayTexture);
		  				splatToAdd.texture = colorTexture;
		  				if(normal != null) splatToAdd.normalMap = normal;
		  				splatToAdd.tileSize = new Vector2 (1, 1);
		  				splatToAdd.smoothness = 0.5f;
		  				splatToAdd.metallic = 0f;
		  				splatPrototypes.Add(splatToAdd);
		  				splatPrototypeIds.Add(underlayTexture);
		  				}
		  			}
		  		}
		  	}
		  }
		  overlaySplat.filterMode = FilterMode.Point;
		  overlaySplat.Apply ();
		  overlaySplat = FastBlur (overlaySplat, 4, 1);
		  overlaySplat.Apply ();
		  SplatPrototype aSplat = new SplatPrototype ();
		  string regionName = regionContainer.name.Replace("Region ","");
		  aSplat.texture = overlaySplat;

		  aSplat.tileSize = new Vector2 (64, 64);
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
		GameObject terrain = Terrain.CreateTerrainGameObject (terrainData);
		terrain.GetComponent<Terrain> ().basemapDistance = 35;
		terrain.GetComponent<Terrain> ().materialType = Terrain.MaterialType.Custom;
		terrain.GetComponent<Terrain> ().materialTemplate = UnityClient.terrainMat;
		terrain.GetComponent<Terrain> ().reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Simple;
		terrain.GetComponent<Terrain> ().terrainData.wavingGrassSpeed = 0.219f;
		terrain.GetComponent<Terrain> ().terrainData.wavingGrassStrength = 0.686f;
		terrain.GetComponent<Terrain> ().terrainData.wavingGrassAmount = 0.106f;
		terrain.GetComponent<Terrain> ().terrainData.SetDetailResolution (64, 8);
		terrain.layer = LayerMask.NameToLayer ("Terrain");
		return terrain;
	}
	
	public Color32 Brighten (Color32 color)
	{
		//  if (color.r < 120 && color.g < 120 && color.b < 120) {
		//  	color.r = ((byte)(color.r * 1.5f));
		//  	color.g = ((byte)(color.g * 1.5f));
		//  	color.b = ((byte)(color.b * 1.5f));
		//  }
		return color;
	}
	
	public int addVertex (Vector3 pos, Vector3 normal, Vector3 texCoord, Color32 colour)
	{
		_vertices.Add (pos);
		_normals.Add (normal);
		_uv.Add (new Vector2 (texCoord.x, texCoord.y));
		_colors.Add (colour);
		vertexCount++;
		return vertexCount - 1;
	}
	
	List<int> texToTriangle = new List<int> ();
	
	private int texToTriangleArray (int tex)
	{
		if (tex == -1)
			tex = 0;
		for (int i = 0; i < texToTriangle.Count; ++i) {
			if (texToTriangle.ElementAt (i) == tex) {
				return i;
			}
		}
		
		texToTriangle.Add (tex);
		_triangles.Add (new List<int> ());
		return texToTriangle.Count - 1;
	}
	
	public void addPolygon (List<int> polylist, int tex)
	{
		for (int i = 0; i < polylist.Count; ++i)
			_triangles.ElementAt (texToTriangleArray (tex)).Add (polylist [i]);
	}
	
	public static void calculateMeshTangents (Mesh mesh)
	{
		//speed up math by copying the mesh arrays
		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;
		Vector2[] uv = mesh.uv;
		Vector3[] normals = mesh.normals;
		
		//variable definitions
		int triangleCount = triangles.Length;
		int vertexCount = vertices.Length;
		
		Vector3[] tan1 = new Vector3[vertexCount];
		Vector3[] tan2 = new Vector3[vertexCount];
		
		Vector4[] tangents = new Vector4[vertexCount];
		
		for (long a = 0; a < triangleCount; a += 3) {
			long i1 = triangles [a + 0];
			long i2 = triangles [a + 1];
			long i3 = triangles [a + 2];
			
			Vector3 v1 = vertices [i1];
			Vector3 v2 = vertices [i2];
			Vector3 v3 = vertices [i3];
			
			Vector2 w1 = uv [i1];
			Vector2 w2 = uv [i2];
			Vector2 w3 = uv [i3];
			
			float x1 = v2.x - v1.x;
			float x2 = v3.x - v1.x;
			float y1 = v2.y - v1.y;
			float y2 = v3.y - v1.y;
			float z1 = v2.z - v1.z;
			float z2 = v3.z - v1.z;
			
			float s1 = w2.x - w1.x;
			float s2 = w3.x - w1.x;
			float t1 = w2.y - w1.y;
			float t2 = w3.y - w1.y;
			
			float r = 1.0f / (s1 * t2 - s2 * t1);
			
			Vector3 sdir = new Vector3 ((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
			Vector3 tdir = new Vector3 ((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);
			
			tan1 [i1] += sdir;
			tan1 [i2] += sdir;
			tan1 [i3] += sdir;
			
			tan2 [i1] += tdir;
			tan2 [i2] += tdir;
			tan2 [i3] += tdir;
		}
		
		
		for (long a = 0; a < vertexCount; ++a) {
			Vector3 n = normals [a];
			Vector3 t = tan1 [a];

			Vector3.OrthoNormalize (ref n, ref t);
			tangents [a].x = t.x;
			tangents [a].y = t.y;
			tangents [a].z = t.z;
			
			tangents [a].w = (Vector3.Dot (Vector3.Cross (n, t), tan2 [a]) < 0.0f) ? -1.0f : 1.0f;
		}
		
		mesh.tangents = tangents;
	}
	
	public List<GameObject> GenerateOverlays (GameObject regionContainer)
	{
		List<GameObject> goList = new List<GameObject> ();
		for (int i = 0; i < 4; ++i)
			overlays [i] = new RSTerrainOverlay (i, region);
		for (int i = 0; i < 4; ++i) {
			RSTerrainOverlay overlay = overlays [i];
			Mesh[] overlayMesh = overlay.renderMesh ();
			if (overlayMesh.Length == 2) {
				Material waterMat = UnityClient.waterMat;
				GameObject meshGOWater = new GameObject ("MeshLayer-Water" + i);
				meshGOWater.AddComponent<MeshRenderer> ().sharedMaterial = waterMat;
//				UnityStandardAssets.Water.Water waterScript = meshGOWater.AddComponent<UnityStandardAssets.Water.Water> ();
//				waterScript.waterMode = UnityStandardAssets.Water.Water.WaterMode.Reflective;
				meshGOWater.AddComponent<MeshFilter> ();
				meshGOWater.GetComponent<MeshFilter> ().mesh = overlayMesh [1];
				meshGOWater.transform.position = new Vector3 (0, 0.25f, 0);
				meshGOWater.transform.localScale = new Vector3 (1 / 128f, -1 / 128f, 1 / 128f);//(0.0078125f, 0.0078125f, 0.0078125f);
				meshGOWater.transform.parent = regionContainer.transform;
				//meshGOWater.isStatic = true;
				meshGOWater.layer = LayerMask.NameToLayer ("Water");
				waterMeshList.Add (meshGOWater.GetComponent<MeshFilter> ());
				goList.Add (meshGOWater);
			}
			if (i > 0) {
				GameObject meshGO = new GameObject ("Overlay:" + i);
				meshGO.AddComponent<MeshFilter> ().mesh = overlayMesh [0];
				Material[] overlayMat = new Material[overlayMesh [0].subMeshCount];
				string[] meshMats = overlayMesh [0].name.Split (',');
				
				for (int a = 0; a < overlayMat.Length; ++a) {
					int texId = int.Parse (meshMats [a]);
					Material localMat = null;
					localMat = UnityClient.getGroundMaterial (texId);
					overlayMat [a] = localMat;
				}
				meshGO.AddComponent<MeshRenderer> ().sharedMaterials = overlayMat;
				meshGO.transform.position = new Vector3 (0, 0.05f, 0);
				meshGO.transform.localScale = new Vector3 (1 / 128f, -1 / 128f, 1 / 128f);
				meshGO.AddComponent<MeshCollider> ();
				meshGO.transform.parent = regionContainer.transform;
				goList.Add (meshGO);
				if (i == 0)
					meshGO.transform.GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
				
				if (i != 0) { //Reverse Floor Triangles
					Mesh overlayMeshUnder = overlay.renderMeshUnder ();
					//9.55
					GameObject meshGOUnder = new GameObject ("Overlay Reversed:" + i);
					meshGOUnder.AddComponent<MeshFilter> ().mesh = overlayMeshUnder;
					Material[] overlayMatUnder = new Material[overlayMeshUnder.subMeshCount];
					string[] meshMatsUnder = overlayMeshUnder.name.Split (',');
					
					for (int a = 0; a < overlayMatUnder.Length; ++a) {
						int texId = int.Parse (meshMatsUnder [a]);
						Material localMat = null;
						localMat = UnityClient.getGroundMaterial (texId);
						overlayMatUnder [a] = localMat;
					}
					meshGOUnder.AddComponent<MeshRenderer> ().sharedMaterials = overlayMatUnder;
					if (i == 1)
						meshGOUnder.transform.position = new Vector3 (0, 0, 0);
					if (i == 2)
						meshGOUnder.transform.position = new Vector3 (0, 0, 0);
					if (i == 3)
						meshGOUnder.transform.position = new Vector3 (0, 0, 0);
					
					meshGOUnder.transform.localScale = new Vector3 (1 / 128f, -1 / 128f, 1 / 128f);
					meshGOUnder.AddComponent<MeshCollider> ();
					meshGOUnder.transform.parent = regionContainer.transform;
					goList.Add (meshGOUnder);
				}
			}
		}
		return goList;
	}
	
	
	Texture2D FastBlur(Texture2D image, int radius, int iterations){
		
		Texture2D tex = image;
		
		for (var i = 0; i < iterations; i++) {
			
			tex = BlurImage(tex, radius, true);
			tex = BlurImage(tex, radius, false);
		}
		
		return tex;
	}
	private float avgR = 0;
	private float avgG = 0;
	private float avgB = 0;
	private float avgA = 0;
	private float blurPixelCount = 0;
	Texture2D BlurImage(Texture2D image, int blurSize, bool horizontal){
		
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
	
	void AddPixel(Color pixel) {
		
		avgR += pixel.r;
		avgG += pixel.g;
		avgB += pixel.b;
		blurPixelCount++;
	}
	
	void ResetPixel() {
		
		avgR = 0.0f;
		avgG = 0.0f;
		avgB = 0.0f;
		blurPixelCount = 0;
	}
	
	void CalcPixel() {
		
		avgR = avgR / blurPixelCount;
		avgG = avgG / blurPixelCount;
		avgB = avgB / blurPixelCount;
	}

	
	
}
