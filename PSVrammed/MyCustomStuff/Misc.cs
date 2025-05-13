using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyCustomStuff
{
    //Miscellaneous functions and extension methods.
    public static class Misc
    {
        public static void Swap<T>(ref T v1, ref T v2)
        {
            T tmp = v1;
            v1 = v2;
            v2 = tmp;
        }

        public static bool TryParseInt(this string s, out int result, bool isHex)
        {
            return isHex
                ? int.TryParse(s, System.Globalization.NumberStyles.AllowHexSpecifier,
                    System.Globalization.CultureInfo.InvariantCulture, out result)
                : int.TryParse(s, out result);
        }

        public static int RoundUp(this float f)
        {
            //Convert float to int, rounding up.
            return (int)(f + 0.5f);
        }

        public static int RoundNear(this float f)
        {
            //Convert float to int, rounding nearest.
            return (int)Math.Round(f);
        }

        public static int EvenDown(int i)
        {
            //Return nearest even integer <= i.
            return i & ~1;
        }

        public static int EvenUp(int i)
        {
            //Return nearest even integer >= i.
            return EvenDown(i + 1);
        }

        public static int DivideDown(int dividend, int divisor)
        {
            //Integer division, round result down.
            return dividend / divisor;
        }

        public static int DivideUp(int dividend, int divisor)
        {
            //Integer division, round result up.
            //return (int)Math.Ceiling((double)dividend / divisor); //Slower.
            return (dividend / divisor) + (dividend % divisor == 0 ? 0 : 1); //Faster. Tested correct!
        }

        public static int Floor(this float f)
        {
            //Convert float to int <= f.
            //return (int)Math.Floor(f); //A bit slower.
            //return (int)(f + 1073741824.0f) - 1073741824; //Fast, but incorrect in some cases?
            int i = (int)f;
            return i > f ? i - 1 : i;
        }

        public static int Floor(this double d)
        {
            //Convert double to int <= d.
            //return (int)Math.Floor(d); //A bit slower.
            int i = (int)d;
            return i > d ? i - 1 : i;
        }

        public static int Ceiling(this float f)
        {
            //Convert float to int >= f.
            //return (int)Math.Ceiling(f); //A bit slower.
            //return 1073741824 - (int)(1073741824.0f - f); //Fast, but incorrect in some cases?
            int i = (int)f;
            return i < f ? i + 1 : i;
        }

        public static int Ceiling(this double d)
        {
            //Convert double to int >= d.
            //return (int)Math.Ceiling(d); //A bit slower.
            int i = (int)d;
            return i < d ? i + 1 : i;
        }

        public static int Clamp(this int i, int min, int max)
        {
            //Adjust i so it is >= min and <= max), assume min < max.
            return Math.Max(min, Math.Min(i, max));
            //return i < min ? min : (i > max ? max : i); //Equally fast.
        }

        public static float Clamp(this float f, float min, float max)
        {
            //Adjust f so it is >= min and <= max), assume min < max.
            return Math.Max(min, Math.Min(f, max));
            //return f < min ? min : (f > max ? max : f); //Equally fast.
        }

        public static double Clamp(this double d, double min, double max)
        {
            //Adjust d so it is >= min and <= max), assume min < max.
            return Math.Max(min, Math.Min(d, max));
            //return d < min ? min : (d > max ? max : d); //Equally fast.
        }

        public static int Snap(this int value, int multiple)
        {
            //Round value to nearest multiple, e.g. multiple == 8 and value == 30 -> 32.
            //Multiple needs to be negative if value is for this to work.
            //if (value < 0) multiple = -multiple;
            //return ((value + (multiple / 2)) / multiple) * multiple;

            //Old code not correct for negative values. This is better:
            if (value >= 0) value += multiple / 2; //Seems a bit faster than ternary op??? Easy branch prediction???
            else value -= multiple / 2;
            return (value / multiple) * multiple; //Or line below...
            //return value - (value % multiple); //...also works, but not faster???
        }

        public static int SnapDown(this int value, int multiple)
        {
            //Round down value to nearest multiple, e.g. multiple == 8 and value == 30 -> 24.
            //Multiple needs to be negative if value is for this to work.
            //if (value < 0) multiple = -multiple;
            //return ((value + (0)) / multiple) * multiple;

            //Old code not correct for negative values. This is better:
            if (value < 0) value -= multiple - 1;
            return (value / multiple) * multiple; //Or line below...
            //return value - (value % multiple); //...also works, but not faster???
        }

        public static int SnapUp(this int value, int multiple)
        {
            //Round up value to nearest multiple, e.g. multiple == 8 and value == 30 -> 32.
            //Multiple needs to be negative if value is for this to work.
            //if (value < 0) multiple = -multiple;
            //return ((value + (multiple - 1)) / multiple) * multiple;

            //Old code not correct for negative values. This is better:
            if (value >= 0) value += multiple - 1;
            return (value / multiple) * multiple; //Or line below...
            //return value - (value % multiple); //...also works, but not faster???
        }

        public static string Clip(this string s, int length)
        {
            return s.Length > length ? s.Substring(0, length) : s;
        }

        public static string FormatWith(this string s, params object[] args)
        {
            //This extension method is about two times slower than using String.Format directly, but a bit nicer.
            return String.Format(s, args);
        }

        public static int FindNearestIndex(this List<int> list, int val)
        {
            return list.FindNearestIndex(val, (a, b) => a - b);
        }

        public static int FindNearestIndex(this List<float> list, float val)
        {
            return list.FindNearestIndex(val, (a, b) => a - b);
        }

        public static int FindNearestIndex(this List<double> list, double val)
        {
            return list.FindNearestIndex(val, (a, b) => a - b);
        }

        public static int FindNearestIndex<T>(this List<T> list, T val, Func<T, T, double> diff)
        {
            //List must be sorted for early break in loop.
            int nearestIndex = 0;
            double minDiff = double.MaxValue;
            for (int i = 0; i < list.Count; i++)
            {
                //double resDiff = Math.Abs((double)list[i] - (double)val); //Compile error. :(
                double resDiff = Math.Abs(diff(list[i], val));
                if (resDiff < minDiff)
                {
                    minDiff = resDiff;
                    nearestIndex = i;
                }
                else break; //Remove this line for unsorted lists.
            }
            return nearestIndex;
        }

        public static int GreatestCommonFactor(this int va11, int val2)
        {
            while (val2 != 0)
            {
                int temp = val2;
                val2 = va11 % val2;
                va11 = temp;
            }
            return va11;
        }

        public static int LeastCommonMultiple(this int va11, int val2)
        {
            return (va11 / GreatestCommonFactor(va11, val2)) * val2;
        }

        public static void CopyTo(this Stream src, Stream dst)
        {
            int nBytes;
            byte[] buffer = new byte[1 << 12];
            while ((nBytes = src.Read(buffer, 0, buffer.Length)) > 0)
            {
                dst.Write(buffer, 0, nBytes);
            }
        }

        public static DirectoryInfo CreateDirectory(string path, string name)
        {
            DirectoryInfo di = new DirectoryInfo(Path.Combine(path, name));
            di.Create();
            return di;
        }

        public static void Rename(this DirectoryInfo di, string newName)
        {
            di.MoveTo(Path.Combine(di.Parent.FullName, newName));
        }

        public static void Rename(this FileInfo fi, string newName)
        {
            fi.MoveTo(Path.Combine(fi.Directory.FullName, newName));
        }

        public static string GetFullPathWithEndingSlashes(this string s)
        {
            //Ensure that path string ends with a directory separator.
            return Path.GetFullPath(s).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
        }

        public static string GetFileNameWithoutExtension(this FileInfo fi)
        {
            return Path.GetFileNameWithoutExtension(fi.FullName);
        }

        public static string GetFilePathWithoutExtension(this FileInfo fi)
        {
            return fi.DirectoryName + Path.DirectorySeparatorChar + fi.GetFileNameWithoutExtension();
        }

        public static string GetRelativePath(this string path, string basepath)
        {
            return GetRelativePath(path, basepath, false);
        }

        public static string GetRelativeSubpath(this string path, string basepath)
        {
            //Path is returned if it is not a subpath of base (i.e. not shares full path of base).
            return GetRelativePath(path, basepath, true);
        }

        private static string GetRelativePath(string path, string basepath, bool subpathOnly)
        {
            //Assumes that paths have the same format (case sensitivity, separator, etc.).

            if (!Path.IsPathRooted(path)) return path; //Already a relative path?
            if (!Path.IsPathRooted(basepath)) throw new ArgumentException("Base path must be rooted!");

            string[] baseDirs = basepath.TrimEnd(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);
            string[] pathDirs = path.TrimEnd(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);
            //Find common root between paths.
            int commonRoot = 0;
            for (int i = 0; i < baseDirs.Length && i < pathDirs.Length; i++, commonRoot++)
            {
                if (baseDirs[i] != pathDirs[i])
                {
                    if (i == 0) return path; //No common root.
                    if (subpathOnly && i < baseDirs.Length) return path; //Not a subpath.
                    break;
                }
            }
            //Build relative path.
            StringBuilder relPath = new StringBuilder();
            for (int i = commonRoot; i < baseDirs.Length; i++) //Add up directions to relative path.
            {
                relPath.Append("..");
                relPath.Append(Path.DirectorySeparatorChar);
            }
            for (int i = commonRoot; i < pathDirs.Length; i++) //Add folders to relative path.
            {
                relPath.Append(pathDirs[i]);
                relPath.Append(Path.DirectorySeparatorChar);
            }
            return relPath.ToString().TrimEnd(Path.DirectorySeparatorChar);
        }

        public static string GetAbsolutePath(this string path, string basePath)
        {
            //Assumes that paths have the same format (case sensitivity, separator, etc.).

            if (Path.IsPathRooted(path)) return path; //Already an absolute path?
            if (!Path.IsPathRooted(basePath)) throw new ArgumentException("Base path must be rooted!");

            //return Path.GetFullPath(Path.Combine(basePath, path)); //Seems to work too, but a bit slower.

            string[] baseDirs = basePath.TrimEnd(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);
            string[] relDirs = path.TrimEnd(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);
            int ups = 0;
            for (int i = 0; i < relDirs.Length; i++, ups++) //Count up directions in relative path.
            {
                if (relDirs[i] != "..") break;
            }
            //Build absolute path.
            StringBuilder absPath = new StringBuilder();
            int baseDirsToAdd = Math.Max(1, baseDirs.Length - ups); //Always append root.
            for (int i = 0; i < baseDirsToAdd; i++) //Add base folders.
            {
                absPath.Append(baseDirs[i]);
                absPath.Append(Path.DirectorySeparatorChar);
            }
            for (int i = ups; i < relDirs.Length; i++) //Add relative folders.
            {
                absPath.Append(relDirs[i]);
                absPath.Append(Path.DirectorySeparatorChar);
            }
            return absPath.ToString().TrimEnd(Path.DirectorySeparatorChar);
        }

        public static void Populate<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        public static void Populate<T>(this T[,] array, T value)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    array[r, c] = value;
                }
            }
        }

        public static T[] CreateArray1D<T>(int length, T init)
        {
            T[] array = new T[length];
            array.Populate(init);
            return array;
        }

        public static T[,] CreateArray2D<T>(int rows, int cols, T init)
        {
            T[,] array = new T[rows, cols];
            array.Populate(init);
            return array;
        }

        public static bool TestArraysEqual<T>(T[] array1, T[] array2, out string msg)
        {
            if (ReferenceEquals(array1, array2))
            {
                msg = "Arrays are equal (same reference).";
                return true;
            }
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                msg = "Error arrays have different length or are null.";
                return false;
            }
            bool isEqual = true;
            msg = "Arrays are equal.";
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < array1.Length; i++)
            {
                if (!comparer.Equals(array1[i], array2[i]))
                {
                    msg = "Arrays are different.";
                    isEqual = false;
                    break;
                }
            }
            return isEqual;
        }

        //BinaryFormatter is deprecated.
        //public static long GetObjectSize(object obj)
        //{
        //    //Mark object with [Serializable()].
        //    //Exclude fields with [NonSerialized()].
        //    using (MemoryStream str = new MemoryStream())
        //    {
        //        new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(str, obj);
        //        return str.Length;
        //    }
        //}

        public static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                || potentialDescendant == potentialBase;
        }

        public static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }

        public static string GetExeBasePath()
        {
            //Get path to exe location.
            //I found many different solutions suggested, but which is correct?

            //if (System.Web.HttpContext.Current == null) return AppDomain.CurrentDomain.BaseDirectory;
            //else return FilePath.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");

            return AppDomain.CurrentDomain.BaseDirectory;

            //return (new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location)).Directory.ToString();

            //var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            //return new FileInfo(location.AbsolutePath).Directory.FullName; 
        }

        public static void DisposeCheck<T>(ref T disposable) where T : IDisposable
        {
            if (disposable != null)
            {
                disposable.Dispose();
                disposable = default(T);
            }
        }

        public static void Delay(this Action method, int delay)
        {
            System.Threading.Timer timer = null;
            timer = new System.Threading.Timer((c) =>
            {
                method.Invoke();
                timer.Dispose();
            }, null, delay, System.Threading.Timeout.Infinite);
        }
    }
}
