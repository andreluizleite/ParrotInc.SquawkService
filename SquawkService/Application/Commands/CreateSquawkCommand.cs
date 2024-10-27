using MediatR;
using ParrotInc.SquawkService.Application.Responses;
using System;

namespace ParrotInc.SquawkService.Application.Commands
{
    public class CreateSquawkCommand : IRequest<CreateSquawkResponse>
    {
        public Guid UserId { get; }
        public string Content { get; }

        public CreateSquawkCommand(Guid userId, string content)
        {
            UserId = userId;
            Content = content;
        }
    }
}
