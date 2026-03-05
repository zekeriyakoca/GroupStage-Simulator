namespace StageSim.Domain;

public record GroupConfiguration(int TeamCount, int QualifierCount, double AverageGoalsPerGame)
{
    public static GroupConfiguration Default => new(4, 2, 3);
}
