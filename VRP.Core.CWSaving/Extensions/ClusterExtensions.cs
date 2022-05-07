using VRP.Core.CWSaving.Entities;

namespace VRP.Core.CWSaving.Extensions
{
    public static class ClusterExtensions
    {
        public static bool IsDealerFirst(this Cluster cluster, int id)
        {
            _ = cluster.Dealers.FirstOrDefault(x => x.Id == id)
                ?? throw new ArgumentOutOfRangeException(nameof(id), "Dealer is not exists in this cluster");
            return cluster.Dealers.First().Id == id;
        }

        public static bool IsDealerLast(this Cluster cluster, int id)
        {
            _ = cluster.Dealers.FirstOrDefault(x => x.Id == id)
                ?? throw new ArgumentOutOfRangeException(nameof(id), "Dealer is not exists in this cluster");
            return cluster.Dealers.Last().Id == id;
        }

        public static bool IsDealerTransient(this Cluster cluster, int id)
        {
            _ = cluster.Dealers.FirstOrDefault(x => x.Id == id)
               ?? throw new ArgumentOutOfRangeException(nameof(id), "Dealer is not exists in this cluster");
            return cluster.Dealers.First().Id != id && cluster.Dealers.Last().Id != id;
        }
    }
}
