namespace VRP.Core.CWSaving.Entities
{
    public class Dealer
    {
        public int Id { get; private set; }
        public int Demands { get; private set; }

        public Dealer(int id, int demands)
        {
            Id = id;
            Demands = demands;
        }
    }
}
