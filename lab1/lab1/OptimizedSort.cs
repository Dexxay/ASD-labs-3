using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    internal class OptimizedSort : SortAssistant
    {
        private int _bufferSize = 1024 * 1024 * 128 / sizeof(int);
        protected void Merge(string filepath, string tempB, string tempC, long partSize, long numberOfElements)
        {
            BinaryWriter binaryWriter = new BinaryWriter(new FileStream(filepath, FileMode.OpenOrCreate));
            BinaryReader binaryReaderB = new BinaryReader(new FileStream(tempB, FileMode.Open));
            BinaryReader binaryReaderC = new BinaryReader(new FileStream(tempC, FileMode.Open));

            long counterB, counterC;
            int[] b, c;

            while (!EndOfStream(binaryReaderC))
            {
                counterB = 0;
                counterC = 0;
                int bIterator = 0, cIterator = 0;
                b = FillArray(binaryReaderB, partSize);
                c = FillArray(binaryReaderC, partSize);
                while (true)
                {
                    if (b[bIterator] <= c[cIterator])
                    {
                        binaryWriter.Write(b[bIterator++]);
                        counterB++;
                        if (b.Length == bIterator)
                        {
                            if (counterB < partSize)
                            {
                                b = FillArray(binaryReaderB, partSize - counterB);
                                bIterator = 0;
                            }
                            else
                            {
                                while (c.Length > cIterator)
                                {
                                    binaryWriter.Write(c[cIterator]);
                                    counterC++;
                                    cIterator++;
                                }
                                while (counterC < partSize && !EndOfStream(binaryReaderC))
                                {
                                    c = FillArray(binaryReaderC, partSize - counterC);
                                    cIterator = 0;
                                    while (c.Length > cIterator)
                                    {
                                        binaryWriter.Write(c[cIterator++]);
                                        counterC++;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        binaryWriter.Write(c[cIterator++]);
                        counterC++;
                        if (c.Length - cIterator == 0)
                        {
                            if (counterC < partSize && !EndOfStream(binaryReaderC))
                            {
                                c = FillArray(binaryReaderC, partSize - counterC);
                                cIterator = 0;
                            }
                            else
                            {
                                while (b.Length > bIterator)
                                {
                                    binaryWriter.Write(b[bIterator++]);
                                    counterB++;
                                }
                                while (counterB < partSize)
                                {
                                    b = FillArray(binaryReaderB, partSize - counterB);
                                    bIterator = 0;
                                    while (b.Length > bIterator)
                                    {
                                        binaryWriter.Write(b[bIterator++]);
                                        counterB++;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
            //binaryWriter.Write(binaryReaderB.ReadBytes((int)(binaryReaderB.BaseStream.Length - binaryReaderB.BaseStream.Length)));
            binaryWriter.Write(binaryReaderB.ReadBytes((int)(binaryReaderC.BaseStream.Length - binaryReaderB.BaseStream.Length)));
            binaryWriter.Close();
            binaryReaderB.Close();
            binaryReaderC.Close();
            File.Delete(tempB);
            File.Delete(tempC);
        }

        protected void Split(string filepath, string tempB, string tempC, long partSize, long numberOfElements)
        {
            BinaryReader binaryReader = new BinaryReader(new FileStream(filepath, FileMode.Open));
            BinaryWriter binaryWriterB = new BinaryWriter(new FileStream(tempB, FileMode.OpenOrCreate));
            BinaryWriter binaryWriterC = new BinaryWriter(new FileStream(tempC, FileMode.OpenOrCreate));
            long counter = 0;

            while ((binaryReader.BaseStream.Length - binaryReader.BaseStream.Position) / sizeof(int) > _bufferSize)
            {
                if ((int)(counter / partSize % 2) == 0)
                    binaryWriterB.Write(binaryReader.ReadBytes(_bufferSize * sizeof(int)));
                else
                    binaryWriterC.Write(binaryReader.ReadBytes(_bufferSize * sizeof(int)));
                counter += _bufferSize;
            }

            if ((int)(counter / partSize % 2) == 0)
                binaryWriterB.Write(binaryReader.ReadBytes((int)(binaryReader.BaseStream.Length - binaryReader.BaseStream.Position)));
            else
                binaryWriterC.Write(binaryReader.ReadBytes((int)(binaryReader.BaseStream.Length - binaryReader.BaseStream.Position)));

            binaryReader.Close();
            binaryWriterB.Close();
            binaryWriterC.Close();
        }

        private void PreSort(string filename, long numberOfElements, string tempFilename = "temp")
        {
            BinaryReader binaryReader = new BinaryReader(new FileStream(filename, FileMode.Open));
            BinaryWriter binaryWriter = new BinaryWriter(new FileStream(tempFilename, FileMode.OpenOrCreate));

            int[] intData;
            byte[] byteData;

            for (long i = 0; i < numberOfElements / _bufferSize; i++)
            {
                intData = new int[_bufferSize];
                byteData = binaryReader.ReadBytes(_bufferSize * sizeof(int));

                for (int j = 0; j < _bufferSize; j++)
                    intData[j] = BitConverter.ToInt32(byteData[(j * sizeof(int))..((j + 1) * sizeof(int))]);

                Array.Sort(intData);
                foreach (int number in intData) binaryWriter.Write(number);
            }

            int numberOfRemains = (int)(numberOfElements % _bufferSize);
            intData = new int[numberOfRemains];
            byteData = binaryReader.ReadBytes(numberOfRemains * sizeof(int));
            for (int c = 0; c < numberOfRemains; c++)
                intData[c] = BitConverter.ToInt32(byteData[(c * sizeof(int))..((c + 1) * sizeof(int))]);
            Array.Sort(intData);

            foreach (int number in intData) binaryWriter.Write(number);
            binaryReader.Close();
            binaryWriter.Close();
            File.Delete(filename);
            FileSystem.RenameFile(tempFilename, filename);
        }

        private int[] FillArray(BinaryReader br, long partSize)
        {
            long count = br.BaseStream.Length - br.BaseStream.Position;
            byte[] binData = br.ReadBytes(FindMin(partSize * sizeof(int), count));

            int[] intData = new int[binData.Length / sizeof(int)];
            for (int i = 0; i < intData.Length; i++)
            {
                intData[i] = BitConverter.ToInt32(binData[(i * sizeof(int))..((i + 1) * sizeof(int))]);
            }
            return intData;
        }

        private int FindMin(long partSizeInBytes, long count)
        {
            int maxArrSize = _bufferSize * sizeof(int);
            if (count <= partSizeInBytes && count <= maxArrSize) 
                return (int)count;

            if (partSizeInBytes < maxArrSize)
                return (int)partSizeInBytes;
            return maxArrSize;
        }

        protected bool EndOfStream(BinaryReader br)
        {
            if (br.BaseStream.Position == br.BaseStream.Length)
                return true;
            return false;
        }

        public void Sort(string filepath, string tempB, string tempC, long numberOfElements)
        {
            PreSort(filepath, numberOfElements);
            for (long i = _bufferSize; i < numberOfElements; i *= 2)
            {
                // i == partSize 
                Split(filepath, tempB, tempC, i, numberOfElements);
                Merge(filepath, tempB, tempC, i, numberOfElements);
            }
        }
    }
}

