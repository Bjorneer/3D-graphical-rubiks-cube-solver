using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D
{
    static class Extensions
    {
        public static System.Drawing.Color ToDrawingColor(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static Color ToXnaColor(this System.Drawing.Color clr)
        {
            return new Color(clr.R, clr.G, clr.B, clr.A);
        }

        public static System.Drawing.Color GetAverageColor(this System.Drawing.Bitmap image, int x, int y, int size)
        {
            int r = 0;
            int b = 0;
            int g = 0;
            int total = 0;
            for (int i = -size / 2; i < size / 2; i++)
            {
                for (int j = -size / 2; j < size / 2; j++)
                {
                    System.Drawing.Color c = image.GetPixel(x + j, y + i);
                    r += c.R;
                    b += c.B;
                    g += c.G;
                    total++;
                }
            }
            return System.Drawing.Color.FromArgb(r / total, g / total, b / total);
        }

        private static void HlsToRgb(float h, float l, float s , out int r, out int g, out int b)
        {
            float p2;
            if (l <= 0.5) p2 = l * (1 + s);
            else p2 = l + s - l * s;

            float p1 = 2 * l - p2;
            float _r, _g, _b;
            if (s == 0)
            {
                _r = l;
                _g = l;
                _b = l;
            }
            else
            {
                _r = QqhToRgb(p1, p2, h + 120);
                _g = QqhToRgb(p1, p2, h);
                _b = QqhToRgb(p1, p2, h - 120);
            }

            // Convert RGB to the 0 to 255 range.
            r = (int)(_r * 255.0);
            g = (int)(_g * 255.0);
            b = (int)(_b * 255.0);
        }

        private static float QqhToRgb(float q1, float q2, float hue)
        {
            if (hue > 360) hue -= 360;
            else if (hue < 0) hue += 360;

            if (hue < 60) return q1 + (q2 - q1) * hue / 60;
            if (hue < 180) return q2;
            if (hue < 240) return q1 + (q2 - q1) * (240 - hue) / 60;
            return q1;
        }

        internal static Color[,] RotateClockwise(this Color[,] colors, int turns)
        {
            Color[,] ret = colors.Clone() as Color[,];
            for (int i = 0; i < turns; i++)
            {
                Color[,] temp = ret.Clone() as Color[,];
                ret[0, 0] = temp[0, 2];
                ret[1, 0] = temp[0, 1];
                ret[2, 0] = temp[0, 0];
                ret[2, 1] = temp[1, 0];
                ret[2, 2] = temp[2, 0];
                ret[1, 2] = temp[2, 1];
                ret[0, 2] = temp[2, 2];
                ret[0, 1] = temp[1, 2];
            }
            return ret;
        }

        internal static Color[,] FlipHorisontally(this Color[,] colors)
        {
            Color[,] temp = colors.Clone() as Color[,];
            temp[0, 0] = colors[2, 0];
            temp[2, 0] = colors[0, 0];
            temp[0, 1] = colors[2, 1];
            temp[2, 1] = colors[0, 1];
            temp[0, 2] = colors[2, 2];
            temp[2, 2] = colors[0, 2];
            return temp;
        }
    }
}
