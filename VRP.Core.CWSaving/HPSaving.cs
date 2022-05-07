using VRP.Core.Common.Abstractions;
using VRP.Core.Common.Entites;
using VRP.Core.Common.Helpers;
using VRP.Core.CWSaving.Entities;

namespace VRP.Core.CWSaving
{
    public class HPSaving : SavingBase, IVRPAlgorithm
    {
        public List<List<int>> Process(InputData input, object? parameters = null)
        {
            //you should remember that first dealer is always depot
            List<Dealer> dealers = input.Demands
                .Select((demand, id) => new Dealer(id + 1, demand))
                .ToList();

            List<SavingMatrixItem> savingItems = GetSavingMatrix(dealers.Skip(1).ToList(), input.DistanceMatrix)
                .OrderByDescending(si => si.SavingCost)
                .ToList();

            List<List<int>> bestSolution = new List<List<int>>();
            double bestPath=int.MaxValue;

            while(savingItems.Count > 0)
            {
                List<List<int>> newSolution = ProcessSaving(savingItems, dealers, input.MaxCapacity);
                double newPath = ResultHelper.GetTotalPath(input.DistanceMatrix, newSolution);

                if(newPath < bestPath)
                {
                    bestSolution = newSolution;
                    bestPath = newPath;
                };
                savingItems.RemoveAt(0);
            }

            return bestSolution;
        }
    }
}
