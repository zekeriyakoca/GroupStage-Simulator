namespace StageSim.Domain;

public record SimulationResult(
    IReadOnlyList<Round> Rounds,
    IReadOnlyList<Standing> Standings,
    IReadOnlyList<Team> Qualified);
