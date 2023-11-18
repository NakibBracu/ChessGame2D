using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame2D.AllPieces
{
    public class King : Piece
    {
        private static King instance = null;

        // Private constructor to prevent external instantiation
        private King()
        {
            // Constructor code here
        }

        // Public static method to get the single instance of the Board class
        public static King GetInstance()
        {
            if (instance == null)
            {
                instance = new King();
            }

            return instance;
        }

        internal bool FindCheckResult(bool isWhite)
        {

            return IsCheck(isWhite);
        }
        internal bool IsKingInCheckAfterMove(PieceType movingPiece, int Row, int Col, int newRow, int newCol, char[,] board)
        {
            // Simulate the move by creating a copy of the board with the piece moved to the new position
            char[,] newBoard = (char[,])board.Clone();
            //newBoard[Row, Col] = new Piece(PieceType.Empty, Row, Col);
            //Keep the old piece from newBoard old (row,column) to new (row,column)
            newBoard[newRow, newCol] = newBoard[Row, Col];

            // Find the king's position on the board
            int kingRow = -1;
            int kingCol = -1;
            PieceType kingType = movingPiece == PieceType.WhiteKing ? PieceType.WhiteKing : PieceType.BlackKing;
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (Board.GetInstance().DetectChessPieceType(newBoard, row, col) == kingType)//What we get from each cell checked with the king Type
                    {
                        kingRow = row;
                        kingCol = col;
                        break;
                    }
                }
            }
            //after find the king Type , we will go check whether opposite palyer can attack my king or not by any of the pieces that opponent have

            // Check if any of the opponent's pieces can attack the king
            PieceType attackingType = movingPiece == PieceType.WhiteKing ? PieceType.OnlyBlack : PieceType.OnlyWhite;
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    PieceType piece = Board.GetInstance().DetectChessPieceType(newBoard, row, col);
                    if (piece != PieceType.Empty && piece != kingType && piece.ToString().StartsWith(attackingType.ToString()))
                    {
                        if (IsValidMove(piece, row, col, kingRow, kingCol))
                        {
                            // The king is attacked by an opponent's piece, so the move is not valid
                            Console.WriteLine("You have a check by opponent for this move!");
                            return true;
                        }
                    }
                }
            }

            // The move is valid and does not put the king in check
            return false;
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
            int rowDiffKing = Math.Abs(newRow - Row);
            int colDiffKing = Math.Abs(newCol - Col);
            return (rowDiffKing == 1 && colDiffKing <= 1) || (rowDiffKing <= 1 && colDiffKing == 1);
        }


        internal bool GetValidMovesCount(char[,] board)
        {

            if (HasValidKingMove(board))
            {
                return true;
            }

            return false;
        }

        private bool HasValidKingMove(char[,] board)
        {
            // Loop through the board and find the current position of the king
            int fromRow = -1;
            int fromCol = -1;
            for (int row = 0; row <= 7; row++)
            {
                for (int col = 0; col <= 7; col++)
                {
                    char piece = board[row, col];
                    if (piece == '\u265A' || piece == '\u2654') // check if the piece is a king
                    {
                        fromRow = row;
                        fromCol = col;
                        break;
                    }
                }
                if (fromRow != -1)
                {
                    break;
                }
            }

            // Check if the king can move to any valid position
            for (int row = fromRow - 1; row <= fromRow + 1; row++)
            {
                for (int col = fromCol - 1; col <= fromCol + 1; col++)
                {
                    if (row >= 0 && row <= 7 && col >= 0 && col <= 7 && (row != fromRow || col != fromCol))
                    {
                        if (IsValidKingMove(board, fromRow, fromCol, row, col))
                        {
                            return true;
                        }
                    }
                }
            }

            // The king cannot move to any valid position
            return false;
        }
        private bool IsValidKingMove(char[,] board, int fromRow, int fromCol, int toRow, int toCol)
        {
            // Check if the move is within the bounds of the board
            if (toRow < 0 || toRow > 7 || toCol < 0 || toCol > 7)
            {
                return false;
            }

            // Check if the target position is empty or contains an opponent piece
            char targetPiece = board[toRow, toCol];
            if (targetPiece != '\u0020' && char.IsUpper(targetPiece))
            {
                return false;
            }

            // Check if the king can move to the target position
            char piece = board[fromRow, fromCol];
            if (piece == '\u265A' || piece == '\u2654') // check if the piece is a king
            {
                int rowDiff = Math.Abs(toRow - fromRow);
                int colDiff = Math.Abs(toCol - fromCol);
                if ((rowDiff == 1 && colDiff == 0) || // check if the move is one step vertically
                    (rowDiff == 0 && colDiff == 1) || // check if the move is one step horizontally
                    (rowDiff == 1 && colDiff == 1))  // check if the move is one step diagonally
                {
                    return true;
                }
            }

            // The move is not valid
            return false;
        }

        public List<(int, int, int, int)> GetAttackingMoves(bool isWhite, char[,] board)
        {
            // Find the position of the king in check
            char king = isWhite ? '\u2654' : '\u265A';
            (int, int) kingPos = FindPiece(king, board);
            if (kingPos == (-1, -1))
            {
                // King not found, return empty list
                return new List<(int, int, int, int)>();
            }

            // Generate all possible moves for each attacking piece
            List<(int, int, int, int)> attackingMoves = new List<(int, int, int, int)>();
            PieceType attackingPieces = isWhite ? PieceType.OnlyBlack : PieceType.OnlyWhite;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    PieceType piece = Board.GetInstance().DetectChessPieceType(board, i, j);
                    if ((piece & attackingPieces) != 0)
                    {
                        List<(int, int)> moves = GenerateMoves(board, i, j);
                        foreach ((int, int) move in moves)
                        {
                            if (move.Item1 == kingPos.Item1 && move.Item2 == kingPos.Item2)
                            {
                                attackingMoves.Add((i, j, move.Item1, move.Item2));
                            }
                        }
                    }
                }
            }

            // Generate all possible moves for each blocking piece, and check if any of these moves result in a legal position
            List<(int, int, int, int)> blockingMoves = new List<(int, int, int, int)>();
            PieceType blockingPieces = isWhite ? PieceType.OnlyWhite : PieceType.OnlyBlack;
            foreach ((int, int, int, int) attackingMove in attackingMoves)
            {
                List<(int, int)> moves = GenerateMoves(board, attackingMove.Item1, attackingMove.Item2);
                foreach ((int, int) move in moves)
                {
                    char[,] newBoard = (char[,])board.Clone();
                    newBoard[attackingMove.Item3, attackingMove.Item4] = newBoard[attackingMove.Item1, attackingMove.Item2];
                    newBoard[attackingMove.Item1, attackingMove.Item2] = '\u0020';
                    newBoard[move.Item1, move.Item2] = newBoard[attackingMove.Item3, attackingMove.Item4];
                    newBoard[attackingMove.Item3, attackingMove.Item4] = '\u0020';
                    if (!IsInCheck(isWhite, newBoard))
                    {
                        blockingMoves.Add((attackingMove.Item1, attackingMove.Item2, move.Item1, move.Item2));
                    }
                }
            }

            return blockingMoves;
        }
        public bool CanBlockOrCaptureAttacker(int attackingRow, int attackingCol, bool isDefendingWhite)
        {
            // Check if any of the defending pieces can capture the attacking piece
            List<(int, int)> capturingMoves = GetCapturingMoves(attackingRow, attackingCol, isDefendingWhite);
            if (capturingMoves.Count > 0)
            {
                return true;
            }

            // Check if any of the defending pieces can block the attack
            List<(int, int)> blockingMoves = GetBlockingMoves(attackingRow, attackingCol, isDefendingWhite);
            if (blockingMoves.Count > 0)
            {
                return true;
            }

            // If no capturing or blocking moves are found, return false
            return false;
        }
        public List<(int, int)> GetBlockingMoves(int attackingRow, int attackingCol, bool isDefendingWhite)
        {
            List<(int, int)> blockingMoves = new List<(int, int)>();
            var board = Board.GetInstance().getBoard();

            // Iterate over all the pieces on the board
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    // Check if the piece belongs to the defending player
                    var piece = Board.GetInstance().GetPiece(board, row, col);
                    var pieceType = Board.GetInstance().DetectChessPieceType(board, row, col);
                    if (IsPieceOfColor(pieceType, isDefendingWhite))
                    {
                        // Check if the piece has a valid move that can block the attack
                        if (CanBlockAttack(attackingRow, attackingCol, row, col))
                        {
                            blockingMoves.Add((row, col));
                        }
                    }
                }
            }

            return blockingMoves;
        }
        private bool IsPieceOfColor(PieceType piece, bool isWhite)
        {
            if (isWhite)
            {
                return (piece & PieceType.OnlyWhite) != 0;
            }
            else
            {
                return (piece & PieceType.OnlyBlack) != 0;
            }
        }

        private bool CanBlockAttack(int attackingRow, int attackingCol, int row, int col)
        {
            var board = Board.GetInstance().getBoard();
            var piece = Board.GetInstance().GetPiece(board, row, col);
            var _pieceType = Board.GetInstance().DetectChessPieceType(board, row, col);

            // Check if the piece can move to a square that blocks the attack
            for (int destRow = 0; destRow < 8; destRow++)
            {
                for (int destCol = 0; destCol < 8; destCol++)
                {
                    if (IsMoveValid(row, col, destRow, destCol, _pieceType))
                    {
                        // Check if the move blocks the attack
                        if (!IsAttackedByPiece(destRow, destCol, attackingRow, attackingCol))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        private bool IsAttackedByPiece(int destRow, int destCol, int attackingRow, int attackingCol)
        {
            var board = Board.GetInstance().getBoard();

            // Determine the type of piece on the attacking square
            var attackingPiece = Board.GetInstance().GetPiece(board, attackingRow, attackingCol);
            var attackingPieceType = Board.GetInstance().DetectChessPieceType(board, attackingRow, attackingCol);

            // Check if the attacking piece can attack the destination square
            if (IsMoveValid(attackingRow, attackingCol, destRow, destCol, attackingPieceType))
            {
                return true;
            }

            return false;
        }



        public List<(int, int)> GetCapturingMoves(int attackingRow, int attackingCol, bool isDefendingWhite)
        {
            // Create an empty list to store capturing moves
            List<(int, int)> capturingMoves = new List<(int, int)>();

            // Determine the color of the attacking piece based on the attacking row and column
            var pieceType = true ? Board.GetInstance().DetectChessPieceType(Board.GetInstance().getBoard(), attackingRow, attackingCol) == PieceType.OnlyWhite : false;
            bool isAttackingWhite = pieceType;

            // Get the possible capturing moves for all defending pieces
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (IsDefendingPiece(row, col, isDefendingWhite))
                    {
                        // Check if the defending piece can capture the attacking piece
                        if (CanCapture(row, col, attackingRow, attackingCol, isAttackingWhite))
                        {
                            capturingMoves.Add((row, col));
                        }
                    }
                }
            }

            // Return the list of capturing moves
            return capturingMoves;
        }
        private bool CanCapture(int row, int col, int attackingRow, int attackingCol, bool isAttackingWhite)
        {
            // Get the chess board instance
            var board = Board.GetInstance().getBoard();

            // Check if the piece at (row, col) is of the opposite color of the attacking piece
            var pieceType = true ? Board.GetInstance().DetectChessPieceType(board, row, col) == PieceType.OnlyWhite : false;
            bool isDefendingWhite = pieceType;
            if (isDefendingWhite == isAttackingWhite)
            {
                return false;
            }

            // Check if the piece at (row, col) can capture the attacking piece
            var piece = board[row, col];
            var _pieceType = Board.GetInstance().DetectChessPieceType(board, row, col);
            if (_pieceType == null)
            {
                return false;
            }

            if (_pieceType == (PieceType.WhitePawn | PieceType.BlackPawn))
            {
                // Check if the pawn can move diagonally to capture the attacking piece
                var dx = attackingCol - col;
                var dy = attackingRow - row;
                if (Math.Abs(dx) == 1 && dy == (isDefendingWhite ? 1 : -1))
                {
                    return true;
                }
            }
            else if (_pieceType == (PieceType.WhiteKnight | PieceType.BlackKnight))
            {
                // Check if the knight can capture the attacking piece
                var dx = Math.Abs(attackingCol - col);
                var dy = Math.Abs(attackingRow - row);
                if ((dx == 1 && dy == 2) || (dx == 2 && dy == 1))
                {
                    return true;
                }
            }
            else
            {
                // Check if the piece can move to capture the attacking piece
                if (IsMoveValid(row, col, attackingRow, attackingCol, _pieceType))
                {
                    // Check if there are any pieces in between the attacking piece and the defending piece
                    var path = GetPath(row, col, attackingRow, attackingCol);
                    foreach (var cell in path)
                    {
                        if (board[cell.row, cell.col] != null)
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            return false;
        }



        private bool IsDefendingPiece(int row, int col, bool isDefendingWhite)
        {
            var pieceType = Board.GetInstance().DetectChessPieceType(Board.GetInstance().getBoard(), row, col);

            if (isDefendingWhite)
            {
                // The defending player is playing white pieces
                return pieceType == PieceType.OnlyWhite;
            }
            else
            {
                // The defending player is playing black pieces
                return pieceType == PieceType.OnlyBlack;
            }
        }


        public bool IsInCheck(bool isWhite, char[,] board)
        {
            // Find the king's location
            int kingRow = -1;
            int kingCol = -1;
            PieceType kingType = isWhite ? PieceType.WhiteKing : PieceType.BlackKing;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board.GetInstance().DetectChessPieceType(board, i, j) == kingType)
                    {
                        kingRow = i;
                        kingCol = j;
                        break;
                    }
                }
            }

            // Check if any opposing piece can attack the king
            PieceType opposingPieces = isWhite ? PieceType.OnlyBlack : PieceType.OnlyWhite;
            List<(int, int)> moves;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    PieceType pieceType = Board.GetInstance().DetectChessPieceType(board, i, j);
                    if (opposingPieces.HasFlag(pieceType))
                    {
                        moves = GenerateMoves(board, i, j);
                        foreach ((int, int) move in moves)
                        {
                            if (move.Item1 == kingRow && move.Item2 == kingCol)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            // The king is not in check
            return false;
        }



        private List<(int, int)> GenerateMoves(char[,] board, int i, int j)
        {
            PieceType piece = Board.GetInstance().DetectChessPieceType(board, i, j);
            if (piece == PieceType.WhitePawn || piece == PieceType.BlackPawn)
            {
                return Pawn.GetInstance().GenerateMovesForPawn(board, i, j);
            }
            else if (piece == PieceType.BlackKing || piece == PieceType.WhiteKing)
            {
                return GenerateKingMoves(board, i, j, piece);
            }
            else if (piece == PieceType.BlackRook || piece == PieceType.WhiteRook)
            {
                return Rook.GetInstance().GenerateMoves(board, i, j);
            }
            else if (piece == PieceType.BlackBishop || piece == PieceType.WhiteBishop)
            {
                return Bishop.GetInstance().GenerateMoves(board, i, j);
            }
            else if (piece == PieceType.BlackKnight || piece == PieceType.WhiteKnight)
            {
                return Knight.GetInstance().GenerateMoves(board, i, j);
            }
            else if (piece == PieceType.WhiteQueen || piece == PieceType.BlackQueen)
            {
                return Queen.GetInstance().GenerateMoves(board, i, j);
            }
            else
            {
                return new List<(int, int)>() { (-1, -1) };
            }
        }
        public List<(int, int)> GenerateKingMoves(char[,] board, int i, int j, PieceType pieceType)
        {
            List<(int, int)> moves = new List<(int, int)>();

            // Determine the piece color
            bool isWhite = ((pieceType & PieceType.OnlyWhite) > 0);

            // Determine the opponent's piece color
            PieceType opponentPieceType = isWhite ? PieceType.OnlyBlack : PieceType.OnlyWhite;

            // Check all adjacent squares
            for (int row = i - 1; row <= i + 1; row++)
            {
                for (int col = j - 1; col <= j + 1; col++)
                {
                    // Skip the current square
                    if (row == i && col == j)
                    {
                        continue;
                    }

                    // Check if the square is on the board
                    if (row >= 0 && row < 8 && col >= 0 && col < 8)
                    {
                        // Check if the square is empty or occupied by an opponent's piece
                        if (board[row, col] == '\u0020' || ((int)opponentPieceType & (int)Char.GetNumericValue(board[row, col])) > 0)
                        {
                            moves.Add((row, col));
                        }
                    }
                }
            }

            return moves;
        }


        private bool IsCheck(bool isWhite)
        {
            var board = Board.GetInstance().getBoard();

            // Find the king of the current player
            char kingSymbol = isWhite ? '\u2654' : '\u265A';
            int kingRow = -1;
            int kingCol = -1;
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (board[row, col] == kingSymbol)
                    {
                        kingRow = row;
                        kingCol = col;
                        break;
                    }
                }
                if (kingRow >= 0)
                {
                    break;
                }
            }

            if (kingRow < 0)
            {
                // King not found, so cannot be in check
                return false;
            }

            // Check if any of the opponent's pieces can attack the king
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    char piece = board[row, col];
                    var pieceType = Board.GetInstance().DetectChessPieceType(board, row, col);
                    bool isOpponent = (isWhite && !IsWhitePiece(piece)) || (!isWhite && IsWhitePiece(piece));
                    if (isOpponent && IsValidMove(pieceType, row, col, kingRow, kingCol))
                    {
                        // This piece can attack the king, so the current player is in check
                        return true;
                    }

                    bool isCurrentPlayer = (isWhite && piece >= '\u2654' && piece <= '\u2659') || (!isWhite && piece >= '\u265A' && piece <= '\u265F');
                    if (isCurrentPlayer && IsValidMove(pieceType, row, col, kingRow, kingCol))
                    {
                        // This piece can attack the opponent's king, so the current player is giving check
                        return true;
                    }
                }
            }

            // None of the opponent's or current player's pieces can attack the king, so the current player is not in check
            return false;
        }
        public bool IsWhitePiece(char piece)
        {
            // Check if the piece is a Unicode chess piece
            if (piece < '\u2654' || piece > '\u265F')
            {

            }

            // Check if the piece is white
            return (piece >= '\u2654' && piece <= '\u2659') || (piece >= '\u265A' && piece <= '\u265F');
        }






    }
}
