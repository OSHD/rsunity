using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//exuse my shitty naming
using RS2Sharp;


public class ParticleGenerator
{

//	public float[][] particles;
//	public int[] verticesZ;
//	public int[] verticesX;
//	public int[] verticesY;
//	
//	ParticleGenerator(int[] xVertices, int[] yVertices, int[] zVertices, float[][] part) {
//		verticesX = xVertices;
//		verticesY = yVertices;
//		verticesZ = zVertices;
//		particles = part;
//	}
//	
//	public static ParticleGenerator generateParticles(RuneMesh model, int[] triangles, int numTriangles) {
//		int[] verticesX = null;
//		int[] verticesY = null;
//		int[] verticesZ = null;
//		float[][] particles = null;
//		if (model.textureCoords != null) {
//			int numTexturedTriangles = model.numTextureTriangles;
//			int[] xVertexBuffer1 = new int[numTexturedTriangles];
//			int[] xVertexBuffer2 = new int[numTexturedTriangles];
//			int[] yVertexBuffer1 = new int[numTexturedTriangles];
//			int[] yVertexBuffer2 = new int[numTexturedTriangles];
//			int[] zVertexBuffer1 = new int[numTexturedTriangles];
//			int[] zVertexBuffer2 = new int[numTexturedTriangles];
//			for (int texturedTriangle = 0; texturedTriangle < numTexturedTriangles; texturedTriangle++) {
//				xVertexBuffer1[texturedTriangle] = 2147483647;
//				xVertexBuffer2[texturedTriangle] = -2147483647;
//				yVertexBuffer1[texturedTriangle] = 2147483647;
//				yVertexBuffer2[texturedTriangle] = -2147483647;
//				zVertexBuffer1[texturedTriangle] = 2147483647;
//				zVertexBuffer2[texturedTriangle] = -2147483647;
//			}
//			for (int triangle = 0; triangle < numTriangles; triangle++) {
//				int tri = triangles[triangle];
//				short textureCoordinate = model.textureCoords[tri];
//				if (textureCoordinate > -1 && textureCoordinate < 32766) {
//					for (int axis = 0; axis < 3; axis++) {
//						short triangleViewspace;
//						if (axis == 0)
//							triangleViewspace = model.triangleViewspaceX[tri];
//						else if (axis == 1)
//							triangleViewspace = model.triangleViewspaceY[tri];
//						else
//							triangleViewspace = model.triangleViewspaceZ[tri];
//						int vertexX = model.verticesX[triangleViewspace];
//						int vertexY = model.verticesY[triangleViewspace];
//						int vertexZ = model.verticesZ[triangleViewspace];
//						if (vertexX < xVertexBuffer1[textureCoordinate])
//							xVertexBuffer1[textureCoordinate] = vertexX;
//						if (vertexX > xVertexBuffer2[textureCoordinate])
//							xVertexBuffer2[textureCoordinate] = vertexX;
//						if (vertexY < yVertexBuffer1[textureCoordinate])
//							yVertexBuffer1[textureCoordinate] = vertexY;
//						if (vertexY > yVertexBuffer2[textureCoordinate])
//							yVertexBuffer2[textureCoordinate] = vertexY;
//						if (vertexZ < zVertexBuffer1[textureCoordinate])
//							zVertexBuffer1[textureCoordinate] = vertexZ;
//						if (vertexZ > zVertexBuffer2[textureCoordinate])
//							zVertexBuffer2[textureCoordinate] = vertexZ;
//					}
//				}
//			}
//			verticesX = new int[numTexturedTriangles];
//			verticesY = new int[numTexturedTriangles];
//			verticesZ = new int[numTexturedTriangles];
//			particles = new float[numTexturedTriangles][];
//			for (int triangle = 0; triangle < numTexturedTriangles; triangle++) {
//				sbyte textureRenderType = model.textureRenderTypes[triangle];
//				if (textureRenderType > 0) {
//					verticesX[triangle] = (xVertexBuffer1[triangle] + xVertexBuffer2[triangle]) / 2;
//					verticesY[triangle] = (yVertexBuffer1[triangle] + yVertexBuffer2[triangle]) / 2;
//					verticesZ[triangle] = (zVertexBuffer1[triangle] + zVertexBuffer2[triangle]) / 2;
//					float scaleX;
//					float scaleY;
//					float scaleZ;
//					if (textureRenderType == 1) {
//						int xDir = model.particleDirectionX[triangle];
//						if (xDir == 0) {
//							scaleX = 1.0F;
//							scaleZ = 1.0F;
//						} else if (xDir > 0) {
//							scaleX = 1.0F;
//							scaleZ = xDir / 1024.0F;
//						} else {
//							scaleZ = 1.0F;
//							scaleX = -xDir / 1024.0F;
//						}
//						scaleY = 64.0F / model.particleDirectionY[triangle];
//					} else if (textureRenderType == 2) {
//						scaleX = 64.0F / model.particleDirectionX[triangle];
//						scaleY = 64.0F / model.particleDirectionY[triangle];
//						scaleZ = 64.0F / model.particleDirectionZ[triangle];
//					} else {
//						scaleX = model.particleDirectionX[triangle] / 1024.0F;
//						scaleY = (model.particleDirectionY[triangle] / 1024.0F);
//						scaleZ = (model.particleDirectionZ[triangle] / 1024.0F);
//					}
//					particles[triangle] = calculateParticle(
//						model.textureTrianglePIndex[triangle],
//						model.textureTriangleMIndex[triangle],
//						model.textureTriangleNIndex[triangle],
//						(model.particleLifespanX[triangle]) & 0xff,
//						scaleX, scaleY, scaleZ);
//				}
//			}
//		}
//		return new ParticleGenerator(verticesX, verticesY, verticesZ, particles);
//	}
//	
//	static float[] calculateParticle(int triX, int triY, int triZ, int rotation, float scaleX,
//	                                 float scaleY, float scaleZ) {
//		float[] originalParticles = new float[9];
//		float[] translatedParticles = new float[9];
//		float sinusoidalPeriod = (float) ((2 * Math.PI) / 256);//angular frequency of 256
//		float f_214_ = (float) Mathf.Cos(sinusoidalPeriod * rotation);
//		float f_215_ = (float) Mathf.Sin(sinusoidalPeriod * rotation);
//		float f_216_ = 1.0F - f_214_;
//		originalParticles[0] = f_214_;
//		originalParticles[1] = 0.0F;
//		originalParticles[2] = f_215_;
//		originalParticles[3] = 0.0F;
//		originalParticles[4] = 1.0F;
//		originalParticles[5] = 0.0F;
//		originalParticles[6] = -f_215_;
//		originalParticles[7] = 0.0F;
//		originalParticles[8] = f_214_;
//		float[] fs_217_ = new float[9];
//		float f_218_ = 1.0F;
//		float f_219_ = 0.0F;
//		f_214_ = triY / 32767.0F;
//		f_215_ = -(float) Mathf.Sqrt(1.0F - f_214_ * f_214_);
//		f_216_ = 1.0F - f_214_;
//		float distanceXZ = (float) Mathf.Sqrt(triX * triX + triZ * triZ);
//		if (distanceXZ == 0.0F && f_214_ == 0.0F)
//			translatedParticles = originalParticles;
//		else {
//			if (distanceXZ != 0.0F) {
//				f_218_ = -triZ / distanceXZ;
//				f_219_ = triX / distanceXZ;
//			}
//			fs_217_[0] = f_214_ + f_218_ * f_218_ * f_216_;
//			fs_217_[1] = f_219_ * f_215_;
//			fs_217_[2] = f_219_ * f_218_ * f_216_;
//			fs_217_[3] = -f_219_ * f_215_;
//			fs_217_[4] = f_214_;
//			fs_217_[5] = f_218_ * f_215_;
//			fs_217_[6] = f_218_ * f_219_ * f_216_;
//			fs_217_[7] = -f_218_ * f_215_;
//			fs_217_[8] = f_214_ + f_219_ * f_219_ * f_216_;
//			translatedParticles[0] = originalParticles[0] * fs_217_[0] + originalParticles[1] * fs_217_[3] + originalParticles[2]
//			* fs_217_[6];
//			translatedParticles[1] = originalParticles[0] * fs_217_[1] + originalParticles[1] * fs_217_[4] + originalParticles[2]
//			* fs_217_[7];
//			translatedParticles[2] = originalParticles[0] * fs_217_[2] + originalParticles[1] * fs_217_[5] + originalParticles[2]
//			* fs_217_[8];
//			translatedParticles[3] = originalParticles[3] * fs_217_[0] + originalParticles[4] * fs_217_[3] + originalParticles[5]
//			* fs_217_[6];
//			translatedParticles[4] = originalParticles[3] * fs_217_[1] + originalParticles[4] * fs_217_[4] + originalParticles[5]
//			* fs_217_[7];
//			translatedParticles[5] = originalParticles[3] * fs_217_[2] + originalParticles[4] * fs_217_[5] + originalParticles[5]
//			* fs_217_[8];
//			translatedParticles[6] = originalParticles[6] * fs_217_[0] + originalParticles[7] * fs_217_[3] + originalParticles[8]
//			* fs_217_[6];
//			translatedParticles[7] = originalParticles[6] * fs_217_[1] + originalParticles[7] * fs_217_[4] + originalParticles[8]
//			* fs_217_[7];
//			translatedParticles[8] = originalParticles[6] * fs_217_[2] + originalParticles[7] * fs_217_[5] + originalParticles[8]
//			* fs_217_[8];
//		}
//		translatedParticles[0] *= scaleX;
//		translatedParticles[1] *= scaleX;
//		translatedParticles[2] *= scaleX;
//		translatedParticles[3] *= scaleY;
//		translatedParticles[4] *= scaleY;
//		translatedParticles[5] *= scaleY;
//		translatedParticles[6] *= scaleZ;
//		translatedParticles[7] *= scaleZ;
//		translatedParticles[8] *= scaleZ;
//		return translatedParticles;
//	}
}