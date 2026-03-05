namespace StageSim.Domain;

public record ScheduledRound(int RoundNumber, IReadOnlyList<FixtureLine> FixtureLines);
