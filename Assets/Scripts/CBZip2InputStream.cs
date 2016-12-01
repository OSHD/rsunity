using System;
using System.IO;

public class CBZip2InputStream : Stream 
{
	private static void Cadvise() {
		//System.out.Println("CRC Error");
		//throw new CCoruptionError();
	}
	
	private static void BadBGLengths() {
		Cadvise();
	}
	
	private static void BitStreamEOF() {
		Cadvise();
	}
	
	private static void CompressedStreamEOF() {
		Cadvise();
	}
	
	private void MakeMaps() {
		int i;
		nInUse = 0;
		for (i = 0; i < 256; i++) {
			if (inUse[i]) {
				seqToUnseq[nInUse] = (char) i;
				unseqToSeq[i] = (char) nInUse;
				nInUse++;
			}
		}
	}
	
	/*
        index of the last char in the block, so
        the block size == last + 1.
        */
	private int  last;
	
	/*
        index in zptr[] of original string after sorting.
        */
	private int  origPtr;
	
	/*
        always: in the range 0 .. 9.
        The current block size is 100000 * this number.
        */
	private int blockSize100k;
	
	private bool blockRandomised;
	
	private int bsBuff;
	private int bsLive;
	private CRC mCrc = new CRC();
	
	private bool[] inUse = new bool[256];
	private int nInUse;
	
	private char[] seqToUnseq = new char[256];
	private char[] unseqToSeq = new char[256];
	
	private char[] selector = new char[BZip2Constants.MAX_SELECTORS];
	private char[] selectorMtf = new char[BZip2Constants.MAX_SELECTORS];
	
	private int[] tt;
	private char[] ll8;
	
	/*
        freq table collected to save a pass over the data
        during decompression.
        */
	private int[] unzftab = new int[256];
	
	private int[][] limit = InitIntArray(BZip2Constants.N_GROUPS, BZip2Constants.MAX_ALPHA_SIZE);
	private int[][] basev = InitIntArray(BZip2Constants.N_GROUPS, BZip2Constants.MAX_ALPHA_SIZE);
	private int[][] perm = InitIntArray(BZip2Constants.N_GROUPS, BZip2Constants.MAX_ALPHA_SIZE);
	private int[] minLens = new int[BZip2Constants.N_GROUPS];
	
	private Stream bsStream;
	
	private bool streamEnd = false;
	
	private int currentChar = -1;
	
	private const int START_BLOCK_STATE = 1;
	private const int RAND_PART_A_STATE = 2;
	private const int RAND_PART_B_STATE = 3;
	private const int RAND_PART_C_STATE = 4;
	private const int NO_RAND_PART_A_STATE = 5;
	private const int NO_RAND_PART_B_STATE = 6;
	private const int NO_RAND_PART_C_STATE = 7;
	
	private int currentState = START_BLOCK_STATE;
	
	private int storedBlockCRC, storedCombinedCRC;
	private int computedBlockCRC, computedCombinedCRC;
	
	int i2, count, chPrev, ch2;
	int i, tPos;
	int rNToGo = 0;
	int rTPos  = 0;
	int j2;
	char z;
	
	public CBZip2InputStream(Stream zStream) {
		ll8 = null;
		tt = null;
		BsSetStream(zStream);
		Initialize();
		InitBlock();
		SetupBlock();
	}
	
	internal static int[][] InitIntArray(int n1, int n2) {
		int[][] a = new int[n1][];
		for (int k = 0; k < n1; ++k) {
			a[k] = new int[n2];
		}
		return a;
	}
	
	internal static char[][] InitCharArray(int n1, int n2) {
		char[][] a = new char[n1][];
		for (int k = 0; k < n1; ++k) {
			a[k] = new char[n2];
		}
		return a;
	}
	
	public override int ReadByte() {
		if (streamEnd) {
			return -1;
		} else {
			int retChar = currentChar;
			switch (currentState) {
			case START_BLOCK_STATE:
				break;
			case RAND_PART_A_STATE:
				break;
			case RAND_PART_B_STATE:
				SetupRandPartB();
				break;
			case RAND_PART_C_STATE:
				SetupRandPartC();
				break;
			case NO_RAND_PART_A_STATE:
				break;
			case NO_RAND_PART_B_STATE:
				SetupNoRandPartB();
				break;
			case NO_RAND_PART_C_STATE:
				SetupNoRandPartC();
				break;
			default:
				break;
			}
			return retChar;
		}
	}
	
	private void Initialize() {
		char magic3, magic4;
		magic3 = BsGetUChar();
		magic4 = BsGetUChar();
		if (magic3 != 'B' && magic4 != 'Z')
		{
			throw new IOException("Not a BZIP2 marked stream");
		}
		magic3 = BsGetUChar();
		magic4 = BsGetUChar();
		if (magic3 != 'h' || magic4 < '1' || magic4 > '9') {
			BsFinishedWithStream();
			streamEnd = true;
			return;
		}
		
		SetDecompressStructureSizes(magic4 - '0');
		computedCombinedCRC = 0;
	}
	
	private void InitBlock() {
		char magic1, magic2, magic3, magic4;
		char magic5, magic6;
		magic1 = BsGetUChar();
		magic2 = BsGetUChar();
		magic3 = BsGetUChar();
		magic4 = BsGetUChar();
		magic5 = BsGetUChar();
		magic6 = BsGetUChar();
		if (magic1 == 0x17 && magic2 == 0x72 && magic3 == 0x45
		    && magic4 == 0x38 && magic5 == 0x50 && magic6 == 0x90) {
			Complete();
			return;
		}
		
		if (magic1 != 0x31 || magic2 != 0x41 || magic3 != 0x59
		    || magic4 != 0x26 || magic5 != 0x53 || magic6 != 0x59) {
			BadBlockHeader();
			streamEnd = true;
			return;
		}
		
		storedBlockCRC = BsGetInt32();
		
		if (BsR(1) == 1) {
			blockRandomised = true;
		} else {
			blockRandomised = false;
		}
		
		//        currBlockNo++;
		GetAndMoveToFrontDecode();
		
		mCrc.InitialiseCRC();
		currentState = START_BLOCK_STATE;
	}
	
	private void EndBlock() {
		computedBlockCRC = mCrc.GetFinalCRC();
		/* A bad CRC is considered a fatal error. */
		if (storedBlockCRC != computedBlockCRC) {
			CrcError();
		}
		
		computedCombinedCRC = (computedCombinedCRC << 1)
			| (int)(((uint)computedCombinedCRC) >> 31);
		computedCombinedCRC ^= computedBlockCRC;
	}
	
	private void Complete() {
		storedCombinedCRC = BsGetInt32();
		if (storedCombinedCRC != computedCombinedCRC) {
			CrcError();
		}
		
		BsFinishedWithStream();
		streamEnd = true;
	}
	
	private static void BlockOverrun() {
		Cadvise();
	}
	
	private static void BadBlockHeader() {
		Cadvise();
	}
	
	private static void CrcError() {
		Cadvise();
	}
	
	private void BsFinishedWithStream() {
		try {
			if (this.bsStream != null) {
				this.bsStream.Close();
				this.bsStream = null;
			}
		} catch {
			//ignore
		}
	}
	
	private void BsSetStream(Stream f) {
		bsStream = f;
		bsLive = 0;
		bsBuff = 0;
	}
	
	private int BsR(int n) {
		int v;
		while (bsLive < n) {
			int zzi;
			char thech = '\0';
			try {
				thech = (char) bsStream.ReadByte();
			} catch (IOException) {
				CompressedStreamEOF();
			}
			if (thech == '\uffff') {
				CompressedStreamEOF();
			}
			zzi = thech;
			bsBuff = (bsBuff << 8) | (zzi & 0xff);
			bsLive += 8;
		}
		
		v = (bsBuff >> (bsLive - n)) & ((1 << n) - 1);
		bsLive -= n;
		return v;
	}
	
	private char BsGetUChar() {
		return (char) BsR(8);
	}
	
	private int BsGetint() {
		int u = 0;
		u = (u << 8) | BsR(8);
		u = (u << 8) | BsR(8);
		u = (u << 8) | BsR(8);
		u = (u << 8) | BsR(8);
		return u;
	}
	
	private int BsGetIntVS(int numBits) {
		return (int) BsR(numBits);
	}
	
	private int BsGetInt32() {
		return (int) BsGetint();
	}
	
	private void HbCreateDecodeTables(int[] limit, int[] basev,
	                                  int[] perm, char[] length,
	                                  int minLen, int maxLen, int alphaSize) {
		int pp, i, j, vec;
		
		pp = 0;
		for (i = minLen; i <= maxLen; i++) {
			for (j = 0; j < alphaSize; j++) {
				if (length[j] == i) {
					perm[pp] = j;
					pp++;
				}
			}
		}
		
		for (i = 0; i < BZip2Constants.MAX_CODE_LEN; i++) {
			basev[i] = 0;
		}
		for (i = 0; i < alphaSize; i++) {
			basev[length[i] + 1]++;
		}
		
		for (i = 1; i < BZip2Constants.MAX_CODE_LEN; i++) {
			basev[i] += basev[i - 1];
		}
		
		for (i = 0; i < BZip2Constants.MAX_CODE_LEN; i++) {
			limit[i] = 0;
		}
		vec = 0;
		
		for (i = minLen; i <= maxLen; i++) {
			vec += (basev[i + 1] - basev[i]);
			limit[i] = vec - 1;
			vec <<= 1;
		}
		for (i = minLen + 1; i <= maxLen; i++) {
			basev[i] = ((limit[i - 1] + 1) << 1) - basev[i];
		}
	}
	
	private void RecvDecodingTables() {
		char[][] len = InitCharArray(BZip2Constants.N_GROUPS, BZip2Constants.MAX_ALPHA_SIZE);
		int i, j, t, nGroups, nSelectors, alphaSize;
		int minLen, maxLen;
		bool[] inUse16 = new bool[16];
		
		/* Receive the mapping table */
		for (i = 0; i < 16; i++) {
			if (BsR(1) == 1) {
				inUse16[i] = true;
			} else {
				inUse16[i] = false;
			}
		}
		
		for (i = 0; i < 256; i++) {
			inUse[i] = false;
		}
		
		for (i = 0; i < 16; i++) {
			if (inUse16[i]) {
				for (j = 0; j < 16; j++) {
					if (BsR(1) == 1) {
						inUse[i * 16 + j] = true;
					}
				}
			}
		}
		
		MakeMaps();
		alphaSize = nInUse + 2;
		
		/* Now the selectors */
		nGroups = BsR(3);
		nSelectors = BsR(15);
		for (i = 0; i < nSelectors; i++) {
			j = 0;
			while (BsR(1) == 1) {
				j++;
			}
			selectorMtf[i] = (char) j;
		}
		
		/* Undo the MTF values for the selectors. */
		{
			char[] pos = new char[BZip2Constants.N_GROUPS];
			char tmp, v;
			for (v = '\0'; v < nGroups; v++) {
				pos[v] = v;
			}
			
			for (i = 0; i < nSelectors; i++) {
				v = selectorMtf[i];
				tmp = pos[v];
				while (v > 0) {
					pos[v] = pos[v - 1];
					v--;
				}
				pos[0] = tmp;
				selector[i] = tmp;
			}
		}
		
		/* Now the coding tables */
		for (t = 0; t < nGroups; t++) {
			int curr = BsR(5);
			for (i = 0; i < alphaSize; i++) {
				while (BsR(1) == 1) {
					if (BsR(1) == 0) {
						curr++;
					} else {
						curr--;
					}
				}
				len[t][i] = (char) curr;
			}
		}
		
		/* Create the Huffman decoding tables */
		for (t = 0; t < nGroups; t++) {
			minLen = 32;
			maxLen = 0;
			for (i = 0; i < alphaSize; i++) {
				if (len[t][i] > maxLen) {
					maxLen = len[t][i];
				}
				if (len[t][i] < minLen) {
					minLen = len[t][i];
				}
			}
			HbCreateDecodeTables(limit[t], basev[t], perm[t], len[t], minLen,
			                     maxLen, alphaSize);
			minLens[t] = minLen;
		}
	}
	
	private void GetAndMoveToFrontDecode() {
		char[] yy = new char[256];
		int i, j, nextSym, limitLast;
		int EOB, groupNo, groupPos;
		
		limitLast = BZip2Constants.baseBlockSize * blockSize100k;
		origPtr = BsGetIntVS(24);
		
		RecvDecodingTables();
		EOB = nInUse + 1;
		groupNo = -1;
		groupPos = 0;
		
		/*
            Setting up the unzftab entries here is not strictly
            necessary, but it does save having to do it later
            in a separate pass, and so saves a block's worth of
            cache misses.
            */
		for (i = 0; i <= 255; i++) {
			unzftab[i] = 0;
		}
		
		for (i = 0; i <= 255; i++) {
			yy[i] = (char) i;
		}
		
		last = -1;
		
		{
			int zt, zn, zvec, zj;
			if (groupPos == 0) {
				groupNo++;
				groupPos = BZip2Constants.G_SIZE;
			}
			groupPos--;
			zt = selector[groupNo];
			zn = minLens[zt];
			zvec = BsR(zn);
			while (zvec > limit[zt][zn]) {
				zn++;
				{
					{
						while (bsLive < 1) {
							int zzi;
							char thech = '\0';
							try {
								thech = (char) bsStream.ReadByte();
							} catch (IOException) {
								CompressedStreamEOF();
							}
							if (thech == '\uffff') {
								CompressedStreamEOF();
							}
							zzi = thech;
							bsBuff = (bsBuff << 8) | (zzi & 0xff);
							bsLive += 8;
						}
					}
					zj = (bsBuff >> (bsLive - 1)) & 1;
					bsLive--;
				}
				zvec = (zvec << 1) | zj;
			}
			nextSym = perm[zt][zvec - basev[zt][zn]];
		}
		
		while (true) {
			
			if (nextSym == EOB) {
				break;
			}
			
			if (nextSym == BZip2Constants.RUNA || nextSym == BZip2Constants.RUNB) {
				char ch;
				int s = -1;
				int N = 1;
				do {
					if (nextSym == BZip2Constants.RUNA) {
						s = s + (0 + 1) * N;
					} else if (nextSym == BZip2Constants.RUNB) {
						s = s + (1 + 1) * N;
					}
					N = N * 2;
					{
						int zt, zn, zvec, zj;
						if (groupPos == 0) {
							groupNo++;
							groupPos = BZip2Constants.G_SIZE;
						}
						groupPos--;
						zt = selector[groupNo];
						zn = minLens[zt];
						zvec = BsR(zn);
						while (zvec > limit[zt][zn]) {
							zn++;
							{
								{
									while (bsLive < 1) {
										int zzi;
										char thech = '\0';
										try {
											thech = (char) bsStream.ReadByte();
										} catch (IOException) {
											CompressedStreamEOF();
										}
										if (thech == '\uffff') {
											CompressedStreamEOF();
										}
										zzi = thech;
										bsBuff = (bsBuff << 8) | (zzi & 0xff);
										bsLive += 8;
									}
								}
								zj = (bsBuff >> (bsLive - 1)) & 1;
								bsLive--;
							}
							zvec = (zvec << 1) | zj;
						}
						nextSym = perm[zt][zvec - basev[zt][zn]];
					}
				} while (nextSym == BZip2Constants.RUNA || nextSym == BZip2Constants.RUNB);
				
				s++;
				ch = seqToUnseq[yy[0]];
				unzftab[ch] += s;
				
				while (s > 0) {
					last++;
					ll8[last] = ch;
					s--;
				}
				
				if (last >= limitLast) {
					BlockOverrun();
				}
				continue;
			} else {
				char tmp;
				last++;
				if (last >= limitLast) {
					BlockOverrun();
				}
				
				tmp = yy[nextSym - 1];
				unzftab[seqToUnseq[tmp]]++;
				ll8[last] = seqToUnseq[tmp];
				
				/*
                    This loop is hammered during decompression,
                    hence the unrolling.

                    for (j = nextSym-1; j > 0; j--) yy[j] = yy[j-1];
                    */
				
				j = nextSym - 1;
				for (; j > 3; j -= 4) {
					yy[j]     = yy[j - 1];
					yy[j - 1] = yy[j - 2];
					yy[j - 2] = yy[j - 3];
					yy[j - 3] = yy[j - 4];
				}
				for (; j > 0; j--) {
					yy[j] = yy[j - 1];
				}
				
				yy[0] = tmp;
				{
					int zt, zn, zvec, zj;
					if (groupPos == 0) {
						groupNo++;
						groupPos = BZip2Constants.G_SIZE;
					}
					groupPos--;
					zt = selector[groupNo];
					zn = minLens[zt];
					zvec = BsR(zn);
					while (zvec > limit[zt][zn]) {
						zn++;
						{
							{
								while (bsLive < 1) {
									int zzi;
									char thech = '\0';
									try {
										thech = (char) bsStream.ReadByte();
									} catch (IOException) {
										CompressedStreamEOF();
									}
									zzi = thech;
									bsBuff = (bsBuff << 8) | (zzi & 0xff);
									bsLive += 8;
								}
							}
							zj = (bsBuff >> (bsLive - 1)) & 1;
							bsLive--;
						}
						zvec = (zvec << 1) | zj;
					}
					nextSym = perm[zt][zvec - basev[zt][zn]];
				}
				continue;
			}
		}
	}
	
	private void SetupBlock() {
		int[] cftab = new int[257];
		char ch;
		
		cftab[0] = 0;
		for (i = 1; i <= 256; i++) {
			cftab[i] = unzftab[i - 1];
		}
		for (i = 1; i <= 256; i++) {
			cftab[i] += cftab[i - 1];
		}
		
		for (i = 0; i <= last; i++) {
			ch = (char) ll8[i];
			tt[cftab[ch]] = i;
			cftab[ch]++;
		}
		cftab = null;
		
		tPos = tt[origPtr];
		
		count = 0;
		i2 = 0;
		ch2 = 256;   /* not a char and not EOF */
		
		if (blockRandomised) {
			rNToGo = 0;
			rTPos = 0;
			SetupRandPartA();
		} else {
			SetupNoRandPartA();
		}
	}
	
	private void SetupRandPartA() {
		if (i2 <= last) {
			chPrev = ch2;
			ch2 = ll8[tPos];
			tPos = tt[tPos];
			if (rNToGo == 0) {
				rNToGo = BZip2Constants.rNums[rTPos];
				rTPos++;
				if (rTPos == 512) {
					rTPos = 0;
				}
			}
			rNToGo--;
			ch2 ^= (int) ((rNToGo == 1) ? 1 : 0);
			i2++;
			
			currentChar = ch2;
			currentState = RAND_PART_B_STATE;
			mCrc.UpdateCRC(ch2);
		} else {
			EndBlock();
			InitBlock();
			SetupBlock();
		}
	}
	
	private void SetupNoRandPartA() {
		if (i2 <= last) {
			chPrev = ch2;
			ch2 = ll8[tPos];
			tPos = tt[tPos];
			i2++;
			
			currentChar = ch2;
			currentState = NO_RAND_PART_B_STATE;
			mCrc.UpdateCRC(ch2);
		} else {
			EndBlock();
			InitBlock();
			SetupBlock();
		}
	}
	
	private void SetupRandPartB() {
		if (ch2 != chPrev) {
			currentState = RAND_PART_A_STATE;
			count = 1;
			SetupRandPartA();
		} else {
			count++;
			if (count >= 4) {
				z = ll8[tPos];
				tPos = tt[tPos];
				if (rNToGo == 0) {
					rNToGo = BZip2Constants.rNums[rTPos];
					rTPos++;
					if (rTPos == 512) {
						rTPos = 0;
					}
				}
				rNToGo--;
				z ^= (char)((rNToGo == 1) ? 1 : 0);
				j2 = 0;
				currentState = RAND_PART_C_STATE;
				SetupRandPartC();
			} else {
				currentState = RAND_PART_A_STATE;
				SetupRandPartA();
			}
		}
	}
	
	private void SetupRandPartC() {
		if (j2 < (int) z) {
			currentChar = ch2;
			mCrc.UpdateCRC(ch2);
			j2++;
		} else {
			currentState = RAND_PART_A_STATE;
			i2++;
			count = 0;
			SetupRandPartA();
		}
	}
	
	private void SetupNoRandPartB() {
		if (ch2 != chPrev) {
			currentState = NO_RAND_PART_A_STATE;
			count = 1;
			SetupNoRandPartA();
		} else {
			count++;
			if (count >= 4) {
				z = ll8[tPos];
				tPos = tt[tPos];
				currentState = NO_RAND_PART_C_STATE;
				j2 = 0;
				SetupNoRandPartC();
			} else {
				currentState = NO_RAND_PART_A_STATE;
				SetupNoRandPartA();
			}
		}
	}
	
	private void SetupNoRandPartC() {
		if (j2 < (int) z) {
			currentChar = ch2;
			mCrc.UpdateCRC(ch2);
			j2++;
		} else {
			currentState = NO_RAND_PART_A_STATE;
			i2++;
			count = 0;
			SetupNoRandPartA();
		}
	}
	
	private void SetDecompressStructureSizes(int newSize100k) {
		if (!(0 <= newSize100k && newSize100k <= 9 && 0 <= blockSize100k
		      && blockSize100k <= 9)) {
			// throw new IOException("Invalid block size");
		}
		
		blockSize100k = newSize100k;
		
		if (newSize100k == 0) {
			return;
		}
		
		int n = BZip2Constants.baseBlockSize * newSize100k;
		ll8 = new char[n];
		tt = new int[n];
	}
	
	public override void Flush() {
	}
	
	public override int Read(byte[] buffer, int offset, int count) {
		int c = -1;
		int k;
		for (k = 0; k < count; ++k) {
			c = ReadByte();
			if (c == -1)
				break;
			buffer[k + offset] = (byte)c;
		}
		return k;
	}
	
	public override long Seek(long offset, SeekOrigin origin) {
		return 0;
	}
	
	public override void SetLength(long value) {
	}
	
	public override void Write(byte[] buffer, int offset, int count) {
	}
	
	public override bool CanRead {
		get {
			return true;
		}
	}
	
	public override bool CanSeek {
		get {
			return false;
		}
	}
	
	public override bool CanWrite {
		get {
			return false;
		}
	}
	
	public override long Length {
		get {
			return 0;
		}
	}
	
	public override long Position {
		get {
			return 0;
		}
		set {
		}
	}
}

/**
    * A simple class the hold and calculate the CRC for sanity checking
    * of the data.
    *
    * @author <a href="mailto:keiron@aftexsw.com">Keiron Liddle</a>
    */
internal class CRC 
{
	public static int[] crc32Table = {
		unchecked((int)0x00000000), unchecked((int)0x04c11db7), unchecked((int)0x09823b6e), unchecked((int)0x0d4326d9),
		unchecked((int)0x130476dc), unchecked((int)0x17c56b6b), unchecked((int)0x1a864db2), unchecked((int)0x1e475005),
		unchecked((int)0x2608edb8), unchecked((int)0x22c9f00f), unchecked((int)0x2f8ad6d6), unchecked((int)0x2b4bcb61),
		unchecked((int)0x350c9b64), unchecked((int)0x31cd86d3), unchecked((int)0x3c8ea00a), unchecked((int)0x384fbdbd),
		unchecked((int)0x4c11db70), unchecked((int)0x48d0c6c7), unchecked((int)0x4593e01e), unchecked((int)0x4152fda9),
		unchecked((int)0x5f15adac), unchecked((int)0x5bd4b01b), unchecked((int)0x569796c2), unchecked((int)0x52568b75),
		unchecked((int)0x6a1936c8), unchecked((int)0x6ed82b7f), unchecked((int)0x639b0da6), unchecked((int)0x675a1011),
		unchecked((int)0x791d4014), unchecked((int)0x7ddc5da3), unchecked((int)0x709f7b7a), unchecked((int)0x745e66cd),
		unchecked((int)0x9823b6e0), unchecked((int)0x9ce2ab57), unchecked((int)0x91a18d8e), unchecked((int)0x95609039),
		unchecked((int)0x8b27c03c), unchecked((int)0x8fe6dd8b), unchecked((int)0x82a5fb52), unchecked((int)0x8664e6e5),
		unchecked((int)0xbe2b5b58), unchecked((int)0xbaea46ef), unchecked((int)0xb7a96036), unchecked((int)0xb3687d81),
		unchecked((int)0xad2f2d84), unchecked((int)0xa9ee3033), unchecked((int)0xa4ad16ea), unchecked((int)0xa06c0b5d),
		unchecked((int)0xd4326d90), unchecked((int)0xd0f37027), unchecked((int)0xddb056fe), unchecked((int)0xd9714b49),
		unchecked((int)0xc7361b4c), unchecked((int)0xc3f706fb), unchecked((int)0xceb42022), unchecked((int)0xca753d95),
		unchecked((int)0xf23a8028), unchecked((int)0xf6fb9d9f), unchecked((int)0xfbb8bb46), unchecked((int)0xff79a6f1),
		unchecked((int)0xe13ef6f4), unchecked((int)0xe5ffeb43), unchecked((int)0xe8bccd9a), unchecked((int)0xec7dd02d),
		unchecked((int)0x34867077), unchecked((int)0x30476dc0), unchecked((int)0x3d044b19), unchecked((int)0x39c556ae),
		unchecked((int)0x278206ab), unchecked((int)0x23431b1c), unchecked((int)0x2e003dc5), unchecked((int)0x2ac12072),
		unchecked((int)0x128e9dcf), unchecked((int)0x164f8078), unchecked((int)0x1b0ca6a1), unchecked((int)0x1fcdbb16),
		unchecked((int)0x018aeb13), unchecked((int)0x054bf6a4), unchecked((int)0x0808d07d), unchecked((int)0x0cc9cdca),
		unchecked((int)0x7897ab07), unchecked((int)0x7c56b6b0), unchecked((int)0x71159069), unchecked((int)0x75d48dde),
		unchecked((int)0x6b93dddb), unchecked((int)0x6f52c06c), unchecked((int)0x6211e6b5), unchecked((int)0x66d0fb02),
		unchecked((int)0x5e9f46bf), unchecked((int)0x5a5e5b08), unchecked((int)0x571d7dd1), unchecked((int)0x53dc6066),
		unchecked((int)0x4d9b3063), unchecked((int)0x495a2dd4), unchecked((int)0x44190b0d), unchecked((int)0x40d816ba),
		unchecked((int)0xaca5c697), unchecked((int)0xa864db20), unchecked((int)0xa527fdf9), unchecked((int)0xa1e6e04e),
		unchecked((int)0xbfa1b04b), unchecked((int)0xbb60adfc), unchecked((int)0xb6238b25), unchecked((int)0xb2e29692),
		unchecked((int)0x8aad2b2f), unchecked((int)0x8e6c3698), unchecked((int)0x832f1041), unchecked((int)0x87ee0df6),
		unchecked((int)0x99a95df3), unchecked((int)0x9d684044), unchecked((int)0x902b669d), unchecked((int)0x94ea7b2a),
		unchecked((int)0xe0b41de7), unchecked((int)0xe4750050), unchecked((int)0xe9362689), unchecked((int)0xedf73b3e),
		unchecked((int)0xf3b06b3b), unchecked((int)0xf771768c), unchecked((int)0xfa325055), unchecked((int)0xfef34de2),
		unchecked((int)0xc6bcf05f), unchecked((int)0xc27dede8), unchecked((int)0xcf3ecb31), unchecked((int)0xcbffd686),
		unchecked((int)0xd5b88683), unchecked((int)0xd1799b34), unchecked((int)0xdc3abded), unchecked((int)0xd8fba05a),
		unchecked((int)0x690ce0ee), unchecked((int)0x6dcdfd59), unchecked((int)0x608edb80), unchecked((int)0x644fc637),
		unchecked((int)0x7a089632), unchecked((int)0x7ec98b85), unchecked((int)0x738aad5c), unchecked((int)0x774bb0eb),
		unchecked((int)0x4f040d56), unchecked((int)0x4bc510e1), unchecked((int)0x46863638), unchecked((int)0x42472b8f),
		unchecked((int)0x5c007b8a), unchecked((int)0x58c1663d), unchecked((int)0x558240e4), unchecked((int)0x51435d53),
		unchecked((int)0x251d3b9e), unchecked((int)0x21dc2629), unchecked((int)0x2c9f00f0), unchecked((int)0x285e1d47),
		unchecked((int)0x36194d42), unchecked((int)0x32d850f5), unchecked((int)0x3f9b762c), unchecked((int)0x3b5a6b9b),
		unchecked((int)0x0315d626), unchecked((int)0x07d4cb91), unchecked((int)0x0a97ed48), unchecked((int)0x0e56f0ff),
		unchecked((int)0x1011a0fa), unchecked((int)0x14d0bd4d), unchecked((int)0x19939b94), unchecked((int)0x1d528623),
		unchecked((int)0xf12f560e), unchecked((int)0xf5ee4bb9), unchecked((int)0xf8ad6d60), unchecked((int)0xfc6c70d7),
		unchecked((int)0xe22b20d2), unchecked((int)0xe6ea3d65), unchecked((int)0xeba91bbc), unchecked((int)0xef68060b),
		unchecked((int)0xd727bbb6), unchecked((int)0xd3e6a601), unchecked((int)0xdea580d8), unchecked((int)0xda649d6f),
		unchecked((int)0xc423cd6a), unchecked((int)0xc0e2d0dd), unchecked((int)0xcda1f604), unchecked((int)0xc960ebb3),
		unchecked((int)0xbd3e8d7e), unchecked((int)0xb9ff90c9), unchecked((int)0xb4bcb610), unchecked((int)0xb07daba7),
		unchecked((int)0xae3afba2), unchecked((int)0xaafbe615), unchecked((int)0xa7b8c0cc), unchecked((int)0xa379dd7b),
		unchecked((int)0x9b3660c6), unchecked((int)0x9ff77d71), unchecked((int)0x92b45ba8), unchecked((int)0x9675461f),
		unchecked((int)0x8832161a), unchecked((int)0x8cf30bad), unchecked((int)0x81b02d74), unchecked((int)0x857130c3),
		unchecked((int)0x5d8a9099), unchecked((int)0x594b8d2e), unchecked((int)0x5408abf7), unchecked((int)0x50c9b640),
		unchecked((int)0x4e8ee645), unchecked((int)0x4a4ffbf2), unchecked((int)0x470cdd2b), unchecked((int)0x43cdc09c),
		unchecked((int)0x7b827d21), unchecked((int)0x7f436096), unchecked((int)0x7200464f), unchecked((int)0x76c15bf8),
		unchecked((int)0x68860bfd), unchecked((int)0x6c47164a), unchecked((int)0x61043093), unchecked((int)0x65c52d24),
		unchecked((int)0x119b4be9), unchecked((int)0x155a565e), unchecked((int)0x18197087), unchecked((int)0x1cd86d30),
		unchecked((int)0x029f3d35), unchecked((int)0x065e2082), unchecked((int)0x0b1d065b), unchecked((int)0x0fdc1bec),
		unchecked((int)0x3793a651), unchecked((int)0x3352bbe6), unchecked((int)0x3e119d3f), unchecked((int)0x3ad08088),
		unchecked((int)0x2497d08d), unchecked((int)0x2056cd3a), unchecked((int)0x2d15ebe3), unchecked((int)0x29d4f654),
		unchecked((int)0xc5a92679), unchecked((int)0xc1683bce), unchecked((int)0xcc2b1d17), unchecked((int)0xc8ea00a0),
		unchecked((int)0xd6ad50a5), unchecked((int)0xd26c4d12), unchecked((int)0xdf2f6bcb), unchecked((int)0xdbee767c),
		unchecked((int)0xe3a1cbc1), unchecked((int)0xe760d676), unchecked((int)0xea23f0af), unchecked((int)0xeee2ed18),
		unchecked((int)0xf0a5bd1d), unchecked((int)0xf464a0aa), unchecked((int)0xf9278673), unchecked((int)0xfde69bc4),
		unchecked((int)0x89b8fd09), unchecked((int)0x8d79e0be), unchecked((int)0x803ac667), unchecked((int)0x84fbdbd0),
		unchecked((int)0x9abc8bd5), unchecked((int)0x9e7d9662), unchecked((int)0x933eb0bb), unchecked((int)0x97ffad0c),
		unchecked((int)0xafb010b1), unchecked((int)0xab710d06), unchecked((int)0xa6322bdf), unchecked((int)0xa2f33668),
		unchecked((int)0xbcb4666d), unchecked((int)0xb8757bda), unchecked((int)0xb5365d03), unchecked((int)0xb1f740b4)
	};
	
	public CRC() {
		InitialiseCRC();
	}
	
	internal void InitialiseCRC() {
		globalCrc = unchecked((int)0xffffffff);
	}
	
	internal int GetFinalCRC() {
		return ~globalCrc;
	}
	
	internal int GetGlobalCRC() {
		return globalCrc;
	}
	
	internal void SetGlobalCRC(int newCrc) {
		globalCrc = newCrc;
	}
	
	internal void UpdateCRC(int inCh) {
		int temp = (globalCrc >> 24) ^ inCh;
		if (temp < 0) {
			temp = 256 + temp;
		}
		globalCrc = (globalCrc << 8) ^ CRC.crc32Table[temp];
	}
	
	internal int globalCrc;
}

public class BZip2Constants {
	
	public const int baseBlockSize = 100000;
	public const int MAX_ALPHA_SIZE = 258;
	public const int MAX_CODE_LEN = 23;
	public const int RUNA = 0;
	public const int RUNB = 1;
	public const int N_GROUPS = 6;
	public const int G_SIZE = 50;
	public const int N_ITERS = 4;
	public const int MAX_SELECTORS = (2 + (900000 / G_SIZE));
	public const int NUM_OVERSHOOT_BYTES = 20;
	
	public static int[] rNums = {
		619, 720, 127, 481, 931, 816, 813, 233, 566, 247,
		985, 724, 205, 454, 863, 491, 741, 242, 949, 214,
		733, 859, 335, 708, 621, 574, 73, 654, 730, 472,
		419, 436, 278, 496, 867, 210, 399, 680, 480, 51,
		878, 465, 811, 169, 869, 675, 611, 697, 867, 561,
		862, 687, 507, 283, 482, 129, 807, 591, 733, 623,
		150, 238, 59, 379, 684, 877, 625, 169, 643, 105,
		170, 607, 520, 932, 727, 476, 693, 425, 174, 647,
		73, 122, 335, 530, 442, 853, 695, 249, 445, 515,
		909, 545, 703, 919, 874, 474, 882, 500, 594, 612,
		641, 801, 220, 162, 819, 984, 589, 513, 495, 799,
		161, 604, 958, 533, 221, 400, 386, 867, 600, 782,
		382, 596, 414, 171, 516, 375, 682, 485, 911, 276,
		98, 553, 163, 354, 666, 933, 424, 341, 533, 870,
		227, 730, 475, 186, 263, 647, 537, 686, 600, 224,
		469, 68, 770, 919, 190, 373, 294, 822, 808, 206,
		184, 943, 795, 384, 383, 461, 404, 758, 839, 887,
		715, 67, 618, 276, 204, 918, 873, 777, 604, 560,
		951, 160, 578, 722, 79, 804, 96, 409, 713, 940,
		652, 934, 970, 447, 318, 353, 859, 672, 112, 785,
		645, 863, 803, 350, 139, 93, 354, 99, 820, 908,
		609, 772, 154, 274, 580, 184, 79, 626, 630, 742,
		653, 282, 762, 623, 680, 81, 927, 626, 789, 125,
		411, 521, 938, 300, 821, 78, 343, 175, 128, 250,
		170, 774, 972, 275, 999, 639, 495, 78, 352, 126,
		857, 956, 358, 619, 580, 124, 737, 594, 701, 612,
		669, 112, 134, 694, 363, 992, 809, 743, 168, 974,
		944, 375, 748, 52, 600, 747, 642, 182, 862, 81,
		344, 805, 988, 739, 511, 655, 814, 334, 249, 515,
		897, 955, 664, 981, 649, 113, 974, 459, 893, 228,
		433, 837, 553, 268, 926, 240, 102, 654, 459, 51,
		686, 754, 806, 760, 493, 403, 415, 394, 687, 700,
		946, 670, 656, 610, 738, 392, 760, 799, 887, 653,
		978, 321, 576, 617, 626, 502, 894, 679, 243, 440,
		680, 879, 194, 572, 640, 724, 926, 56, 204, 700,
		707, 151, 457, 449, 797, 195, 791, 558, 945, 679,
		297, 59, 87, 824, 713, 663, 412, 693, 342, 606,
		134, 108, 571, 364, 631, 212, 174, 643, 304, 329,
		343, 97, 430, 751, 497, 314, 983, 374, 822, 928,
		140, 206, 73, 263, 980, 736, 876, 478, 430, 305,
		170, 514, 364, 692, 829, 82, 855, 953, 676, 246,
		369, 970, 294, 750, 807, 827, 150, 790, 288, 923,
		804, 378, 215, 828, 592, 281, 565, 555, 710, 82,
		896, 831, 547, 261, 524, 462, 293, 465, 502, 56,
		661, 821, 976, 991, 658, 869, 905, 758, 745, 193,
		768, 550, 608, 933, 378, 286, 215, 979, 792, 961,
		61, 688, 793, 644, 986, 403, 106, 366, 905, 644,
		372, 567, 466, 434, 645, 210, 389, 550, 919, 135,
		780, 773, 635, 389, 707, 100, 626, 958, 165, 504,
		920, 176, 193, 713, 857, 265, 203, 50, 668, 108,
		645, 990, 626, 197, 510, 357, 358, 850, 858, 364,
		936, 638
	};
}