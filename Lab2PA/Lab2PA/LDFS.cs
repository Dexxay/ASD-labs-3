using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2PA
{
    internal class LDFS : SearchAssistant
    {
        public LDFS(Node node, int limit)
        {
            statsAssistant = new StatsAssistant();
            statsAssistant.stopwatch.Start();

            result = DepthLimitedSearch(node, limit);
            statsAssistant.stopwatch.Stop();

            statsAssistant._statesInMemory = statsAssistant._statesInMemoryHashSet.Count;
            statsAssistant._totalStates = statsAssistant._totalStatesHashSet.Count;
        }
        private Result DepthLimitedSearch(Node node, int limit)
        {
            return RecursiveDLS(node, limit);
        }

        private Result RecursiveDLS(Node node, int limit)
        {
            statsAssistant._iterations += 1;
            bool cutoffOccured = false;

            if (statsAssistant.stopwatch.ElapsedMilliseconds >= 30 * 60 * 1000)
            {
                statsAssistant._deadEnds += 1;
                return new Result(node, false, false, true);
            }

            if (node.state.CountConflicts() == 0)
                return new Result(node, false, false, false);

            else if (node.depth == limit)
            {
                statsAssistant._deadEnds += 1;
                return new Result(null, false, true, false);
            }

            Node.Expand(node);

            foreach (var successor in node.successors)
            {
                statsAssistant._totalStatesHashSet.Add(successor.GetState());

                Result result = RecursiveDLS(successor, limit);

                if (result.GetTimeIsUp())
                    return result;

                if (result.GetCutOff())
                    cutoffOccured = true;

                if (result != null && result.GetNode() != null)
                {
                    foreach(var s in node.successors)
                        statsAssistant._statesInMemoryHashSet.Add(s.GetState());
                    return result;
                }
            }

            if (cutoffOccured)
                return new Result(null, false, true, false);

            else
                return new Result(null, true, false, false);
        }
    }

}