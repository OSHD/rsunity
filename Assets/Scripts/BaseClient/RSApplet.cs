using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using sign;
using UnityEngine;


namespace RS2Sharp
{
//	public enum DisplayType {
//		FIXED,//(512, 334),
//		RESIZABLE,//(800, 600),
//		FULL_SCREEN//(Client.SCREEN_SIZE.width, Client.SCREEN_SIZE.height);
//	}
//
//	public static class DisplayTypeClass {
//
//		public static int getWidth(DisplayType type) {
//			if (type == DisplayType.FIXED) return 512;
//			if (type == DisplayType.RESIZABLE) return 800;
//			if (type == DisplayType.FULL_SCREEN) return (int)RSApplet.SCREEN_SIZE.x;
//			return 512;
//		}
//
//		public static int getHeight(DisplayType type) {
//			if (type == DisplayType.FIXED) return 334;
//			if (type == DisplayType.RESIZABLE) return 600;
//			if (type == DisplayType.FULL_SCREEN) return (int)RSApplet.SCREEN_SIZE.y;
//			return 334;
//		}
//		
//	}
	public class RSApplet : UnityEngine.MonoBehaviour
	{
//		public static Vector2 SCREEN_SIZE = new Vector2 (Screen.width, Screen.height);//Toolkit.getDefaultToolkit().getScreenSize();    
//		public static DisplayType displayType = DisplayType.FULL_SCREEN;
//
//		public static bool screenFixed = (displayType == DisplayType.FIXED);
//		public static int currentWidth = screenFixed ? 765 : DisplayTypeClass.getWidth(displayType);
//		public static int currentHeight = screenFixed ? 503 : DisplayTypeClass.getHeight(displayType);
		private int lastMouseX;
		private int lastMouseY;
		public UnityEngine.Font helvetica;
		private bool inDrawLoop = false;
		private bool inGameLoop = false;
		private bool shouldDraw = false;
		public float deltaTime = 0.0f;
		public void createClientFrame(int i, int j)
		{
			myWidth = Screen.width;
			myHeight = Screen.height;
			fullGameScreen = new RSImageProducer (myWidth, myHeight);
			helvetica = new UnityEngine.Font("Helvetica");
			drawLoadingText(0, "Loading...");
			signlink.run ();
			Loom.StartSingleThread(()=>{startUp ();},System.Threading.ThreadPriority.Normal,true);
			for (int k1 = 0; k1 < 10; k1++)
				aLongArray7[k1] = NetDrawingTools.CurrentTimeMillis();
		}

		private void exit()
		{
			UnityEngine.Debug.Log ("Exit");
			anInt4 = -2;
			cleanUpForQuit();
		}

		protected void method4(int i)
		{
			delayTime = 1000 / i;
		}

		public void start()
		{
			if (anInt4 >= 0)
				anInt4 = 0;
		}

		public void stop()
		{
			if (anInt4 >= 0)
				anInt4 = 4000 / delayTime;
		}

		public void destroy()
		{
			anInt4 = -1;
			if (anInt4 == -1)
				exit();
		}

//		public void update(object sender)
//		{
//			//if (graphics == null)
//			//	graphics = this.CreateGraphics();
//			shouldClearScreen = true;
//			raiseWelcomeScreen();
//		}

		public void paint(object sender)
		{
			//if (graphics == null)
			//	graphics = e.Graphics;
			shouldClearScreen = true;
			raiseWelcomeScreen();
		}



		public void mousePressed( int mouseX, int mouseY, bool left)
		{
			int i = mouseX;
			int j = mouseY;
			if (gameFrame != null)
			{
				i -= 4;
				j -= 22;
			}
			idleTime = 0;
			clickX = i;
			clickY = j;
			clickTime = NetDrawingTools.CurrentTimeMillis();
			if (!left)
			{
				clickMode1 = 2;
				clickMode2 = 2;
			}
			else
			{
				clickMode1 = 1;
				clickMode2 = 1;
			}
			clickMode3 = clickMode1;
		}

		public void mouseReleased( int mouseX, int mouseY, bool left)
		{
			idleTime = 0;
			clickMode2 = 0;
		}

		public virtual void clientUpdate()
		{

		}

		public void ClientUpdateLoop()
		{
			//startUp();
			 int i = 0;
        int j = 256;
        int k = 1;
        int l = 0;
        int i1 = 0;
		 for(int j1 = 0; j1 < 10; j1++)
        {
            aLongArray7[j1] = NetDrawingTools.CurrentTimeMillis();
        }
			while (true) {
// 				if((NetDrawingTools.CurrentTimeMillis() - lastGameFrame) >= 20)
// 				{
// 					clientUpdate();
// 					lastGameFrame = NetDrawingTools.CurrentTimeMillis();
// 				}
				
				if(anInt4 < 0)
            {
                break;
            }
            if(anInt4 > 0)
            {
                anInt4--;
                if(anInt4 == 0)
                {
                    exit();
                    return;
                }
            }
            int k1 = j;
            int i2 = k;
            j = 300;
            k = 1;
            long l2 = NetDrawingTools.CurrentTimeMillis();
            if(aLongArray7[i] == 0L)
            {
                j = k1;
                k = i2;
            } else
            if(l2 > aLongArray7[i])
            {
                j = (int)((long)(2560 * delayTime) / (l2 - aLongArray7[i]));
            }
            if(j < 25)
            {
                j = 25;
            }
            if(j > 256)
            {
                j = 256;
                k = (int)((long)delayTime - (l2 - aLongArray7[i]) / 10L);
            }
            if(k > delayTime)
            {
                k = delayTime;
            }
            aLongArray7[i] = l2;
            i = (i + 1) % 10;
            if(k > 1)
            {
                for(int j2 = 0; j2 < 10; j2++)
                {
                    if(aLongArray7[j2] != 0L)
                    {
                        aLongArray7[j2] += k;
                    }
                }

            }
            if(k < minDelay)
            {
                k = minDelay;
            }
            try
            {
                Thread.Sleep(k);
            }
            catch(Exception interruptedexception)
            {
                i1++;
            }
            for(; l < 256; l += j)
            {
                clickMode3 = clickMode1;
                saveClickX = clickX;
                saveClickY = clickY;
                aLong29 = clickTime;
                clickMode1 = 0;
                processGameLoop();
                readIndex = writeIndex;
            }

            l &= 0xff;
            if(delayTime > 0)
            {
                fps = (1000 * j) / (delayTime * 256);
            }
				shouldDraw = true;
			}
		}
// 
		public void ClientDrawLoop()
		{
			while (true) {
					if(shouldDraw)
					{
						shouldDraw = false;
						processDrawing();
						}
			}
		}

		public void Update()
		{
			deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
			//SCREEN_SIZE = new Vector2 (Screen.width, Screen.height);
			int mouseY = UnityEngine.Screen.height - (int)UnityEngine.Input.mousePosition.y;
			if(UnityEngine.Input.GetMouseButtonDown(0))
			{
				mousePressed((int)UnityEngine.Input.mousePosition.x,mouseY,true);
			}
			if(UnityEngine.Input.GetMouseButtonDown(1))
			{
				mousePressed((int)UnityEngine.Input.mousePosition.x,mouseY,false);
			}
			if(UnityEngine.Input.GetMouseButtonUp(0))
			{
				mouseReleased((int)UnityEngine.Input.mousePosition.x,mouseY,true);
			}
			if(UnityEngine.Input.GetMouseButtonUp(1))
			{
				mouseReleased((int)UnityEngine.Input.mousePosition.x,mouseY,false);
			}
			if(((int)UnityEngine.Input.mousePosition.x != lastMouseX) && ((int)UnityEngine.Input.mousePosition.y != lastMouseY)) mouseMoved((int)UnityEngine.Input.mousePosition.x,mouseY);
			lastMouseX = (int)UnityEngine.Input.mousePosition.x;
			lastMouseY = (int)UnityEngine.Input.mousePosition.y;
			if(UnityEngine.Input.inputString != "") currentInput = UnityEngine.Input.inputString;
			if(UnityEngine.Input.GetKey(UnityEngine.KeyCode.Backspace)) currentInput = "Backspace";
			if(UnityEngine.Input.GetKey(UnityEngine.KeyCode.KeypadEnter)) currentInput = "Enter";
			if(UnityEngine.Input.GetKey(UnityEngine.KeyCode.Return)) currentInput = "Enter";
			if(!client.instance.currentlyStarting)
			{
					if(!inGameLoop)
					{
						inGameLoop = true;
						Loom.StartSingleThread(()=>{ClientUpdateLoop();},System.Threading.ThreadPriority.Normal,true);
					}
					if(!inDrawLoop)
					{
						inDrawLoop = true;
						Loom.StartSingleThread(()=>{ClientDrawLoop();},System.Threading.ThreadPriority.Normal,true);
					}
//				processGameLoop();
			//	processDrawing();
					renderWorld();
			}
			
			if(Screen.width != graphics.currentScreenWidth || Screen.height != graphics.currentScreenHeight)
			{
				graphics.RefreshScreen(Screen.width,Screen.height);
				myWidth = Screen.width;
				myHeight = Screen.height;
				setBounds();
			}
			
		}
		
		public virtual void renderWorld()
		{
			
		}
		public virtual void setBounds()
		{
			
		}
		public void mouseClicked(object sender)
		{
		}

		public void mouseEntered(object sender, EventArgs e)
		{
		}

		public void mouseExited(object sender, EventArgs e)
		{
			idleTime = 0;
			mouseX = -1;
			mouseY = -1;
		}

		public void mouseDragged(object sender, int mouseX, int mouseY)
		{
			int i = mouseX;
			int j = mouseY;
			if (gameFrame != null)
			{
				i -= 4;
				j -= 22;
			}
			idleTime = 0;
			this.mouseX = i;
			this.mouseY = j;
		}

		public void mouseMoved(int mouseX, int mouseY)
		{
			int i = mouseX;
			int j = mouseY;
			if (gameFrame != null)
			{
				i -= 4;
				j -= 22;
			}
			idleTime = 0;
			this.mouseX = i;
			this.mouseY = j;
		}

		public void keyPressed(string e)
		{
			idleTime = 0;
			int i = 0;//(int)e.KeyValue;
//			if (e == Keys.Enter)
//				i = 10;
			if (e == "LeftArrow")
				i = 1;
			else if (e == "RightArrow")
				i = 2;
			else if (e == "UpArrow")
				i = 3;
			else if (e == "DownArrow")
				i = 4;
			else if (e == "Backspace" || e == "Delete")
				i = 8;
//			else if (e == Keys.Control)
//				i = 5;
			else if (e == "Tab")
				i = 9;
//			else if (e.KeyValue >= (int)Keys.F1 && e.KeyValue <= (int)Keys.F12)
//				i = 1008 + e.KeyValue - (int)Keys.F1;
//			else if (e == Keys.Home)
//				i = 1000;
//			else if (e == Keys.End)
//				i = 1001;
//			else if (e == Keys.PageUp)
//				i = 1002;
//			else if (e == Keys.PageDown)
//				i = 1003;
			else
				return;

			if (i > 0 && i < 128)
				keyArray[i] = 1;
			if (i > 4)
			{
				charQueue[writeIndex] = i;
				writeIndex = writeIndex + 1 & 0x7f;
			}
		}

		//private KeysConverter kc = new KeysConverter();
		public void keyReleased(string e)
		{
			idleTime = 0;
			int i = 0;//(int)e.KeyValue;
//			if (e == Keys.Enter)
//				i = 10;
			if (e == "LeftArrow")
				i = 37;
			else if (e == "RightArrow")
				i = 39;
			else if (e == "UpArrow")
				i = 38;
			else if (e == "DownArrow")
				i = 40;
			else if (e == "Back" || e == "Delete")
				i = 8;
//			else if (e == Keys.Control)
//				i = 5;
			else if (e == "Tab")
				i = 9;
//			else if (e.KeyValue >= (int)Keys.F1 && e.KeyValue <= (int)Keys.F12)
//				i = 1008 + e.KeyValue - (int)Keys.F1;
//			else if (e == Keys.Home)
//				i = 1000;
//			else if (e == Keys.End)
//				i = 1001;
//			else if (e == Keys.PageUp)
//				i = 1002;
//			else if (e == Keys.PageDown)
//				i = 1003;
			else
			{
				string s = e;//kc.ConvertToString(e);
				if (s.Length > 0)
					i = s[0];
			}
			char c = (char)i;
			if (c < (char)36)
				c = (char)0;
			if (i == 37)
				c = (char)1;
			if (i == 39)
				c = (char)2;
			if (i == 38)
				c = (char)3;
			if (i == 40)
				c = (char)4;
			if (i == 17)
				c = (char)5;
			if (i == 8)
				c = '\b';
			if (i == 127)
				c = '\b';
			if (i == 9)
				c = '\t';
			if (i == 10)
				c = '\n';
			if (c > 0 && c < (char)200)
				keyArray[c] = 0;
		}/*

		public void keyTyped(object sender, KeyPressEventArgs e)
		{
			idleTime = 0;
			int i = (int)e.KeyChar;
			if (e.KeyChar >= 32 && e.KeyChar <= 126)
			{
				charQueue[writeIndex] = i;
				writeIndex = writeIndex + 1 & 0x7f;
			}
		}

		public int readChar(int dummy)
		{
			while (dummy >= 0)
			{
				for (int j = 1; j > 0; j++) ;
			}
			int k = -1;
			if (writeIndex != readIndex)
			{
				k = charQueue[readIndex];
				readIndex = readIndex + 1 & 0x7f;
			}
			return k;
		}
*/
		public void focusGained(object sender, EventArgs e)
		{
			awtFocus = true;
			shouldClearScreen = true;
			raiseWelcomeScreen();
		}

		public void focusLost(object sender, EventArgs e)
		{
			awtFocus = false;
			for (int i = 0; i < 128; i++)
				keyArray[i] = 0;

		}

		public void windowActivated(object sender, EventArgs e)
		{
		}

		public void OnGUI()
		{
			UnityEngine.Event e = UnityEngine.Event.current;
			if (e.isKey && e.type == UnityEngine.EventType.KeyDown){
				keyPressed(e.keyCode.ToString());
			}
			else if (e.isKey && e.type == UnityEngine.EventType.KeyUp){
				keyReleased(e.keyCode.ToString());
			}
		}

		public void windowDeactivated(object sender, EventArgs e)
		{
		}

		public void windowDeiconified(object sender, EventArgs e)
		{
		}

		public void windowIconified(object sender, EventArgs e)
		{
		}

		public void windowOpened(object sender, EventArgs e)
		{
		}

		public virtual void startUp()
		{
		}

		public virtual void processGameLoop()
		{
		}

		public virtual void cleanUpForQuit()
		{
		}

		public virtual void processDrawing()
		{
		}

		public virtual void raiseWelcomeScreen()
		{
		}

		/*Component getGameComponent()
		{
			if(gameFrame != null)
				return gameFrame;
			else
				return this;
		}*/

		/*public void startRunnable(ThreadStart t, int priority)
		{
			Thread thread = new Thread(t);
			thread.Start();
			//thread.Priority = (ThreadPriority)priority;
		}*/

		public virtual void drawLoadingText(int i, String s)
		{
			//while (graphics == null)
			//{
				// graphics = getGameComponent().getGraphics();

			//}
			UnityEngine.Font font = helvetica;//new UnityEngine.Font("Helvetica", 13, UnityEngine.FontStyle.Bold);
			UnityEngine.Font font1 = helvetica;//new UnityEngine.Font("Helvetica", 13, UnityEngine.FontStyle.Regular);
			//getGameComponent().getUnityEngine.FontMetrics(font1);
			if (shouldClearScreen)
			{
				graphics.FillRectangle(0, 0, myWidth, myHeight);
				shouldClearScreen = false;
			}
			//SolidBrush color = new SolidBrush(Color.FromArgb(140, 17, 17));
			//Pen penColor = new Pen(color);
			int j = myHeight / 2 - 18;
			graphics.DrawRectangle(myWidth / 2 - 152, j, 304, 34);
			graphics.FillRectangle(myWidth / 2 - 150, j + 2, i * 3, 30);
			graphics.FillRectangle((myWidth / 2 - 150) + i * 3, j + 2, 300 - i * 3, 30);
			graphics.DrawString(s, font, (myWidth - graphics.MeasureString(s, font)) / 2, j + 22);
		}

		public RSApplet()
		{
			delayTime = 20;
			minDelay = 1;
			aLongArray7 = new long[10];
			shouldDebug = false;
			shouldClearScreen = true;
			awtFocus = true;
			keyArray = new int[128];
			charQueue = new int[128];
		}

		private int anInt4;
		public int delayTime;
		public int minDelay;
		private long[] aLongArray7;
		public int fps;
		public bool shouldDebug;
		public int myWidth;
		public int myHeight;
		public Graphics graphics;
		public RSImageProducer fullGameScreen;
		public object gameFrame = null; //This is unneeded.
		private bool shouldClearScreen;
		public bool awtFocus;
		public int idleTime;
		public int clickMode2;
		public int mouseX;
		public int mouseY;
		public int clickMode1;
		public int clickX;
		public int clickY;
		public long clickTime;
		public int clickMode3;
		public int saveClickX;
		public int saveClickY;
		public long aLong29;
		public int[] keyArray;
		private int[] charQueue;
		public int readIndex;
		public int writeIndex;
		public static int anInt34;
		public string currentInput = "";
	}
}
