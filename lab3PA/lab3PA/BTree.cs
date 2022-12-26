using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3PA
{
    internal class BTree
    {
        public BTNode? root { get; set; }
        private int T;
        public List<NodeData> fullTreeList { get; set; }

        public BTree(int degree)
        {
            root = null;
            T = degree;
            fullTreeList = new List<NodeData>();
        }


        public NodeData? Search(int key, ref int compCount)
        {
            return root?.Search(key, ref compCount);
        }

        public BTNode? PrimitiveSearch(int key)
        {
            return root?.PrimitiveSearch(key);
        }

        public void Insert(int k, string Val)
        {
            if (root == null)
            {
                root = new BTNode(T);
                root.records[0] = new NodeData(k, Val);
                root.RecordsAmount++;
                return;
            }

            if (root.IsFull)
            {
                BTNode sub = new BTNode(T, false);
                sub.Children[0] = root;

                sub.SplitChildNode(0, root);

                int i = 0;
                if (sub.records[0].Key < k)
                {
                    i++;
                }
                sub.Children[i].Insert(k, Val);

                root = sub;
            }
            else
            {
                root.Insert(k, Val);
            }
        }

        public void Delete(int key, ref bool success)
        {
            if (root.Equals(null))
            {
                success = false;
                return;
            }

            root.Delete(key);

            if (root.RecordsAmount == 0)
            {
                if (root.IsLeaf)
                {
                    root = null;
                }
                else
                { 
                    root = root.Children[0];
                }
            }
        }

        public void Edit(int key, string newVal, ref bool success)
        {
            BTNode? find = PrimitiveSearch(key);
            if (find == null)
            {
                success = false;
                return;
            }
            int idx = 0;
            while (key > find.records[idx].Key)
            { 
                idx++; 
            }
            find.records[idx].Value = newVal;
            success = true;
        }

        public void SaveData(BTNode node)
        {
            fullTreeList.Clear();
            if (node is not null)
            {
                for (int i = 0; i < node.records.Length; i++)
                {
                    if (!node.IsLeaf)
                        SaveData(node.Children[i]);
                    if (node.records is not null && !node.records[i].IsNull)
                        fullTreeList.Add(node.records[i]);
                }

                if (!node.IsLeaf)
                    SaveData(node.Children[^1]);
            }
        }
    }
}
