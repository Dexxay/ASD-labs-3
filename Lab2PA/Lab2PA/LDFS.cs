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
        }
        private static Result DepthLimitedSearch(Node node, int limit)
        {
            return RecursiveDLS(node, limit);
        }

        private static Result RecursiveDLS(Node node, int limit)
        {
            bool cutoffOccured = false;
            if (node.state.CountConflicts() == 0)
            {
                return new Result(node, false, false);
            }
            else if (node.depth == limit)
            {
                return new Result(null, false, true);
            }

            Node.Expand(node);

            foreach (var successor in node.successors)
            {
                Result result = RecursiveDLS(successor, limit);
                if (result.GetCutOff())
                    cutoffOccured = true;
                else if (!result.GetFailed())
                    return result;
            }

            if (cutoffOccured)
            {
                return new Result(null, false, true);
            }
            else
            {
                return new Result(null, true, false);
            }
        }
    }

}