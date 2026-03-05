namespace StageSim.API.Contracts;

public record TeamInput(string Name, int Strength);

public record SimulateGroupRequest(List<TeamInput> Teams, int QualifierCount = 2);
