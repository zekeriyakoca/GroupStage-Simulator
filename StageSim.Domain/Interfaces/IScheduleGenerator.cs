namespace StageSim.Domain.Interfaces;

public interface IScheduleGenerator
{
    IReadOnlyList<ScheduledRound> Generate(IReadOnlyList<Team> teams);
}
