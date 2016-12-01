using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

namespace RS2Sharp
{
	public class Model : Animable
	{
		public bool isGfx = false;
		public static void nullLoader() {
			aClass21Array1661 = null;
			aBooleanArray1663 = null;
			aBooleanArray1664 = null;
			anIntArray1666 = null;
			anIntArray1667 = null;
			anIntArray1668 = null;
			anIntArray1669 = null;
			anIntArray1670 = null;
			anIntArray1671 = null;
			anIntArrayArray1672 = null;
			anIntArray1673 = null;
			anIntArrayArray1674 = null;
			anIntArray1675 = null;
			anIntArray1676 = null;
			anIntArray1677 = null;
			SINE = null;
			COSINE = null;
			modelIntArray3 = null;
			modelIntArray4 = null;
		}
		
		public void read525Model(byte[] abyte0, sbyte[] sbyteversion, int modelID) {
			try{
//			byte[] converted = new byte[abyte0.Length];
//			Buffer.BlockCopy(abyte0, 0, converted, 0, abyte0.Length);
				Stream nc1 = new Stream(abyte0);
				Stream nc2 = new Stream(abyte0);
				Stream nc3 = new Stream(abyte0);
				Stream nc4 = new Stream(abyte0);
				Stream nc5 = new Stream(abyte0);
				Stream nc6 = new Stream(abyte0);
				Stream nc7 = new Stream(abyte0);
			nc1.currentOffset = abyte0.Length - 23;
			int numVertices = nc1.readUnsignedWord();
			int numTriangles = nc1.readUnsignedWord();
			int numTexTriangles = nc1.readUnsignedByte();
			Class21 ModelDef_1 = aClass21Array1661[modelID] = new Class21();
				ModelDef_1.aByteArray368 = sbyteversion;
			ModelDef_1.anInt369 = numVertices;
			ModelDef_1.anInt370 = numTriangles;
			ModelDef_1.anInt371 = numTexTriangles;
			int l1 = nc1.readUnsignedByte();
			bool abool = (0x1 & l1 ^ 0xffffffff) == -2;
			int i2 = nc1.readUnsignedByte();
			int j2 = nc1.readUnsignedByte();
			int k2 = nc1.readUnsignedByte();
			int l2 = nc1.readUnsignedByte();
			int i3 = nc1.readUnsignedByte();
			int j3 = nc1.readUnsignedWord();
			int k3 = nc1.readUnsignedWord();
			int l3 = nc1.readUnsignedWord();
			int i4 = nc1.readUnsignedWord();
			int j4 = nc1.readUnsignedWord();
			int k4 = 0;
			int l4 = 0;
			int i5 = 0;
			sbyte[] x = null;
			sbyte[] O = null;
			sbyte[] J = null;
			sbyte[] F = null;
			sbyte[] cb = null;
			sbyte[] gb = null;
			sbyte[] lb = null;
			int[] kb = null;
			int[] y = null;
			int[] N = null;
			short[] D = null;
			int[] triangleColours2 = new int[numTriangles];
			if (numTexTriangles > 0) {
				O = new sbyte[numTexTriangles];
				nc1.currentOffset = 0;
				for (int j5 = 0; j5 < numTexTriangles; j5++) {
					sbyte byte0 = O[j5] = nc1.readSignedByte();
					if (byte0 == 0)
						k4++;
					if (byte0 >= 1 && byte0 <= 3)
						l4++;
					if (byte0 == 2)
						i5++;
				}
			}
			int k5 = numTexTriangles;
			int l5 = k5;
			k5 += numVertices;
			int i6 = k5;
			if (l1 == 1)
				k5 += numTriangles;
			int j6 = k5;
			k5 += numTriangles;
			int k6 = k5;
			if (i2 == 255)
				k5 += numTriangles;
			int l6 = k5;
			if (k2 == 1)
				k5 += numTriangles;
			int i7 = k5;
			if (i3 == 1)
				k5 += numVertices;
			int j7 = k5;
			if (j2 == 1)
				k5 += numTriangles;
			int k7 = k5;
			k5 += i4;
			int l7 = k5;
			if (l2 == 1)
				k5 += numTriangles * 2;
			int i8 = k5;
			k5 += j4;
			int j8 = k5;
			k5 += numTriangles * 2;
			int k8 = k5;
			k5 += j3;
			int l8 = k5;
			k5 += k3;
			int i9 = k5;
			k5 += l3;
			int j9 = k5;
			k5 += k4 * 6;
			int k9 = k5;
			k5 += l4 * 6;
			int l9 = k5;
			k5 += l4 * 6;
			int i10 = k5;
			k5 += l4;
			int j10 = k5;
			k5 += l4;
			int k10 = k5;
			k5 += l4 + i5 * 2;
			int[] vertexX = new int[numVertices];
			int[] vertexY = new int[numVertices];
			int[] vertexZ = new int[numVertices];
			int[] facePoint1 = new int[numTriangles];
			int[] facePoint2 = new int[numTriangles];
			int[] facePoint3 = new int[numTriangles];
			anIntArray1655 = new int[numVertices];
			triangleDrawType = new int[numTriangles];
			facePriority = new int[numTriangles];
			triangleAlpha = new int[numTriangles];
			anIntArray1656 = new int[numTriangles];
			if (i3 == 1)
				anIntArray1655 = new int[numVertices];
			if (abool)
				triangleDrawType = new int[numTriangles];
			if (i2 == 255)
				facePriority = new int[numTriangles];
			else {
			}
			if (j2 == 1)
				triangleAlpha = new int[numTriangles];
			if (k2 == 1)
				anIntArray1656 = new int[numTriangles];
			if (l2 == 1)
				D = new short[numTriangles];
			if (l2 == 1 && numTexTriangles > 0)
				x = new sbyte[numTriangles];
			triangleColours2 = new int[numTriangles];
			int[] texTrianglesPoint1 = null;
			int[] texTrianglesPoint2 = null;
			int[] texTrianglesPoint3 = null;
			if (numTexTriangles > 0) {
				texTrianglesPoint1 = new int[numTexTriangles];
				texTrianglesPoint2 = new int[numTexTriangles];
				texTrianglesPoint3 = new int[numTexTriangles];
				if (l4 > 0) {
					kb = new int[l4];
					N = new int[l4];
					y = new int[l4];
					gb = new sbyte[l4];
					lb = new sbyte[l4];
					F = new sbyte[l4];
				}
				if (i5 > 0) {
					cb = new sbyte[i5];
					J = new sbyte[i5];
				}
			}
			nc1.currentOffset = l5;
			nc2.currentOffset = k8;
			nc3.currentOffset = l8;
			nc4.currentOffset = i9;
			nc5.currentOffset = i7;
			int l10 = 0;
			int i11 = 0;
			int j11 = 0;
			for (int k11 = 0; k11 < numVertices; k11++) {
				int l11 = nc1.readUnsignedByte();
				int j12 = 0;
				if ((l11 & 1) != 0)
					j12 = nc2.method421();
				int l12 = 0;
				if ((l11 & 2) != 0)
					l12 = nc3.method421();
				int j13 = 0;
				if ((l11 & 4) != 0)
					j13 = nc4.method421();
				vertexX[k11] = l10 + j12;
				vertexY[k11] = i11 + l12;
				vertexZ[k11] = j11 + j13;
				l10 = vertexX[k11];
				i11 = vertexY[k11];
				j11 = vertexZ[k11];
				if (anIntArray1655 != null)
					anIntArray1655[k11] = nc5.readUnsignedByte();
			}
			nc1.currentOffset = j8;
			nc2.currentOffset = i6;
			nc3.currentOffset = k6;
			nc4.currentOffset = j7;
			nc5.currentOffset = l6;
			nc6.currentOffset = l7;
			nc7.currentOffset = i8;
			for (int i12 = 0; i12 < numTriangles; i12++) {
				triangleColours2[i12] = nc1.readUnsignedWord();
				if (l1 == 1) {
					triangleDrawType[i12] = nc2.readSignedByte();
					if (triangleDrawType[i12] == 2)
						triangleColours2[i12] = 65535;
					triangleDrawType[i12] = 0;
				}
				if (i2 == 255) {
					facePriority[i12] = nc3.readSignedByte();
				}
				if (j2 == 1) {
					triangleAlpha[i12] = nc4.readSignedByte();
					if (triangleAlpha[i12] < 0)
						triangleAlpha[i12] = (256 + triangleAlpha[i12]);
				}
				if (k2 == 1)
					anIntArray1656[i12] = nc5.readUnsignedByte();
				if (l2 == 1)
					D[i12] = (short) (nc6.readUnsignedWord() - 1);
				if (x != null)
					if (D[i12] != -1)
						x[i12] = (sbyte) (nc7.readUnsignedByte() - 1);
				else
					x[i12] = -1;
			}
			nc1.currentOffset = k7;
			nc2.currentOffset = j6;
			int k12 = 0;
			int i13 = 0;
			int k13 = 0;
			int l13 = 0;
			for (int i14 = 0; i14 < numTriangles; i14++) {
				int j14 = nc2.readUnsignedByte();
				if (j14 == 1) {
					k12 = nc1.method421() + l13;
					l13 = k12;
					i13 = nc1.method421() + l13;
					l13 = i13;
					k13 = nc1.method421() + l13;
					l13 = k13;
					facePoint1[i14] = k12;
					facePoint2[i14] = i13;
					facePoint3[i14] = k13;
				}
				if (j14 == 2) {
					i13 = k13;
					k13 = nc1.method421() + l13;
					l13 = k13;
					facePoint1[i14] = k12;
					facePoint2[i14] = i13;
					facePoint3[i14] = k13;
				}
				if (j14 == 3) {
					k12 = k13;
					k13 = nc1.method421() + l13;
					l13 = k13;
					facePoint1[i14] = k12;
					facePoint2[i14] = i13;
					facePoint3[i14] = k13;
				}
				if (j14 == 4) {
					int l14 = k12;
					k12 = i13;
					i13 = l14;
					k13 = nc1.method421() + l13;
					l13 = k13;
					facePoint1[i14] = k12;
					facePoint2[i14] = i13;
					facePoint3[i14] = k13;
				}
			}
			nc1.currentOffset = j9;
			nc2.currentOffset = k9;
			nc3.currentOffset = l9;
			nc4.currentOffset = i10;
			nc5.currentOffset = j10;
			nc6.currentOffset = k10;
			for (int k14 = 0; k14 < numTexTriangles; k14++) {
				int i15 = O[k14] & 0xff;
				if (i15 == 0) {
					texTrianglesPoint1[k14] = nc1.readUnsignedWord();
					texTrianglesPoint2[k14] = nc1.readUnsignedWord();
					texTrianglesPoint3[k14] = nc1.readUnsignedWord();
				}
				if (i15 == 1) {
					texTrianglesPoint1[k14] = nc2.readUnsignedWord();
					texTrianglesPoint2[k14] = nc2.readUnsignedWord();
					texTrianglesPoint3[k14] = nc2.readUnsignedWord();
					kb[k14] = nc3.readUnsignedWord();
					N[k14] = nc3.readUnsignedWord();
					y[k14] = nc3.readUnsignedWord();
					gb[k14] = nc4.readSignedByte();
					lb[k14] = nc5.readSignedByte();
					F[k14] = nc6.readSignedByte();
				}
				if (i15 == 2) {
					texTrianglesPoint1[k14] = nc2.readUnsignedWord();
					texTrianglesPoint2[k14] = nc2.readUnsignedWord();
					texTrianglesPoint3[k14] = nc2.readUnsignedWord();
					kb[k14] = nc3.readUnsignedWord();
					N[k14] = nc3.readUnsignedWord();
					y[k14] = nc3.readUnsignedWord();
					gb[k14] = nc4.readSignedByte();
					lb[k14] = nc5.readSignedByte();
					F[k14] = nc6.readSignedByte();
					cb[k14] = nc6.readSignedByte();
					J[k14] = nc6.readSignedByte();
				}
				if (i15 == 3) {
					texTrianglesPoint1[k14] = nc2.readUnsignedWord();
					texTrianglesPoint2[k14] = nc2.readUnsignedWord();
					texTrianglesPoint3[k14] = nc2.readUnsignedWord();
					kb[k14] = nc3.readUnsignedWord();
					N[k14] = nc3.readUnsignedWord();
					y[k14] = nc3.readUnsignedWord();
					gb[k14] = nc4.readSignedByte();
					lb[k14] = nc5.readSignedByte();
					F[k14] = nc6.readSignedByte();
				}
			}
			if (i2 != 255) {
				for (int i12 = 0; i12 < numTriangles; i12++)
					facePriority[i12] = i2;
			}
			this.triangleColourOrTexture = triangleColours2;
				this.numVertices = numVertices;
				this.numTriangles = numTriangles;
				this.verticesX = vertexX;
				this.verticesY = vertexY;
				this.verticesZ = vertexZ;
				this.triangleX = facePoint1;
				this.triangleY = facePoint2;
				this.triangleZ = facePoint3;
			}
			catch(Exception ex)
			{
				Debug.Log ("Read525Error: " + ex.Message + " - " + ex.StackTrace);
			}
		}

//		public byte[] GetModel(int id)
//		{
//			byte[] modelBytes = ReadAllBytes (sign.signlink.findcachedir()+"index1/" + id + ".dat");
//			Model.method460(modelBytes, id);
//			return modelBytes;
//		}

		public static byte[] ReadAllBytes (string fileName)
		{
			try {
				byte[] buffer = null;
				using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read)) {
					buffer = new byte[fs.Length];
					fs.Read (buffer, 0, (int)fs.Length);
				}
				return buffer;
			} catch (System.Exception ex){
				Debug.Log ("Error: " + ex.Message);
				return null;
			}
		} 


		public Model(byte[] modelData, int modelId) {

			sbyte[] iss = new sbyte[modelData.Length];
			for (int i = 0; i < modelData.Length; ++i)
				iss [i] = (sbyte)modelData [i];
			if (iss [iss.Length - 1] == -1 && iss [iss.Length - 2] == -1) {
				read622Model (modelData, iss, modelId);
			}
			else
				readOldModel(modelData, modelId);
			if (newmodel[modelId]) {
				if (facePriority != null) {
					if (modelId >= 1 && modelId <= 65535) {
						for (int index = 0; index < facePriority.Length; index++) {
							facePriority[index] = 10;
						}
					}
				}
			}
		}
		
		public Model(int modelId) {
			byte[] modelData = UnityClient.GetModel (modelId);
			if(modelData == null) return;
			method460(modelData, modelId);
			sbyte[] iss = new sbyte[modelData.Length];
			for (int i = 0; i < modelData.Length; ++i)
				iss [i] = (sbyte)modelData [i];
			if (iss [iss.Length - 1] == -1 && iss [iss.Length - 2] == -1) {
								read622Model (modelData, iss, modelId);
						}
			else
				readOldModel(modelData, modelId);
			if (newmodel[modelId]) {
				if (facePriority != null) {
					if (modelId >= 1 && modelId <= 65535) {
						for (int index = 0; index < facePriority.Length; index++) {
							facePriority[index] = 10;
						}
					}
				}
			}
		}
		
		public void scale2(int i) {
			for (int i1 = 0; i1 < numVertices; i1++) {
				verticesX[i1] = verticesX[i1] / i;
				verticesY[i1] = verticesY[i1] / i;
				verticesZ[i1] = verticesZ[i1] / i;
			}
		}
		
		public void read622Model(byte[] abyte0, sbyte[] sbyteVersion, int modelID) {
//			byte[] converted = new byte[abyte0.Length];
//			Buffer.BlockCopy(abyte0, 0, converted, 0, abyte0.Length);
			isNew = true;
			try{
			Stream nc1 = new Stream(abyte0);
			Stream nc2 = new Stream(abyte0);
			Stream nc3 = new Stream(abyte0);
			Stream nc4 = new Stream(abyte0);
			Stream nc5 = new Stream(abyte0);
			Stream nc6 = new Stream(abyte0);
			Stream nc7 = new Stream(abyte0);
			nc1.currentOffset = abyte0.Length - 23;
			int numVertices = nc1.readUnsignedWord();
			int numTriangles = nc1.readUnsignedWord();
			int numTexTriangles = nc1.readUnsignedByte();
			Class21 ModelDef_1 = aClass21Array1661[modelID] = new Class21();
			ModelDef_1.aByteArray368 = sbyteVersion;
			ModelDef_1.anInt369 = numVertices;
			ModelDef_1.anInt370 = numTriangles;
			ModelDef_1.anInt371 = numTexTriangles;
			int l1 = nc1.readUnsignedByte();
			bool abool = (0x1 & l1 ^ 0xffffffff) == -2;
			bool bool_26_ = (0x8 & l1) == 8;
			if (!bool_26_) {
				read525Model(abyte0, sbyteVersion, modelID);
				return;
			}
			int newformat = 0;
			if (bool_26_) {
				nc1.currentOffset -= 7;
				newformat = nc1.readUnsignedByte();
				nc1.currentOffset += 6;
			}
			if (newformat == 15)
				newmodel[modelID] = true;
			int i2 = nc1.readUnsignedByte();
			int j2 = nc1.readUnsignedByte();
			int k2 = nc1.readUnsignedByte();
			int l2 = nc1.readUnsignedByte();
			int i3 = nc1.readUnsignedByte();
			int j3 = nc1.readUnsignedWord();
			int k3 = nc1.readUnsignedWord();
			int l3 = nc1.readUnsignedWord();
			int i4 = nc1.readUnsignedWord();
			int j4 = nc1.readUnsignedWord();
			int k4 = 0;
			int l4 = 0;
			int i5 = 0;
			sbyte[] x = null;
			sbyte[] O = null;
			sbyte[] J = null;
			sbyte[] F = null;
			sbyte[] cb = null;
			sbyte[] gb = null;
			sbyte[] lb = null;
			int[] kb = null;
			int[] y = null;
			int[] N = null;
			short[] D = null;
			int[] triangleColours2 = new int[numTriangles];
			if (numTexTriangles > 0) {
				O = new sbyte[numTexTriangles];
				nc1.currentOffset = 0;
				for (int j5 = 0; j5 < numTexTriangles; j5++) {
					sbyte byte0 = O[j5] = nc1.readSignedByte();
					if (byte0 == 0)
						k4++;
					if (byte0 >= 1 && byte0 <= 3)
						l4++;
					if (byte0 == 2)
						i5++;
				}
			}
			int k5 = numTexTriangles;
			int l5 = k5;
			k5 += numVertices;
			int i6 = k5;
			if (abool)
				k5 += numTriangles;
			if (l1 == 1)
				k5 += numTriangles;
			int j6 = k5;
			k5 += numTriangles;
			int k6 = k5;
			if (i2 == 255)
				k5 += numTriangles;
			int l6 = k5;
			if (k2 == 1)
				k5 += numTriangles;
			int i7 = k5;
			if (i3 == 1)
				k5 += numVertices;
			int j7 = k5;
			if (j2 == 1)
				k5 += numTriangles;
			int k7 = k5;
			k5 += i4;
			int l7 = k5;
			if (l2 == 1)
				k5 += numTriangles * 2;
			int i8 = k5;
			k5 += j4;
			int j8 = k5;
			k5 += numTriangles * 2;
			int k8 = k5;
			k5 += j3;
			int l8 = k5;
			k5 += k3;
			int i9 = k5;
			k5 += l3;
			int j9 = k5;
			k5 += k4 * 6;
			int k9 = k5;
			k5 += l4 * 6;
			int i_59_ = 6;
			if (newformat != 14) {
				if (newformat >= 15)
					i_59_ = 9;
			} else
				i_59_ = 7;
			int l9 = k5;
			k5 += i_59_ * l4;
			int i10 = k5;
			k5 += l4;
			int j10 = k5;
			k5 += l4;
			int k10 = k5;
			k5 += l4 + i5 * 2;
			int[] vertexX = new int[numVertices];
			int[] vertexY = new int[numVertices];
			int[] vertexZ = new int[numVertices];
			int[] facePoint1 = new int[numTriangles];
			int[] facePoint2 = new int[numTriangles];
			int[] facePoint3 = new int[numTriangles];
			anIntArray1655 = new int[numVertices];
			triangleDrawType = new int[numTriangles];
			facePriority = new int[numTriangles];
			triangleAlpha = new int[numTriangles];
			anIntArray1656 = new int[numTriangles];
			if (i3 == 1)
				anIntArray1655 = new int[numVertices];
			if (abool)
				triangleDrawType = new int[numTriangles];
			if (i2 == 255)
				facePriority = new int[numTriangles];
			else {
			}
			if (j2 == 1)
				triangleAlpha = new int[numTriangles];
			if (k2 == 1)
				anIntArray1656 = new int[numTriangles];
			if (l2 == 1)
				D = new short[numTriangles];
			if (l2 == 1 && numTexTriangles > 0)
				x = new sbyte[numTriangles];
			triangleColours2 = new int[numTriangles];
			int[] texTrianglesPoint1 = null;
			int[] texTrianglesPoint2 = null;
			int[] texTrianglesPoint3 = null;
			if (numTexTriangles > 0) {
				texTrianglesPoint1 = new int[numTexTriangles];
				texTrianglesPoint2 = new int[numTexTriangles];
				texTrianglesPoint3 = new int[numTexTriangles];
				if (l4 > 0) {
					kb = new int[l4];
					N = new int[l4];
					y = new int[l4];
					gb = new sbyte[l4];
					lb = new sbyte[l4];
					F = new sbyte[l4];
				}
				if (i5 > 0) {
					cb = new sbyte[i5];
					J = new sbyte[i5];
				}
			}
			nc1.currentOffset = l5;
			nc2.currentOffset = k8;
			nc3.currentOffset = l8;
			nc4.currentOffset = i9;
			nc5.currentOffset = i7;
			int l10 = 0;
			int i11 = 0;
			int j11 = 0;
			for (int k11 = 0; k11 < numVertices; k11++) {
				int l11 = nc1.readUnsignedByte();
				int j12 = 0;
				if ((l11 & 1) != 0)
					j12 = nc2.method421();
				int l12 = 0;
				if ((l11 & 2) != 0)
					l12 = nc3.method421();
				int j13 = 0;
				if ((l11 & 4) != 0)
					j13 = nc4.method421();
				vertexX[k11] = l10 + j12;
				vertexY[k11] = i11 + l12;
				vertexZ[k11] = j11 + j13;
				l10 = vertexX[k11];
				i11 = vertexY[k11];
				j11 = vertexZ[k11];
				if (anIntArray1655 != null)
					anIntArray1655[k11] = nc5.readUnsignedByte();
			}
			nc1.currentOffset = j8;
			nc2.currentOffset = i6;
			nc3.currentOffset = k6;
			nc4.currentOffset = j7;
			nc5.currentOffset = l6;
			nc6.currentOffset = l7;
			nc7.currentOffset = i8;
			for (int i12 = 0; i12 < numTriangles; i12++) {
				triangleColours2[i12] = nc1.readUnsignedWord();
				if (l1 == 1) {
					triangleDrawType[i12] = nc2.readSignedByte();
					if (triangleDrawType[i12] == 2)
						triangleColours2[i12] = 65535;
					triangleDrawType[i12] = 0;
				}
				if (i2 == 255) {
					facePriority[i12] = nc3.readSignedByte();
				}
				if (j2 == 1) {
					triangleAlpha[i12] = nc4.readSignedByte();
					if (triangleAlpha[i12] < 0)
						triangleAlpha[i12] = (256 + triangleAlpha[i12]);
				}
				if (k2 == 1)
					anIntArray1656[i12] = nc5.readUnsignedByte();
				if (l2 == 1)
					D[i12] = (short) (nc6.readUnsignedWord() - 1);
				if (x != null)
					if (D[i12] != -1)
						x[i12] = (sbyte) (nc7.readUnsignedByte() - 1);
				else
					x[i12] = -1;
			}
			nc1.currentOffset = k7;
			nc2.currentOffset = j6;
			int k12 = 0;
			int i13 = 0;
			int k13 = 0;
			int l13 = 0;
			for (int i14 = 0; i14 < numTriangles; i14++) {
				int j14 = nc2.readUnsignedByte();
				if (j14 == 1) {
					k12 = nc1.method421() + l13;
					l13 = k12;
					i13 = nc1.method421() + l13;
					l13 = i13;
					k13 = nc1.method421() + l13;
					l13 = k13;
					facePoint1[i14] = k12;
					facePoint2[i14] = i13;
					facePoint3[i14] = k13;
				}
				if (j14 == 2) {
					i13 = k13;
					k13 = nc1.method421() + l13;
					l13 = k13;
					facePoint1[i14] = k12;
					facePoint2[i14] = i13;
					facePoint3[i14] = k13;
				}
				if (j14 == 3) {
					k12 = k13;
					k13 = nc1.method421() + l13;
					l13 = k13;
					facePoint1[i14] = k12;
					facePoint2[i14] = i13;
					facePoint3[i14] = k13;
				}
				if (j14 == 4) {
					int l14 = k12;
					k12 = i13;
					i13 = l14;
					k13 = nc1.method421() + l13;
					l13 = k13;
					facePoint1[i14] = k12;
					facePoint2[i14] = i13;
					facePoint3[i14] = k13;
				}
			}
			nc1.currentOffset = j9;
			nc2.currentOffset = k9;
			nc3.currentOffset = l9;
			nc4.currentOffset = i10;
			nc5.currentOffset = j10;
			nc6.currentOffset = k10;
			for (int k14 = 0; k14 < numTexTriangles; k14++) {
				int i15 = O[k14] & 0xff;
				if (i15 == 0) {
					texTrianglesPoint1[k14] = nc1.readUnsignedWord();
					texTrianglesPoint2[k14] = nc1.readUnsignedWord();
					texTrianglesPoint3[k14] = nc1.readUnsignedWord();
				}
				if (i15 == 1) {
					texTrianglesPoint1[k14] = nc2.readUnsignedWord();
					texTrianglesPoint2[k14] = nc2.readUnsignedWord();
					texTrianglesPoint3[k14] = nc2.readUnsignedWord();
					if (newformat < 15) {
						kb[k14] = nc3.readUnsignedWord();
						if (newformat >= 14)
							N[k14] = nc3.v(-1);
						else
							N[k14] = nc3.readUnsignedWord();
						y[k14] = nc3.readUnsignedWord();
					} else {
						kb[k14] = nc3.v(-1);
						N[k14] = nc3.v(-1);
						y[k14] = nc3.v(-1);
					}
					gb[k14] = nc4.readSignedByte();
					lb[k14] = nc5.readSignedByte();
					F[k14] = nc6.readSignedByte();
				}
				if (i15 == 2) {
					texTrianglesPoint1[k14] = nc2.readUnsignedWord();
					texTrianglesPoint2[k14] = nc2.readUnsignedWord();
					texTrianglesPoint3[k14] = nc2.readUnsignedWord();
					if (newformat >= 15) {
						kb[k14] = nc3.v(-1);
						N[k14] = nc3.v(-1);
						y[k14] = nc3.v(-1);
					} else {
						kb[k14] = nc3.readUnsignedWord();
						if (newformat < 14)
							N[k14] = nc3.readUnsignedWord();
						else
							N[k14] = nc3.v(-1);
						y[k14] = nc3.readUnsignedWord();
					}
					gb[k14] = nc4.readSignedByte();
					lb[k14] = nc5.readSignedByte();
					F[k14] = nc6.readSignedByte();
					cb[k14] = nc6.readSignedByte();
					J[k14] = nc6.readSignedByte();
				}
				if (i15 == 3) {
					texTrianglesPoint1[k14] = nc2.readUnsignedWord();
					texTrianglesPoint2[k14] = nc2.readUnsignedWord();
					texTrianglesPoint3[k14] = nc2.readUnsignedWord();
					if (newformat < 15) {
						kb[k14] = nc3.readUnsignedWord();
						if (newformat < 14)
							N[k14] = nc3.readUnsignedWord();
						else
							N[k14] = nc3.v(-1);
						y[k14] = nc3.readUnsignedWord();
					} else {
						kb[k14] = nc3.v(-1);
						N[k14] = nc3.v(-1);
						y[k14] = nc3.v(-1);
					}
					gb[k14] = nc4.readSignedByte();
					lb[k14] = nc5.readSignedByte();
					F[k14] = nc6.readSignedByte();
				}
			}
			if (i2 != 255) {
				for (int i12 = 0; i12 < numTriangles; i12++)
					facePriority[i12] = i2;
			}
			this.triangleColourOrTexture = triangleColours2;
				this.numVertices = numVertices;
				this.numTriangles = numTriangles;
				this.verticesX = vertexX;
				this.verticesY = vertexY;
				this.verticesZ = vertexZ;
				this.triangleX = facePoint1;
				this.triangleY = facePoint2;
				this.triangleZ = facePoint3;
			scale2(4);
			}
			catch(Exception ex)
			{
				Debug.Log ("Read622Error : " + ex.StackTrace);
			}
		}
		
		private void readOldModel(byte[] abyte, int i) {
			try{
				isNew = false;
			int j = -870;
			aBoolean1618 = true;
			aBoolean1659 = false;
			anInt1620++;
			Class21 class21 = aClass21Array1661[i];
				//abyte = class21.aByteArray368;
				//byte[] converted = new byte[abyte.Length];
				//Buffer.BlockCopy(abyte, 0, converted, 0, /*class21.aByteArray368*/abyte.Length);
			numVertices = class21.anInt369;
			numTriangles = class21.anInt370;
			textureTriangleCount = class21.anInt371;
			verticesX = new int[numVertices];
			verticesY = new int[numVertices];
			verticesZ = new int[numVertices];
			triangleX = new int[numTriangles];
			triangleY = new int[numTriangles];
			while (j >= 0)
				aBoolean1618 = !aBoolean1618;
			triangleZ = new int[numTriangles];
			triPIndex = new int[textureTriangleCount];
			triMIndex = new int[textureTriangleCount];
			triNIndex = new int[textureTriangleCount];
			if (class21.anInt376 >= 0)
				anIntArray1655 = new int[numVertices];
			if (class21.anInt380 >= 0)
				triangleDrawType = new int[numTriangles];
			if (class21.anInt381 >= 0)
				facePriority = new int[numTriangles];
			else
				anInt1641 = -class21.anInt381 - 1;
			if (class21.anInt382 >= 0)
				triangleAlpha = new int[numTriangles];
			if (class21.anInt383 >= 0)
				anIntArray1656 = new int[numTriangles];
			triangleColourOrTexture = new int[numTriangles];
			
				Stream stream = new Stream(abyte);
			stream.currentOffset = class21.anInt372;
				Stream stream_1 = new Stream(abyte);
			stream_1.currentOffset = class21.anInt373;
				Stream stream_2 = new Stream(abyte);
			stream_2.currentOffset = class21.anInt374;
				Stream stream_3 = new Stream(abyte);
			stream_3.currentOffset = class21.anInt375;
				Stream stream_4 = new Stream(abyte);
			stream_4.currentOffset = class21.anInt376;
			int k = 0;
			int l = 0;
			int i1 = 0;
			for (int j1 = 0; j1 < numVertices; j1++) {
				int k1 = stream.readUnsignedByte();
				int i2 = 0;
				if ((k1 & 1) != 0)
					i2 = stream_1.method421();
				int k2 = 0;
				if ((k1 & 2) != 0)
					k2 = stream_2.method421();
				int i3 = 0;
				if ((k1 & 4) != 0)
					i3 = stream_3.method421();
				verticesX[j1] = k + i2;
				verticesY[j1] = l + k2;
				verticesZ[j1] = i1 + i3;
				k = verticesX[j1];
				l = verticesY[j1];
				i1 = verticesZ[j1];
				if (anIntArray1655 != null)
					anIntArray1655[j1] = stream_4.readUnsignedByte();
			}
			stream.currentOffset = class21.anInt379;
			stream_1.currentOffset = class21.anInt380;
			stream_2.currentOffset = class21.anInt381;
			stream_3.currentOffset = class21.anInt382;
			stream_4.currentOffset = class21.anInt383;
			for (int l1 = 0; l1 < numTriangles; l1++) {
				triangleColourOrTexture[l1] = stream.readUnsignedWord();
				if (triangleDrawType != null)
					triangleDrawType[l1] = stream_1.readUnsignedByte();
				if (facePriority != null)
					facePriority[l1] = stream_2.readUnsignedByte();
				if (triangleAlpha != null) {
					triangleAlpha[l1] = stream_3.readUnsignedByte();
				}
				if (anIntArray1656 != null)
					anIntArray1656[l1] = stream_4.readUnsignedByte();
			}
			stream.currentOffset = class21.anInt377;
			stream_1.currentOffset = class21.anInt378;
			int j2 = 0;
			int l2 = 0;
			int j3 = 0;
			int k3 = 0;
			for (int l3 = 0; l3 < numTriangles; l3++) {
				int i4 = stream_1.readUnsignedByte();
				if (i4 == 1) {
					j2 = stream.method421() + k3;
					k3 = j2;
					l2 = stream.method421() + k3;
					k3 = l2;
					j3 = stream.method421() + k3;
					k3 = j3;
					triangleX[l3] = j2;
					triangleY[l3] = l2;
					triangleZ[l3] = j3;
				}
				if (i4 == 2) {
					l2 = j3;
					j3 = stream.method421() + k3;
					k3 = j3;
					triangleX[l3] = j2;
					triangleY[l3] = l2;
					triangleZ[l3] = j3;
				}
				if (i4 == 3) {
					j2 = j3;
					j3 = stream.method421() + k3;
					k3 = j3;
					triangleX[l3] = j2;
					triangleY[l3] = l2;
					triangleZ[l3] = j3;
				}
				if (i4 == 4) {
					int k4 = j2;
					j2 = l2;
					l2 = k4;
					j3 = stream.method421() + k3;
					k3 = j3;
					triangleX[l3] = j2;
					triangleY[l3] = l2;
					triangleZ[l3] = j3;
				}
			}
			stream.currentOffset = class21.anInt384;
			for (int j4 = 0; j4 < textureTriangleCount; j4++) {
				triPIndex[j4] = stream.readUnsignedWord();
				triMIndex[j4] = stream.readUnsignedWord();
				triNIndex[j4] = stream.readUnsignedWord();
			}
			}
			catch(Exception ex)
			{
				Debug.Log ("ReadOldError: " + ex.Message + " - " + ex.StackTrace);
			}
		}
		
		public static void method460(byte[] abyte0, int j) {
			//try {
				if (abyte0 == null) {
					Class21 class21 = aClass21Array1661[j] = new Class21();
					class21.anInt369 = 0;
					class21.anInt370 = 0;
					class21.anInt371 = 0;
					return;
				}
//				byte[] converted = new byte[abyte0.Length];
//				Buffer.BlockCopy(abyte0, 0, converted, 0, abyte0.Length);
				Stream stream = new Stream(abyte0);
				stream.currentOffset = abyte0.Length - 18;
				Class21 class21_1 = aClass21Array1661[j] = new Class21();
				sbyte[] converted = new sbyte[abyte0.Length];
				for (int i = 0; i < abyte0.Length; ++i)
					converted [i] = (sbyte)abyte0 [i];
				class21_1.aByteArray368 = converted;
				class21_1.anInt369 = stream.readUnsignedWord();
				class21_1.anInt370 = stream.readUnsignedWord();
				class21_1.anInt371 = stream.readUnsignedByte();
				int k = stream.readUnsignedByte();
				int l = stream.readUnsignedByte();
				int i1 = stream.readUnsignedByte();
				int j1 = stream.readUnsignedByte();
				int k1 = stream.readUnsignedByte();
				int l1 = stream.readUnsignedWord();
				int i2 = stream.readUnsignedWord();
				int j2 = stream.readUnsignedWord();
				int k2 = stream.readUnsignedWord();
				int l2 = 0;
				class21_1.anInt372 = l2;
				l2 += class21_1.anInt369;
				class21_1.anInt378 = l2;
				l2 += class21_1.anInt370;
				class21_1.anInt381 = l2;
				if (l == 255)
					l2 += class21_1.anInt370;
				else
					class21_1.anInt381 = -l - 1;
				class21_1.anInt383 = l2;
				if (j1 == 1)
					l2 += class21_1.anInt370;
				else
					class21_1.anInt383 = -1;
				class21_1.anInt380 = l2;
				if (k == 1)
					l2 += class21_1.anInt370;
				else
					class21_1.anInt380 = -1;
				class21_1.anInt376 = l2;
				if (k1 == 1)
					l2 += class21_1.anInt369;
				else
					class21_1.anInt376 = -1;
				class21_1.anInt382 = l2;
				if (i1 == 1)
					l2 += class21_1.anInt370;
				else
					class21_1.anInt382 = -1;
				class21_1.anInt377 = l2;
				l2 += k2;
				class21_1.anInt379 = l2;
				l2 += class21_1.anInt370 * 2;
				class21_1.anInt384 = l2;
				l2 += class21_1.anInt371 * 6;
				class21_1.anInt373 = l2;
				l2 += l1;
				class21_1.anInt374 = l2;
				l2 += i2;
				class21_1.anInt375 = l2;
				l2 += j2;
			//} catch (Exception _ex) {
			//}
		}
		
		public static bool[] newmodel;
		
		public static void initialize(int i,
		                             OnDemandFetcherParent onDemandFetcherParent) {
			aClass21Array1661 = new Class21[80000];
			newmodel = new bool[100000];
			aOnDemandFetcherParent_1662 = onDemandFetcherParent;
		}
		
		public static void method461(int j) {
			aClass21Array1661[j] = null;
		}
		
		public static Model method462(int j) {
//			if (aClass21Array1661 == null)
//				return null;
//			Class21 class21 = aClass21Array1661[j];
//			if (class21 == null) {
//				aOnDemandFetcherParent_1662.method548(j);
//				return null;
//			} else {
//				Model aModel = new Model(j);
//				return aModel;
//			}
			return new Model(j);
		}
		
		public static bool method463(int i) {
			if (aClass21Array1661 == null)
				return false;
			
			Class21 class21 = aClass21Array1661[i];
			if (class21 == null) {
				aOnDemandFetcherParent_1662.method548(i);
				return false;
			} else {
				return true;
			}
		}
		
		private Model(bool flag) {
			aBoolean1618 = true;
			aBoolean1659 = false;
			if (!flag)
				aBoolean1618 = !aBoolean1618;
		}
		
		public Model(int i, Model[] amodel) {
			//try{//lol
			aBoolean1618 = true;
			aBoolean1659 = false;
			anInt1620++;
			bool flag = false;
			bool flag1 = false;
			bool flag2 = false;
			bool flag3 = false;
			numVertices = 0;
			numTriangles = 0;
			textureTriangleCount = 0;
			anInt1641 = -1;
			for (int k = 0; k < i; k++) {
				Model model = amodel[k];
				if (model != null) {
					numVertices += model.numVertices;
					numTriangles += model.numTriangles;
					textureTriangleCount += model.textureTriangleCount;
					flag |= model.triangleDrawType != null;
					if (model.facePriority != null) {
						flag1 = true;
					} else {
						if (anInt1641 == -1)
							anInt1641 = model.anInt1641;
						if (anInt1641 != model.anInt1641)
							flag1 = true;
					}
					flag2 |= model.triangleAlpha != null;
					flag3 |= model.anIntArray1656 != null;
				}
			}
			
			verticesX = new int[numVertices];
			verticesY = new int[numVertices];
			verticesZ = new int[numVertices];
			anIntArray1655 = new int[numVertices];
			triangleX = new int[numTriangles];
			triangleY = new int[numTriangles];
			triangleZ = new int[numTriangles];
			triPIndex = new int[textureTriangleCount];
			triMIndex = new int[textureTriangleCount];
			triNIndex = new int[textureTriangleCount];
			if (flag)
				triangleDrawType = new int[numTriangles];
			if (flag1)
				facePriority = new int[numTriangles];
			if (flag2)
				triangleAlpha = new int[numTriangles];
			if (flag3)
				anIntArray1656 = new int[numTriangles];
			triangleColourOrTexture = new int[numTriangles];
			numVertices = 0;
			numTriangles = 0;
			textureTriangleCount = 0;
			int l = 0;
			for (int i1 = 0; i1 < i; i1++) {
				Model model_1 = amodel[i1];
				if (model_1 != null) {
					for (int j1 = 0; j1 < model_1.numTriangles; j1++) {
						if (flag)
						if (model_1.triangleDrawType == null) {
							triangleDrawType[numTriangles] = 0;
						} else {
							int k1 = model_1.triangleDrawType[j1];
							if ((k1 & 2) == 2)
								k1 += l << 2;
							triangleDrawType[numTriangles] = k1;
						}
						if (flag1)
							if (model_1.facePriority == null)
								facePriority[numTriangles] = model_1.anInt1641;
						else
							facePriority[numTriangles] = model_1.facePriority[j1];
						if (flag2)
							if (model_1.triangleAlpha == null)
								triangleAlpha[numTriangles] = 0;
						else
							triangleAlpha[numTriangles] = model_1.triangleAlpha[j1];
						
						if (flag3 && model_1.anIntArray1656 != null)
							anIntArray1656[numTriangles] = model_1.anIntArray1656[j1];
						triangleColourOrTexture[numTriangles] = model_1.triangleColourOrTexture[j1];
						triangleX[numTriangles] = method465(model_1,
						                                      model_1.triangleX[j1]);
						triangleY[numTriangles] = method465(model_1,
						                                      model_1.triangleY[j1]);
						triangleZ[numTriangles] = method465(model_1,
						                                      model_1.triangleZ[j1]);
						numTriangles++;
					}
					
					for (int l1 = 0; l1 < model_1.textureTriangleCount; l1++) {
						triPIndex[textureTriangleCount] = method465(model_1,
						                                      model_1.triPIndex[l1]);
						triMIndex[textureTriangleCount] = method465(model_1,
						                                      model_1.triMIndex[l1]);
						triNIndex[textureTriangleCount] = method465(model_1,
						                                      model_1.triNIndex[l1]);
						textureTriangleCount++;
					}
					
					l += model_1.textureTriangleCount;
				}
			}
			//}catch(System.Exception ex)
			//{
			
			//}
		}
		
		public Model(Model[] amodel) {
			int i = 2;
			aBoolean1618 = true;
			aBoolean1659 = false;
			anInt1620++;
			bool flag1 = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			numVertices = 0;
			numTriangles = 0;
			textureTriangleCount = 0;
			anInt1641 = -1;
			for (int k = 0; k < i; k++) {
				Model model = amodel[k];
				if (model != null) {
					numVertices += model.numVertices;
					numTriangles += model.numTriangles;
					textureTriangleCount += model.textureTriangleCount;
					flag1 |= model.triangleDrawType != null;
					if (model.facePriority != null) {
						flag2 = true;
					} else {
						if (anInt1641 == -1)
							anInt1641 = model.anInt1641;
						if (anInt1641 != model.anInt1641)
							flag2 = true;
					}
					flag3 |= model.triangleAlpha != null;
					flag4 |= model.triangleColourOrTexture != null;
				}
			}
			
			verticesX = new int[numVertices];
			verticesY = new int[numVertices];
			verticesZ = new int[numVertices];
			triangleX = new int[numTriangles];
			triangleY = new int[numTriangles];
			triangleZ = new int[numTriangles];
			triangle_hsl_a = new int[numTriangles];
			triangle_hsl_b = new int[numTriangles];
			triangle_hsl_c = new int[numTriangles];
			triPIndex = new int[textureTriangleCount];
			triMIndex = new int[textureTriangleCount];
			triNIndex = new int[textureTriangleCount];
			if (flag1)
				triangleDrawType = new int[numTriangles];
			if (flag2)
				facePriority = new int[numTriangles];
			if (flag3)
				triangleAlpha = new int[numTriangles];
			if (flag4)
				triangleColourOrTexture = new int[numTriangles];
			numVertices = 0;
			numTriangles = 0;
			textureTriangleCount = 0;
			int i1 = 0;
			for (int j1 = 0; j1 < i; j1++) {
				Model model_1 = amodel[j1];
				if (model_1 != null) {
					int k1 = numVertices;
					for (int l1 = 0; l1 < model_1.numVertices; l1++) {
						verticesX[numVertices] = model_1.verticesX[l1];
						verticesY[numVertices] = model_1.verticesY[l1];
						verticesZ[numVertices] = model_1.verticesZ[l1];
						numVertices++;
					}
					
					for (int i2 = 0; i2 < model_1.numTriangles; i2++) {
						triangleX[numTriangles] = model_1.triangleX[i2] + k1;
						triangleY[numTriangles] = model_1.triangleY[i2] + k1;
						triangleZ[numTriangles] = model_1.triangleZ[i2] + k1;
						triangle_hsl_a[numTriangles] = model_1.triangle_hsl_a[i2];
						triangle_hsl_b[numTriangles] = model_1.triangle_hsl_b[i2];
						triangle_hsl_c[numTriangles] = model_1.triangle_hsl_c[i2];
						if (flag1)
						if (model_1.triangleDrawType == null) {
							triangleDrawType[numTriangles] = 0;
						} else {
							int j2 = model_1.triangleDrawType[i2];
							if ((j2 & 2) == 2)
								j2 += i1 << 2;
							triangleDrawType[numTriangles] = j2;
						}
						if (flag2)
							if (model_1.facePriority == null)
								facePriority[numTriangles] = model_1.anInt1641;
						else
							facePriority[numTriangles] = model_1.facePriority[i2];
						if (flag3)
							if (model_1.triangleAlpha == null)
								triangleAlpha[numTriangles] = 0;
						else
							triangleAlpha[numTriangles] = model_1.triangleAlpha[i2];
						if (flag4 && model_1.triangleColourOrTexture != null)
							triangleColourOrTexture[numTriangles] = model_1.triangleColourOrTexture[i2];
						
						numTriangles++;
					}
					
					for (int k2 = 0; k2 < model_1.textureTriangleCount; k2++) {
						triPIndex[textureTriangleCount] = model_1.triPIndex[k2] + k1;
						triMIndex[textureTriangleCount] = model_1.triMIndex[k2] + k1;
						triNIndex[textureTriangleCount] = model_1.triNIndex[k2] + k1;
						textureTriangleCount++;
					}
					
					i1 += model_1.textureTriangleCount;
				}
			}
			
			method466();
		}
		
		public Model(bool flag, bool flag1, bool flag2, Model model) {
			aBoolean1618 = true;
			aBoolean1659 = false;
			anInt1620++;
			numVertices = model.numVertices;
			numTriangles = model.numTriangles;
			textureTriangleCount = model.textureTriangleCount;
			if (flag2) {
				verticesX = model.verticesX;
				verticesY = model.verticesY;
				verticesZ = model.verticesZ;
			} else {
				verticesX = new int[numVertices];
				verticesY = new int[numVertices];
				verticesZ = new int[numVertices];
				for (int j = 0; j < numVertices; j++) {
					verticesX[j] = model.verticesX[j];
					verticesY[j] = model.verticesY[j];
					verticesZ[j] = model.verticesZ[j];
				}
				
			}
			if (flag) {
				triangleColourOrTexture = model.triangleColourOrTexture;
			} else {
				triangleColourOrTexture = new int[numTriangles];
				for (int k = 0; k < numTriangles; k++)
					triangleColourOrTexture[k] = model.triangleColourOrTexture[k];
				
			}
			if (flag1) {
				triangleAlpha = model.triangleAlpha;
			} else {
				triangleAlpha = new int[numTriangles];
				if (model.triangleAlpha == null) {
					for (int l = 0; l < numTriangles; l++)
						triangleAlpha[l] = 0;
					
				} else {
					for (int i1 = 0; i1 < numTriangles; i1++)
						triangleAlpha[i1] = model.triangleAlpha[i1];
					
				}
			}
			anIntArray1655 = model.anIntArray1655;
			anIntArray1656 = model.anIntArray1656;
			triangleDrawType = model.triangleDrawType;
			triangleX = model.triangleX;
			triangleY = model.triangleY;
			triangleZ = model.triangleZ;
			facePriority = model.facePriority;
			anInt1641 = model.anInt1641;
			triPIndex = model.triPIndex;
			triMIndex = model.triMIndex;
			triNIndex = model.triNIndex;
		}
		
		public Model(bool flag, bool flag1, Model model) {
			aBoolean1618 = true;
			aBoolean1659 = false;
			anInt1620++;
			numVertices = model.numVertices;
			numTriangles = model.numTriangles;
			textureTriangleCount = model.textureTriangleCount;
			if (flag) {
				verticesY = new int[numVertices];
				for (int j = 0; j < numVertices; j++)
					verticesY[j] = model.verticesY[j];
				
			} else {
				verticesY = model.verticesY;
			}
			if (flag1) {
				triangle_hsl_a = new int[numTriangles];
				triangle_hsl_b = new int[numTriangles];
				triangle_hsl_c = new int[numTriangles];
				for (int k = 0; k < numTriangles; k++) {
					triangle_hsl_a[k] = model.triangle_hsl_a[k];
					triangle_hsl_b[k] = model.triangle_hsl_b[k];
					triangle_hsl_c[k] = model.triangle_hsl_c[k];
				}
				
				triangleDrawType = new int[numTriangles];
				if (model.triangleDrawType == null) {
					for (int l = 0; l < numTriangles; l++)
						triangleDrawType[l] = 0;
					
				} else {
					for (int i1 = 0; i1 < numTriangles; i1++)
						triangleDrawType[i1] = model.triangleDrawType[i1];
					
				}
				base.vertexNormalOffset = new VertexNormal[numVertices];
				for (int j1 = 0; j1 < numVertices; j1++) {
					VertexNormal class33 = base.vertexNormalOffset[j1] = new VertexNormal();
					VertexNormal class33_1 = model.vertexNormalOffset[j1];
					class33.x = class33_1.x;
					class33.y = class33_1.y;
					class33.z = class33_1.z;
					class33.magnitude = class33_1.magnitude;
				}
				
				aClass33Array1660 = model.aClass33Array1660;
			} else {
				triangle_hsl_a = model.triangle_hsl_a;
				triangle_hsl_b = model.triangle_hsl_b;
				triangle_hsl_c = model.triangle_hsl_c;
				triangleDrawType = model.triangleDrawType;
			}
			verticesX = model.verticesX;
			verticesZ = model.verticesZ;
			triangleColourOrTexture = model.triangleColourOrTexture;
			triangleAlpha = model.triangleAlpha;
			facePriority = model.facePriority;
			anInt1641 = model.anInt1641;
			triangleX = model.triangleX;
			triangleY = model.triangleY;
			triangleZ = model.triangleZ;
			triPIndex = model.triPIndex;
			triMIndex = model.triMIndex;
			triNIndex = model.triNIndex;
			base.modelHeight = model.modelHeight;
			
			diagonal2DAboveorigin = model.diagonal2DAboveorigin;
			diagonal3DAboveorigin = model.diagonal3DAboveorigin;
			diagonal3D = model.diagonal3D;
			minX = model.minX;
			maxZ = model.maxZ;
			minZ = model.minZ;
			maxX = model.maxX;
		}
		
		public void method464(Model model, bool flag) {
			numVertices = model.numVertices;
			numTriangles = model.numTriangles;
			textureTriangleCount = model.textureTriangleCount;
			if (anIntArray1622.Length < numVertices) {
				anIntArray1622 = new int[numVertices + 10000];
				anIntArray1623 = new int[numVertices + 10000];
				anIntArray1624 = new int[numVertices + 10000];
			}
			verticesX = anIntArray1622;
			verticesY = anIntArray1623;
			verticesZ = anIntArray1624;
			for (int k = 0; k < numVertices; k++) {
				verticesX[k] = model.verticesX[k];
				verticesY[k] = model.verticesY[k];
				verticesZ[k] = model.verticesZ[k];
			}
			
			if (flag) {
				triangleAlpha = model.triangleAlpha;
			} else {
				if (anIntArray1625.Length < numTriangles)
					anIntArray1625 = new int[numTriangles + 100];
				triangleAlpha = anIntArray1625;
				if (model.triangleAlpha == null) {
					for (int l = 0; l < numTriangles; l++)
						triangleAlpha[l] = 0;
					
				} else {
					for (int i1 = 0; i1 < numTriangles; i1++)
						triangleAlpha[i1] = model.triangleAlpha[i1];
					
				}
			}
			triangleDrawType = model.triangleDrawType;
			triangleColourOrTexture = model.triangleColourOrTexture;
			facePriority = model.facePriority;
			anInt1641 = model.anInt1641;
			anIntArrayArray1658 = model.anIntArrayArray1658;
			anIntArrayArray1657 = model.anIntArrayArray1657;
			triangleX = model.triangleX;
			triangleY = model.triangleY;
			triangleZ = model.triangleZ;
			triangle_hsl_a = model.triangle_hsl_a;
			triangle_hsl_b = model.triangle_hsl_b;
			triangle_hsl_c = model.triangle_hsl_c;
			triPIndex = model.triPIndex;
			triMIndex = model.triMIndex;
			triNIndex = model.triNIndex;
		}
		
		private int method465(Model model, int i) {
			try{
			int j = -1;
			int k = model.verticesX[i];
			int l = model.verticesY[i];
			int i1 = model.verticesZ[i];
			for (int j1 = 0; j1 < numVertices; j1++) {
				if (k != verticesX[j1] || l != verticesY[j1]
				    || i1 != verticesZ[j1])
					continue;
				j = j1;
				break;
			}
			
			if (j == -1) {
				verticesX[numVertices] = k;
				verticesY[numVertices] = l;
				verticesZ[numVertices] = i1;
				if (model.anIntArray1655 != null)
					anIntArray1655[numVertices] = model.anIntArray1655[i];
				j = numVertices++;
			}
			return j;
			}
			catch(Exception ex)
			{
				Debug.Log ("method465Error: " + ex.Message);
				return -1;
			}
		}
		
		public void method466() {
			base.modelHeight = 0;
			diagonal2DAboveorigin = 0;
			maxY = 0;
			for (int i = 0; i < numVertices; i++) {
				int j = verticesX[i];
				int k = verticesY[i];
				int l = verticesZ[i];
				if (-k > base.modelHeight)
					base.modelHeight = -k;
				if (k > maxY)
					maxY = k;
				int i1 = j * j + l * l;
				if (i1 > diagonal2DAboveorigin)
					diagonal2DAboveorigin = i1;
			}
			diagonal2DAboveorigin = (int) (Mathf.Sqrt(diagonal2DAboveorigin) + 0.98999999999999999D);
			diagonal3DAboveorigin = (int) (Mathf.Sqrt(diagonal2DAboveorigin * diagonal2DAboveorigin + base.modelHeight
			                             * base.modelHeight) + 0.98999999999999999D);
			diagonal3D = diagonal3DAboveorigin
				+ (int) (Mathf.Sqrt(diagonal2DAboveorigin * diagonal2DAboveorigin + maxY
				                   * maxY) + 0.98999999999999999D);
		}
		
		public void method467() {
			base.modelHeight = 0;
			maxY = 0;
			for (int i = 0; i < numVertices; i++) {
				int j = verticesY[i];
				if (-j > base.modelHeight)
					base.modelHeight = -j;
				if (j > maxY)
					maxY = j;
			}
			
			diagonal3DAboveorigin = (int) (Mathf.Sqrt(diagonal2DAboveorigin * diagonal2DAboveorigin + base.modelHeight
			                             * base.modelHeight) + 0.98999999999999999D);
			diagonal3D = diagonal3DAboveorigin
				+ (int) (Mathf.Sqrt(diagonal2DAboveorigin * diagonal2DAboveorigin + maxY
				                   * maxY) + 0.98999999999999999D);
		}
		
		public void method468(int i) {
			base.modelHeight = 0;
			diagonal2DAboveorigin = 0;
			maxY = 0;
			minX = 0xf423f;
			maxX = unchecked((int)0xfff0bdc1);
			maxZ = unchecked((int)0xfffe7961);
			minZ = 0x1869f;
			for (int j = 0; j < numVertices; j++) {
				int k = verticesX[j];
				int l = verticesY[j];
				int i1 = verticesZ[j];
				if (k < minX)
					minX = k;
				if (k > maxX)
					maxX = k;
				if (i1 < minZ)
					minZ = i1;
				if (i1 > maxZ)
					maxZ = i1;
				if (-l > base.modelHeight)
					base.modelHeight = -l;
				if (l > maxY)
					maxY = l;
				int j1 = k * k + i1 * i1;
				if (j1 > diagonal2DAboveorigin)
					diagonal2DAboveorigin = j1;
			}
			
			diagonal2DAboveorigin = (int) Mathf.Sqrt(diagonal2DAboveorigin);
			diagonal3DAboveorigin = (int) Mathf.Sqrt(diagonal2DAboveorigin * diagonal2DAboveorigin + base.modelHeight
			                            * base.modelHeight);
			if (i != 21073) {
				return;
			} else {
				diagonal3D = diagonal3DAboveorigin
					+ (int) Mathf.Sqrt(diagonal2DAboveorigin * diagonal2DAboveorigin + maxY
					                  * maxY);
				return;
			}
		}
		
		public void method469() { //May be broken :P
			if (anIntArray1655 != null) {
				int[] ai = new int[256];
				int j = 0;
				for (int l = 0; l < numVertices; l++) {
					int j1 = anIntArray1655[l];
					ai[j1]++;
					if (j1 > j)
						j = j1;
				}
				
				if(anIntArrayArray1657 == null || anIntArrayArray1657.Length != j + 1) anIntArrayArray1657 = new int[j + 1][];
				for (int k1 = 0; k1 <= j; k1++) {
					if(anIntArrayArray1657[k1] == null || anIntArrayArray1657[k1].Length != ai[k1]) anIntArrayArray1657[k1] = new int[ai[k1]];
					ai[k1] = 0;
				}
				
				for (int j2 = 0; j2 < numVertices; j2++) {
					int l2 = anIntArray1655[j2];
					anIntArrayArray1657[l2][ai[l2]++] = j2;
				}
				
				anIntArray1655 = null;
			}
			if (anIntArray1656 != null) {
				int[] ai1 = new int[256];
				int k = 0;
				for (int i1 = 0; i1 < numTriangles; i1++) {
					int l1 = anIntArray1656[i1];
					ai1[l1]++;
					if (l1 > k)
						k = l1;
				}
				
				if(anIntArrayArray1658 == null || anIntArrayArray1658.Length != k + 1) anIntArrayArray1658 = new int[k + 1][];
				for (int i2 = 0; i2 <= k; i2++) {
					if(anIntArrayArray1658[i2] == null || anIntArrayArray1658[i2].Length != ai1[i2]) anIntArrayArray1658[i2] = new int[ai1[i2]];
					ai1[i2] = 0;
				}
				
				for (int k2 = 0; k2 < numTriangles; k2++) {
					int i3 = anIntArray1656[k2];
					anIntArrayArray1658[i3][ai1[i3]++] = k2;
				}
				
				anIntArray1656 = null;
			}
		}
		
		public void method470(int firstFrame, int nextFrame, int end, int cycle) {
			
			if (anIntArrayArray1657 != null && firstFrame != -1) {
				Class36 currentAnimation = Class36.method531(firstFrame);
				Class18 list1 = currentAnimation.aClass18_637;
				anInt1681 = 0;
				anInt1682 = 0;
				anInt1683 = 0;
				Class36 nextAnimation = null;
				Class18 list2 = null;
				if (nextFrame != -1) {
					nextAnimation = Class36.method531(nextFrame);
					if (nextAnimation.aClass18_637 != list1)
						nextAnimation = null;
					list2 = nextAnimation.aClass18_637;
				}
				if(nextAnimation == null || list2 == null) {
					for (int i_263_ = 0; i_263_ < currentAnimation.stepCounter; i_263_++) {
						int i_264_ = currentAnimation.opcodeLinkTable[i_263_];
						method472(list1.opcode[i_264_], list1.skinlist[i_264_], currentAnimation.modifier1[i_263_], currentAnimation.modifier2[i_263_], currentAnimation.modifier3[i_263_]);
						
					}
				} else {
					for (int i1 = 0; i1 < currentAnimation.stepCounter; i1++) {
						int n1 = currentAnimation.opcodeLinkTable[i1];
						int opcode = list1.opcode[n1];
						int[] skin = list1.skinlist[n1];
						int x = currentAnimation.modifier1[i1];
						int y = currentAnimation.modifier2[i1];
						int z = currentAnimation.modifier3[i1];
						bool found = false;
						for (int i2 = 0; i2 < nextAnimation.stepCounter; i2++) {
							int n2 = nextAnimation.opcodeLinkTable[i2];
							if (list2.skinlist[n2].Equals(skin)) {
								if (opcode != 2) {
									x += (nextAnimation.modifier1[i2] - x) * cycle / end;
									y += (nextAnimation.modifier2[i2] - y) * cycle / end;
									z += (nextAnimation.modifier3[i2] - z) * cycle / end;
								} else {
									x &= 0xff;
									y &= 0xff;
									z &= 0xff;
									int dx = nextAnimation.modifier1[i2] - x & 0xff;
									int dy = nextAnimation.modifier2[i2] - y & 0xff;
									int dz = nextAnimation.modifier3[i2] - z & 0xff;
									if (dx >= 128) {
										dx -= 256;
									}
									if (dy >= 128) {
										dy -= 256;
									}
									if (dz >= 128) {
										dz -= 256;
									}
									x = x + dx * cycle / end & 0xff;
									y = y + dy * cycle / end & 0xff;
									z = z + dz * cycle / end & 0xff;
								}
								found = true;
								break;
							}
						}
						if (!found) {
							if (opcode != 3 && opcode != 2) {
								x = x * (end - cycle) / end;
								y = y * (end - cycle) / end;
								z = z * (end - cycle) / end;
							} else if (opcode == 3) {
								x = (x * (end - cycle) + (cycle << 7)) / end;
								y = (y * (end - cycle) + (cycle << 7)) / end;
								z = (z * (end - cycle) + (cycle << 7)) / end;
							} else {
								x &= 0xff;
								y &= 0xff;
								z &= 0xff;
								int dx = -x & 0xff;
								int dy = -y & 0xff;
								int dz = -z & 0xff;
								if (dx >= 128) {
									dx -= 256;
								}
								if (dy >= 128) {
									dy -= 256;
								}
								if (dz >= 128) {
									dz -= 256;
								}
								x = x + dx * cycle / end & 0xff;
								y = y + dy * cycle / end & 0xff;
								z = z + dz * cycle / end & 0xff;
							}
						}
						method472(opcode, skin, x, y, z);
					}
				}
			}
		}
		
		public void method470(int i) {
			if (anIntArrayArray1657 == null)
				return;
			if (i == -1)
				return;
			Class36 class36 = Class36.method531(i);
			if (class36 == null)
				return;
			Class18 class18 = class36.aClass18_637;
			anInt1681 = 0;
			anInt1682 = 0;
			anInt1683 = 0;
			for (int k = 0; k < class36.stepCounter; k++) {
				int l = class36.opcodeLinkTable[k];
				method472(class18.opcode[l], class18.skinlist[l],
				          class36.modifier1[k], class36.modifier2[k],
				          class36.modifier3[k]);
			}
			
		}
		
		public void method471(int[] ai, int j, int k) {
			if (k == -1)
				return;
			if (ai == null || j == -1) {
				method470(k);
				return;
			}
			Class36 class36 = Class36.method531(k);
			if (class36 == null)
				return;
			Class36 class36_1 = Class36.method531(j);
			if (class36_1 == null) {
				method470(k);
				return;
			}
			Class18 class18 = class36.aClass18_637;
			anInt1681 = 0;
			anInt1682 = 0;
			anInt1683 = 0;
			int l = 0;
			int i1 = ai[l++];
			for (int j1 = 0; j1 < class36.stepCounter; j1++) {
				int k1;
				for (k1 = class36.opcodeLinkTable[j1]; k1 > i1; i1 = ai[l++])
					;
				if (k1 != i1 || class18.opcode[k1] == 0)
					method472(class18.opcode[k1],
					          class18.skinlist[k1],
					          class36.modifier1[j1], class36.modifier2[j1],
					          class36.modifier3[j1]);
			}
			
			anInt1681 = 0;
			anInt1682 = 0;
			anInt1683 = 0;
			l = 0;
			i1 = ai[l++];
			for (int l1 = 0; l1 < class36_1.stepCounter; l1++) {
				int i2;
				for (i2 = class36_1.opcodeLinkTable[l1]; i2 > i1; i1 = ai[l++])
					;
				if (i2 == i1 || class18.opcode[i2] == 0)
					method472(class18.opcode[i2],
					          class18.skinlist[i2],
					          class36_1.modifier1[l1],
					          class36_1.modifier2[l1],
					          class36_1.modifier3[l1]);
			}
			
		}
		
		private void method472(int i, int[] ai, int j, int k, int l) {
			
			int i1 = ai.Length;
			if (i == 0) {
				int j1 = 0;
				anInt1681 = 0;
				anInt1682 = 0;
				anInt1683 = 0;
				for (int k2 = 0; k2 < i1; k2++) {
					int l3 = ai[k2];
					if (l3 < anIntArrayArray1657.Length) {
						int[] ai5 = anIntArrayArray1657[l3];
						for (int i5 = 0; i5 < ai5.Length; i5++) {
							int j6 = ai5[i5];
							anInt1681 += verticesX[j6];
							anInt1682 += verticesY[j6];
							anInt1683 += verticesZ[j6];
							j1++;
						}
						
					}
				}
				
				if (j1 > 0) {
					anInt1681 = anInt1681 / j1 + j;
					anInt1682 = anInt1682 / j1 + k;
					anInt1683 = anInt1683 / j1 + l;
					return;
				} else {
					anInt1681 = j;
					anInt1682 = k;
					anInt1683 = l;
					return;
				}
			}
			if (i == 1) {
				for (int k1 = 0; k1 < i1; k1++) {
					int l2 = ai[k1];
					if (l2 < anIntArrayArray1657.Length) {
						int[] ai1 = anIntArrayArray1657[l2];
						for (int i4 = 0; i4 < ai1.Length; i4++) {
							int j5 = ai1[i4];
							verticesX[j5] += j;
							verticesY[j5] += k;
							verticesZ[j5] += l;
						}
						
					}
				}
				
				return;
			}
			if (i == 2) {
				for (int l1 = 0; l1 < i1; l1++) {
					int i3 = ai[l1];
					if (i3 < anIntArrayArray1657.Length) {
						int[] ai2 = anIntArrayArray1657[i3];
						for (int j4 = 0; j4 < ai2.Length; j4++) {
							int k5 = ai2[j4];
							verticesX[k5] -= anInt1681;
							verticesY[k5] -= anInt1682;
							verticesZ[k5] -= anInt1683;
							int k6 = (j & 0xff) * 8;
							int l6 = (k & 0xff) * 8;
							int i7 = (l & 0xff) * 8;
							if (i7 != 0) {
								int j7 = SINE[i7];
								int i8 = COSINE[i7];
								int l8 = verticesY[k5] * j7 + verticesX[k5] * i8 >> 16;
								verticesY[k5] = verticesY[k5] * i8 - verticesX[k5] * j7 >> 16;
								verticesX[k5] = l8;
							}
							if (k6 != 0) {
								int k7 = SINE[k6];
								int j8 = COSINE[k6];
								int i9 = verticesY[k5] * j8 - verticesZ[k5] * k7 >> 16;
								verticesZ[k5] = verticesY[k5] * k7 + verticesZ[k5] * j8 >> 16;
								verticesY[k5] = i9;
							}
							if (l6 != 0) {
								int l7 = SINE[l6];
								int k8 = COSINE[l6];
								int j9 = verticesZ[k5] * l7 + verticesX[k5] * k8 >> 16;
								verticesZ[k5] = verticesZ[k5] * k8 - verticesX[k5] * l7 >> 16;
								verticesX[k5] = j9;
							}
							verticesX[k5] += anInt1681;
							verticesY[k5] += anInt1682;
							verticesZ[k5] += anInt1683;
						}
						
					}
				}
				return;
			}
			if (i == 3) {
				for (int i2 = 0; i2 < i1; i2++) {
					int j3 = ai[i2];
					if (j3 < anIntArrayArray1657.Length) {
						int[] ai3 = anIntArrayArray1657[j3];
						for (int k4 = 0; k4 < ai3.Length; k4++) {
							int l5 = ai3[k4];
							verticesX[l5] -= anInt1681;
							verticesY[l5] -= anInt1682;
							verticesZ[l5] -= anInt1683;
							verticesX[l5] = (verticesX[l5] * j) / 128;
							verticesY[l5] = (verticesY[l5] * k) / 128;
							verticesZ[l5] = (verticesZ[l5] * l) / 128;
							verticesX[l5] += anInt1681;
							verticesY[l5] += anInt1682;
							verticesZ[l5] += anInt1683;
						}
					}
				}
				return;
			}
			if (i == 5 && anIntArrayArray1658 != null && triangleAlpha != null) {
				for (int j2 = 0; j2 < i1; j2++) {
					int k3 = ai[j2];
					if (k3 < anIntArrayArray1658.Length) {
						int[] ai4 = anIntArrayArray1658[k3];
						for (int l4 = 0; l4 < ai4.Length; l4++) {
							int i6 = ai4[l4];
							triangleAlpha[i6] += j * 8;
							if (triangleAlpha[i6] < 0)
								triangleAlpha[i6] = 0;
							if (triangleAlpha[i6] > 255)
								triangleAlpha[i6] = 255;
						}
					}
				}
			}
		}
		
		public void method473() {
			for (int j = 0; j < numVertices; j++) {
				int k = verticesX[j];
				verticesX[j] = verticesZ[j];
				verticesZ[j] = -k;
			}
		}
		
		public void method474(int i) {
			int k = SINE[i];
			int l = COSINE[i];
			for (int i1 = 0; i1 < numVertices; i1++) {
				int j1 = verticesY[i1] * l - verticesZ[i1] * k >> 16;
				verticesZ[i1] = verticesY[i1] * k + verticesZ[i1] * l >> 16;
				verticesY[i1] = j1;
			}
		}
		
		public void method475(int i, int j, int l) {
			for (int i1 = 0; i1 < numVertices; i1++) {
				verticesX[i1] += i;
				verticesY[i1] += j;
				verticesZ[i1] += l;
			}
		}
		
		public void method476(int i, int j) {
			for (int k = 0; k < numTriangles; k++)
				if (triangleColourOrTexture[k] == i)
					triangleColourOrTexture[k] = j;
		}
		
		public void method477() {
			for (int j = 0; j < numVertices; j++)
				verticesZ[j] = -verticesZ[j];
			for (int k = 0; k < numTriangles; k++) {
				int l = triangleX[k];
				triangleX[k] = triangleZ[k];
				triangleZ[k] = l;
			}
		}
		
		public void method478(int i, int j, int l) {
			for (int i1 = 0; i1 < numVertices; i1++) {
				verticesX[i1] = (verticesX[i1] * i) / 128;
				verticesY[i1] = (verticesY[i1] * l) / 128;
				verticesZ[i1] = (verticesZ[i1] * j) / 128;
			}
			
		}
		
		public void method479(int i, int j, int k, int l, int i1, bool flag) { //May be broken :P
			int j1 = (int) Mathf.Sqrt(k * k + l * l + i1 * i1);
			int k1 = j * j1 >> 8;
			if (triangle_hsl_a == null) {
				triangle_hsl_a = new int[numTriangles];
				triangle_hsl_b = new int[numTriangles];
				triangle_hsl_c = new int[numTriangles];
			}
			if (base.vertexNormalOffset == null) {
				base.vertexNormalOffset = new VertexNormal[numVertices];
				for (int l1 = 0; l1 < numVertices; l1++)
					base.vertexNormalOffset[l1] = new VertexNormal();
				
			}
			try{
			for (int i2 = 0; i2 < numTriangles; i2++) {
				if (triangleColourOrTexture != null && triangleAlpha != null)
					if (triangleColourOrTexture[i2] == 65535 || triangleColourOrTexture[i2] == 16705)// || triangleColourOrTexture[i2] == 0)
						triangleAlpha[i2] = 255;
				int j2 = triangleX[i2];
				int l2 = triangleY[i2];
				int i3 = triangleZ[i2];
				int j3 = verticesX[l2] - verticesX[j2];
				int k3 = verticesY[l2] - verticesY[j2];
				int l3 = verticesZ[l2] - verticesZ[j2];
				int i4 = verticesX[i3] - verticesX[j2];
				int j4 = verticesY[i3] - verticesY[j2];
				int k4 = verticesZ[i3] - verticesZ[j2];
				int l4 = k3 * k4 - j4 * l3;
				int i5 = l3 * i4 - k4 * j3;
				int j5;
				for (j5 = j3 * j4 - i4 * k3; l4 > 8192 || i5 > 8192 || j5 > 8192
				     || l4 < -8192 || i5 < -8192 || j5 < -8192; j5 >>= 1) {
					l4 >>= 1;
					i5 >>= 1;
				}
				
				int k5 = (int) Mathf.Sqrt(l4 * l4 + i5 * i5 + j5 * j5);
				if (k5 <= 0)
					k5 = 1;
				l4 = (l4 * 256) / k5;
				i5 = (i5 * 256) / k5;
				j5 = (j5 * 256) / k5;
				
				if (triangleDrawType == null || (triangleDrawType[i2] & 1) == 0) {
					
					VertexNormal class33_2 = base.vertexNormalOffset[j2];
					class33_2.x += l4;
					class33_2.y += i5;
					class33_2.z += j5;
					class33_2.magnitude++;
					class33_2 = base.vertexNormalOffset[l2];
					class33_2.x += l4;
					class33_2.y += i5;
					class33_2.z += j5;
					class33_2.magnitude++;
					class33_2 = base.vertexNormalOffset[i3];
					class33_2.x += l4;
					class33_2.y += i5;
					class33_2.z += j5;
					class33_2.magnitude++;
					
				} else {
					
					int l5 = i + (k * l4 + l * i5 + i1 * j5) / (k1 + k1 / 2);
					triangle_hsl_a[i2] = method481(triangleColourOrTexture[i2], l5,
					                               triangleDrawType[i2]);
				}
			}
			}
			catch(Exception ex)
			{
				Debug.Log ("Method 479 error");
			}
			if (flag) {
				method480(i, k1, k, l, i1);
			} else {
				if(aClass33Array1660 == null || aClass33Array1660.Length < numVertices) aClass33Array1660 = new VertexNormal[numVertices];
				for (int k2 = 0; k2 < numVertices; k2++) {
					VertexNormal class33 = base.vertexNormalOffset[k2];
					VertexNormal class33_1 = aClass33Array1660[k2] = new VertexNormal();
					class33_1.x = class33.x;
					class33_1.y = class33.y;
					class33_1.z = class33.z;
					class33_1.magnitude = class33.magnitude;
				}
				
			}
			if (flag) {
				method466();
				return;
			} else {
				method468(21073);
				return;
			}
		}
		
		public static String ccString = "Cla";
		public static String xxString = "at Cl";
		public static String vvString = "nt";
		public static String aString9_9 = "" + ccString + "n Ch" + xxString + "ie"
			+ vvString + " ";
		
		public void method480(int i, int j, int k, int l, int i1) {
		try{
			for (int j1 = 0; j1 < numTriangles; j1++) {
				int k1 = triangleX[j1];
				int i2 = triangleY[j1];
				int j2 = triangleZ[j1];
				if (triangleDrawType == null) {
					int i3 = triangleColourOrTexture[j1];
					VertexNormal class33 = base.vertexNormalOffset[k1];
					int k2 = i
						+ (k * class33.x + l * class33.y + i1
						   * class33.z) / (j * class33.magnitude);
					triangle_hsl_a[j1] = method481(i3, k2, 0);
					class33 = base.vertexNormalOffset[i2];
					k2 = i
						+ (k * class33.x + l * class33.y + i1
						   * class33.z) / (j * class33.magnitude);
					triangle_hsl_b[j1] = method481(i3, k2, 0);
					class33 = base.vertexNormalOffset[j2];
					k2 = i
						+ (k * class33.x + l * class33.y + i1
						   * class33.z) / (j * class33.magnitude);
					triangle_hsl_c[j1] = method481(i3, k2, 0);
				} else if ((triangleDrawType[j1] & 1) == 0) {
					int j3 = triangleColourOrTexture[j1];
					int k3 = triangleDrawType[j1];
					VertexNormal class33_1 = base.vertexNormalOffset[k1];
					int l2 = i + (k * class33_1.x + l * class33_1.y + i1 * class33_1.z) / (j * class33_1.magnitude);
					triangle_hsl_a[j1] = method481(j3, l2, k3);
					class33_1 = base.vertexNormalOffset[i2];
					l2 = i
						+ (k * class33_1.x + l * class33_1.y + i1
						   * class33_1.z)
							/ (j * class33_1.magnitude);
					triangle_hsl_b[j1] = method481(j3, l2, k3);
					class33_1 = base.vertexNormalOffset[j2];
					l2 = i
						+ (k * class33_1.x + l * class33_1.y + i1
						   * class33_1.z)
							/ (j * class33_1.magnitude);
					triangle_hsl_c[j1] = method481(j3, l2, k3);
				}
			}
			
			base.vertexNormalOffset = null;
			aClass33Array1660 = null;
			anIntArray1655 = null;
			anIntArray1656 = null;
			if (triangleDrawType != null) {
				for (int l1 = 0; l1 < numTriangles; l1++)
					if ((triangleDrawType[l1] & 2) == 2)
						return;
				
			}
			triangleColourOrTexture = null;
			}
			catch(Exception ex)
			{
			Debug.Log("Method480 error");
			}
		}
		
		public static int method481(int i, int j, int k) {
			if (i == 65535)
				return 0;
			if ((k & 2) == 2) {
				if (j < 0)
					j = 0;
				else if (j > 127)
					j = 127;
				j = 127 - j;
				return j;
			}
			
			j = j * (i & 0x7f) >> 7;
			if (j < 2)
				j = 2;
			else if (j > 126)
				j = 126;
			return (i & 0xff80) + j;
		}
		
		public void method482(int j, int k, int l, int i1, int j1, int k1) {
			int i = 0;
			int l1 = Texture.center_x;
			int i2 = Texture.center_y;
			int j2 = SINE[i];
			int k2 = COSINE[i];
			int l2 = SINE[j];
			int i3 = COSINE[j];
			int j3 = SINE[k];
			int k3 = COSINE[k];
			int l3 = SINE[l];
			int i4 = COSINE[l];
			int j4 = j1 * l3 + k1 * i4 >> 16;
			for (int k4 = 0; k4 < numVertices; k4++) {
				int l4 = verticesX[k4];
				int i5 = verticesY[k4];
				int j5 = verticesZ[k4];
				if (k != 0) {
					int k5 = i5 * j3 + l4 * k3 >> 16;
					i5 = i5 * k3 - l4 * j3 >> 16;
					l4 = k5;
				}
				if (i != 0) {
					int l5 = i5 * k2 - j5 * j2 >> 16;
					j5 = i5 * j2 + j5 * k2 >> 16;
					i5 = l5;
				}
				if (j != 0) {
					int i6 = j5 * l2 + l4 * i3 >> 16;
					j5 = j5 * i3 - l4 * l2 >> 16;
					l4 = i6;
				}
				l4 += i1;
				i5 += j1;
				j5 += k1;
				int j6 = i5 * i4 - j5 * l3 >> 16;
				j5 = i5 * l3 + j5 * i4 >> 16;
				i5 = j6;
				anIntArray1667[k4] = j5 - j4;
				vertexPerspectiveZAbs[k4] = 0;
				anIntArray1665[k4] = l1 + (l4 << 9) / j5;
				anIntArray1666[k4] = i2 + (i5 << 9) / j5;
				if (textureTriangleCount > 0) {
					anIntArray1668[k4] = l4;
					anIntArray1669[k4] = i5;
					anIntArray1670[k4] = j5;
				}
			}
			
			try {
				method483(false, false, 0);
				return;
			} catch (Exception _ex) {
				return;
			}
		}
		
		public override void renderAtPoint(int i, int j, int k, int l, int i1, int xOrig, int xOffset,
		                                   int yOrig, int yOffset, int zOrig, int zOffset, int i2, RuneObject runeObj) {
			Debug.Log ("renderAtPoint2");
		    runeObj.UpdateObjConf(i2);
			int x = xOrig + xOffset;
			int y = yOrig + yOffset;
			int z = zOrig + zOffset;
		
			int j2 = z * i1 - x * l >> 16;
								int k2 = y * j + j2 * k >> 16;
								int l2 = diagonal2DAboveorigin * k >> 16;
								int i3 = k2 + l2;
								if(i3 == 0) i3 = 51;
//								if (i3 <= 50 || k2 >= 3500)
//										{runeObj.Hide(); return; }
								int j3 = z * l + x * i1 >> 16;
								int k3 = j3 - diagonal2DAboveorigin << 9;
//								if (k3 / i3 >= (DrawingArea.centerY + UnityClient.renderBufferDistance))
//			{runeObj.Hide(); return; }
								int l3 = j3 + diagonal2DAboveorigin << 9;
//			if (l3 / i3 <= -(DrawingArea.centerY  + UnityClient.renderBufferDistance))
//			{runeObj.Hide(); return; }
								int i4 = y * k - j2 * j >> 16;
								int j4 = diagonal2DAboveorigin * j >> 16;
								int k4 = i4 + j4 << 9;
//			if (k4 / i3 <= -(DrawingArea.anInt1387 + UnityClient.renderBufferDistance))
//			{runeObj.Hide(); return; }
								int l4 = j4 + (base.modelHeight * k >> 16);
								int i5 = i4 - l4 << 9;
//			if (i5 / i3 >= (DrawingArea.anInt1387 + UnityClient.renderBufferDistance))
//			{runeObj.Hide(); return; }
								int j5 = l2 + (base.modelHeight * j >> 16);
								bool flag = false;
								if (k2 - j5 <= 50)
										flag = true;
								bool flag1 = false;
								if (i2 > 0 && aBoolean1684) {
										int k5 = k2 - l2;
										if (k5 <= 50)
												k5 = 50;
										if (j3 > 0) {
												k3 /= i3;
												l3 /= k5;
										} else {
												l3 /= i3;
												k3 /= k5;
										}
										if (i4 > 0) {
												i5 /= i3;
												k4 /= k5;
										} else {
												k4 /= i3;
												i5 /= k5;
										}
										int i6 = anInt1685 - Texture.center_x;
										int k6 = anInt1686 - Texture.center_y;
										//if (i6 > k3 && i6 < l3 && k6 > i5 && k6 < k4)
										//if (aBoolean1659)
										//		MouseOnObjects [anInt1687++] = i2;
										//else
										//		flag1 = true;
								}
								int l5 = Texture.center_x;
								int j6 = Texture.center_y;
								int l6 = 0;
								int i7 = 0;
								if (i != 0) {
										l6 = SINE [i];
										i7 = COSINE [i];
								}
								for (int j7 = 0; j7 < numVertices; j7++) {
										int k7 = verticesX [j7];
										int l7 = verticesY [j7];
										int i8 = verticesZ [j7];
										if (i != 0) {
												int j8 = i8 * l6 + k7 * i7 >> 16;
												i8 = i8 * i7 - k7 * l6 >> 16;
												k7 = j8;
										}
										k7 += x;
										l7 += y;
										i8 += z;
										int k8 = i8 * l + k7 * i1 >> 16;
										i8 = i8 * i1 - k7 * l >> 16;
										k7 = k8;
										k8 = l7 * k - i8 * j >> 16;
										i8 = l7 * j + i8 * k >> 16;
										l7 = k8;
										anIntArray1667 [j7] = i8 - k2;
										vertexPerspectiveZAbs [j7] = i8;
										if (i8 >= 50) {
												anIntArray1665 [j7] = l5 + (k7 << 9) / i8;
												anIntArray1666 [j7] = j6 + (l7 << 9) / i8;
										} else {
												anIntArray1665 [j7] = -5000;
												flag = true;
										}
										if (flag || textureTriangleCount > 0) {
												anIntArray1668 [j7] = k7;
												anIntArray1669 [j7] = l7;
												anIntArray1670 [j7] = i8;
										}
								}
			
								// if(Render317) 
			//if(client.draw317) method483 (flag, flag1, i2);
			//if (!Render317) {
				if(runeObj != null)
				{
					if(runeObj.player != null) runeObj.UpdatePos(xOrig,yOrig,zOrig);
					runeObj.Render(this);
				}
			//}
		}
		
		//orientation, j, k, l, i1, x, height, y, key, regionContainer, collidable, region, this
		public void renderAtPoint (int i, int j, int k, int l, int i1, int x, int y,
		                           int z, int i2, RuneObject runeObj)
		{
			//if(myObjectId != 38739) return;
			int xOrig = x;
			int yOrig = y;
			int zOrig = z;
			runeObj.UpdateObjConf(i2);
			int j2 = z * i1 - x * l >> 16;
			int k2 = y * j + j2 * k >> 16;
			int l2 = diagonal2DAboveorigin * k >> 16;
			int i3 = k2 + l2;
			int j3 = z * l + x * i1 >> 16;
			int k3 = j3 - diagonal2DAboveorigin << 9;
			int l3 = j3 + diagonal2DAboveorigin << 9;
			int i4 = y * k - j2 * j >> 16;
			int j4 = diagonal2DAboveorigin * j >> 16;
			int k4 = i4 + j4 << 9;
			int l4 = j4 + (base.modelHeight * k >> 16);
			int i5 = i4 - l4 << 9;
			int j5 = l2 + (base.modelHeight * j >> 16);
			bool flag = false;
			if (k2 - j5 <= 50)
				flag = true;
			bool flag1 = false;
			if (i2 > 0 && aBoolean1684 && i3 > 0) {
				int k5 = k2 - l2;
				if (k5 <= 50)
					k5 = 50;
				if (j3 > 0) {
					k3 /= i3;
					l3 /= k5;
				} else {
					l3 /= i3;
					k3 /= k5;
				}
				if (i4 > 0) {
					i5 /= i3;
					k4 /= k5;
				} else {
					k4 /= i3;
					i5 /= k5;
				}
				int i6 = anInt1685 - Texture.center_x;
				int k6 = anInt1686 - Texture.center_y;
				if (i6 > k3 && i6 < l3 && k6 > i5 && k6 < k4)
					if (aBoolean1659)
						MouseOnObjects [anInt1687++] = i2;
				else
					flag1 = true;
			} else if (i3 == 0) {
			}
			
			int l5 = Texture.center_x;
			int j6 = Texture.center_y;
			int l6 = 0;
			int i7 = 0;
			if (i != 0) {
				l6 = SINE [i];
				i7 = COSINE [i];
			}
			for (int j7 = 0; j7 < numVertices; j7++) {
				int k7 = verticesX [j7];
				int l7 = verticesY [j7];
				int i8 = verticesZ [j7];
				if (i != 0) {
					int j8 = i8 * l6 + k7 * i7 >> 16;
					i8 = i8 * i7 - k7 * l6 >> 16;
					k7 = j8;
				}
				k7 += x;
				l7 += y;
				i8 += z;
				int k8 = i8 * l + k7 * i1 >> 16;
				i8 = i8 * i1 - k7 * l >> 16;
				k7 = k8;
				k8 = l7 * k - i8 * j >> 16;
				i8 = l7 * j + i8 * k >> 16;
				l7 = k8;
				anIntArray1667 [j7] = i8 - k2;
				if (i8 >= 50) {
					anIntArray1665 [j7] = l5 + (k7 << 9) / i8;
					anIntArray1666 [j7] = j6 + (l7 << 9) / i8;
				} else {
					anIntArray1665 [j7] = -5000;
					flag = true;
				}
				if (flag || textureTriangleCount > 0) {
					anIntArray1668 [j7] = k7;
					anIntArray1669 [j7] = l7;
					anIntArray1670 [j7] = i8;
				}
			}
//			try {
//				method483 (flag, flag1, i2, 0);
//			} catch {
//			}
			//renderObject (0, 0, 0, 0, x, y, z, regionContainer, collidable, region, animable, i, j, k, l, i1, i2, y, preMadeObj);
			if(runeObj != null)
			{
				if(runeObj.player != null) runeObj.UpdatePos(xOrig,yOrig,zOrig);
				runeObj.Render(this);
			}
		}
		
		
		private void method483(bool flag, bool flag1, int i) {
			for (int j = 0; j < diagonal3D; j++)
				anIntArray1671[j] = 0;
			
			for (int k = 0; k < numTriangles; k++)
			if (triangleDrawType == null || triangleDrawType[k] != -1) {
				int l = triangleX[k];
				int k1 = triangleY[k];
				int j2 = triangleZ[k];
				int i3 = anIntArray1665[l];
				int l3 = anIntArray1665[k1];
				int k4 = anIntArray1665[j2];
				if (flag && (i3 == -5000 || l3 == -5000 || k4 == -5000)) {
					aBooleanArray1664[k] = true;
					int j5 = (anIntArray1667[l] + anIntArray1667[k1] + anIntArray1667[j2])
						/ 3 + diagonal3DAboveorigin;
					anIntArrayArray1672[j5][anIntArray1671[j5]++] = k;
				} else {
					if (flag1
					    && method486(anInt1685, anInt1686,
					             anIntArray1666[l], anIntArray1666[k1],
					             anIntArray1666[j2], i3, l3, k4)) {
						//MouseOnObjects[anInt1687++] = i;
						flag1 = false;
					}
					if ((i3 - l3) * (anIntArray1666[j2] - anIntArray1666[k1])
					    - (anIntArray1666[l] - anIntArray1666[k1])
					    * (k4 - l3) > 0) {
						aBooleanArray1664[k] = false;
						if (i3 < 0 || l3 < 0 || k4 < 0
						    || i3 > DrawingArea.centerX
						    || l3 > DrawingArea.centerX
						    || k4 > DrawingArea.centerX)
							aBooleanArray1663[k] = true;
						else
							aBooleanArray1663[k] = false;
						int k5 = (anIntArray1667[l] + anIntArray1667[k1] + anIntArray1667[j2])
							/ 3 + diagonal3DAboveorigin;
						anIntArrayArray1672[k5][anIntArray1671[k5]++] = k;
					}
				}
			}
			
			if (facePriority == null) {
				for (int i1 = diagonal3D - 1; i1 >= 0; i1--) {
					int l1 = anIntArray1671[i1];
					if (l1 > 0) {
						int[] ai = anIntArrayArray1672[i1];
						for (int j3 = 0; j3 < l1; j3++)
							method484(ai[j3]);
						
					}
				}
				
				return;
			}
			for (int j1 = 0; j1 < 12; j1++) {
				anIntArray1673[j1] = 0;
				anIntArray1677[j1] = 0;
			}
			
			for (int i2 = diagonal3D - 1; i2 >= 0; i2--) {
				int k2 = anIntArray1671[i2];
				if (k2 > 0) {
					int[] ai1 = anIntArrayArray1672[i2];
					for (int i4 = 0; i4 < k2; i4++) {
						int l4 = ai1[i4];
						int l5 = facePriority[l4];
						int j6 = anIntArray1673[l5]++;
						anIntArrayArray1674[l5][j6] = l4;
						if (l5 < 10)
							anIntArray1677[l5] += i2;
						else if (l5 == 10)
							anIntArray1675[j6] = i2;
						else
							anIntArray1676[j6] = i2;
					}
					
				}
			}
			
			int l2 = 0;
			if (anIntArray1673[1] > 0 || anIntArray1673[2] > 0)
				l2 = (anIntArray1677[1] + anIntArray1677[2])
					/ (anIntArray1673[1] + anIntArray1673[2]);
			int k3 = 0;
			if (anIntArray1673[3] > 0 || anIntArray1673[4] > 0)
				k3 = (anIntArray1677[3] + anIntArray1677[4])
					/ (anIntArray1673[3] + anIntArray1673[4]);
			int j4 = 0;
			if (anIntArray1673[6] > 0 || anIntArray1673[8] > 0)
				j4 = (anIntArray1677[6] + anIntArray1677[8])
					/ (anIntArray1673[6] + anIntArray1673[8]);
			int i6 = 0;
			int k6 = anIntArray1673[10];
			int[] ai2 = anIntArrayArray1674[10];
			int[] ai3 = anIntArray1675;
			if (i6 == k6) {
				i6 = 0;
				k6 = anIntArray1673[11];
				ai2 = anIntArrayArray1674[11];
				ai3 = anIntArray1676;
			}
			int i5;
			if (i6 < k6)
				i5 = ai3[i6];
			else
				i5 = -1000;
			for (int l6 = 0; l6 < 10; l6++) {
				while (l6 == 0 && i5 > l2) {
					method484(ai2[i6++]);
					if (i6 == k6 && ai2 != anIntArrayArray1674[11]) {
						i6 = 0;
						k6 = anIntArray1673[11];
						ai2 = anIntArrayArray1674[11];
						ai3 = anIntArray1676;
					}
					if (i6 < k6)
						i5 = ai3[i6];
					else
						i5 = -1000;
				}
				while (l6 == 3 && i5 > k3) {
					method484(ai2[i6++]);
					if (i6 == k6 && ai2 != anIntArrayArray1674[11]) {
						i6 = 0;
						k6 = anIntArray1673[11];
						ai2 = anIntArrayArray1674[11];
						ai3 = anIntArray1676;
					}
					if (i6 < k6)
						i5 = ai3[i6];
					else
						i5 = -1000;
				}
				while (l6 == 5 && i5 > j4) {
					method484(ai2[i6++]);
					if (i6 == k6 && ai2 != anIntArrayArray1674[11]) {
						i6 = 0;
						k6 = anIntArray1673[11];
						ai2 = anIntArrayArray1674[11];
						ai3 = anIntArray1676;
					}
					if (i6 < k6)
						i5 = ai3[i6];
					else
						i5 = -1000;
				}
				int i7 = anIntArray1673[l6];
				int[] ai4 = anIntArrayArray1674[l6];
				for (int j7 = 0; j7 < i7; j7++)
					method484(ai4[j7]);
				
			}
			
			while (i5 != -1000) {
				method484(ai2[i6++]);
				if (i6 == k6 && ai2 != anIntArrayArray1674[11]) {
					i6 = 0;
					ai2 = anIntArrayArray1674[11];
					k6 = anIntArray1673[11];
					ai3 = anIntArray1676;
				}
				if (i6 < k6)
					i5 = ai3[i6];
				else
					i5 = -1000;
			}
		}
		
		private void method484(int i) {
			if (aBooleanArray1664[i]) {
				method485(i);
				return;
			}
			int j = triangleX[i];
			int k = triangleY[i];
			int l = triangleZ[i];
			Texture.aBoolean1462 = aBooleanArray1663[i];
			if (triangleAlpha == null)
				Texture.anInt1465 = 0;
			else
				Texture.anInt1465 = triangleAlpha[i];
			int i1;
			if (triangleDrawType == null)
				i1 = 0;
			else
				i1 = triangleDrawType[i] & 3;
			if (i1 == 0) {
				Texture.drawShadedTriangle(anIntArray1666[j], anIntArray1666[k],
				                           anIntArray1666[l], anIntArray1665[j], anIntArray1665[k],
				                           anIntArray1665[l], triangle_hsl_a[i], triangle_hsl_b[i],
				                           triangle_hsl_c[i], vertexPerspectiveZAbs[j], vertexPerspectiveZAbs[k], vertexPerspectiveZAbs[l]);
				return;
			}
			if (i1 == 1) {
				Texture.drawFlatTriangle(anIntArray1666[j], anIntArray1666[k],
				                         anIntArray1666[l], anIntArray1665[j], anIntArray1665[k],
				                         anIntArray1665[l], modelIntArray3[triangle_hsl_a[i]], vertexPerspectiveZAbs[j], vertexPerspectiveZAbs[k], vertexPerspectiveZAbs[l]);;
				return;
			}
			if (i1 == 2) {
				int j1 = triangleDrawType[i] >> 2;
				int l1 = triPIndex[j1];
				int j2 = triMIndex[j1];
				int l2 = triNIndex[j1];
				Texture.drawTexturedTriangle(anIntArray1666[j], anIntArray1666[k],
				                             anIntArray1666[l], anIntArray1665[j], anIntArray1665[k],
				                             anIntArray1665[l], triangle_hsl_a[i], triangle_hsl_b[i],
				                             triangle_hsl_c[i], anIntArray1668[l1], anIntArray1668[j2],
				                             anIntArray1668[l2], anIntArray1669[l1], anIntArray1669[j2],
				                             anIntArray1669[l2], anIntArray1670[l1], anIntArray1670[j2],
				                             anIntArray1670[l2], triangleColourOrTexture[i], vertexPerspectiveZAbs[j], vertexPerspectiveZAbs[k], vertexPerspectiveZAbs[l]);
				return;
			}
			if (i1 == 3) {
				int k1 = triangleDrawType[i] >> 2;
				int i2 = triPIndex[k1];
				int k2 = triMIndex[k1];
				int i3 = triNIndex[k1];
				Texture.drawTexturedTriangle(anIntArray1666[j], anIntArray1666[k],
				                             anIntArray1666[l], anIntArray1665[j], anIntArray1665[k],
				                             anIntArray1665[l], triangle_hsl_a[i], triangle_hsl_a[i],
				                             triangle_hsl_a[i], anIntArray1668[i2], anIntArray1668[k2],
				                             anIntArray1668[i3], anIntArray1669[i2], anIntArray1669[k2],
				                             anIntArray1669[i3], anIntArray1670[i2], anIntArray1670[k2],
				                             anIntArray1670[i3], triangleColourOrTexture[i], vertexPerspectiveZAbs[j], vertexPerspectiveZAbs[k], vertexPerspectiveZAbs[l]);
			}
		}
		
		private void method485(int i) {
		if(i < 0) i = 0;
			if(triangleColourOrTexture != null && triangleColourOrTexture.Length <= i) triangleColourOrTexture = new int[i+1];
			if (triangleColourOrTexture != null)
				if (triangleColourOrTexture[i] == 65535)
					return;
			int j = Texture.center_x;
			int k = Texture.center_y;
			int l = 0;
			int i1 = triangleX[i];
			int j1 = triangleY[i];
			int k1 = triangleZ[i];
			int l1 = anIntArray1670[i1];
			int i2 = anIntArray1670[j1];
			int j2 = anIntArray1670[k1];
			
			if (l1 >= 50) {
				anIntArray1678[l] = anIntArray1665[i1];
				anIntArray1679[l] = anIntArray1666[i1];
				anIntArray1680[l++] = triangle_hsl_a[i];
			} else {
				int k2 = anIntArray1668[i1];
				int k3 = anIntArray1669[i1];
				int k4 = triangle_hsl_a[i];
				if (j2 >= 50) {
					int k5 = (50 - l1) * modelIntArray4[j2 - l1];
					anIntArray1678[l] = j
						+ (k2 + ((anIntArray1668[k1] - k2) * k5 >> 16) << 9)
							/ 50;
					anIntArray1679[l] = k
						+ (k3 + ((anIntArray1669[k1] - k3) * k5 >> 16) << 9)
							/ 50;
					anIntArray1680[l++] = k4
						+ ((triangle_hsl_c[i] - k4) * k5 >> 16);
				}
				if (i2 >= 50) {
					int l5 = (50 - l1) * modelIntArray4[i2 - l1];
					anIntArray1678[l] = j
						+ (k2 + ((anIntArray1668[j1] - k2) * l5 >> 16) << 9)
							/ 50;
					anIntArray1679[l] = k
						+ (k3 + ((anIntArray1669[j1] - k3) * l5 >> 16) << 9)
							/ 50;
					anIntArray1680[l++] = k4
						+ ((triangle_hsl_b[i] - k4) * l5 >> 16);
				}
			}
			if (i2 >= 50) {
				anIntArray1678[l] = anIntArray1665[j1];
				anIntArray1679[l] = anIntArray1666[j1];
				anIntArray1680[l++] = triangle_hsl_b[i];
			} else {
				int l2 = anIntArray1668[j1];
				int l3 = anIntArray1669[j1];
				int l4 = triangle_hsl_b[i];
				if (l1 >= 50) {
					int i6 = (50 - i2) * modelIntArray4[l1 - i2];
					anIntArray1678[l] = j
						+ (l2 + ((anIntArray1668[i1] - l2) * i6 >> 16) << 9)
							/ 50;
					anIntArray1679[l] = k
						+ (l3 + ((anIntArray1669[i1] - l3) * i6 >> 16) << 9)
							/ 50;
					anIntArray1680[l++] = l4
						+ ((triangle_hsl_a[i] - l4) * i6 >> 16);
				}
				if (j2 >= 50) {
					int j6 = (50 - i2) * modelIntArray4[j2 - i2];
					anIntArray1678[l] = j
						+ (l2 + ((anIntArray1668[k1] - l2) * j6 >> 16) << 9)
							/ 50;
					anIntArray1679[l] = k
						+ (l3 + ((anIntArray1669[k1] - l3) * j6 >> 16) << 9)
							/ 50;
					anIntArray1680[l++] = l4
						+ ((triangle_hsl_c[i] - l4) * j6 >> 16);
				}
			}
			if (j2 >= 50) {
				anIntArray1678[l] = anIntArray1665[k1];
				anIntArray1679[l] = anIntArray1666[k1];
				anIntArray1680[l++] = triangle_hsl_c[i];
			} else {
				int i3 = anIntArray1668[k1];
				int i4 = anIntArray1669[k1];
				int i5 = triangle_hsl_c[i];
				if (i2 >= 50) {
					int k6 = (50 - j2) * modelIntArray4[i2 - j2];
					anIntArray1678[l] = j
						+ (i3 + ((anIntArray1668[j1] - i3) * k6 >> 16) << 9)
							/ 50;
					anIntArray1679[l] = k
						+ (i4 + ((anIntArray1669[j1] - i4) * k6 >> 16) << 9)
							/ 50;
					anIntArray1680[l++] = i5
						+ ((triangle_hsl_b[i] - i5) * k6 >> 16);
				}
				if (l1 >= 50) {
					int l6 = (50 - j2) * modelIntArray4[l1 - j2];
					anIntArray1678[l] = j
						+ (i3 + ((anIntArray1668[i1] - i3) * l6 >> 16) << 9)
							/ 50;
					anIntArray1679[l] = k
						+ (i4 + ((anIntArray1669[i1] - i4) * l6 >> 16) << 9)
							/ 50;
					anIntArray1680[l++] = i5
						+ ((triangle_hsl_a[i] - i5) * l6 >> 16);
				}
			}
			int j3 = anIntArray1678[0];
			int j4 = anIntArray1678[1];
			int j5 = anIntArray1678[2];
			int i7 = anIntArray1679[0];
			int j7 = anIntArray1679[1];
			int k7 = anIntArray1679[2];
			if ((j3 - j4) * (k7 - j7) - (i7 - j7) * (j5 - j4) > 0) {
				Texture.aBoolean1462 = false;
				if (l == 3) {
					if (j3 < 0 || j4 < 0 || j5 < 0 || j3 > DrawingArea.centerX
					    || j4 > DrawingArea.centerX || j5 > DrawingArea.centerX)
						Texture.aBoolean1462 = true;
					int l7;
					if (triangleDrawType == null)
						l7 = 0;
					else
						l7 = triangleDrawType[i] & 3;
					if (l7 == 0)
						Texture.drawShadedTriangle(i7, j7, k7, j3, j4, j5,
						                           anIntArray1680[0], anIntArray1680[1],
						                           anIntArray1680[2], -1f, -1f, -1f);
					else if (l7 == 1)
						Texture.drawFlatTriangle(i7, j7, k7, j3, j4, j5,
						                         modelIntArray3[triangle_hsl_a[i]], -1f, -1f, -1f);
					else if (l7 == 2) {
						int j8 = triangleDrawType[i] >> 2;
						int k9 = triPIndex[j8];
						int k10 = triMIndex[j8];
						int k11 = triNIndex[j8];
						Texture.drawTexturedTriangle(i7, j7, k7, j3, j4, j5,
						                             anIntArray1680[0], anIntArray1680[1],
						                             anIntArray1680[2], anIntArray1668[k9],
						                             anIntArray1668[k10], anIntArray1668[k11],
						                             anIntArray1669[k9], anIntArray1669[k10],
						                             anIntArray1669[k11], anIntArray1670[k9],
						                             anIntArray1670[k10], anIntArray1670[k11],
						                             triangleColourOrTexture[i], vertexPerspectiveZAbs[i1], vertexPerspectiveZAbs[j1], vertexPerspectiveZAbs[k1]);
					} else if (l7 == 3) {
						int k8 = triangleDrawType[i] >> 2;
						int l9 = triPIndex[k8];
						int l10 = triMIndex[k8];
						int l11 = triNIndex[k8];
						Texture.drawTexturedTriangle(i7, j7, k7, j3, j4, j5,
						                             triangle_hsl_a[i], triangle_hsl_a[i],
						                             triangle_hsl_a[i], anIntArray1668[l9],
						                             anIntArray1668[l10], anIntArray1668[l11],
						                             anIntArray1669[l9], anIntArray1669[l10],
						                             anIntArray1669[l11], anIntArray1670[l9],
						                             anIntArray1670[l10], anIntArray1670[l11],
						                             triangleColourOrTexture[i], vertexPerspectiveZAbs[i1], vertexPerspectiveZAbs[j1], vertexPerspectiveZAbs[k1]);
					}
				}
				if (l == 4) {
					if (j3 < 0 || j4 < 0 || j5 < 0 || j3 > DrawingArea.centerX
					    || j4 > DrawingArea.centerX || j5 > DrawingArea.centerX
					    || anIntArray1678[3] < 0
					    || anIntArray1678[3] > DrawingArea.centerX)
						Texture.aBoolean1462 = true;
					int i8;
					if (triangleDrawType == null)
						i8 = 0;
					else
						i8 = triangleDrawType[i] & 3;
					if (i8 == 0) {
						Texture.drawShadedTriangle(i7, j7, k7, j3, j4, j5,
						                           anIntArray1680[0], anIntArray1680[1],
						                           anIntArray1680[2], -1f, -1f, -1f);
						Texture.drawShadedTriangle(i7, k7, anIntArray1679[3], j3, j5,
						                           anIntArray1678[3], anIntArray1680[0],
						                           anIntArray1680[2], anIntArray1680[3], vertexPerspectiveZAbs[i1], vertexPerspectiveZAbs[j1], vertexPerspectiveZAbs[k1]);
						return;
					}
					if (i8 == 1) {
						int l8 = modelIntArray3[triangle_hsl_a[i]];
						Texture.drawFlatTriangle(i7, j7, k7, j3, j4, j5, l8, -1f, -1f, -1f);
						Texture.drawFlatTriangle(i7, k7, anIntArray1679[3], j3, j5, anIntArray1678[3], l8, vertexPerspectiveZAbs[i1], vertexPerspectiveZAbs[j1], vertexPerspectiveZAbs[k1]);
						return;
					}
					if (i8 == 2) {
						int i9 = triangleDrawType[i] >> 2;
						int i10 = triPIndex[i9];
						int i11 = triMIndex[i9];
						int i12 = triNIndex[i9];
						Texture.drawTexturedTriangle(i7, j7, k7, j3, j4, j5,
						                             anIntArray1680[0], anIntArray1680[1],
						                             anIntArray1680[2], anIntArray1668[i10],
						                             anIntArray1668[i11], anIntArray1668[i12],
						                             anIntArray1669[i10], anIntArray1669[i11],
						                             anIntArray1669[i12], anIntArray1670[i10],
						                             anIntArray1670[i11], anIntArray1670[i12],
						                             triangleColourOrTexture[i], vertexPerspectiveZAbs[i1], vertexPerspectiveZAbs[j1], vertexPerspectiveZAbs[k1]);
						Texture.drawTexturedTriangle(i7, k7, anIntArray1679[3], j3, j5,
						                             anIntArray1678[3], anIntArray1680[0],
						                             anIntArray1680[2], anIntArray1680[3],
						                             anIntArray1668[i10], anIntArray1668[i11],
						                             anIntArray1668[i12], anIntArray1669[i10],
						                             anIntArray1669[i11], anIntArray1669[i12],
						                             anIntArray1670[i10], anIntArray1670[i11],
						                             anIntArray1670[i12], triangleColourOrTexture[i], vertexPerspectiveZAbs[i1], vertexPerspectiveZAbs[j1], vertexPerspectiveZAbs[k1]);
						return;
					}
					if (i8 == 3) {
						int j9 = triangleDrawType[i] >> 2;
						int j10 = triPIndex[j9];
						int j11 = triMIndex[j9];
						int j12 = triNIndex[j9];
						Texture.drawTexturedTriangle(i7, j7, k7, j3, j4, j5,
						                             triangle_hsl_a[i], triangle_hsl_a[i],
						                             triangle_hsl_a[i], anIntArray1668[j10],
						                             anIntArray1668[j11], anIntArray1668[j12],
						                             anIntArray1669[j10], anIntArray1669[j11],
						                             anIntArray1669[j12], anIntArray1670[j10],
						                             anIntArray1670[j11], anIntArray1670[j12],
						                             triangleColourOrTexture[i], vertexPerspectiveZAbs[i1], vertexPerspectiveZAbs[j1], vertexPerspectiveZAbs[k1]);
						Texture.drawTexturedTriangle(i7, k7, anIntArray1679[3], j3, j5,
						                             anIntArray1678[3], triangle_hsl_a[i],
						                             triangle_hsl_a[i], triangle_hsl_a[i],
						                             anIntArray1668[j10], anIntArray1668[j11],
						                             anIntArray1668[j12], anIntArray1669[j10],
						                             anIntArray1669[j11], anIntArray1669[j12],
						                             anIntArray1670[j10], anIntArray1670[j11],
						                             anIntArray1670[j12], triangleColourOrTexture[i], vertexPerspectiveZAbs[i1], vertexPerspectiveZAbs[j1], vertexPerspectiveZAbs[k1]);
					}
				}
			}
		}
		
		private bool method486(int i, int j, int k, int l, int i1, int j1,
		                                int k1, int l1) {
			if (j < k && j < l && j < i1)
				return false;
			if (j > k && j > l && j > i1)
				return false;
			if (i < j1 && i < k1 && i < l1)
				return false;
			return i <= j1 || i <= k1 || i <= l1;
		}
		public bool isNew = false;
		private bool aBoolean1618;
		public static int anInt1620;
		public static Model aModel_1621 = new Model(true);
		private static int[] anIntArray1622 = new int[2000];
		private static int[] anIntArray1623 = new int[2000];
		private static int[] anIntArray1624 = new int[2000];
		private static int[] anIntArray1625 = new int[2000];
		public int numVertices;
		public int[] verticesX;
		public int[] verticesY;
		public int[] verticesZ;
		public int numTriangles;
		public int[] triangleX;
		public int[] triangleY;
		public int[] triangleZ;
		public int[] triangle_hsl_a;
		public int[] triangle_hsl_b;
		public int[] triangle_hsl_c;
		public int[] triangleDrawType;
		public int[] facePriority;
		public int[] triangleAlpha;
		public int[] triangleColourOrTexture;
		public int anInt1641;
		public int textureTriangleCount;
		public int[] triPIndex;
		public int[] triMIndex;
		public int[] triNIndex;
		public int minX;
		public int maxX;
		public int maxZ;
		public int minZ;
		public int diagonal2DAboveorigin;
		public int maxY;
		public int diagonal3D;
		public int diagonal3DAboveorigin;
		public int anInt1654;
		public int[] anIntArray1655;
		public int[] anIntArray1656;
		public int[][] anIntArrayArray1657;
		public int[][] anIntArrayArray1658;
		public bool aBoolean1659;
		public VertexNormal[] aClass33Array1660;
		public static Class21[] aClass21Array1661;
		static OnDemandFetcherParent aOnDemandFetcherParent_1662;
		static bool[] aBooleanArray1663 = new bool[8000];
		static bool[] aBooleanArray1664 = new bool[8000];
		static int[] anIntArray1665 = new int[8000];
		static int[] anIntArray1666 = new int[8000];
		static int[] anIntArray1667 = new int[8000];
		static int[] anIntArray1668 = new int[8000];
		static int[] anIntArray1669 = new int[8000];
		static int[] anIntArray1670 = new int[8000];
		static int[] vertexPerspectiveZAbs = new int[8000];
		static int[] anIntArray1671 = new int[1500];
		static int[][] anIntArrayArray1672 = NetDrawingTools.CreateDoubleIntArray(1500,512);//new int[1500][512];
		static int[] anIntArray1673 = new int[12];
		static int[][] anIntArrayArray1674 = NetDrawingTools.CreateDoubleIntArray(12,2000);//new int[12][2000];
		static int[] anIntArray1675 = new int[2000];
		static int[] anIntArray1676 = new int[2000];
		static int[] anIntArray1677 = new int[12];
		static int[] anIntArray1678 = new int[10];
		static int[] anIntArray1679 = new int[10];
		static int[] anIntArray1680 = new int[10];
		static int anInt1681;
		static int anInt1682;
		static int anInt1683;
		public static bool aBoolean1684;
		public static int anInt1685;
		public static int anInt1686;
		public static int anInt1687;
		public static int[] MouseOnObjects = new int[1000];
		public static int[] SINE;
		public static int[] COSINE;
		public static int[] modelIntArray3;
		public static int[] modelIntArray4;
		public bool Render317 = false;
		static Model(){
			SINE = Texture.SINE;
			COSINE = Texture.COSINE;
			modelIntArray3 = Texture.hsl2rgb;
			modelIntArray4 = Texture.anIntArray1469;
		}
	}
}
