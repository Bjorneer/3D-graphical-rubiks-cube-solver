using System.IO;
using RubiksCube3D.Interfaces;

namespace RubiksCube3D.Options
{
    internal class ControlsSettings : ISettings
    {
        public void Load(StreamReader stream)
        {
        }

        public void Save(StreamWriter stream)
        {
        }

        public object Clone()
        {
            return new ControlsSettings()
            {

            };
        }
    }
}