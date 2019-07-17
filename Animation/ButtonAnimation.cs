using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.Animation
{
    class ButtonAnimation
    {
        public float? Angle = null;
        public Rectangle? Bounds { get; set; } = null;
        public Color? Color { get; set; } = null;

        public ButtonAnimation(float? angle, Rectangle? bounds, Color? color)
        {
            Angle = angle;
            Bounds = bounds;
            Color = color;
        }
    }
}
