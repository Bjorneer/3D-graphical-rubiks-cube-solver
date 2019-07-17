using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.Rubiks
{
    enum CubeSide
    {
        Front,
        Right,
        Top,
        Left,
        Back,
        Bottom
    }
    enum Axis
    {
        X,
        Y,
        Z
    }
    enum Turn
    {
        Negative = 0,
        Positive = 1
    }
}
