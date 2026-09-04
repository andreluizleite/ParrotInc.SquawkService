using MediatR;
using ParrotInc.SquawkService.Application.Dtos;
using ParrotInc.SquawkService.Application.Queries;
using ParrotInc.SquawkService.Domain.Interfaces;
using ParrotInc.SquawkService.Domain.ValueObjects;

namespace ParrotInc.SquawkService.Application.QueryHandlers;

public sealed class GetSquawkByIdQueryHandler
    : IRequestHandler<GetSquawkByIdQuery, SquawkDto?>
{
    private readonly ISquawkRepository _repository;

    public GetSquawkByIdQueryHandler(ISquawkRepository repository)
    {
        _repository = repository;
    }

    public async Task<SquawkDto?> Handle(
        GetSquawkByIdQuery request,
        CancellationToken cancellationToken)
    {
        var squawk = await _repository.GetByIdAsync(
            new SquawkId(request.SquawkId),
            cancellationToken);

        return squawk is null ? null : SquawkDto.FromDomain(squawk);
    }
}
