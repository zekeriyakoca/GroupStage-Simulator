using StageSim.Domain.Interfaces;

namespace StageSim.Domain.Services;

public class StandingsCalculator : IStandingsCalculator
{
    public IReadOnlyList<Standing> Calculate(IReadOnlyList<Team> teams, IReadOnlyList<MatchResult> matches)
    {
        var standings = teams.Select(t => ComputeStanding(t, matches)).ToList();
        return Sort(standings, matches);
    }

    private static Standing ComputeStanding(Team team, IReadOnlyList<MatchResult> matches)
    {
        var homeMatches = matches.Where(m => m.Home.Id == team.Id).ToList();
        var awayMatches = matches.Where(m => m.Away.Id == team.Id).ToList();

        var won   = homeMatches.Count(m => m.IsHomeWin) + awayMatches.Count(m => m.IsAwayWin);
        var drawn = homeMatches.Count(m => m.IsDraw)    + awayMatches.Count(m => m.IsDraw);
        var lost  = homeMatches.Count(m => m.IsAwayWin) + awayMatches.Count(m => m.IsHomeWin);
        var gf    = homeMatches.Sum(m => m.HomeGoals)   + awayMatches.Sum(m => m.AwayGoals);
        var ga    = homeMatches.Sum(m => m.AwayGoals)   + awayMatches.Sum(m => m.HomeGoals);

        return new Standing(team, homeMatches.Count + awayMatches.Count, won, drawn, lost, gf, ga);
    }

    private static List<Standing> Sort(List<Standing> standings, IReadOnlyList<MatchResult> matches)
    {
        return standings
            .OrderByDescending(s => s.Points)
            .ThenByDescending(s => s.GoalDifference)
            .ThenByDescending(s => s.GoalsFor)
            .ThenBy(s => s.GoalsAgainst)
            .ToList();
    }
}