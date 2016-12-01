using System.IO;

public class NewByteBuffer
{
	//'Mode' is only used to determine whether to return data length or capacity from the 'limit' method:
	private enum Mode
	{
		Read,
		Write
	}
	private Mode mode;
	
	private System.IO.MemoryStream stream;
	private System.IO.BinaryReader reader;
	private System.IO.BinaryWriter writer;
	
	public NewByteBuffer(byte[] buffer)
	{
		stream = new System.IO.MemoryStream(buffer);
		reader = new System.IO.BinaryReader(stream);
		writer = new System.IO.BinaryWriter(stream);
	}
	
	~NewByteBuffer()
	{
		reader.Close();
		writer.Close();
		stream.Close();
		stream.Dispose();
	}
	
	public static NewByteBuffer allocate(int capacity)
	{
		NewByteBuffer buffer = new NewByteBuffer(null);
		buffer.stream.Capacity = capacity;
		buffer.mode = Mode.Write;
		return buffer;
	}
	
	public static NewByteBuffer allocateDirect(int capacity)
	{
		//this wrapper class makes no distinction between 'allocate' & 'allocateDirect'
		return allocate(capacity);
	}
	
	public int capacity()
	{
		return stream.Capacity;
	}
	
	public NewByteBuffer flip()
	{
		mode = Mode.Read;
		stream.SetLength(stream.Position);
		stream.Position = 0;
		return this;
	}
	
	public NewByteBuffer clear()
	{
		mode = Mode.Write;
		stream.Position = 0;
		return this;
	}
	
//	public NewByteBuffer compact()
//	{
//		mode = Mode.Write;
//		System.IO.MemoryStream newStream = new System.IO.MemoryStream(stream.Capacity);
//		CopyTo(newStream);
//		stream = newStream;
//		return this;
//	}
//	
//	public void CopyTo(this Stream input, Stream output)
//	{
//		byte[] buffer = new byte[16 * 1024]; // Fairly arbitrary size
//		int bytesRead;
//		
//		while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
//		{
//			output.Write(buffer, 0, bytesRead);
//		}
//	}
	
	public NewByteBuffer rewind()
	{
		stream.Position = 0;
		return this;
	}
	
	public long limit()
	{
		if (mode == Mode.Write)
			return stream.Capacity;
		else
			return stream.Length;
	}
	
	public long position()
	{
		return stream.Position;
	}
	
	public NewByteBuffer position(long newPosition)
	{
		stream.Position = newPosition;
		return this;
	}
	
	public long remaining()
	{
		return this.limit() - this.position();
	}
	
	public bool hasRemaining()
	{
		return this.remaining() > 0;
	}
	
	public int get()
	{
		return stream.ReadByte();
	}
	
	public NewByteBuffer get(byte[] dst, int offset, int length)
	{
		stream.Read(dst, offset, length);
		return this;
	}
	
	public NewByteBuffer put(byte b)
	{
		stream.WriteByte(b);
		return this;
	}
	
	public NewByteBuffer put(byte[] src, int offset, int length)
	{
		stream.Write(src, offset, length);
		return this;
	}
	
	public bool Equals(NewByteBuffer other)
	{
		if (other != null && this.remaining() == other.remaining())
		{
			long thisOriginalPosition = this.position();
			long otherOriginalPosition = other.position();
			
			bool differenceFound = false;
			while (stream.Position < stream.Length)
			{
				if (this.get() != other.get())
				{
					differenceFound = true;
					break;
				}
			}
			
			this.position(thisOriginalPosition);
			other.position(otherOriginalPosition);
			
			return ! differenceFound;
		}
		else
			return false;
	}
	
	//methods using the internal BinaryReader:
	public char getChar()
	{
		return reader.ReadChar();
	}
	public char getChar(int index)
	{
		long originalPosition = stream.Position;
		stream.Position = index;
		char value = reader.ReadChar();
		stream.Position = originalPosition;
		return value;
	}
	public double getDouble()
	{
		return reader.ReadDouble();
	}
	public double getDouble(int index)
	{
		long originalPosition = stream.Position;
		stream.Position = index;
		double value = reader.ReadDouble();
		stream.Position = originalPosition;
		return value;
	}
	public float getFloat()
	{
		return reader.ReadSingle();
	}
	public float getFloat(int index)
	{
		long originalPosition = stream.Position;
		stream.Position = index;
		float value = reader.ReadSingle();
		stream.Position = originalPosition;
		return value;
	}
	public int getInt()
	{
		return reader.ReadInt32();
	}
	public int getInt(int index)
	{
		long originalPosition = stream.Position;
		stream.Position = index;
		int value = reader.ReadInt32();
		stream.Position = originalPosition;
		return value;
	}
	public long getLong()
	{
		return reader.ReadInt64();
	}
	public long getLong(int index)
	{
		long originalPosition = stream.Position;
		stream.Position = index;
		long value = reader.ReadInt64();
		stream.Position = originalPosition;
		return value;
	}
	public short getShort()
	{
		return reader.ReadInt16();
	}
	public short getShort(int index)
	{
		long originalPosition = stream.Position;
		stream.Position = index;
		short value = reader.ReadInt16();
		stream.Position = originalPosition;
		return value;
	}
	
	//methods using the internal BinaryWriter:
	public NewByteBuffer putChar(char value)
	{
		writer.Write(value);
		return this;
	}
	public NewByteBuffer putChar(int index, char value)
	{
		long originalPosition = stream.Position;
		stream.Position = index;
		writer.Write(value);
		stream.Position = originalPosition;
		return this;
	}
	public NewByteBuffer putDouble(double value)
	{
		writer.Write(value);
		return this;
	}
	public NewByteBuffer putDouble(int index, double value)
	{
		long originalPosition = stream.Position;
		stream.Position = index;
		writer.Write(value);
		stream.Position = originalPosition;
		return this;
	}
	public NewByteBuffer putFloat(float value)
	{
		writer.Write(value);
		return this;
	}
	public NewByteBuffer putFloat(int index, float value)
	{
		long originalPosition = stream.Position;
		stream.Position = index;
		writer.Write(value);
		stream.Position = originalPosition;
		return this;
	}
	public NewByteBuffer putInt(int value)
	{
		writer.Write(value);
		return this;
	}
	public NewByteBuffer putInt(int index, int value)
	{
		long originalPosition = stream.Position;
		stream.Position = index;
		writer.Write(value);
		stream.Position = originalPosition;
		return this;
	}
	public NewByteBuffer putLong(long value)
	{
		writer.Write(value);
		return this;
	}
	public NewByteBuffer putLong(int index, long value)
	{
		long originalPosition = stream.Position;
		stream.Position = index;
		writer.Write(value);
		stream.Position = originalPosition;
		return this;
	}
	public NewByteBuffer putShort(short value)
	{
		writer.Write(value);
		return this;
	}
	public NewByteBuffer putShort(int index, short value)
	{
		long originalPosition = stream.Position;
		stream.Position = index;
		writer.Write(value);
		stream.Position = originalPosition;
		return this;
	}
}