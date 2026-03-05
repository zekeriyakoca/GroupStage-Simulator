using StageSim.Domain;

namespace StageSim.API.Contracts.Responses;

public record TeamResponse(string Name, int Strength);

public record MatchResultResponse(TeamResponse Home, TeamResponse Away, int HomeGoals, int AwayGoals);

public record RoundResponse(int Number, IReadOnlyList<MatchResultResponse> Matches);

public record StandingResponse(
    int Position,
    TeamResponse Team,
    int Played,
    int Won,
    int Drawn,
    int Lost,
    int GoalsFor,
    int GoalsAgainst,
    int GoalDifference,
    int Points,
    bool Qualified);

public record SimulationResultResponse(
    IReadOnlyList<RoundResponse> Rounds,
    IReadOnlyList<StandingResponse> Standings);

public static class SimulationResultMapper
{
    public static SimulationResultResponse ToResponse(SimulationResult result)
    {
        var qualifiedIds = result.Qualified.Select(t => t.Id).ToHashSet();

        var standings = result.Standings
            .Select((s, i) => new StandingResponse(
                i + 1,
                new TeamResponse(s.Team.Name, s.Team.Strength),
                s.Played, s.Won, s.Drawn, s.Lost,
                s.GoalsFor, s.GoalsAgainst, s.GoalDifference,
                s.Points, qualifiedIds.Contains(s.Team.Id)))
            .ToList();

        var rounds = result.Rounds
            .Select(r => new RoundResponse(
                r.Number,
                r.Matches
                    .Select(m => new MatchResultResponse(
                        new TeamResponse(m.Home.Name, m.Home.Strength),
                        new TeamResponse(m.Away.Name, m.Away.Strength),
                        m.HomeGoals,
                        m.AwayGoals))
                    .ToList()))
            .ToList();

        return new SimulationResultResponse(rounds, standings);
    }
}