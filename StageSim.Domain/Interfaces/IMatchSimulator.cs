namespace StageSim.Domain.Interfaces;

public interface IMatchSimulator
{
    MatchResult Simulate(FixtureLine fixture, GroupConfiguration config);
}
