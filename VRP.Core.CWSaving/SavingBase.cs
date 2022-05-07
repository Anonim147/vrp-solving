using VRP.Core.CWSaving.Entities;
using VRP.Core.CWSaving.Extensions;

namespace VRP.Core.CWSaving
{
    public class SavingBase
    {
        protected List<List<int>> ProcessSaving(List<SavingMatrixItem> savingItems, List<Dealer> dealers, int MaxCapacity)
        {
            List<Cluster> clusters = dealers
                .Skip(1)
                .Select(dealer => new Cluster(dealer))
                .ToList();

            foreach (var item in savingItems)
            {
                var firstCluster = GetClusterByDealerId(clusters, item.FirstDealer.Id);
                var secondCluster = GetClusterByDealerId(clusters, item.SecondDealer.Id);

                if (firstCluster != secondCluster
                    && !firstCluster.IsDealerTransient(item.FirstDealer.Id)
                    && !secondCluster.IsDealerTransient(item.SecondDealer.Id)
                    && (firstCluster.WorkLoad + secondCluster.WorkLoad) <= MaxCapacity)
                {
                    if (firstCluster.IsDealerFirst(item.FirstDealer.Id))
                    {
                        firstCluster.ReverseDealers();
                    }
                    if (secondCluster.IsDealerLast(item.SecondDealer.Id))
                    {
                        secondCluster.ReverseDealers();
                    }
                    firstCluster.AddCluster(secondCluster);
                    clusters.Remove(secondCluster);
                }
            }

            List<List<int>> solutionList = clusters.Select(cluster =>
                cluster.Dealers
                    .Select(dealer => dealer.Id)
                    .ToList()
                ).ToList();

            solutionList.ForEach(solution =>
            {
                solution.Insert(0, 1);
                solution.Add(1);
            });

            return solutionList;
        }


        protected List<SavingMatrixItem> GetSavingMatrix(List<Dealer> dealers, double[,] distanceMatrix)
        {
            List<SavingMatrixItem> savingItems = new List<SavingMatrixItem>();

            for (int i = 1; i < distanceMatrix.GetLength(0) - 1; i++)
            {
                for (int j = i + 1; j < distanceMatrix.GetLength(1); j++)
                {
                    double distance = distanceMatrix[0, i] + distanceMatrix[0, j] - distanceMatrix[i, j];
                    if (distanceMatrix[i, j] > 0 && distance >= 0)
                        savingItems.Add(new SavingMatrixItem(
                            GetDealerById(dealers, i + 1),
                            GetDealerById(dealers, j + 1),
                            distance)
                        );
                }
            }

            return savingItems;
        }

        protected Dealer GetDealerById(List<Dealer> dealers, int id) => dealers.First(x => x.Id == id);

        protected static Cluster GetClusterByDealerId(List<Cluster> clusters, int id) =>
            clusters.First(cluster =>
                cluster.Dealers.Any(dealer => dealer.Id == id));
    }
}
