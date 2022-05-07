namespace VRP.Core.Common.Helpers
{
    public static class ResultHelper
    {
        public static double GetTotalPath(double[,] distanceMatrix, List<List<int>> routes)
        {
            return routes.Sum(route => GetRoutePath(distanceMatrix, route));
        }

        public static double GetRoutePath(double[,] distanceMatrix, List<int> route)
        {
            double sum = 0;
            for (int i = 0; i < route.Count - 1; i++)
            {
                sum += distanceMatrix[route[i] - 1, route[i + 1] - 1];
            }

            return sum;
        }

        public static int GetRouteWorkload(List<int> demands, List<int> route)
        {
            return route.Sum(dealerId => demands[dealerId-1]);
        }
    }
}
