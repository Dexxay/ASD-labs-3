using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3PA
{
    internal class NodeData
    {
        public int Key { get; set; }
        public string? Value { get; set; }
        public bool IsNull => Key == Int32.MaxValue;

        public NodeData()
        {
            Key = Int32.MaxValue;
            Value = null;
        }

        public NodeData(int k, string v)
        {
            Key = k;
            Value = v;
        }

        public override string ToString()
        {
            return $"{Key}|{Value}";
        }
    }
}
