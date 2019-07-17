using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.Rubiks
{
    class Cube3D
    {
        public GraphicsDevice GraphicsDevice;

        Quadrangle3D[] cubeSides;

        private int sideLength;
        private int Length
        {
            get
            {
                return sideLength;
            }
        }
        private Vector3 centerOfCube;
        public Vector3 CenterOfCube
        {
            get
            {
                return centerOfCube;
            }
            set
            {
                centerOfCube = value;
                SetCubeCenter();
            }
        }

        public Cube3D(int sideLength, Vector3 cubeCenter, GraphicsDevice graphics)
        {
            cubeSides = new Quadrangle3D[6];
            GraphicsDevice = graphics;
            this.sideLength = sideLength;
            this.CenterOfCube = cubeCenter;
        }

        private void SetCubeCenter()
        {
            //Z-axis is inverted with turns
            Vector3 topLeftBack = CenterOfCube;
            topLeftBack.X += sideLength / 2;
            topLeftBack.Y += sideLength / 2;
            topLeftBack.Z -= sideLength / 2;
            cubeSides[0] = new Quadrangle3D(GraphicsDevice, topLeftBack, sideLength, Axis.X, Axis.Y, Turn.Negative);
            topLeftBack = CenterOfCube;
            topLeftBack.X -= sideLength / 2;
            topLeftBack.Y += sideLength / 2;
            topLeftBack.Z += sideLength / 2;
            cubeSides[1] = new Quadrangle3D(GraphicsDevice, topLeftBack, sideLength, Axis.Y, Axis.Z, Turn.Negative);
            topLeftBack = CenterOfCube;
            topLeftBack.X += sideLength / 2;
            topLeftBack.Y += sideLength / 2;
            topLeftBack.Z += sideLength / 2;
            cubeSides[2] = new Quadrangle3D(GraphicsDevice, topLeftBack, sideLength, Axis.X, Axis.Z, Turn.Negative);
            topLeftBack = CenterOfCube;
            topLeftBack.X += sideLength / 2;
            topLeftBack.Y += sideLength / 2;
            topLeftBack.Z += sideLength / 2;
            cubeSides[3] = new Quadrangle3D(GraphicsDevice, topLeftBack, sideLength, Axis.Z, Axis.Y, Turn.Positive);
            topLeftBack = CenterOfCube;
            topLeftBack.X += sideLength / 2;
            topLeftBack.Y += sideLength / 2;
            topLeftBack.Z += sideLength / 2;
            cubeSides[4] = new Quadrangle3D(GraphicsDevice, topLeftBack, sideLength, Axis.X, Axis.Y, Turn.Positive);
            topLeftBack = CenterOfCube;
            topLeftBack.X += sideLength / 2;
            topLeftBack.Y -= sideLength / 2;
            topLeftBack.Z += sideLength / 2;
            cubeSides[5] = new Quadrangle3D(GraphicsDevice, topLeftBack, sideLength, Axis.X, Axis.Z, Turn.Positive);
        }

        public Color GetColor(CubeSide side)
        {
            return cubeSides[(int)side].GetColor();
        }

        public void SetColor(CubeSide side, Color color)
        {
            SetColor(side, color, color, color, color);
        }

        public void SetColor(CubeSide side, Color clr1, Color clr2, Color clr3, Color clr4)
        {
            int sideNum = (int)side;
            cubeSides[sideNum].SetColor(clr1, clr2, clr3, clr4);
        }

        public void Rotate(float radian, Axis rotationAxis)
        {
            Rotate(radian, Vector3.Zero, rotationAxis);
        }

        public void Rotate(float radian, Vector3 rotationCenter, Axis rotationAxis)
        {
            for (int i = 0; i < cubeSides.Length; i++)
            {
                cubeSides[i].Rotate(radian, rotationAxis, rotationCenter);
            }
        }

        public void Draw(BasicEffect effect)
        {
            for (int i = 0; i < cubeSides.Length; i++)
            {
                cubeSides[i].Draw(effect);
            }
        }
    }
}
