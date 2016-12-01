using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RS2Sharp
{
	// Decompiled by Jad v1.5.8f. Copyright 2001 Pavel Kouznetsov.
	// Jad home page: http://www.kpdus.com/jad.html
	// Decompiler options: packimports(3) 

	public class ItemDef
	{

//		public static void nullLoader()
//		{
//			mruNodes2 = null;
//			mruNodes1 = null;
//			streamIndices = null;
//			cache = null;
//			stream = null;
//		}
//
//		public bool method192(int j)
//		{
//			int k = anInt175;
//			int l = anInt166;
//			if (j == 1)
//			{
//				k = anInt197;
//				l = anInt173;
//			}
//			if (k == -1)
//				return true;
//			bool flag = true;
//			if (!Model.method463(k))
//				flag = false;
//			if (l != -1 && !Model.method463(l))
//				flag = false;
//			return flag;
//		}
//
//		public static void unpackConfig(StreamLoader streamLoader)
//		{
//			stream = new Stream(streamLoader.getDataForName("obj.dat"));
//			Stream index = new Stream(streamLoader.getDataForName("obj.idx"));
//			totalItems = index.readUnsignedWord();
//			streamIndices = new int[totalItems + 10000];
//			int i = 2;
//			for (int j = 0; j < totalItems; j++)
//			{
//				streamIndices[j] = i;
//				i += index.readUnsignedWord();
//			}
//
//			cache = new ItemDef[10];
//			for (int k = 0; k < 10; k++)
//				cache[k] = new ItemDef();
//
//		}
//
//		public Model method194(int j)
//		{
//			int k = anInt175;
//			int l = anInt166;
//			if (j == 1)
//			{
//				k = anInt197;
//				l = anInt173;
//			}
//			if (k == -1)
//				return null;
//			Model model = Model.method462(k);
//			if (l != -1)
//			{
//				Model model_1 = Model.method462(l);
//				Model[] aclass30_sub2_sub4_sub6s = {
//                    model, model_1
//            };
//				model = new Model(2, aclass30_sub2_sub4_sub6s);
//			}
//			if (modifiedModelColors != null)
//			{
//				for (int i1 = 0; i1 < modifiedModelColors.Length; i1++)
//					model.method476(modifiedModelColors[i1], originalModelColors[i1]);
//
//			}
//			return model;
//		}
//
//		public bool method195(int j)
//		{
//			int k = anInt165;
//			int l = anInt188;
//			int i1 = anInt185;
//			if (j == 1)
//			{
//				k = anInt200;
//				l = anInt164;
//				i1 = anInt162;
//			}
//			if (k == -1)
//				return true;
//			bool flag = true;
//			if (!Model.method463(k))
//				flag = false;
//			if (l != -1 && !Model.method463(l))
//				flag = false;
//			if (i1 != -1 && !Model.method463(i1))
//				flag = false;
//			return flag;
//		}
//
//		public Model method196(int i)
//		{
//			int j = anInt165;
//			int k = anInt188;
//			int l = anInt185;
//			if (i == 1)
//			{
//				j = anInt200;
//				k = anInt164;
//				l = anInt162;
//			}
//			if (j == -1)
//				return null;
//			Model model = Model.method462(j);
//			if (k != -1)
//				if (l != -1)
//				{
//					Model model_1 = Model.method462(k);
//					Model model_3 = Model.method462(l);
//					Model[] aclass30_sub2_sub4_sub6_1s = {
//                        model, model_1, model_3
//                };
//					model = new Model(3, aclass30_sub2_sub4_sub6_1s);
//				}
//				else
//				{
//					Model model_2 = Model.method462(k);
//					Model[] aclass30_sub2_sub4_sub6s = {
//                        model, model_2
//                };
//					model = new Model(2, aclass30_sub2_sub4_sub6s);
//				}
//			if (i == 0 && aByte205 != 0)
//				model.method475(0, aByte205, 0);
//			if (i == 1 && aByte154 != 0)
//				model.method475(0, aByte154, 0);
//			if (modifiedModelColors != null)
//			{
//				for (int i1 = 0; i1 < modifiedModelColors.Length; i1++)
//					model.method476(modifiedModelColors[i1], originalModelColors[i1]);
//
//			}
//			return model;
//		}
//
//		private void setDefaults()
//		{
//			modelID = 0;
//			name = null;
//			description = null;
//			modifiedModelColors = null;
//			originalModelColors = null;
//			modelZoom = 2000;
//			modelRotation1 = 0;
//			modelRotation2 = 0;
//			anInt204 = 0;
//			modelOffset1 = 0;
//			modelOffset2 = 0;
//			stackable = false;
//			value = 1;
//			membersObject = false;
//			groundActions = null;
//			actions = null;
//			anInt165 = -1;
//			anInt188 = -1;
//			aByte205 = 0;
//			anInt200 = -1;
//			anInt164 = -1;
//			aByte154 = 0;
//			anInt185 = -1;
//			anInt162 = -1;
//			anInt175 = -1;
//			anInt166 = -1;
//			anInt197 = -1;
//			anInt173 = -1;
//			stackIDs = null;
//			stackAmounts = null;
//			certID = -1;
//			certTemplateID = -1;
//			anInt167 = 128;
//			anInt192 = 128;
//			anInt191 = 128;
//			anInt196 = 0;
//			anInt184 = 0;
//			team = 0;
//		}
//
//		public static ItemDef forID(int i)
//		{
//			for (int j = 0; j < 10; j++)
//				if (cache[j].id == i)
//					return cache[j];
//
//			cacheIndex = (cacheIndex + 1) % 10;
//			ItemDef itemDef = cache[cacheIndex];
//			stream.currentOffset = streamIndices[i];
//			itemDef.id = i;
//			itemDef.setDefaults();
//			itemDef.readValues(stream);
//			if (itemDef.certTemplateID != -1)
//				itemDef.toNote();
//			if (!isMembers && itemDef.membersObject)
//			{
//				itemDef.name = "Members Object";
//				itemDef.description = "Login to a members' server to use this object.";
//				itemDef.groundActions = null;
//				itemDef.actions = null;
//				itemDef.team = 0;
//			}
//			return itemDef;
//		}
//
//		private void toNote()
//		{
//			ItemDef itemDef = forID(certTemplateID);
//			modelID = itemDef.modelID;
//			modelZoom = itemDef.modelZoom;
//			modelRotation1 = itemDef.modelRotation1;
//			modelRotation2 = itemDef.modelRotation2;
//
//			anInt204 = itemDef.anInt204;
//			modelOffset1 = itemDef.modelOffset1;
//			modelOffset2 = itemDef.modelOffset2;
//			modifiedModelColors = itemDef.modifiedModelColors;
//			originalModelColors = itemDef.originalModelColors;
//			ItemDef itemDef_1 = forID(certID);
//			name = itemDef_1.name;
//			membersObject = itemDef_1.membersObject;
//			value = itemDef_1.value;
//			String s = "a";
//			char c = itemDef_1.name[0];
//			if (c == 'A' || c == 'E' || c == 'I' || c == 'O' || c == 'U')
//				s = "an";
//			description = "Swap this note at any bank for " + s + " " + itemDef_1.name + ".";
//			stackable = true;
//		}
//
//		public static Sprite getSprite(int i, int j, int k)
//		{
//			Sprite sprite = null;
//			if (k == 0)
//			{
//				sprite = (Sprite)mruNodes1.insertFromCache(i);
//				if (sprite != null && sprite.anInt1445 != j && sprite.anInt1445 != -1)
//				{
//					sprite.unlink();
//					sprite = null;
//				}
//				if (sprite != null)
//					return sprite;
//			}
//			ItemDef itemDef = forID(i);
//			if (itemDef.stackIDs == null)
//				j = -1;
//			if (j > 1)
//			{
//				int i1 = -1;
//				for (int j1 = 0; j1 < 10; j1++)
//					if (j >= itemDef.stackAmounts[j1] && itemDef.stackAmounts[j1] != 0)
//						i1 = itemDef.stackIDs[j1];
//
//				if (i1 != -1)
//					itemDef = forID(i1);
//			}
//			Model model = itemDef.method201(1);
//			if (model == null)
//				return null;
//			sprite = null;
//			if (itemDef.certTemplateID != -1)
//			{
//				sprite = getSprite(itemDef.certID, 10, -1);
//				if (sprite == null)
//					return null;
//			}
//			Sprite sprite2 = new Sprite(32, 32);
//			int k1 = Texture.center_x;
//			int l1 = Texture.center_y;
//			int[] ai = Texture.lineOffsets;
//			int[] ai1 = DrawingArea.pixels;
//			int i2 = DrawingArea.width;
//			int j2 = DrawingArea.height;
//			int k2 = DrawingArea.topX;
//			int l2 = DrawingArea.bottomX;
//			int i3 = DrawingArea.topY;
//			int j3 = DrawingArea.bottomY;
//			Texture.notTextured = false;
//			DrawingArea.initDrawingArea(32, 32, sprite2.myPixels);
//			DrawingArea.method336(32, 0, 0, 0, 32);
//			Texture.method364();
//			int k3 = itemDef.modelZoom;
//			if (k == -1)
//				k3 = (int)((double)k3 * 1.5D);
//			if (k > 0)
//				k3 = (int)((double)k3 * 1.04D);
//			int l3 = Texture.SINE[itemDef.modelRotation1] * k3 >> 16;
//			int i4 = Texture.COSINE[itemDef.modelRotation1] * k3 >> 16;
//			//model.method482(itemDef.modelRotation2, itemDef.anInt204, itemDef.modelRotation1, itemDef.modelOffset1, l3 + model.modelHeight / 2 + itemDef.modelOffset2, i4 + itemDef.modelOffset2);
//			for (int i5 = 31; i5 >= 0; i5--)
//			{
//				for (int j4 = 31; j4 >= 0; j4--)
//					if (sprite2.myPixels[i5 + j4 * 32] == 0)
//						if (i5 > 0 && sprite2.myPixels[(i5 - 1) + j4 * 32] > 1)
//							sprite2.myPixels[i5 + j4 * 32] = 1;
//						else
//							if (j4 > 0 && sprite2.myPixels[i5 + (j4 - 1) * 32] > 1)
//								sprite2.myPixels[i5 + j4 * 32] = 1;
//							else
//								if (i5 < 31 && sprite2.myPixels[i5 + 1 + j4 * 32] > 1)
//									sprite2.myPixels[i5 + j4 * 32] = 1;
//								else
//									if (j4 < 31 && sprite2.myPixels[i5 + (j4 + 1) * 32] > 1)
//										sprite2.myPixels[i5 + j4 * 32] = 1;
//
//			}
//
//			if (k > 0)
//			{
//				for (int j5 = 31; j5 >= 0; j5--)
//				{
//					for (int k4 = 31; k4 >= 0; k4--)
//						if (sprite2.myPixels[j5 + k4 * 32] == 0)
//							if (j5 > 0 && sprite2.myPixels[(j5 - 1) + k4 * 32] == 1)
//								sprite2.myPixels[j5 + k4 * 32] = k;
//							else
//								if (k4 > 0 && sprite2.myPixels[j5 + (k4 - 1) * 32] == 1)
//									sprite2.myPixels[j5 + k4 * 32] = k;
//								else
//									if (j5 < 31 && sprite2.myPixels[j5 + 1 + k4 * 32] == 1)
//										sprite2.myPixels[j5 + k4 * 32] = k;
//									else
//										if (k4 < 31 && sprite2.myPixels[j5 + (k4 + 1) * 32] == 1)
//											sprite2.myPixels[j5 + k4 * 32] = k;
//
//				}
//
//			}
//			else
//				if (k == 0)
//				{
//					for (int k5 = 31; k5 >= 0; k5--)
//					{
//						for (int l4 = 31; l4 >= 0; l4--)
//							if (sprite2.myPixels[k5 + l4 * 32] == 0 && k5 > 0 && l4 > 0 && sprite2.myPixels[(k5 - 1) + (l4 - 1) * 32] > 0)
//								sprite2.myPixels[k5 + l4 * 32] = 0x302020;
//
//					}
//
//				}
//			if (itemDef.certTemplateID != -1)
//			{
//				int l5 = sprite.anInt1444;
//				int j6 = sprite.anInt1445;
//				sprite.anInt1444 = 32;
//				sprite.anInt1445 = 32;
//				sprite.drawSprite(0, 0);
//				sprite.anInt1444 = l5;
//				sprite.anInt1445 = j6;
//			}
//			if (k == 0)
//				mruNodes1.removeFromCache(sprite2, i);
//			DrawingArea.initDrawingArea(j2, i2, ai1);
//			DrawingArea.setDrawingArea(j3, k2, l2, i3);
//			Texture.center_x = k1;
//			Texture.center_y = l1;
//			Texture.lineOffsets = ai;
//			Texture.notTextured = true;
//			if (itemDef.stackable)
//				sprite2.anInt1444 = 33;
//			else
//				sprite2.anInt1444 = 32;
//			sprite2.anInt1445 = j;
//			return sprite2;
//		}
//
//		public Model method201(int i)
//		{
//			if (stackIDs != null && i > 1)
//			{
//				int j = -1;
//				for (int k = 0; k < 10; k++)
//					if (i >= stackAmounts[k] && stackAmounts[k] != 0)
//						j = stackIDs[k];
//
//				if (j != -1)
//					return forID(j).method201(1);
//			}
//			Model model = (Model)mruNodes2.insertFromCache(id);
//			if (model != null)
//				return model;
//			model = Model.method462(modelID);
//			if (model == null)
//				return null;
//			if (anInt167 != 128 || anInt192 != 128 || anInt191 != 128)
//				model.method478(anInt167, anInt191, anInt192);
//			if (modifiedModelColors != null)
//			{
//				for (int l = 0; l < modifiedModelColors.Length; l++)
//					model.method476(modifiedModelColors[l], originalModelColors[l]);
//
//			}
//			model.method479(64 + anInt196, 768 + anInt184, -50, -10, -50, true);
//			model.aBoolean1659 = true;
//			mruNodes2.removeFromCache(model, id);
//			return model;
//		}
//
//		public Model method202(int i)
//		{
//			if (stackIDs != null && i > 1)
//			{
//				int j = -1;
//				for (int k = 0; k < 10; k++)
//					if (i >= stackAmounts[k] && stackAmounts[k] != 0)
//						j = stackIDs[k];
//
//				if (j != -1)
//					return forID(j).method202(1);
//			}
//			Model model = Model.method462(modelID);
//			if (model == null)
//				return null;
//			if (modifiedModelColors != null)
//			{
//				for (int l = 0; l < modifiedModelColors.Length; l++)
//					model.method476(modifiedModelColors[l], originalModelColors[l]);
//
//			}
//			return model;
//		}
//
//		private void readValues(Stream stream)
//		{
//			do
//			{
//				int i = stream.readUnsignedByte();
//				if (i == 0)
//					return;
//				if (i == 1)
//					modelID = stream.readUnsignedWord();
//				else
//					if (i == 2)
//						name = stream.readString();
//					else
//						if (i == 3)
//						description = stream.readString();//gstrbyte();
//						else
//							if (i == 4)
//								modelZoom = stream.readUnsignedWord();
//							else
//								if (i == 5)
//									modelRotation1 = stream.readUnsignedWord();
//								else
//									if (i == 6)
//										modelRotation2 = stream.readUnsignedWord();
//									else
//										if (i == 7)
//										{
//											modelOffset1 = stream.readUnsignedWord();
//											if (modelOffset1 > 32767)
//												modelOffset1 -= 0x10000;
//										}
//										else
//											if (i == 8)
//											{
//												modelOffset2 = stream.readUnsignedWord();
//												if (modelOffset2 > 32767)
//													modelOffset2 -= 0x10000;
//											}
//											else
//												if (i == 10)
//													stream.readUnsignedWord();
//												else
//													if (i == 11)
//														stackable = true;
//													else
//														if (i == 12)
//															value = stream.readDWord();
//														else
//															if (i == 16)
//																membersObject = true;
//															else
//																if (i == 23)
//																{
//																	anInt165 = stream.readUnsignedWord();
//																	aByte205 = (sbyte)stream.readSignedByte();
//																}
//																else
//																	if (i == 24)
//																		anInt188 = stream.readUnsignedWord();
//																	else
//																		if (i == 25)
//																		{
//																			anInt200 = stream.readUnsignedWord();
//																			aByte154 = (sbyte)stream.readSignedByte();
//																		}
//																		else
//																			if (i == 26)
//																				anInt164 = stream.readUnsignedWord();
//																			else
//																				if (i >= 30 && i < 35)
//																				{
//																					if (groundActions == null)
//																						groundActions = new String[5];
//																					groundActions[i - 30] = stream.readString();
//																					if (groundActions[i - 30].ToLower().Equals("hidden"))
//																						groundActions[i - 30] = null;
//																				}
//																				else
//																					if (i >= 35 && i < 40)
//																					{
//																						if (actions == null)
//																							actions = new String[5];
//																						actions[i - 35] = stream.readString();
//																					}
//																					else
//																						if (i == 40)
//																						{
//																							int j = stream.readUnsignedByte();
//																							modifiedModelColors = new int[j];
//																							originalModelColors = new int[j];
//																							for (int k = 0; k < j; k++)
//																							{
//																								modifiedModelColors[k] = stream.readUnsignedWord();
//																								originalModelColors[k] = stream.readUnsignedWord();
//																							}
//
//																						}
//																						else
//																							if (i == 78)
//																								anInt185 = stream.readUnsignedWord();
//																							else
//																								if (i == 79)
//																									anInt162 = stream.readUnsignedWord();
//																								else
//																									if (i == 90)
//																										anInt175 = stream.readUnsignedWord();
//																									else
//																										if (i == 91)
//																											anInt197 = stream.readUnsignedWord();
//																										else
//																											if (i == 92)
//																												anInt166 = stream.readUnsignedWord();
//																											else
//																												if (i == 93)
//																													anInt173 = stream.readUnsignedWord();
//																												else
//																													if (i == 95)
//																														anInt204 = stream.readUnsignedWord();
//																													else
//																														if (i == 97)
//																															certID = stream.readUnsignedWord();
//																														else
//																															if (i == 98)
//																																certTemplateID = stream.readUnsignedWord();
//																															else
//																																if (i >= 100 && i < 110)
//																																{
//																																	if (stackIDs == null)
//																																	{
//																																		stackIDs = new int[10];
//																																		stackAmounts = new int[10];
//																																	}
//																																	stackIDs[i - 100] = stream.readUnsignedWord();
//																																	stackAmounts[i - 100] = stream.readUnsignedWord();
//																																}
//																																else
//																																	if (i == 110)
//																																		anInt167 = stream.readUnsignedWord();
//																																	else
//																																		if (i == 111)
//																																			anInt192 = stream.readUnsignedWord();
//																																		else
//																																			if (i == 112)
//																																				anInt191 = stream.readUnsignedWord();
//																																			else
//																																				if (i == 113)
//																																					anInt196 = stream.readSignedByte();
//																																				else
//																																					if (i == 114)
//																																						anInt184 = stream.readSignedByte() * 5;
//																																					else
//																																						if (i == 115)
//																																							team = stream.readUnsignedByte();
//				else
//					if (i == 121)
//						lendID = stream.readUnsignedWord ();
//				else
//					if (i == 122)
//						lendTemplateID = stream.readUnsignedWord ();
//			} while (true);
//		}
//
//		private ItemDef()
//		{
//			id = -1;
//		}
//		int lendID = 0;
//		int lendTemplateID = 0;
//		private sbyte aByte154;
//		public int value;
//		private int[] modifiedModelColors;
//		public int id;
//		public static MRUNodes mruNodes1 = new MRUNodes(100);
//		public static MRUNodes mruNodes2 = new MRUNodes(50);
//		private int[] originalModelColors;
//		public bool membersObject;
//		private int anInt162;
//		private int certTemplateID;
//		private int anInt164;
//		private int anInt165;
//		private int anInt166;
//		private int anInt167;
//		public String[] groundActions;
//		private int modelOffset1;
//		public String name;
//		private static ItemDef[] cache;
//		private int anInt173;
//		private int modelID;
//		private int anInt175;
//		public bool stackable;
//		public string description;
//		private int certID;
//		private static int cacheIndex;
//		public int modelZoom;
//		public static bool isMembers = true;
//		private static Stream stream;
//		private int anInt184;
//		private int anInt185;
//		private int anInt188;
//		public String[] actions;
//		public int modelRotation1;
//		private int anInt191;
//		private int anInt192;
//		private int[] stackIDs;
//		private int modelOffset2;
//		private static int[] streamIndices;
//		private int anInt196;
//		private int anInt197;
//		public int modelRotation2;
//		private int anInt200;
//		private int[] stackAmounts;
//		public int team;
//		public static int totalItems;
//		private int anInt204;
//		private sbyte aByte205;

		
		public static void nullLoader() {
			mruNodes2 = null;
			mruNodes1 = null;
			streamIndices = null;
			cache = null;
			stream = null;
		}
		
		public bool method192(int j) {
			int k = anInt175;
			int l = anInt166;
			if (j == 1) {
				k = anInt197;
				l = anInt173;
			}
			if (k == -1)
				return true;
			bool flag = true;
			if (!Model.method463(k))
				flag = false;
			if (l != -1 && !Model.method463(l))
				flag = false;
			return flag;
		}
		
		public Model method194(int j) {
			int k = anInt175;
			int l = anInt166;
			if (j == 1) {
				k = anInt197;
				l = anInt173;
			}
			if (k == -1)
				return null;
			Model model = Model.method462(k);
			if (l != -1) {
				Model model_1 = Model.method462(l);
				Model[] models = { model, model_1 };
				model = new Model(2, models);
			}
			if (editedModelColor != null) {
				for (int i1 = 0; i1 < editedModelColor.Length; i1++)
					model.method476(editedModelColor[i1], newModelColor[i1]);
			}
			return model;
		}
		
		public bool method195(int j) {
			int k = maleEquip1;
			int l = maleEquip2;
			int i1 = anInt185;
			if (j == 1) {
				k = femaleEquip1;
				l = femaleEquip2;
				i1 = anInt162;
			}
			if (k == -1)
				return true;
			bool flag = true;
			if (!Model.method463(k))
				flag = false;
			if (l != -1 && !Model.method463(l))
				flag = false;
			if (i1 != -1 && !Model.method463(i1))
				flag = false;
			return flag;
		}
		
		public Model method196(int i) {
			int j = maleEquip1;
			int k = maleEquip2;
			int l = anInt185;
			if (i == 1) {
				j = femaleEquip1;
				k = femaleEquip2;
				l = anInt162;
			}
			if (j == -1)
				return null;
			Model model = Model.method462(j);
			if (k != -1)
			if (l != -1) {
				Model model_1 = Model.method462(k);
				Model model_3 = Model.method462(l);
				Model[] model_1s = { model, model_1, model_3 };
				model = new Model(3, model_1s);
			} else {
				Model model_2 = Model.method462(k);
				Model[] models = { model, model_2 };
				model = new Model(2, models);
			}
			if (i == 0 && aByte205 != 0)
				model.method475(0, aByte205, 0);
			if (i == 1 && aByte154 != 0)
				model.method475(0, aByte154, 0);
			if (editedModelColor != null) {
				for (int i1 = 0; i1 < editedModelColor.Length; i1++)
					model.method476(editedModelColor[i1], newModelColor[i1]);
			}
			return model;
		}
		
		public void setDefaults() {
			modelID = 0;
			name = null;
			description = null;
			editedModelColor = null;
			newModelColor = null;
			modelZoom = 2000;
			modelRotation1 = 0;
			modelRotation2 = 0;
			anInt204 = 0;
			modelOffset1 = 0;
			modelOffset2 = 0;
			stackable = false;
			value = 1;
			membersObject = false;
			groundActions = null;
			actions = null;
			maleEquip1 = -1;
			maleEquip2 = -1;
			aByte205 = 0;
			femaleEquip1 = -1;
			femaleEquip2 = -1;
			aByte154 = 0;
			anInt185 = -1;
			anInt162 = -1;
			anInt175 = -1;
			anInt166 = -1;
			anInt197 = -1;
			anInt173 = -1;
			stackIDs = null;
			stackAmounts = null;
			certID = -1;
			certTemplateID = -1;
			anInt167 = 128;
			anInt192 = 128;
			anInt191 = 128;
			anInt196 = 0;
			anInt184 = 0;
			team = 0;
			lendID = -1;
			lentItemID = -1;
		}
		
		public static void unpackConfig(StreamLoader streamLoader) {
			stream = new Stream(streamLoader.getDataForName("obj.dat"));
			Stream stream2 = new Stream(streamLoader.getDataForName("obj.idx"));
			totalItems = stream2.readUnsignedWord();
			streamIndices = new int[totalItems+10000];
			Debug.Log("Items Loaded: "+totalItems);
			int i = 2;
			for (int j = 0; j < totalItems; j++) {
				streamIndices[j] = i;
				i += stream2.readUnsignedWord();
			}
			cache = new ItemDef[10];
			for (int k = 0; k < 10; k++)
				cache[k] = new ItemDef();
		}
		
		public static ItemDef forID(int i) {
			for (int j = 0; j < 10; j++)
				if (cache[j].ID == i)
					return cache[j];
			cacheIndex = (cacheIndex + 1) % 10;
			ItemDef itemDef = cache[cacheIndex];
			stream.currentOffset = streamIndices[i];
			itemDef.ID = i;
			itemDef.setDefaults();
			itemDef.readValues(stream);
			if (itemDef.editedModelColor != null) {
				int[] oldc = itemDef.editedModelColor;
				int[] newc = itemDef.newModelColor;
				itemDef.editedModelColor = new int[oldc.Length + 1];
				itemDef.newModelColor = new int[newc.Length + 1];
				for (int index = 0; index < itemDef.editedModelColor.Length; index++) {
					if (index < itemDef.editedModelColor.Length - 1) {
						itemDef.editedModelColor[index] = oldc[index];
						itemDef.newModelColor[index] = newc[index];
					} else {
						itemDef.editedModelColor[index] = 0;
						itemDef.newModelColor[index] = 1;
					}
				}
			} else {
				itemDef.editedModelColor = new int[1];
				itemDef.newModelColor = new int[1];
				itemDef.editedModelColor[0] = 0;
				itemDef.newModelColor[0] = 1;
			}
			if (itemDef.certTemplateID != -1)
				itemDef.toNote();
			if (itemDef.lentItemID != -1)
				itemDef.toLend();
			if (!isMembers && itemDef.membersObject) {
				itemDef.name = "Members Object";
				itemDef.description = "Login to a members' server to use this object.";
				itemDef.groundActions = null;
				itemDef.actions = null;
				itemDef.team = 0;
			}
			if (itemDef.editedModelColor != null) {
				int[] oldc = itemDef.editedModelColor;
				int[] newc = itemDef.newModelColor;
				itemDef.editedModelColor = new int[oldc.Length + 1];
				itemDef.newModelColor = new int[newc.Length + 1];
				for (int index = 0; index < itemDef.editedModelColor.Length; index++) {
					if (index < itemDef.editedModelColor.Length - 1) {
						itemDef.editedModelColor[index] = oldc[index];
						itemDef.newModelColor[index] = newc[index];
					} else {
						itemDef.editedModelColor[index] = 0;
						itemDef.newModelColor[index] = 1;
					}
				}
			} else {
				itemDef.editedModelColor = new int[1];
				itemDef.newModelColor = new int[1];
				itemDef.editedModelColor[0] = 0;
				itemDef.newModelColor[0] = 1;
			}
			return itemDef;
		}
		
		private void readValues(Stream stream)
		{
			do
			{
				int i = stream.readUnsignedByte();
				if(i == 0)
					return;
				if(i == 1)
					modelID = stream.readUnsignedWord();
				else
					if(i == 2)
						name = stream.readString();
				else
					if(i == 3)
						description = stream.readString();
				else
					if(i == 4)
						modelZoom = stream.readUnsignedWord();
				else
					if(i == 5)
						modelRotation1 = stream.readUnsignedWord();
				else
					if(i == 6)
						modelRotation2 = stream.readUnsignedWord();
				else
					if(i == 7)
				{
					modelOffset1 = stream.readUnsignedWord();
					if(modelOffset1 > 32767)
						modelOffset1 -= 0x10000;
				} else
					if(i == 8)
				{
					modelOffset2 = stream.readUnsignedWord();
					if(modelOffset2 > 32767)
						modelOffset2 -= 0x10000;
				} else
					if(i == 10)
						stream.readUnsignedWord();
				else
					if(i == 11)
						stackable = true;
				else
					if(i == 12)
						value = stream.readDWord();
				else
					if(i == 16)
						membersObject = true;
				else if(i == 23) {
					maleEquip1 = stream.readUnsignedWord();
					aByte205 = (byte)stream.readSignedByte();
				} else if(i == 24)
					maleEquip2 = stream.readUnsignedWord();
				else if(i == 25) {
					femaleEquip1 = stream.readUnsignedWord();
					aByte154 = (byte)stream.readSignedByte();
				} else if(i == 26)
					femaleEquip2 = stream.readUnsignedWord();
				else
					if(i >= 30 && i < 35)
				{
					if(groundActions == null)
						groundActions = new String[5];
					groundActions[i - 30] = stream.readString();
					if(groundActions[i - 30].ToLower().Equals("hidden"))
						groundActions[i - 30] = null;
				} else
					if(i >= 35 && i < 40)
				{
					if(actions == null)
						actions = new String[5];
					actions[i - 35] = stream.readString();
				} else if (i == 40) {
					int j = stream.readUnsignedByte();
					editedModelColor = new int[j];
					newModelColor = new int[j];
					for (int k = 0; k < j; k++) {
						editedModelColor[k] = stream.readUnsignedWord();
						newModelColor[k] = stream.readUnsignedWord();
					}
					
				} else
					if(i == 78)
						anInt185 = stream.readUnsignedWord();
				else
					if(i == 79)
						anInt162 = stream.readUnsignedWord();
				else
					if(i == 90)
						anInt175 = stream.readUnsignedWord();
				else
					if(i == 91)
						anInt197 = stream.readUnsignedWord();
				else
					if(i == 92)
						anInt166 = stream.readUnsignedWord();
				else
					if(i == 93)
						anInt173 = stream.readUnsignedWord();
				else
					if(i == 95)
						anInt204 = stream.readUnsignedWord();
				else
					if(i == 97)
						certID = stream.readUnsignedWord();
				else
					if(i == 98)
						certTemplateID = stream.readUnsignedWord();
				else
					if(i >= 100 && i < 110)
				{
					if(stackIDs == null)
					{
						stackIDs = new int[10];
						stackAmounts = new int[10];
					}
					stackIDs[i - 100] = stream.readUnsignedWord();
					stackAmounts[i - 100] = stream.readUnsignedWord();
				} else
					if(i == 110)
						anInt167 = stream.readUnsignedWord();
				else
					if(i == 111)
						anInt192 = stream.readUnsignedWord();
				else
					if(i == 112)
						anInt191 = stream.readUnsignedWord();
				else
					if(i == 113)
						anInt196 = stream.readSignedByte();
				else
					if(i == 114)
						anInt184 = stream.readSignedByte() * 5;
				else
					if(i == 115)
						team = stream.readUnsignedByte();
				else
					if(i == 121)
						lendID = stream.readUnsignedWord();
				else
					if(i == 122)
						lendTemplateID = stream.readUnsignedWord();
			} while(true);
		}
		
		public void toNote() {
			ItemDef itemDef = forID(certTemplateID);
			modelID = itemDef.modelID;
			modelZoom = itemDef.modelZoom;
			modelRotation1 = itemDef.modelRotation1;
			modelRotation2 = itemDef.modelRotation2;
			anInt204 = itemDef.anInt204;
			modelOffset1 = itemDef.modelOffset1;
			modelOffset2 = itemDef.modelOffset2;
			editedModelColor = itemDef.editedModelColor;
			newModelColor = itemDef.newModelColor;
			ItemDef itemDef_1 = forID(certID);
			name = itemDef_1.name;
			membersObject = itemDef_1.membersObject;
			value = itemDef_1.value;
			String s = "a";
			char c = itemDef_1.name[0];
			if (c == 'A' || c == 'E' || c == 'I' || c == 'O' || c == 'U')
				s = "an";
			description = ("Swap this note at any bank for " + s + " " + itemDef_1.name + ".");
			stackable = true;
		}
		
		private void toLend() {
			ItemDef itemDef = forID(lentItemID);
			actions = new String[5];
			modelID = itemDef.modelID;
			modelOffset1 = itemDef.modelOffset1;
			modelRotation2 = itemDef.modelRotation2;
			modelOffset2 = itemDef.modelOffset2;
			modelZoom = itemDef.modelZoom;
			modelRotation1 = itemDef.modelRotation1;
			anInt204 = itemDef.anInt204;
			value = 0;
			ItemDef itemDef_1 = forID(lendID);
			anInt166 = itemDef_1.anInt166;
			editedModelColor = itemDef_1.editedModelColor;
			anInt185 = itemDef_1.anInt185;
			maleEquip2 = itemDef_1.maleEquip2;
			anInt173 = itemDef_1.anInt173;
			anInt175 = itemDef_1.anInt175;
			groundActions = itemDef_1.groundActions;
			maleEquip1 = itemDef_1.maleEquip1;
			name = itemDef_1.name;
			femaleEquip1 = itemDef_1.femaleEquip1;
			membersObject = itemDef_1.membersObject;
			anInt197 = itemDef_1.anInt197;
			femaleEquip2 = itemDef_1.femaleEquip2;
			anInt162 = itemDef_1.anInt162;
			newModelColor = itemDef_1.newModelColor;
			team = itemDef_1.team;
			if (itemDef_1.actions != null) {
				for (int i_33_ = 0; i_33_ < 4; i_33_++)
					actions[i_33_] = itemDef_1.actions[i_33_];
			}
			actions[4] = "Discard";
		}
		
		public static Sprite getSprite(int i, int j, int k) {
			if (k == 0) {
				Sprite sprite = (Sprite) mruNodes1.insertFromCache(i);
				if (sprite != null && sprite.anInt1445 != j && sprite.anInt1445 != -1) {
					sprite.unlink();
					sprite = null;
				}
				if (sprite != null)
					return sprite;
			}
			ItemDef itemDef = forID(i);
			if (itemDef.stackIDs == null)
				j = -1;
			if (j > 1) {
				int i1 = -1;
				for (int j1 = 0; j1 < 10; j1++)
					if (j >= itemDef.stackAmounts[j1] && itemDef.stackAmounts[j1] != 0)
						i1 = itemDef.stackIDs[j1];
				if (i1 != -1)
					itemDef = forID(i1);
			}
			Model model = itemDef.method201(1);
			if (model == null)
				return null;
			Sprite sprite2 = null;
			if (itemDef.certTemplateID != -1) {
				sprite2 = getSprite(itemDef.certID, 10, -1);
				if (sprite2 == null)
					return null;
			}
			if (itemDef.lentItemID != -1) {
				sprite2 = getSprite(itemDef.lendID, 50, 0);
				if (sprite2 == null)
					return null;
			}
			Sprite sprite3 = new Sprite(32, 32);
			int k1 = Texture.center_x;
			int l1 = Texture.center_y;
			int[] ai = Texture.lineOffsets;
			int[] ai1 = DrawingArea.pixels;
			//float[] depthBuffer = DrawingArea.depthBuffer;
			int i2 = DrawingArea.width;
			int j2 = DrawingArea.height;
			int k2 = DrawingArea.topX;
			int l2 = DrawingArea.bottomX;
			int i3 = DrawingArea.topY;
			int j3 = DrawingArea.bottomY;
			Texture.notTextured = false;
			DrawingArea.initDrawingArea (32, 32, sprite3.myPixels);//, new float[32*32]);
			DrawingArea.drawPixels(32, 0, 0, 0, 32);
			Texture.method364();
			int k3 = itemDef.modelZoom;
			if (k == -1)
				k3 = (int) ((double) k3 * 1.5D);
			if (k > 0)
				k3 = (int) ((double) k3 * 1.04D);
			int l3 = Texture.SINE[itemDef.modelRotation1] * k3 >> 16;
			int i4 = Texture.COSINE[itemDef.modelRotation1] * k3 >> 16;
			model.method482(itemDef.modelRotation2, itemDef.anInt204, itemDef.modelRotation1, itemDef.modelOffset1, l3 + model.modelHeight / 2 + itemDef.modelOffset2, i4 + itemDef.modelOffset2);
			for (int i5 = 31; i5 >= 0; i5--) {
				for (int j4 = 31; j4 >= 0; j4--)
					if (sprite3.myPixels[i5 + j4 * 32] == 0)
						if (i5 > 0 && sprite3.myPixels[(i5 - 1) + j4 * 32] > 1)
							sprite3.myPixels[i5 + j4 * 32] = 1;
				else if (j4 > 0 && sprite3.myPixels[i5 + (j4 - 1) * 32] > 1)
					sprite3.myPixels[i5 + j4 * 32] = 1;
				else if (i5 < 31 && sprite3.myPixels[i5 + 1 + j4 * 32] > 1)
					sprite3.myPixels[i5 + j4 * 32] = 1;
				else if (j4 < 31 && sprite3.myPixels[i5 + (j4 + 1) * 32] > 1)
					sprite3.myPixels[i5 + j4 * 32] = 1;
			}
			if (k > 0) {
				for (int j5 = 31; j5 >= 0; j5--) {
					for (int k4 = 31; k4 >= 0; k4--)
						if (sprite3.myPixels[j5 + k4 * 32] == 0)
							if (j5 > 0 && sprite3.myPixels[(j5 - 1) + k4 * 32] == 1)
								sprite3.myPixels[j5 + k4 * 32] = k;
					else if (k4 > 0 && sprite3.myPixels[j5 + (k4 - 1) * 32] == 1)
						sprite3.myPixels[j5 + k4 * 32] = k;
					else if (j5 < 31 && sprite3.myPixels[j5 + 1 + k4 * 32] == 1)
						sprite3.myPixels[j5 + k4 * 32] = k;
					else if (k4 < 31 && sprite3.myPixels[j5 + (k4 + 1) * 32] == 1)
						sprite3.myPixels[j5 + k4 * 32] = k;
				}
			} else if (k == 0) {
				for (int k5 = 31; k5 >= 0; k5--) {
					for (int l4 = 31; l4 >= 0; l4--)
						if (sprite3.myPixels[k5 + l4 * 32] == 0 && k5 > 0 && l4 > 0 && sprite3.myPixels[(k5 - 1) + (l4 - 1) * 32] > 0)
							sprite3.myPixels[k5 + l4 * 32] = 0x302020;
				}
			}
			if (itemDef.certTemplateID != -1) {
				int l5 = sprite2.anInt1444;
				int j6 = sprite2.anInt1445;
				sprite2.anInt1444 = 32;
				sprite2.anInt1445 = 32;
				sprite2.drawSprite(0, 0);
				sprite2.anInt1444 = l5;
				sprite2.anInt1445 = j6;
			}
			if (itemDef.lentItemID != -1) {
				int l5 = sprite2.anInt1444;
				int j6 = sprite2.anInt1445;
				sprite2.anInt1444 = 32;
				sprite2.anInt1445 = 32;
				sprite2.drawSprite(0, 0);
				sprite2.anInt1444 = l5;
				sprite2.anInt1445 = j6;
			}
			if (k == 0)
				mruNodes1.removeFromCache(sprite3, i);
			DrawingArea.initDrawingArea (j2, i2, ai1);//, depthBuffer);
			DrawingArea.setDrawingArea(j3, k2, l2, i3);
			Texture.center_x = k1;
			Texture.center_y = l1;
			Texture.lineOffsets = ai;
			Texture.notTextured = true;
			if (itemDef.stackable)
				sprite3.anInt1444 = 33;
			else
				sprite3.anInt1444 = 32;
			sprite3.anInt1445 = j;
			return sprite3;
		}
		
		public Model method201(int i) {
			if (stackIDs != null && i > 1) {
				int j = -1;
				for (int k = 0; k < 10; k++)
					if (i >= stackAmounts[k] && stackAmounts[k] != 0)
						j = stackIDs[k];
				if (j != -1)
					return forID(j).method201(1);
			}
			Model model = (Model) mruNodes2.insertFromCache(ID);
			if (model != null)
				return model;
			model = Model.method462(modelID);
			if (model == null)
				return null;
			if (anInt167 != 128 || anInt192 != 128 || anInt191 != 128)
				model.method478(anInt167, anInt191, anInt192);
			if (editedModelColor != null) {
				for (int l = 0; l < editedModelColor.Length; l++)
					model.method476(editedModelColor[l], newModelColor[l]);
			}
			model.method479(64 + anInt196, 768 + anInt184, -50, -10, -50, true);
			model.aBoolean1659 = true;
			mruNodes2.removeFromCache(model, ID);
			return model;
		}
		
		public Model method202(int i) {
			if (stackIDs != null && i > 1) {
				int j = -1;
				for (int k = 0; k < 10; k++)
					if (i >= stackAmounts[k] && stackAmounts[k] != 0)
						j = stackIDs[k];
				if (j != -1)
					return forID(j).method202(1);
			}
			Model model = Model.method462(modelID);
			if (model == null)
				return null;
			if (editedModelColor != null) {
				for (int l = 0; l < editedModelColor.Length; l++)
					model.method476(editedModelColor[l], newModelColor[l]);
			}
			return model;
		}
		
		public ItemDef() {
			ID = -1;
		}
		
		public byte aByte154;
		public int value;
		public int[] editedModelColor;
		public int ID;
		public static MRUNodes mruNodes1 = new MRUNodes(100);
		public static MRUNodes mruNodes2 = new MRUNodes(50);
		public int[] newModelColor;
		public bool membersObject;
		public int anInt162;
		public int certTemplateID;
		public int femaleEquip2;
		public int maleEquip1;
		public int anInt166;
		public int anInt167;
		public String[] groundActions;
		public int modelOffset1;
		public String name;
		public static ItemDef[] cache;
		public int anInt173;
		public int modelID;
		public int anInt175;
		public bool stackable;
		public String description;
		public int certID;
		public static int cacheIndex;
		public int modelZoom;
		public static bool isMembers = true;
		public static Stream stream;
		public int anInt184;
		public int anInt185;
		public int maleEquip2;
		public String[] actions;
		public int modelRotation1;
		public int anInt191;
		public int anInt192;
		public int[] stackIDs;
		public int modelOffset2;
		public static int[] streamIndices;
		public int anInt196;
		public int anInt197;
		public int modelRotation2;
		public int femaleEquip1;
		public int[] stackAmounts;
		public int team;
		public static int totalItems;
		public int anInt204;
		public byte aByte205;
		public int lendID;
		public int lentItemID;
		public int lendTemplateID;

	}
}
