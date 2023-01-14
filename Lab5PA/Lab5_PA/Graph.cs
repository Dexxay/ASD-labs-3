using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5_PA
{
    public class Graph
    {
        private bool[,] adjMatrix;

        const int vertexCount = 300;

        const int minVertexDegree = 2;
        const int maxVertexDegree = 30;

        public Graph(Graph g)
        {
            this.adjMatrix = new bool[g.adjMatrix.GetLength(0), g.adjMatrix.GetLength(1)];
            Array.Copy(g.adjMatrix, this.adjMatrix, g.adjMatrix.Length);
        }

        public bool[,] GetMatrix()
        {
            return adjMatrix;
        }

        public Graph()
        {
            this.adjMatrix = new bool[vertexCount, vertexCount];
            Random rand = new Random();

            for (int currV = 0; currV < vertexCount; ++currV)
            {
                int finalVertexDegree = Math.Min(rand.Next(minVertexDegree, maxVertexDegree + 1)
                    - GetVertexDegree(currV), vertexCount - currV - 1);
                for (int newConnections = 0; newConnections < finalVertexDegree; ++newConnections)
                {
                    bool isConnectedAlready = true;
                    for (int tryCount = 0, newVertex = rand.Next(currV + 1, vertexCount);
                        isConnectedAlready && tryCount < vertexCount; ++tryCount, newVertex = rand.Next(currV + 1, vertexCount))
                    {
                        if (this.adjMatrix[currV, newVertex] == false && GetVertexDegree(newVertex) < maxVertexDegree)
                        {
                            isConnectedAlready = false;
                            this.adjMatrix[currV, newVertex] = true;
                            this.adjMatrix[newVertex, currV] = true;
                        }
                    }
                }
            }

        }

        public bool IsAdjMatrixValid()
        {
            for (int vertex = 0; vertex < adjMatrix.GetLength(0); vertex++)
            {
                if (GetVertexDegree(vertex) > maxVertexDegree)
                {
                    return false;
                }
            }
            return true;
        }


        public int[] GetDegreesArr()
        {
            int[] degrees = new int[adjMatrix.GetLength(0)];
            for (int i = 0; i < degrees.Length; ++i)
            {
                degrees[i] = GetVertexDegree(i);
            }
            return degrees;
        }

        public int GetVertexDegree(int vertex)
        {
            int sum = 0;
            for (int i = 0; i < adjMatrix.GetLength(0); i++)
            {
                if (adjMatrix[vertex, i])
                {
                    sum += 1;
                }
            }
            return sum;
        }


        public void ShowMatrix()
        {
            for (int i = 0; i < adjMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < adjMatrix.GetLength(1); j++)
                {
                    if (adjMatrix[i, j])
                        Console.Write(1 + " ");
                    else
                        Console.Write(0 + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void ShowArr(int[] arr)
        {
            const int maxRowLength = 10;
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i] + "\t");
                if (i + 1 % maxRowLength == 0)
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
