using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Globalization;

namespace PSVrammed
{
    //Simple reader for a simple text file format where every line is split into fields.
    class FieldsReader
    {
        private const char SeparatorChar = ',';

        private readonly string[] mFields; //Fields in this text line.
        private int mIndex;

        public FieldsReader(string line)
        {
            mFields = line.Split(SeparatorChar);
            mIndex = 0;
        }

        public string this[int index]
        {
            get { return mFields[index]; }
        }

        public int FieldCount
        {
            get { return mFields.Length; }
        }

        public string readString()
        {
            checkRead(1);
            return mFields[mIndex++];
        }

        public int readInt()
        {
            checkRead(1);
            return toInt(mFields[mIndex++]);
        }

        public float readFloat()
        {
            checkRead(1);
            return toFloat(mFields[mIndex++]);
        }

        public bool readBool()
        {
            checkRead(1);
            return toBool(mFields[mIndex++]);
        }

        public Point readPt()
        {
            checkRead(2);
            Point pt = new Point(
                toInt(mFields[mIndex + 0]),
                toInt(mFields[mIndex + 1]));
            mIndex += 2;
            return pt;
        }

        public Rectangle readRc()
        {
            checkRead(4);
            Rectangle rc = new Rectangle(
                toInt(mFields[mIndex + 0]),
                toInt(mFields[mIndex + 1]),
                toInt(mFields[mIndex + 2]),
                toInt(mFields[mIndex + 3]));
            mIndex += 4;
            return rc;
        }

        public T readEnum<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            checkRead(1);
            return (T)Enum.Parse(typeof(T), mFields[mIndex++]);
        }

        private void checkRead(int count)
        {
            if ((mIndex + count) > mFields.Length)
            {
                throw new Exception(Strings.OpenEditReadErrorMsgMissing);
            }
        }

        private static int toInt(string field)
        {
            return int.Parse(field, CultureInfo.InvariantCulture);
        }

        private static float toFloat(string field)
        {
            return float.Parse(field, CultureInfo.InvariantCulture);
        }

        private static bool toBool(string field)
        {
            return bool.Parse(field);
        }
    }

    //Simple writer for a simple text file format where every line is split into fields.
    class FieldsWriter
    {
        private const char SeparatorChar = ',';
        private const string SeparatorString = ",";

        private readonly StringBuilder mSb;
        private int mIndex;

        public FieldsWriter()
        {
            mSb = new StringBuilder();
            mIndex = 0;
        }

        private void addSeparatorCheckFirst() //Add separator if not first field.
        {
            if (mIndex > 0)
            {
                addSeparator();
            }
        }

        private void addSeparator()
        {
            mSb.Append(SeparatorChar);
        }

        public string toLine()
        {
            return mSb.ToString();
        }

        public void writeString(string s)
        {
            addSeparatorCheckFirst();
            mSb.Append(s);
            mIndex++;
        }

        public void writeInt(int i)
        {
            addSeparatorCheckFirst();
            mSb.Append(toField(i));
            mIndex++;
        }

        public void writeFloat(float f)
        {
            addSeparatorCheckFirst();
            mSb.Append(toField(f));
            mIndex++;
        }

        public void writeBool(bool b)
        {
            addSeparatorCheckFirst();
            mSb.Append(b);
            mIndex++;
        }

        public void writePt(Point p)
        {
            addSeparatorCheckFirst();
            mSb.Append(toField(p));
            mIndex += 2;
        }

        public void writeRc(Rectangle r)
        {
            addSeparatorCheckFirst();
            mSb.Append(toField(r));
            mIndex += 4;
        }

        public void writeEnum<T>(T e) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            addSeparatorCheckFirst();
            mSb.Append(e);
            mIndex++;
        }

        private static string toField(int i)
        {
            return i.ToString(CultureInfo.InvariantCulture);
        }

        private static string toField(float f)
        {
            return f.ToString(CultureInfo.InvariantCulture);
        }

        private static string toField(Point p)
        {
            return toField(p.X) + SeparatorString + toField(p.Y);
        }

        private static string toField(Rectangle r)
        {
            return
                toField(r.X) + SeparatorString +
                toField(r.Y) + SeparatorString +
                toField(r.Width) + SeparatorString +
                toField(r.Height);
        }

        public static string toLine(params object[] fields)
        {
            if (fields.Length == 0) return string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append(toFieldString(fields[0]));
            for (int i = 1; i < fields.Length; i++)
            {
                sb.Append(SeparatorChar);
                sb.Append(toFieldString(fields[i]));
            }

            return sb.ToString();
        }

        private static string toFieldString(object field)
        {
            if (field is int) return toField((int)field);
            if (field is float) return toField((float)field);
            if (field is Point) return toField((Point)field);
            if (field is Rectangle) return toField((Rectangle)field);
            return field.ToString();
        }
    }
}
