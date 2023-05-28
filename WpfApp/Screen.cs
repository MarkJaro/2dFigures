using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text;

namespace WpfApp
{
    public class Screen
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public byte[] Pixels { get; private set; }

        public Screen(int x, int y)
        {
            Width = x;
            Height = y;
            Pixels = new byte[Width * 3 * Height];
        }

        public void Add(int x, int y, Color c)
        {

            int pixelIndex = (x + y * Width) * 3;

            Pixels[pixelIndex] = c.R;
            Pixels[pixelIndex + 1] = c.G;
            Pixels[pixelIndex + 2] = c.B;
        }

        public void Fill(Color c)
        {
            for (int y = 0; y < Height; y++)
            { 
                for (int x = 0; x < Width; x++)
                {
                    Add(x, y, c);
                }
            }
        }
    }
}
