namespace VRP.Core.Common.Entites
{
    public class InputData
    {
        public int MaxCapacity { get; set; }
        public List<int> Demands { get; set; }
        public double[,] DistanceMatrix { get; set; }
        public Dictionary<string, object> Params { get; set; }

    }
}
