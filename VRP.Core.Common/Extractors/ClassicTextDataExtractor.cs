using VRP.Core.Common.Abstractions;
using VRP.Core.Common.Entites;
using VRP.Core.Common.Helpers;

namespace VRP.Core.Common.Extractors
{
    public class ClassicTextDataExtractor : IDataExtractor
    {
        public InputData Extract(Stream stream)
        {
            ReadingStateEnum state = ReadingStateEnum.DATA;
            int dealersCount = 0;
            int capacity = 0;
            List<int> demands = new List<int>();
            List<Coord> coords = new List<Coord>();

            using StreamReader sr = new StreamReader(stream);
            while (!sr.EndOfStream)
            {
                var data = sr.ReadLine()!;
                if (string.IsNullOrWhiteSpace(data))
                    continue;

                if (data.Contains("DIMENSION") && state == ReadingStateEnum.DATA)
                {
                    data = data.Trim();
                    dealersCount= int.Parse(data[(data.IndexOf(':') + 1)..]);
                }

                if (data.Contains("CAPACITY") && state == ReadingStateEnum.DATA)
                {
                    data = data.Trim();
                    capacity = int.Parse(data[(data.IndexOf(':') + 1)..]);
                }

                else if (data.Contains("NODE_COORD_SECTION") && state == ReadingStateEnum.DATA)
                {
                    state = ReadingStateEnum.COORDS;
                }

                else if (data.Contains("DEMAND_SECTION") && state == ReadingStateEnum.COORDS)
                {
                    state = ReadingStateEnum.DEMANDS;
                }

                else if (data.Contains("DEPOT_SECTION") && state == ReadingStateEnum.DEMANDS)
                {
                    break;
                }


                else if (state == ReadingStateEnum.COORDS)
                {
                    string[] coords_data = data.Split(" ")
                        .Where(x => x!= "")
                        .ToArray();

                    int id = int.Parse(coords_data[0]);
                    double x = double.Parse(coords_data[1]);
                    double y = double.Parse(coords_data[2]);

                    coords.Add(new Coord(id, x, y));
                }

                else if (state == ReadingStateEnum.DEMANDS)
                {
                    int id = int.Parse(data[..data.IndexOf(" ")]);
                    data = data[(data.IndexOf(" ") + 1)..];

                    int demand = int.Parse(data.Trim());

                    demands.Add(demand);
                }
            }

            if (coords.Count != demands.Count)
                throw new ArgumentException("Coords and dealers quantities must be the same");

            if (coords.Count != dealersCount)
                throw new ArgumentException("Not all coords is loaded. Check file and try again");

            if (demands.Count != dealersCount)
                throw new ArgumentException("Not all demands is loaded. Check file and try again");

            if (capacity < 1)
                throw new ArgumentException("Capacity should not be empty or negative");

            return new InputData
            {
                MaxCapacity = capacity,
                Demands = demands,
                DistanceMatrix = EuclidHelper.GetEuclidDistanceMatrix(coords)
            };
        }
    }
}