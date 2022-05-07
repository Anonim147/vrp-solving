using VRP.Core.Common.Entites;

namespace VRP.Core.Common.Abstractions
{
    public interface IDataExtractor
    {
        public InputData Extract(Stream stream);
    }
}
