using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_git.src.Services
{
    public class FileCompressor
    {
        public byte[] Compress(byte[] data)
        {
            using MemoryStream compressedStream = new();
            using (ZLibStream zLibStream = new(compressedStream, CompressionMode.Compress, true))
            {
                zLibStream.Write(data, 0, data.Length);
            }
            return compressedStream.ToArray();
        }

        public byte[] Decompress(byte[] data)
        {
            using MemoryStream compressedStream = new(data);
            using MemoryStream decompressedStream = new();
            using (ZLibStream zLibStream = new(compressedStream, CompressionMode.Decompress))
            {
                zLibStream.CopyTo(decompressedStream);
            }
            return decompressedStream.ToArray();
        }
    }
}
