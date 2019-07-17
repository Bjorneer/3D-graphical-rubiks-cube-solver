using Microsoft.Xna.Framework;
using RubiksCube3D.Rubiks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.Interfaces
{
    interface IRubiksSolver
    {
        string Solve(RubiksCube rubiks, Color[] repClrs);
    }
}
