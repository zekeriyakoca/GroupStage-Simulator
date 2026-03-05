namespace StageSim.Domain.Interfaces;

public interface IStandingsCalculator
{
    IReadOnlyList<Standing> Calculate(IReadOnlyList<Team> teams, IReadOnlyList<MatchResult> matches);
}
