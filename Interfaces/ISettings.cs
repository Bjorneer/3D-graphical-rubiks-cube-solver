using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.Interfaces
{
    interface ISettings : ICloneable
    {
        void Save(StreamWriter stream);
        void Load(StreamReader stream);
    }
}
