using MediatR;
using ParrotInc.SquawkService.Application.Commands;
using ParrotInc.SquawkService.Application.Dtos;
using ParrotInc.SquawkService.Application.Queries;

namespace ParrotInc.SquawkService.Api;

public static class SquawkEndpoints
{
    public static IEndpointRouteBuilder MapSquawkEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/squawks")
            .WithTags("Squawks")
            .RequireRateLimiting("api");

        group.MapPost("/", CreateSquawkAsync)
            .WithName("CreateSquawk")
            .Produces<SquawkDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status429TooManyRequests);

        group.MapGet("/", GetSquawksAsync)
            .WithName("GetSquawks")
            .Produces<IReadOnlyCollection<SquawkDto>>();

        group.MapGet("/{squawkId:guid}", GetSquawkByIdAsync)
            .WithName("GetSquawkById")
            .Produces<SquawkDto>()
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }

    private static async Task<IResult> CreateSquawkAsync(
        CreateSquawkRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var squawk = await sender.Send(
            new CreateSquawkCommand(request.UserId, request.Content),
            cancellationToken);

        return Results.Created($"/api/squawks/{squawk.Id}", squawk);
    }

    private static async Task<IResult> GetSquawksAsync(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var squawks = await sender.Send(new GetSquawksQuery(), cancellationToken);
        return Results.Ok(squawks);
    }

    private static async Task<IResult> GetSquawkByIdAsync(
        Guid squawkId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var squawk = await sender.Send(
            new GetSquawkByIdQuery(squawkId),
            cancellationToken);

        return squawk is null ? Results.NotFound() : Results.Ok(squawk);
    }
}

public sealed record CreateSquawkRequest(Guid UserId, string? Content);
