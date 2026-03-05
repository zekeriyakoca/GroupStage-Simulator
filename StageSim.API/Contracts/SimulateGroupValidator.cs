using FluentValidation;

namespace StageSim.API.Contracts;

public class SimulateGroupRequestValidator : AbstractValidator<SimulateGroupRequest>
{
    // I've set the restrictions strict to task descripiton
    public SimulateGroupRequestValidator()
    {
        RuleFor(r => r.Teams)
            .NotEmpty()
            .Must(t => t.Count == 4).WithMessage("Exactly 4 teams are required.");

        RuleForEach(r => r.Teams).ChildRules(team =>
        {
            team.RuleFor(t => t.Name).NotEmpty().WithMessage("Team name is required.");
            team.RuleFor(t => t.Strength).InclusiveBetween(1, 100);
        });

        RuleFor(r => r.QualifierCount)
            .GreaterThan(0)
            .LessThan(4).WithMessage("QualifierCount must be between 1 and 3.");
    }
}