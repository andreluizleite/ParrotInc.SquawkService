using MediatR;
using Microsoft.Extensions.Logging;
using ParrotInc.SquawkService.Application.Commands;
using ParrotInc.SquawkService.Application.Dtos;
using ParrotInc.SquawkService.Application.Logging;
using ParrotInc.SquawkService.Application.Responses;
using Serilog;

namespace ParrotInc.SquawkService.Application.CommandHandlers
{
    public class CreateSquawkCommandHandler : IRequestHandler<CreateSquawkCommand, CreateSquawkResponse>
    {
        private readonly ISquawkDomainService _squawkDomainService;
        private readonly ILogger<CreateSquawkCommandHandler>_logger;

        public CreateSquawkCommandHandler(ISquawkDomainService squawkDomainService, ILogger<CreateSquawkCommandHandler> logger)
        {
            _squawkDomainService = squawkDomainService;
            _logger = logger;
        }

        public async Task<CreateSquawkResponse> Handle(CreateSquawkCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Call the domain service to create a squawk
                var squawk = await _squawkDomainService.CreateSquawkAsync(command.UserId, command.Content);

                var squawkDto = new SquawkDto
                {
                    UserId = squawk.Metadata.UserId,
                    Content = squawk.Content,
                    CreatedAt = DateTime.UtcNow
                };

                LogManager.Instance.LogInformation(_logger, "Creating Squawk for user {UserId}", command.UserId);

                return new CreateSquawkResponse(squawkDto);
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError(_logger, ex, "An error occurred while creating a Squawk for user {UserId}", command.UserId);
                throw;
            }
        }
    }
}
