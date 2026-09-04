using MediatR;
using ParrotInc.SquawkService.Application.Dtos;

namespace ParrotInc.SquawkService.Application.Commands;

public sealed record CreateSquawkCommand(
    Guid UserId,
    string? Content) : IRequest<SquawkDto>;
