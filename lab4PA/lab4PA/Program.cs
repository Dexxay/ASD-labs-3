using System.Diagnostics;

namespace lab4PA;

class Program
{
    public static void Main(string[] args)
    {
        const int vertexCount = 150; 

        Console.WriteLine("Welcome to my 4th lab! (9 variant)");
        Graph graph = new Graph(new int[vertexCount, vertexCount]);
        Console.Write("Is adjacency matrix validate? - ");
        if (graph.IsAdjMatrixValid())
            Console.WriteLine("Yes! :)");
        else
            Console.WriteLine("No :'(");

        graph.ShowMatrix();
        Console.WriteLine("Degrees of graph: ");
        graph.ShowArr(graph.GetDegreesArr());

        Console.WriteLine("Training is started successfully, stand by...");
        Stopwatch sw = Stopwatch.StartNew();

        graph = new ABCAlgorithm(graph).TrainAlgorithm();
        sw.Stop();
        Console.WriteLine($"Estimated time of training: {sw.ElapsedMilliseconds/1000}s");


        Console.WriteLine("Final colored graph: ");
        graph.ShowArr(graph.GetColorsArr());

        Console.Write($"Is graph colored properly? - ");
        if (graph.IsGraphValidColored())
            Console.WriteLine("Yes! :)");
        else
            Console.WriteLine("No :'(");
    }
}