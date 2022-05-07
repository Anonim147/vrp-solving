using VRP.Core.Common.Abstractions;
using VRP.Core.Common.Entites;
using VRP.Core.Helpers.SimulateAnnealing;

namespace VRP.Core.SimulateAnnealing
{
    public class SimulateAnnealing : IVRPAlgorithm
    {
        private static readonly FastRandom _rnd = new FastRandom();

        public static List<int> CurrentSolution { get; set; }
        public static double CurrentSolutionCost { get; set; }
        public static List<int> BestSolution { get; set; }
        public static double BestSolutionCost { get; set; }


        public static int MinVehicleCount { get; set; }
        public static List<int> Demands { get; set; }
        public static double[,] DistanceMatrix { get; set; }


        public List<List<int>> Process(InputData input, object? parameters = null)
        {
            SimulateAnnealingParams p = (SimulateAnnealingParams)parameters
                ?? throw new Exception("You must pass SA parameters!");
            double TInitial = p.TInitial;
            double TFinal = p.TFinal;
            double ALPHA = p.Apha;
            int MAX_ITERATIONS_COUNT = p.MaxIterationsCount;

            double currentTemp = TInitial;

            int maxCapacity = input.MaxCapacity;
            Demands = input.Demands;
            DistanceMatrix = input.DistanceMatrix;
            MinVehicleCount = GetMiVehiclesCount(input.Demands, input.MaxCapacity);

            InitializeSolution(maxCapacity);

            while (currentTemp >= TFinal)
            {
                for (int i = 0; i < MAX_ITERATIONS_COUNT; i++)
                {
                    List<int> NewSolution = NeigborhoodSolutionGenerator.Generate(CurrentSolution);
                    double NewSolutionCost = CalculateCost(NewSolution, maxCapacity, DistanceMatrix);

                    double difference = NewSolutionCost - CurrentSolutionCost;

                    if (difference < 0)
                    {
                        CurrentSolution = new List<int>(NewSolution);
                        CurrentSolutionCost = NewSolutionCost;

                        if (CurrentSolutionCost < BestSolutionCost)
                        {
                            BestSolution = new List<int>(CurrentSolution);
                            BestSolutionCost = CurrentSolutionCost;

                            Console.WriteLine("Temperature = " + currentTemp.ToString("#.##") + " BestSolutionPath = " + BestSolutionCost.ToString("#.##"));
                        }
                    }
                    else
                    {
                        double r = _rnd.NextDouble();
                        double boltzman = Math.Exp(-difference / currentTemp); // boltzman acceptance criteria
                        if (r < boltzman)
                        {
                            CurrentSolution = new List<int>(NewSolution);
                            CurrentSolutionCost = NewSolutionCost;
                        }
                    }
                }

                currentTemp *= ALPHA;

                CurrentSolution = new List<int>(BestSolution);
                CurrentSolutionCost = BestSolutionCost;
            }

            return NormalizeSolution(BestSolution, maxCapacity, DistanceMatrix);
        }

        int GetMiVehiclesCount(List<int> demands, int maxCapacity)
        {
            int TotalDemand = demands.Sum();
            int Nvehicle = (int)Math.Ceiling((double)TotalDemand / maxCapacity);
            return (demands.Count - 1) + Nvehicle;
        }

        void InitializeSolution(int maxCapacity)
        {
            CurrentSolution = RandomInitialSolution();
            CurrentSolutionCost = CalculateCost(CurrentSolution, maxCapacity, DistanceMatrix);

            BestSolution = new List<int>(CurrentSolution);
            BestSolutionCost = CurrentSolutionCost;
        }

        static List<int> RandomInitialSolution()
        {
            List<int> sol = new List<int>();
            for (int i = 0; i < MinVehicleCount; i++)
            {
                int node = (i + 1);
                if (i >= (Demands.Count - 1)) node = 0;

                int pos = _rnd.Next(0, sol.Count);

                sol.Insert(pos, node);
            }
            return sol;
        }

        double CalculateCost(List<int> solution, int maxCapacity, double[,] distanceMatrix)
        {
            double path = 0;
            int source = 0;
            int destination = 0;
            int workLoad = 0;

            for (int i = 0; i < solution.Count; i++)
            {
                destination = solution[i];
                workLoad += Demands[destination];

                if (destination == 0)
                {
                    path += distanceMatrix[source, destination];
                    source = destination;
                    workLoad = 0;
                }
                else if (workLoad > maxCapacity)
                {
                    path += distanceMatrix[source, 0];
                    workLoad = 0;

                    path += distanceMatrix[0, destination];
                    workLoad += Demands[destination];
                    source = destination;
                }
                else
                {
                    path += distanceMatrix[source, destination];
                    source = destination;
                }
            }

            if (source != 0)
            {
                path += distanceMatrix[source, 0];
            }
            return path;
        }

        //dealers must starts from 1, not from 0
        static List<List<int>> NormalizeSolution(List<int> solution, int maxCapacity, double[,] distanceMatrix)
        {
            double path = 0;
            int source = 0;
            int destination = 0;
            double workLoad = 0;

            List<List<int>> solList = new List<List<int>>();
            solList.Add(new List<int>());
            solList.Last().Add(1);

            for (int i = 0; i < solution.Count; i++)
            {
                destination = solution[i];
                if (destination == 0 && source == 0)
                    continue;

                workLoad += Demands[destination];

                if (destination == 0)
                {
                    path += distanceMatrix[source, destination];
                    source = destination;
                    workLoad = 0;

                    solList.Last().Add(destination + 1);
                    solList.Add(new List<int>());
                    solList.Last().Add(1);
                }
                else if (workLoad > maxCapacity)
                {
                    path += distanceMatrix[source, 0];
                    workLoad = 0;

                    solList.Last().Add(1);

                    solList.Add(new List<int>());
                    solList.Last().Add(1);
                    solList.Last().Add(destination + 1);

                    path += distanceMatrix[0, destination];
                    workLoad += Demands[destination];

                    source = destination;
                }
                else
                {
                    path += distanceMatrix[source, destination];
                    source = destination;
                    solList.Last().Add(destination + 1);
                }
            }

            if (destination != 0)
            {
                solList.Last().Add(1);
            }

            return solList;
        }




    }
}