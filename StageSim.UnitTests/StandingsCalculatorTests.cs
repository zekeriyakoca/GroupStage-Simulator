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

    [Fact]
    public void Goals_for_and_against_are_accumulated_across_all_matches()
    {
        var a = GenerateTeam("A"); var b = GenerateTeam("B"); var c = GenerateTeam("C");
        var matches = new[] { GenerateMatchResult(a, b, 2, 1), GenerateMatchResult(a, c, 3, 0) };
        var standing = _sut.Calculate([a, b, c], matches).First(s => s.Team.Id == a.Id);
        Assert.Equal(5, standing.GoalsFor);
        Assert.Equal(1, standing.GoalsAgainst);
    }

    [Fact]
    public void Goal_difference_breaks_points_tie()
    {
        var a = GenerateTeam("A"); var b = GenerateTeam("B"); var c = GenerateTeam("C");
        // A draws with C (1pt, GD 0), B beats C 2-1 (3pts, GD +1) — B ranks higher on points
        var matches = new[] { GenerateMatchResult(a, c, 1, 1), GenerateMatchResult(b, c, 2, 1) };
        var result = _sut.Calculate([a, b, c], matches);
        Assert.Equal(b.Id, result[0].Team.Id);
    }

    [Fact]
    public void Goals_for_breaks_equal_goal_difference_tie()
    {
        var a = GenerateTeam("A"); var b = GenerateTeam("B"); var c = GenerateTeam("C");
        // A wins 1-0, B wins 2-1 — both +1 GD but B has more goals for
        var matches = new[] { GenerateMatchResult(a, c, 1, 0), GenerateMatchResult(b, c, 2, 1) };
        var result = _sut.Calculate([a, b, c], matches);
        Assert.Equal(b.Id, result[0].Team.Id);
    }

    [Fact]
    public void Played_count_is_correct()
    {
        var a = GenerateTeam("A"); var b = GenerateTeam("B"); var c = GenerateTeam("C");
        var matches = new[] { GenerateMatchResult(a, b, 1, 0), GenerateMatchResult(a, c, 2, 1) };
        var standing = _sut.Calculate([a, b, c], matches).First(s => s.Team.Id == a.Id);
        Assert.Equal(2, standing.Played);
    }

    [Fact]
    public void Head_to_head_result_breaks_tie_when_all_else_equal()
    {
        var a = GenerateTeam("A"); var b = GenerateTeam("B"); var c = GenerateTeam("C");
        // A and B both beat C with identical scores, so points/GD/GF/GA are equal — A beat B directly
        var matches = new[]
        {
            GenerateMatchResult(a, c, 1, 0),
            GenerateMatchResult(b, c, 1, 0),
            GenerateMatchResult(a, b, 1, 0), // A wins H2H
        };
        var result = _sut.Calculate([a, b, c], matches);
        Assert.Equal(a.Id, result[0].Team.Id);
        Assert.Equal(b.Id, result[1].Team.Id);
    }

    [Fact]
    public void Won_drawn_lost_counts_are_correct()
    {
        var a = GenerateTeam("A"); var b = GenerateTeam("B"); var c = GenerateTeam("C");
        var matches = new[] { GenerateMatchResult(a, b, 2, 0), GenerateMatchResult(a, c, 1, 1) };
        var standing = _sut.Calculate([a, b, c], matches).First(s => s.Team.Id == a.Id);
        Assert.Equal(1, standing.Won);
        Assert.Equal(1, standing.Drawn);
        Assert.Equal(0, standing.Lost);
    }
}
