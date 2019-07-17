using RubiksCube3D.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.Options
{
    class AudioSettings : ISettings
    {
        public float MusicVolume { get; set; }

        public float SoundEffectVolume { get; set; }

        public void Load(StreamReader stream)
        {
            MusicVolume = float.Parse(stream.ReadLine());
            SoundEffectVolume = float.Parse(stream.ReadLine());
        }

        public void Save(StreamWriter stream)
        {
            stream.WriteLine(MusicVolume);
            stream.WriteLine(SoundEffectVolume);
        }

        public object Clone()
        {
            return new AudioSettings
            {
                MusicVolume = this.MusicVolume,
                SoundEffectVolume = this.SoundEffectVolume
            };
        }
    }
}
