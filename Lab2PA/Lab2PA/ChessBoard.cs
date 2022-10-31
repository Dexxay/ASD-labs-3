using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2PA
{
    internal class ChessBoard
    {
        private byte[] table;
        private byte size;

        public ChessBoard(string message, int minLimit, int maxLimit)
        {
            size = InputByte(message, minLimit, maxLimit);
            table = InputQueens();
        }
        public ChessBoard(byte parentSize, byte[] parentTable)
        {
            size = parentSize;
            table = new byte[(int)size];
            Array.Copy(parentTable, table, (int)size);
        }
        public byte[] GetArray() { return table; }
        public byte GetSize() { return size; }
        private byte[] InputQueens()
        {
            byte[] table = new byte[size];
            Random random = new Random();
            for (int i = 0; i < size; i++)
                table[i] = Convert.ToByte(random.Next(0, size));
            return table;
        }

        public byte GetQueenCol (int row){
            return table[row];
        }
        public void MoveQueen(int row, byte newCol)
        {
            if (newCol < 0 || newCol > size-1 || row < 0 || row > size-1)
            {
                Console.Write("Wrong position.");
                return;
            }
            table[row] = newCol;
        }

        public int CountConflicts()
        {
            int count = 0;
            count += VerticalConflictsCount();

            for (int col = 0; col < size - 1; col++)
            {
                count += MainDiagonalCount(0, col);
                count += SecondaryDiagonalCount(size - 1, col);
            }

            for (int row = 1; row < size - 1; row++)
            {
                count += MainDiagonalCount(row, 0);
                count += SecondaryDiagonalCount(row, 0);
            }

            return count;
        }

        // Vertical conflicts;  horizontal conflicts == 0
        private int VerticalConflictsCount()
        {
            int count = 0;
            for (int i = 0; i < size - 1; i++)
            {
                int colCount = 0;
                byte temp = table[i];
                for (int j = i + 1; j < size; j++)
                {
                    if (temp == table[j])
                        colCount++;
                }
                if (colCount > 1)
                    colCount--;
                count += colCount;
            }
            return count;
        }

        private int MainDiagonalCount(int row, int col)
        {
            int count = 0;

            for (int i = row; i < size; i++)
            {
                if (table[i] == col)
                    count++;
                col++;
            }
            if (count > 0)
                count--;
            return count;
        }

        private int SecondaryDiagonalCount(int row, int col)
        {
            int count = 0;

            for (int i = row; i >= 0; i--)
            {
                if (table[i] == col)
                    count++;
                col++;
            }
            if (count > 0)
                count--;
            return count;
        }

        private static byte InputByte(string message, int minLimit, int maxLimit)
        {
            Console.Write($"Enter {message} ({minLimit} <= N <= {maxLimit}): ");
            while (true)
            {
                string input = Console.ReadLine();
                if (byte.TryParse(input, out byte number) && number >= minLimit && number <= maxLimit)
                    return number;
                else
                    Console.Write("Wrong input. Try again: ");
            }
        }  
        public void ShowBoard()
        {
            Console.WriteLine("\nYour table is:");
            ShowArray();
            Console.Write("  ");
            for (int c = 0; c < size; c++)
                Console.Write($" {c%10} ");
            Console.WriteLine();

            for (int i = 0; i < size; i++)
            {
                Console.Write(i%10 + " ");
                for (int j = 0; j < size; j++)
                {
                    if (table[i] == j)
                        Console.Write("[Q]");
                    else
                        Console.Write("[_]");
                }
                Console.WriteLine();
            }
        }
        public void ShowArray()
        {
            Console.Write("{ ");
            for (int i = 0; i < table.Length; i++)
            {
                if (i == table.Length - 1)
                    Console.Write(table[i] + " ");
                else
                    Console.Write(table[i] + "; ");
            }
            Console.WriteLine("}");
        }
    }
}
