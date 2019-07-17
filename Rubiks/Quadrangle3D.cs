using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.Rubiks
{

    class Quadrangle3D
    {
        public readonly GraphicsDevice GraphicsDevice;

        private VertexPositionColor[] _vertexPosition;
        private VertexPositionColor[] _sideLinesPosition;

        /// <summary>
        /// Uses two triangles to draw a square
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="topLeft"></param>
        /// <param name="topRight"></param>
        /// <param name="bottomLeft"></param>
        /// <param name="bottomRight"></param>
        public Quadrangle3D(
            GraphicsDevice graphics,
            VertexPositionColor topLeft,
            VertexPositionColor topRight,
            VertexPositionColor bottomLeft,
            VertexPositionColor bottomRight)
        {
            GraphicsDevice = graphics;

            _vertexPosition = new VertexPositionColor[4];
            _vertexPosition[0] = topLeft;
            _vertexPosition[1] = topRight;
            _vertexPosition[2] = bottomLeft;
            _vertexPosition[3] = bottomRight;
            _sideLinesPosition = new VertexPositionColor[5];
            _sideLinesPosition[0] = new VertexPositionColor(_vertexPosition[0].Position, Color.Black);
            _sideLinesPosition[1] = new VertexPositionColor(_vertexPosition[1].Position, Color.Black);
            _sideLinesPosition[2] = new VertexPositionColor(_vertexPosition[3].Position, Color.Black);
            _sideLinesPosition[3] = new VertexPositionColor(_vertexPosition[2].Position, Color.Black);
            _sideLinesPosition[4] = new VertexPositionColor(_vertexPosition[0].Position, Color.Black);
        }

        /// <summary>
        /// Generates a cube from a position with a specified length and along a axis
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="backTopLeftPos"></param>
        /// <param name="sideLength"></param>
        /// <param name="axis"></param>
        public Quadrangle3D(GraphicsDevice graphics, Vector3 backTopLeftPos, int sideLength, Axis axis1, Axis axis2, Turn turn)
        {
            if (axis1 == axis2)
            {
                throw new Exception("Axises can't be the same");
            }
            GraphicsDevice = graphics;
            _vertexPosition = new VertexPositionColor[4];
            Color color = Color.Black;

            Vector3 backTopRightPos = backTopLeftPos;
            Vector3 frontBottomLeftPos = backTopLeftPos;
            Vector3 frontBottomRightPos = backTopRightPos;

            if ((int)axis1 > (int)axis2)
            {
                int t = (int)axis1;
                axis1 = axis2;
                axis2 = (Axis)t;
            }
            switch (axis1)
            {
                case Axis.X:
                    switch (axis2)
                    {
                        case Axis.Y:
                            frontBottomLeftPos.Y -= sideLength;
                            frontBottomRightPos.Y -= sideLength;
                            frontBottomRightPos.X -= sideLength;
                            backTopRightPos.X -= sideLength;
                            break;
                        case Axis.Z:
                            frontBottomLeftPos.Z -= sideLength;
                            frontBottomRightPos.Z -= sideLength;
                            backTopRightPos.X -= sideLength;
                            frontBottomRightPos.X -= sideLength;
                            break;
                    }
                    break;
                case Axis.Y:
                    //y and z
                    frontBottomLeftPos.Z -= sideLength;
                    frontBottomRightPos.Z -= sideLength;
                    backTopRightPos.Y -= sideLength;
                    frontBottomRightPos.Y -= sideLength;
                    break;
            }

            if (turn == Turn.Positive)
            {
                _vertexPosition[0] = new VertexPositionColor(backTopLeftPos, color);
                _vertexPosition[1] = new VertexPositionColor(frontBottomLeftPos, color);
                _vertexPosition[2] = new VertexPositionColor(backTopRightPos, color);
                _vertexPosition[3] = new VertexPositionColor(frontBottomRightPos, color);
            }
            else
            {
                _vertexPosition[0] = new VertexPositionColor(frontBottomLeftPos, color);
                _vertexPosition[1] = new VertexPositionColor(backTopLeftPos, color);
                _vertexPosition[2] = new VertexPositionColor(frontBottomRightPos, color);
                _vertexPosition[3] = new VertexPositionColor(backTopRightPos, color);
            }
            _sideLinesPosition = new VertexPositionColor[5];
            if (turn == Turn.Positive)
            {
                _vertexPosition[0] = new VertexPositionColor(backTopLeftPos, color);
                _vertexPosition[1] = new VertexPositionColor(frontBottomLeftPos, color);
                _vertexPosition[2] = new VertexPositionColor(backTopRightPos, color);
                _vertexPosition[3] = new VertexPositionColor(frontBottomRightPos, color);

                _sideLinesPosition[0] = new VertexPositionColor(_vertexPosition[0].Position, Color.Black);
                _sideLinesPosition[1] = new VertexPositionColor(_vertexPosition[1].Position, Color.Black);
                _sideLinesPosition[2] = new VertexPositionColor(_vertexPosition[3].Position, Color.Black);
                _sideLinesPosition[3] = new VertexPositionColor(_vertexPosition[2].Position, Color.Black);
                _sideLinesPosition[4] = new VertexPositionColor(_vertexPosition[0].Position, Color.Black);
            }
            else
            {
                _vertexPosition[0] = new VertexPositionColor(frontBottomLeftPos, color);
                _vertexPosition[1] = new VertexPositionColor(backTopLeftPos, color);
                _vertexPosition[2] = new VertexPositionColor(frontBottomRightPos, color);
                _vertexPosition[3] = new VertexPositionColor(backTopRightPos, color);

                _sideLinesPosition[0] = new VertexPositionColor(_vertexPosition[1].Position, Color.Black);
                _sideLinesPosition[1] = new VertexPositionColor(_vertexPosition[0].Position, Color.Black);
                _sideLinesPosition[2] = new VertexPositionColor(_vertexPosition[2].Position, Color.Black);
                _sideLinesPosition[3] = new VertexPositionColor(_vertexPosition[3].Position, Color.Black);
                _sideLinesPosition[4] = new VertexPositionColor(_vertexPosition[1].Position, Color.Black);
            }
        }

        public void Draw(BasicEffect effect)
        {
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives(
                    PrimitiveType.TriangleStrip,
                    _vertexPosition,
                    0,
                    2);
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip,
    _sideLinesPosition,
    0, 4);
            }
        }

        public void Rotate(float radian, Axis rotationAxis, Vector3 rotationCenter)
        {
            for (int i = 0; i < 4; i++)
            {
                Matrix rotationMatrix = Matrix.Identity;
                switch (rotationAxis)
                {
                    case Axis.X:
                        rotationMatrix = Matrix.CreateRotationX(radian);
                        break;
                    case Axis.Y:
                        rotationMatrix = Matrix.CreateRotationY(radian);
                        break;
                    case Axis.Z:
                        rotationMatrix = Matrix.CreateRotationZ(radian);
                        break;
                }
                _vertexPosition[i].Position = Vector3.Transform(_vertexPosition[i].Position - rotationCenter, rotationMatrix);
                _vertexPosition[i].Position += rotationCenter;
            }
            for (int i = 0; i < 5; i++)
            {
                Matrix rotationMatrix = Matrix.Identity;
                switch (rotationAxis)
                {
                    case Axis.X:
                        rotationMatrix = Matrix.CreateRotationX(radian);
                        break;
                    case Axis.Y:
                        rotationMatrix = Matrix.CreateRotationY(radian);
                        break;
                    case Axis.Z:
                        rotationMatrix = Matrix.CreateRotationZ(radian);
                        break;
                }
                _sideLinesPosition[i].Position = Vector3.Transform(_sideLinesPosition[i].Position - rotationCenter, rotationMatrix);
                _sideLinesPosition[i].Position += rotationCenter;
            }
        }

        public Color GetColor()
        {
            return _vertexPosition[0].Color;
        }

        public void SetColor(Color color)
        {
            SetColor(color, color, color, color);
        }

        public void SetColor(Color clr1, Color clr2, Color clr3, Color clr4)
        {
            _vertexPosition[0].Color = clr1;
            _vertexPosition[1].Color = clr2;
            _vertexPosition[2].Color = clr3;
            _vertexPosition[3].Color = clr4;
        }
    }
}
