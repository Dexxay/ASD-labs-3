using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    internal class InputAssistant
    {
        public int GetFileLength()
        {
            while (true)
            {
                Console.Write("Enter your file length in Mb: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int result) && result > 0)
                    return result;
                else
                    Console.Write("Wrong input. Try again. ");
            }
        }

        public string GetFilename()
        {
            while (true)
            {
                Console.Write("Enter your filename: ");
                string result = Console.ReadLine();
                if (!string.IsNullOrEmpty(result))
                    return result;
                else
                    Console.Write("Wrong input. Try again. ");
            }
        }

        public SortAssistant ChooseClasicOrOptimized()
        {
            while (true)
            {
                Console.Write("Choose Classic (1) or Optimized (0) merge sort alorithm:  ");
                string input = Console.ReadLine();
                if (input == "1")
                    return new MergeSort();
                else if (input == "0")
                    return new OptimizedSort();
                else
                    Console.Write("Wrong symbol. Try again: ");
            }
        }
    }
}
