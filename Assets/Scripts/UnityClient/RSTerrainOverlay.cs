using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RS2Sharp;


public class RSTerrainOverlay {
	private Vector3[] texcoord;
	private Vector3[] vertexes;
	private bool[] isWater;
	private int[] triangleA;
	private int[] triangleB;
	private int[] triangleC;
	public int[] triangleHslA;
	public int[] triangleHslB;
	public int[] triangleHslC;
	public int[] triangleAlphaA;
	public int[] triangleAlphaB;
	public int[] triangleAlphaC;
	private int vertexCount = 0;
	private int triangleCount = 0;
	private int[] textureTriangles;
	private int[] textureTypes;
	private int actualTriangleCount = 0;
	private int currentTexture = -2;
	private int textureTriangleCount = 0;
	private int heightLevel = 0;
	private VertexNormal[] vertexNormals;
	private int renderCount = 0;
	private List<Vector3> _vertices = new List<Vector3>();//[this.vertex_count];
	private List<Vector3> _normals = new List<Vector3>();//[this.vertex_count];
	private List<Vector2> _uv = new List<Vector2>();//[this.vertex_count];
	private List<Color32> _colors = new List<Color32>();//[this.vertex_count];
	private List<List<int>> _triangles = new List<List<int>>();//[this.triangle_count * 3];
	private List<int> _waterTriangles = new List<int>();//[this.triangle_count * 3];
	private int vertexCountNew = 0;
	public RSTerrainOverlay(int hl, Region region)
	{
		this.heightLevel = hl;
		loadData(region);
	}
	public Mesh[] renderMesh()
	{
		Mesh[] mesh = new Mesh[1];
		if(_waterTriangles.Count != 0)
		{
			mesh = new Mesh[2];
			mesh[1] = new Mesh();
			mesh[1].vertices = _vertices.ToArray();
			mesh[1].uv = _uv.ToArray();
			mesh[1].triangles = _waterTriangles.ToArray();
			mesh[1].colors32 = _colors.ToArray();
			mesh[1].normals = _normals.ToArray();
			mesh[1].name = "MeshWater"+heightLevel;
			calculateMeshTangents(mesh[1]);
			mesh[1].Optimize();
		}
		
		mesh[0] = new Mesh();
		float yOffset = 3f;
		
		Vector3[] newVertArray = _vertices.ToArray();
		for(int i = 0; i < newVertArray.Count(); ++i)
			newVertArray[i] = new Vector3(newVertArray[i].x,newVertArray[i].y + yOffset,newVertArray[i].z);
		List<Vector3> newVertices = newVertArray.ToList();
		_vertices = _vertices.Concat(newVertices).ToList();
		mesh[0].vertices = _vertices.ToArray();
		
		_uv = _uv.Concat(_uv).ToList();
		mesh[0].uv = _uv.ToArray();
		
		mesh[0].subMeshCount = _triangles.Count;
		for(int inum = 0; inum < _triangles.Count; ++inum)
			mesh[0].SetTriangles(_triangles.ElementAt(inum).ToArray(),inum);
		string meshName = "";
		for(int inum = 0; inum < texToTriangle.Count; ++inum)
		{
			meshName += texToTriangle.ElementAt(inum) + ",";
		}
		mesh[0].name = meshName;
		_colors = _colors.Concat(_colors).ToList();
		mesh[0].colors32 = _colors.ToArray();
		
		Vector3[] normalArray = _normals.ToArray();
		for(int i = 0; i < normalArray.Count (); ++i)
			normalArray[i] = new Vector3(normalArray[i].x, -normalArray[i].y, normalArray[i].z);
		List<Vector3> newNormals = normalArray.ToList();
		_normals = _normals.Concat(newNormals).ToList();
		mesh[0].normals = _normals.ToArray();
		calculateMeshTangents(mesh[0]);
		if(mesh[0].name == "") mesh[0].name = "MeshOverlay"+heightLevel;
		mesh[0].Optimize();
		return mesh;
	}
	
	public Mesh renderMeshUnder()
	{
		Mesh mesh = new Mesh();
		float yOffset = 3f;
		
		Vector3[] newVertArray = _vertices.ToArray();
		for(int i = 0; i < newVertArray.Count(); ++i)
			newVertArray[i] = new Vector3(newVertArray[i].x,newVertArray[i].y + yOffset,newVertArray[i].z);
		List<Vector3> newVertices = newVertArray.ToList();
		_vertices = _vertices.Concat(newVertices).ToList();
		mesh.vertices = _vertices.ToArray();
		
		_uv = _uv.Concat(_uv).ToList();
		mesh.uv = _uv.ToArray();
		
		mesh.subMeshCount = _triangles.Count;
		
		for(int inum = 0; inum < _triangles.Count; ++inum)
		{
			int[] tri = _triangles.ElementAt(inum).ToArray();
			for(int i=0; i<tri.Length; i=i+3)
			{
				int tmp = tri[i];
				tri[i] = tri[i+2];
				tri[i+2] = tmp;
			}
			mesh.SetTriangles(tri,inum);
		}
		string meshName = "";
		for(int inum = 0; inum < texToTriangle.Count; ++inum)
		{
			meshName += texToTriangle.ElementAt(inum) + ",";
		}
		mesh.name = meshName;
		_colors = _colors.Concat(_colors).ToList();
		mesh.colors32 = _colors.ToArray();
		
		Vector3[] normalArray = _normals.ToArray();
		for(int i = 0; i < normalArray.Count (); ++i)
			normalArray[i] = new Vector3(normalArray[i].x, -normalArray[i].y, normalArray[i].z);
		List<Vector3> newNormals = normalArray.ToList();
		_normals = _normals.Concat(newNormals).ToList();
		mesh.normals = _normals.ToArray();
		calculateMeshTangents(mesh);
		if(mesh.name == "") mesh.name = "MeshOverlayReverse"+heightLevel;
		mesh.Optimize();
		return mesh;
	}
	
	private void loadData(Region region) {
		triangleCount = 6 * 104 * 104;//Maximal triangle count
		vertexCount = 6 * 104 * 104;//Maximal triangle count
		textureTriangles = new int[triangleCount];
		textureTypes = new int[triangleCount];
		texcoord = new Vector3[vertexCount];
		vertexes = new Vector3[vertexCount];
		triangleA = new int[triangleCount];
		isWater = new bool[triangleCount];
		triangleB = new int[triangleCount];
		triangleC = new int[triangleCount];
		triangleHslA = new int[triangleCount];
		triangleHslB = new int[triangleCount];
		triangleHslC = new int[triangleCount];
		triangleAlphaA = new int[triangleCount];
		triangleAlphaB = new int[triangleCount];
		triangleAlphaC = new int[triangleCount];
		int[] triangleTex = new int[triangleCount];
		triangleCount = 0;
		vertexCount = 0;
		for (int x = 0; x < 104; x++)
		for (int z = 0; z < 104; z++) {
			int shapeA = region.tile_layer1_shape[heightLevel][x][z] + 1;
			int shapeB = region.tile_layer1_orientation[heightLevel][x][z];
			int overlay = region.tile_layer1_type[heightLevel][x][z] & 0xff;
			int[][][] tileH = MapManager.getTileHeight(region.myRegionX,region.myRegionY);
			if(tileH == null) return;
			int yA = /*-*/tileH[heightLevel][x][z];
			int yB = /*-*/tileH[heightLevel][x + 1][z];
			int yD = /*-*/tileH[heightLevel][x + 1][z + 1];
			int yC = /*-*/tileH[heightLevel][x][z + 1];
			int underlayRgb = region.tile_layer0_colour [heightLevel][x][z];
			if (overlay >= 1) {
				if(overlay >= /*Floor.cache*/OverLayFlo317.overLayFlo317s.Length)
				{
					overlay = 184;
					//Debug.Log ("Error, " + overlay + " is greater than length = >" + /*Floor.cache*/OverLayFlo317.overLayFlo317s.Length);
				}
				//else
				//{
					OverLayFlo317 overlayFloor = /*Floor.cache*/OverLayFlo317.overLayFlo317s[overlay - 1];
					//Debug.Log (overlayFloor.rgb);
				int waterColor = 6321818;
				if /*(overlayFloor.textureId != 669)//*/(overlayFloor.rgb != 0xFF00FF && overlayFloor.rgb != waterColor)// && overlayFloor.rgb != 0xFFFFFF && overlayFloor.rgb != 7509687)
						//if(overlayFloor.textureId != 669 && overlayFloor.textureId != 651)
						generateDataForTile(x, z, yA + 1, yB + 1, yC + 1, yD + 1, overlayFloor.rgb, overlayFloor.rgb, overlayFloor.rgb, overlayFloor.rgb, overlayFloor.textureId, shapeA, shapeB, overlayFloor.alpha, false, overlayFloor.textureId, triangleTex);
					if /*(overlayFloor.textureId == 669)//*/(overlayFloor.rgb == 0xFFFFFF || overlayFloor.rgb == waterColor)//7509687) //Water
						//{
						//else
						generateDataForTile(x, z, yA + 1, yB + 1, yC + 1, yD + 1, overlayFloor.rgb, overlayFloor.rgb, overlayFloor.rgb, overlayFloor.rgb, overlayFloor.textureId, shapeA, shapeB, overlayFloor.alpha, true, overlayFloor.textureId, triangleTex);
					//}
					
				//}
			}
		}
		calculateNormals508();
		for (int triPtr = 0; triPtr < triangleCount; triPtr++)
			addTriangle(triPtr,triangleTex[triPtr]);
	}
	private void generateDataForTile(int tileX, int tileZ, int yA, int yB, int yC, int yD, int cAA, int cBA, int cCA, int cDA, int texture, int shapeA, int shapeB, int alpha, bool isWaterTile, int hdTexture, int[] triangleTex) {
		if (alpha == 2567 && !(texture == 1 || texture == 24))
			return;
		else if (alpha == 2567) {
			texture = 678;
			alpha = 255;
			yA++;
			yB++;
			yC++;
			yD++;
		} else if (texture == 1 || texture == 24) {
			texture = 679;
		}
		char const512 = (char)128;//'È'; //idk /200
		int const256 = const512 / 2;
		int const128 = const512 / 4;
		int const384 = (const512 * 3) / 4;
		int[] shapedTileMesh = ComplexTile.anIntArrayArray696[shapeA];
		int meshLength = shapedTileMesh.Length;
		int[] vertexColourUnderlay = new int[meshLength];
		int x512 = tileX * const512;
		int z512 = tileZ * const512;
		int vertexBasePtr = vertexCount;
		bool[] isInOverlay = new bool[meshLength];
		for (int vertexPtr = 0; vertexPtr < meshLength; vertexPtr++) {
			int vertexType = shapedTileMesh[vertexPtr];
			if ((vertexType & 1) == 0 && vertexType <= 8)
				vertexType = (vertexType - shapeB - shapeB - 1 & 7) + 1;
			if (vertexType > 8 && vertexType <= 12)
				vertexType = (vertexType - 9 - shapeB & 3) + 9;
			if (vertexType > 12 && vertexType <= 16)
				vertexType = (vertexType - 13 - shapeB & 3) + 13;
			int vertexX;
			int vertexZ;
			int vertexY;
			// int vertexCOverlay;
			int vertexCUnderlay;
			if (vertexType == 1) {
				vertexX = x512;
				vertexZ = z512;
				vertexY = yA;
				//    vertexCOverlay = cA;
				vertexCUnderlay = cAA;
			} else if (vertexType == 2) {
				vertexX = x512 + const256;
				vertexZ = z512;
				vertexY = yA + yB >> 1;
				//    vertexCOverlay = cA + cB >> 1;
				vertexCUnderlay = cAA + cBA >> 1;
			} else if (vertexType == 3) {
				vertexX = x512 + const512;
				vertexZ = z512;
				vertexY = yB;
				//    vertexCOverlay = cB;
				vertexCUnderlay = cBA;
			} else if (vertexType == 4) {
				vertexX = x512 + const512;
				vertexZ = z512 + const256;
				vertexY = yB + yD >> 1;
				//   vertexCOverlay = cB + cD >> 1;
				vertexCUnderlay = cBA + cDA >> 1;
			} else if (vertexType == 5) {
				vertexX = x512 + const512;
				vertexZ = z512 + const512;
				vertexY = yD;
				//    vertexCOverlay = cD;
				vertexCUnderlay = cDA;
			} else if (vertexType == 6) {
				vertexX = x512 + const256;
				vertexZ = z512 + const512;
				vertexY = yD + yC >> 1;
				//    vertexCOverlay = cD + cC >> 1;
				vertexCUnderlay = cDA + cCA >> 1;
			} else if (vertexType == 7) {
				vertexX = x512;
				vertexZ = z512 + const512;
				vertexY = yC;
				//    vertexCOverlay = cC;
				vertexCUnderlay = cCA;
			} else if (vertexType == 8) {
				vertexX = x512;
				vertexZ = z512 + const256;
				vertexY = yC + yA >> 1;
				//    vertexCOverlay = cC + cA >> 1;
				vertexCUnderlay = cCA + cAA >> 1;
			} else if (vertexType == 9) {
				vertexX = x512 + const256;
				vertexZ = z512 + const128;
				vertexY = yA + yB >> 1;
				//    vertexCOverlay = cA + cB >> 1;
				vertexCUnderlay = cAA + cBA >> 1;
			} else if (vertexType == 10) {
				vertexX = x512 + const384;
				vertexZ = z512 + const256;
				vertexY = yB + yD >> 1;
				//    vertexCOverlay = cB + cD >> 1;
				vertexCUnderlay = cBA + cDA >> 1;
			} else if (vertexType == 11) {
				vertexX = x512 + const256;
				vertexZ = z512 + const384;
				vertexY = yD + yC >> 1;
				//    vertexCOverlay = cD + cC >> 1;
				vertexCUnderlay = cDA + cCA >> 1;
			} else if (vertexType == 12) {
				vertexX = x512 + const128;
				vertexZ = z512 + const256;
				vertexY = yC + yA >> 1;
				//    vertexCOverlay = cC + cA >> 1;
				vertexCUnderlay = cCA + cAA >> 1;
			} else if (vertexType == 13) {
				vertexX = x512 + const128;
				vertexZ = z512 + const128;
				vertexY = yA;
				//    vertexCOverlay = cA;
				vertexCUnderlay = cAA;
			} else if (vertexType == 14) {
				vertexX = x512 + const384;
				vertexZ = z512 + const128;
				vertexY = yB;
				//    vertexCOverlay = cB;
				vertexCUnderlay = cBA;
			} else if (vertexType == 15) {
				vertexX = x512 + const384;
				vertexZ = z512 + const384;
				vertexY = yD;
				//    vertexCOverlay = cD;
				vertexCUnderlay = cDA;
			} else {
				vertexX = x512 + const128;
				vertexZ = z512 + const384;
				vertexY = yC;
				//   vertexCOverlay = cC;
				vertexCUnderlay = cCA;
			}
			vertexes[vertexCount] = new Vector3(vertexX, vertexY, vertexZ);
			
			Vector3 uvToApply = new Vector3(((vertexX - tileX) / 128.0f) * ((texture == 678) ? 0.1f : 1), ((vertexZ - tileZ) / 128.0f) * ((texture == 678) ? 0.1f : 1), 0);
			texcoord[vertexCount++] = uvToApply;
			// vertexColourOverlay[vertexPtr] = vertexCOverlay;
			vertexColourUnderlay[vertexPtr] = vertexCUnderlay;
		}
		
		int[] shapedTileElements = ComplexTile.anIntArrayArray697[shapeA];
		int triangleCount2 = shapedTileElements.Length / 4;
		if (currentTexture != texture) {
			textureTypes[textureTriangleCount] = texture;
			textureTriangles[textureTriangleCount++] = triangleCount;
			currentTexture = texture;
		}
		int offset = 0;
		for (int tID = 0; tID < triangleCount2; tID++) {
			int overlayOrUnderlay = shapedTileElements[offset];
			int idxA = shapedTileElements[offset + 1];
			int idxB = shapedTileElements[offset + 2];
			int idxC = shapedTileElements[offset + 3];
			offset += 4;
			if (idxA < 4)
				idxA = idxA - shapeB & 3;
			if (idxB < 4)
				idxB = idxB - shapeB & 3;
			if (idxC < 4)
				idxC = idxC - shapeB & 3;
			if (overlayOrUnderlay != 0) {
				isInOverlay[idxA] = true;
				isInOverlay[idxB] = true;
				isInOverlay[idxC] = true;
			}
		}
		offset = 0;
		for (int tID = 0; tID < triangleCount2; tID++) {
			int overlayOrUnderlay = shapedTileElements[offset];
			int idxA = shapedTileElements[offset + 1];
			int idxB = shapedTileElements[offset + 2];
			int idxC = shapedTileElements[offset + 3];
			offset += 4;
			if (idxA < 4)
				idxA = idxA - shapeB & 3;
			if (idxB < 4)
				idxB = idxB - shapeB & 3;
			if (idxC < 4)
				idxC = idxC - shapeB & 3;
			triangleA[triangleCount] = vertexBasePtr + idxA;
			triangleB[triangleCount] = vertexBasePtr + idxB;
			triangleC[triangleCount] = vertexBasePtr + idxC;
			if(isWaterTile) isWater[triangleCount] = true;
			if (overlayOrUnderlay != 0) {
				triangleAlphaA[triangleCount] = alpha;
				triangleAlphaB[triangleCount] = alpha;
				triangleAlphaC[triangleCount] = alpha;
				triangleHslA[triangleCount] = vertexColourUnderlay[idxA];
				triangleHslB[triangleCount] = vertexColourUnderlay[idxB];
				triangleTex[triangleCount] = texture;
				triangleHslC[triangleCount++] = vertexColourUnderlay[idxC];
			}
		}
	}
	public void calculateNormals508() {
		vertexNormals = null;
		if (vertexNormals == null) {
			vertexNormals = new VertexNormal[vertexCount];
			for (int i = 0; i < vertexCount; i++)
				vertexNormals[i] = new VertexNormal();
			for (int i = 0; i < triangleCount; i++) {
				int triA = triangleA[i];
				int triB = triangleB[i];
				int triC = triangleC[i];
				Vector3 A = vertexes[triA];
				Vector3 B = vertexes[triB];
				Vector3 C = vertexes[triC];
				int Ux = (int) (B.x - A.x) * 256;
				int Uy = (int) (B.y - A.y) * 256;
				int Uz = (int) (B.z - A.z) * 256;
				int Vx = (int) (C.x - A.x) * 256;
				int Vy = (int) (C.y - A.y) * 256;
				int Vz = (int) (C.z - A.z) * 256;
				int Nx = Uy * Vz - Vy * Uz;
				int Ny = Uz * Vx - Vz * Ux;
				int Nz;
				for (Nz = Ux * Vy - Vx * Uy; (Nx > 8192 || Ny > 8192 || Nz > 8192 || Nx < -8192 || Ny < -8192 || Nz < -8192); Nz >>= 1) {
					Nx >>= 1;
					Ny >>= 1;
				}
				int i_169_ = (int) Mathf.Sqrt((float)((double) (Nx * Nx + Ny * Ny + Nz * Nz)));
				if (i_169_ <= 0)
					i_169_ = 1;
				Nx = Nx * 256 / i_169_;
				Ny = Ny * 256 / i_169_;
				Nz = Nz * 256 / i_169_;
				VertexNormal vertexNormal = vertexNormals[triA];
				vertexNormal.x += Nx;
				vertexNormal.y += Ny;
				vertexNormal.z += Nz;
				vertexNormal.magnitude++;
				vertexNormal = vertexNormals[triB];
				vertexNormal.x += Nx;
				vertexNormal.y += Ny;
				vertexNormal.z += Nz;
				vertexNormal.magnitude++;
				vertexNormal = vertexNormals[triC];
				vertexNormal.x += Nx;
				vertexNormal.y += Ny;
				vertexNormal.z += Nz;
				vertexNormal.magnitude++;
			}
		}
	}
	private void addTriangle(int triIdx, int tex) {
		int verAIdx = triangleA[triIdx];
		int verBIdx = triangleB[triIdx];
		int verCIdx = triangleC[triIdx];
		Color32 colorA = fromRgb(triangleHslA[triIdx],triangleAlphaA[triIdx]);//fromRgb(Rasterizer.hsl2rgb[triangleHslA[triIdx]], triangleAlphaA[triIdx]);
		Color32 colorB = fromRgb(triangleHslB[triIdx],triangleAlphaB[triIdx]);//fromRgb(Rasterizer.hsl2rgb[triangleHslB[triIdx]], triangleAlphaB[triIdx]);
		Color32 colorC = fromRgb(triangleHslC[triIdx],triangleAlphaC[triIdx]);//fromRgb(Rasterizer.hsl2rgb[triangleHslC[triIdx]], triangleAlphaC[triIdx]);
		Vector3 normalA = new Vector3(vertexNormals[verAIdx].x, vertexNormals[verAIdx].y, vertexNormals[verAIdx].z);
		Vector3 normalB = new Vector3(vertexNormals[verBIdx].x, vertexNormals[verBIdx].y, vertexNormals[verBIdx].z);
		Vector3 normalC = new Vector3(vertexNormals[verCIdx].x, vertexNormals[verCIdx].y, vertexNormals[verCIdx].z);
		int bufAIdx = addVertex(vertexes[verAIdx], normalA, texcoord[verAIdx], colorA, tex);
		int bufBIdx = addVertex(vertexes[verBIdx], normalB, texcoord[verBIdx], colorB, tex);
		int bufCIdx = addVertex(vertexes[verCIdx], normalC, texcoord[verCIdx], colorC, tex);
		actualTriangleCount++;
		List<int> polylist = new List<int>();
		polylist.Add(bufAIdx);
		polylist.Add(bufBIdx);
		polylist.Add(bufCIdx);
		bool isWaterTri = false;
		if(isWater[triIdx])
			isWaterTri = true;
		addPolygon(polylist,isWaterTri, tex);
	}
	public static void calculateMeshTangents(Mesh mesh)
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
		
		for (long a = 0; a < triangleCount; a += 3)
		{
			long i1 = triangles[a + 0];
			long i2 = triangles[a + 1];
			long i3 = triangles[a + 2];
			
			Vector3 v1 = vertices[i1];
			Vector3 v2 = vertices[i2];
			Vector3 v3 = vertices[i3];
			
			Vector2 w1 = uv[i1];
			Vector2 w2 = uv[i2];
			Vector2 w3 = uv[i3];
			
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
			
			Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
			Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);
			
			tan1[i1] += sdir;
			tan1[i2] += sdir;
			tan1[i3] += sdir;
			
			tan2[i1] += tdir;
			tan2[i2] += tdir;
			tan2[i3] += tdir;
		}
		
		
		for (long a = 0; a < vertexCount; ++a)
		{
			Vector3 n = normals[a];
			Vector3 t = tan1[a];
			
			//Vector3 tmp = (t - n * Vector3.Dot(n, t)).normalized;
			//tangents[a] = new Vector4(tmp.x, tmp.y, tmp.z);
			Vector3.OrthoNormalize(ref n, ref t);
			tangents[a].x = t.x;
			tangents[a].y = t.y;
			tangents[a].z = t.z;
			
			tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
		}
		
		mesh.tangents = tangents;
	}
	public Color32 ToColor(int HexVal, int a)
	{
		byte R = (byte)((HexVal >> 16) & 0xFF);
		byte G = (byte)((HexVal >> 8) & 0xFF);
		byte B = (byte)((HexVal) & 0xFF);
		return new Color32(R, G, B, (byte)a);
	}
	private Color32 fromRgb(int rgb,int a){
		return ToColor(rgb,a);
	}
	
	public int addVertex(Vector3 pos, Vector3 normal, Vector3 texCoord, Color32 colour, int texId) {
		_vertices.Add(pos);
		_normals.Add (normal);
		_uv.Add(new Vector2(texCoord.x,texCoord.y));
		_colors.Add (colour);
		vertexCountNew++;
		return vertexCountNew - 1;
	}
	List<int> texToTriangle = new List<int>();
	private int texToTriangleArray(int tex)
	{
		if(tex == -1) tex = 0;
		for(int i = 0; i < texToTriangle.Count; ++i)
		{
			if(texToTriangle[i] == tex)
			{
				return i;
			}
		}
		
		texToTriangle.Add (tex);
		_triangles.Add (new List<int>());
		return texToTriangle.Count-1;
	}
	public void addPolygon(List<int> polylist, bool water, int tex)
	{
		if(!water)
		{
			for(int i = 0; i < polylist.Count; ++i)
			{
				_triangles[texToTriangleArray(tex)].Add(polylist[i]);
			}
		}
		else if(water)
		{
			for(int i = 0; i < polylist.Count; ++i)
			{
				_waterTriangles.Add(polylist[i]);
			}
		}
	}
}
