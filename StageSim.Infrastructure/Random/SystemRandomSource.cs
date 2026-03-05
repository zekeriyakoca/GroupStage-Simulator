using StageSim.Domain.Interfaces;

namespace StageSim.Infrastructure.Random;

public class SystemRandomSource : IRandomSource
{
    public double NextDouble() => System.Random.Shared.NextDouble();
}
