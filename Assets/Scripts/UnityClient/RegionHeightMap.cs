using UnityEngine;
using System.Collections;

public class RegionHeightMap {
	public RegionHeightMap(int[][][] tile_height, int rX, int rY)
	{
		this.tile_height = tile_height;
		this.rX = rX;
		this.rY = rY;
	}
	public int[][][] tile_height;
	public int rX;
	public int rY;
	public int[][][] preMade;
}
