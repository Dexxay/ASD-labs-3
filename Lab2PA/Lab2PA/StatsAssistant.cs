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
        private long _iterations;
        public long GetIterations() { return _iterations; }
        public void SetIterations(long iterations) { _iterations = iterations; }

        private long _totalStates;
        public long GetTotalStates() { return _totalStates; }
        public void SetTotalStates(long totalStates) { _totalStates = totalStates; }

        private long _currentStates;
        public long GetCurrentStates() { return _currentStates; }
        public void SetCurrentStates(long currentStates) { _currentStates = currentStates; }

        private int _deadEnds;
        public int GetDeadEnds() { return _deadEnds; }
        public void SetDeadEnds(int deadEnds) { _deadEnds = deadEnds; }
        public Stopwatch stopwatch;

        public StatsAssistant()
        {
            _iterations = 0;
            _totalStates = 0;
            _currentStates = 0;
            _deadEnds = 0;
            stopwatch = new Stopwatch();
        }

        public void ViewStats()
        {
            Console.WriteLine($"Iterations: {_iterations}, Totalstates: {_totalStates}," +
               $" Current states: {_currentStates}, Deadends: {_deadEnds}");
        }
    }
}
