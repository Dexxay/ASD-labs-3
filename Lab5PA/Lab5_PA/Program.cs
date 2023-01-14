using System.Diagnostics;

namespace Lab5_PA;

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to my 5th lab! (9 variant)");
        Graph graph = new Graph();
        Console.Write("Is adjacency matrix validate? - ");

        if (graph.IsAdjMatrixValid())
            Console.WriteLine("Yes! :)");
        else
            Console.WriteLine("No :'(");

        graph.ShowMatrix();
        Console.WriteLine("Degrees of graph: ");
        graph.ShowArr(graph.GetDegreesArr());

        BalanceAlgorithm balanceAlgorithm = new BalanceAlgorithm(graph);
    }
}