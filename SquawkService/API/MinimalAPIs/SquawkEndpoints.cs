using MediatR;
using ParrotInc.SquawkService.Application.Commands;
using Carter;
using ParrotInc.SquawkService.Application.Responses;

namespace ParrotInc.SquawkService.Api
{
    public class SquawkEndpoints : CarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            // Grouping endpoints under /api/squawks
            var group = app.MapGroup("/api/squawks").WithTags("Squawks");

            group.MapPost("/", CreateSquawk)
            .WithName("CreateSquawk")
            .Produces<CreateSquawkResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireRateLimiting("fixed");

            
        }
        public static async Task<IResult> CreateSquawk(CreateSquawkCommand command, IMediator mediator)
        {
            // Validate command data
            if (command.UserId == Guid.Empty || string.IsNullOrWhiteSpace(command.Content))
            {
                return Results.BadRequest("UserId and Content must not be empty.");
            }

            // Handle the command
            var response = await mediator.Send(command);

            return Results.Created($"/api/squawks/{response.Squawk.UserId}", response.Squawk);
        }
    }
}
