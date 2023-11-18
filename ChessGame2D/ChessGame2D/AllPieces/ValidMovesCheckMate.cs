using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChessGame2D.AllPieces.Piece;

namespace ChessGame2D.AllPieces
{
    public class ValidMovesCheckMate
    {
        private static ValidMovesCheckMate instance = null;

        // Private constructor to prevent external instantiation
        private ValidMovesCheckMate()
        {
            // Constructor code here
        }

        // Public static method to get the single instance of the Pawn class
        public static ValidMovesCheckMate GetInstance()
        {
            if (instance == null)
            {
                instance = new ValidMovesCheckMate();
            }

            return instance;
        }
        public int GetValidMovesCount(bool isWhite, char[,] board)
        {
            int validMoves = 0;
            int pieceMask = isWhite ? (int)PieceType.OnlyWhite : (int)PieceType.OnlyBlack;

            // Loop through all pieces of the specified color
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    char piece = board[row, col];

                    if (piece == '\u0020') continue; // Skip empty squares

                    if (((int)GetPieceType(piece) & pieceMask) == 0) continue; // Skip pieces of the wrong color

                    // Check valid moves for this piece type
                    switch (piece)
                    {
                        case '\u265F':
                        case '\u2659':
                            int direction = isWhite ? -1 : 1; // Determine the direction of movement for the pawn
                            int startRow = isWhite ? 6 : 1; // Determine the starting row for the pawn
                            row = startRow + direction; // Determine the next row to check

                            // Check the space directly in front of the pawn
                            if (board[row, col] == '\u0020')
                            {
                                validMoves++;

                                // Check the space two squares in front of the pawn (if the pawn is in its starting position)
                                if (row == startRow + direction && board[row + direction, col] == '\u0020')
                                {
                                    validMoves++;
                                }
                            }

                            // Check diagonal spaces for capturing
                            if (col > 0 && board[row, col - 1] != '\u0020' && ((int)GetPieceType(board[row, col - 1]) & pieceMask) == 0)
                            {
                                validMoves++;
                            }

                            if (col < 7 && board[row, col + 1] != '\u0020' && ((int)GetPieceType(board[row, col + 1]) & pieceMask) == 0)
                            {
                                validMoves++;
                            }
                            break;
                        case '\u265E':
                        case '\u2658':
                            int[][] knightMoves = new int[][] { new int[] { -1, -2 }, new int[] { -2, -1 }, new int[] { -2, 1 }, new int[] { -1, 2 }, new int[] { 1, 2 }, new int[] { 2, 1 }, new int[] { 2, -1 }, new int[] { 1, -2 } };
                            for (int i = 0; i < knightMoves.Length; i++)
                            {
                                int[] move = knightMoves[i];
                                int newRow = row + move[0];
                                int newCol = col + move[1];
                                if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
                                {
                                    char target = board[newRow, newCol];
                                    if (target == '\u0020' || ((int)GetPieceType(target) & pieceMask) == 0)
                                    {
                                        validMoves++;
                                    }
                                }
                            }
                            break;
                        case '\u265D':
                        case '\u2657':
                            for (int i = 1; i < 8; i++)
                            {
                                if (row + i > 7 || col + i > 7) break; // Out of bounds
                                if (board[row + i, col + i] == '\u0020')
                                {
                                    // Empty square, valid move
                                    validMoves++;
                                }
                                else if (((int)GetPieceType(board[row + i, col + i]) & pieceMask) != 0)
                                {
                                    // Piece of the same color, cannot move further in this direction
                                    break;
                                }
                                else
                                {
                                    // Opponent's piece, valid move
                                    validMoves++;
                                    break;
                                }
                            }

                            for (int i = 1; i < 8; i++)
                            {
                                if (row - i < 0 || col + i > 7) break; // Out of bounds
                                if (board[row - i, col + i] == '\u0020')
                                {
                                    // Empty square, valid move
                                    validMoves++;
                                }
                                else if (((int)GetPieceType(board[row - i, col + i]) & pieceMask) != 0)
                                {
                                    // Piece of the same color, cannot move further in this direction
                                    break;
                                }
                                else
                                {
                                    // Opponent's piece, valid move
                                    validMoves++;
                                    break;
                                }
                            }

                            for (int i = 1; i < 8; i++)
                            {
                                if (row + i > 7 || col - i < 0) break; // Out of bounds
                                if (board[row + i, col - i] == '\u0020')
                                {
                                    // Empty square, valid move
                                    validMoves++;
                                }
                                else if (((int)GetPieceType(board[row + i, col - i]) & pieceMask) != 0)
                                {
                                    // Piece of the same color, cannot move further in this direction
                                    break;
                                }
                                else
                                {
                                    // Opponent's piece, valid move
                                    validMoves++;
                                    break;
                                }
                            }

                            for (int i = 1; i < 8; i++)
                            {
                                if (row - i < 0 || col - i < 0) break; // Out of bounds
                                if (board[row - i, col - i] == '\u0020')
                                {
                                    // Empty square, valid move
                                    validMoves++;
                                }
                                else if (((int)GetPieceType(board[row - i, col - i]) & pieceMask) != 0)
                                {
                                    // Piece of the same color, cannot move further in this direction
                                    break;
                                }
                                else
                                {
                                    // Opponent's piece, valid move
                                    validMoves++;
                                    break;
                                }
                            }
                            break;
                        case '\u265C':
                        case '\u2656':
                            // Rook movement logic
                            for (int i = row + 1; i < 8; i++) // Check valid moves to the right
                            {
                                char square = board[i, col];
                                if (square == '\u0020') // If the square is empty, it is a valid move
                                {
                                    validMoves++;
                                }
                                else if (((int)GetPieceType(square) & pieceMask) != 0) // If the square contains a piece of the same color, the rook cannot move any further in this direction
                                {
                                    break;
                                }
                                else // If the square contains a piece of the opposite color, the rook can capture the piece and then cannot move any further in this direction
                                {
                                    validMoves++;
                                    break;
                                }
                            }
                            for (int i = row - 1; i >= 0; i--) // Check valid moves to the left
                            {
                                char square = board[i, col];
                                if (square == '\u0020') // If the square is empty, it is a valid move
                                {
                                    validMoves++;
                                }
                                else if (((int)GetPieceType(square) & pieceMask) != 0) // If the square contains a piece of the same color, the rook cannot move any further in this direction
                                {
                                    break;
                                }
                                else // If the square contains a piece of the opposite color, the rook can capture the piece and then cannot move any further in this direction
                                {
                                    validMoves++;
                                    break;
                                }
                            }
                            for (int j = col + 1; j < 8; j++) // Check valid moves upwards
                            {
                                char square = board[row, j];
                                if (square == '\u0020') // If the square is empty, it is a valid move
                                {
                                    validMoves++;
                                }
                                else if (((int)GetPieceType(square) & pieceMask) != 0) // If the square contains a piece of the same color, the rook cannot move any further in this direction
                                {
                                    break;
                                }
                                else // If the square contains a piece of the opposite color, the rook can capture the piece and then cannot move any further in this direction
                                {
                                    validMoves++;
                                    break;
                                }
                            }
                            for (int j = col - 1; j >= 0; j--) // Check valid moves downwards
                            {
                                char square = board[row, j];
                                if (square == '\u0020') // If the square is empty, it is a valid move
                                {
                                    validMoves++;
                                }
                                else if (((int)GetPieceType(square) & pieceMask) != 0) // If the square contains a piece of the same color, the rook cannot move any further in this direction
                                {
                                    break;
                                }
                                else // If the square contains a piece of the opposite color, the rook can capture the piece and then cannot move any further in this direction
                                {
                                    validMoves++;
                                    break;
                                }
                            }
                            break;
                        case '\u265B':
                        case '\u2655':
                        case '\u265A':
                        case '\u2654':
                            // Queen or king
                            for (int i = -1; i <= 1; i++)
                            {
                                for (int j = -1; j <= 1; j++)
                                {
                                    if (i == 0 && j == 0) continue; // Skip the current position

                                    int x = row + i;
                                    int y = col + j;

                                    while (x >= 0 && x < 8 && y >= 0 && y < 8)
                                    {
                                        char targetPiece = board[x, y];

                                        if (targetPiece == '\u0020')
                                        {
                                            validMoves++;
                                        }
                                        else
                                        {
                                            if (((int)GetPieceType(targetPiece) & pieceMask) == 0)
                                            {
                                                // Target piece is of the opposite color, add move and stop checking this direction
                                                validMoves++;
                                            }
                                            break; // Target piece is of the same color or the opposite color, stop checking this direction
                                        }

                                        x += i;
                                        y += j;
                                    }
                                }
                            }
                            break;
                    }
                }
            }

            return validMoves;
        }
        private PieceType GetPieceType(char piece)
        {
            return piece switch
            {
                '\u265F' => PieceType.BlackPawn,
                '\u265E' => PieceType.BlackKnight,
                '\u265D' => PieceType.BlackBishop,
                '\u265C' => PieceType.BlackRook,
                '\u265B' => PieceType.BlackQueen,
                '\u265A' => PieceType.BlackKing,
                '\u2659' => PieceType.WhitePawn,
                '\u2658' => PieceType.WhiteKnight,
                '\u2657' => PieceType.WhiteBishop,
                '\u2656' => PieceType.WhiteRook,
                '\u2655' => PieceType.WhiteQueen,
                '\u2654' => PieceType.WhiteKing,
                _ => PieceType.Empty,
            };
        }

        public bool isCheckMate(bool isWhite)
        {
            var board = Board.GetInstance().getBoard();
            if (King.GetInstance().FindCheckResult(isWhite))
            {
                // Check if there are any valid moves for the king, if there is valid moves exited , then we should return false
                if (King.GetInstance().GetValidMovesCount(board))
                {
                    return false;
                }

                // Check if there are any valid moves for other pieces that can block or capture the attacking piece
                List<(int, int, int, int)> attackingMoves = King.GetInstance().GetAttackingMoves(isWhite, board);
                foreach ((int, int, int, int) move in attackingMoves)
                {
                    if (King.GetInstance().CanBlockOrCaptureAttacker(move.Item1, move.Item2, isWhite))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                // Check if there are not enough pieces on the board to force a checkmate
                var pieceCount = Board.GetInstance().GetPieces(board, isWhite);

                // Check if there are any valid moves for the current player
                if (ValidMovesCheckMate.GetInstance().GetValidMovesCount(isWhite, board) == 0)
                {
                    return true;
                }


                var numberOfPiece = pieceCount.Count();

                if (numberOfPiece <= 2)
                {
                    return true;
                }
                else if (numberOfPiece == 3)
                {
                    // Check if the remaining pieces are king and bishop/knight
                    char[,] tempBoard = (char[,])board.Clone();
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            char piece = tempBoard[i, j];
                            if (piece != ' ' && piece != '\u2654' && piece != '\u265A')
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                }

                return false;
            }
        }
    }
}
