using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RS2Sharp
{
	public class ModelOld : Animable
	{
//		public VertexNormal[] vns;
//		public int[] triangleNormalX;
//		public int[] triangleNormalY;
//		public int[] triangleNormalZ;
//		public short[] triangleTexture;
//		public TriangleNormal[] triangleNormals;
//		public long hash;
//		public static void nullLoader() {
//			modelHeaderCache = null;
//			aboolArray1663 = null;
//			aboolArray1664 = null;
//			vertex_screen_x = null;
//			vertex_screen_y = null;
//			vertex_screen_z = null;
//			vertexMvX = null;
//			vertexMvY = null;
//			vertexMvZ = null;
//			depthListIndices = null;
//			faceLists = null;
//			anIntArray1673 = null;
//			anIntArrayArray1674 = null;
//			anIntArray1675 = null;
//			anIntArray1676 = null;
//			anIntArray1677 = null;
//			SINE = null;
//			COSINE = null;
//			HSL2RGB = null;
//			modelIntArray4 = null;
//		}
//		
//		public static void initialize(int i, OnDemandFetcherParent onDemandFetcherParent) {
//			modelHeaderCache = new ModelHeader[i + 70000];//wtf PETER!!!
//			abstractODFetcher = onDemandFetcherParent;
//		}
//		
//		
//		public Model(int modelId, int j) {//unused :/
//			byte[] iss = modelHeaderCache[modelId].modelData;
//			
//			// if(new File(Signlink.findcachedir()+"/converted633Items/"+modelId+".dat").exists()){
//			//       readCustomFormat(FileOperations.ReadFile(Signlink.findcachedir()+"/converted633Items/"+modelId+".dat"),modelId);
//			//readCustomFormat(FileOperations.ReadFile(Signlink.findcachedir()+"/converted633Items/534.dat"),modelId);
//			//  }        else{
//			
//			if (iss[iss.Length - 1] == -1 && iss[iss.Length - 2] == -1)
//				method459(iss, modelId);
//			else
//				readOldModel(modelId);
//			//  }
//		}
//		
//		
//		public void readCustomFormat(byte[] data, int modelId)
//		{//Clienthaxs format
//			Stream stream = new Stream(data);
//			numVertices = stream.g2();
//			verticesX = new int[numVertices];
//			verticesY = new int[numVertices];
//			verticesZ = new int[numVertices];
//			
//			for(int v = 0; v < numVertices; v++) {
//				verticesX[v] = stream.gsmart();
//				verticesY[v] = stream.gsmart();
//				verticesZ[v] = stream.gsmart();
//			}
//			
//			bool hasVSkin = stream.g1() == 1;
//			if(hasVSkin)
//			{
//				vertexVSkin = new int[numVertices];
//				for(int v = 0; v < numVertices; v++) {
//					vertexVSkin[v] = stream.g1();
//				}
//			}
//			
//			numTriangles = stream.g2();
//			triangleColourOrTexture = new int[numTriangles];
//			
//			for(int t = 0; t < numTriangles; t++) {
//				triangleColourOrTexture[t] = stream.g2();
//			}
//			
//			bool hasDrawTypes = stream.g1() == 1;
//			if(hasDrawTypes)
//			{
//				triangleDrawType = new int[numTriangles];
//				for(int t = 0; t < numTriangles; t++)
//				{
//					triangleDrawType[t] = stream.g2();
//				}
//			}
//			
//			bool hasFacePriorities = stream.g1() == 1;
//			if(hasFacePriorities)
//			{
//				facePriority = new int[numTriangles];
//				for(int t = 0; t < numTriangles; t++)
//				{
//					facePriority[t] = stream.g1();
//				}
//			}
//			
//			bool hasAlpha = stream.g1() == 1;
//			if(hasAlpha)
//			{
//				triangleAlpha = new int[numTriangles];
//				for(int t = 0; t < numTriangles; t++)
//				{
//					triangleAlpha[t] = stream.g1();
//				}
//			}
//			
//			bool hasTSkin = stream.g1() == 1;
//			if(hasTSkin)
//			{
//				triangleTSkin = new int[numTriangles];
//				for(int t = 0; t < numTriangles; t++)
//				{
//					triangleTSkin[t] = stream.g1();
//				}
//			}
//			
//			triangleViewspaceX = new int[numTriangles];
//			triangleViewspaceY = new int[numTriangles];
//			triangleViewspaceZ = new int[numTriangles];
//			
//			for(int t = 0; t < numTriangles; t++)
//			{
//				triangleViewspaceX[t] = stream.gsmart();
//				triangleViewspaceY[t] = stream.gsmart();
//				triangleViewspaceZ[t] = stream.gsmart();
//			}
//			
//			textureTriangleCount = stream.g1();
//			bool hasTextures = stream.g1() == 1;
//			if(hasTextures)
//			{
//				triPIndex = new int[textureTriangleCount];
//				triMIndex = new int[textureTriangleCount];
//				triNIndex = new int[textureTriangleCount];
//				
//				for(int t = 0; t < textureTriangleCount; t++) {//this might be a glitch
//					triPIndex[t] = stream.g2();
//					triMIndex[t] = stream.g2();
//					triNIndex[t] = stream.g2();
//					
//				}
//			}
//		}
//		
//		private void readOldModel(int i) {
//			//int j = -870;
//			//anInt1614 = 9;
//			//abool1615 = false;
//			//anInt1616 = 360;
//			//anInt1617 = 1;
//			//abool1618 = true;
//			aBoolean1659 = false;
//			//anInt1620++;
//			hash = i;
//			ModelHeader class21 = modelHeaderCache[i];
//			numVertices = class21.modelVerticeCount;
//			numTriangles = class21.modelTriangleCount;
//			textureTriangleCount = class21.modelTextureTriangleCount;
//			verticesX = new int[numVertices];//vertex_x
//			verticesY = new int[numVertices];//vy
//			verticesZ = new int[numVertices];//vz
//			triangleViewspaceX = new int[numTriangles];//trianglea
//			triangleViewspaceY = new int[numTriangles];//b
//			triangleViewspaceZ = new int[numTriangles];//c
//			triPIndex = new int[textureTriangleCount];//tri_a_buffer
//			triMIndex = new int[textureTriangleCount];//b
//			triNIndex = new int[textureTriangleCount];//c
//			if (class21.vskinBasePos >= 0)
//				vertexVSkin = new int[numVertices];//vertex_vskin
//			if (class21.drawTypeBasePos >= 0)
//				triangleDrawType = new int[numTriangles];//triangle_draw_type
//			if (class21.facePriorityBasePos >= 0)
//				facePriority = new int[numTriangles];//face_priority
//			else
//				anInt1641 = -class21.facePriorityBasePos - 1;
//			if (class21.alphaBasepos >= 0)//alpha_basepos
//				triangleAlpha = new int[numTriangles];//triangleAlpha
//			if (class21.tskinBasepos >= 0)//tskin_basepos
//				triangleTSkin = new int[numTriangles];//triangle_tskin
//			triangleColourOrTexture = new int[numTriangles];//triangleColour
//			
//			Stream class30_sub2_sub2 = new Stream(class21.modelData);
//			class30_sub2_sub2.pos = class21.vertexModOffset;
//			
//			Stream class30_sub2_sub2_1 = new Stream(class21.modelData);
//			class30_sub2_sub2_1.pos = class21.vertexXOffset;
//			
//			Stream class30_sub2_sub2_2 = new Stream(class21.modelData);
//			class30_sub2_sub2_2.pos = class21.vertexYOffset;
//			
//			Stream class30_sub2_sub2_3 = new Stream(class21.modelData);
//			class30_sub2_sub2_3.pos = class21.vertexZOffset;
//			
//			Stream class30_sub2_sub2_4 = new Stream(class21.modelData);
//			class30_sub2_sub2_4.pos = class21.vskinBasePos;
//			
//			int k = 0;
//			int l = 0;
//			int i1 = 0;
//			for (int j1 = 0; j1 < numVertices; j1++) {
//				int k1 = class30_sub2_sub2.g1();
//				int i2 = 0;
//				if ((k1 & 1) != 0)
//					i2 = class30_sub2_sub2_1.gsmart();
//				int k2 = 0;
//				if ((k1 & 2) != 0)
//					k2 = class30_sub2_sub2_2.gsmart();
//				int i3 = 0;
//				if ((k1 & 4) != 0)
//					i3 = class30_sub2_sub2_3.gsmart();
//				verticesX[j1] = k + i2;
//				verticesY[j1] = l + k2;
//				verticesZ[j1] = i1 + i3;
//				k = verticesX[j1];
//				l = verticesY[j1];
//				i1 = verticesZ[j1];
//				if (vertexVSkin != null)
//					vertexVSkin[j1] = class30_sub2_sub2_4.g1();
//			}
//			
//			class30_sub2_sub2.pos = class21.triColourOffset;
//			class30_sub2_sub2_1.pos = class21.drawTypeBasePos;
//			class30_sub2_sub2_2.pos = class21.facePriorityBasePos;
//			class30_sub2_sub2_3.pos = class21.alphaBasepos;
//			class30_sub2_sub2_4.pos = class21.tskinBasepos;
//			for (int l1 = 0; l1 < numTriangles; l1++) {
//				triangleColourOrTexture[l1] = class30_sub2_sub2.g2();
//				if (triangleDrawType != null)
//				{
//					triangleDrawType[l1] = class30_sub2_sub2_1.g1();
//				}
//				if (facePriority != null)
//					facePriority[l1] = class30_sub2_sub2_2.g1();
//				if (triangleAlpha != null) {
//					triangleAlpha[l1] = class30_sub2_sub2_3.g1();
//				}
//				if (triangleTSkin != null)
//					triangleTSkin[l1] = class30_sub2_sub2_4.g1();
//			}
//			
//			class30_sub2_sub2.pos = class21.triVPointOffset;
//			class30_sub2_sub2_1.pos = class21.triMeshLinkOffset;
//			int j2 = 0;
//			int l2 = 0;
//			int j3 = 0;
//			int k3 = 0;
//			for (int l3 = 0; l3 < numTriangles; l3++) {
//				int i4 = class30_sub2_sub2_1.g1();
//				if (i4 == 1) {
//					j2 = class30_sub2_sub2.gsmart() + k3;
//					k3 = j2;
//					l2 = class30_sub2_sub2.gsmart() + k3;
//					k3 = l2;
//					j3 = class30_sub2_sub2.gsmart() + k3;
//					k3 = j3;
//					triangleViewspaceX[l3] = j2;
//					triangleViewspaceY[l3] = l2;
//					triangleViewspaceZ[l3] = j3;
//				}
//				if (i4 == 2) {
//					//j2 = j2;
//					l2 = j3;
//					j3 = class30_sub2_sub2.gsmart() + k3;
//					k3 = j3;
//					triangleViewspaceX[l3] = j2;
//					triangleViewspaceY[l3] = l2;
//					triangleViewspaceZ[l3] = j3;
//				}
//				if (i4 == 3) {
//					j2 = j3;
//					//l2 = l2;
//					j3 = class30_sub2_sub2.gsmart() + k3;
//					k3 = j3;
//					triangleViewspaceX[l3] = j2;
//					triangleViewspaceY[l3] = l2;
//					triangleViewspaceZ[l3] = j3;
//				}
//				if (i4 == 4) {
//					int k4 = j2;
//					j2 = l2;
//					l2 = k4;
//					j3 = class30_sub2_sub2.gsmart() + k3;
//					k3 = j3;
//					triangleViewspaceX[l3] = j2;
//					triangleViewspaceY[l3] = l2;
//					triangleViewspaceZ[l3] = j3;
//				}
//			}
//			
//			class30_sub2_sub2.pos = class21.textureInfoBasePos;
//			for (int j4 = 0; j4 < textureTriangleCount; j4++) {
//				triPIndex[j4] = class30_sub2_sub2.g2();
//				triMIndex[j4] = class30_sub2_sub2.g2();
//				triNIndex[j4] = class30_sub2_sub2.g2();
//			}
//		}
//		
//		public void method459(byte[] abyte0, int modelID) {
//			
//			int anInt2259 = 0;
//			int anInt2265 = 0;
//			byte[] textureDrawType = null;
//			byte[] aByteArray2253 = null;
//			byte[] aByteArray2255 = null;
//			int[] anIntArray2238 = null;
//			int[] anIntArray2228 = null;
//			int[] anIntArray2241 = null;
//			short[] aShortArray2237 = null;
//			int[] anIntArray2272 = null;
//			int[] anIntArray2261 = null;
//			int[] anIntArray2225 = null;
//			sbyte[] aByteArray2263 = null;
//			
//			
//			hash = modelID;
//			
//			Stream rsbuffer = new Stream(abyte0);
//			Stream rsbuffer1 = new Stream(abyte0);
//			Stream rsbuffer2 = new Stream(abyte0);
//			Stream rsbuffer3 = new Stream(abyte0);
//			Stream rsbuffer4 = new Stream(abyte0);
//			Stream rsbuffer5 = new Stream(abyte0);
//			Stream rsbuffer6 = new Stream(abyte0);
//			rsbuffer.pos = -23 + abyte0.Length;
//			int vertexCount = rsbuffer.g2();
//			int triangleCount = rsbuffer.g2();
//			int modelTextureTriangleCount = rsbuffer.g1();
//			
//			ModelHeader ModelDef_1 = modelHeaderCache[modelID] = new ModelHeader();
//			ModelDef_1.modelData = abyte0;
//			ModelDef_1.modelVerticeCount = vertexCount;
//			ModelDef_1.modelTriangleCount = triangleCount;
//			ModelDef_1.modelTextureTriangleCount = modelTextureTriangleCount;
//			
//			int i = rsbuffer.g1();
//			bool flag = (1 & i) == 1;
//			bool flag1 = (2 & i) == 2;
//			bool flag2 = (4 & i) == 4;
//			bool flag3 = (i & 8) == 8;
//			if(flag3)
//			{
//				rsbuffer.pos -= 7;
//				anInt2265 = rsbuffer.g1();
//				rsbuffer.pos += 6;
//			}
//			int j = rsbuffer.g1();
//			int k = rsbuffer.g1();
//			int l = rsbuffer.g1();
//			int i1 = rsbuffer.g1();
//			int j1 = rsbuffer.g1();
//			int k1 = rsbuffer.g2();
//			int l1 = rsbuffer.g2();
//			int i2 = rsbuffer.g2();
//			int j2 = rsbuffer.g2();
//			int k2 = rsbuffer.g2();
//			int l2 = 0;
//			int i3 = 0;
//			int j3 = 0;
//			if(~modelTextureTriangleCount < -1)
//			{
//				rsbuffer.pos = 0;
//				textureDrawType = new byte[modelTextureTriangleCount];
//				for(int k3 = 0; ~modelTextureTriangleCount < ~k3; k3++)
//				{
//					byte byte1 = textureDrawType[k3] = rsbuffer.g1b();
//					if(byte1 >= 1 && byte1 <= 3)
//						i3++;
//					if(byte1 == 2)
//						j3++;
//					if(~byte1 == -1)
//						l2++;
//				}
//				
//			}
//			int l3 = modelTextureTriangleCount;
//			int vertexModOffset = l3;
//			l3 += vertexCount;
//			int triangleDrawTypeBasePos = l3;
//			if(flag)
//				l3 += triangleCount;
//			int triMeshLinkOffset = l3;
//			l3 += triangleCount;
//			int facePriorityBasePos = l3;
//			if(j == 255)
//				l3 += triangleCount;
//			int tskinBasepos = l3;
//			if(l == 1)
//				l3 += triangleCount;
//			int vskinBasePos = l3;
//			if(j1 == 1)
//				l3 += vertexCount;
//			int alphaBasepos = l3;
//			if(k == 1)
//				l3 += triangleCount;
//			int triVPointOffset = l3;
//			l3 += j2;
//			int i6 = l3;
//			if(i1 == 1)
//				l3 += 2 * triangleCount;
//			int j6 = l3;
//			l3 += k2;
//			int triangleColourOrTextureBasePos = l3;
//			l3 += triangleCount * 2;
//			int vertexXOffset = l3;
//			l3 += k1;
//			int vertexYOffset = l3;
//			l3 += l1;
//			int vertexZOffset = l3;
//			l3 += i2;
//			int textureInfoBasePos = l3;
//			l3 += 6 * l2;
//			int l7 = l3;
//			l3 += 6 * i3;
//			byte byte2 = 6;
//			if(anInt2265 == 14)
//				byte2 = 7;
//			else
//				if(anInt2265 >= 15)
//					byte2 = 9;
//			int i8 = l3;
//			l3 += byte2 * i3;
//			int j8 = l3;
//			l3 += i3;
//			int k8 = l3;
//			l3 += i3;
//			int l8 = l3;
//			l3 += i3 - -(j3 * 2);
//			
//			if(k == 1)
//				triangleAlpha = new int[triangleCount];
//			verticesX = new int[vertexCount];
//			rsbuffer.pos = vertexModOffset;
//			if(i1 == 1)
//				aShortArray2237 = new short[triangleCount];
//			verticesZ = new int[vertexCount];
//			if(l == 1)
//				triangleTSkin = new int[triangleCount];
//			int i9 = l3;
//			if(flag)
//				triangleDrawType = new int[triangleCount];
//			triangleViewspaceY = new int[triangleCount];
//			triangleColourOrTexture = new int[triangleCount];
//			if(j1 == 1)
//				vertexVSkin = new int[vertexCount];
//			if(i1 == 1 && modelTextureTriangleCount > 0)
//				aByteArray2263 = new sbyte[triangleCount];
//			verticesY = new int[vertexCount];
//			triangleViewspaceZ = new int[triangleCount];
//			triangleViewspaceX = new int[triangleCount];
//			if(modelTextureTriangleCount > 0)
//			{
//				triPIndex = new int[modelTextureTriangleCount];
//				triMIndex = new int[modelTextureTriangleCount];
//				triNIndex = new int[modelTextureTriangleCount];
//				if(j3 > 0)
//				{
//					anIntArray2241 = new int[j3];
//					anIntArray2228 = new int[j3];
//				}
//				if(~i3 < -1)
//				{
//					aByteArray2255 = new byte[i3];
//					anIntArray2272 = new int[i3];
//					anIntArray2261 = new int[i3];
//					anIntArray2225 = new int[i3];
//					anIntArray2238 = new int[i3];
//					aByteArray2253 = new byte[i3];
//				}
//			}
//			if(j == 255)
//				facePriority = new int[triangleCount];
//			rsbuffer1.pos = vertexXOffset;
//			rsbuffer2.pos = vertexYOffset;
//			rsbuffer3.pos = vertexZOffset;
//			rsbuffer4.pos = vskinBasePos;
//			int j9 = 0;
//			int k9 = 0;
//			int l9 = 0;
//			for(int i10 = 0; ~vertexCount < ~i10; i10++)
//			{
//				int j10 = rsbuffer.g1();
//				int l10 = 0;
//				if((j10 & 1) != 0)
//					l10 = rsbuffer1.gsmart();
//				int j11 = 0;
//				if((j10 & 2) != 0)
//					j11 = rsbuffer2.gsmart();
//				int l11 = 0;
//				if((j10 & 4) != 0)
//					l11 = rsbuffer3.gsmart();
//				verticesX[i10] = l10 + j9;
//				verticesY[i10] = j11 + k9;
//				verticesZ[i10] = l9 - -l11;
//				j9 = verticesX[i10];
//				k9 = verticesY[i10];
//				l9 = verticesZ[i10];
//				if(j1 == 1)
//					vertexVSkin[i10] = rsbuffer4.g1();
//			}
//			
//			rsbuffer.pos = triangleColourOrTextureBasePos;
//			rsbuffer1.pos = triangleDrawTypeBasePos;
//			rsbuffer2.pos = facePriorityBasePos;
//			rsbuffer3.pos = alphaBasepos;
//			rsbuffer4.pos = tskinBasepos;
//			rsbuffer5.pos = i6;
//			rsbuffer6.pos = j6;
//			for(int k10 = 0; triangleCount > k10; k10++)
//			{
//				triangleColourOrTexture[k10] = (short)rsbuffer.g2();
//				if(flag)
//					triangleDrawType[k10] = rsbuffer1.g1b();
//				if(j == 255)
//					facePriority[k10] = rsbuffer2.g1b();
//				if(k == 1)
//					triangleAlpha[k10] = rsbuffer3.g1b();
//				if(l == 1)
//					triangleTSkin[k10] = rsbuffer4.g1();
//				if(i1 == 1)
//					aShortArray2237[k10] = (short)(-1 + rsbuffer5.g2());
//				if(aByteArray2263 != null)
//					if(aShortArray2237[k10] != -1)
//						aByteArray2263[k10] = (sbyte)(-1 + rsbuffer6.g1());
//				else
//					aByteArray2263[k10] = -1;
//			}
//			
//			rsbuffer.pos = triVPointOffset;
//			anInt2259 = -1;
//			rsbuffer1.pos = triMeshLinkOffset;
//			int i11 = 0;
//			int k11 = 0;
//			int i12 = 0;
//			int j12 = 0;
//			for(int k12 = 0; ~triangleCount < ~k12; k12++)
//			{
//				int l12 = rsbuffer1.g1();
//				if(l12 == 1)
//				{
//					i11 = (short)(j12 + rsbuffer.gsmart());
//					j12 = i11;
//					k11 = (short)(j12 + rsbuffer.gsmart());
//					j12 = k11;
//					i12 = (short)(j12 + rsbuffer.gsmart());
//					triangleViewspaceX[k12] = (short) i11;
//					j12 = i12;
//					triangleViewspaceY[k12] = (short) k11;
//					triangleViewspaceZ[k12] = (short) i12;
//					if(i11 > anInt2259)
//						anInt2259 = i11;
//					if(k11 > anInt2259)
//						anInt2259 = k11;
//					if(i12 > anInt2259)
//						anInt2259 = i12;
//				}
//				if(l12 == 2)
//				{
//					k11 = i12;
//					i12 = (short)(j12 + rsbuffer.gsmart());
//					j12 = i12;
//					triangleViewspaceX[k12] = (short) i11;
//					triangleViewspaceY[k12] = (short) k11;
//					triangleViewspaceZ[k12] = (short) i12;
//					if(i12 > anInt2259)
//						anInt2259 = i12;
//				}
//				if(l12 == 3)
//				{
//					i11 = i12;
//					i12 = (short)(rsbuffer.gsmart() + j12);
//					j12 = i12;
//					triangleViewspaceX[k12] = (short) i11;
//					triangleViewspaceY[k12] = (short) k11;
//					triangleViewspaceZ[k12] = (short) i12;
//					if(~anInt2259 > ~i12)
//						anInt2259 = i12;
//				}
//				if(l12 == 4)
//				{
//					int j13 = i11;
//					i11 = k11;
//					i12 = (short)(rsbuffer.gsmart() + j12);
//					k11 = j13;
//					j12 = i12;
//					triangleViewspaceX[k12] = (short) i11;
//					triangleViewspaceY[k12] = (short) k11;
//					triangleViewspaceZ[k12] = (short) i12;
//					if(~anInt2259 > ~i12)
//						anInt2259 = i12;
//				}
//			}
//			
//			anInt2259++;
//			rsbuffer.pos = textureInfoBasePos;
//			rsbuffer1.pos = l7;
//			rsbuffer2.pos = i8;
//			rsbuffer3.pos = j8;
//			rsbuffer4.pos = k8;
//			rsbuffer5.pos = l8;
//			for(int i13 = 0; i13 < modelTextureTriangleCount; i13++)
//			{
//				int readType = 0xff & textureDrawType[i13];
//				if(~readType == -1)
//				{
//					triPIndex[i13] = (short)rsbuffer.g2();
//					triMIndex[i13] = (short)rsbuffer.g2();
//					triNIndex[i13] = (short)rsbuffer.g2();
//				}
//				if(readType == 1)
//				{
//					triPIndex[i13] = (short)rsbuffer1.g2();
//					triMIndex[i13] = (short)rsbuffer1.g2();
//					triNIndex[i13] = (short)rsbuffer1.g2();
//					if(anInt2265 >= 15)
//					{
//						anIntArray2261[i13] = rsbuffer2.g3();
//						anIntArray2272[i13] = rsbuffer2.g3();
//						anIntArray2225[i13] = rsbuffer2.g3();
//					} else
//					{
//						anIntArray2261[i13] = rsbuffer2.g2();
//						if(anInt2265 < 14)
//							anIntArray2272[i13] = rsbuffer2.g2();
//						else
//							anIntArray2272[i13] = rsbuffer2.g3();
//						anIntArray2225[i13] = rsbuffer2.g2();
//					}
//					aByteArray2253[i13] = rsbuffer3.g1b();
//					aByteArray2255[i13] = rsbuffer4.g1b();
//					anIntArray2238[i13] = rsbuffer5.g1b();
//				}
//				if(readType == 2)
//				{
//					triPIndex[i13] = (short)rsbuffer1.g2();
//					triMIndex[i13] = (short)rsbuffer1.g2();
//					triNIndex[i13] = (short)rsbuffer1.g2();
//					if(anInt2265 >= 15)
//					{
//						anIntArray2261[i13] = rsbuffer2.g3();
//						anIntArray2272[i13] = rsbuffer2.g3();
//						anIntArray2225[i13] = rsbuffer2.g3();
//					} else
//					{
//						anIntArray2261[i13] = rsbuffer2.g2();
//						if(anInt2265 < 14)
//							anIntArray2272[i13] = rsbuffer2.g2();
//						else
//							anIntArray2272[i13] = rsbuffer2.g3();
//						anIntArray2225[i13] = rsbuffer2.g2();
//					}
//					aByteArray2253[i13] = rsbuffer3.g1b();
//					aByteArray2255[i13] = rsbuffer4.g1b();
//					anIntArray2238[i13] = rsbuffer5.g1b();
//					anIntArray2228[i13] = rsbuffer5.g1b();
//					anIntArray2241[i13] = rsbuffer5.g1b();
//				}
//				if(readType == 3)
//				{
//					triPIndex[i13] = (short)rsbuffer1.g2();
//					triMIndex[i13] = (short)rsbuffer1.g2();
//					triNIndex[i13] = (short)rsbuffer1.g2();
//					if(anInt2265 >= 15)
//					{
//						anIntArray2261[i13] = rsbuffer2.g3();
//						anIntArray2272[i13] = rsbuffer2.g3();
//						anIntArray2225[i13] = rsbuffer2.g3();
//					} else
//					{
//						anIntArray2261[i13] = rsbuffer2.g2();
//						if(anInt2265 < 14)
//							anIntArray2272[i13] = rsbuffer2.g2();
//						else
//							anIntArray2272[i13] = rsbuffer2.g3();
//						anIntArray2225[i13] = rsbuffer2.g2();
//					}
//					aByteArray2253[i13] = rsbuffer3.g1b();
//					aByteArray2255[i13] = rsbuffer4.g1b();
//					anIntArray2238[i13] = rsbuffer5.g1b();
//				}
//			}
//			
//			rsbuffer.pos = i9;
//			if(flag1)//particles?
//			{
//				int l13 = rsbuffer.g1();
//				if(~l13 < -1)
//				{
//					//aClass351Array2242 = new Class351[l13];
//					for(int j14 = 0; ~j14 > ~l13; j14++)
//					{
//						int i15 = rsbuffer.g2();
//						int l15 = rsbuffer.g2();
//						byte byte3;
//						if(j != 255)
//							byte3 = (byte)j;
//						else
//							byte3 = (byte)facePriority[l15];
//						//aClass351Array2242[j14] = new Class351(i15, triangle_a[l15], triangle_b[l15], triangle_c[l15], byte3);
//					}
//					
//				}
//				int k14 = rsbuffer.g1();
//				if(k14 > 0)
//				{
//					//aClass255Array2226 = new Class255[k14];
//					for(int j15 = 0; ~j15 > ~k14; j15++)
//					{
//						int i16 = rsbuffer.g2();
//						int k16 = rsbuffer.g2();
//						//aClass255Array2226[j15] = new Class255(i16, k16);
//					}
//					
//				}
//			}
//			if(flag2)
//			{
//				int i14 = rsbuffer.g1();
//				if(i14 > 0)
//				{
//					//aClass108Array2276 = new Class108[i14];
//					for(int l14 = 0; ~i14 < ~l14; l14++)
//					{
//						int k15 = rsbuffer.g2();
//						int j16 = rsbuffer.g2();
//						int l16 = rsbuffer.g1();
//						byte byte4 = rsbuffer.g1b();
//						//aClass108Array2276[l14] = new Class108(k15, j16, l16, byte4);
//					}
//					
//				}
//			}
//			
//		}
//		
//		public static void readHeader(byte[] abyte0, int j) {//readheader? - shouldnt realy be being used
//			if (abyte0 == null) {
//				ModelHeader modelHeader = modelHeaderCache[j] = new ModelHeader();
//				modelHeader.modelVerticeCount = 0;
//				modelHeader.modelTriangleCount = 0;
//				modelHeader.modelTextureTriangleCount = 0;
//				return;
//			}
//			Stream stream = new Stream(abyte0);
//			stream.pos = abyte0.Length - 18;
//			ModelHeader modelHeader_1 = modelHeaderCache[j] = new ModelHeader();
//			modelHeader_1.modelData = abyte0;
//			modelHeader_1.modelVerticeCount = stream.g2();
//			modelHeader_1.modelTriangleCount = stream.g2();
//			modelHeader_1.modelTextureTriangleCount = stream.g1();
//			int isTextured = stream.g1();
//			int l = stream.g1();
//			int isTransparent = stream.g1();
//			int hasChangedColours = stream.g1();
//			int hasAnimations = stream.g1();
//			int xSize = stream.g2();
//			int ySize = stream.g2();
//			int zSize = stream.g2();
//			int tDataLength = stream.g2();
//			int l2 = 0;
//			modelHeader_1.vertexModOffset = l2;
//			l2 += modelHeader_1.modelVerticeCount;
//			modelHeader_1.triMeshLinkOffset = l2;
//			l2 += modelHeader_1.modelTriangleCount;
//			modelHeader_1.facePriorityBasePos = l2;
//			if (l == 255)
//				l2 += modelHeader_1.modelTriangleCount;
//			else
//				modelHeader_1.facePriorityBasePos = -l - 1;
//			
//			modelHeader_1.tskinBasepos = l2;
//			if (hasChangedColours == 1)
//				l2 += modelHeader_1.modelTriangleCount;
//			else
//				modelHeader_1.tskinBasepos = -1;
//			
//			modelHeader_1.drawTypeBasePos = l2;
//			if (isTextured == 1)
//				l2 += modelHeader_1.modelTriangleCount;
//			else
//				modelHeader_1.drawTypeBasePos = -1;
//			modelHeader_1.vskinBasePos = l2;
//			
//			if (hasAnimations == 1)
//				l2 += modelHeader_1.modelVerticeCount;
//			else
//				modelHeader_1.vskinBasePos = -1;
//			modelHeader_1.alphaBasepos = l2;
//			if (isTransparent == 1)
//				l2 += modelHeader_1.modelTriangleCount;
//			else
//				modelHeader_1.alphaBasepos = -1;
//			
//			
//			modelHeader_1.triVPointOffset = l2;
//			l2 += tDataLength;
//			modelHeader_1.triColourOffset = l2;
//			l2 += modelHeader_1.modelTriangleCount * 2;
//			modelHeader_1.textureInfoBasePos = l2;
//			l2 += modelHeader_1.modelTextureTriangleCount * 6;
//			modelHeader_1.vertexXOffset = l2;
//			l2 += xSize;
//			modelHeader_1.vertexYOffset = l2;
//			l2 += ySize;
//			modelHeader_1.vertexZOffset = l2;
//			l2 += zSize;
//		}
//		
//		public static void method461(int id)//clearHeader?
//		{
//			modelHeaderCache[id] = null;
//		}
//		
////		public static Model method462(int modelID) {
////			if (modelHeaderCache == null)
////				return null;
////			ModelHeader modelHeader = modelHeaderCache[modelID];
////			if (modelHeader == null) {
////				if (((OnDemandFetcher)abstractODFetcher).clientInstance == null){
////					readHeader(((OnDemandFetcher)abstractODFetcher).getDataFromCache(modelID,0), modelID);
////					return new Model(modelID, 0);//edited for new engine
////				} else {
////					abstractODFetcher.requestData(modelID);
////					return null;
////				}
////			} else {
////				return new Model(modelID, 0);//edited for new engine
////			}
////		}
//
//		public static Model method462(int j)
//		{
//			if (modelHeaderCache == null)
//				return null;
//			ModelHeader class21 = modelHeaderCache[j];
//			if (class21 == null)
//			{
//				abstractODFetcher.method548(j);
//				return null;
//			}
//			else
//			{
//				return new Model(j, 0);
//			}
//		}
//		
////		public static bool method463(int i) {
////			if (modelHeaderCache == null)
////				return false;
////			ModelHeader modelHeader = modelHeaderCache[i];
////			if (modelHeader == null) {
////				abstractODFetcher.requestData(i);
////				return false;
////			} else {
////				return true;
////			}
////		}
//
//		public static bool method463(int i)
//		{
//			if (modelHeaderCache == null)
//				return false;
//			ModelHeader class21 = modelHeaderCache[i];
//			if (class21 == null)
//			{
//				abstractODFetcher.method548(i);
//				return false;
//			}
//			else
//			{
//				return true;
//			}
//		}
//		
//		private Model() {
//			aBoolean1659 = false;
//		}
//		
//		/*private rs2.Model(int i)//never used O_o
//    {
//        abool1659 = false;
//        rs2.ModelHeader modelHeader = modelHeaderCache[i];
//        vertex_count = modelHeader.modelVerticeCount;
//        triangle_count = modelHeader.modelTriangleCount;
//        textureTriangleCount = modelHeader.modelTextureTriangleCount;
//        viewSpaceX = new int[vertex_count];
//        vertex_y = new int[vertex_count];
//        vertex_z = new int[vertex_count];
//        triangle_a = new int[triangle_count];
//        triangle_b = new int[triangle_count];
//        triangle_c = new int[triangle_count];
//        triPIndex = new int[textureTriangleCount];
//        triMIndex = new int[textureTriangleCount];
//        triNIndex = new int[textureTriangleCount];
//        if(modelHeader.anInt376 >= 0)
//            vertexVSkin = new int[vertex_count];
//        if(modelHeader.anInt380 >= 0)
//            triangleDrawType = new int[triangle_count];
//        if(modelHeader.anInt381 >= 0)
//            facePriority = new int[triangle_count];
//        else
//            anInt1641 = -modelHeader.anInt381 - 1;
//        if(modelHeader.alphaBasepos >= 0)
//            triangleAlpha = new int[triangle_count];
//        if(modelHeader.tskinBasepos >= 0)
//            triangleTSkin = new int[triangle_count];
//        triangleColour = new int[triangle_count];
//        rs2.Stream stream = new rs2.Stream(modelHeader.modelData);
//        stream.pos = modelHeader.vertexModOffset;
//        rs2.Stream vXStream = new rs2.Stream(modelHeader.modelData);
//        vXStream.pos = modelHeader.vertexXOffset;
//        rs2.Stream vYStream = new rs2.Stream(modelHeader.modelData);
//        vYStream.pos = modelHeader.vertexYOffset;
//        rs2.Stream vZStream = new rs2.Stream(modelHeader.modelData);
//        vZStream.pos = modelHeader.vertexZOffset;
//        rs2.Stream stream_4 = new rs2.Stream(modelHeader.modelData);
//        stream_4.pos = modelHeader.anInt376;
//        int oldVX = 0;
//        int oldVY = 0;
//        int oldVZ = 0;
//        for(int vID = 0; vID < vertex_count; vID++)
//        {
//            int readValModifier = stream.g1();
//            int vertexx = 0;
//            if((readValModifier & 1) != 0)
//                vertexx = vXStream.gsmart();
//            int vertexy = 0;
//            if((readValModifier & 2) != 0)
//                vertexy = vYStream.gsmart();
//            int vertexz = 0;
//            if((readValModifier & 4) != 0)
//                vertexz = vZStream.gsmart();
//            viewSpaceX[vID] = oldVX + vertexx;
//            vertex_y[vID] = oldVY + vertexy;
//            vertex_z[vID] = oldVZ + vertexz;
//            oldVX = viewSpaceX[vID];
//            oldVY = vertex_y[vID];
//            oldVZ = vertex_z[vID];
//            if(vertexVSkin != null)
//                vertexVSkin[vID] = stream_4.g1();
//        }
//
//        stream.pos = modelHeader.triColourOffset;
//        vXStream.pos = modelHeader.anInt380;
//        vYStream.pos = modelHeader.anInt381;
//        vZStream.pos = modelHeader.alphaBasepos;
//        stream_4.pos = modelHeader.tskinBasepos;
//        for(int l1 = 0; l1 < triangle_count; l1++)
//        {
//            triangleColour[l1] = stream.g2();
//            if(triangleDrawType != null)
//                triangleDrawType[l1] = vXStream.g1();
//            if(facePriority != null)
//                facePriority[l1] = vYStream.g1();
//            if(triangleAlpha != null)
//                triangleAlpha[l1] = vZStream.g1();
//            if(triangleTSkin != null)
//                triangleTSkin[l1] = stream_4.g1();
//        }
//
//        stream.pos = modelHeader.triVPointOffset;
//        vXStream.pos = modelHeader.triMeshLinkOffset;
//        int triA = 0;
//        int triB = 0;
//        int triC = 0;
//        int oldTriX = 0;
//        for(int l3 = 0; l3 < triangle_count; l3++)
//        {
//            int i4 = vXStream.g1();
//            if(i4 == 1)
//            {
//                triA = stream.gsmart() + oldTriX;
//                oldTriX = triA;
//                triB = stream.gsmart() + oldTriX;
//                oldTriX = triB;
//                triC = stream.gsmart() + oldTriX;
//                oldTriX = triC;
//                triangle_a[l3] = triA;
//                triangle_b[l3] = triB;
//                triangle_c[l3] = triC;
//            }
//            if(i4 == 2)
//            {
//                //triA = triA;
//                triB = triC;
//                triC = stream.gsmart() + oldTriX;
//                oldTriX = triC;
//                triangle_a[l3] = triA;
//                triangle_b[l3] = triB;
//                triangle_c[l3] = triC;
//            }
//            if(i4 == 3)
//            {
//                triA = triC;
//                //triB = triB;
//                triC = stream.gsmart() + oldTriX;
//                oldTriX = triC;
//                triangle_a[l3] = triA;
//                triangle_b[l3] = triB;
//                triangle_c[l3] = triC;
//            }
//            if(i4 == 4)
//            {
//                int k4 = triA;
//                triA = triB;
//                triB = k4;
//                triC = stream.gsmart() + oldTriX;
//                oldTriX = triC;
//                triangle_a[l3] = triA;
//                triangle_b[l3] = triB;
//                triangle_c[l3] = triC;
//            }
//        }
//
//        stream.pos = modelHeader.anInt384;
//        for(int j4 = 0; j4 < textureTriangleCount; j4++)
//        {
//            triPIndex[j4] = stream.g2();
//            triMIndex[j4] = stream.g2();
//            triNIndex[j4] = stream.g2();
//        }
//
//    }*/
//		
//		public Model(int numOfModels, Model[] modelParts) {
//			aBoolean1659 = false;
//			bool flag = false;
//			bool flag1 = false;
//			bool flag2 = false;
//			bool flag3 = false;
//			numVertices = 0;
//			numTriangles = 0;
//			textureTriangleCount = 0;
//			anInt1641 = -1;
//			for (int k = 0; k < numOfModels; k++) {
//				Model model = modelParts[k];
//				if (model != null) {
//					numVertices += model.numVertices;
//					numTriangles += model.numTriangles;
//					textureTriangleCount += model.textureTriangleCount;
//					flag |= model.triangleDrawType != null;
//					if (model.facePriority != null) {
//						flag1 = true;
//					} else {
//						if (anInt1641 == -1)
//							anInt1641 = model.anInt1641;
//						if (anInt1641 != model.anInt1641)
//							flag1 = true;
//					}
//					flag2 |= model.triangleAlpha != null;
//					flag3 |= model.triangleTSkin != null;
//					hash += model.hash * (long)Mathf.Pow(10f,(float)k*3f);
//				}
//			}
//			
//			verticesX = new int[numVertices];
//			verticesY = new int[numVertices];
//			verticesZ = new int[numVertices];
//			vertexVSkin = new int[numVertices];
//			triangleViewspaceX = new int[numTriangles];
//			triangleViewspaceY = new int[numTriangles];
//			triangleViewspaceZ = new int[numTriangles];
//			triPIndex = new int[textureTriangleCount];
//			triMIndex = new int[textureTriangleCount];
//			triNIndex = new int[textureTriangleCount];
//			if (flag)
//				triangleDrawType = new int[numTriangles];
//			if (flag1)
//				facePriority = new int[numTriangles];
//			if (flag2)
//				triangleAlpha = new int[numTriangles];
//			if (flag3)
//				triangleTSkin = new int[numTriangles];
//			triangleColourOrTexture = new int[numTriangles];
//			numVertices = 0;
//			numTriangles = 0;
//			textureTriangleCount = 0;
//			int l = 0;
//			for (int i1 = 0; i1 < numOfModels; i1++) {
//				Model model_1 = modelParts[i1];
//				if (model_1 != null) {
//					for (int j1 = 0; j1 < model_1.numTriangles; j1++) {
//						if (flag)
//						if (model_1.triangleDrawType == null) {
//							triangleDrawType[numTriangles] = 0;
//						} else {
//							int k1 = model_1.triangleDrawType[j1];
//							if ((k1 & 2) == 2)
//								k1 += l << 2;
//							triangleDrawType[numTriangles] = k1;
//						}
//						if (flag1)
//							if (model_1.facePriority == null)
//								facePriority[numTriangles] = model_1.anInt1641;
//						else
//							facePriority[numTriangles] = model_1.facePriority[j1];
//						if (flag2)
//							if (model_1.triangleAlpha == null)
//								triangleAlpha[numTriangles] = 0;
//						else
//							triangleAlpha[numTriangles] = model_1.triangleAlpha[j1];
//						if (flag3 && model_1.triangleTSkin != null)
//							triangleTSkin[numTriangles] = model_1.triangleTSkin[j1];
//						triangleColourOrTexture[numTriangles] = model_1.triangleColourOrTexture[j1];
//						triangleViewspaceX[numTriangles] = method465(model_1, model_1.triangleViewspaceX[j1]);
//						triangleViewspaceY[numTriangles] = method465(model_1, model_1.triangleViewspaceY[j1]);
//						triangleViewspaceZ[numTriangles] = method465(model_1, model_1.triangleViewspaceZ[j1]);
//						numTriangles++;
//					}
//					
//					for (int l1 = 0; l1 < model_1.textureTriangleCount; l1++) {
//						triPIndex[textureTriangleCount] = method465(model_1, model_1.triPIndex[l1]);
//						triMIndex[textureTriangleCount] = method465(model_1, model_1.triMIndex[l1]);
//						triNIndex[textureTriangleCount] = method465(model_1, model_1.triNIndex[l1]);
//						textureTriangleCount++;
//					}
//					
//					l += model_1.textureTriangleCount;
//				}
//			}
//			
//		}
//		
//		public Model(Model[] aclass30_sub2_sub4_sub6s) {
//			int i = 2;//was parameter
//			aBoolean1659 = false;
//			bool flag1 = false;
//			bool flag2 = false;
//			bool flag3 = false;
//			bool flag4 = false;
//			numVertices = 0;
//			numTriangles = 0;
//			textureTriangleCount = 0;
//			anInt1641 = -1;
//			for (int k = 0; k < i; k++) {
//				Model model = aclass30_sub2_sub4_sub6s[k];
//				if (model != null) {
//					numVertices += model.numVertices;
//					numTriangles += model.numTriangles;
//					textureTriangleCount += model.textureTriangleCount;
//					flag1 |= model.triangleDrawType != null;
//					if (model.facePriority != null) {
//						flag2 = true;
//					} else {
//						if (anInt1641 == -1)
//							anInt1641 = model.anInt1641;
//						if (anInt1641 != model.anInt1641)
//							flag2 = true;
//					}
//					flag3 |= model.triangleAlpha != null;
//					flag4 |= model.triangleColourOrTexture != null;
//					hash += model.hash * (long)Mathf.Pow(10f,k*3f);
//				}
//			}
//			
//			verticesX = new int[numVertices];
//			verticesY = new int[numVertices];
//			verticesZ = new int[numVertices];
//			triangleViewspaceX = new int[numTriangles];
//			triangleViewspaceY = new int[numTriangles];
//			triangleViewspaceZ = new int[numTriangles];
//			triangle_hsl_a = new int[numTriangles];
//			triangle_hsl_b = new int[numTriangles];
//			triangle_hsl_c = new int[numTriangles];
//			triPIndex = new int[textureTriangleCount];
//			triMIndex = new int[textureTriangleCount];
//			triNIndex = new int[textureTriangleCount];
//			if (flag1)
//				triangleDrawType = new int[numTriangles];
//			if (flag2)
//				facePriority = new int[numTriangles];
//			if (flag3)
//				triangleAlpha = new int[numTriangles];
//			if (flag4)
//				triangleColourOrTexture = new int[numTriangles];
//			numVertices = 0;
//			numTriangles = 0;
//			textureTriangleCount = 0;
//			int i1 = 0;
//			for (int j1 = 0; j1 < i; j1++) {
//				Model model_1 = aclass30_sub2_sub4_sub6s[j1];
//				if (model_1 != null) {
//					int k1 = numVertices;
//					for (int l1 = 0; l1 < model_1.numVertices; l1++) {
//						verticesX[numVertices] = model_1.verticesX[l1];
//						verticesY[numVertices] = model_1.verticesY[l1];
//						verticesZ[numVertices] = model_1.verticesZ[l1];
//						numVertices++;
//					}
//					
//					for (int i2 = 0; i2 < model_1.numTriangles; i2++) {
//						triangleViewspaceX[numTriangles] = model_1.triangleViewspaceX[i2] + k1;
//						triangleViewspaceY[numTriangles] = model_1.triangleViewspaceY[i2] + k1;
//						triangleViewspaceZ[numTriangles] = model_1.triangleViewspaceZ[i2] + k1;
//						triangle_hsl_a[numTriangles] = model_1.triangle_hsl_a[i2];
//						triangle_hsl_b[numTriangles] = model_1.triangle_hsl_b[i2];
//						triangle_hsl_c[numTriangles] = model_1.triangle_hsl_c[i2];
//						if (flag1)
//						if (model_1.triangleDrawType == null) {
//							triangleDrawType[numTriangles] = 0;
//						} else {
//							int j2 = model_1.triangleDrawType[i2];
//							if ((j2 & 2) == 2)
//								j2 += i1 << 2;
//							triangleDrawType[numTriangles] = j2;
//						}
//						if (flag2)
//							if (model_1.facePriority == null)
//								facePriority[numTriangles] = model_1.anInt1641;
//						else
//							facePriority[numTriangles] = model_1.facePriority[i2];
//						if (flag3)
//							if (model_1.triangleAlpha == null)
//								triangleAlpha[numTriangles] = 0;
//						else
//							triangleAlpha[numTriangles] = model_1.triangleAlpha[i2];
//						if (flag4 && model_1.triangleColourOrTexture != null)
//							triangleColourOrTexture[numTriangles] = model_1.triangleColourOrTexture[i2];
//						numTriangles++;
//					}
//					
//					for (int k2 = 0; k2 < model_1.textureTriangleCount; k2++) {
//						triPIndex[textureTriangleCount] = model_1.triPIndex[k2] + k1;
//						triMIndex[textureTriangleCount] = model_1.triMIndex[k2] + k1;
//						triNIndex[textureTriangleCount] = model_1.triNIndex[k2] + k1;
//						textureTriangleCount++;
//					}
//					
//					i1 += model_1.textureTriangleCount;
//				}
//			}
//			
//			method466();
//		}
//		
//		public Model(bool flag, bool animationNull, bool flag2, Model model) {
//			aBoolean1659 = false;
//			numVertices = model.numVertices;
//			numTriangles = model.numTriangles;
//			this.hash = model.hash*2+1;
//			textureTriangleCount = model.textureTriangleCount;
//			if (flag2) {
//				verticesX = model.verticesX;
//				verticesY = model.verticesY;
//				verticesZ = model.verticesZ;
//			} else {
//				verticesX = new int[numVertices];
//				verticesY = new int[numVertices];
//				verticesZ = new int[numVertices];
//				for (int j = 0; j < numVertices; j++) {
//					verticesX[j] = model.verticesX[j];
//					verticesY[j] = model.verticesY[j];
//					verticesZ[j] = model.verticesZ[j];
//				}
//				
//			}
//			if (flag) {
//				triangleColourOrTexture = model.triangleColourOrTexture;
//			} else {
//				triangleColourOrTexture = new int[numTriangles];
//				Array.Copy(model.triangleColourOrTexture, 0, triangleColourOrTexture, 0, numTriangles);
//				
//			}
//			if (animationNull) {
//				triangleAlpha = model.triangleAlpha;
//			} else {
//				triangleAlpha = new int[numTriangles];
//				if (model.triangleAlpha == null) {
//					for (int l = 0; l < numTriangles; l++)
//						triangleAlpha[l] = 0;
//					
//				} else {
//					Array.Copy(model.triangleAlpha, 0, triangleAlpha, 0, numTriangles);
//					
//				}
//			}
//			vertexVSkin = model.vertexVSkin;
//			triangleTSkin = model.triangleTSkin;
//			triangleDrawType = model.triangleDrawType;
//			triangleViewspaceX = model.triangleViewspaceX;
//			triangleViewspaceY = model.triangleViewspaceY;
//			triangleViewspaceZ = model.triangleViewspaceZ;
//			facePriority = model.facePriority;
//			anInt1641 = model.anInt1641;
//			triPIndex = model.triPIndex;
//			triMIndex = model.triMIndex;
//			triNIndex = model.triNIndex;
//		}
//		
//		public Model(bool isSolid, bool nonFlatShading, Model model) {
//			this.hash = model.hash*3+1;
//			aBoolean1659 = false;
//			numVertices = model.numVertices;
//			numTriangles = model.numTriangles;
//			textureTriangleCount = model.textureTriangleCount;
//			if (isSolid) {
//				verticesY = new int[numVertices];
//				Array.Copy(model.verticesY, 0, verticesY, 0, numVertices);
//				
//			} else {
//				verticesY = model.verticesY;
//			}
//			if (nonFlatShading) {
//				triangle_hsl_a = new int[numTriangles];
//				triangle_hsl_b = new int[numTriangles];
//				triangle_hsl_c = new int[numTriangles];
//				for (int k = 0; k < numTriangles; k++) {
//					triangle_hsl_a[k] = model.triangle_hsl_a[k];
//					triangle_hsl_b[k] = model.triangle_hsl_b[k];
//					triangle_hsl_c[k] = model.triangle_hsl_c[k];
//				}
//				
//				triangleDrawType = new int[numTriangles];
//				if (model.triangleDrawType == null) {
//					for (int l = 0; l < numTriangles; l++)
//						triangleDrawType[l] = 0;
//					
//				} else {
//					Array.Copy(model.triangleDrawType, 0, triangleDrawType, 0, numTriangles);
//					
//				}
//				base.vertex_normals = new VertexNormal[numVertices];
//				for (int j1 = 0; j1 < numVertices; j1++) {
//					VertexNormal vertexNormal = base.vertex_normals[j1] = new VertexNormal();
//					VertexNormal vertexNormal_1 = model.vertex_normals[j1];
//					vertexNormal.x = vertexNormal_1.x;
//					vertexNormal.y = vertexNormal_1.y;
//					vertexNormal.z = vertexNormal_1.z;
//					vertexNormal.magnitude = vertexNormal_1.magnitude;
//				}
//				
//				vertexNormalOffset = model.vertexNormalOffset;
//			} else {
//				triangle_hsl_a = model.triangle_hsl_a;
//				triangle_hsl_b = model.triangle_hsl_b;
//				triangle_hsl_c = model.triangle_hsl_c;
//				triangleDrawType = model.triangleDrawType;
//			}
//			verticesX = model.verticesX;
//			verticesZ = model.verticesZ;
//			triangleColourOrTexture = model.triangleColourOrTexture;
//			triangleAlpha = model.triangleAlpha;
//			facePriority = model.facePriority;
//			anInt1641 = model.anInt1641;
//			triangleViewspaceX = model.triangleViewspaceX;
//			triangleViewspaceY = model.triangleViewspaceY;
//			triangleViewspaceZ = model.triangleViewspaceZ;
//			triPIndex = model.triPIndex;
//			triMIndex = model.triMIndex;
//			triNIndex = model.triNIndex;
//			base.modelHeight = model.modelHeight;
//			maxY = model.maxY;
//			anInt1650 = model.anInt1650;
//			diagonal3DAboveorigin = model.diagonal3DAboveorigin;
//			diagonal3D = model.diagonal3D;
//			minX = model.minX;
//			maxZ = model.maxZ;
//			minZ = model.minZ;
//			maxX = model.maxX;
//		}
//		
//		public void method464(Model model, bool flag) {
//			this.hash = model.hash*4+1;
//			numVertices = model.numVertices;
//			numTriangles = model.numTriangles;
//			textureTriangleCount = model.textureTriangleCount;
//			if (anIntArray1622.Length < numVertices) {
//				anIntArray1622 = new int[numVertices + 100];
//				anIntArray1623 = new int[numVertices + 100];
//				anIntArray1624 = new int[numVertices + 100];
//			}
//			verticesX = anIntArray1622;
//			verticesY = anIntArray1623;
//			verticesZ = anIntArray1624;
//			for (int k = 0; k < numVertices; k++) {
//				verticesX[k] = model.verticesX[k];
//				verticesY[k] = model.verticesY[k];
//				verticesZ[k] = model.verticesZ[k];
//			}
//			
//			if (flag) {
//				triangleAlpha = model.triangleAlpha;
//			} else {
//				if (anIntArray1625.Length < numTriangles)
//					anIntArray1625 = new int[numTriangles + 100];
//				triangleAlpha = anIntArray1625;
//				if (model.triangleAlpha == null) {
//					for (int l = 0; l < numTriangles; l++)
//						triangleAlpha[l] = 0;
//					
//				} else {
//					Array.Copy(model.triangleAlpha, 0, triangleAlpha, 0, numTriangles);
//					
//				}
//			}
//			triangleDrawType = model.triangleDrawType;
//			triangleColourOrTexture = model.triangleColourOrTexture;
//			facePriority = model.facePriority;
//			anInt1641 = model.anInt1641;
//			anIntArrayArray1658 = model.anIntArrayArray1658;
//			anIntArrayArray1657 = model.anIntArrayArray1657;
//			triangleViewspaceX = model.triangleViewspaceX;
//			triangleViewspaceY = model.triangleViewspaceY;
//			triangleViewspaceZ = model.triangleViewspaceZ;
//			triangle_hsl_a = model.triangle_hsl_a;
//			triangle_hsl_b = model.triangle_hsl_b;
//			triangle_hsl_c = model.triangle_hsl_c;
//			triPIndex = model.triPIndex;
//			triMIndex = model.triMIndex;
//			triNIndex = model.triNIndex;
//		}
//		
//		private int method465(Model model, int i) {
//			int j = -1;
//			int x = model.verticesX[i];
//			int y = model.verticesY[i];
//			int z = model.verticesZ[i];
//			for (int j1 = 0; j1 < numVertices; j1++) {
//				if (x != verticesX[j1] || y != verticesY[j1] || z != verticesZ[j1])
//					continue;
//				j = j1;
//				break;
//			}
//			
//			if (j == -1) {
//				verticesX[numVertices] = x;
//				verticesY[numVertices] = y;
//				verticesZ[numVertices] = z;
//				if (model.vertexVSkin != null)
//					vertexVSkin[numVertices] = model.vertexVSkin[i];
//				j = numVertices++;
//			}
//			return j;
//		}
//		
//		public void method466() {
//			base.modelHeight = 0;
//			anInt1650 = 0;
//			maxY = 0;
//			for (int verticePointer = 0; verticePointer < numVertices; verticePointer++) {
//				int v_x = verticesX[verticePointer];
//				int v_y = verticesY[verticePointer];
//				int v_z = verticesZ[verticePointer];
//				if (-v_y > base.modelHeight)
//					base.modelHeight = -v_y;
//				if (v_y > maxY)
//					maxY = v_y;
//				int bounds_diagonal = v_x * v_x + v_z * v_z;
//				if (bounds_diagonal > anInt1650)
//					anInt1650 = bounds_diagonal;
//			}
//			anInt1650 = (int) (Mathf.Sqrt(anInt1650) + 0.98999999999999999D);
//			diagonal3DAboveorigin = (int) (Mathf.Sqrt(anInt1650 * anInt1650 + base.modelHeight * base.modelHeight) + 0.98999999999999999D);
//			diagonal3D = diagonal3DAboveorigin + (int) (Mathf.Sqrt(anInt1650 * anInt1650 + maxY * maxY) + 0.98999999999999999D);
//		}
//		
//		public void method467() {//normalise?
//			base.modelHeight = 0;
//			maxY = 0;
//			for (int i = 0; i < numVertices; i++) {
//				int j = verticesY[i];
//				if (-j > base.modelHeight)
//					base.modelHeight = -j;
//				if (j > maxY)
//					maxY = j;
//			}
//			
//			diagonal3DAboveorigin = (int) (Mathf.Sqrt(anInt1650 * anInt1650 + base.modelHeight * base.modelHeight) + 0.98999999999999999D);
//			diagonal3D = diagonal3DAboveorigin + (int) (Mathf.Sqrt(anInt1650 * anInt1650 + maxY * maxY) + 0.98999999999999999D);
//		}
//		
//		private void calculateDiagonalsAndStats() {
//			base.modelHeight = 0;
//			anInt1650 = 0;
//			maxY = 0;
//			minX = 999999;//todo - change to int - 999999
//			maxX = -999999;//4293967297
//			maxZ = -99999;//4294867297
//			minZ = 99999;//99999
//			for (int j = 0; j < numVertices; j++) {
//				int v_x = verticesX[j];
//				int v_y = verticesY[j];
//				int v_z = verticesZ[j];
//				if (v_x < minX)
//					minX = v_x;
//				if (v_x > maxX)
//					maxX = v_x;
//				if (v_z < minZ)
//					minZ = v_z;
//				if (v_z > maxZ)
//					maxZ = v_z;
//				if (-v_y > base.modelHeight)
//					base.modelHeight = -v_y;
//				if (v_y > maxY)
//					maxY = v_y;
//				int _diagonal_2D_aboveorigin = v_x * v_x + v_z * v_z;
//				if (_diagonal_2D_aboveorigin > anInt1650)
//					anInt1650 = _diagonal_2D_aboveorigin;
//			}
//			
//			anInt1650 = (int) Mathf.Sqrt(anInt1650);
//			diagonal3DAboveorigin = (int) Mathf.Sqrt(anInt1650 * anInt1650 + base.modelHeight * base.modelHeight);
//			diagonal3D = diagonal3DAboveorigin + (int) Mathf.Sqrt(anInt1650 * anInt1650 + maxY * maxY);
//		}
//		
//		public void method469() {//groups bones etc
//			if (vertexVSkin != null) {//bones
//				int[] ai = new int[256];
//				int j = 0;
//				for (int l = 0; l < numVertices; l++) {
//					int j1 = vertexVSkin[l];
//					ai[j1]++;
//					if (j1 > j)
//						j = j1;
//				}
//				
//				anIntArrayArray1657 = new int[j + 1][];
//				for (int k1 = 0; k1 <= j; k1++) {
//					anIntArrayArray1657[k1] = new int[ai[k1]];
//					ai[k1] = 0;
//				}
//				
//				for (int j2 = 0; j2 < numVertices; j2++) {
//					int l2 = vertexVSkin[j2];
//					anIntArrayArray1657[l2][ai[l2]++] = j2;
//				}
//				
//				vertexVSkin = null;
//			}
//			if (triangleTSkin != null) {
//				int[] ai1 = new int[256];
//				int k = 0;
//				for (int i1 = 0; i1 < numTriangles; i1++) {
//					int l1 = triangleTSkin[i1];
//					ai1[l1]++;
//					if (l1 > k)
//						k = l1;
//				}
//				
//				anIntArrayArray1658 = new int[k + 1][];
//				for (int i2 = 0; i2 <= k; i2++) {
//					anIntArrayArray1658[i2] = new int[ai1[i2]];
//					ai1[i2] = 0;
//				}
//				
//				for (int k2 = 0; k2 < numTriangles; k2++) {
//					int i3 = triangleTSkin[k2];
//					anIntArrayArray1658[i3][ai1[i3]++] = k2;
//				}
//				
//				triangleTSkin = null;
//			}
//		}
//		
////		public void method470(int frameID) {
////			if (anIntArrayArray1657 == null)
////				return;
////			if (frameID == -1)
////				return;
////			hash+=frameID*1000000;
////			Animation animation = Animation.forID(frameID);
////			if (animation == null)
////				return;
////			Class36 modelTransform = animation.myModelTransform;
////			vertexXModifier = 0;
////			vertexYModifier = 0;
////			vertexZModifier = 0;
////			for (int k = 0; k < animation.stepCount; k++) {
////				int opcodeID = animation.opcodeLinkTable[k];
////				transformStep(modelTransform.opcodes[opcodeID], modelTransform.skinList[opcodeID], animation.modifier1[k], animation.modifier2[k], animation.modifier3[k]);
////			}
////			
////		}
//
//		public void method470(int i)
//		{
//			if (anIntArrayArray1657 == null)
//				return;
//			if (i == -1)
//				return;
//			Class36 class36 = Class36.method531(i);
//			if (class36 == null)
//				return;
//			Class18 class18 = class36.aClass18_637;
//			vertexXModifier = 0;
//			vertexYModifier = 0;
//			vertexZModifier = 0;
//			for (int k = 0; k < class36.anInt638; k++)
//			{
//				int l = class36.anIntArray639[k];
//				transformStep(class18.anIntArray342[l], class18.anIntArrayArray343[l], class36.anIntArray640[k], class36.anIntArray641[k], class36.anIntArray642[k]);
//			}
//			
//		}
//		
////		public void method471(int[] framesFrom2, int frameId2, int frameId1) {
////			if (frameId1 == -1)
////				return;
////			if (framesFrom2 == null || frameId2 == -1) {
////				method470(frameId1);
////				return;
////			}
////			Animation animation = Animation.forID(frameId1);
////			if (animation == null)
////				return;
////			Animation animation_1 = Animation.forID(frameId2);
////			if (animation_1 == null) {
////				method470(frameId1);
////				return;
////			}
////			hash+=frameId2*100000000;
////			hash+=frameId1*1000000;
////			Class36 modelTransform = animation.myModelTransform;
////			vertexXModifier = 0;
////			vertexYModifier = 0;
////			vertexZModifier = 0;
////			int l = 0;
////			int stepIDD = framesFrom2[l++];
////			for (int j1 = 0; j1 < animation.stepCount; j1++) {
////				int k1;
////				for (k1 = animation.opcodeLinkTable[j1]; k1 > stepIDD; stepIDD = framesFrom2[l++]) ;
////				if (k1 != stepIDD || modelTransform.opcodes[k1] == 0)
////					transformStep(modelTransform.opcodes[k1], modelTransform.skinList[k1], animation.modifier1[j1], animation.modifier2[j1], animation.modifier3[j1]);
////			}
////			
////			vertexXModifier = 0;
////			vertexYModifier = 0;
////			vertexZModifier = 0;
////			l = 0;
////			stepIDD = framesFrom2[l++];
////			for (int l1 = 0; l1 < animation_1.stepCount; l1++) {
////				int stepID;
////				for (stepID = animation_1.opcodeLinkTable[l1]; stepID > stepIDD; stepIDD = framesFrom2[l++]) ;
////				if (stepID == stepIDD || modelTransform.opcodes[stepID] == 0)
////					transformStep(modelTransform.opcodes[stepID], modelTransform.skinList[stepID], animation_1.modifier1[l1], animation_1.modifier2[l1], animation_1.modifier3[l1]);
////			}
////			
////		}
//
//		public void method471(int[] ai, int j, int k)
//		{
//			if (k == -1)
//				return;
//			if (ai == null || j == -1)
//			{
//				method470(k);
//				return;
//			}
//			Class36 class36 = Class36.method531(k);
//			if (class36 == null)
//				return;
//			Class36 class36_1 = Class36.method531(j);
//			if (class36_1 == null)
//			{
//				method470(k);
//				return;
//			}
//			Class18 class18 = class36.aClass18_637;
//			vertexXModifier = 0;
//			vertexYModifier = 0;
//			vertexZModifier = 0;
//			int l = 0;
//			int i1 = ai[l++];
//			for (int j1 = 0; j1 < class36.anInt638; j1++)
//			{
//				int k1;
//				for (k1 = class36.anIntArray639[j1]; k1 > i1; i1 = ai[l++]) ;
//				if (k1 != i1 || class18.anIntArray342[k1] == 0)
//					transformStep(class18.anIntArray342[k1], class18.anIntArrayArray343[k1], class36.anIntArray640[j1], class36.anIntArray641[j1], class36.anIntArray642[j1]);
//			}
//			
//			vertexXModifier = 0;
//			vertexYModifier = 0;
//			vertexZModifier = 0;
//			l = 0;
//			i1 = ai[l++];
//			for (int l1 = 0; l1 < class36_1.anInt638; l1++)
//			{
//				int i2;
//				for (i2 = class36_1.anIntArray639[l1]; i2 > i1; i1 = ai[l++]) ;
//				if (i2 == i1 || class18.anIntArray342[i2] == 0)
//					transformStep(class18.anIntArray342[i2], class18.anIntArrayArray343[i2], class36_1.anIntArray640[l1], class36_1.anIntArray641[l1], class36_1.anIntArray642[l1]);
//			}
//			
//		}
//		
//		private void transformStep(int opcode, int[] skinList, int vXOff, int vYOff, int vZOff) {
//			int skinlistCount = skinList.Length;
//			if (opcode == 0) {
//				int vModDiv = 0;
//				vertexXModifier = 0;
//				vertexYModifier = 0;
//				vertexZModifier = 0;
//				for (int k2 = 0; k2 < skinlistCount; k2++) {
//					int vskinID = skinList[k2];
//					if (vskinID < anIntArrayArray1657.Length) {
//						int[] ai5 = anIntArrayArray1657[vskinID];
//						for (int idxVM = 0; idxVM < ai5.Length; idxVM++) {
//							int j6 = ai5[idxVM];
//							vertexXModifier += verticesX[j6];
//							vertexYModifier += verticesY[j6];
//							vertexZModifier += verticesZ[j6];
//							vModDiv++;
//						}
//						
//					}
//				}
//				
//				if (vModDiv > 0) {
//					vertexXModifier = vertexXModifier / vModDiv + vXOff;
//					vertexYModifier = vertexYModifier / vModDiv + vYOff;
//					vertexZModifier = vertexZModifier / vModDiv + vZOff;
//					return;
//				} else {
//					vertexXModifier = vXOff;
//					vertexYModifier = vYOff;
//					vertexZModifier = vZOff;
//					return;
//				}
//			}
//			if (opcode == 1)  //Translate
//			{
//				for (int k1 = 0; k1 < skinlistCount; k1++) {
//					int l2 = skinList[k1];
//					if (l2 < anIntArrayArray1657.Length) {
//						int[] ai1 = anIntArrayArray1657[l2];
//						for (int i4 = 0; i4 < ai1.Length; i4++) {
//							int j5 = ai1[i4];
//							verticesX[j5] += vXOff;
//							verticesY[j5] += vYOff;
//							verticesZ[j5] += vZOff;
//						}
//						
//					}
//				}
//				
//				return;
//			}
//			if (opcode == 2)//Rotate
//			{
//				for (int l1 = 0; l1 < skinlistCount; l1++) {
//					int i3 = skinList[l1];
//					if (i3 < anIntArrayArray1657.Length) {
//						int[] ai2 = anIntArrayArray1657[i3];
//						for (int j4 = 0; j4 < ai2.Length; j4++) {
//							int k5 = ai2[j4];
//							verticesX[k5] -= vertexXModifier;
//							verticesY[k5] -= vertexYModifier;
//							verticesZ[k5] -= vertexZModifier;
//							int k6 = (vXOff & 0xff) * 8;
//							int l6 = (vYOff & 0xff) * 8;
//							int i7 = (vZOff & 0xff) * 8;
//							if (i7 != 0) {
//								int j7 = SINE[i7];
//								int i8 = COSINE[i7];
//								int l8 = verticesY[k5] * j7 + verticesX[k5] * i8 >> 16;
//								verticesY[k5] = verticesY[k5] * i8 - verticesX[k5] * j7 >> 16;
//								verticesX[k5] = l8;
//							}
//							if (k6 != 0) {
//								int k7 = SINE[k6];
//								int j8 = COSINE[k6];
//								int i9 = verticesY[k5] * j8 - verticesZ[k5] * k7 >> 16;
//								verticesZ[k5] = verticesY[k5] * k7 + verticesZ[k5] * j8 >> 16;
//								verticesY[k5] = i9;
//							}
//							if (l6 != 0) {
//								int l7 = SINE[l6];
//								int k8 = COSINE[l6];
//								int j9 = verticesZ[k5] * l7 + verticesX[k5] * k8 >> 16;
//								verticesZ[k5] = verticesZ[k5] * k8 - verticesX[k5] * l7 >> 16;
//								verticesX[k5] = j9;
//							}
//							verticesX[k5] += vertexXModifier;
//							verticesY[k5] += vertexYModifier;
//							verticesZ[k5] += vertexZModifier;
//						}
//						
//					}
//				}
//				
//				return;
//			}
//			if (opcode == 3)//scale
//			{
//				for (int skinListIDX = 0; skinListIDX < skinlistCount; skinListIDX++) {
//					int skinID = skinList[skinListIDX];
//					if (skinID < anIntArrayArray1657.Length) {
//						int[] vSkin = anIntArrayArray1657[skinID];
//						for (int skinPos = 0; skinPos < vSkin.Length; skinPos++) {
//							int vidX = vSkin[skinPos];
//							verticesX[vidX] -= vertexXModifier;
//							verticesY[vidX] -= vertexYModifier;
//							verticesZ[vidX] -= vertexZModifier;
//							verticesX[vidX] = (verticesX[vidX] * vXOff) / 128;
//							verticesY[vidX] = (verticesY[vidX] * vYOff) / 128;
//							verticesZ[vidX] = (verticesZ[vidX] * vZOff) / 128;
//							verticesX[vidX] += vertexXModifier;
//							verticesY[vidX] += vertexYModifier;
//							verticesZ[vidX] += vertexZModifier;
//						}
//						
//					}
//				}
//				
//				return;
//			}
//			if (opcode == 5 && anIntArrayArray1658 != null && triangleAlpha != null)//SetAlpha
//			{
//				for (int mapID = 0; mapID < skinlistCount; mapID++) {
//					int skinID = skinList[mapID];
//					if (skinID < anIntArrayArray1658.Length) {
//						int[] tskin = anIntArrayArray1658[skinID];
//						for (int l4 = 0; l4 < tskin.Length; l4++) {
//							int i6 = tskin[l4];
//							triangleAlpha[i6] += vXOff * 8;
//							if (triangleAlpha[i6] < 0)
//								triangleAlpha[i6] = 0;
//							if (triangleAlpha[i6] > 255)
//								triangleAlpha[i6] = 255;
//						}
//						
//					}
//				}
//				
//			}
//		}
//		
//		public void method473() {//rotate by 90
//			hash += 9000000000L;
//			for (int j = 0; j < numVertices; j++) {
//				int k = verticesX[j];
//				verticesX[j] = verticesZ[j];
//				verticesZ[j] = -k;
//			}
//			
//		}
//		
//		public void method474(int i) {//duno
//			hash += i*100000000L;
//			
//			int k = SINE[i];
//			int l = COSINE[i];
//			for (int i1 = 0; i1 < numVertices; i1++) {
//				int j1 = verticesY[i1] * l - verticesZ[i1] * k >> 16;
//				verticesZ[i1] = verticesY[i1] * k + verticesZ[i1] * l >> 16;
//				verticesY[i1] = j1;
//			}
//		}
//		
//		public void method475(int x, int y, int z) {
//			hash += (x*y*z)*362345L;
//			for (int i1 = 0; i1 < numVertices; i1++) {
//				verticesX[i1] += x;
//				verticesY[i1] += y;
//				verticesZ[i1] += z;
//			}
//			
//		}
//		
//		public void method476(int i, int j) {
//			hash += (i*100000000L)+(j*10000000000L);
//			for (int k = 0; k < numTriangles; k++)
//				if (triangleColourOrTexture[k] == i)
//					triangleColourOrTexture[k] = j;
//			
//		}
//		
//		public bool fliped = false;
//		
//		public void method477() {//mirrors the model, used on lumbys castle doors, alkarid gates etcetc
//			if(fliped)
//			{
//				return;
//			}
//			fliped = true;
//			for (int vertex = 0; vertex < numVertices; vertex++)
//				verticesZ[vertex] = -verticesZ[vertex];
//			
//			for (int triangle = 0; triangle < numTriangles; triangle++) {
//				int l = triangleViewspaceX[triangle];
//				triangleViewspaceX[triangle] = triangleViewspaceZ[triangle];
//				triangleViewspaceZ[triangle] = l;
//			}
//		}
//		
//		public void method478(int x, int y, int z) {
//			hash *= x*y*z;
//			for (int _ctr = 0; _ctr < numVertices; _ctr++) {
//				verticesX[_ctr] = (verticesX[_ctr] * x) / 128;
//				verticesY[_ctr] = (verticesY[_ctr] * y) / 128;
//				verticesZ[_ctr] = (verticesZ[_ctr] * z) / 128;
//			}
//			
//		}
//		
//		public void calculateNormals() {
//			vns = base.vertex_normals;
//			triangleNormalX = new int[numTriangles];
//			triangleNormalY = new int[numTriangles];
//			triangleNormalZ = new int[numTriangles];
//			if (base.vertex_normals == null) {
//				base.vertex_normals = new VertexNormal[numVertices];
//				for (int l1 = 0; l1 < numVertices; l1++)
//					base.vertex_normals[l1] = new VertexNormal();
//				
//			}
//			for (int triID = 0; triID < numTriangles; triID++) {//todo - rename this to camelcode in future (peter plz do this, looks fucking complicated >:)
//				int t_a = triangleViewspaceX[triID];
//				int t_b = triangleViewspaceY[triID];
//				int t_c = triangleViewspaceZ[triID];
//				int u_x = verticesX[t_b] - verticesX[t_a];
//				int u_y = (-verticesY[t_b]) - (-verticesY[t_a]);
//				int u_z = verticesZ[t_b] - verticesZ[t_a];
//				int d_c_a_x = verticesX[t_c] - verticesX[t_a];
//				int d_c_a_y = (-verticesY[t_c]) - (-verticesY[t_a]);
//				int d_c_a_z = verticesZ[t_c] - verticesZ[t_a];
//				int normalX = u_y * d_c_a_z - d_c_a_y * u_z;
//				int normalY = u_z * d_c_a_x - d_c_a_z * u_x;
//				int normalZ;
//				for (normalZ = u_x * d_c_a_y - d_c_a_x * u_y; normalX > 8192 || normalY > 8192 || normalZ > 8192 || normalX < -8192 || normalY < -8192 || normalZ < -8192; normalZ >>= 1) {
//					normalX >>= 1;
//					normalY >>= 1;
//				}
//				
//				int normal_length = (int) Mathf.Sqrt(normalX * normalX + normalY * normalY + normalZ * normalZ);
//				if (normal_length <= 0)
//					normal_length = 1;
//				normalX = (normalX * 256) / normal_length;//Normalization
//				normalY = (normalY * 256) / normal_length;
//				normalZ = (normalZ * 256) / normal_length;
//				if (triangleDrawType == null || (triangleDrawType[triID] & 1) == 0) {
//					VertexNormal vertexNormal_2 = base.vertex_normals[t_a];
//					vertexNormal_2.x += normalX;
//					vertexNormal_2.y += normalY;
//					vertexNormal_2.z += normalZ;
//					vertexNormal_2.magnitude++;
//					vertexNormal_2 = base.vertex_normals[t_b];
//					vertexNormal_2.x += normalX;
//					vertexNormal_2.y += normalY;
//					vertexNormal_2.z += normalZ;
//					vertexNormal_2.magnitude++;
//					vertexNormal_2 = base.vertex_normals[t_c];
//					vertexNormal_2.x += normalX;
//					vertexNormal_2.y += normalY;
//					vertexNormal_2.z += normalZ;
//					vertexNormal_2.magnitude++;
//				}
//				triangleNormalX[triID] = normalX;
//				triangleNormalY[triID] = normalY;
//				triangleNormalZ[triID] = normalZ;
//			}
//		}
//		
//		public void calculateNormals508() {
//			vertex_normals = null;
//			if (vertex_normals == null) {
//				vertex_normals = new VertexNormal[numVertices];
//				for (int i = 0; i < numVertices; i++)
//					vertex_normals[i] = new VertexNormal();
//				for (int i = 0; i < numTriangles; i++) {
//					int i_157_ = triangleViewspaceX[i];
//					int i_158_ = triangleViewspaceY[i];
//					int i_159_ = triangleViewspaceZ[i];
//					int i_160_ = verticesX[i_158_] - verticesX[i_157_];
//					int i_161_ = verticesY[i_158_] - verticesY[i_157_];
//					int i_162_ = verticesZ[i_158_] - verticesZ[i_157_];
//					int i_163_ = verticesX[i_159_] - verticesX[i_157_];
//					int i_164_ = verticesY[i_159_] - verticesY[i_157_];
//					int i_165_ = verticesZ[i_159_] - verticesZ[i_157_];
//					int i_166_ = i_161_ * i_165_ - i_164_ * i_162_;
//					int i_167_ = i_162_ * i_163_ - i_165_ * i_160_;
//					int i_168_;
//					for (i_168_ = i_160_ * i_164_ - i_163_ * i_161_;
//					     (i_166_ > 8192 || i_167_ > 8192 || i_168_ > 8192
//					 || i_166_ < -8192 || i_167_ < -8192 || i_168_ < -8192);
//					     i_168_ >>= 1) {
//						i_166_ >>= 1;
//						i_167_ >>= 1;
//					}
//					int i_169_ = (int) Mathf.Sqrt((float) (i_166_ * i_166_
//					                                       + i_167_ * i_167_
//					                                       + i_168_ * i_168_));
//					if (i_169_ <= 0)
//						i_169_ = 1;
//					i_166_ = i_166_ * 256 / i_169_;
//					i_167_ = i_167_ * 256 / i_169_;
//					i_168_ = i_168_ * 256 / i_169_;
//					int i_170_;
//					if (triangleDrawType == null)
//						i_170_ = (byte) 0;
//					else
//						i_170_ = triangleDrawType[i] & 1;
//					if (i_170_ == 0) {
//						VertexNormal vertexNormal = vertex_normals[i_157_];
//						vertexNormal.x += i_166_;
//						vertexNormal.y += i_167_;
//						vertexNormal.z += i_168_;
//						vertexNormal.magnitude++;
//						vertexNormal = vertex_normals[i_158_];
//						vertexNormal.x += i_166_;
//						vertexNormal.y += i_167_;
//						vertexNormal.z += i_168_;
//						vertexNormal.magnitude++;
//						vertexNormal = vertex_normals[i_159_];
//						vertexNormal.x += i_166_;
//						vertexNormal.y += i_167_;
//						vertexNormal.z += i_168_;
//						vertexNormal.magnitude++;
//					} else if (i_170_ == 1) {
//						if (triangleNormals == null || triangleNormals.Length != numTriangles)
//							triangleNormals = new TriangleNormal[numTriangles];
//						TriangleNormal triangleNormal = triangleNormals[i] = new TriangleNormal();
//						triangleNormal.x = i_166_;
//						triangleNormal.y = i_167_;
//						triangleNormal.z = i_168_;
//					}
//				}
//			}
//		}
//		
//		public void method479(int lightMod, int _magnitude_multiplier, int l_x, int l_y, int l_z, bool flatShading) {
//			int _light_magnitude = (int) Mathf.Sqrt(l_x * l_x + l_y * l_y + l_z * l_z);
//			int mag = _magnitude_multiplier * _light_magnitude >> 8;
//			if (triangle_hsl_a == null) {
//				triangle_hsl_a = new int[numTriangles];
//				triangle_hsl_b = new int[numTriangles];
//				triangle_hsl_c = new int[numTriangles];
//			}
//			if (base.vertex_normals == null) {
//				base.vertex_normals = new VertexNormal[numVertices];
//				for (int l1 = 0; l1 < numVertices; l1++)
//					base.vertex_normals[l1] = new VertexNormal();
//				
//			}
//			for (int triangle_ptr = 0; triangle_ptr < numTriangles; triangle_ptr++) {
//				int t_a = triangleViewspaceX[triangle_ptr];
//				int t_b = triangleViewspaceY[triangle_ptr];
//				int t_c = triangleViewspaceZ[triangle_ptr];
//				int d_a_b_x = verticesX[t_b] - verticesX[t_a];
//				int d_a_b_y = verticesY[t_b] - verticesY[t_a];
//				int d_a_b_z = verticesZ[t_b] - verticesZ[t_a];
//				int d_c_a_x = verticesX[t_c] - verticesX[t_a];
//				int d_c_a_y = verticesY[t_c] - verticesY[t_a];
//				int d_c_a_z = verticesZ[t_c] - verticesZ[t_a];
//				int normal_x = d_a_b_y * d_c_a_z - d_c_a_y * d_a_b_z;
//				int normal_y = d_a_b_z * d_c_a_x - d_c_a_z * d_a_b_x;
//				int normal_z;
//				for (normal_z = d_a_b_x * d_c_a_y - d_c_a_x * d_a_b_y; normal_x > 8192 || normal_y > 8192 || normal_z > 8192 || normal_x < -8192 || normal_y < -8192 || normal_z < -8192; normal_z >>= 1) {
//					normal_x >>= 1;
//					normal_y >>= 1;
//				}
//				
//				int normal_length = (int) Mathf.Sqrt(normal_x * normal_x + normal_y * normal_y + normal_z * normal_z);
//				if (normal_length <= 0)
//					normal_length = 1;
//				normal_x = (normal_x * 256) / normal_length;//Normalization
//				normal_y = (normal_y * 256) / normal_length;
//				normal_z = (normal_z * 256) / normal_length;
//				if (triangleDrawType == null || (triangleDrawType[triangle_ptr] & 1) == 0) {
//					VertexNormal normal = base.vertex_normals[t_a];
//					normal.x += normal_x;
//					normal.y += normal_y;
//					normal.z += normal_z;
//					normal.magnitude++;
//					normal = base.vertex_normals[t_b];
//					normal.x += normal_x;
//					normal.y += normal_y;
//					normal.z += normal_z;
//					normal.magnitude++;
//					normal = base.vertex_normals[t_c];
//					normal.x += normal_x;
//					normal.y += normal_y;
//					normal.z += normal_z;
//					normal.magnitude++;
//				} else {
//					int lightness = lightMod + (l_x * normal_x + l_y * normal_y + l_z * normal_z) / (mag + mag / 2);
//					triangle_hsl_a[triangle_ptr] = mixLightness(triangleColourOrTexture[triangle_ptr], lightness, triangleDrawType[triangle_ptr]);
//				}
//			}
//			
//			if (flatShading) {
//				method480(lightMod, mag, l_x, l_y, l_z);
//				method466();
//			} else {
//				vertexNormalOffset = new VertexNormal[numVertices];
//				for (int vertexPointer = 0; vertexPointer < numVertices; vertexPointer++) {
//					VertexNormal vertexNormal = base.vertex_normals[vertexPointer];
//					VertexNormal vertexNormal_1 = vertexNormalOffset[vertexPointer] = new VertexNormal();
//					vertexNormal_1.x = vertexNormal.x;
//					vertexNormal_1.y = vertexNormal.y;
//					vertexNormal_1.z = vertexNormal.z;
//					vertexNormal_1.magnitude = vertexNormal.magnitude;
//				}
//				calculateDiagonalsAndStats();
//			}
//		}
//		
//		public void lightHD(int lightMod, int magMultiplyer, int l_x, int l_y, int l_z, bool flatShading) {
//			int _mag_pre = (int) Mathf.Sqrt(l_x * l_x + l_y * l_y + l_z * l_z);
//			int mag = magMultiplyer * _mag_pre >> 8;
//			if (triangle_hsl_a == null) {
//				triangle_hsl_a = new int[numTriangles];
//				triangle_hsl_b = new int[numTriangles];
//				triangle_hsl_c = new int[numTriangles];
//			}
//			for (int triID = 0; triID < numTriangles; triID++) {//todo - rename this to camelcode in future (peter plz do this, looks fucking complicated >:)
//				if (triangleDrawType == null || (triangleDrawType[triID] & 1) == 0) {
//				} else {
//					int lightness = lightMod + (l_x * triangleNormals[triID].x +
//					                            l_y * triangleNormals[triID].y +
//					                            l_z * triangleNormals[triID].z) / (mag + mag / 2);
//					triangle_hsl_a[triID] = mixLightness(triangleColourOrTexture[triID], lightness, triangleDrawType[triID]);
//				}
//			}
//			//todo - this can be condensed - DONE
//			
//			if (flatShading) {
//				doShadingHD(lightMod, mag, l_x, l_y, l_z);
//				method466();
//			}  else {
//				vertexNormalOffset = new VertexNormal[numVertices];
//				for (int vertexPointer = 0; vertexPointer < numVertices; vertexPointer++) {
//					VertexNormal vertexNormal = base.vertex_normals[vertexPointer];
//					VertexNormal vertexNormal_1 = vertexNormalOffset[vertexPointer] = new VertexNormal();
//					vertexNormal_1.x = vertexNormal.x;
//					vertexNormal_1.y = vertexNormal.y;
//					vertexNormal_1.z = vertexNormal.z;
//					vertexNormal_1.magnitude = vertexNormal.magnitude;
//				}
//				calculateDiagonalsAndStats();
//			}
//		}
//		
//		public void doShadingHD(int intensity, int falloff, int l_x, int l_y, int l_z) {
//			for (int triID = 0; triID < numTriangles; triID++) {
//				int triA = triangleViewspaceX[triID];
//				int triB = triangleViewspaceY[triID];
//				int triC = triangleViewspaceZ[triID];
//				if (triangleDrawType == null) {
//					int t_hsl = triangleColourOrTexture[triID];
//					VertexNormal vertexNormal = base.vertex_normals[triA];
//					int l = intensity + (l_x * vertexNormal.x + l_y * vertexNormal.y + l_z * vertexNormal.z) / (falloff * vertexNormal.magnitude);
//					triangle_hsl_a[triID] = mixLightness(t_hsl, l, 0);
//					vertexNormal = base.vertex_normals[triB];
//					l = intensity + (l_x * vertexNormal.x + l_y * vertexNormal.y + l_z * vertexNormal.z) / (falloff * vertexNormal.magnitude);
//					triangle_hsl_b[triID] = mixLightness(t_hsl, l, 0);
//					vertexNormal = base.vertex_normals[triC];
//					l = intensity + (l_x * vertexNormal.x + l_y * vertexNormal.y + l_z * vertexNormal.z) / (falloff * vertexNormal.magnitude);
//					triangle_hsl_c[triID] = mixLightness(t_hsl, l, 0);
//				} else if ((triangleDrawType[triID] & 1) == 0) {
//					//Bit 1 of triangle_draw_type ON means mix_lightness returns just lightness
//					//instead of mixed hsl
//					int t_hsl = triangleColourOrTexture[triID];
//					int t_flags = triangleDrawType[triID];
//					VertexNormal vertexNormal_1 = base.vertex_normals[triA];
//					int l = intensity + (l_x * vertexNormal_1.x + l_y * vertexNormal_1.y + l_z * vertexNormal_1.z) / (falloff * vertexNormal_1.magnitude);
//					triangle_hsl_a[triID] = mixLightness(t_hsl, l, t_flags);
//					vertexNormal_1 = base.vertex_normals[triB];
//					l = intensity + (l_x * vertexNormal_1.x + l_y * vertexNormal_1.y + l_z * vertexNormal_1.z) / (falloff * vertexNormal_1.magnitude);
//					triangle_hsl_b[triID] = mixLightness(t_hsl, l, t_flags);
//					vertexNormal_1 = base.vertex_normals[triC];
//					l = intensity + (l_x * vertexNormal_1.x + l_y * vertexNormal_1.y + l_z * vertexNormal_1.z) / (falloff * vertexNormal_1.magnitude);
//					triangle_hsl_c[triID] = mixLightness(t_hsl, l, t_flags);
//				}
//			}
//			if (triangleDrawType != null) {
//				for (int l1 = 0; l1 < numTriangles; l1++)
//					if ((triangleDrawType[l1] & 2) == 2)
//						return;
//				
//			}
//		}
//		
//		public void method480(int intensity, int falloff, int l_x, int l_y, int l_z) {
//			for (int tri_ptr = 0; tri_ptr < numTriangles; tri_ptr++) {
//				int tri_a = triangleViewspaceX[tri_ptr];
//				int tri_b = triangleViewspaceY[tri_ptr];
//				int tri_c = triangleViewspaceZ[tri_ptr];
//				if (triangleDrawType == null) {
//					int t_hsl = triangleColourOrTexture[tri_ptr];
//					VertexNormal normal = base.vertex_normals[tri_a];
//					int l = intensity + (l_x * normal.x + l_y * normal.y + l_z * normal.z) / (falloff * normal.magnitude);
//					triangle_hsl_a[tri_ptr] = mixLightness(t_hsl, l, 0);
//					normal = base.vertex_normals[tri_b];
//					l = intensity + (l_x * normal.x + l_y * normal.y + l_z * normal.z) / (falloff * normal.magnitude);
//					triangle_hsl_b[tri_ptr] = mixLightness(t_hsl, l, 0);
//					normal = base.vertex_normals[tri_c];
//					l = intensity + (l_x * normal.x + l_y * normal.y + l_z * normal.z) / (falloff * normal.magnitude);
//					triangle_hsl_c[tri_ptr] = mixLightness(t_hsl, l, 0);
//				} else if ((triangleDrawType[tri_ptr] & 1) == 0) {
//					//Bit 1 of triangle_draw_type ON means mix_lightness returns just lightness
//					//instead of mixed hsl
//					int t_hsl = triangleColourOrTexture[tri_ptr];
//					int t_flags = triangleDrawType[tri_ptr];
//					VertexNormal normal = base.vertex_normals[tri_a];
//					int l = intensity + (l_x * normal.x + l_y * normal.y + l_z * normal.z) / (falloff * normal.magnitude);
//					triangle_hsl_a[tri_ptr] = mixLightness(t_hsl, l, t_flags);
//					normal = base.vertex_normals[tri_b];
//					l = intensity + (l_x * normal.x + l_y * normal.y + l_z * normal.z) / (falloff * normal.magnitude);
//					triangle_hsl_b[tri_ptr] = mixLightness(t_hsl, l, t_flags);
//					normal = base.vertex_normals[tri_c];
//					l = intensity + (l_x * normal.x + l_y * normal.y + l_z * normal.z) / (falloff * normal.magnitude);
//					triangle_hsl_c[tri_ptr] = mixLightness(t_hsl, l, t_flags);
//				}
//			}
//			
//			base.vertex_normals = null;
//			vertexNormalOffset = null;
//			vertexVSkin = null;
//			triangleTSkin = null;
//			if (triangleDrawType != null) {
//				for (int l1 = 0; l1 < numTriangles; l1++)
//					if ((triangleDrawType[l1] & 2) == 2)
//						return;
//				
//			}
//			//triangleColour = null;
//		}
//		
//		private static int mixLightness(int hsl, int l, int flags) {
//			if ((flags & 2) == 2) {
//				if (l < 0)
//					l = 0;
//				else if (l > 127)
//					l = 127;
//				l = 127 - l;
//				return l;
//			}
//			l = l * (hsl & 0x7f) >> 7;
//			if (l < 2)
//				l = 2;
//			else if (l > 126)
//				l = 126;
//			return (hsl & 0xff80) + l;
//		}
//		
//		public void method482(int rot_x, int rot_y, int rot_z, int trans_x, int trans_y, int trans_z, int rot_xw) {//todo figure if i has any significence to its value.
//			int vp_center_x = Texture.center_x;
//			int vp_center_y = Texture.center_y;
//			int sin_x = SINE[rot_x];//[i]
//			int cos_x = COSINE[rot_x];//[i]
//			int sin_y = SINE[rot_y];
//			int cos_y = COSINE[rot_y];
//			int sin_z = SINE[rot_z];
//			int cos_z = COSINE[rot_z];
//			int sin_x_world = SINE[rot_xw];
//			int cos_x_world = COSINE[rot_xw];
//			int j4 = trans_y * sin_x_world + trans_z * cos_x_world >> 16;
//			for (int vertex_ptr = 0; vertex_ptr < numVertices; vertex_ptr++) {
//				int _x = verticesX[vertex_ptr];
//				int _y = verticesY[vertex_ptr];
//				int _z = verticesZ[vertex_ptr];
//				if (rot_z != 0) {
//					int __x = _y * sin_z + _x * cos_z >> 16;
//					_y = _y * cos_z - _x * sin_z >> 16;
//					_x = __x;
//				}
//				if (rot_x != 0) {
//					int __y = _y * cos_x - _z * sin_x >> 16;
//					_z = _y * sin_x + _z * cos_x >> 16;
//					_y = __y;
//				}
//				if (rot_y != 0) {
//					int __x = _z * sin_y + _x * cos_y >> 16;
//					_z = _z * cos_y - _x * sin_y >> 16;
//					_x = __x;
//				}
//				_x += trans_x;
//				_y += trans_y;
//				_z += trans_z;
//				int __y2 = _y * cos_x_world - _z * sin_x_world >> 16;
//				_z = _y * sin_x_world + _z * cos_x_world >> 16;
//				_y = __y2;
//				vertex_screen_z[vertex_ptr] = _z - j4;
//				vertex_screen_x[vertex_ptr] = vp_center_x + (_x << 9) / _z;
//				vertex_screen_y[vertex_ptr] = vp_center_y + (_y << 9) / _z;
//				if (textureTriangleCount > 0) {
//					vertexMvX[vertex_ptr] = _x;
//					vertexMvY[vertex_ptr] = _y;
//					vertexMvZ[vertex_ptr] = _z;
//				}
//			}
//			
//			try {
//				method483(false, false, 0);
//			} catch (Exception _ex) {
//			}
//		}
//		
//		public void renderAtPoint2(int i, int yCameraSine, int yCameraCosine, int xCurveSine, int xCurveCosine, int x, int y,
//		                           int z, int i2) {
//			int j2 = z * xCurveCosine - x * xCurveSine >> 16;
//			int k2 = y * yCameraSine + j2 * yCameraCosine >> 16;
//			int l2 = anInt1650 * yCameraCosine >> 16;
//			int i3 = k2 + l2;
//			if (i3 <= 50 || k2 >= 3500)
//				return;
//			int j3 = z * xCurveSine + x * xCurveCosine >> 16;
//			int k3 = j3 - anInt1650 << 9;
//			if (k3 / i3 >= DrawingArea.centerX)
//				return;
//			int l3 = j3 + anInt1650 << 9;
//			if (l3 / i3 <= -DrawingArea.centerX)
//				return;
//			int i4 = y * yCameraCosine - j2 * yCameraSine >> 16;
//			int j4 = anInt1650 * yCameraSine >> 16;
//			int k4 = i4 + j4 << 9;
//			if (k4 / i3 <= -DrawingArea.anInt1387)
//				return;
//			int l4 = j4 + (base.modelHeight * yCameraCosine >> 16);
//			int i5 = i4 - l4 << 9;
//			if (i5 / i3 >= DrawingArea.anInt1387)
//				return;
//			int j5 = l2 + (base.modelHeight * yCameraSine >> 16);
//			bool flag = false;
//			if (k2 - j5 <= 50)
//				flag = true;
//			bool flag1 = false;
//			if (i2 > 0 && aBoolean1684) {
//				int k5 = k2 - l2;
//				if (k5 <= 50)
//					k5 = 50;
//				if (j3 > 0) {
//					k3 /= i3;
//					l3 /= k5;
//				} else {
//					l3 /= i3;
//					k3 /= k5;
//				}
//				if (i4 > 0) {
//					i5 /= i3;
//					k4 /= k5;
//				} else {
//					k4 /= i3;
//					i5 /= k5;
//				}
//				int i6 = anInt1685 - Texture.center_x;
//				int k6 = anInt1686 - Texture.center_y;
//				if (i6 > k3 && i6 < l3 && k6 > i5 && k6 < k4)
//					if (aBoolean1659)
//						anIntArray1688[anInt1687++] = i2;
//				else
//					flag1 = true;
//			}
//			int l5 = Texture.center_x;
//			int j6 = Texture.center_y;
//			int l6 = 0;
//			int i7 = 0;
//			if (i != 0) {
//				l6 = SINE[i];
//				i7 = COSINE[i];
//			}
//			for (int vetexIdx = 0; vetexIdx < numVertices; vetexIdx++) {
//				int vX = verticesX[vetexIdx];
//				int vY = verticesY[vetexIdx];
//				int vZ = verticesZ[vetexIdx];
//				if (i != 0) {
//					int j8 = vZ * l6 + vX * i7 >> 16;
//					vZ = vZ * i7 - vX * l6 >> 16;
//					vX = j8;
//				}
//				vX += x;
//				vY += y;
//				vZ += z;
//				int k8 = vZ * xCurveSine + vX * xCurveCosine >> 16;
//				vZ = vZ * xCurveCosine - vX * xCurveSine >> 16;
//				vX = k8;
//				k8 = vY * yCameraCosine - vZ * yCameraSine >> 16;
//				vZ = vY * yCameraSine + vZ * yCameraCosine >> 16;
//				vY = k8;
//				vertex_screen_z[vetexIdx] = vZ - k2;
//				if (vZ >= 50) {
//					vertex_screen_x[vetexIdx] = l5 + (vX << 9) / vZ;
//					vertex_screen_y[vetexIdx] = j6 + (vY << 9) / vZ;
//				} else {
//					vertex_screen_x[vetexIdx] = -5000;
//					flag = true;
//				}
//				if (flag || textureTriangleCount > 0) {
//					vertexMvX[vetexIdx] = vX;
//					vertexMvY[vetexIdx] = vY;
//					vertexMvZ[vetexIdx] = vZ;
//				}
//			}
//			
//			try {
//				method483(flag, flag1, i2);
//			} catch (Exception _ex) {
//			}
//		}
//		
//		private void method483(bool flag, bool flag1, int i) {
//			for (int j = 0; j < diagonal3D; j++)
//				depthListIndices[j] = 0;
//			
//			for (int k = 0; k < numTriangles; k++)
//			if (triangleDrawType == null || triangleDrawType[k] != -1) {
//				int l = triangleViewspaceX[k];
//				int k1 = triangleViewspaceY[k];
//				int j2 = triangleViewspaceZ[k];
//				int i3 = vertex_screen_x[l];
//				int l3 = vertex_screen_x[k1];
//				int k4 = vertex_screen_x[j2];
//				if (flag && (i3 == -5000 || l3 == -5000 || k4 == -5000)) {
//					aboolArray1664[k] = true;
//					int j5 = (vertex_screen_z[l] + vertex_screen_z[k1] + vertex_screen_z[j2]) / 3 + diagonal3DAboveorigin;
//					faceLists[j5][depthListIndices[j5]++] = k;
//				} else {
//					if (flag1 && method486(anInt1685, anInt1686, vertex_screen_y[l], vertex_screen_y[k1], vertex_screen_y[j2], i3, l3, k4)) {
//						anIntArray1688[anInt1687++] = i;
//						flag1 = false;
//					}
//					if ((i3 - l3) * (vertex_screen_y[j2] - vertex_screen_y[k1]) - (vertex_screen_y[l] - vertex_screen_y[k1]) * (k4 - l3) > 0) {
//						aboolArray1664[k] = false;
//						aboolArray1663[k] = i3 < 0 || l3 < 0 || k4 < 0 || i3 > DrawingArea.centerX || l3 > DrawingArea.centerX || k4 > DrawingArea.centerX;
//						int k5 = (vertex_screen_z[l] + vertex_screen_z[k1] + vertex_screen_z[j2]) / 3 + diagonal3DAboveorigin;
//						faceLists[k5][depthListIndices[k5]++] = k;
//					}
//				}
//			}
//			
//			if (facePriority == null) {
//				for (int i1 = diagonal3D - 1; i1 >= 0; i1--) {
//					int l1 = depthListIndices[i1];
//					if (l1 > 0) {
//						int[] ai = faceLists[i1];
//						for (int j3 = 0; j3 < l1; j3++)
//							rasterize(ai[j3]);
//						
//					}
//				}
//				
//				return;
//			}
//			for (int j1 = 0; j1 < 12; j1++) {
//				anIntArray1673[j1] = 0;
//				anIntArray1677[j1] = 0;
//			}
//			
//			for (int i2 = diagonal3D - 1; i2 >= 0; i2--) {
//				int k2 = depthListIndices[i2];
//				if (k2 > 0) {
//					int[] ai1 = faceLists[i2];
//					for (int i4 = 0; i4 < k2; i4++) {
//						int l4 = ai1[i4];
//						int l5 = facePriority[l4];
//						int j6 = anIntArray1673[l5]++;
//						anIntArrayArray1674[l5][j6] = l4;
//						if (l5 < 10)
//							anIntArray1677[l5] += i2;
//						else if (l5 == 10)
//							anIntArray1675[j6] = i2;
//						else
//							anIntArray1676[j6] = i2;
//					}
//					
//				}
//			}
//			
//			int l2 = 0;
//			if (anIntArray1673[1] > 0 || anIntArray1673[2] > 0)
//				l2 = (anIntArray1677[1] + anIntArray1677[2]) / (anIntArray1673[1] + anIntArray1673[2]);
//			int k3 = 0;
//			if (anIntArray1673[3] > 0 || anIntArray1673[4] > 0)
//				k3 = (anIntArray1677[3] + anIntArray1677[4]) / (anIntArray1673[3] + anIntArray1673[4]);
//			int j4 = 0;
//			if (anIntArray1673[6] > 0 || anIntArray1673[8] > 0)
//				j4 = (anIntArray1677[6] + anIntArray1677[8]) / (anIntArray1673[6] + anIntArray1673[8]);
//			int i6 = 0;
//			int k6 = anIntArray1673[10];
//			int[] ai2 = anIntArrayArray1674[10];
//			int[] ai3 = anIntArray1675;
//			if (i6 == k6) {
//				i6 = 0;
//				k6 = anIntArray1673[11];
//				ai2 = anIntArrayArray1674[11];
//				ai3 = anIntArray1676;
//			}
//			int i5;
//			if (i6 < k6)
//				i5 = ai3[i6];
//			else
//				i5 = -1000;
//			for (int l6 = 0; l6 < 10; l6++) {
//				while (l6 == 0 && i5 > l2) {
//					rasterize(ai2[i6++]);
//					if (i6 == k6 && ai2 != anIntArrayArray1674[11]) {
//						i6 = 0;
//						k6 = anIntArray1673[11];
//						ai2 = anIntArrayArray1674[11];
//						ai3 = anIntArray1676;
//					}
//					if (i6 < k6)
//						i5 = ai3[i6];
//					else
//						i5 = -1000;
//				}
//				while (l6 == 3 && i5 > k3) {
//					rasterize(ai2[i6++]);
//					if (i6 == k6 && ai2 != anIntArrayArray1674[11]) {
//						i6 = 0;
//						k6 = anIntArray1673[11];
//						ai2 = anIntArrayArray1674[11];
//						ai3 = anIntArray1676;
//					}
//					if (i6 < k6)
//						i5 = ai3[i6];
//					else
//						i5 = -1000;
//				}
//				while (l6 == 5 && i5 > j4) {
//					rasterize(ai2[i6++]);
//					if (i6 == k6 && ai2 != anIntArrayArray1674[11]) {
//						i6 = 0;
//						k6 = anIntArray1673[11];
//						ai2 = anIntArrayArray1674[11];
//						ai3 = anIntArray1676;
//					}
//					if (i6 < k6)
//						i5 = ai3[i6];
//					else
//						i5 = -1000;
//				}
//				int i7 = anIntArray1673[l6];
//				int[] ai4 = anIntArrayArray1674[l6];
//				for (int j7 = 0; j7 < i7; j7++)
//					rasterize(ai4[j7]);
//				
//			}
//			
//			while (i5 != -1000) {
//				rasterize(ai2[i6++]);
//				if (i6 == k6 && ai2 != anIntArrayArray1674[11]) {
//					i6 = 0;
//					ai2 = anIntArrayArray1674[11];
//					k6 = anIntArray1673[11];
//					ai3 = anIntArray1676;
//				}
//				if (i6 < k6)
//					i5 = ai3[i6];
//				else
//					i5 = -1000;
//			}
//		}
//		
//		private void rasterize(int triPtr) {
//			if (aboolArray1664[triPtr]) {
//				method485(triPtr);
//				return;
//			}
//			int tA = triangleViewspaceX[triPtr];
//			int tB = triangleViewspaceY[triPtr];
//			int tC = triangleViewspaceZ[triPtr];
//			Texture.restrict_edges = aboolArray1663[triPtr];
//			if (triangleAlpha == null)
//				Texture.alpha = 0;
//			else
//				Texture.alpha = triangleAlpha[triPtr];
//			int triangleDrawType;
//			if (this.triangleDrawType == null)
//				triangleDrawType = 0;
//			else
//				triangleDrawType = this.triangleDrawType[triPtr] & 3;
//			if (triangleDrawType == 0) {
//				Texture.drawShadedTriangle(vertex_screen_y[tA], vertex_screen_y[tB], vertex_screen_y[tC], vertex_screen_x[tA], vertex_screen_x[tB], vertex_screen_x[tC], triangle_hsl_a[triPtr], triangle_hsl_b[triPtr], triangle_hsl_c[triPtr]);
//				return;
//			}
//			if (triangleDrawType == 1) {
//				Texture.drawFlatTriangle(vertex_screen_y[tA], vertex_screen_y[tB], vertex_screen_y[tC], vertex_screen_x[tA], vertex_screen_x[tB], vertex_screen_x[tC], HSL2RGB[triangle_hsl_a[triPtr]]);
//				return;
//			}
//			if (triangleDrawType == 2) {
//				int textriPtr = this.triangleDrawType[triPtr] >> 2;
//				int tP = triPIndex[textriPtr];
//				int tM = triMIndex[textriPtr];
//				int tN = triNIndex[textriPtr];
//				Texture.drawTexturedTriangle(vertex_screen_y[tA], vertex_screen_y[tB], vertex_screen_y[tC], vertex_screen_x[tA], vertex_screen_x[tB], vertex_screen_x[tC], triangle_hsl_a[triPtr], triangle_hsl_b[triPtr], triangle_hsl_c[triPtr], vertexMvX[tP], vertexMvX[tM], vertexMvX[tN], vertexMvY[tP], vertexMvY[tM], vertexMvY[tN], vertexMvZ[tP], vertexMvZ[tM], vertexMvZ[tN], triangleColourOrTexture[triPtr]);
//				return;
//			}
//			if (triangleDrawType == 3) {
//				int k1 = this.triangleDrawType[triPtr] >> 2;
//				int i2 = triPIndex[k1];
//				int k2 = triMIndex[k1];
//				int i3 = triNIndex[k1];
//				Texture.drawTexturedTriangle(vertex_screen_y[tA], vertex_screen_y[tB], vertex_screen_y[tC], vertex_screen_x[tA], vertex_screen_x[tB], vertex_screen_x[tC], triangle_hsl_a[triPtr], triangle_hsl_a[triPtr], triangle_hsl_a[triPtr], vertexMvX[i2], vertexMvX[k2], vertexMvX[i3], vertexMvY[i2], vertexMvY[k2], vertexMvY[i3], vertexMvZ[i2], vertexMvZ[k2], vertexMvZ[i3], triangleColourOrTexture[triPtr]);
//			}
//		}
//		
//		private void method485(int i) {
//			int j = Texture.center_x;
//			int k = Texture.center_y;
//			int l = 0;
//			int i1 = triangleViewspaceX[i];
//			int j1 = triangleViewspaceY[i];
//			int k1 = triangleViewspaceZ[i];
//			int l1 = vertexMvZ[i1];
//			int i2 = vertexMvZ[j1];
//			int j2 = vertexMvZ[k1];
//			if (l1 >= 50) {
//				anIntArray1678[l] = vertex_screen_x[i1];
//				anIntArray1679[l] = vertex_screen_y[i1];
//				anIntArray1680[l++] = triangle_hsl_a[i];
//			} else {
//				int k2 = vertexMvX[i1];
//				int k3 = vertexMvY[i1];
//				int k4 = triangle_hsl_a[i];
//				if (j2 >= 50) {
//					int k5 = (50 - l1) * modelIntArray4[j2 - l1];
//					anIntArray1678[l] = j + (k2 + ((vertexMvX[k1] - k2) * k5 >> 16) << 9) / 50;
//					anIntArray1679[l] = k + (k3 + ((vertexMvY[k1] - k3) * k5 >> 16) << 9) / 50;
//					anIntArray1680[l++] = k4 + ((triangle_hsl_c[i] - k4) * k5 >> 16);
//				}
//				if (i2 >= 50) {
//					int l5 = (50 - l1) * modelIntArray4[i2 - l1];
//					anIntArray1678[l] = j + (k2 + ((vertexMvX[j1] - k2) * l5 >> 16) << 9) / 50;
//					anIntArray1679[l] = k + (k3 + ((vertexMvY[j1] - k3) * l5 >> 16) << 9) / 50;
//					anIntArray1680[l++] = k4 + ((triangle_hsl_b[i] - k4) * l5 >> 16);
//				}
//			}
//			if (i2 >= 50) {
//				anIntArray1678[l] = vertex_screen_x[j1];
//				anIntArray1679[l] = vertex_screen_y[j1];
//				anIntArray1680[l++] = triangle_hsl_b[i];
//			} else {
//				int l2 = vertexMvX[j1];
//				int l3 = vertexMvY[j1];
//				int l4 = triangle_hsl_b[i];
//				if (l1 >= 50) {
//					int i6 = (50 - i2) * modelIntArray4[l1 - i2];
//					anIntArray1678[l] = j + (l2 + ((vertexMvX[i1] - l2) * i6 >> 16) << 9) / 50;
//					anIntArray1679[l] = k + (l3 + ((vertexMvY[i1] - l3) * i6 >> 16) << 9) / 50;
//					anIntArray1680[l++] = l4 + ((triangle_hsl_a[i] - l4) * i6 >> 16);
//				}
//				if (j2 >= 50) {
//					int j6 = (50 - i2) * modelIntArray4[j2 - i2];
//					anIntArray1678[l] = j + (l2 + ((vertexMvX[k1] - l2) * j6 >> 16) << 9) / 50;
//					anIntArray1679[l] = k + (l3 + ((vertexMvY[k1] - l3) * j6 >> 16) << 9) / 50;
//					anIntArray1680[l++] = l4 + ((triangle_hsl_c[i] - l4) * j6 >> 16);
//				}
//			}
//			if (j2 >= 50) {
//				anIntArray1678[l] = vertex_screen_x[k1];
//				anIntArray1679[l] = vertex_screen_y[k1];
//				anIntArray1680[l++] = triangle_hsl_c[i];
//			} else {
//				int i3 = vertexMvX[k1];
//				int i4 = vertexMvY[k1];
//				int i5 = triangle_hsl_c[i];
//				if (i2 >= 50) {
//					int k6 = (50 - j2) * modelIntArray4[i2 - j2];
//					anIntArray1678[l] = j + (i3 + ((vertexMvX[j1] - i3) * k6 >> 16) << 9) / 50;
//					anIntArray1679[l] = k + (i4 + ((vertexMvY[j1] - i4) * k6 >> 16) << 9) / 50;
//					anIntArray1680[l++] = i5 + ((triangle_hsl_b[i] - i5) * k6 >> 16);
//				}
//				if (l1 >= 50) {
//					int l6 = (50 - j2) * modelIntArray4[l1 - j2];
//					anIntArray1678[l] = j + (i3 + ((vertexMvX[i1] - i3) * l6 >> 16) << 9) / 50;
//					anIntArray1679[l] = k + (i4 + ((vertexMvY[i1] - i4) * l6 >> 16) << 9) / 50;
//					anIntArray1680[l++] = i5 + ((triangle_hsl_a[i] - i5) * l6 >> 16);
//				}
//			}
//			int j3 = anIntArray1678[0];
//			int j4 = anIntArray1678[1];
//			int j5 = anIntArray1678[2];
//			int i7 = anIntArray1679[0];
//			int j7 = anIntArray1679[1];
//			int k7 = anIntArray1679[2];
//			if ((j3 - j4) * (k7 - j7) - (i7 - j7) * (j5 - j4) > 0) {
//				Texture.restrict_edges = false;
//				if (l == 3) {
//					if (j3 < 0 || j4 < 0 || j5 < 0 || j3 > DrawingArea.centerX || j4 > DrawingArea.centerX || j5 > DrawingArea.centerX)
//						Texture.restrict_edges = true;
//					int l7;
//					if (triangleDrawType == null)
//						l7 = 0;
//					else
//						l7 = triangleDrawType[i] & 3;
//					if (l7 == 0)
//						Texture.drawShadedTriangle(i7, j7, k7, j3, j4, j5, anIntArray1680[0], anIntArray1680[1], anIntArray1680[2]);
//					else if (l7 == 1)
//						Texture.drawFlatTriangle(i7, j7, k7, j3, j4, j5, HSL2RGB[triangle_hsl_a[i]]);
//					else if (l7 == 2) {
//						int j8 = triangleDrawType[i] >> 2;
//						int k9 = triPIndex[j8];
//						int k10 = triMIndex[j8];
//						int k11 = triNIndex[j8];
//						Texture.drawTexturedTriangle(i7, j7, k7, j3, j4, j5, anIntArray1680[0], anIntArray1680[1], anIntArray1680[2], vertexMvX[k9], vertexMvX[k10], vertexMvX[k11], vertexMvY[k9], vertexMvY[k10], vertexMvY[k11], vertexMvZ[k9], vertexMvZ[k10], vertexMvZ[k11], triangleColourOrTexture[i]);
//					} else if (l7 == 3) {
//						int k8 = triangleDrawType[i] >> 2;
//						int l9 = triPIndex[k8];
//						int l10 = triMIndex[k8];
//						int l11 = triNIndex[k8];
//						Texture.drawTexturedTriangle(i7, j7, k7, j3, j4, j5, triangle_hsl_a[i], triangle_hsl_a[i], triangle_hsl_a[i], vertexMvX[l9], vertexMvX[l10], vertexMvX[l11], vertexMvY[l9], vertexMvY[l10], vertexMvY[l11], vertexMvZ[l9], vertexMvZ[l10], vertexMvZ[l11], triangleColourOrTexture[i]);
//					}
//				}
//				if (l == 4) {
//					if (j3 < 0 || j4 < 0 || j5 < 0 || j3 > DrawingArea.centerX || j4 > DrawingArea.centerX || j5 > DrawingArea.centerX || anIntArray1678[3] < 0 || anIntArray1678[3] > DrawingArea.centerX)
//						Texture.restrict_edges = true;
//					int i8;
//					if (triangleDrawType == null)
//						i8 = 0;
//					else
//						i8 = triangleDrawType[i] & 3;
//					if (i8 == 0) {
//						Texture.drawShadedTriangle(i7, j7, k7, j3, j4, j5, anIntArray1680[0], anIntArray1680[1], anIntArray1680[2]);
//						Texture.drawShadedTriangle(i7, k7, anIntArray1679[3], j3, j5, anIntArray1678[3], anIntArray1680[0], anIntArray1680[2], anIntArray1680[3]);
//						return;
//					}
//					if (i8 == 1) {
//						int l8 = HSL2RGB[triangle_hsl_a[i]];
//						Texture.drawFlatTriangle(i7, j7, k7, j3, j4, j5, l8);
//						Texture.drawFlatTriangle(i7, k7, anIntArray1679[3], j3, j5, anIntArray1678[3], l8);
//						return;
//					}
//					if (i8 == 2) {
//						int i9 = triangleDrawType[i] >> 2;
//						int i10 = triPIndex[i9];
//						int i11 = triMIndex[i9];
//						int i12 = triNIndex[i9];
//						Texture.drawTexturedTriangle(i7, j7, k7, j3, j4, j5, anIntArray1680[0], anIntArray1680[1], anIntArray1680[2], vertexMvX[i10], vertexMvX[i11], vertexMvX[i12], vertexMvY[i10], vertexMvY[i11], vertexMvY[i12], vertexMvZ[i10], vertexMvZ[i11], vertexMvZ[i12], triangleColourOrTexture[i]);
//						Texture.drawTexturedTriangle(i7, k7, anIntArray1679[3], j3, j5, anIntArray1678[3], anIntArray1680[0], anIntArray1680[2], anIntArray1680[3], vertexMvX[i10], vertexMvX[i11], vertexMvX[i12], vertexMvY[i10], vertexMvY[i11], vertexMvY[i12], vertexMvZ[i10], vertexMvZ[i11], vertexMvZ[i12], triangleColourOrTexture[i]);
//						return;
//					}
//					if (i8 == 3) {
//						int j9 = triangleDrawType[i] >> 2;
//						int j10 = triPIndex[j9];
//						int j11 = triMIndex[j9];
//						int j12 = triNIndex[j9];
//						Texture.drawTexturedTriangle(i7, j7, k7, j3, j4, j5, triangle_hsl_a[i], triangle_hsl_a[i], triangle_hsl_a[i], vertexMvX[j10], vertexMvX[j11], vertexMvX[j12], vertexMvY[j10], vertexMvY[j11], vertexMvY[j12], vertexMvZ[j10], vertexMvZ[j11], vertexMvZ[j12], triangleColourOrTexture[i]);
//						Texture.drawTexturedTriangle(i7, k7, anIntArray1679[3], j3, j5, anIntArray1678[3], triangle_hsl_a[i], triangle_hsl_a[i], triangle_hsl_a[i], vertexMvX[j10], vertexMvX[j11], vertexMvX[j12], vertexMvY[j10], vertexMvY[j11], vertexMvY[j12], vertexMvZ[j10], vertexMvZ[j11], vertexMvZ[j12], triangleColourOrTexture[i]);
//					}
//				}
//			}
//		}
//		
//		private bool method486(int i, int j, int k, int l, int i1, int j1, int k1,
//		                          int l1) {
//			if (j < k && j < l && j < i1)
//				return false;
//			if (j > k && j > l && j > i1)
//				return false;
//			return !(i < j1 && i < k1 && i < l1) && (i <= j1 || i <= k1 || i <= l1);
//		}
//
//		public bool equals(System.Object o) {
//			if (this == o)
//				return true;
//			if (o == null || this == o)
//				return false;
//			
//			Model model = (Model) o;
//			
//			if (aBoolean1659 != model.aBoolean1659)
//				return false;
//			if (anInt1641 != model.anInt1641)
//				return false;
//			if (anInt1654 != model.anInt1654)
//				return false;
//			if (diagonal3D != model.diagonal3D)
//				return false;
//			if (diagonal3DAboveorigin != model.diagonal3DAboveorigin)
//				return false;
//			if (hash != model.hash)
//				return false;
//			if (maxX != model.maxX)
//				return false;
//			if (maxY != model.maxY)
//				return false;
//			if (maxZ != model.maxZ)
//				return false;
//			if (minX != model.minX)
//				return false;
//			if (minZ != model.minZ)
//				return false;
//			if (textureTriangleCount != model.textureTriangleCount)
//				return false;
//			if (numTriangles != model.numTriangles)
//				return false;
//			if (numVertices != model.numVertices)
//				return false;
//			
//			return true;
//		}
//
//		public int hashCode() {
//			int result = (int) (hash ^ (hash >> 32));
//			result = 31 * result + numVertices;
//			result = 31 * result + numTriangles;
//			result = 31 * result + anInt1641;
//			result = 31 * result + textureTriangleCount;
//			result = 31 * result + minX;
//			result = 31 * result + maxX;
//			result = 31 * result + maxZ;
//			result = 31 * result + minZ;
//			result = 31 * result + maxY;
//			result = 31 * result + diagonal3D;
//			result = 31 * result + diagonal3DAboveorigin;
//			result = 31 * result + anInt1654;
//			result = 31 * result + (aBoolean1659 ? 1 : 0);
//			return result;
//		}
//		
//		public static Model aModel_1621 = new Model();
//		private static int[] anIntArray1622 = new int[2000];
//		private static int[] anIntArray1623 = new int[2000];
//		private static int[] anIntArray1624 = new int[2000];
//		private static int[] anIntArray1625 = new int[2000];
//		public int numVertices;
//		public int[] verticesX;
//		public int[] verticesY;
//		public int[] verticesZ;
//		public int numTriangles;
//		public int[] triangleViewspaceX;
//		public int[] triangleViewspaceY;
//		public int[] triangleViewspaceZ;
//		public int[] triangle_hsl_a;
//		public int[] triangle_hsl_b;
//		public int[] triangle_hsl_c;
//		public int[] triangleDrawType;
//		public int[] facePriority;
//		public int[] triangleAlpha;
//		public int[] triangleColourOrTexture;
//		
//		private int anInt1641;
//		public int textureTriangleCount;
//		public int[] triPIndex;
//		public int[] triMIndex;
//		public int[] triNIndex;
//		public int minX;
//		public int maxX;
//		public int maxZ;
//		public int minZ;
//		public int anInt1650;
//		public int maxY;
//		private int diagonal3D;
//		private int diagonal3DAboveorigin;
//		public int anInt1654;
//		public int[] vertexVSkin;
//		public int[] triangleTSkin;
//		public int[][] anIntArrayArray1657;
//		public int[][] anIntArrayArray1658;
//		public bool aBoolean1659;
//		VertexNormal[] vertexNormalOffset;
//		private static ModelHeader[] modelHeaderCache;
//		private static OnDemandFetcherParent abstractODFetcher;
//		private static bool[] aboolArray1663 = new bool[4096];
//		public static bool[] aboolArray1664 = new bool[4096];
//		private static int[] vertex_screen_x = new int[4096];
//		private static int[] vertex_screen_y = new int[4096];
//		private static int[] vertex_screen_z = new int[4096];
//		private static int[] vertexMvX = new int[4096];
//		private static int[] vertexMvY = new int[4096];
//		private static int[] vertexMvZ = new int[4096];
//		private static int[] depthListIndices = new int[1500];
//		private static int[][] faceLists = NetDrawingTools.CreateDoubleIntArray (1500, 512);//new int[1500][512];
//		private static int[] anIntArray1673 = new int[12];
//		private static int[][] anIntArrayArray1674 = NetDrawingTools.CreateDoubleIntArray (12, 2000);//new int[12][2000];
//		private static int[] anIntArray1675 = new int[2000];
//		private static int[] anIntArray1676 = new int[2000];
//		private static int[] anIntArray1677 = new int[12];
//		private static int[] anIntArray1678 = new int[10];
//		private static int[] anIntArray1679 = new int[10];
//		private static int[] anIntArray1680 = new int[10];
//		private static int vertexXModifier;
//		private static int vertexYModifier;
//		private static int vertexZModifier;
//		public static bool aBoolean1684;
//		public static int anInt1685;//1685
//		public static int anInt1686;//1686
//		public static int anInt1687;
//		public static int[] anIntArray1688 = new int[1000];
//		public static int[] SINE;
//		public static int[] COSINE;
//		private static int[] HSL2RGB;
//		private static int[] modelIntArray4;
//		
//		static Model(){
//			SINE = Texture.SINE;
//			COSINE = Texture.COSINE;
//			HSL2RGB = Texture.hsl2rgb;
//			modelIntArray4 = Texture.anIntArray1469;
//		}
	}
}
