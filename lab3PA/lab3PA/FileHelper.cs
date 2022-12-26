using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3PA
{
    internal static class FileHelper
    {
        public static BTree? ReadFromFile(string path, int degree)
        {
            string line = "";
            BTree tree = new BTree(degree);
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    while (!string.IsNullOrEmpty(line = reader.ReadLine()))
                    {
                        string[] data = line.Split('|');
                        if (data.Length == 2)
                        {
                            if (int.TryParse(data[0], out int key))
                            {
                                tree.Insert(key, data[1]);
                            }
                        }
                        else
                        {
                            return tree;
                        }
                    }
                }
                return tree;
            }
            else
                return tree;
        }

        public static void SaveFile(string path, BTree tree, out string message)
        {
            tree.SaveData(tree.root);
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                for (int i = 0; i < tree.fullTreeList.Count; i++)
                {
                    writer.WriteLine(tree.fullTreeList[i].ToString());
                }
            }
            message = "Файл записано успішно!";
        }

        public static bool IsThereRepeatingKeys(BTree tree, int key)
        {
            if (tree is null)
                return false;

            int count = 0;
            NodeData? result = tree.Search(key, ref count);
            if (result is null)
            {
                return false;
            }
            else
                return true;
        }

    }
}
