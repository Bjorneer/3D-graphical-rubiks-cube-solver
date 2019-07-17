using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RubiksCube3D.Rubiks.Solvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.Rubiks
{
    class RubiksCube
    {
        private BasicEffect drawEffect;
        public readonly GraphicsDevice GraphicsDevice;
        public Vector3 Position
        {
            get;
            set;
        }
        private Color[] _defaultColors;

        private Cube3D[,,] _cube;

        public float AngleX { get; set; }
        public float AngleY { get; set; }
        public float AngleZ { get; set; }


        public void SetRotation(CubeSide cubeSide)
        {
            if (inRotation == false)
            {
                inRotation = true;
                _rotation = cubeSide;
            }
        }

        public RubiksCube(int cubeLength, Vector3 position, GraphicsDevice graphics)
        {
            drawEffect = new BasicEffect(graphics);
            drawEffect.VertexColorEnabled = true;
            GraphicsDevice = graphics;
            Position = position;
            DefaultColors();
            InitializeCube(cubeLength);
        }

        private void InitializeCube(int cubeLength)
        {
            _cube = new Cube3D[3, 3, 3];
            int sizeOfSquare = cubeLength / 3;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        int j2 = j - 1;
                        int k2 = k - 1;
                        int i2 = i - 1;
                        _cube[i, j, k] = new Cube3D(sizeOfSquare, new Vector3(i2 * sizeOfSquare, j2 * sizeOfSquare, k2 * sizeOfSquare), GraphicsDevice);
                    }
                }
            }
        }

        internal void SetDefaultColor()
        {
            SetColor(CubeSide.Front, _defaultColors[0], _defaultColors[1], _defaultColors[2], _defaultColors[3]);
            SetColor(CubeSide.Back, _defaultColors[0], _defaultColors[1], _defaultColors[2], _defaultColors[3]);
            SetColor(CubeSide.Right, _defaultColors[0], _defaultColors[1], _defaultColors[2], _defaultColors[3]);
            SetColor(CubeSide.Left, _defaultColors[0], _defaultColors[1], _defaultColors[2], _defaultColors[3]);
            SetColor(CubeSide.Bottom, _defaultColors[0], _defaultColors[1], _defaultColors[2], _defaultColors[3]);
            SetColor(CubeSide.Top, _defaultColors[0], _defaultColors[1], _defaultColors[2], _defaultColors[3]);
        }

        private void DefaultColors()
        {
            _defaultColors = new Color[4];
            _defaultColors[0] = Color.White;
            _defaultColors[1] = Color.LightGray;
            _defaultColors[2] = Color.LightSlateGray;
            _defaultColors[3] = Color.WhiteSmoke;
        }

        public void SetSideColor(CubeSide side, Color[,] colors)
        {
            if (colors.GetLength(0) != 3 && colors.GetLength(1) != 3)
            {
                throw new ArgumentException();
            }
            Color[] color = new Color[9];
            int t = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    color[t] = colors[j,i];
                    t++;
                }
            }
            int xf, yf,zf;
            xf = yf = zf = 0;
            int xt,yt,zt;
            xt = yt = zt = 3;
            switch (side)
            {
                case CubeSide.Front:
                    zt = 1;
                    break;
                case CubeSide.Right:
                    xt = 1;
                    break;
                case CubeSide.Top:
                    yf = 2;
                    break;
                case CubeSide.Left:
                    xf = 2;
                    break;
                case CubeSide.Back:
                    zf = 2;
                    break;
                case CubeSide.Bottom:
                    yt = 1;
                    break;
            }
            int idx = 0;
            for (int x = xf; x < xt; x++)
            {
                for (int y = yf; y < yt; y++)
                {
                    for (int z = zf; z < zt; z++)
                    {
                        _cube[x, y, z].SetColor(side, color[idx]);
                        idx++;
                    }
                }
            }
        }

        public void SetColor(CubeSide side, Color c1, Color c2, Color c3, Color c4)
        {
            int xf, yf, zf;
            xf = yf = zf = 0;
            int xt, yt, zt;
            xt = yt = zt = 3;
            switch (side)
            {
                case CubeSide.Front:
                    zt = 1;
                    break;
                case CubeSide.Right:
                    xt = 1;
                    break;
                case CubeSide.Top:
                    yf = 2;
                    break;
                case CubeSide.Left:
                    xf = 2;
                    break;
                case CubeSide.Back:
                    zf = 2;
                    break;
                case CubeSide.Bottom:
                    yt = 1;
                    break;
            }
            for (int x = xf; x < xt; x++)
            {
                for (int y = yf; y < yt; y++)
                {
                    for (int z = zf; z < zt; z++)
                    {
                        _cube[x, y, z].SetColor(side, c1,c2,c3,c4);
                    }
                }
            }
        }

        public Color[] GetCubeSideColors(CubeSide side)
        {
            Color[] clrs = new Color[9];
            int xf, yf, zf;
            xf = yf = zf = 0;
            int xt, yt, zt;
            xt = yt = zt = 3;
            switch (side)
            {
                case CubeSide.Front:
                    zt = 1;
                    break;
                case CubeSide.Right:
                    xt = 1;
                    break;
                case CubeSide.Top:
                    yf = 2;
                    break;
                case CubeSide.Left:
                    xf = 2;
                    break;
                case CubeSide.Back:
                    zf = 2;
                    break;
                case CubeSide.Bottom:
                    yt = 1;
                    break;
                default:
                    throw new ArgumentException();
            }
            int k = 0;
            for (int x = xf; x < xt; x++)
            {
                for (int y = yf; y < yt; y++)
                {
                    for (int z = zf; z < zt; z++)
                    {
                        clrs[k] = _cube[x, y, z].GetColor(side);
                        k++;
                    }
                }
            }
            return clrs;
        }

        public void Draw(Camera camera)
        {
            drawEffect.View = camera.ViewMatrix;
            drawEffect.Projection = camera.ProjectionMatrix;
            drawEffect.World = Matrix.CreateRotationX(AngleX) * Matrix.CreateRotationY(AngleY) * Matrix.CreateRotationZ(AngleZ) * Matrix.CreateTranslation(Position);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        _cube[i, j, k].Draw(drawEffect);
                    }
                }
            }
        }

        private CubeSide _rotation;
        private float radianPerTick
        {
            get
            {
                return MathHelper.PiOver2 / 60 / _timePerRotation;
            }
        }
        private float _timePerRotation = 1;
        public float TimePerRotation
        {
            get
            {
                return _timePerRotation;
            }
            set
            {
                _timePerRotation = value;
            }
        }
        private bool inRotation = false;
        private float totalRotation = 0;
        public bool InRotation { get => inRotation; private set => inRotation = value; }
        public void UpdateOld()
        {
            if (inRotation)
            {
                float curRot = (float)Math.Min(radianPerTick, MathHelper.PiOver2 - totalRotation);
                totalRotation += curRot;
                if (_rotation == CubeSide.Front)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            _cube[i, j, 0].Rotate(curRot, Axis.Z);
                        }
                    }
                }
                else if (_rotation == CubeSide.Left)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            _cube[2, i, j].Rotate(-curRot, Axis.X);
                        }
                    }
                }
                else if (_rotation == CubeSide.Back)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            _cube[i, j, 2].Rotate(-curRot, Axis.Z);
                        }
                    }
                }
                else if (_rotation == CubeSide.Right)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            _cube[0, i, j].Rotate(curRot, Axis.X);
                        }
                    }
                }
                else if (_rotation == CubeSide.Top)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            _cube[i, 2, j].Rotate(-curRot, Axis.Y);
                        }
                    }
                }
                else if (_rotation == CubeSide.Bottom)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            _cube[i, 0, j].Rotate(curRot, Axis.Y);
                        }
                    }
                }
                if (totalRotation >= MathHelper.PiOver2)
                {
                    switch (_rotation)
                    {
                        case CubeSide.Front:
                            Front();
                            break;
                        case CubeSide.Right:
                            Right();
                            break;
                        case CubeSide.Left:
                            Left();
                            break;
                        case CubeSide.Back:
                            Back();
                            break;
                        case CubeSide.Top:
                            Top();
                            break;
                        case CubeSide.Bottom:
                            Bottom();
                            break;
                    }
                    totalRotation = 0;
                    inRotation = false;
                }
            }
        }

        float quarterTimeRate;
        CubeSide rotationSide;
        int rotationTurns;
        float radPerTick;
        public bool SetAnimatiedMove(CubeSide side, float turnRate, int turns)
        {
            if (!inRotation)
            {
                quarterTimeRate = turnRate;
                rotationSide = side;
                rotationTurns = turns;
                totalRotation = 0;
                inRotation = true;
                radPerTick = MathHelper.PiOver2 / 60 / turnRate * (turns < 0 ? -1 : 1);
            }
            else
            {
                return false;
            }
            return true;
        }

        public void ResetMove()
        {
            if (inRotation)
            {
                if (rotationSide == CubeSide.Front)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            _cube[i, j, 0].Rotate(-totalRotation, Axis.Z);
                        }
                    }
                }
                else if (rotationSide == CubeSide.Left)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            _cube[2, i, j].Rotate(-totalRotation, Axis.X);
                        }
                    }
                }
                else if (rotationSide == CubeSide.Back)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            _cube[i, j, 2].Rotate(totalRotation, Axis.Z);
                        }
                    }
                }
                else if (rotationSide == CubeSide.Right)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            _cube[0, i, j].Rotate(-totalRotation, Axis.X);
                        }
                    }
                }
                else if (rotationSide == CubeSide.Top)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            _cube[i, 2, j].Rotate(totalRotation, Axis.Y);
                        }
                    }
                }
                else if (rotationSide == CubeSide.Bottom)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            _cube[i, 0, j].Rotate(-totalRotation, Axis.Y);
                        }
                    }
                }
                inRotation = false;
                totalRotation = 0;
            }
        }

        public void Update()
        {
            if (inRotation)
            {
                float curRot;
                if (rotationTurns > 0)
                    curRot = (float)Math.Min(radPerTick, (MathHelper.PiOver2 * rotationTurns) - totalRotation);
                else
                    curRot = (float)Math.Max(radPerTick, (MathHelper.PiOver2 * rotationTurns) - totalRotation);
                totalRotation += curRot;
                if (rotationSide == CubeSide.Front)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            _cube[i, j, 0].Rotate(curRot, Axis.Z);
                        }
                    }
                }
                else if (rotationSide == CubeSide.Left)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            _cube[2, i, j].Rotate(-curRot, Axis.X);
                        }
                    }
                }
                else if (rotationSide == CubeSide.Back)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            _cube[i, j, 2].Rotate(-curRot, Axis.Z);
                        }
                    }
                }
                else if (rotationSide == CubeSide.Right)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            _cube[0, i, j].Rotate(curRot, Axis.X);
                        }
                    }
                }
                else if (rotationSide == CubeSide.Top)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            _cube[i, 2, j].Rotate(-curRot, Axis.Y);
                        }
                    }
                }
                else if (rotationSide == CubeSide.Bottom)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            _cube[i, 0, j].Rotate(curRot, Axis.Y);
                        }
                    }
                }
                if (Math.Abs(totalRotation) >= Math.Abs(MathHelper.PiOver2 * rotationTurns))
                {
                    int am = rotationTurns < 0 ? 3:(rotationTurns == 2 ? 2 : 1);
                    for (int i = 0; i < am; i++)
                    {
                        switch (rotationSide)
                        {
                            case CubeSide.Front:
                                Front();
                                break;
                            case CubeSide.Right:
                                Right();
                                break;
                            case CubeSide.Left:
                                Left();
                                break;
                            case CubeSide.Back:
                                Back();
                                break;
                            case CubeSide.Top:
                                Top();
                                break;
                            case CubeSide.Bottom:
                                Bottom();
                                break;
                        }
                    }
                    totalRotation = 0;
                    inRotation = false;
                }
            }
        }

        #region Moves
        public void Front()
        {
            Cube3D[,,] temp = _cube.Clone() as Cube3D[,,];
            _cube[0, 0, 0] = temp[0, 2, 0];
            _cube[1, 0, 0] = temp[0, 1, 0];
            _cube[2, 0, 0] = temp[0, 0, 0];
            _cube[2, 1, 0] = temp[1, 0, 0];
            _cube[2, 2, 0] = temp[2, 0, 0];
            _cube[1, 2, 0] = temp[2, 1, 0];
            _cube[0, 2, 0] = temp[2, 2, 0];
            _cube[0, 1, 0] = temp[1, 2, 0];
        }

        public void Left()
        {
            Rotate();
            Rotate();
            Rotate();
            Front();
            Rotate();
        }

        public void Right()
        {
            Rotate();
            Front();
            Rotate();
            Rotate();
            Rotate();
        }

        public void Back()
        {
            Rotate();
            Rotate();
            Front();
            Rotate();
            Rotate();
        }

        public void Bottom()
        {
            Turn();
            Left();
            Turn();
            Turn();
            Turn();
        }

        public void Top()
        {
            Turn();
            Right();
            Turn();
            Turn();
            Turn();
        }

        public void Rotate()
        {
            Cube3D[,,] temp = _cube.Clone() as Cube3D[,,];
            for (int y = 0; y < 3; y++)
            {
                _cube[0, y, 0] = temp[0, y, 2];
                _cube[1, y, 0] = temp[0, y, 1];
                _cube[2, y, 0] = temp[0, y, 0];

                _cube[2, y, 0] = temp[0, y, 0];
                _cube[2, y, 1] = temp[1, y, 0];
                _cube[2, y, 2] = temp[2, y, 0];

                _cube[2, y, 2] = temp[2, y, 0];
                _cube[1, y, 2] = temp[2, y, 1];
                _cube[0, y, 2] = temp[2, y, 2];

                _cube[0, y, 2] = temp[2, y, 2];
                _cube[0, y, 1] = temp[1, y, 2];
                _cube[0, y, 0] = temp[0, y, 2];
            }
        }
        /// <summary>
        /// Vänder kuben vrid framåt medurs
        /// </summary>
        public void Turn()
        {
            Cube3D[,,] temp = _cube.Clone() as Cube3D[,,];
            for (int z = 0; z < 3; z++)
            {
                _cube[0, 0, z] = temp[0, 2, z];
                _cube[0, 1, z] = temp[1, 2, z];
                _cube[0, 2, z] = temp[2, 2, z];

                _cube[0, 2, z] = temp[2, 2, z];
                _cube[1, 2, z] = temp[2, 1, z];
                _cube[2, 2, z] = temp[2, 0, z];

                _cube[2, 2, z] = temp[2, 0, z];
                _cube[2, 1, z] = temp[1, 0, z];
                _cube[2, 0, z] = temp[0, 0, z];

                _cube[2, 0, z] = temp[0, 0, z];
                _cube[1, 0, z] = temp[0, 1, z];
                _cube[0, 0, z] = temp[0, 2, z];
            }
        }
        #endregion

        public void Scramble(Color[] representedColors)
        {
            CubeSide side = 0;
            RubiksColor[,] cubeColors = new RubiksColor[9, 6];

            for (int i = 0; i < 6; i++)
            {
                int b = 0;
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++, b++)
                    {
                        cubeColors[b, i] = (RubiksColor)i;
                    }
                }
            }
            int amountOfMoves = GameEngine.Random.Next(20, 41);
            for (int i = 0; i < amountOfMoves; i++)
            {
                int move = GameEngine.Random.Next(12);
                side = (CubeSide)(move % 6);
                for (int j = 0; j < (move >= 6 ? 3 : 1); j++)
                {
                    switch (side)
                    {
                        case CubeSide.Front:
                            Front(cubeColors);
                            break;
                        case CubeSide.Right:
                            Right(cubeColors);
                            break;
                        case CubeSide.Top:
                            Up(cubeColors);
                            break;
                        case CubeSide.Left:
                            Left(cubeColors);
                            break;
                        case CubeSide.Back:
                            Back(cubeColors);
                            break;
                        case CubeSide.Bottom:
                            Down(cubeColors);
                            break;
                    }
                }
            }
            CubeSide prevSide;
            side = CubeSide.Back;
            for (int i = 0; i < 6; i++)
            {
                prevSide = side;
                Color[,] clrs = new Color[3,3];
                RubiksColor[] tclrs = new RubiksColor[9];
                for (int j = 0; j < 9; j++)
                {
                    tclrs[j] = cubeColors[j, i];
                }
                switch (side)
                {
                    case CubeSide.Front:
                        RotateClock(tclrs);
                        Flip(tclrs);
                        break;
                    case CubeSide.Right:
                        RotateClock(tclrs);
                        Flip(tclrs);
                        break;
                    case CubeSide.Top:
                        RotateClock(tclrs);
                        Flip(tclrs);
                        break;
                    case CubeSide.Left:
                        RotateClock(tclrs);

                        break;
                    case CubeSide.Back:
                        RotateClock(tclrs);
                        RotateClock(tclrs);
                        RotateClock(tclrs);
                        break;
                    case CubeSide.Bottom:
                        RotateClock(tclrs);
                        RotateClock(tclrs);
                        RotateClock(tclrs);
                        break;
                }
                int x = 0;
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++, x++)
                    {
                        clrs[k, j] = representedColors[(int)tclrs[x]];
                    }
                }
                switch (side)
                {
                    case CubeSide.Front:
                        side = CubeSide.Bottom;
                        break;
                    case CubeSide.Right:
                        side = CubeSide.Front;
                        break;
                    case CubeSide.Top:
                        side = CubeSide.Right;
                        break;
                    case CubeSide.Left:
                        side = CubeSide.Top;
                        break;
                    case CubeSide.Back:
                        side = CubeSide.Left;
                        break;
                    case CubeSide.Bottom:
                        break;
                }
                SetSideColor(prevSide, clrs);
            }
        }

        #region ScrambleHelpers
        private void RotateClock(RubiksColor[] clrs)
        {
            RubiksColor[] t = clrs.Clone() as RubiksColor[];
            clrs[0] = t[6];
            clrs[1] = t[3];
            clrs[2] = t[0];
            clrs[3] = t[7];
            clrs[5] = t[1];
            clrs[6] = t[8];
            clrs[7] = t[5];
            clrs[8] = t[2];
        }

        private void Flip(RubiksColor[] clrs)
        {
            RubiksColor[] t = clrs.Clone() as RubiksColor[];
            clrs[0] = t[6];
            clrs[1] = t[7];
            clrs[2] = t[8];
            clrs[6] = t[0];
            clrs[7] = t[1];
            clrs[8] = t[2];
        }
        #region PossibleMoves
        private void Rotate(RubiksColor[,] rubiks)
        {
            int[] faceByte = { 1, 4, 2, 0, 3, 5 };
            int[] platsbyteBottom = { 2, 5, 8, 1, 4, 7, 0, 3, 6 };
            int[] platsbyte = { 6, 3, 0, 7, 4, 1, 8, 5, 2 };
            RubiksColor[,] tempRubiks = rubiks.Clone() as RubiksColor[,];
            for (int i = 0; i < 6; i++)
            {
                if (i == 5)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        rubiks[j, i] = tempRubiks[platsbyteBottom[j], 5];
                    }
                    continue;
                }
                for (int j = 0; j < 9; j++)
                {
                    rubiks[j, i] = tempRubiks[platsbyte[j], faceByte[i]];
                }
            }
        }
        private void Turn(RubiksColor[,] rubiks)
        {
            RubiksColor[,] copyRubiks = rubiks.Clone() as RubiksColor[,];
            int[] faceByte = { 2, 1, 4, 3, 5, 0 };
            int[] platsByte1 = { 2, 5, 8, 1, 4, 7, 0, 3, 6 };
            int[] platsByte3 = { 6, 3, 0, 7, 4, 1, 8, 5, 2 };
            for (int i = 0; i < 6; i++)
            {
                if (i == 3)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        rubiks[j, i] = copyRubiks[platsByte3[j], 3];
                    }
                }
                else if (i == 1)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        rubiks[j, i] = copyRubiks[platsByte1[j], 1];
                    }
                }
                else
                {
                    for (int j = 0; j < 9; j++)
                    {
                        rubiks[j, i] = copyRubiks[j, faceByte[i]];
                    }
                }
            }
        }
        private void Right(RubiksColor[,] rubiks)
        {
            RubiksColor[,] copyRubiks = rubiks.Clone() as RubiksColor[,];
            int[] platsByte = { 6, 3, 0, 7, 4, 1, 8, 5, 2 };
            int[] faceByte = { 2, 1, 4, 3, 5, 0 };
            for (int i = 0; i < 9; i++)
            {
                rubiks[i, 3] = copyRubiks[platsByte[i], 3];
            }
            for (int i = 0; i < 6; i++)
            {
                if (i == 3 || i == 1)
                {
                    continue;
                }
                rubiks[2, i] = copyRubiks[2, faceByte[i]];
                rubiks[5, i] = copyRubiks[5, faceByte[i]];
                rubiks[8, i] = copyRubiks[8, faceByte[i]];
            }
        }
        private void Left(RubiksColor[,] rubiks)
        {
            Rotate(rubiks);
            Rotate(rubiks);
            Right(rubiks);
            Rotate(rubiks);
            Rotate(rubiks);
        }
        private void Up(RubiksColor[,] rubiks)
        {
            Turn(rubiks);
            Back(rubiks);
            Turn(rubiks);
            Turn(rubiks);
            Turn(rubiks);
        }
        private void Front(RubiksColor[,] rubiks)
        {
            Rotate(rubiks);
            Rotate(rubiks);
            Rotate(rubiks);
            Right(rubiks);
            Rotate(rubiks);
        }
        private void Down(RubiksColor[,] rubiks)
        {
            Turn(rubiks);
            Front(rubiks);
            Turn(rubiks);
            Turn(rubiks);
            Turn(rubiks);
        }
        private void Back(RubiksColor[,] rubiks)
        {
            Rotate(rubiks);
            Right(rubiks);
            Rotate(rubiks);
            Rotate(rubiks);
            Rotate(rubiks);
        }
        private void RightPrime(RubiksColor[,] rubiks)
        {
            Right(rubiks);
            Right(rubiks);
            Right(rubiks);
        }
        private void BackPrime(RubiksColor[,] rubiks)
        {
            Back(rubiks);
            Back(rubiks);
            Back(rubiks);
        }
        private void LeftPrime(RubiksColor[,] rubiks)
        {
            Left(rubiks);
            Left(rubiks);
            Left(rubiks);
        }
        private void UpPrime(RubiksColor[,] rubiks)
        {
            Up(rubiks);
            Up(rubiks);
            Up(rubiks);
        }
        private void DownPrime(RubiksColor[,] rubiks)
        {
            Down(rubiks);
            Down(rubiks);
            Down(rubiks);
        }
        private void FrontPrime(RubiksColor[,] rubiks)
        {
            Front(rubiks);
            Front(rubiks);
            Front(rubiks);
        }
        private void FrontDouble(RubiksColor[,] rubiks)
        {
            Front(rubiks);
            Front(rubiks);
        }
        private void BackDouble(RubiksColor[,] rubiks)
        {
            Back(rubiks);
            Back(rubiks);
        }
        private void DownDouble(RubiksColor[,] rubiks)
        {
            Down(rubiks);
            Down(rubiks);
        }
        private void LeftDouble(RubiksColor[,] rubiks)
        {
            Left(rubiks);
            Left(rubiks);
        }
        private void RightDouble(RubiksColor[,] rubiks)
        {
            Right(rubiks);
            Right(rubiks);
        }
        private void UpDouble(RubiksColor[,] rubiks)
        {
            Up(rubiks);
            Up(rubiks);
        }
        #endregion
        #endregion
    }
}
