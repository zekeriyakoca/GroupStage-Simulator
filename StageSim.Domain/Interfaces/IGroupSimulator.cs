namespace StageSim.Domain.Interfaces;

public interface IGroupSimulator
{
    Result<SimulationResult> Simulate(IReadOnlyList<Team> teams, GroupConfiguration config);
}
