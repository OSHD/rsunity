using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/*
     * Texture mapping that converts to UV coords.
     * Ripped from the client by me. threw out the one clienthax gave 
     * me because it wasnt complete. 
     * 
     * For now it just loads every coord into a vector3
     * 
     */
using RS2Sharp;


public class TextureMapper
{

	public TextureMapper ()
	{
	}

	public Vector3[] textureCoordU;
	public Vector3[] textureCoordV;

	public void convertTextureCoordinates (RuneMesh model)
	{
		textureCoordU = new Vector3[model.numTriangles];
		textureCoordV = new Vector3[model.numTriangles];
//		int[] faces = new int[model.numTriangles];
//		for (int triangle = 0; triangle < model.numTriangles; triangle++)
//			faces [triangle] = triangle;
//		ParticleGenerator particles = ParticleGenerator.generateParticles (model, faces, model.numTriangles);
		for (int triangle = 0; triangle < model.numTriangles; triangle++) {
			float[] U = new float[3];
			float[] V = new float[3];
//			long l_2242_ = 0;
//			long l_2241_ = 0;
//			long l = 0;
//			int i_2249_ = 0;
//			int i_2248_ = 0;
//			int i_2247_ = 0;
//			int face = faces [triangle];
//			int textureId = (model.faceTexture == null ? (int)-1
//                        : (model.faceTexture [face] & 0xffff));
//
//			if (textureId != -1) {
//				int coord = (model.textureCoords != null ? (int)model.textureCoords [triangle]
//				             : -1);
//				if (coord == 32766) {
//					int a = model.faceUVIndexA [triangle] & 0xff;
//					int b = model.faceUVIndexB [triangle] & 0xff;
//					int c = model.faceUVIndexC [triangle] & 0xff;
//					a += (model.uvVertexOffset [model.triangleViewspaceX [face]]);
//					l_2242_ = a;
//					b += (model.uvVertexOffset [model.triangleViewspaceY [face]]);
//					l_2241_ = a;
//					c += (model.uvVertexOffset [model.triangleViewspaceZ [face]]);
//					l = a;
//					U [0] = model.texCoordU [a];
//					V [0] = model.texCoordV [a];
//					U [1] = model.texCoordU [b];
//					V [1] = model.texCoordV [b];
//					U [2] = model.texCoordU [c];
//					V [2] = model.texCoordV [c];
//				} else if (coord == -1) {
//					U [0] = 0.0F;
//					V [0] = 1.0F;
//					l_2242_ = 65535L;
//					U [1] = 1.0F;
//					V [1] = 1.0F;
//					l_2241_ = 131071L;
//					U [2] = 0.0F;
//					V [2] = 0.0F;
//					l = 196607L;
//				} else {
//					if (model.newFormat)
//						coord &= 0xffff;
//					else
//						coord &= 0xff;
//					sbyte type = model.textureRenderTypes [coord];
//					if (type == 0) {
//						short viewspaceX = model.triangleViewspaceX [triangle];
//						short viewspaceY = model.triangleViewspaceY [triangle];
//						short viewspaceZ = model.triangleViewspaceZ [triangle];
//						short texA = (short)model.textureTrianglePIndex [coord];
//						short texB = (short)model.textureTriangleMIndex [coord];
//						short texC = (short)model.textureTriangleNIndex [coord];
//						float pX = (float)model.verticesX [texA];
//						float pY = (float)model.verticesY [texA];
//						float pZ = (float)model.verticesZ [texA];
//						float mX = ((float)model.verticesX [texB] - pX);
//						float mY = ((float)model.verticesY [texB] - pY);
//						float mZ = ((float)model.verticesZ [texB] - pZ);
//						float nX = ((float)model.verticesX [texC] - pX);
//						float nY = ((float)model.verticesY [texC] - pY);
//						float nZ = ((float)model.verticesZ [texC] - pZ);
//						float iA = ((float)model.verticesX [viewspaceX] - pX);
//						float jA = ((float)model.verticesY [viewspaceX] - pY);
//						float kA = ((float)model.verticesZ [viewspaceX] - pZ);
//						float iB = ((float)model.verticesX [viewspaceY] - pX);
//						float jB = ((float)model.verticesY [viewspaceY] - pY);
//						float kB = ((float)model.verticesZ [viewspaceY] - pZ);
//						float iC = ((float)model.verticesX [viewspaceZ] - pX);
//						float jC = ((float)model.verticesY [viewspaceZ] - pY);
//						float kC = ((float)model.verticesZ [viewspaceZ] - pZ);
//						float horizontal = mY * nZ - mZ * nY;
//						float origin = mZ * nX - mX * nZ;
//						float vertical = mX * nY - mY * nX;
//						float a = nY * vertical - nZ * origin;
//						float b = nZ * horizontal - nX * vertical;
//						float c = nX * origin - nY * horizontal;
//						float d = 1.0F / (a * mX + b * mY + c * mZ);
//						U [0] = (a * iA + b * jA + c * kA) * d;
//						U [1] = (a * iB + b * jB + c * kB) * d;
//						U [2] = (a * iC + b * jC + c * kC) * d;
//						a = mY * vertical - mZ * origin;
//						b = mZ * horizontal - mX * vertical;
//						c = mX * origin - mY * horizontal;
//						d = 1.0F / (a * nX + b * nY + c * nZ);
//						V [0] = (a * iA + b * jA + c * kA) * d;
//						V [1] = (a * iB + b * jB + c * kB) * d;
//						V [2] = (a * iC + b * jC + c * kC) * d;
////						if (U [0] < 0 || U [1] < 0 || U [2] < 0 || V [0] < 0 || V [1] < 0 || V [2] < 0 || U [0] > 1 || U [1] > 1 || U [2] > 1 || V [0] > 1 || V [1] > 1 || V [2] > 1) {
////							//	Debug.Log (type + " : " + U[0] + " : " + U[1] + " : " + U[2] + " : " + V[0] + " : " + V[1] + " : " + V[2]);
////						}
//
//					} else {
//						short faceA = model.triangleViewspaceX [triangle];
//						short faceB = model.triangleViewspaceY [triangle];
//						short faceC = model.triangleViewspaceZ [triangle];
//						int vertexX = particles.verticesX [coord];
//						int vertexY = particles.verticesY [coord];
//						int vertexZ = particles.verticesZ [coord];
//						float[] fs = particles.particles [coord];
//						float[] aFloatArray9098 = new float[2];
//						aFloatArray9098 [0] = 0;
//						aFloatArray9098 [1] = 0;
//						sbyte i_2288_ = model.particleLifespanY [coord];
//						float f_2289_ = (model.particleLifespanZ [coord] / 256.0F);
//						if (type == 1) {
//							float f_2290_ = (model.particleDirectionZ [coord] / 1024.0F);
//							method3208 (model.verticesX [faceA], model.verticesY [faceA],
//							           model.verticesZ [faceA], vertexX, vertexY,
//							           vertexZ, fs, f_2290_, i_2288_, f_2289_,
//							            aFloatArray9098);
//							U [0] = aFloatArray9098 [0];
//							V [0] = aFloatArray9098 [1];
//							method3208 (model.verticesX [faceB], model.verticesY [faceB],
//							           model.verticesZ [faceB], vertexX, vertexY,
//							           vertexZ, fs, f_2290_, i_2288_, f_2289_,
//							            aFloatArray9098);
//							U [1] = aFloatArray9098 [0];
//							V [1] = aFloatArray9098 [1];
//							method3208 (model.verticesX [faceC], model.verticesY [faceC],
//							           model.verticesZ [faceC], vertexX, vertexY,
//							           vertexZ, fs, f_2290_, i_2288_, f_2289_,
//							            aFloatArray9098);
//							U [2] = aFloatArray9098 [0];
//							V [2] = aFloatArray9098 [1];
//							float f_2291_ = f_2290_ / 2.0F;
//							if ((i_2288_ & 0x1) == 0) {
//								if (U [1] - U [0] > f_2291_) {
//									U [1] -= f_2290_;
//									i_2248_ = 1;
//								} else if (U [0] - U [1] > f_2291_) {
//									U [1] += f_2290_;
//									i_2248_ = 2;
//								}
//								if (U [2] - U [0] > f_2291_) {
//									U [2] -= f_2290_;
//									i_2249_ = 1;
//								} else if (U [0] - U [2] > f_2291_) {
//									U [2] += f_2290_;
//									i_2249_ = 2;
//								}
//							} else {
//								if (V [1] - V [0] > f_2291_) {
//									V [1] -= f_2290_;
//									i_2248_ = 1;
//								} else if (V [0] - V [1] > f_2291_) {
//									V [1] += f_2290_;
//									i_2248_ = 2;
//								}
//								if (V [2] - V [0] > f_2291_) {
//									V [2] -= f_2290_;
//									i_2249_ = 1;
//								} else if (V [0] - V [2] > f_2291_) {
//									V [2] += f_2290_;
//									i_2249_ = 2;
//								}
//							}
//						} else if (type == 2) {
//							float f_2292_ = (model.texturePrimaryColor [coord] / 256.0F);
//							float f_2293_ = (model.textureSecondaryColor [coord] / 256.0F);
//							int i_2294_ = (model.verticesX [faceB] - model.verticesX [faceA]);
//							int i_2295_ = (model.verticesY [faceB] - model.verticesY [faceA]);
//							int i_2296_ = (model.verticesZ [faceB] - model.verticesZ [faceA]);
//							int i_2297_ = (model.verticesX [faceC] - model.verticesX [faceA]);
//							int i_2298_ = (model.verticesY [faceC] - model.verticesY [faceA]);
//							int i_2299_ = (model.verticesZ [faceC] - model.verticesZ [faceA]);
//							int i_2300_ = i_2295_ * i_2299_ - i_2298_ * i_2296_;
//							int i_2301_ = i_2296_ * i_2297_ - i_2299_ * i_2294_;
//							int i_2302_ = i_2294_ * i_2298_ - i_2297_ * i_2295_;
//							float f_2303_ = (64.0F / model.particleDirectionX [coord]);
//							float f_2304_ = (64.0F / model.particleDirectionY [coord]);
//							float f_2305_ = (64.0F / model.particleDirectionZ [coord]);
//							float f_2306_ = ((i_2300_ * fs [0]
//								+ i_2301_ * fs [1] + i_2302_
//								* fs [2]) / f_2303_);
//							float f_2307_ = ((i_2300_ * fs [3]
//								+ i_2301_ * fs [4] + i_2302_
//								* fs [5]) / f_2304_);
//							float f_2308_ = ((i_2300_ * fs [6]
//								+ i_2301_ * fs [7] + i_2302_
//								* fs [8]) / f_2305_);
//							i_2247_ = method3051 (f_2306_, f_2307_, f_2308_);
//							method3053 (model.verticesX [faceA], model.verticesY [faceA],
//							           model.verticesZ [faceA], vertexX, vertexY,
//							           vertexZ, i_2247_, fs, i_2288_, f_2289_,
//							            f_2292_, f_2293_, aFloatArray9098);
//							U [0] = aFloatArray9098 [0];
//							V [0] = aFloatArray9098 [1];
//							method3053 (model.verticesX [faceB], model.verticesY [faceB],
//							           model.verticesZ [faceB], vertexX, vertexY,
//							           vertexZ, i_2247_, fs, i_2288_, f_2289_,
//							            f_2292_, f_2293_, aFloatArray9098);
//							U [1] = aFloatArray9098 [0];
//							V [1] = aFloatArray9098 [1];
//							method3053 (model.verticesX [faceC], model.verticesY [faceC],
//							           model.verticesZ [faceC], vertexX, vertexY,
//							           vertexZ, i_2247_, fs, i_2288_, f_2289_,
//							            f_2292_, f_2293_, aFloatArray9098);
//							U [2] = aFloatArray9098 [0];
//							V [2] = aFloatArray9098 [1];
//						} else if (type == 3) {
//							method3052 (model.verticesX [faceA], model.verticesY [faceA],
//							           model.verticesZ [faceA], vertexX, vertexY,
//							           vertexZ, fs, i_2288_, f_2289_,
//							            aFloatArray9098);
//							U [0] = aFloatArray9098 [0];
//							V [0] = aFloatArray9098 [1];
//							method3052 (model.verticesX [faceB], model.verticesY [faceB],
//							           model.verticesZ [faceB], vertexX, vertexY,
//							           vertexZ, fs, i_2288_, f_2289_,
//							            aFloatArray9098);
//							U [1] = aFloatArray9098 [0];
//							V [1] = aFloatArray9098 [1];
//							method3052 (model.verticesX [faceC], model.verticesY [faceC],
//							           model.verticesZ [faceC], vertexX, vertexY,
//							           vertexZ, fs, i_2288_, f_2289_,
//							            aFloatArray9098);
//							U [2] = aFloatArray9098 [0];
//							V [2] = aFloatArray9098 [1];
//							if ((i_2288_ & 0x1) == 0) {
//								if (U [1] - U [0] > 0.5F) {
//									U [1]--;
//									i_2248_ = 1;
//								} else if (U [0] - U [1] > 0.5F) {
//									U [1]++;
//									i_2248_ = 2;
//								}
//								if (U [2] - U [0] > 0.5F) {
//									U [2]--;
//									i_2249_ = 1;
//								} else if (U [0] - U [2] > 0.5F) {
//									U [2]++;
//									i_2249_ = 2;
//								}
//							} else {
//								if (V [1] - V [0] > 0.5F) {
//									V [1]--;
//									i_2248_ = 1;
//								} else if (V [0] - V [1] > 0.5F) {
//									V [1]++;
//									i_2248_ = 2;
//								}
//								if (V [2] - V [0] > 0.5F) {
//									V [2]--;
//									i_2249_ = 1;
//								} else if (V [0] - V [2] > 0.5F) {
//									V [2]++;
//									i_2249_ = 2;
//								}
//							}
//						}
//					}
//					l_2242_ = i_2247_ << 16 | coord;
//					l_2241_ = i_2248_ << 19 | l_2242_;
//					l = i_2249_ << 19 | l_2242_;
//				}
//			} else {
//				l = 0L;
//				l_2241_ = 0L;
//				l_2242_ = 0L;
//			}
//						if (U [0] < 0 || U [1] < 0 || U [2] < 0 || V [0] < 0|| V [1] < 0 || V [2] < 0 || U [0] > 1 || U [1] > 1 || U [2] > 1 || V [0] > 1 || V [1] > 1 || V [2] > 1)
//						{
							U [0] = 0.0F;
							V [0] = 1.0F;
							U [1] = 1.0F;
							V [1] = 1.0F;
							U [2] = 0.0F;
							V [2] = 0.0F;
//						}
			textureCoordU [triangle] = new Vector3 (U [0], U [1], U [2]);
			textureCoordV [triangle] = new Vector3 (V [0], V [1], V [2]);
//			if (textureId >= 0 && textureId < Engine.atlasInfo.Length) {
//				textureCoordU [triangle] *= float.Parse (Engine.atlasInfo [textureId, 3]);
//				textureCoordV [triangle] *= float.Parse (Engine.atlasInfo [textureId, 4]);
//				textureCoordU [triangle] += new Vector3(float.Parse (Engine.atlasInfo [textureId, 1]),float.Parse (Engine.atlasInfo [textureId, 1]),float.Parse (Engine.atlasInfo [textureId, 1]));
//				textureCoordV [triangle] += new Vector3(float.Parse (Engine.atlasInfo [textureId, 2]),float.Parse (Engine.atlasInfo [textureId, 2]),float.Parse (Engine.atlasInfo [textureId, 2]));
//			}
		}
	}
//
//	float[] method699 (int argument, float[] argument_1_, int lifespanY, float argument_3_,
//            int argument_4_, int vertexY, float argument_6_, int argument_7_,
//            int vertexZ, float[] argument_9_, int vertexX,
//            int argument_11_)
//	{
//		vertexY -= argument_4_;
//		vertexZ -= argument_7_;
//		vertexX -= argument;
//		float f = (argument_1_ [0] * (float)vertexX + argument_1_ [1] * (float)vertexY + argument_1_ [2] * (float)vertexZ);
//		float f_12_ = (argument_1_ [5] * (float)vertexZ + ((float)vertexX * argument_1_ [3] + argument_1_ [4] * (float)vertexY));
//		float f_13_ = ((float)vertexZ * argument_1_ [8] + ((float)vertexX * argument_1_ [6] + (float)vertexY * argument_1_ [7]));
//		float f_14_ = ((float)Mathf.Atan2 (f, f_13_) / 6.2831855F + 0.5F);
//		if (argument_6_ != 1.0F)
//			f_14_ *= argument_6_;
//		float f_15_ = f_12_ + 0.5F + argument_3_;
//		if (lifespanY != 1) {
//			if (lifespanY == 2) {
//				f_15_ = -f_15_;
//				f_14_ = -f_14_;
//			} else if (lifespanY == 3) {
//				float f_16_ = f_14_;
//				f_14_ = f_15_;
//				f_15_ = -f_16_;
//			}
//		} else {
//			float f_17_ = f_14_;
//			f_14_ = -f_15_;
//			f_15_ = f_17_;
//		}
//		argument_9_ [0] = f_14_;
//		argument_9_ [1] = f_15_;
//
//		return argument_9_;
//	}
//
//	void method3208 (int i, int i_410_, int i_411_, int i_412_, int i_413_,
//	                int i_414_, float[] fs, float f, int i_415_, float f_416_,
//	                float[] fs_417_)
//	{
//		i -= i_412_;
//		i_410_ -= i_413_;
//		i_411_ -= i_414_;
//		float f_418_ = (i * fs [0] + i_410_ * fs [1] + i_411_
//			* fs [2]);
//		float f_419_ = (i * fs [3] + i_410_ * fs [4] + i_411_
//			* fs [5]);
//		float f_420_ = (i * fs [6] + i_410_ * fs [7] + i_411_
//			* fs [8]);
//		float f_421_ = (((float)Mathf.Atan2 (f_418_, f_420_) / 6.2831855F) + 0.5F);
//		if (f != 1.0F)
//			f_421_ *= f;
//		float f_422_ = f_419_ + 0.5F + f_416_;
//		if (i_415_ == 1) {
//			float f_423_ = f_421_;
//			f_421_ = -f_422_;
//			f_422_ = f_423_;
//		} else if (i_415_ == 2) {
//			f_421_ = -f_421_;
//			f_422_ = -f_422_;
//		} else if (i_415_ == 3) {
//			float f_424_ = f_421_;
//			f_421_ = f_422_;
//			f_422_ = -f_424_;
//		}
//		fs_417_ [0] = f_421_;
//		fs_417_ [1] = f_422_;
//	}
//	
//	void method3052 (int i, int i_5_, int i_6_, int i_7_, int i_8_, int i_9_,
//	                float[] fs, int i_10_, float f, float[] fs_11_)
//	{
//		i -= i_7_;
//		i_5_ -= i_8_;
//		i_6_ -= i_9_;
//		float f_12_ = i * fs [0] + i_5_ * fs [1] + i_6_
//			* fs [2];
//		float f_13_ = i * fs [3] + i_5_ * fs [4] + i_6_
//			* fs [5];
//		float f_14_ = i * fs [6] + i_5_ * fs [7] + i_6_
//			* fs [8];
//		float f_15_ = (float)RuneMath
//			.Sqrt (f_12_ * f_12_ + f_13_ * f_13_ + f_14_ * f_14_);
//		float f_16_ = ((float)Mathf.Atan2 (f_12_, f_14_) / 6.2831855F + 0.5F);
//		float f_17_ = ((float)Mathf.Asin (f_13_ / f_15_) / 3.1415927F + 0.5F + f);
//		if (i_10_ == 1) {
//			float f_18_ = f_16_;
//			f_16_ = -f_17_;
//			f_17_ = f_18_;
//		} else if (i_10_ == 2) {
//			f_16_ = -f_16_;
//			f_17_ = -f_17_;
//		} else if (i_10_ == 3) {
//			float f_19_ = f_16_;
//			f_16_ = f_17_;
//			f_17_ = -f_19_;
//		}
//		fs_11_ [0] = f_16_;
//		fs_11_ [1] = f_17_;
//	}
//	
//	void method3053 (int i, int i_20_, int i_21_, int i_22_, int i_23_,
//	                int i_24_, int i_25_, float[] fs, int i_26_, float f, float f_27_,
//	                float f_28_, float[] fs_29_)
//	{
//		i -= i_22_;
//		i_20_ -= i_23_;
//		i_21_ -= i_24_;
//		float f_30_ = (i * fs [0] + i_20_ * fs [1] + i_21_
//			* fs [2]);
//		float f_31_ = (i * fs [3] + i_20_ * fs [4] + i_21_
//			* fs [5]);
//		float f_32_ = (i * fs [6] + i_20_ * fs [7] + i_21_
//			* fs [8]);
//		float f_33_;
//		float f_34_;
//		if (i_25_ == 0) {
//			f_33_ = f_30_ + f + 0.5F;
//			f_34_ = -f_32_ + f_28_ + 0.5F;
//		} else if (i_25_ == 1) {
//			f_33_ = f_30_ + f + 0.5F;
//			f_34_ = f_32_ + f_28_ + 0.5F;
//		} else if (i_25_ == 2) {
//			f_33_ = -f_30_ + f + 0.5F;
//			f_34_ = -f_31_ + f_27_ + 0.5F;
//		} else if (i_25_ == 3) {
//			f_33_ = f_30_ + f + 0.5F;
//			f_34_ = -f_31_ + f_27_ + 0.5F;
//		} else if (i_25_ == 4) {
//			f_33_ = f_32_ + f_28_ + 0.5F;
//			f_34_ = -f_31_ + f_27_ + 0.5F;
//		} else {
//			f_33_ = -f_32_ + f_28_ + 0.5F;
//			f_34_ = -f_31_ + f_27_ + 0.5F;
//		}
//		if (i_26_ == 1) {
//			float f_35_ = f_33_;
//			f_33_ = -f_34_;
//			f_34_ = f_35_;
//		} else if (i_26_ == 2) {
//			f_33_ = -f_33_;
//			f_34_ = -f_34_;
//		} else if (i_26_ == 3) {
//			float f_36_ = f_33_;
//			f_33_ = f_34_;
//			f_34_ = -f_36_;
//		}
//		fs_29_ [0] = f_33_;
//		fs_29_ [1] = f_34_;
//	}
//	
//	int method3051 (float f, float f_0_, float f_1_)
//	{
//		float f_2_ = f < 0.0F ? -f : f;
//		float f_3_ = f_0_ < 0.0F ? -f_0_ : f_0_;
//		float f_4_ = f_1_ < 0.0F ? -f_1_ : f_1_;
//		if (f_3_ > f_2_ && f_3_ > f_4_) {
//			if (f_0_ > 0.0F)
//				return 0;
//			return 1;
//		}
//		if (f_4_ > f_2_ && f_4_ > f_3_) {
//			if (f_1_ > 0.0F)
//				return 2;
//			return 3;
//		}
//		if (f > 0.0F)
//			return 4;
//		return 5;
//	}
//
//	float[] method226 (float[] argument, int argument_34_, int argument_35_,
//            int argument_36_, int argument_37_, int argument_38_,
//            float[] argument_39_, int argument_40_, bool argument_41_,
//            int argument_42_, float argument_43_)
//	{
//		argument_38_ -= argument_36_;
//		argument_37_ -= argument_40_;
//		argument_35_ -= argument_34_;
//		float f = ((float)argument_37_ * argument_39_ [2]
//			+ ((float)argument_35_ * argument_39_ [0]
//			+ argument_39_ [1] * (float)argument_38_));
//		float f_44_ = (argument_39_ [5] * (float)argument_37_
//			+ ((float)argument_35_ * argument_39_ [3]
//			+ argument_39_ [4] * (float)argument_38_));
//		float f_45_ = (argument_39_ [8] * (float)argument_37_
//			+ ((float)argument_38_ * argument_39_ [7]
//			+ argument_39_ [6] * (float)argument_35_));
//		float f_46_ = (float)RuneMath.Sqrt ((f_44_ * f_44_ + f * f
//			+ f_45_ * f_45_));
//		float f_47_
//                = ((float)Mathf.Atan2 (f, f_45_) / 6.2831855F
//			+ 0.5F);
//		float f_48_
//                = (argument_43_
//			+ ((float)Mathf.Asin ((f_44_ / f_46_)) / 3.1415927F
//			+ 0.5F));
//		if (argument_42_ == 1) {
//			float f_49_ = f_47_;
//			f_47_ = -f_48_;
//			f_48_ = f_49_;
//		} else if (argument_42_ != 2) {
//			if (argument_42_ == 3) {
//				float f_50_ = f_47_;
//				f_47_ = f_48_;
//				f_48_ = -f_50_;
//			}
//		} else {
//			f_48_ = -f_48_;
//			f_47_ = -f_47_;
//		}
//		argument [0] = f_47_;
//		argument [1] = f_48_;
//		return argument;
//	}
//
//	float[] method1689 (int argument, float[] argument_0_, int argument_1_, int argument_2_,
//            int argument_3_, int argument_4_, int argument_5_, int argument_6_,
//            float[] argument_7_, float argument_8_, int argument_9_,
//            int argument_10_, float argument_11_, float argument_12_)
//	{
//		argument_10_ -= argument_2_;
//		argument_9_ -= argument_3_;
//		argument_4_ -= argument_5_;
//		float f = (argument_7_ [2] * (float)argument_9_
//			+ (argument_7_ [0] * (float)argument_4_
//			+ argument_7_ [1] * (float)argument_10_));
//		float f_13_ = (argument_7_ [5] * (float)argument_9_
//			+ (argument_7_ [4] * (float)argument_10_
//			+ argument_7_ [3] * (float)argument_4_));
//		float f_14_ = (argument_7_ [7] * (float)argument_10_
//			+ (float)argument_4_ * argument_7_ [6]
//			+ (float)argument_9_ * argument_7_ [8]);
//		float f_15_;
//		float f_16_;
//		if (argument != 0) {
//			if (argument != 1) {
//				if (argument == 2) {
//					f_16_ = argument_11_ + -f + 0.5F;
//					f_15_ = argument_12_ + -f_13_ + 0.5F;
//				} else if (argument == 3) {
//					f_15_ = argument_12_ + -f_13_ + 0.5F;
//					f_16_ = f + argument_11_ + 0.5F;
//				} else if (argument == 4) {
//					f_16_ = argument_8_ + f_14_ + 0.5F;
//					f_15_ = -f_13_ + argument_12_ + 0.5F;
//				} else {
//					f_16_ = argument_8_ + -f_14_ + 0.5F;
//					f_15_ = argument_12_ + -f_13_ + 0.5F;
//				}
//			} else {
//				f_16_ = f + argument_11_ + 0.5F;
//				f_15_ = argument_8_ + f_14_ + 0.5F;
//			}
//		} else {
//			f_15_ = argument_8_ + -f_14_ + 0.5F;
//			f_16_ = argument_11_ + f + 0.5F;
//		}
//		if (argument_1_ == 1) {
//			float f_17_ = f_16_;
//			f_16_ = -f_15_;
//			f_15_ = f_17_;
//		} else if (argument_1_ == 2) {
//			f_15_ = -f_15_;
//			f_16_ = -f_16_;
//		} else if (argument_1_ == 3) {
//			float f_18_ = f_16_;
//			f_16_ = f_15_;
//			f_15_ = -f_18_;
//		}
//		argument_0_ [1] = f_15_;
//		argument_0_ [argument_6_] = f_16_;
//		return argument_0_;
//	}
//
//	int method1337 (float argument, float argument_2_,
//               float argument_3_, int argument_4_)
//	{
//		float f = argument < 0.0F ? -argument : argument;
//		float f_5_ = argument_2_ < 0.0F ? -argument_2_ : argument_2_;
//		float f_6_ = !(argument_3_ < 0.0F) ? argument_3_ : -argument_3_;
//		if (!(f < f_5_) || !(f_5_ > f_6_)) {
//			if (f_6_ > f && f_5_ < f_6_) {
//				if (argument_3_ > 0.0F)
//					return 2;
//				return 3;
//			}
//			if (!(argument > 0.0F))
//				return 5;
//			return 4;
//		}
//		if (argument_2_ > 0.0F)
//			return 0;
//		return 1;
//	}
}
