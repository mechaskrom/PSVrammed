using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MyCustomStuff;

namespace PSVrammed
{
    //PsxFin aka pSX emulator.
    class StateFilePsxFin : StateFile
    {
        private const string Description = "pSX state file";
        private static readonly string[] PossibleExtensions = { "psv" };
        public static readonly StateFileOpener Opener = new StateFileOpener(open, Description, PossibleExtensions);

        private readonly byte[] mFileContent;
        private readonly long mVramPointer; //State file vram location.

        protected StateFilePsxFin(byte[] vram, string info, FileInfo fileInfo, byte[] fileContent, long vramPointer)
            : base(vram, info, fileInfo)
        {
            mFileContent = fileContent;
            mVramPointer = vramPointer;
        }

        private static StateFilePsxFin open(FileInfo fileInfo, MemoryStream ms)
        {
            //pSX state format:
            //4 bytes header (ARS2)
            //chunks

            //chunk format:
            //4 bytes type (a string, sometimes in reverse with a null terminator first?)
            //uint size
            //uint ??? (not according to specs I could find?)

            //Read and check header.
            string header = ms.readChars(4);
            if (header != "ARS2")
            {
                throw new InvalidDataException("Signature is missing!");
            }

            StringWriter sw = new StringWriter();

            //File type.
            sw.WriteLine(Description);
            sw.WriteLine();

            //Header.
            sw.WriteLine("Header: {0}", header);

            //Process chunks. Read at least 3 uints.
            long vramPointer = -1;
            while (ms.getBytesRemaining() >= (3 * 4))
            {
                sw.WriteLine();

                //Read uint type.
                string type = ms.readChars(4);
                sw.WriteLine("Chunk type= {0}", type);

                //Read uint size.
                uint size = ms.readUInt32();
                sw.WriteLine("Chunk size= 0x{0:X}", size);

                //Read uint ???. Hmm what? Header description wrong?
                uint unknown = ms.readUInt32();

                //UPG chunk type = 0x47505500 ("\0UPG"), VRAM + some more?
                if (type == "\0UPG") //Reversed string? Big endian? Should probably read "GPU\0".
                {
                    vramPointer = ms.Position + (size - VramSize);
                }

                ms.Seek(size, SeekOrigin.Current); //Move to next chunk.
            }

            if (vramPointer < 0)
            {
                throw new InvalidDataException("VRAM chunk is missing!");
            }

            if ((vramPointer + VramSize) >= ms.Length)
            {
                throw new InvalidDataException("File is too small!");
            }

            string info = sw.ToString().Replace("\0", string.Empty);
            sw.Close();

            byte[] fileContent = ms.ToArray();
            byte[] vram = new byte[VramSize];
            Array.Copy(fileContent, vramPointer, vram, 0, vram.Length);

            return new StateFilePsxFin(vram, info, fileInfo, fileContent, vramPointer);
        }

        protected override void writeFile(string filePath)
        {
            Array.Copy(mVram, 0, mFileContent, mVramPointer, mVram.Length);
            File.WriteAllBytes(filePath, mFileContent);
        }
    }
}
