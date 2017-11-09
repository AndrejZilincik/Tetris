using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Tetris;

namespace TetrisTests
{
    [TestClass]
    public class PieceTests
    {
        [TestMethod]
        public void IPieceTest()
        {
            TetrisPiece piece = new TetrisPiece(PieceType.I);
            List<Square> squares = piece.OccupiedSquares;
            Assert.AreEqual(4, squares.Count);
            Assert.IsTrue(squares.Any(s => s.Y == 0 && s.X == 5));
            Assert.IsTrue(squares.Any(s => s.Y == 1 && s.X == 5));
            Assert.IsTrue(squares.Any(s => s.Y == 2 && s.X == 5));
            Assert.IsTrue(squares.Any(s => s.Y == 3 && s.X == 5));
        }

        [TestMethod]
        public void OPieceTest()
        {
            TetrisPiece piece = new TetrisPiece(PieceType.O);
            List<Square> squares = piece.OccupiedSquares;
            Assert.AreEqual(4, squares.Count);
            Assert.IsTrue(squares.Any(s => s.Y == 1 && s.X == 5));
            Assert.IsTrue(squares.Any(s => s.Y == 2 && s.X == 5));
            Assert.IsTrue(squares.Any(s => s.Y == 1 && s.X == 6));
            Assert.IsTrue(squares.Any(s => s.Y == 2 && s.X == 6));
        }

        [TestMethod]
        public void TPieceTest()
        {
            TetrisPiece piece = new TetrisPiece(PieceType.T);
            List<Square> squares = piece.OccupiedSquares;
            Assert.AreEqual(4, squares.Count);
            Assert.IsTrue(squares.Any(s => s.Y == 1 && s.X == 5));
            Assert.IsTrue(squares.Any(s => s.Y == 1 && s.X == 6));
            Assert.IsTrue(squares.Any(s => s.Y == 1 && s.X == 7));
            Assert.IsTrue(squares.Any(s => s.Y == 2 && s.X == 6));
        }

        [TestMethod]
        public void TRotateTest()
        {
            TetrisPiece piece = new TetrisPiece(PieceType.T);

            piece.Rotate();
            List<Square> squares = piece.OccupiedSquares;
            Assert.AreEqual(4, squares.Count);
            Assert.IsTrue(squares.Any(s => s.Y == 1 && s.X == 6));
            Assert.IsTrue(squares.Any(s => s.Y == 2 && s.X == 5));
            Assert.IsTrue(squares.Any(s => s.Y == 2 && s.X == 6));
            Assert.IsTrue(squares.Any(s => s.Y == 3 && s.X == 6));

            piece.Rotate();
            squares = piece.OccupiedSquares;
            Assert.AreEqual(4, squares.Count);
            Assert.IsTrue(squares.Any(s => s.Y == 1 && s.X == 5));
            Assert.IsTrue(squares.Any(s => s.Y == 2 && s.X == 4));
            Assert.IsTrue(squares.Any(s => s.Y == 2 && s.X == 5));
            Assert.IsTrue(squares.Any(s => s.Y == 2 && s.X == 6));

            piece.Rotate();
            squares = piece.OccupiedSquares;
            Assert.AreEqual(4, squares.Count);
            Assert.IsTrue(squares.Any(s => s.Y == 0 && s.X == 5));
            Assert.IsTrue(squares.Any(s => s.Y == 1 && s.X == 5));
            Assert.IsTrue(squares.Any(s => s.Y == 1 && s.X == 6));
            Assert.IsTrue(squares.Any(s => s.Y == 2 && s.X == 5));
        }

        [TestMethod]
        public void IEraseTest()
        {
            TetrisPiece piece = new TetrisPiece(PieceType.I);

            piece.EraseRow(4);
            List<Square> squares = piece.OccupiedSquares;
            Assert.AreEqual(4, squares.Count);
            Assert.IsTrue(squares.Any(s => s.Y == 1 && s.X == 5));
            Assert.IsTrue(squares.Any(s => s.Y == 2 && s.X == 5));
            Assert.IsTrue(squares.Any(s => s.Y == 3 && s.X == 5));
            Assert.IsTrue(squares.Any(s => s.Y == 4 && s.X == 5));

            piece.EraseRow(4);
            squares = piece.OccupiedSquares;
            Assert.AreEqual(3, squares.Count);
            Assert.IsTrue(squares.Any(s => s.Y == 2 && s.X == 5));
            Assert.IsTrue(squares.Any(s => s.Y == 3 && s.X == 5));
            Assert.IsTrue(squares.Any(s => s.Y == 4 && s.X == 5));

            piece.EraseRow(4);
            squares = piece.OccupiedSquares;
            Assert.AreEqual(2, squares.Count);
            Assert.IsTrue(squares.Any(s => s.Y == 3 && s.X == 5));
            Assert.IsTrue(squares.Any(s => s.Y == 4 && s.X == 5));

            piece.EraseRow(4);
            squares = piece.OccupiedSquares;
            Assert.AreEqual(1, squares.Count);
            Assert.IsTrue(squares.Any(s => s.Y == 4 && s.X == 5));

            piece.EraseRow(4);
            squares = piece.OccupiedSquares;
            Assert.AreEqual(0, squares.Count);
        }

        [TestMethod]
        public void IRotateEraseTest()
        {
            TetrisPiece piece = new TetrisPiece(PieceType.I);
            piece.Rotate();

            piece.EraseRow(2);
            List<Square> squares = piece.OccupiedSquares;
            Assert.AreEqual(4, squares.Count);
            Assert.IsTrue(squares.Any(s => s.Y == 2 && s.X == 4));
            Assert.IsTrue(squares.Any(s => s.Y == 2 && s.X == 5));
            Assert.IsTrue(squares.Any(s => s.Y == 2 && s.X == 6));
            Assert.IsTrue(squares.Any(s => s.Y == 2 && s.X == 7));

            piece.EraseRow(2);
            squares = piece.OccupiedSquares;
            Assert.AreEqual(0, squares.Count);
        }
    }
}
