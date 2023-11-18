using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame2D.AllPieces
{
    internal class Pawn : Piece
    {
        private static Pawn instance = null;

        // Private constructor to prevent external instantiation
        private Pawn()
        {
            // Constructor code here
        }

        // Public static method to get the single instance of the Pawn class
        public static Pawn GetInstance()
        {
            if (instance == null)
            {
                instance = new Pawn();
            }

            return instance;
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
            int rowDiff = newRow - Row;
            int colDiff = Math.Abs(newCol - Col);
            int forwardDir = piece == PieceType.WhitePawn ? -1 : 1;
            return colDiff == 0 && (rowDiff == forwardDir || (rowDiff == 2 * forwardDir && Row == (piece == PieceType.WhitePawn ? 6 : 1))) ||
                colDiff == 1 && rowDiff == forwardDir;
        }
        internal List<(int, int)> GenerateMovesForPawn(char[,] board, int i, int j)
        {
            List<(int, int)> moves = new List<(int, int)>();

            // Get the piece color
            bool isWhite = char.IsUpper(board[i, j]);

            // Determine the direction to move
            int direction = isWhite ? -1 : 1;

            // Check if there is a pawn in front of the current pawn
            int nextRow = i + direction;
            if (nextRow >= 0 && nextRow < 8 && board[nextRow, j] == '\u0020')
            {
                moves.Add((nextRow, j));

                // Check if the pawn can move two squares forward from the starting position
                if ((isWhite && i == 6) || (!isWhite && i == 1))
                {
                    int nextNextRow = i + 2 * direction;
                    if (board[nextNextRow, j] == '\u0020')
                    {
                        moves.Add((nextNextRow, j));
                    }
                }
            }

            // Check if the pawn can capture an opponent's piece diagonally
            if (i + direction >= 0 && i + direction < 8)
            {
                if (j - 1 >= 0 && board[i + direction, j - 1] != '\u0020' && isWhite != char.IsUpper(board[i + direction, j - 1]))
                {
                    moves.Add((i + direction, j - 1));
                }

                if (j + 1 < 8 && board[i + direction, j + 1] != '\u0020' && isWhite != char.IsUpper(board[i + direction, j + 1]))
                {
                    moves.Add((i + direction, j + 1));
                }
            }

            return moves;
        }

    }
}
