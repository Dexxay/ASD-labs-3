using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2PA
{
    internal class Node
    {
        private Node parent;

        public ChessBoard state;
        public int depth;
        public List<Node> successors;

        public int GetDepth()
        {
            return depth;
        }
        public ChessBoard GetState()
        {
            return state;
        }
        public List<Node> GetSuccesors()
        {
            return successors;
        }
        public Node(ChessBoard InitialState)
        {
            parent = null;
            state = InitialState;
            depth = 0;
        }

        public Node(ref Node parentNode, int queenRow, int dest)
        {
            parent = parentNode;
            depth = parentNode.depth + 1;
            state = new ChessBoard(parentNode.state.GetSize(), parentNode.state.GetArray());
            state.MoveQueen(queenRow, (byte)dest);
        }

        public static void Expand(Node node)
        {
            int index = 0;
            node.successors = new List<Node>();

            for (int row = 0; row < node.state.GetSize(); row++)
            {
                for (int newCol = 0; newCol < node.state.GetSize(); newCol++)
                {
                    if (newCol != node.state.GetQueenCol(row))
                    {
                        node.successors.Add(new Node(ref node, row, newCol));
                    }
                }
            }
        }

    }
}