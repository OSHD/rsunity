using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RS2Sharp
{
	public class Graphics : MonoBehaviour
	{

		private Texture2D mainImage;
		public RawImage gameMat;
		public RenderTexture mainRenderTexture;
		public int currentScreenWidth = 768;
		public int currentScreenHeight = 503;
		public static byte[] gameScreen = null;
		public static byte[] gameScreen2 = null;
		public bool dirty = false;

		public void FillRectangle (int a, int b, int myWidth, int myHeight)
		{

		}

		public void DrawRectangle (int a, int b, int c, int d)
		{

		}

		public int MeasureString (string s, Font f)
		{
			return 1;
		}

		public void DrawString (string s, Font f, int a, int b)
		{
		}

		public void RefreshScreen (int newX, int newY)
		{
			Debug.Log ("Refresh Screen Size");
			Loom.DispatchToMainThread (() => {
				currentScreenWidth = newX;
				currentScreenHeight = newY;
				mainImage = new Texture2D ((int)currentScreenWidth, (int)currentScreenHeight, TextureFormat.RGB24, false);
				mainImage.filterMode = FilterMode.Point;
				gameMat.texture = mainImage;
				//gameScreen = new byte[currentScreenWidth * currentScreenHeight * 4];
			}, true, true);

		}

		public void Start ()
		{
		//mainRenderTexture = new RenderTexture(768,503,24);
		//gameMat.texture = mainRenderTexture;
			RefreshScreen (currentScreenWidth, currentScreenHeight);
			mainImage = new Texture2D ((int)currentScreenWidth, (int)currentScreenHeight, TextureFormat.RGB24, false);
			mainImage.filterMode = FilterMode.Point;
			gameMat.texture = mainImage;
			gameScreen = null;
			gameScreen2 = null;
			//gameScreen = new byte[currentScreenWidth * currentScreenHeight * 4];
		}

		public void Update ()
		{
			if (dirty) {
				dirty = false;
				//mainImage.LoadRawTextureData (gameScreen);
				if(gameScreen != null && gameScreen2 != null)
				{
					byte[] finalImage = new byte[gameScreen.Length + gameScreen2.Length];
					for(int i = 0 ; i < gameScreen.Length; ++i) finalImage[i] = gameScreen[i];
					for(int i = 0 ; i < gameScreen2.Length; ++i) finalImage[i+gameScreen.Length] = gameScreen2[i];
					mainImage.LoadImage(finalImage);
					mainImage.Apply ();
				}
				gameScreen = null;
				gameScreen2 = null;
				//UnityEngine.Graphics.Blit(mainImage, mainRenderTexture);
			}
		}

//		public void DrawImage (int[] pixels, int x, int y, int width, int height)//(int[] pixels, int x, int y, int width, int height)
//		{
//
//			dirty = true;
//			int colour = 0;
//			int index = 0;
//			for (int xx = 0; xx < width; xx++) {
//			for (int yy = 0; yy < height; yy++) {
//					colour = pixels [xx + yy * width];
//					index = (((yy+y)*currentScreenWidth+(xx+x))*4);//((yy * width + xx) * 4);
//					if(allBytes.Length > index) allBytes [index] = (byte)(((colour >> 16) & 0xFF));
//					if(allBytes.Length > index+1) allBytes [index + 1] = (byte)(((colour >> 8) & 0xFF));
//					if(allBytes.Length > index+2) allBytes [index + 2] = (byte)((colour & 0xFF));
//					if(allBytes.Length > index+3) allBytes [index + 3] = colour == 65281 ? (byte)0 : (byte)255;
//				}
//			}
//
//		}
		
	}
}