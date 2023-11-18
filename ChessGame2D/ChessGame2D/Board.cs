using ChessGame2D.AllPieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChessGame2D.AllPieces.Piece;

namespace ChessGame2D
{
    public class Board : IBoard
    {
        private static Board instance = null;

        // Private constructor to prevent external instantiation
        private Board()
        {
            // Constructor code here
        }

        // Public static method to get the single instance of the Board class
        public static Board GetInstance()
        {
            if (instance == null)
            {
                instance = new Board();
            }

            return instance;
        }

        public string currentPlayer = "";
        private List<string> validcellNumbers = new List<string>();
        private char[,] board = new char[8, 8];
        private string[,] cellName = new string[8, 8];
        private char[] prefix_cell = new char[8] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
        private Piece Piece { get; set; }
        //we can get the whole PieceObject by this and maintain the points who to win
        public int blackPoint { get; set; }// pieceInfo.isWhite = false ; then Blackpoint Increase
        public int whitePoint { get; set; }// pieceInfo.isWhite = true ; then whitePoint Increase
                                           //public bool isCaptured { get; set; }


        public string winStatus
        {
            get
            {
                if (blackPoint > whitePoint)
                    return "Black Wins";
                else if (blackPoint < whitePoint)
                    return "White Wins";
                else return "Draw";
            }
        }



        public bool IsValidMove(PieceType piece, int existingRow, int existingColumn, int newRow, int newCol)
    => (newRow, newCol) switch
    {
        // Check if the new position is on the board
        var (r, c) when r < 0 || r > 7 || c < 0 || c > 7 => false,
        // Call the appropriate move validation method based on the piece type
        _ => piece switch
        {
            PieceType.WhitePawn or PieceType.BlackPawn => Pawn.GetInstance().IsValidMove(piece, existingRow, existingColumn, newRow, newCol),
            PieceType.WhiteKnight or PieceType.BlackKnight => Knight.GetInstance().IsValidMove(piece, existingRow, existingColumn, newRow, newCol),
            PieceType.WhiteBishop or PieceType.BlackBishop => Bishop.GetInstance().IsValidMove(piece, existingRow, existingColumn, newRow, newCol),
            PieceType.WhiteRook or PieceType.BlackRook => Rook.GetInstance().IsValidMove(piece, existingRow, existingColumn, newRow, newCol),
            PieceType.WhiteQueen or PieceType.BlackQueen => Queen.GetInstance().IsValidMove(piece, existingRow, existingColumn, newRow, newCol),
            PieceType.WhiteKing or PieceType.BlackKing => King.GetInstance().IsValidMove(piece, existingRow, existingColumn, newRow, newCol),
            _ => false
        }
    };
        public PieceType DetectChessPieceType(char[,] chessBoard, int row, int col)
        {
            // Check if the position is out of bounds or empty
            if (row < 0 || row >= 8 || col < 0 || col >= 8 || chessBoard[row, col] == ' ')
            {
                return PieceType.Empty;
            }
            // Determine the color of the chess piece at the given position
            bool isWhite = chessBoard[row, col] >= '\u2654' && chessBoard[row, col] <= '\u2659';
            bool isBlack = chessBoard[row, col] >= '\u265A' && chessBoard[row, col] <= '\u265F';


            // Determine the type of the chess piece based on its Unicode character and color
            return chessBoard[row, col] switch
            {
                '\u2659' => isWhite ? PieceType.WhitePawn : PieceType.BlackPawn,
                '\u2658' => isWhite ? PieceType.WhiteKnight : PieceType.BlackKnight,
                '\u2657' => isWhite ? PieceType.WhiteBishop : PieceType.BlackBishop,
                '\u2656' => isWhite ? PieceType.WhiteRook : PieceType.BlackRook,
                '\u2655' => isWhite ? PieceType.WhiteQueen : PieceType.BlackQueen,
                '\u2654' => isWhite ? PieceType.WhiteKing : PieceType.BlackKing,
                '\u265F' => isBlack ? PieceType.BlackPawn : PieceType.Empty,
                '\u265E' => isBlack ? PieceType.BlackKnight : PieceType.Empty,
                '\u265D' => isBlack ? PieceType.BlackBishop : PieceType.Empty,
                '\u265C' => isBlack ? PieceType.BlackRook : PieceType.Empty,
                '\u265B' => isBlack ? PieceType.BlackQueen : PieceType.Empty,
                '\u265A' => isBlack ? PieceType.BlackKing : PieceType.Empty,
                _ => PieceType.Empty
            };

        }

        public void DoMove(PieceType piece, int existingRow, int existingColumn, int newRow, int newCol)
        {
            bool isWhite;
            var pieceTypeOfCurrentPlayer = DetectChessPieceType(board, existingRow, existingColumn);
            isWhite = Piece.IsOnlyWhiteOrBlack(pieceTypeOfCurrentPlayer, out isWhite);
            //if (isCheckMate(isWhite))
            //{
            //    Console.WriteLine("The game is over");
            //}
            //else
            //{
            var moving_Piece_Type = DetectChessPieceType(board, existingRow, existingColumn);
            //If the move is valid then copy the character in new Cell and replace existing cell with empty
            Console.WriteLine("Capture is " + CaptureElement.GetInstance().IsCaptured(existingRow, existingColumn, newRow, newCol) + " After the Move.");

            if (moving_Piece_Type == PieceType.OnlyWhite)
            {
                currentPlayer = "White";
            }
            else
            {
                currentPlayer = "Black";
            }
            bool isValidMove = IsValidMove(piece, existingRow, existingColumn, newRow, newCol);

            Console.WriteLine("Check before movement " + King.GetInstance().FindCheckResult(isWhite));
            if (isValidMove && TurnCheck.GetInstance().ValidTurn(existingRow, existingColumn, isValidMove) &&
                !King.GetInstance().IsKingInCheckAfterMove(piece, existingRow, existingColumn, newRow, newCol, board))
            {

                char movingPiece = GetPiece(board, existingRow, existingColumn);
                board[newRow, newCol] = movingPiece;
                board[existingRow, existingColumn] = ' ';

                if (movingPiece >= '\u2654' && movingPiece <= '\u2659')
                {
                    Console.WriteLine("White moves {0} from {1} to {2}.", movingPiece, cellName[existingRow, existingColumn], cellName[newRow, newCol]);
                }
                else if (movingPiece >= '\u265A' && movingPiece <= '\u265F')
                {
                    Console.WriteLine("Black moves {0} from {1} to {2}.", movingPiece, cellName[existingRow, existingColumn], cellName[newRow, newCol]);
                }

                // printBoard(); 
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Please give a valid move!");
            }

            Console.WriteLine("Check after movement " + King.GetInstance().FindCheckResult(isWhite));
            Console.WriteLine();

        }

        internal List<char> GetPieces(char[,] board, bool isWhite)
        {
            List<char> pieces = new List<char>();
            char startPiece, endPiece;
            if (isWhite)
            {
                startPiece = '\u2654'; // White King
                endPiece = '\u265F'; // White Pawn
            }
            else
            {
                startPiece = '\u265A'; // Black King
                endPiece = '\u2659'; // Black Pawn
            }
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    char piece = board[row, col];
                    if (piece >= startPiece && piece <= endPiece && piece % 2 != 0)
                    {
                        pieces.Add(piece);
                    }
                }
            }
            return pieces;
        }
        internal List<(int, int)> GetPiecesIndexs(bool isWhite)
        {
            List<(int, int)> pieces = new List<(int, int)>();
            char startPiece, endPiece;
            if (isWhite)
            {
                startPiece = '\u2654'; // White King
                endPiece = '\u265F'; // White Pawn
            }
            else
            {
                startPiece = '\u265A'; // Black King
                endPiece = '\u2659'; // Black Pawn
            }
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    char piece = board[row, col];
                    if (piece >= startPiece && piece <= endPiece && piece % 2 != 0)
                    {
                        pieces.Add((row, col));
                    }
                }
            }
            return pieces;
        }


        public char GetPiece(char[,] board, int row, int col)
        {
            // Console.WriteLine(board[row,col]);
            return board[row, col];
        }




        public void createBoard()
        {
            //initialize the board with all pieces
            board = new char[,] {
            {'\u265C', '\u265E', '\u265D', '\u265B', '\u265A', '\u265D', '\u265E', '\u265C'},
            {'\u265F', '\u265F', '\u265F', '\u265F', '\u265F', '\u265F', '\u265F', '\u265F'},
            {'\u0020', '\u0020', '\u0020', '\u0020', '\u0020', '\u0020', '\u0020', '\u0020'},
            {'\u0020', '\u0020', '\u0020', '\u0020', '\u0020', '\u0020', '\u0020', '\u0020'},
            {'\u0020', '\u0020', '\u0020', '\u0020', '\u0020', '\u0020', '\u0020', '\u0020'},
            {'\u0020', '\u0020', '\u0020', '\u0020', '\u0020', '\u0020', '\u0020', '\u0020'},
            {'\u2659', '\u2659', '\u2659', '\u2659', '\u2659', '\u2659', '\u2659', '\u2659'},
            {'\u2656', '\u2658', '\u2657', '\u2655', '\u2654', '\u2657', '\u2658', '\u2656'}
        };

        }





        public char[,] getBoard()
        {
            //we can modify the board by getting the whole board
            return board;
        }
        public string[,] GetcellName()
        {
            //by getting cellName we can get the index of that cell and modify it through board
            return cellName;
        }

        public (int, int) showIndex(string _cellName)
        {
            //Here we will use tuple.Item1 to get the start & tuple.Item2 to get the end and use this in the valid move method.
            return FindElementIndex(cellName, _cellName.ToLower());
        }
        public (int, int) FindElementIndex<T>(T[,] arr, T element)
        {
            // As array can be char or string if we want to use cellName -> string[,]
            // But Board array is full with characters
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (EqualityComparer<T>.Default.Equals(arr[i, j], element))
                    {
                        return (i, j);
                    }
                }
            }
            return (-1, -1); // If element is not found, return -1 for both indexes
        }







        internal void CreatecellName()
        {


            for (int i = 0; i < 8; i++)
            {

                for (int j = 0; j < 8; j++)
                {
                    cellName[i, j] = prefix_cell[j].ToString() + "" + (8 - (i + 1) + 1);
                    validcellNumbers.Add(cellName[i, j]);
                }

            }

        }

        internal void printCellName()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    string piece = cellName[i, j];
                    Console.Write(" " + piece + " ");
                }
                Console.WriteLine();
            }
        }

        public void printBoard()
        {
            //Here we are colorng the board and print it.

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    char piece = board[i, j];
                    ConsoleColor background = (i + j) % 2 == 0 ? ConsoleColor.Blue : ConsoleColor.Magenta;
                    ConsoleColor foreground = piece == ('\u2800') ? ConsoleColor.Black : ConsoleColor.White;
                    Console.BackgroundColor = background;
                    Console.ForegroundColor = foreground;
                    Console.Write(" " + piece + " ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }
        internal bool isValidCell(string Inputcell)
        {
            if (!validcellNumbers.Contains(Inputcell.ToLower()))
            {
                return false;
            }
            return true;
        }
    }
}
