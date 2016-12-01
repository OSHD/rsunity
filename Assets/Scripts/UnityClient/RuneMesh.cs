using UnityEngine;
using System.Collections;
using RS2Sharp;
using System.Collections.Generic;
using System.Linq;
using System;

public class RuneMesh
{

	public int[] faceTexture;
	public int[] triangleViewspaceX;
	public int[] triangleViewspaceY;
	public int[] triangleViewspaceZ;
	public int[] textureTrianglePIndex;
	public int[] textureTriangleMIndex;
	public int[] textureTriangleNIndex;
	public int[] verticesX;
	public int[] verticesY;
	public int[] verticesZ;

	public int numTriangles;
	public int[] faceRenderType;
	public TriangleNormal[] triangleNormals;
	public VertexNormal[] isolatedVertexNormals;
	public int[] faceAlpha;
	public int numVertices;
	public bool justReset = false;
	TextureMapper textureMap = new TextureMapper ();

	public void Fill (Model model, bool firstTime)
	{
		if(firstTime)
		{
//		justReset = true;
//		SetUp(model);
//		System.Array.Copy(model.triangleColourOrTexture,faceTexture,model.triangleColourOrTexture.Length);
//		System.Array.Copy(model.triangleX,triangleViewspaceX,model.triangleX.Length);
//		System.Array.Copy(model.triangleY,triangleViewspaceY,model.triangleY.Length);
//		System.Array.Copy(model.triangleZ,triangleViewspaceZ,model.triangleZ.Length);
//		System.Array.Copy(model.triangle_hsl_a,textureTrianglePIndex,model.triangle_hsl_a.Length);
//		System.Array.Copy(model.triangle_hsl_b,textureTriangleMIndex,model.triangle_hsl_b.Length);
//		System.Array.Copy(model.triangle_hsl_c,textureTriangleNIndex,model.triangle_hsl_c.Length);
//		System.Array.Copy(model.verticesX,verticesX,model.verticesX.Length);
//		System.Array.Copy(model.verticesY,verticesY,model.verticesY.Length);
//		System.Array.Copy(model.verticesZ,verticesZ,model.verticesZ.Length);
//		numTriangles = model.numTriangles;
//		System.Array.Copy(model.triangleDrawType,faceRenderType,model.triangleDrawType.Length);
//		System.Array.Copy(model.aClass33Array1660,isolatedVertexNormals,model.aClass33Array1660.Length);
//		System.Array.Copy(model.triangleAlpha,faceAlpha,model.triangleAlpha.Length);
//		numVertices = model.numVertices;
//		}
//		else
//		{
//			numVertices = model.numVertices;
//			System.Array.Copy(model.triangleX,triangleViewspaceX,model.triangleX.Length);
//			System.Array.Copy(model.triangleY,triangleViewspaceY,model.triangleY.Length);
//			System.Array.Copy(model.triangleZ,triangleViewspaceZ,model.triangleZ.Length);
//			System.Array.Copy(model.verticesX,verticesX,model.verticesX.Length);
//			System.Array.Copy(model.verticesY,verticesY,model.verticesY.Length);
//			System.Array.Copy(model.verticesZ,verticesZ,model.verticesZ.Length);
//			numTriangles = model.numTriangles;
//		}
		
		justReset = true;
		SetUp(model);
		Buffer.BlockCopy(model.triangleColourOrTexture,0, faceTexture,0, model.triangleColourOrTexture.Length * sizeof(int));
		Buffer.BlockCopy(model.triangleX,0, triangleViewspaceX,0, model.triangleX.Length * sizeof(int));
		Buffer.BlockCopy(model.triangleY,0, triangleViewspaceY,0, model.triangleY.Length * sizeof(int));
		Buffer.BlockCopy(model.triangleZ,0, triangleViewspaceZ,0, model.triangleZ.Length * sizeof(int));
		Buffer.BlockCopy(model.triangle_hsl_a,0, textureTrianglePIndex,0, model.triangle_hsl_a.Length * sizeof(int));
		Buffer.BlockCopy(model.triangle_hsl_b,0, textureTriangleMIndex,0, model.triangle_hsl_b.Length * sizeof(int));
		Buffer.BlockCopy(model.triangle_hsl_c,0, textureTriangleNIndex,0, model.triangle_hsl_c.Length * sizeof(int));
		Buffer.BlockCopy(model.verticesX,0, verticesX,0, model.verticesX.Length * sizeof(int));
		Buffer.BlockCopy(model.verticesY,0, verticesY,0, model.verticesY.Length * sizeof(int));
		Buffer.BlockCopy(model.verticesZ,0, verticesZ,0, model.verticesZ.Length * sizeof(int));
		numTriangles = model.numTriangles;
		Buffer.BlockCopy(model.triangleDrawType,0, faceRenderType,0, model.triangleDrawType.Length * sizeof(int));
		if (isolatedVertexNormals == null) isolatedVertexNormals = new VertexNormal[numVertices];
		System.Array.Copy(model.aClass33Array1660,isolatedVertexNormals,model.aClass33Array1660.Length);
		Buffer.BlockCopy(model.triangleAlpha,0, faceAlpha,0, model.triangleAlpha.Length * sizeof(int));
		numVertices = model.numVertices;
	}
	else
	{
		numVertices = model.numVertices;
		Buffer.BlockCopy(model.triangleX,0, triangleViewspaceX,0, model.triangleX.Length * sizeof(int));
		Buffer.BlockCopy(model.triangleY,0, triangleViewspaceY,0, model.triangleY.Length * sizeof(int));
		Buffer.BlockCopy(model.triangleZ,0, triangleViewspaceZ,0, model.triangleZ.Length * sizeof(int));
		Buffer.BlockCopy(model.verticesX,0, verticesX,0, model.verticesX.Length * sizeof(int));
		Buffer.BlockCopy(model.verticesY,0, verticesY,0, model.verticesY.Length * sizeof(int));
		Buffer.BlockCopy(model.verticesZ,0, verticesZ,0, model.verticesZ.Length * sizeof(int));
		numTriangles = model.numTriangles;
	}
	}

	public void SetUp (Model model)
	{
		if(model.triangleColourOrTexture == null) model.triangleColourOrTexture = new int[0];
		faceTexture = new int[model.triangleColourOrTexture.Length];
		
		if(model.triangleX == null) model.triangleX = new int[0];
		triangleViewspaceX = new int[model.triangleX.Length];
		
		if(model.triangleY == null) model.triangleY = new int[0];
		triangleViewspaceY = new int[model.triangleY.Length];
		
		if(model.triangleZ == null) model.triangleZ = new int[0];
		triangleViewspaceZ = new int[model.triangleZ.Length];
		
		if(model.triangle_hsl_a == null) model.triangle_hsl_a = new int[0];
		textureTrianglePIndex = new int[model.triangle_hsl_a.Length];
		
		if(model.triangle_hsl_b == null) model.triangle_hsl_b = new int[0];
		textureTriangleMIndex = new int[model.triangle_hsl_b.Length];
		
		if(model.triangle_hsl_c == null) model.triangle_hsl_c = new int[0];
		textureTriangleNIndex = new int[model.triangle_hsl_c.Length];
		
		if(model.verticesX == null) model.verticesX = new int[0];
		verticesX = new int[model.verticesX.Length];
		
		if(model.verticesY == null) model.verticesY = new int[0];
		verticesY = new int[model.verticesY.Length];
		
		if(model.verticesZ == null) model.verticesZ = new int[0];
		verticesZ = new int[model.verticesZ.Length];
		
		numTriangles = 0;
		
		if(model.triangleDrawType == null) model.triangleDrawType = new int[0];
		faceRenderType = new int[model.triangleDrawType.Length];
		
		triangleNormals = null;
		
		if(model.aClass33Array1660 == null) model.aClass33Array1660 = new VertexNormal[0];
		isolatedVertexNormals = new VertexNormal[model.aClass33Array1660.Length];
		
		if(model.triangleAlpha == null) model.triangleAlpha = new int[0];
		faceAlpha = new int[model.triangleAlpha.Length];

		numVertices = 0;
		//matOrder = new List<int> ();
		containsAlpha = false;
		vertexCount = 0;
		triangleCount = 0;
		subMeshCount = 0;
	}

	public MeshAndMat Render (Mesh returnMesh, bool animated)
	{
		uvIndex = 0;
		triangleIndex = new int[10];
		vertIndex = 0;
		colorIndex = 0;
		normalIndex = 0;
		float model_scale = 1f;
		MeshAndMat mandm = new MeshAndMat ();
		if(justReset) animated = false;
		justReset = false;
		if(faceRenderType == null || faceRenderType.Length < numTriangles) faceRenderType = new int[numTriangles];
		if (animated) {
			vertIndex = 0;
			vertexCount = 0;
			int triangle_type = 0;
			int texture = 0;
			bool textured = false;
			//calculateNormals508();
			for (int triangle = 0; triangle < numTriangles; ++triangle) {
				if (faceRenderType == null) {
					triangle_type = 0;
				} else {
					triangle_type = faceRenderType [triangle] & 3;
				}
				textured = false;
				if (this.faceTexture != null && this.faceTexture.Length > 0)
					textured = true;
				texture = 0;
				if (textured) {
					texture = 0;
				}
				
				switch (triangle_type) {
				case 0://shaded triangle
					AddVertex (new Vector3 ((float)this.verticesX [this.triangleViewspaceX [triangle]] / model_scale, -((float)this.verticesY [this.triangleViewspaceX [triangle]] / model_scale), (float)this.verticesZ [this.triangleViewspaceX [triangle]] / model_scale));
					AddVertex (new Vector3 ((float)this.verticesX [this.triangleViewspaceY [triangle]] / model_scale, -((float)this.verticesY [this.triangleViewspaceY [triangle]] / model_scale), (float)this.verticesZ [this.triangleViewspaceY [triangle]] / model_scale));
					AddVertex (new Vector3 ((float)this.verticesX [this.triangleViewspaceZ [triangle]] / model_scale, -((float)this.verticesY [this.triangleViewspaceZ [triangle]] / model_scale), (float)this.verticesZ [this.triangleViewspaceZ [triangle]] / model_scale));
					if (triangleNormals != null && triangleNormals [triangle] != null) {
						Vector3 normalT = new Vector3 (triangleNormals [triangle].x / 255.0f, -(triangleNormals [triangle].y / 255.0f), triangleNormals [triangle].z / 255.0f);
						AddNormal (normalT);
						AddNormal (normalT);
						AddNormal (normalT);
					} else {
						Vector3 normalA = new Vector3 (isolatedVertexNormals [this.triangleViewspaceX [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceX [triangle]].y), isolatedVertexNormals [this.triangleViewspaceX [triangle]].z);
						Vector3 normalB = new Vector3 (isolatedVertexNormals [this.triangleViewspaceY [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceY [triangle]].y), isolatedVertexNormals [this.triangleViewspaceY [triangle]].z);
						Vector3 normalC = new Vector3 (isolatedVertexNormals [this.triangleViewspaceZ [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceZ [triangle]].y), isolatedVertexNormals [this.triangleViewspaceZ [triangle]].z);
						AddNormal (normalA);
						AddNormal (normalB);
						AddNormal (normalC);
					}
					break;
				case 1://flat triangle
					AddVertex (new Vector3 ((float)this.verticesX [this.triangleViewspaceX [triangle]] / model_scale, -((float)this.verticesY [this.triangleViewspaceX [triangle]] / model_scale), (float)this.verticesZ [this.triangleViewspaceX [triangle]] / model_scale));
					AddVertex (new Vector3 ((float)this.verticesX [this.triangleViewspaceY [triangle]] / model_scale, -((float)this.verticesY [this.triangleViewspaceY [triangle]] / model_scale), (float)this.verticesZ [this.triangleViewspaceY [triangle]] / model_scale));
					AddVertex (new Vector3 ((float)this.verticesX [this.triangleViewspaceZ [triangle]] / model_scale, -((float)this.verticesY [this.triangleViewspaceZ [triangle]] / model_scale), (float)this.verticesZ [this.triangleViewspaceZ [triangle]] / model_scale));
					if (triangleNormals != null && triangleNormals [triangle] != null) {
						Vector3 normalT = new Vector3 (triangleNormals [triangle].x / 255.0f, -(triangleNormals [triangle].y / 255.0f), triangleNormals [triangle].z / 255.0f);
						AddNormal (normalT);
						AddNormal (normalT);
						AddNormal (normalT);
					} else {
						Vector3 normalA = new Vector3 (isolatedVertexNormals [this.triangleViewspaceX [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceX [triangle]].y), isolatedVertexNormals [this.triangleViewspaceX [triangle]].z);
						Vector3 normalB = new Vector3 (isolatedVertexNormals [this.triangleViewspaceY [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceY [triangle]].y), isolatedVertexNormals [this.triangleViewspaceY [triangle]].z);
						Vector3 normalC = new Vector3 (isolatedVertexNormals [this.triangleViewspaceZ [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceZ [triangle]].y), isolatedVertexNormals [this.triangleViewspaceZ [triangle]].z);
						AddNormal (normalA);
						AddNormal (normalB);
						AddNormal (normalC);
					}
					break;
				case 2://textured?
					if (textureTrianglePIndex [(faceRenderType [triangle] >> 2)] > 1000 || textureTrianglePIndex [(faceRenderType [triangle] >> 2)] < 0 || textureTriangleMIndex [(faceRenderType [triangle] >> 2)] > 1000 || textureTriangleMIndex [(faceRenderType [triangle] >> 2)] < 0 || textureTriangleNIndex [(faceRenderType [triangle] >> 2)] > 1000 || textureTriangleNIndex [(faceRenderType [triangle] >> 2)] < 0) {
						textureTrianglePIndex [(faceRenderType [triangle] >> 2)] = 0;
						textureTriangleMIndex [(faceRenderType [triangle] >> 2)] = 0;
						textureTriangleNIndex [(faceRenderType [triangle] >> 2)] = 0;
					}
					AddVertex (new Vector3 ((float)this.verticesX [textureTrianglePIndex [(faceRenderType [triangle] >> 2)]] / model_scale, -((float)this.verticesY [textureTrianglePIndex [(faceRenderType [triangle] >> 2)]] / model_scale), (float)this.verticesZ [textureTrianglePIndex [(faceRenderType [triangle] >> 2)]] / model_scale));
					AddVertex (new Vector3 ((float)this.verticesX [textureTriangleMIndex [(faceRenderType [triangle] >> 2)]] / model_scale, -((float)this.verticesY [textureTriangleMIndex [(faceRenderType [triangle] >> 2)]] / model_scale), (float)this.verticesZ [textureTriangleMIndex [(faceRenderType [triangle] >> 2)]] / model_scale));
					AddVertex (new Vector3 ((float)this.verticesX [textureTriangleNIndex [(faceRenderType [triangle] >> 2)]] / model_scale, -((float)this.verticesY [textureTriangleNIndex [(faceRenderType [triangle] >> 2)]] / model_scale), (float)this.verticesZ [textureTriangleNIndex [(faceRenderType [triangle] >> 2)]] / model_scale));
					if (triangleNormals != null && triangleNormals [triangle] != null) {
						Vector3 normalT = new Vector3 (triangleNormals [triangle].x / 255.0f, -(triangleNormals [triangle].y / 255.0f), triangleNormals [triangle].z / 255.0f);
						AddNormal (normalT);
						AddNormal (normalT);
						AddNormal (normalT);
					} else {
						Vector3 normalA = new Vector3 (isolatedVertexNormals [this.triangleViewspaceX [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceX [triangle]].y), isolatedVertexNormals [this.triangleViewspaceX [triangle]].z);
						Vector3 normalB = new Vector3 (isolatedVertexNormals [this.triangleViewspaceY [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceY [triangle]].y), isolatedVertexNormals [this.triangleViewspaceY [triangle]].z);
						Vector3 normalC = new Vector3 (isolatedVertexNormals [this.triangleViewspaceZ [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceZ [triangle]].y), isolatedVertexNormals [this.triangleViewspaceZ [triangle]].z);
						AddNormal (normalA);
						AddNormal (normalB);
						AddNormal (normalC);
					}
					break;
				case 3://textured?
					if (textureTrianglePIndex [(faceRenderType [triangle] >> 2)] > 1000 || textureTrianglePIndex [(faceRenderType [triangle] >> 2)] < 0 || textureTriangleMIndex [(faceRenderType [triangle] >> 2)] > 1000 || textureTriangleMIndex [(faceRenderType [triangle] >> 2)] < 0 || textureTriangleNIndex [(faceRenderType [triangle] >> 2)] > 1000 || textureTriangleNIndex [(faceRenderType [triangle] >> 2)] < 0) {
						textureTrianglePIndex [(faceRenderType [triangle] >> 2)] = 0;
						textureTriangleMIndex [(faceRenderType [triangle] >> 2)] = 0;
						textureTriangleNIndex [(faceRenderType [triangle] >> 2)] = 0;
					}
					AddVertex (new Vector3 ((float)this.verticesX [textureTrianglePIndex [(faceRenderType [triangle] >> 2)]] / model_scale, -((float)this.verticesY [textureTrianglePIndex [(faceRenderType [triangle] >> 2)]] / model_scale), (float)this.verticesZ [textureTrianglePIndex [(faceRenderType [triangle] >> 2)]] / model_scale));
					AddVertex (new Vector3 ((float)this.verticesX [textureTriangleMIndex [(faceRenderType [triangle] >> 2)]] / model_scale, -((float)this.verticesY [textureTriangleMIndex [(faceRenderType [triangle] >> 2)]] / model_scale), (float)this.verticesZ [textureTriangleMIndex [(faceRenderType [triangle] >> 2)]] / model_scale));
					AddVertex (new Vector3 ((float)this.verticesX [textureTriangleNIndex [(faceRenderType [triangle] >> 2)]] / model_scale, -((float)this.verticesY [textureTriangleNIndex [(faceRenderType [triangle] >> 2)]] / model_scale), (float)this.verticesZ [textureTriangleNIndex [(faceRenderType [triangle] >> 2)]] / model_scale));				
					if (triangleNormals != null && triangleNormals [triangle] != null) {
						Vector3 normalT = new Vector3 (triangleNormals [triangle].x / 255.0f, -(triangleNormals [triangle].y / 255.0f), triangleNormals [triangle].z / 255.0f);
						AddNormal (normalT);
						AddNormal (normalT);
						AddNormal (normalT);
					} else {
						Vector3 normalA = new Vector3 (isolatedVertexNormals [this.triangleViewspaceX [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceX [triangle]].y), isolatedVertexNormals [this.triangleViewspaceX [triangle]].z);
						Vector3 normalB = new Vector3 (isolatedVertexNormals [this.triangleViewspaceY [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceY [triangle]].y), isolatedVertexNormals [this.triangleViewspaceY [triangle]].z);
						Vector3 normalC = new Vector3 (isolatedVertexNormals [this.triangleViewspaceZ [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceZ [triangle]].y), isolatedVertexNormals [this.triangleViewspaceZ [triangle]].z);
						AddNormal (normalA);
						AddNormal (normalB);
						AddNormal (normalC);
					}
					break;
				}
			}
			returnMesh.vertices = _vertices;
			returnMesh.normals = _normals;
			mandm.mesh = returnMesh;
			mandm.materials = new Material[1]{UnityClient.getMaterial (0)};
			return mandm;
		}


		returnMesh.Clear ();
		if (faceTexture == null)
			faceTexture = new int[numVertices];

		if (this.faceTexture != null && this.faceTexture.Length > 0) {
			textureMap.convertTextureCoordinates (this);
		}

		calculateNormals508 ();
		_vertices = new Vector3[numTriangles * 3];
		_normals = new Vector3[numTriangles * 3];
		_uv = new Vector2[numTriangles * 3];
		_colors = new Color32[numTriangles * 3];
		_triangles = new int[10][];
		for(int i = 0; i < _triangles.Length; ++i) _triangles[i] = new int[numTriangles * 3];
		_triangleAlphas = new int[numTriangles * 3];
		int triangleType;
		bool isTextured = false;
		int textureId = -1;
		int alphaTri = 0;
		for (int triangle = 0; triangle < numTriangles; ++triangle) {
			if (faceRenderType == null) {
				triangleType = 0;
			} else {
				triangleType = faceRenderType [triangle] & 3;
			}
			if (this.faceTexture != null && this.faceTexture.Length > triangle)
				isTextured = true;
			if (isTextured) {
				textureId = this.faceTexture [triangle] & 0xffff;//a lot of Ids are WRONG
			}
				
//				alphaTri = (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle]);
//					if(alphaTri == 0) goto skip;
				
			switch (triangleType) {
			case 0://shaded triangle
				int colorA = RS2Sharp.Texture.hsl2rgb [textureTrianglePIndex [triangle] & 0xffff];
				int colorB = RS2Sharp.Texture.hsl2rgb [textureTriangleMIndex [triangle] & 0xffff];
				int colorC = RS2Sharp.Texture.hsl2rgb [textureTriangleNIndex [triangle] & 0xffff];
				if (isTextured) {
					AddUv (new Vector3 (textureMap.textureCoordU [triangle].x, textureMap.textureCoordV [triangle].x, 0), 0);
				} else {
					AddUv (new Vector3 (1, 1, 0), 0);
				}
				int a = AddVertex (new Vector3 ((float)this.verticesX [this.triangleViewspaceX [triangle]] / model_scale, -((float)this.verticesY [this.triangleViewspaceX [triangle]] / model_scale), (float)this.verticesZ [this.triangleViewspaceX [triangle]] / model_scale));
				if (isTextured) {
					AddUv (new Vector3 (textureMap.textureCoordU [triangle].y, textureMap.textureCoordV [triangle].y, 0), 1);
				} else {
					AddUv (new Vector3 (1, 1, 0), 1);
				}
				int b = AddVertex (new Vector3 ((float)this.verticesX [this.triangleViewspaceY [triangle]] / model_scale, -((float)this.verticesY [this.triangleViewspaceY [triangle]] / model_scale), (float)this.verticesZ [this.triangleViewspaceY [triangle]] / model_scale));
				if (isTextured) {
					AddUv (new Vector3 (textureMap.textureCoordU [triangle].z, textureMap.textureCoordV [triangle].z, 0), 2);
					
				} else {
					AddUv (new Vector3 (1, 1, 0), 2);
				}
				if (triangleNormals != null && triangleNormals [triangle] != null) {
					Vector3 normalT = new Vector3 (triangleNormals [triangle].x / 255.0f, -(triangleNormals [triangle].y / 255.0f), triangleNormals [triangle].z / 255.0f);
					AddNormal (normalT);
					AddNormal (normalT);
					AddNormal (normalT);
				} else {
					Vector3 normalA = new Vector3 (isolatedVertexNormals [this.triangleViewspaceX [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceX [triangle]].y), isolatedVertexNormals [this.triangleViewspaceX [triangle]].z);
					Vector3 normalB = new Vector3 (isolatedVertexNormals [this.triangleViewspaceY [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceY [triangle]].y), isolatedVertexNormals [this.triangleViewspaceY [triangle]].z);
					Vector3 normalC = new Vector3 (isolatedVertexNormals [this.triangleViewspaceZ [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceZ [triangle]].y), isolatedVertexNormals [this.triangleViewspaceZ [triangle]].z);
					AddNormal (normalA);
					AddNormal (normalB);
					AddNormal (normalC);
				}
				int c = AddVertex (new Vector3 ((float)this.verticesX [this.triangleViewspaceZ [triangle]] / model_scale, -((float)this.verticesY [this.triangleViewspaceZ [triangle]] / model_scale), (float)this.verticesZ [this.triangleViewspaceZ [triangle]] / model_scale));
				int alpha = (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle]);
				AddTriangle (a, textureId, alpha);
				AddTriangle (b, textureId, alpha);
				AddTriangle (c, textureId, alpha);
//				if (textureId == 40) {
//					AddColor (new Color32 (180, 180, 180, (byte)(faceAlpha == null || faceAlpha.Length <= triangle? 255 : ~faceAlpha [triangle])));
//					AddColor (new Color32 (180, 180, 180, (byte)(faceAlpha == null || faceAlpha.Length <= triangle? 255 : ~faceAlpha [triangle])));
//					AddColor (new Color32 (180, 180, 180, (byte)(faceAlpha == null || faceAlpha.Length <= triangle? 255 : ~faceAlpha [triangle])));
//				} else if (textureId == 30 || textureId == 8 || (textureId > 930 && textureId < 968)) {
//					AddColor (new Color32 (255, 255, 255, (byte)(faceAlpha == null || faceAlpha.Length <= triangle? 255 : ~faceAlpha [triangle])), false);
//					AddColor (new Color32 (255, 255, 255, (byte)(faceAlpha == null || faceAlpha.Length <= triangle? 255 : ~faceAlpha [triangle])), false);
//					AddColor (new Color32 (255, 255, 255, (byte)(faceAlpha == null || faceAlpha.Length <= triangle? 255 : ~faceAlpha [triangle])), false);
//				} else {
					AddColor (new Color32 ((byte)(colorA >> 16), (byte)(colorA >> 8), (byte)(colorA & 0xFF), (byte)(faceAlpha == null || faceAlpha.Length <= triangle? 255 : ~faceAlpha [triangle])));
					AddColor (new Color32 ((byte)(colorB >> 16), (byte)(colorB >> 8), (byte)(colorB & 0xFF), (byte)(faceAlpha == null || faceAlpha.Length <= triangle? 255 : ~faceAlpha [triangle])));
					AddColor (new Color32 ((byte)(colorC >> 16), (byte)(colorC >> 8), (byte)(colorC & 0xFF), (byte)(faceAlpha == null || faceAlpha.Length <= triangle? 255 : ~faceAlpha [triangle])));
					
				//}
				
				break;
			case 1://flat triangle
				if(faceTexture.Length <= triangle) faceTexture = new int[triangle+1];
				int color = RS2Sharp.Texture.hsl2rgb [faceTexture [triangle] & 0xffff];
				if (isTextured) {
					AddUv (new Vector3 (textureMap.textureCoordU [triangle].x, textureMap.textureCoordV [triangle].x, 0), 0);
				} else {
					AddUv (new Vector3 (1, 1, 0), 0);
				}
				int a2 = AddVertex (new Vector3 ((float)this.verticesX [this.triangleViewspaceX [triangle]] / model_scale, -((float)this.verticesY [this.triangleViewspaceX [triangle]] / model_scale), (float)this.verticesZ [this.triangleViewspaceX [triangle]] / model_scale));
				if (isTextured) {
					AddUv (new Vector3 (textureMap.textureCoordU [triangle].y, textureMap.textureCoordV [triangle].y, 0), 1);
				} else {
					AddUv (new Vector3 (1, 1, 0), 1);
				}
				int b4 = AddVertex (new Vector3 ((float)this.verticesX [this.triangleViewspaceY [triangle]] / model_scale, -((float)this.verticesY [this.triangleViewspaceY [triangle]] / model_scale), (float)this.verticesZ [this.triangleViewspaceY [triangle]] / model_scale));
				if (isTextured) {
					AddUv (new Vector3 (textureMap.textureCoordU [triangle].z, textureMap.textureCoordV [triangle].z, 0), 2);
						
				} else {
					AddUv (new Vector3 (1, 1, 0), 2);
				}
				if (triangleNormals != null && triangleNormals [triangle] != null) {
					Vector3 normalT = new Vector3 (triangleNormals [triangle].x / 255.0f, -(triangleNormals [triangle].y / 255.0f), triangleNormals [triangle].z / 255.0f);
					AddNormal (normalT);
					AddNormal (normalT);
					AddNormal (normalT);
				} else {
					Vector3 normalA = new Vector3 (isolatedVertexNormals [this.triangleViewspaceX [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceX [triangle]].y), isolatedVertexNormals [this.triangleViewspaceX [triangle]].z);
					Vector3 normalB = new Vector3 (isolatedVertexNormals [this.triangleViewspaceY [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceY [triangle]].y), isolatedVertexNormals [this.triangleViewspaceY [triangle]].z);
					Vector3 normalC = new Vector3 (isolatedVertexNormals [this.triangleViewspaceZ [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceZ [triangle]].y), isolatedVertexNormals [this.triangleViewspaceZ [triangle]].z);
					AddNormal (normalA);
					AddNormal (normalB);
					AddNormal (normalC);
				}
				int c3 = AddVertex (new Vector3 ((float)this.verticesX [this.triangleViewspaceZ [triangle]] / model_scale, -((float)this.verticesY [this.triangleViewspaceZ [triangle]] / model_scale), (float)this.verticesZ [this.triangleViewspaceZ [triangle]] / model_scale));
				int alpha3 = (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle]);
				AddTriangle (a2, textureId, alpha3);
				AddTriangle (b4, textureId, alpha3);
				AddTriangle (c3, textureId, alpha3);
//				if (textureId == 40) {
//					AddColor (new Color32 (180, 180, 180, (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])));
//					AddColor (new Color32 (180, 180, 180, (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])));
//					AddColor (new Color32 (180, 180, 180, (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])));
//				} else if (textureId == 30 || textureId == 8 || (textureId > 930 && textureId < 968)) {
//					AddColor (new Color32 (255, 255, 255, (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])), false);
//					AddColor (new Color32 (255, 255, 255, (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])), false);
//					AddColor (new Color32 (255, 255, 255, (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])), false);
//				} else {
					AddColor (new Color32 ((byte)(color >> 16), (byte)(color >> 8), (byte)(color & 0xFF), (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])));
					AddColor (new Color32 ((byte)(color >> 16), (byte)(color >> 8), (byte)(color & 0xFF), (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])));
					AddColor (new Color32 ((byte)(color >> 16), (byte)(color >> 8), (byte)(color & 0xFF), (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])));
						
			//	}
					
				break;
			case 2://textured?
				int colorAA = RS2Sharp.Texture.hsl2rgb [textureTrianglePIndex [triangle] & 0xffff];
				int colorBB = RS2Sharp.Texture.hsl2rgb [textureTriangleMIndex [triangle] & 0xffff];
				int colorCC = RS2Sharp.Texture.hsl2rgb [textureTriangleNIndex [triangle] & 0xffff];
				int ptr = faceRenderType [triangle] >> 2;
				int tex_point_a = this.triangleViewspaceX [triangle];//textureTrianglePIndex [ptr];
				int tex_point_b = this.triangleViewspaceY [triangle];//textureTriangleMIndex [ptr];
				int tex_point_c = this.triangleViewspaceZ [triangle];//textureTriangleNIndex [ptr];
				
				if (isTextured)
					AddUv (new Vector3 (textureMap.textureCoordU [triangle].x, textureMap.textureCoordV [triangle].x, 0), 0);
				else
					AddUv (new Vector3 (1, 1, 0), 0);
				int aa = AddVertex (new Vector3 ((float)this.verticesX [tex_point_a] / model_scale, -((float)this.verticesY [tex_point_a] / model_scale), (float)this.verticesZ [tex_point_a] / model_scale));
				if (isTextured)
					AddUv (new Vector3 (textureMap.textureCoordU [triangle].y, textureMap.textureCoordV [triangle].y, 0), 1);
				else
					AddUv (new Vector3 (1, 1, 0), 1);
				int bb = AddVertex (new Vector3 ((float)this.verticesX [tex_point_b] / model_scale, -((float)this.verticesY [tex_point_b] / model_scale), (float)this.verticesZ [tex_point_b] / model_scale));
				if (isTextured)
					AddUv (new Vector3 (textureMap.textureCoordU [triangle].z, textureMap.textureCoordV [triangle].z, 0), 2);
				else
					AddUv (new Vector3 (1, 1, 0), 2);
				int cc = AddVertex (new Vector3 ((float)this.verticesX [tex_point_c] / model_scale, -((float)this.verticesY [tex_point_c] / model_scale), (float)this.verticesZ [tex_point_c] / model_scale));
				int alpha2 = (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle]);
				if (triangleNormals != null && triangleNormals [triangle] != null) {
					Vector3 normalT = new Vector3 (triangleNormals [triangle].x / 255.0f, -(triangleNormals [triangle].y / 255.0f), triangleNormals [triangle].z / 255.0f);
					AddNormal (normalT);
					AddNormal (normalT);
					AddNormal (normalT);
				} else {
					Vector3 normalA = new Vector3 (isolatedVertexNormals [this.triangleViewspaceX [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceX [triangle]].y), isolatedVertexNormals [this.triangleViewspaceX [triangle]].z);
					Vector3 normalB = new Vector3 (isolatedVertexNormals [this.triangleViewspaceY [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceY [triangle]].y), isolatedVertexNormals [this.triangleViewspaceY [triangle]].z);
					Vector3 normalC = new Vector3 (isolatedVertexNormals [this.triangleViewspaceZ [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceZ [triangle]].y), isolatedVertexNormals [this.triangleViewspaceZ [triangle]].z);
					AddNormal (normalA);
					AddNormal (normalB);
					AddNormal (normalC);
				}
				AddTriangle (aa, textureId, alpha2);
				AddTriangle (bb, textureId, alpha2);
				AddTriangle (cc, textureId, alpha2);
//				if (textureId == 40) {
//					AddColor (new Color32 (180, 180, 180, (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])));
//					AddColor (new Color32 (180, 180, 180, (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])));
//					AddColor (new Color32 (180, 180, 180, (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])));
//				} else if (textureId == 30 || textureId == 8 || (textureId > 930 && textureId < 968)) {
//					AddColor (new Color32 (255, 255, 255, (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])), false);
//					AddColor (new Color32 (255, 255, 255, (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])), false);
//					AddColor (new Color32 (255, 255, 255, (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])), false);
//				} else {
					AddColor (new Color32 ((byte)(colorAA >> 16), (byte)(colorAA >> 8), (byte)(colorAA & 0xFF), (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])));
					AddColor (new Color32 ((byte)(colorBB >> 16), (byte)(colorBB >> 8), (byte)(colorBB & 0xFF), (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])));
					AddColor (new Color32 ((byte)(colorCC >> 16), (byte)(colorCC >> 8), (byte)(colorCC & 0xFF), (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])));
				//}
				break;
			case 3://textured?
							int color3 = RS2Sharp.Texture.hsl2rgb [textureTrianglePIndex [triangle] & 0xffff];
				int ptr3 = faceRenderType [triangle] >> 2;
				int tex_point_aa = this.triangleViewspaceX [triangle];//textureTrianglePIndex [ptr3];
				int tex_point_bb = this.triangleViewspaceY [triangle];//textureTriangleMIndex [ptr3];
				int tex_point_cc = this.triangleViewspaceZ [triangle];//textureTriangleNIndex [ptr3];
				if (isTextured)
					AddUv (new Vector3 (textureMap.textureCoordU [triangle].x, textureMap.textureCoordV [triangle].x, 0), 0);
				else
					AddUv (new Vector3 (1, 1, 0), 0);
				int aaa = AddVertex (new Vector3 ((float)this.verticesX [tex_point_aa] / model_scale, -((float)this.verticesY [tex_point_aa] / model_scale), (float)this.verticesZ [tex_point_aa] / model_scale));
				if (isTextured)
					AddUv (new Vector3 (textureMap.textureCoordU [triangle].y, textureMap.textureCoordV [triangle].y, 0), 1);
				else
					AddUv (new Vector3 (1, 1, 0), 1);
				int bbb = AddVertex (new Vector3 ((float)this.verticesX [tex_point_bb] / model_scale, -((float)this.verticesY [tex_point_bb] / model_scale), (float)this.verticesZ [tex_point_bb] / model_scale));
				if (isTextured)
					AddUv (new Vector3 (textureMap.textureCoordU [triangle].z, textureMap.textureCoordV [triangle].z, 0), 2);
				else
					AddUv (new Vector3 (1, 1, 0), 2);
				int ccc = AddVertex (new Vector3 ((float)this.verticesX [tex_point_cc] / model_scale, -((float)this.verticesY [tex_point_cc] / model_scale), (float)this.verticesZ [tex_point_cc] / model_scale));
				int alpha22 = (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle]);
				if (triangleNormals != null && triangleNormals [triangle] != null) {
					Vector3 normalT = new Vector3 (triangleNormals [triangle].x / 255.0f, -(triangleNormals [triangle].y / 255.0f), triangleNormals [triangle].z / 255.0f);
					AddNormal (normalT);
					AddNormal (normalT);
					AddNormal (normalT);
				} else {
					Vector3 normalA = new Vector3 (isolatedVertexNormals [this.triangleViewspaceX [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceX [triangle]].y), isolatedVertexNormals [this.triangleViewspaceX [triangle]].z);
					Vector3 normalB = new Vector3 (isolatedVertexNormals [this.triangleViewspaceY [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceY [triangle]].y), isolatedVertexNormals [this.triangleViewspaceY [triangle]].z);
					Vector3 normalC = new Vector3 (isolatedVertexNormals [this.triangleViewspaceZ [triangle]].x, -(isolatedVertexNormals [this.triangleViewspaceZ [triangle]].y), isolatedVertexNormals [this.triangleViewspaceZ [triangle]].z);
					AddNormal (normalA);
					AddNormal (normalB);
					AddNormal (normalC);
				}
				AddTriangle (aaa, textureId, alpha22);
				AddTriangle (bbb, textureId, alpha22);
				AddTriangle (ccc, textureId, alpha22);
//				if (textureId == 40) {
//					AddColor (new Color32 (180, 180, 180, (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])));
//					AddColor (new Color32 (180, 180, 180, (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])));
//					AddColor (new Color32 (180, 180, 180, (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])));
//				} else if (textureId == 30 || textureId == 8 || (textureId > 930 && textureId < 968)) {
//					AddColor (new Color32 (255, 255, 255, (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])), false);
//					AddColor (new Color32 (255, 255, 255, (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])), false);
//					AddColor (new Color32 (255, 255, 255, (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])), false);
//				} else {
					AddColor (new Color32 ((byte)(color3 >> 16), (byte)(color3 >> 8), (byte)(color3 & 0xFF), (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])));
					AddColor (new Color32 ((byte)(color3 >> 16), (byte)(color3 >> 8), (byte)(color3 & 0xFF), (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])));
					AddColor (new Color32 ((byte)(color3 >> 16), (byte)(color3 >> 8), (byte)(color3 & 0xFF), (byte)(faceAlpha == null || faceAlpha.Length <= triangle ? 255 : ~faceAlpha [triangle])));
				//}
				break;
			}
			skip: int dummy = 0;
		}
		if (_vertices.Length > 64000)
			return null;
		returnMesh.vertices = _vertices;
		returnMesh.uv = _uv;

		returnMesh.subMeshCount = subMeshIndex;
		for(int a = 0; a < subMeshIndex; ++a)
		{
			if(_triangles[a] != null && _triangles[a].Length > 0)
			{
				int[] newTriangles = new int[_triangles[a].Length];
				int idx = _triangles[a].Length-1;
				for(int i = 0; i < _triangles[a].Length; ++i)
				{
					newTriangles[idx] = _triangles[a][i];
					idx--;
				}
				returnMesh.SetTriangles(newTriangles,a);
			}
		}

		returnMesh.colors32 = _colors;
		returnMesh.normals = _normals;

		mandm.mesh = returnMesh;
		mandm.materials = new Material[subMeshIndex];
		for(int i = 0; i < subMeshIndex; ++i)
		{
			int texId = 0;
			textureTable.TryGetValue(i, out texId);
			mandm.materials[i] = UnityClient.getMaterial(texId);
		}
		//if(alphaCount == 0) mandm.materials = new Material[1]{UnityClient.getMaterial (0)};
		//else mandm.materials = new Material[2]{UnityClient.getMaterial (0),UnityClient.alphaObject};
		return mandm;
	}

	public void calculateNormals508 ()
	{
		isolatedVertexNormals = null;
		if (isolatedVertexNormals == null) {
			isolatedVertexNormals = new VertexNormal[numVertices];
			for (int i = 0; i < numVertices; i++)
				isolatedVertexNormals [i] = new VertexNormal ();
			for (int i = 0; i < numTriangles; i++) {
				int i_157_ = triangleViewspaceX [i];
				int i_158_ = triangleViewspaceY [i];
				int i_159_ = triangleViewspaceZ [i];
				int i_160_ = verticesX [i_158_] - verticesX [i_157_];
				int i_161_ = verticesY [i_158_] - verticesY [i_157_];
				int i_162_ = verticesZ [i_158_] - verticesZ [i_157_];
				int i_163_ = verticesX [i_159_] - verticesX [i_157_];
				int i_164_ = verticesY [i_159_] - verticesY [i_157_];
				int i_165_ = verticesZ [i_159_] - verticesZ [i_157_];
				int i_166_ = i_161_ * i_165_ - i_164_ * i_162_;
				int i_167_ = i_162_ * i_163_ - i_165_ * i_160_;
				int i_168_;
				for (i_168_ = i_160_ * i_164_ - i_163_ * i_161_;
				     (i_166_ > 8192 || i_167_ > 8192 || i_168_ > 8192
				 || i_166_ < -8192 || i_167_ < -8192 || i_168_ < -8192);
				     i_168_ >>= 1) {
					i_166_ >>= 1;
					i_167_ >>= 1;
				}
				int i_169_ = (int)Mathf.Sqrt ((float)(i_166_ * i_166_
					+ i_167_ * i_167_
					+ i_168_ * i_168_));
				if (i_169_ <= 0)
					i_169_ = 1;
				i_166_ = i_166_ * 256 / i_169_;
				i_167_ = i_167_ * 256 / i_169_;
				i_168_ = i_168_ * 256 / i_169_;
				int i_170_;
				if (faceRenderType == null)
					i_170_ = (byte)0;
				else
					i_170_ = faceRenderType [i] & 1;
				if (i_170_ == 0) {
					VertexNormal vertexNormal = isolatedVertexNormals [i_157_];
					vertexNormal.x += i_166_;
					vertexNormal.y += i_167_;
					vertexNormal.z += i_168_;
					vertexNormal.magnitude++;
					vertexNormal = isolatedVertexNormals [i_158_];
					vertexNormal.x += i_166_;
					vertexNormal.y += i_167_;
					vertexNormal.z += i_168_;
					vertexNormal.magnitude++;
					vertexNormal = isolatedVertexNormals [i_159_];
					vertexNormal.x += i_166_;
					vertexNormal.y += i_167_;
					vertexNormal.z += i_168_;
					vertexNormal.magnitude++;
				} else if (i_170_ == 1) {
					if (triangleNormals == null || triangleNormals.Length != numTriangles)
						triangleNormals = new TriangleNormal[numTriangles];
					TriangleNormal triangleNormal = triangleNormals [i] = new TriangleNormal ();
					triangleNormal.x = i_166_;
					triangleNormal.y = i_167_;
					triangleNormal.z = i_168_;
				}
			}
		}
	}

	public void calculateMeshTangents (Mesh mesh)
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
	int vertIndex = 0;
	public int AddVertex (Vector3 vertex)
	{
		_vertices[vertIndex] = (vertex * 0.0078125f);
		vertIndex++;
		vertexCount++;
		return vertexCount - 1;
	}
	int normalIndex = 0;
	public void AddNormal (Vector3 normal)
	{
		_normals[normalIndex] = normal;
		normalIndex++;
	}
	int uvIndex = 0;
	public void AddUv (Vector2 uv, int count)
	{
		_uv[uvIndex] = uv;
		uvIndex++;
	}
	int[] triangleIndex = new int[10];
	public void AddTriangle (int triangle, int texture, int alpha)
	{
		int submeshId = GetSubMesh(texture);
		_triangles[submeshId][triangleIndex[submeshId]++] = triangle;
		triangleCount++;
	}
	
	private bool containsAlpha = false;
	private int vertexCount = 0;
	private int triangleCount = 0;
	private Vector3[] _vertices;
	private Vector3[] _normals;
	private Vector2[] _uv;
	private Color32[] _colors;
	private int[][] _triangles;
	private int subMeshIndex = 0;
	private int subMeshCount = 0;
	private int[] _triangleAlphas;
	
	private Dictionary<int,int> subMeshTable = new Dictionary<int,int>();
	private Dictionary<int,int> textureTable = new Dictionary<int,int>();
	private int GetSubMesh (int textureId)
	{
		int subMeshId = 0;
		foreach(int key in subMeshTable.Keys) if(key == textureId) 
		{
			subMeshTable.TryGetValue(key, out subMeshId);
			return subMeshId;
		}
		subMeshTable.Add(textureId,subMeshIndex++);
		textureTable.Add (subMeshIndex-1,textureId);
		return subMeshIndex-1;
	}
	
	int colorIndex = 0;
	public void AddColor (Color32 colorToAdd, bool shouldChange = true)
	{
		_colors[colorIndex] = colorToAdd;
		colorIndex++;
	}

}
