using MediatR;
using ParrotInc.SquawkService.Application.Commands;
using ParrotInc.SquawkService.Application.Dtos;
using ParrotInc.SquawkService.Application.Responses;

namespace ParrotInc.SquawkService.Application.CommandHandlers
{
    public class CreateSquawkCommandHandler : IRequestHandler<CreateSquawkCommand, CreateSquawkResponse>
    {
        private readonly ISquawkDomainService _squawkDomainService;

        public CreateSquawkCommandHandler(ISquawkDomainService squawkDomainService)
        {
            _squawkDomainService = squawkDomainService;
        }

        public async Task<CreateSquawkResponse> Handle(CreateSquawkCommand command, CancellationToken cancellationToken)
        {
            // Call the domain service to create a squawk
            var squawk = await _squawkDomainService.CreateSquawkAsync(command.UserId, command.Content);

            var squawkDto = new SquawkDto
            {
                UserId = squawk.Metadata.UserId,
                Content = squawk.Content,
                CreatedAt = DateTime.UtcNow 
            };

            return new CreateSquawkResponse(squawkDto);
        }
    }
}
