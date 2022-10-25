using System;
using System.Diagnostics;

namespace Lab2PA
{
    class Program
    {
        static void Main()
        {
            ChessBoard board = new ChessBoard("size of table", 4, 25);
            board.ShowBoard();
            int a = board.CountConflicts();
        }
    }
}