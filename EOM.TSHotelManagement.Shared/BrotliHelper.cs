using System.IO.Compression;
using System.Text;

namespace EOM.TSHotelManagement.Shared
{
    public static class BrotliHelper
    {
        public static string CompressString(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            using var outputStream = new MemoryStream();
            using (var brotliStream = new BrotliStream(outputStream, CompressionLevel.Optimal))
            {
                int bufferSize = 81920;
                for (int i = 0; i < inputBytes.Length; i += bufferSize)
                {
                    int chunkSize = Math.Min(bufferSize, inputBytes.Length - i);
                    brotliStream.Write(inputBytes, i, chunkSize);
                }
            }

            return Convert.ToBase64String(outputStream.ToArray());
        }


        public static string DecompressString(string input)
        {
            var compressedData = Convert.FromBase64String(input);

            using var inputStream = new MemoryStream(compressedData);
            using var outputStream = new MemoryStream();
            using (var brotliStream = new BrotliStream(inputStream, CompressionMode.Decompress))
            {
                brotliStream.CopyTo(outputStream);
            }

            return Encoding.UTF8.GetString(outputStream.ToArray());
        }
    }
}
