using StageSim.Domain.Interfaces;

namespace StageSim.Domain.Services;

public class RoundRobinScheduleGenerator : IScheduleGenerator
{
    public IReadOnlyList<ScheduledRound> Generate(IReadOnlyList<Team> teams)
    {
        if (teams is null) throw new ArgumentNullException(nameof(teams));
        if (teams.Count < 2) return Array.Empty<ScheduledRound>();

        // TODO: Add BYE support for odd team counts
        if (teams.Count % 2 != 0)
            throw new ArgumentException("Even number of teams required (or add a BYE).", nameof(teams));

        var pivotTeam = teams[0];
        var rotatingTeams = teams.Skip(1).ToList();
        var rounds = new List<ScheduledRound>(teams.Count - 1);

        for (int roundIndex = 0; roundIndex < teams.Count - 1; roundIndex++)
        {
            // Pivot plays the first rotating team
            var fixtures = new List<FixtureLine> { new(pivotTeam, rotatingTeams[0]) };

            // Pair remaining rotating teams from ends towards the middle: (1 vs last), (2 vs last-1), ...
            for (int i = 1; i < teams.Count / 2; i++)
            {
                fixtures.Add(new FixtureLine(rotatingTeams[i], rotatingTeams[teams.Count - 1 - i]));
            }

            rounds.Add(new ScheduledRound(roundIndex + 1, fixtures));

            // Rotate rotating teams only (pivot stays fixed)
            var lastTeam = rotatingTeams[^1];
            rotatingTeams.RemoveAt(rotatingTeams.Count - 1);
            rotatingTeams.Insert(0, lastTeam);
        }

        return rounds;
    }
}
