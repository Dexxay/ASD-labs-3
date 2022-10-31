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

            statsAssistant._statesInMemory = statsAssistant._statesInMemoryHashSet.Count;
            statsAssistant._totalStates = statsAssistant._totalStatesHashSet.Count;
        }

        private Result RecursiveBestFirstSearch(Node node)
        {
            int limit = int.MaxValue;
            int depthOfRecursion = 0;
            return RecursiveBFS(node, limit, depthOfRecursion);
        }

        private Result RecursiveBFS(Node node, int limit, int depth)
        {
            statsAssistant._iterations += 1;

            int MAXDEPTH = 16 * (node.state.GetSize() * node.state.GetSize() - 1);
            // to avoid StackOverflow exception

            if (statsAssistant.stopwatch.ElapsedMilliseconds >= 30 * 60 * 1000)
            {
                statsAssistant._deadEnds += 1;
                return new Result(null, false, false, true);
            }

            if (node.state.CountConflicts() == 0)
                return new Result(node, false, false, false);

            if (depth > MAXDEPTH)
            {
                statsAssistant._deadEnds += 1;
                return new Result(null, false, true, false);
            }

            Node.Expand(node);
            if (node.successors.Count == 0)
            {
                statsAssistant._deadEnds += 1;
                return new Result(null, true, false, false);
            }

            List<int> f = new List<int>();
            foreach (var successor in node.successors)
            {
                statsAssistant._totalStatesHashSet.Add(successor.GetState());
                f.Add(successor.state.CountConflicts());
                //f.Add(Math.Max(s.state.CountConflicts() + s.depth, s.state.CountConflicts())); (for test)
            }

            while (true)
            {
                int bestValue = f.Min();
                int bestIndex = f.IndexOf(bestValue);
                Node bestNode = node.successors[bestIndex];

                if (bestValue > limit)
                    return new Result(null, true, false, false);

                f.Remove(bestValue);
                node.successors.Remove(bestNode);

                int alternative = f.Min();
                limit = Math.Min(limit, alternative);

                Result result = RecursiveBFS(bestNode, limit, depth + 1);

                if (result.GetTimeIsUp())
                    return result;

                if (result != null && result.GetNode() != null)
                {
                    foreach (var s in node.successors)
                        statsAssistant._statesInMemoryHashSet.Add(s.GetState());
                    return result;
                }

                return new Result(null, true, false, false);
            }
        }
    }
}