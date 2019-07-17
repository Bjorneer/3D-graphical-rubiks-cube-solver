using Microsoft.Xna.Framework;
using RubiksCube3D.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.Options
{
    class RubiksCubeSettings : ISettings
    {
        //Colors
        private System.Drawing.Color[] cameraColors;
        private Color[] visualColors;

        public System.Drawing.Color GetCameraColor(int idx)
        {
            return cameraColors[idx];
        }
        public Color GetVisualColor(int idx)
        {
            return visualColors[idx];
        }

        public RubiksCubeSettings()
        {
            cameraColors = new System.Drawing.Color[6];
            cameraColors[0] = System.Drawing.Color.FromArgb(38, 229, 51);
            cameraColors[1] = System.Drawing.Color.FromArgb(246, 41, 41);
            cameraColors[2] = System.Drawing.Color.FromArgb(246, 171, 41);
            cameraColors[3] = System.Drawing.Color.FromArgb(225, 225, 235);
            cameraColors[4] = System.Drawing.Color.FromArgb(10, 10, 170);
            cameraColors[5] = System.Drawing.Color.FromArgb(250, 255, 68);
            visualColors = new Color[6];
            visualColors[0] = Color.Green;
            visualColors[1] = Color.Red;
            visualColors[2] = Color.Orange;
            visualColors[3] = Color.White;
            visualColors[4] = Color.Blue;
            visualColors[5] = Color.Yellow;
        }

        public void SetCameraColor(int idx, System.Drawing.Color color)
        {
            if (idx >= 0 && idx < 6)
            {
                cameraColors[idx] = color;
            }
        }

        public void SetVisualColor(int idx, Color color)
        {
            if (idx >= 0 && idx < 6)
            {
                visualColors[idx] = color;
            }
        }

        public void Load(StreamReader stream)
        {
            cameraColors = new System.Drawing.Color[6];
            for (int i = 0; i < 6; i++)
            {
                string[] clrValues = stream.ReadLine().Split();
                cameraColors[i] = System.Drawing.Color.FromArgb(int.Parse(clrValues[3]), int.Parse(clrValues[0]), int.Parse(clrValues[1]), int.Parse(clrValues[2]));
            }
            visualColors = new Color[6];
            for (int i = 0; i < 6; i++)
            {
                string[] clrValues = stream.ReadLine().Split();
                visualColors[i] = new Color(int.Parse(clrValues[0]), int.Parse(clrValues[1]), int.Parse(clrValues[2]), int.Parse(clrValues[3]));
            }
        }

        public void Save(StreamWriter stream)
        {
            for (int i = 0; i < 6; i++)
            {
                stream.WriteLine(cameraColors[i].R + " " + cameraColors[i].G + " " + cameraColors[i].B + " " + cameraColors[i].A);
            }
            for (int i = 0; i < 6; i++)
            {
                stream.WriteLine(visualColors[i].R + " " + visualColors[i].G + " " + visualColors[i].B + " " + visualColors[i].A);
            }
        }

        public object Clone()
        {
            Color[] visCol = new Color[6];
            System.Drawing.Color[] camCol = new System.Drawing.Color[6];
            for (int i = 0; i < 6; i++)
            {
                visCol[i] = visualColors[i];
                camCol[i] = cameraColors[i];
            }
            return new RubiksCubeSettings
            {
                visualColors = visCol,
                cameraColors = camCol
            };
        }
    }
}
