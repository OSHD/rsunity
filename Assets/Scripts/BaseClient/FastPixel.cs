using System;
using UnityEngine;

namespace RS2Sharp
{
	public class FastPixel
	{
		public byte[] rgbValues = new byte[4];
		//private BitmapData bmpData;
		private IntPtr bmpPtr;
		//private bool locked = false;

		private bool _isAlpha = false;
		//private Texture2D _bitmap;
		private int _width;
		private int _height;
		public int Width
		{
			get { return this._width; }
		}
		public int Height
		{
			get { return this._height; }
		}
		public bool IsAlphaBitmap
		{
			get { return this._isAlpha; }
		}
		//public Texture2D Bitmap
		//{
		//	get { return this._bitmap; }
		//}

		public FastPixel(int width, int height)
		{
		
			//this._bitmap = bitmap;
			this._isAlpha = false;//(this.Bitmap.format == (TextureFormat.Alpha8));
			//Loom.DispatchToMainThread (() => {
								this._width = width;
								this._height = height;
					//	}, true, false);
		}

		public void SetPixel(Vector2 location, Color32 colour)
		{
			this.SetPixel((int)location.x, (int)location.y, colour);
		}
		public void SetPixel(int x, int y, Color32 colour)
		{
			//if (!this.locked)
		//	{
			//	throw new Exception("Bitmap not locked.");
			//}

//			if (this.IsAlphaBitmap)
//			{
//				int index = ((y * this.Width + x) * 4);
//				this.rgbValues[index] = colour.r;
//				this.rgbValues[index + 1] = colour.g;
//				this.rgbValues[index + 2] = colour.b;
//				this.rgbValues[index + 3] = colour.a;
//			}
//			else
//			{
				int index = ((y * this.Width + x) * 3);
				this.rgbValues[index] = colour.r;
				this.rgbValues[index + 1] = colour.g;
				this.rgbValues[index + 2] = colour.b;
			//}
		}
		public Color32 GetPixel(Vector2 location)
		{
			return this.GetPixel((int)location.x, (int)location.y);
		}
		public Color32 GetPixel(int x, int y)
		{
			//if (!this.locked)
			//{
			//	throw new Exception("Bitmap not locked.");
			//}

			if (this.IsAlphaBitmap)
			{
				int index = ((y * this.Width + x) * 4);
				byte r = this.rgbValues[index];
				byte g = this.rgbValues[index + 1];
				byte b = this.rgbValues[index + 2];
				byte a = this.rgbValues[index + 3];
				return new Color32(r,g,b,a);
			}
			else
			{
				int index = ((y * this.Width + x) * 3);
				byte r = this.rgbValues[index];
				byte g = this.rgbValues[index + 1];
				byte b = this.rgbValues[index + 2];
				return new Color32(r,g,b,(byte)255);
			}
		}
	}
}