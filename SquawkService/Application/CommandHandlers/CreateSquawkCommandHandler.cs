using MediatR;
using ParrotInc.SquawkService.Application.Commands;
using ParrotInc.SquawkService.Application.Dtos;
using ParrotInc.SquawkService.Domain.Interfaces;

namespace ParrotInc.SquawkService.Application.CommandHandlers;

public sealed class CreateSquawkCommandHandler
    : IRequestHandler<CreateSquawkCommand, SquawkDto>
{
    private readonly ISquawkDomainService _domainService;

    public CreateSquawkCommandHandler(ISquawkDomainService domainService)
    {
        _domainService = domainService;
    }

    public async Task<SquawkDto> Handle(
        CreateSquawkCommand request,
        CancellationToken cancellationToken)
    {
        var squawk = await _domainService.CreateSquawkAsync(
            request.UserId,
            request.Content,
            cancellationToken);

        return SquawkDto.FromDomain(squawk);
    }
}
