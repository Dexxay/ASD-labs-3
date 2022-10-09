using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    internal interface SortAssistant
    {
        protected void Split(string filepath, string tempB, string tempC, long partSize, long numberOfElements) { }
        protected void Merge(string filepath, string tempB, string tempC, long partSize, long numberOfElements) { }
        public void Sort(string filepath, string tempB, string tempC, long numberOfElements) { }
    }
}
