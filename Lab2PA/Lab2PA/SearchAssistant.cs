using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2PA
{
    abstract class SearchAssistant
    {
        protected Result result;
        public Result GetResult()
        {
            return result;
        }

        protected StatsAssistant statsAssistant;
        public StatsAssistant GetStats()
        {
            return statsAssistant;
        }

        public static SearchAssistant ChooseAlgorithm(Node node, int limit)
        {
            Console.Write("Choose algorithm: (0 - LDFS; 1 - RBFS) ");
            while (true)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out int number) && number == 0)
                {
                    return new LDFS(node, limit);
                }
                else if(number == 1)
                {
                    return new RBFS(node);
                }
                else
                {
                    Console.Write("Wrong input. Try again: ");
                }
            }
        }
        public ChessBoard GetBoard()
        {
            return result.GetNode().GetState();
        }
    }
}
