using VRP.Core.Common.Entites;
using VRP.Core.Common.Helpers;

namespace VRP
{
    public static class WriteHelper
    {
        public static void WriteOutMatring(double[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(Math.Round(matrix[i, j], 2) + " ");
                }
                Console.Write("\n");
            }
        }

        public static int Factorial(int number)
        {
            int factorial = 0;
            for (int i = 1; i <= number; i++)
            {
                factorial *= i;
            }

            return factorial;
        }

        public static void PrintResult(InputData inputData, List<List<int>> solution)
        {
            double minTrucksCount = (double)inputData.Demands.Sum() / inputData.MaxCapacity;
            Console.WriteLine($"Min trucks count: {Math.Ceiling(minTrucksCount)}");

            foreach (var route in solution)
            {
                Console.Write("Route ");
                route.ForEach(x => Console.Write(x + " "));
                Console.Write("Workload: ");
                Console.Write(ResultHelper.GetRouteWorkload(inputData.Demands, route));
                Console.Write("\n");
            }

            Console.Write("Total distance: ");
            Console.WriteLine(ResultHelper.GetTotalPath(inputData.DistanceMatrix, solution));
            Console.Write("Trucks needed: ");
            Console.WriteLine(solution.Count); 
        }
    }
}
