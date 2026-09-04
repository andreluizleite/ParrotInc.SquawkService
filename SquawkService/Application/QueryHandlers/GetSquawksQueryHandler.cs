using MediatR;
using ParrotInc.SquawkService.Application.Dtos;
using ParrotInc.SquawkService.Application.Queries;
using ParrotInc.SquawkService.Domain.Interfaces;

namespace ParrotInc.SquawkService.Application.QueryHandlers;

public sealed class GetSquawksQueryHandler
    : IRequestHandler<GetSquawksQuery, IReadOnlyCollection<SquawkDto>>
{
    private readonly ISquawkRepository _repository;

    public GetSquawksQueryHandler(ISquawkRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<SquawkDto>> Handle(
        GetSquawksQuery request,
        CancellationToken cancellationToken)
    {
        var squawks = await _repository.GetAllAsync(cancellationToken);
        return squawks.Select(SquawkDto.FromDomain).ToArray();
    }
}
