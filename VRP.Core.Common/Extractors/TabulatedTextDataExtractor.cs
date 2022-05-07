using VRP.Core.Common.Abstractions;
using VRP.Core.Common.Entites;
using VRP.Core.Common.Helpers;

namespace VRP.Core.Common.Extractors
{
    public class TabulatedTextDataExtractor : IDataExtractor
    {
        public InputData Extract(Stream stream)
        {
            List<Coord> coords = new List<Coord>();
            List<int> demands = new List<int>();
            int maxCapacity = 0;
            int id = 0;

            using StreamReader sr = new StreamReader(stream);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine()!;

                if (line.Contains("max_capacity"))
                {
                    maxCapacity = Convert.ToInt32(line[(line.IndexOf(':')+1)..]);
                }
                else
                {
                    List<string> data = line.Split(' ')
                        .ToList()
                        .Where(s => s != "")
                        .ToList();

                    var dealerId = ++id;
                    var x = Convert.ToDouble(data[1]);
                    var y = Convert.ToDouble(data[2]);
                    var demand = (int)Convert.ToDouble(data[3]);
                    coords.Add(new Coord(id, x, y));
                    demands.Add(demand);
                }
            }

            return new InputData
            {
                MaxCapacity = maxCapacity,
                Demands = demands,
                DistanceMatrix = EuclidHelper.GetEuclidDistanceMatrix(coords),
            };
        }
    }
}
