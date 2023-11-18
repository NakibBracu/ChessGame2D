using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame2D.AllPieces
{
    internal class Knight : Piece
    {
        private static Knight instance = null;

        // Private constructor to prevent external instantiation
        private Knight()
        {
            // Constructor code here
        }

        // Public static method to get the single instance of the Board class
        public static Knight GetInstance()
        {
            if (instance == null)
            {
                instance = new Knight();
            }

            return instance;
        }
        public List<(int, int)> GenerateMoves(char[,] board, int i, int j)
        {
            List<(int, int)> moves = new List<(int, int)>();

            // Get the piece color
            bool isWhite = char.IsUpper(board[i, j]);

            // Check all possible knight moves
            int[] dx = { 1, 1, -1, -1, 2, 2, -2, -2 };
            int[] dy = { 2, -2, 2, -2, 1, -1, 1, -1 };
            for (int d = 0; d < 8; d++)
            {
                int x = i + dx[d];
                int y = j + dy[d];
                if (x >= 0 && x < 8 && y >= 0 && y < 8 && (board[x, y] == '\u0020' || isWhite != char.IsUpper(board[x, y])))
                {
                    moves.Add((x, y));
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
            int rowDist = Math.Abs(newRow - Row);
            int colDist = Math.Abs(newCol - Col);
            return (rowDist == 2 && colDist == 1) || (rowDist == 1 && colDist == 2);
        }
    }
}
