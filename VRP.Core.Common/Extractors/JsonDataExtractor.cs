using Newtonsoft.Json;
using VRP.Core.Common.Abstractions;
using VRP.Core.Common.Entites;

namespace VRP.Core.Common.Extractors
{
    public class JsonDataExtractor : IDataExtractor
    {
        public InputData Extract(Stream stream)
        {
            using StreamReader sr = new StreamReader(stream);
            string dataString = sr.ReadToEnd();
            InputData input = JsonConvert.DeserializeObject<InputData>(dataString)!;

            for (int i = 0; i < input.DistanceMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < input.DistanceMatrix.GetLength(1); j++)
                {
                    if (input.DistanceMatrix[i, j] != input.DistanceMatrix[j, i])
                        throw new Exception("Matrix must be mirrored");
                }
            }

            if (input.DistanceMatrix.GetLength(0) != input.DistanceMatrix.GetLength(1))
                throw new ArgumentException("Matrix must be squared");

            if (input.DistanceMatrix.GetLength(0) != input.Demands.Count)
                throw new ArgumentException("Matrix must have size as a demands");

            if (input.Demands[0] != 0)
                throw new ArgumentException("The demand of depot must be 0!");

            return input;
        }
    }
}
