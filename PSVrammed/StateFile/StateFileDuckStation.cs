using MyCustomStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp;

namespace PSVrammed
{
    class StateFileDuckStation : StateFile
    {
        private const string Description = "DuckStation state file";
        private static readonly string[] PossibleExtensions = { "sav" };
        public static readonly StateFileOpener Opener = new StateFileOpener(open, Description, PossibleExtensions);

        private readonly Header mHeader;
        private readonly byte[] mMediaPath;
        private readonly byte[] mScreenshot;
        private readonly byte[] mData;
        private readonly int mVramPointer; //VRAM location in data section.

        protected StateFileDuckStation(byte[] vram, string info, FileInfo fileInfo, Header header, byte[] mediaPath, byte[] screenshot, byte[] data, int vramPointer)
            : base(vram, info, fileInfo)
        {
            mHeader = header;
            mMediaPath = mediaPath;
            mScreenshot = screenshot;
            mData = data;
            mVramPointer = vramPointer;
        }

        private static StateFileDuckStation open(FileInfo fileInfo, MemoryStream ms)
        {
            if (ms.Length < 256)
            {
                throw new InvalidDataException("File is too small!");
            }

            //Read and check header.
            Header header = new Header();
            header.read(ms);
            if (header.magic != Header.MagicValue)
            {
                throw new InvalidDataException("Signature is missing!");
            }

            //Validate some header values.
            if ((header.offset_to_media_path + header.media_path_length) > ms.Length ||
                (header.offset_to_screenshot + header.screenshot_compressed_size) > ms.Length ||
                header.screenshot_width >= 32768 || header.screenshot_height >= 32768 ||
                (header.offset_to_data + header.data_compressed_size) > ms.Length ||
                header.data_uncompressed_size > Header.MaxStateSize)
            {
                throw new InvalidDataException("Header is corrupted!");
            }

            byte[] fileContent = ms.ToArray();
            byte[] mediaPath = readMediaPath(header, fileContent);
            byte[] screenshot = readScreenshot(header, fileContent);
            byte[] data = readData(header, fileContent);

            if (header.data_uncompressed_size != data.Length)
            {
                throw new InvalidDataException("Mismatched data size values!");
            }

            int vramPointer = findVramPointer(data);
            if (vramPointer < 0)
            {
                throw new InvalidDataException("VRAM marker is missing!");
            }

            byte[] vram = new byte[VramSize];
            Array.Copy(data, vramPointer, vram, 0, VramSize);

            string info = toInfo(header, vramPointer);
            return new StateFileDuckStation(vram, info, fileInfo, header, mediaPath, screenshot, data, vramPointer);
        }

        private static byte[] readMediaPath(Header header, byte[] fileContent)
        {
            byte[] mediaPath = new byte[header.media_path_length];
            Array.Copy(fileContent, header.offset_to_media_path, mediaPath, 0, mediaPath.Length);
            return mediaPath;
        }

        private static byte[] readScreenshot(Header header, byte[] fileContent)
        {
            byte[] screenshot = new byte[header.screenshot_compressed_size];
            Array.Copy(fileContent, header.offset_to_screenshot, screenshot, 0, screenshot.Length);
            return screenshot;
        }

        private static byte[] readData(Header header, byte[] fileContent)
        {
            byte[] data = new byte[header.data_compressed_size];
            Array.Copy(fileContent, header.offset_to_data, data, 0, data.Length);

            if (header.data_compression_type == (UInt32)Header.Compression.None)
            {
                return data;
            }
            else if (header.data_compression_type == (UInt32)Header.Compression.Zstandard)
            {
                using (MemoryStream msData = new MemoryStream(data))
                {
                    using (MemoryStream msDecompressed = msData.getDecompressedZst())
                    {
                        return msDecompressed.ToArray();
                    }
                }
            }
            else if (header.data_compression_type == (UInt32)Header.Compression.Deflate)
            {
                using (MemoryStream msData = new MemoryStream(data))
                {
                    using (MemoryStream msDecompressed = msData.getDecompressedZlib())
                    {
                        return msDecompressed.ToArray();
                    }
                }
            }
            throw new InvalidDataException("Unknown data compression type!");
        }

        private static int findVramPointer(byte[] data)
        {
            //VRAM starts after the string "GPU-VRAM" in duckstation states?
            //Seems to be at the end of the state data so search backwards to speed this up a bit.
            for (int i = data.Length - VramSize; i >= 8; i--)
            {
                if (data[i - 8] == 'G' &&
                    data[i - 7] == 'P' &&
                    data[i - 6] == 'U' &&
                    data[i - 5] == '-' &&
                    data[i - 4] == 'V' &&
                    data[i - 3] == 'R' &&
                    data[i - 2] == 'A' &&
                    data[i - 1] == 'M') return i;
            }
            return -1;
        }

        protected override void writeFile(string filePath)
        {
            using (FileStream fs = File.Create(filePath))
            {
                //Update VRAM.
                Array.Copy(mVram, 0, mData, mVramPointer, VramSize);

                byte[] data = mData;
                if (mHeader.data_compression_type == (UInt32)Header.Compression.Zstandard)
                {
                    data = data.getCompressedZst();
                }
                else if (mHeader.data_compression_type == (UInt32)Header.Compression.Deflate)
                {
                    data = data.getCompressedZlib();
                }

                //Update header.
                mHeader.data_compressed_size = (UInt32)data.Length;

                mHeader.write(fs);
                fs.writeAllBytes(mMediaPath);
                fs.writeAllBytes(mScreenshot);
                fs.writeAllBytes(data);
            }
        }

        private static string toInfo(Header header, int vramPointer)
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine(Description);
            sw.WriteLine();

            sw.WriteLine("Header values:");
            sw.WriteLine("Magic = " + header.magic);
            sw.WriteLine("Version = " + header.version);
            sw.WriteLine("Title = " + Encoding.UTF8.GetString(header.title).TrimEnd('\0'));
            sw.WriteLine("Serial = " + Encoding.UTF8.GetString(header.serial).TrimEnd('\0'));
            sw.WriteLine("Media path length = " + header.media_path_length);
            sw.WriteLine("Offset to media path = " + header.offset_to_media_path);
            sw.WriteLine("Media subimage index = " + header.media_subimage_index);
            sw.WriteLine("Screenshot compression type = " + toCompressionDescr(header.screenshot_compression_type));
            sw.WriteLine("Screenshot width = " + header.screenshot_width);
            sw.WriteLine("Screenshot height = " + header.screenshot_height);
            sw.WriteLine("Screenshot compressed size = " + header.screenshot_compressed_size);
            sw.WriteLine("Offset to screenshot = " + header.offset_to_screenshot);
            sw.WriteLine("Data compression type = " + toCompressionDescr(header.data_compression_type));
            sw.WriteLine("Data compressed size = " + header.data_compressed_size);
            sw.WriteLine("Data uncompressed size = " + header.data_uncompressed_size);
            sw.WriteLine("Offset to data = " + header.offset_to_data);

            sw.WriteLine();
            sw.WriteLine("VRAM pointer = {0} (0x{1:X})", vramPointer, vramPointer);

            return sw.ToString();
        }

        private static string toCompressionDescr(UInt32 v)
        {
            if (v == (UInt32)Header.Compression.None) return "None";
            if (v == (UInt32)Header.Compression.Deflate) return "Deflate";
            if (v == (UInt32)Header.Compression.Zstandard) return "Zstandard";
            return "(unknown)";
        }

        protected class Header
        {
            //Format according to "save_state_version.h" in DuckStation's source code may 2025.

            public const UInt32 MagicValue = 0x43435544; //= "DUCC".
            public const UInt32 MaxStateSize = 32 * 1024 * 1024;

            public enum Compression
            {
                None = 0,
                Deflate = 1,
                Zstandard = 2,
            }

            public UInt32 magic;
            public UInt32 version;
            public byte[] title; //128 bytes.
            public byte[] serial; //32 bytes.

            public UInt32 media_path_length;
            public UInt32 offset_to_media_path;
            public UInt32 media_subimage_index;

            // Screenshot compression added in version 69.
            // Uncompressed size not stored, it can be inferred from width/height.
            public UInt32 screenshot_compression_type;
            public UInt32 screenshot_width;
            public UInt32 screenshot_height;
            public UInt32 screenshot_compressed_size;
            public UInt32 offset_to_screenshot;

            public UInt32 data_compression_type;
            public UInt32 data_compressed_size;
            public UInt32 data_uncompressed_size;
            public UInt32 offset_to_data;

            public void read(Stream stream)
            {
                magic = stream.readUInt32();
                version = stream.readUInt32();
                title = stream.readArray(128);
                serial = stream.readArray(32);

                media_path_length = stream.readUInt32();
                offset_to_media_path = stream.readUInt32();
                media_subimage_index = stream.readUInt32();

                screenshot_compression_type = version >= 69 ? stream.readUInt32() : 0;
                screenshot_width = stream.readUInt32();
                screenshot_height = stream.readUInt32();
                screenshot_compressed_size = version >= 69 ? stream.readUInt32() : screenshot_width * screenshot_height * 4;
                offset_to_screenshot = stream.readUInt32();

                data_compression_type = stream.readUInt32();
                data_compressed_size = stream.readUInt32();
                data_uncompressed_size = stream.readUInt32();
                offset_to_data = stream.readUInt32();
            }

            public void write(Stream stream)
            {
                stream.writeUInt32(magic);
                stream.writeUInt32(version);
                stream.writeAllBytes(title);
                stream.writeAllBytes(serial);

                stream.writeUInt32(media_path_length);
                stream.writeUInt32(offset_to_media_path);
                stream.writeUInt32(media_subimage_index);

                if (version >= 69) stream.writeUInt32(screenshot_compression_type);
                stream.writeUInt32(screenshot_width);
                stream.writeUInt32(screenshot_height);
                if (version >= 69) stream.writeUInt32(screenshot_compressed_size);
                stream.writeUInt32(offset_to_screenshot);

                stream.writeUInt32(data_compression_type);
                stream.writeUInt32(data_compressed_size);
                stream.writeUInt32(data_uncompressed_size);
                stream.writeUInt32(offset_to_data);
            }
        }
    }
}
