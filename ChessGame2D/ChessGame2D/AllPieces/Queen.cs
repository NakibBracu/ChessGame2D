using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame2D.AllPieces
{
    internal class Queen : Piece
    {
        private static Queen instance = null;

        // Private constructor to prevent external instantiation
        private Queen()
        {
            // Constructor code here
        }


        public static Queen GetInstance()
        {
            if (instance == null)
            {
                instance = new Queen();
            }

            return instance;
        }
        public List<(int, int)> GenerateMoves(char[,] board, int i, int j)
        {
            List<(int, int)> moves = new List<(int, int)>();

            // Get the piece color
            bool isWhite = char.IsUpper(board[i, j]);

            // Check for possible moves along each diagonal (like a bishop)
            int[] dx = { -1, -1, 1, 1 };
            int[] dy = { -1, 1, -1, 1 };
            for (int d = 0; d < 4; d++)
            {
                int x = i + dx[d];
                int y = j + dy[d];
                while (x >= 0 && x < 8 && y >= 0 && y < 8)
                {
                    if (board[x, y] == '\u0020')
                    {
                        moves.Add((x, y));
                    }
                    else if (isWhite != char.IsUpper(board[x, y]))
                    {
                        moves.Add((x, y));
                        break;
                    }
                    else
                    {
                        break;
                    }
                    x += dx[d];
                    y += dy[d];
                }
            }

            // Check for possible moves along each row/column (like a rook)
            int[] dx2 = { 0, 0, -1, 1 };
            int[] dy2 = { -1, 1, 0, 0 };
            for (int d = 0; d < 4; d++)
            {
                int x = i + dx2[d];
                int y = j + dy2[d];
                while (x >= 0 && x < 8 && y >= 0 && y < 8)
                {
                    if (board[x, y] == '\u0020')
                    {
                        moves.Add((x, y));
                    }
                    else if (isWhite != char.IsUpper(board[x, y]))
                    {
                        moves.Add((x, y));
                        break;
                    }
                    else
                    {
                        break;
                    }
                    x += dx2[d];
                    y += dy2[d];
                }
            }

            return moves;
        }

        public override bool IsValidMove(PieceType piece, int Row, int Col, int newRow, int newCol)
        {
            var board = Board.GetInstance().getBoard();
            PieceType existingPiece = Board.GetInstance().DetectChessPieceType(board, Row, Col);
            PieceType destinationPiece = Board.GetInstance().DetectChessPieceType(board, newRow, newCol);
            if (PiecesHaveSameColor(existingPiece, destinationPiece))
            {
                return false;
            }
            if (destinationPiece != PieceType.Empty && ((piece & PieceType.OnlyWhite) == (destinationPiece & PieceType.OnlyWhite)))
            {
                return false;
            }
            return Rook.GetInstance().IsValidMove(piece, Row, Col, newRow, newCol) || Bishop.GetInstance().IsValidMove(piece, Row, Col, newRow, newCol);
        }

    }
}
