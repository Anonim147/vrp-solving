namespace VRP.Core.SimulateAnnealing
{
    public class SimulateAnnealingParams
    {
        public double TInitial { get; set; }
        public double TFinal { get; set; }
        public double Apha { get; set; }
        public int MaxIterationsCount { get; set; }

        public static SimulateAnnealingParams CreateDefault()
        {
            return new SimulateAnnealingParams()
            {
                TInitial = 10000000000000,
                TFinal = 0.001,
                Apha = 0.99,
                MaxIterationsCount = 10000
            };
        }
    }
}
