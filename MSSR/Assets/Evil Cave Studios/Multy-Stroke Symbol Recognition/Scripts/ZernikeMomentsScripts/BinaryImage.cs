using UnityEngine;

/// Represents a binary image as a float matrix and provides basic operations.

namespace SymbolRecognizer
{
    public class BinaryImage
    {
        public readonly float[,] Pixels;
        public readonly int Size;

        public BinaryImage(int imageSize)
        {
            Size = imageSize;
            Pixels = new float[imageSize, imageSize];
        }

        /// Sets all pixels to 0.
  
        public void Clear()
        {
            System.Array.Clear(Pixels, 0, Pixels.Length);
        }


        /// Sets the value of a pixel while ensuring it stays within bounds.
        /// Intensity is accumulated to handle overlapping strokes.

        public void SetPixelSafe(int x, int y, float intensity)
        {
            if (x >= 0 && x < Size && y >= 0 && y < Size)
            {
                Pixels[x, y] = Mathf.Min(Pixels[x, y] + intensity, 1f);
            }
        }


        /// Calculates the sum of intensities across all pixels.

        /// returns The total intensity sum.
        public float GetActivePixelCount()
        {
            float count = 0;
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    count += Pixels[x, y];
                }
            }
            return count;
        }

        /// Generates a string representation of the matrix for debugging.
        public void PrintToDebugLog()
        {
            string text = "";
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    text += Pixels[row, col].ToString("F1") + ", ";
                }
                text += "\n";
            }
            Debug.Log(text);
        }
    }
}

