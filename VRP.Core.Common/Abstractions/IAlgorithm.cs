using VRP.Core.Common.Entites;

namespace VRP.Core.Common.Abstractions
{
    public interface IVRPAlgorithm
    {
        List<List<int>> Process(InputData input, object? parameters = null);
    }
}
