using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Lab5_PA
{
    public class GeneticAlgorithm
    {
        private readonly bool[,] _graphAdjMatrix;

        private List<KeyValuePair<List<bool>, int>> _population;

        private const int MutationProbability = 5; // %
        private const int MaxPopulationSize = 50;
        private const int MaxIterationsCount = 1000;
        private const int numberOfParts = 10;

        private readonly int _crossoverPartition;

        public enum CrossoverType
        {
            crossoverByParts, crossoverByEachGene, crossoverRandomGene
        }
        public enum MutationType
        {
            oneGeneMutation, chromosomeMutation
        }
        public enum LocalEnh
        {
            localEnhType1, localEnhType2
        }

        public GeneticAlgorithm(bool[,] graphAdjMatrix)
        {
            _graphAdjMatrix = graphAdjMatrix;
            Array.Copy(graphAdjMatrix, _graphAdjMatrix, graphAdjMatrix.Length);
            _crossoverPartition = graphAdjMatrix.Length / numberOfParts;
        }

        // Evaluation
        private int Evaluate(List<bool> chromosome)
        {
            return _graphAdjMatrix.GetLength(0) - CountOfSelectedEdges(chromosome) + CountPaintedVertexes(chromosome);
        }

        public KeyValuePair<int, List<int>> Start(CrossoverType crosT, MutationType mutationT, LocalEnh localEnhT)
        {
            _population = CreateInitialPopulation(mutationT);
            for (int i = 0; i < MaxIterationsCount; i++)
            {
                _population = MakeNewGeneration(_population, crosT, mutationT, localEnhT);
            }

            _population.Sort(Comparer<KeyValuePair<List<bool>, int>>.Create((x, y) => x.Value.CompareTo(y.Value)));
            return new KeyValuePair<int, List<int>>(CountPaintedVertexes(_population[0].Key), ConvBoolListToInt(_population[0].Key));
        }

        // Create population
        private List<KeyValuePair<List<bool>, int>> CreateInitialPopulation(MutationType mutationT)
        {
            List<KeyValuePair<List<bool>, int>> population = new List<KeyValuePair<List<bool>, int>>();
            Random random = new Random();

            for (int i = 0; i < MaxPopulationSize; i++)
            {
                List<bool> genes = new List<bool>(_graphAdjMatrix.GetLength(0));
                for (int j = 0; j < _graphAdjMatrix.GetLength(0); j++)
                {
                    if (random.Next(0, 2) == 1)
                        genes.Add(true);
                    else
                        genes.Add(false);
                }

                population.Add(new KeyValuePair<List<bool>, int>(genes, Evaluate(genes)));
            }
            return population;
        }

        // Create new Generation
        private List<KeyValuePair<List<bool>, int>> MakeNewGeneration(List<KeyValuePair<List<bool>, int>> population, CrossoverType crosT, MutationType mutationT, LocalEnh localEnhT)
        {
            List<KeyValuePair<List<bool>, int>> evaluationList = new List<KeyValuePair<List<bool>, int>>(population.OrderBy(x => x.Value).ToList());

            evaluationList.RemoveAt(evaluationList.Count - 1);
            KeyValuePair<List<bool>, int> newChromosome = SetCrossover(population[0].Key, population[1].Key, crosT);

            newChromosome = SetMutation(newChromosome, mutationT);
            newChromosome = SetLocalEnhancement(newChromosome, localEnhT);
            evaluationList.Add(new KeyValuePair<List<bool>, int>(newChromosome.Key, Evaluate(newChromosome.Key)));
            return evaluationList;
        }

        // Mutations
        private KeyValuePair<List<bool>, int> SetMutation(KeyValuePair<List<bool>, int> chromosome, MutationType mutationT)
        {
            Random random = new Random();
            bool mutationHappen = random.Next(101) < MutationProbability;
            var res = chromosome;
            if (mutationHappen)
            {
                if (mutationT == MutationType.oneGeneMutation)
                    res = MutateGen(chromosome.Key);

                else if (mutationT == MutationType.chromosomeMutation)
                    res = MutateChromosome(chromosome.Key);
            }
            return res;
        }
        private KeyValuePair<List<bool>, int> MutateChromosome(List<bool> chromosome)
        {
            Random random = new Random();
            List<bool> mutatedChromosome = new List<bool>();
            foreach (bool gen in chromosome)
            {
                bool isMutated = random.Next(101) < MutationProbability;

                if (isMutated)
                    mutatedChromosome.Add(!gen);
                else
                    mutatedChromosome.Add(gen);
            }

            return new KeyValuePair<List<bool>, int>(mutatedChromosome, Evaluate(mutatedChromosome));
        }

        private KeyValuePair<List<bool>, int> MutateGen(List<bool> chromosome)
        {
            Random random = new Random();
            int genIndex = random.Next(0, chromosome.Count);
            chromosome[genIndex] = !chromosome[genIndex];
            return new KeyValuePair<List<bool>, int>(chromosome, Evaluate(chromosome));
        }

        // Crossovers
        private KeyValuePair<List<bool>, int> SetCrossover(List<bool> chromosome1, List<bool> chromosome2, CrossoverType crosT)
        {
            List<bool> resChromosome = new List<bool>();
            if (crosT == CrossoverType.crossoverByParts)
                resChromosome = CrossoverByParts(chromosome1, chromosome2);

            else if (crosT == CrossoverType.crossoverByEachGene)
                resChromosome = CrossoverEachGen(chromosome1, chromosome2);

            else if (crosT == CrossoverType.crossoverRandomGene)
                resChromosome = CrossoverRandomGen(chromosome1, chromosome2);

            return new KeyValuePair<List<bool>, int>(resChromosome, Evaluate(resChromosome));
        }

        private List<bool> CrossoverByParts(List<bool> chromosome1, List<bool> chromosome2)
        {
            List<bool> newChromosome = new List<bool>(_graphAdjMatrix.GetLength(0));
            for (int i = 0; i < _graphAdjMatrix.GetLength(0); i++)
            {
                if (i % _crossoverPartition != 0)
                    newChromosome.Add(chromosome1[i]);

                else
                    newChromosome.Add(chromosome2[i]);
            }
            return newChromosome;
        }

        private List<bool> CrossoverEachGen(List<bool> chromosome1, List<bool> chromosome2)
        {
            List<bool> newChromosome = new List<bool>(_graphAdjMatrix.GetLength(0));
            for (int i = 0; i < _graphAdjMatrix.GetLength(0); i++)
            {
                if (i % 2 == 0)
                    newChromosome.Add(chromosome1[i]);
                else
                    newChromosome.Add(chromosome2[i]);
            }
            return newChromosome;
        }

        private List<bool> CrossoverRandomGen(List<bool> chromosome1, List<bool> chromosome2)
        {
            Random random = new Random();
            List<bool> newChromosome = new List<bool>(_graphAdjMatrix.GetLength(0));
            for (int i = 0; i < _graphAdjMatrix.GetLength(0); i++)
            {
                if (random.Next(0, 2) == 1)
                    newChromosome.Add(chromosome1[i]);
                else
                    newChromosome.Add(chromosome2[i]);
            }
            return newChromosome;
        }

        // Local Enhancement

        private KeyValuePair<List<bool>, int> SetLocalEnhancement(KeyValuePair<List<bool>, int> chromosome, LocalEnh localEnhT)
        {
            KeyValuePair<List<bool>, int> enhanced = chromosome;
            if (localEnhT == LocalEnh.localEnhType1)
                return LocalEnh1(enhanced);

            else if (localEnhT == LocalEnh.localEnhType2)
                return LocalEnh2(enhanced);
            return enhanced;
        }
        private KeyValuePair<List<bool>, int> LocalEnh1(KeyValuePair<List<bool>, int> chromosome)
        {
            var chromMaxEval = new List<bool>(chromosome.Key);
            int minEval = chromosome.Value;
            int elIndex = 0;

            for (int i = 0; i < chromosome.Key.Count; i++)
            {
                chromMaxEval[i] = !chromosome.Key[i];
                int evaluation = Evaluate(chromMaxEval);
                if (evaluation < minEval && CountOfSelectedEdges(chromosome.Key) == _graphAdjMatrix.Length)
                {
                    minEval = evaluation;
                    elIndex = i;
                }

                chromMaxEval[i] = !chromMaxEval[i];
            }
            chromMaxEval[elIndex] = !chromMaxEval[elIndex];
            return new KeyValuePair<List<bool>, int>(chromMaxEval, minEval);
        }

        private KeyValuePair<List<bool>, int> LocalEnh2(KeyValuePair<List<bool>, int> chromosome)
        {
            int maxConIndex = 0;
            int maxCon = GetVertexConnections(0).Count;

            int curVertexCon;
            for (int i = 1; i < chromosome.Key.Count; i++)
            {
                curVertexCon = GetVertexConnections(i).Count;
                if (!chromosome.Key[i])
                {
                    if (maxCon < curVertexCon)
                    {
                        maxCon = curVertexCon;
                        maxConIndex = i;
                    }
                }
            }

            List<int> con = GetVertexConnections(maxConIndex);

            int bestEval = chromosome.Value;
            foreach (int conVertexIndex in con)
            {
                chromosome.Key[conVertexIndex] = false;
                if (CountOfSelectedEdges(chromosome.Key) != _graphAdjMatrix.Length)
                    chromosome.Key[conVertexIndex] = true;
            }

            chromosome.Key[maxConIndex] = true;
            return chromosome;
        }

        private int CountOfSelectedEdges(List<bool> chromosome)
        {
            HashSet<int> coloredHashSet = new HashSet<int>();

            for (int i = 0; i < chromosome.Count; i++)
            {
                if (chromosome[i])
                {
                    coloredHashSet.Add(i);
                    coloredHashSet.UnionWith(GetVertexConnections(i));
                }
            }
            return coloredHashSet.Count;
        }

        private List<int> GetVertexConnections(int vertexIndex)
        {
            List<int> connections = new List<int>();

            for (int i = 0; i < _graphAdjMatrix.GetLength(1); i++)
            {
                if (_graphAdjMatrix[vertexIndex, i])
                    connections.Add(i);
            }

            return connections;
        }

        private int CountPaintedVertexes(List<bool> chromosome)
        {
            return chromosome.Where(x => x).Count();
        }

        private List<int> ConvBoolListToInt(List<bool> lst)
        {
            var resList = new List<int>();
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i])
                    resList.Add(i);
            }
            return resList;
        }
    }
}