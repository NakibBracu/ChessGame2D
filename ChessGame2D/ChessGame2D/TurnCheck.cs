using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame2D
{
    public class TurnCheck
    {
        private static TurnCheck instance = null;

        // Private constructor to prevent external instantiation
        private TurnCheck()
        {
            // Constructor code here
        }

        // Public static method to get the single instance of the Board class
        public static TurnCheck GetInstance()
        {
            if (instance == null)
            {
                instance = new TurnCheck();
            }

            return instance;
        }

        bool isFirstMove = true; //This will help to start with any pieces can move first.
        bool isWhiteTurn = true; //This will help to track which player's turn it is
        internal bool ValidTurn(int existingRow, int existingColumn, bool isValidMove)
        {
            var board = Board.GetInstance().getBoard();
            char chessPieceGet = Board.GetInstance().GetPiece(board, existingRow, existingColumn);
            bool isWhite = chessPieceGet >= '\u2654' && chessPieceGet <= '\u2659';
            bool isBlack = chessPieceGet >= '\u265A' && chessPieceGet <= '\u265F';

            if (isFirstMove && isWhite)
            {
                isFirstMove = false;
                isWhiteTurn = false; // Only allow black to move next
                return true;
            }

            if (isValidMove && ((isWhite && isWhiteTurn) || (isBlack && !isWhiteTurn)))
            {
                // Allow the player to make a move
                // ...

                // Toggle the turn to the other player
                isWhiteTurn = !isWhiteTurn;
                return true;
            }
            Console.WriteLine("Invalid move. Please try again.");
            return false;
        }
    }
}
