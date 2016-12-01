using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace RS2Sharp
{

public class RSSocket : MonoBehaviour
{

    public void Init(RSApplet RSApplet_, Socket socket1)
    {
        closed = false;
        isWriter = false;
        hasIOError = false;
        rsApplet = RSApplet_;
        socket = socket1;
			socket.ReceiveTimeout = 30000;
			socket.SendTimeout = 30000;
		socket.NoDelay = true;
		inputStream = new NetworkStream(socket);
		outputStream = new NetworkStream(socket);
    }

//	public void Update()
//		{
//			run ();
//		}

    public void close()
    {
        closed = true;
        try
        {
            if(inputStream != null)
                inputStream.Close();
            if(outputStream != null)
                outputStream.Close();
            if(socket != null)
                socket.Close();
        }
        catch(SocketException _ex)
        {
            UnityEngine.Debug.Log("Error closing stream");
        }
        isWriter = false;
        lock(this)
        {
            
        }
        buffer = null;
    }

    public int read()
    {
		if (closed)
			return 0;
		else
			return inputStream.ReadByte();
    }

    public int available()
    {
        if(closed)
            return 0;
        else
            return socket.Available;
    }

    public void flushInputStream(byte[] abyte0, int j)
    {
        int i = 0;//was parameter
        if(closed)
            return;
        int k;
        for(; j > 0; j -= k)
        {
            k = inputStream.Read(abyte0, i, j);
            if(k <= 0)
                throw new Exception("EOF");
            i += k;
        }

    }

		public void queueBytes(int i, byte[] abyte0)
    {
        if(closed)
            return;
        if(hasIOError)
        {
            hasIOError = false;
            throw new Exception("Error in writer thread");
        }
        if(buffer == null)
            buffer = new byte[5000];
        lock(this)
        {
            for(int l = 0; l < i; l++)
            {
                buffer[buffIndex] = abyte0[l];
                buffIndex = (buffIndex + 1) % 5000;
                if(buffIndex == (writeIndex + 4900) % 5000)
					throw new Exception("buffer overflow");
            }

            if(!isWriter)
            {
                isWriter = true;
                //rsApplet.startRunnable(new ThreadStart(run), 3);
					Loom.StartSingleThread(()=>run(),System.Threading.ThreadPriority.Normal,true);
					//run ();
            }
			Monitor.Pulse(this);
        }
    }

    public void run()
    {
			//if(!client.finished)
        //{
        while(isWriter)
        {
            int i;
            int j;
            lock(this)
            {
                if(buffIndex == writeIndex)
                    try
                    {
						Monitor.Wait(this);
                    }
                    catch(Exception _ex) { }
                if(!isWriter)
                    return;
                j = writeIndex;
                if(buffIndex >= writeIndex)
                    i = buffIndex - writeIndex;
                else
                    i = 5000 - writeIndex;
            }
            if(i > 0)
            {
                try
                {
                    outputStream.Write(buffer, j, i);
                }
                catch(IOException _ex)
                {
                    hasIOError = true;
                }
                writeIndex = (writeIndex + i) % 5000;
                try
                {
                    if(buffIndex == writeIndex)
                        outputStream.Flush();
                }
				catch (SocketException _ex)
                {
						Debug.Log (_ex.Message);
                    hasIOError = true;
                }
            }
           }
      //  }
    }

    public void printDebug()
    {
        UnityEngine.Debug.Log("dummy:" + closed);
        UnityEngine.Debug.Log("tcycl:" + writeIndex);
        UnityEngine.Debug.Log("tnum:" + buffIndex);
        UnityEngine.Debug.Log("writer:" + isWriter);
        UnityEngine.Debug.Log("ioerror:" + hasIOError);
        try
        {
            UnityEngine.Debug.Log("available:" + available());
        }
        catch(SocketException _ex)
        {
        }
    }

    private NetworkStream inputStream;
    private NetworkStream outputStream;
    private Socket socket;
    private bool closed;
    private RSApplet rsApplet;
    private byte[] buffer;
    private int writeIndex;
    private int buffIndex;
    public bool isWriter;
    private bool hasIOError;
}

}
