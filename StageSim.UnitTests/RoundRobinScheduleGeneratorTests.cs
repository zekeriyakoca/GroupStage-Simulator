using StageSim.Domain;
using StageSim.Domain.Services;
using Xunit;

namespace StageSim.Tests;

public class RoundRobinScheduleGeneratorTests
{
    private readonly RoundRobinScheduleGenerator _sut = new();

    private static IReadOnlyList<Team> Teams(int count) =>
        Enumerable.Range(1, count)
            .Select(i => new Team(Guid.NewGuid(), $"Team{i}", 50))
            .ToList();

    [Fact]
    public void Four_teams_produce_three_rounds()
    {
        var result = _sut.Generate(Teams(4));
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void Each_round_has_correct_fixture_count()
    {
        var result = _sut.Generate(Teams(4));
        Assert.All(result, r => Assert.Equal(2, r.FixtureLines.Count));
    }

    [Fact]
    public void All_pairs_play_exactly_once()
    {
        var teams = Teams(4);
        var fixtures = _sut.Generate(teams).SelectMany(r => r.FixtureLines).ToList();

        Assert.Equal(6, fixtures.Count);

        foreach (var t1 in teams)
        foreach (var t2 in teams)
        {
            if (t1.Id == t2.Id) continue;
            var played = fixtures.Count(f =>
                (f.Home.Id == t1.Id && f.Away.Id == t2.Id) ||
                (f.Home.Id == t2.Id && f.Away.Id == t1.Id));
            Assert.Equal(1, played);
        }
    }

    [Fact]
    public void Each_team_plays_exactly_three_matches()
    {
        var teams = Teams(4);
        var fixtures = _sut.Generate(teams).SelectMany(r => r.FixtureLines).ToList();

        foreach (var team in teams)
        {
            var appearances = fixtures.Count(f => f.Home.Id == team.Id || f.Away.Id == team.Id);
            Assert.Equal(3, appearances);
        }
    }

    [Fact]
    public void No_team_plays_itself()
    {
        var fixtures = _sut.Generate(Teams(4)).SelectMany(r => r.FixtureLines);
        Assert.All(fixtures, f => Assert.NotEqual(f.Home.Id, f.Away.Id));
    }

    [Fact]
    public void Six_teams_produce_five_rounds_with_three_fixtures_each()
    {
        var result = _sut.Generate(Teams(6));
        Assert.Equal(5, result.Count);
        Assert.All(result, r => Assert.Equal(3, r.FixtureLines.Count));
    }
}
