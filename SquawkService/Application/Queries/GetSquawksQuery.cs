using MediatR;
using ParrotInc.SquawkService.Application.Dtos;

namespace ParrotInc.SquawkService.Application.Queries;

public sealed record GetSquawksQuery : IRequest<IReadOnlyCollection<SquawkDto>>;
