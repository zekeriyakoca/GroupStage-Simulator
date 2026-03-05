using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StageSim.API.Contracts;
using StageSim.API.Contracts.Responses;
using StageSim.API.Middlewares;
using StageSim.Domain;
using StageSim.Domain.Interfaces;
using StageSim.Domain.Services;
using StageSim.Infrastructure.Random;
using StageSim.Infrastructure.Simulation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IRandomSource, SystemRandomSource>();
builder.Services.AddSingleton<IMatchSimulator, SimpleMatchSimulator>();
builder.Services.AddSingleton<IScheduleGenerator, RoundRobinScheduleGenerator>();
builder.Services.AddSingleton<IStandingsCalculator, StandingsCalculator>();
builder.Services.AddSingleton<IGroupSimulator, GroupSimulator>();
builder.Services.AddSingleton<IValidator<SimulateGroupRequest>, SimulateGroupRequestValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()));

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/groups/simulate", (
    [FromBody] SimulateGroupRequest request,
    [FromServices] IGroupSimulator simulator,
    [FromServices] IValidator<SimulateGroupRequest> validator) =>
{
    var validation = validator.Validate(request);
    if (!validation.IsValid)
        return Results.ValidationProblem(validation.ToDictionary());

    var config = GroupConfiguration.Default with { QualifierCount = request.QualifierCount };

    var teams = request.Teams
        .Select(t => new Team(Guid.NewGuid(), t.Name, t.Strength))
        .ToList();

    var result = simulator.Simulate(teams, config);
    if (!result.IsSuccess)
        return Results.BadRequest(result.Error);

    return Results.Ok(SimulationResultMapper.ToResponse(result.Value!));
})
.WithName("SimulateGroup");

app.Run();
