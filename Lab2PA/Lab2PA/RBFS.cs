using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2PA
{
    internal class RBFS : SearchAssistant
    {
        public RBFS(Node node)
        {
            statsAssistant = new StatsAssistant();
            statsAssistant.stopwatch.Start();
            result = RecursiveBestFirstSearch(node);
            statsAssistant.stopwatch.Stop();
        }

        private static Result RecursiveBestFirstSearch(Node node)
        {
            int limit = int.MaxValue;
            int depthOfRecursion = 0;
            return RecursiveBFS(node, limit, depthOfRecursion);
        }

        private static Result RecursiveBFS(Node node, int limit, int depth)
        {
            // to avoid StackOverflow exception
            int MAXDEPTH = (node.state.GetSize() * node.state.GetSize() - 1);
            if (node.state.CountConflicts() == 0)
            {
                return new Result(node, false, false);
            }

            if (depth > MAXDEPTH)
            {
                return new Result(null, false, true);
            }

            Node.Expand(node);

            if (node.successors.Count == 0)
            {
                return new Result(null, true, false);
            }

            List<int> f = new List<int>();

            foreach (var s in node.successors)
            {
                //f.Add(Math.Max(s.state.CountConflicts() + s.depth, s.state.CountConflicts())); (for test)
                f.Add(s.state.CountConflicts());
            }

            while (true)
            {
                int bestValue = f.Min();
                int bestIndex = f.IndexOf(bestValue);
                Node bestNode = node.successors[bestIndex];

                if (bestValue > limit)
                {
                    return new Result(null, true, false);
                }

                f.Remove(bestValue);
                int alternative = f.Min();
                limit = Math.Min(limit, alternative);


                Result result = RecursiveBFS(bestNode, limit, depth + 1);
                if (result != null && result.GetNode() != null)
                {
                    return result;
                }

                return new Result(null, true, false);
            }
        }
    }
}