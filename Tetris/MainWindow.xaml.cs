using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tetris
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Width of the game area in squares
        /// </summary>
        public readonly int Columns = 10;

        /// <summary>
        /// Height of the game area in squares
        /// </summary>
        public readonly int Rows = 20;

        /// <summary>
        /// Matrix of Rectangle UI elements that form the main display area
        /// </summary>
        Rectangle[,] Rects;
        
        /// <summary>
        /// Matrix of Rectangle UI elements where the future piece is displayed
        /// </summary>
        Rectangle[,] FutureRects;

        /// <summary>
        /// Represents a game of Tetris
        /// </summary>
        TetrisGame game;

        public MainWindow()
        {
            InitializeComponent();
            InitialiseDisplay();
            InitialiseGame();
        }

        /// <summary>
        /// Initialises the display areas
        /// </summary>
        private void InitialiseDisplay()
        {
            int size = 20;

            Rects = new Rectangle[Rows, Columns];
            for (int h = 0; h < Rows; h++)
            {
                for (int w = 0; w < Columns; w++)
                {
                    Rectangle r = new Rectangle();
                    r.Width = size;
                    r.Height = size;
                    r.Fill = Brushes.LightGray;
                    Rects[h, w] = r;

                    MainCanvas.Children.Add(r);
                    Canvas.SetLeft(r, w * (size + 1));
                    Canvas.SetTop(r, h * (size + 1));
                }
            }

            FutureRects = new Rectangle[4, 4];
            for (int h = 0; h < 4; h++)
            {
                for (int w = 0; w < 4; w++)
                {
                    Rectangle r = new Rectangle();
                    r.Width = size;
                    r.Height = size;
                    r.Fill = Brushes.LightGray;
                    FutureRects[h, w] = r;

                    FuturePiece.Children.Add(r);
                    Canvas.SetLeft(r, w * (size + 1));
                    Canvas.SetTop(r, h * (size + 1));
                }
            }
        }

        /// <summary>
        /// Initialises and starts a game of Tetris
        /// </summary>
        private void InitialiseGame()
        {
            game = new TetrisGame(Columns, Rows);
            game.StartGame();
            game.Timer.Tick += (s, e) => UpdateDisplay();
        }

        /// <summary>
        /// Updates the display areas
        /// </summary>
        private void UpdateDisplay()
        {
            EraseSquares(game.PrevPosition);
            EraseRows(game.ErasedRows);

            DrawPiece(game.MovingPiece);
            ClearFutureRects();
            DrawFuturePiece(game.FuturePiece);
            ScoreBox.Text = game.Score.ToString();
        }

        /// <summary>
        /// Returns a brush whose colour depends on the type of Tetris piece
        /// </summary>
        private SolidColorBrush GetBrush(TetrisPiece piece)
        {
            switch (piece.Type)
            {
                case PieceType.I:
                    return Brushes.OrangeRed;
                case PieceType.O:
                    return Brushes.Goldenrod;
                case PieceType.T:
                    return Brushes.SlateGray;
                case PieceType.S:
                    return Brushes.DeepSkyBlue;
                case PieceType.Z:
                    return Brushes.LimeGreen;
                case PieceType.L:
                    return Brushes.DarkBlue;
                case PieceType.J:
                    return Brushes.DarkGreen;
                default:
                    return Brushes.DarkGray;
            }
        }

        /// <summary>
        /// Draws a Tetris piece onto the main display area
        /// </summary>
        private void DrawPiece(TetrisPiece piece)
        {
            SolidColorBrush brush = GetBrush(piece);
            foreach (Square point in piece.OccupiedSquares)
            {
                Rects[point.Y, point.X].Fill = brush;
            }
        }
        
        /// <summary>
        /// Draws the future piece onto the display
        /// </summary>
        private void DrawFuturePiece(TetrisPiece piece)
        {
            SolidColorBrush brush = GetBrush(piece);
            foreach (Square point in piece.OccupiedSquares)
            {
                FutureRects[point.Y, point.X - 4].Fill = brush;
            }
        }
        
        /// <summary>
        /// Clears all of the specified squares
        /// </summary>
        private void EraseSquares(List<Square> squares)
        {
            foreach (Square point in squares)
            {
                Rects[point.Y, point.X].Fill = Brushes.LightGray;
            }
        }
        
        /// <summary>
        /// Clears a number of rows from the board, rows above the cleared area get shifted down
        /// </summary>
        private void EraseRows(List<int> rows)
        {
            if (rows.Count < 1)
            {
                return;
            }

            for (int h = rows[rows.Count - 1]; h >= 0; h--)
            {
                for (int w = 0; w < Columns; w++)
                {
                    if (h >= rows.Count)
                    {
                        Rects[h, w].Fill = Rects[h - rows.Count, w].Fill;
                    }
                    else
                    {
                        Rects[h, w].Fill = Brushes.LightGray;
                    }
                }
            }
        }

        /// <summary>
        /// Clears the area which shows the future piece
        /// </summary>
        private void ClearFutureRects()
        {
            for (int h = 0; h < 4; h++)
            {
                for (int w = 0; w < 4; w++)
                {
                    FutureRects[h, w].Fill = Brushes.LightGray;
                }
            }
        }

        /// <summary>
        /// Handles key inputs
        /// </summary>
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Left)
            {
                game.DoAction(TetrisAction.MoveLeft);
            }
            else if (e.Key == System.Windows.Input.Key.Right)
            {
                game.DoAction(TetrisAction.MoveRight);
            }
            else if (e.Key == System.Windows.Input.Key.Down)
            {
                game.DoAction(TetrisAction.Drop);
            }
            else if (e.Key == System.Windows.Input.Key.Up)
            {
                game.DoAction(TetrisAction.Rotate);
            }

            UpdateDisplay();
        }
    }
}
