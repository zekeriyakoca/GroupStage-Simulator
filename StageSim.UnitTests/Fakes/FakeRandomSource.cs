using StageSim.Domain.Interfaces;

namespace StageSim.Tests.Fakes;

public class FakeRandomSource(IEnumerable<double> values) : IRandomSource
{
    private readonly Queue<double> _values = new(values);

    public double NextDouble() => _values.Count > 0 ? _values.Dequeue() : 0.5;
}
