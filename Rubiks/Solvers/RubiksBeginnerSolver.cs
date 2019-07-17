using Microsoft.Xna.Framework;
using RubiksCube3D.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.Rubiks.Solvers
{
    class RubiksBeginnerSolver : IRubiksSolver
    {
        public string Solve(RubiksCube rubiks, Color[] representedColors)
        {
            RubiksColor[,] rubiksCube = ReadRubiksColors(rubiks, representedColors);

            StringBuilder solution = new StringBuilder();
            solution.Append(SolveCross(rubiksCube));
            Console.WriteLine(solution.ToString());
            solution.Append(SolveFirstLayer(rubiksCube));
            Console.WriteLine(solution.ToString());
            solution.Append(SolveSecondLayer(rubiksCube));
            Console.WriteLine(solution.ToString());
            solution.Append(SolveTopCross(rubiksCube));
            Console.WriteLine(solution.ToString());
            solution.Append(SolveTopEdges(rubiksCube));
            Console.WriteLine(solution.ToString());
            solution.Append(SolvePermutateCorners(rubiksCube));
            Console.WriteLine(solution.ToString());
            solution.Append(SolveOrientCorners(rubiksCube));
            return solution.ToString();
        }

        private void PrintConsole(RubiksColor[,] cube)
        {
            ConsoleColor[] clr = { ConsoleColor.Green, ConsoleColor.Red, ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.Yellow };
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.BackgroundColor = clr[(int)(cube[j, i])];
                    Console.Write("  ");
                    if (j % 3 == 2)
                    {
                        Console.WriteLine();
                    }
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private string ShortenSolution(string currentSolution)
        {
            currentSolution = currentSolution.Replace("Ro Ro Ro Ro ", "");
            currentSolution = currentSolution.Replace("U U U ", "U' ");
            currentSolution = currentSolution.Replace("L L L ", "L' ");
            currentSolution = currentSolution.Replace("F F F ", "F' ");
            currentSolution = currentSolution.Replace("R R R ", "R' ");
            currentSolution = currentSolution.Replace("D D D ", "D' ");
            currentSolution = currentSolution.Replace("B B B ", "B' ");
            currentSolution = currentSolution.Replace("U' U ", "");
            currentSolution = currentSolution.Replace("U U' ", "");
            currentSolution = currentSolution.Replace("L' L ", "");
            currentSolution = currentSolution.Replace("L L' ", "");
            currentSolution = currentSolution.Replace("R' R ", "");
            currentSolution = currentSolution.Replace("R R' ", "");
            currentSolution = currentSolution.Replace("F' F ", "");
            currentSolution = currentSolution.Replace("F F' ", "");
            currentSolution = currentSolution.Replace("D' D ", "");
            currentSolution = currentSolution.Replace("D D' ", "");
            currentSolution = currentSolution.Replace("B' B ", "");
            currentSolution = currentSolution.Replace("B B' ", "");
            currentSolution = currentSolution.Replace("U U ", "U2 ");
            currentSolution = currentSolution.Replace("L L ", "L2 ");
            currentSolution = currentSolution.Replace("F F ", "F2 ");
            currentSolution = currentSolution.Replace("R R ", "R2 ");
            currentSolution = currentSolution.Replace("D D ", "D2 ");
            currentSolution = currentSolution.Replace("B B ", "B2 ");
            currentSolution.Trim(' ');
            return currentSolution;
        }

        #region Read

        private RubiksColor[,] ReadRubiksColors(RubiksCube rubiks, Color[] repClr)
        {
            //Läs in färger
            RubiksColor[,] cube = new RubiksColor[9, 6];
            CubeSide sideToRead = CubeSide.Back;
            for (int i = 0; i < 6; i++)
            {
                Color[] clrs = rubiks.GetCubeSideColors(sideToRead);
                switch (sideToRead)
                {
                    case CubeSide.Front:
                        Flip(clrs);
                        RotateClock(clrs);
                        RotateClock(clrs);
                        RotateClock(clrs);
                        sideToRead = CubeSide.Bottom;
                        break;
                    case CubeSide.Right:
                        Flip(clrs);
                        RotateClock(clrs);
                        RotateClock(clrs);
                        RotateClock(clrs);
                        sideToRead = CubeSide.Front;
                        break;
                    case CubeSide.Top:
                        Flip(clrs);
                        RotateClock(clrs);
                        RotateClock(clrs);
                        RotateClock(clrs);
                        sideToRead = CubeSide.Right;
                        break;
                    case CubeSide.Left:
                        RotateClock(clrs);
                        RotateClock(clrs);
                        RotateClock(clrs);
                        sideToRead = CubeSide.Top;
                        break;
                    case CubeSide.Back:
                        RotateClock(clrs);
                        sideToRead = CubeSide.Left;
                        break;
                    case CubeSide.Bottom:
                        RotateClock(clrs);
                        break;
                }
                for (int j = 0; j < 9; j++)
                {
                    cube[j, i] = (RubiksColor)((int)Array.FindIndex(repClr, x => x.R == clrs[j].R && x.G == clrs[j].G && x.B == clrs[j].B));
                }
            }
            return cube;
        }

        private void RotateClock(Color[] clrs)
        {
            Color[] t = clrs.Clone() as Color[];
            clrs[0] = t[6];
            clrs[1] = t[3];
            clrs[2] = t[0];
            clrs[3] = t[7];
            clrs[5] = t[1];
            clrs[6] = t[8];
            clrs[7] = t[5];
            clrs[8] = t[2];
        }

        private void Flip(Color[] clrs)
        {
            Color[] t = clrs.Clone() as Color[];
            clrs[0] = t[6];
            clrs[1] = t[7];
            clrs[2] = t[8];
            clrs[6] = t[0];
            clrs[7] = t[1];
            clrs[8] = t[2];
        }

        #endregion

        #region SolvingMethods
        private string DoMoves(string movesStr, RubiksColor[,] rubiks)
        {
            string[] moves = movesStr.Split();
            foreach (var move in moves)
            {
                if (move == "F")
                    Front(rubiks);
                else if (move == "U")
                    Up(rubiks);
                else if (move == "R")
                    Right(rubiks);
                else if (move == "L")
                    Left(rubiks);
                else if (move == "D")
                    Down(rubiks);
                else if (move == "Tu")
                    Turn(rubiks);
                else if (move == "Ro")
                    Rotate(rubiks);
                else if (move == "B")
                    Back(rubiks);
                else if (move == "B'")
                    BackPrime(rubiks);
                else if (move == "F'")
                    FrontPrime(rubiks);
                else if (move == "U'")
                    UpPrime(rubiks);
                else if (move == "R'")
                    RightPrime(rubiks);
                else if (move == "L'")
                    LeftPrime(rubiks);
                else if (move == "D'")
                    DownPrime(rubiks);
                else if (move == "D2")
                    DownDouble(rubiks);
                else if (move == "L2")
                    LeftDouble(rubiks);
                else if (move == "R2")
                    RightDouble(rubiks);
                else if (move == "F2")
                    FrontDouble(rubiks);
                else if (move == "U2")
                    UpDouble(rubiks);
                else if (move == "B2")
                    BackDouble(rubiks);
            }
            return movesStr + " ";
        }

        private string SolveCross(RubiksColor[,] rubiks)
        {
            StringBuilder crossSolve = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                RubiksColor targetTopColor = rubiks[4, 2];
                RubiksColor targetFrontColor = rubiks[4, 4];
                //4 övre kanter
                if (rubiks[7, 2] == targetFrontColor && rubiks[1, 4] == targetTopColor)
                    crossSolve.Append(DoMoves("F U' R U", rubiks));
                else if (rubiks[5, 2] == targetFrontColor && rubiks[3, 3] == targetTopColor)
                    crossSolve.Append(DoMoves("R' F'", rubiks));
                else if (rubiks[1, 2] == targetFrontColor && rubiks[7, 0] == targetTopColor)
                    crossSolve.Append(DoMoves("U R' U' F'", rubiks));
                else if (rubiks[3, 2] == targetFrontColor && rubiks[5, 1] == targetTopColor)
                    crossSolve.Append(DoMoves("L F", rubiks));
                else if (rubiks[3, 3] == targetFrontColor && rubiks[5, 2] == targetTopColor)
                    crossSolve.Append(DoMoves("U F U' F'", rubiks));
                else if (rubiks[7, 0] == targetFrontColor && rubiks[1, 2] == targetTopColor)
                    crossSolve.Append(DoMoves("U2 F U2 F'", rubiks));
                else if (rubiks[5, 1] == targetFrontColor && rubiks[3, 2] == targetTopColor)
                    crossSolve.Append(DoMoves("U' F U F'", rubiks));
                //4 mitten kanter
                else if (rubiks[5, 4] == targetFrontColor && rubiks[7, 3] == targetTopColor)
                    crossSolve.Append( DoMoves("F'", rubiks));
                else if (rubiks[7, 3] == targetFrontColor && rubiks[5, 4] == targetTopColor)
                    crossSolve.Append( DoMoves("U' R U", rubiks));
                else if (rubiks[3, 4] == targetFrontColor && rubiks[7, 1] == targetTopColor)
                    crossSolve.Append( DoMoves("F", rubiks));
                else if (rubiks[7, 1] == targetFrontColor && rubiks[3, 4] == targetTopColor)
                    crossSolve.Append( DoMoves("U L' U'", rubiks));
                else if (rubiks[1, 1] == targetFrontColor && rubiks[3, 0] == targetTopColor)
                    crossSolve.Append( DoMoves("U L U'", rubiks));
                else if (rubiks[3, 0] == targetFrontColor && rubiks[1, 1] == targetTopColor)
                    crossSolve.Append( DoMoves("U2 B' U2", rubiks));
                else if (rubiks[5, 0] == targetFrontColor && rubiks[1, 3] == targetTopColor)
                    crossSolve.Append( DoMoves("U2 B U2", rubiks));
                else if (rubiks[1, 3] == targetFrontColor && rubiks[5, 0] == targetTopColor)
                    crossSolve.Append( DoMoves("U' R' U", rubiks));
                //4 undre kanter
                else if (rubiks[7, 4] == targetFrontColor && rubiks[1, 5] == targetTopColor)
                    crossSolve.Append( DoMoves("F2", rubiks));
                else if (rubiks[1, 5] == targetFrontColor && rubiks[7, 4] == targetTopColor)
                    crossSolve.Append(DoMoves("F U L' U'", rubiks));
                else if (rubiks[5, 3] == targetFrontColor && rubiks[5, 5] == targetTopColor)
                    crossSolve.Append( DoMoves("U' R2 U", rubiks));
                else if (rubiks[5, 5] == targetFrontColor && rubiks[5, 3] == targetTopColor)
                    crossSolve.Append( DoMoves("U' R U F'", rubiks));
                else if (rubiks[3, 1] == targetFrontColor && rubiks[3, 5] == targetTopColor)
                    crossSolve.Append( DoMoves("U L2 U'", rubiks));
                else if (rubiks[3, 5] == targetFrontColor && rubiks[3, 1] == targetTopColor)
                    crossSolve.Append( DoMoves("U L' U' F", rubiks));
                else if (rubiks[1, 0] == targetFrontColor && rubiks[7, 5] == targetTopColor)
                    crossSolve.Append( DoMoves("D2 F2", rubiks));
                else if (rubiks[7, 5] == targetFrontColor && rubiks[1, 0] == targetTopColor)
                    crossSolve.Append( DoMoves("D U L' U' F", rubiks));

                if (rubiks[7, 2] == targetTopColor && rubiks[1, 4] == targetFrontColor)
                {
                    if (i != 3)
                    {
                        crossSolve.Append(DoMoves("Ro", rubiks));
                    }
                }
                else
                {
                    throw new RubiksCubeUnsolvableException();
                }
            }
            crossSolve.Append(" ");
            return crossSolve.ToString();
        }

        private string SolveFirstLayer(RubiksColor[,] rubiks)
        {
            StringBuilder firstLayerSolve = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                RubiksColor targetTopColor = rubiks[4, 2];
                RubiksColor targetRightColor = rubiks[4, 3];
                RubiksColor targetFrontColor = rubiks[4, 4];
                if (rubiks[2, 2] == targetTopColor && rubiks[8, 0] == targetRightColor && rubiks[0, 3] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("R D2 R' D", rubiks));
                else if (rubiks[0, 3] == targetTopColor && rubiks[2, 2] == targetRightColor && rubiks[8, 0] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("R D2 R' D", rubiks));
                else if (rubiks[8, 0] == targetTopColor && rubiks[0, 3] == targetRightColor && rubiks[2, 2] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("R D2 R' D", rubiks));

                else if (rubiks[2, 4] == targetTopColor && rubiks[8, 2] == targetRightColor && rubiks[6, 3] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("R' D' R D", rubiks));
                else if (rubiks[6, 3] == targetTopColor && rubiks[2, 4] == targetRightColor && rubiks[8, 2] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("R' D' R D", rubiks));

                else if (rubiks[0, 2] == targetTopColor && rubiks[2, 1] == targetRightColor && rubiks[6, 0] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("L' D2 L", rubiks));
                else if (rubiks[2, 1] == targetTopColor && rubiks[6, 0] == targetRightColor && rubiks[0, 2] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("L' D2 L", rubiks));
                else if (rubiks[6, 0] == targetTopColor && rubiks[0, 2] == targetRightColor && rubiks[2, 1] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("L' D2 L", rubiks));


                else if (rubiks[8, 1] == targetTopColor && rubiks[6, 2] == targetRightColor && rubiks[0, 4] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("L D L'", rubiks));
                else if (rubiks[0, 4] == targetTopColor && rubiks[8, 1] == targetRightColor && rubiks[6, 2] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("L D L'", rubiks));
                else if (rubiks[6, 2] == targetTopColor && rubiks[0, 4] == targetRightColor && rubiks[8, 1] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("L D L'", rubiks));

                else if (rubiks[6, 1] == targetTopColor && rubiks[6, 4] == targetRightColor && rubiks[0, 5] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("D", rubiks));
                else if (rubiks[0, 5] == targetTopColor && rubiks[6, 1] == targetRightColor && rubiks[6, 4] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("D", rubiks));
                else if (rubiks[6, 4] == targetTopColor && rubiks[0, 5] == targetRightColor && rubiks[6, 1] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("D", rubiks));

                else if (rubiks[0, 1] == targetTopColor && rubiks[6, 5] == targetRightColor && rubiks[0, 0] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("D2", rubiks));
                else if (rubiks[6, 5] == targetTopColor && rubiks[0, 0] == targetRightColor && rubiks[0, 1] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("D2", rubiks));
                else if (rubiks[0, 0] == targetTopColor && rubiks[0, 1] == targetRightColor && rubiks[6, 5] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("D2", rubiks));

                else if (rubiks[2, 0] == targetTopColor && rubiks[8, 5] == targetRightColor && rubiks[2, 3] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("D'", rubiks));
                else if (rubiks[8, 5] == targetTopColor && rubiks[2, 3] == targetRightColor && rubiks[2, 0] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("D'", rubiks));
                else if (rubiks[2, 3] == targetTopColor && rubiks[2, 0] == targetRightColor && rubiks[8, 5] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("D'", rubiks));


                if (rubiks[8, 4] == targetTopColor && rubiks[8, 3] == targetRightColor && rubiks[2, 5] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("F D F'", rubiks));
                else if (rubiks[8, 3] == targetTopColor && rubiks[2, 5] == targetRightColor && rubiks[8, 4] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("R' D' R", rubiks));
                else if (rubiks[2, 5] == targetTopColor && rubiks[8, 4] == targetRightColor && rubiks[8, 3] == targetFrontColor)
                    firstLayerSolve.Append(DoMoves("R' D2 R D R' D' R", rubiks));

                if (rubiks[8, 2] == targetTopColor && rubiks[6, 3] == targetRightColor && rubiks[2, 4] == targetFrontColor)
                {
                    if (i != 3)
                    {
                        firstLayerSolve.Append(DoMoves("Ro", rubiks));
                    }
                }
                else
                {
                    Console.WriteLine(firstLayerSolve.ToString());
                    throw new RubiksCubeUnsolvableException();
                }
            }
            firstLayerSolve.Append(" ");
            return firstLayerSolve.ToString();
        }

        private string SolveSecondLayer(RubiksColor[,] rubiks)
        {
            StringBuilder secondLayerSolve = new StringBuilder();
            secondLayerSolve.Append(DoMoves("Tu Tu", rubiks));
            for (int i = 0; i < 4; i++)
            {
                RubiksColor targetFrontColor = rubiks[4, 4];
                RubiksColor targetRightColor = rubiks[4, 3];

                if (rubiks[1, 4] == targetFrontColor && rubiks[7, 2] == targetRightColor)
                    secondLayerSolve.Append(DoMoves("U R U' R' U' F' U F Ro", rubiks));
                else if (rubiks[7, 2] == targetFrontColor && rubiks[1, 4] == targetRightColor)
                    secondLayerSolve.Append(DoMoves("U' Ro U' L' U L U F U' F'", rubiks));

                else if (rubiks[3, 3] == targetFrontColor && rubiks[5, 2] == targetRightColor)
                    secondLayerSolve.Append(DoMoves("U U R U' R' U' F' U F Ro", rubiks));
                else if (rubiks[5, 2] == targetFrontColor && rubiks[3, 3] == targetRightColor)
                    secondLayerSolve.Append(DoMoves("Ro U' L' U L U F U' F'", rubiks));

                else if (rubiks[7, 0] == targetFrontColor && rubiks[1, 2] == targetRightColor)
                    secondLayerSolve.Append(DoMoves("U2 U R U' R' U' F' U F Ro", rubiks));
                else if (rubiks[1, 2] == targetFrontColor && rubiks[7, 0] == targetRightColor)
                    secondLayerSolve.Append(DoMoves("U Ro U' L' U L U F U' F'", rubiks));

                else if (rubiks[5, 1] == targetFrontColor && rubiks[3, 2] == targetRightColor)
                    secondLayerSolve.Append(DoMoves("R U' R' U' F' U F Ro", rubiks));
                else if (rubiks[3, 2] == targetFrontColor && rubiks[5, 1] == targetRightColor)
                    secondLayerSolve.Append(DoMoves("U2 Ro U' L' U L U F U' F'", rubiks));

                else if (rubiks[7, 3] == targetFrontColor && rubiks[5, 4] == targetRightColor)
                    secondLayerSolve.Append(DoMoves("R U' R' U' F' U F U' R U' R' U' F' U F Ro", rubiks));

                else if (rubiks[3, 4] == targetFrontColor && rubiks[7, 1] == targetRightColor)
                    secondLayerSolve.Append(DoMoves("U' L' U L U F U' F' U Ro U' L' U L U F U' F'", rubiks));
                else if (rubiks[7, 1] == targetFrontColor && rubiks[3, 4] == targetRightColor)
                    secondLayerSolve.Append(DoMoves("U' L' U L U F U' F' U2 U R U' R' U' F' U F Ro", rubiks));

                else if (rubiks[3, 0] == targetFrontColor && rubiks[1, 1] == targetRightColor)
                    secondLayerSolve.Append(DoMoves("Ro Ro Ro U' L' U L U F U' F' Ro U U R U' R' U' F' U F Ro", rubiks));
                else if (rubiks[1, 1] == targetFrontColor && rubiks[3, 0] == targetRightColor)
                    secondLayerSolve.Append(DoMoves("Ro Ro Ro U' L' U L U F U' F' Ro Ro U' L' U L U F U' F'", rubiks));

                else if (rubiks[5, 0] == targetFrontColor && rubiks[1, 3] == targetRightColor)
                    secondLayerSolve.Append(DoMoves("Ro R U' R' U' F' U F Ro Ro Ro R U' R' U' F' U F Ro", rubiks));
                else if (rubiks[1, 3] == targetFrontColor && rubiks[5, 0] == targetRightColor)
                    secondLayerSolve.Append(DoMoves("Ro R U' R' U' F' U F U2 U' L' U L U F U' F'", rubiks));
                else
                    secondLayerSolve.Append(DoMoves("Ro", rubiks));

                if (rubiks[3, 4] != targetRightColor || rubiks[7, 1] != targetFrontColor)
                {
                    Console.WriteLine(secondLayerSolve.ToString());
                    throw new RubiksCubeUnsolvableException();
                }
            }
            secondLayerSolve.Append(" ");
            return secondLayerSolve.ToString();
        }

        private string SolveTopCross(RubiksColor[,] rubiks)
        {
            StringBuilder topCrossSolve = new StringBuilder();
            RubiksColor topTargetColor = rubiks[4, 2];
            for (int i = 0; i < 4; i++)
            {
                int[] top = new int[9];
                for (int j = 0; j < 9; j++)
                {
                    if (rubiks[j, 2] == topTargetColor && new int[] { 1, 3, 4, 5, 7 }.Contains(j))
                        top[j] = 1;
                    else
                        top[j] = 0;
                }

                if (top.SequenceEqual(new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 0 }))
                    topCrossSolve.Append(DoMoves("F R U R' U' F' U2 F  R U R' U' R U R' U' F'", rubiks));
                else if (top.SequenceEqual(new int[] { 0, 1, 0, 1, 1, 0, 0, 0, 0 }))
                    topCrossSolve.Append(DoMoves("F R U R' U' R U R' U' F'", rubiks));
                else if (top.SequenceEqual(new int[] { 0, 0, 0, 1, 1, 1, 0, 0, 0 }))
                    topCrossSolve.Append(DoMoves("F R U R' U' F'", rubiks));
                else if (top.SequenceEqual(new int[] { 0, 1, 0, 1, 1, 1, 0, 1, 0 }))
                    break;
                else
                {
                    topCrossSolve.Append(DoMoves("Ro", rubiks));
                    continue;
                }
                break;
            }
            if (new RubiksColor[] { rubiks[1, 2], rubiks[3, 2], rubiks[5, 2], rubiks[7, 2] }.Any(x => x != topTargetColor))
            {
                throw new RubiksCubeUnsolvableException();
            }
            return topCrossSolve.ToString();
        }

        private string SolveTopEdges(RubiksColor[,] rubiks)
        {
            StringBuilder topEdgesSolve = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                RubiksColor targetFront = rubiks[4, 4];
                if (rubiks[5, 1] == targetFront)
                    topEdgesSolve.Append(DoMoves("R U R' U R U2 R' U", rubiks));
                else if (rubiks[3, 3] == targetFront)
                    topEdgesSolve.Append(DoMoves("Ro R U R' U R U2 R' U", rubiks));
                else if (rubiks[7, 0] == targetFront)
                    topEdgesSolve.Append(DoMoves("R U R' U R U2 R' U Ro Ro Ro R U R' U R U2 R' U Ro R U R' U R U2 R' U ", rubiks));
                topEdgesSolve.Append(DoMoves("Ro", rubiks));
            }
            if (rubiks[1, 4] != rubiks[4, 4] || rubiks[5, 1] != rubiks[4, 1] || rubiks[7, 0] != rubiks[4, 0] || rubiks[3, 3] != rubiks[4, 3])
            {
                throw new RubiksCubeUnsolvableException();
            }
            topEdgesSolve.Append(" ");
            return topEdgesSolve.ToString();
        }

        private string SolvePermutateCorners(RubiksColor[,] rubiks)
        {
            StringBuilder permutateCornersSolve = new StringBuilder();
            RubiksColor targetFront = rubiks[4, 4];
            RubiksColor targetTop = rubiks[4, 2];
            RubiksColor targetRight = rubiks[4, 3];
            bool correctCornerFound = false;
            int tests = 0;
            while (!correctCornerFound)
            {
                correctCornerFound = true;
                for (int i = 0; i < 4; i++)
                {
                    targetFront = rubiks[4, 4];
                    targetTop = rubiks[4, 2];
                    targetRight = rubiks[4, 3];
                    if (rubiks[8, 2] == targetTop && rubiks[6, 3] == targetRight && rubiks[2, 4] == targetFront)
                        break;
                    else if (rubiks[6, 3] == targetTop && rubiks[2, 4] == targetRight && rubiks[8, 2] == targetFront)
                        break;
                    else if (rubiks[2, 4] == targetTop && rubiks[8, 2] == targetRight && rubiks[6, 3] == targetFront)
                        break;
                    if (i == 3)
                    {
                        correctCornerFound = false;
                        permutateCornersSolve.Append(DoMoves("U R U' L' U R' U' L", rubiks));
                        break;
                    }
                    permutateCornersSolve.Append(DoMoves("Ro", rubiks));
                }
                if (tests > 3)
                {
                    throw new RubiksCubeUnsolvableException();
                }
                tests++;
            }
            
            RubiksColor targetLeft = rubiks[4, 1];
            int ggr = 0;
            while (!(new RubiksColor[] { rubiks[6, 2], rubiks[8, 1], rubiks[0, 4] }.Contains(targetFront) && new RubiksColor[] { rubiks[6, 2], rubiks[8, 1], rubiks[0, 4] }.Contains(targetLeft)))
            {
                permutateCornersSolve.Append(DoMoves("U R U' L' U R' U' L", rubiks));
                if (ggr > 1)
                {
                    throw new RubiksCubeUnsolvableException();
                }
                ggr++;
            }
            permutateCornersSolve.Append(" ");
            return permutateCornersSolve.ToString();
        }

        private string SolveOrientCorners(RubiksColor[,] rubiks)
        {
            StringBuilder orientCornersSolve = new StringBuilder();
            RubiksColor targetTop = rubiks[4, 2];
            for (int i = 0; i < 4; i++)
            {
                int rotation = 0;
                while (rubiks[8, 2] != targetTop)
                {
                    orientCornersSolve.Append(DoMoves("R' D' R D", rubiks));
                    if (rotation > 4)
                    {
                        throw new RubiksCubeUnsolvableException();
                    }
                    rotation++;
                }
                orientCornersSolve.Append(DoMoves("U", rubiks));
            }
            while (rubiks[1, 4] != rubiks[4, 4])
                orientCornersSolve.Append(DoMoves("U", rubiks));
            return orientCornersSolve.ToString();
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
