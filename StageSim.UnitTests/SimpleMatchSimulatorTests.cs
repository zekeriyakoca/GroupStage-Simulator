using StageSim.Domain;
using StageSim.Infrastructure.Simulation;
using StageSim.Tests.Fakes;
using Xunit;

namespace StageSim.Tests;

public class SimpleMatchSimulatorTests
{
    private readonly GroupConfiguration _config = GroupConfiguration.Default;

    private SimpleMatchSimulator Sut(params double[] values) =>
        new(new FakeRandomSource(values));

    private static Team TeamWith(int strength) => new(Guid.NewGuid(), "T", strength);

    [Fact]
    public void Goals_are_never_negative()
    {
        var result = Sut(0.0, 0.0).Simulate(new FixtureLine(TeamWith(50), TeamWith(50)), _config);
        Assert.True(result.HomeGoals >= 0);
        Assert.True(result.AwayGoals >= 0);
    }

    [Fact]
    public void Stronger_home_team_scores_more_with_neutral_random()
    {
        // advantage = (90-10) * 0.01 = 0.8; homeRaw = clamp(0.5+0.8) = 1.3; awayRaw = clamp(0.5-0.8) = 0
        var result = Sut(0.5, 0.5).Simulate(new FixtureLine(TeamWith(90), TeamWith(10)), _config);
        Assert.True(result.HomeGoals > result.AwayGoals);
    }

    [Fact]
    public void Result_carries_correct_team_references()
    {
        var home = TeamWith(50); 
        var away = TeamWith(50);
        var result = Sut(0.5, 0.5).Simulate(new FixtureLine(home, away), _config);
        Assert.Equal(home.Id, result.Home.Id);
        Assert.Equal(away.Id, result.Away.Id);
    }

    [Theory]
    [InlineData(50, 50, true)]   // equal strength → no advantage
    [InlineData(90, 10, false)]  // strong home → home won't score less
    public void Simulate_never_marks_both_home_and_away_as_winner(int homeStr, int awayStr, bool expectDraw)
    {
        var result = Sut(0.5, 0.5).Simulate(new FixtureLine(TeamWith(homeStr), TeamWith(awayStr)), _config);
        Assert.False(result.IsHomeWin && result.IsAwayWin);
        if (expectDraw) Assert.True(result.IsDraw);
    }

    [Fact]
    public void Throws_when_home_strength_is_negative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Sut(0.5, 0.5).Simulate(new FixtureLine(TeamWith(-1), TeamWith(50)), _config));
    }

    [Fact]
    public void Throws_when_away_strength_is_negative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Sut(0.5, 0.5).Simulate(new FixtureLine(TeamWith(50), TeamWith(-1)), _config));
    }

    [Fact]
    public void Throws_when_average_goals_is_negative()
    {
        var config = GroupConfiguration.Default with { AverageGoalsPerGame = -1 };
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Sut(0.5, 0.5).Simulate(new FixtureLine(TeamWith(50), TeamWith(50)), config));
    }
}
