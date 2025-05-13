using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PSVrammed
{
    //1MB raw dump of vram.
    class StateFileRaw : StateFile
    {
        private const string Description = "1MB VRAM dump file";
        private static readonly string[] PossibleExtensions = { "", "bin" };
        public static readonly StateFileOpener Opener = new StateFileOpener(open, Description, PossibleExtensions);

        private byte[] mFileContent;

        protected StateFileRaw(byte[] vram, string info, FileInfo fileInfo, byte[] fileContent)
            : base(vram, info, fileInfo)
        {
            mFileContent = fileContent;
        }

        private static StateFileRaw open(FileInfo fileInfo, MemoryStream ms)
        {
            //1MB raw dump of vram.

            //Check size.
            if (ms.Length != VramSize)
            {
                throw new InvalidDataException("File size isn't 1MB!");
            }

            StringWriter sw = new StringWriter();

            //File type.
            sw.WriteLine(Description);
            sw.WriteLine();

            string info = sw.ToString();
            sw.Close();

            byte[] fileContent = ms.ToArray();
            byte[] vram = new byte[VramSize];
            Array.Copy(fileContent, 0, vram, 0, vram.Length);

            return new StateFileRaw(vram, info, fileInfo, fileContent);
        }

        protected override void writeFile(string filePath)
        {
            Array.Copy(mVram, 0, mFileContent, 0, mVram.Length);
            File.WriteAllBytes(filePath, mFileContent);
        }
    }
}
