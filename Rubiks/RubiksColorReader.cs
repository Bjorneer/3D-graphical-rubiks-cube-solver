using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.Rubiks
{
    class RubiksColorReader
    {
        Color[] visualColors;
        System.Drawing.Color[] targetColors;

        public RubiksColorReader(System.Drawing.Color[] targets, Color[] visuals)
        {
            visualColors = visuals;
            targetColors = targets;
        }

        public Color[,] ReadRubiksSide(System.Drawing.Bitmap image, Vector2 pos, int dist)
        {
            Color[,] values = new Color[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    values[j, i] = visualColors[GetClosestColor(image.GetPixel((int)pos.X + dist * j, (int)pos.Y + dist * i))];
                }
            }

            return values;
        }

        public Color[,] ReadRubiksSide(System.Drawing.Bitmap image, Vector2 pos, int dist, int size)
        {
            Color[,] values = new Color[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    System.Drawing.Color avr = GetAveragePixel(image, (int)pos.X + dist * j, (int)pos.Y + dist * i, size);
                    values[j, i] = visualColors[GetClosestColor(avr)];
                }
            }

            return values;
        }

        private System.Drawing.Color GetAveragePixel(System.Drawing.Bitmap image, int x, int y, int size)
        {
            int r = 0;
            int b = 0;
            int g = 0;
            int total = 0;
            for (int i = -size / 2; i < size / 2; i++)
            {
                for (int j = -size / 2; j < size / 2; j++)
                {
                    System.Drawing.Color c = image.GetPixel(x + i, y + j);
                    r += c.R;
                    b += c.B;
                    g += c.G;
                    total++;
                }
            }
            return System.Drawing.Color.FromArgb(r / total, g / total, b / total);
        }

        private int GetClosestColor(System.Drawing.Color target)
        {
            var colorDiffs = targetColors.Select(n => ColorDiff(n, target)).Min(n => n);
            return Array.FindIndex(targetColors, n => ColorDiff(n, target) == colorDiffs);
        }



        float getHueDistance(float hue1, float hue2)
        {
            float d = Math.Abs(hue1 - hue2); return d > 180 ? 360 - d : d;
        }

        int ColorDiff(System.Drawing.Color c1, System.Drawing.Color c2)
        {
            return (int)Math.Sqrt((c1.R - c2.R) * (c1.R - c2.R)
                                   + (c1.G - c2.G) * (c1.G - c2.G)
                                   + (c1.B - c2.B) * (c1.B - c2.B));
        }
    }
}
