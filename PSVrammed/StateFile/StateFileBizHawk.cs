using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MyCustomStuff;

namespace PSVrammed
{
    //BizHawk emulator.
    class StateFileBizHawk : StateFile
    {
        private const string Description = "BizHawk state file";
        private static readonly string[] PossibleExtensions = { "state", "bak" };
        public static readonly StateFileOpener Opener = new StateFileOpener(open, Description, PossibleExtensions);

        private const int CoreVramOffsetOcto = 0x209A01; //BizHawk 2.7 Octoshock PSX core VRAM start.
        //private const int CoreVramOffsetNyma = 0x21D918; //Around here. BizHawk 2.10 Nymashock PSX core VRAM start. Varies! :(

        private readonly Dictionary<string, byte[]> mArchiveContent; //Extracted files in ZIP archive.
        private readonly byte[] mCoreContent; //Reference to the extracted core file.
        private readonly bool mIsCoreZsted; //Core file is zst compressed.
        private readonly int mCoreVramOffset;

        protected StateFileBizHawk(byte[] vram, string info, FileInfo fileInfo, Dictionary<string, byte[]> archiveContent, byte[] coreContent, bool isCoreZsted, int coreVramOffset)
            : base(vram, info, fileInfo)
        {
            mArchiveContent = archiveContent;
            mCoreContent = coreContent;
            mIsCoreZsted = isCoreZsted;
            mCoreVramOffset = coreVramOffset;
        }

        private static StateFileBizHawk open(FileInfo fileInfo, MemoryStream ms)
        {
            //BizHawk state format (Octoshock PSX core):
            //Cannot find any info about it, but it seems to be a ZIP archive with a few files.
            //The emulator state is dumped into the "Core.bin" file, where VRAM starts at offset 0x209A01.
            //Not sure how constant this offset is. Probably may change between different versions
            //of the Octoshock PSX core? This worked for BizHawk 2.7, 2.9 and 2.10 at least.
            //BizHawk 2.9 and later uses Zstandard (zst) compression on "Core.bin".

            //The other PSX core, Nymashock, is similar, but has a variable VRAM offset unfortunately
            //that is different between games (and states?).

            //TODO: Figure out a proper way to get the VRAM location in a BizHawk PSX state file.
            //The current method is really hacky and doesn't work at all for the Nymashock core.

            //Extract files in ZIP archive.
            Dictionary<string, byte[]> archiveContent;
            if (!tryOpenZipArchive(ms, out archiveContent))
            {
                throw new InvalidDataException("Open ZIP archive failed!");
            }

            byte[] versContent;
            if (!archiveContent.TryGetValue("BizVersion.txt", out versContent))
            {
                throw new InvalidDataException("ZIP archive doesn't contain 'BizVersion.txt'!");
            }
            if (!isSupportedVersion(versContent))
            {
                throw new InvalidDataException("Unsupported BizHawk version!");
            }

            byte[] coreContent;
            if (!archiveContent.TryGetValue("Core.bin", out coreContent))
            {
                throw new InvalidDataException("ZIP archive doesn't contain 'Core.bin'!");
            }

            //Decompress core if needed.
            bool isCoreZsted = tryDecompressZst(ref coreContent);

            bool isCoreNyma = isNymaCore(coreContent);
            if (isCoreNyma)
            {
                throw new InvalidDataException("Nymashock core is unsupported! Please change to Octoshock.");
            }

            //int coreVramOffset = isCoreNyma ? CoreVramOffsetNyma : CoreVramOffsetOcto;
            int coreVramOffset = CoreVramOffsetOcto;
            if (coreContent.Length < (coreVramOffset + VramSize))
            {
                throw new InvalidDataException("Core file is too small!");
            }

            //Copy VRAM from "Core.bin" file.
            byte[] vram = new byte[VramSize];
            Array.Copy(coreContent, coreVramOffset, vram, 0, vram.Length);

            string info = Description + (isCoreNyma ? " (nymashock)" : " (octoshock)");
            return new StateFileBizHawk(vram, info, fileInfo, archiveContent, coreContent, isCoreZsted, coreVramOffset);
        }

        protected override void writeFile(string filePath)
        {
            //Update VRAM.
            Array.Copy(mVram, 0, mCoreContent, mCoreVramOffset, mVram.Length);

            byte[] core = mIsCoreZsted ? mCoreContent.getCompressedZst() : mCoreContent;
            mArchiveContent["Core.bin"] = core;
            filePath.saveZipArchive(mArchiveContent);
        }

        private static bool isSupportedVersion(byte[] versContent)
        {
            //BizVersion.txt contains a string. Examples:
            //2.7    == "Version 2.7.0\r\n"
            //2.9.1  == "Version 2.9.1\r\n"
            //2.10   == "Version 2.10\r\n"

            string v = Encoding.ASCII.GetString(versContent);
            //string[] fields = v.Split('.');
            //if (fields.Length >= 2)
            //{
            //    int res;
            //    if (int.TryParse(fields[1], out res))
            //    {
            //        return v.StartsWith("Version 2.") && res >= 5;
            //    }
            //}

            //Is this enough to cover all supported versions?
            return v.StartsWith("Version 2.");
        }

        private static bool isNymaCore(byte[] coreContent)
        {
            //BizHawk has two PSX cores, Octoshock and Nymashock, and there doesn't
            //seem to exist a good way to identify which core a state used.
            //Nymashock is newer and was introduced in BizHawk 2.8 and the few states
            //I looked at all started with "ActivatedWaterboxHost". Probably not a very
            //reliable method, but let's use that for now.

            //TODO: Figure out a proper way to identify BizHawk PSX core used in a state file.
            string v = Encoding.ASCII.GetString(coreContent, 0, 14);
            return v == "ActivatedWater";
        }
    }
}
