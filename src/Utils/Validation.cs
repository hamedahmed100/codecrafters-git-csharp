using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_git.src.Utils
{
    internal class Validation
    {
        public static void ValidateFileExists(string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException($"File {filePath} does not exist.");
        }

        public static void ValidateBlobLength(int declaredSize, int actualSize)
        {
            if (declaredSize != actualSize)
                throw new ArgumentException("Blob length mismatch.");
        }
    }
}
