using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyCustomStuff;

namespace PSVrammed
{
    class StateFileOpener
    {
        public delegate StateFile Opener(FileInfo fi, MemoryStream ms);

        private readonly Opener mOpener;
        private readonly string mDescription;
        private readonly string[] mExtensions; //Possible file extensions.

        public StateFileOpener(Opener opener, string description, string[] extensions)
        {
            mOpener = opener;
            mDescription = description;
            mExtensions = extensions;
        }

        public string Description
        {
            get { return mDescription; }
        }

        public bool isPossibleExt(string ext)
        {
            return mExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase);
        }

        public bool tryOpen(FileInfo fi, MemoryStream ms, StringWriter sw, out StateFile state)
        {
            Log.timeBegin();
            try
            {
                ms.Seek(0, SeekOrigin.Begin);
                state = mOpener(fi, ms);
                return true;
            }
            catch (Exception ex)
            {
                sw.WriteLine(mDescription + ": " + ex.Message);
            }
            finally
            {
                Log.timeEnd(mDescription + " try open");
            }
            state = null;
            return false;
        }
    }
}
