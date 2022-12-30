using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4PA
{
    internal class ABCAlgorithm
    {
        private Graph initialGraph;
        private Graph graph;
        private List<int> availableVertices;
        private int[] paletteArr;
        private List<int> usedColorsList;

        const int explorerBeesCount = 3;
        const int totalBeesCount = 25;
        const int maxVertexDegree = 30;
        const int iterationsPerStep = 20;
        const int maxIterationsCount = 1000;

        public ABCAlgorithm(Graph initialGraph)
        {
            this.initialGraph = initialGraph;
            graph = new Graph(initialGraph);
            availableVertices = Graph.GetVertexList();
            paletteArr = new int[maxVertexDegree + 1];
            for (int i = 0; i < maxVertexDegree + 1; i++)
            {
                paletteArr[i] = i;
            }
            usedColorsList = new List<int>();
        }

        public Graph TrainAlgorithm()
        {
            Graph resultGraph = new Graph(graph);
            int bestChromNumber = CalculateChromNumber();
            Console.WriteLine("Init colored graph: ");
            graph.ShowArr(graph.GetColorsArr());
            Console.WriteLine($"The new best solution of the graph found on 0 iteration, old = {maxVertexDegree + 1}," +
            $" new = {bestChromNumber}, estimated time = 0 s");
            resetAlgorithm();

            for (int iterations = 0; iterations < maxIterationsCount;)
            {
                Stopwatch sw = Stopwatch.StartNew();
                for (int k = 1; k < iterationsPerStep + 1; k++, resetAlgorithm())
                {
                    int newChromNumber = CalculateChromNumber();
                    if (newChromNumber < bestChromNumber)
                    {
                        Console.WriteLine($"The new best solution of the graph found on {iterations + k} " +
                        $"iteration, old = {bestChromNumber}," + $" new = {bestChromNumber = newChromNumber}, " +
                        $"estimated time = {sw.ElapsedMilliseconds / 1000} s");
                        resultGraph = new Graph(graph);
                    }
                }
                Console.WriteLine($"Iteration {iterations += iterationsPerStep}, best result = " +
                    $"{bestChromNumber}, estimated time = {sw.ElapsedMilliseconds / 1000} s");
            }
            Console.WriteLine("Initial colors of graph are (-1 - no color): ");
            graph.ShowArr(graph.GetColorsArr());
            return resultGraph;
        }

        public void resetAlgorithm()
        {
            usedColorsList.Clear();
            availableVertices = Graph.GetVertexList();
            graph = new Graph(initialGraph);
        }

        public int CalculateChromNumber()
        {
            while (!graph.IsGraphValidColored())
            {
                List<int> selectedVertices = SendEmployeedBees();
                SendOnlookerBees(selectedVertices);
            }
            return usedColorsList.Count();
        }

        private List<int> SendEmployeedBees()
        {
            List<int> selectedVertices = new List<int>();
            for (int employeedBee = 0; employeedBee < explorerBeesCount; ++employeedBee)
            {
                if (availableVertices.Count == 0)
                {
                    continue;
                }
                int index = new Random().Next(availableVertices.Count);
                int randomSelectedVertex = availableVertices[index];
                availableVertices.RemoveAt(index);
                selectedVertices.Add(randomSelectedVertex);
            }
            return selectedVertices;
        }

        private void SendOnlookerBees(List<int> selectedVertices)
        {
            int[] selectedVerticesDegrees = new int[selectedVertices.Count];
            for (int i = 0; i < selectedVerticesDegrees.Length; ++i)
            {
                selectedVerticesDegrees[i] = graph.GetVertexDegree(selectedVertices[i]);
            }

            int[] onlookerBeesSplit = GetOnlookersBeesSplit(selectedVerticesDegrees);
            for (int i = 0; i < selectedVertices.Count; ++i)
            {
                int onlookerBeesCountForVertex = onlookerBeesSplit[i];
                int[] connectedVertices = graph.GetConnectedVertexes(selectedVertices[i]);
                ColorConnectedVertex(connectedVertices, onlookerBeesCountForVertex);
                ColorVertex(selectedVertices[i]);
            }
        }

        private int[] GetOnlookersBeesSplit(int[] selectedVerticesDegrees)
        {
            double[] nectarValues = GetNectarValues(selectedVerticesDegrees);
            int onlookerBeesCount = totalBeesCount - explorerBeesCount;
            int[] res = new int[nectarValues.Length];
            for (int i = 0; i < nectarValues.Length; ++i)
            {
                res[i] = (int)(onlookerBeesCount * nectarValues[i]);
                onlookerBeesCount -= res[i];
            }
            return res;
        }

        private double[] GetNectarValues(int[] selectedVerticesDegrees)
        {
            double[] res = new double[selectedVerticesDegrees.Length];
            for (int i = 0, totalDegrees = selectedVerticesDegrees.Sum(); i < selectedVerticesDegrees.Length; ++i)
            {
                res[i] = (double)selectedVerticesDegrees[i] / totalDegrees;
            }
            return res;
        }

        private void ColorConnectedVertex(int[] connectedVertices, int onlookerBeesCount)
        {
            for (int i = 0; i < connectedVertices.Length; ++i)
            {
                if (i < onlookerBeesCount - 1)
                {
                    ColorVertex(connectedVertices[i]);
                }
            }
        }

        private void ColorVertex(int vertex)
        {
            List<int> availableColorsList = new List<int>(usedColorsList);
            bool isColoredSuccessfully = false;
            while (!isColoredSuccessfully)
            {
                if (availableColorsList.Count == 0)
                {
                    int newColor = paletteArr[usedColorsList.Count];
                    usedColorsList.Add(newColor);
                    graph.TryToColorIfValid(vertex, newColor);
                    break;
                }

                int index = new Random().Next(availableColorsList.Count);
                int color = availableColorsList[index];
                availableColorsList.RemoveAt(index);
                isColoredSuccessfully = graph.TryToColorIfValid(vertex, color);
            }
        }

        
    }
}
