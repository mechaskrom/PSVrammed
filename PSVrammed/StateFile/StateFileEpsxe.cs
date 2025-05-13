using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MyCustomStuff;

namespace PSVrammed
{
    //ePSXe emulator.
    class StateFileEpsxe : StateFile
    {
        private const string Description = "ePSXe state file";
        private static readonly string[] PossibleExtensions = { "000", "001", "002", "003", "004" };
        public static readonly StateFileOpener Opener = new StateFileOpener(open, Description, PossibleExtensions);

        private readonly byte[] mFileContent;
        private readonly long mVramPointer; //State file VRAM location.
        private readonly bool mIsGzipped; //State file is gzip compressed.

        protected StateFileEpsxe(byte[] vram, string info, FileInfo fileInfo, byte[] fileContent, long vramPointer, bool isGzipped)
            : base(vram, info, fileInfo)
        {
            mFileContent = fileContent;
            mVramPointer = vramPointer;
            mIsGzipped = isGzipped;
        }

        private static StateFileEpsxe open(FileInfo fileInfo, MemoryStream ms)
        {
            //ePSXe state format:
            //0x40 bytes header (ePSXe + sony license number)
            //chunks

            //chunk format:
            //3 bytes type (a string?)
            //uint size
            //uint pointer

            //ePSXe states are often GZip compressed.
            bool isGzipped = tryDecompressGzip(ref ms);

            //Read and check header.
            string header = ms.readChars(0x40);
            if (!header.StartsWith("ePSXe", StringComparison.Ordinal))
            {
                throw new InvalidDataException("Signature is missing!");
            }

            StringWriter sw = new StringWriter();

            //File type.
            sw.WriteLine(Description);
            sw.WriteLine();

            //Header.
            sw.WriteLine("Header: {0}", header);

            //Process chunks. Read at least 3 bytes + 2 uints.
            long vramPointer = -1;
            while (ms.getBytesRemaining() >= (3 + (2 * 4)))
            {
                sw.WriteLine();

                //Read byte[3] type.
                string type = ms.readChars(3);
                sw.WriteLine("Chunk type= {0}", type);

                //Read uint size.
                uint size = ms.readUInt32();
                sw.WriteLine("Chunk size= 0x{0:X}", size);

                if (type == "SIO")
                {
                    size -= 0x190; //SIO chunk has wrong size???
                }
                else if (type == "GPU") //GPU chunk, VRAM + some more?
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

            return new StateFileEpsxe(vram, info, fileInfo, fileContent, vramPointer, isGzipped);
        }

        protected override void writeFile(string filePath)
        {
            //Update VRAM.
            Array.Copy(mVram, 0, mFileContent, mVramPointer, mVram.Length);

            byte[] fileContent = mIsGzipped ? mFileContent.getCompressedGzip() : mFileContent;
            File.WriteAllBytes(filePath, fileContent);
        }
    }
}
