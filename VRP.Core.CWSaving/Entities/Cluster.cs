namespace VRP.Core.CWSaving.Entities
{
    public class Cluster
    {
        public List<Dealer> Dealers{ get; private set; }
        public int WorkLoad { get; private set; }

        public Cluster(Dealer dealer)
        {
            Dealers = new List<Dealer> { dealer };
            WorkLoad = dealer.Demands;
        }
        public void AddDealer(Dealer newDealer)
        {
            if (Dealers.Any(dealer => dealer.Id == newDealer.Id))
                throw new ArgumentException("Dealer with such Id in this Cluster already");

            Dealers.Add(newDealer);
            WorkLoad += newDealer.Demands;
        }

        public void AddCluster(Cluster cluster)
        {
            Dealers.AddRange(cluster.Dealers);
            WorkLoad += cluster.WorkLoad;
        }

        public void ReverseDealers() 
        {
            Dealers.Reverse();
        }
    }
}
