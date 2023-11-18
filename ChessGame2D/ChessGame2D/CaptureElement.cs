using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChessGame2D.AllPieces.Piece;

namespace ChessGame2D
{
    public class CaptureElement
    {
        private static CaptureElement instance = null;

        // Private constructor to prevent external instantiation
        private CaptureElement()
        {
            // Constructor code here
        }

        // Public static method to get the single instance of the Board class
        public static CaptureElement GetInstance()
        {
            if (instance == null)
            {
                instance = new CaptureElement();
            }

            return instance;
        }
        internal bool IsCaptured(int existingRow, int existingColumn, int newRow, int newCol)
        {
            var board = Board.GetInstance().getBoard();
            var existingPiece = Board.GetInstance().GetPiece(board, existingRow, existingColumn);
            var newPiece = Board.GetInstance().GetPiece(board, newRow, newCol);
            var existingPieceType = Board.GetInstance().DetectChessPieceType(board, existingRow, existingColumn);
            var newPieceType = Board.GetInstance().DetectChessPieceType(board, newRow, newCol);

            if (existingPieceType == PieceType.Empty || newPieceType == PieceType.Empty)
            {
                return false; // no capture if either piece is empty
            }

            if (existingPieceType == newPieceType)
            {
                return false; // no capture if both pieces are of the same type
            }

            if ((existingPieceType & PieceType.OnlyWhite) != 0 && (newPieceType & PieceType.OnlyWhite) != 0)
            {
                return false; // no capture if both pieces are white
            }

            if ((existingPieceType & PieceType.OnlyBlack) != 0 && (newPieceType & PieceType.OnlyBlack) != 0)
            {
                return false; // no capture if both pieces are black
            }

            if ((existingPieceType & PieceType.OnlyWhite) != 0 && (newPieceType & PieceType.OnlyBlack) != 0)
            {
                return true; // white piece captures black piece
            }

            if ((existingPieceType & PieceType.OnlyBlack) != 0 && (newPieceType & PieceType.OnlyWhite) != 0)
            {
                return true; // black piece captures white piece
            }

            return false; // no capture if pieces are of the same color
        }
    }
}
