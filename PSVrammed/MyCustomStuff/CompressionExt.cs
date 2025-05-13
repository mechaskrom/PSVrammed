using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCustomStuff
{
    //Zstandard (ZST) compression/decompression is using the ZstdSharp nuget package by Oleg.
    //https://github.com/oleg-st/ZstdSharp

    static class CompressionExt
    {
        private static byte[] getCompressed(byte[] src, Func<Stream, MemoryStream> compressor)
        {
            using (MemoryStream msSrc = new MemoryStream(src))
            {
                using (MemoryStream msDst = compressor(msSrc))
                {
                    return msDst.ToArray();
                }
            }
        }

        public static byte[] getCompressedGzip(this byte[] src)
        {
            return getCompressed(src, getCompressedGzip);
        }

        public static byte[] getCompressedZlib(this byte[] src)
        {
            return getCompressed(src, getCompressedZlib);
        }

        public static byte[] getCompressedZst(this byte[] src)
        {
            return getCompressed(src, getCompressedZst);
        }

        public static MemoryStream getCompressedGzip(this Stream src)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (GZipStream compressionStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                src.CopyTo(compressionStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public static MemoryStream getCompressedZlib(this Stream src)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (ZLibStream compressionStream = new ZLibStream(memoryStream, CompressionMode.Compress, true))
            {
                src.CopyTo(compressionStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public static MemoryStream getCompressedZst(this Stream src)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (ZstdSharp.CompressionStream compressionStream = new ZstdSharp.CompressionStream(memoryStream))
            {
                src.CopyTo(compressionStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public static MemoryStream getDecompressedGzip(this Stream src)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (GZipStream decompressionStream = new GZipStream(src, CompressionMode.Decompress, true))
            {
                decompressionStream.CopyTo(memoryStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public static MemoryStream getDecompressedZlib(this Stream src)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (ZLibStream decompressionStream = new ZLibStream(src, CompressionMode.Decompress, true))
            {
                decompressionStream.CopyTo(memoryStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public static MemoryStream getDecompressedZst(this Stream src)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (ZstdSharp.DecompressionStream decompressionStream = new ZstdSharp.DecompressionStream(src))
            {
                decompressionStream.CopyTo(memoryStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public static Dictionary<string, byte[]> openZipArchive(this Stream src)
        {
            //Extract files from a ZIP archive.
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
            using (ZipArchive zip = new ZipArchive(src, ZipArchiveMode.Read, true))
            {
                foreach (ZipArchiveEntry zipEntry in zip.Entries)
                {
                    using (Stream entryStream = zipEntry.Open())
                    {
                        byte[] entryContent = entryStream.readAllBytes();
                        files.Add(zipEntry.FullName, entryContent);
                    }
                }
            }
            return files;
        }

        public static void saveZipArchive(this string filePath, Dictionary<string, byte[]> files)
        {
            //Save files into a ZIP archive.
            using (FileStream fileStream = File.Create(filePath))
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                using (ZipArchive zip = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
                {
                    foreach (KeyValuePair<string, byte[]> file in files)
                    {
                        ZipArchiveEntry zipEntry = zip.CreateEntry(file.Key);
                        using (Stream stream = zipEntry.Open())
                        {
                            stream.writeAllBytes(file.Value);
                        }
                    }
                }
            }
        }

        public static MemoryStream tryDecompressGzip(this Stream stream) //Returns null on fail.
        {
            if (stream.Length < 2) return null;

            long orgPos = stream.Position;
            if (stream.readUInt16() == 0x8B1F) //Check for gzip signature.
            {
                stream.Position = orgPos;
                try
                {
                    return stream.getDecompressedGzip(); //File was gzip compressed.
                }
                catch (Exception ex)
                {
                    //Failed to decompress file, assume it wasn't compressed?
                    Log.add("GZip decompress failed: " + ex.Message);
                }
            }
            stream.Position = orgPos;
            return null;
        }

        public static MemoryStream tryDecompressZst(this Stream stream) //Returns null on fail.
        {
            if (stream.Length < 4) return null;

            long orgPos = stream.Position;
            if (stream.readUInt32() == 0xFD2FB528) //Check for zst signature.
            {
                stream.Position = orgPos;
                try
                {
                    return stream.getDecompressedZst(); //File was zst compressed.
                }
                catch (Exception ex)
                {
                    //Failed to decompress file, assume it wasn't compressed?
                    Log.add("Zstandard decompress failed: " + ex.Message);
                }
            }
            stream.Position = orgPos;
            return null;
        }

        public static Dictionary<string, byte[]> tryOpenZipArchive(this Stream stream) //Returns null on fail.
        {
            if (stream.Length < 2) return null;

            long orgPos = stream.Position;
            if (stream.readUInt16() == 0x4B50) //Check for zip signature (PK).
            {
                stream.Position = orgPos;
                try
                {
                    return stream.openZipArchive();
                }
                catch (Exception ex)
                {
                    //Failed to decompress file, assume it wasn't compressed?
                    Log.add("Zip decompress failed: " + ex.Message);
                }
            }
            stream.Position = orgPos;
            return null;
        }
    }
}
