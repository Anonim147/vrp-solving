namespace VRP.Core.Common.Entites
{
    public class Coord
    {
        public int Id { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }

        public Coord(int id, double x, double y)
        {
            (Id, X, Y) = (id, x, y);
        }
    }
}
