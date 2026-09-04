using MediatR;
using ParrotInc.SquawkService.Application.Dtos;

namespace ParrotInc.SquawkService.Application.Queries;

public sealed record GetSquawkByIdQuery(Guid SquawkId) : IRequest<SquawkDto?>;
