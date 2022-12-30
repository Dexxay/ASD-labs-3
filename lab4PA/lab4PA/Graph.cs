using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4PA
{
    internal class Graph
    {
        private int[,] adjMatrix;
        private int[] colorsArr;

        const int noColor = -1;
        const int vertexCount = 150;
        const int minVertexDegree = 1;
        const int maxVertexDegree = 30;

        public Graph(Graph g)
        {
            this.adjMatrix = new int[g.adjMatrix.GetLength(0), g.adjMatrix.GetLength(1)];
            this.colorsArr = new int[g.colorsArr.Length];

            Array.Copy(g.adjMatrix, this.adjMatrix, g.adjMatrix.Length);
            Array.Copy(g.colorsArr, this.colorsArr, g.colorsArr.Length);
            /*
            this.adjMatrix = g.adjMatrix;
            this.colors = g.colors;
            */
        }

        public Graph(int[,] adjMatrix)
        {
            Random rand = new Random();
            this.adjMatrix = adjMatrix;
            this.colorsArr = new int[adjMatrix.GetLength(0)];
            Array.Fill(this.colorsArr, noColor);

            for (int currV = 0; currV < vertexCount; ++currV)
            {
                int finalVertexDegree = Math.Min(rand.Next(minVertexDegree, maxVertexDegree + 1)
                    - GetVertexDegree(currV), vertexCount - currV - 1);
                for (int newConnections = 0; newConnections < finalVertexDegree; ++newConnections)
                {
                    bool isConnectedAlready = true;
                    for (int tryCount = 0, newVertex = rand.Next(currV+1, vertexCount);
                        isConnectedAlready && tryCount < vertexCount; ++tryCount, newVertex=rand.Next(currV+1,vertexCount))
                    {
                        if (this.adjMatrix[currV,newVertex] == 0 && GetVertexDegree(newVertex) < maxVertexDegree)
                        {
                            isConnectedAlready = false;
                            this.adjMatrix[currV, newVertex] = 1;
                            this.adjMatrix[newVertex, currV] = 1;
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

        public bool IsGraphValidColored()
        {
            for (int i = 0; i < colorsArr.Length; i++)
            {
                if (colorsArr[i] == noColor)
                {
                    return false;
                }
            }
            if (IsColoringValid())
            {
                return true;
            }
            return false;
        }

        public int[] GetColorsArr()
        {
            return colorsArr;
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
                sum += adjMatrix[vertex, i];
            }
            return sum;
        }

        public int[] GetConnectedVertexes(int vertex)
        {
            int[] connectedVertexes = new int[GetVertexDegree(vertex)];
            for (int i = 0, k = -1; i < adjMatrix.GetLength(0); ++i)
            {
                if (adjMatrix[vertex, i] == 1)
                {
                    connectedVertexes[++k] = i;
                }
            }
            return connectedVertexes;
        }

        public bool TryToColorIfValid(int vertex, int newColor)
        {
            int oldColor = colorsArr[vertex];
            colorsArr[vertex] = newColor;
            bool isValid = IsColoringValid();
            if (!isValid)
            {
                colorsArr[vertex] = oldColor;
            }
            return isValid;
        }

        private bool IsColoringValid()
        {
            for (int row = 0; row < adjMatrix.GetLength(0); ++row)
            {
                for (int col = 0; col < adjMatrix.GetLength(1); ++col)
                {
                    if (adjMatrix[row, col] == 1 && colorsArr[row] != noColor && colorsArr[row] == colorsArr[col])
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        public void ShowMatrix()
        {
            for (int i = 0; i < adjMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < adjMatrix.GetLength(1); j++)
                {
                    Console.Write(adjMatrix[i, j] + " ");
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

        public static List<int> GetVertexList()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < vertexCount; i++)
            {
                list.Add(i);
            }
            return list;
        }
    }
}
