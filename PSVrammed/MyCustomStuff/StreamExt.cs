using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace MyCustomStuff
{
    static class StreamExt
    {
        private static byte[] mBufferInt = new byte[8];
        private static char[] mBufferChars = new char[64];

        public static byte readUInt8(this Stream s)
        {
            //s.Read(mBufferInt, 0, 1);
            //return mBufferInt[0];
            return (byte)s.ReadByte();
        }

        public static UInt16 readUInt16(this Stream s)
        {
            s.Read(mBufferInt, 0, 2);
            return (UInt16)(
                (mBufferInt[0] << 0) |
                (mBufferInt[1] << 8));
        }

        public static UInt32 readUInt24(this Stream s) //Read and convert 3 bytes to UInt32.
        {
            s.Read(mBufferInt, 0, 3);
            return (UInt32)(
                (mBufferInt[0] << 0) |
                (mBufferInt[1] << 8) |
                (mBufferInt[2] << 16));
        }

        public static UInt32 readUInt32(this Stream s)
        {
            s.Read(mBufferInt, 0, 4);
            return (UInt32)(
                (mBufferInt[0] << 0) |
                (mBufferInt[1] << 8) |
                (mBufferInt[2] << 16) |
                (mBufferInt[3] << 24));
        }

        public static UInt64 readUInt64(this Stream s)
        {
            s.Read(mBufferInt, 0, 8);
            return (UInt64)(
                (mBufferInt[0] << 0) |
                (mBufferInt[1] << 8) |
                (mBufferInt[2] << 16) |
                (mBufferInt[3] << 24) |
                (mBufferInt[4] << 32) |
                (mBufferInt[5] << 40) |
                (mBufferInt[6] << 48) |
                (mBufferInt[7] << 56));
        }

        public static Int32 readInt32(this Stream s)
        {
            return (Int32)s.readUInt32();
        }

        public static Int32 readInt32BE(this Stream s) //Big endian.
        {
            return (Int32)s.readUInt32BE();
        }

        public static UInt32 readUInt32BE(this Stream s) //Big endian.
        {
            s.Read(mBufferInt, 0, 4);
            return (UInt32)(
                (mBufferInt[0] << 24) |
                (mBufferInt[1] << 16) |
                (mBufferInt[2] << 8) |
                (mBufferInt[3] << 0));
        }

        public static void writeUInt8(this Stream s, byte val)
        {
            s.WriteByte(val);
        }

        public static void writeUInt16(this Stream s, UInt16 val)
        {
            mBufferInt[0] = (byte)(val >> 0);
            mBufferInt[1] = (byte)(val >> 8);
            s.Write(mBufferInt, 0, 2);
        }

        public static void writeUInt24(this Stream s, UInt32 val)
        {
            mBufferInt[0] = (byte)(val >> 0);
            mBufferInt[1] = (byte)(val >> 8);
            mBufferInt[2] = (byte)(val >> 16);
            s.Write(mBufferInt, 0, 3);
        }

        public static void writeUInt32(this Stream s, UInt32 val)
        {
            mBufferInt[0] = (byte)(val >> 0);
            mBufferInt[1] = (byte)(val >> 8);
            mBufferInt[2] = (byte)(val >> 16);
            mBufferInt[3] = (byte)(val >> 24);
            s.Write(mBufferInt, 0, 4);
        }

        public static void writeUInt64(this Stream s, UInt64 val)
        {
            mBufferInt[0] = (byte)(val >> 0);
            mBufferInt[1] = (byte)(val >> 8);
            mBufferInt[2] = (byte)(val >> 16);
            mBufferInt[3] = (byte)(val >> 24);
            mBufferInt[4] = (byte)(val >> 32);
            mBufferInt[5] = (byte)(val >> 40);
            mBufferInt[6] = (byte)(val >> 48);
            mBufferInt[7] = (byte)(val >> 56);
            s.Write(mBufferInt, 0, 8);
        }

        public static void writeInt32(this Stream s, Int32 val)
        {
            s.writeUInt32((UInt32)val);
        }

        public static void writeInt32BE(this Stream s, Int32 val) //Big endian.
        {
            s.writeUInt32BE((UInt32)val);
        }

        public static void writeUInt32BE(this Stream s, UInt32 val) //Big endian.
        {
            mBufferInt[0] = (byte)(val >> 24);
            mBufferInt[1] = (byte)(val >> 16);
            mBufferInt[2] = (byte)(val >> 8);
            mBufferInt[3] = (byte)(val >> 0);
            s.Write(mBufferInt, 0, 4);
        }

        public static long getBytesRemaining(this Stream s)
        {
            return s.Length - s.Position;
        }
        public static byte[] readAllBytes(this Stream s)
        {
            MemoryStream ms = s as MemoryStream;
            if (ms != null) return ms.ToArray(); //Stream is already a MemoryStream.

            using (MemoryStream msOut = new MemoryStream())
            {
                s.CopyTo(msOut);
                return msOut.ToArray();
            }
        }

        public static void writeAllBytes(this Stream s, byte[] array)
        {
            long length = array.LongLength;
            if (length <= int.MaxValue)
            {
                s.Write(array, 0, array.Length);
            }
            else
            {
                for (long i = 0; i < length; i++)
                {
                    s.writeUInt8(array[i]);
                }
            }
        }

        public static byte[] readArray(this Stream s)
        {
            return readArrayInner(s, null, s.Length);
        }

        public static byte[] readArray(this Stream s, long length)
        {
            return readArrayInner(s, null, length);
        }

        public static void readArray(this Stream s, byte[] array)
        {
            readArrayInner(s, array, array.LongLength);
        }

        private static byte[] readArrayInner(this Stream s, byte[] array, long length)
        {
            if ((s.Position + length) > s.Length)
            {
                throw new ArgumentException("Stream is too short to read '" + length + "' bytes from its current position!");
            }

            if (array == null) //Create array if no one was provided.
            {
                array = new byte[length];
            }

            if (length <= int.MaxValue) //Can use Stream.Read?
            {
                s.Read(array, 0, array.Length);
            }
            else
            {
                for (long i = 0; i < length; i++)
                {
                    array[i] = s.readUInt8();
                }
            }
            return array;
        }

        public static string readChars(this Stream s, int count) //Read count chars as a string.
        {
            if (mBufferChars.Length < count) //Check that pre-allocated buffer is big enough.
            {
                mBufferChars = new char[count];
            }
            char[] chars = mBufferChars;
            for (int i = 0; i < count; i++)
            {
                int b = s.ReadByte();
                if (b < 0)
                {
                    throw new ArgumentException(string.Format("Couldn't read '{0}' bytes from stream!", count));
                }
                chars[i] = (char)b;
            }
            return new string(chars, 0, count); //Returns "" if count is 0.
        }

        //A bit slower than the char array version below, but simpler/safer?
        //private static List<char> mListChars = new List<char>();
        //public static string readString(this Stream s) //Read bytes until null or end of stream.
        //{
        //    mListChars.Clear();
        //    for (int b = s.ReadByte(); b > 0; b = s.ReadByte()) //Read until null byte or end of stream.
        //    {
        //        mListChars.Add((char)b);
        //    }
        //    return new string(mListChars.ToArray());
        //}

        public static string readString(this Stream s) //Read bytes until null or end of stream.
        {
            char[] chars = mBufferChars; //Start with pre-allocated buffer.
            int count = 0;
            for (int b = s.ReadByte(); b > 0; b = s.ReadByte()) //Read until null byte or end of stream.
            {
                if (count >= chars.Length) //Need to temporarily expand chars buffer?
                {
                    char[] oldChars = chars;
                    chars = new char[oldChars.Length * 2];
                    Array.Copy(oldChars, chars, oldChars.Length);
                }
                chars[count++] = (char)b;
            }
            return new string(chars, 0, count); //Returns "" if count is 0.
        }

        public static void writeString(this Stream s, string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            s.Write(bytes, 0, bytes.Length);
        }

        public static void writeStringLine(this Stream s, string str)
        {
            s.writeString(str + Environment.NewLine);
        }

        public static void writeStringZ(this Stream s, string str) //C-style zero-terminated string.
        {
            s.writeString(str + '\0');
        }

        public static void readSkip(this Stream s, byte value) //Read bytes until value or end of stream.
        {
            for (int b = s.ReadByte(); b != value && b >= 0; b = s.ReadByte())
            {
            }
        }

        public static void copyToUntil(this Stream src, Stream dst, byte value) //Copy bytes until value or end of src stream.
        {
            for (int b = src.ReadByte(); b != value && b >= 0; b = src.ReadByte())
            {
                dst.WriteByte((byte)b);
            }
        }

        public static long copyTo(this Stream src, Stream dst, long length) //Return total bytes copied. May be less than requested length.
        {
            byte[] buffer = new byte[8192];
            long total = 0;
            int read = (int)Math.Min(buffer.Length, length);
            while (length > 0 && (read = src.Read(buffer, 0, read)) > 0)
            {
                dst.Write(buffer, 0, read);
                total += read;
                length -= read;
                read = (int)Math.Min(buffer.Length, length);
            }
            return total;
        }
    }
}
