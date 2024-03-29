﻿using Microsoft.VisualBasic.FileIO;
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

        private void PreSort(string filename, string tempFilename = "temp")
        {
            BinaryReader binReader = new BinaryReader(new FileStream(filename, FileMode.Open));
            BinaryWriter binWriter = new BinaryWriter(new FileStream(tempFilename, FileMode.OpenOrCreate));

            int[] intData;
            while (!StreamEnd(binReader))
            {
                intData = FillIntDataArr(binReader, _bufferSize);
                Array.Sort(intData);
                for (int number = 0; number < intData.Length; number++)
                {
                    binWriter.Write(number);
                }
            }
            binReader.Close();
            binWriter.Close();
            File.Delete(filename);
            FileSystem.RenameFile(tempFilename, filename);
            Console.WriteLine("Pre-Sort is done");
        }
        protected void Split(string filepath, string tempB, string tempC, long partSize)
        {
            BinaryReader binReader = new BinaryReader(new FileStream(filepath, FileMode.Open));
            BinaryWriter binWriterB = new BinaryWriter(new FileStream(tempB, FileMode.OpenOrCreate));
            BinaryWriter binWriterC = new BinaryWriter(new FileStream(tempC, FileMode.OpenOrCreate));
            long counter = 0;

            while ((binReader.BaseStream.Length - binReader.BaseStream.Position) / sizeof(int) > _bufferSize)
            {
                if ((int)(counter / partSize % 2) == 0)
                    binWriterB.Write(binReader.ReadBytes(_bufferSize * sizeof(int)));
                else
                    binWriterC.Write(binReader.ReadBytes(_bufferSize * sizeof(int)));
                counter += _bufferSize;
            }

            if ((int)(counter / partSize % 2) == 0)
                binWriterB.Write(binReader.ReadBytes((int)(binReader.BaseStream.Length - binReader.BaseStream.Position)));
            else
                binWriterC.Write(binReader.ReadBytes((int)(binReader.BaseStream.Length - binReader.BaseStream.Position)));

            binReader.Close();
            binWriterB.Close();
            binWriterC.Close();
        }

        protected void Merge(string filepath, string tempB, string tempC, long partSize)
        {
            BinaryWriter binWriter = new BinaryWriter(new FileStream(filepath, FileMode.OpenOrCreate));
            BinaryReader binReaderB = new BinaryReader(new FileStream(tempB, FileMode.Open));
            BinaryReader binReaderC = new BinaryReader(new FileStream(tempC, FileMode.Open));


            while (!StreamEnd(binReaderC))
            {
                long counterB = 0, counterC = 0;
                int bIndex = 0, cIndex = 0;
                int[] b = FillIntDataArr(binReaderB, partSize);
                int[] c = FillIntDataArr(binReaderC, partSize);
                while (true)
                {
                    if (b[bIndex] <= c[cIndex])
                    {
                        WriteNumberToA(b, ref bIndex, ref counterB, binWriter);
                        if (b.Length == bIndex)
                        {
                            if (counterB < partSize)
                            {
                                b = FillIntDataArr(binReaderB, partSize - counterB);
                                bIndex = 0;
                            }
                            else
                            {
                                WriteArrayToA(c, ref cIndex, ref counterC, binWriter);
                                while (counterC < partSize && !StreamEnd(binReaderC))
                                {
                                    c = FillIntDataArr(binReaderC, partSize - counterC);
                                    cIndex = 0;
                                    WriteArrayToA(c, ref cIndex, ref counterC, binWriter);
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        WriteNumberToA(c, ref cIndex, ref counterC, binWriter);
                        if (c.Length - cIndex == 0)
                        {
                            if (counterC < partSize && !StreamEnd(binReaderC))
                            {
                                c = FillIntDataArr(binReaderC, partSize - counterC);
                                cIndex = 0;
                            }
                            else
                            {
                                WriteArrayToA(b, ref bIndex, ref counterB, binWriter);
                                while (counterB < partSize)
                                {
                                    b = FillIntDataArr(binReaderB, partSize - counterB);
                                    bIndex = 0;
                                    WriteArrayToA(b, ref bIndex, ref counterB, binWriter);
                                }
                                break;
                            }
                        }
                    }
                }
            }
            binWriter.Write(binReaderB.ReadBytes((int)(binReaderC.BaseStream.Length - binReaderB.BaseStream.Length)));
            binWriter.Close();
            binReaderB.Close();
            binReaderC.Close();
            File.Delete(tempB);
            File.Delete(tempC);
        }
        private int[] FillIntDataArr(BinaryReader binReader, long partSize)
        {
            byte[] binData;
            long count = binReader.BaseStream.Length - binReader.BaseStream.Position;

            if (count <= partSize * sizeof(int) && count <= _bufferSize * sizeof(int))
                binData = binReader.ReadBytes((int)count);
            else if (partSize < _bufferSize)
                binData = binReader.ReadBytes((int)partSize * sizeof(int));
            else
                binData = binReader.ReadBytes((int)_bufferSize * sizeof(int));

            int[] intData = new int[binData.Length / sizeof(int)];
            for (int i = 0; i < intData.Length; i++)
                intData[i] = BitConverter.ToInt32(binData[(i * sizeof(int))..((i + 1) * sizeof(int))]);

            return intData;
        }

        private void WriteArrayToA(int[] numbers, ref int index, ref long counter, BinaryWriter binWriter)
        {
            while (numbers.Length > index)
            {
                binWriter.Write(numbers[index]);
                counter++;
                index++;
            }
        }
        private void WriteNumberToA(int[] numbers, ref int index, ref long counter, BinaryWriter binWriter)
        {
            binWriter.Write(numbers[index]);
            counter++;
            index++;
        }
        protected bool StreamEnd(BinaryReader binReader)
        {
            if (binReader.BaseStream.Position == binReader.BaseStream.Length)
                return true;
            return false;
        }

        public void Sort(string filepath, string tempB, string tempC, long numberOfElements)
        {
            PreSort(filepath);
            for (long i = _bufferSize; i < numberOfElements; i *= 2)
            {
                // i == partSize 
                Split(filepath, tempB, tempC, i);
                Merge(filepath, tempB, tempC, i);
            }
        }
    }
}