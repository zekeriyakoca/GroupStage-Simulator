namespace StageSim.Domain;

public record MatchResult(Team Home, Team Away, int HomeGoals, int AwayGoals)
{
    public bool IsHomeWin => HomeGoals > AwayGoals;
    public bool IsAwayWin => AwayGoals > HomeGoals;
    public bool IsDraw => HomeGoals == AwayGoals;
}
