using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3PA
{
    internal class BTNode
    {
        public NodeData[] records { get; set; } = Array.Empty<NodeData>();
        public int T { get; set; }
        public BTNode[] Children { get; set; }
        public int RecordsAmount { get; set; }
        public bool IsLeaf { get; set; }

        internal BTNode(int t, bool isleaf = true)
        {
            T = t;
            records = new NodeData[2 * T - 1];
            for (int i = 0; i < 2 * T - 1; i++)
            {
                records[i] = new NodeData();
            }

            Children = new BTNode[2 * T];
            RecordsAmount = 0;
            IsLeaf = isleaf;
        }

        public bool IsFull => RecordsAmount == records.Length;

        public BTNode? PrimitiveSearch(int key)
        {
            int index = FindIndex(key);
            if (records[index].Key == key)
            {
                return this;
            }
            if (IsLeaf)
            {
                return null;
            }
            return Children[index].PrimitiveSearch(key);
        }


        public NodeData? Search(int key, ref int compCount)
        {
            NodeData? result = BinarySearch(key, ref compCount);
            if (result is not null)
            {
                return result;
            }

            if (IsLeaf)
            {
                return null;
            }
            int i = 0;
            while (i < records.Length && key > records[i].Key)
            {
                i++;
            }
            return Children[i].Search(key, ref compCount);
        }

        public NodeData? BinarySearch(int key, ref int comparisonsCount)
        {
            int start = 0;
            int end = records.Length - 1;
            int middle;

            while (start <= end)
            {
                middle = (end + start) / 2;
                comparisonsCount++;
                if (records[middle].Key == key)
                {
                    return records[middle];
                }
                comparisonsCount++;
                if (records[middle].Key > key)
                {
                    end = middle - 1;
                }
                else if(records[middle].Key < key)
                {
                    start = middle + 1;
                }
                comparisonsCount++;
            }
            return null;
        }

        public void Insert(int key, string value)
        {
            int i = RecordsAmount - 1;
            if (IsLeaf)
            {
                while (i >= 0 && key < records[i].Key)
                {
                    records[i + 1] = records[i];
                    i--;
                }
                records[i + 1] = new NodeData(key, value);
                RecordsAmount++;
                return;
            }

            while (i >= 0 && key < records[i].Key)
            {
                i--;
            }

            if (Children[i + 1].IsFull)
            {
                SplitChildNode(i + 1, Children[i + 1]);
                if (records[i + 1].Key < key)
                {
                    i++;
                }
            }
            Children[i + 1].Insert(key, value);
        }

        public void SplitChildNode(int index, BTNode child)
        {
            BTNode newChild = new BTNode(T, child.IsLeaf);
            newChild.RecordsAmount = T - 1;

            for (int i = 0; i < T - 1; i++)
            {
                newChild.records[i] = child.records[i + T];
            }

            if (!IsLeaf)
            {
                for (int i = 0; i < T; i++)
                {
                    newChild.Children[i] = child.Children[i + T];
                }
            }
            child.RecordsAmount = T - 1;

            for (int i = RecordsAmount; i >= index + 1; i--)
            {
                Children[i + 1] = Children[i];
            }
            Children[index + 1] = newChild;

            for (int i = RecordsAmount - 1; i >= index; i--)
            {
                records[i + 1] = records[i];
            }

            records[index] = child.records[T - 1];
            RecordsAmount++;
        }

        private int FindIndex(int key)
        {
            int index = 0;
            while (index < RecordsAmount && key > records[index].Key)
            { 
                index++;
            }
            return index;
        }
        internal void Delete(int key)
        {
            int index = FindIndex(key);
            if (index < RecordsAmount && records[index].Key == key)
            {
                if (IsLeaf)
                {
                    DeleteLeaf(index);
                }
                else
                {
                    if (Children[index].RecordsAmount >= T)
                    {
                        DeletePrev(index);
                    }
                    else if (Children[index + 1].RecordsAmount >= T)
                    {
                        DeleteNext(index);
                    }
                    else
                    {
                        Merge(index);
                        Children[index].Delete(key);
                    }
                }
            }
            else
            {
                if (IsLeaf) return;

                bool isInLastSubtree = index == RecordsAmount;

                if (Children[index].RecordsAmount < T)
                {
                    TakeOneKey(index);
                }

                if (isInLastSubtree && index > RecordsAmount)
                {
                    Children[index - 1].Delete(key);
                }
                    
                else
                {
                    Children[index].Delete(key);
                }
            }
        }

        private void DeleteLeaf(int index)
        {
            for (int i = index + 1; i < RecordsAmount - 1; i++)
            {
                records[i - 1] = records[i];
            }
            RecordsAmount--;
        }

        private void DeletePrev(int index)
        {
            BTNode ptr = Children[index];
            while (!ptr.IsLeaf)
            {
                ptr = ptr.Children[ptr.RecordsAmount];
            }    

            NodeData prev = ptr.records[ptr.RecordsAmount - 1];
            ptr.Delete(prev.Key);
            records[index] = prev;
        }

        private void DeleteNext(int index)
        {
            BTNode ptr = Children[index + 1];
            while (!ptr.IsLeaf)
            {
                ptr = ptr.Children[0];
            }    

            NodeData next = ptr.records[0];
            ptr.Delete(next.Key);
            records[index] = next;
        }

        private void Merge(int index)
        {
            BTNode left = Children[index];
            BTNode right = Children[index + 1];
            left.records[RecordsAmount] = records[index];
            left.RecordsAmount++;

            for (int i = 0; i < right.RecordsAmount; i++)
            {
                left.records[i + T] = right.records[i];
            }

            if (!left.IsLeaf)
            {
                for (int i = 0; i <= right.RecordsAmount; i++)
                {
                    left.Children[i + T] = right.Children[i];
                }
            }

            for (int i = index + 1; i < RecordsAmount - 1; i++)
            {
                records[i - 1] = records[i];
                Children[i] = Children[i + 1];
            }
            RecordsAmount--;
            left.RecordsAmount += right.RecordsAmount;
        }

        private void TakeOneKey(int index)
        {
            if (!Children[index - 1].Equals(null) && Children[index - 1].RecordsAmount >= T)
            {
                TakeFromLeft(index);
            }
            else if (!Children[index + 1].Equals(null) && Children[index + 1].RecordsAmount >= T)
            {
                TakeFromRight(index);
            }
            else
            {
                if (index != RecordsAmount)
                {
                    Merge(index);
                }
                else
                {
                    Merge(index - 1);
                }
            }

        }

        private void TakeFromLeft(int index)
        {
            for (int i = Children[index].RecordsAmount - 1; i > 0; i--)
            {
                Children[index].records[i] = Children[index].records[i - 1];
            }

            Children[index].records[0] = Children[index - 1].records[RecordsAmount - 1];

            if (!Children[index].IsLeaf)
            {
                for (int i = Children[index].RecordsAmount - 1; i > 0; i--)
                {
                    Children[index].Children[i + 1] = Children[index].Children[i];
                }

                Children[index].Children[0] = Children[index - 1].Children[RecordsAmount];
            }

            Children[index].RecordsAmount++;
            Children[index - 1].RecordsAmount--;
        }

        private void TakeFromRight(int index)
        {
            Children[index].records[RecordsAmount] = Children[index + 1].records[0];

            for (int i = 0; i < Children[index + 1].RecordsAmount - 1; i++)
            {
                Children[index + 1].records[i] = Children[index + 1].records[i + 1];
            }


            if (!Children[index].IsLeaf)
            {
                Children[index].Children[RecordsAmount + 1] = Children[index + 1].Children[0];
                for (int i = 0; i < Children[index + 1].RecordsAmount; i++)
                {
                    Children[index + 1].Children[i] = Children[index + 1].Children[i + 1];
                }
            }

            Children[index].RecordsAmount++;
            Children[index + 1].RecordsAmount--;
        }
    }
}
