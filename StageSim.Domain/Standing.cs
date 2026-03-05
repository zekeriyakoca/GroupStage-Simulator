namespace StageSim.Domain;

public record Standing(Team Team, int Played, int Won, int Drawn, int Lost, int GoalsFor, int GoalsAgainst)
{
    public int Points => Won * 3 + Drawn;
    public int GoalDifference => GoalsFor - GoalsAgainst;
}
