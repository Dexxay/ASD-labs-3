using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    internal class MergeSort : SortAssistant
    {

        protected void Split(string filepath, string tempB, string tempC, long partSize)
        {
            BinaryReader binReader = new BinaryReader(new FileStream(filepath, FileMode.OpenOrCreate));
            BinaryWriter binWriterB = new BinaryWriter(new FileStream(tempB, FileMode.OpenOrCreate));
            BinaryWriter binWriterC = new BinaryWriter(new FileStream(tempC, FileMode.OpenOrCreate));
            int cnt = 0;

            while (!StreamEnd(binReader))
            {
                if (cnt % 2 == 0)
                {
                    for (int i = 0; i < partSize; i++)
                    {
                        if (!StreamEnd(binReader))
                            binWriterB.Write(binReader.ReadInt32());
                    }
                }

                else
                {
                    for (int i = 0; i < partSize; i++)
                    {
                        if (!StreamEnd(binReader))
                            binWriterC.Write(binReader.ReadInt32());
                    }
                }
                cnt++;
            }

            binReader.Close();
            binWriterB.Close();
            binWriterC.Close();
        }
        protected void Merge(string filepath, string tempB, string tempC, long partSize)
        {
            BinaryWriter binWriter = new BinaryWriter(new FileStream(filepath, FileMode.OpenOrCreate));
            BinaryReader binReaderB = new BinaryReader(new FileStream(tempB, FileMode.OpenOrCreate));
            BinaryReader binReaderC = new BinaryReader(new FileStream(tempC, FileMode.OpenOrCreate));
            int b, c;
            int counterB, counterC;

            while (!StreamEnd(binReaderC))
            {
                counterB = 0;
                counterC = 0;
                b = binReaderB.ReadInt32();
                c = binReaderC.ReadInt32();
                while (counterB < partSize && counterC < partSize)
                {
                    if (b <= c)
                    {
                        binWriter.Write(b);
                        counterB++;
                        if (counterB < partSize)
                            b = binReaderB.ReadInt32();
                        else
                        {
                            while (counterC < partSize)
                            {
                                binWriter.Write(c);
                                counterC++;
                                if (counterC < partSize && !StreamEnd(binReaderC))
                                    c = binReaderC.ReadInt32();
                            }
                        }
                    }
                    else
                    {
                        binWriter.Write(c);
                        counterC++;
                        if (counterC < partSize && !StreamEnd(binReaderC))
                            c = binReaderC.ReadInt32();
                        else
                        {
                            while (counterB < partSize)
                            {
                                binWriter.Write(b);
                                counterB++;
                                if (counterB < partSize)
                                    b = binReaderB.ReadInt32();
                            }
                        }
                    }
                }
            }
            while (!StreamEnd(binReaderB))
                binWriter.Write(binReaderB.ReadInt32());

            binWriter.Close();
            binReaderB.Close();
            binReaderC.Close();
            File.Delete(tempB);
            File.Delete(tempC);
        }

        protected bool StreamEnd(BinaryReader binReader)
        {
            if (binReader.BaseStream.Position == binReader.BaseStream.Length)
                return true;
            return false;
        }
        public void Sort(string filepath, string tempB, string tempC, long numberOfElements)
        {
            for (long i = 1; i < numberOfElements; i *= 2)
            {
                // i == partSize 
                Split(filepath, tempB, tempC, i);
                Merge(filepath, tempB, tempC, i);
            }
        }
    }
}