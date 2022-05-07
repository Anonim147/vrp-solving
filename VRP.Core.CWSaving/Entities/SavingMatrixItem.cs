namespace VRP.Core.CWSaving.Entities
{
    public class SavingMatrixItem
    {
        public Dealer FirstDealer { get; private set; }
        public Dealer SecondDealer { get; private set; }
        public double SavingCost { get; private set; }

        public SavingMatrixItem(Dealer firstDealer, Dealer secondDealer, double savingCost)
        {
            FirstDealer = firstDealer;
            SecondDealer = secondDealer;
            SavingCost = savingCost;
        }
    }
}
