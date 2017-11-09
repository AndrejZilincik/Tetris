using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Threading;

namespace Tetris
{
    /// <summary>
    /// Represents an action which can be performed while playing a game of Tetris
    /// </summary>
    public enum TetrisAction { None, MoveLeft, MoveRight, Rotate, Drop }

    /// <summary>
    /// Represents a game of Tetris
    /// </summary>
    public class TetrisGame
    {
        /// <summary>
        /// Width of the game area in squares
        /// </summary>
        int Width;

        /// <summary>
        /// Height of the game area in squares
        /// </summary>
        int Height;

        /// <summary>
        /// List of pieces which are fixed in place
        /// </summary>
        public List<TetrisPiece> FixedPieces { get; private set; }

        /// <summary>
        /// The piece which is currently moving
        /// </summary>
        public TetrisPiece MovingPiece { get; private set; }

        /// <summary>
        /// The next piece in line to become the moving piece
        /// </summary>
        public TetrisPiece FuturePiece { get; private set; }

        /// <summary>
        /// Contains the squares occupied by the moving piece before its last move
        /// </summary>
        public List<Square> PrevPosition { get; private set; }

        /// <summary>
        /// List of rows which were erased during the last tick
        /// </summary>
        public List<int> ErasedRows = new List<int>(4);

        /// <summary>
        /// Allows the game to keep track of time
        /// </summary>
        public DispatcherTimer Timer { get; private set; }

        /// <summary>
        /// The total number of rows cleared
        /// </summary>
        public int Score { get; private set; }

        public TetrisGame(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            FixedPieces = new List<TetrisPiece>();
            InitialiseTimer();
        }

        /// <summary>
        /// Initialises the game's tick timer
        /// </summary>
        private void InitialiseTimer()
        {
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(300);
            Timer.Tick += (s, e) => TickUpdate();
        }

        /// <summary>
        /// Starts a new game of Tetris
        /// </summary>
        public void StartGame()
        {
            FixedPieces.Clear();
            MovingPiece = TetrisPiece.GetRandomPiece();
            FuturePiece = TetrisPiece.GetRandomPiece();
            Score = 0;
            Timer.Start();
        }

        /// <summary>
        /// Called on every tick of the timer, updates the state of the game
        /// </summary>
        private void TickUpdate()
        {
            PrevPosition = MovingPiece.OccupiedSquares;
            ErasedRows.Clear();
            MovePieceDown();
        }

        /// <summary>
        /// Performs the specified action
        /// </summary>
        public void DoAction(TetrisAction action)
        {
            PrevPosition = MovingPiece.OccupiedSquares;
            switch (action)
            {
                case TetrisAction.MoveLeft:
                    MovePieceLeft();
                    break;
                case TetrisAction.MoveRight:
                    MovePieceRight();
                    break;
                case TetrisAction.Drop:
                    TickUpdate();
                    break;
                case TetrisAction.Rotate:
                    RotatePiece();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Shifts the moving piece 1 square left, if possible
        /// </summary>
        private void MovePieceLeft()
        {
            List<Square> squares = MovingPiece.OccupiedSquares;

            // Can't move out of bounds
            if (squares.Any(ms => ms.X == 0))
            {
                return;
            }
            // Can't move into an already occupied square
            else if (FixedPieces.Any(fp => fp.OccupiedSquares.Any(fs => squares.Any(ms => ms.X - 1 == fs.X && ms.Y == fs.Y))))
            {
                return;
            }

            MovingPiece.MoveLeft();
        }

        /// <summary>
        /// Shifts the moving piece 1 square right, if possible
        /// </summary>
        private void MovePieceRight()
        {
            List<Square> squares = MovingPiece.OccupiedSquares;

            // Can't move out of bounds
            if (squares.Any(ms => ms.X == Width - 1))
            {
                return;
            }
            // Can't move into an already occupied square
            else if (FixedPieces.Any(fp => fp.OccupiedSquares.Any(fs => squares.Any(ms => ms.X + 1 == fs.X && ms.Y == fs.Y))))
            {
                return;
            }

            MovingPiece.MoveRight();
        }
        
        /// <summary>
        /// Shifts the moving piece 1 square down, if possible
        /// </summary>
        private void MovePieceDown()
        {
            List<Square> squares = MovingPiece.OccupiedSquares;

            // Can't move out of bounds
            if (squares.Any(ms => ms.Y == Height - 1))
            {
                NextPiece();
                return;
            }
            // Can't move into an already occupied square
            else if (FixedPieces.Any(fp => fp.OccupiedSquares.Any(fs => squares.Any(ms => ms.X == fs.X && ms.Y + 1 == fs.Y))))
            {
                NextPiece();
                return;
            }

            MovingPiece.MoveDown();
        }
        
        /// <summary>
        /// Rotates the moving piece
        /// </summary>
        private void RotatePiece()
        {
            List<Square> afterRotation = MovingPiece.SimulateRotation();

            // Can't move out of bounds
            if (afterRotation.Any(ms => ms.X < 0) || afterRotation.Any(ms => ms.X >= Width) ||
                afterRotation.Any(ms => ms.Y < 0) || afterRotation.Any(ms => ms.Y >= Height))
            {
                return;
            }
            // Can't move into an already occupied square
            if (FixedPieces.Any(fp => fp.OccupiedSquares.Any(fs => afterRotation.Any(rs => rs.X == fs.X && rs.Y == fs.Y))))
            {
                return;
            }

            MovingPiece.Rotate();
        }

        /// <summary>
        /// Moves on to the next piece
        /// </summary>
        private void NextPiece()
        {
            PrevPosition.Clear();
            FixedPieces.Add(MovingPiece);
            MovingPiece = FuturePiece;
            FuturePiece = TetrisPiece.GetRandomPiece();

            CheckFilledRows();
        }

        /// <summary>
        /// Finds all rows that are completely filled and removes them
        /// </summary>
        private void CheckFilledRows()
        {
            for (int y = 0; y < Height; y++)
            {
                int filled = FixedPieces.Sum(fp => fp.OccupiedSquares.Where(fs => fs.Y == y).Count());
                if (filled == Width)
                {
                    // Clear row
                    foreach (TetrisPiece piece in FixedPieces)
                    {
                        piece.EraseRow(y);
                    }

                    ErasedRows.Add(y);
                    Score++;
                }
            }
        }
    }
}
