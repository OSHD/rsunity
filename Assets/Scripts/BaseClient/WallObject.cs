using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS2Sharp
{
	// Decompiled by Jad v1.5.8f. Copyright 2001 Pavel Kouznetsov.
	// Jad home page: http://www.kpdus.com/jad.html
	// Decompiler options: packimports(3) 
	public class WallObject
	{

	public int spawnType;
	public int orientation;
	public int centre;
	public int east;
	public int northEast;
	public int north;
	public int id;
	public int anim;
	public int type;
	
	public int anInt276;
	public int anInt277;
	private byte config;
	private int height;
	private int key;
	private int positionX;
	private int positionY;
		public Animable primary;
		public Animable secondary;
	public RuneObject runeObj;
	
	/**
	 * Gets the secondary.
	 *
	 * @return The secondary.
	 */
	public Animable getSecondary() {
		return secondary;
	}
	
	/**
	 * Sets the secondary.
	 *
	 * @param secondary The secondary.
	 */
	public void setSecondary(Animable secondary) {
		this.secondary = secondary;
//		if(secondary != null)
//		{
//			secondary.wallObj = this;
//			secondary.switchType = 1;
//		}
	}
	
	/**
	 * Gets the primary.
	 *
	 * @return The primary.
	 */
	public Animable getPrimary() {
		return primary;
	}
	
	/**
	 * Sets the primary.
	 *
	 * @param primary The primary.
	 */
	public void setPrimary(Animable primary) {
		this.primary = primary;
//		if(primary != null)
//		{
//			primary.wallObj = this;
//			primary.switchType = 1;
//		}
	}
	
	/**
	 * Gets the positionY.
	 *
	 * @return The positionY.
	 */
	public int getPositionY() {
		return positionY;
	}
	
	/**
	 * Sets the positionY.
	 *
	 * @param positionY The positionY.
	 */
	public void setPositionY(int positionY) {
		this.positionY = positionY;
	}
	
	/**
	 * Gets the positionX.
	 *
	 * @return The positionX.
	 */
	public int getPositionX() {
		return positionX;
	}
	
	/**
	 * Sets the positionX.
	 *
	 * @param positionX The positionX.
	 */
	public void setPositionX(int positionX) {
		this.positionX = positionX;
	}
	
	/**
	 * Gets the height.
	 *
	 * @return The height.
	 */
	public int getHeight() {
		return height;
	}
	
	/**
	 * Sets the height.
	 *
	 * @param height The height.
	 */
	public void setHeight(int height) {
		this.height = height;
	}
	
	/**
	 * Gets the key.
	 *
	 * @return The key.
	 */
	public int getKey() {
		return key;
	}
	
	/**
	 * Sets the key.
	 *
	 * @param key The key.
	 */
	public void setKey(int key) {
		this.key = key;
	}
	
	/**
	 * Gets the config.
	 *
	 * @return The config.
	 */
	public byte getConfig() {
		return config;
	}
	
	/**
	 * Sets the config.
	 *
	 * @param config The config.
	 */
	public void setConfig(byte config) {
		this.config = config;
	}
	}
}
