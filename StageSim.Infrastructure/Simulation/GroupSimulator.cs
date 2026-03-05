using StageSim.Domain;
using StageSim.Domain.Interfaces;

namespace StageSim.Infrastructure.Simulation;

public class GroupSimulator(
    IScheduleGenerator scheduleGenerator,
    IMatchSimulator matchSimulator,
    IStandingsCalculator standingsCalculator)
    : IGroupSimulator
{
    public Result<SimulationResult> Simulate(IReadOnlyList<Team> teams, GroupConfiguration config)
    {
        if (teams.Count != config.TeamCount)
            return Result<SimulationResult>.Failure($"Expected {config.TeamCount} teams, got {teams.Count}.");

        var schedule = scheduleGenerator.Generate(teams);

        var roundsPlayed = schedule
            .Select(sr => new Round(
                sr.RoundNumber,
                sr.FixtureLines.Select(f => matchSimulator.Simulate(f, config)).ToList()))
            .ToList();

        var allMatches = roundsPlayed.SelectMany(r => r.Matches).ToList();
        var standings = standingsCalculator.Calculate(teams, allMatches);
        var qualifiedTeamNames = standings.Take(config.QualifierCount).Select(s => s.Team).ToList();

        return Result<SimulationResult>.Success(new SimulationResult(roundsPlayed, standings, qualifiedTeamNames));
    }
}
