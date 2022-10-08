using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    internal class MergeSort : SortAssistant
    {
        protected void Merge(string filepath, string tempB, string tempC, long partSize, long numberOfElements)
        {
            BinaryWriter binaryWriter = new BinaryWriter(new FileStream(filepath, FileMode.OpenOrCreate));
            BinaryReader binaryReaderB = new BinaryReader(new FileStream(tempB, FileMode.OpenOrCreate));
            BinaryReader binaryReaderC = new BinaryReader(new FileStream(tempC, FileMode.OpenOrCreate));
            int b, c;
            int counterB, counterC;

            while (!EndOfStream(binaryReaderC))
            {
                counterB = 0;
                counterC = 0;
                b = binaryReaderB.ReadInt32();
                c = binaryReaderC.ReadInt32();
                while (counterB < partSize && counterC < partSize)
                {
                    if (b <= c)
                    {
                        binaryWriter.Write(b);
                        counterB++;
                        if (counterB < partSize)
                            b = binaryReaderB.ReadInt32();
                        else
                        {
                            while (counterC < partSize)
                            {
                                binaryWriter.Write(c);
                                counterC++;
                                if (counterC < partSize && !EndOfStream(binaryReaderC))
                                    c = binaryReaderC.ReadInt32();
                            }
                        }
                    }
                    else
                    {
                        binaryWriter.Write(c);
                        counterC++;
                        if (counterC < partSize && !EndOfStream(binaryReaderC))
                            c = binaryReaderC.ReadInt32();
                        else
                        {
                            while (counterB < partSize)
                            {
                                binaryWriter.Write(b);
                                counterB++;
                                if (counterB < partSize)
                                    b = binaryReaderB.ReadInt32();
                            }
                        }
                    }
                }
            }
            while (!EndOfStream(binaryReaderB))
                binaryWriter.Write(binaryReaderB.ReadInt32());

            binaryWriter.Close();
            binaryReaderB.Close();
            binaryReaderC.Close();
            File.Delete(tempB);
            File.Delete(tempC);
        }

        protected void Split(string filepath, string tempB, string tempC, long partSize, long numberOfElements)
        {
            BinaryReader binaryReader = new BinaryReader(new FileStream(filepath, FileMode.OpenOrCreate));
            BinaryWriter binaryWriterB = new BinaryWriter(new FileStream(tempB, FileMode.OpenOrCreate));
            BinaryWriter binaryWriterC = new BinaryWriter(new FileStream(tempC, FileMode.OpenOrCreate));
            int counter = 0;

            while (!EndOfStream(binaryReader))
            {
                if (counter % 2 == 0)
                {
                    for (int i = 0; i < partSize; i++)
                    {
                        if (!EndOfStream(binaryReader))
                            binaryWriterB.Write(binaryReader.ReadInt32());
                    }
                }
                else
                {
                    for (int i = 0; i < partSize; i++)
                    {
                        if (!EndOfStream(binaryReader))
                            binaryWriterC.Write(binaryReader.ReadInt32());
                    }
                }
                counter++;
            }
            binaryReader.Close();
            binaryWriterB.Close();
            binaryWriterC.Close();
        }
        protected bool EndOfStream(BinaryReader br)
        {
            if (br.BaseStream.Position == br.BaseStream.Length)
                return true;
            return false;
        }
        public void Sort(string filepath, string tempB, string tempC, long numberOfElements)
        {
            for (long i = 1; i < numberOfElements; i *= 2)
            {
                // i == partSize 
                Split(filepath, tempB, tempC, i, numberOfElements);
                Merge(filepath, tempB, tempC, i, numberOfElements);
            }
        }
    }
}