using System;
using System.Collections.Generic;

namespace Tetris
{
    /// <summary>
    /// Represents a type of Tetris piece
    /// </summary>
    public enum PieceType { None, I, O, T, S, Z, L, J }

    /// <summary>
    /// Represents a square of the game area, identified by its X and Y co-ordinates
    /// </summary>
    public struct Square
    {
        public int Y;
        public int X;

        public Square(int y, int x)
        {
            this.Y = y;
            this.X = x;
        }
    }

    /// <summary>
    /// Represents a Tetris piece
    /// </summary>
    public class TetrisPiece
    {
        private static Random random = new Random();
        private static readonly int dim = 4;
        private List<Square> _occupiedSquares;
        public List<Square> OccupiedSquares
        {
            get
            {
                if (SquaresModified)
                {
                    return _occupiedSquares;
                }
                else
                {
                    return GetOccupiedSquares(Rotation);
                }
            }
            set
            {
                _occupiedSquares = value;
            }
        }
        private bool SquaresModified = false;

        public PieceType Type { get; private set; }
        int Y;
        int X;
        int Rotation;

        public TetrisPiece(PieceType type, int y = 0, int x = 4)
        {
            this.Type = type;
            this.Y = y;
            this.X = x;
        }

        public static TetrisPiece GetRandomPiece()
        {
            PieceType type = (PieceType)(random.Next(7) + 1);
            return new TetrisPiece(type);
        }

        private List<Square> GetOccupiedSquares(int rotation)
        {
            bool[,] matrix = GetPieceMatrix(this.Type);
            matrix = RotatePieceMatrix(matrix, rotation);
            List<Square> squares = new List<Square>();
            for (int y = 0; y < dim; y++)
            {
                for (int x = 0; x < dim; x++)
                {
                    if (matrix[y, x])
                    {
                        squares.Add(new Square(this.Y + y, this.X + x));
                    }
                }
            }
            return squares;
        }

        private bool[,] GetPieceMatrix(PieceType type)
        {
            switch (type)
            {
                case PieceType.I:
                    return new bool[,] { { false, true, false, false },
                                         { false, true, false, false },
                                         { false, true, false, false },
                                         { false, true, false, false } };
                case PieceType.O:
                    return new bool[,] { { false, false, false, false },
                                         { false, true, true, false },
                                         { false, true, true, false },
                                         { false, false, false, false } };
                case PieceType.T:
                    return new bool[,] { { false, false, false, false },
                                         { false, true, true, true },
                                         { false, false, true, false },
                                         { false, false, false, false } };
                case PieceType.S:
                    return new bool[,] { { false, false, false, false },
                                         { false, false, true, true },
                                         { false, true, true, false },
                                         { false, false, false, false } };
                case PieceType.Z:
                    return new bool[,] { { false, false, false, false },
                                         { false, true, true, false },
                                         { false, false, true, true },
                                         { false, false, false, false } };
                case PieceType.L:
                    return new bool[,] { { false, true, false, false },
                                         { false, true, false, false },
                                         { false, true, true, false },
                                         { false, false, false, false } };
                case PieceType.J:
                    return new bool[,] { { false, false, true, false },
                                         { false, false, true, false },
                                         { false, true, true, false },
                                         { false, false, false, false } };
                default:
                    return new bool[,] { { false, false, false, false },
                                         { false, false, false, false },
                                         { false, false, false, false },
                                         { false, false, false, false } };
            }
        }
        private bool[,] RotatePieceMatrix(bool[,] matrix, int Rotation)
        {
            for (int rot = 0; rot < Rotation; rot++)
            {
                bool[,] newMatrix = new bool[dim, dim];
                for (int y = 0; y < dim; y++)
                {
                    for (int x = 0; x < dim; x++)
                    {
                        newMatrix[y, x] = matrix[dim - x - 1, y];
                    }
                }
                matrix = newMatrix;
            }
            return matrix;
        }

        public void MoveDown()
        {
            Y += 1;
        }

        public void MoveLeft()
        {
            X -= 1;
        }

        public void MoveRight()
        {
            X += 1;
        }

        public void Rotate()
        {
            Rotation = (Rotation + 1) % 4;
        }

        public List<Square> SimulateRotation()
        {
            return GetOccupiedSquares(Rotation + 1);
        }

        public void EraseRow(int y)
        {
            List<Square> squares = new List<Square>();
            foreach (Square point in OccupiedSquares)
            {
                if (point.Y < y)
                {
                    squares.Add(new Square(point.Y + 1, point.X));
                }
                else if (point.Y > y)
                {
                    squares.Add(point);
                }
            }
            OccupiedSquares = squares;
            SquaresModified = true;
        }
    }
}
