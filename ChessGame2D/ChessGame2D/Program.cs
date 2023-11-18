using ChessGame2D;
using static ChessGame2D.AllPieces.Piece;

Console.OutputEncoding = System.Text.Encoding.Unicode; // set the console encoding to Unicode
                                                       //printAllChessPieces();
                                                       //printChessBoard();
                                                       //printAllChessPiecesInBoard();

//int row = int.Parse(Console.ReadLine());
//int col = int.Parse(Console.ReadLine());


//moveApiece("knight", row, col);
Board board1 = Board.GetInstance();
board1.createBoard();
board1.CreatecellName();

while (true)
{

    Console.WriteLine("The current Chessboard Is " + "\n");
    board1.printBoard();
    Console.WriteLine();
    board1.printCellName();
    Console.WriteLine("Please give the cellName from the chessboard you want to move");
    string inputCell = Console.ReadLine();
    while (!board1.isValidCell(inputCell))
    {
        Console.WriteLine("Please Give a valid input cell like a1,b2,c2 etc.");
        inputCell = Console.ReadLine();
    }
    //Console.WriteLine(board1.showIndex(inputCell).Item1);
    //Lets detect the chess Piece in the board
    var full_Chess_Board = board1.getBoard();
    var existing_Row = board1.showIndex(inputCell).Item1;
    var existing_Column = board1.showIndex(inputCell).Item2;
    Console.WriteLine(board1.DetectChessPieceType(full_Chess_Board, existing_Row, existing_Column));
    Console.WriteLine("Please give the destination cellName from the chessboard you want to reach");
    string OutputCell = Console.ReadLine();
    while (!board1.isValidCell(OutputCell))
    {
        Console.WriteLine("Please Give a valid input cell like a1,b2,c2 etc.");
        OutputCell = Console.ReadLine();
    }
    //Console.WriteLine(board1.showIndex(OutputCell).Item1);
    var destiNation_Row = board1.showIndex(OutputCell).Item1;
    var destiNation_Column = board1.showIndex(OutputCell).Item2;
    Console.WriteLine(board1.DetectChessPieceType(full_Chess_Board, destiNation_Row, destiNation_Column));
    PieceType piece = board1.DetectChessPieceType(full_Chess_Board, existing_Row, existing_Column);
    //Console.WriteLine(board1.IsValidMove(piece, board1.showIndex(inputCell).Item1, board1.showIndex(inputCell).Item2, board1.showIndex(OutputCell).Item1, board1.showIndex(OutputCell).Item2));
    board1.DoMove(piece, existing_Row, existing_Column, destiNation_Row, destiNation_Column);
}

