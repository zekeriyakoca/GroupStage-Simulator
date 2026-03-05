using StageSim.Domain;
using StageSim.Domain.Interfaces;

namespace StageSim.Infrastructure.Simulation;

public class SimpleMatchSimulator(IRandomSource random) : IMatchSimulator
{
    private const double StrengthDiffScale = 0.01; // controls how much the strength gap between teams affects the final score
    private const double ClampMin = 0.0;            // lowest possible score factor — keeps goals from going negative
    private const double ClampMax = 1.5;            // highest possible score factor — caps how many goals a team can score

    public MatchResult Simulate(FixtureLine fixture, GroupConfiguration config)
    {
        var (homeGoals, awayGoals) = ScoresForTeams(
            fixture.Home.Strength,
            fixture.Away.Strength,
            config.AverageGoalsPerGame);

        return new MatchResult(fixture.Home, fixture.Away, homeGoals, awayGoals);
    }

    private (int homeScore, int awayScore) ScoresForTeams(int teamStrength, int opponentStrength, double averageGoals)
    {
        if (teamStrength < 0)
            throw new ArgumentOutOfRangeException(nameof(teamStrength));
        if (opponentStrength < 0)
            throw new ArgumentOutOfRangeException(nameof(opponentStrength));
        if (averageGoals < 0)
            throw new ArgumentOutOfRangeException(nameof(averageGoals));

        var advantage = (teamStrength - opponentStrength) * StrengthDiffScale;

        var homeRaw = Math.Clamp(random.NextDouble() + advantage, ClampMin, ClampMax);
        var awayRaw = Math.Clamp(random.NextDouble() - advantage, ClampMin, ClampMax);

        return ((int)Math.Round(homeRaw * averageGoals), (int)Math.Round(awayRaw * averageGoals));
    }
}
