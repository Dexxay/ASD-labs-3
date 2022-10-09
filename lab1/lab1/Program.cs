using System;
using System.Diagnostics;

namespace lab1
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Welcome to my program!");
            FileAssistant fileAssistant = new FileAssistant();
            InputAssistant inputAssistant = new InputAssistant();

            string filename = inputAssistant.GetFilename();
            int fileSize = inputAssistant.GetFileLength();
            long numberOfElements = fileSize * 1024 * 1024 / 4;

            Stopwatch sw = Stopwatch.StartNew();
            fileAssistant.GenerateFile(filename, fileSize);
            sw.Stop();
            Console.WriteLine($"File generated: {sw.ElapsedMilliseconds} ms" + "\n");

            SortAssistant sorter = inputAssistant.ChooseClasicOrOptimized();
            sw.Restart();
            sorter.Sort(filename, "tempB", "tempC", numberOfElements);
            sw.Stop();
            Console.WriteLine($"File sorted: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            if (fileAssistant.CheckFile(filename, 1000000))
                Console.WriteLine($"File sorted successfully! Check time is: {sw.ElapsedMilliseconds} ms");
            else
                Console.WriteLine($"File sorted with an error! Check time is: {sw.ElapsedMilliseconds} ms");
        }
    }
}