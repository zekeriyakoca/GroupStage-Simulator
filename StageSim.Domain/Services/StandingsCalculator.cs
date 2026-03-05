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
        standings.Sort((x, y) => CompareStandings(x, y, matches));
        return standings;
    }

    private static int CompareStandings(Standing x, Standing y, IReadOnlyList<MatchResult> matches)
    {
        if (x.Points != y.Points)                 return y.Points.CompareTo(x.Points);
        if (x.GoalDifference != y.GoalDifference) return y.GoalDifference.CompareTo(x.GoalDifference);
        if (x.GoalsFor != y.GoalsFor)             return y.GoalsFor.CompareTo(x.GoalsFor);
        if (x.GoalsAgainst != y.GoalsAgainst)     return x.GoalsAgainst.CompareTo(y.GoalsAgainst);

        var h2h = matches.FirstOrDefault(m =>
            (m.Home.Id == x.Team.Id && m.Away.Id == y.Team.Id) ||
            (m.Home.Id == y.Team.Id && m.Away.Id == x.Team.Id));

        if (h2h is null) return 0;

        // negative = x ranks higher, positive = y ranks higher
        return h2h.Home.Id == x.Team.Id
            ? h2h.AwayGoals.CompareTo(h2h.HomeGoals)  // x is home: home win → x ranks higher
            : h2h.HomeGoals.CompareTo(h2h.AwayGoals); // x is away: away win → x ranks higher
    }
}