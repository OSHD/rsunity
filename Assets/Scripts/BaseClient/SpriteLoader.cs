using RS2Sharp;
using System.Collections;
using System;
using System.IO;
using sign;

public class SpriteLoader {

	
	/*
     * Loads the sprite data and index files from the cache location.
     * This can be edited to use an archive such as config or media to load from the cache.
     *
     * @param archive
     */
	public static void loadSprites(StreamLoader streamLoader) {
		//try {
		RS2Sharp.Stream index = new RS2Sharp.Stream(UnityClient.ReadAllBytes(signlink.findcachedir() + "sprites.idx"));
		RS2Sharp.Stream data = new RS2Sharp.Stream(UnityClient.ReadAllBytes(signlink.findcachedir() + "sprites.dat"));
		DataInputStream indexFile = new DataInputStream(new Ionic.Zlib.GZipStream(new MemoryStream(index.buffer),Ionic.Zlib.CompressionMode.Decompress));
		DataInputStream dataFile = new DataInputStream(new Ionic.Zlib.GZipStream(new MemoryStream(data.buffer),Ionic.Zlib.CompressionMode.Decompress));
		int totalSprites = indexFile.ReadInt();
		UnityEngine.Debug.Log ("Sprites loaded: " + totalSprites);
		if (cache == null) {
			cache = new SpriteLoader[totalSprites];
			sprites = new Sprite[totalSprites];
		}
		for (int i = 0; i < totalSprites; i++) {
			int id = indexFile.ReadInt();
			if (cache[id] == null) {
				cache[id] = new SpriteLoader();
			}
			cache[id].readValues(indexFile, dataFile);
			createSprite(cache[id]);
		}
		//indexFile.Close();
		//dataFile.Close();
		//} catch (Exception e) {
		//	e.printStackTrace();
		//}
	}
	
	/*
     * Reads the information from the index and data files.
     *
     * @param index holds the sprite indices
     * @param data  holds the sprite data per index
     * @throws IOException
     */
	public void readValues(DataInputStream index, DataInputStream data) {
		do {
			int opCode = data.ReadByte();
			if (opCode == 0) {
				break;
			}
			if (opCode == 1) {
				id = data.ReadShort();
			} else if (opCode == 2) {
				name = data.ReadUTF();
			} else if (opCode == 3) {
				drawOffsetX = data.ReadShort();
			} else if (opCode == 4) {
				drawOffsetY = data.ReadShort();
			} else if (opCode == 5) {
				int indexLength = index.ReadInt();
				byte[] dataread = new byte[indexLength];
				data.Read(dataread,0,indexLength);
				spriteData = dataread;
			}
		} while (true);
	}
	
	/**
     * Creates a sprite out of the spriteData.
     *
     * @param sprite
     */
	public static void createSprite(SpriteLoader sprite) {
		/*File directory = new File(Signlink.findcachedir() + "dump");
		if (!directory.exists()) {
			directory.mkdir();
		}
		Utility.writeFile(new File(directory.getAbsolutePath() + System.getProperty("file.separator") + sprite.id + ".png"), sprite.spriteData);*/
		sprites[sprite.id] = new Sprite(sprite.spriteData);
		sprites[sprite.id].anInt1442 = sprite.drawOffsetX;
		sprites[sprite.id].anInt1443 = sprite.drawOffsetY;
	}
	
	/**
     * Gets the name of a specified sprite index.
     *
     * @param index
     * @return
     */
	public static String getName(int index) {
		if (cache[index].name != null) {
			return cache[index].name;
		} else {
			return "null";
		}
	}
	
	/**
     * Gets the drawOffsetX of a specified sprite index.
     *
     * @param index
     * @return
     */
	public static int getOffsetX(int index) {
		return cache[index].drawOffsetX;
	}
	
	/**
     * Gets the drawOffsetY of a specified sprite index.
     *
     * @param index
     * @return
     */
	public static int getOffsetY(int index) {
		return cache[index].drawOffsetY;
	}
	
	/**
     * Sets the default values.
     */
	public SpriteLoader() {
		name = "name";
		id = -1;
		drawOffsetX = 0;
		drawOffsetY = 0;
		spriteData = null;
	}
	
	public static SpriteLoader[] cache;
	public static Sprite[] sprites = null;
	public static int totalSprites;
	public String name;
	public int id;
	public int drawOffsetX;
	public int drawOffsetY;
	public byte[] spriteData;
}
