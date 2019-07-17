using RubiksCube3D.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RubiksCube3D.IO;
using RubiksCube3D.Managers;
using RubiksCube3D.Rubiks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RubiksCube3D.Rubiks.Solvers;
using RubiksCube3D.Options;
using RubiksCube3D.Screens.ScreenClasses;

namespace RubiksCube3D.Screens
{
    class RubiksSolverScreen : IScreen
    {
        ScreenManager screenManager;
        GraphicsDevice GraphicsDevice;

        private ContentManager _content;

        Camera cubeCamera;
        RubiksCube cube;

        IRubiksSolver rubiksSolver;
        SolutionViewer viewer;
        SolutionControl control;

        string[] solutionMoves;

        public RubiksSolverScreen(RubiksCube rubiks)
        {
            cube = rubiks;
            cube.Position = new Vector3(0,0,0);
            cube.AngleY = 0.3f;
            cube.AngleX = -0.3f;
            cubeCamera = new Camera(cube.GraphicsDevice.Viewport.Width / (float)cube.GraphicsDevice.Viewport.Height, new Vector3(0, 0, 0));
            cubeCamera.Zoom = 100;
        }

        public void Initialize(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
            this.GraphicsDevice = screenManager.GraphicsDevice;
            _content = new ContentManager(screenManager.ServiceProvider, Constants.CONTENT_DIRECTORY);

            Color[] repColors = new Color[6];
            for (int i = 0; i < repColors.Length; i++)
            {
                repColors[i] = Settings.Instance.RubiksCube.GetVisualColor(i);
            }
            rubiksSolver = new RubiksBeginnerSolver();
            try
            {
                solutionMoves = rubiksSolver.Solve(cube, repColors).Split();
                solutionMoves = ConvertAndShortenSolution(solutionMoves);
            }
            catch (RubiksCubeUnsolvableException)
            {
                Console.WriteLine("Unsolvable");
            }

            viewer = new SolutionViewer(solutionMoves, _content);
            control = new SolutionControl(_content);
            control.OnPlay += new EventHandler((s, e) => 
            {
                if (moveIdx >= solutionMoves.Length)
                {
                    control.Play = false;
                }
            });
            control.OnForward += new EventHandler((s, e) =>
            {
                if (moveIdx <= solutionMoves.Length)
                {
                    viewer.Increment();
                    if (moveIdx < solutionMoves.Length)
                    {
                        cube.ResetMove();
                        CubeRotate(0);
                        cube.Update();
                        moveIdx++;
                    }
                    control.Play = false;
                }
            });
            control.OnBackward += new EventHandler((s, e) =>
            {
                if (moveIdx > 0)
                {
                    viewer.Decrement();
                    moveIdx--;
                    cube.ResetMove();
                    CubeRotateBackward(0);
                    cube.Update();
                    control.Play = false;
                }
            });
            control.OnFastBackward += new EventHandler((s, e) =>
            {
                if (moveIdx > 0)
                {
                    int times = moveIdx > 4 ? 5 : moveIdx;
                    Console.WriteLine(times);
                    cube.ResetMove();
                    for (int i = 0; i < times; i++)
                    {
                        viewer.Decrement();
                        moveIdx--;
                        CubeRotateBackward(0);
                        cube.Update();
                    }
                    control.Play = false;
                }
            });
            control.OnFastForward += new EventHandler((s, e) =>
            {
                if (moveIdx <= solutionMoves.Length)
                {
                    int times = (solutionMoves.Length - moveIdx) > 4 ? 5 : solutionMoves.Length - moveIdx;
                    Console.WriteLine(times);
                    cube.ResetMove();
                    for (int i = 0; i < times; i++)
                    {
                        viewer.Increment();
                        if (moveIdx < solutionMoves.Length)
                        {
                            CubeRotate(0);
                            cube.Update();
                            moveIdx++;
                        }
                    }
                    control.Play = false;
                }
            });
        }

        public void CubeRotateBackward(float time)
        {
            CubeSide rotationSide = 0;
            switch (solutionMoves[moveIdx][0])
            {
                case 'D':
                    rotationSide = CubeSide.Bottom;
                    break;
                case 'U':
                    rotationSide = CubeSide.Top;
                    break;
                case 'F':
                    rotationSide = CubeSide.Front;
                    break;
                case 'L':
                    rotationSide = CubeSide.Left;
                    break;
                case 'R':
                    rotationSide = CubeSide.Right;
                    break;
                case 'B':
                    rotationSide = CubeSide.Back;
                    break;
            }
            int turns = -1;
            if (solutionMoves[moveIdx].Length != 0)
            {
                if (solutionMoves[moveIdx].Length > 1)
                {
                    turns = 1;
                    if (solutionMoves[moveIdx][1] == '2')
                    {
                        turns = 2;
                    }
                }
            }
            cube.SetAnimatiedMove(rotationSide, time, turns);
        }

        public void CubeRotate(float time)
        {
            CubeSide rotationSide = 0;
            switch (solutionMoves[moveIdx][0])
            {
                case 'D':
                    rotationSide = CubeSide.Bottom;
                    break;
                case 'U':
                    rotationSide = CubeSide.Top;
                    break;
                case 'F':
                    rotationSide = CubeSide.Front;
                    break;
                case 'L':
                    rotationSide = CubeSide.Left;
                    break;
                case 'R':
                    rotationSide = CubeSide.Right;
                    break;
                case 'B':
                    rotationSide = CubeSide.Back;
                    break;
            }
            int turns = 1;
            if (solutionMoves[moveIdx].Length != 0)
            {
                if (solutionMoves[moveIdx].Length > 1)
                {
                    turns = -1;
                    if (solutionMoves[moveIdx][1] == '2')
                    {
                        turns = 2;
                    }
                }
            }
            cube.SetAnimatiedMove(rotationSide, time, turns);
        }

        private string[] ConvertAndShortenSolution(string[] solMoves)
        {
            Console.Clear();
            //Convert
            char[] side = { 'F', 'R', 'U', 'L', 'B', 'D' };
            CubeSide[] corSide = { CubeSide.Front, CubeSide.Right, CubeSide.Top, CubeSide.Left, CubeSide.Back, CubeSide.Bottom };
            List<string> list = new List<string>();
            foreach (var move in solMoves)
            {
                CubeSide rotationSide = 0;
                if (move.Length != 0)
                {
                    if (move == "Ro")
                    {
                        CubeSide[] t = corSide.Clone() as CubeSide[];
                        corSide[0] = t[1];
                        corSide[1] = t[4];
                        corSide[4] = t[3];
                        corSide[3] = t[0];
                    }
                    else if (move == "Tu")
                    {
                        CubeSide[] t = corSide.Clone() as CubeSide[];
                        corSide[0] = t[5];
                        corSide[5] = t[4];
                        corSide[4] = t[2];
                        corSide[2] = t[0];
                    }
                    else if (side.Any(x => x == move[0]))
                    {
                        rotationSide = corSide[Array.FindIndex(side, x => x == move[0])];
                        switch (rotationSide)
                        {
                            case CubeSide.Back:
                                list.Add("B");
                                break;
                            case CubeSide.Front:
                                list.Add("F");
                                break;
                            case CubeSide.Top:
                                list.Add("U");
                                break;
                            case CubeSide.Left:
                                list.Add("L");
                                break;
                            case CubeSide.Right:
                                list.Add("R");
                                break;
                            case CubeSide.Bottom:
                                list.Add("D");
                                break;
                            default:
                                break;
                        }
                        if (move.Length > 1)
                        {
                            list[list.Count - 1] = list.Last() + move[1];
                        }
                    }
                }
            }
            //Shorten
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append(list[i]);
                if (i != list.Count-1)
                {
                    sb.Append(" ");
                }
            }
            string sol = sb.ToString();
            //for (int i = 0; i < 5; i++)
            //{
            //    foreach (var t in side)
            //    {
            //        sol = sol.Replace($"{t} {t} {t} {t} ", "");
            //        sol = sol.Replace($"{t} {t} {t} ", t + "' ");
            //        sol = sol.Replace($"{t} {t}", t + "2 ");
            //        sol = sol.Replace($"{t}' {t} ", "");
            //        sol = sol.Replace($"{t} {t}' ", "");
            //        sol = sol.Replace($"{t}2 {t}' ", t + " ");
            //        sol = sol.Replace($"{t}' {t}2 ", t + " ");
            //    }
            //}

            sol = sol.Replace("  ", " ").Trim(' ');
            Console.WriteLine(sol);
            return sol.Split();
        }

        public void Update(GameTime gameTime, Input current, Input previous)
        {
            UpdateCubeAngle(current);

            control.Update(gameTime, current, previous);

            UpdateCube(gameTime, current, previous);
        }

        private void UpdateCubeAngle(Input current)
        {
            if (current.Keyboard.IsKeyDown(Keys.Left))
            {
                cube.AngleY = cube.AngleY + 0.05f;
            }
            else if (current.Keyboard.IsKeyDown(Keys.Right))
            {
                cube.AngleY = cube.AngleY - 0.05f;
            }
            if (current.Keyboard.IsKeyDown(Keys.Up))
            {
                cube.AngleX = cube.AngleX - 0.05f;
            }
            else if (current.Keyboard.IsKeyDown(Keys.Down))
            {
                cube.AngleX = cube.AngleX + 0.05f;
            }
        }

        int moveIdx = 0;
        float timePerQuarterTurn = 0.7f;
        private void UpdateCube(GameTime gameTime, Input current, Input previous)
        {
            if (cube.InRotation == false && control.Play)
            {
                if (moveIdx <= solutionMoves.Length)
                {
                    if (moveIdx != solutionMoves.Length)
                    {
                        if (solutionMoves[moveIdx].Length > 0)
                        {
                            CubeRotate(timePerQuarterTurn);
                        }
                        moveIdx++;
                    }
                    if (moveIdx != 0)
                    {
                        viewer.Increment();
                    }

                }
                else if(control.Play)
                {
                    control.Play = false;
                }
            }
            cube.Update();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //2D
            spriteBatch.Begin();
            viewer.Draw(spriteBatch);
            control.Draw(spriteBatch);
            spriteBatch.End();


            //3D
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            cube.Draw(cubeCamera);
        }

        public void Dispose()
        {
            _content.Dispose();
        }
    }
}
