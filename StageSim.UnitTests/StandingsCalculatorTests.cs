using StageSim.Domain;
using StageSim.Domain.Services;
using Xunit;

namespace StageSim.Tests;

public class StandingsCalculatorTests
{
    private readonly StandingsCalculator _sut = new();

    private static Team GenerateTeam(string name) => new(Guid.NewGuid(), name, 50);
    private static MatchResult GenerateMatchResult(Team h, Team a, int hg, int ag) => new(h, a, hg, ag);

    [Fact]
    public void Win_awards_three_points()
    {
        var a = GenerateTeam("A"); var b = GenerateTeam("B");
        var result = _sut.Calculate([a, b], [GenerateMatchResult(a, b, 1, 0)]);
        Assert.Equal(3, result.First(s => s.Team.Id == a.Id).Points);
        Assert.Equal(0, result.First(s => s.Team.Id == b.Id).Points);
    }

    [Fact]
    public void Draw_awards_one_point_each()
    {
        var a = GenerateTeam("A"); var b = GenerateTeam("B");
        var result = _sut.Calculate([a, b], [GenerateMatchResult(a, b, 1, 1)]);
        Assert.All(result, s => Assert.Equal(1, s.Points));
    }

    [Fact]
    public void Loss_awards_zero_points()
    {
        var a = GenerateTeam("A"); var b = GenerateTeam("B");
        var result = _sut.Calculate([a, b], [GenerateMatchResult(a, b, 0, 2)]);
        Assert.Equal(0, result.First(s => s.Team.Id == a.Id).Points);
    }

    [Fact]
    public void Higher_points_ranks_first()
    {
        var a = GenerateTeam("A"); var b = GenerateTeam("B");
        var result = _sut.Calculate([a, b], [GenerateMatchResult(a, b, 1, 0)]);
        Assert.Equal(a.Id, result[0].Team.Id);
    }
}
