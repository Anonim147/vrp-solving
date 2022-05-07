using VRP.Core.CWSaving.Entities;
using VRP.Core.CWSaving.Extensions;

namespace VRP.Core.CWSaving
{
    public static class CWSaverOld
    {
        public static List<List<int>> ProcessSaving(int maxCapacity, List<SavingMatrixItem> savingItems)
        {
            savingItems = savingItems
                .OrderByDescending(x => x.SavingCost)
                .ToList();

            List<Cluster> clusters = new List<Cluster>();

            foreach (var item in savingItems)
            {
                var firstItemCluster = GetClusterByDealerId(clusters, item.FirstDealer.Id);
                var secondItemCluster = GetClusterByDealerId(clusters, item.SecondDealer.Id);

                if (firstItemCluster == null && secondItemCluster == null)
                {
                    var cluster = new Cluster(item.FirstDealer);
                    clusters.Add(cluster);
                    if (cluster.WorkLoad + item.SecondDealer.Demands <= maxCapacity)
                    {
                        cluster.AddDealer(item.SecondDealer);
                    }
                    else
                    {
                        clusters.Add(new Cluster(item.SecondDealer));
                    }
                }

                else if (firstItemCluster != null && secondItemCluster != null && firstItemCluster != secondItemCluster)
                {
                    if (!firstItemCluster.IsDealerTransient(item.FirstDealer.Id)
                        && !secondItemCluster.IsDealerTransient(item.SecondDealer.Id))
                    {
                        if (firstItemCluster.WorkLoad + secondItemCluster.WorkLoad <= maxCapacity)
                        {
                            if (firstItemCluster.IsDealerFirst(item.FirstDealer.Id))
                            {
                                firstItemCluster.ReverseDealers();
                            }

                            if (secondItemCluster.IsDealerLast(item.SecondDealer.Id))
                            {
                                secondItemCluster.ReverseDealers();
                            }
                            firstItemCluster.AddCluster(secondItemCluster);
                            clusters.Remove(secondItemCluster);
                        }
                    }
                }

                else if (firstItemCluster != null && secondItemCluster == null)
                {
                    if (!firstItemCluster.IsDealerTransient(item.FirstDealer.Id)
                        && firstItemCluster.WorkLoad + item.SecondDealer.Demands <= maxCapacity)
                    {
                        if (firstItemCluster.IsDealerFirst(item.FirstDealer.Id))
                        {
                            firstItemCluster.ReverseDealers();
                        }
                        firstItemCluster.AddDealer(item.SecondDealer);
                    }
                    else
                    {
                        clusters.Add(new Cluster(item.SecondDealer));
                    }
                }

                else if (firstItemCluster == null && secondItemCluster != null)
                {
                    if (!secondItemCluster.IsDealerTransient(item.SecondDealer.Id)
                        && secondItemCluster.WorkLoad + item.SecondDealer.Demands <= maxCapacity)
                    {
                        if (secondItemCluster.IsDealerLast(item.SecondDealer.Id))
                        {
                            secondItemCluster.ReverseDealers();
                        }
                        secondItemCluster.AddDealer(item.FirstDealer);
                    }
                    else
                    {
                        clusters.Add(new Cluster(item.FirstDealer));
                    }
                }
            }

            List<List<int>> solutionsList = clusters.Select(cluster =>
                   cluster.Dealers
                       .Select(dealer => dealer.Id)
                       .ToList()
                ).ToList();

            solutionsList.ForEach(solution =>
            {
                solution.Insert(0, 1);
                solution.Add(1);
            });

            return solutionsList;
        }

        private static Cluster? GetClusterByDealerId(List<Cluster> clusters, int id) =>
            clusters.FirstOrDefault(cluster =>
                cluster.Dealers.Any(dealer => dealer.Id == id));
    }
}
