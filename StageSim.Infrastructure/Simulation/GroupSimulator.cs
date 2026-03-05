using StageSim.Domain;
using StageSim.Domain.Exceptions;
using StageSim.Domain.Interfaces;

namespace StageSim.Infrastructure;

public class GroupSimulator(
    IScheduleGenerator scheduleGenerator,
    IMatchSimulator matchSimulator,
    IStandingsCalculator standingsCalculator)
    : IGroupSimulator
{
    public SimulationResult Simulate(IReadOnlyList<Team> teams, GroupConfiguration config)
    {
        // TODO: Control flow with exception. Should be replaced with proper result wrapper. Take Ardalis, for example
        if (teams.Count != config.TeamCount)
            throw new BadRequestException($"Expected {config.TeamCount} teams, got {teams.Count}.");

        var schedule = scheduleGenerator.Generate(teams);

        var roundsPlayed = schedule
            .Select(sr => new Round(
                sr.RoundNumber,
                sr.FixtureLines.Select(f => matchSimulator.Simulate(f, config)).ToList()))
            .ToList();

        var allMatches = roundsPlayed.SelectMany(r => r.Matches).ToList();
        var standings = standingsCalculator.Calculate(teams, allMatches);
        var qualifiedTeamNames = standings.Take(config.QualifierCount).Select(s => s.Team).ToList();

        return new SimulationResult(roundsPlayed, standings, qualifiedTeamNames);
    }
}
