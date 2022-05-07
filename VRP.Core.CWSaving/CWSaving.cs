using VRP.Core.Common.Abstractions;
using VRP.Core.Common.Entites;
using VRP.Core.CWSaving.Entities;

namespace VRP.Core.CWSaving
{
    public class CWSaving : SavingBase, IVRPAlgorithm
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

            return ProcessSaving(savingItems, dealers, input.MaxCapacity);
        }
    }
}
