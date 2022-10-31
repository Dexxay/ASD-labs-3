using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2PA
{
    internal class StatsAssistant
    {
        public long _iterations { get; set; }

        public long _totalStates { get; set; }

        public long _statesInMemory { get; set; }

        public int _deadEnds { get; set; }
        public Stopwatch stopwatch;

        public HashSet<ChessBoard> _totalStatesHashSet { get; set; }
        public HashSet<ChessBoard> _statesInMemoryHashSet { get; set; }

        public StatsAssistant()
        {
            _iterations = 0;
            _totalStates = 0;
            _statesInMemory = 0;
            _deadEnds = 0;
            stopwatch = new Stopwatch();

            _totalStatesHashSet = new HashSet<ChessBoard>();
            _statesInMemoryHashSet = new HashSet<ChessBoard>();
        }

        public void ViewStats()
        {
            Console.WriteLine($"Iterations: {_iterations} , Deadends: {_deadEnds} , Total states: {_totalStates} ," +
               $" States in memory: {_statesInMemory} ");
        }
    }
}
