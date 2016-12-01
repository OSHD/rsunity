// Decompiled by Jad v1.5.8f. Copyright 2001 Pavel Kouznetsov.
// Jad home page: http://www.kpdus.com/jad.html
// Decompiler options: packimports(3) 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using sign;
using System.Threading;
using RS2Sharp;
using UnityEngine;

public class client : RSApplet {
	public static bool newDamage = true;
	public int jVar = 0;
	public List<RuneObject> objectsToRender = new List<RuneObject>();
	public List<GameObject> objectsToDispose = new List<GameObject>();
	public static bool drawCanvas = true;
	public UnityClient unityClient;
	public static bool draw317 = false;
	public ObjectManager objectManager;
	public bool roofsOff = false;
	public string serverIP = "127.0.0.1";
	public int serverPort = 43594;
	public int xOffsetDebug = 10;
	private RS2Sharp.Sprite HPBarFull;
	private RS2Sharp.Sprite HPBarEmpty;
	private RS2Sharp.Sprite critHit;
	private RS2Sharp.Sprite bigHit;
	private RS2Sharp.Sprite oldHit;
	public int yOffsetDebug = 10;
	public bool currentlyStarting = true;
	public static bool finished = false;
	private static bool enableNewDamage = false;
	public int MapX, MapY;
	public RS2Sharp.Sprite[] cacheSprite;
	public static bool isEditor = true;//Application.isEditor;
	
	public String indexLocation(int cacheIndex, int index) {
		return signlink.findcachedir() + "index" + cacheIndex + "/"
			+ (index != -1 ? index + ".gz" : "");
	}
	
	public static OnDemandFetcher onDemandFetcher;
	public bool
		isPoisoned,
		clickedQuickPrayers;
	public bool 
		summOrb = true,
		runEnergy = false;
	private void drawMinimapHovers() {
		hpHover = base.mouseX >= 690 && base.mouseX <= 745 && base.mouseY >= 13 && base.mouseY < 47;
		prayHover = base.mouseX >= 704 && base.mouseX <= 760 && base.mouseY >= 49 && base.mouseY < 83;
		runHover = base.mouseX >= 704 && base.mouseX <= 760 && base.mouseY >= 88 && base.mouseY < 120;
		logHover = base.mouseX >= 744 && base.mouseX <= 765 && base.mouseY >= 3 && base.mouseY <= 25;
		worldHover = base.mouseX >= 522 && base.mouseX <= 561 && base.mouseY >= 124 && base.mouseY < 161;
		sumHover = base.mouseX > 687 && base.mouseX < 744 && base.mouseY > 133 && base.mouseY < 164;
		counterHover = base.mouseX >= 520 && base.mouseX <= 554 && base.mouseY >= 47 && base.mouseY <= 81;
	}
	private bool
		runHover, prayHover, hpHover, prayClicked, logHover, 
		worldHover, counterOn, sumHover, sumActive, counterHover, 
		runClicked = true;
	
	public int getOrbTextColor(int statusInt){
		if(statusInt >= 75 && statusInt <= Int32.MaxValue)
			return 0x00FF00;
		else if(statusInt >= 50 && statusInt <= 74)
			return 0xFFFF00;
		else if(statusInt >= 25 && statusInt <= 49)
			return 0xFF981F;
		else
			return 0xFF0000;
	}
	
	public int getOrbFill(int statusInt){
		if(statusInt <= Int32.MaxValue && statusInt >= 97)
			return 0; 
		else if(statusInt <= 96 && statusInt >= 93)
			return 1;
		else if(statusInt <= 92 && statusInt >= 89)
			return 2;
		else if(statusInt <= 88 && statusInt >= 85)
			return 3;
		else if(statusInt <= 84 && statusInt >= 81)
			return 4;
		else if(statusInt <= 80 && statusInt >= 77)
			return 5;
		else if(statusInt <= 76 && statusInt >= 73)
			return 6;
		else if(statusInt <= 72 && statusInt >= 69)
			return 7;
		else if(statusInt <= 68 && statusInt >= 65)
			return 8;
		else if(statusInt <= 64 && statusInt >= 61)
			return 9;
		else if(statusInt <= 60 && statusInt >= 57)
			return 10;
		else if(statusInt <= 56 && statusInt >= 53)
			return 11;
		else if(statusInt <= 52 && statusInt >= 49)
			return 12;
		else if(statusInt <= 48 && statusInt >= 45)
			return 13;
		else if(statusInt <= 44 && statusInt >= 41)
			return 14;
		else if(statusInt <= 40 && statusInt >= 37)
			return 15;
		else if(statusInt <= 36 && statusInt >= 33)
			return 16;
		else if(statusInt <= 32 && statusInt >= 29)
			return 17;
		else if(statusInt <= 28 && statusInt >= 25)
			return 18;
		else if(statusInt <= 24 && statusInt >= 21)
			return 19;
		else if(statusInt <= 20 && statusInt >= 17)
			return 20;
		else if(statusInt <= 16 && statusInt >= 13)
			return 21;
		else if(statusInt <= 12 && statusInt >= 9)
			return 22;
		else if(statusInt <= 8 && statusInt >= 7)
			return 23;
		else if(statusInt <= 6 && statusInt >= 5)
			return 24;
		else if(statusInt <= 4 && statusInt >= 3)
			return 25;
		else if(statusInt <= 2 && statusInt >= 1)
			return 26;
		else if(statusInt <= 0)
			return 27;
		return 0;
	}	
	
	
	public int[] 
	orbX = {174, 187, 187, 174},
	orbY = {14, 53, 92, 127},
	orbTextX = {215, 228, 228, 215},
	orbTextY = {40, 79, 118, 153},
	coloredOrbX = {177, 190, 190, 177},
	coloredOrbY = {17, 56, 95, 130},
	currentInterface = {4016, 4012, 149, 4030},
	maximumInterface = {4017, 4013, 149, 4031};
	
	private void loadAllOrbs() {
		int[] 
		spriteID = {isPoisoned && hpHover ? 34 : 35, prayHover ? 34 : 35, runHover ? 34 : 35, sumActive && sumHover ? 34 : 35}, 
		coloredOrbSprite = {38, clickedQuickPrayers ? 36 : 37, runClicked ? 39 : 40, sumActive ? 41 : 42}, 
		orbIconX = {183, 194, 197, 183}, orbIconY = {24, 60, 99, 136}, orbSprite = {43, 44, runClicked ? 46 : 45, 47};
		String cEnergy = RSInterface.interfaceCache[149].message.Replace("%", "");
		String hp = RSInterface.interfaceCache[4016].message.Replace("%", "");
		int currentHP = Int32.Parse(hp), currentEnergy = Int32.Parse(cEnergy);
		for (int i = 0; i < (summOrb ? 4 : 3); i++) {
			String currentStats = RSInterface.interfaceCache[currentInterface[i]].message.Replace("%", ""), maxStats = RSInterface.interfaceCache[maximumInterface[i]].message.Replace("%", "");
			int currentLevel = Int32.Parse(currentStats), maxLevel = Int32.Parse(maxStats), level = (int)(((double) currentLevel / (double) maxLevel) * 100D);
			cacheSprite[spriteID[i]].drawSprite(orbX[i], orbY[i]);
			cacheSprite[coloredOrbSprite[i]].drawSprite(coloredOrbX[i], coloredOrbY[i]);
			double percent = (i == 2 ? runEnergy ? currentEnergy / 100D : 100 : level / 100D), fillHp = 27 * percent, fillPrayer = 27 * percent, fillRun = 27 * percent, fillSumm = 27 * percent;
			double[] fill = {fillHp, fillPrayer, fillRun, fillSumm};
			int depleteFill = 27 - (int) fill[i];
			cacheSprite[48].myHeight = depleteFill;
			cacheSprite[48].drawSprite(coloredOrbX[i], coloredOrbY[i]);
			cacheSprite[orbSprite[i]].drawSprite(orbIconX[i], orbIconY[i]);
//			smallText.method382(getOrbTextColor(i == 2 ? runEnergy ? currentEnergy : 100 : level), orbTextX[i], "" + (i == 2 ? runEnergy ? cEnergy : 100 : i == 0 && Client.enableNewDamage  ? currentHP * 10 : RSInterface.interfaceCache[currentInterface[i]].message.Replace("%", "")), orbTextY[i], true);
		}
	}
	public void repackCacheIndex(int cacheIndex) {
		UnityEngine.Debug.Log("Started repacking index " + cacheIndex + ".");
		int indexLength = Directory.GetFiles(indexLocation(cacheIndex, -1)).Length;//new File(indexLocation(cacheIndex, -1)).listFiles().length;
		string[] file = Directory.GetFiles(indexLocation(cacheIndex, -1));//new File(indexLocation(cacheIndex, -1)).listFiles();
		//try {
		for (int index = 0; index < indexLength; index++) {
			int fileIndex = Int32.Parse(getFileNameWithoutExtension(file[index]));
			byte[] data = fileToByteArray(cacheIndex, fileIndex);
			if(data != null && data.Length > 0) {
				decompressors[cacheIndex].method234(data.Length, data, fileIndex);
				UnityEngine.Debug.Log("Repacked " + fileIndex + ".");
			} else {
				UnityEngine.Debug.Log("Unable to locate index " + fileIndex + ".");
			}
		}
		//} catch(Exception e) {
		//	UnityEngine.Debug.Log("Error packing cache index " + cacheIndex + ".");
		//}
		UnityEngine.Debug.Log("Finished repacking " + cacheIndex + ".");
	}
	
	public byte[] fileToByteArray(int cacheIndex, int index) {
		try {
			if (indexLocation(cacheIndex, index).Length <= 0 || indexLocation(cacheIndex, index) == null) {
				return null;
			}
			// 				File file = new File(indexLocation(cacheIndex, index));
			// 				byte[] fileData = new byte[(int)file.Length];
			// 				FileInputStream fis = new FileInputStream(file);
			// 				fis.read(fileData);
			// 				fis.close();
			// 				return fileData;
			return client.ReadAllBytes(indexLocation(cacheIndex, index));
		} catch(Exception e) {
			return null;
		}
	}

	
	private static String intToKOrMilLongName(int i) {
		String s = "" + i;
		for (int k = s.Length - 3; k > 0; k -= 3)
			s = s.Substring(0, k) + "," + s.Substring(k);
		if (s.Length > 8)
			s = "@gre@" + s.Substring(0, s.Length - 8) + " million @whi@("
				+ s + ")";
		else if (s.Length > 4)
			s = "@cya@" + s.Substring(0, s.Length - 4) + "K @whi@(" + s + ")";
		return " " + s;
	}
	
	private void stopMidi() {
		signlink.midifade = 0;
		signlink.midi = "stop";
	}
	
	private bool menuHasAddFriend(int j) {
		if (j < 0)
			return false;
		int k = menuActionID[j];
		if (k >= 2000)
			k -= 2000;
		return k == 337;
	}
	
	public void drawChannelButtons() {
		String[] text = { "On", "Friends", "Off", "Hide" };
		int[] disabledColor = { 65280, 0xffff00, 0xff0000, 65535 };
		switch (cButtonCPos) {
		case 0:
			chatButtons[1].drawSprite(5 + 1, myHeight - 23);
			break;
		case 1:
			chatButtons[1].drawSprite(62 + 10, myHeight - 23);
			break;
		case 2:
			chatButtons[1].drawSprite(119 + 19, myHeight - 23);
			break;
		case 3:
			chatButtons[1].drawSprite(176 + 28, myHeight - 23);
			break;
		case 4:
			chatButtons[1].drawSprite(233 + 37, myHeight - 23);
			break;
		case 5:
			chatButtons[1].drawSprite(290 + 46, myHeight - 23);
			break;
		case 6:
			chatButtons[1].drawSprite(347 + 55, myHeight - 23);
			break;
		}
		if (cButtonHPos == cButtonCPos) {
			switch (cButtonHPos) {
			case 0:
				chatButtons[2].drawSprite(5 + 1, myHeight - 23);
				break;
			case 1:
				chatButtons[2].drawSprite(62 + 10, myHeight - 23);
				break;
			case 2:
				chatButtons[2].drawSprite(119 + 19, myHeight - 23);
				break;
			case 3:
				chatButtons[2].drawSprite(176 + 28, myHeight - 23);
				break;
			case 4:
				chatButtons[2].drawSprite(233 + 37, myHeight - 23);
				break;
			case 5:
				chatButtons[2].drawSprite(290 + 46, myHeight - 23);
				break;
// 			case 7:
// 				chatButtons[2].drawSprite(347 + 55, myHeight - 23);
// 				break;
			case 6:
				chatButtons[3].drawSprite(404 + 64, myHeight - 23);
				break;
			}
		} else {
			switch (cButtonHPos) {
			case 0:
				chatButtons[2].drawSprite(5 + 1, myHeight - 23);
				break;
			case 1:
				chatButtons[2].drawSprite(62 + 10, myHeight - 23);
				break;
			case 2:
				chatButtons[2].drawSprite(119 + 19, myHeight - 23);
				break;
			case 3:
				chatButtons[2].drawSprite(176 + 28, myHeight - 23);
				break;
			case 4:
				chatButtons[2].drawSprite(233 + 37, myHeight - 23);
				break;
			case 5:
				chatButtons[2].drawSprite(290 + 46, myHeight - 23);
				break;
// 			case 7:
// 				chatButtons[2].drawSprite(347 + 55, myHeight - 23);
// 				break;
			case 6:
				chatButtons[3].drawSprite(404 + 64, myHeight - 23);
				break;
			}
		}
		try{
		smallText.method389(true, 425, 0xffffff, "Report Abuse",  myHeight - 7);//158);
		smallText.method389(true, 26+1, 0xffffff, "All", myHeight - 7);//158);
		smallText.method389(true, 76+11, 0xffffff, "Game", myHeight - 7);//158);
		smallText.method389(true, 131+20, 0xffffff, "Public", myHeight - 11);//154);
		smallText.method389(true, 184+29, 0xffffff, "Private", myHeight - 11);//154);
		smallText.method389(true, 249+38, 0xffffff, "Clan", myHeight - 11);//154);
		smallText.method389(true, 304+47, 0xffffff, "Trade", myHeight - 11);//154);
		//smallText.method389(true, 359+56, 0xffffff, " Duel", myHeight - 11);//154);
		smallText.drawChatInput(disabledColor[publicChatMode], 147+19,
		                    text[publicChatMode], myHeight - 3, true);//165, true);
		smallText.drawChatInput(disabledColor[privateChatMode], 203+29,
		                    text[privateChatMode], myHeight - 3, true);//165, true);
		smallText.drawChatInput(disabledColor[clanChatMode], 261+38,
		                    text[clanChatMode], myHeight - 3, true);//165, true);
		smallText.drawChatInput(disabledColor[tradeMode], 318+47, text[tradeMode],
		                    myHeight - 3, true);//165, true);
		}
		catch(Exception ex)
		{
			
		}
		//smallText.drawChatInput(65280, 374, "On", myHeight - 3, true);//165, true);
	}
	
	private void drawChatArea() {
	//try{ //lol
// 		aRSImageProducer_1166.initDrawingArea();
// 		RS2Sharp.Texture.lineOffsets = anIntArray1180;
// 		chatArea.drawSprite(0, 0);
// 		drawChannelButtons();
// 		TextDrawingArea textDrawingArea = aTextDrawingArea_1271;
// 		if (messagePromptRaised) {
// 			chatTextDrawingArea.drawText(0, aString1121, 60, 259);
// 			chatTextDrawingArea.drawText(128, promptInput + "*", 80, 259);
// 		} else if (inputDialogState == 1) {
// 			chatTextDrawingArea.drawText(0, "Enter amount:", 60, 259);
// 			chatTextDrawingArea.drawText(128, amountOrNameInput + "*", 80, 259);
// 		} else if (inputDialogState == 2) {
// 			chatTextDrawingArea.drawText(0, "Enter name:", 60, 259);
// 			chatTextDrawingArea.drawText(128, amountOrNameInput + "*", 80, 259);
// 		} else if (aString844 != null) {
// 			chatTextDrawingArea.drawText(0, aString844, 60, 259);
// 			chatTextDrawingArea.drawText(128, "Click to continue", 80, 259);
// 		} else if (backDialogID != -1) {
// 			drawInterface(0, 20, RSInterface.interfaceCache[backDialogID], 20);
// 		} else if (dialogID != -1) {
// 			drawInterface(0, 20, RSInterface.interfaceCache[dialogID], 20);
// 		} else {
// 			int j77 = -3;
// 			int j = 0;
// 			DrawingArea.setDrawingArea(122, 8, 497, 7);
// 			for (int k = 0; k < 500; k++)
// 			if (chatMessages[k] != null) {
// 				int chatType = chatTypes[k];
// 				int yPos = (70 - j77 * 14) + anInt1089 + 5;
// 				String s1 = chatNames[k];
// 				byte byte0 = 0;
// 				if (s1 != null && s1.StartsWith("@cr1@")) {
// 					s1 = s1.Substring(5);
// 					byte0 = 1;
// 				} else if (s1 != null && s1.StartsWith("@cr2@")) {
// 					s1 = s1.Substring(5);
// 					byte0 = 2;
// 				} else if (s1 != null && s1.StartsWith("@cr3@")) {
// 					s1 = s1.Substring(5);
// 					byte0 = 3;
// 				}
// 				if (chatType == 0) {
// 					if (chatTypeView == 5 || chatTypeView == 0) {
// 						if (yPos > 0 && yPos < 210)
// 							textDrawingArea.method389(false, 11, 0,
// 							                          chatMessages[k], yPos);// chat color
// 						// enabled
// 						j++;
// 						j77++;
// 					}
// 				}
// 				if ((chatType == 1 || chatType == 2)
// 				    && (chatType == 1 || publicChatMode == 0 || publicChatMode == 1
// 				    && isFriendOrSelf(s1))) {
// 					if (chatTypeView == 1 || chatTypeView == 0) {
// 						if (yPos > 0 && yPos < 210) {
// 							int xPos = 11;
// 							if (byte0 == 1) {
// 								modIcons[0].drawSprite(xPos + 1,
// 								                           yPos - 12);
// 								xPos += 14;
// 							} else if (byte0 == 2) {
// 								modIcons[1].drawSprite(xPos + 1,
// 								                           yPos - 12);
// 								xPos += 14;
// 							} else if (byte0 == 3) {
// 								modIcons[2].drawSprite(xPos + 1,
// 								                           yPos - 12);
// 								xPos += 14;
// 							}
// 							textDrawingArea.method385(0, s1 + ":", yPos,
// 							                          xPos);
// 							xPos += textDrawingArea.getTextWidth(s1) + 8;
// 							textDrawingArea.method389(false, xPos, 255,
// 							                          chatMessages[k], yPos);
// 						}
// 						j++;
// 						j77++;
// 					}
// 				}
// 				if ((chatType == 3 || chatType == 7)
// 				    && (splitPrivateChat == 0 || chatTypeView == 2)
// 				    && (chatType == 7 || privateChatMode == 0 || privateChatMode == 1
// 				    && isFriendOrSelf(s1))) {
// 					if (chatTypeView == 2 || chatTypeView == 0) {
// 						if (yPos > 0 && yPos < 210) {
// 							int k1 = 11;
// 							textDrawingArea.method385(0, "From", yPos, k1);
// 							k1 += textDrawingArea.getTextWidth("From ");
// 							if (byte0 == 1) {
// 								modIcons[0].drawSprite(k1, yPos - 12);
// 								k1 += 12;
// 							} else if (byte0 == 2) {
// 								modIcons[1].drawSprite(k1, yPos - 12);
// 								k1 += 12;
// 							} else if (byte0 == 3) {
// 								modIcons[2].drawSprite(k1, yPos - 12);
// 								k1 += 12;
// 							}
// 							textDrawingArea
// 								.method385(0, s1 + ":", yPos, k1);
// 							k1 += textDrawingArea.getTextWidth(s1) + 8;
// 							textDrawingArea.method385(0x800000,
// 							                          chatMessages[k], yPos, k1);
// 						}
// 						j++;
// 						j77++;
// 					}
// 				}
// 				if (chatType == 4
// 				    && (tradeMode == 0 || tradeMode == 1
// 				    && isFriendOrSelf(s1))) {
// 					if (chatTypeView == 3 || chatTypeView == 0) {
// 						if (yPos > 0 && yPos < 210)
// 							textDrawingArea.method385(0x800080, s1 + " "
// 							                          + chatMessages[k], yPos, 11);
// 						j++;
// 						j77++;
// 					}
// 				}
// 				if (chatType == 5 && splitPrivateChat == 0
// 				    && privateChatMode < 2) {
// 					if (chatTypeView == 2 || chatTypeView == 0) {
// 						if (yPos > 0 && yPos < 210)
// 							textDrawingArea.method385(0x800000,
// 							                          chatMessages[k], yPos, 11);
// 						j++;
// 						j77++;
// 					}
// 				}
// 				if (chatType == 6
// 				    && (splitPrivateChat == 0 || chatTypeView == 2)
// 				    && privateChatMode < 2) {
// 					if (chatTypeView == 2 || chatTypeView == 0) {
// 						if (yPos > 0 && yPos < 210) {
// 							textDrawingArea.method385(0, "To " + s1 + ":",
// 							                          yPos, 11);
// 							textDrawingArea.method385(0x800000,
// 							                          chatMessages[k], yPos,
// 							                          15 + textDrawingArea
// 							                          .getTextWidth("To :" + s1));
// 						}
// 						j++;
// 						j77++;
// 					}
// 				}
// 				if (chatType == 8
// 				    && (tradeMode == 0 || tradeMode == 1
// 				    && isFriendOrSelf(s1))) {
// 					if (chatTypeView == 3 || chatTypeView == 0) {
// 						if (yPos > 0 && yPos < 210)
// 							textDrawingArea.method385(0x7e3200, s1 + " "
// 							                          + chatMessages[k], yPos, 11);
// 						j++;
// 						j77++;
// 					}
// 					if (chatType == 11 && (clanChatMode == 0)) {
// 						if (chatTypeView == 11) {
// 							if (yPos > 0 && yPos < 210)
// 								textDrawingArea.method385(0x7e3200, s1
// 								                          + " " + chatMessages[k], yPos, 11);
// 							j++;
// 							j77++;
// 						}
// 						if (chatType == 12) {
// 							if (yPos > 0 && yPos < 110)
// 								textDrawingArea.method385(0x7e3200,
// 								                          chatMessages[k] + " @blu@" + s1,
// 								                          yPos, 11);
// 							j++;
// 						}
// 					}
// 				}
// 			}
// 			DrawingArea.defaultDrawingAreaSize();
// 			anInt1211 = j * 14 + 7 + 5;
// 			if (anInt1211 < 111)
// 				anInt1211 = 111;
// 			drawScrollbar(114, anInt1211 - anInt1089 - 113, 7, 496, anInt1211);
// 			String s;
// 			if (myPlayer != null && myPlayer.name != null)
// 				s = myPlayer.name;
// 			else
// 				s = TextClass.fixName(myUsername);
// 			textDrawingArea.method385(0, s + ":", 133, 11);
// 			textDrawingArea.drawChatInput(255,
// 			                              12 + textDrawingArea.getTextWidth(s + ": "), inputString
// 			                              + "*", 133, false);
// 			DrawingArea.method339(121, 0x807660, 506, 7);
// 		}
// 		if (menuOpen && menuScreenArea == 2) {
// 			drawMenu();
// 		}
// 		aRSImageProducer_1166.drawGraphics(338, base.graphics, 0);
// 		aRSImageProducer_1165.initDrawingArea();
// 		RS2Sharp.Texture.lineOffsets = anIntArray1182;

		RS2Sharp.Texture.lineOffsets = anIntArray1180;
		int yOffset = myHeight - 165;
		chatArea.drawSprite(0, 0 + yOffset);
		drawChannelButtons();
		TextDrawingArea textDrawingArea = aTextDrawingArea_1271;
		if(messagePromptRaised) {
			chatTextDrawingArea.drawText(0, aString1121, 60 + yOffset, 259);
			chatTextDrawingArea.drawText(128, promptInput + "*", 80 + yOffset, 259);
		} else if(inputDialogState == 1) {
			chatTextDrawingArea.drawText(0, "Enter amount:", 60 + yOffset, 259);
			chatTextDrawingArea.drawText(128, amountOrNameInput + "*", 80 + yOffset, 259);
		} else if(inputDialogState == 2) {
			chatTextDrawingArea.drawText(0, "Enter name:", 60 + yOffset, 259);
			chatTextDrawingArea.drawText(128, amountOrNameInput + "*", 80 + yOffset, 259);
		} else if(aString844 != null) {
			chatTextDrawingArea.drawText(0, aString844, 60 + yOffset, 259);
			chatTextDrawingArea.drawText(128, "Click to continue", 80 + yOffset, 259);
		} else if(backDialogID != -1) {
			drawInterface(0, 20, RSInterface.interfaceCache[backDialogID], 20 + yOffset);
		} else if(dialogID != -1) {
			drawInterface(0, 20, RSInterface.interfaceCache[dialogID], 20 + yOffset);
		} else {
			int j77 = -3;
			int j = 0;
			DrawingArea.setDrawingArea(122 + yOffset, 8, 497, 7 + yOffset);
			for(int k = 0; k < 500; k++)
			if(chatMessages[k] != null) {
				int chatType = chatTypes[k];
				int yPos = (70 - j77 * 14) + anInt1089 + 5;
				String s1 = chatNames[k];
				byte byte0 = 0;
				if(s1 != null && s1.StartsWith("@cr1@")) {
					s1 = s1.Substring(5);
					byte0 = 1;
				} else if(s1 != null && s1.StartsWith("@cr2@")) {
					s1 = s1.Substring(5);
					byte0 = 2;
				} else if(s1 != null && s1.StartsWith("@cr3@")) {
					s1 = s1.Substring(5);
					byte0 = 3;
				}
				if(chatType == 0) {
					if (chatTypeView == 5 || chatTypeView == 0) {
					if(yPos > 0 && yPos < 210)
						textDrawingArea.method389(false, 11, 0, chatMessages[k], yPos + yOffset);//chat color enabled
					j++;
					j77++;
					}
				}
				if((chatType == 1 || chatType == 2) && (chatType == 1 || publicChatMode == 0 || publicChatMode == 1 && isFriendOrSelf(s1))) {
					if (chatTypeView == 1 || chatTypeView == 0) {
						if(yPos > 0 && yPos < 210) {
							int xPos = 11;
							//modIcons[byte0 - 1].drawSprite(1 + xPos, yPos - 12 + yOffset);
							xPos += 14;
							textDrawingArea.method385(0, s1 + ":", yPos + yOffset, xPos);
							xPos += textDrawingArea.getTextWidth(s1) + 8;
							textDrawingArea.method389(false, xPos, 255, chatMessages[k], yPos + yOffset);
						}
						j++;
						j77++;
					}
				}
				if((chatType == 3 || chatType == 7) && (splitPrivateChat == 0 || chatTypeView == 2) && (chatType == 7 || privateChatMode == 0 || privateChatMode == 1 && isFriendOrSelf(s1))) {
					if (chatTypeView == 2 || chatTypeView == 0) {
						if(yPos > 0 && yPos < 210) {
							int k1 = 11;
							textDrawingArea.method385(0, "From", yPos + yOffset, k1);
							k1 += textDrawingArea.getTextWidth("From ");
							if(byte0 == 1) {
								//modIcons[0].drawSprite(k1, yPos - 12 + yOffset);
								k1 += 12;
							} else if(byte0 == 2) {
								//modIcons[1].drawSprite(k1, yPos - 12 + yOffset);
								k1 += 12;
							} else if(byte0 == 3) {
								//modIcons[2].drawSprite(k1, yPos - 12 + yOffset);
								k1 += 12;
							}
							textDrawingArea.method385(0, s1 + ":", yPos + yOffset, k1);
							k1 += textDrawingArea.getTextWidth(s1) + 8;
							textDrawingArea.method385(0x800000, chatMessages[k], yPos + yOffset, k1);
						}
						j++;
						j77++;
					}
				}
				if(chatType == 4 && (tradeMode == 0 || tradeMode == 1 && isFriendOrSelf(s1))) {
					if (chatTypeView == 3 || chatTypeView == 0) {
						if(yPos > 0 && yPos < 210)
							textDrawingArea.method385(0x800080, s1 + " " + chatMessages[k], yPos + yOffset, 11);
						j++;
						j77++;
					}
				}
				if(chatType == 5 && splitPrivateChat == 0 && privateChatMode < 2) {
					if (chatTypeView == 2 || chatTypeView == 0) {
						if(yPos > 0 && yPos < 210)
							textDrawingArea.method385(0x800000, chatMessages[k], yPos + yOffset, 11);
						j++;
						j77++;
					}
				}
				if(chatType == 6 && (splitPrivateChat == 0 || chatTypeView == 2) && privateChatMode < 2) {
					if (chatTypeView == 2 || chatTypeView == 0) {
						if(yPos > 0 && yPos < 210) {
							textDrawingArea.method385(0, "To " + s1 + ":", yPos + yOffset, 11);
							textDrawingArea.method385(0x800000, chatMessages[k], yPos + yOffset, 15 + textDrawingArea.getTextWidth("To :" + s1));
						}
					j++;
					j77++;
					}
				}
				if(chatType == 8 && (tradeMode == 0 || tradeMode == 1 && isFriendOrSelf(s1))) {
					if (chatTypeView == 3 || chatTypeView == 0) {
						if(yPos > 0 && yPos < 210)
							textDrawingArea.method385(0x7e3200, s1 + " " + chatMessages[k], yPos + yOffset, 11);
						j++;
						j77++;
					}
					if(chatType == 11 && (clanChatMode == 0)) {
						if (chatTypeView == 11) {
						if(yPos > 0 && yPos < 210)
							textDrawingArea.method385(0x7e3200, s1 + " " + chatMessages[k], yPos + yOffset, 11);
						j++;
						j77++;
					}
					if(chatType == 12) {
						if(yPos > 0 && yPos < 110)
							textDrawingArea.method385(0x7e3200, chatMessages[k] + " @blu@" + s1, yPos + yOffset, 11);
							j++;
						}
					}
				}
				if(chatType == 16) {
					int j2 = 40;
					int clanNameWidth = textDrawingArea.getTextWidth(clanname);
					if(chatTypeView == 11 || chatTypeView == 0) {
						if(yPos > 3 && yPos < 130)
							switch(chatRights[k]) {
								case 1:
									j2 += clanNameWidth;
									//modIcons[0].drawSprite(j2 - 18, yPos - 12 + yOffset);
									j2 += 14;
									break;
									
								case 2:
									j2 += clanNameWidth;
									//modIcons[1].drawSprite(j2 - 18, yPos - 12 + yOffset);
									j2 += 14;
									break;
									
								case 3:
									j2 += clanNameWidth;
									//modIcons[1].drawSprite(j2 - 18, yPos - 12 + yOffset);
									j2 += 14;
									break;
				
								default:
									j2 += clanNameWidth;
									break;
							}
							textDrawingArea.method385(0, "[", yPos + yOffset, 8);
							textDrawingArea.method385(255, ""+clanname+"", yPos + yOffset, 14);
							textDrawingArea.method385(0, "]", yPos + yOffset, clanNameWidth + 14);
							
							textDrawingArea.method385(0, chatNames[k]+":", yPos + yOffset, j2 - 17); //j2
							j2 += textDrawingArea.getTextWidth(chatNames[k]) + 7;
							textDrawingArea.method385(0x800000, chatMessages[k], yPos + yOffset, j2 - 16);//j2
							j++;
							j77++;
						}
					}
			}
			DrawingArea.defaultDrawingAreaSize();
			anInt1211 = j * 14 + 7 + 5;
			if(anInt1211 < 111)
				anInt1211 = 111;
			drawScrollbar(114, anInt1211 - anInt1089 - 113, 7 + yOffset, 496, anInt1211);
			String s;
			if(myPlayer != null && myPlayer.name != null)
				s = myPlayer.name;
			else
				s = TextClass.fixName(myUsername);
			textDrawingArea.method385(0, s + ":", 133 + yOffset, 11);
			textDrawingArea.drawChatInput(255, 12 + textDrawingArea.getTextWidth(s + ": "), inputString + "*", 133 + yOffset, false);
			DrawingArea.method339(121 + yOffset, 0x807660, 506, 7);
		}
		if(menuOpen) {
			drawMenu(0,  0);
		}

		RS2Sharp.Texture.lineOffsets = anIntArray1182;
		//}catch(Exception ex) {}
	}
	

	public void init() {
		nodeID = 10;
		portOff = 0;
		setHighMem();
		isMembers = true;
//		initClientFrame(503, 765);
	}

//	public void startRunnable(Runnable runnable, int i) {
//		if (i > 10)
//			i = 10;
//		if (signlink.mainapp != null) {
//			signlink.startthread(runnable, i);
//		} else {
//			base.startRunnable(runnable, i);
//		}
//	}
	
	public static Socket openSocket(int port) {
		return signlink.opensocket(port);
	}
	
	private void processMenuClick() {
		if (activeInterfaceType != 0)
			return;
		int j = base.clickMode3;
		if (spellSelected == 1 && base.saveClickX >= 516
		    && base.saveClickY >= 160 && base.saveClickX <= 765
		    && base.saveClickY <= 205)
			j = 0;
		if (menuOpen) {
			if (j != 1) {
				int k = base.mouseX;
				int j1 = base.mouseY;
				if (menuScreenArea == 0) {
					k -= 4;
					j1 -= 4;
				}
				if (menuScreenArea == 1) {
					k -= 519;
					j1 -= 168;
				}
				if (menuScreenArea == 2) {
					k -= 17;
					j1 -= 338;
				}
				if (menuScreenArea == 3) {
					k -= 515;
					j1 -= 0;
				}
				if (k < menuOffsetX - 10 || k > menuOffsetX + menuWidth + 10
				    || j1 < menuOffsetY - 10
				    || j1 > menuOffsetY + menuHeight + 10) {
					menuOpen = false;
					if (menuScreenArea == 1)
						needDrawTabArea = true;
					if (menuScreenArea == 2)
						inputTaken = true;
				}
			}
			if (j == 1) {
				int l = menuOffsetX;
				int k1 = menuOffsetY;
				int i2 = menuWidth;
				int k2 = base.saveClickX;
				int l2 = base.saveClickY;
				if (menuScreenArea == 0) {
					k2 -= 4;
					l2 -= 4;
				}
				if (menuScreenArea == 1) {
					k2 -= 519;
					l2 -= 168;
				}
				if (menuScreenArea == 2) {
					k2 -= 17;
					l2 -= 338;
				}
				if (menuScreenArea == 3) {
					k2 -= 515;
					l2 -= 0;
				}
				int i3 = -1;
				for (int j3 = 0; j3 < menuActionRow; j3++) {
					int k3 = k1 + 31 + (menuActionRow - 1 - j3) * 15;
					if (k2 > l && k2 < l + i2 && l2 > k3 - 13 && l2 < k3 + 3)
						i3 = j3;
				}
				if (i3 != -1)
					doAction(i3);
				menuOpen = false;
				if (menuScreenArea == 1)
					needDrawTabArea = true;
				if (menuScreenArea == 2) {
					inputTaken = true;
				}
			}
		} else {
			if (j == 1 && menuActionRow > 0) {
				int i1 = menuActionID[menuActionRow - 1];
				if (i1 == 632 || i1 == 78 || i1 == 867 || i1 == 431 || i1 == 53
				    || i1 == 74 || i1 == 454 || i1 == 539 || i1 == 493
				    || i1 == 847 || i1 == 447 || i1 == 1125) {
					int l1 = menuActionCmd2[menuActionRow - 1];
					int j2 = menuActionCmd3[menuActionRow - 1];
					RSInterface class9 = RSInterface.interfaceCache[j2];
					if (class9.aBoolean259 || class9.aBoolean235) {
						aBoolean1242 = false;
						anInt989 = 0;
						anInt1084 = j2;
						anInt1085 = l1;
						activeInterfaceType = 2;
						anInt1087 = base.saveClickX;
						anInt1088 = base.saveClickY;
						if (RSInterface.interfaceCache[j2].parentID == openInterfaceID)
							activeInterfaceType = 1;
						if (RSInterface.interfaceCache[j2].parentID == backDialogID)
							activeInterfaceType = 3;
						return;
					}
				}
			}
			if (j == 1
			    && (anInt1253 == 1 || menuHasAddFriend(menuActionRow - 1))
			    && menuActionRow > 2)
				j = 2;
			if (j == 1 && menuActionRow > 0)
				doAction(menuActionRow - 1);
			if (j == 2 && menuActionRow > 0)
				determineMenuSize();
			drawMinimapHovers();
		}
	}
	
	public byte[] getModel(int index) {
//		try {
//			File model = new File("./Models/" + index + ".gz");
//			byte[] aByte = new byte[(int) model.Length];
//			FileInputStream Fis = new FileInputStream(model);
//			Fis.read(aByte);
//			pushMessage("aByte = [" + aByte + "]!", 0, "");
//			Fis.close();
//			return aByte;
//		} catch (Exception e) {
//			return null;
//		}
		return client.ReadAllBytes (signlink.findcachedir () + "Models/" + index + ".gz");
	}
	
	public static int totalRead = 0;
	public static bool enableTweening = true;
	
	public static string getFileNameWithoutExtension(string fileName) {
		FileInfo tmpFile = new FileInfo(fileName);
		int whereDot = ((String)tmpFile.Name).LastIndexOf('.');
		if (0 < whereDot && whereDot <= tmpFile.Name.Length - 2 ) {
			return tmpFile.Name.Substring(0, whereDot);
		}
		return "";
	}
	
//	public void preloadModels() {
//		File file = new File("./Raw/");
//		File[] fileArray = file.listFiles();
//		for (int y = 0; y < fileArray.Length; y++) {
//			String s = fileArray[y].getName();
//			byte[] buffer = ReadFile("./Raw/" + s);
//			Model.method460(buffer,
//			                Int32.Parse(getFileNameWithoutExtension(s)));
//		}
//	}
	
	public static byte[] ReadFile(String s) {
// 		try {
// 			byte[] abyte0;
// 			File file = new File(s);
// 			int i = (int) file.Length;
// 			abyte0 = new byte[i];
// 			DataInputStream datainputstream = new DataInputStream(
// 				new BufferedInputStream(new FileInputStream(s)));
// 			datainputstream.readFully(abyte0, 0, i);
// 			datainputstream.close();
// 			totalRead++;
// 			return abyte0;
// 		} catch (Exception e) {
// 			UnityEngine.Debug.Log((new StringBuilder()).Append("Read Error: ")
// 			                   .Append(s).ToString());
// 			return null;
// 		}
		return client.ReadAllBytes(s);
	}
	
	public void models() {
		for (int ModelIndex = 0; ModelIndex < 50000; ModelIndex++) {
			byte[] abyte0 = getModel(ModelIndex);
			if (abyte0 != null && abyte0.Length > 0) {
				decompressors[1].method234(abyte0.Length, abyte0, ModelIndex);
				pushMessage("Model added successfully!", 0, "");
			}
		}
	}
	
	private void saveMidi(bool flag, byte[] abyte0) {
		signlink.midifade = flag ? 1 : 0;
		signlink.midisave(abyte0, abyte0.Length);
	}
	
	public void method22() {
		//try {
			anInt985 = -1;
			aClass19_1056.removeAll();
			projectiles.removeAll();
			RS2Sharp.Texture.method366();
			unlinkMRUNodes();
			//worldController.initToNull();
			System.GC.Collect();
			for (int i = 0; i < 4; i++)
				aClass11Array1230[i].method210();
			
			for (int l = 0; l < 4; l++) {
				for (int k1 = 0; k1 < 104; k1++) {
					for (int j2 = 0; j2 < 104; j2++)
						byteGroundArray[l][k1][j2] = 0;
					
				}
				
			}
			
			int regionX = 49;
			int regionY = 54;
			int xCoord = regionX * 8;
			int yCoord = regionY * 8;
			
			localRegionMapData = new byte[1][];
			localRegionIds = new int[1];
			int[] terrainIndices = new int[1];
			int[] anIntArray1236 = new int[1];
			localRegionLandscapeData = new byte[1][];
		
			localRegionIds[0] = ((xCoord) / 8 << 8) + (yCoord) / 8;
			localRegionMapData[0] = decompressors[4].decompressgzip(terrainIndices[0]);//CacheManager.getFile(4, terrainIndices[0]);	
			terrainIndices[0] = MapManager.getMapData(0, (yCoord) / 8, (xCoord) / 8);
			anIntArray1236[0] = MapManager.getMapData(1, (yCoord) / 8, (xCoord) / 8);
		localRegionLandscapeData[0] = decompressors[4].decompressgzip( anIntArray1236[0]);
			//objectManager = new ObjectManager(byteGroundArray,
			//                                                intGroundArray);
			                                                
		if (decompressors[4].decompress(terrainIndices[0]) == null)
				terrainIndices[0] = -1;
			int k2 = localRegionMapData.Length;
			//stream.createFrame(0);
			if (!aBoolean1159) {
				for (int i3 = 0; i3 < k2; i3++) {
					int i4 = (localRegionIds[i3] >> 8) * 64 - baseX;
					int k5 = (localRegionIds[i3] & 0xff) * 64 - baseY;
					byte[] abyte0 = localRegionMapData[i3];
//					if (abyte0 != null)
//						objectManager.method180(abyte0, k5, i4,
//						                        (anInt1069 - 6) * 8, (anInt1070 - 6) * 8,
//						                        aClass11Array1230);
				}
				
				for (int j4 = 0; j4 < k2; j4++) {
					int l5 = (localRegionIds[j4] >> 8) * 64 - baseX;
					int k7 = (localRegionIds[j4] & 0xff) * 64 - baseY;
					byte[] abyte2 = localRegionMapData[j4];
//					if (abyte2 == null && anInt1070 < 800)
//						objectManager.method174(k7, 64, 64, l5);
				}
				
				anInt1097++;
				if (anInt1097 > 160) {
					anInt1097 = 0;
					//stream.createFrame(238);
					//stream.writeWordBigEndian(96);
				}
				//stream.createFrame(0);
				for (int i6 = 0; i6 < k2; i6++) {
					byte[] abyte1 = localRegionLandscapeData[i6];
					if (abyte1 != null) {
						int l8 = (localRegionIds[i6] >> 8) * 64 - baseX;
						int k9 = (localRegionIds[i6] & 0xff) * 64 - baseY;
						//objectManager.method190(l8, aClass11Array1230, k9,
						                        //worldController, abyte1);
					}
				}
				
			}
			if (aBoolean1159) {
				for (int j3 = 0; j3 < 4; j3++) {
					for (int k4 = 0; k4 < 13; k4++) {
						for (int j6 = 0; j6 < 13; j6++) {
							int l7 = anIntArrayArrayArray1129[j3][k4][j6];
							if (l7 != -1) {
								int i9 = l7 >> 24 & 3;
								int l9 = l7 >> 1 & 3;
								int j10 = l7 >> 14 & 0x3ff;
								int l10 = l7 >> 3 & 0x7ff;
								int j11 = (j10 / 8 << 8) + l10 / 8;
//								for (int l11 = 0; l11 < localRegionIds.Length; l11++) {
//									if (localRegionIds[l11] != j11
//									    || localRegionMapData[l11] == null)
//										continue;
//									objectManager.method179(i9, l9,
//									                        aClass11Array1230, k4 * 8,
//									                        (j10 & 7) * 8,
//									                        localRegionMapData[l11],
//									                        (l10 & 7) * 8, j3, j6 * 8);
//									break;
//								}
								
							}
						}
						
					}
					
				}
				
				for (int l4 = 0; l4 < 13; l4++) {
					for (int k6 = 0; k6 < 13; k6++) {
						int i8 = anIntArrayArrayArray1129[0][l4][k6];
//						if (i8 == -1)
//							objectManager.method174(k6 * 8, 8, 8, l4 * 8);
					}
					
				}
				
				//stream.createFrame(0);
				for (int l6 = 0; l6 < 4; l6++) {
					for (int j8 = 0; j8 < 13; j8++) {
						for (int j9 = 0; j9 < 13; j9++) {
							int i10 = anIntArrayArrayArray1129[l6][j8][j9];
							if (i10 != -1) {
								int k10 = i10 >> 24 & 3;
								int i11 = i10 >> 1 & 3;
								int k11 = i10 >> 14 & 0x3ff;
								int i12 = i10 >> 3 & 0x7ff;
								int j12 = (k11 / 8 << 8) + i12 / 8;
								for (int k12 = 0; k12 < localRegionIds.Length; k12++) {
									if (localRegionIds[k12] != j12
									    || localRegionLandscapeData[k12] == null)
										continue;
//									objectManager.method183(aClass11Array1230,
//									                        //worldController, k10, j8 * 8,
//									                        (i12 & 7) * 8, l6,
//									                        localRegionLandscapeData[k12],
//									                        (k11 & 7) * 8, i11, j9 * 8);
									break;
								}
								
							}
						}
						
					}
					
				}
				
			}
			//stream.createFrame(0);
			//objectManager.method171(aClass11Array1230, //worldController);
			aRSImageProducer_1165.initDrawingArea();
			//stream.createFrame(0);
			int k3 = 0;//ObjectManager.anInt145;
			if (k3 > plane)
				k3 = plane;
			if (k3 < plane - 1)
				k3 = plane - 1;
//			if (lowMem)
//				//worldController.method275(ObjectManager.anInt145);
//			else
//				//worldController.method275(0);
			for (int i5 = 0; i5 < 104; i5++) {
				for (int i7 = 0; i7 < 104; i7++)
					spawnGroundItem(i5, i7);
				
			}
			
			anInt1051++;
			if (anInt1051 > 98) {
				anInt1051 = 0;
//				stream.createFrame(150);
			}
			method63();
		//} catch (Exception exception) {
		//}
		ObjectDef.mruNodes1.unlinkAll();
		if (base.gameFrame != null) {
//			stream.createFrame(210);
//			stream.writeDWord(0x3f008edd);
		}
		if (lowMem && signlink.cache_dat != null) {
			int j = onDemandFetcher.getVersionCount(0);
			for (int i1 = 0; i1 < j; i1++) {
				int l1 = onDemandFetcher.getModelIndex(i1);
				if ((l1 & 0x79) == 0)
					Model.method461(i1);
			}
			
		}
		System.GC.Collect();
		RS2Sharp.Texture.method367();
		onDemandFetcher.method566();
		int k = (anInt1069 - 6) / 8 - 1;
		int j1 = (anInt1069 + 6) / 8 + 1;
		int i2 = (anInt1070 - 6) / 8 - 1;
		int l2 = (anInt1070 + 6) / 8 + 1;
		if (aBoolean1141) {
			k = 49;
			j1 = 50;
			i2 = 49;
			l2 = 50;
		}
		for (int l3 = k; l3 <= j1; l3++) {
			for (int j5 = i2; j5 <= l2; j5++)
			if (l3 == k || l3 == j1 || j5 == i2 || j5 == l2) {
				int j7 = onDemandFetcher.method562(0, j5, l3);
				if (j7 != -1)
					onDemandFetcher.method560(j7, 3);
				int k8 = onDemandFetcher.method562(1, j5, l3);
				if (k8 != -1)
					onDemandFetcher.method560(k8, 3);
			}
			
		}
		
	}
	
	private void unlinkMRUNodes() {
		ObjectDef.mruNodes1.unlinkAll();
		ObjectDef.mruNodes2.unlinkAll();
		EntityDef.mruNodes.unlinkAll();
		ItemDef.mruNodes2.unlinkAll();
		ItemDef.mruNodes1.unlinkAll();
		Player.mruNodes.unlinkAll();
		SpotAnim.aMRUNodes_415.unlinkAll();
	}
	
	private void method24(int i) {
		int[] ai = aClass30_Sub2_Sub1_Sub1_1263.myPixels;
		int j = ai.Length;
		for (int k = 0; k < j; k++)
			ai[k] = 0;
		
		for (int l = 1; l < 103; l++) {
			int i1 = 24628 + (103 - l) * 512 * 4;
			for (int k1 = 1; k1 < 103; k1++) {
				if ((byteGroundArray[i][k1][l] & 0x18) == 0)
					//worldController.method309(ai, i1, i, k1, l);
				if (i < 3 && (byteGroundArray[i + 1][k1][l] & 8) != 0)
					//worldController.method309(ai, i1, i + 1, k1, l);
				i1 += 4;
			}
			
		}
		
//		int j1 = ((238 + (int) (RS2Sharp.Random.Next() * 20D)) - 10 << 16)
//			+ ((238 + (int) (RS2Sharp.Random.Next() * 20D)) - 10 << 8)
//				+ ((238 + (int) (RS2Sharp.Random.Next() * 20D)) - 10);
//		int l1 = (238 + (int) (RS2Sharp.Random.Next() * 20D)) - 10 << 16;
		int j1 = ((238 + (int)(RS2Sharp.Random.Next(20))) - 10 << 16) + ((238 + (int)(RS2Sharp.Random.Next(20))) - 10 << 8) + ((238 + (int)(RS2Sharp.Random.Next(20))) - 10);
		int l1 = (238 + (int)(RS2Sharp.Random.Next(20))) - 10 << 16;
		aClass30_Sub2_Sub1_Sub1_1263.method343();
		for (int i2 = 1; i2 < 103; i2++) {
			for (int j2 = 1; j2 < 103; j2++) {
				if ((byteGroundArray[i][j2][i2] & 0x18) == 0)
					method50(i2, j1, j2, l1, i);
				if (i < 3 && (byteGroundArray[i + 1][j2][i2] & 8) != 0)
					method50(i2, j1, j2, l1, i + 1);
			}
			
		}
		
		aRSImageProducer_1165.initDrawingArea();
		anInt1071 = 0;
		for (int k2 = 0; k2 < 104; k2++) {
			for (int l2 = 0; l2 < 104; l2++) {
				int i3 = 0;//worldController.method303(plane, k2, l2);
				if (i3 != 0) {
					i3 = i3 >> 14 & 0x7fff;
					int j3 = ObjectDef.forID(i3).anInt746;
					if (j3 >= 0) {
						int k3 = k2;
						int l3 = l2;
						if (j3 != 22 && j3 != 29 && j3 != 34 && j3 != 36
						    && j3 != 46 && j3 != 47 && j3 != 48) {
							byte byte0 = 104;
							byte byte1 = 104;
							uint[][] ai1 = aClass11Array1230[plane].anIntArrayArray294;
							for (int i4 = 0; i4 < 10; i4++) {
								int j4 = (int) (RS2Sharp.Random.Next(4));
								if (j4 == 0 && k3 > 0 && k3 > k2 - 3
								    && (ai1[k3 - 1][l3] & 0x1280108) == 0)
									k3--;
								if (j4 == 1 && k3 < byte0 - 1 && k3 < k2 + 3
								    && (ai1[k3 + 1][l3] & 0x1280180) == 0)
									k3++;
								if (j4 == 2 && l3 > 0 && l3 > l2 - 3
								    && (ai1[k3][l3 - 1] & 0x1280102) == 0)
									l3--;
								if (j4 == 3 && l3 < byte1 - 1 && l3 < l2 + 3
								    && (ai1[k3][l3 + 1] & 0x1280120) == 0)
									l3++;
							}
							
						}
						try{
						aClass30_Sub2_Sub1_Sub1Array1140[anInt1071] = mapFunctions[j3];
						anIntArray1072[anInt1071] = k3;
						anIntArray1073[anInt1071] = l3;
						} catch(Exception ex)
						{
							
						}
						anInt1071++;
					}
				}
			}
			
		}
		
	}
	
	private void spawnGroundItem(int i, int j) {
		NodeList class19 = groundArray[plane][i][j];
		if (class19 == null) {
			//worldController.method295(plane, i, j);
			return;
		}
		int k = unchecked((int)0xfa0a1f01);
		System.Object obj = null;
		for (Item item = (Item) class19.reverseGetFirst(); item != null; item = (Item) class19
		     .reverseGetNext()) {
			ItemDef itemDef = ItemDef.forID(item.ID);
			int l = itemDef.value;
			if (itemDef.stackable)
				l *= item.anInt1559 + 1;
			// notifyItemSpawn(item, i + baseX, j + baseY);
			
			if (l > k) {
				k = l;
				obj = item;
			}
		}
		
		class19.insertTail(((Node) (obj)));
		System.Object obj1 = null;
		System.Object obj2 = null;
		for (Item class30_sub2_sub4_sub2_1 = (Item) class19.reverseGetFirst(); class30_sub2_sub4_sub2_1 != null; class30_sub2_sub4_sub2_1 = (Item) class19
		     .reverseGetNext()) {
			if (class30_sub2_sub4_sub2_1.ID != ((Item) (obj)).ID
			    && obj1 == null)
				obj1 = class30_sub2_sub4_sub2_1;
			if (class30_sub2_sub4_sub2_1.ID != ((Item) (obj)).ID
			    && class30_sub2_sub4_sub2_1.ID != ((Item) (obj1)).ID
			    && obj2 == null)
				obj2 = class30_sub2_sub4_sub2_1;
		}
		
		int i1 = i + (j << 7) + 0x60000000;
		//worldController.method281(i, i1, ((Animable) (obj1)),
//		                          method42(plane, j * 128 + 64, i * 128 + 64),
		 //                         ((Animable) (obj2)), ((Animable) (obj)), plane, j, 0);
	}
	
	private void method26(bool flag) {
		for (int j = 0; j < npcCount; j++) {
			NPC npc = npcArray[npcIndices[j]];
			int k = 0x20000000 + (npcIndices[j] << 14);
			if (npc == null || !npc.isVisible() || npc.desc.aBoolean93 != flag)
				continue;
			int l = npc.x >> 7;
			int i1 = npc.y >> 7;
			if (l < 0 || l >= 104 || i1 < 0 || i1 >= 104)
				continue;
			if (npc.anInt1540 == 1 && (npc.x & 0x7f) == 64
			    && (npc.y & 0x7f) == 64) {
				if (anIntArrayArray929[l][i1] == anInt1265)
					continue;
				anIntArrayArray929[l][i1] = anInt1265;
			}
			if (!npc.desc.aBoolean84)
				k += unchecked((int)0x80000000);
			//worldController
//				.addEntity(plane, npc.anInt1552,
//				           method42(plane, npc.y, npc.x), k, npc.y,
//				           (npc.anInt1540 - 1) * 64 + 60, npc.x, npc,
//				           npc.aBoolean1541);
		}
	}
	
	private bool replayWave() {
		return signlink.wavereplay();
	}
	
	private void loadError() {
		String s = "ondemand";// was a constant parameter
		UnityEngine.Debug.Log(s);
//		try {
//			getAppletContext().showDocument(
//				new URL(getCodeBase(), "loaderror_" + s + ".html"));
//		} catch (Exception exception) {
//			Debug.Log(exception.StackTrace);
//		}
		do
		try {
			//Thread.Sleep(1000);
		} catch (Exception _ex) {
		}
		while (true);
	}
	
	private void buildInterfaceMenu(int i, RSInterface class9, int k, int l,
	                                int i1, int j1) {
		if (class9.type != 0 || class9.children == null
		    || class9.aBoolean266)
			return;
		if (k < i || i1 < l || k > i + class9.width || i1 > l + class9.height)
			return;
		int k1 = class9.children.Length;
		for (int l1 = 0; l1 < k1; l1++) {
			int i2 = class9.childX[l1] + i;
			int j2 = (class9.childY[l1] + l) - j1;
			RSInterface class9_1 = RSInterface.interfaceCache[class9.children[l1]];
			i2 += class9_1.anInt263;
			j2 += class9_1.anInt265;
			if ((class9_1.anInt230 >= 0 || class9_1.anInt216 != 0)
			    && k >= i2 && i1 >= j2 && k < i2 + class9_1.width
			    && i1 < j2 + class9_1.height)
				if (class9_1.anInt230 >= 0)
					anInt886 = class9_1.anInt230;
			else
				anInt886 = class9_1.id;
			if (class9_1.type == 8 && k >= i2 && i1 >= j2 && k < i2 + class9_1.width && i1 < j2 + class9_1.height) {
				anInt1315 = class9_1.id;
			}
			if (class9_1.type == 0) {
				buildInterfaceMenu(i2, class9_1, k, j2, i1,
				                   class9_1.scrollPosition);
				if (class9_1.scrollMax > class9_1.height)
					method65(i2 + class9_1.width, class9_1.height, k, i1,
					         class9_1, j2, true, class9_1.scrollMax);
			} else {
				if (class9_1.atActionType == 1 && k >= i2 && i1 >= j2
				    && k < i2 + class9_1.width && i1 < j2 + class9_1.height) {
					bool flag = false;
					if (class9_1.anInt214 != 0)
						flag = buildFriendsListMenu(class9_1);
					if (!flag) {
						// UnityEngine.Debug.Log("1"+class9_1.tooltip + ", " +
						// class9_1.interfaceID);
						menuActionName[menuActionRow] = class9_1.tooltip + ", "
							+ class9_1.id;
						menuActionID[menuActionRow] = 315;
						menuActionCmd3[menuActionRow] = class9_1.id;
						menuActionRow++;
					}
				}
				if (class9_1.atActionType == 2 && spellSelected == 0 && k >= i2
				    && i1 >= j2 && k < i2 + class9_1.width
				    && i1 < j2 + class9_1.height) {
					String s = class9_1.selectedActionName;
					if (s.IndexOf(" ") != -1)
						s = s.Substring(0, s.IndexOf(" "));
					menuActionName[menuActionRow] = s + " @gre@"
						+ class9_1.spellName;
					menuActionID[menuActionRow] = 626;
					menuActionCmd3[menuActionRow] = class9_1.id;
					menuActionRow++;
				}
				if (class9_1.atActionType == 3 && k >= i2 && i1 >= j2
				    && k < i2 + class9_1.width && i1 < j2 + class9_1.height) {
					menuActionName[menuActionRow] = "Close";
					menuActionID[menuActionRow] = 200;
					menuActionCmd3[menuActionRow] = class9_1.id;
					menuActionRow++;
				}
				if (class9_1.atActionType == 4 && k >= i2 && i1 >= j2
				    && k < i2 + class9_1.width && i1 < j2 + class9_1.height) {
					// UnityEngine.Debug.Log("2"+class9_1.tooltip + ", " +
					// class9_1.interfaceID);
					menuActionName[menuActionRow] = class9_1.tooltip + ", "
						+ class9_1.id;
					menuActionID[menuActionRow] = 169;
					menuActionCmd3[menuActionRow] = class9_1.id;
					menuActionRow++;
				}
				if (class9_1.atActionType == 5 && k >= i2 && i1 >= j2
				    && k < i2 + class9_1.width && i1 < j2 + class9_1.height) {
					// UnityEngine.Debug.Log("3"+class9_1.tooltip + ", " +
					// class9_1.interfaceID);
					menuActionName[menuActionRow] = class9_1.tooltip + ", "
						+ class9_1.id;
					menuActionID[menuActionRow] = 646;
					menuActionCmd3[menuActionRow] = class9_1.id;
					menuActionRow++;
				}
				if (class9_1.atActionType == 6 && !aBoolean1149 && k >= i2
				    && i1 >= j2 && k < i2 + class9_1.width
				    && i1 < j2 + class9_1.height) {
					// UnityEngine.Debug.Log("4"+class9_1.tooltip + ", " +
					// class9_1.interfaceID);
					menuActionName[menuActionRow] = class9_1.tooltip + ", "
						+ class9_1.id;
					menuActionID[menuActionRow] = 679;
					menuActionCmd3[menuActionRow] = class9_1.id;
					menuActionRow++;
				}
				if (class9_1.type == 2) {
					int k2 = 0;
					for (int l2 = 0; l2 < class9_1.height; l2++) {
						for (int i3 = 0; i3 < class9_1.width; i3++) {
							int j3 = i2 + i3 * (32 + class9_1.invSpritePadX);
							int k3 = j2 + l2 * (32 + class9_1.invSpritePadY);
							if (k2 < 20) {
								j3 += class9_1.spritesX[k2];
								k3 += class9_1.spritesY[k2];
							}
							if (k >= j3 && i1 >= k3 && k < j3 + 32
							    && i1 < k3 + 32) {
								mouseInvInterfaceIndex = k2;
								lastActiveInvInterface = class9_1.id;
								if (class9_1.inv[k2] > 0) {
									ItemDef itemDef = ItemDef
										.forID(class9_1.inv[k2] - 1);
									if (itemSelected == 1
									    && class9_1.isInventoryInterface) {
										if (class9_1.id != anInt1284
										    || k2 != anInt1283) {
											menuActionName[menuActionRow] = "Use "
												+ selectedItemName
													+ " with @lre@"
													+ itemDef.name;
											menuActionID[menuActionRow] = 870;
											menuActionCmd1[menuActionRow] = itemDef.ID;
											menuActionCmd2[menuActionRow] = k2;
											menuActionCmd3[menuActionRow] = class9_1.id;
											menuActionRow++;
										}
									} else if (spellSelected == 1
									           && class9_1.isInventoryInterface) {
										if ((spellUsableOn & 0x10) == 16) {
											menuActionName[menuActionRow] = spellTooltip
												+ " @lre@" + itemDef.name;
											menuActionID[menuActionRow] = 543;
											menuActionCmd1[menuActionRow] = itemDef.ID;
											menuActionCmd2[menuActionRow] = k2;
											menuActionCmd3[menuActionRow] = class9_1.id;
											menuActionRow++;
										}
									} else {
										if (class9_1.isInventoryInterface) {
											for (int l3 = 4; l3 >= 3; l3--)
												if (itemDef.actions != null
												   && itemDef.actions[l3] != null) {
												menuActionName[menuActionRow] = itemDef.actions[l3]
												+ " @lre@"
													+ itemDef.name;
												if (l3 == 3)
													menuActionID[menuActionRow] = 493;
												if (l3 == 4)
													menuActionID[menuActionRow] = 847;
												menuActionCmd1[menuActionRow] = itemDef.ID;
												menuActionCmd2[menuActionRow] = k2;
												menuActionCmd3[menuActionRow] = class9_1.id;
												menuActionRow++;
											} else if (l3 == 4) {
												menuActionName[menuActionRow] = "Drop @lre@"
													+ itemDef.name;
												menuActionID[menuActionRow] = 847;
												menuActionCmd1[menuActionRow] = itemDef.ID;
												menuActionCmd2[menuActionRow] = k2;
												menuActionCmd3[menuActionRow] = class9_1.id;
												menuActionRow++;
											}
											
										}
										if (class9_1.usableItemInterface) {
											menuActionName[menuActionRow] = "Use @lre@"
												+ itemDef.name;
											menuActionID[menuActionRow] = 447;
											menuActionCmd1[menuActionRow] = itemDef.ID;
											menuActionCmd2[menuActionRow] = k2;
											menuActionCmd3[menuActionRow] = class9_1.id;
											menuActionRow++;
										}
										if (class9_1.isInventoryInterface
										    && itemDef.actions != null) {
											for (int i4 = 2; i4 >= 0; i4--)
											if (itemDef.actions[i4] != null) {
												menuActionName[menuActionRow] = itemDef.actions[i4]
												+ " @lre@"
													+ itemDef.name;
												if (i4 == 0)
													menuActionID[menuActionRow] = 74;
												if (i4 == 1)
													menuActionID[menuActionRow] = 454;
												if (i4 == 2)
													menuActionID[menuActionRow] = 539;
												menuActionCmd1[menuActionRow] = itemDef.ID;
												menuActionCmd2[menuActionRow] = k2;
												menuActionCmd3[menuActionRow] = class9_1.id;
												menuActionRow++;
											}
											
										}
										if (class9_1.actions != null) {
											for (int j4 = 4; j4 >= 0; j4--)
											if (class9_1.actions[j4] != null) {
												menuActionName[menuActionRow] = class9_1.actions[j4]
												+ " @lre@"
													+ itemDef.name;
												if (j4 == 0)
													menuActionID[menuActionRow] = 632;
												if (j4 == 1)
													menuActionID[menuActionRow] = 78;
												if (j4 == 2)
													menuActionID[menuActionRow] = 867;
												if (j4 == 3)
													menuActionID[menuActionRow] = 431;
												if (j4 == 4)
													menuActionID[menuActionRow] = 53;
												menuActionCmd1[menuActionRow] = itemDef.ID;
												menuActionCmd2[menuActionRow] = k2;
												menuActionCmd3[menuActionRow] = class9_1.id;
												menuActionRow++;
											}
											
										}
										// menuActionName[menuActionRow] =
										// "Examine @lre@" + itemDef.name +
										// " @gre@(@whi@" + (class9_1.inv[k2] -
										// 1) + "@gre@)";
										menuActionName[menuActionRow] = "Examine @lre@"
											+ itemDef.name;
										menuActionID[menuActionRow] = 1125;
										menuActionCmd1[menuActionRow] = itemDef.ID;
										menuActionCmd2[menuActionRow] = k2;
										menuActionCmd3[menuActionRow] = class9_1.id;
										menuActionRow++;
									}
								}
							}
							k2++;
						}
						
					}
					
				}
			}
		}
		
	}
	
	public void drawScrollbar(int j, int k, int l, int i1, int j1) {
		scrollBar1.drawSprite(i1, l);
		scrollBar2.drawSprite(i1, (l + j) - 16);
		DrawingArea.drawPixels(j - 32, l + 16, i1, 0x000001, 16);
		DrawingArea.drawPixels(j - 32, l + 16, i1, 0x3d3426, 15);
		DrawingArea.drawPixels(j - 32, l + 16, i1, 0x342d21, 13);
		DrawingArea.drawPixels(j - 32, l + 16, i1, 0x2e281d, 11);
		DrawingArea.drawPixels(j - 32, l + 16, i1, 0x29241b, 10);
		DrawingArea.drawPixels(j - 32, l + 16, i1, 0x252019, 9);
		DrawingArea.drawPixels(j - 32, l + 16, i1, 0x000001, 1);
		int k1 = ((j - 32) * j) / j1;
		if (k1 < 8)
			k1 = 8;
		int l1 = ((j - 32 - k1) * k) / (j1 - j);
		DrawingArea.drawPixels(k1, l + 16 + l1, i1, barFillColor, 16);
		DrawingArea.method341(l + 16 + l1, 0x000001, k1, i1);
		DrawingArea.method341(l + 16 + l1, 0x817051, k1, i1 + 1);
		DrawingArea.method341(l + 16 + l1, 0x73654a, k1, i1 + 2);
		DrawingArea.method341(l + 16 + l1, 0x6a5c43, k1, i1 + 3);
		DrawingArea.method341(l + 16 + l1, 0x6a5c43, k1, i1 + 4);
		DrawingArea.method341(l + 16 + l1, 0x655841, k1, i1 + 5);
		DrawingArea.method341(l + 16 + l1, 0x655841, k1, i1 + 6);
		DrawingArea.method341(l + 16 + l1, 0x61553e, k1, i1 + 7);
		DrawingArea.method341(l + 16 + l1, 0x61553e, k1, i1 + 8);
		DrawingArea.method341(l + 16 + l1, 0x5d513c, k1, i1 + 9);
		DrawingArea.method341(l + 16 + l1, 0x5d513c, k1, i1 + 10);
		DrawingArea.method341(l + 16 + l1, 0x594e3a, k1, i1 + 11);
		DrawingArea.method341(l + 16 + l1, 0x594e3a, k1, i1 + 12);
		DrawingArea.method341(l + 16 + l1, 0x514635, k1, i1 + 13);
		DrawingArea.method341(l + 16 + l1, 0x4b4131, k1, i1 + 14);
		DrawingArea.method339(l + 16 + l1, 0x000001, 15, i1);
		DrawingArea.method339(l + 17 + l1, 0x000001, 15, i1);
		DrawingArea.method339(l + 17 + l1, 0x655841, 14, i1);
		DrawingArea.method339(l + 17 + l1, 0x6a5c43, 13, i1);
		DrawingArea.method339(l + 17 + l1, 0x6d5f48, 11, i1);
		DrawingArea.method339(l + 17 + l1, 0x73654a, 10, i1);
		DrawingArea.method339(l + 17 + l1, 0x76684b, 7, i1);
		DrawingArea.method339(l + 17 + l1, 0x7b6a4d, 5, i1);
		DrawingArea.method339(l + 17 + l1, 0x7e6e50, 4, i1);
		DrawingArea.method339(l + 17 + l1, 0x817051, 3, i1);
		DrawingArea.method339(l + 17 + l1, 0x000001, 2, i1);
		DrawingArea.method339(l + 18 + l1, 0x000001, 16, i1);
		DrawingArea.method339(l + 18 + l1, 0x564b38, 15, i1);
		DrawingArea.method339(l + 18 + l1, 0x5d513c, 14, i1);
		DrawingArea.method339(l + 18 + l1, 0x625640, 11, i1);
		DrawingArea.method339(l + 18 + l1, 0x655841, 10, i1);
		DrawingArea.method339(l + 18 + l1, 0x6a5c43, 7, i1);
		DrawingArea.method339(l + 18 + l1, 0x6e6046, 5, i1);
		DrawingArea.method339(l + 18 + l1, 0x716247, 4, i1);
		DrawingArea.method339(l + 18 + l1, 0x7b6a4d, 3, i1);
		DrawingArea.method339(l + 18 + l1, 0x817051, 2, i1);
		DrawingArea.method339(l + 18 + l1, 0x000001, 1, i1);
		DrawingArea.method339(l + 19 + l1, 0x000001, 16, i1);
		DrawingArea.method339(l + 19 + l1, 0x514635, 15, i1);
		DrawingArea.method339(l + 19 + l1, 0x564b38, 14, i1);
		DrawingArea.method339(l + 19 + l1, 0x5d513c, 11, i1);
		DrawingArea.method339(l + 19 + l1, 0x61553e, 9, i1);
		DrawingArea.method339(l + 19 + l1, 0x655841, 7, i1);
		DrawingArea.method339(l + 19 + l1, 0x6a5c43, 5, i1);
		DrawingArea.method339(l + 19 + l1, 0x6e6046, 4, i1);
		DrawingArea.method339(l + 19 + l1, 0x73654a, 3, i1);
		DrawingArea.method339(l + 19 + l1, 0x817051, 2, i1);
		DrawingArea.method339(l + 19 + l1, 0x000001, 1, i1);
		DrawingArea.method339(l + 20 + l1, 0x000001, 16, i1);
		DrawingArea.method339(l + 20 + l1, 0x4b4131, 15, i1);
		DrawingArea.method339(l + 20 + l1, 0x544936, 14, i1);
		DrawingArea.method339(l + 20 + l1, 0x594e3a, 13, i1);
		DrawingArea.method339(l + 20 + l1, 0x5d513c, 10, i1);
		DrawingArea.method339(l + 20 + l1, 0x61553e, 8, i1);
		DrawingArea.method339(l + 20 + l1, 0x655841, 6, i1);
		DrawingArea.method339(l + 20 + l1, 0x6a5c43, 4, i1);
		DrawingArea.method339(l + 20 + l1, 0x73654a, 3, i1);
		DrawingArea.method339(l + 20 + l1, 0x817051, 2, i1);
		DrawingArea.method339(l + 20 + l1, 0x000001, 1, i1);
		DrawingArea.method341(l + 16 + l1, 0x000001, k1, i1 + 15);
		DrawingArea.method339(l + 15 + l1 + k1, 0x000001, 16, i1);
		DrawingArea.method339(l + 14 + l1 + k1, 0x000001, 15, i1);
		DrawingArea.method339(l + 14 + l1 + k1, 0x3f372a, 14, i1);
		DrawingArea.method339(l + 14 + l1 + k1, 0x443c2d, 10, i1);
		DrawingArea.method339(l + 14 + l1 + k1, 0x483e2f, 9, i1);
		DrawingArea.method339(l + 14 + l1 + k1, 0x4a402f, 7, i1);
		DrawingArea.method339(l + 14 + l1 + k1, 0x4b4131, 4, i1);
		DrawingArea.method339(l + 14 + l1 + k1, 0x564b38, 3, i1);
		DrawingArea.method339(l + 14 + l1 + k1, 0x000001, 2, i1);
		DrawingArea.method339(l + 13 + l1 + k1, 0x000001, 16, i1);
		DrawingArea.method339(l + 13 + l1 + k1, 0x443c2d, 15, i1);
		DrawingArea.method339(l + 13 + l1 + k1, 0x4b4131, 11, i1);
		DrawingArea.method339(l + 13 + l1 + k1, 0x514635, 9, i1);
		DrawingArea.method339(l + 13 + l1 + k1, 0x544936, 7, i1);
		DrawingArea.method339(l + 13 + l1 + k1, 0x564b38, 6, i1);
		DrawingArea.method339(l + 13 + l1 + k1, 0x594e3a, 4, i1);
		DrawingArea.method339(l + 13 + l1 + k1, 0x625640, 3, i1);
		DrawingArea.method339(l + 13 + l1 + k1, 0x6a5c43, 2, i1);
		DrawingArea.method339(l + 13 + l1 + k1, 0x000001, 1, i1);
		DrawingArea.method339(l + 12 + l1 + k1, 0x000001, 16, i1);
		DrawingArea.method339(l + 12 + l1 + k1, 0x443c2d, 15, i1);
		DrawingArea.method339(l + 12 + l1 + k1, 0x4b4131, 14, i1);
		DrawingArea.method339(l + 12 + l1 + k1, 0x544936, 12, i1);
		DrawingArea.method339(l + 12 + l1 + k1, 0x564b38, 11, i1);
		DrawingArea.method339(l + 12 + l1 + k1, 0x594e3a, 10, i1);
		DrawingArea.method339(l + 12 + l1 + k1, 0x5d513c, 7, i1);
		DrawingArea.method339(l + 12 + l1 + k1, 0x61553e, 4, i1);
		DrawingArea.method339(l + 12 + l1 + k1, 0x6e6046, 3, i1);
		DrawingArea.method339(l + 12 + l1 + k1, 0x7b6a4d, 2, i1);
		DrawingArea.method339(l + 12 + l1 + k1, 0x000001, 1, i1);
		DrawingArea.method339(l + 11 + l1 + k1, 0x000001, 16, i1);
		DrawingArea.method339(l + 11 + l1 + k1, 0x4b4131, 15, i1);
		DrawingArea.method339(l + 11 + l1 + k1, 0x514635, 14, i1);
		DrawingArea.method339(l + 11 + l1 + k1, 0x564b38, 13, i1);
		DrawingArea.method339(l + 11 + l1 + k1, 0x594e3a, 11, i1);
		DrawingArea.method339(l + 11 + l1 + k1, 0x5d513c, 9, i1);
		DrawingArea.method339(l + 11 + l1 + k1, 0x61553e, 7, i1);
		DrawingArea.method339(l + 11 + l1 + k1, 0x655841, 5, i1);
		DrawingArea.method339(l + 11 + l1 + k1, 0x6a5c43, 4, i1);
		DrawingArea.method339(l + 11 + l1 + k1, 0x73654a, 3, i1);
		DrawingArea.method339(l + 11 + l1 + k1, 0x7b6a4d, 2, i1);
		DrawingArea.method339(l + 11 + l1 + k1, 0x000001, 1, i1);
	}
	
	private void updateNPCs(RS2Sharp.Stream stream, int i) {
//		anInt839 = 0;
//		anInt893 = 0;
//		method139(stream);
//		method46(i, stream);
//		method86(stream);
//		for (int k = 0; k < anInt839; k++) {
//			int l = anIntArray840[k];
//			if (npcArray[l].anInt1537 != loopCycle) {
//				npcArray[l].desc = null;
//				npcArray[l] = null;
//			}
//		}
//		
//		if (stream.currentOffset != i) {
//			signlink.reporterror(myUsername
//			                     + " size mismatch in getnpcpos - pos:"
//			                     + stream.currentOffset + " psize:" + i);
//			throw new Exception("eek");
//		}
//		for (int i1 = 0; i1 < npcCount; i1++)
//		if (npcArray[npcIndices[i1]] == null) {
//			signlink.reporterror(myUsername
//			                     + " null entry in npc list - pos:" + i1 + " size:"
//			                     + npcCount);
//			throw new Exception("eek");
//		}

		anInt839 = 0;
		anInt893 = 0;
		method139(stream);
		method46(i, stream);
		method86(stream);
		for(int k = 0; k < anInt839; k++)
		{
			int l = anIntArray840[k];
			if(npcArray[l].anInt1537 != loopCycle)
			{
				npcArray[l].npcObj.isDead = true;
				npcArray[l].desc = null;
				npcArray[l] = null;
			}
		}
		
		if(stream.currentOffset != i)
		{
			signlink.reporterror(myUsername + " size mismatch in getnpcpos - pos:" + stream.currentOffset + " psize:" + i);
			throw new Exception("eek");
		}
		for(int i1 = 0; i1 < npcCount; i1++)
			if(npcArray[npcIndices[i1]] == null)
		{
			signlink.reporterror(myUsername + " null entry in npc list - pos:" + i1 + " size:" + npcCount);
			throw new Exception("eek");
		}
		
		
	}
	
	private int cButtonHPos;
	private int cButtonHCPos;
	private int cButtonCPos;
	
	private void processChatModeClick() {
		if (base.mouseX >= 5 && base.mouseX <= 61 && base.mouseY >= 482
		    && base.mouseY <= 503) {
			cButtonHPos = 0;
			aBoolean1233 = true;
			inputTaken = true;
		} else if (base.mouseX >= 62 && base.mouseX <= 117
		           && base.mouseY >= 482 && base.mouseY <= 503) {
			cButtonHPos = 1;
			aBoolean1233 = true;
			inputTaken = true;
		} else if (base.mouseX >= 119 && base.mouseX <= 174
		           && base.mouseY >= 482 && base.mouseY <= 503) {
			cButtonHPos = 2;
			aBoolean1233 = true;
			inputTaken = true;
		} else if (base.mouseX >= 176 && base.mouseX <= 231
		           && base.mouseY >= 482 && base.mouseY <= 503) {
			cButtonHPos = 3;
			aBoolean1233 = true;
			inputTaken = true;
		} else if (base.mouseX >= 233 && base.mouseX <= 288
		           && base.mouseY >= 482 && base.mouseY <= 503) {
			cButtonHPos = 4;
			aBoolean1233 = true;
			inputTaken = true;
		} else if (base.mouseX >= 290 && base.mouseX <= 345
		           && base.mouseY >= 482 && base.mouseY <= 503) {
			cButtonHPos = 5;
			aBoolean1233 = true;
			inputTaken = true;
		} else if (base.mouseX >= 347 && base.mouseX <= 402
		           && base.mouseY >= 482 && base.mouseY <= 503) {
			cButtonHPos = 7;
			aBoolean1233 = true;
			inputTaken = true;
		} else if (base.mouseX >= 404 && base.mouseX <= 514
		           && base.mouseY >= 480 && base.mouseY <= 501) {
			cButtonHPos = 6;
			aBoolean1233 = true;
			inputTaken = true;
		} else {
			cButtonHPos = -1;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (base.clickMode3 == 1) {
			if (base.saveClickX >= 5 && base.saveClickX <= 61
			    && base.saveClickY >= 482 && base.saveClickY <= 505) {
				cButtonCPos = 0;
				chatTypeView = 0;
				aBoolean1233 = true;
				inputTaken = true;
			} else if (base.saveClickX >= 62 && base.saveClickX <= 117
			           && base.saveClickY >= 482 && base.saveClickY <= 505) {
				cButtonCPos = 1;
				chatTypeView = 5;
				aBoolean1233 = true;
				inputTaken = true;
			} else if (base.saveClickX >= 119 && base.saveClickX <= 174
			           && base.saveClickY >= 482 && base.saveClickY <= 505) {
				cButtonCPos = 2;
				chatTypeView = 1;
				aBoolean1233 = true;
				inputTaken = true;
			} else if (base.saveClickX >= 176 && base.saveClickX <= 231
			           && base.saveClickY >= 482 && base.saveClickY <= 505) {
				cButtonCPos = 3;
				chatTypeView = 2;
				aBoolean1233 = true;
				inputTaken = true;
			} else if (base.saveClickX >= 233 && base.saveClickX <= 288
			           && base.saveClickY >= 482 && base.saveClickY <= 505) {
				cButtonCPos = 4;
				chatTypeView = 11;
				aBoolean1233 = true;
				inputTaken = true;
			} else if (base.saveClickX >= 290 && base.saveClickX <= 345
			           && base.saveClickY >= 482 && base.saveClickY <= 505) {
				cButtonCPos = 5;
				chatTypeView = 3;
				aBoolean1233 = true;
				inputTaken = true;
			} else if (base.saveClickX >= 347 && base.saveClickX <= 402
			           && base.saveClickY >= 482 && base.saveClickY <= 505) {
				cButtonCPos = 6;
				aBoolean1233 = true;
				inputTaken = true;
			} else if (base.saveClickX >= 404 && base.saveClickX <= 515
			           && base.saveClickY >= 482 && base.saveClickY <= 505) {
				if (openInterfaceID == -1) {
					clearTopInterfaces();
					reportAbuseInput = "";
					canMute = false;
					for (int i = 0; i < RSInterface.interfaceCache.Length; i++) {
						if (RSInterface.interfaceCache[i] == null
						    || RSInterface.interfaceCache[i].anInt214 != 600)
							continue;
						reportAbuseInterfaceID = openInterfaceID = RSInterface.interfaceCache[i].parentID;
						break;
					}
				} else {
					pushMessage(
						"Please close the interface you have open before using 'report abuse'",
						0, "");
				}
			}
		}
	}
	
	private void method33(int i) {
		int j = Varp.cache[i].anInt709;
		if (j == 0)
			return;
		int k = variousSettings[i];
		if (j == 1) {
			if (k == 1)
				RS2Sharp.Texture.method372(0.90000000000000002D);
			if (k == 2)
				RS2Sharp.Texture.method372(0.80000000000000004D);
			if (k == 3)
				RS2Sharp.Texture.method372(0.69999999999999996D);
			if (k == 4)
				RS2Sharp.Texture.method372(0.59999999999999998D);
			ItemDef.mruNodes1.unlinkAll();
			welcomeScreenRaised = true;
		}
		if (j == 3) {
			bool flag1 = musicEnabled;
			if (k == 0) {
				adjustVolume(musicEnabled, 0);
				musicEnabled = true;
			}
			if (k == 1) {
				adjustVolume(musicEnabled, -400);
				musicEnabled = true;
			}
			if (k == 2) {
				adjustVolume(musicEnabled, -800);
				musicEnabled = true;
			}
			if (k == 3) {
				adjustVolume(musicEnabled, -1200);
				musicEnabled = true;
			}
			if (k == 4)
				musicEnabled = false;
			if (musicEnabled != flag1 && !lowMem) {
				if (musicEnabled) {
					nextSong = currentSong;
					songChanging = true;
					onDemandFetcher.method558(2, nextSong);
				} else {
					stopMidi();
				}
				prevSong = 0;
			}
		}
		if (j == 4) {
			if (k == 0) {
				aBoolean848 = true;
				setWaveVolume(0);
			}
			if (k == 1) {
				aBoolean848 = true;
				setWaveVolume(-400);
			}
			if (k == 2) {
				aBoolean848 = true;
				setWaveVolume(-800);
			}
			if (k == 3) {
				aBoolean848 = true;
				setWaveVolume(-1200);
			}
			if (k == 4)
				aBoolean848 = false;
		}
		if (j == 5)
			anInt1253 = k;
		if (j == 6)
			anInt1249 = k;
		if (j == 8) {
			splitPrivateChat = k;
			inputTaken = true;
		}
		if (j == 9)
			anInt913 = k;
	}
	
	private void updateEntities() {
//		try {
			int anInt974 = 0;
			for (int j = -1; j < playerCount + npcCount; j++) {
				System.Object obj;
				if (j == -1)
					obj = myPlayer;
				else if (j < playerCount)
					obj = playerArray[playerIndices[j]];
				else
					obj = npcArray[npcIndices[j - playerCount]];
				if (obj == null || !((Entity) (obj)).isVisible())
					continue;
				if (obj is NPC) {
					EntityDef entityDef = ((NPC) obj).desc;
					if (entityDef.childrenIDs != null)
						entityDef = entityDef.method161();
					if (entityDef == null)
						continue;
				}
				if (j < playerCount) {
					int l = 30;
					Player player = (Player) obj;
					if (player.headIcon >= 0) {
						npcScreenPos(((Entity) (obj)),
						             ((Entity) (obj)).height + 15);
						if (spriteDrawX > -1) {
							if (player.skullIcon < 2) {
								skullIcons[player.skullIcon].drawSprite(
									spriteDrawX - 12, spriteDrawY - l);
								l += 25;
							}
							if (player.headIcon < 7) {
								headIcons[player.headIcon].drawSprite(
									spriteDrawX - 12, spriteDrawY - l);
								l += 18;
							}
						}
					}
					if (j >= 0 && anInt855 == 10
					    && anInt933 == playerIndices[j]) {
						npcScreenPos(((Entity) (obj)),
						             ((Entity) (obj)).height + 15);
						if (spriteDrawX > -1)
							headIconsHint[player.hintIcon].drawSprite(
								spriteDrawX - 12, spriteDrawY - l);
					}
				} else {
					EntityDef entityDef_1 = ((NPC) obj).desc;
					if (entityDef_1.anInt75 >= 0
					    && entityDef_1.anInt75 < headIcons.Length) {
						npcScreenPos(((Entity) (obj)),
						             ((Entity) (obj)).height + 15);
						if (spriteDrawX > -1)
							headIcons[entityDef_1.anInt75].drawSprite(
								spriteDrawX - 12, spriteDrawY - 30);
					}
					if (anInt855 == 1
					    && anInt1222 == npcIndices[j - playerCount]
					    && loopCycle % 20 < 10) {
						npcScreenPos(((Entity) (obj)),
						             ((Entity) (obj)).height + 15);
						if (spriteDrawX > -1)
							headIconsHint[0].drawSprite(spriteDrawX - 12,
							                            spriteDrawY - 28);
					}
				}
				if (((Entity) (obj)).textSpoken != null
				    && (j >= playerCount || publicChatMode == 0
				    || publicChatMode == 3 || publicChatMode == 1
				    && isFriendOrSelf(((Player) obj).name))) {
					npcScreenPos(((Entity) (obj)), ((Entity) (obj)).height);
					if (spriteDrawX > -1 && anInt974 < anInt975) {
						anIntArray979[anInt974] = chatTextDrawingArea
							.method384(((Entity) (obj)).textSpoken) / 2;
						anIntArray978[anInt974] = chatTextDrawingArea.anInt1497;
						anIntArray976[anInt974] = spriteDrawX;
						anIntArray977[anInt974] = spriteDrawY;
						anIntArray980[anInt974] = ((Entity) (obj)).anInt1513;
						anIntArray981[anInt974] = ((Entity) (obj)).anInt1531;
						anIntArray982[anInt974] = ((Entity) (obj)).textCycle;
						aStringArray983[anInt974++] = ((Entity) (obj)).textSpoken;
						if (anInt1249 == 0 && ((Entity) (obj)).anInt1531 >= 1
						    && ((Entity) (obj)).anInt1531 <= 3) {
							anIntArray978[anInt974] += 10;
							anIntArray977[anInt974] += 5;
						}
						if (anInt1249 == 0 && ((Entity) (obj)).anInt1531 == 4)
							anIntArray979[anInt974] = 60;
						if (anInt1249 == 0 && ((Entity) (obj)).anInt1531 == 5)
							anIntArray978[anInt974] += 5;
					}
				}
				
				if (((Entity) (obj)).loopCycleStatus > loopCycle) {
					try {
						npcScreenPos(((Entity) (obj)),
						             ((Entity) (obj)).height + 15);
						if (spriteDrawX > -1) {
							int i1 = (((Entity) (obj)).currentHealth * 30)
								/ ((Entity) (obj)).maxHealth;
							if (i1 > 30)
								i1 = 30;
							DrawingArea.drawPixels(5, spriteDrawY - 3,
							                       spriteDrawX - 15, 65280, i1);
							DrawingArea.drawPixels(5, spriteDrawY - 3,
							                       (spriteDrawX - 15) + i1, 0xff0000, 30 - i1);
						}
					} catch (Exception e) {
						Debug.Log(e.Message);
					}
				}
				for (int j1 = 0; j1 < 4; j1++)
				if (((Entity) (obj)).hitsLoopCycle[j1] > loopCycle) {
					npcScreenPos(((Entity) (obj)),
					             ((Entity) (obj)).height / 2);
					if (spriteDrawX > -1) {
						if (j1 == 1)
							spriteDrawY -= 20;
						if (j1 == 2) {
							spriteDrawX -= 15;
							spriteDrawY -= 10;
						}
						if (j1 == 3) {
							spriteDrawX += 15;
							spriteDrawY -= 10;
						}
						hitMarks[((Entity) (obj)).hitMarkTypes[j1]]
						.drawSprite(spriteDrawX - 12,
						            spriteDrawY - 12);
						smallText.drawText(0, ((Entity) (obj)).hitArray[j1] + "",
						                   spriteDrawY + 4, spriteDrawX);
						smallText.drawText(0xffffff, ((Entity) (obj)).hitArray[j1] + "",
						                   spriteDrawY + 3, spriteDrawX - 1);
					}
				}
			}
			for (int k = 0; k < anInt974; k++) {
				int k1 = anIntArray976[k];
				int l1 = anIntArray977[k];
				int j2 = anIntArray979[k];
				int k2 = anIntArray978[k];
				bool flag = true;
				while (flag) {
					flag = false;
					for (int l2 = 0; l2 < k; l2++)
						if (l1 + 2 > anIntArray977[l2] - anIntArray978[l2]
						    && l1 - k2 < anIntArray977[l2] + 2
						    && k1 - j2 < anIntArray976[l2]
						    + anIntArray979[l2]
						    && k1 + j2 > anIntArray976[l2]
						    - anIntArray979[l2]
						   && anIntArray977[l2] - anIntArray978[l2] < l1) {
						l1 = anIntArray977[l2] - anIntArray978[l2];
						flag = true;
					}
					
				}
				spriteDrawX = anIntArray976[k];
				spriteDrawY = anIntArray977[k] = l1;
				String s = aStringArray983[k];
				if (anInt1249 == 0) {
					int i3 = 0xffff00;
					if (anIntArray980[k] < 6)
						i3 = anIntArray965[anIntArray980[k]];
					if (anIntArray980[k] == 6)
						i3 = anInt1265 % 20 >= 10 ? 0xffff00 : 0xff0000;
					if (anIntArray980[k] == 7)
						i3 = anInt1265 % 20 >= 10 ? 65535 : 255;
					if (anIntArray980[k] == 8)
						i3 = anInt1265 % 20 >= 10 ? 0x80ff80 : 45056;
					if (anIntArray980[k] == 9) {
						int j3 = 150 - anIntArray982[k];
						if (j3 < 50)
							i3 = 0xff0000 + 1280 * j3;
						else if (j3 < 100)
							i3 = 0xffff00 - 0x50000 * (j3 - 50);
						else if (j3 < 150)
							i3 = 65280 + 5 * (j3 - 100);
					}
					if (anIntArray980[k] == 10) {
						int k3 = 150 - anIntArray982[k];
						if (k3 < 50)
							i3 = 0xff0000 + 5 * k3;
						else if (k3 < 100)
							i3 = 0xff00ff - 0x50000 * (k3 - 50);
						else if (k3 < 150)
							i3 = (255 + 0x50000 * (k3 - 100)) - 5 * (k3 - 100);
					}
					if (anIntArray980[k] == 11) {
						int l3 = 150 - anIntArray982[k];
						if (l3 < 50)
							i3 = 0xffffff - 0x50005 * l3;
						else if (l3 < 100)
							i3 = 65280 + 0x50005 * (l3 - 50);
						else if (l3 < 150)
							i3 = 0xffffff - 0x50000 * (l3 - 100);
					}
					if (anIntArray981[k] == 0) {
						chatTextDrawingArea.drawText(0, s, spriteDrawY + 1,
						                             spriteDrawX);
						chatTextDrawingArea.drawText(i3, s, spriteDrawY,
						                             spriteDrawX);
					}
					if (anIntArray981[k] == 1) {
						chatTextDrawingArea.method386(0, s, spriteDrawX,
						                              anInt1265, spriteDrawY + 1);
						chatTextDrawingArea.method386(i3, s, spriteDrawX,
						                              anInt1265, spriteDrawY);
					}
					if (anIntArray981[k] == 2) {
						chatTextDrawingArea.method387(spriteDrawX, s,
						                              anInt1265, spriteDrawY + 1, 0);
						chatTextDrawingArea.method387(spriteDrawX, s,
						                              anInt1265, spriteDrawY, i3);
					}
					if (anIntArray981[k] == 3) {
						chatTextDrawingArea.method388(150 - anIntArray982[k],
						                              s, anInt1265, spriteDrawY + 1, spriteDrawX, 0);
						chatTextDrawingArea.method388(150 - anIntArray982[k],
						                              s, anInt1265, spriteDrawY, spriteDrawX, i3);
					}
					if (anIntArray981[k] == 4) {
						int i4 = chatTextDrawingArea.method384(s);
						int k4 = ((150 - anIntArray982[k]) * (i4 + 100)) / 150;
						DrawingArea.setDrawingArea(334, spriteDrawX - 50,
						                           spriteDrawX + 50, 0);
						chatTextDrawingArea.method385(0, s, spriteDrawY + 1,
						                              (spriteDrawX + 50) - k4);
						chatTextDrawingArea.method385(i3, s, spriteDrawY,
						                              (spriteDrawX + 50) - k4);
						DrawingArea.defaultDrawingAreaSize();
					}
					if (anIntArray981[k] == 5) {
						int j4 = 150 - anIntArray982[k];
						int l4 = 0;
						if (j4 < 25)
							l4 = j4 - 25;
						else if (j4 > 125)
							l4 = j4 - 125;
						DrawingArea
							.setDrawingArea(spriteDrawY + 5, 0, 512,
							                spriteDrawY
							                - chatTextDrawingArea.anInt1497
							                - 1);
						chatTextDrawingArea.drawText(0, s,
						                             spriteDrawY + 1 + l4, spriteDrawX);
						chatTextDrawingArea.drawText(i3, s, spriteDrawY + l4,
						                             spriteDrawX);
						DrawingArea.defaultDrawingAreaSize();
					}
				} else {
					chatTextDrawingArea.drawText(0, s, spriteDrawY + 1,
					                             spriteDrawX);
					chatTextDrawingArea.drawText(0xffff00, s, spriteDrawY,
					                             spriteDrawX);
				}
			}
//		} catch (Exception e) {
//		}
		//try{
//			int anInt974 = 0;
//			for(int j = -1; j < playerCount + npcCount; j++) {
//				System.Object obj;
//				if(j == -1)
//					obj = myPlayer;
//				else
//					if(j < playerCount)
//						obj = playerArray[playerIndices[j]];
//				else
//					obj = npcArray[npcIndices[j - playerCount]];
//				if(obj == null || !((Entity) (obj)).isVisible())
//					continue;
//				if (obj is NPC) {
//					EntityDef entityDef = ((NPC) obj).desc;
//					//  if (headname) {
//					//  	String s = entityDef.name;
//					//  	s = s
//					//  		+ combatDiffColor(myPlayer.combatLevel,
//					//  		                  entityDef.combatLevel) + " (level-"
//					//  			+ entityDef.combatLevel + ")";
//					//  	if (entityDef.combatLevel != 0) {
//					//  		npcScreenPos(((Entity) (obj)),
//					//  		             ((Entity) (obj)).height + 15);
//					//  		smallText.method382(0xFFFF33, spriteDrawX, s,
//					//  		                    spriteDrawY - yOffsetForText, true);
//					//  	} else if (entityDef.combatLevel == 0) {
//					//  		npcScreenPos(((Entity) (obj)),
//					//  		             ((Entity) (obj)).height + 15);
//					//  		smallText.method382(0xFFFF33, spriteDrawX,
//					//  		                    entityDef.name, spriteDrawY
//					//  		                    - yOffsetForText, true);
//					//  	}
//					//  }
//					if (entityDef.childrenIDs != null)
//						entityDef = entityDef.method161();
//					if (entityDef == null)
//						continue;
//				}
//				if(j < playerCount) {
//					int l = 30;
//					Player player = (Player)obj;
//					if(player.headIcon >= 0) {
//						npcScreenPos(((Entity) (obj)), ((Entity) (obj)).height + 15);
//						if(spriteDrawX > -1) {
//							if (player.skullIcon < 2) {
//								skullIcons[player.skullIcon].drawSprite(spriteDrawX - 12, spriteDrawY - l);
//								l += 25;
//							}
//							if (player.skullIcon > 1 && player.skullIcon < 7) {
//								//bountyHunter[player.skullIcon].drawSprite(spriteDrawX - 12, spriteDrawY - l);
//								l += 30;
//							}
//							if (player.headIcon < 18) {
//								headIcons[player.headIcon].drawSprite(spriteDrawX - 12, spriteDrawY - l);
//								l += 18;
//							}
//						}
//					}
//					if(j >= 0 && anInt855 == 10 && anInt933 == playerIndices[j]) {
//						npcScreenPos(((Entity) (obj)), ((Entity) (obj)).height + 15);
//						if(spriteDrawX > -1)
//							headIconsHint[player.hintIcon].drawSprite(spriteDrawX - 12, spriteDrawY - l);
//					}
//					//  if (headname) {
//					//  	npcScreenPos(((Entity) (obj)),
//					//  	             ((Entity) (obj)).height + 15);
//					//  	int col = 0x00ff00;// Green
//					//  	aTextDrawingArea_1271.drawText(col, player.name,
//					//  	                               spriteDrawY - yOffsetForText, spriteDrawX);
//					//  }
//				} else {
//					EntityDef entityDef_1 = ((NPC)obj).desc;
//					if(entityDef_1.anInt75 >= 0 && entityDef_1.anInt75 < headIcons.Length) {
//						npcScreenPos(((Entity) (obj)), ((Entity) (obj)).height + 15);
//						if(spriteDrawX > -1)
//							headIcons[entityDef_1.anInt75].drawSprite(spriteDrawX - 12, spriteDrawY - 30);
//					}
//					
//					if(anInt855 == 1 && anInt1222 == npcIndices[j - playerCount] && loopCycle % 20 < 10) {
//						npcScreenPos(((Entity) (obj)), ((Entity) (obj)).height + 15);
//						if(spriteDrawX > -1)
//							headIconsHint[0].drawSprite(spriteDrawX - 12, spriteDrawY - 28);
//					}
//				}
//				if(((Entity) (obj)).textSpoken != null && (j >= playerCount || publicChatMode == 0 || publicChatMode == 3 || publicChatMode == 1 && isFriendOrSelf(((Player)obj).name)))
//				{
//					npcScreenPos(((Entity) (obj)), ((Entity) (obj)).height);
//					if(spriteDrawX > -1 && anInt974 < anInt975)
//					{
//						anIntArray979[anInt974] = chatTextDrawingArea.method384(((Entity) (obj)).textSpoken) / 2;
//						anIntArray978[anInt974] = chatTextDrawingArea.anInt1497;
//						anIntArray976[anInt974] = spriteDrawX;
//						anIntArray977[anInt974] = spriteDrawY;
//						anIntArray980[anInt974] = ((Entity) (obj)).anInt1513;
//						anIntArray981[anInt974] = ((Entity) (obj)).anInt1531;
//						anIntArray982[anInt974] = ((Entity) (obj)).textCycle;
//						aStringArray983[anInt974++] = ((Entity) (obj)).textSpoken;
//						if(anInt1249 == 0 && ((Entity) (obj)).anInt1531 >= 1 && ((Entity) (obj)).anInt1531 <= 3)
//						{
//							anIntArray978[anInt974] += 10;
//							anIntArray977[anInt974] += 5;
//						}
//						if(anInt1249 == 0 && ((Entity) (obj)).anInt1531 == 4)
//							anIntArray979[anInt974] = 60;
//						if(anInt1249 == 0 && ((Entity) (obj)).anInt1531 == 5)
//							anIntArray978[anInt974] += 5;
//					}
//				}
//				if (((Entity) (obj)).loopCycleStatus > loopCycle) {
//					try {
//						npcScreenPos(((Entity) (obj)),
//						             ((Entity) (obj)).height + 15);
//						if (spriteDrawX > -1) {
//							int i1 = (((Entity) (obj)).currentHealth * 30)
//								/ ((Entity) (obj)).maxHealth;
//							if (i1 > 30) {
//								i1 = 30;
//							}
//							int HpPercent = (((Entity) (obj)).currentHealth * 56)
//								/ ((Entity) (obj)).maxHealth;
//							if (HpPercent > 56) {
//								HpPercent = 56;
//							}
//							//  if (!HPBarToggle == true) {
//							//  	DrawingArea.method336(5, spriteDrawY - 3,
//							//  	                      spriteDrawX - 15, 65280, i1);
//							//  	DrawingArea.method336(5, spriteDrawY - 3,
//							//  	                      (spriteDrawX - 15) + i1, 0xff0000,
//							//  	                      30 - i1);
//							//  } else {
//								HPBarEmpty.drawSprite(spriteDrawX - 28,
//								                      spriteDrawY - 3);
//								HPBarFull = 	new RS2Sharp.Sprite(client.ReadAllBytes(signlink.findcachedir() + "Sprites/HITPOINTS_0.png"));
//								HPBarFull.drawSprite(spriteDrawX - 28,
//								                     spriteDrawY - 3);
//							//}
//							int health = (int) (((double) ((Entity) (obj)).currentHealth / (double) ((Entity) (obj)).maxHealth) * 100D);
//							int col = 65280;
//							if (health <= 100 && health >= 75)
//								col = 65280;
//							else if (health <= 74 && health >= 50)
//								col = 0xffff00;
//							else if (health <= 49 && health >= 25)
//								col = 0xfca607;
//							else if (health <= 24 && health >= 0)
//								col = 0xf50d0d;
//							//  if (headhp) {
//							//  	smallText
//							//  		.method382(
//							//  			col,
//							//  			spriteDrawX,
//							//  			(new StringBuilder())
//							//  			.append(
//							//  			((Entity) (Entity) obj).currentHealth)
//							//  			.append("/")
//							//  			.append(
//							//  			((Entity) (Entity) obj).maxHealth)
//							//  			.toString(),
//							//  			spriteDrawY - 5, true);
//							//  }
//						}
//					} catch (Exception e) {
//					}
//				}
//				
//				for(int k1 = 0; k1 < 4; k1++)
//				if (((Entity) (obj)).hitsLoopCycle[k1] > loopCycle) {
//					npcScreenPos(((Entity) (obj)),((Entity) (obj)).height / 2);
//					if(k1 == 1) {
//						spriteDrawY -= 20;
//					}
//					if(k1 == 2) {
//						spriteDrawX -= 0;
//						spriteDrawY += 20;
//					}
//					if(k1 == 3) {
//						spriteDrawX -= 0;
//						spriteDrawY += 40;
//					}
//					if(((Entity)(Entity)obj).hitsLoopCycle[k1] > loopCycle)
//					{
//						spriteDrawY -= 30;
//						int hitCycle = ((Entity)(Entity)obj).hitsLoopCycle[k1] - loopCycle;
//						int move1 = (int)Mathf.Round((float)hitCycle * 3.5f);
//						int move2 = (int)Mathf.Round((float)hitCycle / 6f);
//						int fade = (int)Mathf.Round((float)hitCycle * 3.7101449275362319f);	 
//						if (((Entity)(Entity)obj).hitArray[k1] >= 399) {
//							critHit.drawSpriteWithOpacity(spriteDrawX - 12,(spriteDrawY - 13) + move2, move1);
//							//newBoldFont.setTrans(0, 0xffffff, fade);
//							//newBoldFont.drawCenteredString((new StringBuilder()).append(String.valueOf(((Entity)obj).hitArray[k1])).append("").toString(), spriteDrawX + 5, spriteDrawY + 4 + move2);
//						} 
//						if (((Entity)(Entity)obj).hitArray[k1] >= 100 && ((Entity)(Entity)obj).hitArray[k1] < 399) {
//							bigHit.drawSpriteWithOpacity(spriteDrawX - 12,(spriteDrawY - 13) + move2, move1);
//							//newSmallFont.setTrans(0, 0xffffff, fade);
//							//newSmallFont.drawCenteredString((new StringBuilder()).append(String.valueOf(((Entity)obj).hitArray[k1])).append("").toString(), spriteDrawX + 2, spriteDrawY + 4 + move2);
//						} 
//						if (((Entity)(Entity)obj).hitArray[k1] >= 1 && ((Entity)(Entity)obj).hitArray[k1] < 100) {
//							oldHit.drawSpriteWithOpacity(spriteDrawX - 12,(spriteDrawY - 13) + move2, move1);
//							//newSmallFont.setTrans(0, 0xffffff, fade);
//							//newSmallFont.drawCenteredString((new StringBuilder()).append(String.valueOf(((Entity)obj).hitArray[k1])).append("").toString(), spriteDrawX, spriteDrawY + 4 + move2);
//						} 
//						if (((Entity) (obj)).hitArray[k1] == 0 || ((Entity) (obj)).hitType[k1] == 3)
//							{}//combatIcons[3].drawSpriteWithOpacity(spriteDrawX - 12, (spriteDrawY - 12) + move2, move1);
//						else {
//							//combatIcons[((Entity) (obj)).hitType[k1]].drawSpriteWithOpacity(spriteDrawX - 35, (spriteDrawY - 12) + move2, move1);
//						}
//					}
//				}
//			}		
//			for (int k = 0; k < anInt974; k++) {
//				int k1 = anIntArray976[k];
//				int l1 = anIntArray977[k];
//				int j2 = anIntArray979[k];
//				int k2 = anIntArray978[k];
//				bool flag = true;
//				while (flag) {
//					flag = false;
//					for (int l2 = 0; l2 < k; l2++)
//						if (l1 + 2 > anIntArray977[l2] - anIntArray978[l2]
//						    && l1 - k2 < anIntArray977[l2] + 2
//						    && k1 - j2 < anIntArray976[l2]
//						    + anIntArray979[l2]
//						    && k1 + j2 > anIntArray976[l2]
//						    - anIntArray979[l2]
//						   && anIntArray977[l2] - anIntArray978[l2] < l1) {
//						l1 = anIntArray977[l2] - anIntArray978[l2];
//						flag = true;
//					}
//					
//				}
//				spriteDrawX = anIntArray976[k];
//				spriteDrawY = anIntArray977[k] = l1;
//				String s = aStringArray983[k];
//				if (anInt1249 == 0) {
//					int i3 = 0xffff00;
//					if (anIntArray980[k] < 6)
//						i3 = anIntArray965[anIntArray980[k]];
//					if (anIntArray980[k] == 6)
//						i3 = anInt1265 % 20 >= 10 ? 0xffff00 : 0xff0000;
//					if (anIntArray980[k] == 7)
//						i3 = anInt1265 % 20 >= 10 ? 65535 : 255;
//					if (anIntArray980[k] == 8)
//						i3 = anInt1265 % 20 >= 10 ? 0x80ff80 : 45056;
//					if (anIntArray980[k] == 9) {
//						int j3 = 150 - anIntArray982[k];
//						if (j3 < 50)
//							i3 = 0xff0000 + 1280 * j3;
//						else if (j3 < 100)
//							i3 = 0xffff00 - 0x50000 * (j3 - 50);
//						else if (j3 < 150)
//							i3 = 65280 + 5 * (j3 - 100);
//					}
//					if (anIntArray980[k] == 10) {
//						int k3 = 150 - anIntArray982[k];
//						if (k3 < 50)
//							i3 = 0xff0000 + 5 * k3;
//						else if (k3 < 100)
//							i3 = 0xff00ff - 0x50000 * (k3 - 50);
//						else if (k3 < 150)
//							i3 = (255 + 0x50000 * (k3 - 100)) - 5 * (k3 - 100);
//					}
//					if (anIntArray980[k] == 11) {
//						int l3 = 150 - anIntArray982[k];
//						if (l3 < 50)
//							i3 = 0xffffff - 0x50005 * l3;
//						else if (l3 < 100)
//							i3 = 65280 + 0x50005 * (l3 - 50);
//						else if (l3 < 150)
//							i3 = 0xffffff - 0x50000 * (l3 - 100);
//					}
//					if (anIntArray981[k] == 0) {
//						chatTextDrawingArea.drawText(0, s, spriteDrawY + 1,
//						                             spriteDrawX);
//						chatTextDrawingArea.drawText(i3, s, spriteDrawY,
//						                             spriteDrawX);
//					}
//					if (anIntArray981[k] == 1) {
//						chatTextDrawingArea.method386(0, s, spriteDrawX,
//						                              anInt1265, spriteDrawY + 1);
//						chatTextDrawingArea.method386(i3, s, spriteDrawX,
//						                              anInt1265, spriteDrawY);
//					}
//					if (anIntArray981[k] == 2) {
//						chatTextDrawingArea.method387(spriteDrawX, s,
//						                              anInt1265, spriteDrawY + 1, 0);
//						chatTextDrawingArea.method387(spriteDrawX, s,
//						                              anInt1265, spriteDrawY, i3);
//					}
//					if (anIntArray981[k] == 3) {
//						chatTextDrawingArea.method388(150 - anIntArray982[k],
//						                              s, anInt1265, spriteDrawY + 1, spriteDrawX, 0);
//						chatTextDrawingArea.method388(150 - anIntArray982[k],
//						                              s, anInt1265, spriteDrawY, spriteDrawX, i3);
//					}
//					if (anIntArray981[k] == 4) {
//						int i4 = chatTextDrawingArea.method384(s);
//						int k4 = ((150 - anIntArray982[k]) * (i4 + 100)) / 150;
//						DrawingArea.setDrawingArea(334, spriteDrawX - 50,
//						                           spriteDrawX + 50, 0);
//						chatTextDrawingArea.method385(0, s, spriteDrawY + 1,
//						                              (spriteDrawX + 50) - k4);
//						chatTextDrawingArea.method385(i3, s, spriteDrawY,
//						                              (spriteDrawX + 50) - k4);
//						DrawingArea.defaultDrawingAreaSize();
//					}
//					if (anIntArray981[k] == 5) {
//						int j4 = 150 - anIntArray982[k];
//						int l4 = 0;
//						if (j4 < 25)
//							l4 = j4 - 25;
//						else if (j4 > 125)
//							l4 = j4 - 125;
//						DrawingArea
//							.setDrawingArea(spriteDrawY + 5, 0, 512,
//							                spriteDrawY
//							                - chatTextDrawingArea.anInt1497
//							                - 1);
//						chatTextDrawingArea.drawText(0, s,
//						                             spriteDrawY + 1 + l4, spriteDrawX);
//						chatTextDrawingArea.drawText(i3, s, spriteDrawY + l4,
//						                             spriteDrawX);
//						DrawingArea.defaultDrawingAreaSize();
//					}
//				} else {
//					chatTextDrawingArea.drawText(0, s, spriteDrawY + 1,
//					                             spriteDrawX);
//					chatTextDrawingArea.drawText(0xffff00, s, spriteDrawY,
//					                             spriteDrawX);
//				}
//			}
		//} catch (Exception e) {
		//}
		}
	
	private void delFriend(long l) {
		try {
			if (l == 0L)
				return;
			for (int i = 0; i < friendsCount; i++) {
				if (friendsListAsLongs[i] != l)
					continue;
				friendsCount--;
				needDrawTabArea = true;
				for (int j = i; j < friendsCount; j++) {
					friendsList[j] = friendsList[j + 1];
					friendsNodeIDs[j] = friendsNodeIDs[j + 1];
					friendsListAsLongs[j] = friendsListAsLongs[j + 1];
				}
				
				stream.createFrame(215);
				stream.writeQWord(l);
				break;
			}
		} catch (Exception runtimeexception) {
			signlink.reporterror("18622, " + false + ", " + l + ", "
			                     + runtimeexception.ToString());
			throw new Exception();
		}
	}
	
	public void drawSideIcons() {
		int xOffset = myWidth - 247;
		int yOffset = myHeight - 336;
		/* Top sideIcons */
		if (tabInterfaceIDs[0] != -1)// attack
			sideIcons[0].drawSprite(12+ xOffset, 8+ yOffset);
		if (tabInterfaceIDs[1] != -1)// stat
			sideIcons[1].drawSprite(45+ xOffset, 5+ yOffset);
		if (tabInterfaceIDs[2] != -1)// quest
			sideIcons[2].drawSprite(79+ xOffset, 6+ yOffset);
		if (tabInterfaceIDs[3] != -1)// inventory
			sideIcons[3].drawSprite(111+ xOffset, 5+ yOffset);
		if (tabInterfaceIDs[4] != -1)// equipment
			sideIcons[4].drawSprite(143+ xOffset, 1+ yOffset);
		if (tabInterfaceIDs[5] != -1)// prayer
			sideIcons[5].drawSprite(176+ xOffset, 2+ yOffset);
		if (tabInterfaceIDs[6] != -1)// magic
			sideIcons[6].drawSprite(210+ xOffset, 6+ yOffset);
		/* Bottom sideIcons */
		// if (tabInterfaceIDs[7] != -1)// clan
		// sideIcons[7].drawSprite(11, 303);
		if (tabInterfaceIDs[8] != -1)// friends
			sideIcons[8].drawSprite(46+ xOffset, 306+ yOffset);
		if (tabInterfaceIDs[9] != -1)// ignore
			sideIcons[9].drawSprite(79+ xOffset, 306+ yOffset);
		if (tabInterfaceIDs[7] != -1)// options
			sideIcons[7].drawSprite(113+ xOffset, 304+ yOffset);
		if (tabInterfaceIDs[11] != -1)// options
			sideIcons[11].drawSprite(145+ xOffset, 304+ yOffset);
		if (tabInterfaceIDs[12] != -1)// emotes
			sideIcons[12].drawSprite(181+ xOffset, 302+ yOffset);
		if (tabInterfaceIDs[13] != -1)// music
			sideIcons[13].drawSprite(213+ xOffset, 303+ yOffset);
	}
	
	public void drawRedStones() {
		int xOffset = myWidth - 247;
		int yOffset = myHeight - 336;
		if (tabInterfaceIDs[tabID] != -1) {
			switch (tabID) {
			case 0:
				redStones[0].drawSprite(3+ xOffset, 0+ yOffset);
				break;
			case 1:
				redStones[4].drawSprite(41+ xOffset, 0+ yOffset);
				break;
			case 2:
				redStones[4].drawSprite(74+ xOffset, 0+ yOffset);
				break;
			case 3:
				redStones[4].drawSprite(107+ xOffset, 0+ yOffset);
				break;
			case 4:
				redStones[4].drawSprite(140+ xOffset, 0+ yOffset);
				break;
			case 5:
				redStones[4].drawSprite(173+ xOffset, 0+ yOffset);
				break;
			case 6:
				redStones[1].drawSprite(206+ xOffset, 0+ yOffset);
				break;
			case 7:
				redStones[2].drawSprite(3+ xOffset, 298+ yOffset);
				break;
			case 8:
				redStones[4].drawSprite(41+ xOffset, 298+ yOffset);
				break;
			case 9:
				redStones[4].drawSprite(74+ xOffset, 298+ yOffset);
				break;
			case 13:
				redStones[4].drawSprite(107+ xOffset, 298+ yOffset);
				break;
			case 11:
				redStones[4].drawSprite(140+ xOffset, 298+ yOffset);
				break;
			case 12:
				redStones[4].drawSprite(173+ xOffset, 298+ yOffset);
				break;
				/*
			 * case 10: redStones[3].drawSprite(206, 298); break;
			 */
			}
		}
	}
	
	private void drawTabArea() {

		RS2Sharp.Texture.lineOffsets = anIntArray1181;
		int yOffset = myHeight - 335;
		int xOffset = myWidth - 249;
		tabArea.drawSprite(0 + xOffset + 5, 0 + yOffset -2);
		if(invOverlayInterfaceID == -1) {
			drawRedStones();
			drawSideIcons();
		}
		if(invOverlayInterfaceID != -1)
			drawInterface(0, 31 + xOffset, RSInterface.interfaceCache[invOverlayInterfaceID], 37 + yOffset);
		else if(tabInterfaceIDs[tabID] != -1 && RSInterface.interfaceCache.Length > tabInterfaceIDs[tabID])
			drawInterface(0, 31 + xOffset, RSInterface.interfaceCache[tabInterfaceIDs[tabID]], 37 + yOffset);
		if(menuOpen)
			drawMenu(0, 0);
		RS2Sharp.Texture.lineOffsets = anIntArray1182;
// 
// 	int xOffset = myWidth - 241;
// 		int yOffset = myHeight - 336;
// 		RS2Sharp.Texture.lineOffsets = anIntArray1181;
// 		bool changeTabArea = true;
// 		bool transparentTabArea = true;
// 		bool showTabComponents = true;
// 		if (!changeTabArea){
// 			//DrawingArea.method335(0x3E3529, myHeight - 304, 195, 270, transparentTabArea ? 80 : 256, myWidth - 217);
// 			//cacheSprite[47].drawSprite(xOffset, yOffset);
// 		} else {
// 			if (myWidth >= 1000) {
// 				if (showTabComponents) {
// 					DrawingArea.method335(0x3E3529, myHeight - 304, 197, 265, transparentTabArea ? 80 : 256, myWidth - 197);
// 					//RSInterface.interfaceCache[invOverlayInterfaceID].drawSprite(myWidth - 204, myHeight - 311);
// 						if(invOverlayInterfaceID != -1) drawInterface(0, 28,
// 					RSInterface.interfaceCache[invOverlayInterfaceID], 37);
// 				}
// 				for (int x = myWidth - 417, y = myHeight - 37, index = 0; x <= myWidth - 30 && index < 13; x += 32, index++) {
// 					//RSInterface.interfaceCache[tabInterfaceIDs[tabID]].drawSprite(x, y);
// 					if(tabID != -1) drawInterface(0, 28,
// 					RSInterface.interfaceCache[tabInterfaceIDs[tabID]], 37);
// 				}
// 			} else if (myWidth < 1000) {
// 				if (showTabComponents) {
// 					DrawingArea.method335(0x3E3529, myHeight - 341, 195, 265, transparentTabArea ? 80 : 256, myWidth - 197);
// 					//RSInterface.interfaceCache[invOverlayInterfaceID].drawSprite(myWidth - 204, myHeight - 348);
// 						if(invOverlayInterfaceID != -1) drawInterface(0, 28,
// 					RSInterface.interfaceCache[invOverlayInterfaceID], 37);
// 				}
// 				for (int x = myWidth - 226, y = myHeight - 73, index = 0; x <= myWidth - 32 && index < 7; x += 32, index++) {
// 					//RSInterface.interfaceCache[tabInterfaceIDs[tabID]].drawSprite(x, y);
// 					if(tabID != -1) drawInterface(0, 28,
// 					RSInterface.interfaceCache[tabInterfaceIDs[tabID]], 37);
// 				}
// 				for (int x = myWidth - 226, y = myHeight - 37, index = 0; x <= myWidth - 32 && index < 7; x += 32, index++) {
// 					//RSInterface.interfaceCache[tabInterfaceIDs[tabID]].drawSprite(x, y);
// 					if(tabID != -1) drawInterface(0, 28,
// 					RSInterface.interfaceCache[tabInterfaceIDs[tabID]], 37);
// 				}
// 			}
// 		}
// 		if (invOverlayInterfaceID == -1) {
// 			drawRedStones();
// 			drawSideIcons();
// 		}
// 		if (showTabComponents) {
// 			int x = myWidth - 215;
// 			int y = myHeight - 299;
// 			if (changeTabArea) {
// 				x = myWidth - 196;
// 				y = myWidth >= 1000 ? myHeight - 303 : myHeight - 340;
// 			}
// 			if (invOverlayInterfaceID != -1) {
// 				drawInterface(0, x, RSInterface.interfaceCache[invOverlayInterfaceID], y);
// 			} else if (tabInterfaceIDs[tabID] != -1) {
// 				drawInterface(0, x, RSInterface.interfaceCache[tabInterfaceIDs[tabID]], y);
// 			}
// 		}
// 		if (menuOpen) {
// 			drawMenu(0, 0);
// 		}
// 		RS2Sharp.Texture.lineOffsets = anIntArray1182;
	}
	
	private void method37(int j) {
		if (!lowMem) {
			if (RS2Sharp.Texture.anIntArray1480[17] >= j) {
				Background background = RS2Sharp.Texture.aBackgroundArray1474s[17];
				int k = background.anInt1452 * background.anInt1453 - 1;
				int j1 = background.anInt1452 * anInt945 * 2;
				byte[] abyte0 = background.aByteArray1450;
				byte[] abyte3 = aByteArray912;
				for (int i2 = 0; i2 <= k; i2++)
					abyte3[i2] = abyte0[i2 - j1 & k];
				
				background.aByteArray1450 = abyte3;
				aByteArray912 = abyte0;
				RS2Sharp.Texture.method370(17);
				anInt854++;
				if (anInt854 > 1235) {
					anInt854 = 0;
					stream.createFrame(226);
					stream.writeWordBigEndian(0);
					int l2 = stream.currentOffset;
					stream.writeWord(58722);
					stream.writeWordBigEndian(240);
					stream.writeWord((int) (RS2Sharp.Random.Next(65536)));
					stream.writeWordBigEndian((int) (RS2Sharp.Random.Next(256)));
					if ((int) (RS2Sharp.Random.Next(2)) == 0)
						stream.writeWord(51825);
					stream.writeWordBigEndian((int) (RS2Sharp.Random.Next(256)));
					stream.writeWord((int) (RS2Sharp.Random.Next(65536)));
					stream.writeWord(7130);
					stream.writeWord((int) (RS2Sharp.Random.Next(65536)));
					stream.writeWord(61657);
					stream.writeBytes(stream.currentOffset - l2);
				}
			}
			if (RS2Sharp.Texture.anIntArray1480[24] >= j) {
				Background background_1 = RS2Sharp.Texture.aBackgroundArray1474s[24];
				int l = background_1.anInt1452 * background_1.anInt1453 - 1;
				int k1 = background_1.anInt1452 * anInt945 * 2;
				byte[] abyte1 = background_1.aByteArray1450;
				byte[] abyte4 = aByteArray912;
				for (int j2 = 0; j2 <= l; j2++)
					abyte4[j2] = abyte1[j2 - k1 & l];
				
				background_1.aByteArray1450 = abyte4;
				aByteArray912 = abyte1;
				RS2Sharp.Texture.method370(24);
			}
			if (RS2Sharp.Texture.anIntArray1480[34] >= j) {
				Background background_2 = RS2Sharp.Texture.aBackgroundArray1474s[34];
				int i1 = background_2.anInt1452 * background_2.anInt1453 - 1;
				int l1 = background_2.anInt1452 * anInt945 * 2;
				byte[] abyte2 = background_2.aByteArray1450;
				byte[] abyte5 = aByteArray912;
				for (int k2 = 0; k2 <= i1; k2++)
					abyte5[k2] = abyte2[k2 - l1 & i1];
				
				background_2.aByteArray1450 = abyte5;
				aByteArray912 = abyte2;
				RS2Sharp.Texture.method370(34);
			}
		}
	}
	
	private void method38() {
		for (int i = -1; i < playerCount; i++) {
			int j;
			if (i == -1)
				j = myPlayerIndex;
			else
				j = playerIndices[i];
			Player player = playerArray[j];
			if (player != null && player.textCycle > 0) {
				player.textCycle--;
				if (player.textCycle == 0)
					player.textSpoken = null;
			}
		}
		for (int k = 0; k < npcCount; k++) {
			int l = npcIndices[k];
			NPC npc = npcArray[l];
			if (npc != null && npc.textCycle > 0) {
				npc.textCycle--;
				if (npc.textCycle == 0)
					npc.textSpoken = null;
			}
		}
	}
	
	private void calcCameraPos() {
		int i = anInt1098 * 128 + 64;
		int j = anInt1099 * 128 + 64;
		int k = method42(plane, j, i) - anInt1100;
		if (xCameraPos < i) {
			xCameraPos += anInt1101 + ((i - xCameraPos) * anInt1102) / 1000;
			if (xCameraPos > i)
				xCameraPos = i;
		}
		if (xCameraPos > i) {
			xCameraPos -= anInt1101 + ((xCameraPos - i) * anInt1102) / 1000;
			if (xCameraPos < i)
				xCameraPos = i;
		}
		if (zCameraPos < k) {
			zCameraPos += anInt1101 + ((k - zCameraPos) * anInt1102) / 1000;
			if (zCameraPos > k)
				zCameraPos = k;
		}
		if (zCameraPos > k) {
			zCameraPos -= anInt1101 + ((zCameraPos - k) * anInt1102) / 1000;
			if (zCameraPos < k)
				zCameraPos = k;
		}
		if (yCameraPos < j) {
			yCameraPos += anInt1101 + ((j - yCameraPos) * anInt1102) / 1000;
			if (yCameraPos > j)
				yCameraPos = j;
		}
		if (yCameraPos > j) {
			yCameraPos -= anInt1101 + ((yCameraPos - j) * anInt1102) / 1000;
			if (yCameraPos < j)
				yCameraPos = j;
		}
		i = anInt995 * 128 + 64;
		j = anInt996 * 128 + 64;
		k = method42(plane, j, i) - anInt997;
		int l = i - xCameraPos;
		int i1 = k - zCameraPos;
		int j1 = j - yCameraPos;
		int k1 = (int) Mathf.Sqrt(l * l + j1 * j1);
		int l1 = (int) (Mathf.Atan2(i1, k1) * 325.94900000000001D) & 0x7ff;
		int i2 = (int) (Mathf.Atan2(l, j1) * -325.94900000000001D) & 0x7ff;
		if (l1 < 128)
			l1 = 128;
		if (l1 > 383)
			l1 = 383;
		if (yCameraCurve < l1) {
			yCameraCurve += anInt998 + ((l1 - yCameraCurve) * anInt999) / 1000;
			if (yCameraCurve > l1)
				yCameraCurve = l1;
		}
		if (yCameraCurve > l1) {
			yCameraCurve -= anInt998 + ((yCameraCurve - l1) * anInt999) / 1000;
			if (yCameraCurve < l1)
				yCameraCurve = l1;
		}
		int j2 = i2 - xCameraCurve;
		if (j2 > 1024)
			j2 -= 2048;
		if (j2 < -1024)
			j2 += 2048;
		if (j2 > 0) {
			xCameraCurve += anInt998 + (j2 * anInt999) / 1000;
			xCameraCurve &= 0x7ff;
		}
		if (j2 < 0) {
			xCameraCurve -= anInt998 + (-j2 * anInt999) / 1000;
			xCameraCurve &= 0x7ff;
		}
		int k2 = i2 - xCameraCurve;
		if (k2 > 1024)
			k2 -= 2048;
		if (k2 < -1024)
			k2 += 2048;
		if (k2 < 0 && j2 > 0 || k2 > 0 && j2 < 0)
			xCameraCurve = i2;
	}
	
	private void drawMenu(int xOffset, int yOffset) {
		int i = menuOffsetX - (xOffset - 4);
		int j = (-yOffset + 4) + (menuOffsetY);
		int k = menuWidth;
		int l = menuHeight + 1;//anInt952
		//DrawingArea.drawPixels(height, yPos, xPos, color, width);
		//DrawingArea.fillPixels(xPos, width, height, color, yPos);
		DrawingArea.drawPixels(l - 4, j + 2, i, 0x706a5e, k);
		DrawingArea.drawPixels(l - 2, j + 1, i + 1, 0x706a5e, k - 2);
		DrawingArea.drawPixels(l, j, i + 2, 0x706a5e, k - 4);
		DrawingArea.drawPixels(l - 2, j + 1, i + 3, 0x2d2822, k - 6);
		DrawingArea.drawPixels(l - 4, j + 2, i + 2, 0x2d2822, k - 4);
		DrawingArea.drawPixels(l - 6, j + 3, i + 1, 0x2d2822, k - 2);
		DrawingArea.drawPixels(l - 22, j + 19, i + 2, 0x524a3d, k - 4);
		DrawingArea.drawPixels(l - 22, j + 20, i + 3, 0x524a3d, k - 6);
		DrawingArea.drawPixels(l - 23, j + 20, i + 3, 0x2b271c, k - 6);
		DrawingArea.fillPixels(i + 3, k - 6, 1, 0x2a291b, j + 2);
		DrawingArea.fillPixels(i + 2, k - 4, 1, 0x2a261b, j + 3);
		DrawingArea.fillPixels(i + 2, k - 4, 1, 0x252116, j + 4);
		DrawingArea.fillPixels(i + 2, k - 4, 1, 0x211e15, j + 5);
		DrawingArea.fillPixels(i + 2, k - 4, 1, 0x1e1b12, j + 6);
		DrawingArea.fillPixels(i + 2, k - 4, 1, 0x1a170e, j + 7);
		DrawingArea.fillPixels(i + 2, k - 4, 2, 0x15120b, j + 8);
		DrawingArea.fillPixels(i + 2, k - 4, 1, 0x100d08, j + 10);
		DrawingArea.fillPixels(i + 2, k - 4, 1, 0x090a04, j + 11);
		DrawingArea.fillPixels(i + 2, k - 4, 1, 0x080703, j + 12);
		DrawingArea.fillPixels(i + 2, k - 4, 1, 0x090a04, j + 13);
		DrawingArea.fillPixels(i + 2, k - 4, 1, 0x070802, j + 14);
		DrawingArea.fillPixels(i + 2, k - 4, 1, 0x090a04, j + 15);
		DrawingArea.fillPixels(i + 2, k - 4, 1, 0x070802, j + 16);
		DrawingArea.fillPixels(i + 2, k - 4, 1, 0x090a04, j + 17);
		DrawingArea.fillPixels(i + 2, k - 4, 1, 0x2a291b, j + 18);
		DrawingArea.fillPixels(i + 3, k - 6, 1, 0x564943, j + 19);
		chatTextDrawingArea.method385(0xc6b895, "Choose Option", j + 14, i + 3);
		int j1 = base.mouseX - (xOffset);
		int k1 = (-yOffset) + base.mouseY;
		if(menuScreenArea == 0) {
			j1 -= 4;
			k1 -= 4;
		}
		if(menuScreenArea == 1) {
			j1 -= 519;
			k1 -= 168;
		}
		if(menuScreenArea == 2) {
			j1 -= 17;
			k1 -= 338;
		}
		if(menuScreenArea == 3) {
			j1 -= 515;
			k1 -= 0;
		}
		for(int l1 = 0; l1 < menuActionRow; l1++) {
			int i2 = j + 31 + (menuActionRow - 1 - l1) * 15;
			int j2 = 0xc6b895;
			if (j1 > i && j1 < i + k && k1 > i2 - 13 && k1 < i2 + 3) {
				DrawingArea.drawPixels(15, i2 - 11, i + 3, 0x6f695d, menuWidth - 6);
				j2 = 0xeee5c6;
			}
			chatTextDrawingArea.method389(true, i + 4, j2, menuActionName[l1], i2 + 1);
		}
	}
	
	private void addFriend(long l) {
		try {
			if (l == 0L)
				return;
			if (friendsCount >= 100 && anInt1046 != 1) {
				pushMessage(
					"Your friendlist is full. Max of 100 for free users, and 200 for members",
					0, "");
				return;
			}
			if (friendsCount >= 200) {
				pushMessage(
					"Your friendlist is full. Max of 100 for free users, and 200 for members",
					0, "");
				return;
			}
			String s = TextClass.fixName(TextClass.nameForLong(l));
			for (int i = 0; i < friendsCount; i++)
			if (friendsListAsLongs[i] == l) {
				pushMessage(s + " is already on your friend list", 0, "");
				return;
			}
			for (int j = 0; j < ignoreCount; j++)
			if (ignoreListAsLongs[j] == l) {
				pushMessage("Please remove " + s
				            + " from your ignore list first", 0, "");
				return;
			}
			
			if (s.Equals(myPlayer.name)) {
				return;
			} else {
				friendsList[friendsCount] = s;
				friendsListAsLongs[friendsCount] = l;
				friendsNodeIDs[friendsCount] = 0;
				friendsCount++;
				needDrawTabArea = true;
				stream.createFrame(188);
				stream.writeQWord(l);
				return;
			}
		} catch (Exception runtimeexception) {
			signlink.reporterror("15283, " + (byte) 68 + ", " + l + ", "
			                     + runtimeexception.ToString());
		}
		throw new Exception();
	}
	
	private int method42(int i, int j, int k) {
		int l = k >> 7;
		int i1 = j >> 7;
		if (l < 0 || i1 < 0 || l > 103 || i1 > 103)
			return 0;
		int j1 = i;
		if (j1 < 3 && (byteGroundArray[1][l][i1] & 2) == 2)
			j1++;
		int k1 = k & 0x7f;
		int l1 = j & 0x7f;
		int i2 = intGroundArray[j1][l][i1] * (128 - k1)
			+ intGroundArray[j1][l + 1][i1] * k1 >> 7;
		int j2 = intGroundArray[j1][l][i1 + 1] * (128 - k1)
			+ intGroundArray[j1][l + 1][i1 + 1] * k1 >> 7;
		return i2 * (128 - l1) + j2 * l1 >> 7;
	}
	
	private static String intToKOrMil(int j) {
		if (j < 0x186a0)
			return ""+(j);
		if (j < 0x989680)
			return j / 1000 + "K";
		else
			return j / 0xf4240 + "M";
	}
	
	private void resetLogout() {
		try {
			if (socketStream != null)
				socketStream.close();
		} catch (Exception _ex) {
		}
		//socketStream = null;
		loggedIn = false;
		loginScreenState = 0;
		// myUsername = "";
		// myPassword = "";
		unlinkMRUNodes();
		//worldController.initToNull();
		for (int i = 0; i < 4; i++)
			aClass11Array1230[i].method210();
		System.GC.Collect();
		stopMidi();
		currentSong = -1;
		nextSong = -1;
		prevSong = 0;
	}
	
	private void method45() {
		aBoolean1031 = true;
		for (int j = 0; j < 7; j++) {
			anIntArray1065[j] = -1;
			for (int k = 0; k < IDK.length; k++) {
				if (IDK.cache[k].aBoolean662
				    || IDK.cache[k].anInt657 != j + (aBoolean1047 ? 0 : 7))
					continue;
				anIntArray1065[j] = k;
				break;
			}
		}
	}
	
	private void method46(int i, RS2Sharp.Stream stream) {
//		while (stream.bitPosition + 21 < i * 8) {
//			int k = stream.readBits(14);
//			if (k == 16383)
//				break;
//			if (npcArray[k] == null)
//				npcArray[k] = new NPC();
//			NPC npc = npcArray[k];
//			npcIndices[npcCount++] = k;
//			npc.anInt1537 = loopCycle;
//			int l = stream.readBits(5);
//			if (l > 15)
//				l -= 32;
//			int i1 = stream.readBits(5);
//			if (i1 > 15)
//				i1 -= 32;
//			int j1 = stream.readBits(1);
//			npc.desc = EntityDef.forID(stream.readBits(14));
//			int k1 = stream.readBits(1);
//			if (k1 == 1)
//				anIntArray894[anInt893++] = k;
//			npc.anInt1540 = npc.desc.aByte68;
//			npc.anInt1504 = npc.desc.anInt79;
//			npc.anInt1554 = npc.desc.walkAnim;
//			npc.anInt1555 = npc.desc.anInt58;
//			npc.anInt1556 = npc.desc.anInt83;
//			npc.anInt1557 = npc.desc.anInt55;
//			npc.anInt1511 = npc.desc.standAnim;
//			npc.setPos(myPlayer.smallX[0] + i1, myPlayer.smallY[0] + l, j1 == 1);
//		}
//		stream.finishBitAccess();
		while(stream.bitPosition + 21 < i * 8) {
			int k = stream.readBits(14);
			if(k == 16383)
				break;
			if(npcArray[k] == null)
				npcArray[k] = new NPC();
			NPC npc = npcArray[k];
			npcIndices[npcCount++] = k;
			npc.anInt1537 = loopCycle;
			int l = stream.readBits(5);
			if(l > 15)
				l -= 32;
			int i1 = stream.readBits(5);
			if(i1 > 15)
				i1 -= 32;
			int j1 = stream.readBits(1);
			npc.desc = EntityDef.forID(stream.readBits(14));
			int k1 = stream.readBits(1);
			if(k1 == 1)
				anIntArray894[anInt893++] = k;
			npc.anInt1540 = npc.desc.aByte68;
			npc.anInt1504 = npc.desc.anInt79;
			npc.anInt1554 = npc.desc.walkAnim;
			npc.anInt1555 = npc.desc.anInt58;
			npc.anInt1556 = npc.desc.anInt83;
			npc.anInt1557 = npc.desc.anInt55;
			npc.anInt1511 = npc.desc.standAnim;
			npc.setPos(myPlayer.smallX[0] + i1, myPlayer.smallY[0] + l, j1 == 1);
		}
		stream.finishBitAccess();
	}

	public override void processGameLoop() {
		if (rsAlreadyLoaded || loadingError || genericLoadingError)
			return;
		loopCycle++;
		if (!loggedIn)
			processLoginScreenInput();
		else
			mainGameProcessor();
		processOnDemandQueue();
		currentInput = "";
	}
	
	private void method47(bool flag) {
		if (myPlayer.x >> 7 == destX && myPlayer.y >> 7 == destY)
			destX = 0;
		int j = playerCount;
		if (flag)
			j = 1;
		for (int l = 0; l < j; l++) {
			Player player;
			int i1;
			if (flag) {
				player = myPlayer;
				i1 = myPlayerIndex << 14;
			} else {
				player = playerArray[playerIndices[l]];
				i1 = playerIndices[l] << 14;
			}
			if (player == null || !player.isVisible())
				continue;
			player.aBoolean1699 = (lowMem && playerCount > 50 || playerCount > 200)
				&& !flag && player.anInt1517 == player.anInt1511;
			int j1 = player.x >> 7;
			int k1 = player.y >> 7;
			if (j1 < 0 || j1 >= 104 || k1 < 0 || k1 >= 104)
				continue;
			if (player.aModel_1714 != null && loopCycle >= player.anInt1707
			    && loopCycle < player.anInt1708) {
				player.aBoolean1699 = false;
				player.anInt1709 = method42(plane, player.y, player.x);
				//worldController.method286(plane, player.y, player,
//				                          player.anInt1552, player.anInt1722, player.x,
//				                          player.anInt1709, player.anInt1719, player.anInt1721,
//				                          i1, player.anInt1720);
				continue;
			}
			if ((player.x & 0x7f) == 64 && (player.y & 0x7f) == 64) {
				if (anIntArrayArray929[j1][k1] == anInt1265)
					continue;
				anIntArrayArray929[j1][k1] = anInt1265;
			}
			player.anInt1709 = method42(plane, player.y, player.x);
			//worldController.addEntity(plane, player.anInt1552,
//			                          player.anInt1709, i1, player.y, 60, player.x, player,
//			                          player.aBoolean1541);
		}
	}
	
	private bool promptUserForInput(RSInterface class9) {
		int j = class9.anInt214;
		if (anInt900 == 2) {
			if (j == 201) {
				inputTaken = true;
				inputDialogState = 0;
				messagePromptRaised = true;
				promptInput = "";
				friendsListAction = 1;
				aString1121 = "Enter name of friend to add to list";
			}
			if (j == 202) {
				inputTaken = true;
				inputDialogState = 0;
				messagePromptRaised = true;
				promptInput = "";
				friendsListAction = 2;
				aString1121 = "Enter name of friend to delete from list";
			}
		}
		if (j == 205) {
			anInt1011 = 250;
			return true;
		}
		if (j == 501) {
			inputTaken = true;
			inputDialogState = 0;
			messagePromptRaised = true;
			promptInput = "";
			friendsListAction = 4;
			aString1121 = "Enter name of player to add to list";
		}
		if (j == 502) {
			inputTaken = true;
			inputDialogState = 0;
			messagePromptRaised = true;
			promptInput = "";
			friendsListAction = 5;
			aString1121 = "Enter name of player to delete from list";
		}
		if (j == 550) {
			inputTaken = true;
			inputDialogState = 0;
			messagePromptRaised = true;
			promptInput = "";
			aString1121 = "Enter the name of the chat you wish to join";
		}
		if (j >= 300 && j <= 313) {
			int k = (j - 300) / 2;
			int j1 = j & 1;
			int i2 = anIntArray1065[k];
			if (i2 != -1) {
				do {
					if (j1 == 0 && --i2 < 0)
						i2 = IDK.length - 1;
					if (j1 == 1 && ++i2 >= IDK.length)
						i2 = 0;
				} while (IDK.cache[i2].aBoolean662
				         || IDK.cache[i2].anInt657 != k + (aBoolean1047 ? 0 : 7));
				anIntArray1065[k] = i2;
				aBoolean1031 = true;
			}
		}
		if (j >= 314 && j <= 323) {
			int l = (j - 314) / 2;
			int k1 = j & 1;
			int j2 = anIntArray990[l];
			if (k1 == 0 && --j2 < 0)
				j2 = anIntArrayArray1003[l].Length - 1;
			if (k1 == 1 && ++j2 >= anIntArrayArray1003[l].Length)
				j2 = 0;
			anIntArray990[l] = j2;
			aBoolean1031 = true;
		}
		if (j == 324 && !aBoolean1047) {
			aBoolean1047 = true;
			method45();
		}
		if (j == 325 && aBoolean1047) {
			aBoolean1047 = false;
			method45();
		}
		if (j == 326) {
			stream.createFrame(101);
			stream.writeWordBigEndian(aBoolean1047 ? 0 : 1);
			for (int i1 = 0; i1 < 7; i1++)
				stream.writeWordBigEndian(anIntArray1065[i1]);
			
			for (int l1 = 0; l1 < 5; l1++)
				stream.writeWordBigEndian(anIntArray990[l1]);
			
			return true;
		}
		if (j == 613)
			canMute = !canMute;
		if (j >= 601 && j <= 612) {
			clearTopInterfaces();
			if (reportAbuseInput.Length > 0) {
				stream.createFrame(218);
				stream.writeQWord(TextClass.longForName(reportAbuseInput));
				stream.writeWordBigEndian(j - 601);
				stream.writeWordBigEndian(canMute ? 1 : 0);
			}
		}
		return false;
	}
	
	private void method49(RS2Sharp.Stream stream) {
		for (int j = 0; j < anInt893; j++) {
			int k = anIntArray894[j];
			Player player = playerArray[k];
			int l = stream.readUnsignedByte();
			if ((l & 0x40) != 0)
				l += stream.readUnsignedByte() << 8;
			method107(l, k, stream, player);
		}
	}
	
	public void method50(int i, int k, int l, int i1, int j1) {
		int k1 = 0;//worldController.method300(j1, l, i);
		if (k1 != 0) {//(k1 ^ 0xffffffffffffffff) != -1L) {
			int l1 = 0;//worldController.method304(j1, l, i, k1);
			int k2 = l1 >> 6 & 3;
			int i3 = l1 & 0x1f;
			int k3 = k;
			if(k1 > 0)
				k3 = i1;
			int[] ai = aClass30_Sub2_Sub1_Sub1_1263.myPixels;
			int k4 = 24624 + l * 4 + (103 - i) * 512 * 4;
			int i5 = k1 >> 14 & 0x7fff;
			ObjectDef class46_2 = ObjectDef.forID(i5);
			if ((class46_2.anInt758 ^ 0xffffffff) == 0) {
				if (i3 == 0 || i3 == 2) {
					if (k2 == 0) {
						ai[k4] = k3;
						ai[k4 + 512] = k3;
						ai[1024 + k4] = k3;
						ai[1536 + k4] = k3;
					} else if ((k2 ^ 0xffffffff) == -2) {
						ai[k4] = k3;
						ai[k4 + 1] = k3;
						ai[k4 + 2] = k3;
						ai[3 + k4] = k3;
					} else if (k2 == 2) {
						ai[k4 - -3] = k3;
						ai[3 + (k4 + 512)] = k3;
						ai[3 + (k4 + 1024)] = k3;
						ai[1536 + (k4 - -3)] = k3;
					} else if (k2 == 3) {
						ai[k4 + 1536] = k3;
						ai[k4 + 1536 + 1] = k3;
						ai[2 + k4 + 1536] = k3;
						ai[k4 + 1539] = k3;
					}
				}
				if(i3 == 3)
					if(k2 == 0)
						ai[k4] = k3;
				else if(k2 == 1)
					ai[k4 + 3] = k3;
				else if(k2 == 2)
					ai[k4 + 3 + 1536] = k3;
				else if(k2 == 3)
					ai[k4 + 1536] = k3;
				if(i3 == 2)
				if(k2 == 3) {
					ai[k4] = k3;
					ai[k4 + 512] = k3;
					ai[k4 + 1024] = k3;
					ai[k4 + 1536] = k3;
				} else if(k2 == 0) {
					ai[k4] = k3;
					ai[k4 + 1] = k3;
					ai[k4 + 2] = k3;
					ai[k4 + 3] = k3;
				} else if(k2 == 1) {
					ai[k4 + 3] = k3;
					ai[k4 + 3 + 512] = k3;
					ai[k4 + 3 + 1024] = k3;
					ai[k4 + 3 + 1536] = k3;
				} else if(k2 == 2) {
					ai[k4 + 1536] = k3;
					ai[k4 + 1536 + 1] = k3;
					ai[k4 + 1536 + 2] = k3;
					ai[k4 + 1536 + 3] = k3;
				}
			}
		}
		k1 = 0;//worldController.method302(j1, l, i);
		if(k1 != 0) {
			int i2 = 0;//worldController.method304(j1, l, i, k1);
			int l2 = i2 >> 6 & 3;
			int j3 = i2 & 0x1f;
			int l3 = k1 >> 14 & 0x7fff;
			ObjectDef class46_1 = ObjectDef.forID(l3);
			if(class46_1.anInt758 != -1) {
				try{
				Background background_1 = mapScenes[class46_1.anInt758];
				if(background_1 != null) {
					int j5 = (class46_1.anInt744 * 4 - background_1.anInt1452) / 2;
					int k5 = (class46_1.anInt761 * 4 - background_1.anInt1453) / 2;
					background_1.drawSprite(48 + l * 4 + j5, 48 + (104 - i - class46_1.anInt761) * 4 + k5);
				}
				}catch(Exception ex)
				{
					Debug.Log("Idk background error?");
				}
			} else if(j3 == 9) {
				int l4 = 0xeeeeee;
				if(k1 > 0)
					l4 = 0xee0000;
				int[] ai1 = aClass30_Sub2_Sub1_Sub1_1263.myPixels;
				int l5 = 24624 + l * 4 + (103 - i) * 512 * 4;
				if(l2 == 0 || l2 == 2) {
					ai1[l5 + 1536] = l4;
					ai1[l5 + 1024 + 1] = l4;
					ai1[l5 + 512 + 2] = l4;
					ai1[l5 + 3] = l4;
				} else {
					ai1[l5] = l4;
					ai1[l5 + 512 + 1] = l4;
					ai1[l5 + 1024 + 2] = l4;
					ai1[l5 + 1536 + 3] = l4;
				}
			}
		}
		k1 = 0;//worldController.method303(j1, l, i);
		if(k1 != 0) {
			int j2 = k1 >> 14 & 0x7fff;
			ObjectDef class46 = ObjectDef.forID(j2);
			if(class46.anInt758 != -1) {
				try{
				Background background = mapScenes[class46.anInt758];
				if(background != null) {
					int i4 = (class46.anInt744 * 4 - background.anInt1452) / 2;
					int j4 = (class46.anInt761 * 4 - background.anInt1453) / 2;
					background.drawSprite(48 + l * 4 + i4, 48 + (104 - i - class46.anInt761) * 4 + j4);
				}
				}catch(Exception ex)
				{
					Debug.Log("Idk background error");
				}
			}
		}
	}
	
	private void loadTitleScreen() {
		aBackground_966 = new Background(titleStreamLoader, "titlebox", 0);
		aBackground_967 = new Background(titleStreamLoader, "titlebutton", 0);
		aBackgroundArray1152s = new Background[12];
		int j = 0;
		try {
			j = 0;//Int32.Parse(getParameter("fl_icon"));
		} catch (Exception _ex) {
		}
		if (j == 0) {
			for (int k = 0; k < 12; k++)
				aBackgroundArray1152s[k] = new Background(titleStreamLoader,
				                                          "runes", k);
			
		} else {
			for (int l = 0; l < 12; l++)
				aBackgroundArray1152s[l] = new Background(titleStreamLoader,
				                                          "runes", 12 + (l & 3));
			
		}
		aClass30_Sub2_Sub1_Sub1_1201 = new RS2Sharp.Sprite(128, 265);
		aClass30_Sub2_Sub1_Sub1_1202 = new RS2Sharp.Sprite(128, 265);
		System.Array.Copy(aRSImageProducer_1110.anIntArray315, 0,
		                 aClass30_Sub2_Sub1_Sub1_1201.myPixels, 0, 33920);
		System.Array.Copy(aRSImageProducer_1111.anIntArray315, 0,
		                 aClass30_Sub2_Sub1_Sub1_1202.myPixels, 0, 33920);
		
		anIntArray851 = new int[256];
		for (int k1 = 0; k1 < 64; k1++)
			anIntArray851[k1] = 0xFFC01C;
		
		for (int l1 = 0; l1 < 64; l1++)
			anIntArray851[l1 + 64] = 0xFFC01C;
		
		for (int i2 = 0; i2 < 64; i2++)
			anIntArray851[i2 + 128] = 0xFFC01C;
		
		for (int j2 = 0; j2 < 64; j2++)
			anIntArray851[j2 + 192] = 0xFFC01C;
		
		anIntArray852 = new int[256];
		for (int k2 = 0; k2 < 64; k2++)
			anIntArray852[k2] = 0xFFC01C;
		
		for (int l2 = 0; l2 < 64; l2++)
			anIntArray852[l2 + 64] = 0xFFC01C;
		
		for (int i3 = 0; i3 < 64; i3++)
			anIntArray852[i3 + 128] = 0xFFC01C;
		
		for (int j3 = 0; j3 < 64; j3++)
			anIntArray852[j3 + 192] = 0xFFC01C;
		
		anIntArray853 = new int[256];
		for (int k3 = 0; k3 < 64; k3++)
			anIntArray853[k3] = k3 * 4;
		
		for (int l3 = 0; l3 < 64; l3++)
			anIntArray853[l3 + 64] = 255 + 0x40000 * l3;
		
		for (int i4 = 0; i4 < 64; i4++)
			anIntArray853[i4 + 128] = 0xff00ff + 1024 * i4;
		
		for (int j4 = 0; j4 < 64; j4++)
			anIntArray853[j4 + 192] = 0xffffff;
		
		anIntArray850 = new int[256];
		anIntArray1190 = new int[32768];
		anIntArray1191 = new int[32768];
		randomizeBackground(null);
		anIntArray828 = new int[32768];
		anIntArray829 = new int[32768];
		drawLoadingText(10, "Connecting to fileserver");
		if (!aBoolean831) {
			//drawFlames = true;
			aBoolean831 = true;
			//startRunnable(this, 2);
		}
	}
	
	private static void setHighMem() {
		//WorldController.lowMem = false;
		RS2Sharp.Texture.lowMem = false;
		lowMem = false;
		//ObjectManager.lowMem = false;
		ObjectDef.lowMem = false;
	}
	
// 	public static void main() {
// 		try {
// 			nodeID = 10;
// 			portOff = 0;
// 			isMembers = true;
// 			signlink.storeid = 32;
// 			signlink.startpriv(InetAddress.getLocalHost());
// 			new Jframe(args);
// 		} catch (Exception exception) {
// 			Debug.Log(exception.StackTrace);
// 		}
// 	}
	
	private void loadingStages() {
		if (lowMem && loadingStage == 2){// && ObjectManager.anInt131 != plane) {
			aRSImageProducer_1165.initDrawingArea();
			aTextDrawingArea_1271.drawText(0xffffff, "Loading - please wait.", 151,
			                               257);
			aTextDrawingArea_1271.drawText(0xffffff, "Loading - please wait.",
			                               150, 256);
			aRSImageProducer_1165.drawGraphics(0, base.graphics, 0);
			loadingStage = 1;
			aLong824 = NetDrawingTools.CurrentTimeMillis();
		}
		if (loadingStage == 1) {
			int j = method54();
			if (j != 0 && NetDrawingTools.CurrentTimeMillis() - aLong824 > 0x57e40L) {
				signlink.reporterror(myUsername + " glcfb " + aLong1215 + ","
				                     + j + "," + lowMem + "," + decompressors[0] + ","
				                     + onDemandFetcher.getNodeCount() + "," + plane + ","
				                     + anInt1069 + "," + anInt1070);
				aLong824 = NetDrawingTools.CurrentTimeMillis();
			}
		}
		if (loadingStage == 2 && plane != anInt985) {
			anInt985 = plane;
			method24(plane);
		}
	}
	
	private int method54() {
		for (int i = 0; i < localRegionMapData.Length; i++) {
			if (localRegionMapData[i] == null && anIntArray1235[i] != -1)
				return -1;
			if (localRegionLandscapeData[i] == null && anIntArray1236[i] != -1)
				return -2;
		}
		bool flag = true;
		for (int j = 0; j < localRegionMapData.Length; j++) {
			byte[] abyte0 = localRegionLandscapeData[j];
			if (abyte0 != null) {
				int k = (localRegionIds[j] >> 8) * 64 - baseX;
				int l = (localRegionIds[j] & 0xff) * 64 - baseY;
				if (aBoolean1159) {
					k = 10;
					l = 10;
				}
//				flag &= ObjectManager.method189(k, abyte0, l);
			}
		}
		if (!flag)
			return -3;
		if (aBoolean1080) {
			return -4;
		} else {
			loadingStage = 2;
			//ObjectManager.anInt131 = plane;
			method22();
			stream.createFrame(121);
			return 0;
		}
	}
	
	private void processProjectiles() {
		for (Projectile projectile = (Projectile) projectiles
		     .reverseGetFirst(); projectile != null; projectile = (Projectile) projectiles
		     .reverseGetNext())
			if (projectile.plane != plane
			    || loopCycle > projectile.startTick)
				projectile.unlink();
		else if (loopCycle >= projectile.endTick) {
			if (projectile.target2 > 0) {
				NPC npc = npcArray[projectile.target2 - 1];
				if (npc != null && npc.x >= 0 && npc.x < 13312
				    && npc.y >= 0 && npc.y < 13312)
					projectile.target(npc.x, npc.y, method42(projectile.plane, npc.y, npc.x)
					                  - projectile.anInt1583, loopCycle);
//						loopCycle,
//						npc.y,
//						method42(projectile.plane,
//					         npc.y, npc.x)
//						- projectile.anInt1583,
//						npc.x);
			}
			if (projectile.target2 < 0) {
				int j = -projectile.target2 - 1;
				Player player;
				if (j == localPlayerIndex)
					player = myPlayer;
				else
					player = playerArray[j];
				if (player != null && player.x >= 0 && player.x < 13312
				    && player.y >= 0 && player.y < 13312)
							projectile.target(player.x, player.y, method42(projectile.plane, player.y, player.x)
							                  - projectile.anInt1583, loopCycle);
//						loopCycle,
//						player.y,
//						method42(projectile.plane,
//					         player.y, player.x)
//						- projectile.anInt1583,
//						player.x);
			}
			projectile.update(anInt945);
			//worldController.addEntity(plane,
//			                          projectile.yaw,
//			                          (int) projectile.z, -1,
//			                          (int) projectile.y, 60,
//			                          (int) projectile.x,
//			                          projectile, false);
		}
		
	}

//	public AppletContext getAppletContext() {
//		if (signlink.mainapp != null)
//			return signlink.mainapp.getAppletContext();
//		else
//			return base.getAppletContext();
//	}
	
	private void drawLogo() {
		byte[] abyte0 = titleStreamLoader.getDataForName("title.dat");
		RS2Sharp.Sprite sprite = new RS2Sharp.Sprite(abyte0, this);
		aRSImageProducer_1110.initDrawingArea();
		sprite.method346(0, 0);
		aRSImageProducer_1111.initDrawingArea();
		sprite.method346(-637, 0);
		aRSImageProducer_1107.initDrawingArea();
		sprite.method346(-128, 0);
		aRSImageProducer_1108.initDrawingArea();
		sprite.method346(-202, -371);
		aRSImageProducer_1109.initDrawingArea();
		sprite.method346(-202, -171);
		aRSImageProducer_1112.initDrawingArea();
		sprite.method346(0, -265);
		aRSImageProducer_1113.initDrawingArea();
		sprite.method346(-562, -265);
		aRSImageProducer_1114.initDrawingArea();
		sprite.method346(-128, -171);
		aRSImageProducer_1115.initDrawingArea();
		sprite.method346(-562, -171);
		int[] ai = new int[sprite.myWidth];
		for (int j = 0; j < sprite.myHeight; j++) {
			for (int k = 0; k < sprite.myWidth; k++)
				ai[k] = sprite.myPixels[(sprite.myWidth - k - 1)
				                        + sprite.myWidth * j];
			
			System.Array.Copy(ai, 0, sprite.myPixels, sprite.myWidth * j,
			                 sprite.myWidth);
		}
		aRSImageProducer_1110.initDrawingArea();
		sprite.method346(382, 0);
		aRSImageProducer_1111.initDrawingArea();
		sprite.method346(-255, 0);
		aRSImageProducer_1107.initDrawingArea();
		sprite.method346(254, 0);
		aRSImageProducer_1108.initDrawingArea();
		sprite.method346(180, -371);
		aRSImageProducer_1109.initDrawingArea();
		sprite.method346(180, -171);
		aRSImageProducer_1112.initDrawingArea();
		sprite.method346(382, -265);
		aRSImageProducer_1113.initDrawingArea();
		sprite.method346(-180, -265);
		aRSImageProducer_1114.initDrawingArea();
		sprite.method346(254, -171);
		aRSImageProducer_1115.initDrawingArea();
		sprite.method346(-180, -171);
		sprite = new RS2Sharp.Sprite(titleStreamLoader, "logo", 0);
		aRSImageProducer_1107.initDrawingArea();
		sprite.drawSprite(77, 3);
		sprite = null;
		System.Object obj = null;
		System.Object obj1 = null;
		System.GC.Collect();
	}
	
	private void processOnDemandQueue() {
		do {
			OnDemandData onDemandData;
			do {
				onDemandData = onDemandFetcher.getNextNode();
				if (onDemandData == null)
					return;
				if (onDemandData.dataType == 0) {
					Model.method460(onDemandData.buffer, onDemandData.ID);
					if ((onDemandFetcher.getModelIndex(onDemandData.ID) & 0x62) != 0) {
						needDrawTabArea = true;
						if (backDialogID != -1)
							inputTaken = true;
					}
				}
				if (onDemandData.dataType == 1 && onDemandData.buffer != null)
					Class36.load(onDemandData.ID, onDemandData.buffer);
				if (onDemandData.dataType == 2 && onDemandData.ID == nextSong
				    && onDemandData.buffer != null)
					saveMidi(songChanging, onDemandData.buffer);
				if (onDemandData.dataType == 3 && loadingStage == 1) {
					for (int i = 0; i < localRegionMapData.Length; i++) {
						if (anIntArray1235[i] == onDemandData.ID) {
							localRegionMapData[i] = onDemandData.buffer;
							if (onDemandData.buffer == null)
								anIntArray1235[i] = -1;
							break;
						}
						if (anIntArray1236[i] != onDemandData.ID)
							continue;
						localRegionLandscapeData[i] = onDemandData.buffer;
						if (onDemandData.buffer == null)
							anIntArray1236[i] = -1;
						break;
					}
					
				}
			} while (onDemandData.dataType != 93
			         || !onDemandFetcher.method564(onDemandData.ID));
		//	ObjectManager.method173(new RS2Sharp.Stream(onDemandData.buffer),
			//                        onDemandFetcher);
		} while (true);
	}
	
	private void calcFlamesPosition() {
		char c = '\u0100';
		for (int j = 10; j < 117; j++) { // lightness
			int k = (int) (RS2Sharp.Random.Next(100));
			if (k < 50)
				anIntArray828[j + (c - 2 << 7)] = 255;
		}
		// add back for longer flames
		/*
		 * for (int l = 0; l < 100; l++) { int i1 = (int) (RS2Sharp.Random.Next() * 124D)
		 * + 2; int k1 = (int) (RS2Sharp.Random.Next() * 128D) + 128; int k2 = i1 + (k1
		 * << 7); anIntArray828[k2] = 192; }
		 */
		
		for (int j1 = 1; j1 < c - 1; j1++) {
			for (int l1 = 1; l1 < 127; l1++) {
				int l2 = l1 + (j1 << 7);
				anIntArray829[l2] = (anIntArray828[l2 - 1]
				                     + anIntArray828[l2 + 1] + anIntArray828[l2 - 128] + anIntArray828[l2 + 128]) / 4;
			}
			
		}
		
		anInt1275 += 128;
		if (anInt1275 > anIntArray1190.Length) {
			anInt1275 -= anIntArray1190.Length;
			int i2 = (int) (RS2Sharp.Random.Next(12));
			randomizeBackground(aBackgroundArray1152s[i2]);
		}
		for (int j2 = 1; j2 < c - 1; j2++) {
			for (int i3 = 1; i3 < 127; i3++) {
				int k3 = i3 + (j2 << 7);
				int i4 = anIntArray829[k3 + 128]
				- anIntArray1190[k3 + anInt1275 & anIntArray1190.Length
				                 - 1] / 5;
				if (i4 < 0)
					i4 = 0;
				anIntArray828[k3] = i4;
			}
			
		}
		
		System.Array.Copy(anIntArray969, 1, anIntArray969, 0, c - 1);
		
		anIntArray969[c - 1] = (int) (Mathf.Sin(loopCycle / 14f) * 16f
		                              + Mathf.Sin(loopCycle / 15f) * 14f + Mathf.Sin(loopCycle / 16f) * 12f);
		if (anInt1040 > 0)
			anInt1040 -= 4;
		if (anInt1041 > 0)
			anInt1041 -= 4;
		if (anInt1040 == 0 && anInt1041 == 0) {
			int l3 = (int) (RS2Sharp.Random.Next(2000));
			if (l3 == 0)
				anInt1040 = 1024;
			if (l3 == 1)
				anInt1041 = 1024;
		}
	}
	
	private bool saveWave(byte[] abyte0, int i) {
		return abyte0 == null || signlink.wavesave(abyte0, i);
	}
	
	private void method60(int i) {
		RSInterface class9 = RSInterface.interfaceCache[i];
		if(class9 == null) return;
		for (int j = 0; j < class9.children.Length; j++) {
			if (class9.children[j] == -1)
				break;
			RSInterface class9_1 = RSInterface.interfaceCache[class9.children[j]];
			if (class9_1.type == 1)
				method60(class9_1.id);
			class9_1.anInt246 = 0;
			class9_1.anInt208 = 0;
		}
	}
	
	private void drawHeadIcon() {
		if (anInt855 != 2)
			return;
		calcEntityScreenPos((anInt934 - baseX << 7) + anInt937, anInt936 * 2,
		                    (anInt935 - baseY << 7) + anInt938);
		if (spriteDrawX > -1 && loopCycle % 20 < 10)
			headIconsHint[0].drawSprite(spriteDrawX - 12, spriteDrawY - 28);
	}
	
	private void mainGameProcessor() {

		if (anInt1104 > 1)
			anInt1104--;
		if (anInt1011 > 0)
			anInt1011--;
		for (int j = 0; j < 5; j++)
			if (!parsePacket())
				break;
		
		if (!loggedIn)
			return;
		lock (mouseDetection.syncObject) {
			if (flagged) {
				if (base.clickMode3 != 0 || mouseDetection.coordsIndex >= 40) {
					stream.createFrame(45);
					stream.writeWordBigEndian(0);
					int j2 = stream.currentOffset;
					int j3 = 0;
					for (int j4 = 0; j4 < mouseDetection.coordsIndex; j4++) {
						if (j2 - stream.currentOffset >= 240)
							break;
						j3++;
						int l4 = mouseDetection.coordsY[j4];
						if (l4 < 0)
							l4 = 0;
						else if (l4 > 502)
							l4 = 502;
						int k5 = mouseDetection.coordsX[j4];
						if (k5 < 0)
							k5 = 0;
						else if (k5 > 764)
							k5 = 764;
						int i6 = l4 * 765 + k5;
						if (mouseDetection.coordsY[j4] == -1
						    && mouseDetection.coordsX[j4] == -1) {
							k5 = -1;
							l4 = -1;
							i6 = 0x7ffff;
						}
						if (k5 == anInt1237 && l4 == anInt1238) {
							if (anInt1022 < 2047)
								anInt1022++;
						} else {
							int j6 = k5 - anInt1237;
							anInt1237 = k5;
							int k6 = l4 - anInt1238;
							anInt1238 = l4;
							if (anInt1022 < 8 && j6 >= -32 && j6 <= 31
							    && k6 >= -32 && k6 <= 31) {
								j6 += 32;
								k6 += 32;
								stream.writeWord((anInt1022 << 12) + (j6 << 6)
								                 + k6);
								anInt1022 = 0;
							} else if (anInt1022 < 8) {
								stream.writeDWordBigEndian(0x800000
								                           + (anInt1022 << 19) + i6);
								anInt1022 = 0;
							} else {
								stream.writeDWord(unchecked((int)0xc0000000 + (anInt1022 << 19) + i6));
								anInt1022 = 0;
							}
						}
					}
					
					stream.writeBytes(stream.currentOffset - j2);
					if (j3 >= mouseDetection.coordsIndex) {
						mouseDetection.coordsIndex = 0;
					} else {
						mouseDetection.coordsIndex -= j3;
						for (int i5 = 0; i5 < mouseDetection.coordsIndex; i5++) {
							mouseDetection.coordsX[i5] = mouseDetection.coordsX[i5
							                                                    + j3];
							mouseDetection.coordsY[i5] = mouseDetection.coordsY[i5
							                                                    + j3];
						}
						
					}
				}
			} else {
				mouseDetection.coordsIndex = 0;
			}
		}
		if (base.clickMode3 != 0) {
			long l = (base.aLong29 - aLong1220) / 50L;
			if (l > 4095L)
				l = 4095L;
			aLong1220 = base.aLong29;
			int k2 = base.saveClickY;
			if (k2 < 0)
				k2 = 0;
			else if (k2 > 502)
				k2 = 502;
			int k3 = base.saveClickX;
			if (k3 < 0)
				k3 = 0;
			else if (k3 > 764)
				k3 = 764;
			int k4 = k2 * 765 + k3;
			int j5 = 0;
			if (base.clickMode3 == 2)
				j5 = 1;
			int l5 = (int) l;
			stream.createFrame(241);
			stream.writeDWord((l5 << 20) + (j5 << 19) + k4);
		}
		if (anInt1016 > 0)
			anInt1016--;
		if (base.keyArray[1] == 1 || base.keyArray[2] == 1
		    || base.keyArray[3] == 1 || base.keyArray[4] == 1)
			aBoolean1017 = true;
		if (aBoolean1017 && anInt1016 <= 0) {
			anInt1016 = 20;
			aBoolean1017 = false;
			stream.createFrame(86);
			stream.writeWord(anInt1184);
			stream.method432(minimapInt1);
		}
		if (base.awtFocus && !aBoolean954) {
			aBoolean954 = true;
			stream.createFrame(3);
			stream.writeWordBigEndian(1);
		}
		if (!base.awtFocus && aBoolean954) {
			aBoolean954 = false;
			stream.createFrame(3);
			stream.writeWordBigEndian(0);
		}
		loadingStages();
		method115();
		method90();
		anInt1009++;
		if (anInt1009 > 750)
			dropClient();
		method114();
		method95();
		method38();
		anInt945++;
		if (crossType != 0) {
			crossIndex += 20;
			if (crossIndex >= 400)
				crossType = 0;
		}
		if (atInventoryInterfaceType != 0) {
			atInventoryLoopCycle++;
			if (atInventoryLoopCycle >= 15) {
				if (atInventoryInterfaceType == 2)
					needDrawTabArea = true;
				if (atInventoryInterfaceType == 3)
					inputTaken = true;
				atInventoryInterfaceType = 0;
			}
		}
		if (activeInterfaceType != 0) {
			anInt989++;
			if (base.mouseX > anInt1087 + 5 || base.mouseX < anInt1087 - 5
			    || base.mouseY > anInt1088 + 5
			    || base.mouseY < anInt1088 - 5)
				aBoolean1242 = true;
			if (base.clickMode2 == 0) {
				if (activeInterfaceType == 2)
					needDrawTabArea = true;
				if (activeInterfaceType == 3)
					inputTaken = true;
				activeInterfaceType = 0;
				if (aBoolean1242 && anInt989 >= 5) {
					lastActiveInvInterface = -1;
					processRightClick();
					if (lastActiveInvInterface == anInt1084
					    && mouseInvInterfaceIndex != anInt1085) {
						RSInterface class9 = RSInterface.interfaceCache[anInt1084];
						int j1 = 0;
						if (anInt913 == 1 && class9.anInt214 == 206)
							j1 = 1;
						if (class9.inv[mouseInvInterfaceIndex] <= 0)
							j1 = 0;
						if (class9.aBoolean235) {
							int l2 = anInt1085;
							int l3 = mouseInvInterfaceIndex;
							class9.inv[l3] = class9.inv[l2];
							class9.invStackSizes[l3] = class9.invStackSizes[l2];
							class9.inv[l2] = -1;
							class9.invStackSizes[l2] = 0;
						} else if (j1 == 1) {
							int i3 = anInt1085;
							for (int i4 = mouseInvInterfaceIndex; i3 != i4;)
							if (i3 > i4) {
								class9.swapInventoryItems(i3, i3 - 1);
								i3--;
							} else if (i3 < i4) {
								class9.swapInventoryItems(i3, i3 + 1);
								i3++;
							}
							
						} else {
							class9.swapInventoryItems(anInt1085,
							                          mouseInvInterfaceIndex);
						}
						stream.createFrame(214);
						stream.method433(anInt1084);
						stream.method424(j1);
						stream.method433(anInt1085);
						stream.method431(mouseInvInterfaceIndex);
					}
				} else if ((anInt1253 == 1 || menuHasAddFriend(menuActionRow - 1))
				           && menuActionRow > 2)
					determineMenuSize();
				else if (menuActionRow > 0)
					doAction(menuActionRow - 1);
				atInventoryLoopCycle = 10;
				base.clickMode3 = 0;
			}
		}
//		if (WorldController.anInt470 != -1) {
//			int k = WorldController.anInt470;
//			int k1 = WorldController.anInt471;
//			bool flag = doWalkTo(0, 0, 0, 0, myPlayer.smallY[0], 0, 0, k1,
//			                        myPlayer.smallX[0], true, k);
//			WorldController.anInt470 = -1;
//			if (flag) {
//				crossX = base.saveClickX;
//				crossY = base.saveClickY;
//				crossType = 1;
//				crossIndex = 0;
//			}
//		}
		if (base.clickMode3 == 1 && aString844 != null) {
			aString844 = null;
			inputTaken = true;
			base.clickMode3 = 0;
		}
		processMenuClick();
		processMainScreenClick();
		processTabClick();
		processChatModeClick();
		if (base.clickMode2 == 1 || base.clickMode3 == 1)
			anInt1213++;
		if (anInt1500 != 0 || anInt1044 != 0 || anInt1129 != 0) {
			if (anInt1501 < 50 && !menuOpen) {
				anInt1501++;
				if (anInt1501 == 50) {
					if (anInt1500 != 0) {
						inputTaken = true;
					}
					if (anInt1044 != 0) {
						needDrawTabArea = true;
					}
				}
			}
		} else if (anInt1501 > 0) {
			anInt1501--;
		}
		
		if (loadingStage == 2)
			method108();
		if (loadingStage == 2 && aBoolean1160)
			calcCameraPos();
		for (int i1 = 0; i1 < 5; i1++)
			anIntArray1030[i1]++;
		
		method73();
		base.idleTime++;
		if (base.idleTime > 4500) {
			anInt1011 = 250;
			base.idleTime -= 500;
			stream.createFrame(202);
		}
		anInt1010++;
		if (anInt1010 > 50)
			stream.createFrame(0);
		try {
			if (socketStream != null && stream.currentOffset > 0) {
				socketStream.queueBytes(stream.currentOffset, stream.buffer);
				stream.currentOffset = 0;
				anInt1010 = 0;
			}
		} catch (IOException _ex) {
			dropClient();
		} catch (Exception exception) {
			resetLogout();
		}
	}
	
	private void method63() {
		Class30_Sub1 class30_sub1 = (Class30_Sub1) aClass19_1179
			.reverseGetFirst();
		for (; class30_sub1 != null; class30_sub1 = (Class30_Sub1) aClass19_1179
		     .reverseGetNext())
		if (class30_sub1.anInt1294 == -1) {
			class30_sub1.anInt1302 = 0;
			method89(class30_sub1);
		} else {
			class30_sub1.unlink();
		}
		
	}
	
	private void resetImageProducers() {
		if (aRSImageProducer_1107 != null)
			return;
		base.fullGameScreen = null;
		aRSImageProducer_1166 = null;
		aRSImageProducer_1164 = null;
		aRSImageProducer_1163 = null;
		aRSImageProducer_1165 = null;
		aRSImageProducer_1123 = null;
		aRSImageProducer_1124 = null;
		aRSImageProducer_1125 = null;
		aRSImageProducer_1110 = new RSImageProducer(128, 265);
		DrawingArea.setAllPixelsToZero();
		aRSImageProducer_1111 = new RSImageProducer(128, 265);
		DrawingArea.setAllPixelsToZero();
		aRSImageProducer_1107 = new RSImageProducer(509, 171);
		DrawingArea.setAllPixelsToZero();
		aRSImageProducer_1108 = new RSImageProducer(360, 132);
		DrawingArea.setAllPixelsToZero();
		aRSImageProducer_1109 = new RSImageProducer(765, 503);
		DrawingArea.setAllPixelsToZero();
		aRSImageProducer_1112 = new RSImageProducer(202, 238);
		DrawingArea.setAllPixelsToZero();
		aRSImageProducer_1113 = new RSImageProducer(203, 238);
		DrawingArea.setAllPixelsToZero();
		aRSImageProducer_1114 = new RSImageProducer(74, 94);
		DrawingArea.setAllPixelsToZero();
		aRSImageProducer_1115 = new RSImageProducer(75, 94);
		DrawingArea.setAllPixelsToZero();
		if (titleStreamLoader != null) {
			drawLogo();
			loadTitleScreen();
		}
		welcomeScreenRaised = true;
	}

	public override void drawLoadingText(int i, String s) {
		Debug.Log (s + " - " + i);
		anInt1079 = i;
		aString1049 = s;
		resetImageProducers();
		if (titleStreamLoader == null) {
			base.drawLoadingText(i, s);
			return;
		}
		aRSImageProducer_1109.initDrawingArea();
//		char c = '\u0168';
//		char c1 = '\310';
		int c = 360;
		int c1 = 200;
		byte byte1 = 20;
		chatTextDrawingArea.drawText(0xffffff,
		                             "RuneScape is loading - please wait...", c1 / 2 - 26 - byte1,
		                             c / 2);
		int j = c1 / 2 - 18 - byte1;
		DrawingArea.fillPixels(c / 2 - 152, 304, 34, 0x8c1111, j);
		DrawingArea.fillPixels(c / 2 - 151, 302, 32, 0, j + 1);
		DrawingArea.drawPixels(30, j + 2, c / 2 - 150, 0x8c1111, i * 3);
		DrawingArea
			.drawPixels(30, j + 2, (c / 2 - 150) + i * 3, 0, 300 - i * 3);
		chatTextDrawingArea.drawText(0xffffff, s, (c1 / 2 + 5) - byte1, c / 2);
		aRSImageProducer_1109.drawGraphics(171, base.graphics, 202);
		if (welcomeScreenRaised) {
			welcomeScreenRaised = false;
			if (!aBoolean831) {
				aRSImageProducer_1110.drawGraphics(0, base.graphics, 0);
				aRSImageProducer_1111.drawGraphics(0, base.graphics, 637);
			}
			aRSImageProducer_1107.drawGraphics(0, base.graphics, 128);
			aRSImageProducer_1108.drawGraphics(371, base.graphics, 202);
			aRSImageProducer_1112.drawGraphics(265, base.graphics, 0);
			aRSImageProducer_1113.drawGraphics(265, base.graphics, 562);
			aRSImageProducer_1114.drawGraphics(171, base.graphics, 128);
			aRSImageProducer_1115.drawGraphics(171, base.graphics, 562);
		}
	}
	
	private void method65(int i, int j, int k, int l, RSInterface class9,
	                      int i1, bool flag, int j1) {
		int anInt992;
		if (aBoolean972)
			anInt992 = 32;
		else
			anInt992 = 0;
		aBoolean972 = false;
		if (k >= i && k < i + 16 && l >= i1 && l < i1 + 16) {
			class9.scrollPosition -= anInt1213 * 4;
			if (flag) {
				needDrawTabArea = true;
			}
		} else if (k >= i && k < i + 16 && l >= (i1 + j) - 16 && l < i1 + j) {
			class9.scrollPosition += anInt1213 * 4;
			if (flag) {
				needDrawTabArea = true;
			}
		} else if (k >= i - anInt992 && k < i + 16 + anInt992 && l >= i1 + 16
		           && l < (i1 + j) - 16 && anInt1213 > 0) {
			int l1 = ((j - 32) * j) / j1;
			if (l1 < 8)
				l1 = 8;
			int i2 = l - i1 - 16 - l1 / 2;
			int j2 = j - 32 - l1;
			class9.scrollPosition = ((j1 - j) * i2) / j2;
			if (flag)
				needDrawTabArea = true;
			aBoolean972 = true;
		}
	}
	
	private bool method66(int i, int j, int k) {
		int i1 = i >> 14 & 0x7fff;
		int j1 = 0;//worldController.method304(plane, k, j, i);
		if (j1 == -1)
			return false;
		int k1 = j1 & 0x1f;
		int l1 = j1 >> 6 & 3;
		if (k1 == 10 || k1 == 11 || k1 == 22) {
			ObjectDef class46 = ObjectDef.forID(i1);
			int i2;
			int j2;
			if (l1 == 0 || l1 == 2) {
				i2 = class46.anInt744;
				j2 = class46.anInt761;
			} else {
				i2 = class46.anInt761;
				j2 = class46.anInt744;
			}
			int k2 = class46.anInt768;
			if (l1 != 0)
				k2 = (k2 << l1 & 0xf) + (k2 >> 4 - l1);
			doWalkTo(2, 0, j2, 0, myPlayer.smallY[0], i2, k2, j,
			         myPlayer.smallX[0], false, k);
		} else {
			doWalkTo(2, l1, 0, k1 + 1, myPlayer.smallY[0], 0, 0, j,
			         myPlayer.smallX[0], false, k);
		}
		crossX = base.saveClickX;
		crossY = base.saveClickY;
		crossType = 2;
		crossIndex = 0;
		return true;
	}
	
	private StreamLoader streamLoaderForName(int i, String s, String s1, int j,
	                                         int k) {
		byte[] abyte0 = null;
		int l = 5;
		try {
			if (decompressors[0] != null)
				abyte0 = decompressors[0].decompress(i);
		} catch (Exception _ex) {
		}
		//if (abyte0 != null) {
		// aCRC32_930.reset();
		// aCRC32_930.update(abyte0);
		// int i1 = (int)aCRC32_930.getValue();
		// if(i1 != j)
		//	}
		if (abyte0 != null) {
			StreamLoader streamLoader = new StreamLoader(abyte0);
			return streamLoader;
		}
		int j1 = 0;
		while (abyte0 == null) {
		Debug.Log ("here");
			String s2 = "Unknown error";
			drawLoadingText(k, "Requesting " + s);
			System.Object obj = null;
			//try {
				int k1 = 0;
				NetworkStream datainputstream = openJagGrabInputStream(s1 + j);
				byte[] abyte1 = new byte[6];
				datainputstream.Read(abyte1, 0, 6);
				RS2Sharp.Stream stream = new RS2Sharp.Stream(abyte1);
				stream.currentOffset = 3;
				int i2 = stream.read3Bytes() + 6;
				int j2 = 6;
				abyte0 = new byte[i2];
				System.Array.Copy(abyte1, 0, abyte0, 0, 6);
				
				while (j2 < i2) {
					int l2 = i2 - j2;
					if (l2 > 1000)
						l2 = 1000;
					int j3 = datainputstream.Read(abyte0, j2, l2);
					if (j3 < 0) {
						s2 = "Length error: " + j2 + "/" + i2;
						throw new IOException("EOF");
					}
					j2 += j3;
					int k3 = (j2 * 100) / i2;
					if (k3 != k1)
						drawLoadingText(k, "Loading " + s + " - " + k3 + "%");
					k1 = k3;
				}
				datainputstream.Close();
				try {
					if (decompressors[0] != null)
						decompressors[0].method234(abyte0.Length, abyte0, i);
				} catch (Exception _ex) {
					decompressors[0] = null;
				}
				/*
				 * if(abyte0 != null) { aCRC32_930.reset();
				 * aCRC32_930.update(abyte0); int i3 =
				 * (int)aCRC32_930.getValue(); if(i3 != j) { abyte0 = null;
				 * j1++; s2 = "Checksum error: " + i3; } }
				 */
// 			} catch (IOException ioexception) {
// 				if (s2.Equals("Unknown error"))
// 					s2 = "Connection error";
// 				abyte0 = null;
// 			} catch (Exception _ex) {
// 				s2 = "Null error";
// 				abyte0 = null;
// // 				if (!signlink.reporterror)
// // 					return null;
// 			} catch (Exception _ex) {
// 				s2 = "Bounds error";
// 				abyte0 = null;
// // 				if (!signlink.reporterror)
// // 					return null;
// 			} catch (Exception _ex) {
// 				s2 = "Unexpected error";
// 				abyte0 = null;
// // 				if (!signlink.reporterror)
// // 					return null;
// 			}
			if (abyte0 == null) {
				for (int l1 = l; l1 > 0; l1--) {
					if (j1 >= 3) {
						drawLoadingText(k, "Game updated - please reload page");
						l1 = 10;
					} else {
						drawLoadingText(k, s2 + " - Retrying in " + l1);
					}
					try {
						//Thread.Sleep(1000);
					} catch (Exception _ex) {
					}
				}
				
				l *= 2;
				if (l > 60)
					l = 60;
				aBoolean872 = !aBoolean872;
			}
			
		}
		
		StreamLoader streamLoader_1 = new StreamLoader(abyte0);
		return streamLoader_1;
	}
	
	public static StreamLoader streamLoaderForName2(int i, String s, String s1, int j,
	                                         int k) {
		byte[] abyte0 = null;
		int l = 5;
		//try {
			if (decompressors[0] != null)
				abyte0 = decompressors[0].decompress(i);
		//} catch (Exception _ex) {
		//}
		//if (abyte0 != null) {
		// aCRC32_930.reset();
		// aCRC32_930.update(abyte0);
		// int i1 = (int)aCRC32_930.getValue();
		// if(i1 != j)
		//	}
		if (abyte0 != null) {
			StreamLoader streamLoader = new StreamLoader(abyte0);
			return streamLoader;
		}
		int j1 = 0;
		while (abyte0 == null) {
			String s2 = "Unknown error";
			System.Object obj = null;
			//try {
				int k1 = 0;
				NetworkStream datainputstream = openJagGrabInputStream(s1 + j);
				byte[] abyte1 = new byte[6];
				datainputstream.Read(abyte1, 0, 6);
				RS2Sharp.Stream stream = new RS2Sharp.Stream(abyte1);
				stream.currentOffset = 3;
				int i2 = stream.read3Bytes() + 6;
				int j2 = 6;
				abyte0 = new byte[i2];
				System.Array.Copy(abyte1, 0, abyte0, 0, 6);
				
				while (j2 < i2) {
					int l2 = i2 - j2;
					if (l2 > 1000)
						l2 = 1000;
					int j3 = datainputstream.Read(abyte0, j2, l2);
					if (j3 < 0) {
						s2 = "Length error: " + j2 + "/" + i2;
						throw new IOException("EOF");
					}
					j2 += j3;
					int k3 = (j2 * 100) / i2;


					k1 = k3;
				}
				datainputstream.Close();
				try {
					if (decompressors[0] != null)
						decompressors[0].method234(abyte0.Length, abyte0, i);
				} catch (Exception _ex) {
					decompressors[0] = null;
				}
			if (abyte0 == null) {
				for (int l1 = l; l1 > 0; l1--) {
					if (j1 >= 3) {
						l1 = 10;
					} else {
					}
					try {
						//Thread.Sleep(1000);
					} catch (Exception _ex) {
					}
				}
				
				l *= 2;
				if (l > 60)
					l = 60;
				aBoolean872 = !aBoolean872;
			}
			
		}
		
		StreamLoader streamLoader_1 = new StreamLoader(abyte0);
		return streamLoader_1;
	}
	
	private void dropClient() {
		if (anInt1011 > 0) {
			resetLogout();
			return;
		}
		aRSImageProducer_1165.initDrawingArea();
		aTextDrawingArea_1271.drawText(0, "Connection lost", 144, 257);
		aTextDrawingArea_1271.drawText(0xffffff, "Connection lost", 143, 256);
		aTextDrawingArea_1271.drawText(0,
		                               "Please wait - attempting to reestablish", 159, 257);
		aTextDrawingArea_1271.drawText(0xffffff,
		                               "Please wait - attempting to reestablish", 158, 256);
		aRSImageProducer_1165.drawGraphics(0, base.graphics, 0);
		anInt1021 = 0;
		destX = 0;
		RSSocket rsSocket = socketStream;
		loggedIn = false;
		loginFailures = 0;
		login(myUsername, myPassword, true);
		if (!loggedIn)
			resetLogout();
		try {
			rsSocket.close();
		} catch (Exception _ex) {
		}
	}
	
	private void doAction(int i) {
		if (i < 0)
			return;
		if (inputDialogState != 0) {
			inputDialogState = 0;
			inputTaken = true;
		}
		int j = menuActionCmd2[i];
		int k = menuActionCmd3[i];
		int l = menuActionID[i];
		int i1 = menuActionCmd1[i];
		if (l >= 2000)
			l -= 2000;
		
		if (l == 696) {
			setNorth();
		}
		if(l == 1050) {
			runClicked = !runClicked;
			stream.createFrame(185); 
			stream.writeWord(152);
		}
		if(l == 1506) { //Select quick prayers
			stream.createFrame(185); 
			stream.writeWord(5001);
		}
		if(l == 1500) { //Toggle quick prayers
			prayClicked = !prayClicked;
			stream.createFrame(185); 
			stream.writeWord(5000);
		}		
		if (l == 582) {
			NPC npc = npcArray[i1];
			if (npc != null) {
				doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0, npc.smallY[0],
				         myPlayer.smallX[0], false, npc.smallX[0]);
				crossX = base.saveClickX;
				crossY = base.saveClickY;
				crossType = 2;
				crossIndex = 0;
				stream.createFrame(57);
				stream.method432(anInt1285);
				stream.method432(i1);
				stream.method431(anInt1283);
				stream.method432(anInt1284);
			}
		}
		if (l == 234) {
			bool flag1 = doWalkTo(2, 0, 0, 0, myPlayer.smallY[0], 0, 0, k,
			                         myPlayer.smallX[0], false, j);
			if (!flag1)
				flag1 = doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0, k,
				                 myPlayer.smallX[0], false, j);
			crossX = base.saveClickX;
			crossY = base.saveClickY;
			crossType = 2;
			crossIndex = 0;
			stream.createFrame(236);
			stream.method431(k + baseY);
			stream.writeWord(i1);
			stream.method431(j + baseX);
		}
		if (l == 62 && method66(i1, k, j)) {
			stream.createFrame(192);
			stream.writeWord(anInt1284);
			stream.method431(i1 >> 14 & 0x7fff);
			stream.method433(k + baseY);
			stream.method431(anInt1283);
			stream.method433(j + baseX);
			stream.writeWord(anInt1285);
		}
		if (l == 511) {
			bool flag2 = doWalkTo(2, 0, 0, 0, myPlayer.smallY[0], 0, 0, k,
			                         myPlayer.smallX[0], false, j);
			if (!flag2)
				flag2 = doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0, k,
				                 myPlayer.smallX[0], false, j);
			crossX = base.saveClickX;
			crossY = base.saveClickY;
			crossType = 2;
			crossIndex = 0;
			stream.createFrame(25);
			stream.method431(anInt1284);
			stream.method432(anInt1285);
			stream.writeWord(i1);
			stream.method432(k + baseY);
			stream.method433(anInt1283);
			stream.writeWord(j + baseX);
		}
		if (l == 74) {
			stream.createFrame(122);
			stream.method433(k);
			stream.method432(j);
			stream.method431(i1);
			atInventoryLoopCycle = 0;
			atInventoryInterface = k;
			atInventoryIndex = j;
			atInventoryInterfaceType = 2;
			if (RSInterface.interfaceCache[k].parentID == openInterfaceID)
				atInventoryInterfaceType = 1;
			if (RSInterface.interfaceCache[k].parentID == backDialogID)
				atInventoryInterfaceType = 3;
		}
		if (l == 315) {
			RSInterface class9 = RSInterface.interfaceCache[k];
			bool flag8 = true;
			if (class9.anInt214 > 0)
				flag8 = promptUserForInput(class9);
			if (flag8) {
				switch (k) {
					/* Client-sided button clicking */
				case 1668:
					sendFrame248(17500, 3213);
					break;
				default:
					stream.createFrame(185);
					stream.writeWord(k);
					break;
				}
			}
		}
		if (l == 561) {
			Player player = playerArray[i1];
			if (player != null) {
				doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0,
				         player.smallY[0], myPlayer.smallX[0], false,
				         player.smallX[0]);
				crossX = base.saveClickX;
				crossY = base.saveClickY;
				crossType = 2;
				crossIndex = 0;
				anInt1188 += i1;
				if (anInt1188 >= 90) {
					stream.createFrame(136);
					anInt1188 = 0;
				}
				stream.createFrame(128);
				stream.writeWord(i1);
			}
		}
		if (l == 20) {
			NPC class30_sub2_sub4_sub1_sub1_1 = npcArray[i1];
			if (class30_sub2_sub4_sub1_sub1_1 != null) {
				doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0,
				         class30_sub2_sub4_sub1_sub1_1.smallY[0],
				         myPlayer.smallX[0], false,
				         class30_sub2_sub4_sub1_sub1_1.smallX[0]);
				crossX = base.saveClickX;
				crossY = base.saveClickY;
				crossType = 2;
				crossIndex = 0;
				stream.createFrame(155);
				stream.method431(i1);
			}
		}
		if (l == 779) {
			Player class30_sub2_sub4_sub1_sub2_1 = playerArray[i1];
			if (class30_sub2_sub4_sub1_sub2_1 != null) {
				doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0,
				         class30_sub2_sub4_sub1_sub2_1.smallY[0],
				         myPlayer.smallX[0], false,
				         class30_sub2_sub4_sub1_sub2_1.smallX[0]);
				crossX = base.saveClickX;
				crossY = base.saveClickY;
				crossType = 2;
				crossIndex = 0;
				stream.createFrame(153);
				stream.method431(i1);
			}
		}
		if (l == 516)
			if (!menuOpen){}
				//worldController.method312(base.saveClickY - 4,
//				                          base.saveClickX - 4);
		else
			//worldController.method312(k - 4, j - 4);
		if (l == 1062) {
			anInt924 += baseX;
			if (anInt924 >= 113) {
				stream.createFrame(183);
				stream.writeDWordBigEndian(0xe63271);
				anInt924 = 0;
			}
			method66(i1, k, j);
			stream.createFrame(228);
			stream.method432(i1 >> 14 & 0x7fff);
			stream.method432(k + baseY);
			stream.writeWord(j + baseX);
		}
		if (l == 679 && !aBoolean1149) {
			stream.createFrame(40);
			stream.writeWord(k);
			aBoolean1149 = true;
		}
		if (l == 431) {
			stream.createFrame(129);
			stream.method432(j);
			stream.writeWord(k);
			stream.method432(i1);
			atInventoryLoopCycle = 0;
			atInventoryInterface = k;
			atInventoryIndex = j;
			atInventoryInterfaceType = 2;
			if (RSInterface.interfaceCache[k].parentID == openInterfaceID)
				atInventoryInterfaceType = 1;
			if (RSInterface.interfaceCache[k].parentID == backDialogID)
				atInventoryInterfaceType = 3;
		}
		if (l == 337 || l == 42 || l == 792 || l == 322) {
			String s = menuActionName[i];
			int k1 = s.IndexOf("@whi@");
			if (k1 != -1) {
				long l3 = TextClass.longForName(s.Substring(k1 + 5).Trim());
				if (l == 337)
					addFriend(l3);
				if (l == 42)
					addIgnore(l3);
				if (l == 792)
					delFriend(l3);
				if (l == 322)
					delIgnore(l3);
			}
		}
		if (l == 53) {
			stream.createFrame(135);
			stream.method431(j);
			stream.method432(k);
			stream.method431(i1);
			atInventoryLoopCycle = 0;
			atInventoryInterface = k;
			atInventoryIndex = j;
			atInventoryInterfaceType = 2;
			if (RSInterface.interfaceCache[k].parentID == openInterfaceID)
				atInventoryInterfaceType = 1;
			if (RSInterface.interfaceCache[k].parentID == backDialogID)
				atInventoryInterfaceType = 3;
		}
		if (l == 539) {
			stream.createFrame(16);
			stream.method432(i1);
			stream.method433(j);
			stream.method433(k);
			atInventoryLoopCycle = 0;
			atInventoryInterface = k;
			atInventoryIndex = j;
			atInventoryInterfaceType = 2;
			if (RSInterface.interfaceCache[k].parentID == openInterfaceID)
				atInventoryInterfaceType = 1;
			if (RSInterface.interfaceCache[k].parentID == backDialogID)
				atInventoryInterfaceType = 3;
		}
		if (l == 484 || l == 6) {
			String s1 = menuActionName[i];
			int l1 = s1.IndexOf("@whi@");
			if (l1 != -1) {
				s1 = s1.Substring(l1 + 5).Trim();
				String s7 = TextClass.fixName(TextClass.nameForLong(TextClass
				                                                    .longForName(s1)));
				bool flag9 = false;
				for (int j3 = 0; j3 < playerCount; j3++) {
					Player class30_sub2_sub4_sub1_sub2_7 = playerArray[playerIndices[j3]];
					if (class30_sub2_sub4_sub1_sub2_7 == null
					    || class30_sub2_sub4_sub1_sub2_7.name == null
					    || !class30_sub2_sub4_sub1_sub2_7.name
					    .ToLower().Equals(s7))
						continue;
					doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0,
					         class30_sub2_sub4_sub1_sub2_7.smallY[0],
					         myPlayer.smallX[0], false,
					         class30_sub2_sub4_sub1_sub2_7.smallX[0]);
					if (l == 484) {
						stream.createFrame(139);
						stream.method431(playerIndices[j3]);
					}
					if (l == 6) {
						anInt1188 += i1;
						if (anInt1188 >= 90) {
							stream.createFrame(136);
							anInt1188 = 0;
						}
						stream.createFrame(128);
						stream.writeWord(playerIndices[j3]);
					}
					flag9 = true;
					break;
				}
				
				if (!flag9)
					pushMessage("Unable to find " + s7, 0, "");
			}
		}
		if (l == 870) {
			stream.createFrame(53);
			stream.writeWord(j);
			stream.method432(anInt1283);
			stream.method433(i1);
			stream.writeWord(anInt1284);
			stream.method431(anInt1285);
			stream.writeWord(k);
			atInventoryLoopCycle = 0;
			atInventoryInterface = k;
			atInventoryIndex = j;
			atInventoryInterfaceType = 2;
			if (RSInterface.interfaceCache[k].parentID == openInterfaceID)
				atInventoryInterfaceType = 1;
			if (RSInterface.interfaceCache[k].parentID == backDialogID)
				atInventoryInterfaceType = 3;
		}
		if (l == 847) {
			stream.createFrame(87);
			stream.method432(i1);
			stream.writeWord(k);
			stream.method432(j);
			atInventoryLoopCycle = 0;
			atInventoryInterface = k;
			atInventoryIndex = j;
			atInventoryInterfaceType = 2;
			if (RSInterface.interfaceCache[k].parentID == openInterfaceID)
				atInventoryInterfaceType = 1;
			if (RSInterface.interfaceCache[k].parentID == backDialogID)
				atInventoryInterfaceType = 3;
		}
		if (l == 626) {
			RSInterface class9_1 = RSInterface.interfaceCache[k];
			spellSelected = 1;
			anInt1137 = k;
			spellUsableOn = class9_1.spellUsableOn;
			itemSelected = 0;
			needDrawTabArea = true;
			String s4 = class9_1.selectedActionName;
			if (s4.IndexOf(" ") != -1)
				s4 = s4.Substring(0, s4.IndexOf(" "));
			String s8 = class9_1.selectedActionName;
			if (s8.IndexOf(" ") != -1)
				s8 = s8.Substring(s8.IndexOf(" ") + 1);
			spellTooltip = s4 + " " + class9_1.spellName + " " + s8;
			if (spellUsableOn == 16) {
				needDrawTabArea = true;
				tabID = 3;
				tabAreaAltered = true;
			}
			return;
		}
		if (l == 78) {
			stream.createFrame(117);
			stream.method433(k);
			stream.method433(i1);
			stream.method431(j);
			atInventoryLoopCycle = 0;
			atInventoryInterface = k;
			atInventoryIndex = j;
			atInventoryInterfaceType = 2;
			if (RSInterface.interfaceCache[k].parentID == openInterfaceID)
				atInventoryInterfaceType = 1;
			if (RSInterface.interfaceCache[k].parentID == backDialogID)
				atInventoryInterfaceType = 3;
		}
		if (l == 27) {
			Player class30_sub2_sub4_sub1_sub2_2 = playerArray[i1];
			if (class30_sub2_sub4_sub1_sub2_2 != null) {
				doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0,
				         class30_sub2_sub4_sub1_sub2_2.smallY[0],
				         myPlayer.smallX[0], false,
				         class30_sub2_sub4_sub1_sub2_2.smallX[0]);
				crossX = base.saveClickX;
				crossY = base.saveClickY;
				crossType = 2;
				crossIndex = 0;
				anInt986 += i1;
				if (anInt986 >= 54) {
					stream.createFrame(189);
					stream.writeWordBigEndian(234);
					anInt986 = 0;
				}
				stream.createFrame(73);
				stream.method431(i1);
			}
		}
		if (l == 213) {
			bool flag3 = doWalkTo(2, 0, 0, 0, myPlayer.smallY[0], 0, 0, k,
			                         myPlayer.smallX[0], false, j);
			if (!flag3)
				flag3 = doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0, k,
				                 myPlayer.smallX[0], false, j);
			crossX = base.saveClickX;
			crossY = base.saveClickY;
			crossType = 2;
			crossIndex = 0;
			stream.createFrame(79);
			stream.method431(k + baseY);
			stream.writeWord(i1);
			stream.method432(j + baseX);
		}
		if (l == 632) {
			stream.createFrame(145);
			stream.method432(k);
			stream.method432(j);
			stream.method432(i1);
			atInventoryLoopCycle = 0;
			atInventoryInterface = k;
			atInventoryIndex = j;
			atInventoryInterfaceType = 2;
			if (RSInterface.interfaceCache[k].parentID == openInterfaceID)
				atInventoryInterfaceType = 1;
			if (RSInterface.interfaceCache[k].parentID == backDialogID)
				atInventoryInterfaceType = 3;
		}
		if (l == 1004) {
			if (tabInterfaceIDs[10] != -1) {
				needDrawTabArea = true;
				tabID = 10;
				tabAreaAltered = true;
			}
		}
		if (l == 1003) {
			clanChatMode = 2;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 1002) {
			clanChatMode = 1;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 1001) {
			clanChatMode = 0;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 1000) {
			cButtonCPos = 4;
			chatTypeView = 11;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 999) {
			cButtonCPos = 0;
			chatTypeView = 0;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 998) {
			cButtonCPos = 1;
			chatTypeView = 5;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 997) {
			publicChatMode = 3;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 996) {
			publicChatMode = 2;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 995) {
			publicChatMode = 1;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 994) {
			publicChatMode = 0;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 993) {
			cButtonCPos = 2;
			chatTypeView = 1;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 992) {
			privateChatMode = 2;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 991) {
			privateChatMode = 1;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 990) {
			privateChatMode = 0;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 989) {
			cButtonCPos = 3;
			chatTypeView = 2;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 987) {
			tradeMode = 2;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 986) {
			tradeMode = 1;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 985) {
			tradeMode = 0;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 984) {
			cButtonCPos = 5;
			chatTypeView = 3;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 983) {
			duelMode = 2;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 982) {
			duelMode = 1;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 981) {
			duelMode = 0;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 980) {
			cButtonCPos = 6;
			chatTypeView = 4;
			aBoolean1233 = true;
			inputTaken = true;
		}
		if (l == 493) {
			stream.createFrame(75);
			stream.method433(k);
			stream.method431(j);
			stream.method432(i1);
			atInventoryLoopCycle = 0;
			atInventoryInterface = k;
			atInventoryIndex = j;
			atInventoryInterfaceType = 2;
			if (RSInterface.interfaceCache[k].parentID == openInterfaceID)
				atInventoryInterfaceType = 1;
			if (RSInterface.interfaceCache[k].parentID == backDialogID)
				atInventoryInterfaceType = 3;
		}
		if (l == 652) {
			bool flag4 = doWalkTo(2, 0, 0, 0, myPlayer.smallY[0], 0, 0, k,
			                         myPlayer.smallX[0], false, j);
			if (!flag4)
				flag4 = doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0, k,
				                 myPlayer.smallX[0], false, j);
			crossX = base.saveClickX;
			crossY = base.saveClickY;
			crossType = 2;
			crossIndex = 0;
			stream.createFrame(156);
			stream.method432(j + baseX);
			stream.method431(k + baseY);
			stream.method433(i1);
		}
		if (l == 94) {
			bool flag5 = doWalkTo(2, 0, 0, 0, myPlayer.smallY[0], 0, 0, k,
			                         myPlayer.smallX[0], false, j);
			if (!flag5)
				flag5 = doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0, k,
				                 myPlayer.smallX[0], false, j);
			crossX = base.saveClickX;
			crossY = base.saveClickY;
			crossType = 2;
			crossIndex = 0;
			stream.createFrame(181);
			stream.method431(k + baseY);
			stream.writeWord(i1);
			stream.method431(j + baseX);
			stream.method432(anInt1137);
		}
		if (l == 646) {
			stream.createFrame(185);
			stream.writeWord(k);
			RSInterface class9_2 = RSInterface.interfaceCache[k];
			if (class9_2.valueIndexArray != null
			    && class9_2.valueIndexArray[0][0] == 5) {
				int i2 = class9_2.valueIndexArray[0][1];
				if (variousSettings[i2] != class9_2.anIntArray212[0]) {
					variousSettings[i2] = class9_2.anIntArray212[0];
					method33(i2);
					needDrawTabArea = true;
				}
			}
		}
		if (l == 225) {
			NPC class30_sub2_sub4_sub1_sub1_2 = npcArray[i1];
			if (class30_sub2_sub4_sub1_sub1_2 != null) {
				doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0,
				         class30_sub2_sub4_sub1_sub1_2.smallY[0],
				         myPlayer.smallX[0], false,
				         class30_sub2_sub4_sub1_sub1_2.smallX[0]);
				crossX = base.saveClickX;
				crossY = base.saveClickY;
				crossType = 2;
				crossIndex = 0;
				anInt1226 += i1;
				if (anInt1226 >= 85) {
					stream.createFrame(230);
					stream.writeWordBigEndian(239);
					anInt1226 = 0;
				}
				stream.createFrame(17);
				stream.method433(i1);
			}
		}
		if (l == 965) {
			NPC class30_sub2_sub4_sub1_sub1_3 = npcArray[i1];
			if (class30_sub2_sub4_sub1_sub1_3 != null) {
				doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0,
				         class30_sub2_sub4_sub1_sub1_3.smallY[0],
				         myPlayer.smallX[0], false,
				         class30_sub2_sub4_sub1_sub1_3.smallX[0]);
				crossX = base.saveClickX;
				crossY = base.saveClickY;
				crossType = 2;
				crossIndex = 0;
				anInt1134++;
				if (anInt1134 >= 96) {
					stream.createFrame(152);
					stream.writeWordBigEndian(88);
					anInt1134 = 0;
				}
				stream.createFrame(21);
				stream.writeWord(i1);
			}
		}
		if (l == 413) {
			NPC class30_sub2_sub4_sub1_sub1_4 = npcArray[i1];
			if (class30_sub2_sub4_sub1_sub1_4 != null) {
				doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0,
				         class30_sub2_sub4_sub1_sub1_4.smallY[0],
				         myPlayer.smallX[0], false,
				         class30_sub2_sub4_sub1_sub1_4.smallX[0]);
				crossX = base.saveClickX;
				crossY = base.saveClickY;
				crossType = 2;
				crossIndex = 0;
				stream.createFrame(131);
				stream.method433(i1);
				stream.method432(anInt1137);
			}
		}
		if (l == 200)
			clearTopInterfaces();
		if (l == 1025) {
			NPC class30_sub2_sub4_sub1_sub1_5 = npcArray[i1];
			if (class30_sub2_sub4_sub1_sub1_5 != null) {
				EntityDef entityDef = class30_sub2_sub4_sub1_sub1_5.desc;
				if (entityDef.childrenIDs != null)
					entityDef = entityDef.method161();
				if (entityDef != null) {
					String s9;
					if (entityDef.description != null)
						s9 = ASCIIEncoding.ASCII.GetString(entityDef.description);//new String(entityDef.description);
					else
						s9 = "It's a " + entityDef.name + ".";
					pushMessage(s9, 0, "");
				}
			}
		}
		if (l == 900) {
			method66(i1, k, j);
			stream.createFrame(252);
			stream.method433(i1 >> 14 & 0x7fff);
			stream.method431(k + baseY);
			stream.method432(j + baseX);
		}
		if (l == 412) {
			NPC class30_sub2_sub4_sub1_sub1_6 = npcArray[i1];
			if (class30_sub2_sub4_sub1_sub1_6 != null) {
				doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0,
				         class30_sub2_sub4_sub1_sub1_6.smallY[0],
				         myPlayer.smallX[0], false,
				         class30_sub2_sub4_sub1_sub1_6.smallX[0]);
				crossX = base.saveClickX;
				crossY = base.saveClickY;
				crossType = 2;
				crossIndex = 0;
				stream.createFrame(72);
				stream.method432(i1);
			}
		}
		if (l == 365) {
			Player class30_sub2_sub4_sub1_sub2_3 = playerArray[i1];
			if (class30_sub2_sub4_sub1_sub2_3 != null) {
				doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0,
				         class30_sub2_sub4_sub1_sub2_3.smallY[0],
				         myPlayer.smallX[0], false,
				         class30_sub2_sub4_sub1_sub2_3.smallX[0]);
				crossX = base.saveClickX;
				crossY = base.saveClickY;
				crossType = 2;
				crossIndex = 0;
				stream.createFrame(249);
				stream.method432(i1);
				stream.method431(anInt1137);
			}
		}
		if (l == 729) {
			Player class30_sub2_sub4_sub1_sub2_4 = playerArray[i1];
			if (class30_sub2_sub4_sub1_sub2_4 != null) {
				doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0,
				         class30_sub2_sub4_sub1_sub2_4.smallY[0],
				         myPlayer.smallX[0], false,
				         class30_sub2_sub4_sub1_sub2_4.smallX[0]);
				crossX = base.saveClickX;
				crossY = base.saveClickY;
				crossType = 2;
				crossIndex = 0;
				stream.createFrame(39);
				stream.method431(i1);
			}
		}
		if (l == 577) {
			Player class30_sub2_sub4_sub1_sub2_5 = playerArray[i1];
			if (class30_sub2_sub4_sub1_sub2_5 != null) {
				doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0,
				         class30_sub2_sub4_sub1_sub2_5.smallY[0],
				         myPlayer.smallX[0], false,
				         class30_sub2_sub4_sub1_sub2_5.smallX[0]);
				crossX = base.saveClickX;
				crossY = base.saveClickY;
				crossType = 2;
				crossIndex = 0;
				stream.createFrame(139);
				stream.method431(i1);
			}
		}
		if (l == 956 && method66(i1, k, j)) {
			stream.createFrame(35);
			stream.method431(j + baseX);
			stream.method432(anInt1137);
			stream.method432(k + baseY);
			stream.method431(i1 >> 14 & 0x7fff);
		}
		if (l == 567) {
			bool flag6 = doWalkTo(2, 0, 0, 0, myPlayer.smallY[0], 0, 0, k,
			                         myPlayer.smallX[0], false, j);
			if (!flag6)
				flag6 = doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0, k,
				                 myPlayer.smallX[0], false, j);
			crossX = base.saveClickX;
			crossY = base.saveClickY;
			crossType = 2;
			crossIndex = 0;
			stream.createFrame(23);
			stream.method431(k + baseY);
			stream.method431(i1);
			stream.method431(j + baseX);
		}
		if (l == 867) {
			if ((i1 & 3) == 0)
				anInt1175++;
			if (anInt1175 >= 59) {
				stream.createFrame(200);
				stream.writeWord(25501);
				anInt1175 = 0;
			}
			stream.createFrame(43);
			stream.method431(k);
			stream.method432(i1);
			stream.method432(j);
			atInventoryLoopCycle = 0;
			atInventoryInterface = k;
			atInventoryIndex = j;
			atInventoryInterfaceType = 2;
			if (RSInterface.interfaceCache[k].parentID == openInterfaceID)
				atInventoryInterfaceType = 1;
			if (RSInterface.interfaceCache[k].parentID == backDialogID)
				atInventoryInterfaceType = 3;
		}
		if (l == 543) {
			stream.createFrame(237);
			stream.writeWord(j);
			stream.method432(i1);
			stream.writeWord(k);
			stream.method432(anInt1137);
			atInventoryLoopCycle = 0;
			atInventoryInterface = k;
			atInventoryIndex = j;
			atInventoryInterfaceType = 2;
			if (RSInterface.interfaceCache[k].parentID == openInterfaceID)
				atInventoryInterfaceType = 1;
			if (RSInterface.interfaceCache[k].parentID == backDialogID)
				atInventoryInterfaceType = 3;
		}
		if (l == 606) {
			String s2 = menuActionName[i];
			int j2 = s2.IndexOf("@whi@");
			if (j2 != -1)
			if (openInterfaceID == -1) {
				clearTopInterfaces();
				reportAbuseInput = s2.Substring(j2 + 5).Trim();
				canMute = false;
				for (int i3 = 0; i3 < RSInterface.interfaceCache.Length; i3++) {
					if (RSInterface.interfaceCache[i3] == null
					    || RSInterface.interfaceCache[i3].anInt214 != 600)
						continue;
					reportAbuseInterfaceID = openInterfaceID = RSInterface.interfaceCache[i3].parentID;
					break;
				}
				
			} else {
				pushMessage(
					"Please close the interface you have open before using 'report abuse'",
					0, "");
			}
		}
		if (l == 491) {
			Player class30_sub2_sub4_sub1_sub2_6 = playerArray[i1];
			if (class30_sub2_sub4_sub1_sub2_6 != null) {
				doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0,
				         class30_sub2_sub4_sub1_sub2_6.smallY[0],
				         myPlayer.smallX[0], false,
				         class30_sub2_sub4_sub1_sub2_6.smallX[0]);
				crossX = base.saveClickX;
				crossY = base.saveClickY;
				crossType = 2;
				crossIndex = 0;
				stream.createFrame(14);
				stream.method432(anInt1284);
				stream.writeWord(i1);
				stream.writeWord(anInt1285);
				stream.method431(anInt1283);
			}
		}
		if (l == 639) {
			String s3 = menuActionName[i];
			int k2 = s3.IndexOf("@whi@");
			if (k2 != -1) {
				long l4 = TextClass.longForName(s3.Substring(k2 + 5).Trim());
				int k3 = -1;
				for (int i4 = 0; i4 < friendsCount; i4++) {
					if (friendsListAsLongs[i4] != l4)
						continue;
					k3 = i4;
					break;
				}
				
				if (k3 != -1 && friendsNodeIDs[k3] > 0) {
					inputTaken = true;
					inputDialogState = 0;
					messagePromptRaised = true;
					promptInput = "";
					friendsListAction = 3;
					aLong953 = friendsListAsLongs[k3];
					aString1121 = "Enter message to send to " + friendsList[k3];
				}
			}
		}
		if (l == 454) {
			stream.createFrame(41);
			stream.writeWord(i1);
			stream.method432(j);
			stream.method432(k);
			atInventoryLoopCycle = 0;
			atInventoryInterface = k;
			atInventoryIndex = j;
			atInventoryInterfaceType = 2;
			if (RSInterface.interfaceCache[k].parentID == openInterfaceID)
				atInventoryInterfaceType = 1;
			if (RSInterface.interfaceCache[k].parentID == backDialogID)
				atInventoryInterfaceType = 3;
		}
		if (l == 478) {
			NPC class30_sub2_sub4_sub1_sub1_7 = npcArray[i1];
			if (class30_sub2_sub4_sub1_sub1_7 != null) {
				doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0,
				         class30_sub2_sub4_sub1_sub1_7.smallY[0],
				         myPlayer.smallX[0], false,
				         class30_sub2_sub4_sub1_sub1_7.smallX[0]);
				crossX = base.saveClickX;
				crossY = base.saveClickY;
				crossType = 2;
				crossIndex = 0;
				if ((i1 & 3) == 0)
					anInt1155++;
				if (anInt1155 >= 53) {
					stream.createFrame(85);
					stream.writeWordBigEndian(66);
					anInt1155 = 0;
				}
				stream.createFrame(18);
				stream.method431(i1);
			}
		}
		if (l == 113) {
			method66(i1, k, j);
			stream.createFrame(70);
			stream.method431(j + baseX);
			stream.writeWord(k + baseY);
			stream.method433(i1 >> 14 & 0x7fff);
		}
		if (l == 872) {
			method66(i1, k, j);
			stream.createFrame(234);
			stream.method433(j + baseX);
			stream.method432(i1 >> 14 & 0x7fff);
			stream.method433(k + baseY);
		}
		if (l == 502) {
			method66(i1, k, j);
			stream.createFrame(132);
			stream.method433(j + baseX);
			stream.writeWord(i1 >> 14 & 0x7fff);
			stream.method432(k + baseY);
		}
		if (l == 1125) {
			ItemDef itemDef = ItemDef.forID(i1);
			RSInterface class9_4 = RSInterface.interfaceCache[k];
			String s5;
			if (class9_4 != null && class9_4.invStackSizes[j] >= 0x186a0)
				s5 = class9_4.invStackSizes[j] + " x " + itemDef.name;
			else if (itemDef.description != null)
				s5 = itemDef.description;//new String(itemDef.description);
			else
				s5 = "It's a " + itemDef.name + ".";
			pushMessage(s5, 0, "");
		}
		if (l == 169) {
			stream.createFrame(185);
			stream.writeWord(k);
			RSInterface class9_3 = RSInterface.interfaceCache[k];
			if (class9_3.valueIndexArray != null
			    && class9_3.valueIndexArray[0][0] == 5) {
				int l2 = class9_3.valueIndexArray[0][1];
				variousSettings[l2] = 1 - variousSettings[l2];
				method33(l2);
				needDrawTabArea = true;
			}
		}
		if (l == 447) {
			itemSelected = 1;
			anInt1283 = j;
			anInt1284 = k;
			anInt1285 = i1;
			selectedItemName = ItemDef.forID(i1).name;
			spellSelected = 0;
			needDrawTabArea = true;
			return;
		}
		if (l == 1226) {
			int j1 = i1 >> 14 & 0x7fff;
			ObjectDef class46 = ObjectDef.forID(j1);
			String s10;
			if (class46.description != null)
				s10 = ASCIIEncoding.ASCII.GetString(class46.description) + " ID: " + j1;//new String(class46.description);
			else
				s10 = "It's a " + class46.name + "." + " ID: " + j1;
			pushMessage(s10, 0, "");
		}
		if (l == 244) {
			bool flag7 = doWalkTo(2, 0, 0, 0, myPlayer.smallY[0], 0, 0, k,
			                         myPlayer.smallX[0], false, j);
			if (!flag7)
				flag7 = doWalkTo(2, 0, 1, 0, myPlayer.smallY[0], 1, 0, k,
				                 myPlayer.smallX[0], false, j);
			crossX = base.saveClickX;
			crossY = base.saveClickY;
			crossType = 2;
			crossIndex = 0;
			stream.createFrame(253);
			stream.method431(j + baseX);
			stream.method433(k + baseY);
			stream.method432(i1);
		}
		if (l == 1448) {
			ItemDef itemDef_1 = ItemDef.forID(i1);
			String s6;
			if (itemDef_1.description != null)
				s6 = itemDef_1.description;//new String(itemDef_1.description);
			else
				s6 = "It's a " + itemDef_1.name + ".";
			pushMessage(s6, 0, "");
		}
		itemSelected = 0;
		spellSelected = 0;
		needDrawTabArea = true;
		
	}
	
	private void method70() {
		anInt1251 = 0;
		int j = (myPlayer.x >> 7) + baseX;
		int k = (myPlayer.y >> 7) + baseY;
		if (j >= 3053 && j <= 3156 && k >= 3056 && k <= 3136)
			anInt1251 = 1;
		if (j >= 3072 && j <= 3118 && k >= 9492 && k <= 9535)
			anInt1251 = 1;
		if (anInt1251 == 1 && j >= 3139 && j <= 3199 && k >= 3008 && k <= 3062)
			anInt1251 = 0;
	}

//	public void run() {
//		if (drawFlames) {
//			// drawFlames();
//		} else {
//			base.run();
//		}
//	}
	
	private void build3dScreenMenu() {
		if (itemSelected == 0 && spellSelected == 0) {
			menuActionName[menuActionRow] = "Walk here";
			menuActionID[menuActionRow] = 516;
			menuActionCmd2[menuActionRow] = base.mouseX;
			menuActionCmd3[menuActionRow] = base.mouseY;
			menuActionRow++;
		}
		int previous = -1;
		for (int k = 0; k < Model.anInt1687; k++) {
			int config = Model.MouseOnObjects[k];
			int x = config & 0x7f;
			int y = config >> 7 & 0x7f;
			int type = config >> 29 & 3;
			int id = config >> 14 & 0x7fff;
			if (config == previous)
				continue;
			previous = config;
			if (type == 2) {// && //worldController.method304(plane, x, y, config) >= 0) {
				ObjectDef class46 = ObjectDef.forID(id);
				if (class46.childrenIDs != null)
					class46 = class46.method580();
				if (class46 == null)
					continue;
				if (itemSelected == 1) {
					menuActionName[menuActionRow] = "Use " + selectedItemName
						+ " with @cya@" + class46.name;
					menuActionID[menuActionRow] = 62;
					menuActionCmd1[menuActionRow] = config;
					menuActionCmd2[menuActionRow] = x;
					menuActionCmd3[menuActionRow] = y;
					menuActionRow++;
				} else if (spellSelected == 1) {
					if ((spellUsableOn & 4) == 4) {
						menuActionName[menuActionRow] = spellTooltip + " @cya@"
							+ class46.name;
						menuActionID[menuActionRow] = 956;
						menuActionCmd1[menuActionRow] = config;
						menuActionCmd2[menuActionRow] = x;
						menuActionCmd3[menuActionRow] = y;
						menuActionRow++;
					}
				} else {
					if (class46.actions != null) {
						for (int i2 = 4; i2 >= 0; i2--)
						if (class46.actions[i2] != null) {
							menuActionName[menuActionRow] = class46.actions[i2]
							+ " @cya@" + class46.name;
							if (i2 == 0)
								menuActionID[menuActionRow] = 502;
							if (i2 == 1)
								menuActionID[menuActionRow] = 900;
							if (i2 == 2)
								menuActionID[menuActionRow] = 113;
							if (i2 == 3)
								menuActionID[menuActionRow] = 872;
							if (i2 == 4)
								menuActionID[menuActionRow] = 1062;
							menuActionCmd1[menuActionRow] = config;
							menuActionCmd2[menuActionRow] = x;
							menuActionCmd3[menuActionRow] = y;
							menuActionRow++;
						}
						
					}
					// menuActionName[menuActionRow] = "Examine @cya@" +
					// class46.name + " @gre@(@whi@" + l1 + "@gre@) (@whi@" +
					// (i1 + baseX) + "," + (j1 + baseY) + "@gre@)";
					menuActionName[menuActionRow] = "Examine @cya@"
						+ class46.name;
					menuActionID[menuActionRow] = 1226;
					menuActionCmd1[menuActionRow] = class46.type << 14;
					menuActionCmd2[menuActionRow] = x;
					menuActionCmd3[menuActionRow] = y;
					menuActionRow++;
				}
			}
			if (type == 1) {
				NPC npc = npcArray[id];
				if(npc != null) {
				if (npc.desc.aByte68 == 1 && (npc.x & 0x7f) == 64
				    && (npc.y & 0x7f) == 64) {
					for (int j2 = 0; j2 < npcCount; j2++) {
						NPC npc2 = npcArray[npcIndices[j2]];
						if (npc2 != null && npc2 != npc
						    && npc2.desc.aByte68 == 1 && npc2.x == npc.x
						    && npc2.y == npc.y)
							buildAtNPCMenu(npc2.desc, npcIndices[j2], y, x);
					}
					
					for (int l2 = 0; l2 < playerCount; l2++) {
						Player player = playerArray[playerIndices[l2]];
						if (player != null && player.x == npc.x
						    && player.y == npc.y)
							buildAtPlayerMenu(x, playerIndices[l2], player, y);
					}
					
				}
				buildAtNPCMenu(npc.desc, id, y, x);
				}
			}
			if (type == 0) {
				Player player = playerArray[id];
				if(player != null)
				{
				if ((player.x & 0x7f) == 64 && (player.y & 0x7f) == 64) {
					for (int k2 = 0; k2 < npcCount; k2++) {
						NPC class30_sub2_sub4_sub1_sub1_2 = npcArray[npcIndices[k2]];
						if (class30_sub2_sub4_sub1_sub1_2 != null && class30_sub2_sub4_sub1_sub1_2.desc != null
						    && class30_sub2_sub4_sub1_sub1_2.desc.aByte68 == 1
						    && class30_sub2_sub4_sub1_sub1_2.x == player.x
						    && class30_sub2_sub4_sub1_sub1_2.y == player.y)
							buildAtNPCMenu(class30_sub2_sub4_sub1_sub1_2.desc,
							               npcIndices[k2], y, x);
					}
					
					for (int i3 = 0; i3 < playerCount; i3++) {
						Player class30_sub2_sub4_sub1_sub2_2 = playerArray[playerIndices[i3]];
						if (class30_sub2_sub4_sub1_sub2_2 != null
						    && class30_sub2_sub4_sub1_sub2_2 != player
						    && class30_sub2_sub4_sub1_sub2_2.x == player.x
						    && class30_sub2_sub4_sub1_sub2_2.y == player.y)
							buildAtPlayerMenu(x, playerIndices[i3],
							                  class30_sub2_sub4_sub1_sub2_2, y);
					}
					
				}
				buildAtPlayerMenu(x, id, player, y);
				}
			}
			if (type == 3) {
			try{
				NodeList class19 = groundArray[plane][x][y];
				if (class19 != null) {
					for (Item item = (Item) class19.getFirst(); item != null; item = (Item) class19
					     .getNext()) {
						ItemDef itemDef = ItemDef.forID(item.ID);
						if (itemSelected == 1) {
							menuActionName[menuActionRow] = "Use "
								+ selectedItemName + " with @lre@"
									+ itemDef.name;
							menuActionID[menuActionRow] = 511;
							menuActionCmd1[menuActionRow] = item.ID;
							menuActionCmd2[menuActionRow] = x;
							menuActionCmd3[menuActionRow] = y;
							menuActionRow++;
						} else if (spellSelected == 1) {
							if ((spellUsableOn & 1) == 1) {
								menuActionName[menuActionRow] = spellTooltip
									+ " @lre@" + itemDef.name;
								menuActionID[menuActionRow] = 94;
								menuActionCmd1[menuActionRow] = item.ID;
								menuActionCmd2[menuActionRow] = x;
								menuActionCmd3[menuActionRow] = y;
								menuActionRow++;
							}
						} else {
							for (int j3 = 4; j3 >= 0; j3--)
								if (itemDef.groundActions != null
								   && itemDef.groundActions[j3] != null) {
								menuActionName[menuActionRow] = itemDef.groundActions[j3]
								+ " @lre@" + itemDef.name;
								if (j3 == 0)
									menuActionID[menuActionRow] = 652;
								if (j3 == 1)
									menuActionID[menuActionRow] = 567;
								if (j3 == 2)
									menuActionID[menuActionRow] = 234;
								if (j3 == 3)
									menuActionID[menuActionRow] = 244;
								if (j3 == 4)
									menuActionID[menuActionRow] = 213;
								menuActionCmd1[menuActionRow] = item.ID;
								menuActionCmd2[menuActionRow] = x;
								menuActionCmd3[menuActionRow] = y;
								menuActionRow++;
							} else if (j3 == 2) {
								menuActionName[menuActionRow] = "Take @lre@"
									+ itemDef.name;
								menuActionID[menuActionRow] = 234;
								menuActionCmd1[menuActionRow] = item.ID;
								menuActionCmd2[menuActionRow] = x;
								menuActionCmd3[menuActionRow] = y;
								menuActionRow++;
							}
							
							// menuActionName[menuActionRow] = "Examine @lre@" +
							// itemDef.name + " @gre@(@whi@" + item.ID +
							// "@gre@)";
							menuActionName[menuActionRow] = "Examine @lre@"
								+ itemDef.name;
							menuActionID[menuActionRow] = 1448;
							menuActionCmd1[menuActionRow] = item.ID;
							menuActionCmd2[menuActionRow] = x;
							menuActionCmd3[menuActionRow] = y;
							menuActionRow++;
						}
					}
					
				}
				}catch(Exception ex){}
			}
		}
	}

	public void cleanUpForQuit() {
		//signlink.reporterror = false;
		try {
			if (socketStream != null)
				socketStream.close();
		} catch (Exception _ex) {
		}
		//socketStream = null;
		stopMidi();
		if (mouseDetection != null)
			mouseDetection.running = false;
		mouseDetection = null;
		onDemandFetcher.disable();
		onDemandFetcher = null;
		aStream_834 = null;
		stream = null;
		aStream_847 = null;
		inStream = null;
		localRegionIds = null;
		localRegionMapData = null;
		localRegionLandscapeData = null;
		anIntArray1235 = null;
		anIntArray1236 = null;
		intGroundArray = null;
		byteGroundArray = null;
		//worldController = null;
		aClass11Array1230 = null;
		anIntArrayArray901 = null;
		anIntArrayArray825 = null;
		bigX = null;
		bigY = null;
		aByteArray912 = null;
		aRSImageProducer_1163 = null;
		mapEdgeIP = null;
		leftFrame = null;
		topFrame = null;
		rightFrame = null;
		aRSImageProducer_1164 = null;
		aRSImageProducer_1165 = null;
		aRSImageProducer_1166 = null;
		aRSImageProducer_1123 = null;
		aRSImageProducer_1124 = null;
		aRSImageProducer_1125 = null;
		/* Null pointers for custom sprites */
		chatArea = null;
		chatButtons = null;
		tabArea = null;
		mapArea = null;
		/**/
		mapBack = null;
		sideIcons = null;
		redStones = null;
		compass = null;
		hitMarks = null;
		headIcons = null;
		skullIcons = null;
		headIconsHint = null;
		crosses = null;
		mapDotItem = null;
		mapDotNPC = null;
		mapDotPlayer = null;
		mapDotFriend = null;
		mapDotTeam = null;
		mapScenes = null;
		mapFunctions = null;
		anIntArrayArray929 = null;
		playerArray = null;
		playerIndices = null;
		anIntArray894 = null;
		aStreamArray895s = null;
		anIntArray840 = null;
		npcArray = null;
		npcIndices = null;
		groundArray = null;
		aClass19_1179 = null;
		projectiles = null;
		aClass19_1056 = null;
		menuActionCmd2 = null;
		menuActionCmd3 = null;
		menuActionID = null;
		menuActionCmd1 = null;
		menuActionName = null;
		variousSettings = null;
		anIntArray1072 = null;
		anIntArray1073 = null;
		aClass30_Sub2_Sub1_Sub1Array1140 = null;
		aClass30_Sub2_Sub1_Sub1_1263 = null;
		friendsList = null;
		friendsListAsLongs = null;
		friendsNodeIDs = null;
		aRSImageProducer_1110 = null;
		aRSImageProducer_1111 = null;
		aRSImageProducer_1107 = null;
		aRSImageProducer_1108 = null;
		aRSImageProducer_1109 = null;
		aRSImageProducer_1112 = null;
		aRSImageProducer_1113 = null;
		aRSImageProducer_1114 = null;
		aRSImageProducer_1115 = null;
		nullLoader();
		ObjectDef.nullLoader();
		EntityDef.nullLoader();
		ItemDef.nullLoader();
		Flo.cache = null;
		IDK.cache = null;
		RSInterface.interfaceCache = null;
		DummyClass.cache = null;
		RS2Sharp.Animation.anims = null;
		SpotAnim.cache = null;
		SpotAnim.aMRUNodes_415 = null;
		Varp.cache = null;
		base.fullGameScreen = null;
		Player.mruNodes = null;
		RS2Sharp.Texture.nullLoader();
		//WorldController.nullLoader();
		Model.nullLoader();
		Class36.nullLoader();
		System.GC.Collect();
	}

	public override void clientUpdate()
	{
		//if(doDrawFlames)
			//drawFlames();
		//signlink.run ();
		if (!currentlyStarting) {
			clickMode3 = clickMode1;
			saveClickX = clickX;
			saveClickY = clickY;
			aLong29 = clickTime;
			clickMode1 = 0;
			//Loom.StartSingleThread (() => {
			processGameLoop ();//}, ThreadPriority.Normal, true);
			readIndex = writeIndex;
		}
	}
	
	private void printDebug() {
		UnityEngine.Debug.Log("============");
		UnityEngine.Debug.Log("flame-cycle:" + anInt1208);
		if (onDemandFetcher != null)
			UnityEngine.Debug.Log("Od-cycle:" + onDemandFetcher.onDemandCycle);
		UnityEngine.Debug.Log("loop-cycle:" + loopCycle);
		UnityEngine.Debug.Log("draw-cycle:" + anInt1061);
		UnityEngine.Debug.Log("ptype:" + pktType);
		UnityEngine.Debug.Log("psize:" + pktSize);
		if (socketStream != null)
			socketStream.printDebug();
		base.shouldDebug = true;
	}

//	Component getGameComponent() {
//		if (signlink.mainapp != null)
//			return signlink.mainapp;
//		if (base.gameFrame != null)
//			return base.gameFrame;
//		else
//			return this;
//	}

		public static byte[] ReadAllBytes (string fileName)
		{
			try {
				byte[] buffer = null;
				using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read)) {
					buffer = new byte[fs.Length];
					fs.Read (buffer, 0, (int)fs.Length);
				}
				return buffer;
			} catch (System.Exception ex){
				Debug.Log ("Error: " + ex.Message);
				return null;
			}
		} 

	private char convertChar(string s)
	{
		char c;
		char.TryParse(s,out c);
		return c;
	}
	
	private void method73() {
 		//do {
 			//int j = readChar(-796);
 			//if (j == -1)
 			//	break;
		if (currentInput == "")
						return;
 			if (openInterfaceID != -1 && openInterfaceID == reportAbuseInterfaceID) {
// 				if (j == 8 && reportAbuseInput.Length > 0)
// 					reportAbuseInput = reportAbuseInput.Substring(0,
// 					                                              reportAbuseInput.Length - 1);
// 				if ((j >= 97 && j <= 122 || j >= 65 && j <= 90 || j >= 48
// 				     && j <= 57 || j == 32)
// 				    && reportAbuseInput.Length < 12)
// 					reportAbuseInput += (char) j;
 			} else if (messagePromptRaised) {
// 				if (j >= 32 && j <= 122 && promptInput.Length < 80) {
// 					promptInput += (char) j;
// 					inputTaken = true;
// 				}
// 				if (j == 8 && promptInput.Length > 0) {
// 					promptInput = promptInput.Substring(0,
// 					                                    promptInput.Length - 1);
// 					inputTaken = true;
// 				}
// 				if (j == 13 || j == 10) {
// 					messagePromptRaised = false;
// 					inputTaken = true;
// 					if (friendsListAction == 1) {
// 						long l = TextClass.longForName(promptInput);
// 						addFriend(l);
// 					}
// 					if (friendsListAction == 2 && friendsCount > 0) {
// 						long l1 = TextClass.longForName(promptInput);
// 						delFriend(l1);
// 					}
// 					if (friendsListAction == 3 && promptInput.Length > 0) {
// 						stream.createFrame(126);
// 						stream.writeWordBigEndian(0);
// 						int k = stream.currentOffset;
// 						stream.writeQWord(aLong953);
// 						TextInput.method526(promptInput, stream);
// 						stream.writeBytes(stream.currentOffset - k);
// 						promptInput = TextInput.processText(promptInput);
// 						// promptInput = Censor.doCensor(promptInput);
// 						pushMessage(promptInput, 6, TextClass.fixName(TextClass
// 						                                              .nameForLong(aLong953)));
// 						if (privateChatMode == 2) {
// 							privateChatMode = 1;
// 							aBoolean1233 = true;
// 							stream.createFrame(95);
// 							stream.writeWordBigEndian(publicChatMode);
// 							stream.writeWordBigEndian(privateChatMode);
// 							stream.writeWordBigEndian(tradeMode);
// 						}
// 					}
// 					if (friendsListAction == 4 && ignoreCount < 100) {
// 						long l2 = TextClass.longForName(promptInput);
// 						addIgnore(l2);
// 					}
// 					if (friendsListAction == 5 && ignoreCount > 0) {
// 						long l3 = TextClass.longForName(promptInput);
// 						delIgnore(l3);
// 					}
 				//}
 			} else if (inputDialogState == 1) {
// 				if (j >= 48 && j <= 57 && amountOrNameInput.Length < 10) {
// 					amountOrNameInput += (char) j;
// 					inputTaken = true;
// 				}
// 				if (j == 8 && amountOrNameInput.Length > 0) {
// 					amountOrNameInput = amountOrNameInput.Substring(0,
// 					                                                amountOrNameInput.Length - 1);
// 					inputTaken = true;
// 				}
// 				if (j == 13 || j == 10) {
// 					if (amountOrNameInput.Length > 0) {
// 						int i1 = 0;
// 						try {
// 							i1 = Int32.Parse(amountOrNameInput);
// 						} catch (Exception _ex) {
// 						}
// 						stream.createFrame(208);
// 						stream.writeDWord(i1);
// 					}
// 					inputDialogState = 0;
// 					inputTaken = true;
// 				}
 			} else if (inputDialogState == 2) {
// 				if (j >= 32 && j <= 122 && amountOrNameInput.Length < 12) {
// 					amountOrNameInput += (char) j;
// 					inputTaken = true;
// 				}
// 				if (j == 8 && amountOrNameInput.Length > 0) {
// 					amountOrNameInput = amountOrNameInput.Substring(0,
// 					                                                amountOrNameInput.Length - 1);
// 					inputTaken = true;
// 				}
// 				if (j == 13 || j == 10) {
// 					if (amountOrNameInput.Length > 0) {
// 						stream.createFrame(60);
// 						stream.writeQWord(TextClass
// 						                  .longForName(amountOrNameInput));
// 					}
// 					inputDialogState = 0;
// 					inputTaken = true;
// 				}
 			} else if (backDialogID == -1) {
 				//if (j >= 32 && j <= 122 && inputString.Length < 80) {
			bool flag1 = false;
			for (int i2 = 0; i2 < validUserPassChars.Length; i2++)
			{
				if (convertChar(currentInput) != validUserPassChars[i2])
					continue;
				flag1 = true;
				break;
			}
			if(flag1)
			{
				inputString += convertChar(currentInput);
				inputTaken = true;
			}
 					//inputString += (char) j;
 					//inputTaken = true;
 				//}
			if (currentInput == "Backspace" && inputString.Length > 0) {
 					inputString = inputString.Substring(0,
 					                                    inputString.Length - 1);
 					inputTaken = true;
 				}
			if ((currentInput == "Enter") && inputString.Length > 0) {
 					if (myPrivilege == 2) {
 						if (inputString.StartsWith("//setspecto")) {
 							int amt = Int32.Parse(inputString
 							                           .Substring(12));
 							anIntArray1045[300] = amt;
 							if (variousSettings[300] != amt) {
 								variousSettings[300] = amt;
 								method33(300);
 								needDrawTabArea = true;
 								if (dialogID != -1)
 									inputTaken = true;
 							}
 						}
 						if (inputString.Equals("clientdrop"))
 							dropClient();
 						if (inputString.StartsWith("full")) {
 							try {
 								String[] args = inputString.Split(' ');
 								int id1 = Int32.Parse(args[1]);
 								int id2 = Int32.Parse(args[2]);
 								fullscreenInterfaceID = id1;
 								openInterfaceID = id2;
 								pushMessage("Opened Interface", 0, "");
 							} catch (Exception e) {
 								pushMessage("Interface Failed to load", 0, "");
 							}
 						}
 						if(inputString.Equals("::rel")) {
 							method22();
 						}
					if(inputString.Equals("::resetday")) {
						Loom.DispatchToMainThread(()=>{GameObject.Find("Sky Dome").GetComponent<TOD_Sky>().Cycle.Hour = 12;},true,true);
					}
 						if (inputString.Equals("::lag"))
 							printDebug();
 						if (inputString.Equals("::prefetchmusic")) {
 							for (int j1 = 0; j1 < onDemandFetcher
 							     .getVersionCount(2); j1++)
 								onDemandFetcher.method563((byte) 1, 2, j1);
 							
 						}
 						if (inputString.Equals("::fpson"))
 							fpsOn = true;
 						if (inputString.Equals("::fpsoff"))
 							fpsOn = false;
 						if (inputString.Equals("::dataon"))
 							clientData = true;
 						if (inputString.Equals("::dataoff"))
 							clientData = false;
 						if (inputString.Equals("::noclip")) {
 							for (int k1 = 0; k1 < 4; k1++) {
 								for (int i2 = 1; i2 < 103; i2++) {
 									for (int k2 = 1; k2 < 103; k2++)
 										aClass11Array1230[k1].anIntArrayArray294[i2][k2] = 0;
 									
 								}
 							}
 						}
 					}
 					if (inputString.Equals("add model")) {
// 						try {
// 							int ModelIndex = Int32.Parse(JOptionPane
// 							                                  .showInputDialog(this, "Enter model ID",
// 							                 "Model", 3));
// 							byte[] abyte0 = getModel(ModelIndex);
// 							if (abyte0 != null && abyte0.Length > 0) {
// 								decompressors[1].method234(abyte0.Length,
// 								                           abyte0, ModelIndex);
// 								pushMessage("Model: [" + ModelIndex
// 								            + "] added successfully!", 0, "");
// 							} else {
// 								pushMessage("Unable to find the model. "
// 								            + ModelIndex, 0, "");
// 							}
// 						} catch (Exception e) {
// 							pushMessage("Syntax - ::add model <path>", 0, "");
// 						}
 					}
 					if (inputString.StartsWith("::")) {
 						stream.createFrame(103);
 						stream.writeWordBigEndian(inputString.Length - 1);
 						stream.writeString(inputString.Substring(2));
 					} else {
 						String s = inputString.ToLower();
 						int j2 = 0;
 						if (s.StartsWith("yellow:")) {
 							j2 = 0;
 							inputString = inputString.Substring(7);
 						} else if (s.StartsWith("red:")) {
 							j2 = 1;
 							inputString = inputString.Substring(4);
 						} else if (s.StartsWith("green:")) {
 							j2 = 2;
 							inputString = inputString.Substring(6);
 						} else if (s.StartsWith("cyan:")) {
 							j2 = 3;
 							inputString = inputString.Substring(5);
 						} else if (s.StartsWith("purple:")) {
 							j2 = 4;
 							inputString = inputString.Substring(7);
 						} else if (s.StartsWith("white:")) {
 							j2 = 5;
 							inputString = inputString.Substring(6);
 						} else if (s.StartsWith("flash1:")) {
 							j2 = 6;
 							inputString = inputString.Substring(7);
 						} else if (s.StartsWith("flash2:")) {
 							j2 = 7;
 							inputString = inputString.Substring(7);
 						} else if (s.StartsWith("flash3:")) {
 							j2 = 8;
 							inputString = inputString.Substring(7);
 						} else if (s.StartsWith("glow1:")) {
 							j2 = 9;
 							inputString = inputString.Substring(6);
 						} else if (s.StartsWith("glow2:")) {
 							j2 = 10;
 							inputString = inputString.Substring(6);
 						} else if (s.StartsWith("glow3:")) {
 							j2 = 11;
 							inputString = inputString.Substring(6);
 						}
					s = inputString.ToLower();
 						int i3 = 0;
 						if (s.StartsWith("wave:")) {
 							i3 = 1;
 							inputString = inputString.Substring(5);
 						} else if (s.StartsWith("wave2:")) {
 							i3 = 2;
 							inputString = inputString.Substring(6);
 						} else if (s.StartsWith("shake:")) {
 							i3 = 3;
 							inputString = inputString.Substring(6);
 						} else if (s.StartsWith("scroll:")) {
 							i3 = 4;
 							inputString = inputString.Substring(7);
 						} else if (s.StartsWith("slide:")) {
 							i3 = 5;
 							inputString = inputString.Substring(6);
 						}
 						stream.createFrame(4);
 						stream.writeWordBigEndian(0);
 						int j3 = stream.currentOffset;
 						stream.method425(i3);
 						stream.method425(j2);
 						aStream_834.currentOffset = 0;
 						TextInput.method526(inputString, aStream_834);
 						stream.method441(0, aStream_834.buffer,
 						                 aStream_834.currentOffset);
 						stream.writeBytes(stream.currentOffset - j3);
 						inputString = TextInput.processText(inputString);
 						// inputString = Censor.doCensor(inputString);
 						myPlayer.textSpoken = inputString;
 						myPlayer.anInt1513 = j2;
 						myPlayer.anInt1531 = i3;
 						myPlayer.textCycle = 150;
 						if (myPrivilege == 2)
 							pushMessage(myPlayer.textSpoken, 2, "@cr2@"
 							            + myPlayer.name);
 						else if (myPrivilege == 1)
 							pushMessage(myPlayer.textSpoken, 2, "@cr1@"
 							            + myPlayer.name);
 						else
 							pushMessage(myPlayer.textSpoken, 2, myPlayer.name);
 						if (publicChatMode == 2) {
 							publicChatMode = 3;
 							aBoolean1233 = true;
 							stream.createFrame(95);
 							stream.writeWordBigEndian(publicChatMode);
 							stream.writeWordBigEndian(privateChatMode);
 							stream.writeWordBigEndian(tradeMode);
 						}
 					}
 					inputString = "";
 					inputTaken = true;
 				}
 			}
 		//} while (true);
	}
	
	private void buildPublicChat(int j) {
		int l = 0;
		for (int i1 = 0; i1 < 500; i1++) {
			if (chatMessages[i1] == null)
				continue;
			if (chatTypeView != 1)
				continue;
			int j1 = chatTypes[i1];
			String s = chatNames[i1];
			String ct = chatMessages[i1];
			int k1 = (70 - l * 14 + 42) + anInt1089 + 4 + 5;
			if (k1 < -23)
				break;
			if (s != null && s.StartsWith("@cr1@"))
				s = s.Substring(5);
			if (s != null && s.StartsWith("@cr2@"))
				s = s.Substring(5);
			if (s != null && s.StartsWith("@cr3@"))
				s = s.Substring(5);
			if ((j1 == 1 || j1 == 2)
			    && (j1 == 1 || publicChatMode == 0 || publicChatMode == 1
			    && isFriendOrSelf(s))) {
				if (j > k1 - 14 && j <= k1 && !s.Equals(myPlayer.name)) {
					if (myPrivilege >= 1) {
						menuActionName[menuActionRow] = "Report abuse @whi@"
							+ s;
						menuActionID[menuActionRow] = 606;
						menuActionRow++;
					}
					menuActionName[menuActionRow] = "Add ignore @whi@" + s;
					menuActionID[menuActionRow] = 42;
					menuActionRow++;
					menuActionName[menuActionRow] = "Add friend @whi@" + s;
					menuActionID[menuActionRow] = 337;
					menuActionRow++;
				}
				l++;
			}
		}
	}
	
	private void buildFriendChat(int j) {
		int l = 0;
		for (int i1 = 0; i1 < 500; i1++) {
			if (chatMessages[i1] == null)
				continue;
			if (chatTypeView != 2)
				continue;
			int j1 = chatTypes[i1];
			String s = chatNames[i1];
			String ct = chatMessages[i1];
			int k1 = (70 - l * 14 + 42) + anInt1089 + 4 + 5;
			if (k1 < -23)
				break;
			if (s != null && s.StartsWith("@cr1@"))
				s = s.Substring(5);
			if (s != null && s.StartsWith("@cr2@"))
				s = s.Substring(5);
			if (s != null && s.StartsWith("@cr3@"))
				s = s.Substring(5);
			if ((j1 == 5 || j1 == 6)
			    && (splitPrivateChat == 0 || chatTypeView == 2)
			    && (j1 == 6 || privateChatMode == 0 || privateChatMode == 1
			    && isFriendOrSelf(s)))
				l++;
			if ((j1 == 3 || j1 == 7)
			    && (splitPrivateChat == 0 || chatTypeView == 2)
			    && (j1 == 7 || privateChatMode == 0 || privateChatMode == 1
			    && isFriendOrSelf(s))) {
				if (j > k1 - 14 && j <= k1) {
					if (myPrivilege >= 1) {
						menuActionName[menuActionRow] = "Report abuse @whi@"
							+ s;
						menuActionID[menuActionRow] = 606;
						menuActionRow++;
					}
					menuActionName[menuActionRow] = "Add ignore @whi@" + s;
					menuActionID[menuActionRow] = 42;
					menuActionRow++;
					menuActionName[menuActionRow] = "Add friend @whi@" + s;
					menuActionID[menuActionRow] = 337;
					menuActionRow++;
				}
				l++;
			}
		}
	}
	
	private void buildDuelorTrade(int j) {
		int l = 0;
		for (int i1 = 0; i1 < 500; i1++) {
			if (chatMessages[i1] == null)
				continue;
			if (chatTypeView != 3 && chatTypeView != 4)
				continue;
			int j1 = chatTypes[i1];
			String s = chatNames[i1];
			int k1 = (70 - l * 14 + 42) + anInt1089 + 4 + 5;
			if (k1 < -23)
				break;
			if (s != null && s.StartsWith("@cr1@"))
				s = s.Substring(5);
			if (s != null && s.StartsWith("@cr2@"))
				s = s.Substring(5);
			if (s != null && s.StartsWith("@cr3@"))
				s = s.Substring(5);
			if (chatTypeView == 3 && j1 == 4
			    && (tradeMode == 0 || tradeMode == 1 && isFriendOrSelf(s))) {
				if (j > k1 - 14 && j <= k1) {
					menuActionName[menuActionRow] = "Accept trade @whi@" + s;
					menuActionID[menuActionRow] = 484;
					menuActionRow++;
				}
				l++;
			}
			if (chatTypeView == 4 && j1 == 8
			    && (tradeMode == 0 || tradeMode == 1 && isFriendOrSelf(s))) {
				if (j > k1 - 14 && j <= k1) {
					menuActionName[menuActionRow] = "Accept challenge @whi@"
						+ s;
					menuActionID[menuActionRow] = 6;
					menuActionRow++;
				}
				l++;
			}
			if (j1 == 12) {
				if (j > k1 - 14 && j <= k1) {
					menuActionName[menuActionRow] = "Go-to @blu@" + s;
					menuActionID[menuActionRow] = 915;
					menuActionRow++;
				}
				l++;
			}
		}
	}
	
	private void buildChatAreaMenu(int j) {
		int l = 0;
		int test = 0;
		for (int i1 = 0; i1 < 500; i1++) {
			if (chatMessages[i1] == null)
				continue;
			int j1 = chatTypes[i1];
			int k1 = (70 - l * 14 + 42) + anInt1089 + 4 + 5;
			if (k1 < -23)
				break;
			String s = chatNames[i1];
			String ct = chatMessages[i1];
			bool flag = false;
			if (chatTypeView == 1) {
				buildPublicChat(j);
				break;
			}
			if (chatTypeView == 2) {
				buildFriendChat(j);
				break;
			}
			if (chatTypeView == 3 || chatTypeView == 4) {
				buildDuelorTrade(j);
				break;
			}
			if (chatTypeView == 5) {
				break;
			}
			if (s != null && s.StartsWith("@cr1@")) {
				s = s.Substring(5);
				bool flag1 = true;
				byte byte0 = 1;
			}
			if (s != null && s.StartsWith("@cr2@")) {
				s = s.Substring(5);
				byte byte0 = 2;
			}
			if (s != null && s.StartsWith("@cr3@")) {
				s = s.Substring(5);
				byte byte0 = 3;
			}
			if (j1 == 0)
				l++;
			if ((j1 == 1 || j1 == 2)
			    && (j1 == 1 || publicChatMode == 0 || publicChatMode == 1
			    && isFriendOrSelf(s))) {
				if (j > k1 - 14 && j <= k1 && !s.Equals(myPlayer.name)) {
					if (myPrivilege >= 1) {
						menuActionName[menuActionRow] = "Report abuse @whi@"
							+ s;
						menuActionID[menuActionRow] = 606;
						menuActionRow++;
					}
					menuActionName[menuActionRow] = "Add ignore @whi@" + s;
					menuActionID[menuActionRow] = 42;
					menuActionRow++;
					menuActionName[menuActionRow] = "Add friend @whi@" + s;
					menuActionID[menuActionRow] = 337;
					menuActionRow++;
				}
				l++;
			}
			if ((j1 == 3 || j1 == 7)
			    && splitPrivateChat == 0
			    && (j1 == 7 || privateChatMode == 0 || privateChatMode == 1
			    && isFriendOrSelf(s))) {
				if (j > k1 - 14 && j <= k1) {
					if (myPrivilege >= 1) {
						menuActionName[menuActionRow] = "Report abuse @whi@"
							+ s;
						menuActionID[menuActionRow] = 606;
						menuActionRow++;
					}
					menuActionName[menuActionRow] = "Add ignore @whi@" + s;
					menuActionID[menuActionRow] = 42;
					menuActionRow++;
					menuActionName[menuActionRow] = "Add friend @whi@" + s;
					menuActionID[menuActionRow] = 337;
					menuActionRow++;
				}
				l++;
			}
			if (j1 == 4
			    && (tradeMode == 0 || tradeMode == 1 && isFriendOrSelf(s))) {
				if (j > k1 - 14 && j <= k1) {
					menuActionName[menuActionRow] = "Accept trade @whi@" + s;
					menuActionID[menuActionRow] = 484;
					menuActionRow++;
				}
				l++;
			}
			if ((j1 == 5 || j1 == 6) && splitPrivateChat == 0
			    && privateChatMode < 2)
				l++;
			if (j1 == 8
			    && (tradeMode == 0 || tradeMode == 1 && isFriendOrSelf(s))) {
				if (j > k1 - 14 && j <= k1) {
					menuActionName[menuActionRow] = "Accept challenge @whi@"
						+ s;
					menuActionID[menuActionRow] = 6;
					menuActionRow++;
				}
				l++;
			}
		}
	}
	
	private void drawFriendsListOrWelcomeScreen(RSInterface class9) {
		int j = class9.anInt214;
		if (j >= 1 && j <= 100 || j >= 701 && j <= 800) {
			if (j == 1 && anInt900 == 0) {
				class9.message = "Loading friend list";
				class9.atActionType = 0;
				return;
			}
			if (j == 1 && anInt900 == 1) {
				class9.message = "Connecting to friendserver";
				class9.atActionType = 0;
				return;
			}
			if (j == 2 && anInt900 != 2) {
				class9.message = "Please wait...";
				class9.atActionType = 0;
				return;
			}
			int k = friendsCount;
			if (anInt900 != 2)
				k = 0;
			if (j > 700)
				j -= 601;
			else
				j--;
			if (j >= k) {
				class9.message = "";
				class9.atActionType = 0;
				return;
			} else {
				class9.message = friendsList[j];
				class9.atActionType = 1;
				return;
			}
		}
		if (j >= 101 && j <= 200 || j >= 801 && j <= 900) {
			int l = friendsCount;
			if (anInt900 != 2)
				l = 0;
			if (j > 800)
				j -= 701;
			else
				j -= 101;
			if (j >= l) {
				class9.message = "";
				class9.atActionType = 0;
				return;
			}
			if (friendsNodeIDs[j] == 0)
				class9.message = "@red@Offline";
			else if (friendsNodeIDs[j] == nodeID)
				class9.message = "@gre@Online"/* + (friendsNodeIDs[j] - 9) */;
			else
				class9.message = "@red@Offline"/* + (friendsNodeIDs[j] - 9) */;
			class9.atActionType = 1;
			return;
		}
		if (j == 203) {
			int i1 = friendsCount;
			if (anInt900 != 2)
				i1 = 0;
			class9.scrollMax = i1 * 15 + 20;
			if (class9.scrollMax <= class9.height)
				class9.scrollMax = class9.height + 1;
			return;
		}
		if (j >= 401 && j <= 500) {
			if ((j -= 401) == 0 && anInt900 == 0) {
				class9.message = "Loading ignore list";
				class9.atActionType = 0;
				return;
			}
			if (j == 1 && anInt900 == 0) {
				class9.message = "Please wait...";
				class9.atActionType = 0;
				return;
			}
			int j1 = ignoreCount;
			if (anInt900 == 0)
				j1 = 0;
			if (j >= j1) {
				class9.message = "";
				class9.atActionType = 0;
				return;
			} else {
				class9.message = TextClass.fixName(TextClass
				                                   .nameForLong(ignoreListAsLongs[j]));
				class9.atActionType = 1;
				return;
			}
		}
		if (j == 503) {
			class9.scrollMax = ignoreCount * 15 + 20;
			if (class9.scrollMax <= class9.height)
				class9.scrollMax = class9.height + 1;
			return;
		}
		if (j == 327) {
			class9.anInt270 = 150;
			class9.anInt271 = (int) (Mathf.Sin(loopCycle / 40f) * 256f) & 0x7ff;
			if (aBoolean1031) {
				for (int k1 = 0; k1 < 7; k1++) {
					int l1 = anIntArray1065[k1];
					if (l1 >= 0 && !IDK.cache[l1].method537())
						return;
				}
				
				aBoolean1031 = false;
				Model[] aclass30_sub2_sub4_sub6s = new Model[7];
				int i2 = 0;
				for (int j2 = 0; j2 < 7; j2++) {
					int k2 = anIntArray1065[j2];
					if (k2 >= 0)
						aclass30_sub2_sub4_sub6s[i2++] = IDK.cache[k2]
						.method538();
				}
				
				Model model = new Model(i2, aclass30_sub2_sub4_sub6s);
				for (int l2 = 0; l2 < 5; l2++)
				if (anIntArray990[l2] != 0) {
					model.method476(anIntArrayArray1003[l2][0],
					                anIntArrayArray1003[l2][anIntArray990[l2]]);
					if (l2 == 1)
						model.method476(anIntArray1204[0],
						                anIntArray1204[anIntArray990[l2]]);
				}
				
				model.method469();
				model.method470(RS2Sharp.Animation.anims[myPlayer.anInt1511].anIntArray353[0]);
				model.method479(64, 850, -30, -50, -30, true);
				class9.anInt233 = 5;
				class9.mediaID = 0;
				RSInterface.method208(aBoolean994, model);
			}
			return;
		}
		if (j == 328) {
			RSInterface rsInterface = class9;
			int verticleTilt = 150;
			int animationSpeed = (int) (Mathf.Sin(loopCycle / 40f) * 256f) & 0x7ff;
			rsInterface.anInt270 = verticleTilt;
			rsInterface.anInt271 = animationSpeed;
			if (aBoolean1031) {
				Model characterDisplay = myPlayer.method452();
				for (int l2 = 0; l2 < 5; l2++)
				if (anIntArray990[l2] != 0) {
					characterDisplay.method476(anIntArrayArray1003[l2][0],
					                           anIntArrayArray1003[l2][anIntArray990[l2]]);
					if (l2 == 1)
						characterDisplay.method476(anIntArray1204[0],
						                           anIntArray1204[anIntArray990[l2]]);
				}
				int staticFrame = myPlayer.anInt1511;
				characterDisplay.method469();
				characterDisplay
					.method470(RS2Sharp.Animation.anims[staticFrame].anIntArray353[0]);
				// characterDisplay.method479(64, 850, -30, -50, -30, true);
				rsInterface.anInt233 = 5;
				rsInterface.mediaID = 0;
				RSInterface.method208(aBoolean994, characterDisplay);
			}
			return;
		}
		if (j == 324) {
			if (aClass30_Sub2_Sub1_Sub1_931 == null) {
				aClass30_Sub2_Sub1_Sub1_931 = class9.sprite1;
				aClass30_Sub2_Sub1_Sub1_932 = class9.sprite2;
			}
			if (aBoolean1047) {
				class9.sprite1 = aClass30_Sub2_Sub1_Sub1_932;
				return;
			} else {
				class9.sprite1 = aClass30_Sub2_Sub1_Sub1_931;
				return;
			}
		}
		if (j == 325) {
			if (aClass30_Sub2_Sub1_Sub1_931 == null) {
				aClass30_Sub2_Sub1_Sub1_931 = class9.sprite1;
				aClass30_Sub2_Sub1_Sub1_932 = class9.sprite2;
			}
			if (aBoolean1047) {
				class9.sprite1 = aClass30_Sub2_Sub1_Sub1_931;
				return;
			} else {
				class9.sprite1 = aClass30_Sub2_Sub1_Sub1_932;
				return;
			}
		}
		if (j == 600) {
			class9.message = reportAbuseInput;
			if (loopCycle % 20 < 10) {
				class9.message += "|";
				return;
			} else {
				class9.message += " ";
				return;
			}
		}
		if (j == 613)
		if (myPrivilege >= 1) {
			if (canMute) {
				class9.textColor = 0xff0000;
				class9.message = "Moderator option: Mute player for 48 hours: <ON>";
			} else {
				class9.textColor = 0xffffff;
				class9.message = "Moderator option: Mute player for 48 hours: <OFF>";
			}
		} else {
			class9.message = "";
		}
		if (j == 650 || j == 655)
		if (anInt1193 != 0) {
			String s;
			if (daysSinceLastLogin == 0)
				s = "earlier today";
			else if (daysSinceLastLogin == 1)
				s = "yesterday";
			else
				s = daysSinceLastLogin + " days ago";
			class9.message = "You last logged in " + s + " from: "
				+ signlink.dns;
		} else {
			class9.message = "";
		}
		if (j == 651) {
			if (unreadMessages == 0) {
				class9.message = "0 unread messages";
				class9.textColor = 0xffff00;
			}
			if (unreadMessages == 1) {
				class9.message = "1 unread message";
				class9.textColor = 65280;
			}
			if (unreadMessages > 1) {
				class9.message = unreadMessages + " unread messages";
				class9.textColor = 65280;
			}
		}
		if (j == 652)
		if (daysSinceRecovChange == 201) {
			if (membersInt == 1)
				class9.message = "@yel@This is a non-members world: @whi@Since you are a member we";
			else
				class9.message = "";
		} else if (daysSinceRecovChange == 200) {
			class9.message = "You have not yet set any password recovery questions.";
		} else {
			String s1;
			if (daysSinceRecovChange == 0)
				s1 = "Earlier today";
			else if (daysSinceRecovChange == 1)
				s1 = "Yesterday";
			else
				s1 = daysSinceRecovChange + " days ago";
			class9.message = s1 + " you changed your recovery questions";
		}
		if (j == 653)
		if (daysSinceRecovChange == 201) {
			if (membersInt == 1)
				class9.message = "@whi@recommend you use a members world instead. You may use";
			else
				class9.message = "";
		} else if (daysSinceRecovChange == 200)
			class9.message = "We strongly recommend you do so now to secure your account.";
		else
			class9.message = "If you do not remember making this change then cancel it immediately";
		if (j == 654) {
			if (daysSinceRecovChange == 201)
			if (membersInt == 1) {
				class9.message = "@whi@this world but member benefits are unavailable whilst here.";
				return;
			} else {
				class9.message = "";
				return;
			}
			if (daysSinceRecovChange == 200) {
				class9.message = "Do this from the 'account management' area on our front webpage";
				return;
			}
			class9.message = "Do this from the 'account management' area on our front webpage";
		}
	}
	
	private void drawSplitPrivateChat() {
		if (splitPrivateChat == 0)
			return;
		TextDrawingArea textDrawingArea = aTextDrawingArea_1271;
		int i = 0;
		if (anInt1104 != 0)
			i = 1;
		for (int j = 0; j < 100; j++)
		if (chatMessages[j] != null) {
			int k = chatTypes[j];
			String s = chatNames[j];
			byte byte1 = 0;
			if (s != null && s.StartsWith("@cr1@")) {
				s = s.Substring(5);
				byte1 = 1;
			}
			if (s != null && s.StartsWith("@cr2@")) {
				s = s.Substring(5);
				byte1 = 2;
			}
			if ((k == 3 || k == 7)
			    && (k == 7 || privateChatMode == 0 || privateChatMode == 1
			    && isFriendOrSelf(s))) {
				int l = 329 - i * 13;
				int k1 = 4;
				textDrawingArea.method385(0, "From", l, k1);
				textDrawingArea.method385(65535, "From", l - 1, k1);
				k1 += textDrawingArea.getTextWidth("From ");
				if (byte1 == 1) {
					modIcons[0].drawSprite(k1, l - 12);
					k1 += 12;
				}
				if (byte1 == 2) {
					modIcons[1].drawSprite(k1, l - 12);
					k1 += 12;
				}
				textDrawingArea.method385(0, s + ": " + chatMessages[j], l,
				                          k1);
				textDrawingArea.method385(65535,
				                          s + ": " + chatMessages[j], l - 1, k1);
				if (++i >= 5)
					return;
			}
			if (k == 5 && privateChatMode < 2) {
				int i1 = 329 - i * 13;
				textDrawingArea.method385(0, chatMessages[j], i1, 4);
				textDrawingArea
					.method385(65535, chatMessages[j], i1 - 1, 4);
				if (++i >= 5)
					return;
			}
			if (k == 6 && privateChatMode < 2) {
				int j1 = 329 - i * 13;
				textDrawingArea.method385(0, "To " + s + ": "
				                          + chatMessages[j], j1, 4);
				textDrawingArea.method385(65535, "To " + s + ": "
				                          + chatMessages[j], j1 - 1, 4);
				if (++i >= 5)
					return;
			}
		}
		
	}
	
	public void pushMessage(String s, int i, String s1) {
		if (i == 0 && dialogID != -1) {
			aString844 = s;
			base.clickMode3 = 0;
		}
		if (backDialogID == -1)
			inputTaken = true;
		for (int j = 499; j > 0; j--) {
			chatTypes[j] = chatTypes[j - 1];
			chatNames[j] = chatNames[j - 1];
			chatMessages[j] = chatMessages[j - 1];
			chatRights[j] = chatRights[j - 1];
		}
		chatTypes[0] = i;
		chatNames[0] = s1;
		chatMessages[0] = s;
		chatRights[0] = rights;
	}
	
	private void processTabClick() {
// 		if (base.clickMode3 == 1) {
// 			if (base.saveClickX >= 524 && base.saveClickX <= 561
// 			    && base.saveClickY >= 169 && base.saveClickY < 205
// 			    && tabInterfaceIDs[0] != -1) {
// 				needDrawTabArea = true;
// 				tabID = 0;
// 				tabAreaAltered = true;
// 			}
// 			if (base.saveClickX >= 562 && base.saveClickX <= 594
// 			    && base.saveClickY >= 168 && base.saveClickY < 205
// 			    && tabInterfaceIDs[1] != -1) {
// 				needDrawTabArea = true;
// 				tabID = 1;
// 				tabAreaAltered = true;
// 			}
// 			if (base.saveClickX >= 595 && base.saveClickX <= 626
// 			    && base.saveClickY >= 168 && base.saveClickY < 205
// 			    && tabInterfaceIDs[2] != -1) {
// 				needDrawTabArea = true;
// 				tabID = 2;
// 				tabAreaAltered = true;
// 			}
// 			if (base.saveClickX >= 627 && base.saveClickX <= 660
// 			    && base.saveClickY >= 168 && base.saveClickY < 203
// 			    && tabInterfaceIDs[3] != -1) {
// 				needDrawTabArea = true;
// 				tabID = 3;
// 				tabAreaAltered = true;
// 			}
// 			if (base.saveClickX >= 661 && base.saveClickX <= 693
// 			    && base.saveClickY >= 168 && base.saveClickY < 205
// 			    && tabInterfaceIDs[4] != -1) {
// 				needDrawTabArea = true;
// 				tabID = 4;
// 				tabAreaAltered = true;
// 			}
// 			if (base.saveClickX >= 694 && base.saveClickX <= 725
// 			    && base.saveClickY >= 168 && base.saveClickY < 205
// 			    && tabInterfaceIDs[5] != -1) {
// 				needDrawTabArea = true;
// 				tabID = 5;
// 				tabAreaAltered = true;
// 			}
// 			if (base.saveClickX >= 726 && base.saveClickX <= 765
// 			    && base.saveClickY >= 169 && base.saveClickY < 205
// 			    && tabInterfaceIDs[6] != -1) {
// 				needDrawTabArea = true;
// 				tabID = 6;
// 				tabAreaAltered = true;
// 			}
// 			if (base.saveClickX >= 524 && base.saveClickX <= 561
// 			    && base.saveClickY >= 466 && base.saveClickY < 503
// 			    && tabInterfaceIDs[7] != -1) {
// 				needDrawTabArea = true;
// 				tabID = 7;
// 				tabAreaAltered = true;
// 			}
// 			if (base.saveClickX >= 562 && base.saveClickX <= 594
// 			    && base.saveClickY >= 466 && base.saveClickY < 503
// 			    && tabInterfaceIDs[8] != -1) {
// 				needDrawTabArea = true;
// 				tabID = 8;
// 				tabAreaAltered = true;
// 			}
// 			if (base.saveClickX >= 595 && base.saveClickX <= 627
// 			    && base.saveClickY >= 466 && base.saveClickY < 503
// 			    && tabInterfaceIDs[9] != -1) {
// 				needDrawTabArea = true;
// 				tabID = 9;
// 				tabAreaAltered = true;
// 			}
// 			if (base.saveClickX >= 627 && base.saveClickX <= 664
// 			    && base.saveClickY >= 466 && base.saveClickY < 503
// 			    && tabInterfaceIDs[13] != -1) {
// 				needDrawTabArea = true;
// 				tabID = 13;
// 				tabAreaAltered = true;
// 			}
// 			if (base.saveClickX >= 661 && base.saveClickX <= 694
// 			    && base.saveClickY >= 466 && base.saveClickY < 503
// 			    && tabInterfaceIDs[11] != -1) {
// 				needDrawTabArea = true;
// 				tabID = 11;
// 				tabAreaAltered = true;
// 			}
// 			if (base.saveClickX >= 695 && base.saveClickX <= 725
// 			    && base.saveClickY >= 466 && base.saveClickY < 503
// 			    && tabInterfaceIDs[12] != -1) {
// 				needDrawTabArea = true;
// 				tabID = 12;
// 				tabAreaAltered = true;
// 			}
// 			if (base.saveClickX >= 726 && base.saveClickX <= 765
// 			    && base.saveClickY >= 466 && base.saveClickY < 502
// 			    && tabInterfaceIDs[13] != -1) {
// 				needDrawTabArea = true;
// 				tabID = 13;
// 				tabAreaAltered = true;
// 			} else if (base.saveClickX >= 745 && base.saveClickX <= 765
// 			           && base.saveClickY >= 0 && base.saveClickY < 21
// 			           && tabInterfaceIDs[10] != -1) {
// 				needDrawTabArea = true;
// 				tabID = 10;
// 				tabAreaAltered = true;
// 			}
// 		}
//minimapHovers();
		int yOffset = myHeight - 503;
		int xOffset = myWidth - 765;
		if (base.clickMode3 == 1) {
			for (int i = 0; i < tabClickX.Length; i++) {
				if (base.mouseX >= tabClickStart[i] + xOffset && base.mouseX <= tabClickStart[i] + tabClickX[i] + xOffset
						&& base.mouseY >= tabClickY[i] + yOffset && base.mouseY < tabClickY[i] + 37 + yOffset && tabInterfaceIDs[i] != -1) {
					tabID = i;
					tabAreaAltered = true;
					break;
				}
			}
		}
		
	}
	
	 private  int[] tabClickX = { 38, 33, 33, 33, 33, 33, 38, 38, 33, 33,
                        33, 33, 33, 38 }, tabClickStart = { 522, 560, 593, 625, 659, 692,
                        724, 522, 560, 593, 625, 659, 692, 724 }, tabClickY = { 169, 169,
                        169, 169, 169, 169, 169, 466, 466, 466, 466, 466, 466, 466 };
	public override void setBounds() {
		RS2Sharp.Texture.method365(myWidth, myHeight);
		fullScreenTextureArray = RS2Sharp.Texture.lineOffsets;
		RS2Sharp.Texture.method365(myWidth, myHeight);
		anIntArray1180 = RS2Sharp.Texture.lineOffsets;
		RS2Sharp.Texture.method365(myWidth, myHeight);
		anIntArray1181 = RS2Sharp.Texture.lineOffsets;
		RS2Sharp.Texture.method365(myWidth, myHeight);
		anIntArray1182 = RS2Sharp.Texture.lineOffsets;
		int[] ai = new int[9];
		for(int i8 = 0; i8 < 9; i8++) {
			int k8 = 128 + i8 * 32 + 15;
			int l8 = 600 + k8 * 3;
			int i9 = RS2Sharp.Texture.SINE[k8];
			ai[i8] = l8 * i9 >> 16;
		}
		//WorldController.method310(500, 800, myWidth, myHeight, ai);
		aRSImageProducer_1165 = new RSImageProducer(myWidth, myHeight);//gamescreen
	}
	
	private void resetImageProducers2() {
		if (aRSImageProducer_1166 != null)
			return;
		nullLoader();
		base.fullGameScreen = null;
		aRSImageProducer_1107 = null;
		aRSImageProducer_1108 = null;
		aRSImageProducer_1109 = null;
		aRSImageProducer_1110 = null;
		aRSImageProducer_1111 = null;
		aRSImageProducer_1112 = null;
		aRSImageProducer_1113 = null;
		aRSImageProducer_1114 = null;
		aRSImageProducer_1115 = null;
		aRSImageProducer_1166 = new RSImageProducer(519, 165);
		aRSImageProducer_1164 = new RSImageProducer(246, 168);
		DrawingArea.setAllPixelsToZero();
		mapArea.drawSprite(0, 0);
		aRSImageProducer_1163 = new RSImageProducer(246, 335);
		aRSImageProducer_1165 = new RSImageProducer(myWidth, myHeight);
		DrawingArea.setAllPixelsToZero();
		aRSImageProducer_1123 = new RSImageProducer(496, 50);
		aRSImageProducer_1124 = new RSImageProducer(269, 37);
		aRSImageProducer_1125 = new RSImageProducer(249, 45);
		welcomeScreenRaised = true;
	}
	
	public String getDocumentBaseHost() {
// 		if (signlink.mainapp != null) {
// 			return signlink.mainapp.getDocumentBase().getHost().toLowerCase();
// 		}
// 		if (base.gameFrame != null) {
// 			return ""; // runescape.com <- removed for Jframe to work
// 		} else {
// 			return ""; // base.getDocumentBase().getHost().toLowerCase() <-
// 			// removed for Jframe to work
// 		}
return "";
	}
	
	private void method81(RS2Sharp.Sprite sprite, int j, int k)
	{
		int l = k * k + j * j;
		if(l > 4225 && l < 0x15f90)
		{
			int i1 = minimapInt1 + minimapInt2 & 0x7ff;
			int j1 = Model.SINE[i1];
			int k1 = Model.COSINE[i1];
			j1 = (j1 * 256) / (minimapInt3 + 256);
			k1 = (k1 * 256) / (minimapInt3 + 256);
			int l1 = j * j1 + k * k1 >> 16;
			int i2 = j * k1 - k * j1 >> 16;
			double d = Mathf.Atan2(l1, i2);
			int j2 = (int)(Mathf.Sin((float)d) * 63f);
			int k2 = (int)(Mathf.Cos((float)d) * 57f);
			mapEdge.method353(83 - k2 - 20, d, (94 + j2 + 4) - 10);
		} else
		{
			markMinimap(sprite, k, j);
		}
	}
	
	private void rightClickChatButtons() {
// 		if (base.mouseX >= 5 && base.mouseX <= 61 && base.mouseY >= 482
// 		    && base.mouseY <= 503) {
// 			menuActionName[1] = "View All";
// 			menuActionID[1] = 999;
// 			menuActionRow = 2;
// 		} else if (base.mouseX >= 62 && base.mouseX <= 117
// 		           && base.mouseY >= 482 && base.mouseY <= 503) {
// 			menuActionName[1] = "View Game";
// 			menuActionID[1] = 998;
// 			menuActionRow = 2;
// 		} else if (base.mouseX >= 119 && base.mouseX <= 174
// 		           && base.mouseY >= 482 && base.mouseY <= 503) {
// 			menuActionName[1] = "Hide public";
// 			menuActionID[1] = 997;
// 			menuActionName[2] = "Off public";
// 			menuActionID[2] = 996;
// 			menuActionName[3] = "Friends public";
// 			menuActionID[3] = 995;
// 			menuActionName[4] = "On public";
// 			menuActionID[4] = 994;
// 			menuActionName[5] = "View public";
// 			menuActionID[5] = 993;
// 			menuActionRow = 6;
// 		} else if (base.mouseX >= 176 && base.mouseX <= 231
// 		           && base.mouseY >= 482 && base.mouseY <= 503) {
// 			menuActionName[1] = "Off private";
// 			menuActionID[1] = 992;
// 			menuActionName[2] = "Friends private";
// 			menuActionID[2] = 991;
// 			menuActionName[3] = "On private";
// 			menuActionID[3] = 990;
// 			menuActionName[4] = "View private";
// 			menuActionID[4] = 989;
// 			menuActionRow = 5;
// 		} else if (base.mouseX >= 233 && base.mouseX <= 288
// 		           && base.mouseY >= 482 && base.mouseY <= 503) {
// 			menuActionName[1] = "Off clan chat";
// 			menuActionID[1] = 1003;
// 			menuActionName[2] = "Friends clan chat";
// 			menuActionID[2] = 1002;
// 			menuActionName[3] = "On clan chat";
// 			menuActionID[3] = 1001;
// 			menuActionName[4] = "View clan chat";
// 			menuActionID[4] = 1000;
// 			menuActionRow = 5;
// 		} else if (base.mouseX >= 290 && base.mouseX <= 345
// 		           && base.mouseY >= 482 && base.mouseY <= 503) {
// 			menuActionName[1] = "Off trade";
// 			menuActionID[1] = 987;
// 			menuActionName[2] = "Friends trade";
// 			menuActionID[2] = 986;
// 			menuActionName[3] = "On trade";
// 			menuActionID[3] = 985;
// 			menuActionName[4] = "View trade";
// 			menuActionID[4] = 984;
// 			menuActionRow = 5;
// 		} else if (base.mouseX >= 347 && base.mouseX <= 402
// 		           && base.mouseY >= 482 && base.mouseY <= 503) {
// 			menuActionName[1] = "Off duel";
// 			menuActionID[1] = -1;
// 			menuActionName[2] = "Friends duel";
// 			menuActionID[2] = -1;
// 			menuActionName[3] = "On duel";
// 			menuActionID[3] = -1;
// 			menuActionName[4] = "View duel";
// 			menuActionID[4] = -1;
// 			menuActionRow = 5;
// 		} else if (base.mouseX >= 404 && base.mouseX <= 514
// 		           && base.mouseY >= 480 && base.mouseY <= 501) {
// 			menuActionName[1] = "Report Abuse";
// 			menuActionID[1] = 606;
// 			menuActionRow = 2;
// 		}
if (mouseY >= myHeight - 22 && mouseY <= myHeight) {
			if (base.mouseX >= 6 && base.mouseX <= 62) {
				menuActionName[1] = "View All";
				menuActionID[1] = 999;
				menuActionRow = 2;
			} else if (base.mouseX >= 63 && base.mouseX <= 118) {
				menuActionName[1] = "View Game";
				menuActionID[1] = 998;
				menuActionRow = 2;
			} else if (base.mouseX >= 119 && base.mouseX <= 175) {
				menuActionName[1] = "Hide public";
				menuActionID[1] = 997;
				menuActionName[2] = "Off public";
				menuActionID[2] = 996;
				menuActionName[3] = "Friends public";
				menuActionID[3] = 995;
				menuActionName[4] = "On public";
				menuActionID[4] = 994;
				menuActionName[5] = "View public";
				menuActionID[5] = 993;
				menuActionRow = 6;
			} else if (base.mouseX >= 177 && base.mouseX <= 232) {
				menuActionName[1] = "Off public";
				menuActionID[1] = 992;
				menuActionName[2] = "Friends public";
				menuActionID[2] = 991;
				menuActionName[3] = "On public";
				menuActionID[3] = 990;
				menuActionName[4] = "View public";
				menuActionID[4] = 989;
				menuActionRow = 5;
			} else if (base.mouseX >= 233 && base.mouseX <= 289) {
				menuActionName[1] = "Off clan chat";
				menuActionID[1] = 1003;
				menuActionName[2] = "Friends clan chat";
				menuActionID[2] = 1002;
				menuActionName[3] = "On clan chat";
				menuActionID[3] = 1001;
				menuActionName[4] = "View clan chat";
				menuActionID[4] = 1000;
				menuActionRow = 5;
			} else if (base.mouseX >= 290 && base.mouseX <= 346) {
				menuActionName[1] = "Off trade";
				menuActionID[1] = 987;
				menuActionName[2] = "Friends trade";
				menuActionID[2] = 986;
				menuActionName[3] = "On trade";
				menuActionID[3] = 985;
				menuActionName[4] = "View trade";
				menuActionID[4] = 984;
				menuActionRow = 5;
			} else if (base.mouseX >= 347 && base.mouseX <= 403) {
				menuActionName[1] = "Off duel";
				menuActionID[1] = 983;
				menuActionName[2] = "Friends duel";
				menuActionID[2] = 982;
				menuActionName[3] = "On duel";
				menuActionID[3] = 981;
				menuActionName[4] = "View duel";
				menuActionID[4] = 980;
				menuActionRow = 5;
			}
		}
	}
	
	public bool validMousePosition() {

			if (base.mouseX > 0 && base.mouseY > 0 && base.mouseX < myWidth && base.mouseY < myHeight) {
				if (base.mouseX <= 519 && base.mouseY >= myHeight - 165) {
					return false;
				}
				if (base.mouseX >= myWidth - 249 && base.mouseY >= myHeight - 335) {
					return false;
				}
				return true;
			}
		return false;
	}
	
	public void processRightClick() {
		if (activeInterfaceType != 0) {
			return;
		}
		menuActionName[0] = "Cancel";
		menuActionID[0] = 1107;
		menuActionRow = 1;
		if (base.mouseX >= 515 && base.mouseY >= 0 && base.mouseX <= 765 && base.mouseY <= 167)
			if (openInterfaceID != -1)
				buildInterfaceMenu(0, RSInterface.interfaceCache[openInterfaceID], base.mouseX, 0, base.mouseY, 0);
		if (anInt886 != anInt1026)
			anInt1026 = anInt886;
		anInt886 = 0;
		if (fullscreenInterfaceID != -1) {
			anInt886 = 0;
			anInt1315 = 0;
			buildInterfaceMenu(8,
			                   RSInterface.interfaceCache[fullscreenInterfaceID],
			                   base.mouseX, 8, base.mouseY, 0);
			if (anInt886 != anInt1026) {
				anInt1026 = anInt886;
			}
			if (anInt1315 != anInt1129) {
				anInt1129 = anInt1315;
			}
			return;
		}
		buildSplitPrivateChatMenu();
		anInt886 = 0;
		anInt1315 = 0;
// 		if (base.mouseX > 0 && base.mouseY > 0 && base.mouseX < 516
// 		    && base.mouseY < 338) {
// 			if (openInterfaceID != -1) {
// 				buildInterfaceMenu(4,
// 				                   RSInterface.interfaceCache[openInterfaceID],
// 				                   base.mouseX, 4, base.mouseY, 0);
// 			} else {
// 				build3dScreenMenu();
// 			}
// 		}
if (validMousePosition()) {
			if (openInterfaceID != -1) {
				buildInterfaceMenu( 0, RSInterface.interfaceCache[openInterfaceID], base.mouseX,  0, base.mouseY, 0);
			} else {
				build3dScreenMenu();
			}
		}
		if (anInt886 != anInt1026) {
			anInt1026 = anInt886;
		}
		if (anInt1315 != anInt1129) {
			anInt1129 = anInt1315;
		}
		anInt886 = 0;
		anInt1315 = 0;
// 		if (base.mouseX > 548 && base.mouseY > 207 && base.mouseX < 740
// 		    && base.mouseY < 468) {
// 			if (invOverlayInterfaceID != -1) {
// 				buildInterfaceMenu(548,
// 				                   RSInterface.interfaceCache[invOverlayInterfaceID],
// 				                   base.mouseX, 207, base.mouseY, 0);
// 			} else if (tabInterfaceIDs[tabID] != -1) {
// 				buildInterfaceMenu(548,
// 				                   RSInterface.interfaceCache[tabInterfaceIDs[tabID]],
// 				                   base.mouseX, 207, base.mouseY, 0);
// 			}
// 		}
int yOffset = myHeight - 503;
		int xOffset = myWidth - 765;
		if(base.mouseX > 548 + xOffset && base.mouseX < 740 + xOffset && base.mouseY > 207 + yOffset && base.mouseY < 468 + yOffset) {
			if(invOverlayInterfaceID != -1) {
				buildInterfaceMenu(548 + xOffset, RSInterface.interfaceCache[invOverlayInterfaceID], base.mouseX, 207 + yOffset, base.mouseY, 0);
			} else if(tabInterfaceIDs[tabID] != -1) {
				buildInterfaceMenu(548 + xOffset, RSInterface.interfaceCache[tabInterfaceIDs[tabID]], base.mouseX, 207 + yOffset, base.mouseY, 0);
			}
		}
		if (anInt886 != anInt1048) {
			needDrawTabArea = true;
			tabAreaAltered = true;
			anInt1048 = anInt886;
		}
		if (anInt1315 != anInt1044) {
			needDrawTabArea = true;
			tabAreaAltered = true;
			anInt1044 = anInt1315;
		}
		anInt886 = 0;
		anInt1315 = 0;
		if (base.mouseX > 0 && base.mouseY > 338 && base.mouseX < 490
		    && base.mouseY < 463) {
			if (backDialogID != -1) {
				buildInterfaceMenu(20,
				                   RSInterface.interfaceCache[backDialogID], base.mouseX,
				                   358, base.mouseY, 0);
			} else if (base.mouseY < 463 && base.mouseX < 490) {
				buildChatAreaMenu(base.mouseY - 338);
			}
		}
		if (backDialogID != -1 && anInt886 != anInt1039) {
			inputTaken = true;
			anInt1039 = anInt886;
		}
		if (backDialogID != -1 && anInt1315 != anInt1500) {
			inputTaken = true;
			anInt1500 = anInt1315;
		}
		/* Enable custom right click areas */
// 		if (base.mouseX > 4 && base.mouseY > 480 && base.mouseX < 516
// 		    && base.mouseY < 503)
// 			rightClickChatButtons();
if(base.mouseX > 5 && base.mouseY > myHeight - 22 && base.mouseX < 516 && base.mouseY < myHeight) {
			rightClickChatButtons();
		}
		
		if(base.mouseX >= 704 && base.mouseX <= 760 && base.mouseY >= 88 && base.mouseY < 120) { 
			menuActionName[2] = runClicked ? "Turn run mode off" : "Turn run mode on";
			menuActionID[2] = 1050;
			menuActionRow = 2;
			menuActionName[1] = "Rest";
			menuActionID[1] = 1051;
			menuActionRow = 3;
		}	if (base.mouseX >= 526 && base.mouseX <= 561 && base.mouseY >= 7 && base.mouseY <= 41) {
			menuActionName[1] = "Face North";
			menuActionID[1] = 696;
			menuActionRow = 2;
		}
		if(base.mouseX >= 706 && base.mouseX <= 762 && base.mouseY >= 52 && base.mouseY < 87) {
			menuActionName[2] = clickedQuickPrayers ? "Turn quick prayers off" : "Turn quick prayers on";
			menuActionID[2] = 1500;
			menuActionRow = 2;
			menuActionName[1] = "Select quick prayers";
			menuActionID[1] = 1506;
			menuActionRow = 3;
		}
		if (base.mouseX >= 743 && base.mouseX <= 765 && base.mouseY >= 0
		    && base.mouseY <= 17) {
			menuActionName[1] = "Log Out";
			menuActionID[1] = 1051;
			menuActionRow = 2;
			logIconHPos = 1;
		} else {
			logIconHPos = 0;
		}
		/**/
		bool flag = false;
		while (!flag) {
			flag = true;
			for (int j = 0; j < menuActionRow - 1; j++) {
				if (menuActionID[j] < 1000 && menuActionID[j + 1] > 1000) {
					String s = menuActionName[j];
					menuActionName[j] = menuActionName[j + 1];
					menuActionName[j + 1] = s;
					int k = menuActionID[j];
					menuActionID[j] = menuActionID[j + 1];
					menuActionID[j + 1] = k;
					k = menuActionCmd2[j];
					menuActionCmd2[j] = menuActionCmd2[j + 1];
					menuActionCmd2[j + 1] = k;
					k = menuActionCmd3[j];
					menuActionCmd3[j] = menuActionCmd3[j + 1];
					menuActionCmd3[j + 1] = k;
					k = menuActionCmd1[j];
					menuActionCmd1[j] = menuActionCmd1[j + 1];
					menuActionCmd1[j + 1] = k;
					flag = false;
				}
			}
		}
	}
	
	
	private int method83(int i, int j, int k) {
// 		int l = 256 - k;
// 		return ((i & 0xff00ff) * l + (j & 0xff00ff) * k & 0xff00ff00)
// 			+ ((i & 0xff00) * l + (j & 0xff00) * k & 0xff0000) >> 8;
		int l = 256 - k;
			return unchecked((int)((i & 0xff00ff) * l + (j & 0xff00ff) * k & 0xff00ff00) + ((i & 0xff00) * l + (j & 0xff00) * k & 0xff0000) >> 8);
		
	}
	public void setNorth() {
		anInt1278  = 0;
		anInt1131  = 0;
		anInt896  = 0;
		minimapInt1  = 0;
		minimapInt2  = 0;
		minimapInt3  = 0;
	}
	private void login(String s, String s1, bool flag) {
	return;
		signlink.errorname = s;
		try {
			if (!flag) {
				loginMessage1 = "";
				loginMessage2 = "Connecting to server...";
				drawLoginScreen(true);
			}
			if(socketStream == null) socketStream = gameObject.GetComponent<RSSocket>();
			socketStream.Init(this, new System.Net.Sockets.TcpClient(serverIP, serverPort).Client);//socketStream = new RSSocket(this, openSocket(43594 + portOff));
			long l = TextClass.longForName(s);
			int i = (int) (l >> 16 & 31L);
			stream.currentOffset = 0;
			stream.writeWordBigEndian(14);
			stream.writeWordBigEndian(i);
			socketStream.queueBytes(2, stream.buffer);
			for (int j = 0; j < 8; j++)
				socketStream.read();
			
			int k = socketStream.read();
			int i1 = k;
			if (k == 0) {
				socketStream.flushInputStream(inStream.buffer, 8);
				inStream.currentOffset = 0;
				aLong1215 = inStream.readQWord();
				int[] ai = new int[4];
				ai[0] = (int) (RS2Sharp.Random.Next(99999999));
				ai[1] = (int) (RS2Sharp.Random.Next(99999999));
				ai[2] = (int) (aLong1215 >> 32);
				ai[3] = (int) aLong1215;
				stream.currentOffset = 0;
				stream.writeWordBigEndian(10);
				stream.writeDWord(ai[0]);
				stream.writeDWord(ai[1]);
				stream.writeDWord(ai[2]);
				stream.writeDWord(ai[3]);
				stream.writeDWord(999999);//signlink.uid);
				stream.writeString(s);
				stream.writeString(s1);
				stream.doKeys();
				aStream_847.currentOffset = 0;
				if (flag)
					aStream_847.writeWordBigEndian(18);
				else
					aStream_847.writeWordBigEndian(16);
				aStream_847.writeWordBigEndian(stream.currentOffset + 36 + 1
				                               + 1 + 2);
				aStream_847.writeWordBigEndian(255);
				aStream_847.writeWord(317);
				aStream_847.writeWordBigEndian(lowMem ? 1 : 0);
				for (int l1 = 0; l1 < 9; l1++)
					aStream_847.writeDWord(expectedCRCs[l1]);
				
				aStream_847.writeBytes(stream.buffer, stream.currentOffset, 0);
				stream.encryption = new ISAACRandomGen(ai);
				for (int j2 = 0; j2 < 4; j2++)
					ai[j2] += 50;
				
				encryption = new ISAACRandomGen(ai);
				socketStream.queueBytes(aStream_847.currentOffset,
				                        aStream_847.buffer);
				k = socketStream.read();
			}
			if (k == 1) {
				try {
					//Thread.Sleep(2000);
				} catch (Exception _ex) {
				}
				login(s, s1, flag);
				return;
			}
			if (k == 2) {
				myPrivilege = socketStream.read();
				flagged = socketStream.read() == 1;
				aLong1220 = 0L;
				anInt1022 = 0;
				mouseDetection.coordsIndex = 0;
				base.awtFocus = true;
				aBoolean954 = true;
				loggedIn = true;
				stream.currentOffset = 0;
				inStream.currentOffset = 0;
				pktType = -1;
				anInt841 = -1;
				anInt842 = -1;
				anInt843 = -1;
				pktSize = 0;
				anInt1009 = 0;
				anInt1104 = 0;
				anInt1011 = 0;
				anInt855 = 0;
				menuActionRow = 0;
				menuOpen = false;
				base.idleTime = 0;
				for (int j1 = 0; j1 < 100; j1++)
					chatMessages[j1] = null;
				
				itemSelected = 0;
				spellSelected = 0;
				loadingStage = 0;
				anInt1062 = 0;
				anInt1278 = (int) (RS2Sharp.Random.Next(100)) - 50;
				anInt1131 = (int) (RS2Sharp.Random.Next(110)) - 55;
				anInt896 = (int) (RS2Sharp.Random.Next(80)) - 40;
				minimapInt2= (int) (RS2Sharp.Random.Next(120)) - 60;
				minimapInt3 = (int) (RS2Sharp.Random.Next(30)) - 20;
				minimapInt1 = (int) (RS2Sharp.Random.Next(20)) - 10 & 0x7ff;
				
				anInt1021 = 0;
				anInt985 = -1;
				destX = 0;
				destY = 0;
				playerCount = 0;
				npcCount = 0;
				for (int i2 = 0; i2 < maxPlayers; i2++) {
					playerArray[i2] = null;
					aStreamArray895s[i2] = null;
				}
				
				for (int k2 = 0; k2 < 16384; k2++)
					npcArray[k2] = null;
				
				myPlayer = playerArray[myPlayerIndex] = new Player();
				myPlayer.playerObj.isMainPlayer = true;
				projectiles.removeAll();
				aClass19_1056.removeAll();
				for (int l2 = 0; l2 < 4; l2++) {
					for (int i3 = 0; i3 < 104; i3++) {
						for (int k3 = 0; k3 < 104; k3++)
							groundArray[l2][i3][k3] = null;
						
					}
					
				}
				
				aClass19_1179 = new NodeList();
				fullscreenInterfaceID = -1;
				anInt900 = 0;
				friendsCount = 0;
				dialogID = -1;
				backDialogID = -1;
				openInterfaceID = -1;
				invOverlayInterfaceID = -1;
				anInt1018 = -1;
				aBoolean1149 = false;
				tabID = 3;
				inputDialogState = 0;
				menuOpen = false;
				messagePromptRaised = false;
				aString844 = null;
				anInt1055 = 0;
				anInt1054 = -1;
				aBoolean1047 = true;
				method45();
				for (int j3 = 0; j3 < 5; j3++)
					anIntArray990[j3] = 0;
				
				for (int l3 = 0; l3 < 5; l3++) {
					atPlayerActions[l3] = null;
					atPlayerArray[l3] = false;
				}
				
				anInt1175 = 0;
				anInt1134 = 0;
				anInt986 = 0;
				anInt1288 = 0;
				anInt924 = 0;
				anInt1188 = 0;
				anInt1155 = 0;
				anInt1226 = 0;
				int anInt941 = 0;
				int anInt1260 = 0;
				resetImageProducers2();
				return;
			}
			if (k == 3) {
				loginMessage1 = "";
				loginMessage2 = "Invalid username or password.";
				return;
			}
			if (k == 4) {
				loginMessage1 = "Your account has been disabled.";
				loginMessage2 = "Please check your message-center for details.";
				return;
			}
			if (k == 5) {
				loginMessage1 = "Your account is already logged in.";
				loginMessage2 = "Try again in 60 secs...";
				return;
			}
			if (k == 6) {
				loginMessage1 = "RuneScape has been updated!";
				loginMessage2 = "Please reload this page.";
				return;
			}
			if (k == 7) {
				loginMessage1 = "This world is full.";
				loginMessage2 = "Please use a different world.";
				return;
			}
			if (k == 8) {
				loginMessage1 = "Unable to connect.";
				loginMessage2 = "Login server offline.";
				return;
			}
			if (k == 9) {
				loginMessage1 = "Login limit exceeded.";
				loginMessage2 = "Too many connections from your address.";
				return;
			}
			if (k == 10) {
				loginMessage1 = "Unable to connect.";
				loginMessage2 = "Bad session id.";
				return;
			}
			if (k == 11) {
				loginMessage2 = "Login server rejected session.";
				loginMessage2 = "Please try again.";
				return;
			}
			if (k == 12) {
				loginMessage1 = "You need a members account to login to this world.";
				loginMessage2 = "Please subscribe, or use a different world.";
				return;
			}
			if (k == 13) {
				loginMessage1 = "Could not complete login.";
				loginMessage2 = "Please try using a different world.";
				return;
			}
			if (k == 14) {
				loginMessage1 = "The server is being updated.";
				loginMessage2 = "Please wait 1 minute and try again.";
				return;
			}
			if (k == 15) {
				loggedIn = true;
				stream.currentOffset = 0;
				inStream.currentOffset = 0;
				pktType = -1;
				anInt841 = -1;
				anInt842 = -1;
				anInt843 = -1;
				pktSize = 0;
				anInt1009 = 0;
				anInt1104 = 0;
				menuActionRow = 0;
				menuOpen = false;
				aLong824 = NetDrawingTools.CurrentTimeMillis();
				return;
			}
			if (k == 16) {
				loginMessage1 = "Login attempts exceeded.";
				loginMessage2 = "Please wait 1 minute and try again.";
				return;
			}
			if (k == 17) {
				loginMessage1 = "You are standing in a members-only area.";
				loginMessage2 = "To play on this world move to a free area first";
				return;
			}
			if (k == 20) {
				loginMessage1 = "Invalid loginserver requested";
				loginMessage2 = "Please try using a different world.";
				return;
			}
			if (k == 21) {
				for (int k1 = socketStream.read(); k1 >= 0; k1--) {
					loginMessage1 = "You have only just left another world";
					loginMessage2 = "Your profile will be transferred in: "
						+ k1 + " seconds";
					drawLoginScreen(true);
					try {
						//Thread.Sleep(1000);
					} catch (Exception _ex) {
					}
				}
				
				login(s, s1, flag);
				return;
			}
			if (k == -1) {
				if (i1 == 0) {
					if (loginFailures < 2) {
						try {
							//Thread.Sleep(2000);
						} catch (Exception _ex) {
						}
						loginFailures++;
						login(s, s1, flag);
						return;
					} else {
						loginMessage1 = "No response from loginserver";
						loginMessage2 = "Please wait 1 minute and try again.";
						return;
					}
				} else {
					loginMessage1 = "No response from server";
					loginMessage2 = "Please try using a different world.";
					return;
				}
			} else {
				UnityEngine.Debug.Log("response:" + k);
				loginMessage1 = "Unexpected server response";
				loginMessage2 = "Please try using a different world.";
				return;
			}
		} catch (IOException _ex) {
			loginMessage1 = "";
		}
		loginMessage2 = "Error connecting to server.";
	}
	
	private bool doWalkTo(int i, int j, int k, int i1, int j1, int k1,
	                         int l1, int i2, int j2, bool flag, int k2) {
		byte byte0 = 104;
		byte byte1 = 104;
		for (int l2 = 0; l2 < byte0; l2++) {
			for (int i3 = 0; i3 < byte1; i3++) {
				anIntArrayArray901[l2][i3] = 0;
				anIntArrayArray825[l2][i3] = 0x5f5e0ff;
			}
		}
		int j3 = j2;
		int k3 = j1;
		anIntArrayArray901[j2][j1] = 99;
		anIntArrayArray825[j2][j1] = 0;
		int l3 = 0;
		int i4 = 0;
		bigX[l3] = j2;
		bigY[l3++] = j1;
		bool flag1 = false;
		int j4 = bigX.Length;
		uint[][] ai = aClass11Array1230[plane].anIntArrayArray294;
		while (i4 != l3) {
			j3 = bigX[i4];
			k3 = bigY[i4];
			i4 = (i4 + 1) % j4;
			if (j3 == k2 && k3 == i2) {
				flag1 = true;
				break;
			}
			if (i1 != 0) {
				if ((i1 < 5 || i1 == 10)
				    && aClass11Array1230[plane].method219(k2, j3, k3, j,
				                                      i1 - 1, i2)) {
					flag1 = true;
					break;
				}
				if (i1 < 10
				    && aClass11Array1230[plane].method220(k2, i2, k3,
				                                      i1 - 1, j, j3)) {
					flag1 = true;
					break;
				}
			}
			if (k1 != 0
			    && k != 0
			    && aClass11Array1230[plane].method221(i2, k2, j3, k, l1,
			                                      k1, k3)) {
				flag1 = true;
				break;
			}
			int l4 = anIntArrayArray825[j3][k3] + 1;
			if (j3 > 0 && anIntArrayArray901[j3 - 1][k3] == 0
			    && (ai[j3 - 1][k3] & 0x1280108) == 0) {
				bigX[l3] = j3 - 1;
				bigY[l3] = k3;
				l3 = (l3 + 1) % j4;
				anIntArrayArray901[j3 - 1][k3] = 2;
				anIntArrayArray825[j3 - 1][k3] = l4;
			}
			if (j3 < byte0 - 1 && anIntArrayArray901[j3 + 1][k3] == 0
			    && (ai[j3 + 1][k3] & 0x1280180) == 0) {
				bigX[l3] = j3 + 1;
				bigY[l3] = k3;
				l3 = (l3 + 1) % j4;
				anIntArrayArray901[j3 + 1][k3] = 8;
				anIntArrayArray825[j3 + 1][k3] = l4;
			}
			if (k3 > 0 && anIntArrayArray901[j3][k3 - 1] == 0
			    && (ai[j3][k3 - 1] & 0x1280102) == 0) {
				bigX[l3] = j3;
				bigY[l3] = k3 - 1;
				l3 = (l3 + 1) % j4;
				anIntArrayArray901[j3][k3 - 1] = 1;
				anIntArrayArray825[j3][k3 - 1] = l4;
			}
			if (k3 < byte1 - 1 && anIntArrayArray901[j3][k3 + 1] == 0
			    && (ai[j3][k3 + 1] & 0x1280120) == 0) {
				bigX[l3] = j3;
				bigY[l3] = k3 + 1;
				l3 = (l3 + 1) % j4;
				anIntArrayArray901[j3][k3 + 1] = 4;
				anIntArrayArray825[j3][k3 + 1] = l4;
			}
			if (j3 > 0 && k3 > 0 && anIntArrayArray901[j3 - 1][k3 - 1] == 0
			    && (ai[j3 - 1][k3 - 1] & 0x128010e) == 0
			    && (ai[j3 - 1][k3] & 0x1280108) == 0
			    && (ai[j3][k3 - 1] & 0x1280102) == 0) {
				bigX[l3] = j3 - 1;
				bigY[l3] = k3 - 1;
				l3 = (l3 + 1) % j4;
				anIntArrayArray901[j3 - 1][k3 - 1] = 3;
				anIntArrayArray825[j3 - 1][k3 - 1] = l4;
			}
			if (j3 < byte0 - 1 && k3 > 0
			    && anIntArrayArray901[j3 + 1][k3 - 1] == 0
			    && (ai[j3 + 1][k3 - 1] & 0x1280183) == 0
			    && (ai[j3 + 1][k3] & 0x1280180) == 0
			    && (ai[j3][k3 - 1] & 0x1280102) == 0) {
				bigX[l3] = j3 + 1;
				bigY[l3] = k3 - 1;
				l3 = (l3 + 1) % j4;
				anIntArrayArray901[j3 + 1][k3 - 1] = 9;
				anIntArrayArray825[j3 + 1][k3 - 1] = l4;
			}
			if (j3 > 0 && k3 < byte1 - 1
			    && anIntArrayArray901[j3 - 1][k3 + 1] == 0
			    && (ai[j3 - 1][k3 + 1] & 0x1280138) == 0
			    && (ai[j3 - 1][k3] & 0x1280108) == 0
			    && (ai[j3][k3 + 1] & 0x1280120) == 0) {
				bigX[l3] = j3 - 1;
				bigY[l3] = k3 + 1;
				l3 = (l3 + 1) % j4;
				anIntArrayArray901[j3 - 1][k3 + 1] = 6;
				anIntArrayArray825[j3 - 1][k3 + 1] = l4;
			}
			if (j3 < byte0 - 1 && k3 < byte1 - 1
			    && anIntArrayArray901[j3 + 1][k3 + 1] == 0
			    && (ai[j3 + 1][k3 + 1] & 0x12801e0) == 0
			    && (ai[j3 + 1][k3] & 0x1280180) == 0
			    && (ai[j3][k3 + 1] & 0x1280120) == 0) {
				bigX[l3] = j3 + 1;
				bigY[l3] = k3 + 1;
				l3 = (l3 + 1) % j4;
				anIntArrayArray901[j3 + 1][k3 + 1] = 12;
				anIntArrayArray825[j3 + 1][k3 + 1] = l4;
			}
		}
		anInt1264 = 0;
		if (!flag1) {
			if (flag) {
				int i5 = 100;
				for (int k5 = 1; k5 < 2; k5++) {
					for (int i6 = k2 - k5; i6 <= k2 + k5; i6++) {
						for (int l6 = i2 - k5; l6 <= i2 + k5; l6++)
							if (i6 >= 0 && l6 >= 0 && i6 < 104 && l6 < 104
							   && anIntArrayArray825[i6][l6] < i5) {
							i5 = anIntArrayArray825[i6][l6];
							j3 = i6;
							k3 = l6;
							anInt1264 = 1;
							flag1 = true;
						}
						
					}
					
					if (flag1)
						break;
				}
				
			}
			if (!flag1)
				return false;
		}
		i4 = 0;
		bigX[i4] = j3;
		bigY[i4++] = k3;
		int l5;
		for (int j5 = l5 = anIntArrayArray901[j3][k3]; j3 != j2 || k3 != j1; j5 = anIntArrayArray901[j3][k3]) {
			if (j5 != l5) {
				l5 = j5;
				bigX[i4] = j3;
				bigY[i4++] = k3;
			}
			if ((j5 & 2) != 0)
				j3++;
			else if ((j5 & 8) != 0)
				j3--;
			if ((j5 & 1) != 0)
				k3++;
			else if ((j5 & 4) != 0)
				k3--;
		}
		// if(cancelWalk) { return i4 > 0; }
		
		if (i4 > 0) {
			int k4 = i4;
			if (k4 > 25)
				k4 = 25;
			i4--;
			int k6 = bigX[i4];
			int i7 = bigY[i4];
			anInt1288 += k4;
			if (anInt1288 >= 92) {
				stream.createFrame(36);
				stream.writeDWord(0);
				anInt1288 = 0;
			}
			if (i == 0) {
				stream.createFrame(164);
				stream.writeWordBigEndian(k4 + k4 + 3);
			}
			if (i == 1) {
				stream.createFrame(248);
				stream.writeWordBigEndian(k4 + k4 + 3 + 14);
			}
			if (i == 2) {
				stream.createFrame(98);
				stream.writeWordBigEndian(k4 + k4 + 3);
			}
			stream.method433(k6 + baseX);
			destX = bigX[0];
			destY = bigY[0];
			for (int j7 = 1; j7 < k4; j7++) {
				i4--;
				stream.writeWordBigEndian(bigX[i4] - k6);
				stream.writeWordBigEndian(bigY[i4] - i7);
			}
			
			stream.method431(i7 + baseY);
			stream.method424(base.keyArray[5] != 1 ? 0 : 1);
			return true;
		}
		return i != 1;
	}
	
	private void method86(RS2Sharp.Stream stream) {
//		for (int j = 0; j < anInt893; j++) {
//			int k = anIntArray894[j];
//			NPC npc = npcArray[k];
//			int l = stream.readUnsignedByte();
//			if ((l & 0x10) != 0) {
//				int i1 = stream.method434();
//				if (i1 == 65535)
//					i1 = -1;
//				int i2 = stream.readUnsignedByte();
//				if (i1 == npc.anim && i1 != -1) {
//					int l2 = RS2Sharp.Animation.anims[i1].anInt365;
//					if (l2 == 1) {
//						npc.anInt1527 = 0;
//						npc.anInt1528 = 0;
//						npc.anInt1529 = i2;
//						npc.anInt1530 = 0;
//					}
//					if (l2 == 2)
//						npc.anInt1530 = 0;
//				} else if (i1 == -1
//				           || npc.anim == -1
//				           || RS2Sharp.Animation.anims[i1].anInt359 >= RS2Sharp.Animation.anims[npc.anim].anInt359) {
//					npc.anim = i1;
//					npc.anInt1527 = 0;
//					npc.anInt1528 = 0;
//					npc.anInt1529 = i2;
//					npc.anInt1530 = 0;
//					npc.anInt1542 = npc.smallXYIndex;
//				}
//			}
//			if ((l & 8) != 0) {
//				int j1 = stream.method426();
//				int j2 = stream.method427();
//				npc.updateHitData(j2, j1, loopCycle);
//				npc.loopCycleStatus = loopCycle + 300;
//				npc.currentHealth = stream.method426();
//				npc.maxHealth = stream.readUnsignedByte();
//			}
//			if ((l & 0x80) != 0) {
//				npc.anInt1520 = stream.readUnsignedWord();
//				int k1 = stream.readDWord();
//				npc.anInt1524 = k1 >> 16;
//				npc.anInt1523 = loopCycle + (k1 & 0xffff);
//				npc.anInt1521 = 0;
//				npc.anInt1522 = 0;
//				if (npc.anInt1523 > loopCycle)
//					npc.anInt1521 = -1;
//				if (npc.anInt1520 == 65535)
//					npc.anInt1520 = -1;
//			}
//			if ((l & 0x20) != 0) {
//				npc.interactingEntity = stream.readUnsignedWord();
//				if (npc.interactingEntity == 65535)
//					npc.interactingEntity = -1;
//			}
//			if ((l & 1) != 0) {
//				npc.textSpoken = stream.readString();
//				npc.textCycle = 100;
//				// entityMessage(npc);
//				
//			}
//			if ((l & 0x40) != 0) {
//				int l1 = stream.method427();
//				int k2 = stream.method428();
//				npc.updateHitData(k2, l1, loopCycle);
//				npc.loopCycleStatus = loopCycle + 300;
//				npc.currentHealth = stream.method428();
//				npc.maxHealth = stream.method427();
//			}
//			if ((l & 2) != 0) {
//				npc.desc = EntityDef.forID(stream.method436());
//				npc.anInt1540 = npc.desc.aByte68;
//				npc.anInt1504 = npc.desc.anInt79;
//				npc.anInt1554 = npc.desc.walkAnim;
//				npc.anInt1555 = npc.desc.anInt58;
//				npc.anInt1556 = npc.desc.anInt83;
//				npc.anInt1557 = npc.desc.anInt55;
//				npc.anInt1511 = npc.desc.standAnim;
//			}
//			if ((l & 4) != 0) {
//				npc.anInt1538 = stream.method434();
//				npc.anInt1539 = stream.method434();
//			}
//		}
		for (int j = 0; j < anInt893; j++) {
			int k = anIntArray894[j];
			NPC npc = npcArray[k];
			int l = stream.readUnsignedByte();
			if ((l & 0x10) != 0) {
				int i1 = stream.method434();
				if (i1 == 65535)
					i1 = -1;
				int i2 = stream.readUnsignedByte();
				if (i1 == npc.anim && i1 != -1) {
					int l2 = RS2Sharp.Animation.anims[i1].anInt365;
					if (l2 == 1) {
						npc.anInt1527 = 0;
						npc.anInt1528 = 0;
						npc.anInt1529 = i2;
						npc.anInt1530 = 0;
					}
					if (l2 == 2)
						npc.anInt1530 = 0;
				} else if (i1 == -1
				           || npc.anim == -1
				           || RS2Sharp.Animation.anims[i1].anInt359 >= RS2Sharp.Animation.anims[npc.anim].anInt359) {
					npc.anim = i1;
					npc.anInt1527 = 0;
					npc.anInt1528 = 0;
					npc.anInt1529 = i2;
					npc.anInt1530 = 0;
					npc.anInt1542 = npc.smallXYIndex;
				}
			}
			if ((l & 8) != 0) {
				int j1 = stream.method426();
				int j2 = stream.method427();
				int type1 = stream.readUnsignedByte();
				npc.updateHitData(j2, j1, type1, loopCycle);
				npc.loopCycleStatus = loopCycle + 300;
				npc.currentHealth = stream.method426();
				npc.maxHealth = stream.readUnsignedByte();
			}
			if ((l & 0x80) != 0) {
				npc.anInt1520 = stream.readUnsignedWord();
				int k1 = stream.readDWord();
				npc.anInt1524 = k1 >> 16;
				npc.anInt1523 = loopCycle + (k1 & 0xffff);
				npc.anInt1521 = 0;
				npc.anInt1522 = 0;
				if (npc.anInt1523 > loopCycle)
					npc.anInt1521 = -1;
				if (npc.anInt1520 == 65535)
					npc.anInt1520 = -1;
			}
			if ((l & 0x20) != 0) {
				npc.interactingEntity = stream.readUnsignedWord();
				if (npc.interactingEntity == 65535)
					npc.interactingEntity = -1;
			}
			if ((l & 1) != 0) {
				npc.textSpoken = stream.readString();
				npc.textCycle = 100;
				// entityMessage(npc);
				
			}
			if ((l & 0x40) != 0) {
				int l1 = stream.method427();
				int k2 = stream.method428();
				int type2 = stream.readUnsignedByte();
				npc.updateHitData(k2, l1, type2, loopCycle);
				npc.loopCycleStatus = loopCycle + 300;
				npc.currentHealth = stream.method428();
				npc.maxHealth = stream.method427();
			}
			if ((l & 2) != 0) {
				npc.desc = EntityDef.forID(stream.method436());
				npc.anInt1540 = npc.desc.aByte68;
				npc.anInt1504 = npc.desc.anInt79;
				npc.anInt1554 = npc.desc.walkAnim;
				npc.anInt1555 = npc.desc.anInt58;
				npc.anInt1556 = npc.desc.anInt83;
				npc.anInt1557 = npc.desc.anInt55;
				npc.anInt1511 = npc.desc.standAnim;
			}
			if ((l & 4) != 0) {
				npc.anInt1538 = stream.method434();
				npc.anInt1539 = stream.method434();
			}
		}
	}
	
	private void buildAtNPCMenu(EntityDef entityDef, int i, int j, int k) {
		if (menuActionRow >= 400)
			return;
		if (entityDef.childrenIDs != null)
			entityDef = entityDef.method161();
		if (entityDef == null)
			return;
		if (!entityDef.aBoolean84)
			return;
		String s = entityDef.name;
		if (entityDef.combatLevel != 0)
			s = s
				+ combatDiffColor(myPlayer.combatLevel,
				                  entityDef.combatLevel) + " (level-"
				+ entityDef.combatLevel + ")";
		if (itemSelected == 1) {
			menuActionName[menuActionRow] = "Use " + selectedItemName
				+ " with @yel@" + s;
			menuActionID[menuActionRow] = 582;
			menuActionCmd1[menuActionRow] = i;
			menuActionCmd2[menuActionRow] = k;
			menuActionCmd3[menuActionRow] = j;
			menuActionRow++;
			return;
		}
		if (spellSelected == 1) {
			if ((spellUsableOn & 2) == 2) {
				menuActionName[menuActionRow] = spellTooltip + " @yel@" + s;
				menuActionID[menuActionRow] = 413;
				menuActionCmd1[menuActionRow] = i;
				menuActionCmd2[menuActionRow] = k;
				menuActionCmd3[menuActionRow] = j;
				menuActionRow++;
			}
		} else {
			if (entityDef.actions != null) {
				for (int l = 4; l >= 0; l--)
					if (entityDef.actions[l] != null
					   && !entityDef.actions[l].ToLower().Equals("attack")) {
					menuActionName[menuActionRow] = entityDef.actions[l]
					+ " @yel@" + s;
					if (l == 0)
						menuActionID[menuActionRow] = 20;
					if (l == 1)
						menuActionID[menuActionRow] = 412;
					if (l == 2)
						menuActionID[menuActionRow] = 225;
					if (l == 3)
						menuActionID[menuActionRow] = 965;
					if (l == 4)
						menuActionID[menuActionRow] = 478;
					menuActionCmd1[menuActionRow] = i;
					menuActionCmd2[menuActionRow] = k;
					menuActionCmd3[menuActionRow] = j;
					menuActionRow++;
				}
				
			}
			if (entityDef.actions != null) {
				for (int i1 = 4; i1 >= 0; i1--)
					if (entityDef.actions[i1] != null
					   && entityDef.actions[i1].ToLower().Equals("attack")) {
					char c = '\0';
					if (entityDef.combatLevel > myPlayer.combatLevel)
						c = '\u07D0';
					menuActionName[menuActionRow] = entityDef.actions[i1]
					+ " @yel@" + s;
					if (i1 == 0)
						menuActionID[menuActionRow] = 20 + c;
					if (i1 == 1)
						menuActionID[menuActionRow] = 412 + c;
					if (i1 == 2)
						menuActionID[menuActionRow] = 225 + c;
					if (i1 == 3)
						menuActionID[menuActionRow] = 965 + c;
					if (i1 == 4)
						menuActionID[menuActionRow] = 478 + c;
					menuActionCmd1[menuActionRow] = i;
					menuActionCmd2[menuActionRow] = k;
					menuActionCmd3[menuActionRow] = j;
					menuActionRow++;
				}
				
			}
			// menuActionName[menuActionRow] = "Examine @yel@" + s +
			// " @gre@(@whi@" + entityDef.type + "@gre@)";
			menuActionName[menuActionRow] = "Examine @yel@" + s;
			menuActionID[menuActionRow] = 1025;
			menuActionCmd1[menuActionRow] = i;
			menuActionCmd2[menuActionRow] = k;
			menuActionCmd3[menuActionRow] = j;
			menuActionRow++;
		}
	}
	
	private void buildAtPlayerMenu(int i, int j, Player player, int k) {
		if (player == myPlayer)
			return;
		if (menuActionRow >= 400)
			return;
		String s;
		if (player.skill == 0)
			s = player.name
				+ combatDiffColor(myPlayer.combatLevel, player.combatLevel)
				+ " (level-" + player.combatLevel + ")";
		else
			s = player.name + " (skill-" + player.skill + ")";
		if (itemSelected == 1) {
			menuActionName[menuActionRow] = "Use " + selectedItemName
				+ " with @whi@" + s;
			menuActionID[menuActionRow] = 491;
			menuActionCmd1[menuActionRow] = j;
			menuActionCmd2[menuActionRow] = i;
			menuActionCmd3[menuActionRow] = k;
			menuActionRow++;
		} else if (spellSelected == 1) {
			if ((spellUsableOn & 8) == 8) {
				menuActionName[menuActionRow] = spellTooltip + " @whi@" + s;
				menuActionID[menuActionRow] = 365;
				menuActionCmd1[menuActionRow] = j;
				menuActionCmd2[menuActionRow] = i;
				menuActionCmd3[menuActionRow] = k;
				menuActionRow++;
			}
		} else {
			for (int l = 4; l >= 0; l--)
			if (atPlayerActions[l] != null) {
				menuActionName[menuActionRow] = atPlayerActions[l]
				+ " @whi@" + s;
				char c = '\0';
				if (atPlayerActions[l].ToLower().Equals("attack")) {
					if (player.combatLevel > myPlayer.combatLevel)
						c = '\u07D0';
					if (myPlayer.team != 0 && player.team != 0)
						if (myPlayer.team == player.team)
							c = '\u07D0';
					else
						c = '\0';
				} else if (atPlayerArray[l])
					c = '\u07D0';
				if (l == 0)
					menuActionID[menuActionRow] = 561 + c;
				if (l == 1)
					menuActionID[menuActionRow] = 779 + c;
				if (l == 2)
					menuActionID[menuActionRow] = 27 + c;
				if (l == 3)
					menuActionID[menuActionRow] = 577 + c;
				if (l == 4)
					menuActionID[menuActionRow] = 729 + c;
				menuActionCmd1[menuActionRow] = j;
				menuActionCmd2[menuActionRow] = i;
				menuActionCmd3[menuActionRow] = k;
				menuActionRow++;
			}
			
		}
 		for (int i1 = 0; i1 < menuActionRow; i1++)
 		if (menuActionID[i1] == 516) {
 			menuActionName[i1] = "Walk here @whi@" + s;
 			return;
 		}
		
	}
	
	private void method89(Class30_Sub1 class30_sub1) {
		int i = 0;
		int j = -1;
		int k = 0;
		int l = 0;
		if (class30_sub1.anInt1296 == 0)
			i = 0;//worldController.method300(class30_sub1.anInt1295,
//			                              class30_sub1.anInt1297, class30_sub1.anInt1298);
		if (class30_sub1.anInt1296 == 1)
			i = 0;//worldController.method301(class30_sub1.anInt1295,
			                             // class30_sub1.anInt1297, class30_sub1.anInt1298);
		if (class30_sub1.anInt1296 == 2)
			i = 0;//worldController.method302(class30_sub1.anInt1295,
			                             // class30_sub1.anInt1297, class30_sub1.anInt1298);
		if (class30_sub1.anInt1296 == 3)
			i = 0;//worldController.method303(class30_sub1.anInt1295,
			                              //class30_sub1.anInt1297, class30_sub1.anInt1298);
		if (i != 0) {
			int i1 = 0;//worldController.method304(class30_sub1.anInt1295,
			                                  // class30_sub1.anInt1297, class30_sub1.anInt1298, i);
			j = i >> 14 & 0x7fff;
			k = i1 & 0x1f;
			l = i1 >> 6;
		}
		class30_sub1.anInt1299 = j;
		class30_sub1.anInt1301 = k;
		class30_sub1.anInt1300 = l;
	}
	
	private void method90() {
		for (int i = 0; i < anInt1062; i++)
		if (anIntArray1250[i] <= 0) {
			bool flag1 = false;
			try {
				if (anIntArray1207[i] == anInt874
				    && anIntArray1241[i] == anInt1289) {
					if (!replayWave())
						flag1 = true;
				} else {
					RS2Sharp.Stream stream = Sounds.method241(anIntArray1241[i],
					                                 anIntArray1207[i]);
					if (NetDrawingTools.CurrentTimeMillis() + stream.currentOffset
					    / 22 > aLong1172 + anInt1257 / 22) {
						anInt1257 = stream.currentOffset;
						aLong1172 = NetDrawingTools.CurrentTimeMillis();
						if (saveWave(stream.buffer, stream.currentOffset)) {
							anInt874 = anIntArray1207[i];
							anInt1289 = anIntArray1241[i];
						} else {
							flag1 = true;
						}
					}
				}
			} catch (Exception exception) {
			}
			if (!flag1 || anIntArray1250[i] == -5) {
				anInt1062--;
				for (int j = i; j < anInt1062; j++) {
					anIntArray1207[j] = anIntArray1207[j + 1];
					anIntArray1241[j] = anIntArray1241[j + 1];
					anIntArray1250[j] = anIntArray1250[j + 1];
				}
				
				i--;
			} else {
				anIntArray1250[i] = -5;
			}
		} else {
			anIntArray1250[i]--;
		}
		
		if (prevSong > 0) {
			prevSong -= 20;
			if (prevSong < 0)
				prevSong = 0;
			if (prevSong == 0 && musicEnabled && !lowMem) {
				nextSong = currentSong;
				songChanging = true;
				onDemandFetcher.method558(2, nextSong);
			}
		}
	}

	public void Start()
	{
		//socketStream = gameObject.AddComponent<RSSocket> ();
//		string[] args = new string[0];
//		UnityEngine.Debug.Log("RS2 user client - release #" + 317);
		nodeID = 0;
		portOff = 0;
		setHighMem();
		isMembers = true;
		signlink.storeid = 0;
		//signlink.startpriv(IPAddress.Parse("127.0.0.1"));
		client client1 = this;
		client1.createClientFrame(503, 765);
	}

	public override void startUp() {
		drawLoadingText(20, "Starting up");
		bool policyOK = Security.PrefetchSocketPolicy(serverIP, 43593);
		UnityEngine.Debug.Log("Policy ok? : " + policyOK);
		if (signlink.sunjava)
			base.minDelay = 5;
		aBoolean993 = true;
		bool flag = true;
		String s = getDocumentBaseHost();
		if (signlink.cache_dat != null) {
			for (int i = 0; i < 5; i++)
				decompressors[i] = new Decompressor(signlink.cache_dat,
				                                    signlink.cache_idx[i], i + 1);
		}
			titleStreamLoader = streamLoaderForName(1, "title screen", "title",
			                                        expectedCRCs[1], 25);
			smallText = new TextDrawingArea(false, "p11_full",
			                                titleStreamLoader);
			aTextDrawingArea_1271 = new TextDrawingArea(false, "p12_full",
			                                            titleStreamLoader);
			chatTextDrawingArea = new TextDrawingArea(false, "b12_full",
			                                          titleStreamLoader);
			TextDrawingArea aTextDrawingArea_1273 = new TextDrawingArea(true,
			                                                            "q8_full", titleStreamLoader);
			drawLogo();
			loadTitleScreen();
			
			StreamLoader streamLoader = streamLoaderForName(2, "config",
			                                                "config", expectedCRCs[2], 30);
			StreamLoader streamLoader_1 = streamLoaderForName(3, "interface",
			                                                  "interface", expectedCRCs[3], 35);
			StreamLoader streamLoader_2 = streamLoaderForName(4, "2d graphics",
			                                                  "media", expectedCRCs[4], 40);
			StreamLoader streamLoader_3 = streamLoaderForName(6, "textures",
			                                                  "textures", expectedCRCs[6], 45);
			StreamLoader streamLoader_4 = streamLoaderForName(7, "chat system",
			                                                  "wordenc", expectedCRCs[7], 50);
			StreamLoader streamLoader_5 = streamLoaderForName(8,
			                                                  "sound effects", "sounds", expectedCRCs[8], 55);
			StreamLoader streamLoader_6 = streamLoaderForName(5, "update list",
		                                                  "versionlist", expectedCRCs[5], 60);
			SpriteLoader.loadSprites(streamLoader_2);
			cacheSprite = SpriteLoader.sprites;
			byteGroundArray = NetDrawingTools.CreateTrippleByteArray(4, 104, 104);// new byte[4][104][104];
			intGroundArray = NetDrawingTools.CreateTrippleIntArray(4, 105, 105);// new int[4][105][105];
			//worldController = new WorldController(intGroundArray, this);
			for (int j = 0; j < 4; j++)
				aClass11Array1230[j] = new CollisionMap();
			
			aClass30_Sub2_Sub1_Sub1_1263 = new RS2Sharp.Sprite(512, 512);
			drawLoadingText(60, "Connecting to update server");
//			onDemandFetcher = new OnDemandFetcher();
//			onDemandFetcher.start(streamLoader_6, this);
			Class36.method528(onDemandFetcher.getAnimCount());
			Model.initialize(65535, onDemandFetcher);
			
			drawLoadingText(80, "Unpacking media");
			/* Custom sprite unpacking */
			welcomeScreen = cacheSprite[8];
			loginBox = cacheSprite[9];
			// optionsBox = cacheSprite[9];
			camera = cacheSprite[10];
			cameraHover = cacheSprite[11];
			disabledCamera = cacheSprite[12];
			disabledCameraHover = cacheSprite[13];
			loginBackground = cacheSprite[14];
			settingsBox = cacheSprite[15];
			fixedHover = cacheSprite[16];
			resizableHover = cacheSprite[17];
			fullscreenHover = cacheSprite[18];
			sdHover = cacheSprite[19];
			hdHover = cacheSprite[20];
			sdClicked = cacheSprite[21];
			hdClicked = cacheSprite[22];
			xHover = cacheSprite[23];
			musicButton = cacheSprite[24];
			rememberButton = cacheSprite[25];
			loginButton = cacheSprite[26];
			boxHover = cacheSprite[27];
			brightHover = cacheSprite[28];
			loginHover = cacheSprite[29];
			logIconH = cacheSprite[32];
			logIconC = cacheSprite[31];
			logIconN = cacheSprite[30];
			logo = cacheSprite[33];
			chatArea = new RS2Sharp.Sprite(client.ReadAllBytes(signlink.findcachedir() + "Sprites/Fullscreen/20.png"));//cacheSprite[0];
			tabArea = new RS2Sharp.Sprite(client.ReadAllBytes(signlink.findcachedir() + "Sprites/Fullscreen/47.png"));//cacheSprite[2];

			mapArea = new RS2Sharp.Sprite(client.ReadAllBytes(signlink.findcachedir() + "Sprites/Fullscreen/44.png"));
			
			HPBarEmpty =  new RS2Sharp.Sprite(client.ReadAllBytes(signlink.findcachedir() + "Sprites/HITPOINTS_1.png"));//new Sprite("newSprites/HITPOINTS_1");
			oldHit = new RS2Sharp.Sprite(client.ReadAllBytes(signlink.findcachedir() + "Sprites/hit.png"));//new Sprite("newSprites/hit");
			bigHit = new RS2Sharp.Sprite(client.ReadAllBytes(signlink.findcachedir() + "Sprites/bhit.png"));//new Sprite("newSprites/bhit");
			critHit = new RS2Sharp.Sprite(client.ReadAllBytes(signlink.findcachedir() + "Sprites/chit.png"));//new Sprite("newSprites/chit");
			
			/**/
			mapBack = new Background(streamLoader_2, "mapback", 0);
			for (int c1 = 0; c1 <= 3; c1++)
				chatButtons[c1] = new RS2Sharp.Sprite(streamLoader_2, "chatbuttons", c1);
			for (int j3 = 0; j3 <= 14; j3++)
				sideIcons[j3] = new RS2Sharp.Sprite(streamLoader_2, "sideicons", j3);
			
			redStones[0] = cacheSprite[3];
			redStones[1] = cacheSprite[4];
			redStones[2] = cacheSprite[5];
			redStones[3] = cacheSprite[6];
			redStones[4] = cacheSprite[7];
			
			compass = new RS2Sharp.Sprite(streamLoader_2, "compass", 0);
			mapEdge = new RS2Sharp.Sprite(streamLoader_2, "mapedge", 0);
			mapEdge.method345();
			try {
				for (int k3 = 0; k3 < 100; k3++)
					mapScenes[k3] = new Background(streamLoader_2, "mapscene",
					                               k3);
			} catch (Exception _ex) {
			}
			try {
				for (int l3 = 0; l3 < 100; l3++)
				if(l3 < 75 ) {
					mapFunctions[l3] = new RS2Sharp.Sprite(streamLoader_2,"mapfunction", l3);
				} else {
					mapFunctions[l3] = new RS2Sharp.Sprite(streamLoader_2,"mapfunctions2", (l3-76));
				}
			} catch (Exception _ex) {
			}
			try {
				for (int i4 = 0; i4 < 20; i4++)
					hitMarks[i4] = new RS2Sharp.Sprite(streamLoader_2, "hitmarks", i4);
			} catch (Exception _ex) {
			}
			try {
				for (int h1 = 0; h1 < 6; h1++)
					headIconsHint[h1] = new RS2Sharp.Sprite(streamLoader_2,
					                               "headicons_hint", h1);
			} catch (Exception _ex) {
			}
			try {
				for (int j4 = 0; j4 < 8; j4++)
					headIcons[j4] = new RS2Sharp.Sprite(streamLoader_2,
					                           "headicons_prayer", j4);
				for (int j45 = 0; j45 < 3; j45++)
					skullIcons[j45] = new RS2Sharp.Sprite(streamLoader_2,
					                             "headicons_pk", j45);
			} catch (Exception _ex) {
			}
			mapFlag = new RS2Sharp.Sprite(streamLoader_2, "mapmarker", 0);
			mapMarker = new RS2Sharp.Sprite(streamLoader_2, "mapmarker", 1);
			for (int k4 = 0; k4 < 8; k4++)
				crosses[k4] = new RS2Sharp.Sprite(streamLoader_2, "cross", k4);
			
			mapDotItem = new RS2Sharp.Sprite(streamLoader_2, "mapdots", 0);
			mapDotNPC = new RS2Sharp.Sprite(streamLoader_2, "mapdots", 1);
			mapDotPlayer = new RS2Sharp.Sprite(streamLoader_2, "mapdots", 2);
			mapDotFriend = new RS2Sharp.Sprite(streamLoader_2, "mapdots", 3);
			mapDotTeam = new RS2Sharp.Sprite(streamLoader_2, "mapdots", 4);
			scrollBar1 = new RS2Sharp.Sprite(streamLoader_2, "scrollbar", 0);
			scrollBar2 = new RS2Sharp.Sprite(streamLoader_2, "scrollbar", 1);
			
			for (int l4 = 0; l4 < 2; l4++)
				modIcons[l4] = new Background(streamLoader_2, "mod_icons", l4);
			
			RS2Sharp.Sprite sprite = new RS2Sharp.Sprite(streamLoader_2, "screenframe", 0);
			leftFrame = new RSImageProducer(sprite.myWidth, sprite.myHeight);
			sprite.method346(0, 0);
			sprite = new RS2Sharp.Sprite(streamLoader_2, "screenframe", 1);
			topFrame = new RSImageProducer(sprite.myWidth, sprite.myHeight);
			sprite.method346(0, 0);
			sprite = new RS2Sharp.Sprite(streamLoader_2, "screenframe", 2);
			rightFrame = new RSImageProducer(sprite.myWidth, sprite.myHeight);
			sprite.method346(0, 0);
			sprite = new RS2Sharp.Sprite(streamLoader_2, "mapedge", 0);
			mapEdgeIP = new RSImageProducer(sprite.myWidth, sprite.myHeight);
			sprite.method346(0, 0);
			
			int i5 = (int) (RS2Sharp.Random.Next(21)) - 10;
			int j5 = (int) (RS2Sharp.Random.Next(21)) - 10;
			int k5 = (int) (RS2Sharp.Random.Next(21)) - 10;
			int l5 = (int) (RS2Sharp.Random.Next(41)) - 20;
			for (int i6 = 0; i6 < 100; i6++) {
				if (mapFunctions[i6] != null)
					mapFunctions[i6].method344(i5 + l5, j5 + l5, k5 + l5);
				if (mapScenes[i6] != null)
					mapScenes[i6].method360(i5 + l5, j5 + l5, k5 + l5);
			}
			
			drawLoadingText(83, "Unpacking textures");
			RS2Sharp.Texture.method368(streamLoader_3);
			RS2Sharp.Texture.method372(0.80000000000000004D);
			RS2Sharp.Texture.method367();
			drawLoadingText(86, "Unpacking config");
			RS2Sharp.Animation.unpackConfig(streamLoader);
			ObjectDef.unpackConfig(streamLoader);
			Flo.unpackConfig(streamLoader);
			OverLayFlo317.unpackConfig(streamLoader);
			ItemDef.unpackConfig(streamLoader);
			EntityDef.unpackConfig(streamLoader);
			IDK.unpackConfig(streamLoader);
			SpotAnim.unpackConfig(streamLoader);
			Varp.unpackConfig(streamLoader);
			VarBit.unpackConfig(streamLoader);
			ItemDef.isMembers = isMembers;
			if (!lowMem) {
				drawLoadingText(90, "Unpacking sounds");
				byte[] abyte0 = streamLoader_5.getDataForName("sounds.dat");
				RS2Sharp.Stream stream = new RS2Sharp.Stream(abyte0);
				Sounds.unpack(stream);
			}
			drawLoadingText(95, "Unpacking interfaces");
			TextDrawingArea[] aclass30_sub2_sub1_sub4s = { smallText,
				aTextDrawingArea_1271, chatTextDrawingArea,
				aTextDrawingArea_1273 };
			RSInterface.unpack(streamLoader_1, aclass30_sub2_sub1_sub4s,
			                   streamLoader_2);
			drawLoadingText(100, "Preparing game engine");
			for (int j6 = 0; j6 < 33; j6++) {
				int k6 = 999;
				int i7 = 0;
				for (int k7 = 0; k7 < 34; k7++) {
					if (mapBack.aByteArray1450[k7 + j6 * mapBack.anInt1452] == 0) {
						if (k6 == 999)
							k6 = k7;
						continue;
					}
					if (k6 == 999)
						continue;
					i7 = k7;
					break;
				}
				
				anIntArray968[j6] = k6;
				anIntArray1057[j6] = i7 - k6;
			}
			
			for (int l6 = 5; l6 < 156; l6++) {
				int j7 = 999;
				int l7 = 0;
				for (int j8 = 25; j8 < 172; j8++) {
					if (mapBack.aByteArray1450[j8 + l6 * mapBack.anInt1452] == 0
					    && (j8 > 34 || l6 > 34)) {
						if (j7 == 999)
							j7 = j8;
						continue;
					}
					if (j7 == 999)
						continue;
					l7 = j8;
					break;
				}
				
				anIntArray1052[l6 - 5] = j7 - 25;
				anIntArray1229[l6 - 5] = l7 - j7;
			}

			setBounds();

			Censor.loadConfig(streamLoader_4);
//			mouseDetection = new MouseDetection(this);
			//Animable_Sub5.clientInstance = this;
			//ObjectDef.clientInstance = this;
			//EntityDef.clientInstance = this;
			login (myUsername, myPassword, false);
		currentlyStarting = false;
			return;
		loadingError = true;
	}
	
	private void method91(RS2Sharp.Stream stream, int i) {
		while (stream.bitPosition + 10 < i * 8) {
			int j = stream.readBits(11);
			if (j == 2047)
				break;
			if (playerArray[j] == null) {
				playerArray[j] = new Player();
				if (aStreamArray895s[j] != null)
					playerArray[j].updatePlayer(aStreamArray895s[j]);
			}
			playerIndices[playerCount++] = j;
			Player player = playerArray[j];
			player.anInt1537 = loopCycle;
			int k = stream.readBits(1);
			if (k == 1)
				anIntArray894[anInt893++] = j;
			int l = stream.readBits(1);
			int i1 = stream.readBits(5);
			if (i1 > 15)
				i1 -= 32;
			int j1 = stream.readBits(5);
			if (j1 > 15)
				j1 -= 32;
			player.setPos(myPlayer.smallX[0] + j1, myPlayer.smallY[0] + i1,
			              l == 1);
		}
		stream.finishBitAccess();
	}
	
	private void processMainScreenClick() {
		if (anInt1021 != 0)
			return;
		if (base.clickMode3 == 1) {
			int i = base.saveClickX - 25 - 547;
			int j = base.saveClickY - 5 - 3;
			
			i = base.saveClickX - (myWidth - 182 + 25) + 36;
			j = base.saveClickY - 12;
			
			
			if (i >= 0 && j >= 0 && i < 146 && j < 151) {
				i -= 73;
				j -= 75;
				int k = minimapInt1 + minimapInt2 & 0x7ff;
				int i1 = RS2Sharp.Texture.SINE[k];
				int j1 = RS2Sharp.Texture.COSINE[k];
				i1 = i1 * (minimapInt3 + 256) >> 8;
				j1 = j1 * (minimapInt3 + 256) >> 8;
				int k1 = j * i1 + i * j1 >> 11;
				int l1 = j * j1 - i * i1 >> 11;
				int i2 = myPlayer.x + k1 >> 7;
				int j2 = myPlayer.y - l1 >> 7;
				bool flag1 = doWalkTo(1, 0, 0, 0, myPlayer.smallY[0], 0, 0,
				                         j2, myPlayer.smallX[0], true, i2);
				if (flag1) {
					stream.writeWordBigEndian(i);
					stream.writeWordBigEndian(j);
					stream.writeWord(minimapInt1);
					stream.writeWordBigEndian(57);
					stream.writeWordBigEndian(minimapInt2);
					stream.writeWordBigEndian(minimapInt3);
					stream.writeWordBigEndian(89);
					stream.writeWord(myPlayer.x);
					stream.writeWord(myPlayer.y);
					stream.writeWordBigEndian(anInt1264);
					stream.writeWordBigEndian(63);
				}
			}
			anInt1117++;
			if (anInt1117 > 1151) {
				anInt1117 = 0;
				stream.createFrame(246);
				stream.writeWordBigEndian(0);
				int l = stream.currentOffset;
				if ((int) (RS2Sharp.Random.Next(2)) == 0)
					stream.writeWordBigEndian(101);
				stream.writeWordBigEndian(197);
				stream.writeWord((int) (RS2Sharp.Random.Next( 65536)));
				stream.writeWordBigEndian((int) (RS2Sharp.Random.Next( 256)));
				stream.writeWordBigEndian(67);
				stream.writeWord(14214);
				if ((int) (RS2Sharp.Random.Next( 2)) == 0)
					stream.writeWord(29487);
				stream.writeWord((int) (RS2Sharp.Random.Next( 65536)));
				if ((int) (RS2Sharp.Random.Next( 2)) == 0)
					stream.writeWordBigEndian(220);
				stream.writeWordBigEndian(180);
				stream.writeBytes(stream.currentOffset - l);
			}
		}
	}
	
	private String interfaceIntToString(int j) {
		if (j < 0x3b9ac9ff)
			return ""+j;
		else
			return "*";
	}
	
	private void showErrorScreen() {
// 		Graphics g = getGameComponent().getGraphics();
// 		g.setColor(Color.black);
// 		g.fillRect(0, 0, 765, 503);
// 		method4(1);
// 		if (loadingError) {
// 			aBoolean831 = false;
// 			g.setFont(new Font("Helvetica", 1, 16));
// 			g.setColor(Color.yellow);
// 			int k = 35;
// 			g.drawString(
// 				"Sorry, an error has occured whilst loading RuneScape", 30,
// 				k);
// 			k += 50;
// 			g.setColor(Color.white);
// 			g.drawString("To fix this try the following (in order):", 30, k);
// 			k += 50;
// 			g.setColor(Color.white);
// 			g.setFont(new Font("Helvetica", 1, 12));
// 			g.drawString(
// 				"1: Try closing ALL open web-browser windows, and reloading",
// 				30, k);
// 			k += 30;
// 			g.drawString(
// 				"2: Try clearing your web-browsers cache from tools->internet options",
// 				30, k);
// 			k += 30;
// 			g.drawString("3: Try using a different game-world", 30, k);
// 			k += 30;
// 			g.drawString("4: Try rebooting your computer", 30, k);
// 			k += 30;
// 			g.drawString(
// 				"5: Try selecting a different version of Java from the play-game menu",
// 				30, k);
// 		}
// 		if (genericLoadingError) {
// 			aBoolean831 = false;
// 			g.setFont(new Font("Helvetica", 1, 20));
// 			g.setColor(Color.white);
// 			g.drawString("Error - unable to load game!", 50, 50);
// 			g.drawString("To play RuneScape make sure you play from", 50, 100);
// 			g.drawString("http://www.runescape.com", 50, 150);
// 		}
// 		if (rsAlreadyLoaded) {
// 			aBoolean831 = false;
// 			g.setColor(Color.yellow);
// 			int l = 35;
// 			g.drawString(
// 				"Error a copy of RuneScape already appears to be loaded",
// 				30, l);
// 			l += 50;
// 			g.setColor(Color.white);
// 			g.drawString("To fix this try the following (in order):", 30, l);
// 			l += 50;
// 			g.setColor(Color.white);
// 			g.setFont(new Font("Helvetica", 1, 12));
// 			g.drawString(
// 				"1: Try closing ALL open web-browser windows, and reloading",
// 				30, l);
// 			l += 30;
// 			g.drawString("2: Try rebooting your computer, and reloading", 30, l);
// 			l += 30;
// 		}
	}

//	public URL getCodeBase() {
//		try {
//			return new URL(server + ":" + (80 + portOff));
//		} catch (Exception _ex) {
//		}
//		return null;
//	}
	
	private void method95() {
		for (int j = 0; j < npcCount; j++) {
			int k = npcIndices[j];
			NPC npc = npcArray[k];
			if (npc != null)
				method96(npc);
		}
	}
	
	private void method96(Entity entity) {
		if (entity.x < 128 || entity.y < 128 || entity.x >= 13184
		    || entity.y >= 13184) {
			entity.anim = -1;
			entity.anInt1520 = -1;
			entity.anInt1547 = 0;
			entity.anInt1548 = 0;
			entity.x = entity.smallX[0] * 128 + entity.anInt1540 * 64;
			entity.y = entity.smallY[0] * 128 + entity.anInt1540 * 64;
			entity.method446();
		}
		if (entity == myPlayer
		    && (entity.x < 1536 || entity.y < 1536 || entity.x >= 11776 || entity.y >= 11776)) {
			entity.anim = -1;
			entity.anInt1520 = -1;
			entity.anInt1547 = 0;
			entity.anInt1548 = 0;
			entity.x = entity.smallX[0] * 128 + entity.anInt1540 * 64;
			entity.y = entity.smallY[0] * 128 + entity.anInt1540 * 64;
			entity.method446();
		}
		if (entity.anInt1547 > loopCycle)
			method97(entity);
		else if (entity.anInt1548 >= loopCycle)
			method98(entity);
		else
			method99(entity);
		method100(entity);
		method101(entity);
	}
	
	private void method97(Entity entity) {
		int i = entity.anInt1547 - loopCycle;
		int j = entity.anInt1543 * 128 + entity.anInt1540 * 64;
		int k = entity.anInt1545 * 128 + entity.anInt1540 * 64;
		entity.x += (j - entity.x) / i;
		entity.y += (k - entity.y) / i;
		entity.anInt1503 = 0;
		if (entity.anInt1549 == 0)
			entity.turnDirection = 1024;
		if (entity.anInt1549 == 1)
			entity.turnDirection = 1536;
		if (entity.anInt1549 == 2)
			entity.turnDirection = 0;
		if (entity.anInt1549 == 3)
			entity.turnDirection = 512;
	}
	
	private void method98(Entity entity) {
		if (entity.anInt1548 == loopCycle
		    || entity.anim == -1
		    || entity.anInt1529 != 0
		    || entity.anInt1528 + 1 > RS2Sharp.Animation.anims[entity.anim]
		    .duration(entity.anInt1527)) {
			int i = entity.anInt1548 - entity.anInt1547;
			int j = loopCycle - entity.anInt1547;
			int k = entity.anInt1543 * 128 + entity.anInt1540 * 64;
			int l = entity.anInt1545 * 128 + entity.anInt1540 * 64;
			int i1 = entity.anInt1544 * 128 + entity.anInt1540 * 64;
			int j1 = entity.anInt1546 * 128 + entity.anInt1540 * 64;
			entity.x = (k * (i - j) + i1 * j) / i;
			entity.y = (l * (i - j) + j1 * j) / i;
		}
		entity.anInt1503 = 0;
		if (entity.anInt1549 == 0)
			entity.turnDirection = 1024;
		if (entity.anInt1549 == 1)
			entity.turnDirection = 1536;
		if (entity.anInt1549 == 2)
			entity.turnDirection = 0;
		if (entity.anInt1549 == 3)
			entity.turnDirection = 512;
		entity.anInt1552 = entity.turnDirection;
	}
	
	private void method99(Entity entity) {
		entity.anInt1517 = entity.anInt1511;
		if (entity.smallXYIndex == 0) {
			entity.anInt1503 = 0;
			return;
		}
		if (entity.anim != -1 && entity.anInt1529 == 0) {
			RS2Sharp.Animation animation = RS2Sharp.Animation.anims[entity.anim];
			if (entity.anInt1542 > 0 && animation.anInt363 == 0) {
				entity.anInt1503++;
				return;
			}
			if (entity.anInt1542 <= 0 && animation.anInt364 == 0) {
				entity.anInt1503++;
				return;
			}
		}
		int i = entity.x;
		int j = entity.y;
		int k = entity.smallX[entity.smallXYIndex - 1] * 128 + entity.anInt1540
			* 64;
		int l = entity.smallY[entity.smallXYIndex - 1] * 128 + entity.anInt1540
			* 64;
		if (k - i > 256 || k - i < -256 || l - j > 256 || l - j < -256) {
			entity.x = k;
			entity.y = l;
			return;
		}
		if (i < k) {
			if (j < l)
				entity.turnDirection = 1280;
			else if (j > l)
				entity.turnDirection = 1792;
			else
				entity.turnDirection = 1536;
		} else if (i > k) {
			if (j < l)
				entity.turnDirection = 768;
			else if (j > l)
				entity.turnDirection = 256;
			else
				entity.turnDirection = 512;
		} else if (j < l)
			entity.turnDirection = 1024;
		else
			entity.turnDirection = 0;
		int i1 = entity.turnDirection - entity.anInt1552 & 0x7ff;
		if (i1 > 1024)
			i1 -= 2048;
		int j1 = entity.anInt1555;
		if (i1 >= -256 && i1 <= 256)
			j1 = entity.anInt1554;
		else if (i1 >= 256 && i1 < 768)
			j1 = entity.anInt1557;
		else if (i1 >= -768 && i1 <= -256)
			j1 = entity.anInt1556;
		if (j1 == -1)
			j1 = entity.anInt1554;
		entity.anInt1517 = j1;
		int k1 = 4;
		if (entity.anInt1552 != entity.turnDirection
		    && entity.interactingEntity == -1 && entity.anInt1504 != 0)
			k1 = 2;
		if (entity.smallXYIndex > 2)
			k1 = 6;
		if (entity.smallXYIndex > 3)
			k1 = 8;
		if (entity.anInt1503 > 0 && entity.smallXYIndex > 1) {
			k1 = 8;
			entity.anInt1503--;
		}
		if (entity.aBooleanArray1553[entity.smallXYIndex - 1])
			k1 <<= 1;
		if (k1 >= 8 && entity.anInt1517 == entity.anInt1554
		    && entity.anInt1505 != -1)
			entity.anInt1517 = entity.anInt1505;
		if (i < k) {
			entity.x += k1;
			if (entity.x > k)
				entity.x = k;
		} else if (i > k) {
			entity.x -= k1;
			if (entity.x < k)
				entity.x = k;
		}
		if (j < l) {
			entity.y += k1;
			if (entity.y > l)
				entity.y = l;
		} else if (j > l) {
			entity.y -= k1;
			if (entity.y < l)
				entity.y = l;
		}
		if (entity.x == k && entity.y == l) {
			entity.smallXYIndex--;
			if (entity.anInt1542 > 0)
				entity.anInt1542--;
		}
	}
	
	private void method100(Entity entity) {
		if (entity.anInt1504 == 0)
			return;
		if (entity.interactingEntity != -1 && entity.interactingEntity < 32768) {
			NPC npc = npcArray[entity.interactingEntity];
			if (npc != null) {
				int i1 = entity.x - npc.x;
				int k1 = entity.y - npc.y;
				if (i1 != 0 || k1 != 0)
					entity.turnDirection = (int) (Mathf.Atan2(i1, k1) * 325.94900000000001D) & 0x7ff;
			}
		}
		if (entity.interactingEntity >= 32768) {
			int j = entity.interactingEntity - 32768;
			if (j == localPlayerIndex)
				j = myPlayerIndex;
			Player player = playerArray[j];
			if (player != null) {
				int l1 = entity.x - player.x;
				int i2 = entity.y - player.y;
				if (l1 != 0 || i2 != 0)
					entity.turnDirection = (int) (Mathf.Atan2(l1, i2) * 325.94900000000001D) & 0x7ff;
			}
		}
		if ((entity.anInt1538 != 0 || entity.anInt1539 != 0)
		    && (entity.smallXYIndex == 0 || entity.anInt1503 > 0)) {
			int k = entity.x - (entity.anInt1538 - baseX - baseX) * 64;
			int j1 = entity.y - (entity.anInt1539 - baseY - baseY) * 64;
			if (k != 0 || j1 != 0)
				entity.turnDirection = (int) (Mathf.Atan2(k, j1) * 325.94900000000001D) & 0x7ff;
			entity.anInt1538 = 0;
			entity.anInt1539 = 0;
		}
		int l = entity.turnDirection - entity.anInt1552 & 0x7ff;
		if (l != 0) {
			if (l < entity.anInt1504 || l > 2048 - entity.anInt1504)
				entity.anInt1552 = entity.turnDirection;
			else if (l > 1024)
				entity.anInt1552 -= entity.anInt1504;
			else
				entity.anInt1552 += entity.anInt1504;
			entity.anInt1552 &= 0x7ff;
			if (entity.anInt1517 == entity.anInt1511
			    && entity.anInt1552 != entity.turnDirection) {
				if (entity.anInt1512 != -1) {
					entity.anInt1517 = entity.anInt1512;
					return;
				}
				entity.anInt1517 = entity.anInt1554;
			}
		}
	}
	public void method101(Entity entity) {
		entity.aBoolean1541 = false;
		if (entity.anInt1517 != -1) {
			if (entity.anInt1517 > RS2Sharp.Animation.anims.Length)
				entity.anInt1517 = 0;
			RS2Sharp.Animation animation = RS2Sharp.Animation.anims[entity.anInt1517];
			entity.anInt1519++;
			if (entity.anInt1518 < animation.frameCount && entity.anInt1519 > animation.duration(entity.anInt1518)) {
				entity.anInt1519 = 1;//this is the frame delay. 0 is what it's normally at. higher number = faster animations.
				entity.anInt1518++;
				entity.nextIdleAnimationFrame++;
			}
			entity.nextIdleAnimationFrame = entity.anInt1518 + 1;
			if(entity.nextIdleAnimationFrame >= animation.frameCount) {
				if (entity.nextIdleAnimationFrame >= animation.frameCount)
					entity.nextIdleAnimationFrame = 0;
			}
			if (entity.anInt1518 >= animation.frameCount) {
				entity.anInt1519 = 1;
				entity.anInt1518 = 0;
			}
		}
		if (entity.anInt1520 != -1 && loopCycle >= entity.anInt1523) {
			if (entity.anInt1521 < 0)
				entity.anInt1521 = 0;
			RS2Sharp.Animation animation_1 = SpotAnim.cache[entity.anInt1520].aAnimation_407;
			for (entity.anInt1522++; entity.anInt1521 < animation_1.frameCount
			     && entity.anInt1522 > animation_1
			     .duration(entity.anInt1521); entity.anInt1521++)
				entity.anInt1522 -= animation_1.duration(entity.anInt1521);
			
			if (entity.anInt1521 >= animation_1.frameCount
			    && (entity.anInt1521 < 0 || entity.anInt1521 >= animation_1.frameCount))
				entity.anInt1520 = -1;
			entity.nextGraphicsAnimationFrame = entity.anInt1521 + 1;
			if(entity.nextGraphicsAnimationFrame >= animation_1.frameCount) {
				if (entity.nextGraphicsAnimationFrame < 0
				    || entity.nextGraphicsAnimationFrame >= animation_1.frameCount)
					entity.anInt1520 = -1;
			}
		}
		if (entity.anim != -1 && entity.anInt1529 <= 1) {
			RS2Sharp.Animation animation_2 = RS2Sharp.Animation.anims[entity.anim];
			if (animation_2.anInt363 == 1 && entity.anInt1542 > 0
			    && entity.anInt1547 <= loopCycle
			    && entity.anInt1548 < loopCycle) {
				entity.anInt1529 = 1;
				return;
			}
		}
		if (entity.anim != -1 && entity.anInt1529 == 0) {
			RS2Sharp.Animation animation_3 = RS2Sharp.Animation.anims[entity.anim];
			for (entity.anInt1528++; entity.anInt1527 < animation_3.frameCount
			     && entity.anInt1528 > animation_3
			     .duration(entity.anInt1527); entity.anInt1527++)
				entity.anInt1528 -= animation_3.duration(entity.anInt1527);
			
			if (entity.anInt1527 >= animation_3.frameCount) {
				entity.anInt1527 -= animation_3.anInt356;
				entity.anInt1530++;
				if (entity.anInt1530 >= animation_3.anInt362)
					entity.anim = -1;
				if (entity.anInt1527 < 0
				    || entity.anInt1527 >= animation_3.frameCount)
					entity.anim = -1;
			}
			entity.nextAnimationFrame = entity.anInt1527 + 1;
			if(entity.nextAnimationFrame >= animation_3.frameCount) {
				if (entity.anInt1530 >= animation_3.anInt362)
					entity.nextAnimationFrame = entity.anInt1527 + 1;
				if (entity.nextAnimationFrame < 0
				    || entity.nextAnimationFrame >= animation_3.frameCount)
					entity.nextAnimationFrame = entity.anInt1527;
			}
			entity.aBoolean1541 = animation_3.aBoolean358;
		}
		if (entity.anInt1529 > 0)
			entity.anInt1529--;
	}
	
	
	public int logIconHPos = 0;

//	private bool roofsOff = true;
//	{
//		if (base.mouseX >= 743 && base.mouseX <= 765 && base.mouseY >= 0
//		    && base.mouseY <= 16) {
//			logIconHPos = 1;
//		} else {
//			logIconHPos = 0;
//		}
//	}
	
	public void drawLogoutButton() {
//		if (tabInterfaceIDs[10] != -1 && anInt1054 == 10)
//			if (loopCycle % 20 >= 10)
//				;
		logIconN.drawSprite(225, 0);
		if (logIconHPos == 1) {
			logIconC.drawSprite(225, 0);
			if (tabID == 10)
				logIconH.drawSprite(225, 0);
		}
	}
	
	private void drawGameScreen() {
		if (fullscreenInterfaceID != -1
		    && (loadingStage == 2 || base.fullGameScreen != null)) {
			if (loadingStage == 2) {
				method119(anInt945, fullscreenInterfaceID);
				if (openInterfaceID != -1) {
					method119(anInt945, openInterfaceID);
				}
				anInt945 = 0;
				resetAllImageProducers();
				base.fullGameScreen.initDrawingArea();
				RS2Sharp.Texture.lineOffsets = fullScreenTextureArray;
				DrawingArea.setAllPixelsToZero();
				welcomeScreenRaised = true;
				if (openInterfaceID != -1) {
					RSInterface rsInterface_1 = RSInterface.interfaceCache[openInterfaceID];
					if (rsInterface_1.width == 512
					    && rsInterface_1.height == 334
					    && rsInterface_1.type == 0) {
						rsInterface_1.width = 765;
						rsInterface_1.height = 503;
					}
					drawInterface(0, 0, rsInterface_1, 8);
				}
				RSInterface rsInterface = RSInterface.interfaceCache[fullscreenInterfaceID];
				if (rsInterface.width == 512 && rsInterface.height == 334
				    && rsInterface.type == 0) {
					rsInterface.width = 765;
					rsInterface.height = 503;
				}
				drawInterface(0, 0, rsInterface, 8);
				
// 				if (!menuOpen) {
// 					processRightClick();
// 					drawTooltip();
// 				} else {
// 					drawMenu();
// 				}
			}
			drawCount++;
			base.fullGameScreen.drawGraphics(0, base.graphics, 0);
			return;
		} else {
			if (drawCount != 0) {
				resetImageProducers2();
			}
		}
		if (welcomeScreenRaised) {
// 			welcomeScreenRaised = false;
// 			topFrame.drawGraphics(0, base.graphics, 0);
// 			leftFrame.drawGraphics(4, base.graphics, 0);
// 			rightFrame.drawGraphics(4, base.graphics, 516);
// 			mapEdgeIP.drawGraphics(4, base.graphics, 519);
// 			needDrawTabArea = true;
// 			inputTaken = true;
// 			tabAreaAltered = true;
// 			aBoolean1233 = true;
// 			if (loadingStage != 2) {
// 				aRSImageProducer_1165.drawGraphics(4, base.graphics, 4);
// 				aRSImageProducer_1164.drawGraphics(0, base.graphics, 519);
// 			}
welcomeScreenRaised = false;
            inputTaken = true;
            tabAreaAltered = true;
                if(loadingStage != 2) {
                    aRSImageProducer_1165.drawGraphics(0, base.graphics, 0);
                }
		}
		if (!menuOpen) {
			drawTooltip();
			processRightClick();
		} else {
			drawMenu(0, 0);
		}
		if (menuOpen && menuScreenArea == 1)
			needDrawTabArea = true;
		if (invOverlayInterfaceID != -1) {
			bool flag1 = method119(anInt945, invOverlayInterfaceID);
			if (flag1)
				needDrawTabArea = true;
		}
		if (atInventoryInterfaceType == 2)
			needDrawTabArea = true;
		if (activeInterfaceType == 2)
			needDrawTabArea = true;
		if (needDrawTabArea) {
			drawTabArea();
			needDrawTabArea = false;
		}
// 		if (backDialogID == -1) {
// 			aClass9_1059.scrollPosition = anInt1211 - anInt1089 - 110;
// 			if (base.mouseX > 478 && base.mouseX < 580 && base.mouseY > 342)
// 				method65(494, 110, base.mouseX - 0, base.mouseY - 348,
// 				         aClass9_1059, 0, false, anInt1211);
// 			int i = anInt1211 - 110 - aClass9_1059.scrollPosition;
// 			if (i < 0)
// 				i = 0;
// 			if (i > anInt1211 - 110)
// 				i = anInt1211 - 110;
// 			if (anInt1089 != i) {
// 				anInt1089 = i;
// 				inputTaken = true;
// 			}
// 		}
if(backDialogID == -1) {
			aClass9_1059.scrollPosition = anInt1211 - anInt1089 - 110;
			if(base.mouseX >= 496 && base.mouseX <= 511 && base.mouseY > (myHeight - 158))
				method65(494, 110, base.mouseX, base.mouseY - (myHeight - 158), aClass9_1059, 0, false, anInt1211);
			int i = anInt1211 - 110 - aClass9_1059.scrollPosition;
			if(i < 0)
				i = 0;
			if(i > anInt1211 - 110)
				i = anInt1211 - 110;
			if(anInt1089 != i)
			{
				anInt1089 = i;
				inputTaken = true;
			}
		}
		if (backDialogID != -1) {
			bool flag2 = method119(anInt945, backDialogID);
			if (flag2)
				inputTaken = true;
		}
		if (atInventoryInterfaceType == 3)
			inputTaken = true;
		if (activeInterfaceType == 3)
			inputTaken = true;
		if (aString844 != null)
			inputTaken = true;
		if (menuOpen && menuScreenArea == 2)
			inputTaken = true;
		if (inputTaken) {
			drawChatArea();
			inputTaken = false;
		}
		if (loadingStage == 2)
			method146();
		//if (loadingStage == 2) {
		//	drawMinimap();
		//	aRSImageProducer_1164.drawGraphics(0, base.graphics, 519);
		//}
		if (anInt1054 != -1)
			tabAreaAltered = true;
		if (tabAreaAltered) {
			if (anInt1054 != -1 && anInt1054 == tabID) {
				anInt1054 = -1;
				stream.createFrame(120);
				stream.writeWordBigEndian(tabID);
			}
			tabAreaAltered = false;
			aRSImageProducer_1125.initDrawingArea();
			aRSImageProducer_1165.initDrawingArea();
		}
		anInt945 = 0;
	}
	
	private bool buildFriendsListMenu(RSInterface class9) {
		int i = class9.anInt214;
		if (i >= 1 && i <= 200 || i >= 701 && i <= 900) {
			if (i >= 801)
				i -= 701;
			else if (i >= 701)
				i -= 601;
			else if (i >= 101)
				i -= 101;
			else
				i--;
			menuActionName[menuActionRow] = "Remove @whi@" + friendsList[i];
			menuActionID[menuActionRow] = 792;
			menuActionRow++;
			menuActionName[menuActionRow] = "Message @whi@" + friendsList[i];
			menuActionID[menuActionRow] = 639;
			menuActionRow++;
			return true;
		}
		if (i >= 401 && i <= 500) {
			menuActionName[menuActionRow] = "Remove @whi@" + class9.message;
			menuActionID[menuActionRow] = 322;
			menuActionRow++;
			return true;
		} else {
			return false;
		}
	}
	
	private void method104() {
		Animable_Sub3 class30_sub2_sub4_sub3 = (Animable_Sub3) aClass19_1056
			.reverseGetFirst();
		for (; class30_sub2_sub4_sub3 != null; class30_sub2_sub4_sub3 = (Animable_Sub3) aClass19_1056
		     .reverseGetNext())
			if (class30_sub2_sub4_sub3.anInt1560 != plane
			    || class30_sub2_sub4_sub3.aBoolean1567)
				class30_sub2_sub4_sub3.unlink();
		else if (loopCycle >= class30_sub2_sub4_sub3.anInt1564) {
			class30_sub2_sub4_sub3.method454(anInt945);
			if (class30_sub2_sub4_sub3.aBoolean1567)
				class30_sub2_sub4_sub3.unlink();
			//else
				//worldController.addEntity(class30_sub2_sub4_sub3.anInt1560,
				                          //0, class30_sub2_sub4_sub3.anInt1563, -1,
				                         // class30_sub2_sub4_sub3.anInt1562, 60,
//				                          class30_sub2_sub4_sub3.anInt1561,
//				                          class30_sub2_sub4_sub3, false);
		}
		
	}
	
	private void drawInterface(int j, int k, RSInterface class9, int l) {
		if (class9 == null || class9.type != 0 || class9.children == null)
			return;
		if (class9.aBoolean266 && anInt1026 != class9.id
		    && anInt1048 != class9.id && anInt1039 != class9.id)
			return;
		int i1 = DrawingArea.topX;
		int j1 = DrawingArea.topY;
		int k1 = DrawingArea.bottomX;
		int l1 = DrawingArea.bottomY;
		DrawingArea.setDrawingArea(l + class9.height, k, k + class9.width, l);
		int i2 = class9.children.Length;
		for (int j2 = 0; j2 < i2; j2++) {
			int k2 = class9.childX[j2] + k;
			int l2 = (class9.childY[j2] + l) - j;
			RSInterface class9_1 = RSInterface.interfaceCache[class9.children[j2]];
			k2 += class9_1.anInt263;
			l2 += class9_1.anInt265;
			if (class9_1.anInt214 > 0)
				drawFriendsListOrWelcomeScreen(class9_1);
			if (class9_1.type == 0) {
				if (class9_1.scrollPosition > class9_1.scrollMax
				    - class9_1.height)
					class9_1.scrollPosition = class9_1.scrollMax
						- class9_1.height;
				if (class9_1.scrollPosition < 0)
					class9_1.scrollPosition = 0;
				drawInterface(class9_1.scrollPosition, k2, class9_1, l2);
				if (class9_1.scrollMax > class9_1.height)
					drawScrollbar(class9_1.height, class9_1.scrollPosition, l2,
					              k2 + class9_1.width, class9_1.scrollMax);
			} else if (class9_1.type != 1)
			if (class9_1.type == 2) {
				int i3 = 0;
				for (int l3 = 0; l3 < class9_1.height; l3++) {
					for (int l4 = 0; l4 < class9_1.width; l4++) {
						int k5 = k2 + l4 * (32 + class9_1.invSpritePadX);
						int j6 = l2 + l3 * (32 + class9_1.invSpritePadY);
						if (i3 < 20) {
							k5 += class9_1.spritesX[i3];
							j6 += class9_1.spritesY[i3];
						}
						if (class9_1.inv[i3] > 0) {
							int k6 = 0;
							int j7 = 0;
							int j9 = class9_1.inv[i3] - 1;
							if (k5 > DrawingArea.topX - 32
							    && k5 < DrawingArea.bottomX
							    && j6 > DrawingArea.topY - 32
							    && j6 < DrawingArea.bottomY
							    || activeInterfaceType != 0
							    && anInt1085 == i3) {
								int l9 = 0;
								if (itemSelected == 1 && anInt1283 == i3
								    && anInt1284 == class9_1.id)
									l9 = 0xffffff;
								RS2Sharp.Sprite class30_sub2_sub1_sub1_2 = ItemDef
									.getSprite(j9,
									           class9_1.invStackSizes[i3],
									           l9);
								if (class30_sub2_sub1_sub1_2 != null) {
									if (activeInterfaceType != 0
									    && anInt1085 == i3
									    && anInt1084 == class9_1.id) {
										k6 = base.mouseX - anInt1087;
										j7 = base.mouseY - anInt1088;
										if (k6 < 5 && k6 > -5)
											k6 = 0;
										if (j7 < 5 && j7 > -5)
											j7 = 0;
										if (anInt989 < 5) {
											k6 = 0;
											j7 = 0;
										}
										class30_sub2_sub1_sub1_2
											.drawSprite1(k5 + k6, j6
											             + j7);
										if (j6 + j7 < DrawingArea.topY
										    && class9.scrollPosition > 0) {
											int i10 = (anInt945 * (DrawingArea.topY
											                       - j6 - j7)) / 3;
											if (i10 > anInt945 * 10)
												i10 = anInt945 * 10;
											if (i10 > class9.scrollPosition)
												i10 = class9.scrollPosition;
											class9.scrollPosition -= i10;
											anInt1088 += i10;
										}
										if (j6 + j7 + 32 > DrawingArea.bottomY
										    && class9.scrollPosition < class9.scrollMax
										    - class9.height) {
											int j10 = (anInt945 * ((j6 + j7 + 32) - DrawingArea.bottomY)) / 3;
											if (j10 > anInt945 * 10)
												j10 = anInt945 * 10;
											if (j10 > class9.scrollMax
											    - class9.height
											    - class9.scrollPosition)
												j10 = class9.scrollMax
													- class9.height
													- class9.scrollPosition;
											class9.scrollPosition += j10;
											anInt1088 -= j10;
										}
									} else if (atInventoryInterfaceType != 0
									           && atInventoryIndex == i3
									           && atInventoryInterface == class9_1.id)
										class30_sub2_sub1_sub1_2
											.drawSprite1(k5, j6);
									else
										class30_sub2_sub1_sub1_2
											.drawSprite(k5, j6);
									if (class30_sub2_sub1_sub1_2.anInt1444 == 33
									    || class9_1.invStackSizes[i3] != 1) {
										int k10 = class9_1.invStackSizes[i3];
										smallText.method385(0,
										                    intToKOrMil(k10), j6 + 10
										                    + j7, k5 + 1 + k6);
										smallText.method385(0xffff00,
										                    intToKOrMil(k10), j6 + 9
										                    + j7, k5 + k6);
									}
								}
							}
						} else if (class9_1.sprites != null && i3 < 20) {
							RS2Sharp.Sprite class30_sub2_sub1_sub1_1 = class9_1.sprites[i3];
							if (class30_sub2_sub1_sub1_1 != null)
								class30_sub2_sub1_sub1_1.drawSprite(k5, j6);
						}
						i3++;
					}
				}
			} else if (class9_1.type == 3) {
				bool flag = false;
				if (anInt1039 == class9_1.id || anInt1048 == class9_1.id
				    || anInt1026 == class9_1.id)
					flag = true;
				int j3;
				if (interfaceIsSelected(class9_1)) {
					j3 = class9_1.anInt219;
					if (flag && class9_1.anInt239 != 0)
						j3 = class9_1.anInt239;
				} else {
					j3 = class9_1.textColor;
					if (flag && class9_1.anInt216 != 0)
						j3 = class9_1.anInt216;
				}
				if (class9_1.aByte254 == 0) {
					if (class9_1.aBoolean227)
						DrawingArea.drawPixels(class9_1.height, l2, k2, j3,
						                       class9_1.width);
					else
						DrawingArea.fillPixels(k2, class9_1.width,
						                       class9_1.height, j3, l2);
				} else if (class9_1.aBoolean227)
					DrawingArea.method335(j3, l2, class9_1.width,
					                      class9_1.height,
					                      256 - (class9_1.aByte254 & 0xff), k2);
				else
					DrawingArea.method338(l2, class9_1.height,
					                      256 - (class9_1.aByte254 & 0xff), j3,
					                      class9_1.width, k2);
			} else if (class9_1.type == 4) {
				TextDrawingArea textDrawingArea = class9_1.textDrawingAreas;
				String s = class9_1.message;
				bool flag1 = false;
				if (anInt1039 == class9_1.id || anInt1048 == class9_1.id
				    || anInt1026 == class9_1.id)
					flag1 = true;
				int i4;
				if (interfaceIsSelected(class9_1)) {
					i4 = class9_1.anInt219;
					if (flag1 && class9_1.anInt239 != 0)
						i4 = class9_1.anInt239;
					if (class9_1.aString228.Length > 0)
						s = class9_1.aString228;
				} else {
					i4 = class9_1.textColor;
					if (flag1 && class9_1.anInt216 != 0)
						i4 = class9_1.anInt216;
				}
				if (class9_1.atActionType == 6 && aBoolean1149) {
					s = "Please wait...";
					i4 = class9_1.textColor;
				}
				if (DrawingArea.width == 519) {
					if (i4 == 0xffff00)
						i4 = 255;
					if (i4 == 49152)
						i4 = 0xffffff;
				}
				for (int l6 = l2 + textDrawingArea.anInt1497; s.Length > 0; l6 += textDrawingArea.anInt1497) {
					if (s.IndexOf("%") != -1) {
						do {
							int k7 = s.IndexOf("%1");
							if (k7 == -1)
								break;
							s = s.Substring(0, k7)
								+ interfaceIntToString(extractInterfaceValues(
									class9_1, 0))
								+ s.Substring(k7 + 2);
						} while (true);
						do {
							int l7 = s.IndexOf("%2");
							if (l7 == -1)
								break;
							s = s.Substring(0, l7)
								+ interfaceIntToString(extractInterfaceValues(
									class9_1, 1))
								+ s.Substring(l7 + 2);
						} while (true);
						do {
							int i8 = s.IndexOf("%3");
							if (i8 == -1)
								break;
							s = s.Substring(0, i8)
								+ interfaceIntToString(extractInterfaceValues(
									class9_1, 2))
								+ s.Substring(i8 + 2);
						} while (true);
						do {
							int j8 = s.IndexOf("%4");
							if (j8 == -1)
								break;
							s = s.Substring(0, j8)
								+ interfaceIntToString(extractInterfaceValues(
									class9_1, 3))
								+ s.Substring(j8 + 2);
						} while (true);
						do {
							int k8 = s.IndexOf("%5");
							if (k8 == -1)
								break;
							s = s.Substring(0, k8)
								+ interfaceIntToString(extractInterfaceValues(
									class9_1, 4))
								+ s.Substring(k8 + 2);
						} while (true);
					}
					int l8 = s.IndexOf("\\n");
					String s1;
					if (l8 != -1) {
						s1 = s.Substring(0, l8);
						s = s.Substring(l8 + 2);
					} else {
						s1 = s;
						s = "";
					}
					if (class9_1.aBoolean223)
						textDrawingArea.drawChatInput(i4, k2 + class9_1.width
						                          / 2, s1, l6, class9_1.aBoolean268);
					else
						textDrawingArea.method389(class9_1.aBoolean268, k2,
						                          i4, s1, l6);
				}
			} else if (class9_1.type == 5) {
				RS2Sharp.Sprite sprite;
				if (interfaceIsSelected(class9_1))
					sprite = class9_1.sprite2;
				else
					sprite = class9_1.sprite1;
				if (sprite != null)
					//if (class9_1.drawsTransparent == true)
						sprite.drawSprite(k2, l2);
					//else
					//	sprite.drawSprite(k2, l2);
			} else if (class9_1.type == 6) {
				int k3 = RS2Sharp.Texture.center_x;
				int j4 = RS2Sharp.Texture.center_y;
				RS2Sharp.Texture.center_x = k2 + class9_1.width / 2;
				RS2Sharp.Texture.center_y = l2 + class9_1.height / 2;
				int i5 = RS2Sharp.Texture.SINE[class9_1.anInt270]
				* class9_1.anInt269 >> 16;
				int l5 = RS2Sharp.Texture.COSINE[class9_1.anInt270]
				* class9_1.anInt269 >> 16;
				bool flag2 = interfaceIsSelected(class9_1);
				int i7;
				if (flag2)
					i7 = class9_1.anInt258;
				else
					i7 = class9_1.anInt257;
				Model model;
				if (i7 == -1) {
					model = class9_1.method209(-1, -1, flag2);
				} else {
					RS2Sharp.Animation animation = RS2Sharp.Animation.anims[i7];
					model = class9_1.method209(
						animation.anIntArray354[class9_1.anInt246],
						animation.anIntArray353[class9_1.anInt246],
						flag2);
				}
				if (model != null)
					model.method482(class9_1.anInt271, 0,
					                class9_1.anInt270, 0, i5, l5);
				RS2Sharp.Texture.center_x = k3;
				RS2Sharp.Texture.center_y = j4;
			} else if (class9_1.type == 7) {
				TextDrawingArea textDrawingArea_1 = class9_1.textDrawingAreas;
				int k4 = 0;
				for (int j5 = 0; j5 < class9_1.height; j5++) {
					for (int i6 = 0; i6 < class9_1.width; i6++) {
						if (class9_1.inv[k4] > 0) {
							ItemDef itemDef = ItemDef
								.forID(class9_1.inv[k4] - 1);
							String s2 = itemDef.name;
							if (itemDef.stackable
							    || class9_1.invStackSizes[k4] != 1)
								s2 = s2
									+ " x"
									+ intToKOrMilLongName(class9_1.invStackSizes[k4]);
							int i9 = k2 + i6
								* (115 + class9_1.invSpritePadX);
							int k9 = l2 + j5
								* (12 + class9_1.invSpritePadY);
							if (class9_1.aBoolean223)
								textDrawingArea_1.drawChatInput(
									class9_1.textColor, i9
									+ class9_1.width / 2, s2,
									k9, class9_1.aBoolean268);
							else
								textDrawingArea_1.method389(
									class9_1.aBoolean268, i9,
									class9_1.textColor, s2, k9);
						}
						k4++;
					}
				}
			}
			else if (class9_1.type == 8
			         && (anInt1500 == class9_1.id
			    || anInt1044 == class9_1.id || anInt1129 == class9_1.id)
			         && anInt1501 == 50 && !menuOpen) {
				int boxWidth = 0;
				int boxHeight = 0;
				TextDrawingArea textDrawingArea_2 = aTextDrawingArea_1271;
				for (String s1 = class9_1.message; s1.Length > 0;) {
					if (s1.IndexOf("%") != -1) {
						do {
							int k7 = s1.IndexOf("%1");
							if (k7 == -1)
								break;
							s1 = s1.Substring(0, k7)
								+ interfaceIntToString(extractInterfaceValues(
									class9_1, 0))
								+ s1.Substring(k7 + 2);
						} while (true);
						do {
							int l7 = s1.IndexOf("%2");
							if (l7 == -1)
								break;
							s1 = s1.Substring(0, l7)
								+ interfaceIntToString(extractInterfaceValues(
									class9_1, 1))
								+ s1.Substring(l7 + 2);
						} while (true);
						do {
							int i8 = s1.IndexOf("%3");
							if (i8 == -1)
								break;
							s1 = s1.Substring(0, i8)
								+ interfaceIntToString(extractInterfaceValues(
									class9_1, 2))
								+ s1.Substring(i8 + 2);
						} while (true);
						do {
							int j8 = s1.IndexOf("%4");
							if (j8 == -1)
								break;
							s1 = s1.Substring(0, j8)
								+ interfaceIntToString(extractInterfaceValues(
									class9_1, 3))
								+ s1.Substring(j8 + 2);
						} while (true);
						do {
							int k8 = s1.IndexOf("%5");
							if (k8 == -1)
								break;
							s1 = s1.Substring(0, k8)
								+ interfaceIntToString(extractInterfaceValues(
									class9_1, 4))
								+ s1.Substring(k8 + 2);
						} while (true);
					}
					int l77 = s1.IndexOf("\\n");
					String s4;
					if (l77 != -1) {
						s4 = s1.Substring(0, l77);
						s1 = s1.Substring(l77 + 2);
					} else {
						s4 = s1;
						s1 = "";
					}
					int j10 = textDrawingArea_2.getTextWidth(s4);
					if (j10 > boxWidth) {
						boxWidth = j10;
					}
					boxHeight += textDrawingArea_2.anInt1497 + 1;
				}
				boxWidth += 6;
				boxHeight += 7;
				int xPos = (k2 + class9_1.width) - 5 - boxWidth;
				int yPos = l2 + class9_1.height + 5;
				if (xPos < k2 + 5)
					xPos = k2 + 5;
				if (xPos + boxWidth > k + class9.width)
					xPos = (k + class9.width) - boxWidth;
				if (yPos + boxHeight > l + class9.height)
					yPos = (l2 - boxHeight);
				DrawingArea.drawPixels(boxHeight, yPos, xPos, 0xFFFFA0,
				                       boxWidth);
				DrawingArea.fillPixels(xPos, boxWidth, boxHeight, 0, yPos);
				String s2 = class9_1.message;
				for (int j11 = yPos + textDrawingArea_2.anInt1497 + 2; s2
				     .Length > 0; j11 += textDrawingArea_2.anInt1497 + 1) {// anInt1497
					if (s2.IndexOf("%") != -1) {
						do {
							int k7 = s2.IndexOf("%1");
							if (k7 == -1)
								break;
							s2 = s2.Substring(0, k7)
								+ interfaceIntToString(extractInterfaceValues(
									class9_1, 0))
								+ s2.Substring(k7 + 2);
						} while (true);
						do {
							int l7 = s2.IndexOf("%2");
							if (l7 == -1)
								break;
							s2 = s2.Substring(0, l7)
								+ interfaceIntToString(extractInterfaceValues(
									class9_1, 1))
								+ s2.Substring(l7 + 2);
						} while (true);
						do {
							int i8 = s2.IndexOf("%3");
							if (i8 == -1)
								break;
							s2 = s2.Substring(0, i8)
								+ interfaceIntToString(extractInterfaceValues(
									class9_1, 2))
								+ s2.Substring(i8 + 2);
						} while (true);
						do {
							int j8 = s2.IndexOf("%4");
							if (j8 == -1)
								break;
							s2 = s2.Substring(0, j8)
								+ interfaceIntToString(extractInterfaceValues(
									class9_1, 3))
								+ s2.Substring(j8 + 2);
						} while (true);
						do {
							int k8 = s2.IndexOf("%5");
							if (k8 == -1)
								break;
							s2 = s2.Substring(0, k8)
								+ interfaceIntToString(extractInterfaceValues(
									class9_1, 4))
								+ s2.Substring(k8 + 2);
						} while (true);
					}
					int l11 = s2.IndexOf("\\n");
					String s5;
					if (l11 != -1) {
						s5 = s2.Substring(0, l11);
						s2 = s2.Substring(l11 + 2);
					} else {
						s5 = s2;
						s2 = "";
					}
					if (class9_1.aBoolean223) {
						textDrawingArea_2.drawChatInput(yPos, xPos
						                            + class9_1.width / 2, s5, j11, false);
					} else {
						if (s5.Contains("\\r")) {
							String text = s5
								.Substring(0, s5.IndexOf("\\r"));
							String text2 = s5
								.Substring(s5.IndexOf("\\r") + 2);
							textDrawingArea_2.method389(false, xPos + 3, 0,
							                            text, j11);
							int rightX = boxWidth + xPos
								- textDrawingArea_2.getTextWidth(text2)
									- 2;
							textDrawingArea_2.method389(false, rightX, 0,
							                            text2, j11);
							UnityEngine.Debug.Log("Box: " + boxWidth + "");
						} else
							textDrawingArea_2.method389(false, xPos + 3, 0,
							                            s5, j11);
					}
				}
			}
		}
		DrawingArea.setDrawingArea(l1, i1, k1, j1);
	}
	
	
	private void randomizeBackground(Background background) {
//		int j = 256;
//		for (int k = 0; k < anIntArray1190.Length; k++)
//			anIntArray1190[k] = 0;
//		
//		for (int l = 0; l < 5000; l++) {
//			int i1 = (int) (RS2Sharp.Random.Next() * 128D * j);
//			anIntArray1190[i1] = (int) (RS2Sharp.Random.Next() * 256D);
//		}
//		for (int j1 = 0; j1 < 20; j1++) {
//			for (int k1 = 1; k1 < j - 1; k1++) {
//				for (int i2 = 1; i2 < 127; i2++) {
//					int k2 = i2 + (k1 << 7);
//					anIntArray1191[k2] = (anIntArray1190[k2 - 1]
//					                      + anIntArray1190[k2 + 1] + anIntArray1190[k2 - 128] + anIntArray1190[k2 + 128]) / 4;
//				}
//				
//			}
//			int[] ai = anIntArray1190;
//			anIntArray1190 = anIntArray1191;
//			anIntArray1191 = ai;
//		}
//		if (background != null) {
//			int l1 = 0;
//			for (int j2 = 0; j2 < background.anInt1453; j2++) {
//				for (int l2 = 0; l2 < background.anInt1452; l2++)
//				if (background.aByteArray1450[l1++] != 0) {
//					int i3 = l2 + 16 + background.anInt1454;
//					int j3 = j2 + 16 + background.anInt1455;
//					int k3 = i3 + (j3 << 7);
//					anIntArray1190[k3] = 0;
//				}
//			}
//		}
	}
	
	private void method107(int i, int j, RS2Sharp.Stream stream, Player player) {
		if ((i & 0x400) != 0) {
			player.anInt1543 = stream.method428();
			player.anInt1545 = stream.method428();
			player.anInt1544 = stream.method428();
			player.anInt1546 = stream.method428();
			player.anInt1547 = stream.method436() + loopCycle;
			player.anInt1548 = stream.method435() + loopCycle;
			player.anInt1549 = stream.method428();
			player.method446();
		}
		if ((i & 0x100) != 0) {
			player.anInt1520 = stream.method434();
			int k = stream.readDWord();
			player.anInt1524 = k >> 16;
			player.anInt1523 = loopCycle + (k & 0xffff);
			player.anInt1521 = 0;
			player.anInt1522 = 0;
			if (player.anInt1523 > loopCycle)
				player.anInt1521 = -1;
			if (player.anInt1520 == 65535)
				player.anInt1520 = -1;
		}
		if ((i & 8) != 0) {
			int l = stream.method434();
			if (l == 65535)
				l = -1;
			int i2 = stream.method427();
			if (l == player.anim && l != -1) {
				int i3 = RS2Sharp.Animation.anims[l].anInt365;
				if (i3 == 1) {
					player.anInt1527 = 0;
					player.anInt1528 = 0;
					player.anInt1529 = i2;
					player.anInt1530 = 0;
				}
				if (i3 == 2)
					player.anInt1530 = 0;
			} else if (l == -1
			           || player.anim == -1
			           || RS2Sharp.Animation.anims[l].anInt359 >= RS2Sharp.Animation.anims[player.anim].anInt359) {
				player.anim = l;
				player.anInt1527 = 0;
				player.anInt1528 = 0;
				player.anInt1529 = i2;
				player.anInt1530 = 0;
				player.anInt1542 = player.smallXYIndex;
			}
		}
		if ((i & 4) != 0) {
			player.textSpoken = stream.readString();
			if (player.textSpoken[0] == '~') {
				player.textSpoken = player.textSpoken.Substring(1);
				pushMessage(player.textSpoken, 2, player.name);
			} else if (player == myPlayer)
				pushMessage(player.textSpoken, 2, player.name);
			player.anInt1513 = 0;
			player.anInt1531 = 0;
			player.textCycle = 150;
		}
		if ((i & 0x80) != 0) {
			int i1 = stream.method434();
			int j2 = stream.readUnsignedByte();
			int j3 = stream.method427();
			int k3 = stream.currentOffset;
			if (player.name != null && player.visible) {
				long l3 = TextClass.longForName(player.name);
				bool flag = false;
				if (j2 <= 1) {
					for (int i4 = 0; i4 < ignoreCount; i4++) {
						if (ignoreListAsLongs[i4] != l3)
							continue;
						flag = true;
						break;
					}
					
				}
				if (!flag && anInt1251 == 0)
				try {
					aStream_834.currentOffset = 0;
					stream.method442(j3, 0, aStream_834.buffer);
					aStream_834.currentOffset = 0;
					String s = TextInput.method525(j3, aStream_834);
					// s = Censor.doCensor(s);
					player.textSpoken = s;
					player.anInt1513 = i1 >> 8;
					player.privelage = j2;
					player.anInt1531 = i1 & 0xff;
					player.textCycle = 150;
					if (j2 == 2 || j2 == 3)
						pushMessage(s, 1, "@cr2@" + player.name);
					else if (j2 == 1)
						pushMessage(s, 1, "@cr1@" + player.name);
					else
						pushMessage(s, 2, player.name);
				} catch (Exception exception) {
					signlink.reporterror("cde2");
				}
			}
			stream.currentOffset = k3 + j3;
		}
		if ((i & 1) != 0) {
			player.interactingEntity = stream.method434();
			if (player.interactingEntity == 65535)
				player.interactingEntity = -1;
		}
		if ((i & 0x10) != 0) {
			int j1 = stream.method427();
			byte[] abyte0 = new byte[j1];
			RS2Sharp.Stream stream_1 = new RS2Sharp.Stream(abyte0);
			stream.readBytes(j1, 0, abyte0);
			aStreamArray895s[j] = stream_1;
			player.updatePlayer(stream_1);
		}
		if ((i & 2) != 0) {
			player.anInt1538 = stream.method436();
			player.anInt1539 = stream.method434();
		}
		if ((i & 0x20) != 0) {
			int k1 = stream.readUnsignedByte();
			int k2 = stream.method426();
			 int type1 = stream.readUnsignedByte();
			player.updateHitData(k2, k1, type1, loopCycle);
			player.loopCycleStatus = loopCycle + 300;
			player.currentHealth = stream.method427();
			player.maxHealth = stream.readUnsignedByte();
		}
		if ((i & 0x200) != 0) {
			int l1 = stream.readUnsignedByte();
			int l2 = stream.method428();
			 int type2 = stream.readUnsignedByte();
			player.updateHitData(l2, l1, type2, loopCycle);
			player.loopCycleStatus = loopCycle + 300;
			player.currentHealth = stream.readUnsignedByte();
			player.maxHealth = stream.method427();
		}
	}
	
	private void method108() {
		try {
			int j = myPlayer.x + anInt1278;
			int k = myPlayer.y + anInt1131;
			if (anInt1014 - j < -500 || anInt1014 - j > 500
			    || anInt1015 - k < -500 || anInt1015 - k > 500) {
				anInt1014 = j;
				anInt1015 = k;
			}
			if (anInt1014 != j)
				anInt1014 += (j - anInt1014) / 16;
			if (anInt1015 != k)
				anInt1015 += (k - anInt1015) / 16;
			if (base.keyArray[1] == 1)
				anInt1186 += (-24 - anInt1186) / 2;
			else if (base.keyArray[2] == 1)
				anInt1186 += (24 - anInt1186) / 2;
			else
				anInt1186 /= 2;
			if (base.keyArray[3] == 1)
				anInt1187 += (12 - anInt1187) / 2;
			else if (base.keyArray[4] == 1)
				anInt1187 += (-12 - anInt1187) / 2;
			else
				anInt1187 /= 2;
			minimapInt1 = minimapInt1 + anInt1186 / 2 & 0x7ff;
			anInt1184 += anInt1187 / 2;
			if (anInt1184 < 128)
				anInt1184 = 128;
			if (anInt1184 > 383)
				anInt1184 = 383;
			int l = anInt1014 >> 7;
			int i1 = anInt1015 >> 7;
			int j1 = method42(plane, anInt1015, anInt1014);
			int k1 = 0;
			if (l > 3 && i1 > 3 && l < 100 && i1 < 100) {
				for (int l1 = l - 4; l1 <= l + 4; l1++) {
					for (int k2 = i1 - 4; k2 <= i1 + 4; k2++) {
						int l2 = plane;
						if (l2 < 3 && (byteGroundArray[1][l1][k2] & 2) == 2)
							l2++;
						int i3 = j1 - intGroundArray[l2][l1][k2];
						if (i3 > k1)
							k1 = i3;
					}
					
				}
				
			}
			anInt1005++;
			if (anInt1005 > 1512) {
				anInt1005 = 0;
				stream.createFrame(77);
				stream.writeWordBigEndian(0);
				int i2 = stream.currentOffset;
				stream.writeWordBigEndian((int) (RS2Sharp.Random.Next(256)));
				stream.writeWordBigEndian(101);
				stream.writeWordBigEndian(233);
				stream.writeWord(45092);
				if ((int) (RS2Sharp.Random.Next( 2)) == 0)
					stream.writeWord(35784);
				stream.writeWordBigEndian((int) (RS2Sharp.Random.Next( 256)));
				stream.writeWordBigEndian(64);
				stream.writeWordBigEndian(38);
				stream.writeWord((int) (RS2Sharp.Random.Next( 65536)));
				stream.writeWord((int) (RS2Sharp.Random.Next( 65536)));
				stream.writeBytes(stream.currentOffset - i2);
			}
			int j2 = k1 * 192;
			if (j2 > 0x17f00)
				j2 = 0x17f00;
			if (j2 < 32768)
				j2 = 32768;
			if (j2 > anInt984) {
				anInt984 += (j2 - anInt984) / 24;
				return;
			}
			if (j2 < anInt984) {
				anInt984 += (j2 - anInt984) / 80;
			}
		} catch (Exception _ex) {
			signlink.reporterror("glfc_ex " + myPlayer.x + "," + myPlayer.y
			                     + "," + anInt1014 + "," + anInt1015 + "," + anInt1069 + ","
			                     + anInt1070 + "," + baseX + "," + baseY);
			throw new Exception("eek");
		}
	}

	public override void processDrawing() {
		if (rsAlreadyLoaded || loadingError || genericLoadingError) {
			showErrorScreen();
			return;
		}
		anInt1061++;
//		if (!loggedIn)
//			drawLoginScreen(false);
//		else
//			drawGameScreen();
		anInt1213 = 0;
	}
	
	private bool isFriendOrSelf(String s) {
		if (s == null)
			return false;
		for (int i = 0; i < friendsCount; i++)
			if (s.ToLower().Equals(friendsList[i]))
				return true;
		return s.ToLower().Equals(myPlayer.name);
	}
	
	private static String combatDiffColor(int i, int j) {
		int k = i - j;
		if (k < -9)
			return "@red@";
		if (k < -6)
			return "@or3@";
		if (k < -3)
			return "@or2@";
		if (k < 0)
			return "@or1@";
		if (k > 9)
			return "@gre@";
		if (k > 6)
			return "@gr3@";
		if (k > 3)
			return "@gr2@";
		if (k > 0)
			return "@gr1@";
		else
			return "@yel@";
	}
	
	private void setWaveVolume(int i) {
		signlink.wavevol = i;
	}
	
	private void draw3dScreen() {
		drawSplitPrivateChat();
		if (crossType == 1) {
			crosses[crossIndex / 100]
			.drawSprite(crossX - 8 - 4, crossY - 8 - 4);
			anInt1142++;
			if (anInt1142 > 67) {
				anInt1142 = 0;
				stream.createFrame(78);
			}
		}
		if (crossType == 2)
			crosses[4 + crossIndex / 100].drawSprite(crossX - 8 - 4,
			                                         crossY - 8 - 4);
		if (anInt1018 != -1) {
// 			method119(anInt945, anInt1018);
// 			drawInterface(0, 0, RSInterface.interfaceCache[anInt1018], 0);
method119(anInt945, anInt1018);
				if (myWidth >= 900 && myHeight >= 650) {
					drawInterface(0, (myWidth / 2) - 256, RSInterface.interfaceCache[anInt1018], (myHeight / 2) - 167);
				} else {
					drawInterface(0, 0, RSInterface.interfaceCache[anInt1018], 0);
				}
		}
		if (openInterfaceID != -1) {
// 			method119(anInt945, openInterfaceID);
// 			drawInterface(0, 0, RSInterface.interfaceCache[openInterfaceID], 0);
method119(anInt945, openInterfaceID);
				if (myWidth >= 900 && myHeight >= 650) {
					drawInterface(0, (myWidth / 2) - 256, RSInterface.interfaceCache[openInterfaceID],(myHeight / 2) - 167);
				} else {
					drawInterface(0, 0, RSInterface.interfaceCache[openInterfaceID], 0);
				}
		}
		method70();
		if (!menuOpen) {
			drawTooltip();
			processRightClick();
		} else if (menuScreenArea == 0)
			drawMenu(0, 0);
		if (anInt1055 == 1)
			headIcons[1].drawSprite(myWidth - 260, 6);//headIcons[1].drawSprite(472, 296);
		if (fpsOn) {
			char c = '\u01FB';
			int k = 20;
			int i1 = 0xffff00;
			if (base.fps < 15)
				i1 = 0xff0000;
			aTextDrawingArea_1271.method380("Rune Fps:" + base.fps, c, i1, k);
			k += 15;
// 			Runtime runtime = Runtime.getRuntime();
 			int j1 = (int)(System.GC.GetTotalMemory (false)/1024L);//(int) ((runtime.totalMemory() - runtime.freeMemory()) / 1024L);
			i1 = 0xffff00;
			if (j1 > 0x2000000 && lowMem)
				i1 = 0xff0000;
			aTextDrawingArea_1271.method380("Mem:" + j1 + "k", c, 0xffff00, k);
			k += 15;
			float msec = deltaTime * 1000.0f;
			float fps = 1.0f / deltaTime;
			string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
			aTextDrawingArea_1271.method380("Unity Fps: " + text, c, i1, k);
			k += 15;
		}
		int il1 = 0xffff00;
		int x = baseX + (myPlayer.x - 6 >> 7);
		int y = baseY + (myPlayer.y - 6 >> 7);
		if (clientData) {
			char c = '\u01FB';
			int k = 20;
			if (base.fps < 15)
				il1 = 0xff0000;
			aTextDrawingArea_1271.method385(0xffff00, "Fps: " + base.fps, 285,
			                                5);
			//Runtime runtime = Runtime.getRuntime();
			int j1 = (int)(System.GC.GetTotalMemory (false)/1024L);//(int) ((runtime.totalMemory() - runtime.freeMemory()) / 1024L);
			il1 = 0xffff00;
			if (j1 > 0x2000000 && lowMem)
				il1 = 0xff0000;
			k += 15;
			aTextDrawingArea_1271.method385(0xffff00, "Mem: " + j1 + "k", 299,
			                                5);
			aTextDrawingArea_1271.method385(0xffff00, "Mouse X: "
			                                + base.mouseX + " , Mouse Y: " + base.mouseY, 314, 5);
			aTextDrawingArea_1271.method385(0xffff00,
			                                "Coords: " + x + ", " + y, 329, 5);
		}
		if (anInt1104 != 0) {
			int j = anInt1104 / 50;
			int l = j / 60;
			j %= 60;
			if (j < 10)
				aTextDrawingArea_1271.method385(0xffff00, "System update in: "
				                                + l + ":0" + j, 329, 4);
			else
				aTextDrawingArea_1271.method385(0xffff00, "System update in: "
				                                + l + ":" + j, 329, 4);
			anInt849++;
			if (anInt849 > 75) {
				anInt849 = 0;
				stream.createFrame(148);
			}
		}
	}
	
	private void addIgnore(long l) {
		try {
			if (l == 0L)
				return;
			if (ignoreCount >= 100) {
				pushMessage("Your ignore list is full. Max of 100 hit", 0, "");
				return;
			}
			String s = TextClass.fixName(TextClass.nameForLong(l));
			for (int j = 0; j < ignoreCount; j++)
			if (ignoreListAsLongs[j] == l) {
				pushMessage(s + " is already on your ignore list", 0, "");
				return;
			}
			for (int k = 0; k < friendsCount; k++)
			if (friendsListAsLongs[k] == l) {
				pushMessage("Please remove " + s
				            + " from your friend list first", 0, "");
				return;
			}
			
			ignoreListAsLongs[ignoreCount++] = l;
			needDrawTabArea = true;
			stream.createFrame(133);
			stream.writeQWord(l);
			return;
		} catch (Exception runtimeexception) {
			signlink.reporterror("45688, " + l + ", " + 4 + ", "
			                     + runtimeexception.ToString());
		}
		throw new Exception();
	}
	
	private void method114() {
		for (int i = -1; i < playerCount; i++) {
			int j;
			if (i == -1)
				j = myPlayerIndex;
			else
				j = playerIndices[i];
			Player player = playerArray[j];
			if (player != null)
				method96(player);
		}
		
	}
	
	public void method115() {
//		if (loadingStage == 2) {
//			Debug.Log ("Method115");
//			for (Class30_Sub1 class30_sub1 = (Class30_Sub1) aClass19_1179.reverseGetFirst(); class30_sub1 != null; class30_sub1 = (Class30_Sub1) aClass19_1179.reverseGetNext()) {
//				if (class30_sub1.anInt1294 > 0)
//					class30_sub1.anInt1294--;
//				Debug.Log ("Method115a");
//				if (class30_sub1.anInt1294 == 0) {
//					Debug.Log ("Method115b");
//					if (class30_sub1.anInt1299 < 0
//					    || ObjectManager.method178(class30_sub1.anInt1299,
//					                           class30_sub1.anInt1301)) {
//						method142(class30_sub1.anInt1298,
//						          class30_sub1.anInt1295, class30_sub1.anInt1300,
//						          class30_sub1.anInt1301, class30_sub1.anInt1297,
//						          class30_sub1.anInt1296, class30_sub1.anInt1299);
//						class30_sub1.unlink();
//					}
//				} else {
//					Debug.Log ("Method115c");
//					if (class30_sub1.anInt1302 > 0)
//						class30_sub1.anInt1302--;
//					if (class30_sub1.anInt1302 == 0
//					    && class30_sub1.anInt1297 >= 1
//					    && class30_sub1.anInt1298 >= 1
//					    && class30_sub1.anInt1297 <= 102
//					    && class30_sub1.anInt1298 <= 102
//					    && (class30_sub1.anInt1291 < 0 || ObjectManager
///					    .method178(class30_sub1.anInt1291,
//					           class30_sub1.anInt1293))) {
//						Debug.Log ("Method115d");
//						method142(class30_sub1.anInt1298,
//						          class30_sub1.anInt1295, class30_sub1.anInt1292,
//						          class30_sub1.anInt1293, class30_sub1.anInt1297,
//						          class30_sub1.anInt1296, class30_sub1.anInt1291);
//						class30_sub1.anInt1302 = -1;
//						if (class30_sub1.anInt1291 == class30_sub1.anInt1299
//						    && class30_sub1.anInt1299 == -1)
//							class30_sub1.unlink();
//						else if (class30_sub1.anInt1291 == class30_sub1.anInt1299
//						         && class30_sub1.anInt1292 == class30_sub1.anInt1300
//						         && class30_sub1.anInt1293 == class30_sub1.anInt1301)
//							class30_sub1.unlink();
//					}
//				}
//			}
//			
//		}
	}
	
	private void determineMenuSize() {
// 		int i = chatTextDrawingArea.getTextWidth("Choose Option");
// 		for (int j = 0; j < menuActionRow; j++) {
// 			int k = chatTextDrawingArea.getTextWidth(menuActionName[j]);
// 			if (k > i)
// 				i = k;
// 		}
// 		
// 		i += 8;
// 		int l = 15 * menuActionRow + 21;
// 		if (base.saveClickX > 4 && base.saveClickY > 4
// 		    && base.saveClickX < 516 && base.saveClickY < 338) {
// 			int i1 = base.saveClickX - 4 - i / 2;
// 			if (i1 + i > 512)
// 				i1 = 512 - i;
// 			if (i1 < 0)
// 				i1 = 0;
// 			int l1 = base.saveClickY - 4;
// 			if (l1 + l > 334)
// 				l1 = 334 - l;
// 			if (l1 < 0)
// 				l1 = 0;
// 			menuOpen = true;
// 			menuScreenArea = 0;
// 			menuOffsetX = i1;
// 			menuOffsetY = l1;
// 			menuWidth = i;
// 			menuHeight = 15 * menuActionRow + 22;
// 		}
// 		if (base.saveClickX >= 515 && base.saveClickY >= 0 && base.saveClickX <= 765 && base.saveClickY <= 169) {
// 			int k1 = base.saveClickX -515- i / 2;
// 			if (k1 < 0)
// 				k1 = 0;
// 			else if (k1 + i > 249)
// 				k1 =249-i;
// 			int j2 = base.saveClickY;
// 			if (j2 < 0)
// 				j2 = 0;
// 			else if (j2 + l > 168)
// 				j2 = 168 - l;
// 			menuOpen = true;
// 			menuScreenArea = 3;
// 			menuOffsetX = k1;
// 			menuOffsetY = j2;
// 			menuWidth = i;
// 			menuHeight = 15 * menuActionRow + 22;
// 		}
// 		if (base.saveClickX > 519 && base.saveClickY > 168
// 		    && base.saveClickX < 765 && base.saveClickY < 503) {
// 			int j1 = base.saveClickX - 519 - i / 2;
// 			if (j1 < 0)
// 				j1 = 0;
// 			else if (j1 + i > 245)
// 				j1 = 245 - i;
// 			int i2 = base.saveClickY - 168;
// 			if (i2 < 0)
// 				i2 = 0;
// 			else if (i2 + l > 333)
// 				i2 = 333 - l;
// 			menuOpen = true;
// 			menuScreenArea = 1;
// 			menuOffsetX = j1;
// 			menuOffsetY = i2;
// 			menuWidth = i;
// 			menuHeight = 15 * menuActionRow + 22;
// 		}
// 		if (base.saveClickX > 0 && base.saveClickY > 338
// 		    && base.saveClickX < 516 && base.saveClickY < 503) {
// 			int k1 = base.saveClickX - 0 - i / 2;
// 			if (k1 < 0)
// 				k1 = 0;
// 			else if (k1 + i > 516)
// 				k1 = 516 - i;
// 			int j2 = base.saveClickY - 338;
// 			if (j2 < 0)
// 				j2 = 0;
// 			else if (j2 + l > 165)
// 				j2 = 165 - l;
// 			menuOpen = true;
// 			menuScreenArea = 2;
// 			menuOffsetX = k1;
// 			menuOffsetY = j2;
// 			menuWidth = i;
// 			menuHeight = 15 * menuActionRow + 22;
// 		}
int boxLength = chatTextDrawingArea.getTextWidth("Choose option");
		for(int row = 0; row < menuActionRow; row++) {
			int actionLength = chatTextDrawingArea.getTextWidth(menuActionName[row]);
			if(actionLength > boxLength)
				boxLength = actionLength;
		}
		boxLength += 8;
		int offset = 15 * menuActionRow + 21;
		if(base.saveClickX > 0 && base.saveClickY > 0 && base.saveClickX < myWidth && base.saveClickY < myHeight) {
			int xClick = base.saveClickX - boxLength / 2;
			if(xClick + boxLength > myWidth - 4) {
				xClick = (myWidth - 4) - boxLength;
			}
			if(xClick < 0){
				xClick = 0;
			}
			int yClick = base.saveClickY - 0;
			if(yClick + offset > myHeight - 6) {
				yClick = (myHeight - 6) - offset;
			}
			if(yClick < 0) {
				yClick = 0;
			}
			menuOpen = true;
			menuOffsetX = xClick;
			menuOffsetY = yClick;
			menuWidth = boxLength;
			menuHeight = 15 * menuActionRow + 22;
		}
	}
	
	private void method117(RS2Sharp.Stream stream) {
		stream.initBitAccess();
		int j = stream.readBits(1);
		if (j == 0)
			return;
		int k = stream.readBits(2);
		if (k == 0) {
			anIntArray894[anInt893++] = myPlayerIndex;
			return;
		}
		if (k == 1) {
			int l = stream.readBits(3);
			myPlayer.moveInDir(false, l);
			int k1 = stream.readBits(1);
			if (k1 == 1)
				anIntArray894[anInt893++] = myPlayerIndex;
			return;
		}
		if (k == 2) {
			int i1 = stream.readBits(3);
			myPlayer.moveInDir(true, i1);
			int l1 = stream.readBits(3);
			myPlayer.moveInDir(true, l1);
			int j2 = stream.readBits(1);
			if (j2 == 1)
				anIntArray894[anInt893++] = myPlayerIndex;
			return;
		}
		if (k == 3) {
			plane = stream.readBits(2);
			int j1 = stream.readBits(1);
			int i2 = stream.readBits(1);
			if (i2 == 1)
				anIntArray894[anInt893++] = myPlayerIndex;
			int k2 = stream.readBits(7);
			int l2 = stream.readBits(7);
			myPlayer.setPos(l2, k2, j1 == 1);
		}
	}
	
	private void nullLoader() {
		aBoolean831 = false;
		while (drawingFlames) {
			aBoolean831 = false;
			try {
				//Thread.Sleep(50);
			} catch (Exception _ex) {
			}
		}
		aBackground_966 = null;
		aBackground_967 = null;
		aBackgroundArray1152s = null;
		anIntArray850 = null;
		anIntArray851 = null;
		anIntArray852 = null;
		anIntArray853 = null;
		anIntArray1190 = null;
		anIntArray1191 = null;
		anIntArray828 = null;
		anIntArray829 = null;
		aClass30_Sub2_Sub1_Sub1_1201 = null;
		aClass30_Sub2_Sub1_Sub1_1202 = null;
	}
	
	private bool method119(int i, int j) {
		bool flag1 = false;
		RSInterface class9 = RSInterface.interfaceCache[j];
		if (class9 == null) return flag1;
		for (int k = 0; k < class9.children.Length; k++) {
			if (class9.children[k] == -1)
				break;
			RSInterface class9_1 = RSInterface.interfaceCache[class9.children[k]];
			if (class9_1.type == 1)
				flag1 |= method119(i, class9_1.id);
			if (class9_1.type == 6
			    && (class9_1.anInt257 != -1 || class9_1.anInt258 != -1)) {
				bool flag2 = interfaceIsSelected(class9_1);
				int l;
				if (flag2)
					l = class9_1.anInt258;
				else
					l = class9_1.anInt257;
				if (l != -1) {
					RS2Sharp.Animation animation = RS2Sharp.Animation.anims[l];
					for (class9_1.anInt208 += i; class9_1.anInt208 > animation
					     .duration(class9_1.anInt246);) {
						class9_1.anInt208 -= animation
							.duration(class9_1.anInt246) + 1;
						class9_1.anInt246++;
						if (class9_1.anInt246 >= animation.frameCount) {
							class9_1.anInt246 -= animation.anInt356;
							if (class9_1.anInt246 < 0
							    || class9_1.anInt246 >= animation.frameCount)
								class9_1.anInt246 = 0;
						}
						flag1 = true;
					}
					
				}
			}
		}
		
		return flag1;
	}
	
	private int method120() {
		if (roofsOff) {
			return plane;
		}
		int j = 3;
		if (yCameraCurve < 310) {
			int k = xCameraPos >> 7;
			int l = yCameraPos >> 7;
			int i1 = myPlayer.x >> 7;
			int j1 = myPlayer.y >> 7;
			try{
			if ((byteGroundArray[plane][k][l] & 4) != 0)
				j = plane;
			}
			catch(Exception ex)
			{
				return plane;
			}
			int k1;
			if (i1 > k)
				k1 = i1 - k;
			else
				k1 = k - i1;
			int l1;
			if (j1 > l)
				l1 = j1 - l;
			else
				l1 = l - j1;
			if(l1 == 0) l1 = 1;
			if (k1 > l1) {
				int i2 = (l1 * 0x10000) / k1;
				int k2 = 32768;
				while (k != i1) {
					if (k < i1)
						k++;
					else if (k > i1)
						k--;
					if ((byteGroundArray[plane][k][l] & 4) != 0)
						j = plane;
					k2 += i2;
					if (k2 >= 0x10000) {
						k2 -= 0x10000;
						if (l < j1)
							l++;
						else if (l > j1)
							l--;
						if ((byteGroundArray[plane][k][l] & 4) != 0)
							j = plane;
					}
				}
			} else {
				int j2 = (k1 * 0x10000) / l1;
				int l2 = 32768;
				while (l != j1) {
					if (l < j1)
						l++;
					else if (l > j1)
						l--;
					if ((byteGroundArray[plane][k][l] & 4) != 0)
						j = plane;
					l2 += j2;
					if (l2 >= 0x10000) {
						l2 -= 0x10000;
						if (k < i1)
							k++;
						else if (k > i1)
							k--;
						if ((byteGroundArray[plane][k][l] & 4) != 0)
							j = plane;
					}
				}
			}
		}
		if ((byteGroundArray[plane][myPlayer.x >> 7][myPlayer.y >> 7] & 4) != 0)
			j = plane;
		return j;
	}
	
	private int method121() {
		if (roofsOff ) {
			return plane;
		}
		int j = method42(plane, yCameraPos, xCameraPos);
		if (j - zCameraPos < 800
		    && (byteGroundArray[plane][xCameraPos >> 7][yCameraPos >> 7] & 4) != 0)
			return plane;
		else
			return 3;
	}
	
	private void delIgnore(long l) {
		try {
			if (l == 0L)
				return;
			for (int j = 0; j < ignoreCount; j++)
			if (ignoreListAsLongs[j] == l) {
				ignoreCount--;
				needDrawTabArea = true;
				System.Array.Copy(ignoreListAsLongs, j + 1,
				                 ignoreListAsLongs, j, ignoreCount - j);
				
				stream.createFrame(74);
				stream.writeQWord(l);
				return;
			}
			
			return;
		} catch (Exception runtimeexception) {
			signlink.reporterror("47229, " + 3 + ", " + l + ", "
			                     + runtimeexception.ToString());
		}
		throw new Exception();
	}

//	public String getParameter(String s) {
//		if (signlink.mainapp != null)
//			return signlink.mainapp.getParameter(s);
//		else
//			return base.getParameter(s);
//	}
	
	private void adjustVolume(bool flag, int i) {
		signlink.midivol = i;
		if (flag)
			signlink.midi = "voladjust";
	}
	
	private int extractInterfaceValues(RSInterface class9, int j) {
		if (class9.valueIndexArray == null
		    || j >= class9.valueIndexArray.Length)
			return -2;
		try {
			int[] ai = class9.valueIndexArray[j];
			int k = 0;
			int l = 0;
			int i1 = 0;
			do {
				int j1 = ai[l++];
				int k1 = 0;
				byte byte0 = 0;
				if (j1 == 0)
					return k;
				if (j1 == 1)
					k1 = currentStats[ai[l++]];
				if (j1 == 2)
					k1 = maxStats[ai[l++]];
				if (j1 == 3)
					k1 = currentExp[ai[l++]];
				if (j1 == 4) {
					RSInterface class9_1 = RSInterface.interfaceCache[ai[l++]];
					int k2 = ai[l++];
					if (k2 >= 0 && k2 < ItemDef.totalItems
					    && (!ItemDef.forID(k2).membersObject || isMembers)) {
						for (int j3 = 0; j3 < class9_1.inv.Length; j3++)
							if (class9_1.inv[j3] == k2 + 1)
								k1 += class9_1.invStackSizes[j3];
						
					}
				}
				if (j1 == 5)
					k1 = variousSettings[ai[l++]];
				if (j1 == 6)
					k1 = anIntArray1019[maxStats[ai[l++]] - 1];
				if (j1 == 7)
					k1 = (variousSettings[ai[l++]] * 100) / 46875;
				if (j1 == 8)
					k1 = myPlayer.combatLevel;
				if (j1 == 9) {
					for (int l1 = 0; l1 < Skills.skillsCount; l1++)
						if (Skills.skillEnabled[l1])
							k1 += maxStats[l1];
					
				}
				if (j1 == 10) {
					RSInterface class9_2 = RSInterface.interfaceCache[ai[l++]];
					int l2 = ai[l++] + 1;
					if (l2 >= 0 && l2 < ItemDef.totalItems
					    && (!ItemDef.forID(l2).membersObject || isMembers)) {
						for (int k3 = 0; k3 < class9_2.inv.Length; k3++) {
							if (class9_2.inv[k3] != l2)
								continue;
							k1 = 0x3b9ac9ff;
							break;
						}
						
					}
				}
				if (j1 == 11)
					k1 = energy;
				if (j1 == 12)
					k1 = weight;
				if (j1 == 13) {
					int i2 = variousSettings[ai[l++]];
					int i3 = ai[l++];
					k1 = (i2 & 1 << i3) == 0 ? 0 : 1;
				}
				if (j1 == 14) {
					int j2 = ai[l++];
					VarBit varBit = VarBit.cache[j2];
					int l3 = varBit.anInt648;
					int i4 = varBit.anInt649;
					int j4 = varBit.anInt650;
					int k4 = anIntArray1232[j4 - i4];
					k1 = variousSettings[l3] >> i4 & k4;
				}
				if (j1 == 15)
					byte0 = 1;
				if (j1 == 16)
					byte0 = 2;
				if (j1 == 17)
					byte0 = 3;
				if (j1 == 18)
					k1 = (myPlayer.x >> 7) + baseX;
				if (j1 == 19)
					k1 = (myPlayer.y >> 7) + baseY;
				if (j1 == 20)
					k1 = ai[l++];
				if (byte0 == 0) {
					if (i1 == 0)
						k += k1;
					if (i1 == 1)
						k -= k1;
					if (i1 == 2 && k1 != 0)
						k /= k1;
					if (i1 == 3)
						k *= k1;
					i1 = 0;
				} else {
					i1 = byte0;
				}
			} while (true);
		} catch (Exception _ex) {
			return -1;
		}
	}
	
	private void drawTooltip() {
		if (menuActionRow < 2 && itemSelected == 0 && spellSelected == 0)
			return;
		String s;
		if (itemSelected == 1 && menuActionRow < 2)
			s = "Use " + selectedItemName + " with...";
		else if (spellSelected == 1 && menuActionRow < 2)
			s = spellTooltip + "...";
		else
		{
			s = menuActionName[menuActionRow - 1];
		}
		
		if (menuActionRow > 2)
			s = s + "@whi@ / " + (menuActionRow - 2) + " more options";
		
		if(menuActionRow - 2 == 1)
		 s = "Walk here";
		
		chatTextDrawingArea.method390(4, 0xffffff, s, loopCycle / 1000, 15);
	}
	
	private void drawMinimap() {
// 		//aRSImageProducer_1164.initDrawingArea();
// 		int x = 549+extraWidth;
// 		int markY = 6;
// 		int markX = 9;
// 		if(anInt1021 == 2) {
// 			byte[] abyte0 = mapBack.aByteArray1450;
// 			int[] ai = DrawingArea.pixels;
// 			int k2 = abyte0.Length;
// 			for(int i5 = 0; i5 < k2; i5++)
// 				if(abyte0[i5] == 0)
// 					ai[i5] = 0;
// 			//compass.method352(33, minimapInt1, anIntArray1057, 256, anIntArray968, 25, 8, 8, 33, 25);
// 			aRSImageProducer_1165.initDrawingArea();
// 			return;
// 		}
// 		int i = minimapInt1 + minimapInt2 & 0x7ff;
// 		int j = 48 + myPlayer.x / 32;
// 		int l2 = 464 - myPlayer.y / 32;
// 		for (int x = 0; x < anIntArray1229.Length; x++){
// 			anIntArray1229[x] = 170;
// 			anIntArray1052[x] = -23;
// 		}
// 		aClass30_Sub2_Sub1_Sub1_1263.method352(151, i, anIntArray1229, 256 + minimapInt3, anIntArray1052, l2, 12, 34+x, 146, j);
// 		//compass.method352(33, minimapInt1, anIntArray1057, 256, anIntArray968, 25, 8, 8, 33, 25);
// 		for(int j5 = 0; j5 < anInt1071; j5++) {
// 			int k = (anIntArray1072[j5] * 4 + 2) - myPlayer.x / 32;
// 			int i3 = (anIntArray1073[j5] * 4 + 2) - myPlayer.y / 32;
// 			markMinimap(aClass30_Sub2_Sub1_Sub1Array1140[j5], k, i3);
// 		}
// 		
// 		for(int k5 = 0; k5 < 104; k5++) {
// 			for(int l5 = 0; l5 < 104; l5++) {
// 				NodeList class19 = groundArray[plane][k5][l5];
// 				if(class19 != null) {
// 					int l = (k5 * 4 + 2) - myPlayer.x / 32;
// 					int j3 = (l5 * 4 + 2) - myPlayer.y / 32;
// 					markMinimap(mapDotItem, l, j3);
// 				}
// 			}
// 		}
// 		
// 		for(int i6 = 0; i6 < npcCount; i6++) {
// 			NPC npc = npcArray[npcIndices[i6]];
// 			if(npc != null && npc.isVisible()) {
// 				EntityDef entityDef = npc.desc;
// 				if(entityDef.childrenIDs != null)
// 					entityDef = entityDef.method161();
// 				if(entityDef != null && entityDef.aBoolean87 && entityDef.aBoolean84) {
// 					int i1 = npc.x / 32 - myPlayer.x / 32;
// 					int k3 = npc.y / 32 - myPlayer.y / 32;
// 					markMinimap(mapDotNPC, i1, k3);
// 				}
// 			}
// 		}
// 		
// 		for(int j6 = 0; j6 < playerCount; j6++) {
// 			Player player = playerArray[playerIndices[j6]];
// 			if(player != null && player.isVisible()) {
// 				int j1 = player.x / 32 - myPlayer.x / 32;
// 				int l3 = player.y / 32 - myPlayer.y / 32;
// 				bool flag1 = false;
// 				long l6 = TextClass.longForName(player.name);
// 				for(int k6 = 0; k6 < friendsCount; k6++) {
// 					if(l6 != friendsListAsLongs[k6] || friendsNodeIDs[k6] == 0)
// 						continue;
// 					flag1 = true;
// 					break;
// 				}
// 				bool flag2 = false;
// 				if(myPlayer.team != 0 && player.team != 0 && myPlayer.team == player.team)
// 					flag2 = true;
// 				if(flag1)
// 					markMinimap(mapDotFriend, j1, l3);
// 				else
// 					markMinimap(mapDotPlayer, j1, l3);
// 			}
// 		}
// 		
// 		if(anInt855 != 0 && loopCycle % 20 < 10) {
// 			if(anInt855 == 1 && anInt1222 >= 0 && anInt1222 < npcArray.Length) {
// 				NPC class30_sub2_sub4_sub1_sub1_1 = npcArray[anInt1222];
// 				if(class30_sub2_sub4_sub1_sub1_1 != null)
// 				{
// 					int k1 = class30_sub2_sub4_sub1_sub1_1.x / 32 - myPlayer.x / 32;
// 					int i4 = class30_sub2_sub4_sub1_sub1_1.y / 32 - myPlayer.y / 32;
// 					method81(mapMarker, i4, k1);
// 				}
// 			}
// 			if(anInt855 == 2) {
// 				int l1 = ((anInt934 - baseX) * 4 + 2) - myPlayer.x / 32;
// 				int j4 = ((anInt935 - baseY) * 4 + 2) - myPlayer.y / 32;
// 				method81(mapMarker, j4, l1);
// 			}
// 			if(anInt855 == 10 && anInt933 >= 0 && anInt933 < playerArray.Length)
// 			{
// 				Player class30_sub2_sub4_sub1_sub2_1 = playerArray[anInt933];
// 				if(class30_sub2_sub4_sub1_sub2_1 != null) {
// 					int i2 = class30_sub2_sub4_sub1_sub2_1.x / 32 - myPlayer.x / 32;
// 					int k4 = class30_sub2_sub4_sub1_sub2_1.y / 32 - myPlayer.y / 32;
// 					method81(mapMarker, k4, i2);
// 				}
// 			}
// 		}
// 		if(destX != 0) {
// 			int j2 = (destX * 4 + 2) - myPlayer.x / 32;
// 			int l4 = (destY * 4 + 2) - myPlayer.y / 32;
// 			markMinimap(mapFlag, j2, l4);
// 		}
// 		DrawingArea.drawPixels(3, 78 + 5, 97 + 20, 0xffffff, 3);
// 		
// 		mapArea.drawSprite(0, 0);
// 		drawLogoutButton();
// 		loadAllOrbs();
// 		if(menuOpen && menuScreenArea == 3){
// 			drawMenu(0,0);
// 		}
// 		aRSImageProducer_1165.initDrawingArea();
	int extraWidth = myWidth -765 - 11;
	int x = 549+extraWidth;
		int markY = 6;
		int markX = 9;

		if(anInt1021 == 2) {
			byte[] abyte0 = mapBack.aByteArray1450;
			int[] ai = DrawingArea.pixels;
			int k2 = abyte0.Length;
			for(int i5 = 0; i5 < k2; i5++)
				if(abyte0[i5] == 0)
					ai[i5] = 0;
			aRSImageProducer_1165.initDrawingArea();
			return;
		}
		int i = minimapInt1 + minimapInt2 & 0x7ff;
		int j = 48 + myPlayer.x / 32;
		int l2 = 464 - myPlayer.y / 32;

			//aClass30_Sub2_Sub1_Sub1_1263.method352(151, i, anIntArray1229, 256 + minimapInt3, anIntArray1052, l2, 12+ +yOffsetDebug, 60+x, 146, j);
aClass30_Sub2_Sub1_Sub1_1263.method352(151, i, anIntArray1229, 256 + minimapInt3, anIntArray1052, l2, 12, 34+x, 146, j);
		for(int j5 = 0; j5 < anInt1071; j5++) {
			int k = (anIntArray1072[j5] * 4 + 2) - myPlayer.x / 32;
			int i3 = (anIntArray1073[j5] * 4 + 2) - myPlayer.y / 32;
			markMinimap(aClass30_Sub2_Sub1_Sub1Array1140[j5], k, i3);
		}	
		
		for(int k5 = 0; k5 < 104; k5++) {
			for(int l5 = 0; l5 < 104; l5++) {
				NodeList class19 = groundArray[plane][k5][l5];
				if(class19 != null) {
					int l = (k5 * 4 + 2) - myPlayer.x / 32;
					int j3 = (l5 * 4 + 2) - myPlayer.y / 32;
					markMinimap(mapDotItem, l, j3);
				}
			}
		}

		for(int i6 = 0; i6 < npcCount; i6++) {
			NPC npc = npcArray[npcIndices[i6]];
			if(npc != null && npc.isVisible()) {
				EntityDef entityDef = npc.desc;
				if(entityDef.childrenIDs != null)
					entityDef = entityDef.method161();
				if(entityDef != null && entityDef.aBoolean87 && entityDef.aBoolean84) {
					int i1 = npc.x / 32 - myPlayer.x / 32;
					int k3 = npc.y / 32 - myPlayer.y / 32;
					markMinimap(mapDotNPC, i1, k3);
				}
			}
		}

		for(int j6 = 0; j6 < playerCount; j6++) {
			Player player = playerArray[playerIndices[j6]];
			if(player != null && player.isVisible()) {
				int j1 = player.x / 32 - myPlayer.x / 32;
				int l3 = player.y / 32 - myPlayer.y / 32;
				bool flag1 = false;
				bool flag3 = false;
// 				for (int j3 = 0; j3 < clanList.length; j3++) {
// 					if (clanList[j3] == null)
// 						continue;
// 					if (!clanList[j3].equalsIgnoreCase(player.name))
// 						continue;
// 					flag3 = true;
// 					break;
// 				}
				long l6 = TextClass.longForName(player.name);
				for(int k6 = 0; k6 < friendsCount; k6++) {
					if(l6 != friendsListAsLongs[k6] || friendsNodeIDs[k6] == 0)
						continue;
					flag1 = true;
					break;
				}
				bool flag2 = false;
				if(myPlayer.team != 0 && player.team != 0 && myPlayer.team == player.team)
					flag2 = true;
				if(flag1)
					markMinimap(mapDotFriend, j1, l3);
// 				else if(flag3)
// 					markMinimap(mapDotClan, j1, l3);
				else if(flag2)
					markMinimap(mapDotTeam, j1, l3);
				else
					markMinimap(mapDotPlayer, j1, l3);
			}
		}

		if(anInt855 != 0 && loopCycle % 20 < 10) {
			if(anInt855 == 1 && anInt1222 >= 0 && anInt1222 < npcArray.Length) {
					NPC class30_sub2_sub4_sub1_sub1_1 = npcArray[anInt1222];
					if(class30_sub2_sub4_sub1_sub1_1 != null)
					{
						int k1 = class30_sub2_sub4_sub1_sub1_1.x / 32 - myPlayer.x / 32;
						int i4 = class30_sub2_sub4_sub1_sub1_1.y / 32 - myPlayer.y / 32;
						method81(mapMarker, i4, k1);
					}
			}
			if(anInt855 == 2) {
				int l1 = ((anInt934 - baseX) * 4 + 2) - myPlayer.x / 32;
				int j4 = ((anInt935 - baseY) * 4 + 2) - myPlayer.y / 32;
				method81(mapMarker, j4, l1);
			}
			if(anInt855 == 10 && anInt933 >= 0 && anInt933 < playerArray.Length)
			{
				Player class30_sub2_sub4_sub1_sub2_1 = playerArray[anInt933];
				if(class30_sub2_sub4_sub1_sub2_1 != null) {
					int i2 = class30_sub2_sub4_sub1_sub2_1.x / 32 - myPlayer.x / 32;
					int k4 = class30_sub2_sub4_sub1_sub2_1.y / 32 - myPlayer.y / 32;
					method81(mapMarker, k4, i2);
				}
			}
		}
		if(destX != 0) {
			int j2 = (destX * 4 + 2) - myPlayer.x / 32;
			int l4 = (destY * 4 + 2) - myPlayer.y / 32;
			markMinimap(mapFlag, j2, l4);
		}

			DrawingArea.drawPixels(3, 78+markY, 97+x+markX, 0xffffff, 3);
		//Compass
			compass.method352(33, minimapInt1, anIntArray1057, 256, anIntArray968, 25, 9,13+x, 33, 25);
//
			mapArea.drawSprite(8+x, 4);

// 		if(!newMap) {
// 			drawWorldMapButton();
// 			drawHPOrb();
// 			drawXPCounter();
// 			drawPrayerOrb();
// 			drawRunOrb();
// 			drawLogoutButton();
// 		}
// 		if (!hasBeenBlanked) {
// 			aRSImageProducer_1164.blankImage();
// 			hasBeenBlanked = true;
// 		}
		if(menuOpen && menuScreenArea == 3){
			drawMenu(0, 0);
		}
		aRSImageProducer_1165.initDrawingArea();
	}
	
	private void npcScreenPos(Entity entity, int i) {
		calcEntityScreenPos(entity.x, i, entity.y);
	}
	
	private void calcEntityScreenPos(int i, int j, int l) {
		if (i < 128 || l < 128 || i > 13056 || l > 13056) {
			spriteDrawX = -1;
			spriteDrawY = -1;
			return;
		}
		int i1 = method42(plane, l, i) - j;
		i -= xCameraPos;
		i1 -= zCameraPos;
		l -= yCameraPos;
		int j1 = Model.SINE[yCameraCurve];
		int k1 = Model.COSINE[yCameraCurve];
		int l1 = Model.SINE[xCameraCurve];
		int i2 = Model.COSINE[xCameraCurve];
		int j2 = l * l1 + i * i2 >> 16;
		l = l * i2 - i * l1 >> 16;
		i = j2;
		j2 = i1 * k1 - l * j1 >> 16;
		l = i1 * j1 + l * k1 >> 16;
		i1 = j2;
		if (l >= 50) {
			spriteDrawX = RS2Sharp.Texture.center_x + (i << 9) / l;
			spriteDrawY = RS2Sharp.Texture.center_y + (i1 << 9) / l;
		} else {
			spriteDrawX = -1;
			spriteDrawY = -1;
		}
	}
	
	private void buildSplitPrivateChatMenu() {
		if (splitPrivateChat == 0)
			return;
		int i = 0;
		if (anInt1104 != 0)
			i = 1;
		for (int j = 0; j < 100; j++)
		if (chatMessages[j] != null) {
			int k = chatTypes[j];
			String s = chatNames[j];
			bool flag1 = false;
			if (s != null && s.StartsWith("@cr1@")) {
				s = s.Substring(5);
				bool flag2 = true;
			}
			if (s != null && s.StartsWith("@cr2@")) {
				s = s.Substring(5);
				byte byte0 = 2;
			}
			if ((k == 3 || k == 7)
			    && (k == 7 || privateChatMode == 0 || privateChatMode == 1
			    && isFriendOrSelf(s))) {
				int l = 329 - i * 13;
				if (base.mouseX > 4 && base.mouseY - 4 > l - 10
				    && base.mouseY - 4 <= l + 3) {
					int i1 = aTextDrawingArea_1271.getTextWidth("From:  "
					                                            + s + chatMessages[j]) + 25;
					if (i1 > 450)
						i1 = 450;
					if (base.mouseX < 4 + i1) {
						if (myPrivilege >= 1) {
							menuActionName[menuActionRow] = "Report abuse @whi@"
								+ s;
							menuActionID[menuActionRow] = 2606;
							menuActionRow++;
						}
						menuActionName[menuActionRow] = "Add ignore @whi@"
							+ s;
						menuActionID[menuActionRow] = 2042;
						menuActionRow++;
						menuActionName[menuActionRow] = "Add friend @whi@"
							+ s;
						menuActionID[menuActionRow] = 2337;
						menuActionRow++;
					}
				}
				if (++i >= 5)
					return;
			}
			if ((k == 5 || k == 6) && privateChatMode < 2 && ++i >= 5)
				return;
		}
		
	}
	
	private void method130(int j, int k, int l, int i1, int j1, int k1, int l1,
	                       int i2, int j2) {
		Class30_Sub1 class30_sub1 = null;
		for (Class30_Sub1 class30_sub1_1 = (Class30_Sub1) aClass19_1179
		     .reverseGetFirst(); class30_sub1_1 != null; class30_sub1_1 = (Class30_Sub1) aClass19_1179
		     .reverseGetNext()) {
			if (class30_sub1_1.anInt1295 != l1
			    || class30_sub1_1.anInt1297 != i2
			    || class30_sub1_1.anInt1298 != j1
			    || class30_sub1_1.anInt1296 != i1)
				continue;
			class30_sub1 = class30_sub1_1;
			break;
		}
		
		if (class30_sub1 == null) {
			class30_sub1 = new Class30_Sub1();
			class30_sub1.anInt1295 = l1;
			class30_sub1.anInt1296 = i1;
			class30_sub1.anInt1297 = i2;
			class30_sub1.anInt1298 = j1;
			method89(class30_sub1);
			aClass19_1179.insertHead(class30_sub1);
		}
		class30_sub1.anInt1291 = k;
		class30_sub1.anInt1293 = k1;
		class30_sub1.anInt1292 = l;
		class30_sub1.anInt1302 = j2;
		class30_sub1.anInt1294 = j;
	}
	
	private bool interfaceIsSelected(RSInterface class9) {
		if (class9.anIntArray245 == null)
			return false;
		for (int i = 0; i < class9.anIntArray245.Length; i++) {
			int j = extractInterfaceValues(class9, i);
			int k = class9.anIntArray212[i];
			if (class9.anIntArray245[i] == 2) {
				if (j >= k)
					return false;
			} else if (class9.anIntArray245[i] == 3) {
				if (j <= k)
					return false;
			} else if (class9.anIntArray245[i] == 4) {
				if (j == k)
					return false;
			} else if (j != k)
				return false;
		}
		
		return true;
	}
	
	private static NetworkStream openJagGrabInputStream(String s) {
		// if(!aBoolean872)
		// if(signlink.mainapp != null)
		// return signlink.openurl(s);
		// else
		// return new DataInputStream((new URL(getCodeBase(), s)).openStream());
// 		if (aSocket832 != null) {
// 			try {
// 				aSocket832.close();
// 			} catch (Exception _ex) {
// 			}
// 			aSocket832 = null;
// 		}
// 		aSocket832 = openSocket(43595);
// 		aSocket832.setSoTimeout(10000);
// 		java.io.InputStream inputstream = aSocket832.getInputStream();
// 		OutputStream outputstream = aSocket832.getOutputStream();
// 		outputstream.write(("JAGGRAB /" + s + "\n\n").getBytes());
// 		return new DataInputStream(inputstream);

//	if (aSocket832 != null)
//			{
//					aSocket832.Close();
//				aSocket832 = null;
//			}
//			aSocket832 = openSocket(43595);
//			aSocket832.SendTimeout = 10000;
//			aSocket832.ReceiveTimeout = 10000;
//			NetworkStream inputstream = new NetworkStream(aSocket832);
//			NetworkStream outputstream = new NetworkStream(aSocket832);
//			outputstream.Write(ASCIIEncoding.ASCII.GetBytes("JAGGRAB /" + s + "\n\n"), 0, ("JAGGRAB /" + s + "\n\n").Length);
//			return inputstream;
return null;
	}
	
	private void doFlamesDrawing() {
		char c = '\u0100';
		if (anInt1040 > 0) {
			for (int i = 0; i < 256; i++)
				if (anInt1040 > 768)
					anIntArray850[i] = method83(anIntArray851[i],
					                            anIntArray852[i], 1024 - anInt1040);
			else if (anInt1040 > 256)
				anIntArray850[i] = anIntArray852[i];
			else
				anIntArray850[i] = method83(anIntArray852[i],
				                            anIntArray851[i], 256 - anInt1040);
			
		} else if (anInt1041 > 0) {
			for (int j = 0; j < 256; j++)
				if (anInt1041 > 768)
					anIntArray850[j] = method83(anIntArray851[j],
					                            anIntArray853[j], 1024 - anInt1041);
			else if (anInt1041 > 256)
				anIntArray850[j] = anIntArray853[j];
			else
				anIntArray850[j] = method83(anIntArray853[j],
				                            anIntArray851[j], 256 - anInt1041);
			
		} else {
			System.Array.Copy(anIntArray851, 0, anIntArray850, 0, 256);
			
		}
		System.Array.Copy(aClass30_Sub2_Sub1_Sub1_1201.myPixels, 0,
		                 aRSImageProducer_1110.anIntArray315, 0, 33920);
		
		int i1 = 0;
		int j1 = 1152;
		for (int k1 = 1; k1 < c - 1; k1++) {
			int l1 = (anIntArray969[k1] * (c - k1)) / c;
			int j2 = 22 + l1;
			if (j2 < 0)
				j2 = 0;
			i1 += j2;
			for (int l2 = j2; l2 < 128; l2++) {
				int j3 = anIntArray828[i1++];
				if (j3 != 0) {
					int l3 = j3;
					int j4 = 256 - j3;
					j3 = anIntArray850[j3];
					int l4 = aRSImageProducer_1110.anIntArray315[j1];
					aRSImageProducer_1110.anIntArray315[j1++] =  unchecked((int)((j3 & 0xff00ff)
					                                             * l3 + (l4 & 0xff00ff) * j4 & 0xff00ff00)
						+ ((j3 & 0xff00) * l3 + (l4 & 0xff00) * j4 & 0xff0000) >> 8);
				} else {
					j1++;
				}
			}
			
			j1 += j2;
		}
		
		aRSImageProducer_1110.drawGraphics(0, base.graphics, 0);
		System.Array.Copy(aClass30_Sub2_Sub1_Sub1_1202.myPixels, 0,
		                 aRSImageProducer_1111.anIntArray315, 0, 33920);
		
		i1 = 0;
		j1 = 1176;
		for (int k2 = 1; k2 < c - 1; k2++) {
			int i3 = (anIntArray969[k2] * (c - k2)) / c;
			int k3 = 103 - i3;
			j1 += i3;
			for (int i4 = 0; i4 < k3; i4++) {
				int k4 = anIntArray828[i1++];
				if (k4 != 0) {
					int i5 = k4;
					int j5 = 256 - k4;
					k4 = anIntArray850[k4];
					int k5 = aRSImageProducer_1111.anIntArray315[j1];
					aRSImageProducer_1111.anIntArray315[j1++] = unchecked((int)((k4 & 0xff00ff)
					                                             * i5 + (k5 & 0xff00ff) * j5 & 0xff00ff00)
						+ ((k4 & 0xff00) * i5 + (k5 & 0xff00) * j5 & 0xff0000) >> 8);
				} else {
					j1++;
				}
			}
			
			i1 += 128 - k3;
			j1 += 128 - k3 - i3;
		}
		
		aRSImageProducer_1111.drawGraphics(0, base.graphics, 637);
	}
	
	private void method134(RS2Sharp.Stream stream) {
		int j = stream.readBits(8);
		if (j < playerCount) {
			for (int k = j; k < playerCount; k++)
				anIntArray840[anInt839++] = playerIndices[k];
			
		}
		if (j > playerCount) {
			signlink.reporterror(myUsername + " Too many players");
			throw new Exception("eek");
		}
		playerCount = 0;
		for (int l = 0; l < j; l++) {
			int i1 = playerIndices[l];
			Player player = playerArray[i1];
			int j1 = stream.readBits(1);
			if (j1 == 0) {
				playerIndices[playerCount++] = i1;
				player.anInt1537 = loopCycle;
			} else {
				int k1 = stream.readBits(2);
				if (k1 == 0) {
					playerIndices[playerCount++] = i1;
					player.anInt1537 = loopCycle;
					anIntArray894[anInt893++] = i1;
				} else if (k1 == 1) {
					playerIndices[playerCount++] = i1;
					player.anInt1537 = loopCycle;
					int l1 = stream.readBits(3);
					player.moveInDir(false, l1);
					int j2 = stream.readBits(1);
					if (j2 == 1)
						anIntArray894[anInt893++] = i1;
				} else if (k1 == 2) {
					playerIndices[playerCount++] = i1;
					player.anInt1537 = loopCycle;
					int i2 = stream.readBits(3);
					player.moveInDir(true, i2);
					int k2 = stream.readBits(3);
					player.moveInDir(true, k2);
					int l2 = stream.readBits(1);
					if (l2 == 1)
						anIntArray894[anInt893++] = i1;
				} else if (k1 == 3)
					anIntArray840[anInt839++] = i1;
			}
		}
	}
	
	public bool inLoginScreen, inOptionsTab, isAnimated = false;
	
	public RS2Sharp.Sprite loginBackground;
	
	public void drawLoginScreen(bool flag) {
		inLoginScreen = true;
		
		resetImageProducers();
		loginBackground.drawSprite(0, 0);
		logo.drawSprite(205, 3);
		if (loginScreenState == 0)
			drawWelcomeScreen();
		else if (loginScreenState == 1)
			drawLoginBox();
		else if (loginScreenState == 3)
			drawSettingsBox();
		bool debug = true;
		if (debug) {
			aTextDrawingArea_1271.method385(0xffff00, "FPS: " + base.fps, 15,
			                                5);
			aTextDrawingArea_1271.method385(0xffff00, "Mouse X: "
			                                + base.mouseX, 30, 5);
			aTextDrawingArea_1271.method385(0xffff00, "Mouse Y: "
			                                + base.mouseY, 45, 5);
		}
		drawLoginScreenHovers();
		aRSImageProducer_1109.initDrawingArea();
		aRSImageProducer_1109.drawGraphics(0, base.graphics, 0);
		if (welcomeScreenRaised)
			welcomeScreenRaised = false;
	}
	
	public void drawLoginScreenHovers() {
		if (isAnimated) {
			if (base.mouseX >= 0 && base.mouseX <= 32 && base.mouseY >= 469
			    && base.mouseY <= 501)
				cameraHover.drawSprite(0, 484);
			else
				camera.drawSprite(0, 474);
		} else {
			if (base.mouseX >= 0 && base.mouseX <= 32 && base.mouseY >= 469
			    && base.mouseY <= 501)
				disabledCameraHover.drawSprite(0, 474);
			else
				disabledCamera.drawSprite(0, 474);
		}
	}
	
	public void drawWelcomeScreenHovers() {
		if (detail.ToLower().Equals("sd"))
			sdClicked.drawSprite(306, 280);
		else if (detail.ToLower().Equals("hd"))
			hdClicked.drawSprite(402, 280);
		
		if (detail.ToLower().Equals("rem"))
			rememberButton.drawSprite(436, 232);
		
		if (detail != "sd" && base.mouseX >= 436 && base.mouseX <= 449
		    && base.mouseY >= 232 && base.mouseY <= 245) {
			rememberButton.drawSprite(436, 232);
		}
		if (detail != "sd" && base.mouseX >= 305 && base.mouseX <= 361
		    && base.mouseY >= 280 && base.mouseY <= 311) {
			sdHover.drawSprite(306, 280);
		}
		if (detail != "hd" && base.mouseX >= 401 && base.mouseX <= 456
		    && base.mouseY >= 280 && base.mouseY <= 311) {
			hdHover.drawSprite(402, 280);
		}
		musicButton.drawSprite(412, 355);
		if (base.mouseX >= 296 && base.mouseX <= 465 && base.mouseY >= 161
		    && base.mouseY <= 181) {
			boxHover.drawSprite(288, 159);
		}
		if (base.mouseX >= 296 && base.mouseX <= 465 && base.mouseY >= 192
		    && base.mouseY <= 213) {
			boxHover.drawSprite(288, 190);
		}
	}
	
	public void drawSettingScreenHovers() {
		if (detail.ToLower().Equals("sd"))
			sdClicked.drawSprite(293, 155);
		else if (detail.ToLower().Equals("hd"))
			hdClicked.drawSprite(413, 155);
		
		if (screenSize.ToLower().Equals("fixed"))
			fixedHover.drawSprite(264, 218);
		else if (screenSize.ToLower().Equals("resizable"))
			resizableHover.drawSprite(354, 218);
		else if (screenSize.ToLower().Equals("fullscreen"))
			fullscreenHover.drawSprite(444, 218);
		brightHover.drawSprite(322, 311);
		
		if (detail != "sd" && base.mouseX >= 293 && base.mouseX <= 347
		    && base.mouseY >= 155 && base.mouseY <= 186) {
			sdHover.drawSprite(293, 155);
		}
		if (detail != "hd" && base.mouseX >= 413 && base.mouseX <= 467
		    && base.mouseY >= 155 && base.mouseY <= 186) {
			hdHover.drawSprite(413, 155);
		}
		
		if (screenSize != "fixed" && base.mouseX >= 264 && base.mouseX <= 317
		    && base.mouseY >= 218 && base.mouseY <= 252) {
			fixedHover.drawSprite(264, 218);
		}
		
		if (screenSize != "resizable" && base.mouseX >= 354
		    && base.mouseX <= 408 && base.mouseY >= 218
		    && base.mouseY <= 252) {
			resizableHover.drawSprite(354, 218);
		}
		
		if (screenSize != "fullscreen" && base.mouseX >= 444
		    && base.mouseX <= 497 && base.mouseY >= 218
		    && base.mouseY <= 252) {
			fullscreenHover.drawSprite(444, 218);
		}
		
		if (base.mouseX >= 597 && base.mouseX <= 620 && base.mouseY >= 88
		    && base.mouseY <= 110) {
			xHover.drawSprite(597, 88);
		}
		
	}
	
	public RS2Sharp.Sprite loginBox, optionsBox, welcomeScreen, settingsBox;
	public RS2Sharp.Sprite fixedHover, resizableHover, fullscreenHover, sdHover,
	sdClicked, hdHover, hdClicked, xHover, musicButton, brightHover,
	boxHover, rememberButton, loginButton, loginHover, logIconC,
	logIconH, logIconN, logo;
	
	public void drawWelcomeScreen() {
		welcomeScreen.drawSprite1(0, 0);
		drawWelcomeScreenHovers();
	}
	
	public void drawSettingsBox() {
		settingsBox.drawSprite1(0, 0);
		drawSettingScreenHovers();
	}
	
	public void drawLoginBox() {
		
		loginBox.drawSprite1(0, 0);
		smallText.drawChatInput(0x653200, 383, loginMessage1, 180, false);
		smallText.drawChatInput(0x653200, 383, loginMessage2, 203, false);
		aTextDrawingArea_1271.method389(
			false,
			311,
			0x653200,
			(new StringBuilder())
			.Append("")
			.Append(capitalize(myUsername))
			.Append((loginScreenCursorPos == 0)
		        & (loopCycle % 40 < 20) ? "|" : "").ToString(),
			261);
		aTextDrawingArea_1271.method389(
			false,
			311,
			0x653200,
			(new StringBuilder())
			.Append("")
			.Append(TextClass.passwordAsterisks(myPassword))
			.Append((loginScreenCursorPos == 1)
		        & (loopCycle % 40 < 20) ? "|" : "").ToString(),
			328);
		if (base.mouseX >= 297 && base.mouseX <= 466 && base.mouseY >= 240
		    && base.mouseY <= 270) {
			loginHover.drawSprite(288, 237);
		}
		if (base.mouseX >= 297 && base.mouseX <= 466 && base.mouseY >= 306
		    && base.mouseY <= 336) {
			loginHover.drawSprite(288, 305);
		}
		if (base.mouseX >= 355 && base.mouseX <= 408 && base.mouseY >= 356
		    && base.mouseY <= 369) {
			loginButton.drawSprite(355, 356);
		}
	}
	
	public static String capitalize(String s) {
// 		for (int i = 0; i < s.Length; i++) {
// 			if (i == 0)
// 				s = String.format(
// 					"%s%s",
// 					new Object[] {
// 					Character.valueOf(Character.toUpperCase(s
// 					                                        .charAt(0))), s.Substring(1) });
// 			if (!Character.isLetterOrDigit(s.charAt(i)) && i + 1 < s.Length)
// 				s = String.format(
// 					"%s%s%s",
// 					new Object[] {
// 					s.subSequence(0, i + 1),
// 					Character.valueOf(Character.toUpperCase(s
// 					                                        .charAt(i + 1))), s.Substring(i + 2) });
// 		}
		
		return s.ToUpper();
	}
	
	public void drawOptionsTab() {
		optionsBox.drawSprite1(233, 102);
		
	}
	
	private void drawLoginScreen1(bool flag) {
		resetImageProducers();
		aRSImageProducer_1109.initDrawingArea();
		
		aBackground_966.drawSprite(0, 0); // achterkant loginbox
		aTextDrawingArea_1271.method385(0xffff00, "Fps: " + base.fps, 15, 5);
		aTextDrawingArea_1271.method385(0xffff00, "Mouse X: " + base.mouseX,
		                                30, 5);
		aTextDrawingArea_1271.method385(0xffff00, "Mouse Y: " + base.mouseY,
		                                45, 5);
		char c = (char)168;
		char c1 = (char)310;
		if (loginScreenState == 0) {
			int i = c1 / 2 + 80;
			smallText.drawChatInput(0x75a9a9, c / 2, onDemandFetcher.statusString,
			                    i, true);
			i = c1 / 2 - 20;
			chatTextDrawingArea.drawChatInput(0xffff00, c / 2,
			                              "Welcome to RuneScape", i, true);
			i += 30;
			int l = c / 2 - 80;
			int k1 = c1 / 2 + 20;
			aBackground_967.drawSprite(l - 73, k1 - 20); // box background
			chatTextDrawingArea
				.drawChatInput(0xffffff, l, "New User", k1 + 5, true);
			l = c / 2 + 80;
			aBackground_967.drawSprite(l - 73, k1 - 20); // box background
			chatTextDrawingArea.drawChatInput(0xffffff, l, "Existing User", k1 + 5,
			                              true);
		}
		if (loginScreenState == 2) {
			int j = c1 / 2 - 40;
			if (loginMessage1.Length > 0) {
				chatTextDrawingArea.drawChatInput(0xffff00, c / 2, loginMessage1,
				                              j - 15, true);
				chatTextDrawingArea.drawChatInput(0xffff00, c / 2, loginMessage2,
				                              j, true);
				j += 30;
			} else {
				chatTextDrawingArea.drawChatInput(0xffff00, c / 2, loginMessage2,
				                              j - 7, true);
				j += 30;
			}
			chatTextDrawingArea
				.method389(true, c / 2 - 90, 0xffffff, "Username: "
				           + myUsername
				           + ((loginScreenCursorPos == 0)
				   & (loopCycle % 40 < 20) ? "@yel@|" : ""), j);
			j += 15;
			chatTextDrawingArea
				.method389(true, c / 2 - 88, 0xffffff, "Password: "
				           + TextClass.passwordAsterisks(myPassword)
				           + ((loginScreenCursorPos == 1)
				   & (loopCycle % 40 < 20) ? "@yel@|" : ""), j);
			j += 15;
			if (!flag) {
				int i1 = c / 2 - 80;
				int l1 = c1 / 2 + 50;
				aBackground_967.drawSprite(i1 - 73, l1 - 20);
				chatTextDrawingArea.drawChatInput(0xffffff, i1, "Login", l1 + 5,
				                              true);
				i1 = c / 2 + 80;
				aBackground_967.drawSprite(i1 - 73, l1 - 20);
				chatTextDrawingArea.drawChatInput(0xffffff, i1, "Cancel", l1 + 5,
				                              true);
			}
		}
		if (loginScreenState == 3) {
			chatTextDrawingArea.drawChatInput(0xffff00, c / 2,
			                              "Create a free account", c1 / 2 - 60, true);
			int k = c1 / 2 - 35;
			chatTextDrawingArea.drawChatInput(0xffffff, c / 2,
			                              "To create a new account you need to", k, true);
			k += 15;
			chatTextDrawingArea.drawChatInput(0xffffff, c / 2,
			                              "go back to the main RuneScape webpage", k, true);
			k += 15;
			chatTextDrawingArea.drawChatInput(0xffffff, c / 2,
			                              "and choose the red 'create account'", k, true);
			k += 15;
			chatTextDrawingArea.drawChatInput(0xffffff, c / 2,
			                              "button at the top right of that page.", k, true);
			k += 15;
			int j1 = c / 2;
			int i2 = c1 / 2 + 50;
			aBackground_967.drawSprite(j1 - 73, i2 - 20);
			chatTextDrawingArea.drawChatInput(0xffffff, j1, "Cancel", i2 + 5, true);
		}
		aRSImageProducer_1109.drawGraphics(171, base.graphics, 202);
		if (welcomeScreenRaised) {
			welcomeScreenRaised = false;
			aRSImageProducer_1107.drawGraphics(0, base.graphics, 128);
			aRSImageProducer_1108.drawGraphics(371, base.graphics, 202);
			aRSImageProducer_1112.drawGraphics(265, base.graphics, 0);
			aRSImageProducer_1113.drawGraphics(265, base.graphics, 562);
			aRSImageProducer_1114.drawGraphics(171, base.graphics, 128);
			aRSImageProducer_1115.drawGraphics(171, base.graphics, 562);
		}
	}
	
	private void drawFlames() {
		drawingFlames = true;
		try {
			long l = NetDrawingTools.CurrentTimeMillis();
			int i = 0;
			int j = 20;
			while (aBoolean831) {
				anInt1208++;
				calcFlamesPosition();
				calcFlamesPosition();
				doFlamesDrawing();
				if (++i > 10) {
					long l1 = NetDrawingTools.CurrentTimeMillis();
					int k = (int) (l1 - l) / 10 - j;
					j = 40 - k;
					if (j < 5)
						j = 5;
					i = 0;
					l = l1;
				}
				try {
					//Thread.Sleep(j);
				} catch (Exception _ex) {
				}
			}
		} catch (Exception _ex) {
		}
		drawingFlames = false;
	}

	public void raiseWelcomeScreen() {
		welcomeScreenRaised = true;
	}
	
	private void method137(RS2Sharp.Stream stream, int j) {
		if (j == 84) {
			int k = stream.readUnsignedByte();
			int j3 = anInt1268 + (k >> 4 & 7);
			int i6 = anInt1269 + (k & 7);
			int l8 = stream.readUnsignedWord();
			int k11 = stream.readUnsignedWord();
			int l13 = stream.readUnsignedWord();
			if (j3 >= 0 && i6 >= 0 && j3 < 104 && i6 < 104) {
				NodeList class19_1 = groundArray[plane][j3][i6];
				if (class19_1 != null) {
					for (Item class30_sub2_sub4_sub2_3 = (Item) class19_1
					     .reverseGetFirst(); class30_sub2_sub4_sub2_3 != null; class30_sub2_sub4_sub2_3 = (Item) class19_1
					     .reverseGetNext()) {
						if (class30_sub2_sub4_sub2_3.ID != (l8 & 0x7fff)
						    || class30_sub2_sub4_sub2_3.anInt1559 != k11)
							continue;
						class30_sub2_sub4_sub2_3.anInt1559 = l13;
						break;
					}
					
					spawnGroundItem(j3, i6);
				}
			}
			return;
		}
		if (j == 105) {
			int l = stream.readUnsignedByte();
			int k3 = anInt1268 + (l >> 4 & 7);
			int j6 = anInt1269 + (l & 7);
			int i9 = stream.readUnsignedWord();
			int l11 = stream.readUnsignedByte();
			int i14 = l11 >> 4 & 0xf;
			int i16 = l11 & 7;
			if (myPlayer.smallX[0] >= k3 - i14
			    && myPlayer.smallX[0] <= k3 + i14
			    && myPlayer.smallY[0] >= j6 - i14
			    && myPlayer.smallY[0] <= j6 + i14 && aBoolean848 && !lowMem
			    && anInt1062 < 50) {
				anIntArray1207[anInt1062] = i9;
				anIntArray1241[anInt1062] = i16;
				anIntArray1250[anInt1062] = Sounds.anIntArray326[i9];
				anInt1062++;
			}
		}
		if (j == 215) {
			int i1 = stream.method435();
			int l3 = stream.method428();
			int k6 = anInt1268 + (l3 >> 4 & 7);
			int j9 = anInt1269 + (l3 & 7);
			int i12 = stream.method435();
			int j14 = stream.readUnsignedWord();
			if (k6 >= 0 && j9 >= 0 && k6 < 104 && j9 < 104
			    && i12 != localPlayerIndex) {
				Item class30_sub2_sub4_sub2_2 = new Item();
				class30_sub2_sub4_sub2_2.ID = i1;
				class30_sub2_sub4_sub2_2.anInt1559 = j14;
				if (groundArray[plane][k6][j9] == null)
					groundArray[plane][k6][j9] = new NodeList();
				groundArray[plane][k6][j9].insertHead(class30_sub2_sub4_sub2_2);
				spawnGroundItem(k6, j9);
			}
			return;
		}
		if (j == 156) {
			int j1 = stream.method426();
			int i4 = anInt1268 + (j1 >> 4 & 7);
			int l6 = anInt1269 + (j1 & 7);
			int k9 = stream.readUnsignedWord();
			if (i4 >= 0 && l6 >= 0 && i4 < 104 && l6 < 104) {
				NodeList class19 = groundArray[plane][i4][l6];
				if (class19 != null) {
					for (Item item = (Item) class19.reverseGetFirst(); item != null; item = (Item) class19
					     .reverseGetNext()) {
						if (item.ID != (k9 & 0x7fff))
							continue;
						item.unlink();
						break;
					}
					
					if (class19.reverseGetFirst() == null)
						groundArray[plane][i4][l6] = null;
					spawnGroundItem(i4, l6);
				}
			}
			return;
		}
		if (j == 160) {
			int k1 = stream.method428();
			int j4 = anInt1268 + (k1 >> 4 & 7);
			int i7 = anInt1269 + (k1 & 7);
			int l9 = stream.method428();
			int j12 = l9 >> 2;
			int k14 = l9 & 3;
			int j16 = anIntArray1177[j12];
			int j17 = stream.method435();
			if (j4 >= 0 && i7 >= 0 && j4 < 103 && i7 < 103) {
				int j18 = intGroundArray[plane][j4][i7];
				int i19 = intGroundArray[plane][j4 + 1][i7];
				int l19 = intGroundArray[plane][j4 + 1][i7 + 1];
				int k20 = intGroundArray[plane][j4][i7 + 1];
				if (j16 == 0) {
//					WallObject class10 = 0;//worldController.method296(plane, j4, i7);
//					if (class10 != null) {
//						int k21 = class10.uid >> 14 & 0x7fff;
//						if (j12 == 2) {
//							class10.aClass30_Sub2_Sub4_278 = new Animable_Sub5(
//								k21, 4 + k14, 2, i19, l19, j18, k20, j17,
//								false);
//							class10.aClass30_Sub2_Sub4_279 = new Animable_Sub5(
//								k21, k14 + 1 & 3, 2, i19, l19, j18, k20,
//								j17, false);
//						} else {
//							class10.aClass30_Sub2_Sub4_278 = new Animable_Sub5(
//								k21, k14, j12, i19, l19, j18, k20, j17,
//								false);
//						}
//					}
				}
				if (j16 == 1) {
//					WallDecoration class26 = 0;//worldController.method297(j4, i7, plane);
//					if (class26 != null)
//						class26.aClass30_Sub2_Sub4_504 = new Animable_Sub5(
//							class26.uid >> 14 & 0x7fff, 0, 4, i19, l19,
//							j18, k20, j17, false);
				}
				if (j16 == 2) {
//					InteractiveObject class28 = 0;//worldController.method298(j4, i7, plane);
//					if (j12 == 11)
//						j12 = 10;
//					if (class28 != null)
//						class28.renderable = new Animable_Sub5(
//							class28.uid >> 14 & 0x7fff, k14, j12, i19, l19,
//							j18, k20, j17, false);
				}
				if (j16 == 3) {
//					GroundDecoration class49 = 0;//worldController.method299(i7, j4, plane);
//					if (class49 != null)
//						class49.aClass30_Sub2_Sub4_814 = new Animable_Sub5(
//							class49.uid >> 14 & 0x7fff, k14, 22, i19, l19,
//							j18, k20, j17, false);
				}
			}
			return;
		}
		if (j == 147) {
			int l1 = stream.method428();
			int k4 = anInt1268 + (l1 >> 4 & 7);
			int j7 = anInt1269 + (l1 & 7);
			int i10 = stream.readUnsignedWord();
			byte byte0 = stream.method430();
			int l14 = stream.method434();
			byte byte1 = stream.method429();
			int k17 = stream.readUnsignedWord();
			int k18 = stream.method428();
			int j19 = k18 >> 2;
			int i20 = k18 & 3;
			int l20 = anIntArray1177[j19];
			byte byte2 = (byte)stream.readSignedByte();
			int l21 = stream.readUnsignedWord();
			byte byte3 = stream.method429();
			Player player;
			if (i10 == localPlayerIndex)
				player = myPlayer;
			else
				player = playerArray[i10];
			if (player != null) {
				ObjectDef class46 = ObjectDef.forID(l21);
				int i22 = intGroundArray[plane][k4][j7];
				int j22 = intGroundArray[plane][k4 + 1][j7];
				int k22 = intGroundArray[plane][k4 + 1][j7 + 1];
				int l22 = intGroundArray[plane][k4][j7 + 1];
				Model model = class46.method578(j19, i20, i22, j22, k22, l22,
				                                -1);
				if (model != null) {
					method130(k17 + 1, -1, 0, l20, j7, 0, plane, k4, l14 + 1);
					player.anInt1707 = l14 + loopCycle;
					player.anInt1708 = k17 + loopCycle;
					player.aModel_1714 = model;
					int i23 = class46.anInt744;
					int j23 = class46.anInt761;
					if (i20 == 1 || i20 == 3) {
						i23 = class46.anInt761;
						j23 = class46.anInt744;
					}
					player.anInt1711 = k4 * 128 + i23 * 64;
					player.anInt1713 = j7 * 128 + j23 * 64;
					player.anInt1712 = method42(plane, player.anInt1713,
					                            player.anInt1711);
					if (byte2 > byte0) {
						byte byte4 = byte2;
						byte2 = byte0;
						byte0 = byte4;
					}
					if (byte3 > byte1) {
						byte byte5 = byte3;
						byte3 = byte1;
						byte1 = byte5;
					}
					player.anInt1719 = k4 + byte2;
					player.anInt1721 = k4 + byte0;
					player.anInt1720 = j7 + byte3;
					player.anInt1722 = j7 + byte1;
				}
			}
		}
		if (j == 151) {
			int i2 = stream.method426();
			int l4 = anInt1268 + (i2 >> 4 & 7);
			int k7 = anInt1269 + (i2 & 7);
			int j10 = stream.method434();
			int k12 = stream.method428();
			int i15 = k12 >> 2;
			int k16 = k12 & 3;
			int l17 = anIntArray1177[i15];
			if (l4 >= 0 && k7 >= 0 && l4 < 104 && k7 < 104)
				method130(-1, j10, k16, l17, k7, i15, plane, l4, 0);
			return;
		}
		if (j == 4) {
			int j2 = stream.readUnsignedByte();
			int i5 = anInt1268 + (j2 >> 4 & 7);
			int l7 = anInt1269 + (j2 & 7);
			int k10 = stream.readUnsignedWord();
			int l12 = stream.readUnsignedByte();
			int j15 = stream.readUnsignedWord();
			if (i5 >= 0 && l7 >= 0 && i5 < 104 && l7 < 104) {
				i5 = i5 * 128 + 64;
				l7 = l7 * 128 + 64;
				Animable_Sub3 class30_sub2_sub4_sub3 = new Animable_Sub3(plane,
				                                                         loopCycle, j15, k10, method42(plane, l7, i5) - l12, l7,
				                                                         i5);
				aClass19_1056.insertHead(class30_sub2_sub4_sub3);
			}
			return;
		}
		if (j == 44) {
			int k2 = stream.method436();
			int j5 = stream.readUnsignedWord();
			int i8 = stream.readUnsignedByte();
			int l10 = anInt1268 + (i8 >> 4 & 7);
			int i13 = anInt1269 + (i8 & 7);
			if (l10 >= 0 && i13 >= 0 && l10 < 104 && i13 < 104) {
				Item class30_sub2_sub4_sub2_1 = new Item();
				class30_sub2_sub4_sub2_1.ID = k2;
				class30_sub2_sub4_sub2_1.anInt1559 = j5;
				if (groundArray[plane][l10][i13] == null)
					groundArray[plane][l10][i13] = new NodeList();
				groundArray[plane][l10][i13]
				.insertHead(class30_sub2_sub4_sub2_1);
				spawnGroundItem(l10, i13);
			}
			return;
		}
		if (j == 101) {
			int l2 = stream.method427();
			int k5 = l2 >> 2;
			int j8 = l2 & 3;
			int i11 = anIntArray1177[k5];
			int j13 = stream.readUnsignedByte();
			int k15 = anInt1268 + (j13 >> 4 & 7);
			int l16 = anInt1269 + (j13 & 7);
			if (k15 >= 0 && l16 >= 0 && k15 < 104 && l16 < 104)
				method130(-1, -1, j8, i11, l16, k5, plane, k15, 0);
			return;
		}
		if (j == 117) {
			int i3 = stream.readUnsignedByte();
			int l5 = anInt1268 + (i3 >> 4 & 7);
			int k8 = anInt1269 + (i3 & 7);
			int j11 = l5 + stream.readSignedByte();
			int k13 = k8 + stream.readSignedByte();
			int l15 = stream.readSignedWord();
			int i17 = stream.readUnsignedWord();
			int i18 = stream.readUnsignedByte() * 4;
			int l18 = stream.readUnsignedByte() * 4;
			int k19 = stream.readUnsignedWord();
			int j20 = stream.readUnsignedWord();
			int i21 = stream.readUnsignedByte();
			int j21 = stream.readUnsignedByte();
			if (l5 >= 0 && k8 >= 0 && l5 < 104 && k8 < 104 && j11 >= 0
			    && k13 >= 0 && j11 < 104 && k13 < 104 && i17 != 65535) {
				l5 = l5 * 128 + 64;
				k8 = k8 * 128 + 64;
				j11 = j11 * 128 + 64;
				k13 = k13 * 128 + 64;
				Projectile class30_sub2_sub4_sub4 = new Projectile(i21,
				                                                         l18, k19 + loopCycle, j20 + loopCycle, j21, plane,
				                                                         method42(plane, k8, l5) - i18, k8, l5, l15, i17);
				class30_sub2_sub4_sub4.target(j11, j11, method42(plane, k13, j11) - l18,
				                               k19 + loopCycle);
				//k19 + loopCycle, k13,
				//                                 method42(plane, k13, j11) - l18, j11);
				projectiles.insertHead(class30_sub2_sub4_sub4);
			}
		}
	}
	
	private static void setLowMem() {
		//WorldController.lowMem = true;
		RS2Sharp.Texture.lowMem = true;
		lowMem = true;
		//ObjectManager.lowMem = true;
		ObjectDef.lowMem = true;
	}
	
	private void method139(RS2Sharp.Stream stream) {
//		stream.initBitAccess();
//		int k = stream.readBits(8);
//		if (k < npcCount) {
//			for (int l = k; l < npcCount; l++)
//				anIntArray840[anInt839++] = npcIndices[l];
//			
//		}
//		if (k > npcCount) {
//			signlink.reporterror(myUsername + " Too many npcs");
//			throw new Exception("eek");
//		}
//		npcCount = 0;
//		for (int i1 = 0; i1 < k; i1++) {
//			int j1 = npcIndices[i1];
//			NPC npc = npcArray[j1];
//			int k1 = stream.readBits(1);
//			if (k1 == 0) {
//				npcIndices[npcCount++] = j1;
//				npc.anInt1537 = loopCycle;
//			} else {
//				int l1 = stream.readBits(2);
//				if (l1 == 0) {
//					npcIndices[npcCount++] = j1;
//					npc.anInt1537 = loopCycle;
//					anIntArray894[anInt893++] = j1;
//				} else if (l1 == 1) {
//					npcIndices[npcCount++] = j1;
//					npc.anInt1537 = loopCycle;
//					int i2 = stream.readBits(3);
//					npc.moveInDir(false, i2);
//					int k2 = stream.readBits(1);
//					if (k2 == 1)
//						anIntArray894[anInt893++] = j1;
//				} else if (l1 == 2) {
//					npcIndices[npcCount++] = j1;
//					npc.anInt1537 = loopCycle;
//					int j2 = stream.readBits(3);
//					npc.moveInDir(true, j2);
//					int l2 = stream.readBits(3);
//					npc.moveInDir(true, l2);
//					int i3 = stream.readBits(1);
//					if (i3 == 1)
//						anIntArray894[anInt893++] = j1;
//				} else if (l1 == 3)
//					anIntArray840[anInt839++] = j1;
//			}
//		}

		stream.initBitAccess();
		int k = stream.readBits(8);
		if(k < npcCount)
		{
			for(int l = k; l < npcCount; l++)
				anIntArray840[anInt839++] = npcIndices[l];
			
		}
		if(k > npcCount)
		{
			signlink.reporterror(myUsername + " Too many npcs");
			throw new Exception("eek");
		}
		npcCount = 0;
		for(int i1 = 0; i1 < k; i1++)
		{
			int j1 = npcIndices[i1];
			NPC npc = npcArray[j1];
			int k1 = stream.readBits(1);
			if(k1 == 0)
			{
				npcIndices[npcCount++] = j1;
				npc.anInt1537 = loopCycle;
			} else
			{
				int l1 = stream.readBits(2);
				if(l1 == 0)
				{
					npcIndices[npcCount++] = j1;
					npc.anInt1537 = loopCycle;
					anIntArray894[anInt893++] = j1;
				} else
					if(l1 == 1)
				{
					npcIndices[npcCount++] = j1;
					npc.anInt1537 = loopCycle;
					int i2 = stream.readBits(3);
					npc.moveInDir(false, i2);
					int k2 = stream.readBits(1);
					if(k2 == 1)
						anIntArray894[anInt893++] = j1;
				} else
					if(l1 == 2)
				{
					npcIndices[npcCount++] = j1;
					npc.anInt1537 = loopCycle;
					int j2 = stream.readBits(3);
					npc.moveInDir(true, j2);
					int l2 = stream.readBits(3);
					npc.moveInDir(true, l2);
					int i3 = stream.readBits(1);
					if(i3 == 1)
						anIntArray894[anInt893++] = j1;
				} else
					if(l1 == 3)
						anIntArray840[anInt839++] = j1;
			}
		}
						
	}
	
	public RS2Sharp.Sprite camera, disabledCamera, cameraHover, disabledCameraHover;
	public String detail = "";
	public String screenSize = "";
	
	private void processLoginScreenInput() {
		if (base.clickMode3 == 1 && base.saveClickX >= 0
		    && base.saveClickX <= 32 && base.saveClickY >= 469
		    && base.saveClickY <= 501)
			isAnimated = !isAnimated;
		if (loginScreenState == 0) { // welcome screen
			if (base.clickMode3 == 1 && base.saveClickX >= 295
			    && base.saveClickX <= 468)
			if (base.saveClickY >= 160 && base.saveClickY <= 182) {
				loginScreenState = 1;
			}
			if (base.clickMode3 == 1 && base.saveClickX >= 316
			    && base.saveClickX <= 451)
			if (base.saveClickY >= 257 && base.saveClickY <= 273) {
				loginScreenState = 3;
			}
			if (base.clickMode3 == 1 && base.saveClickX >= 361
			    && base.saveClickX <= 400)
			if (base.saveClickY >= 396 && base.saveClickY <= 411) {
				//System.exit(1);
			}
			if (base.clickMode3 == 1 && base.saveClickX >= 305
			    && base.saveClickX <= 361 && base.saveClickY >= 280
			    && base.saveClickY <= 311) {
				detail = "sd";
			}
			if (base.clickMode3 == 1 && base.saveClickX >= 401
			    && base.saveClickX <= 456 && base.saveClickY >= 280
			    && base.saveClickY <= 311) {
				detail = "hd";
			}
		} else if (loginScreenState == 1) { // login screen
			if (base.clickMode3 == 1 && base.saveClickX >= 332
			    && base.saveClickX <= 422)
			if (base.saveClickY >= 389 && base.saveClickY <= 404) {
				
				loginScreenState = 0;
			}
			if (base.clickMode3 == 1 && base.saveClickX >= 350
			    && base.saveClickX <= 406 && base.saveClickY >= 351
			    && base.saveClickY <= 367) {
				loginFailures = 0;
				login(myUsername, myPassword, false);
				if (loggedIn)
					return;
			}
			if (base.clickMode3 == 1 && base.saveClickX >= 310
			    && base.saveClickX <= 452 && base.saveClickY >= 246
			    && base.saveClickY <= 261) {
				loginScreenCursorPos = 0;
			}
			if (base.clickMode3 == 1 && base.saveClickX >= 309
			    && base.saveClickX <= 452 && base.saveClickY >= 315
			    && base.saveClickY <= 330) {
				loginScreenCursorPos = 1;
			}
		} else if (loginScreenState == 3) { // settings screen
			if (base.clickMode3 == 1 && base.saveClickX >= 596
			    && base.saveClickX <= 622)
			if (base.saveClickY >= 86 && base.saveClickY <= 110) {
				loginScreenState = 0;
			}
			if (base.clickMode3 == 1 && base.saveClickX >= 293
			    && base.saveClickX <= 347 && base.saveClickY >= 155
			    && base.saveClickY <= 186) {
				detail = "sd";
			}
			if (base.clickMode3 == 1 && base.saveClickX >= 413
			    && base.saveClickX <= 467 && base.saveClickY >= 155
			    && base.saveClickY <= 186) {
				detail = "hd";
			}
			if (base.clickMode3 == 1 && base.saveClickX >= 264
			    && base.saveClickX <= 317 && base.saveClickY >= 218
			    && base.saveClickY <= 252) {
				screenSize = "fixed";
			}
			
			if (base.clickMode3 == 1 && base.saveClickX >= 354
			    && base.saveClickX <= 408 && base.saveClickY >= 218
			    && base.saveClickY <= 252) {
				screenSize = "resizable";
			}
			
			if (base.clickMode3 == 1 && base.saveClickX >= 444
			    && base.saveClickX <= 497 && base.saveClickY >= 218
			    && base.saveClickY <= 252) {
				screenSize = "fullscreen";
			}
		} else {
			if (loginScreenState == 2) {
				int j = base.myHeight / 2 - 40;
				j += 30;
				j += 25;
				j += 15;
				int i1 = base.myWidth / 2 - 80;
				int k1 = base.myHeight / 2 + 50;
				k1 += 20;
				if (base.clickMode3 == 1 && base.saveClickX >= i1 - 75
				    && base.saveClickX <= i1 + 75
				    && base.saveClickY >= k1 - 20
				    && base.saveClickY <= k1 + 20) {
					loginFailures = 0;
					login(myUsername, myPassword, false);
					if (loggedIn)
						return;
				}
				i1 = base.myWidth / 2 + 80;
				if (base.clickMode3 == 1 && base.saveClickX >= i1 - 75
				    && base.saveClickX <= i1 + 75
				    && base.saveClickY >= k1 - 20
				    && base.saveClickY <= k1 + 20) {
					loginScreenState = 0;
					// myUsername = "";
					// myPassword = "";
				}
// 				do {
// 					int l1 = readChar(-796);
// 					if (l1 == -1)
// 						break;
// 					bool flag1 = false;
// 					for (int i2 = 0; i2 < validUserPassChars.Length; i2++) {
// 						if (l1 != validUserPassChars[i2])
// 							continue;
// 						flag1 = true;
// 						break;
// 					}
// 					
// 					if (loginScreenCursorPos == 0) {
// 						if (l1 == 8 && myUsername.Length > 0)
// 							myUsername = myUsername.Substring(0,
// 							                                  myUsername.Length - 1);
// 						if (l1 == 9 || l1 == 10 || l1 == 13)
// 							loginScreenCursorPos = 1;
// 						if (flag1)
// 							myUsername += (char) l1;
// 						if (myUsername.Length > 12)
// 							myUsername = myUsername.Substring(0, 12);
// 					} else if (loginScreenCursorPos == 1) {
// 						if (l1 == 8 && myPassword.Length > 0)
// 							myPassword = myPassword.Substring(0,
// 							                                  myPassword.Length - 1);
// 						if (l1 == 9 || l1 == 10 || l1 == 13)
// 							loginScreenCursorPos = 0;
// 						if (flag1)
// 							myPassword += (char) l1;
// 						if (myPassword.Length > 20)
// 							myPassword = myPassword.Substring(0, 20);
// 					}
// 				} while (true);
				return;
			}
			if (loginScreenState == 3) {
				int k = base.myWidth / 2;
				int j1 = base.myHeight / 2 + 50;
				j1 += 20;
				if (base.clickMode3 == 1 && base.saveClickX >= k - 75
				    && base.saveClickX <= k + 75
				    && base.saveClickY >= j1 - 20
				    && base.saveClickY <= j1 + 20)
					loginScreenState = 0;
			}
		}
	}
	
	private void markMinimap(RS2Sharp.Sprite sprite, int i, int j)
	{
			int extraWidth = myWidth -765 - 11;
		int x = 549+extraWidth;
		int markY = 6;
		int markX = 9;
		int k = minimapInt1 + minimapInt2 & 0x7ff;
		int l = i * i + j * j;
		if(l > 6400)
			return;
		int i1 = Model.SINE[k];
		int j1 = Model.COSINE[k];
		i1 = (i1 * 256) / (minimapInt3 + 256);
		j1 = (j1 * 256) / (minimapInt3 + 256);
		int k1 = j * i1 + i * j1 >> 16;
		int l1 = j * j1 - i * i1 >> 16;
		if(l > 2500) {
			sprite.drawSprite((((94 + k1) - sprite.anInt1444 / 2) + 4) + x + markX, (83 - l1 - sprite.anInt1445 / 2 - 4)+ markY);
		} else {
			sprite.drawSprite((((94 + k1) - sprite.anInt1444 / 2) + 4)+ x + markX, (83 - l1 - sprite.anInt1445 / 2 - 4)+ markY);
		}
	}
	
	private void method142(int i, int j, int k, int l, int i1, int j1, int k1) {
		Debug.Log ("method142tru");
		if (i1 >= 1 && i >= 1 && i1 <= 102 && i <= 102) {
			if (lowMem && j != plane)
				return;
				Debug.Log ("method142");
			int i2 = 0;
			if (j1 == 0)
				i2 = 0;//worldController.method300(j, i1, i);
			if (j1 == 1)
				i2 = 0;//worldController.method301(j, i1, i);
			if (j1 == 2)
				i2 = 0;//worldController.method302(j, i1, i);
			if (j1 == 3)
				i2 = 0;//worldController.method303(j, i1, i);
			if (i2 != 0) {
				int i3 = 0;//worldController.method304(j, i1, i, i2);
				int j2 = i2 >> 14 & 0x7fff;
				int k2 = i3 & 0x1f;
				int l2 = i3 >> 6;
				if (j1 == 0) {
					//worldController.method291(i1, j, i, unchecked((byte)-119));
					ObjectDef class46 = ObjectDef.forID(j2);
					if (class46.aBoolean767)
						aClass11Array1230[j].method215(l2, k2,
						                               class46.aBoolean757, i1, i);
				}
				if (j1 == 1)
					//worldController.method292(i, j, i1);
				if (j1 == 2) {
					//worldController.method293(j, i1, i);
					ObjectDef class46_1 = ObjectDef.forID(j2);
					if (i1 + class46_1.anInt744 > 103
					    || i + class46_1.anInt744 > 103
					    || i1 + class46_1.anInt761 > 103
					    || i + class46_1.anInt761 > 103)
						return;
					if (class46_1.aBoolean767)
						aClass11Array1230[j].method216(l2, class46_1.anInt744,
						                               i1, i, class46_1.anInt761,
						                               class46_1.aBoolean757);
				}
				if (j1 == 3) {
					//worldController.method294(j, i, i1);
					ObjectDef class46_2 = ObjectDef.forID(j2);
					if (class46_2.aBoolean767 && class46_2.hasActions)
						aClass11Array1230[j].method218(i, i1);
				}
			}
			if (k1 >= 0) {
				int j3 = j;
				if (j3 < 3 && (byteGroundArray[1][i1][i] & 2) == 2)
					j3++;
				//ObjectManager.method188(//worldController, k, i, l, j3,
				  //                      aClass11Array1230[j], intGroundArray, i1, k1, j);
			}
		}
	}
	
	private void updatePlayers(int i, RS2Sharp.Stream stream) {
		anInt839 = 0;
		anInt893 = 0;
		method117(stream);
		method134(stream);
		method91(stream, i);
		method49(stream);
		for (int k = 0; k < anInt839; k++) {
			int l = anIntArray840[k];
			if (playerArray[l].anInt1537 != loopCycle)
				playerArray[l] = null;
		}
		
		if (stream.currentOffset != i) {
			signlink.reporterror("Error packet size mismatch in getplayer pos:"
			                     + stream.currentOffset + " psize:" + i);
			throw new Exception("eek");
		}
		for (int i1 = 0; i1 < playerCount; i1++)
		if (playerArray[playerIndices[i1]] == null) {
			signlink.reporterror(myUsername
			                     + " null entry in pl list - pos:" + i1 + " size:"
			                     + playerCount);
			throw new Exception("eek");
		}
		
	}
	
	private void setCameraPos(int j, int k, int l, int i1, int j1, int k1) {
		int l1 = 2048 - k & 0x7ff;
		int i2 = 2048 - j1 & 0x7ff;
		int j2 = 0;
		int k2 = 0;
		int l2 = j;
		if (l1 != 0) {
			int i3 = Model.SINE[l1];
			int k3 = Model.COSINE[l1];
			int i4 = k2 * k3 - l2 * i3 >> 16;
			l2 = k2 * i3 + l2 * k3 >> 16;
			k2 = i4;
		}
		if (i2 != 0) {
			/*
			 * xxx if(cameratoggle){ if(zoom == 0) zoom = k2; if(lftrit == 0)
			 * lftrit = j2; if(fwdbwd == 0) fwdbwd = l2; k2 = zoom; j2 = lftrit;
			 * l2 = fwdbwd; }
			 */
			int j3 = Model.SINE[i2];
			int l3 = Model.COSINE[i2];
			int j4 = l2 * j3 + j2 * l3 >> 16;
			l2 = l2 * l3 - j2 * j3 >> 16;
			j2 = j4;
		}
		xCameraPos = l - j2;
		zCameraPos = i1 - k2;
		yCameraPos = k1 - l2;
		yCameraCurve = k;
		xCameraCurve = j1;
	}
	
	public void updateStrings(String str, int i) {
		switch (i) {
		case 1675:
			sendFrame126(str, 17508);
			break;// Stab
		case 1676:
			sendFrame126(str, 17509);
			break;// Slash
		case 1677:
			sendFrame126(str, 17510);
			break;// Cursh
		case 1678:
			sendFrame126(str, 17511);
			break;// Magic
		case 1679:
			sendFrame126(str, 17512);
			break;// Range
		case 1680:
			sendFrame126(str, 17513);
			break;// Stab
		case 1681:
			sendFrame126(str, 17514);
			break;// Slash
		case 1682:
			sendFrame126(str, 17515);
			break;// Crush
		case 1683:
			sendFrame126(str, 17516);
			break;// Magic
		case 1684:
			sendFrame126(str, 17517);
			break;// Range
		case 1686:
			sendFrame126(str, 17518);
			break;// Strength
		case 1687:
			sendFrame126(str, 17519);
			break;// Prayer
		}
	}
	
	public void sendFrame126(String str, int i) {
		if(RSInterface.interfaceCache.Length <= i || RSInterface.interfaceCache[i] == null)
		{
			Debug.Log("Missing interface : " + i);
			return;
		}
		RSInterface.interfaceCache[i].message = str;
		if (RSInterface.interfaceCache[i].parentID == tabInterfaceIDs[tabID])
			needDrawTabArea = true;
	}
	
	public void sendPacket185(int button, int toggle, int type) {
		switch (type) {
		case 135:
			RSInterface class9 = RSInterface.interfaceCache[button];
			bool flag8 = true;
			if (class9.anInt214 > 0)
				flag8 = promptUserForInput(class9);
			if (flag8) {
				stream.createFrame(185);
				stream.writeWord(button);
			}
			break;
		case 646:
			stream.createFrame(185);
			stream.writeWord(button);
			RSInterface class9_2 = RSInterface.interfaceCache[button];
			if (class9_2.valueIndexArray != null
			    && class9_2.valueIndexArray[0][0] == 5) {
				if (variousSettings[toggle] != class9_2.anIntArray212[0]) {
					variousSettings[toggle] = class9_2.anIntArray212[0];
					method33(toggle);
					needDrawTabArea = true;
				}
			}
			break;
		case 169:
			stream.createFrame(185);
			stream.writeWord(button);
			RSInterface class9_3 = RSInterface.interfaceCache[button];
			if (class9_3.valueIndexArray != null
			    && class9_3.valueIndexArray[0][0] == 5) {
				variousSettings[toggle] = 1 - variousSettings[toggle];
				method33(toggle);
				needDrawTabArea = true;
			}
			switch (button) {
			case 19136:
				UnityEngine.Debug.Log("toggle = " + toggle);
				if (toggle == 0)
					sendFrame36(173, toggle);
				if (toggle == 1)
					sendPacket185(153, 173, 646);
				break;
			}
			break;
		}
	}
	
	public void sendFrame36(int id, int state) {
		anIntArray1045[id] = state;
		if (variousSettings[id] != state) {
			variousSettings[id] = state;
			method33(id);
			needDrawTabArea = true;
			if (dialogID != -1)
				inputTaken = true;
		}
	}
	
	public void sendFrame219() {
		if (invOverlayInterfaceID != -1) {
			invOverlayInterfaceID = -1;
			needDrawTabArea = true;
			tabAreaAltered = true;
		}
		if (backDialogID != -1) {
			backDialogID = -1;
			inputTaken = true;
		}
		if (inputDialogState != 0) {
			inputDialogState = 0;
			inputTaken = true;
		}
		openInterfaceID = -1;
		aBoolean1149 = false;
	}
	
	public void sendFrame248(int interfaceID, int sideInterfaceID) {
		if (backDialogID != -1) {
			backDialogID = -1;
			inputTaken = true;
		}
		if (inputDialogState != 0) {
			inputDialogState = 0;
			inputTaken = true;
		}
		openInterfaceID = interfaceID;
		invOverlayInterfaceID = sideInterfaceID;
		needDrawTabArea = true;
		tabAreaAltered = true;
		aBoolean1149 = false;
	}
	
	private bool parsePacket() {
//		if (socketStream == null)
//			return false;
//		try {
//			int i = socketStream.available();
//			if (i == 0)
//				return false;
//			if (pktType == -1) {
//				socketStream.flushInputStream(inStream.buffer, 1);
//				pktType = inStream.buffer[0] & 0xff;
//				if (encryption != null)
//					pktType = pktType - encryption.getNextKey() & 0xff;
//				pktSize = SizeConstants.packetSizes[pktType];
//				i--;
//			}
//			if (pktSize == -1)
//			if (i > 0) {
//				socketStream.flushInputStream(inStream.buffer, 1);
//				pktSize = inStream.buffer[0] & 0xff;
//				i--;
//			} else {
//				return false;
//			}
//			if (pktSize == -2)
//			if (i > 1) {
//				socketStream.flushInputStream(inStream.buffer, 2);
//				inStream.currentOffset = 0;
//				pktSize = inStream.readUnsignedWord();
//				i -= 2;
//			} else {
//				return false;
//			}
//			if (i < pktSize)
//				return false;
//			inStream.currentOffset = 0;
//			socketStream.flushInputStream(inStream.buffer, pktSize);
//			anInt1009 = 0;
//			anInt843 = anInt842;
//			anInt842 = anInt841;
//			anInt841 = pktType;
//			switch (pktType) {
//			case 81:
//				updatePlayers(pktSize, inStream);
//				aBoolean1080 = false;
//				pktType = -1;
//				return true;
//				
//			case 176:
//				daysSinceRecovChange = inStream.method427();
//				unreadMessages = inStream.method435();
//				membersInt = inStream.readUnsignedByte();
//				anInt1193 = inStream.method440();
//				daysSinceLastLogin = inStream.readUnsignedWord();
//				if (anInt1193 != 0 && openInterfaceID == -1) {
//					signlink.dnslookup(TextClass.method586(anInt1193));
//					clearTopInterfaces();
//					char c = '\u028A';
//					if (daysSinceRecovChange != 201 || membersInt == 1)
//						c = '\u028F';
//					reportAbuseInput = "";
//					canMute = false;
//					for (int k9 = 0; k9 < RSInterface.interfaceCache.Length; k9++) {
//						if (RSInterface.interfaceCache[k9] == null
//						    || RSInterface.interfaceCache[k9].anInt214 != c)
//							continue;
//						openInterfaceID = RSInterface.interfaceCache[k9].parentID;
//						
//					}
//				}
//				pktType = -1;
//				return true;
//				
//			case 64:
//				anInt1268 = inStream.method427();
//				anInt1269 = inStream.method428();
//				for (int j = anInt1268; j < anInt1268 + 8; j++) {
//					for (int l9 = anInt1269; l9 < anInt1269 + 8; l9++)
//					if (groundArray[plane][j][l9] != null) {
//						groundArray[plane][j][l9] = null;
//						spawnGroundItem(j, l9);
//					}
//				}
//				for (Class30_Sub1 class30_sub1 = (Class30_Sub1) aClass19_1179
//				     .reverseGetFirst(); class30_sub1 != null; class30_sub1 = (Class30_Sub1) aClass19_1179
//				     .reverseGetNext())
//					if (class30_sub1.anInt1297 >= anInt1268
//					    && class30_sub1.anInt1297 < anInt1268 + 8
//					    && class30_sub1.anInt1298 >= anInt1269
//					    && class30_sub1.anInt1298 < anInt1269 + 8
//					    && class30_sub1.anInt1295 == plane)
//						class30_sub1.anInt1294 = 0;
//				pktType = -1;
//				return true;
//				
//			case 123: pktType = -1; return true;
//				
//			case 185:
//				int k = inStream.method436();
//				RSInterface.interfaceCache[k].anInt233 = 3;
//				if (myPlayer.desc == null)
//					RSInterface.interfaceCache[k].mediaID = (myPlayer.anIntArray1700[0] << 25)
//						+ (myPlayer.anIntArray1700[4] << 20)
//						+ (myPlayer.equipment[0] << 15)
//						+ (myPlayer.equipment[8] << 10)
//						+ (myPlayer.equipment[11] << 5)
//						+ myPlayer.equipment[1];
//				else
//					RSInterface.interfaceCache[k].mediaID = (int) (0x12345678L + myPlayer.desc.type);
//				pktType = -1;
//				return true;
//				
//				/* Clan message packet */
//			case 217:
//				try {
//					name = inStream.readString();
//					message = inStream.readString();
//					clanname = inStream.readString();
//					rights = inStream.readUnsignedWord();
//					message = TextInput.processText(message);
//					// message = Censor.doCensor(message);
//					UnityEngine.Debug.Log(clanname);
//					pushMessage(message, 16, name);
//				} catch (Exception e) {
//					Debug.Log(e.StackTrace);
//				}
//				pktType = -1;
//				return true;
//				
//			case 107:
//				aBoolean1160 = false;
//				for (int l = 0; l < 5; l++)
//					aBooleanArray876[l] = false;
//				pktType = -1;
//				return true;
//				
//			case 72:
//				int i1 = inStream.method434();
//				RSInterface class9 = RSInterface.interfaceCache[i1];
//				for (int k15 = 0; k15 < class9.inv.Length; k15++) {
//					class9.inv[k15] = -1;
//					class9.inv[k15] = 0;
//				}
//				pktType = -1;
//				return true;
//				
//			case 214:
//				ignoreCount = pktSize / 8;
//				for (int j1 = 0; j1 < ignoreCount; j1++)
//					ignoreListAsLongs[j1] = inStream.readQWord();
//				pktType = -1;
//				return true;
//				
//			case 166:
//				aBoolean1160 = true;
//				anInt1098 = inStream.readUnsignedByte();
//				anInt1099 = inStream.readUnsignedByte();
//				anInt1100 = inStream.readUnsignedWord();
//				anInt1101 = inStream.readUnsignedByte();
//				anInt1102 = inStream.readUnsignedByte();
//				if (anInt1102 >= 100) {
//					xCameraPos = anInt1098 * 128 + 64;
//					yCameraPos = anInt1099 * 128 + 64;
//					zCameraPos = method42(plane, yCameraPos, xCameraPos)
//						- anInt1100;
//				}
//				pktType = -1;
//				return true;
//				
//			case 134:
//				needDrawTabArea = true;
//				int k1 = inStream.readUnsignedByte();
//				int i10 = inStream.method439();
//				int l15 = inStream.readUnsignedByte();
//				currentExp[k1] = i10;
//				currentStats[k1] = l15;
//				maxStats[k1] = 1;
//				for (int k20 = 0; k20 < 98; k20++)
//					if (i10 >= anIntArray1019[k20])
//						maxStats[k1] = k20 + 2;
//				pktType = -1;
//				return true;
//				
//			case 71:
//				int l1 = inStream.readUnsignedWord();
//				int j10 = inStream.method426();
//				if (l1 == 65535)
//					l1 = -1;
//				tabInterfaceIDs[j10] = l1; //lol
//				needDrawTabArea = true;
//				tabAreaAltered = true;
//				pktType = -1;
//				return true;
//				
//			case 74:
//				int i2 = inStream.method434();
//				if (i2 == 65535)
//					i2 = -1;
//				if (i2 != currentSong && musicEnabled && !lowMem
//				    && prevSong == 0) {
//					nextSong = i2;
//					songChanging = true;
//					onDemandFetcher.method558(2, nextSong);
//				}
//				currentSong = i2;
//				pktType = -1;
//				return true;
//				
//			case 121:
//				int j2 = inStream.method436();
//				int k10 = inStream.method435();
//				if (musicEnabled && !lowMem) {
//					nextSong = j2;
//					songChanging = false;
//					onDemandFetcher.method558(2, nextSong);
//					prevSong = k10;
//				}
//				pktType = -1;
//				return true;
//				
//			case 109:
//				resetLogout();
//				pktType = -1;
//				return false;
//				
//			case 70:
//				int k2 = inStream.readSignedWord();
//				int l10 = inStream.method437();
//				int i16 = inStream.method434();
//				RSInterface class9_5 = RSInterface.interfaceCache[i16];
//				class9_5.anInt263 = k2;
//				class9_5.anInt265 = l10;
//				pktType = -1;
//				return true;
//				
//			case 73:
//			case 241:
//				int l2 = anInt1069;
//				int i11 = anInt1070;
//				if (pktType == 73) {
//					l2 = inStream.method435();
//					i11 = inStream.readUnsignedWord();
//					aBoolean1159 = false;
//				}
//				if (pktType == 241) {
//					i11 = inStream.method435();
//					inStream.initBitAccess();
//					for (int j16 = 0; j16 < 4; j16++) {
//						for (int l20 = 0; l20 < 13; l20++) {
//							for (int j23 = 0; j23 < 13; j23++) {
//								int i26 = inStream.readBits(1);
//								if (i26 == 1)
//									anIntArrayArrayArray1129[j16][l20][j23] = inStream
//										.readBits(26);
//								else
//									anIntArrayArrayArray1129[j16][l20][j23] = -1;
//							}
//						}
//					}
//					inStream.finishBitAccess();
//					l2 = inStream.readUnsignedWord();
//					aBoolean1159 = true;
//				}
//				if (anInt1069 == l2 && anInt1070 == i11 && loadingStage == 2) {
//					pktType = -1;
//					return true;
//				}
//				anInt1069 = l2;
//				anInt1070 = i11;
//				baseX = (anInt1069 - 6) * 8;
//				baseY = (anInt1070 - 6) * 8;
//				aBoolean1141 = (anInt1069 / 8 == 48 || anInt1069 / 8 == 49)
//					&& anInt1070 / 8 == 48;
//				if (anInt1069 / 8 == 48 && anInt1070 / 8 == 148)
//					aBoolean1141 = true;
//				loadingStage = 1;
//				aLong824 = NetDrawingTools.CurrentTimeMillis();
//				aRSImageProducer_1165.initDrawingArea();
//				aTextDrawingArea_1271.drawText(0, "Loading - please wait.",
//				                               151, 257);
//				aTextDrawingArea_1271.drawText(0xffffff,
//				                               "Loading - please wait.", 150, 256);
//				aRSImageProducer_1165.drawGraphics(0, base.graphics, 0);
//				if (pktType == 73) {
//					int k16 = 0;
//					for (int i21 = (anInt1069 - 6) / 8; i21 <= (anInt1069 + 6) / 8; i21++) {
//						for (int k23 = (anInt1070 - 6) / 8; k23 <= (anInt1070 + 6) / 8; k23++)
//							k16++;
//					}
//					aByteArrayArray1183 = new byte[k16][];
//					aByteArrayArray1247 = new byte[k16][];
//					anIntArray1234 = new int[k16];
//					anIntArray1235 = new int[k16];
//					anIntArray1236 = new int[k16];
//					k16 = 0;
//					for (int l23 = (anInt1069 - 6) / 8; l23 <= (anInt1069 + 6) / 8; l23++) {
//						for (int j26 = (anInt1070 - 6) / 8; j26 <= (anInt1070 + 6) / 8; j26++) {
//							anIntArray1234[k16] = (l23 << 8) + j26;
//							if (aBoolean1141
//							    && (j26 == 49 || j26 == 149 || j26 == 147
//							    || l23 == 50 || l23 == 49
//							    && j26 == 47)) {
//								anIntArray1235[k16] = -1;
//								anIntArray1236[k16] = -1;
//								k16++;
//							} else {
//								int k28 = anIntArray1235[k16] = onDemandFetcher
//									.method562(0, j26, l23);
//								if (k28 != -1)
//									onDemandFetcher.method558(3, k28);
//								int j30 = anIntArray1236[k16] = onDemandFetcher
//									.method562(1, j26, l23);
//								if (j30 != -1)
//									onDemandFetcher.method558(3, j30);
//								k16++;
//							}
//						}
//					}
//				}
//				if (pktType == 241) {
//					int l16 = 0;
//					int[] ai = new int[676];
//					for (int i24 = 0; i24 < 4; i24++) {
//						for (int k26 = 0; k26 < 13; k26++) {
//							for (int l28 = 0; l28 < 13; l28++) {
//								int k30 = anIntArrayArrayArray1129[i24][k26][l28];
//								if (k30 != -1) {
//									int k31 = k30 >> 14 & 0x3ff;
//									int i32 = k30 >> 3 & 0x7ff;
//									int k32 = (k31 / 8 << 8) + i32 / 8;
//									for (int j33 = 0; j33 < l16; j33++) {
//										if (ai[j33] != k32)
//											continue;
//										k32 = -1;
//										
//									}
//									if (k32 != -1)
//										ai[l16++] = k32;
//								}
//							}
//						}
//					}
//					aByteArrayArray1183 = new byte[l16][];
//					aByteArrayArray1247 = new byte[l16][];
//					anIntArray1234 = new int[l16];
//					anIntArray1235 = new int[l16];
//					anIntArray1236 = new int[l16];
//					for (int l26 = 0; l26 < l16; l26++) {
//						int i29 = anIntArray1234[l26] = ai[l26];
//						int l30 = i29 >> 8 & 0xff;
//						int l31 = i29 & 0xff;
//						int j32 = anIntArray1235[l26] = onDemandFetcher
//							.method562(0, l31, l30);
//						UnityEngine.Debug.Log ("j32 " + j32);
//						if (j32 != -1)
//							onDemandFetcher.method558(3, j32);
//						int i33 = anIntArray1236[l26] = onDemandFetcher
//							.method562(1, l31, l30);
//						UnityEngine.Debug.Log ("i33 " + i33);
//						if (i33 != -1)
//							onDemandFetcher.method558(3, i33);
//					}
//				}
//				int i17 = baseX - anInt1036;
//				int j21 = baseY - anInt1037;
//				anInt1036 = baseX;
//				anInt1037 = baseY;
//				for (int j24 = 0; j24 < 16384; j24++) {
//					NPC npc = npcArray[j24];
//					if (npc != null) {
//						for (int j29 = 0; j29 < 10; j29++) {
//							npc.smallX[j29] -= i17;
//							npc.smallY[j29] -= j21;
//						}
//						npc.x -= i17 * 128;
//						npc.y -= j21 * 128;
//					}
//				}
//				for (int i27 = 0; i27 < maxPlayers; i27++) {
//					Player player = playerArray[i27];
//					if (player != null) {
//						for (int i31 = 0; i31 < 10; i31++) {
//							player.smallX[i31] -= i17;
//							player.smallY[i31] -= j21;
//						}
//						player.x -= i17 * 128;
//						player.y -= j21 * 128;
//					}
//				}
//				aBoolean1080 = true;
//				sbyte byte1 = 0;
//				sbyte byte2 = 104;
//				sbyte byte3 = 1;
//				if (i17 < 0) {
//					byte1 = 103;
//					byte2 = -1;
//					byte3 = -1;
//				}
//				sbyte byte4 = 0;
//				sbyte byte5 = 104;
//				sbyte byte6 = 1;
//				if (j21 < 0) {
//					byte4 = 103;
//					byte5 = -1;
//					byte6 = -1;
//				}
//				for (int k33 = byte1; k33 != byte2; k33 += byte3) {
//					for (int l33 = byte4; l33 != byte5; l33 += byte6) {
//						int i34 = k33 + i17;
//						int j34 = l33 + j21;
//						for (int k34 = 0; k34 < 4; k34++)
//							if (i34 >= 0 && j34 >= 0 && i34 < 104 && j34 < 104)
//								groundArray[k34][k33][l33] = groundArray[k34][i34][j34];
//						else
//							groundArray[k34][k33][l33] = null;
//					}
//				}
//				for (Class30_Sub1 class30_sub1_1 = (Class30_Sub1) aClass19_1179
//				     .reverseGetFirst(); class30_sub1_1 != null; class30_sub1_1 = (Class30_Sub1) aClass19_1179
//				     .reverseGetNext()) {
//					class30_sub1_1.anInt1297 -= i17;
//					class30_sub1_1.anInt1298 -= j21;
//					if (class30_sub1_1.anInt1297 < 0
//					    || class30_sub1_1.anInt1298 < 0
//					    || class30_sub1_1.anInt1297 >= 104
//					    || class30_sub1_1.anInt1298 >= 104)
//						class30_sub1_1.unlink();
//				}
//				if (destX != 0) {
//					destX -= i17;
//					destY -= j21;
//				}
//				aBoolean1160 = false;
//				pktType = -1;
//				return true;
//				
//			case 208:
//				int i3 = inStream.method437();
//				if (i3 >= 0)
//					method60(i3);
//				anInt1018 = i3;
//				pktType = -1;
//				return true;
//				
//			case 99:
//				anInt1021 = inStream.readUnsignedByte();
//				pktType = -1;
//				return true;
//				
//			case 75:
//				int j3 = inStream.method436();
//				int j11 = inStream.method436();
//				RSInterface.interfaceCache[j11].anInt233 = 2;
//				RSInterface.interfaceCache[j11].mediaID = j3;
//				pktType = -1;
//				return true;
//				
//			case 114:
//				anInt1104 = inStream.method434() * 30;
//				pktType = -1;
//				return true;
//				
//			case 60:
//				anInt1269 = inStream.readUnsignedByte();
//				anInt1268 = inStream.method427();
//				while (inStream.currentOffset < pktSize) {
//					int k3 = inStream.readUnsignedByte();
//					method137(inStream, k3);
//				}
//				pktType = -1;
//				return true;
//				
//			case 35:
//				int l3 = inStream.readUnsignedByte();
//				int k11 = inStream.readUnsignedByte();
//				int j17 = inStream.readUnsignedByte();
//				int k21 = inStream.readUnsignedByte();
//				aBooleanArray876[l3] = true;
//				anIntArray873[l3] = k11;
//				anIntArray1203[l3] = j17;
//				anIntArray928[l3] = k21;
//				anIntArray1030[l3] = 0;
//				pktType = -1;
//				return true;
//				
//			case 174:
//				int i4 = inStream.readUnsignedWord();
//				int l11 = inStream.readUnsignedByte();
//				int k17 = inStream.readUnsignedWord();
//				if (aBoolean848 && !lowMem && anInt1062 < 50) {
//					anIntArray1207[anInt1062] = i4;
//					anIntArray1241[anInt1062] = l11;
//					anIntArray1250[anInt1062] = k17 + Sounds.anIntArray326[i4];
//					anInt1062++;
//				}
//				pktType = -1;
//				return true;
//				
//			case 104:
//				int j4 = inStream.method427();
//				int i12 = inStream.method426();
//				String s6 = inStream.readString();
//				if (j4 >= 1 && j4 <= 5) {
//					if (s6.ToLower().Equals("null"))
//						s6 = null;
//					atPlayerActions[j4 - 1] = s6;
//					atPlayerArray[j4 - 1] = i12 == 0;
//				}
//				pktType = -1;
//				return true;
//				
//			case 78:
//				destX = 0;
//				pktType = -1;
//				return true;
//				
//			case 253:
//				String s = inStream.readString();
//				if (s.EndsWith(":tradereq:")) {
//					String s3 = s.Substring(0, s.IndexOf(":"));
//					long l17 = TextClass.longForName(s3);
//					bool flag2 = false;
//					for (int j27 = 0; j27 < ignoreCount; j27++) {
//						if (ignoreListAsLongs[j27] != l17)
//							continue;
//						flag2 = true;
//						
//					}
//					if (!flag2 && anInt1251 == 0)
//						pushMessage("wishes to trade with you.", 4, s3);
//				} else if (s.EndsWith("#url#")) {
//					String link = s.Substring(0, s.IndexOf("#"));
//					pushMessage("Join us at: ", 9, link);
//				} else if (s.EndsWith(":duelreq:")) {
//					String s4 = s.Substring(0, s.IndexOf(":"));
//					long l18 = TextClass.longForName(s4);
//					bool flag3 = false;
//					for (int k27 = 0; k27 < ignoreCount; k27++) {
//						if (ignoreListAsLongs[k27] != l18)
//							continue;
//						flag3 = true;
//						
//					}
//					if (!flag3 && anInt1251 == 0)
//						pushMessage("wishes to duel with you.", 8, s4);
//				} else if (s.EndsWith(":chalreq:")) {
//					String s5 = s.Substring(0, s.IndexOf(":"));
//					long l19 = TextClass.longForName(s5);
//					bool flag4 = false;
//					for (int l27 = 0; l27 < ignoreCount; l27++) {
//						if (ignoreListAsLongs[l27] != l19)
//							continue;
//						flag4 = true;
//						
//					}
//					if (!flag4 && anInt1251 == 0) {
//						String s8 = s.Substring(s.IndexOf(":") + 1,
//						                        s.Length - 9);
//						pushMessage(s8, 8, s5);
//					}
//				} else {
//					pushMessage(s, 0, "");
//				}
//				pktType = -1;
//				return true;
//				
//			case 1:
//				for (int k4 = 0; k4 < playerArray.Length; k4++)
//					if (playerArray[k4] != null)
//						playerArray[k4].anim = -1;
//				for (int j12 = 0; j12 < npcArray.Length; j12++)
//					if (npcArray[j12] != null)
//						npcArray[j12].anim = -1;
//				pktType = -1;
//				return true;
//				
//			case 50:
//				long l4 = inStream.readQWord();
//				int i18 = inStream.readUnsignedByte();
//				String s7 = TextClass.fixName(TextClass.nameForLong(l4));
//				for (int k24 = 0; k24 < friendsCount; k24++) {
//					if (l4 != friendsListAsLongs[k24])
//						continue;
//					if (friendsNodeIDs[k24] != i18) {
//						friendsNodeIDs[k24] = i18;
//						needDrawTabArea = true;
//						if (i18 >= 2) {
//							pushMessage(s7 + " has logged in.", 5, "");
//						}
//						if (i18 <= 1) {
//							pushMessage(s7 + " has logged out.", 5, "");
//						}
//					}
//					s7 = null;
//					
//				}
//				if (s7 != null && friendsCount < 200) {
//					friendsListAsLongs[friendsCount] = l4;
//					friendsList[friendsCount] = s7;
//					friendsNodeIDs[friendsCount] = i18;
//					friendsCount++;
//					needDrawTabArea = true;
//				}
//				for (bool flag6 = false; !flag6;) {
//					flag6 = true;
//					for (int k29 = 0; k29 < friendsCount - 1; k29++)
//						if (friendsNodeIDs[k29] != nodeID
//						    && friendsNodeIDs[k29 + 1] == nodeID
//						    || friendsNodeIDs[k29] == 0
//						   && friendsNodeIDs[k29 + 1] != 0) {
//						int j31 = friendsNodeIDs[k29];
//						friendsNodeIDs[k29] = friendsNodeIDs[k29 + 1];
//						friendsNodeIDs[k29 + 1] = j31;
//						String s10 = friendsList[k29];
//						friendsList[k29] = friendsList[k29 + 1];
//						friendsList[k29 + 1] = s10;
//						long l32 = friendsListAsLongs[k29];
//						friendsListAsLongs[k29] = friendsListAsLongs[k29 + 1];
//						friendsListAsLongs[k29 + 1] = l32;
//						needDrawTabArea = true;
//						flag6 = false;
//					}
//				}
//				pktType = -1;
//				return true;
//				
//			case 110:
//				if (tabID == 12)
//					needDrawTabArea = true;
//				energy = inStream.readUnsignedByte();
//				pktType = -1;
//				return true;
//				
//			case 254:
//				anInt855 = inStream.readUnsignedByte();
//				if (anInt855 == 1)
//					anInt1222 = inStream.readUnsignedWord();
//				if (anInt855 >= 2 && anInt855 <= 6) {
//					if (anInt855 == 2) {
//						anInt937 = 64;
//						anInt938 = 64;
//					}
//					if (anInt855 == 3) {
//						anInt937 = 0;
//						anInt938 = 64;
//					}
//					if (anInt855 == 4) {
//						anInt937 = 128;
//						anInt938 = 64;
//					}
//					if (anInt855 == 5) {
//						anInt937 = 64;
//						anInt938 = 0;
//					}
//					if (anInt855 == 6) {
//						anInt937 = 64;
//						anInt938 = 128;
//					}
//					anInt855 = 2;
//					anInt934 = inStream.readUnsignedWord();
//					anInt935 = inStream.readUnsignedWord();
//					anInt936 = inStream.readUnsignedByte();
//				}
//				if (anInt855 == 10)
//					anInt933 = inStream.readUnsignedWord();
//				pktType = -1;
//				return true;
//				
//			case 248:
//				int i5 = inStream.method435();
//				int k12 = inStream.readUnsignedWord();
//				if (backDialogID != -1) {
//					backDialogID = -1;
//					inputTaken = true;
//				}
//				if (inputDialogState != 0) {
//					inputDialogState = 0;
//					inputTaken = true;
//				}
//				openInterfaceID = i5;
//				invOverlayInterfaceID = k12;
//				needDrawTabArea = true;
//				tabAreaAltered = true;
//				aBoolean1149 = false;
//				pktType = -1;
//				return true;
//				
//			case 79:
//				int j5 = inStream.method434();
//				int l12 = inStream.method435();
//				RSInterface class9_3 = RSInterface.interfaceCache[j5];
//				if (class9_3 != null && class9_3.type == 0) {
//					if (l12 < 0)
//						l12 = 0;
//					if (l12 > class9_3.scrollMax - class9_3.height)
//						l12 = class9_3.scrollMax - class9_3.height;
//					class9_3.scrollPosition = l12;
//				}
//				pktType = -1;
//				return true;
//				
//			case 68:
//				for (int k5 = 0; k5 < variousSettings.Length; k5++)
//				if (variousSettings[k5] != anIntArray1045[k5]) {
//					variousSettings[k5] = anIntArray1045[k5];
//					method33(k5);
//					needDrawTabArea = true;
//				}
//				pktType = -1;
//				return true;
//				
//			case 196:
//				long l5 = inStream.readQWord();
//				int j18 = inStream.readDWord();
//				int l21 = inStream.readUnsignedByte();
//				bool flag5 = false;
//				for (int i28 = 0; i28 < 100; i28++) {
//					if (anIntArray1240[i28] != j18)
//						continue;
//					flag5 = true;
//					
//				}
//				if (l21 <= 1) {
//					for (int l29 = 0; l29 < ignoreCount; l29++) {
//						if (ignoreListAsLongs[l29] != l5)
//							continue;
//						flag5 = true;
//						
//					}
//				}
//				if (!flag5 && anInt1251 == 0)
//				try {
//					anIntArray1240[anInt1169] = j18;
//					anInt1169 = (anInt1169 + 1) % 100;
//					String s9 = TextInput.method525(pktSize - 13, inStream);
//					if (l21 != 3)
//						// s9 = Censor.doCensor(s9);
//						if (l21 == 2 || l21 == 3)
//							pushMessage(
//								s9,
//								7,
//								"@cr2@"
//								+ TextClass.fixName(TextClass
//							                    .nameForLong(l5)));
//					else if (l21 == 1)
//						pushMessage(
//							s9,
//							7,
//							"@cr1@"
//							+ TextClass.fixName(TextClass
//						                    .nameForLong(l5)));
//					else
//						pushMessage(s9, 3, TextClass.fixName(TextClass
//						                                     .nameForLong(l5)));
//				} catch (Exception exception1) {
//					signlink.reporterror("cde1");
//				}
//				pktType = -1;
//				return true;
//				
//			case 85:
//				anInt1269 = inStream.method427();
//				anInt1268 = inStream.method427();
//				pktType = -1;
//				return true;
//				
//			case 24:
//				anInt1054 = inStream.method428();
//				if (anInt1054 == tabID) {
//					if (anInt1054 == 3)
//						tabID = 1;
//					else
//						tabID = 3;
//					needDrawTabArea = true;
//				}
//				pktType = -1;
//				return true;
//				
//			case 246:
//				int i6 = inStream.method434();
//				int i13 = inStream.readUnsignedWord();
//				int k18 = inStream.readUnsignedWord();
//				if (k18 == 65535) {
//					RSInterface.interfaceCache[i6].anInt233 = 0;
//					pktType = -1;
//					return true;
//				} else {
//					ItemDef itemDef = ItemDef.forID(k18);
//					RSInterface.interfaceCache[i6].anInt233 = 4;
//					RSInterface.interfaceCache[i6].mediaID = k18;
//					RSInterface.interfaceCache[i6].anInt270 = itemDef.modelRotation1;
//					RSInterface.interfaceCache[i6].anInt271 = itemDef.modelRotation2;
//					RSInterface.interfaceCache[i6].anInt269 = (itemDef.modelZoom * 100)
//						/ i13;
//					pktType = -1;
//					return true;
//				}
//				
//			case 171:
//				bool flag1 = inStream.readUnsignedByte() == 1;
//				int j13 = inStream.readUnsignedWord();
//				RSInterface.interfaceCache[j13].aBoolean266 = flag1;
//				pktType = -1;
//				return true;
//				
//			case 142:
//				int j6 = inStream.method434();
//				method60(j6);
//				if (backDialogID != -1) {
//					backDialogID = -1;
//					inputTaken = true;
//				}
//				if (inputDialogState != 0) {
//					inputDialogState = 0;
//					inputTaken = true;
//				}
//				invOverlayInterfaceID = j6;
//				needDrawTabArea = true;
//				tabAreaAltered = true;
//				openInterfaceID = -1;
//				aBoolean1149 = false;
//				pktType = -1;
//				return true;
//				
//			case 126:
//				String text = inStream.readString();
//				int frame = inStream.method435();
//				updateStrings(text, frame);
//				sendFrame126(text, frame);
//				pktType = -1;
//				return true;
//				
//			case 206:
//				publicChatMode = inStream.readUnsignedByte();
//				privateChatMode = inStream.readUnsignedByte();
//				tradeMode = inStream.readUnsignedByte();
//				aBoolean1233 = true;
//				inputTaken = true;
//				pktType = -1;
//				return true;
//				
//			case 240:
//				if (tabID == 12)
//					needDrawTabArea = true;
//				weight = inStream.readSignedWord();
//				pktType = -1;
//				return true;
//				
//			case 8:
//				int k6 = inStream.method436();
//				int l13 = inStream.readUnsignedWord();
//				RSInterface.interfaceCache[k6].anInt233 = 1;
//				RSInterface.interfaceCache[k6].mediaID = l13;
//				pktType = -1;
//				return true;
//				
//			case 122:
//				int l6 = inStream.method436();
//				int i14 = inStream.method436();
//				int i19 = i14 >> 10 & 0x1f;
//				int i22 = i14 >> 5 & 0x1f;
//				int l24 = i14 & 0x1f;
//				RSInterface.interfaceCache[l6].textColor = (i19 << 19)
//					+ (i22 << 11) + (l24 << 3);
//				pktType = -1;
//				return true;
//				
//			case 53:
//				needDrawTabArea = true;
//				int i7 = inStream.readUnsignedWord();
//				RSInterface class9_1 = RSInterface.interfaceCache[i7];
//				int j19 = inStream.readUnsignedWord();
//				for (int j22 = 0; j22 < j19; j22++) {
//					int i25 = inStream.readUnsignedByte();
//					if (i25 == 255)
//						i25 = inStream.method440();
//					class9_1.inv[j22] = inStream.method436();
//					class9_1.invStackSizes[j22] = i25;
//				}
//				for (int j25 = j19; j25 < class9_1.inv.Length; j25++) {
//					class9_1.inv[j25] = 0;
//					class9_1.invStackSizes[j25] = 0;
//				}
//				pktType = -1;
//				return true;
//				
//			case 230:
//				int j7 = inStream.method435();
//				int j14 = inStream.readUnsignedWord();
//				int k19 = inStream.readUnsignedWord();
//				int k22 = inStream.method436();
//				RSInterface.interfaceCache[j14].anInt270 = k19;
//				RSInterface.interfaceCache[j14].anInt271 = k22;
//				RSInterface.interfaceCache[j14].anInt269 = j7;
//				pktType = -1;
//				return true;
//				
//			case 221:
//				anInt900 = inStream.readUnsignedByte();
//				needDrawTabArea = true;
//				pktType = -1;
//				return true;
//				
//			case 177:
//				aBoolean1160 = true;
//				anInt995 = inStream.readUnsignedByte();
//				anInt996 = inStream.readUnsignedByte();
//				anInt997 = inStream.readUnsignedWord();
//				anInt998 = inStream.readUnsignedByte();
//				anInt999 = inStream.readUnsignedByte();
//				if (anInt999 >= 100) {
//					int k7 = anInt995 * 128 + 64;
//					int k14 = anInt996 * 128 + 64;
//					int i20 = method42(plane, k14, k7) - anInt997;
//					int l22 = k7 - xCameraPos;
//					int k25 = i20 - zCameraPos;
//					int j28 = k14 - yCameraPos;
//					int i30 = (int) Mathf.Sqrt(l22 * l22 + j28 * j28);
//					yCameraCurve = (int) (Mathf.Atan2(k25, i30) * 325.94900000000001D) & 0x7ff;
//					xCameraCurve = (int) (Mathf.Atan2(l22, j28) * -325.94900000000001D) & 0x7ff;
//					if (yCameraCurve < 128)
//						yCameraCurve = 128;
//					if (yCameraCurve > 383)
//						yCameraCurve = 383;
//				}
//				pktType = -1;
//				return true;
//				
//			case 249:
//				anInt1046 = inStream.method426();
//				unknownInt10 = inStream.method436();
//				pktType = -1;
//				return true;
//				
//			case 65:
//				updateNPCs(inStream, pktSize);
//				pktType = -1;
//				return true;
//				
//			case 27:
//				messagePromptRaised = false;
//				inputDialogState = 1;
//				amountOrNameInput = "";
//				inputTaken = true;
//				pktType = -1;
//				return true;
//				
//			case 187:
//				messagePromptRaised = false;
//				inputDialogState = 2;
//				amountOrNameInput = "";
//				inputTaken = true;
//				pktType = -1;
//				return true;
//				
//			case 97:
//				int l7 = inStream.readUnsignedWord();
//				method60(l7);
//				if (invOverlayInterfaceID != -1) {
//					invOverlayInterfaceID = -1;
//					needDrawTabArea = true;
//					tabAreaAltered = true;
//				}
//				if (backDialogID != -1) {
//					backDialogID = -1;
//					inputTaken = true;
//				}
//				if (inputDialogState != 0) {
//					inputDialogState = 0;
//					inputTaken = true;
//				}
//				openInterfaceID = l7;
//				aBoolean1149 = false;
//				pktType = -1;
//				return true;
//				
//			case 218:
//				int i8 = inStream.method438();
//				dialogID = i8;
//				inputTaken = true;
//				pktType = -1;
//				return true;
//				
//			case 87:
//				int j8 = inStream.method434();
//				int l14 = inStream.method439();
//				anIntArray1045[j8] = l14;
//				if (variousSettings[j8] != l14) {
//					variousSettings[j8] = l14;
//					method33(j8);
//					needDrawTabArea = true;
//					if (dialogID != -1)
//						inputTaken = true;
//				}
//				pktType = -1;
//				return true;
//				
//			case 36:
//				int k8 = inStream.method434();
//				byte byte0 = (byte)inStream.readSignedByte();
//				anIntArray1045[k8] = byte0;
//				if (variousSettings[k8] != byte0) {
//					variousSettings[k8] = byte0;
//					method33(k8);
//					needDrawTabArea = true;
//					if (dialogID != -1)
//						inputTaken = true;
//				}
//				pktType = -1;
//				return true;
//				
//			case 61:
//				anInt1055 = inStream.readUnsignedByte();
//				pktType = -1;
//				return true;
//				
//			case 200:
//				int l8 = inStream.readUnsignedWord();
//				int i15 = inStream.readSignedWord();
//				RSInterface class9_4 = RSInterface.interfaceCache[l8];
//				class9_4.anInt257 = i15;
//				if (i15 == -1) {
//					class9_4.anInt246 = 0;
//					class9_4.anInt208 = 0;
//				}
//				pktType = -1;
//				return true;
//				
//			case 219:
//				if (invOverlayInterfaceID != -1) {
//					invOverlayInterfaceID = -1;
//					needDrawTabArea = true;
//					tabAreaAltered = true;
//				}
//				if (backDialogID != -1) {
//					backDialogID = -1;
//					inputTaken = true;
//				}
//				if (inputDialogState != 0) {
//					inputDialogState = 0;
//					inputTaken = true;
//				}
//				openInterfaceID = -1;
//				aBoolean1149 = false;
//				pktType = -1;
//				return true;
//				
//			case 34:
//				needDrawTabArea = true;
//				int i9 = inStream.readUnsignedWord();
//				RSInterface class9_2 = RSInterface.interfaceCache[i9];
//				while (inStream.currentOffset < pktSize) {
//					int j20 = inStream.method422();
//					int i23 = inStream.readUnsignedWord();
//					int l25 = inStream.readUnsignedByte();
//					if (l25 == 255)
//						l25 = inStream.readDWord();
//					if (j20 >= 0 && j20 < class9_2.inv.Length) {
//						class9_2.inv[j20] = i23;
//						class9_2.invStackSizes[j20] = l25;
//					}
//				}
//				pktType = -1;
//				return true;
//				
//			case 4:
//			case 44:
//			case 84:
//			case 101:
//			case 105:
//			case 117:
//			case 147:
//			case 151:
//			case 156:
//			case 160:
//			case 215:
//				method137(inStream, pktType);
//				pktType = -1;
//				return true;
//				
//			case 106:
//				tabID = inStream.method427();
//				needDrawTabArea = true;
//				tabAreaAltered = true;
//				pktType = -1;
//				return true;
//				
//			case 164:
//				int j9 = inStream.method434();
//				method60(j9);
//				if (invOverlayInterfaceID != -1) {
//					invOverlayInterfaceID = -1;
//					needDrawTabArea = true;
//					tabAreaAltered = true;
//				}
//				backDialogID = j9;
//				inputTaken = true;
//				openInterfaceID = -1;
//				aBoolean1149 = false;
//				pktType = -1;
//				return true;
//				
//			}
//			signlink.reporterror("T1 - " + pktType + "," + pktSize + " - "
//			                     + anInt842 + "," + anInt843);
//
//		} catch (IOException _ex) {
//			dropClient();
//		} catch (Exception exception) {
//			String s2 = "T2 - " + pktType + "," + anInt842 + "," + anInt843
//				+ " - " + pktSize + "," + (baseX + myPlayer.smallX[0])
//					+ "," + (baseY + myPlayer.smallY[0]) + " - ";
//			for (int j15 = 0; j15 < pktSize && j15 < 50; j15++)
//				s2 = s2 + inStream.buffer[j15] + ",";
//			signlink.reporterror(s2);
////			resetLogout();
//		}
//		pktType = -1;
//		return true;
		if(socketStream == null)
			return false;
		try {
			int i = socketStream.available();
			if(i == 0)
				return false;
			if(pktType == -1) {
				socketStream.flushInputStream(inStream.buffer, 1);
				pktType = inStream.buffer[0] & 0xff;
				if(encryption != null)
					pktType = pktType - encryption.getNextKey() & 0xff;
				pktSize = SizeConstants.packetSizes[pktType];
				i--;
			}
			if(pktSize == -1)
			if(i > 0) {
				socketStream.flushInputStream(inStream.buffer, 1);
				pktSize = inStream.buffer[0] & 0xff;
				i--;
			} else {
				return false;
			}
			if(pktSize == -2)
			if(i > 1) {
				socketStream.flushInputStream(inStream.buffer, 2);
				inStream.currentOffset = 0;
				pktSize = inStream.readUnsignedWord();
				i -= 2;
			} else {
				return false;
			}
			if(i < pktSize)
				return false;
			inStream.currentOffset = 0;
			socketStream.flushInputStream(inStream.buffer, pktSize);
			anInt1009 = 0;
			anInt843 = anInt842;
			anInt842 = anInt841;
			anInt841 = pktType;
			switch(pktType) {
			case 81:
				updatePlayers(pktSize, inStream);
				aBoolean1080 = false;
				pktType = -1;
				return true;
				
			case 176:
				daysSinceRecovChange = inStream.method427();
				unreadMessages = inStream.method435();
				membersInt = inStream.readUnsignedByte();
				anInt1193 = inStream.method440();
				daysSinceLastLogin = inStream.readUnsignedWord();
				if(anInt1193 != 0 && openInterfaceID == -1) {
					signlink.dnslookup(TextClass.method586(anInt1193));
					clearTopInterfaces();
					char c = '\u028A';
					if(daysSinceRecovChange != 201 || membersInt == 1)
						c = '\u028F';
					reportAbuseInput = "";
					canMute = false;
					for(int k9 = 0; k9 < RSInterface.interfaceCache.Length; k9++) {
						if(RSInterface.interfaceCache[k9] == null || RSInterface.interfaceCache[k9].anInt214 != c)
							continue;
						openInterfaceID = RSInterface.interfaceCache[k9].parentID;
						
					}
				}
				pktType = -1;
				return true;
				
			case 64:
				anInt1268 = inStream.method427();
				anInt1269 = inStream.method428();
				for(int j = anInt1268; j < anInt1268 + 8; j++) {
					for(int l9 = anInt1269; l9 < anInt1269 + 8; l9++)
					if(groundArray[plane][j][l9] != null) {
						groundArray[plane][j][l9] = null;
						spawnGroundItem(j, l9);
					}
				}
				for(Class30_Sub1 class30_sub1 = (Class30_Sub1)aClass19_1179.reverseGetFirst(); class30_sub1 != null; class30_sub1 = (Class30_Sub1)aClass19_1179.reverseGetNext())
					if(class30_sub1.anInt1297 >= anInt1268 && class30_sub1.anInt1297 < anInt1268 + 8 && class30_sub1.anInt1298 >= anInt1269 && class30_sub1.anInt1298 < anInt1269 + 8 && class30_sub1.anInt1295 == plane)
						class30_sub1.anInt1294 = 0;
				pktType = -1;
				return true;
				
			case 185:
				int k = inStream.method436();
				RSInterface.interfaceCache[k].anInt233 = 3;
				if(myPlayer.desc == null) {
					RSInterface.interfaceCache[k].mediaID = 
						(myPlayer.anIntArray1700[0] << 25) + 
							(myPlayer.anIntArray1700[4] << 20) + 
							(myPlayer.equipment[0] << 15) + 
							(myPlayer.equipment[8] << 10) + 
							(myPlayer.equipment[11] << 5) + 
							myPlayer.equipment[1];
				} else
					RSInterface.interfaceCache[k].mediaID = (int)(0x12345678L + myPlayer.desc.type);
				pktType = -1;
				return true;
				
				/* Clan chat packet */
			case 217:
				try {
					name = inStream.readString();
					message = inStream.readString();
					clanname = inStream.readString();
					rights = inStream.readUnsignedWord();
					pushMessage(message, 16, name);
				} catch(Exception e) {
					Debug.Log(e.Message);
				}
				pktType = -1;
				return true;
				
			case 107:
				aBoolean1160 = false;
				for(int l = 0; l < 5; l++)
					aBooleanArray876[l] = false;
				//xpCounter = 0;
				pktType = -1;
				return true;
				
			case 72:
				int i1 = inStream.method434();
				RSInterface class9 = RSInterface.interfaceCache[i1];
				for(int k15 = 0; k15 < class9.inv.Length; k15++) {
					class9.inv[k15] = -1;
					class9.inv[k15] = 0;
				}
				pktType = -1;
				return true;
				
			case 214:
				ignoreCount = pktSize / 8;
				for(int j1 = 0; j1 < ignoreCount; j1++)
					ignoreListAsLongs[j1] = inStream.readQWord();
				pktType = -1;
				return true;
				
			case 166:
				aBoolean1160 = true;
				anInt1098 = inStream.readUnsignedByte();
				anInt1099 = inStream.readUnsignedByte();
				anInt1100 = inStream.readUnsignedWord();
				anInt1101 = inStream.readUnsignedByte();
				anInt1102 = inStream.readUnsignedByte();
				if(anInt1102 >= 100) {
					xCameraPos = anInt1098 * 128 + 64;
					yCameraPos = anInt1099 * 128 + 64;
					zCameraPos = method42(plane, yCameraPos, xCameraPos) - anInt1100;
				}
				pktType = -1;
				return true;
				
			case 134:
				needDrawTabArea = true;
				int k1 = inStream.readUnsignedByte();
				int i10 = inStream.method439();
				int l15 = inStream.readUnsignedByte();
				int xp = currentExp[k1];
				currentExp[k1] = i10;
				currentStats[k1] = l15;
				maxStats[k1] = 1;
				//xpCounter += currentExp[k1]-xp;
				//expAdded = currentExp[k1]-xp;
				for(int k20 = 0; k20 < 98; k20++)
					if(i10 >= anIntArray1019[k20])
						maxStats[k1] = k20 + 2;
				pktType = -1;
				return true;
				
			case 71:
				int l1 = inStream.readUnsignedWord();
				int j10 = inStream.method426();
				if(l1 == 65535)
					l1 = -1;
				if(tabInterfaceIDs.Length > j10 && tabInterfaceIDs[j10] != null)
				{
					tabInterfaceIDs[j10] = l1;
				}
				else
				{
					Debug.Log("TabInterface missing : " + j10);
				}
				needDrawTabArea = true;
				tabAreaAltered = true;
				pktType = -1;
				return true;
				
			case 74:
				int i2 = inStream.method434();
				if(i2 == 65535)
					i2 = -1;
				if(i2 != currentSong && musicEnabled && !lowMem && prevSong == 0) {
					nextSong = i2;
					songChanging = true;
					onDemandFetcher.method558(2, nextSong);
				}
				currentSong = i2;
				pktType = -1;
				return true;
				
			case 121:
				int j2 = inStream.method436();
				int k10 = inStream.method435();
				if(musicEnabled && !lowMem) {
					nextSong = j2;
					songChanging = false;
					onDemandFetcher.method558(2, nextSong);
					prevSong = k10;
				}
				pktType = -1;
				return true;
				
			case 109:
				resetLogout();
				pktType = -1;
				return false;
				
			case 70:
				int k2 = inStream.readSignedWord();
				int l10 = inStream.method437();
				int i16 = inStream.method434();
				RSInterface class9_5 = RSInterface.interfaceCache[i16];
				class9_5.anInt263 = k2;
				class9_5.anInt265 = l10;
				pktType = -1;
				return true;
				
			case 73:
			case 241:
				int l2 = anInt1069;
				int i11 = anInt1070;
				if(pktType == 73) {
					l2 = inStream.method435();
					i11 = inStream.readUnsignedWord();
					aBoolean1159 = false;
				}
				if(pktType == 241) {
					i11 = inStream.method435();
					inStream.initBitAccess();
					for(int j16 = 0; j16 < 4; j16++) {
						for(int l20 = 0; l20 < 13; l20++) {
							for(int j23 = 0; j23 < 13; j23++) {
								int i26 = inStream.readBits(1);
								if(i26 == 1)
									anIntArrayArrayArray1129[j16][l20][j23] = inStream.readBits(26);
								else
									anIntArrayArrayArray1129[j16][l20][j23] = -1;
							}
						}
					}
					inStream.finishBitAccess();
					l2 = inStream.readUnsignedWord();
					aBoolean1159 = true;
				}
				if(anInt1069 == l2 && anInt1070 == i11 && loadingStage == 2) {
					pktType = -1;
					return true;
				}
				anInt1069 = l2;
				anInt1070 = i11;
				unityClient.justTeleported = true;
				baseX = (anInt1069 - 6) * 8;
				baseY = (anInt1070 - 6) * 8;
				aBoolean1141 = (anInt1069 / 8 == 48 || anInt1069 / 8 == 49) && anInt1070 / 8 == 48;
				if(anInt1069 / 8 == 48 && anInt1070 / 8 == 148)
					aBoolean1141 = true;
				loadingStage = 1;
				aLong824 = NetDrawingTools.CurrentTimeMillis();
				aRSImageProducer_1165.initDrawingArea();
				//drawLoadingMessages(1, "Loading - please wait.", null);
				aTextDrawingArea_1271.drawText(0xffffff, "Loading - please wait.",
				                               151, 257);
				aRSImageProducer_1165.drawGraphics(4, base.graphics, 4);
				if(pktType == 73) {
					int k16 = 0;
					for(int i21 = (anInt1069 - 6) / 8; i21 <= (anInt1069 + 6) / 8; i21++) {
						for(int k23 = (anInt1070 - 6) / 8; k23 <= (anInt1070 + 6) / 8; k23++)
							k16++;
					}
					localRegionMapData = new byte[k16][];
					localRegionLandscapeData = new byte[k16][];
					localRegionIds = new int[k16];
					anIntArray1235 = new int[k16];
					anIntArray1236 = new int[k16];
					k16 = 0;
					for(int l23 = (anInt1069 - 6) / 8; l23 <= (anInt1069 + 6) / 8; l23++) {
						for(int j26 = (anInt1070 - 6) / 8; j26 <= (anInt1070 + 6) / 8; j26++) {
							localRegionIds[k16] = (l23 << 8) + j26;
							if(aBoolean1141 && (j26 == 49 || j26 == 149 || j26 == 147 || l23 == 50 || l23 == 49 && j26 == 47)) {
								anIntArray1235[k16] = -1;
								anIntArray1236[k16] = -1;
								k16++;
							} else {
								int k28 = anIntArray1235[k16] = onDemandFetcher.method562(0, j26, l23);
								if(k28 != -1)
									onDemandFetcher.method558(3, k28);
								int j30 = anIntArray1236[k16] = onDemandFetcher.method562(1, j26, l23);
								if(j30 != -1)
									onDemandFetcher.method558(3, j30);
								k16++;
							}
						}
					}
				}
				if(pktType == 241) {
					int l16 = 0;
					int[] ai = new int[676];
					for(int i24 = 0; i24 < 4; i24++) {
						for(int k26 = 0; k26 < 13; k26++) {
							for(int l28 = 0; l28 < 13; l28++) {
								int k30 = anIntArrayArrayArray1129[i24][k26][l28];
								if(k30 != -1) {
									int k31 = k30 >> 14 & 0x3ff;
									int i32 = k30 >> 3 & 0x7ff;
									int k32 = (k31 / 8 << 8) + i32 / 8;
									for(int j33 = 0; j33 < l16; j33++) {
										if(ai[j33] != k32)
											continue;
										k32 = -1;
										
									}
									if(k32 != -1)
										ai[l16++] = k32;
								}
							}
						}
					}
					localRegionMapData = new byte[l16][];
					localRegionLandscapeData = new byte[l16][];
					localRegionIds = new int[l16];
					anIntArray1235 = new int[l16];
					anIntArray1236 = new int[l16];
					for(int l26 = 0; l26 < l16; l26++) {
						int i29 = localRegionIds[l26] = ai[l26];
						int l30 = i29 >> 8 & 0xff;
						int l31 = i29 & 0xff;
						int j32 = anIntArray1235[l26] = onDemandFetcher.method562(0, l31, l30);
						if(j32 != -1)
							onDemandFetcher.method558(3, j32);
						int i33 = anIntArray1236[l26] = onDemandFetcher.method562(1, l31, l30);
						if(i33 != -1)
							onDemandFetcher.method558(3, i33);
					}
				}
				int i17 = baseX - anInt1036;
				int j21 = baseY - anInt1037;
				anInt1036 = baseX;
				anInt1037 = baseY;
				for(int j24 = 0; j24 < 16384; j24++) {
					NPC npc = npcArray[j24];
					if(npc != null) {
						for(int j29 = 0; j29 < 10; j29++) {
							npc.smallX[j29] -= i17;
							npc.smallY[j29] -= j21;
						}
						npc.x -= i17 * 128;
						npc.y -= j21 * 128;
					}
				}
				for(int i27 = 0; i27 < maxPlayers; i27++) {
					Player player = playerArray[i27];
					if(player != null) {
						for(int i31 = 0; i31 < 10; i31++) {
							player.smallX[i31] -= i17;
							player.smallY[i31] -= j21;
						}
						player.x -= i17 * 128;
						player.y -= j21 * 128;
					}
				}
				aBoolean1080 = true;
				sbyte byte1 = 0;
				sbyte byte2 = 104;
				sbyte byte3 = 1;
				if(i17 < 0) {
					byte1 = 103;
					byte2 = -1;
					byte3 = -1;
				}
				sbyte byte4 = 0;
				sbyte byte5 = 104;
				sbyte byte6 = 1;
				if(j21 < 0) {
					byte4 = 103;
					byte5 = -1;
					byte6 = -1;
				}
				for(int k33 = byte1; k33 != byte2; k33 += byte3) {
					for(int l33 = byte4; l33 != byte5; l33 += byte6) {
						int i34 = k33 + i17;
						int j34 = l33 + j21;
						for(int k34 = 0; k34 < 4; k34++)
							if(i34 >= 0 && j34 >= 0 && i34 < 104 && j34 < 104)
								groundArray[k34][k33][l33] = groundArray[k34][i34][j34];
						else
							groundArray[k34][k33][l33] = null;
					}
				}
				for(Class30_Sub1 class30_sub1_1 = (Class30_Sub1)aClass19_1179.reverseGetFirst(); class30_sub1_1 != null; class30_sub1_1 = (Class30_Sub1)aClass19_1179.reverseGetNext()) {
					class30_sub1_1.anInt1297 -= i17;
					class30_sub1_1.anInt1298 -= j21;
					if(class30_sub1_1.anInt1297 < 0 || class30_sub1_1.anInt1298 < 0 || class30_sub1_1.anInt1297 >= 104 || class30_sub1_1.anInt1298 >= 104)
						class30_sub1_1.unlink();
				}
				if(destX != 0) {
					destX -= i17;
					destY -= j21;
				}
				aBoolean1160 = false;
				pktType = -1;
				return true;
				
			case 208:
				int i3 = inStream.method437();
				if(i3 >= 0)
					method60(i3);
				anInt1018 = i3;
				pktType = -1;
				return true;
				
			case 99:
				anInt1021 = inStream.readUnsignedByte();
				pktType = -1;
				return true;
				
			case 75:
				int j3 = inStream.method436();
				int j11 = inStream.method436();
				RSInterface.interfaceCache[j11].anInt233 = 2;
				RSInterface.interfaceCache[j11].mediaID = j3;
				pktType = -1;
				return true;
				
			case 114:
				anInt1104 = inStream.method434() * 30;
				pktType = -1;
				return true;
				
			case 60:
				anInt1269 = inStream.readUnsignedByte();
				anInt1268 = inStream.method427();
				while(inStream.currentOffset < pktSize) {
					int k3 = inStream.readUnsignedByte();
					method137(inStream, k3);
				}
				pktType = -1;
				return true;
				
			case 35:
				int l3 = inStream.readUnsignedByte();
				int k11 = inStream.readUnsignedByte();
				int j17 = inStream.readUnsignedByte();
				int k21 = inStream.readUnsignedByte();
				aBooleanArray876[l3] = true;
				anIntArray873[l3] = k11;
				anIntArray1203[l3] = j17;
				anIntArray928[l3] = k21;
				anIntArray1030[l3] = 0;
				pktType = -1;
				return true;
				
			case 174:
				int i4 = inStream.readUnsignedWord();
				int l11 = inStream.readUnsignedByte();
				int k17 = inStream.readUnsignedWord();
				if(aBoolean848 && !lowMem && anInt1062 < 50) {
					anIntArray1207[anInt1062] = i4;
					anIntArray1241[anInt1062] = l11;
					anIntArray1250[anInt1062] = k17 + Sounds.anIntArray326[i4];
					anInt1062++;
				}
				pktType = -1;
				return true;
				
			case 104:
				int j4 = inStream.method427();
				int i12 = inStream.method426();
				String s6 = inStream.readString();
				if(j4 >= 1 && j4 <= 5) {
					if(s6.ToLower().Equals("null"))
						s6 = null;
					atPlayerActions[j4 - 1] = s6;
					atPlayerArray[j4 - 1] = i12 == 0;
				}
				pktType = -1;
				return true;
				
			case 78:
				destX = 0;
				pktType = -1;
				return true;
				
			case 253:
				String s = inStream.readString();
				//if(consoleOpen)
				//	printConsoleMessage(s, 1);
				if(s.EndsWith(":tradereq:")) {
					String s3 = s.Substring(0, s.IndexOf(":"));
					long l17 = TextClass.longForName(s3);
					bool flag2 = false;
					for(int j27 = 0; j27 < ignoreCount; j27++) {
						if(ignoreListAsLongs[j27] != l17)
							continue;
						flag2 = true;
					}
					if(!flag2 && anInt1251 == 0)
						pushMessage("wishes to trade with you.", 4, s3);
				} else if (s.EndsWith(":clan:")) {
					String s4 = s.Substring(0, s.IndexOf(":"));
					long l18 = TextClass.longForName(s4);
					pushMessage("Clan: ", 8, s4);	
				} else if(s.EndsWith("#url#")) {
					String text = s.Substring(0, s.IndexOf("-"));
					s = s.Substring(text.Length+1).Trim();
					String link = s.Substring(0, s.IndexOf("#"));
					pushMessage(text, 9, link);
					
				} else if(s.EndsWith(":duelreq:")) {
					String s4 = s.Substring(0, s.IndexOf(":"));
					long l18 = TextClass.longForName(s4);
					bool flag3 = false;
					for(int k27 = 0; k27 < ignoreCount; k27++) {
						if(ignoreListAsLongs[k27] != l18)
							continue;
						flag3 = true;
						
					}
					if(!flag3 && anInt1251 == 0)
						pushMessage("wishes to duel with you.", 8, s4);
				} else if(s.EndsWith(":chalreq:")) {
					String s5 = s.Substring(0, s.IndexOf(":"));
					long l19 = TextClass.longForName(s5);
					bool flag4 = false;
					for(int l27 = 0; l27 < ignoreCount; l27++) {
						if(ignoreListAsLongs[l27] != l19)
							continue;
						flag4 = true;
						
					}
					if(!flag4 && anInt1251 == 0) {
						String s8 = s.Substring(s.IndexOf(":") + 1, s.Length - 9);
						pushMessage(s8, 8, s5);
					}
				} else {
					pushMessage(s, 0, "");
				}
				pktType = -1;
				return true;
				
			case 1:
				for(int k4 = 0; k4 < playerArray.Length; k4++)
					if(playerArray[k4] != null)
						playerArray[k4].anim = -1;
				for(int j12 = 0; j12 < npcArray.Length; j12++)
					if(npcArray[j12] != null)
						npcArray[j12].anim = -1;
				pktType = -1;
				return true;
				
			case 50:
				long l4 = inStream.readQWord();
				int i18 = inStream.readUnsignedByte();
				String s7 = TextClass.fixName(TextClass.nameForLong(l4));
				for(int k24 = 0; k24 < friendsCount; k24++) {
					if(l4 != friendsListAsLongs[k24])
						continue;
					if(friendsNodeIDs[k24] != i18) {
						friendsNodeIDs[k24] = i18;
						needDrawTabArea = true;
						if(i18 >= 2) {
							pushMessage(s7 + " has logged in.", 5, "");
						}
						if(i18 <= 1) {
							pushMessage(s7 + " has logged out.", 5, "");
						}
					}
					s7 = null;
					
				}
				if(s7 != null && friendsCount < 200) {
					friendsListAsLongs[friendsCount] = l4;
					friendsList[friendsCount] = s7;
					friendsNodeIDs[friendsCount] = i18;
					friendsCount++;
					needDrawTabArea = true;
				}
				for(bool flag6 = false; !flag6;) {
					flag6 = true;
					for(int index = 0; index < friendsCount - 1; index++)
					if(friendsNodeIDs[index] != nodeID && friendsNodeIDs[index + 1] == nodeID || friendsNodeIDs[index] == 0 && friendsNodeIDs[index + 1] != 0) {
						int j31 = friendsNodeIDs[index];
						friendsNodeIDs[index] = friendsNodeIDs[index + 1];
						friendsNodeIDs[index + 1] = j31;
						String s10 = friendsList[index];
						friendsList[index] = friendsList[index + 1];
						friendsList[index + 1] = s10;
						long l32 = friendsListAsLongs[index];
						friendsListAsLongs[index] = friendsListAsLongs[index + 1];
						friendsListAsLongs[index + 1] = l32;
						needDrawTabArea = true;
						flag6 = false;
					}
				}//friends list sorting
				pktType = -1;
				return true;
				
			case 110:
				if(tabID == 12)
					needDrawTabArea = true;
				energy = inStream.readUnsignedByte();
				pktType = -1;
				return true;
				
			case 254:
				anInt855 = inStream.readUnsignedByte();
				if(anInt855 == 1)
					anInt1222 = inStream.readUnsignedWord();
				if(anInt855 >= 2 && anInt855 <= 6) {
					if(anInt855 == 2) {
						anInt937 = 64;
						anInt938 = 64;
					}
					if(anInt855 == 3) {
						anInt937 = 0;
						anInt938 = 64;
					}
					if(anInt855 == 4) {
						anInt937 = 128;
						anInt938 = 64;
					}
					if(anInt855 == 5) {
						anInt937 = 64;
						anInt938 = 0;
					}
					if(anInt855 == 6) {
						anInt937 = 64;
						anInt938 = 128;
					}
					anInt855 = 2;
					anInt934 = inStream.readUnsignedWord();
					anInt935 = inStream.readUnsignedWord();
					anInt936 = inStream.readUnsignedByte();
				}
				if(anInt855 == 10)
					anInt933 = inStream.readUnsignedWord();
				pktType = -1;
				return true;
				
			case 248:
				int i5 = inStream.method435();
				int k12 = inStream.readUnsignedWord();
				if(backDialogID != -1) {
					backDialogID = -1;
					inputTaken = true;
				}
				if(inputDialogState != 0) {
					inputDialogState = 0;
					inputTaken = true;
				}
				openInterfaceID = i5;
				invOverlayInterfaceID = k12;
				needDrawTabArea = true;
				tabAreaAltered = true;
				aBoolean1149 = false;
				pktType = -1;
				return true;
				
			case 79:
				int j5 = inStream.method434();
				int l12 = inStream.method435();
				RSInterface class9_3 = RSInterface.interfaceCache[j5];
				if(class9_3 != null && class9_3.type == 0) {
					if(l12 < 0)
						l12 = 0;
					if(l12 > class9_3.scrollMax - class9_3.height)
						l12 = class9_3.scrollMax - class9_3.height;
					class9_3.scrollPosition = l12;
				}
				pktType = -1;
				return true;
				
			case 68:
				for(int k5 = 0; k5 < variousSettings.Length; k5++)
				if(variousSettings[k5] != anIntArray1045[k5]) {
					variousSettings[k5] = anIntArray1045[k5];
					method33(k5);
					needDrawTabArea = true;
				}
				pktType = -1;
				return true;
				
			case 196:
				long l5 = inStream.readQWord();
				int j18 = inStream.readDWord();
				int rights = inStream.readUnsignedByte();
				bool flag5 = false;
				for(int i28 = 0; i28 < 100; i28++) {
					if(anIntArray1240[i28] != j18)
						continue;
					flag5 = true;
					
				}
				if(rights <= 1) {
					for(int l29 = 0; l29 < ignoreCount; l29++) {
						if(ignoreListAsLongs[l29] != l5)
							continue;
						flag5 = true;
						
					}
				}
				
				if(!flag5 && anInt1251 == 0)//privatechat
				try {
					anIntArray1240[anInt1169] = j18;
					anInt1169 = (anInt1169 + 1) % 100;
					String input = TextInput.method525(pktSize - 13, inStream);
					Player player;
					String hasTitle = null;
					for(int count = -1; count < playerCount; count++) {
						if(count == -1) {
							player = myPlayer;
							//hasTitle = displayTitle(myPlayer.title);
						} else if(count < playerCount) {
							player = playerArray[playerIndices[count]];
							//hasTitle = displayTitle(player.title);
						}
					}
					
					if(rights == 2 || rights == 3)
						pushMessage(input, 7, "@cr2@" + hasTitle + TextClass.fixName(TextClass.nameForLong(l5)));
					else if(rights == 1)
						pushMessage(input, 7, "@cr1@" + hasTitle + TextClass.fixName(TextClass.nameForLong(l5)));
					else
						pushMessage(input, 3, hasTitle + TextClass.fixName(TextClass.nameForLong(l5)));
					
				} catch(Exception exception1) {
					signlink.reporterror("cde1");
				}
				pktType = -1;
				return true;
				
			case 85:
				anInt1269 = inStream.method427();
				anInt1268 = inStream.method427();
				pktType = -1;
				return true;
				
			case 24:
				anInt1054 = inStream.method428();
				if(anInt1054 == tabID) {
					if(anInt1054 == 3)
						tabID = 1;
					else
						tabID = 3;
					needDrawTabArea = true;
				}
				pktType = -1;
				return true;
				
			case 246:
				int i6 = inStream.method434();
				int i13 = inStream.readUnsignedWord();
				int k18 = inStream.readUnsignedWord();
				if(k18 == 65535) {
					RSInterface.interfaceCache[i6].anInt233 = 0;
					pktType = -1;
					return true;
				} else {
					ItemDef itemDef = ItemDef.forID(k18);
					RSInterface.interfaceCache[i6].anInt233 = 4;
					RSInterface.interfaceCache[i6].mediaID = k18;
					RSInterface.interfaceCache[i6].anInt270 = itemDef.modelRotation1;
					RSInterface.interfaceCache[i6].anInt271 = itemDef.modelRotation2;
					RSInterface.interfaceCache[i6].anInt269 = (itemDef.modelZoom * 100) / i13;
					pktType = -1;
					return true;
				}
				
			case 171:
				bool flag1 = inStream.readUnsignedByte() == 1;
				int j13 = inStream.readUnsignedWord();
				RSInterface.interfaceCache[j13].aBoolean266 = flag1;
				pktType = -1;
				return true;
				
			case 142:
				int j6 = inStream.method434();
				method60(j6);
				if(backDialogID != -1) {
					backDialogID = -1;
					inputTaken = true;
				}
				if(inputDialogState != 0) {
					inputDialogState = 0;
					inputTaken = true;
				}
				invOverlayInterfaceID = j6;
				needDrawTabArea = true;
				tabAreaAltered = true;
				openInterfaceID = -1;
				aBoolean1149 = false;
				pktType = -1;
				return true;
				
			case 126:
				String text = inStream.readString();
				int frame = inStream.method435();
				if (text.StartsWith("www.")) {
					//openURL(text);
					pktType = -1;
					return true;
				}
				if(text != null && frame != null)
				{
				updateStrings(text, frame);
				sendFrame126(text, frame);
				}
				if (frame >= 18144 && frame <= 18244) {
					//clanList[frame - 18144] = text;
				}
				pktType = -1;
				return true;
				
			case 206:
				publicChatMode = inStream.readUnsignedByte();
				privateChatMode = inStream.readUnsignedByte();
				tradeMode = inStream.readUnsignedByte();
				aBoolean1233 = true;
				inputTaken = true;
				pktType = -1;
				return true;
				
			case 240:
				if(tabID == 12)
					needDrawTabArea = true;
				weight = inStream.readSignedWord();
				pktType = -1;
				return true;
				
			case 8:
				int k6 = inStream.method436();
				int l13 = inStream.readUnsignedWord();
				RSInterface.interfaceCache[k6].anInt233 = 1;
				RSInterface.interfaceCache[k6].mediaID = l13;
				pktType = -1;
				return true;
				
			case 122:
				int l6 = inStream.method436();
				int i14 = inStream.method436();
				int i19 = i14 >> 10 & 0x1f;
				int i22 = i14 >> 5 & 0x1f;
				int l24 = i14 & 0x1f;
				RSInterface.interfaceCache[l6].textColor = (i19 << 19) + (i22 << 11) + (l24 << 3);
				pktType = -1;
				return true;
				
			case 53:
				needDrawTabArea = true;
				int i7 = inStream.readUnsignedWord();
				RSInterface rsi = RSInterface.interfaceCache[i7];
				int j19 = inStream.readUnsignedWord();
				for(int j22 = 0; j22 < j19; j22++) {
					int i25 = inStream.readUnsignedByte();
					if(i25 == 255)
						i25 = inStream.method440();
					rsi.inv[j22] = inStream.method436();
					rsi.invStackSizes[j22] = i25;
				}
				for(int j25 = j19; j25 < rsi.inv.Length; j25++) {
					rsi.inv[j25] = 0;
					rsi.invStackSizes[j25] = 0;
				}
				pktType = -1;
				return true;
				
			case 230:
				int j7 = inStream.method435();
				int j14 = inStream.readUnsignedWord();
				int k19 = inStream.readUnsignedWord();
				int k22 = inStream.method436();
				RSInterface.interfaceCache[j14].anInt270 = k19;
				RSInterface.interfaceCache[j14].anInt271 = k22;
				RSInterface.interfaceCache[j14].anInt269 = j7;
				pktType = -1;
				return true;
				
			case 221:
				anInt900 = inStream.readUnsignedByte();
				needDrawTabArea = true;
				pktType = -1;
				return true;
				
			case 177:
				aBoolean1160 = true;
				anInt995 = inStream.readUnsignedByte();
				anInt996 = inStream.readUnsignedByte();
				anInt997 = inStream.readUnsignedWord();
				anInt998 = inStream.readUnsignedByte();
				anInt999 = inStream.readUnsignedByte();
				if(anInt999 >= 100) {
					int k7 = anInt995 * 128 + 64;
					int k14 = anInt996 * 128 + 64;
					int i20 = method42(plane, k14, k7) - anInt997;
					int l22 = k7 - xCameraPos;
					int k25 = i20 - zCameraPos;
					int j28 = k14 - yCameraPos;
					int i30 = (int)Math.Sqrt(l22 * l22 + j28 * j28);
					yCameraCurve = (int)(Math.Atan2(k25, i30) * 325.94900000000001D) & 0x7ff;
					xCameraCurve = (int)(Math.Atan2(l22, j28) * -325.94900000000001D) & 0x7ff;
					if(yCameraCurve < 128)
						yCameraCurve = 128;
					if(yCameraCurve > 383)
						yCameraCurve = 383;
				}
				pktType = -1;
				return true;
				
			case 249:
				anInt1046 = inStream.method426();
				localPlayerIndex = inStream.method436();
				pktType = -1;
				return true;
				
			case 65:
				updateNPCs(inStream, pktSize);
				pktType = -1;
				return true;
				
			case 27:
				messagePromptRaised = false;
				inputDialogState = 1;
				amountOrNameInput = "";
				inputTaken = true;
				pktType = -1;
				return true;
				
			case 187:
				messagePromptRaised = false;
				inputDialogState = 2;
				amountOrNameInput = "";
				inputTaken = true;
				pktType = -1;
				return true;
			case 97:
				int l7 = inStream.readUnsignedWord();
				method60(l7);
				if (invOverlayInterfaceID != -1) {
					invOverlayInterfaceID = -1;
					needDrawTabArea = true;
					tabAreaAltered = true;
				}
				if (backDialogID != -1) {
					backDialogID = -1;
					inputTaken = true;
				}
				if (inputDialogState != 0) {
					inputDialogState = 0;
					inputTaken = true;
				}
				//17511 = Question Type
				//15819 = Christmas Type
				//15812 = Security Type
				//15801 = GroundItem Scam Type
				//15791 = Password Safety ?
				//15774 = Good/Bad Password
				//15767 = Drama Type ????
				if (l7 == 15244) {
					openInterfaceID = 17511;
					fullscreenInterfaceID = 15244;
				} else {
					openInterfaceID = l7;
				}
				aBoolean1149 = false;
				pktType = -1;
				return true;
			case 218:
				int i8 = inStream.method438();
				dialogID = i8;
				inputTaken = true;
				pktType = -1;
				return true;
				
			case 87:
				int j8 = inStream.method434();
				int l14 = inStream.method439();
				anIntArray1045[j8] = l14;
				if(variousSettings[j8] != l14) {
					variousSettings[j8] = l14;
					method33(j8);
					needDrawTabArea = true;
					if(dialogID != -1)
						inputTaken = true;
				}
				pktType = -1;
				return true;
				
			case 36:
				int k8 = inStream.method434();
				sbyte byte0 = inStream.readSignedByte();
				anIntArray1045[k8] = byte0;
				if(variousSettings[k8] != byte0) {
					variousSettings[k8] = byte0;
					method33(k8);
					needDrawTabArea = true;
					if(dialogID != -1)
						inputTaken = true;
				}
				pktType = -1;
				return true;
				
			case 61:
				anInt1055 = inStream.readUnsignedByte();
				pktType = -1;
				return true;
				
			case 200:
				int l8 = inStream.readUnsignedWord();
				int i15 = inStream.readSignedWord();
				RSInterface class9_4 = RSInterface.interfaceCache[l8];
				class9_4.anInt257 = i15;
				if(i15 == 591 || i15 == 588) {
					class9_4.anInt269 = 1620;
				}
				if(i15 == -1) {
					class9_4.anInt246 = 0;
					class9_4.anInt208 = 0;
				}
				pktType = -1;
				return true;
				
			case 219:
				if(invOverlayInterfaceID != -1) {
					invOverlayInterfaceID = -1;
					needDrawTabArea = true;
					tabAreaAltered = true;
				}
				if(backDialogID != -1) {
					backDialogID = -1;
					inputTaken = true;
				}
				if(inputDialogState != 0) {
					inputDialogState = 0;
					inputTaken = true;
				}
				openInterfaceID = -1;
				aBoolean1149 = false;
				pktType = -1;
				return true;
				
			case 34:
				needDrawTabArea = true;
				int i9 = inStream.readUnsignedWord();
				RSInterface class9_2 = RSInterface.interfaceCache[i9];
				while(inStream.currentOffset < pktSize) {
					int j20 = inStream.method422();
					int i23 = inStream.readUnsignedWord();
					int l25 = inStream.readUnsignedByte();
					if(l25 == 255)
						l25 = inStream.readDWord();
					if(j20 >= 0 && j20 < class9_2.inv.Length) {
						class9_2.inv[j20] = i23;
						class9_2.invStackSizes[j20] = l25;
					}
				}
				pktType = -1;
				return true;
				
				
			case 4:
			case 44:
			case 84:
			case 101:
			case 105:
			case 117:
			case 147:
			case 151:
			case 156:
			case 160:
			case 215:
				method137(inStream, pktType);
				pktType = -1;
				return true;
				
			case 106:
				tabID = inStream.method427();
				needDrawTabArea = true;
				tabAreaAltered = true;
				pktType = -1;
				return true;
				
			case 164:
				int j9 = inStream.method434();
				method60(j9);
				if(invOverlayInterfaceID != -1) {
					invOverlayInterfaceID = -1;
					needDrawTabArea = true;
					tabAreaAltered = true;
				}
				backDialogID = j9;
				inputTaken = true;
				openInterfaceID = -1;
				aBoolean1149 = false;
				pktType = -1;
				return true;
				
			}
			signlink.reporterror("T1 - " + pktType + "," + pktSize + " - " + anInt842 + "," + anInt843);
			//resetLogout();
		} catch(IOException _ex) {
			dropClient();
		} catch(Exception exception) {
			String s2 = "T2 - " + pktType + "," + anInt842 + "," + anInt843 + " - " + pktSize + "," + (baseX + myPlayer.smallX[0]) + "," + (baseY + myPlayer.smallY[0]) + " - " + exception.Message + " - " + exception.StackTrace;
			for(int j15 = 0; j15 < pktSize && j15 < 50; j15++)
				s2 = s2 + inStream.buffer[j15] + ",";
			signlink.reporterror(s2);
			//resetLogout();
		}
		pktType = -1;
		return true;
	}
	
	private void method146() {
		anInt1265++;
		method47(true);
		method26(true);
		method47(false);
		method26(false);
		processProjectiles();
		method104();
		if (!aBoolean1160) {
			int i = anInt1184;
			if (anInt984 / 256 > i)
				i = anInt984 / 256;
			if (aBooleanArray876[4] && anIntArray1203[4] + 128 > i)
				i = anIntArray1203[4] + 128;
			int k = minimapInt1 + anInt896 & 0x7ff;
			setCameraPos(600 + i * 3, i, anInt1014,
			             method42(plane, myPlayer.y, myPlayer.x) - 50, k, anInt1015);
		}
//
//		if (!aBoolean1160)
//			jVar = method120();
//		else
//			jVar = method121();
		int l = xCameraPos;
		int i1 = zCameraPos;
		int j1 = yCameraPos;
		int k1 = yCameraCurve;
		int l1 = xCameraCurve;
		for (int i2 = 0; i2 < 5; i2++)
		if (aBooleanArray876[i2]) {
//			int j2 = (int) ((RS2Sharp.Random.Next() * (anIntArray873[i2] * 2 + 1) - anIntArray873[i2]) + Math
//			                .Sin(anIntArray1030[i2] * (anIntArray928[i2] / 100D))
//			                * anIntArray1203[i2]);
			int j2 = (int)((RS2Sharp.Random.Next((anIntArray873[i2] * 2 + 1)) - anIntArray873[i2]) + Math.Sin((double)anIntArray1030[i2] * ((double)anIntArray928[i2] / 100D)) * (double)anIntArray1203[i2]);

			if (i2 == 0)
				xCameraPos += j2;
			if (i2 == 1)
				zCameraPos += j2;
			if (i2 == 2)
				yCameraPos += j2;
			if (i2 == 3)
				xCameraCurve = xCameraCurve + j2 & 0x7ff;
			if (i2 == 4) {
				yCameraCurve += j2;
				if (yCameraCurve < 128)
					yCameraCurve = 128;
				if (yCameraCurve > 383)
					yCameraCurve = 383;
			}
		}
		int k2 = RS2Sharp.Texture.anInt1481;
		Model.aBoolean1684 = true;
		Model.anInt1687 = 0;
		Model.anInt1685 = base.mouseX - 4;
		Model.anInt1686 = base.mouseY - 4;
		DrawingArea.setAllPixelsToZero();
		  //worldController.method313(xCameraPos, yCameraPos, xCameraCurve,
//		                            zCameraPos, jVar, yCameraCurve);
		//worldController.clearObj5Cache();
		double fogDistance = Mathf.Sqrt(Mathf.Pow(zCameraPos, 2));
		int fogStartDistance = 1430;
		int fogEndDistance = 2100;
		//fogHandler.setFogDistance((float) fogDistance);
		//fogHandler.renderFog(false, fogStartDistance, fogEndDistance, 3);
		
		updateEntities();
		drawHeadIcon();
		method37(k2);
		draw3dScreen();
		drawChatArea();
		drawTabArea();
		drawMinimap();
		aRSImageProducer_1165.drawGraphics(0, base.graphics, 0);
		xCameraPos = l;
		zCameraPos = i1;
		yCameraPos = j1;
		yCameraCurve = k1;
		xCameraCurve = l1;
	}

	public override void renderWorld()
	{
//		while(objectsToRender.Count > 0)
//		{
//			RuneObject rune = null;
//			if(objectsToRender.Count > 0) rune = objectsToRender[0];
//			if(rune != null) rune.RenderMesh();
//			if(objectsToRender.Count > 0) objectsToRender.RemoveAt(0);
//		}
//		while(objectsToDispose.Count > 0)
//		{
//			GameObject rune = null;
//			if(objectsToDispose.Count > 0) rune = objectsToDispose[0];
//			if(rune != null) UnityClient.DisposeRuneObject(rune);
//			if(objectsToDispose.Count > 0) objectsToDispose.RemoveAt(0);
//		}
	}
	
	public void clearTopInterfaces() {
		stream.createFrame(130);
		if (invOverlayInterfaceID != -1) {
			invOverlayInterfaceID = -1;
			needDrawTabArea = true;
			aBoolean1149 = false;
			tabAreaAltered = true;
		}
		if (backDialogID != -1) {
			backDialogID = -1;
			inputTaken = true;
			aBoolean1149 = false;
		}
		openInterfaceID = -1;
		fullscreenInterfaceID = -1;
	}
	public static client instance;	
	public client() {
		instance = this;
		fullscreenInterfaceID = -1;
		chatRights = new int[500];
		chatTypeView = 0;
		clanChatMode = 0;
		cButtonHPos = -1;
		cButtonHCPos = -1;
		cButtonCPos = 0;
		anIntArrayArray825 = NetDrawingTools.CreateDoubleIntArray (104, 104);//new int[104][104];
		friendsNodeIDs = new int[200];
		groundArray = NetDrawingTools.CreateTrippleNodeListArray (4, 104, 104);//new NodeList[4][104][104];
		aBoolean831 = false;
		aStream_834 = new RS2Sharp.Stream(new byte[5000]);
		npcArray = new NPC[16384];
		npcIndices = new int[16384];
		anIntArray840 = new int[1000];
		aStream_847 = RS2Sharp.Stream.create();
		aBoolean848 = true;
		openInterfaceID = -1;
		currentExp = new int[Skills.skillsCount];
		aBoolean872 = false;
		anIntArray873 = new int[5];
		anInt874 = -1;
		aBooleanArray876 = new bool[5];
		//drawFlames = false;
		reportAbuseInput = "";
		localPlayerIndex = -1;
		menuOpen = false;
		inputString = "";
		maxPlayers = 2048;
		myPlayerIndex = 2047;
		playerArray = new Player[maxPlayers];
		playerIndices = new int[maxPlayers];
		anIntArray894 = new int[maxPlayers];
		aStreamArray895s = new RS2Sharp.Stream[maxPlayers];
		anInt897 = 1;
		anIntArrayArray901 = NetDrawingTools.CreateDoubleIntArray (104, 104);//new int[104][104];
		anInt902 = 0x766654;
		aByteArray912 = new byte[16384];
		currentStats = new int[Skills.skillsCount];
		ignoreListAsLongs = new long[100];
		loadingError = false;
		anInt927 = 0x332d25;
		anIntArray928 = new int[5];
		anIntArrayArray929 = NetDrawingTools.CreateDoubleIntArray (104, 104);//new int[104][104];
		chatTypes = new int[500];
		chatNames = new String[500];
		chatMessages = new String[500];
		chatButtons = new RS2Sharp.Sprite[4];
		sideIcons = new RS2Sharp.Sprite[15];
		redStones = new RS2Sharp.Sprite[5];
		aBoolean954 = true;
		friendsListAsLongs = new long[200];
		currentSong = -1;
		drawingFlames = false;
		spriteDrawX = -1;
		spriteDrawY = -1;
		anIntArray968 = new int[33];
		anIntArray969 = new int[256];
		decompressors = new Decompressor[5];
		variousSettings = new int[2000];
		aBoolean972 = false;
		anInt975 = 50;
		anIntArray976 = new int[anInt975];
		anIntArray977 = new int[anInt975];
		anIntArray978 = new int[anInt975];
		anIntArray979 = new int[anInt975];
		anIntArray980 = new int[anInt975];
		anIntArray981 = new int[anInt975];
		anIntArray982 = new int[anInt975];
		aStringArray983 = new String[anInt975];
		anInt985 = -1;
		hitMarks = new RS2Sharp.Sprite[20];
		anIntArray990 = new int[5];
		aBoolean994 = false;
		anInt1002 = 0x23201b;
		amountOrNameInput = "";
		projectiles = new NodeList();
		aBoolean1017 = false;
		anInt1018 = -1;
		anIntArray1030 = new int[5];
		aBoolean1031 = false;
		mapFunctions = new RS2Sharp.Sprite[100];
		dialogID = -1;
		maxStats = new int[Skills.skillsCount];
		anIntArray1045 = new int[2000];
		aBoolean1047 = true;
		anIntArray1052 = new int[151];
		anInt1054 = -1;
		aClass19_1056 = new NodeList();
		anIntArray1057 = new int[33];
		aClass9_1059 = new RSInterface();
		mapScenes = new Background[100];
		barFillColor = 0x4d4233;
		anIntArray1065 = new int[7];
		anIntArray1072 = new int[1000];
		anIntArray1073 = new int[1000];
		aBoolean1080 = false;
		friendsList = new String[200];
		inStream = RS2Sharp.Stream.create();
		expectedCRCs = new int[9];
		menuActionCmd2 = new int[500];
		menuActionCmd3 = new int[500];
		menuActionID = new int[500];
		menuActionCmd1 = new int[500];
		headIcons = new RS2Sharp.Sprite[20];
		skullIcons = new RS2Sharp.Sprite[20];
		headIconsHint = new RS2Sharp.Sprite[20];
		tabAreaAltered = false;
		aString1121 = "";
		atPlayerActions = new String[5];
		atPlayerArray = new bool[5];
		anIntArrayArrayArray1129 = NetDrawingTools.CreateTrippleIntArray (4,13,13);//new int[4][13][13];
		anInt1132 = 2;
		aClass30_Sub2_Sub1_Sub1Array1140 = new RS2Sharp.Sprite[1000];
		aBoolean1141 = false;
		aBoolean1149 = false;
		crosses = new RS2Sharp.Sprite[8];
		musicEnabled = true;
		needDrawTabArea = false;
		loggedIn = false;
		canMute = false;
		aBoolean1159 = false;
		aBoolean1160 = false;
		anInt1171 = 1;
		myUsername = "";
		myPassword = "";
		genericLoadingError = false;
		reportAbuseInterfaceID = -1;
		aClass19_1179 = new NodeList();
		anInt1184 = 128;
		invOverlayInterfaceID = -1;
		stream = RS2Sharp.Stream.create();
		menuActionName = new String[500];
		anIntArray1203 = new int[5];
		anIntArray1207 = new int[50];
		anInt1210 = 2;
		anInt1211 = 78;
		promptInput = "";
		modIcons = new Background[2];
		tabID = 3;
		inputTaken = false;
		songChanging = true;
		anIntArray1229 = new int[151];
		aClass11Array1230 = new CollisionMap[4];
		aBoolean1233 = false;
		anIntArray1240 = new int[100];
		anIntArray1241 = new int[50];
		aBoolean1242 = false;
		anIntArray1250 = new int[50];
		rsAlreadyLoaded = false;
		welcomeScreenRaised = false;
		messagePromptRaised = false;
		loginMessage1 = "";
		loginMessage2 = "";
		backDialogID = -1;
		anInt1279 = 2;
		bigX = new int[4000];
		bigY = new int[4000];
		anInt1289 = -1;
	}
	
	public int rights;
	public String name;
	public String message;
	public String clanname;
	private int[] chatRights;
	public int chatTypeView;
	public int clanChatMode;
	public int duelMode;
	/* Declare custom sprites */
	private RS2Sharp.Sprite chatArea;
	private RS2Sharp.Sprite[] chatButtons;
	private RS2Sharp.Sprite tabArea;
	private RS2Sharp.Sprite mapArea;
	/**/
	private RSImageProducer leftFrame;
	private RSImageProducer topFrame;
	private RSImageProducer rightFrame;
	private int ignoreCount;
	private long aLong824;
	private int[][] anIntArrayArray825;
	private int[] friendsNodeIDs;
	private NodeList[][][] groundArray;
	private int[] anIntArray828;
	private int[] anIntArray829;
	private volatile bool aBoolean831;
	private static Socket aSocket832;
	private int loginScreenState;
	private RS2Sharp.Stream aStream_834;
	private NPC[] npcArray;
	private int npcCount;
	private int[] npcIndices;
	private int anInt839;
	private int[] anIntArray840;
	private int anInt841;
	private int anInt842;
	private int anInt843;
	private String aString844;
	private int privateChatMode;
	private RS2Sharp.Stream aStream_847;
	private bool aBoolean848;
	private static int anInt849;
	private int[] anIntArray850;
	private int[] anIntArray851;
	private int[] anIntArray852;
	private int[] anIntArray853;
	private static int anInt854;
	private int anInt855;
	private int openInterfaceID;
	public int xCameraPos;
	public int zCameraPos;
	public int yCameraPos;
	public int yCameraCurve;
	public int xCameraCurve;
	private int myPrivilege;
	private int[] currentExp;
	private RS2Sharp.Sprite[] redStones;
	private RS2Sharp.Sprite mapFlag;
	private RS2Sharp.Sprite mapMarker;
	public static bool aBoolean872;
	private int[] anIntArray873;
	private int anInt874;
	private bool[] aBooleanArray876;
	private int weight;
	private MouseDetection mouseDetection;
//	private volatile bool drawFlames;
	private String reportAbuseInput;
	private int localPlayerIndex;
	private bool menuOpen;
	private int anInt886;
	private String inputString;
	private int maxPlayers;
	private int myPlayerIndex;
	private Player[] playerArray;
	private int playerCount;
	private int[] playerIndices;
	private int anInt893;
	private int[] anIntArray894;
	private RS2Sharp.Stream[] aStreamArray895s;
	private int anInt896;
	private int anInt897;
	private int friendsCount;
	private int anInt900;
	private int[][] anIntArrayArray901;
	private int anInt902;
	private byte[] aByteArray912;
	private int anInt913;
	private int crossX;
	private int crossY;
	private int crossIndex;
	private int crossType;
	public int plane;
	private int[] currentStats;
	private static int anInt924;
	private long[] ignoreListAsLongs;
	private bool loadingError;
	private int anInt927;
	private int[] anIntArray928;
	private int[][] anIntArrayArray929;
	private RS2Sharp.Sprite aClass30_Sub2_Sub1_Sub1_931;
	private RS2Sharp.Sprite aClass30_Sub2_Sub1_Sub1_932;
	private int anInt933;
	private int anInt934;
	private int anInt935;
	private int anInt936;
	private int anInt937;
	private int anInt938;
	private static int anInt940;
	private int[] chatTypes;
	private String[] chatNames;
	private String[] chatMessages;
	private int anInt945;
	public WorldController worldController;
	private RS2Sharp.Sprite[] sideIcons;
	private int menuScreenArea;
	private int menuOffsetX;
	private int menuOffsetY;
	private int menuWidth;
	private int menuHeight;
	private long aLong953;
	private bool aBoolean954;
	private long[] friendsListAsLongs;
	private int currentSong;
	private static int nodeID = 10;
	public static int portOff;
	public static bool clientData;
	private static bool isMembers = true;
	private static bool lowMem;
	private volatile bool drawingFlames;
	private int spriteDrawX;
	private int spriteDrawY;
	private int[] anIntArray965 = { 0xffff00, 0xff0000, 65280, 65535,
		0xff00ff, 0xffffff };
	private Background aBackground_966;
	private Background aBackground_967;
	private int[] anIntArray968;
	private int[] anIntArray969;
	public static Decompressor[] decompressors;
	public int[] variousSettings;
	private bool aBoolean972;
	private int anInt975;
	private int[] anIntArray976;
	private int[] anIntArray977;
	private int[] anIntArray978;
	private int[] anIntArray979;
	private int[] anIntArray980;
	private int[] anIntArray981;
	private int[] anIntArray982;
	private String[] aStringArray983;
	private int anInt984;
	private int anInt985;
	private static int anInt986;
	private RS2Sharp.Sprite[] hitMarks;
	private int anInt988;
	private int anInt989;
	private int[] anIntArray990;
	private static bool aBoolean993;
	private bool aBoolean994;
	private int anInt995;
	private int anInt996;
	private int anInt997;
	private int anInt998;
	private int anInt999;
	private ISAACRandomGen encryption;
	private RS2Sharp.Sprite mapEdge;
	private int anInt1002;
	public static int[][] anIntArrayArray1003 = {
		new int[]{ 6798, 107, 10283, 16, 4797, 7744, 5799, 4634, 33697, 22433, 2983,
			54193 },
		new int[]{ 8741, 12, 64030, 43162, 7735, 8404, 1701, 38430, 24094, 10153,
			56621, 4783, 1341, 16578, 35003, 25239 },
		new int[]{ 25238, 8742, 12, 64030, 43162, 7735, 8404, 1701, 38430, 24094,
			10153, 56621, 4783, 1341, 16578, 35003 },
		new int[]{ 4626, 11146, 6439, 12, 4758, 10270 },
		new int[]{ 4550, 4537, 5681, 5673, 5790, 6806, 8076, 4574 } };
	private String amountOrNameInput;
	private static int anInt1005;
	private int daysSinceLastLogin;
	private int pktSize;
	private int pktType;
	private int anInt1009;
	private int anInt1010;
	private int anInt1011;
	private NodeList projectiles;
	private int anInt1014;
	private int anInt1015;
	private int anInt1016;
	private bool aBoolean1017;
	private int anInt1018;
	private static int[] anIntArray1019;
	private int anInt1021;
	private int anInt1022;
	public int loadingStage;
	private RS2Sharp.Sprite scrollBar1;
	private RS2Sharp.Sprite scrollBar2;
	private int anInt1026;
	private Background backBase1;
	private Background backBase2;
	private Background backHmid1;
	private int[] anIntArray1030;
	private bool aBoolean1031;
	private RS2Sharp.Sprite[] mapFunctions;
	public int baseX;
	public int baseY;
	private int anInt1036;
	private int anInt1037;
	private int loginFailures;
	private int anInt1039;
	private int anInt1040;
	private int anInt1041;
	private int dialogID;
	private int[] maxStats;
	private int[] anIntArray1045;
	private int anInt1046;
	private bool aBoolean1047;
	private int anInt1048;
	private String aString1049;
	private static int anInt1051;
	private int[] anIntArray1052;
	private StreamLoader titleStreamLoader;
	private int anInt1054;
	private int anInt1055;
	private NodeList aClass19_1056;
	private int[] anIntArray1057;
	public RSInterface aClass9_1059;
	private Background[] mapScenes;
	private static int anInt1061;
	private int anInt1062;
	private int barFillColor;
	private int friendsListAction;
	private int[] anIntArray1065;
	private int mouseInvInterfaceIndex;
	private int lastActiveInvInterface;
	
	private int anInt1069;
	private int anInt1070;
	private int anInt1071;
	private int[] anIntArray1072;
	private int[] anIntArray1073;
	private RS2Sharp.Sprite mapDotItem;
	private RS2Sharp.Sprite mapDotNPC;
	private RS2Sharp.Sprite mapDotPlayer;
	private RS2Sharp.Sprite mapDotFriend;
	private RS2Sharp.Sprite mapDotTeam;
	private int anInt1079;
	private bool aBoolean1080;
	private String[] friendsList;
	private RS2Sharp.Stream inStream;
	private int anInt1084;
	private int anInt1085;
	private int activeInterfaceType;
	private int anInt1087;
	private int anInt1088;
	public static int anInt1089;
	public static int[] expectedCRCs;
	private int[] menuActionCmd2;
	private int[] menuActionCmd3;
	private int[] menuActionID;
	private int[] menuActionCmd1;
	private RS2Sharp.Sprite[] headIcons;
	private RS2Sharp.Sprite[] skullIcons;
	private RS2Sharp.Sprite[] headIconsHint;
	private static int anInt1097;
	private int anInt1098;
	private int anInt1099;
	private int anInt1100;
	private int anInt1101;
	private int anInt1102;
	private bool tabAreaAltered;
	private int anInt1104;
	private RSImageProducer aRSImageProducer_1107;
	private RSImageProducer aRSImageProducer_1108;
	private RSImageProducer aRSImageProducer_1109;
	private RSImageProducer aRSImageProducer_1110;
	private RSImageProducer aRSImageProducer_1111;
	private RSImageProducer aRSImageProducer_1112;
	private RSImageProducer aRSImageProducer_1113;
	private RSImageProducer aRSImageProducer_1114;
	private RSImageProducer aRSImageProducer_1115;
	private static int anInt1117;
	private int membersInt;
	private String aString1121;
	private RS2Sharp.Sprite compass;
	private RSImageProducer aRSImageProducer_1123;
	private RSImageProducer aRSImageProducer_1124;
	private RSImageProducer aRSImageProducer_1125;
	public static Player myPlayer;
	private String[] atPlayerActions;
	private bool[] atPlayerArray;
	private int[][][] anIntArrayArrayArray1129;
	private int[] tabInterfaceIDs = { -1, -1, -1, -1, -1, -1, -1, -1, -1,
		-1, -1, -1, -1, -1, -1 };
	private int anInt1131;
	private int anInt1132;
	private int menuActionRow;
	private static int anInt1134;
	private int spellSelected;
	private int anInt1137;
	private int spellUsableOn;
	private String spellTooltip;
	private RS2Sharp.Sprite[] aClass30_Sub2_Sub1_Sub1Array1140;
	private bool aBoolean1141;
	private static int anInt1142;
	private int energy;
	private bool aBoolean1149;
	private RS2Sharp.Sprite[] crosses;
	private bool musicEnabled;
	private Background[] aBackgroundArray1152s;
	private bool needDrawTabArea;
	private int unreadMessages;
	private static int anInt1155;
	private static bool fpsOn;
	public bool loggedIn;
	private bool canMute;
	private bool aBoolean1159;
	private bool aBoolean1160;
	public static int loopCycle;
	private static String validUserPassChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!43$%^&*()-_=+[{]};:'@#~,<.>/?\\| ";
	private RSImageProducer aRSImageProducer_1163;
	private RSImageProducer mapEdgeIP;
	private RSImageProducer aRSImageProducer_1164;
	private RSImageProducer aRSImageProducer_1165;
	private RSImageProducer aRSImageProducer_1166;
	private int daysSinceRecovChange;
	private RSSocket socketStream;
	private int anInt1169;
	private int minimapInt3;
	private int anInt1171;
	private long aLong1172;
	public string myUsername;
	public string myPassword;
	private static int anInt1175;
	private bool genericLoadingError;
	private int[] anIntArray1177 = { 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2,
		2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3 };
	private int reportAbuseInterfaceID;
	private NodeList aClass19_1179;
	private int[] anIntArray1180;
	private int[] anIntArray1181;
	private int[] anIntArray1182;
	private byte[][] localRegionMapData;
	private int anInt1184;
	public int minimapInt1;
	private int anInt1186;
	private int anInt1187;
	private static int anInt1188;
	private int invOverlayInterfaceID;
	private int[] anIntArray1190;
	private int[] anIntArray1191;
	private RS2Sharp.Stream stream;
	private int anInt1193;
	private int splitPrivateChat;
	private Background mapBack;
	private String[] menuActionName;
	private RS2Sharp.Sprite aClass30_Sub2_Sub1_Sub1_1201;
	private RS2Sharp.Sprite aClass30_Sub2_Sub1_Sub1_1202;
	private int[] anIntArray1203;
	public static int[] anIntArray1204 = { 9104, 10275, 7595, 3610, 7975, 8526,
		918, 38802, 24466, 10145, 58654, 5027, 1457, 16565, 34991, 25486 };
	private static bool flagged;
	private int[] anIntArray1207;
	private int anInt1208;
	private int minimapInt2;
	private int anInt1210;
	private int anInt1211;
	private String promptInput;
	private int anInt1213;
	private int[][][] intGroundArray;
	private long aLong1215;
	private int loginScreenCursorPos;
	private Background[] modIcons;
	private long aLong1220;
	private int tabID;
	private int anInt1222;
	public static bool inputTaken;
	private int inputDialogState;
	private static int anInt1226;
	private int nextSong;
	private bool songChanging;
	private int[] anIntArray1229;
	private CollisionMap[] aClass11Array1230;
	public static int[] anIntArray1232;
	private bool aBoolean1233;
	private int[] localRegionIds;
	private int[] anIntArray1235;
	private int[] anIntArray1236;
	private int anInt1237;
	private int anInt1238;
	public int anInt1239 = 100;
	private int[] anIntArray1240;
	private int[] anIntArray1241;
	private bool aBoolean1242;
	private int atInventoryLoopCycle;
	private int atInventoryInterface;
	private int atInventoryIndex;
	private int atInventoryInterfaceType;
	private byte[][] localRegionLandscapeData;
	private int tradeMode;
	private int anInt1249;
	private int[] anIntArray1250;
	private int anInt1251;
	private bool rsAlreadyLoaded;
	private int anInt1253;
	private int anInt1254;
	private bool welcomeScreenRaised;
	private bool messagePromptRaised;
	private int anInt1257;
	private byte[][][] byteGroundArray;
	private int prevSong;
	private int destX;
	private int destY;
	private RS2Sharp.Sprite aClass30_Sub2_Sub1_Sub1_1263;
	private int anInt1264;
	private int anInt1265;
	private String loginMessage1;
	private String loginMessage2;
	private int anInt1268;
	private int anInt1269;
	private TextDrawingArea smallText;
	private TextDrawingArea aTextDrawingArea_1271;
	private TextDrawingArea chatTextDrawingArea;
	private int anInt1275;
	private int backDialogID;
	private int anInt1278;
	private int anInt1279;
	private int[] bigX;
	private int[] bigY;
	private int itemSelected;
	private int anInt1283;
	private int anInt1284;
	private int anInt1285;
	private String selectedItemName;
	private int publicChatMode;
	private static int anInt1288;
	private int anInt1289;
	public static int anInt1290;
	//public static String server = "";
	public int drawCount;
	public int fullscreenInterfaceID;
	public int anInt1044;// 377
	public int anInt1129;// 377
	public int anInt1315;// 377
	public int anInt1500;// 377
	public int anInt1501;// 377
	public int[] fullScreenTextureArray;
	
	public void resetAllImageProducers() {
		if (base.fullGameScreen != null) {
			return;
		}
		aRSImageProducer_1166 = null;
		aRSImageProducer_1164 = null;
		aRSImageProducer_1163 = null;
		aRSImageProducer_1165 = null;
		aRSImageProducer_1123 = null;
		aRSImageProducer_1124 = null;
		aRSImageProducer_1125 = null;
		aRSImageProducer_1107 = null;
		aRSImageProducer_1108 = null;
		aRSImageProducer_1109 = null;
		aRSImageProducer_1110 = null;
		aRSImageProducer_1111 = null;
		aRSImageProducer_1112 = null;
		aRSImageProducer_1113 = null;
		aRSImageProducer_1114 = null;
		aRSImageProducer_1115 = null;
		base.fullGameScreen = new RSImageProducer(myWidth, myHeight);
		welcomeScreenRaised = true;
	}
	
	static client(){
		anIntArray1019 = new int[99];
		int i = 0;
		for (int j = 0; j < 99; j++) {
			int l = j + 1;
			int i1 = (int) (l + 300D * Mathf.Pow(2f, l / 7f));
			i += i1;
			anIntArray1019[j] = i / 4;
		}
		anIntArray1232 = new int[32];
		i = 2;
		for (int k = 0; k < 32; k++) {
			anIntArray1232[k] = i - 1;
			i += i;
		}
	}
}
