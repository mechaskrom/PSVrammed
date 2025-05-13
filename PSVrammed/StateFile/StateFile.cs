using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MyCustomStuff;

namespace PSVrammed
{
    //Read and write (VRAM) emulator save state file.
    abstract class StateFile
    {
        protected const int VramSize = 1024 * 512 * 2; //1048576 bytes total.

        private static readonly List<StateFileOpener> AvailableOpeners = new List<StateFileOpener>()
            { StateFileBizHawk.Opener, StateFileEpsxe.Opener, StateFilePsxFin.Opener, StateFileDuckStation.Opener, StateFileRaw.Opener };

        protected readonly byte[] mVram; //VRAM content from state.
        protected readonly string mInfo; //Info about state.
        protected FileInfo mFileInfo;

        protected StateFile(byte[] vram, string info, FileInfo fileInfo)
        {
            mVram = vram;
            mInfo = "File type: " + info;
            mFileInfo = fileInfo;
        }

        public byte[] Vram
        {
            get { return mVram; }
        }

        public string Info
        {
            get { return mInfo; }
        }

        public string FullName
        {
            get { return mFileInfo.FullName; }
        }

        public string Name
        {
            get { return Path.GetFileNameWithoutExtension(FullName); }
        }

        public string NameWithExtension
        {
            get { return mFileInfo.Name; }
        }

        public string Extension
        {
            get { return mFileInfo.Extension.TrimStart(new char[] { '.' }); }
        }

        public void saveInfo(string filePath)
        {
            File.WriteAllText(filePath, mInfo, Encoding.UTF8);
        }

        public static StateFile open(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            byte[] fileContent = File.ReadAllBytes(fi.FullName);
            using (MemoryStream ms = new MemoryStream(fileContent))
            using (StringWriter sw = new StringWriter())
            {
                //Make a list of state file openers to try on the file.
                string extension = fi.Extension.TrimStart('.');
                List<StateFileOpener> tryOpeners = new List<StateFileOpener>();
                foreach (StateFileOpener opener in AvailableOpeners)
                {
                    //Add opener first or last in list depending on file's extension.
                    //I.e. sort openers so likely candidates are tried first.
                    if (opener.isPossibleExt(extension)) tryOpeners.Insert(0, opener);
                    else tryOpeners.Add(opener);
                }

                StateFile stateFile;
                foreach (StateFileOpener opener in tryOpeners)
                {
                    if (opener.tryOpen(fi, ms, sw, out stateFile)) return stateFile;
                }

                //All available state file openers failed.
                throw new IOException(Strings.OpenStateUnrecognized + sw.ToString());
            }
        }

        protected static bool tryDecompressGzip(ref MemoryStream stream)
        {
            MemoryStream result = stream.tryDecompressGzip();
            if (result != null) stream = result;
            return result != null;
        }

        protected static bool tryDecompressZst(ref MemoryStream stream)
        {
            MemoryStream result = stream.tryDecompressZst();
            if (result != null) stream = result;
            return result != null;
        }

        protected static bool tryDecompressZst(ref byte[] data)
        {
            using (MemoryStream dataStream = new MemoryStream(data))
            {
                MemoryStream result = dataStream.tryDecompressZst();
                if (result != null)
                {
                    data = result.ToArray();
                    result.Dispose();
                }
                return result != null;
            }
        }

        protected static bool tryOpenZipArchive(MemoryStream stream, out Dictionary<string, byte[]> archiveFiles)
        {
            archiveFiles = stream.tryOpenZipArchive();
            return archiveFiles != null;
        }

        public void save(string filePath)
        {
            writeFile(filePath);
            //Update file info if path changed.
            if (filePath != mFileInfo.FullName)
            {
                mFileInfo = new FileInfo(filePath);
            }
        }

        protected abstract void writeFile(string filePath);
    }
}
