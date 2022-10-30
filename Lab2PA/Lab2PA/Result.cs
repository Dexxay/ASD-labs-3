using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2PA
{
    internal class Result
    {
        private Node _node;
        public Node GetNode()
        {
            return _node;
        }
        private bool _isFailed;
        public bool GetFailed() { return _isFailed; }
        private bool _cutOff;
        public bool GetCutOff() { return _cutOff; }
        public Result(Node node, bool isFailed, bool cutOff)
        {
            _node = node;
            _cutOff = cutOff;
            _isFailed = isFailed;
        }

    }
}
