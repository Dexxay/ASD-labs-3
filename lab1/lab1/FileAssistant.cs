using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    internal class FileAssistant
    {
        public void GenerateFile(string filename, int sizeOfFileInMb)
        {
            const int NUMBEROFELEMENTSINONEMB = 1024 * 1024 / sizeof(int);
            Random random = new Random();
            using (BinaryWriter writer = new BinaryWriter(new FileStream(filename, FileMode.Create)))
            {
                for (int i = 0; i < sizeOfFileInMb; i++)
                {
                    for (int j = 0; j < NUMBEROFELEMENTSINONEMB; j++)
                        writer.Write(random.Next(int.MinValue, int.MaxValue));
                }
            }
        }

        protected bool StreamEnd(BinaryReader binReader)
        {
            if (binReader.BaseStream.Position == binReader.BaseStream.Length)
                return true;
            return false;
        }

        // Перевірити весь файл
        public bool CheckFile(string filename)
        {
            using (BinaryReader binaryReader = new BinaryReader(new FileStream(filename, FileMode.Open)))
            {
                int prev = binaryReader.ReadInt32();
                while (!StreamEnd(binaryReader))
                {
                    int temp = binaryReader.ReadInt32();
                    if (temp >= prev)
                        prev = temp;
                    else
                        return false;
                }
                return true;
            }
        }
        
        // Перевірити файл частково
        public bool CheckFile(string filename, int count)
        {
            using (BinaryReader binReader = new BinaryReader(new FileStream(filename, FileMode.Open)))
            {
                int prev = binReader.ReadInt32();
                while (!StreamEnd(binReader) && binReader.BaseStream.Position < count)
                {
                    int temp = binReader.ReadInt32();
                    if (temp >= prev)
                        prev = temp;
                    else
                        return false;
                }
                return true;
            }
        }
    }
}
