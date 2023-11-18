using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame2D.AllPieces
{
    public abstract class Piece
    {
        public PieceType Type { get; protected set; } // PieceType field




        public enum PieceType
        {
            Empty = 0,
            WhitePawn = 1,
            WhiteKnight = 2,
            WhiteBishop = 4,
            WhiteRook = 8,
            WhiteQueen = 16,
            WhiteKing = 32,
            BlackPawn = 64,
            BlackKnight = 128,
            BlackBishop = 256,
            BlackRook = 512,
            BlackQueen = 1024,
            BlackKing = 2048,
            OnlyWhite = WhitePawn | WhiteKnight | WhiteBishop | WhiteRook | WhiteQueen | WhiteKing,
            OnlyBlack = BlackPawn | BlackKnight | BlackBishop | BlackRook | BlackQueen | BlackKing
        }



        internal bool PiecesHaveSameColor(PieceType piece1, PieceType piece2)
        {
            // Check if both pieces are white
            if ((piece1 & PieceType.OnlyWhite) != 0 && (piece2 & PieceType.OnlyWhite) != 0)
            {
                return true;
            }
            // Check if both pieces are black
            else if ((piece1 & PieceType.OnlyBlack) != 0 && (piece2 & PieceType.OnlyBlack) != 0)
            {
                return true;
            }
            // Pieces have different colors
            else
            {
                return false;
            }
        }
        internal (int, int) FindPiece(char piece, char[,] board)
        {
            int numRows = board.GetLength(0);
            int numCols = board.GetLength(1);

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    if (board[i, j] == piece)
                    {
                        return (i, j);
                    }
                }
            }

            return (-1, -1);
        }

        public static bool IsOnlyWhiteOrBlack(PieceType piece, out bool isWhite)
        {
            if ((piece & PieceType.OnlyWhite) == piece)
            {
                isWhite = true;
                return true;
            }
            else if ((piece & PieceType.OnlyBlack) == piece)
            {
                isWhite = false;
                return true;
            }
            else
            {
                isWhite = false;
                return false;
            }
        }






        internal bool IsMoveValid(int startRow, int startCol, int endRow, int endCol, PieceType pieceType)
        {
            var board = Board.GetInstance().getBoard();

            var dx = Math.Abs(endCol - startCol);
            var dy = Math.Abs(endRow - startRow);

            switch (pieceType)
            {
                case PieceType.WhitePawn:
                    // White pawns can only move forward
                    if (endRow <= startRow)
                    {
                        return false;
                    }
                    // Pawns can only move one or two squares forward on their first move
                    if (startRow == 1 && endRow == 3 && startCol == endCol && board[2, startCol] == null)
                    {
                        return true;
                    }
                    if (endRow - startRow != 1 || dx > 1)
                    {
                        return false;
                    }
                    // Pawns can only move diagonally if capturing an enemy piece
                    if (dx == 1 && board[endRow, endCol] == null)
                    {
                        return false;
                    }
                    return true;
                case PieceType.BlackPawn:
                    // Black pawns can only move backwards
                    if (endRow >= startRow)
                    {
                        return false;
                    }
                    // Pawns can only move one or two squares forward on their first move
                    if (startRow == 6 && endRow == 4 && startCol == endCol && board[5, startCol] == null)
                    {
                        return true;
                    }
                    if (startRow - endRow != 1 || dx > 1)
                    {
                        return false;
                    }
                    // Pawns can only move diagonally if capturing an enemy piece
                    if (dx == 1 && board[endRow, endCol] == null)
                    {
                        return false;
                    }
                    return true;
                case PieceType.WhiteKnight:
                case PieceType.BlackKnight:
                    // Knights can move in an L-shape: two squares in one direction and one square in the other
                    return (dx == 2 && dy == 1) || (dx == 1 && dy == 2);
                case PieceType.WhiteBishop:
                case PieceType.BlackBishop:
                    // Bishops can move diagonally
                    return dx == dy && IsPathClear(startRow, startCol, endRow, endCol);
                case PieceType.WhiteRook:
                case PieceType.BlackRook:
                    // Rooks can move horizontally or vertically
                    return (dx == 0 || dy == 0) && IsPathClear(startRow, startCol, endRow, endCol);
                case PieceType.WhiteQueen:
                case PieceType.BlackQueen:
                    // Queens can move like a bishop or a rook
                    return (dx == 0 || dy == 0 || dx == dy) && IsPathClear(startRow, startCol, endRow, endCol);
                case PieceType.WhiteKing:
                case PieceType.BlackKing:
                    // Kings can move one square in any direction
                    return dx <= 1 && dy <= 1;
                default:
                    return false;
            }
        }

        internal List<(int row, int col)> GetPath(int row, int col, int attackingRow, int attackingCol)
        {
            var path = new List<(int row, int col)>();

            var dx = Math.Sign(attackingCol - col);
            var dy = Math.Sign(attackingRow - row);
            var x = col + dx;
            var y = row + dy;

            while (x != attackingCol || y != attackingRow)
            {
                path.Add((y, x));
                x += dx;
                y += dy;
            }

            return path;
        }

        internal bool IsPathClear(int startRow, int startCol, int endRow, int endCol)
        {
            var board = Board.GetInstance().getBoard();

            // Check if moving horizontally
            if (startRow == endRow)
            {
                int step = Math.Sign(endCol - startCol);
                for (int col = startCol + step; col != endCol; col += step)
                {
                    if (board[startRow, col] != null)
                    {
                        return false;
                    }
                }
            }
            // Check if moving vertically
            else if (startCol == endCol)
            {
                int step = Math.Sign(endRow - startRow);
                for (int row = startRow + step; row != endRow; row += step)
                {
                    if (board[row, startCol] != null)
                    {
                        return false;
                    }
                }
            }
            // Check if moving diagonally
            else if (Math.Abs(endRow - startRow) == Math.Abs(endCol - startCol))
            {
                int rowStep = Math.Sign(endRow - startRow);
                int colStep = Math.Sign(endCol - startCol);
                for (int row = startRow + rowStep, col = startCol + colStep;
                     row != endRow && col != endCol;
                     row += rowStep, col += colStep)
                {
                    if (board[row, col] != null)
                    {
                        return false;
                    }
                }
            }
            // If not moving along a straight line, then there is no path to check
            else
            {
                return true;
            }

            // If we made it here, then the path is clear
            return true;
        }

        public abstract bool IsValidMove(PieceType piece, int Row, int Col, int newRow, int newCol);
    }
}
