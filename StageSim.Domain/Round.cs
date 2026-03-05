namespace StageSim.Domain;

public record Round(int Number, IReadOnlyList<MatchResult> Matches);
