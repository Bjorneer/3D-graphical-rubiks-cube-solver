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
    class GraphicsSettings : ISettings
    {
        public Resolution Resolution { get; set; }

        public GraphicsSettings()
        {
            Resolution = new Resolution(1280, 720);
        }

        public void Load(StreamReader stream)
        {
            string[] res = stream.ReadLine().Split();
            Resolution = new Resolution(int.Parse(res[0]), int.Parse(res[1]));
        }

        public void Save(StreamWriter stream)
        {
            stream.WriteLine(Resolution.Width + " " + Resolution.Height);
        }

        public object Clone()
        {
            return new GraphicsSettings()
            {
                Resolution = new Resolution(this.Resolution.Width, this.Resolution.Height)
            };
        }
    }
}
