using System;

namespace Lab2PA
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                ChessBoard startBoard = new ChessBoard("size of the table", 4, 25);
                startBoard.ShowBoard();
                Console.WriteLine($"There are {startBoard.CountConflicts()} conflicts on a board");

                Node root = new Node(startBoard);
                SearchAssistant searchAssistant = SearchAssistant.ChooseAlgorithm(root, (int)startBoard.GetSize());
                Result result = searchAssistant.GetResult();
                StatsAssistant statsAssistant = searchAssistant.GetStats();
                Console.WriteLine($"\nTime taken: {statsAssistant.stopwatch.ElapsedMilliseconds} ms");
                statsAssistant.ViewStats();

                if (!result.GetCutOff() && !result.GetFailed())
                {
                    Console.WriteLine("Solution was found succesfully");
                    ChessBoard endBoard = searchAssistant.GetBoard();
                    endBoard.ShowBoard();
                    Console.WriteLine($"There are {endBoard.CountConflicts()} conflicts on a board");
                }
                else if (result.GetCutOff())
                {
                    Console.WriteLine("The search was cut off");
                }
                else
                {
                    Console.WriteLine("The search was failed");
                }
            }

        }
    }
}