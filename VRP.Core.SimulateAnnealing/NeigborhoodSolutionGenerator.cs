using VRP.Core.Helpers.SimulateAnnealing;

namespace VRP.Core.SimulateAnnealing
{
    public static class NeigborhoodSolutionGenerator
    {
        static FastRandom _rnd = new FastRandom();
        public static List<int> Generate(List<int> existedSolution)
        {
            List<int> newSolution = new List<int>(existedSolution);
            double p = _rnd.NextDouble();

            if (p < 0.337)
            {
                Swap(newSolution);
            }
            else if (p < 0.667)
            {
                Insert(newSolution);
            }
            else
            {
                Reverse(newSolution);
            }

            return newSolution;
        }

        static void Swap(List<int> solution)
        {
            int N = solution.Count();
            int i = _rnd.Next(N - 1);
            int j = _rnd.Next(N - 1);

            int temp = solution[i];
            solution[i] = solution[j];
            solution[j] = temp;
        }

        static void Insert(List<int> solution)
        {
            int N = solution.Count();
            int i = _rnd.Next(N - 1);
            int j = _rnd.Next(N - 1);

            int tempJ = solution[j];
            solution.RemoveAt(j);
            solution.Insert(i, tempJ);
        }

        static void Reverse(List<int> solution)
        {
            int N = solution.Count();
            int i = _rnd.Next(N - 1);
            int j = _rnd.Next(N - 1);
            int index;
            int count;

            if (i > j)
            {
                index = j;
                count = i - j;
            }
            else
            {
                index = i;
                count = j - i;
            }

            solution.Reverse(index, count);
        }
    }
}
