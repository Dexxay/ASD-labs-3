using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5_PA
{
    internal class BalanceAlgorithm
    {
        public BalanceAlgorithm(Graph graph)
        {
            const int amountOfTests = 10;

            bool[,] matrix = graph.GetMatrix();
            GeneticAlgorithm geneticAlg = new GeneticAlgorithm(matrix);
            double bestScore = double.MaxValue;

            GeneticAlgorithm.CrossoverType[] crossOverTypes = { GeneticAlgorithm.CrossoverType.crossoverByParts,
            GeneticAlgorithm.CrossoverType.crossoverByEachGene, GeneticAlgorithm.CrossoverType.crossoverRandomGene };

            GeneticAlgorithm.MutationType[] mutationTypes = { GeneticAlgorithm.MutationType.chromosomeMutation,
            GeneticAlgorithm.MutationType.oneGeneMutation };

            GeneticAlgorithm.LocalEnh[] localEnhancements = { GeneticAlgorithm.LocalEnh.localEnhType1,
            GeneticAlgorithm.LocalEnh .localEnhType2};



            Console.WriteLine("*****-=Balance of params is started, stand by...=-*****");
            Console.WriteLine();
            Console.WriteLine("#####-=Crossover balance=-#####");
            Console.WriteLine();
            GeneticAlgorithm.CrossoverType bestCrossoverType = GeneticAlgorithm.CrossoverType.crossoverByParts;

            foreach (GeneticAlgorithm.CrossoverType crossoverT in crossOverTypes)
            {
                Console.WriteLine("-=" + crossoverT + "=-");

                double score = TestResults(geneticAlg, amountOfTests, crossoverT, GeneticAlgorithm.MutationType.chromosomeMutation, GeneticAlgorithm.LocalEnh.localEnhType2);
                Console.WriteLine("#avg " + crossoverT + " mark: " + score);
                if (score < bestScore)
                {
                    bestScore = score;
                    bestCrossoverType = crossoverT;
                }
                Console.WriteLine();
            }

            Console.WriteLine("####################################");
            Console.WriteLine("#Best crossover type: " + bestCrossoverType);
            Console.WriteLine("####################################");

            Console.WriteLine();
            Console.WriteLine("#####-=Mutation balance=-#####");
            GeneticAlgorithm.MutationType bestMutationType = GeneticAlgorithm.MutationType.chromosomeMutation;

            bestScore = double.MaxValue;
            foreach (GeneticAlgorithm.MutationType mutationT in mutationTypes)
            {
                Console.WriteLine("-=" + mutationT + "=-");

                double mark = TestResults(geneticAlg, amountOfTests, bestCrossoverType, mutationT, GeneticAlgorithm.LocalEnh.localEnhType2);
                Console.WriteLine("#avg " + mutationT + " mark: " + mark);
                if (mark < bestScore)
                {
                    bestScore = mark;
                    bestMutationType = mutationT;
                }
                Console.WriteLine();
            }

            Console.WriteLine("####################################");
            Console.WriteLine("#Best mutation type: " + bestMutationType);
            Console.WriteLine("####################################");

            Console.WriteLine();
            Console.WriteLine("#####-=Local enh balance=-#####");
            Console.WriteLine();
            GeneticAlgorithm.LocalEnh bestLocalEnhType = GeneticAlgorithm.LocalEnh.localEnhType1;

            bestScore = double.MaxValue;
            foreach (GeneticAlgorithm.LocalEnh localEnhT in localEnhancements)
            {
                Console.WriteLine("-=" + localEnhT + "=-");

                double mark = TestResults(geneticAlg, amountOfTests, bestCrossoverType, bestMutationType, localEnhT);
                Console.WriteLine("#avg " + localEnhT + " mark: " + mark);
                if (mark < bestScore)
                {
                    bestScore = mark;
                    bestLocalEnhType = localEnhT;
                }
                Console.WriteLine();
            }

            Console.WriteLine("####################################");
            Console.WriteLine("#Best local enhancement type: " + bestLocalEnhType);
            Console.WriteLine("####################################");


            Console.WriteLine("\n*****-=Balance results=-*****");
            Console.WriteLine("Best crossover: " + bestCrossoverType);
            Console.WriteLine("Best mutation: " + bestMutationType);
            Console.WriteLine("Best local enhancement: " + bestLocalEnhType);
            Console.WriteLine("####################################");
        }

        public static double TestResults(GeneticAlgorithm ga, int amountOfTests, GeneticAlgorithm.CrossoverType crossOverType, GeneticAlgorithm.MutationType mutationType, GeneticAlgorithm.LocalEnh localEnhancement)
        {
            double evaluationsSum = 0;

            int testEval;
            for (int i = 1; i <= amountOfTests; i++)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                testEval = ga.Start(crossOverType, mutationType, localEnhancement).Key;
                evaluationsSum += testEval;
                stopwatch.Stop();
                Console.WriteLine("№" + i + " evaluation: " + testEval + ", time taken: " + stopwatch.ElapsedMilliseconds/1000.0 + " s");
            }
            return evaluationsSum / amountOfTests;
        }
    }
}
