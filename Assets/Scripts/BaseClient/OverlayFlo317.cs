using UnityEngine;
using System.Collections;
using RS2Sharp;

public class OverLayFlo317 {
	
	public bool aBoolean393;//To add
	public static OverLayFlo317[] overLayFlo317s;
	
	public int textureId = -1;
	public int rgb;
	public bool boolean_5;
	public int int_7;
	public int int_9;
	public bool boolean_10;
	public int int_11;
	public bool boolean_12;
	public int int_13;
	public int int_14;
	public int int_15;
	public int int_16;
	public int alpha = 255;
	/**
     * Hsl
     */
	public int anInt394;
	public int anInt395;
	public int anInt396;
	public int anInt397;
	public int anInt398;
	public int anInt399;
	
	public static void unpackConfig(StreamLoader streamLoader) {
		ByteBuffer buffer = ByteBuffer.Wrap(streamLoader.getDataForName("flo2.dat"));
		int count = buffer.GetShort();
		overLayFlo317s = new OverLayFlo317[count];
		for (int i = 0; i < count ; i++) {
			if (overLayFlo317s[i] == null)
				overLayFlo317s[i] = new OverLayFlo317();
			overLayFlo317s[i].parse(buffer);
		}
		Debug.Log("530 Overlay Floors amount: "+count);
	}
	
	private void parse(ByteBuffer byteBuffer) {
		for (;;) {
			int attributeId = byteBuffer.Get();
			if (attributeId == 0)
				break;
			else if (attributeId == 1) {
				rgb = ((byteBuffer.Get() & 0xff) << 16) + ((byteBuffer.Get() & 0xff) << 8) + (byteBuffer.Get() & 0xff);
				method262(rgb);
			} else if (attributeId == 2) {
				textureId = byteBuffer.Get() & 0xff;
			} else if (attributeId == 3) {
				textureId = byteBuffer.GetShort() & 0xffff;
				if (textureId == 65535) {
					textureId = -1;
				}
			} else if (attributeId == 4) {
				
			} else if (attributeId == 5) {
				boolean_5 = false;
			} else if (attributeId == 6) {
				
			} else if (attributeId == 7) {
				int_7 = ((byteBuffer.Get() & 0xff) << 16) + ((byteBuffer.Get() & 0xff) << 8) + (byteBuffer.Get() & 0xff);
			} else if (attributeId == 8) {
				
			} else if (attributeId == 9) {
				int_9 = byteBuffer.GetShort() & 0xffff;
			} else if (attributeId == 10) {
				boolean_10 = false;
			} else if (attributeId == 11) {
				int_11 = byteBuffer.Get() & 0xff;
			} else if (attributeId == 12) {
				boolean_12 = true;
			} else if (attributeId == 13) {
				int_13 = ((byteBuffer.Get() & 0xff) << 16) + ((byteBuffer.Get() & 0xff) << 8) + (byteBuffer.Get() & 0xff);
			} else if (attributeId == 14) {
				int_14 = byteBuffer.Get() & 0xff;
			} else if (attributeId == 15) {
				int_15 = byteBuffer.GetShort() & 0xffff;
				if (int_15 == 65535) {
					int_15 = -1;
				}
			} else if (attributeId == 16) {
				int_16 = byteBuffer.Get() & 0xff;
			} else {
				Debug.Log("[OverlayFloor] Missing AttributeId: "+attributeId);
			}
		}
	}
	
	private void method262(int arg0) {
		double d = (double) (arg0 >> 16 & 0xff) / 256.0;
		double d_5_ = (double) (arg0 >> 8 & 0xff) / 256.0;
		double d_6_ = (double) (arg0 & 0xff) / 256.0;
		double d_7_ = d;
		if (d_5_ < d_7_)
			d_7_ = d_5_;
		if (d_6_ < d_7_)
			d_7_ = d_6_;
		double d_8_ = d;
		if (d_5_ > d_8_)
			d_8_ = d_5_;
		if (d_6_ > d_8_)
			d_8_ = d_6_;
		double d_9_ = 0.0;
		double d_10_ = 0.0;
		double d_11_ = (d_7_ + d_8_) / 2.0;
		if (d_7_ != d_8_) {
			if (d_11_ < 0.5)
				d_10_ = (d_8_ - d_7_) / (d_8_ + d_7_);
			if (d_11_ >= 0.5)
				d_10_ = (d_8_ - d_7_) / (2.0 - d_8_ - d_7_);
			if (d == d_8_)
				d_9_ = (d_5_ - d_6_) / (d_8_ - d_7_);
			else if (d_5_ == d_8_)
				d_9_ = 2.0 + (d_6_ - d) / (d_8_ - d_7_);
			else if (d_6_ == d_8_)
				d_9_ = 4.0 + (d - d_5_) / (d_8_ - d_7_);
		}
		d_9_ /= 6.0;
		anInt394 = (int) (d_9_ * 256.0);
		anInt395 = (int) (d_10_ * 256.0);
		anInt396 = (int) (d_11_ * 256.0);
		if (anInt395 < 0)
			anInt395 = 0;
		else if (anInt395 > 255)
			anInt395 = 255;
		if (anInt396 < 0)
			anInt396 = 0;
		else if (anInt396 > 255)
			anInt396 = 255;
		if (d_11_ > 0.5)
			anInt398 = (int) ((1.0 - d_11_) * d_10_ * 512.0);
		else
			anInt398 = (int) (d_11_ * d_10_ * 512.0);
		if (anInt398 < 1)
			anInt398 = 1;
		anInt397 = (int) (d_9_ * (double) anInt398);
		int i = anInt394 + (int) (RS2Sharp.Random.Next() * 16.0) - 8;
		if (i < 0)
			i = 0;
		else if (i > 255)
			i = 255;
		int i_12_ = anInt395 + (int) (RS2Sharp.Random.Next() * 48.0) - 24;
		if (i_12_ < 0)
			i_12_ = 0;
		else if (i_12_ > 255)
			i_12_ = 255;
		int i_13_ = anInt396 + (int) (RS2Sharp.Random.Next() * 48.0) - 24;
		if (i_13_ < 0)
			i_13_ = 0;
		else if (i_13_ > 255)
			i_13_ = 255;
		anInt399 = method263(i, i_12_, i_13_);
	}
	
	private int method263(int arg0, int arg1, int arg2) {
		if (arg2 > 179)
			arg1 /= 2;
		if (arg2 > 192)
			arg1 /= 2;
		if (arg2 > 217)
			arg1 /= 2;
		if (arg2 > 243)
			arg1 /= 2;
		int i = (arg0 / 4 << 10) + (arg1 / 32 << 7) + arg2 / 2;
		return i;
	}
}
