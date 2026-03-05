namespace StageSim.Domain.Interfaces;

public interface IGroupSimulator
{
    SimulationResult Simulate(IReadOnlyList<Team> teams, GroupConfiguration config);
}
