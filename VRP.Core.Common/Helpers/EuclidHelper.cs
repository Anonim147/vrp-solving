using VRP.Core.Common.Entites;

namespace VRP.Core.Common.Helpers
{
    public static class EuclidHelper
    {
        public static double[,] GetEuclidDistanceMatrix(List<Coord> coords)
        {
            double[,] distanceMatrix = new double[coords.Count, coords.Count];
            for (int i = 0; i < coords.Count; i++)
            {
                for(int j =0; j < coords.Count; j++)
                {
                    distanceMatrix[i, j] = GetEuclidDistance(coords[i].X, coords[i].Y, coords[j].X, coords[j].Y);
                }
            }

            return distanceMatrix;
        }

        public static double GetEuclidDistance(double x1, double y1, double x2, double y2)
            => Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
    }
}
